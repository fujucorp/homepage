/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:33:12
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
	/// <summary>
    /// 代收費用檔 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class SchoolRidEntity : Entity
    {
		#region Const
		#region [MDY:202203XX] 2022擴充案 改寫所以 MARK
		///// <summary>
		///// 最大代收科目數量 (40)
		///// </summary>
		//private const int MaxItemCount = 40;
		///// <summary>
		///// 其他(擴展)的代收科目數量 (16)
		///// </summary>
		//private const int OtherItemCount = MaxItemCount - 16;
		#endregion

		#region [MDY:202203XX] 2022擴充案 收入科目最大數量
		/// <summary>
		/// 收入科目最大數量 (40)
		/// </summary>
		public const int ReceiveItemMaxCount = 40;
		#endregion

		#region [MDY:202203XX] 2022擴充案 備註項目最大數量
		/// <summary>
		/// 備註項目最大數量 (21)
		/// </summary>
		public const int MemoTitleMaxCount = 21;
		#endregion

		#region [MDY:202203XX] 2022擴充案 注意事項最大數量
		/// <summary>
		/// 注意事項最大數量 (6)
		/// </summary>
		public const int BriefMaxCount = 6;
		#endregion
		#endregion

		public const string TABLE_NAME = "School_Rid";

		#region Field Name Const Class
		/// <summary>
		/// 代收費用檔 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代收類別代碼 (商家代號) 欄位名稱常數定義
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 代收費用型態 (土銀沒用，固定設為 1) (1=已有繳費者資料之收費; 2=需登錄繳費者資料之收費 (本校生); 3=為需登錄繳費者資料之收費 (不分，本校或外界人士) 欄位名稱常數定義
			/// </summary>
			public const string ReceiveStatus = "Receive_status";

			/// <summary>
			/// 學年代碼 欄位名稱常數定義
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代碼 欄位名稱常數定義
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代碼 (土銀不使用此部別) 欄位名稱常數定義
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 代收費用別代碼 欄位名稱常數定義
			/// </summary>
			public const string ReceiveId = "Receive_Id";
			#endregion

			#region Data
			/// <summary>
			/// 字軌 欄位名稱常數定義
			/// </summary>
			public const string SchWord = "Sch_Word";

			/// <summary>
			/// 學校會計 (土銀沒用) 欄位名稱常數定義
			/// </summary>
			public const string SchAccounting = "Sch_Accounting";

			/// <summary>
			/// 學校出納 (土銀沒用) 欄位名稱常數定義
			/// </summary>
			public const string SchCashier = "Sch_Cashier";

			/// <summary>
			/// 繳費期限 (民國年3碼+月2碼+日2碼)
			/// </summary>
			public const string PayDate = "Pay_Date";

			/// <summary>
			/// 繳費期限2 (中信信用卡) (民國年3碼+月2碼+日2碼) (土銀用來存放中信信用卡繳費期限)
			/// </summary>
			public const string PayDueDate2 = "Pay_Due_Date2";

			/// <summary>
			/// 繳費期限3 (財金信用卡) (民國年3碼+月2碼+日2碼) (土銀用來存放財金信用卡繳費期限)
			/// </summary>
			public const string PayDueDate3 = "Pay_Due_Date3";

			/// <summary>
			/// 扣款期限 (停用) (民國年3碼+月2碼+日2碼) 欄位名稱常數定義
			/// </summary>
			public const string DelmDate = "DelM_Date";

			/// <summary>
			/// 學生學分費計算方式 (1=此代收費用別無學分費之收入科目; 2=以學分數計算; 3=以學分數計算; 4=以小於某學分數才收學分費; 5=以小於某上課時數才以上課時數收學分費; 6=以小於某學分數才以上課時數收學分費) 欄位名稱常數定義
			/// </summary>
			public const string StudentType = "Student_Type";

			/// <summary>
			/// CS_Type (停用) 欄位名稱常數定義
			/// </summary>
			public const string CsType = "CS_Type";

			/// <summary>
			/// 課程或學分基準所屬收入科目 欄位名稱常數定義
			/// </summary>
			public const string CreditItem = "Credit_Item";

			/// <summary>
			/// 整批退費所屬收入科目 欄位名稱常數定義
			/// </summary>
			public const string ReturnItem = "Return_Item";

			/// <summary>
			/// 學分費比較基準 欄位名稱常數定義
			/// </summary>
			public const string CreditBasic = "Credit_Basic";

			/// <summary>
			/// 計算金額順序 (1=減免先; 2=就貸先) 欄位名稱常數定義
			/// </summary>
			public const string BillingType = "Billing_Type";

			#region 40 項收入科目名稱
			/// <summary>
			/// 收入科目01名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem01 = "Receive_Item01";

			/// <summary>
			/// 收入科目02名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem02 = "Receive_Item02";

			/// <summary>
			/// 收入科目03名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem03 = "Receive_Item03";

			/// <summary>
			/// 收入科目04名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem04 = "Receive_Item04";

			/// <summary>
			/// 收入科目05名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem05 = "Receive_Item05";

			/// <summary>
			/// 收入科目06名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem06 = "Receive_Item06";

			/// <summary>
			/// 收入科目07名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem07 = "Receive_Item07";

			/// <summary>
			/// 收入科目08名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem08 = "Receive_Item08";

			/// <summary>
			/// 收入科目09名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem09 = "Receive_Item09";

			/// <summary>
			/// 收入科目10名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem10 = "Receive_Item10";

			/// <summary>
			/// 收入科目11名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem11 = "Receive_Item11";

			/// <summary>
			/// 收入科目12名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem12 = "Receive_Item12";

			/// <summary>
			/// 收入科目13名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem13 = "Receive_Item13";

			/// <summary>
			/// 收入科目14名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem14 = "Receive_Item14";

			/// <summary>
			/// 收入科目15名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem15 = "Receive_Item15";

			/// <summary>
			/// 收入科目16名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem16 = "Receive_Item16";

			/// <summary>
			/// 收入科目17名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem17 = "Receive_Item17";

			/// <summary>
			/// 收入科目18名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem18 = "Receive_Item18";

			/// <summary>
			/// 收入科目19名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem19 = "Receive_Item19";

			/// <summary>
			/// 收入科目20名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem20 = "Receive_Item20";

			/// <summary>
			/// 收入科目21名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem21 = "Receive_Item21";

			/// <summary>
			/// 收入科目22名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem22 = "Receive_Item22";

			/// <summary>
			/// 收入科目23名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem23 = "Receive_Item23";

			/// <summary>
			/// 收入科目24名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem24 = "Receive_Item24";

			/// <summary>
			/// 收入科目25名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem25 = "Receive_Item25";

			/// <summary>
			/// 收入科目26名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem26 = "Receive_Item26";

			/// <summary>
			/// 收入科目27名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem27 = "Receive_Item27";

			/// <summary>
			/// 收入科目28名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem28 = "Receive_Item28";

			/// <summary>
			/// 收入科目29名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem29 = "Receive_Item29";

			/// <summary>
			/// 收入科目30名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem30 = "Receive_Item30";

			/// <summary>
			/// 收入科目31名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem31 = "Receive_Item31";

			/// <summary>
			/// 收入科目32名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem32 = "Receive_Item32";

			/// <summary>
			/// 收入科目33名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem33 = "Receive_Item33";

			/// <summary>
			/// 收入科目34名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem34 = "Receive_Item34";

			/// <summary>
			/// 收入科目35名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem35 = "Receive_Item35";

			/// <summary>
			/// 收入科目36名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem36 = "Receive_Item36";

			/// <summary>
			/// 收入科目37名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem37 = "Receive_Item37";

			/// <summary>
			/// 收入科目38名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem38 = "Receive_Item38";

			/// <summary>
			/// 收入科目39名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem39 = "Receive_Item39";

			/// <summary>
			/// 收入科目40名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItem40 = "Receive_Item40";
			#endregion

			#region 40 項收入科目是否助貸旗標 (Y=是; N=否)
			/// <summary>
			/// 收入科目01是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem01 = "Loan_Item01";

			/// <summary>
			/// 收入科目02是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem02 = "Loan_Item02";

			/// <summary>
			/// 收入科目03是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem03 = "Loan_Item03";

			/// <summary>
			/// 收入科目04是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem04 = "Loan_Item04";

			/// <summary>
			/// 收入科目05是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem05 = "Loan_Item05";

			/// <summary>
			/// 收入科目06是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem06 = "Loan_Item06";

			/// <summary>
			/// 收入科目07是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem07 = "Loan_Item07";

			/// <summary>
			/// 收入科目08是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem08 = "Loan_Item08";

			/// <summary>
			/// 收入科目09是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem09 = "Loan_Item09";

			/// <summary>
			/// 收入科目10是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem10 = "Loan_Item10";

			/// <summary>
			/// 收入科目11是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem11 = "Loan_Item11";

			/// <summary>
			/// 收入科目12是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem12 = "Loan_Item12";

			/// <summary>
			/// 收入科目13是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem13 = "Loan_Item13";

			/// <summary>
			/// 收入科目14是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem14 = "Loan_Item14";

			/// <summary>
			/// 收入科目15是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem15 = "Loan_Item15";

			/// <summary>
			/// 收入科目16是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem16 = "Loan_Item16";

			/// <summary>
			/// 收入科目17是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem17 = "Loan_Item17";

			/// <summary>
			/// 收入科目18是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem18 = "Loan_Item18";

			/// <summary>
			/// 收入科目19是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem19 = "Loan_Item19";

			/// <summary>
			/// 收入科目20是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem20 = "Loan_Item20";

			/// <summary>
			/// 收入科目21是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem21 = "Loan_Item21";

			/// <summary>
			/// 收入科目22是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem22 = "Loan_Item22";

			/// <summary>
			/// 收入科目23是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem23 = "Loan_Item23";

			/// <summary>
			/// 收入科目24是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem24 = "Loan_Item24";

			/// <summary>
			/// 收入科目25是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem25 = "Loan_Item25";

			/// <summary>
			/// 收入科目26是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem26 = "Loan_Item26";

			/// <summary>
			/// 收入科目27是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem27 = "Loan_Item27";

			/// <summary>
			/// 收入科目28是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem28 = "Loan_Item28";

			/// <summary>
			/// 收入科目29是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem29 = "Loan_Item29";

			/// <summary>
			/// 收入科目30是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem30 = "Loan_Item30";

			/// <summary>
			/// 收入科目31是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem31 = "Loan_Item31";

			/// <summary>
			/// 收入科目32是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem32 = "Loan_Item32";

			/// <summary>
			/// 收入科目33是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem33 = "Loan_Item33";

			/// <summary>
			/// 收入科目34是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem34 = "Loan_Item34";

			/// <summary>
			/// 收入科目35是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem35 = "Loan_Item35";

			/// <summary>
			/// 收入科目36是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem36 = "Loan_Item36";

			/// <summary>
			/// 收入科目37是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem37 = "Loan_Item37";

			/// <summary>
			/// 收入科目38是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem38 = "Loan_Item38";

			/// <summary>
			/// 收入科目39是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem39 = "Loan_Item39";

			/// <summary>
			/// 收入科目40是否助貸旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItem40 = "Loan_Item40";
			#endregion

			/// <summary>
			/// 除收入科目外就學貸款可貸之額外固定金額 欄位名稱常數定義
			/// </summary>
			public const string LoanFee = "Loan_Fee";

			#region 40 項收入科目是否代辦旗標 (土銀沒用) (Y=是; N=否)
			/// <summary>
			/// 收入科目01是否代辦旗標 (土銀沒用) (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem01 = "Agency_Item01";

			/// <summary>
			/// Agency_Item02 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem02 = "Agency_Item02";

			/// <summary>
			/// Agency_Item03 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem03 = "Agency_Item03";

			/// <summary>
			/// Agency_Item04 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem04 = "Agency_Item04";

			/// <summary>
			/// Agency_Item05 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem05 = "Agency_Item05";

			/// <summary>
			/// Agency_Item06 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem06 = "Agency_Item06";

			/// <summary>
			/// Agency_Item07 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem07 = "Agency_Item07";

			/// <summary>
			/// Agency_Item08 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem08 = "Agency_Item08";

			/// <summary>
			/// Agency_Item09 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem09 = "Agency_Item09";

			/// <summary>
			/// Agency_Item10 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem10 = "Agency_Item10";

			/// <summary>
			/// Agency_Item11 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem11 = "Agency_Item11";

			/// <summary>
			/// Agency_Item12 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem12 = "Agency_Item12";

			/// <summary>
			/// Agency_Item13 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem13 = "Agency_Item13";

			/// <summary>
			/// Agency_Item14 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem14 = "Agency_Item14";

			/// <summary>
			/// Agency_Item15 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem15 = "Agency_Item15";

			/// <summary>
			/// Agency_Item16 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem16 = "Agency_Item16";

			/// <summary>
			/// Agency_Item17 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem17 = "Agency_Item17";

			/// <summary>
			/// Agency_Item18 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem18 = "Agency_Item18";

			/// <summary>
			/// Agency_Item19 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem19 = "Agency_Item19";

			/// <summary>
			/// Agency_Item20 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem20 = "Agency_Item20";

			/// <summary>
			/// Agency_Item21 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem21 = "Agency_Item21";

			/// <summary>
			/// Agency_Item22 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem22 = "Agency_Item22";

			/// <summary>
			/// Agency_Item23 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem23 = "Agency_Item23";

			/// <summary>
			/// Agency_Item24 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem24 = "Agency_Item24";

			/// <summary>
			/// Agency_Item25 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem25 = "Agency_Item25";

			/// <summary>
			/// Agency_Item26 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem26 = "Agency_Item26";

			/// <summary>
			/// Agency_Item27 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem27 = "Agency_Item27";

			/// <summary>
			/// Agency_Item28 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem28 = "Agency_Item28";

			/// <summary>
			/// Agency_Item29 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem29 = "Agency_Item29";

			/// <summary>
			/// Agency_Item30 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem30 = "Agency_Item30";

			/// <summary>
			/// Agency_Item31 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem31 = "Agency_Item31";

			/// <summary>
			/// Agency_Item32 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem32 = "Agency_Item32";

			/// <summary>
			/// Agency_Item33 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem33 = "Agency_Item33";

			/// <summary>
			/// Agency_Item34 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem34 = "Agency_Item34";

			/// <summary>
			/// Agency_Item35 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem35 = "Agency_Item35";

			/// <summary>
			/// Agency_Item36 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem36 = "Agency_Item36";

			/// <summary>
			/// Agency_Item37 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem37 = "Agency_Item37";

			/// <summary>
			/// Agency_Item38 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem38 = "Agency_Item38";

			/// <summary>
			/// Agency_Item39 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem39 = "Agency_Item39";

			/// <summary>
			/// Agency_Item40 欄位名稱常數定義
			/// </summary>
			public const string AgencyItem40 = "Agency_Item40";
			#endregion

			#region 40 項收入科目是否可更改旗標 (土銀沒用) (Y=是; N=否)
			/// <summary>
			/// 收入科目01是否可更改旗標 (土銀沒用) (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck01 = "Agency_Check01";

			/// <summary>
			/// Agency_Check02 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck02 = "Agency_Check02";

			/// <summary>
			/// Agency_Check03 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck03 = "Agency_Check03";

			/// <summary>
			/// Agency_Check04 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck04 = "Agency_Check04";

			/// <summary>
			/// Agency_Check05 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck05 = "Agency_Check05";

			/// <summary>
			/// Agency_Check06 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck06 = "Agency_Check06";

			/// <summary>
			/// Agency_Check07 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck07 = "Agency_Check07";

			/// <summary>
			/// Agency_Check08 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck08 = "Agency_Check08";

			/// <summary>
			/// Agency_Check09 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck09 = "Agency_Check09";

			/// <summary>
			/// Agency_Check10 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck10 = "Agency_Check10";

			/// <summary>
			/// Agency_Check11 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck11 = "Agency_Check11";

			/// <summary>
			/// Agency_Check12 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck12 = "Agency_Check12";

			/// <summary>
			/// Agency_Check13 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck13 = "Agency_Check13";

			/// <summary>
			/// Agency_Check14 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck14 = "Agency_Check14";

			/// <summary>
			/// Agency_Check15 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck15 = "Agency_Check15";

			/// <summary>
			/// Agency_Check16 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck16 = "Agency_Check16";

			/// <summary>
			/// Agency_Check17 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck17 = "Agency_Check17";

			/// <summary>
			/// Agency_Check18 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck18 = "Agency_Check18";

			/// <summary>
			/// Agency_Check19 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck19 = "Agency_Check19";

			/// <summary>
			/// Agency_Check20 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck20 = "Agency_Check20";

			/// <summary>
			/// Agency_Check21 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck21 = "Agency_Check21";

			/// <summary>
			/// Agency_Check22 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck22 = "Agency_Check22";

			/// <summary>
			/// Agency_Check23 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck23 = "Agency_Check23";

			/// <summary>
			/// Agency_Check24 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck24 = "Agency_Check24";

			/// <summary>
			/// Agency_Check25 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck25 = "Agency_Check25";

			/// <summary>
			/// Agency_Check26 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck26 = "Agency_Check26";

			/// <summary>
			/// Agency_Check27 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck27 = "Agency_Check27";

			/// <summary>
			/// Agency_Check28 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck28 = "Agency_Check28";

			/// <summary>
			/// Agency_Check29 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck29 = "Agency_Check29";

			/// <summary>
			/// Agency_Check30 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck30 = "Agency_Check30";

			/// <summary>
			/// Agency_Check31 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck31 = "Agency_Check31";

			/// <summary>
			/// Agency_Check32 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck32 = "Agency_Check32";

			/// <summary>
			/// Agency_Check33 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck33 = "Agency_Check33";

			/// <summary>
			/// Agency_Check34 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck34 = "Agency_Check34";

			/// <summary>
			/// Agency_Check35 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck35 = "Agency_Check35";

			/// <summary>
			/// Agency_Check36 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck36 = "Agency_Check36";

			/// <summary>
			/// Agency_Check37 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck37 = "Agency_Check37";

			/// <summary>
			/// Agency_Check38 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck38 = "Agency_Check38";

			/// <summary>
			/// Agency_Check39 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck39 = "Agency_Check39";

			/// <summary>
			/// Agency_Check40 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheck40 = "Agency_Check40";
			#endregion

			/// <summary>
			/// 備註 欄位名稱常數定義
			/// </summary>
			public const string ReceiveMemo = "Receive_Memo";

			/// <summary>
			/// FileName (停用) 欄位名稱常數定義
			/// </summary>
			public const string FileName = "File_Name";

			/// <summary>
			/// FilePath (停用) 欄位名稱常數定義
			/// </summary>
			public const string FilePath = "File_Path";

			/// <summary>
			/// 依收費標準計算時，就貸可貸金額計算依據 ((null)0=依減免前繳費金額; (否則)1=依減免後繳費金額) 欄位名稱常數定義
			/// </summary>
			public const string FlagRl = "Flag_RL";

			/// <summary>
			/// 收手續費旗標 (土銀沒用，固定設為 null) (1=臨櫃繳費需加收手續費; 0=免加手續費) 欄位名稱常數定義
			/// </summary>
			public const string FeeFlag = "fee_flag";

			/// <summary>
			/// 繳費單格式 (土銀沒用) (""或1=三聯式預設; 2~6=三聯式可調; A~G=三聯式複合; 0=二聯式; 8=學制、類別公版; 9=專屬) 欄位名稱常數定義
			/// </summary>
			public const string Paystyle = "PayStyle";

			#region A 聯 機關長官、主辦會計、主辦出納 的職稱與姓名
			/// <summary>
			/// A聯機關長官職稱 欄位名稱常數定義
			/// </summary>
			public const string ATitle1 = "A_Title1";

			/// <summary>
			/// A聯機關長官姓名 欄位名稱常數定義
			/// </summary>
			public const string AName1 = "A_Name1";

			/// <summary>
			/// A聯主辦會計職稱 欄位名稱常數定義
			/// </summary>
			public const string ATitle2 = "A_Title2";

			/// <summary>
			/// A聯主辦會計姓名 欄位名稱常數定義
			/// </summary>
			public const string AName2 = "A_Name2";

			/// <summary>
			/// A聯主辦出納職稱 欄位名稱常數定義
			/// </summary>
			public const string ATitle3 = "A_Title3";

			/// <summary>
			/// A聯主辦出納姓名 欄位名稱常數定義
			/// </summary>
			public const string AName3 = "A_Name3";
			#endregion

			#region B 聯 機關長官、主辦會計、主辦出納 的職稱與姓名 (土銀沒用)
			/// <summary>
			/// B_Title1 欄位名稱常數定義
			/// </summary>
			public const string BTitle1 = "B_Title1";

			/// <summary>
			/// B_Name1 欄位名稱常數定義
			/// </summary>
			public const string BName1 = "B_Name1";

			/// <summary>
			/// B_Title2 欄位名稱常數定義
			/// </summary>
			public const string BTitle2 = "B_Title2";

			/// <summary>
			/// B_Name2 欄位名稱常數定義
			/// </summary>
			public const string BName2 = "B_Name2";

			/// <summary>
			/// B_Title3 欄位名稱常數定義
			/// </summary>
			public const string BTitle3 = "B_Title3";

			/// <summary>
			/// B_Name3 欄位名稱常數定義
			/// </summary>
			public const string BName3 = "B_Name3";
			#endregion

			#region 收入科目列印在繳費單的哪一聯 (A、B、C) (土銀沒用)
			/// <summary>
			/// 收入科目01列印在繳費單的哪一聯 (A、B、C) 欄位名稱常數定義 (土銀沒用)
			/// </summary>
			public const string Itema1 = "ItemA1";

			/// <summary>
			/// ItemA2 欄位名稱常數定義
			/// </summary>
			public const string Itema2 = "ItemA2";

			/// <summary>
			/// ItemA3 欄位名稱常數定義
			/// </summary>
			public const string Itema3 = "ItemA3";

			/// <summary>
			/// ItemA4 欄位名稱常數定義
			/// </summary>
			public const string Itema4 = "ItemA4";

			/// <summary>
			/// ItemA5 欄位名稱常數定義
			/// </summary>
			public const string Itema5 = "ItemA5";

			/// <summary>
			/// ItemA6 欄位名稱常數定義
			/// </summary>
			public const string Itema6 = "ItemA6";

			/// <summary>
			/// ItemA7 欄位名稱常數定義
			/// </summary>
			public const string Itema7 = "ItemA7";

			/// <summary>
			/// ItemA8 欄位名稱常數定義
			/// </summary>
			public const string Itema8 = "ItemA8";

			/// <summary>
			/// ItemA9 欄位名稱常數定義
			/// </summary>
			public const string Itema9 = "ItemA9";

			/// <summary>
			/// ItemA10 欄位名稱常數定義
			/// </summary>
			public const string Itema10 = "ItemA10";

			/// <summary>
			/// ItemA11 欄位名稱常數定義
			/// </summary>
			public const string Itema11 = "ItemA11";

			/// <summary>
			/// ItemA12 欄位名稱常數定義
			/// </summary>
			public const string Itema12 = "ItemA12";

			/// <summary>
			/// ItemA13 欄位名稱常數定義
			/// </summary>
			public const string Itema13 = "ItemA13";

			/// <summary>
			/// ItemA14 欄位名稱常數定義
			/// </summary>
			public const string Itema14 = "ItemA14";

			/// <summary>
			/// ItemA15 欄位名稱常數定義
			/// </summary>
			public const string Itema15 = "ItemA15";

			/// <summary>
			/// ItemA16 欄位名稱常數定義
			/// </summary>
			public const string Itema16 = "ItemA16";
			#endregion

			/// <summary>
			/// 備註 A (土銀沒用) 欄位名稱常數定義
			/// </summary>
			public const string ReceiveMemoa = "Receive_MemoA";

			/// <summary>
			/// 備註 B (土銀沒用) 欄位名稱常數定義
			/// </summary>
			public const string ReceiveMemob = "Receive_MemoB";

			/// <summary>
			/// 最後更新者 UserId 欄位名稱常數定義
			/// </summary>
			public const string UpdateWho = "Update_Who";

			/// <summary>
			/// 最後更新時間 欄位名稱常數定義
			/// </summary>
			public const string UpdateTime = "Update_Time";

			/// <summary>
			/// 超商可延遲日數 欄位名稱常數定義
			/// </summary>
			public const string ExtraDays = "Extra_Days";

			/// <summary>
			/// 繳費單上就學貸款可貸金額列印欄位 (1=可貸金額; 2=就貸金額) 欄位名稱常數定義
			/// </summary>
			public const string LoanQual = "loan_qual";

			#region 6 項注意事項
			/// <summary>
			/// 繳費單(通訊聯)注意事項1內容 欄位名稱常數定義
			/// </summary>
			public const string Brief1 = "brief1";

			/// <summary>
			///v繳費單(通訊聯)注意事項2內容 欄位名稱常數定義
			/// </summary>
			public const string Brief2 = "brief2";

			/// <summary>
			/// 繳費單(通訊聯)注意事項3內容 欄位名稱常數定義
			/// </summary>
			public const string Brief3 = "brief3";

			/// <summary>
			/// 繳費單(通訊聯)注意事項4內容 欄位名稱常數定義
			/// </summary>
			public const string Brief4 = "brief4";

			/// <summary>
			/// 繳費單(通訊聯)注意事項5內容 欄位名稱常數定義
			/// </summary>
			public const string Brief5 = "brief5";

			/// <summary>
			/// 繳費單(通訊聯)注意事項6內容 欄位名稱常數定義
			/// </summary>
			public const string Brief6 = "brief6";
			#endregion

			/// <summary>
			/// 開放列印日期 (格式：yyyyMMdd) 欄位名稱常數定義
			/// </summary>
			public const string BillValidDate = "Bill_Valid_Date";

			/// <summary>
			/// (啟用大專院校學費電子化申報) 是否顯示資料旗標 (Y=學生可看到繳費單或收據) 欄位名稱常數定義
			/// </summary>
			public const string Hide = "hide";

			/// <summary>
			/// (啟用大專院校學費電子化申報) 學制 (2=二專; 5=二專; B1=大學; B2=四技; C=二技; D=博士; M=碩士) 欄位名稱常數定義
			/// </summary>
			public const string SchLevel = "sch_level";

			/// <summary>
			/// (啟用大專院校學費電子化申報) 收入科目是否申報學雜費旗標MAPPING (40個字元，由左至右對應01至40，Y=申報) 欄位名稱常數定義
			/// </summary>
			public const string EduTax = "edu_tax";

			/// <summary>
			/// (啟用大專院校學費電子化申報) 收入科目是否申報住宿費旗標MAPPING (40個字元，由左至右對應01至40，Y=申報) 欄位名稱常數定義
			/// </summary>
			public const string StayTax = "stay_tax";

			/// <summary>
			/// 是否啟用大專院校學費電子化申報旗標 (Y=啟用) 欄位名稱常數定義
			/// </summary>
			public const string EnabledTax = "enabled_tax";

			/// <summary>
			/// 關閉列印日期 (格式：yyyyMMdd) 欄位名稱常數定義
			/// </summary>
			public const string BillCloseDate = "Bill_Close_Date";

			/// <summary>
			/// 繳費單模板代碼 欄位名稱常數定義
			/// </summary>
			public const string BillformId = "BillForm_id";

			#region 40 項收入科目是否教育部補助旗標 (Y=是; N=否)
			/// <summary>
			/// 收入科目01是否教育部補助旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy01 = "isSubsidy01";

			/// <summary>
			/// 收入科目02是否教育部補助旗標 (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy02 = "isSubsidy02";

			/// <summary>
			/// isSubsidy03 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy03 = "isSubsidy03";

			/// <summary>
			/// isSubsidy04 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy04 = "isSubsidy04";

			/// <summary>
			/// isSubsidy05 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy05 = "isSubsidy05";

			/// <summary>
			/// isSubsidy06 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy06 = "isSubsidy06";

			/// <summary>
			/// isSubsidy07 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy07 = "isSubsidy07";

			/// <summary>
			/// isSubsidy08 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy08 = "isSubsidy08";

			/// <summary>
			/// isSubsidy09 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy09 = "isSubsidy09";

			/// <summary>
			/// isSubsidy10 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy10 = "isSubsidy10";

			/// <summary>
			/// isSubsidy11 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy11 = "isSubsidy11";

			/// <summary>
			/// isSubsidy12 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy12 = "isSubsidy12";

			/// <summary>
			/// isSubsidy13 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy13 = "isSubsidy13";

			/// <summary>
			/// isSubsidy14 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy14 = "isSubsidy14";

			/// <summary>
			/// isSubsidy15 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy15 = "isSubsidy15";

			/// <summary>
			/// isSubsidy16 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy16 = "isSubsidy16";

			/// <summary>
			/// isSubsidy17 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy17 = "isSubsidy17";

			/// <summary>
			/// isSubsidy18 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy18 = "isSubsidy18";

			/// <summary>
			/// isSubsidy19 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy19 = "isSubsidy19";

			/// <summary>
			/// isSubsidy20 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy20 = "isSubsidy20";

			/// <summary>
			/// isSubsidy21 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy21 = "isSubsidy21";

			/// <summary>
			/// isSubsidy22 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy22 = "isSubsidy22";

			/// <summary>
			/// isSubsidy23 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy23 = "isSubsidy23";

			/// <summary>
			/// isSubsidy24 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy24 = "isSubsidy24";

			/// <summary>
			/// isSubsidy25 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy25 = "isSubsidy25";

			/// <summary>
			/// isSubsidy26 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy26 = "isSubsidy26";

			/// <summary>
			/// isSubsidy27 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy27 = "isSubsidy27";

			/// <summary>
			/// isSubsidy28 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy28 = "isSubsidy28";

			/// <summary>
			/// isSubsidy29 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy29 = "isSubsidy29";

			/// <summary>
			/// isSubsidy30 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy30 = "isSubsidy30";

			/// <summary>
			/// isSubsidy31 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy31 = "isSubsidy31";

			/// <summary>
			/// isSubsidy32 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy32 = "isSubsidy32";

			/// <summary>
			/// isSubsidy33 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy33 = "isSubsidy33";

			/// <summary>
			/// isSubsidy34 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy34 = "isSubsidy34";

			/// <summary>
			/// isSubsidy35 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy35 = "isSubsidy35";

			/// <summary>
			/// isSubsidy36 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy36 = "isSubsidy36";

			/// <summary>
			/// isSubsidy37 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy37 = "isSubsidy37";

			/// <summary>
			/// isSubsidy38 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy38 = "isSubsidy38";

			/// <summary>
			/// isSubsidy39 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy39 = "isSubsidy39";

			/// <summary>
			/// isSubsidy40 欄位名稱常數定義
			/// </summary>
			public const string Issubsidy40 = "isSubsidy40";
			#endregion

			/// <summary>
			/// 郵局可延遲日數 (土銀沒用，固定設為 0) 欄位名稱常數定義
			/// </summary>
			public const string PostExtraDays = "Post_Extra_Days";

			#region C 聯 機關長官、主辦會計、主辦出納 的職稱與姓名 (土銀沒用)
			/// <summary>
			/// C_Title1 欄位名稱常數定義
			/// </summary>
			public const string CTitle1 = "C_Title1";

			/// <summary>
			/// C_Name1 欄位名稱常數定義
			/// </summary>
			public const string CName1 = "C_Name1";

			/// <summary>
			/// C_Title2 欄位名稱常數定義
			/// </summary>
			public const string CTitle2 = "C_Title2";

			/// <summary>
			/// C_Name2 欄位名稱常數定義
			/// </summary>
			public const string CName2 = "C_Name2";

			/// <summary>
			/// C_Title3 欄位名稱常數定義
			/// </summary>
			public const string CTitle3 = "C_Title3";

			/// <summary>
			/// C_Name3 欄位名稱常數定義
			/// </summary>
			public const string CName3 = "C_Name3";
			#endregion

			/// <summary>
			/// 備註 C (土銀沒用) 欄位名稱常數定義
			/// </summary>
			public const string ReceiveMemoc = "Receive_MemoC";

			#region 印鑑相關 (土銀沒用)
			/// <summary>
			/// 是否使用印鑑旗標 (Y=是; N=否) (土銀沒用，固定設為 N) 欄位名稱常數定義
			/// </summary>
			public const string Usestamp = "useStamp";

			/// <summary>
			/// A聯機關長官印鑑樣式圖檔Byte內容 欄位名稱常數定義
			/// </summary>
			public const string AStamp1 = "A_Stamp1";

			/// <summary>
			/// A_Stamp2 欄位名稱常數定義
			/// </summary>
			public const string AStamp2 = "A_Stamp2";

			/// <summary>
			/// A_Stamp3 欄位名稱常數定義
			/// </summary>
			public const string AStamp3 = "A_Stamp3";

			/// <summary>
			/// B_Stamp1 欄位名稱常數定義
			/// </summary>
			public const string BStamp1 = "B_Stamp1";

			/// <summary>
			/// B_Stamp2 欄位名稱常數定義
			/// </summary>
			public const string BStamp2 = "B_Stamp2";

			/// <summary>
			/// B_Stamp3 欄位名稱常數定義
			/// </summary>
			public const string BStamp3 = "B_Stamp3";

			/// <summary>
			/// C_Stamp1 欄位名稱常數定義
			/// </summary>
			public const string CStamp1 = "C_Stamp1";

			/// <summary>
			/// C_Stamp2 欄位名稱常數定義
			/// </summary>
			public const string CStamp2 = "C_Stamp2";

			/// <summary>
			/// C_Stamp3 欄位名稱常數定義
			/// </summary>
			public const string CStamp3 = "C_Stamp3";
			#endregion

			#region 浮水印相關 (土銀沒用)
			/// <summary>
			/// 是否使用浮水印旗標 (Y=使用; 否則=不用) (土銀沒用，固定設為 N) 欄位名稱常數定義
			/// </summary>
			public const string Usewatermark = "useWaterMark";

			/// <summary>
			/// 浮水印圖檔Byte內容 欄位名稱常數定義
			/// </summary>
			public const string Watermark = "watermark";
			#endregion

			/// <summary>
			/// 收據模版代碼 欄位名稱常數定義
			/// </summary>
			public const string InvoiceformId = "invoiceform_id";

			/// <summary>
			/// 是否使用郵局繳款期限 (Y=是; N=否) (土銀沒用，固定設為 空白) 欄位名稱常數定義
			/// </summary>
			public const string Usepostdueday = "usePostDueDay";

			#region [MDY:202203XX] 2022擴充案 對應欄位已擴充，無須這四項對應旗標欄位 (不再使用，固定設為 空白)
			/// <summary>
			/// 其他收入科目是否就貸旗標，null 自動轉成空字串並自動去頭尾空白 (不再使用，固定設為 空白) (索引 0 ~ 23 的字元對應收入科目 17 ~ 40) (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string LoanItemOthers = "Loan_Item_Others";

			/// <summary>
			/// 其他收入科目是否教育部補助旗標，null 自動轉成空字串並自動去頭尾空白 (不再使用，固定設為 空白) (索引 0 ~ 23 的字元對應收入科目 17 ~ 40) (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string IssubsidyOthers = "isSubsidy_Others";

			/// <summary>
			/// 其他收入科目是否代辦旗標 (不再使用，固定設為 空白) (索引 0 ~ 24 的字元對應 收入科目 17 ~ 40) (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string AgencyItemOthers = "Agency_Item_Others";

			/// <summary>
			/// 其他收入科目是否可更改旗標 (不再使用，固定設為 空白) (索引 0 ~ 23 的字元對應 收入科目 17 ~ 40) (Y=是; N=否) 欄位名稱常數定義
			/// </summary>
			public const string AgencyCheckOthers = "Agency_Check_Others";
			#endregion

			#region 21 項備註標題 (土銀新增)
            /// <summary>
            /// 備註標題01
            /// </summary>
            public const string MemoTitle01 = "Memo_Title01";

            /// <summary>
            /// 備註標題02
            /// </summary>
            public const string MemoTitle02 = "Memo_Title02";

            /// <summary>
            /// 備註標題03
            /// </summary>
            public const string MemoTitle03 = "Memo_Title03";

            /// <summary>
            /// 備註標題04
            /// </summary>
            public const string MemoTitle04 = "Memo_Title04";

            /// <summary>
            /// 備註標題05
            /// </summary>
            public const string MemoTitle05 = "Memo_Title05";

            /// <summary>
            /// 備註標題06
            /// </summary>
            public const string MemoTitle06 = "Memo_Title06";

            /// <summary>
            /// 備註標題07
            /// </summary>
            public const string MemoTitle07 = "Memo_Title07";

            /// <summary>
            /// 備註標題08
            /// </summary>
            public const string MemoTitle08 = "Memo_Title08";

            /// <summary>
            /// 備註標題09
            /// </summary>
            public const string MemoTitle09 = "Memo_Title09";

            /// <summary>
            /// 備註標題10
            /// </summary>
            public const string MemoTitle10 = "Memo_Title10";

            /// <summary>
            /// 備註標題11
            /// </summary>
            public const string MemoTitle11 = "Memo_Title11";

            /// <summary>
            /// 備註標題12
            /// </summary>
            public const string MemoTitle12 = "Memo_Title12";

            /// <summary>
            /// 備註標題13
            /// </summary>
            public const string MemoTitle13 = "Memo_Title13";

            /// <summary>
            /// 備註標題14
            /// </summary>
            public const string MemoTitle14 = "Memo_Title14";

            /// <summary>
            /// 備註標題15
            /// </summary>
            public const string MemoTitle15 = "Memo_Title15";

            /// <summary>
            /// 備註標題16
            /// </summary>
            public const string MemoTitle16 = "Memo_Title16";

            /// <summary>
            /// 備註標題17
            /// </summary>
            public const string MemoTitle17 = "Memo_Title17";

            /// <summary>
            /// 備註標題18
            /// </summary>
            public const string MemoTitle18 = "Memo_Title18";

            /// <summary>
            /// 備註標題19
            /// </summary>
            public const string MemoTitle19 = "Memo_Title19";

            /// <summary>
            /// 備註標題20
            /// </summary>
            public const string MemoTitle20 = "Memo_Title20";

            /// <summary>
            /// 備註標題21
            /// </summary>
            public const string MemoTitle21 = "Memo_Title21";
			#endregion

			#region [MDY:2018xxxx] 新增 列印收據關閉日、備註不自動換行 欄位
			/// <summary>
			/// 列印收據關閉日 (格式：yyyyMMdd)
			/// </summary>
			public const string InvoiceCloseDate = "Invoice_Close_Date";

			/// <summary>
			/// 備註是否動換行 (Y:是 / N:否) (預設N)
			/// </summary>
			public const string ReceiveMemoNoWrap = "Receive_Memo_NoWrap";
			#endregion

			#region [MDY:202203XX] 2022擴充案 收入科目英文名稱 欄位
			/// <summary>
			/// 收入科目01英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE01 = "Receive_ItemE01";

			/// <summary>
			/// 收入科目02英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE02 = "Receive_ItemE02";

			/// <summary>
			/// 收入科目03英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE03 = "Receive_ItemE03";

			/// <summary>
			/// 收入科目04英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE04 = "Receive_ItemE04";

			/// <summary>
			/// 收入科目05英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE05 = "Receive_ItemE05";

			/// <summary>
			/// 收入科目06英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE06 = "Receive_ItemE06";

			/// <summary>
			/// 收入科目07英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE07 = "Receive_ItemE07";

			/// <summary>
			/// 收入科目08英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE08 = "Receive_ItemE08";

			/// <summary>
			/// 收入科目09英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE09 = "Receive_ItemE09";

			/// <summary>
			/// 收入科目10英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE10 = "Receive_ItemE10";

			/// <summary>
			/// 收入科目11英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE11 = "Receive_ItemE11";

			/// <summary>
			/// 收入科目12英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE12 = "Receive_ItemE12";

			/// <summary>
			/// 收入科目13英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE13 = "Receive_ItemE13";

			/// <summary>
			/// 收入科目14英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE14 = "Receive_ItemE14";

			/// <summary>
			/// 收入科目15英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE15 = "Receive_ItemE15";

			/// <summary>
			/// 收入科目16英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE16 = "Receive_ItemE16";

			/// <summary>
			/// 收入科目17英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE17 = "Receive_ItemE17";

			/// <summary>
			/// 收入科目18英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE18 = "Receive_ItemE18";

			/// <summary>
			/// 收入科目19英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE19 = "Receive_ItemE19";

			/// <summary>
			/// 收入科目20英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE20 = "Receive_ItemE20";

			/// <summary>
			/// 收入科目21英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE21 = "Receive_ItemE21";

			/// <summary>
			/// 收入科目22英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE22 = "Receive_ItemE22";

			/// <summary>
			/// 收入科目23英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE23 = "Receive_ItemE23";

			/// <summary>
			/// 收入科目24英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE24 = "Receive_ItemE24";

			/// <summary>
			/// 收入科目25英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE25 = "Receive_ItemE25";

			/// <summary>
			/// 收入科目26英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE26 = "Receive_ItemE26";

			/// <summary>
			/// 收入科目27英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE27 = "Receive_ItemE27";

			/// <summary>
			/// 收入科目28英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE28 = "Receive_ItemE28";

			/// <summary>
			/// 收入科目29英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE29 = "Receive_ItemE29";

			/// <summary>
			/// 收入科目30英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE30 = "Receive_ItemE30";

			/// <summary>
			/// 收入科目31英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE31 = "Receive_ItemE31";

			/// <summary>
			/// 收入科目32英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE32 = "Receive_ItemE32";

			/// <summary>
			/// 收入科目33英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE33 = "Receive_ItemE33";

			/// <summary>
			/// 收入科目34英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE34 = "Receive_ItemE34";

			/// <summary>
			/// 收入科目35英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE35 = "Receive_ItemE35";

			/// <summary>
			/// 收入科目36英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE36 = "Receive_ItemE36";

			/// <summary>
			/// 收入科目37英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE37 = "Receive_ItemE37";

			/// <summary>
			/// 收入科目38英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE38 = "Receive_ItemE38";

			/// <summary>
			/// 收入科目39英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE39 = "Receive_ItemE39";

			/// <summary>
			/// 收入科目40英文名稱 欄位名稱常數定義
			/// </summary>
			public const string ReceiveItemE40 = "Receive_ItemE40";
			#endregion

			#region [MDY:202203XX] 2022擴充案 備註英文標題 欄位
			/// <summary>
			/// 備註01英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE01 = "Memo_TitleE01";

			/// <summary>
			/// 備註02英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE02 = "Memo_TitleE02";

			/// <summary>
			/// 備註03英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE03 = "Memo_TitleE03";

			/// <summary>
			/// 備註04英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE04 = "Memo_TitleE04";

			/// <summary>
			/// 備註05英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE05 = "Memo_TitleE05";

			/// <summary>
			/// 備註06英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE06 = "Memo_TitleE06";

			/// <summary>
			/// 備註07英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE07 = "Memo_TitleE07";

			/// <summary>
			/// 備註08英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE08 = "Memo_TitleE08";

			/// <summary>
			/// 備註09英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE09 = "Memo_TitleE09";

			/// <summary>
			/// 備註10英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE10 = "Memo_TitleE10";

			/// <summary>
			/// 備註11英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE11 = "Memo_TitleE11";

			/// <summary>
			/// 備註12英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE12 = "Memo_TitleE12";

			/// <summary>
			/// 備註13英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE13 = "Memo_TitleE13";

			/// <summary>
			/// 備註14英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE14 = "Memo_TitleE14";

			/// <summary>
			/// 備註15英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE15 = "Memo_TitleE15";

			/// <summary>
			/// 備註16英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE16 = "Memo_TitleE16";

			/// <summary>
			/// 備註17英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE17 = "Memo_TitleE17";

			/// <summary>
			/// 備註18英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE18 = "Memo_TitleE18";

			/// <summary>
			/// 備註19英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE19 = "Memo_TitleE19";

			/// <summary>
			/// 備註20英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE20 = "Memo_TitleE20";

			/// <summary>
			/// 備註21英文標題 欄位名稱常數定義
			/// </summary>
			public const string MemoTitleE21 = "Memo_TitleE21";
			#endregion

			#region [MDY:202203XX] 2022擴充案 注意事項英文內容 欄位
			/// <summary>
			/// 繳費單(通訊聯)注意事項1英文內容 欄位名稱常數定義
			/// </summary>
			public const string BriefE1 = "BriefE1";

			/// <summary>
			///v繳費單(通訊聯)注意事項2英文內容 欄位名稱常數定義
			/// </summary>
			public const string BriefE2 = "BriefE2";

			/// <summary>
			/// 繳費單(通訊聯)注意事項3英文內容 欄位名稱常數定義
			/// </summary>
			public const string BriefE3 = "BriefE3";

			/// <summary>
			/// 繳費單(通訊聯)注意事項4英文內容 欄位名稱常數定義
			/// </summary>
			public const string BriefE4 = "BriefE4";

			/// <summary>
			/// 繳費單(通訊聯)注意事項5英文內容 欄位名稱常數定義
			/// </summary>
			public const string BriefE5 = "BriefE5";

			/// <summary>
			/// 繳費單(通訊聯)注意事項6英文內容 欄位名稱常數定義
			/// </summary>
			public const string BriefE6 = "BriefE6";
			#endregion

			#region [MDY:202203XX] 2022擴充案 英文模板 欄位
			/// <summary>
			/// 繳費單英文模板代號 欄位名稱常數定義
			/// </summary>
			public const string BillFormEId = "BillFormE_Id";

			/// <summary>
			/// 收據英文模板代號 欄位名稱常數定義
			/// </summary>
			public const string InvoiceFormEId = "InvoiceFormE_Id";
			#endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// SchoolRidEntity 類別建構式
		/// </summary>
		public SchoolRidEntity()
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

		private string _ReceiveStatus = "1";
		/// <summary>
		/// 代收費用型態 (土銀沒用，固定設為 1) (1=已有繳費者資料之收費; 2=需登錄繳費者資料之收費 (本校生); 3=為需登錄繳費者資料之收費 (不分，本校或外界人士)
		/// </summary>
		[FieldSpec(Field.ReceiveStatus, true, FieldTypeEnum.Char, 1, false)]
		public string ReceiveStatus
		{
			get
			{
				return _ReceiveStatus;
			}
			set
			{
				//_ReceiveStatus = value == null ? null : value.Trim();
			}
		}

		private string _YearId = null;
		/// <summary>
        /// 學年代碼
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
        /// 學期代碼
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

		private string _DepId = String.Empty;
		/// <summary>
		/// 部別代碼 (土銀不使用此部別，固定設為 空白)
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
				_DepId = value == null ? String.Empty : value.Trim();
			}
		}

		private string _ReceiveId = null;
		/// <summary>
		/// 代收費用別代碼
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
		#endregion

		#region Data
		/// <summary>
		/// 字軌
		/// </summary>
		[FieldSpec(Field.SchWord, false, FieldTypeEnum.NVarChar, 40, true)]
		public string SchWord
		{
			get;
			set;
		}

		/// <summary>
		/// 學校會計 (土銀沒用)
		/// </summary>
		[FieldSpec(Field.SchAccounting, false, FieldTypeEnum.NVarChar, 10, true)]
		public string SchAccounting
		{
			get;
			set;
		}

		/// <summary>
		/// 學校出納 (土銀沒用)
		/// </summary>
		[FieldSpec(Field.SchCashier, false, FieldTypeEnum.NVarChar, 10, true)]
		public string SchCashier
		{
			get;
			set;
		}

		/// <summary>
        /// 繳費期限 (民國年3碼+月2碼+日2碼)
		/// </summary>
		[FieldSpec(Field.PayDate, false, FieldTypeEnum.Char, 7, true)]
		public string PayDate
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費期限2 (中信信用卡) (民國年3碼+月2碼+日2碼) (土銀用來存放中信信用卡繳費期限)
		/// </summary>
		[FieldSpec(Field.PayDueDate2, false, FieldTypeEnum.Char, 7, true)]
		public string PayDueDate2
        {
            get;
            set;
        }

		/// <summary>
		/// 繳費期限3 (財金信用卡) (民國年3碼+月2碼+日2碼) (土銀用來存放財金信用卡繳費期限)
		/// </summary>
		[FieldSpec(Field.PayDueDate3, false, FieldTypeEnum.Char, 7, true)]
		public string PayDueDate3
        {
            get;
            set;
        }

		/// <summary>
        /// 扣款期限 (民國年3碼+月2碼+日2碼) (停用)
		/// </summary>
		[FieldSpec(Field.DelmDate, false, FieldTypeEnum.Char, 7, true)]
		public string DelmDate
		{
			get;
			set;
		}

		/// <summary>
		/// 學生學分費計算方式 (1=此代收費用別無學分費之收入科目; 2=以學分數計算; 3=以學分數計算; 4=以小於某學分數才收學分費; 5=以小於某上課時數才以上課時數收學分費; 6=以小於某學分數才以上課時數收學分費)
		/// </summary>
		[FieldSpec(Field.StudentType, false, FieldTypeEnum.Char, 1, true)]
		public string StudentType
		{
			get;
			set;
		}

		/// <summary>
		/// CS_Type (停用) 欄位屬性
		/// </summary>
		[FieldSpec(Field.CsType, false, FieldTypeEnum.Char, 1, true)]
		public string CsType
		{
			get;
			set;
		}

		/// <summary>
		/// 課程或學分基準所屬收入科目
		/// </summary>
		[FieldSpec(Field.CreditItem, false, FieldTypeEnum.Char, 2, true)]
		public string CreditItem
		{
			get;
			set;
		}

		/// <summary>
		/// 整批退費所屬收入科目
		/// </summary>
		[FieldSpec(Field.ReturnItem, false, FieldTypeEnum.Char, 2, true)]
		public string ReturnItem
		{
			get;
			set;
		}

		/// <summary>
		/// 學分費比較基準
		/// </summary>
		[FieldSpec(Field.CreditBasic, false, FieldTypeEnum.Integer, true)]
		public int? CreditBasic
		{
			get;
			set;
		}

		/// <summary>
        /// 計算金額順序 (1=減免先; 2=就貸先)
		/// </summary>
		[FieldSpec(Field.BillingType, false, FieldTypeEnum.Char, 1, true)]
		public string BillingType
		{
			get;
			set;
		}

		#region 40 項收入科目名稱
		/// <summary>
		/// 收入科目01名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem01, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem01
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目02名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem02, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem02
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目03名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem03, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem03
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目04名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem04, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem04
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目05名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem05, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem05
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目06名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem06, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem06
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目07名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem07, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem07
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目08名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem08, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem08
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目09名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem09, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem09
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目10名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem10, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem10
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目11名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem11, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem11
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目12名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem12, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem12
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目13名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem13, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem13
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目14名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem14, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem14
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目15名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem15, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem15
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目16名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem16, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem16
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目17名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem17, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem17
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目18名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem18, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem18
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目19名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem19, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem19
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目20名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem20, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem20
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目21名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem21, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem21
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目22名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem22, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem22
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目23名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem23, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem23
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目24名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem24, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem24
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目25名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem25, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem25
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目26名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem26, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem26
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目27名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem27, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem27
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目28名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem28, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem28
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目29名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem29, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem29
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目30名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem30, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem30
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目31名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem31, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem31
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目32名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem32, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem32
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目33名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem33, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem33
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目34名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem34, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem34
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目35名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem35, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem35
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目36名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem36, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem36
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目37名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem37, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem37
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目38名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem38, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem38
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目39名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem39, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem39
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目40名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItem40, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItem40
		{
			get;
			set;
		}
		#endregion

		#region 40 項收入科目是否助貸旗標 (Y=是; N=否)
		/// <summary>
		/// 收入科目01是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem01, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem01
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目02是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem02, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem02
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目03是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem03, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem03
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目04是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem04, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem04
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目05是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem05, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem05
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目06是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem06, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem06
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目07是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem07, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem07
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目08是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem08, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem08
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目09是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem09, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem09
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目10是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem10, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem10
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目11是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem11, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem11
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目12是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem12, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem12
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目13是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem13, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem13
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目14是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem14, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem14
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目15是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem15, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem15
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目16是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem16, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem16
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目17是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem17, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem17
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目18是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem18, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem18
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目19是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem19, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem19
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目20是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem20, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem20
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目21是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem21, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem21
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目22是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem22, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem22
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目23是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem23, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem23
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目24是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem24, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem24
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目25是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem25, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem25
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目26是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem26, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem26
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目27是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem27, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem27
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目28是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem28, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem28
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目29是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem29, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem29
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目30是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem30, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem30
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目31是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem31, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem31
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目32是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem32, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem32
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目33是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem33, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem33
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目34是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem34, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem34
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目35是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem35, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem35
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目36是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem36, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem36
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目37是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem37, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem37
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目38是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem38, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem38
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目39是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem39, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem39
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目40是否助貸旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.LoanItem40, false, FieldTypeEnum.Char, 1, true)]
		public string LoanItem40
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 除收入科目外就學貸款可貸之額外固定金額
		/// </summary>
		[FieldSpec(Field.LoanFee, false, FieldTypeEnum.Decimal, true)]
		public decimal? LoanFee
		{
			get;
			set;
		}

		#region 40 項收入科目是否代辦旗標 (土銀沒用) (Y=是; N=否)
		/// <summary>
		/// 收入科目01是否代辦旗標 (土銀沒用) (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.AgencyItem01, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem01
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item02 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem02, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem02
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item03 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem03, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem03
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item04 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem04, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem04
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item05 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem05, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem05
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item06 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem06, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem06
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item07 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem07, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem07
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item08 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem08, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem08
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item09 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem09, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem09
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item10 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem10, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem10
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item11 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem11, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem11
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item12 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem12, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem12
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item13 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem13, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem13
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item14 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem14, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem14
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item15 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem15, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem15
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item16 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem16, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem16
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item17 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem17, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem17
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item18 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem18, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem18
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item19 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem19, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem19
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item20 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem20, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem20
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item21 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem21, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem21
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item22 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem22, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem22
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item23 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem23, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem23
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item24 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem24, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem24
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item25 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem25, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem25
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item26 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem26, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem26
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item27 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem27, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem27
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item28 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem28, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem28
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item29 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem29, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem29
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item30 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem30, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem30
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item31 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem31, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem31
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item32 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem32, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem32
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item33 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem33, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem33
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item34 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem34, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem34
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item35 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem35, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem35
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item36 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem36, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem36
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item37 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem37, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem37
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item38 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem38, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem38
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item39 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem39, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem39
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Item40 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItem40, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyItem40
		{
			get;
			set;
		}
		#endregion

		#region 40 項收入科目是否可更改旗標 (土銀沒用) (Y=是; N=否)
		/// <summary>
		/// 收入科目01是否可更改旗標 (土銀沒用) (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.AgencyCheck01, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck01
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check02 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck02, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck02
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check03 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck03, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck03
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check04 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck04, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck04
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check05 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck05, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck05
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check06 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck06, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck06
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check07 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck07, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck07
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check08 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck08, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck08
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check09 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck09, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck09
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check10 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck10, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck10
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check11 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck11, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck11
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check12 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck12, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck12
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check13 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck13, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck13
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check14 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck14, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck14
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check15 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck15, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck15
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check16 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck16, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck16
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check17 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck17, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck17
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check18 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck18, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck18
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check19 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck19, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck19
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check20 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck20, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck20
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check21 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck21, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck21
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check22 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck22, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck22
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check23 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck23, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck23
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check24 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck24, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck24
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check25 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck25, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck25
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check26 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck26, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck26
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check27 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck27, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck27
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check28 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck28, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck28
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check29 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck29, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck29
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check30 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck30, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck30
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check31 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck31, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck31
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check32 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck32, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck32
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check33 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck33, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck33
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check34 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck34, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck34
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check35 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck35, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck35
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check36 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck36, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck36
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check37 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck37, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck37
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check38 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck38, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck38
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check39 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck39, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck39
		{
			get;
			set;
		}

		/// <summary>
		/// Agency_Check40 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheck40, false, FieldTypeEnum.Char, 1, true)]
		public string AgencyCheck40
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 備註 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReceiveMemo, false, FieldTypeEnum.NVarChar, 500, true)]
		public string ReceiveMemo
		{
			get;
			set;
		}

		/// <summary>
		/// FileName (停用) 欄位屬性
		/// </summary>
		[FieldSpec(Field.FileName, false, FieldTypeEnum.NVarChar, 30, true)]
		public string FileName
		{
			get;
			set;
		}

		/// <summary>
		/// FilePath (停用) 欄位屬性
		/// </summary>
		[FieldSpec(Field.FilePath, false, FieldTypeEnum.NVarChar, 200, true)]
		public string FilePath
		{
			get;
			set;
		}

		/// <summary>
		/// 依收費標準計算時，就貸可貸金額計算依據 ((null)0=依減免前繳費金額; (否則)1=依減免後繳費金額)
		/// </summary>
		[FieldSpec(Field.FlagRl, false, FieldTypeEnum.Char, 1, true)]
		public string FlagRl
		{
			get;
			set;
		}

		/// <summary>
		/// 收手續費旗標 (土銀沒用，固定設為 null) (1=臨櫃繳費需加收手續費; 0=免加手續費)
		/// </summary>
		[FieldSpec(Field.FeeFlag, false, FieldTypeEnum.Char, 1, true)]
		public string FeeFlag
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單格式 (土銀沒用) (""或1=三聯式預設; 2~6=三聯式可調; A~G=三聯式複合; 0=二聯式; 8=學制、類別公版; 9=專屬)
		/// </summary>
		[FieldSpec(Field.Paystyle, false, FieldTypeEnum.Char, 1, true)]
		public string Paystyle
		{
			get;
			set;
		}

		#region A 聯 機關長官、主辦會計、主辦出納 的職稱與姓名
		/// <summary>
		/// A聯機關長官職稱
		/// </summary>
		[FieldSpec(Field.ATitle1, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ATitle1
		{
			get;
			set;
		}

		/// <summary>
		/// A聯機關長官姓名
		/// </summary>
		[FieldSpec(Field.AName1, false, FieldTypeEnum.NVarChar, 40, true)]
		public string AName1
		{
			get;
			set;
		}

		/// <summary>
		/// A聯主辦會計職稱
		/// </summary>
		[FieldSpec(Field.ATitle2, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ATitle2
		{
			get;
			set;
		}

		/// <summary>
		///  A聯主辦會計姓名
		/// </summary>
		[FieldSpec(Field.AName2, false, FieldTypeEnum.NVarChar, 40, true)]
		public string AName2
		{
			get;
			set;
		}

		/// <summary>
		/// A聯主辦出納職稱
		/// </summary>
		[FieldSpec(Field.ATitle3, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ATitle3
		{
			get;
			set;
		}

		/// <summary>
		/// A聯主辦出納姓名
		/// </summary>
		[FieldSpec(Field.AName3, false, FieldTypeEnum.NVarChar, 40, true)]
		public string AName3
		{
			get;
			set;
		}
		#endregion

		#region B 聯 機關長官、主辦會計、主辦出納 的職稱與姓名 (土銀沒用)
		/// <summary>
		/// B_Title1 欄位屬性
		/// </summary>
		[FieldSpec(Field.BTitle1, false, FieldTypeEnum.NVarChar, 40, true)]
		public string BTitle1
		{
			get;
			set;
		}

		/// <summary>
		/// B_Name1 欄位屬性
		/// </summary>
		[FieldSpec(Field.BName1, false, FieldTypeEnum.NVarChar, 40, true)]
		public string BName1
		{
			get;
			set;
		}

		/// <summary>
		/// B_Title2 欄位屬性
		/// </summary>
		[FieldSpec(Field.BTitle2, false, FieldTypeEnum.NVarChar, 40, true)]
		public string BTitle2
		{
			get;
			set;
		}

		/// <summary>
		/// B_Name2 欄位屬性
		/// </summary>
		[FieldSpec(Field.BName2, false, FieldTypeEnum.NVarChar, 40, true)]
		public string BName2
		{
			get;
			set;
		}

		/// <summary>
		/// B_Title3 欄位屬性
		/// </summary>
		[FieldSpec(Field.BTitle3, false, FieldTypeEnum.NVarChar, 40, true)]
		public string BTitle3
		{
			get;
			set;
		}

		/// <summary>
		/// B_Name3 欄位屬性
		/// </summary>
		[FieldSpec(Field.BName3, false, FieldTypeEnum.NVarChar, 40, true)]
		public string BName3
		{
			get;
			set;
		}
		#endregion

		#region 收入科目列印在繳費單的哪一聯 (A、B、C) (土銀沒用)
		/// <summary>
		/// 收入科目01列印在繳費單的哪一聯 (A、B、C) (土銀沒用)
		/// </summary>
		[FieldSpec(Field.Itema1, false, FieldTypeEnum.Char, 1, true)]
		public string Itema1
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA2 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema2, false, FieldTypeEnum.Char, 1, true)]
		public string Itema2
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA3 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema3, false, FieldTypeEnum.Char, 1, true)]
		public string Itema3
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA4 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema4, false, FieldTypeEnum.Char, 1, true)]
		public string Itema4
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA5 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema5, false, FieldTypeEnum.Char, 1, true)]
		public string Itema5
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA6 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema6, false, FieldTypeEnum.Char, 1, true)]
		public string Itema6
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA7 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema7, false, FieldTypeEnum.Char, 1, true)]
		public string Itema7
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA8 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema8, false, FieldTypeEnum.Char, 1, true)]
		public string Itema8
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA9 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema9, false, FieldTypeEnum.Char, 1, true)]
		public string Itema9
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema10, false, FieldTypeEnum.Char, 1, true)]
		public string Itema10
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema11, false, FieldTypeEnum.Char, 1, true)]
		public string Itema11
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema12, false, FieldTypeEnum.Char, 1, true)]
		public string Itema12
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema13, false, FieldTypeEnum.Char, 1, true)]
		public string Itema13
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema14, false, FieldTypeEnum.Char, 1, true)]
		public string Itema14
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema15, false, FieldTypeEnum.Char, 1, true)]
		public string Itema15
		{
			get;
			set;
		}

		/// <summary>
		/// ItemA16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Itema16, false, FieldTypeEnum.Char, 1, true)]
		public string Itema16
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 備註A (土銀沒用)
		/// </summary>
		[FieldSpec(Field.ReceiveMemoa, false, FieldTypeEnum.NVarChar, 500, true)]
		public string ReceiveMemoa
		{
			get;
			set;
		}

		/// <summary>
		/// 備註B (土銀沒用)
		/// </summary>
		[FieldSpec(Field.ReceiveMemob, false, FieldTypeEnum.NVarChar, 500, true)]
		public string ReceiveMemob
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新者 UserId
		/// </summary>
		[FieldSpec(Field.UpdateWho, false, FieldTypeEnum.VarChar, 20, true)]
		public string UpdateWho
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新時間
		/// </summary>
		[FieldSpec(Field.UpdateTime, false, FieldTypeEnum.DateTime, true)]
		public DateTime? UpdateTime
		{
			get;
			set;
		}

		/// <summary>
		/// 超商可延遲日數
		/// </summary>
		[FieldSpec(Field.ExtraDays, false, FieldTypeEnum.Integer, false)]
		public int ExtraDays
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單上就學貸款可貸金額列印欄位 (1=可貸金額; 2=就貸金額)
		/// </summary>
		[FieldSpec(Field.LoanQual, false, FieldTypeEnum.Char, 1, false)]
		public string LoanQual
		{
			get;
			set;
		}

		#region 6 項注意事項
		/// <summary>
		/// 繳費單(通訊聯)注意事項1內容
		/// </summary>
		[FieldSpec(Field.Brief1, false, FieldTypeEnum.NVarChar, 500, true)]
		public string Brief1
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項2內容
		/// </summary>
		[FieldSpec(Field.Brief2, false, FieldTypeEnum.NVarChar, 500, true)]
		public string Brief2
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項3內容
		/// </summary>
		[FieldSpec(Field.Brief3, false, FieldTypeEnum.NVarChar, 500, true)]
		public string Brief3
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項4內容
		/// </summary>
		[FieldSpec(Field.Brief4, false, FieldTypeEnum.NVarChar, 500, true)]
		public string Brief4
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項5內容
		/// </summary>
		[FieldSpec(Field.Brief5, false, FieldTypeEnum.NVarChar, 500, true)]
		public string Brief5
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項6內容
		/// </summary>
		[FieldSpec(Field.Brief6, false, FieldTypeEnum.NVarChar, 500, true)]
		public string Brief6
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 開放列印日期 (格式：yyyyMMdd)
		/// </summary>
		[FieldSpec(Field.BillValidDate, false, FieldTypeEnum.Char, 8, true)]
		public string BillValidDate
		{
			get;
			set;
		}

		/// <summary>
		/// (啟用大專院校學費電子化申報) 是否顯示資料旗標 (Y=學生可看到繳費單或收據)
		/// </summary>
		[FieldSpec(Field.Hide, false, FieldTypeEnum.Char, 1, false)]
		public string Hide
		{
			get;
			set;
		}

		/// <summary>
		/// (啟用大專院校學費電子化申報) 學制 (2=二專; 5=二專; B1=大學; B2=四技; C=二技; D=博士; M=碩士)
		/// </summary>
		[FieldSpec(Field.SchLevel, false, FieldTypeEnum.VarChar, 2, false)]
		public string SchLevel
		{
			get;
			set;
		}

		#region [MDY:202203XX] 2022擴充案 改寫
		private static readonly string NTax = "".PadRight(ReceiveItemMaxCount, 'N');

		private string _EduTax = null;
		private Char[] _EduTaxFlags = NTax.ToCharArray();
		/// <summary>
		/// (啟用大專院校學費電子化申報) 收入科目是否申報學雜費旗標MAPPING (40個字元，由左至右對應01至40，Y=申報)
		/// </summary>
		[FieldSpec(Field.EduTax, false, FieldTypeEnum.Char, 40, true)]
		public string EduTax
		{
			get
			{
				if (String.IsNullOrEmpty(_EduTax))
				{
					_EduTax = String.Join("", _EduTaxFlags);
				}
				return _EduTax;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					_EduTax = NTax;
				}
				else
				{
					value = value.TrimEnd().PadRight(ReceiveItemMaxCount, 'N');
					if (value.Length > ReceiveItemMaxCount)
					{
						_EduTax = value.Substring(0, ReceiveItemMaxCount);
					}
					else
					{
						_EduTax = value;
					}
				}
				_EduTaxFlags = _EduTax.ToCharArray();
			}
		}

		private string _StayTax = null;
		private Char[] _StayTaxFlags = NTax.ToCharArray();
		/// <summary>
		/// (啟用大專院校學費電子化申報) 收入科目是否申報住宿費旗標MAPPING (40個字元，由左至右對應01至40，Y=申報)
		/// </summary>
		[FieldSpec(Field.StayTax, false, FieldTypeEnum.Char, 40, true)]
		public string StayTax
		{
			get
			{
				if (String.IsNullOrEmpty(_StayTax))
				{
					_StayTax = String.Join("", _StayTaxFlags);
				}
				return _StayTax;
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					_StayTax = NTax;
				}
				else
				{
					value = value.TrimEnd().PadRight(ReceiveItemMaxCount, 'N');
					if (value.Length > ReceiveItemMaxCount)
					{
						_StayTax = value.Substring(0, ReceiveItemMaxCount);
					}
					else
					{
						_StayTax = value;
					}
				}
				_StayTaxFlags = _StayTax.ToCharArray();
			}
		}
		#endregion

		/// <summary>
		/// 是否啟用大專院校學費電子化申報旗標 (Y=啟用)
		/// </summary>
		[FieldSpec(Field.EnabledTax, false, FieldTypeEnum.Char, 1, false)]
		public string EnabledTax
		{
			get;
			set;
		}

		private string _BillCloseDate = String.Empty;
		/// <summary>
        /// 關閉列印日期 (格式：yyyyMMdd)
		/// </summary>
		[FieldSpec(Field.BillCloseDate, false, FieldTypeEnum.Char, 8, false)]
		public string BillCloseDate
		{
            get
            {
                return _BillCloseDate;
            }
            set
            {
                _BillCloseDate = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
		}

		/// <summary>
		/// 繳費單模板代碼
		/// </summary>
		[FieldSpec(Field.BillformId, false, FieldTypeEnum.VarChar, 32, true)]
		public string BillformId
		{
			get;
			set;
		}

		#region 40 項收入科目是否教育部補助旗標 (Y=是; N=否)
		private string _Issubsidy01 = "N";
		/// <summary>
		/// 收入科目01是否教育部補助旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.Issubsidy01, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy01
		{
            get
            {
                return _Issubsidy01;
            }
            set
            {
                _Issubsidy01 = value == null ? "N" : value;
            }
		}

		private string _Issubsidy02 = "N";
		/// <summary>
		/// 收入科目02是否教育部補助旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.Issubsidy02, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy02
		{
            get
            {
                return _Issubsidy02;
            }
            set
            {
                _Issubsidy02 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy03 = "N";
		/// <summary>
		/// isSubsidy03 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy03, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy03
		{
            get
            {
                return _Issubsidy03;
            }
            set
            {
                _Issubsidy03 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy04 = "N";
		/// <summary>
		/// isSubsidy04 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy04, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy04
		{
            get
            {
                return _Issubsidy04;
            }
            set
            {
                _Issubsidy04 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy05 = "N";
		/// <summary>
		/// isSubsidy05 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy05, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy05
		{
            get
            {
                return _Issubsidy05;
            }
            set
            {
                _Issubsidy05 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy06 = "N";
		/// <summary>
		/// isSubsidy06 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy06, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy06
		{
            get
            {
                return _Issubsidy06;
            }
            set
            {
                _Issubsidy06 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy07 = "N";
		/// <summary>
		/// isSubsidy07 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy07, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy07
		{
            get
            {
                return _Issubsidy07;
            }
            set
            {
                _Issubsidy07 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy08 = "N";
		/// <summary>
		/// isSubsidy08 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy08, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy08
		{
            get
            {
                return _Issubsidy08;
            }
            set
            {
                _Issubsidy08 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy09 = "N";
		/// <summary>
		/// isSubsidy09 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy09, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy09
		{
            get
            {
                return _Issubsidy09;
            }
            set
            {
                _Issubsidy09 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy10 = "N";
		/// <summary>
		/// isSubsidy10 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy10, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy10
		{
            get
            {
                return _Issubsidy10;
            }
            set
            {
                _Issubsidy10 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy11 = "N";
		/// <summary>
		/// isSubsidy11 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy11, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy11
		{
            get
            {
                return _Issubsidy11;
            }
            set
            {
                _Issubsidy11 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy12 = "N";
		/// <summary>
		/// isSubsidy12 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy12, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy12
		{
            get
            {
                return _Issubsidy12;
            }
            set
            {
                _Issubsidy12 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy13 = "N";
		/// <summary>
		/// isSubsidy13 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy13, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy13
		{
            get
            {
                return _Issubsidy13;
            }
            set
            {
                _Issubsidy13 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy14 = "N";
		/// <summary>
		/// isSubsidy14 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy14, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy14
		{
            get
            {
                return _Issubsidy14;
            }
            set
            {
                _Issubsidy14 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy15 = "N";
		/// <summary>
		/// isSubsidy15 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy15, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy15
		{
            get
            {
                return _Issubsidy15;
            }
            set
            {
                _Issubsidy15 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy16 = "N";
		/// <summary>
		/// isSubsidy16 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy16, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy16
		{
            get
            {
                return _Issubsidy16;
            }
            set
            {
                _Issubsidy16 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy17 = "N";
		/// <summary>
		/// isSubsidy17 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy17, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy17
		{
            get
            {
                return _Issubsidy17;
            }
            set
            {
                _Issubsidy17 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy18 = "N";
		/// <summary>
		/// isSubsidy18 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy18, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy18
		{
            get
            {
                return _Issubsidy18;
            }
            set
            {
                _Issubsidy18 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy19 = "N";
		/// <summary>
		/// isSubsidy19 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy19, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy19
		{
            get
            {
                return _Issubsidy19;
            }
            set
            {
                _Issubsidy19 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy20 = "N";
		/// <summary>
		/// isSubsidy20 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy20, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy20
		{
            get
            {
                return _Issubsidy20;
            }
            set
            {
                _Issubsidy20 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy21 = "N";
		/// <summary>
		/// isSubsidy21 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy21, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy21
		{
            get
            {
                return _Issubsidy21;
            }
            set
            {
                _Issubsidy21 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy22 = "N";
		/// <summary>
		/// isSubsidy22 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy22, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy22
		{
            get
            {
                return _Issubsidy22;
            }
            set
            {
                _Issubsidy22 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy23 = "N";
		/// <summary>
		/// isSubsidy23 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy23, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy23
		{
            get
            {
                return _Issubsidy23;
            }
            set
            {
                _Issubsidy23 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy24 = "N";
		/// <summary>
		/// isSubsidy24 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy24, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy24
		{
            get
            {
                return _Issubsidy24;
            }
            set
            {
                _Issubsidy24 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy25 = "N";
		/// <summary>
		/// isSubsidy25 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy25, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy25
		{
            get
            {
                return _Issubsidy25;
            }
            set
            {
                _Issubsidy25 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy26 = "N";
		/// <summary>
		/// isSubsidy26 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy26, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy26
		{
            get
            {
                return _Issubsidy26;
            }
            set
            {
                _Issubsidy26 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy27= "N";
		/// <summary>
		/// isSubsidy27 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy27, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy27
		{
            get
            {
                return _Issubsidy27;
            }
            set
            {
                _Issubsidy27 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy28 = "N";
		/// <summary>
		/// isSubsidy28 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy28, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy28
		{
            get
            {
                return _Issubsidy28;
            }
            set
            {
                _Issubsidy28 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy29 = "N";
		/// <summary>
		/// isSubsidy29 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy29, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy29
		{
            get
            {
                return _Issubsidy29;
            }
            set
            {
                _Issubsidy29 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy30 = "N";
		/// <summary>
		/// isSubsidy30 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy30, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy30
		{
            get
            {
                return _Issubsidy30;
            }
            set
            {
                _Issubsidy30 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy31 = "N";
		/// <summary>
		/// isSubsidy31 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy31, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy31
		{
            get
            {
                return _Issubsidy31;
            }
            set
            {
                _Issubsidy31 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy32 = "N";
		/// <summary>
		/// isSubsidy32 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy32, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy32
		{
            get
            {
                return _Issubsidy32;
            }
            set
            {
                _Issubsidy32 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy33 = "N";
		/// <summary>
		/// isSubsidy33 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy33, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy33
		{
            get
            {
                return _Issubsidy33;
            }
            set
            {
                _Issubsidy33 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy34 = "N";
		/// <summary>
		/// isSubsidy34 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy34, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy34
		{
            get
            {
                return _Issubsidy34;
            }
            set
            {
                _Issubsidy34 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy35 = "N";
		/// <summary>
		/// isSubsidy35 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy35, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy35
		{
            get
            {
                return _Issubsidy35;
            }
            set
            {
                _Issubsidy35 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy36 = "N";
		/// <summary>
		/// isSubsidy36 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy36, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy36
		{
            get
            {
                return _Issubsidy36;
            }
            set
            {
                _Issubsidy36 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy37 = "N";
		/// <summary>
		/// isSubsidy37 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy37, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy37
		{
            get
            {
                return _Issubsidy37;
            }
            set
            {
                _Issubsidy37 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy38 = "N";
		/// <summary>
		/// isSubsidy38 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy38, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy38
		{
            get
            {
                return _Issubsidy38;
            }
            set
            {
                _Issubsidy38 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy39 = "N";
		/// <summary>
		/// isSubsidy39 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy39, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy39
		{
            get
            {
                return _Issubsidy39;
            }
            set
            {
                _Issubsidy39 = value == null ? "N" : value;
            }
		}

        private string _Issubsidy40 = "N";
		/// <summary>
		/// isSubsidy40 欄位屬性
		/// </summary>
		[FieldSpec(Field.Issubsidy40, false, FieldTypeEnum.Char, 1, false)]
		public string Issubsidy40
		{
            get
            {
                return _Issubsidy40;
            }
            set
            {
                _Issubsidy40 = value == null ? "N" : value;
            }
		}
		#endregion

		/// <summary>
		/// (土銀沒用，固定設為 0)
		/// </summary>
		[FieldSpec(Field.PostExtraDays, false, FieldTypeEnum.Integer, false)]
		public int PostExtraDays
		{
			get;
			set;
		}

		#region C 聯 機關長官、主辦會計、主辦出納 的職稱與姓名 (土銀沒用)
		/// <summary>
		/// C_Title1 欄位屬性
		/// </summary>
		[FieldSpec(Field.CTitle1, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CTitle1
		{
			get;
			set;
		}

		/// <summary>
		/// C_Name1 欄位屬性
		/// </summary>
		[FieldSpec(Field.CName1, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CName1
		{
			get;
			set;
		}

		/// <summary>
		/// C_Title2 欄位屬性
		/// </summary>
		[FieldSpec(Field.CTitle2, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CTitle2
		{
			get;
			set;
		}

		/// <summary>
		/// C_Name2 欄位屬性
		/// </summary>
		[FieldSpec(Field.CName2, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CName2
		{
			get;
			set;
		}

		/// <summary>
		/// C_Title3 欄位屬性
		/// </summary>
		[FieldSpec(Field.CTitle3, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CTitle3
		{
			get;
			set;
		}

		/// <summary>
		/// C_Name3 欄位屬性
		/// </summary>
		[FieldSpec(Field.CName3, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CName3
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 備註 C (土銀沒用)
		/// </summary>
		[FieldSpec(Field.ReceiveMemoc, false, FieldTypeEnum.NVarChar, 500, true)]
		public string ReceiveMemoc
		{
			get;
			set;
		}

		#region 印鑑相關 (土銀沒用)
		private string _UseStamp = "N";
		/// <summary>
		/// 是否使用印鑑旗標 (Y=是; N=否) (土銀沒用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.Usestamp, false, FieldTypeEnum.Char, 1, false)]
		public string Usestamp
		{
			get
			{
				return _UseStamp;
			}
			set
			{
				_UseStamp = value;
			}
		}

		/// <summary>
		/// A聯機關長官印鑑樣式圖檔Byte內容
		/// </summary>
		[FieldSpec(Field.AStamp1, false, FieldTypeEnum.Binary, true)]
		public byte[] AStamp1
		{
			get;
			set;
		}

		/// <summary>
		/// A_Stamp2 欄位屬性
		/// </summary>
		[FieldSpec(Field.AStamp2, false, FieldTypeEnum.Binary, true)]
		public byte[] AStamp2
		{
			get;
			set;
		}

		/// <summary>
		/// A_Stamp3 欄位屬性
		/// </summary>
		[FieldSpec(Field.AStamp3, false, FieldTypeEnum.Binary, true)]
		public byte[] AStamp3
		{
			get;
			set;
		}

		/// <summary>
		/// B_Stamp1 欄位屬性
		/// </summary>
		[FieldSpec(Field.BStamp1, false, FieldTypeEnum.Binary, true)]
		public byte[] BStamp1
		{
			get;
			set;
		}

		/// <summary>
		/// B_Stamp2 欄位屬性
		/// </summary>
		[FieldSpec(Field.BStamp2, false, FieldTypeEnum.Binary, true)]
		public byte[] BStamp2
		{
			get;
			set;
		}

		/// <summary>
		/// B_Stamp3 欄位屬性
		/// </summary>
		[FieldSpec(Field.BStamp3, false, FieldTypeEnum.Binary, true)]
		public byte[] BStamp3
		{
			get;
			set;
		}

		/// <summary>
		/// C_Stamp1 欄位屬性
		/// </summary>
		[FieldSpec(Field.CStamp1, false, FieldTypeEnum.Binary, true)]
		public byte[] CStamp1
		{
			get;
			set;
		}

		/// <summary>
		/// C_Stamp2 欄位屬性
		/// </summary>
		[FieldSpec(Field.CStamp2, false, FieldTypeEnum.Binary, true)]
		public byte[] CStamp2
		{
			get;
			set;
		}

		/// <summary>
		/// C_Stamp3 欄位屬性
		/// </summary>
		[FieldSpec(Field.CStamp3, false, FieldTypeEnum.Binary, true)]
		public byte[] CStamp3
		{
			get;
			set;
		}
		#endregion

		#region 浮水印相關 (土銀沒用)
		private string _UseWatermark = "N";
		/// <summary>
		/// 是否使用浮水印旗標 (Y=使用; 否則=不用) (土銀沒用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.Usewatermark, false, FieldTypeEnum.Char, 1, false)]
		public string Usewatermark
		{
			get
			{
				return _UseWatermark;
			}
			set
			{
				_UseWatermark = value;
			}
		}

		/// <summary>
		/// 浮水印圖檔Byte內容
		/// </summary>
		[FieldSpec(Field.Watermark, false, FieldTypeEnum.Binary, true)]
		public byte[] Watermark
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 收據模版代碼
		/// </summary>
		[FieldSpec(Field.InvoiceformId, false, FieldTypeEnum.VarChar, 32, true)]
		public string InvoiceformId
		{
			get;
			set;
		}

		private string _UsePostDueday = String.Empty;
		/// <summary>
		/// 是否使用郵局繳款期限 (Y=是; N=否) (土銀沒用，固定設為 空白)
		/// </summary>
		[FieldSpec(Field.Usepostdueday, false, FieldTypeEnum.Char, 1, false)]
		public string Usepostdueday
		{
			get
			{
				return _UsePostDueday;
			}
			set
			{
				_UsePostDueday = value;
			}
		}

		#region [MDY:202203XX] 2022擴充案 對應欄位已擴充，無須這四項對應旗標欄位 (不再使用，固定設為 空白)
		private string _LoanItemOthers = String.Empty;
		/// <summary>
		/// Loan_Item_Others (不再使用，固定設為 空白) 欄位屬性
		/// </summary>
		[FieldSpec(Field.LoanItemOthers, false, FieldTypeEnum.VarChar, 24, false)]
		public string LoanItemOthers
		{
            get
            {
                return _LoanItemOthers;
            }
            set
            {
                _LoanItemOthers = value == null ? String.Empty : value.Trim();
            }
		}

		private string _IssubsidyOthers = String.Empty;
		/// <summary>
		/// isSubsidy_Others (不再使用，固定設為 空白) 欄位屬性
		/// </summary>
		[FieldSpec(Field.IssubsidyOthers, false, FieldTypeEnum.VarChar, 24, false)]
		public string IssubsidyOthers
		{
            get
            {
                return _IssubsidyOthers;
            }
            set
            {
                _IssubsidyOthers = value == null ? String.Empty : value.Trim();
            }
		}

		private string _AgencyItemOthers = String.Empty;
		/// <summary>
		/// Agency_Item_Others (不再使用，固定設為 空白) 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyItemOthers, false, FieldTypeEnum.VarChar, 24, false)]
		public string AgencyItemOthers
		{
            get
            {
                return _AgencyItemOthers;
            }
            set
            {
                _AgencyItemOthers = value == null ? String.Empty : value.Trim();
            }
		}

		private string _AgencyCheckOthers = String.Empty;
		/// <summary>
		/// Agency_Check_Others (不再使用，固定設為 空白) 欄位屬性
		/// </summary>
		[FieldSpec(Field.AgencyCheckOthers, false, FieldTypeEnum.VarChar, 24, false)]
		public string AgencyCheckOthers
		{
            get
            {
                return _AgencyCheckOthers;
            }
            set
            {
                _AgencyCheckOthers = value == null ? String.Empty : value.Trim();
            }
		}
		#endregion

        #region 21 項備註標題 (土銀新增)
        private string _MemoTitle01 = null;
        /// <summary>
        /// 備註標題01
        /// </summary>
        [FieldSpec(Field.MemoTitle01, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle01
        {
            get
            {
                return _MemoTitle01;
            }
            set
            {
                _MemoTitle01 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle02 = null;
        /// <summary>
        /// 備註標題02
        /// </summary>
        [FieldSpec(Field.MemoTitle02, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle02
        {
            get
            {
                return _MemoTitle02;
            }
            set
            {
                _MemoTitle02 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle03 = null;
        /// <summary>
        /// 備註標題03
        /// </summary>
        [FieldSpec(Field.MemoTitle03, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle03
        {
            get
            {
                return _MemoTitle03;
            }
            set
            {
                _MemoTitle03 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle04 = null;
        /// <summary>
        /// 備註標題04
        /// </summary>
        [FieldSpec(Field.MemoTitle04, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle04
        {
            get
            {
                return _MemoTitle04;
            }
            set
            {
                _MemoTitle04 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle05 = null;
        /// <summary>
        /// 備註標題05
        /// </summary>
        [FieldSpec(Field.MemoTitle05, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle05
        {
            get
            {
                return _MemoTitle05;
            }
            set
            {
                _MemoTitle05 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle06 = null;
        /// <summary>
        /// 備註標題06
        /// </summary>
        [FieldSpec(Field.MemoTitle06, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle06
        {
            get
            {
                return _MemoTitle06;
            }
            set
            {
                _MemoTitle06 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle07 = null;
        /// <summary>
        /// 備註標題07
        /// </summary>
        [FieldSpec(Field.MemoTitle07, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle07
        {
            get
            {
                return _MemoTitle07;
            }
            set
            {
                _MemoTitle07 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle08 = null;
        /// <summary>
        /// 備註標題08
        /// </summary>
        [FieldSpec(Field.MemoTitle08, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle08
        {
            get
            {
                return _MemoTitle08;
            }
            set
            {
                _MemoTitle08 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle09 = null;
        /// <summary>
        /// 備註標題09
        /// </summary>
        [FieldSpec(Field.MemoTitle09, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle09
        {
            get
            {
                return _MemoTitle09;
            }
            set
            {
                _MemoTitle09 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle10 = null;
        /// <summary>
        /// 備註標題10
        /// </summary>
        [FieldSpec(Field.MemoTitle10, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle10
        {
            get
            {
                return _MemoTitle10;
            }
            set
            {
                _MemoTitle10 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle11 = null;
        /// <summary>
        /// <summary>
        /// 備註標題11
        /// </summary>
        [FieldSpec(Field.MemoTitle11, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle11
        {
            get
            {
                return _MemoTitle11;
            }
            set
            {
                _MemoTitle11 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle12 = null;
        /// <summary>
        /// 備註標題12
        /// </summary>
        [FieldSpec(Field.MemoTitle12, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle12
        {
            get
            {
                return _MemoTitle12;
            }
            set
            {
                _MemoTitle12 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle13 = null;
        /// <summary>
        /// 備註標題13
        /// </summary>
        [FieldSpec(Field.MemoTitle13, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle13
        {
            get
            {
                return _MemoTitle13;
            }
            set
            {
                _MemoTitle13 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle14 = null;
        /// <summary>
        /// 備註標題14
        /// </summary>
        [FieldSpec(Field.MemoTitle14, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle14
        {
            get
            {
                return _MemoTitle14;
            }
            set
            {
                _MemoTitle14 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle15 = null;
        /// <summary>
        /// 備註標題15
        /// </summary>
        [FieldSpec(Field.MemoTitle15, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle15
        {
            get
            {
                return _MemoTitle15;
            }
            set
            {
                _MemoTitle15 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle16 = null;
        /// <summary>
        /// 備註標題16
        /// </summary>
        [FieldSpec(Field.MemoTitle16, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle16
        {
            get
            {
                return _MemoTitle16;
            }
            set
            {
                _MemoTitle16 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle17 = null;
        /// <summary>
        /// 備註標題17
        /// </summary>
        [FieldSpec(Field.MemoTitle17, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle17
        {
            get
            {
                return _MemoTitle17;
            }
            set
            {
                _MemoTitle17 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle18 = null;
        /// <summary>
        /// 備註標題18
        /// </summary>
        [FieldSpec(Field.MemoTitle18, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle18
        {
            get
            {
                return _MemoTitle18;
            }
            set
            {
                _MemoTitle18 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle19 = null;
        /// <summary>
        /// 備註標題19
        /// </summary>
        [FieldSpec(Field.MemoTitle19, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle19
        {
            get
            {
                return _MemoTitle19;
            }
            set
            {
                _MemoTitle19 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle20 = null;
        /// <summary>
        /// 備註標題20
        /// </summary>
        [FieldSpec(Field.MemoTitle20, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle20
        {
            get
            {
                return _MemoTitle20;
            }
            set
            {
                _MemoTitle20 = value == null ? null : value.Trim();
            }
        }

        private string _MemoTitle21 = null;
        /// <summary>
        /// 備註標題21
        /// </summary>
        [FieldSpec(Field.MemoTitle21, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MemoTitle21
        {
            get
            {
                return _MemoTitle21;
            }
            set
            {
                _MemoTitle21 = value == null ? null : value.Trim();
            }
        }
		#endregion

		#region [MDY:2018xxxx] 新增 列印收據關閉日、備註不自動換行 欄位
		private string _InvoiceCloseDate = String.Empty;
		/// <summary>
		/// 列印收據關閉日 (格式：yyyyMMdd)
		/// </summary>
		[FieldSpec(Field.InvoiceCloseDate, false, FieldTypeEnum.Char, 8, false)]
		public string InvoiceCloseDate
		{
			get
			{
				return _InvoiceCloseDate;
			}
			set
			{
				_InvoiceCloseDate = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
			}
		}

		private string _ReceiveMemoNoWrap = "N";
		/// <summary>
		/// 備註是否動換行 (Y:是 / N:否) (預設N)
		/// </summary>
		[FieldSpec(Field.ReceiveMemoNoWrap, false, FieldTypeEnum.Char, 1, false)]
		public string ReceiveMemoNoWrap
        {
            get
            {
                return _ReceiveMemoNoWrap;
            }
            set
            {
                _ReceiveMemoNoWrap = value == null ? "N" : value;
            }
        }
		#endregion

		#region [MDY:202203XX] 2022擴充案 收入科目英文名稱 欄位
		/// <summary>
		/// 收入科目01英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE01, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE01
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目02英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE02, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE02
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目03英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE03, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE03
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目04英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE04, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE04
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目05英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE05, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE05
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目06英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE06, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE06
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目07英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE07, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE07
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目08英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE08, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE08
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目09英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE09, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE09
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目10英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE10, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE10
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目11英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE11, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE11
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目12英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE12, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE12
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目13英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE13, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE13
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目14英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE14, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE14
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目15英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE15, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE15
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目16英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE16, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE16
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目17英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE17, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE17
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目18英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE18, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE18
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目19英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE19, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE19
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目20英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE20, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE20
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目21英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE21, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE21
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目22英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE22, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE22
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目23英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE23, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE23
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目24英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE24, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE24
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目25英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE25, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE25
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目26英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE26, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE26
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目27英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE27, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE27
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目28英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE28, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE28
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目29英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE29, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE29
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目30英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE30, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE30
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目31英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE31, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE31
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目32英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE32, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE32
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目33英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE33, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE33
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目34英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE34, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE34
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目35英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE35, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE35
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目36英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE36, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE36
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目37英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE37, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE37
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目38英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE38, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE38
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目39英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE39, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE39
		{
			get;
			set;
		}

		/// <summary>
		/// 收入科目40英文名稱
		/// </summary>
		[FieldSpec(Field.ReceiveItemE40, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ReceiveItemE40
		{
			get;
			set;
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 備註英文標題 欄位
		private string _MemoTitleE01 = null;
		/// <summary>
		/// 備註01英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE01, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE01
		{
			get
			{
				return _MemoTitleE01;
			}
			set
			{
				_MemoTitleE01 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE02 = null;
		/// <summary>
		/// 備註02英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE02, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE02
		{
			get
			{
				return _MemoTitleE02;
			}
			set
			{
				_MemoTitleE02 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE03 = null;
		/// <summary>
		/// 備註03英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE03, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE03
		{
			get
			{
				return _MemoTitleE03;
			}
			set
			{
				_MemoTitleE03 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE04 = null;
		/// <summary>
		/// 備註04英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE04, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE04
		{
			get
			{
				return _MemoTitleE04;
			}
			set
			{
				_MemoTitleE04 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE05 = null;
		/// <summary>
		/// 備註05英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE05, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE05
		{
			get
			{
				return _MemoTitleE05;
			}
			set
			{
				_MemoTitleE05 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE06 = null;
		/// <summary>
		/// 備註06英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE06, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE06
		{
			get
			{
				return _MemoTitleE06;
			}
			set
			{
				_MemoTitleE06 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE07 = null;
		/// <summary>
		/// 備註07英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE07, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE07
		{
			get
			{
				return _MemoTitleE07;
			}
			set
			{
				_MemoTitleE07 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE08 = null;
		/// <summary>
		/// 備註08英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE08, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE08
		{
			get
			{
				return _MemoTitleE08;
			}
			set
			{
				_MemoTitleE08 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE09 = null;
		/// <summary>
		/// 備註09英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE09, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE09
		{
			get
			{
				return _MemoTitleE09;
			}
			set
			{
				_MemoTitleE09 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE10 = null;
		/// <summary>
		/// 備註10英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE10, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE10
		{
			get
			{
				return _MemoTitleE10;
			}
			set
			{
				_MemoTitleE10 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE11 = null;
		/// <summary>
		/// <summary>
		/// 備註11英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE11, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE11
		{
			get
			{
				return _MemoTitleE11;
			}
			set
			{
				_MemoTitleE11 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE12 = null;
		/// <summary>
		/// 備註12英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE12, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE12
		{
			get
			{
				return _MemoTitleE12;
			}
			set
			{
				_MemoTitleE12 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE13 = null;
		/// <summary>
		/// 備註13英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE13, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE13
		{
			get
			{
				return _MemoTitleE13;
			}
			set
			{
				_MemoTitleE13 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE14 = null;
		/// <summary>
		/// 備註14英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE14, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE14
		{
			get
			{
				return _MemoTitleE14;
			}
			set
			{
				_MemoTitleE14 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE15 = null;
		/// <summary>
		/// 備註15英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE15, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE15
		{
			get
			{
				return _MemoTitleE15;
			}
			set
			{
				_MemoTitleE15 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE16 = null;
		/// <summary>
		/// 備註16英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE16, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE16
		{
			get
			{
				return _MemoTitleE16;
			}
			set
			{
				_MemoTitleE16 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE17 = null;
		/// <summary>
		/// 備註17英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE17, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE17
		{
			get
			{
				return _MemoTitleE17;
			}
			set
			{
				_MemoTitleE17 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE18 = null;
		/// <summary>
		/// 備註18英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE18, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE18
		{
			get
			{
				return _MemoTitleE18;
			}
			set
			{
				_MemoTitleE18 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE19 = null;
		/// <summary>
		/// 備註19英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE19, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE19
		{
			get
			{
				return _MemoTitleE19;
			}
			set
			{
				_MemoTitleE19 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE20 = null;
		/// <summary>
		/// 備註20英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE20, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE20
		{
			get
			{
				return _MemoTitleE20;
			}
			set
			{
				_MemoTitleE20 = value == null ? null : value.Trim();
			}
		}

		private string _MemoTitleE21 = null;
		/// <summary>
		/// 備註21英文標題
		/// </summary>
		[FieldSpec(Field.MemoTitleE21, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MemoTitleE21
		{
			get
			{
				return _MemoTitleE21;
			}
			set
			{
				_MemoTitleE21 = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 注意事項英文內容 欄位
		/// <summary>
		/// 繳費單(通訊聯)注意事項1英文內容
		/// </summary>
		[FieldSpec(Field.BriefE1, false, FieldTypeEnum.NVarChar, 500, true)]
		public string BriefE1
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項2英文內容
		/// </summary>
		[FieldSpec(Field.BriefE2, false, FieldTypeEnum.NVarChar, 500, true)]
		public string BriefE2
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項3英文內容
		/// </summary>
		[FieldSpec(Field.BriefE3, false, FieldTypeEnum.NVarChar, 500, true)]
		public string BriefE3
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項4英文內容
		/// </summary>
		[FieldSpec(Field.BriefE4, false, FieldTypeEnum.NVarChar, 500, true)]
		public string BriefE4
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項5英文內容
		/// </summary>
		[FieldSpec(Field.BriefE5, false, FieldTypeEnum.NVarChar, 500, true)]
		public string BriefE5
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費單(通訊聯)注意事項6英文內容
		/// </summary>
		[FieldSpec(Field.BriefE6, false, FieldTypeEnum.NVarChar, 500, true)]
		public string BriefE6
		{
			get;
			set;
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 英文模板 欄位
		/// <summary>
		/// 繳費單英文模板代號
		/// </summary>
		[FieldSpec(Field.BillFormEId, false, FieldTypeEnum.VarChar, 32, true)]
		public string BillFormEId
		{
			get;
			set;
		}

		/// <summary>
		/// 收據英文模板代號
		/// </summary>
		[FieldSpec(Field.InvoiceFormEId, false, FieldTypeEnum.VarChar, 32, true)]
		public string InvoiceFormEId
		{
			get;
			set;
		}
        #endregion
        #endregion
        #endregion

        #region [OLD:202202XX] 2022擴充案 改寫所以 MARK
        //#region Method
        ///// <summary>
        ///// 取得所有的收入科目名稱陣列，陣列元素依序為由 ReceiveItem01 至 ReceiveItem40
        ///// </summary>
        ///// <returns>傳回所有的收入科目名稱陣列</returns>
        //public string[] GetAllReceiveItems()
        //{
        //    string[] items = new string[] {
        //        this.ReceiveItem01, this.ReceiveItem02, this.ReceiveItem03, this.ReceiveItem04, this.ReceiveItem05,
        //        this.ReceiveItem06, this.ReceiveItem07, this.ReceiveItem08, this.ReceiveItem09, this.ReceiveItem10,
        //        this.ReceiveItem11, this.ReceiveItem12, this.ReceiveItem13, this.ReceiveItem14, this.ReceiveItem15,
        //        this.ReceiveItem16, this.ReceiveItem17, this.ReceiveItem18, this.ReceiveItem19, this.ReceiveItem20,
        //        this.ReceiveItem21, this.ReceiveItem22, this.ReceiveItem23, this.ReceiveItem24, this.ReceiveItem25,
        //        this.ReceiveItem26, this.ReceiveItem27, this.ReceiveItem28, this.ReceiveItem29, this.ReceiveItem30,
        //        this.ReceiveItem31, this.ReceiveItem32, this.ReceiveItem33, this.ReceiveItem34, this.ReceiveItem35,
        //        this.ReceiveItem36, this.ReceiveItem37, this.ReceiveItem38, this.ReceiveItem39, this.ReceiveItem40
        //    };

        //    return items;
        //}

        ///// <summary>
        ///// 取得所有的收入科目是否就貸旗標陣列，陣列元素依序為由 LoanItem01 至 LoanItem16 + LoanItemOthers 的 14 碼字元字串
        ///// </summary>
        ///// <returns>傳回所有的收入科目是否就貸旗標陣列</returns>
        //public string[] GetAllLoanItems()
        //{
        //    char[] chrs = this.LoanItemOthers.ToUpper().PadRight(OtherItemCount, 'N').ToCharArray();
        //    string[] items = new string[] {
        //        this.LoanItem01, this.LoanItem02, this.LoanItem03, this.LoanItem04, this.LoanItem05,
        //        this.LoanItem06, this.LoanItem07, this.LoanItem08, this.LoanItem09, this.LoanItem10,
        //        this.LoanItem11, this.LoanItem12, this.LoanItem13, this.LoanItem14, this.LoanItem15,
        //        this.LoanItem16,
        //        (chrs[00] == 'Y' ? "Y" : "N"), (chrs[01] == 'Y' ? "Y" : "N"),
        //        (chrs[02] == 'Y' ? "Y" : "N"), (chrs[03] == 'Y' ? "Y" : "N"),
        //        (chrs[04] == 'Y' ? "Y" : "N"), (chrs[05] == 'Y' ? "Y" : "N"),
        //        (chrs[06] == 'Y' ? "Y" : "N"), (chrs[07] == 'Y' ? "Y" : "N"),
        //        (chrs[08] == 'Y' ? "Y" : "N"), (chrs[09] == 'Y' ? "Y" : "N"),
        //        (chrs[10] == 'Y' ? "Y" : "N"), (chrs[11] == 'Y' ? "Y" : "N"),
        //        (chrs[12] == 'Y' ? "Y" : "N"), (chrs[13] == 'Y' ? "Y" : "N"),
        //        (chrs[14] == 'Y' ? "Y" : "N"), (chrs[15] == 'Y' ? "Y" : "N"),
        //        (chrs[16] == 'Y' ? "Y" : "N"), (chrs[17] == 'Y' ? "Y" : "N"),
        //        (chrs[18] == 'Y' ? "Y" : "N"), (chrs[19] == 'Y' ? "Y" : "N"),
        //        (chrs[20] == 'Y' ? "Y" : "N"), (chrs[21] == 'Y' ? "Y" : "N"),
        //        (chrs[22] == 'Y' ? "Y" : "N"), (chrs[23] == 'Y' ? "Y" : "N")
        //    };
        //    return items;
        //}

        ///// <summary>
        ///// 取得所有的收入科目是否教育部補助旗標陣列，陣列元素依序為由 IsSubsidy01 至 IsSubsidy16 + IsSubsidyOthers 的 14 碼字元字串 (不足的字元傳回 N)
        ///// </summary>
        ///// <returns>傳回所有的收入科目是否教育部補助旗標陣列</returns>
        //public string[] GetAllIsSubsidys()
        //{
        //    char[] chrs = this.IssubsidyOthers.ToUpper().PadRight(OtherItemCount, 'N').ToCharArray();
        //    string[] items = new string[] {
        //        this.Issubsidy01, this.Issubsidy02, this.Issubsidy03, this.Issubsidy04, this.Issubsidy05,
        //        this.Issubsidy06, this.Issubsidy07, this.Issubsidy08, this.Issubsidy09, this.Issubsidy10,
        //        this.Issubsidy11, this.Issubsidy12, this.Issubsidy13, this.Issubsidy14, this.Issubsidy15,
        //        this.Issubsidy16,
        //        (chrs[00] == 'Y' ? "Y" : "N"), (chrs[01] == 'Y' ? "Y" : "N"),
        //        (chrs[02] == 'Y' ? "Y" : "N"), (chrs[03] == 'Y' ? "Y" : "N"),
        //        (chrs[04] == 'Y' ? "Y" : "N"), (chrs[05] == 'Y' ? "Y" : "N"),
        //        (chrs[06] == 'Y' ? "Y" : "N"), (chrs[07] == 'Y' ? "Y" : "N"),
        //        (chrs[08] == 'Y' ? "Y" : "N"), (chrs[09] == 'Y' ? "Y" : "N"),
        //        (chrs[10] == 'Y' ? "Y" : "N"), (chrs[11] == 'Y' ? "Y" : "N"),
        //        (chrs[12] == 'Y' ? "Y" : "N"), (chrs[13] == 'Y' ? "Y" : "N"),
        //        (chrs[14] == 'Y' ? "Y" : "N"), (chrs[15] == 'Y' ? "Y" : "N"),
        //        (chrs[16] == 'Y' ? "Y" : "N"), (chrs[17] == 'Y' ? "Y" : "N"),
        //        (chrs[18] == 'Y' ? "Y" : "N"), (chrs[19] == 'Y' ? "Y" : "N"),
        //        (chrs[20] == 'Y' ? "Y" : "N"), (chrs[21] == 'Y' ? "Y" : "N"),
        //        (chrs[22] == 'Y' ? "Y" : "N"), (chrs[23] == 'Y' ? "Y" : "N")
        //    };
        //    return items;
        //}

        ///// <summary>
        ///// 取得所有的收入科目是否申報學雜費旗標陣列，陣列元素依序為 EduTax 索引 0 至 39 的字元字串 (不足的字元傳回 N)
        ///// </summary>
        ///// <returns>傳回所有的收入科目是否申報學雜費旗標陣列</returns>
        //public string[] GetAllEduTaxFlags()
        //{
        //    char[] chrs = null;
        //    if (String.IsNullOrEmpty(this.EduTax))
        //    {
        //        chrs = "".PadRight(MaxItemCount, 'N').ToCharArray();
        //    }
        //    else
        //    {
        //        //為了相容性，只能去尾空白
        //        chrs = this.EduTax.TrimEnd().ToUpper().PadRight(MaxItemCount, 'N').ToCharArray();
        //    }
        //    string[] items = new string[MaxItemCount];
        //    for (int idx = 0; idx < MaxItemCount; idx++)
        //    {
        //        if (chrs[idx] == 'Y')
        //        {
        //            items[idx] = "Y";
        //        }
        //        else
        //        {
        //            items[idx] = "N";
        //        }
        //    }
        //    return items;
        //}

        ///// <summary>
        ///// 取得所有的收入科目是否申報住宿費旗標陣列，陣列元素依序為 StayTax 索引 0 至 29 的字元字串 (不足的字元傳回 N)
        ///// </summary>
        ///// <returns>傳回所有的收入科目是否申報學雜費旗標陣列</returns>
        //public string[] GetAllStayTaxFlags()
        //{
        //    char[] chrs = null;
        //    if (String.IsNullOrEmpty(this.StayTax))
        //    {
        //        chrs = "".PadRight(MaxItemCount, 'N').ToCharArray();
        //    }
        //    else
        //    {
        //        //為了相容性，只能去尾空白
        //        chrs = this.StayTax.TrimEnd().ToUpper().PadRight(MaxItemCount, 'N').ToCharArray();
        //    }
        //    string[] items = new string[MaxItemCount];
        //    for (int idx = 0; idx < MaxItemCount; idx++)
        //    {
        //        if (chrs[idx] == 'Y')
        //        {
        //            items[idx] = "Y";
        //        }
        //        else
        //        {
        //            items[idx] = "N";
        //        }
        //    }
        //    return items;
        //}
        //#endregion
        #endregion

        #region [OLD:202202XX] 2022擴充案 備註項目 改寫所以 MARK
        //#region 備註標題相關 Method
        ///// <summary>
        ///// 取得所有的備註標題陣列，陣列元素依序為由 MemoTitle01 至 MemoTitle21
        ///// </summary>
        ///// <returns>傳回所有的備註標題陣列</returns>
        //public string[] GetAllMemoTitles()
        //{
        //	string[] items = new string[MemoTitleCount] {
        //		this.MemoTitle01, this.MemoTitle02, this.MemoTitle03, this.MemoTitle04, this.MemoTitle05,
        //		this.MemoTitle06, this.MemoTitle07, this.MemoTitle08, this.MemoTitle09, this.MemoTitle10,
        //		this.MemoTitle11, this.MemoTitle12, this.MemoTitle13, this.MemoTitle14, this.MemoTitle15,
        //		this.MemoTitle16, this.MemoTitle17, this.MemoTitle18, this.MemoTitle19, this.MemoTitle20,
        //		this.MemoTitle21
        //	};

        //	return items;
        //}

        ///// <summary>
        ///// 設定指定備註項目(號碼 1 ~ 21)的備註標題
        ///// </summary>
        ///// <param name="no">指定備註項目(號碼 1 ~ 21)</param>
        ///// <param name="value">指定備註標題</param>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //public bool SetMemoTitle(int no, string value)
        //{
        //	switch (no)
        //	{
        //		case 01:
        //			this.MemoTitle01 = value;
        //			return true;
        //		case 02:
        //			this.MemoTitle02 = value;
        //			return true;
        //		case 03:
        //			this.MemoTitle03 = value;
        //			return true;
        //		case 04:
        //			this.MemoTitle04 = value;
        //			return true;
        //		case 05:
        //			this.MemoTitle05 = value;
        //			return true;
        //		case 06:
        //			this.MemoTitle06 = value;
        //			return true;
        //		case 07:
        //			this.MemoTitle07 = value;
        //			return true;
        //		case 08:
        //			this.MemoTitle08 = value;
        //			return true;
        //		case 09:
        //			this.MemoTitle09 = value;
        //			return true;
        //		case 10:
        //			this.MemoTitle10 = value;
        //			return true;
        //		case 11:
        //			this.MemoTitle11 = value;
        //			return true;
        //		case 12:
        //			this.MemoTitle12 = value;
        //			return true;
        //		case 13:
        //			this.MemoTitle13 = value;
        //			return true;
        //		case 14:
        //			this.MemoTitle14 = value;
        //			return true;
        //		case 15:
        //			this.MemoTitle15 = value;
        //			return true;
        //		case 16:
        //			this.MemoTitle16 = value;
        //			return true;
        //		case 17:
        //			this.MemoTitle17 = value;
        //			return true;
        //		case 18:
        //			this.MemoTitle18 = value;
        //			return true;
        //		case 19:
        //			this.MemoTitle19 = value;
        //			return true;
        //		case 20:
        //			this.MemoTitle20 = value;
        //			return true;
        //		case 21:
        //			this.MemoTitle21 = value;
        //			return true;
        //	}
        //	return false;
        //}
        //#endregion
        #endregion

        #region [保留] Method
        //private string[] ToCharTexts(string text, int size, char padChar)
        //{
        //    text = (text == null ? String.Empty : text).PadRight(size, padChar);
        //    string[] chars = new string[size];
        //    for (int idx = 0; idx < size; idx++)
        //    {
        //        chars[idx] = text.Substring(idx, 1);
        //    }
        //    return chars;
        //}

        ///// <summary>
        ///// 設定指定編號 (1 ~ 40) 的收入科目名稱 (ReceiveItem) 值
        ///// </summary>
        ///// <param name="no"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public bool SetReceiveItems(int no, string value)
        //{
        //    switch (no)
        //    {
        //        #region 01 ~ 10
        //        case 01:
        //            this.ReceiveItem01 = value == null ? String.Empty : value.Trim();
        //            return true;
        //        case 02:
        //            this.ReceiveItem02 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 03:
        //            this.ReceiveItem03 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 04:
        //            this.ReceiveItem04 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 05:
        //            this.ReceiveItem05 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 06:
        //            this.ReceiveItem06 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 07:
        //            this.ReceiveItem07 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 08:
        //            this.ReceiveItem08 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 09:
        //            this.ReceiveItem09 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 10:
        //            this.ReceiveItem10 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        #endregion

        //        #region 11 ~ 20
        //        case 11:
        //            this.ReceiveItem11 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 12:
        //            this.ReceiveItem12 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 13:
        //            this.ReceiveItem13 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 14:
        //            this.ReceiveItem14 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 15:
        //            this.ReceiveItem15 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 16:
        //            this.ReceiveItem16 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 17:
        //            this.ReceiveItem17 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 18:
        //            this.ReceiveItem18 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 19:
        //            this.ReceiveItem19 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 20:
        //            this.ReceiveItem20 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        #endregion

        //        #region 21 ~ 30
        //        case 21:
        //            this.ReceiveItem21 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 22:
        //            this.ReceiveItem22 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 23:
        //            this.ReceiveItem23 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 24:
        //            this.ReceiveItem24 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 25:
        //            this.ReceiveItem25 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 26:
        //            this.ReceiveItem26 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 27:
        //            this.ReceiveItem27 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 28:
        //            this.ReceiveItem28 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 29:
        //            this.ReceiveItem29 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 30:
        //            this.ReceiveItem30 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        #endregion

        //        #region 31 ~ 40
        //        case 31:
        //            this.ReceiveItem31 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 32:
        //            this.ReceiveItem32 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 33:
        //            this.ReceiveItem33 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 34:
        //            this.ReceiveItem34 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 35:
        //            this.ReceiveItem35 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 36:
        //            this.ReceiveItem36 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 37:
        //            this.ReceiveItem37 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 38:
        //            this.ReceiveItem38 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 39:
        //            this.ReceiveItem39 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        case 40:
        //            this.ReceiveItem40 = value == null ? String.Empty : value.Trim()
        //            return true;
        //        #endregion
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 設定指定編號 (1 ~ 40) 的收入科目是否就貸旗標 (LoanItem) 值
        ///// </summary>
        ///// <param name="no"></param>
        ///// <param name="value"></param>
        //public bool SetLoanItemValue(int no, bool value)
        //{
        //    string[] charTexts = this.ToCharTexts(this.LoanItemOthers, OtherItemCount, 'N');
        //    string charText = null;
        //    switch (no)
        //    {
        //        #region 01 ~ 10
        //        case 01:
        //            this.LoanItem01 = value ? "Y" : "N";
        //            return true;
        //        case 02:
        //            this.LoanItem02 = value ? "Y" : "N";
        //            return true;
        //        case 03:
        //            this.LoanItem03 = value ? "Y" : "N";
        //            return true;
        //        case 04:
        //            this.LoanItem04 = value ? "Y" : "N";
        //            return true;
        //        case 05:
        //            this.LoanItem05 = value ? "Y" : "N";
        //            return true;
        //        case 06:
        //            this.LoanItem06 = value ? "Y" : "N";
        //            return true;
        //        case 07:
        //            this.LoanItem07 = value ? "Y" : "N";
        //            return true;
        //        case 08:
        //            this.LoanItem08 = value ? "Y" : "N";
        //            return true;
        //        case 09:
        //            this.LoanItem09 = value ? "Y" : "N";
        //            return true;
        //        case 10:
        //            this.LoanItem10 = value ? "Y" : "N";
        //            return true;
        //        #endregion

        //        #region 11 ~ 20
        //        case 11:
        //            this.LoanItem11 = value ? "Y" : "N";
        //            return true;
        //        case 12:
        //            this.LoanItem12 = value ? "Y" : "N";
        //            return true;
        //        case 13:
        //            this.LoanItem13 = value ? "Y" : "N";
        //            return true;
        //        case 14:
        //            this.LoanItem14 = value ? "Y" : "N";
        //            return true;
        //        case 15:
        //            this.LoanItem15 = value ? "Y" : "N";
        //            return true;
        //        case 16:
        //            this.LoanItem16 = value ? "Y" : "N";
        //            return true;
        //        case 17:
        //            this.LoanItem17 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 18:
        //            this.LoanItem18 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 19:
        //            this.LoanItem19 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 20:
        //            this.LoanItem20 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        #endregion

        //        #region 21 ~ 30
        //        case 21:
        //            this.LoanItem21 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 22:
        //            this.LoanItem22 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 23:
        //            this.LoanItem23 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 24:
        //            this.LoanItem24 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 25:
        //            this.LoanItem25 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 26:
        //            this.LoanItem26 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 27:
        //            this.LoanItem27 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 28:
        //            this.LoanItem28 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 29:
        //            this.LoanItem29 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 30:
        //            this.LoanItem30 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        #endregion

        //        #region 31 ~ 40
        //        case 31:
        //            this.LoanItem31 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 32:
        //            this.LoanItem32 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 33:
        //            this.LoanItem33 = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 34:
        //            this.LoanItem34 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 35:
        //            this.LoanItem35 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 36:
        //            this.LoanItem36 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 37:
        //            this.LoanItem37 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 38:
        //            this.LoanItem38 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 39:
        //            this.LoanItem39 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 40:
        //            this.LoanItem40 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        #endregion
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 設定指定編號 (1 ~ 40) 的收入科目是否教育部補助旗標 (LoanItem) 值
        ///// </summary>
        ///// <param name="no"></param>
        ///// <param name="value"></param>
        //public bool SetSubsidyValue(int no, bool value)
        //{
        //    string[] charTexts = this.ToCharTexts(this.LoanItemOthers, OtherItemCount, 'N');
        //    string charText = null;
        //    switch (no)
        //    {
        //        #region 01 ~ 10
        //        case 01:
        //            this.Issubsidy01 = value ? "Y" : "N";
        //            return true;
        //        case 02:
        //            this.Issubsidy02 = value ? "Y" : "N";
        //            return true;
        //        case 03:
        //            this.Issubsidy03 = value ? "Y" : "N";
        //            return true;
        //        case 04:
        //            this.Issubsidy04 = value ? "Y" : "N";
        //            return true;
        //        case 05:
        //            this.Issubsidy05 = value ? "Y" : "N";
        //            return true;
        //        case 06:
        //            this.Issubsidy06 = value ? "Y" : "N";
        //            return true;
        //        case 07:
        //            this.Issubsidy07 = value ? "Y" : "N";
        //            return true;
        //        case 08:
        //            this.Issubsidy08 = value ? "Y" : "N";
        //            return true;
        //        case 09:
        //            this.Issubsidy09 = value ? "Y" : "N";
        //            return true;
        //        case 10:
        //            this.Issubsidy10 = value ? "Y" : "N";
        //            return true;
        //        #endregion

        //        #region 11 ~ 20
        //        case 11:
        //            this.Issubsidy11 = value ? "Y" : "N";
        //            return true;
        //        case 12:
        //            this.Issubsidy12 = value ? "Y" : "N";
        //            return true;
        //        case 13:
        //            this.Issubsidy13 = value ? "Y" : "N";
        //            return true;
        //        case 14:
        //            this.Issubsidy14 = value ? "Y" : "N";
        //            return true;
        //        case 15:
        //            this.Issubsidy15 = value ? "Y" : "N";
        //            return true;
        //        case 16:
        //            this.Issubsidy16 = value ? "Y" : "N";
        //            return true;
        //        case 17:
        //            this.Issubsidy17 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 18:
        //            this.Issubsidy18 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 19:
        //            this.Issubsidy19 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 20:
        //            this.Issubsidy20 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        #endregion

        //        #region 21 ~ 30
        //        case 21:
        //            this.Issubsidy21 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 22:
        //            this.Issubsidy22 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 23:
        //            this.Issubsidy23 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 24:
        //            this.Issubsidy24 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 25:
        //            this.Issubsidy25 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 26:
        //            this.Issubsidy26 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 27:
        //            this.Issubsidy27 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 28:
        //            this.Issubsidy28 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 29:
        //            this.Issubsidy29 = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 30:
        //            this.Issubsidy30 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        #endregion

        //        #region 31 ~ 40
        //        case 31:
        //            this.Issubsidy31 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 32:
        //            this.Issubsidy32 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 33:
        //            this.Issubsidy33 = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 34:
        //            this.Issubsidy34 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 35:
        //            this.Issubsidy35 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 36:
        //            this.Issubsidy36 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 37:
        //            this.Issubsidy37 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 38:
        //            this.Issubsidy38 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 39:
        //            this.Issubsidy39 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        case 40:
        //            this.Issubsidy40 = charText = value ? "Y" : "N";
        //            charTexts[no - 17] = charText;
        //            this.LoanItemOthers = String.Join("", charTexts);
        //            return true;
        //        #endregion
        //    }
        //    return false;
        //}
        #endregion

        #region [MDY:202203XX] 2022擴充案 收入科目是否就貸旗標相關 Method
        /// <summary>
        /// 取得所有的收入科目是否就貸旗標陣列，陣列元素依序為由 LoanItem01 ~ LoanItem40
        /// </summary>
        /// <returns>傳回所有的收入科目是否就貸旗標陣列</returns>
        public string[] GetAllLoanItems()
		{
			return new string[] {
				this.LoanItem01, this.LoanItem02, this.LoanItem03, this.LoanItem04, this.LoanItem05,
				this.LoanItem06, this.LoanItem07, this.LoanItem08, this.LoanItem09, this.LoanItem10,
				this.LoanItem11, this.LoanItem12, this.LoanItem13, this.LoanItem14, this.LoanItem15,
				this.LoanItem16, this.LoanItem17, this.LoanItem18, this.LoanItem19, this.LoanItem20,
				this.LoanItem21, this.LoanItem22, this.LoanItem23, this.LoanItem24, this.LoanItem25,
				this.LoanItem26, this.LoanItem27, this.LoanItem28, this.LoanItem29, this.LoanItem30,
				this.LoanItem31, this.LoanItem32, this.LoanItem33, this.LoanItem34, this.LoanItem35,
				this.LoanItem36, this.LoanItem37, this.LoanItem38, this.LoanItem39, this.LoanItem40
			};
		}

		/// <summary>
		/// 取得指定編號的收入科目是否就貸旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <returns>找到則傳回收入科目是否就貸旗標，否則傳回 null</returns>
		public string GetLoanItemByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.LoanItem01;
				case 02:
					return this.LoanItem02;
				case 03:
					return this.LoanItem03;
				case 04:
					return this.LoanItem04;
				case 05:
					return this.LoanItem05;
				case 06:
					return this.LoanItem06;
				case 07:
					return this.LoanItem07;
				case 08:
					return this.LoanItem08;
				case 09:
					return this.LoanItem09;
				case 10:
					return this.LoanItem10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.LoanItem11;
				case 12:
					return this.LoanItem12;
				case 13:
					return this.LoanItem13;
				case 14:
					return this.LoanItem14;
				case 15:
					return this.LoanItem15;
				case 16:
					return this.LoanItem16;
				case 17:
					return this.LoanItem17;
				case 18:
					return this.LoanItem18;
				case 19:
					return this.LoanItem19;
				case 20:
					return this.LoanItem20;
				#endregion

				#region 21 ~ 30
				case 21:
					return this.LoanItem21;
				case 22:
					return this.LoanItem22;
				case 23:
					return this.LoanItem23;
				case 24:
					return this.LoanItem24;
				case 25:
					return this.LoanItem25;
				case 26:
					return this.LoanItem26;
				case 27:
					return this.LoanItem27;
				case 28:
					return this.LoanItem28;
				case 29:
					return this.LoanItem29;
				case 30:
					return this.LoanItem30;
				#endregion

				#region 31 ~ 40
				case 31:
					return this.LoanItem31;
				case 32:
					return this.LoanItem32;
				case 33:
					return this.LoanItem33;
				case 34:
					return this.LoanItem34;
				case 35:
					return this.LoanItem35;
				case 36:
					return this.LoanItem36;
				case 37:
					return this.LoanItem37;
				case 38:
					return this.LoanItem38;
				case 39:
					return this.LoanItem39;
				case 40:
					return this.LoanItem40;
				#endregion

				default:
					return null;
			}
		}

		/// <summary>
		/// 設定指定編號的收入科目是否就貸旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="value">指定收入科目是否就貸旗標</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetLoanItemByNo(int no, string value)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.LoanItem01 = value;
					return true;
				case 02:
					this.LoanItem02 = value;
					return true;
				case 03:
					this.LoanItem03 = value;
					return true;
				case 04:
					this.LoanItem04 = value;
					return true;
				case 05:
					this.LoanItem05 = value;
					return true;
				case 06:
					this.LoanItem06 = value;
					return true;
				case 07:
					this.LoanItem07 = value;
					return true;
				case 08:
					this.LoanItem08 = value;
					return true;
				case 09:
					this.LoanItem09 = value;
					return true;
				case 10:
					this.LoanItem10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.LoanItem11 = value;
					return true;
				case 12:
					this.LoanItem12 = value;
					return true;
				case 13:
					this.LoanItem13 = value;
					return true;
				case 14:
					this.LoanItem14 = value;
					return true;
				case 15:
					this.LoanItem15 = value;
					return true;
				case 16:
					this.LoanItem16 = value;
					return true;
				case 17:
					this.LoanItem17 = value;
					return true;
				case 18:
					this.LoanItem18 = value;
					return true;
				case 19:
					this.LoanItem19 = value;
					return true;
				case 20:
					this.LoanItem20 = value;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					this.LoanItem21 = value;
					return true;
				case 22:
					this.LoanItem22 = value;
					return true;
				case 23:
					this.LoanItem23 = value;
					return true;
				case 24:
					this.LoanItem24 = value;
					return true;
				case 25:
					this.LoanItem25 = value;
					return true;
				case 26:
					this.LoanItem26 = value;
					return true;
				case 27:
					this.LoanItem27 = value;
					return true;
				case 28:
					this.LoanItem28 = value;
					return true;
				case 29:
					this.LoanItem29 = value;
					return true;
				case 30:
					this.LoanItem30 = value;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					this.LoanItem31 = value;
					return true;
				case 32:
					this.LoanItem32 = value;
					return true;
				case 33:
					this.LoanItem33 = value;
					return true;
				case 34:
					this.LoanItem34 = value;
					return true;
				case 35:
					this.LoanItem35 = value;
					return true;
				case 36:
					this.LoanItem36 = value;
					return true;
				case 37:
					this.LoanItem37 = value;
					return true;
				case 38:
					this.LoanItem38 = value;
					return true;
				case 39:
					this.LoanItem39 = value;
					return true;
				case 40:
					this.LoanItem40 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 收入科目是否教育部補助旗標相關 Method
		/// <summary>
		/// 取得所有的收入科目是否教育部補助旗標陣列，陣列元素依序為由 IsSubsidy01 ~ IsSubsidy40
		/// </summary>
		/// <returns>傳回所有的收入科目是否教育部補助旗標陣列</returns>
		public string[] GetAllIsSubsidys()
		{
			return new string[] {
				this.Issubsidy01, this.Issubsidy02, this.Issubsidy03, this.Issubsidy04, this.Issubsidy05,
				this.Issubsidy06, this.Issubsidy07, this.Issubsidy08, this.Issubsidy09, this.Issubsidy10,
				this.Issubsidy11, this.Issubsidy12, this.Issubsidy13, this.Issubsidy14, this.Issubsidy15,
				this.Issubsidy16, this.Issubsidy17, this.Issubsidy18, this.Issubsidy19, this.Issubsidy20,
				this.Issubsidy21, this.Issubsidy22, this.Issubsidy23, this.Issubsidy24, this.Issubsidy25,
				this.Issubsidy26, this.Issubsidy27, this.Issubsidy28, this.Issubsidy29, this.Issubsidy30,
				this.Issubsidy31, this.Issubsidy32, this.Issubsidy33, this.Issubsidy34, this.Issubsidy35,
				this.Issubsidy36, this.Issubsidy37, this.Issubsidy38, this.Issubsidy39, this.Issubsidy40
			};
		}

		/// <summary>
		/// 取得指定編號的收入科目是否教育部補助旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <returns>找到則傳回收入科目是否教育部補助旗標，否則傳回 null</returns>
		public string GetIsSubsidyByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.Issubsidy01;
				case 02:
					return this.Issubsidy02;
				case 03:
					return this.Issubsidy03;
				case 04:
					return this.Issubsidy04;
				case 05:
					return this.Issubsidy05;
				case 06:
					return this.Issubsidy06;
				case 07:
					return this.Issubsidy07;
				case 08:
					return this.Issubsidy08;
				case 09:
					return this.Issubsidy09;
				case 10:
					return this.Issubsidy10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.Issubsidy11;
				case 12:
					return this.Issubsidy12;
				case 13:
					return this.Issubsidy13;
				case 14:
					return this.Issubsidy14;
				case 15:
					return this.Issubsidy15;
				case 16:
					return this.Issubsidy16;
				case 17:
					return this.Issubsidy17;
				case 18:
					return this.Issubsidy18;
				case 19:
					return this.Issubsidy19;
				case 20:
					return this.Issubsidy20;
				#endregion

				#region 21 ~ 30
				case 21:
					return this.Issubsidy21;
				case 22:
					return this.Issubsidy22;
				case 23:
					return this.Issubsidy23;
				case 24:
					return this.Issubsidy24;
				case 25:
					return this.Issubsidy25;
				case 26:
					return this.Issubsidy26;
				case 27:
					return this.Issubsidy27;
				case 28:
					return this.Issubsidy28;
				case 29:
					return this.Issubsidy29;
				case 30:
					return this.Issubsidy30;
				#endregion

				#region 31 ~ 40
				case 31:
					return this.Issubsidy31;
				case 32:
					return this.Issubsidy32;
				case 33:
					return this.Issubsidy33;
				case 34:
					return this.Issubsidy34;
				case 35:
					return this.Issubsidy35;
				case 36:
					return this.Issubsidy36;
				case 37:
					return this.Issubsidy37;
				case 38:
					return this.Issubsidy38;
				case 39:
					return this.Issubsidy39;
				case 40:
					return this.Issubsidy40;
				#endregion

				default:
					return null;
			}
		}

		/// <summary>
		/// 設定指定編號的收入科目是否教育部補助旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="value">指定收入科目是否教育部補助旗標</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetIsSubsidyByNo(int no, string value)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.Issubsidy01 = value;
					return true;
				case 02:
					this.Issubsidy02 = value;
					return true;
				case 03:
					this.Issubsidy03 = value;
					return true;
				case 04:
					this.Issubsidy04 = value;
					return true;
				case 05:
					this.Issubsidy05 = value;
					return true;
				case 06:
					this.Issubsidy06 = value;
					return true;
				case 07:
					this.Issubsidy07 = value;
					return true;
				case 08:
					this.Issubsidy08 = value;
					return true;
				case 09:
					this.Issubsidy09 = value;
					return true;
				case 10:
					this.Issubsidy10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.Issubsidy11 = value;
					return true;
				case 12:
					this.Issubsidy12 = value;
					return true;
				case 13:
					this.Issubsidy13 = value;
					return true;
				case 14:
					this.Issubsidy14 = value;
					return true;
				case 15:
					this.Issubsidy15 = value;
					return true;
				case 16:
					this.Issubsidy16 = value;
					return true;
				case 17:
					this.Issubsidy17 = value;
					return true;
				case 18:
					this.Issubsidy18 = value;
					return true;
				case 19:
					this.Issubsidy19 = value;
					return true;
				case 20:
					this.Issubsidy20 = value;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					this.Issubsidy21 = value;
					return true;
				case 22:
					this.Issubsidy22 = value;
					return true;
				case 23:
					this.Issubsidy23 = value;
					return true;
				case 24:
					this.Issubsidy24 = value;
					return true;
				case 25:
					this.Issubsidy25 = value;
					return true;
				case 26:
					this.Issubsidy26 = value;
					return true;
				case 27:
					this.Issubsidy27 = value;
					return true;
				case 28:
					this.Issubsidy28 = value;
					return true;
				case 29:
					this.Issubsidy29 = value;
					return true;
				case 30:
					this.Issubsidy30 = value;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					this.Issubsidy31 = value;
					return true;
				case 32:
					this.Issubsidy32 = value;
					return true;
				case 33:
					this.Issubsidy33 = value;
					return true;
				case 34:
					this.Issubsidy34 = value;
					return true;
				case 35:
					this.Issubsidy35 = value;
					return true;
				case 36:
					this.Issubsidy36 = value;
					return true;
				case 37:
					this.Issubsidy37 = value;
					return true;
				case 38:
					this.Issubsidy38 = value;
					return true;
				case 39:
					this.Issubsidy39 = value;
					return true;
				case 40:
					this.Issubsidy40 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 收入科目是否申報學雜費旗標 Method
		/// <summary>
		/// 取得所有的收入科目是否申報學雜費旗標陣列，陣列元素依序為 EduTax 索引 0 至 39 的字元字串 (不足的字元傳回 N)
		/// </summary>
		/// <returns>傳回所有的收入科目是否申報學雜費旗標陣列</returns>
		public string[] GetAllEduTaxFlags()
		{
			string[] flags = _EduTaxFlags.Select(c => c.ToString()).ToArray();
			for (int idx = 0; idx < flags.Length; idx++)
			{
				if (!"Y".Equals(flags[idx]) && !"N".Equals(flags[idx]))
				{
					flags[idx] = "N";
				}
			}
			return flags;
		}

		/// <summary>
		/// 取得指定編號的收入科目是否申報學雜費旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <returns>找到則傳回收入科目是否申報學雜費旗標，否則傳回 null</returns>
		public string GetEduTaxFlagByNo(int no)
		{
			if (no >= 1 && no <= _EduTaxFlags.Length)
			{
				string value = _EduTaxFlags[no - 1].ToString();
				return value;
			}
			return null;
		}

		/// <summary>
		/// 設定指定編號的收入科目是否申報學雜費旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="value">指定收入科目是否申報學雜費旗標</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetEduTaxFlagByNo(int no, string value)
		{
			if (no >= 1 && no <= _EduTaxFlags.Length)
			{
				_EduTax = null;
				if ("Y".Equals(value) || "N".Equals(value))
				{
					_EduTaxFlags[no - 1] = value[0];
				}
				else
				{
					_EduTaxFlags[no - 1] = 'N';
				}
				return true;
			}
			return false;
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 收入科目是否申報住宿費旗標 Method
		/// <summary>
		/// 取得所有的收入科目是否申報住宿費旗標陣列，陣列元素依序為 StayTax 索引 0 至 39 的字元字串 (不足的字元傳回 N)
		/// </summary>
		/// <returns>傳回所有的收入科目是否申報學雜費旗標陣列</returns>
		public string[] GetAllStayTaxFlags()
		{
			string[] flags = _StayTaxFlags.Select(c => c.ToString()).ToArray();
			for (int idx = 0; idx < flags.Length; idx++)
			{
				if (!"Y".Equals(flags[idx]) && !"N".Equals(flags[idx]))
				{
					flags[idx] = "N";
				}
			}
			return flags;
		}

		/// <summary>
		/// 取得指定編號的收入科目是否申報住宿費旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <returns>找到則傳回收入科目是否申報住宿費旗標，否則傳回 null</returns>
		public string GetStayTaxFlagByNo(int no)
		{
			if (no >= 1 && no <= _StayTaxFlags.Length)
			{
				string value = _StayTaxFlags[no - 1].ToString();
				return value;
			}
			return null;
		}

		/// <summary>
		/// 設定指定編號的收入科目是否申報住宿費旗標
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="value">指定收入科目是否申報學雜費旗標</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetStayTaxFlagByNo(int no, string value)
		{
			if (no >= 1 && no <= _StayTaxFlags.Length)
			{
				_StayTax = null;
				if ("Y".Equals(value) || "N".Equals(value))
				{
					_StayTaxFlags[no - 1] = value[0];
				}
				else
				{
					_StayTaxFlags[no - 1] = 'N';
				}
				return true;
			}
			return false;
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 收入科目相關 Method
		/// <summary>
		/// 取得所有的收入科目中文名稱陣列，陣列元素依序為由 ReceiveItem01 至 ReceiveItem40
		/// </summary>
		/// <returns>傳回所有的收入科目中文名稱陣列</returns>
		public string[] GetAllReceiveItemChts()
		{
			return new string[] {
				this.ReceiveItem01, this.ReceiveItem02, this.ReceiveItem03, this.ReceiveItem04, this.ReceiveItem05,
				this.ReceiveItem06, this.ReceiveItem07, this.ReceiveItem08, this.ReceiveItem09, this.ReceiveItem10,
				this.ReceiveItem11, this.ReceiveItem12, this.ReceiveItem13, this.ReceiveItem14, this.ReceiveItem15,
				this.ReceiveItem16, this.ReceiveItem17, this.ReceiveItem18, this.ReceiveItem19, this.ReceiveItem20,
				this.ReceiveItem21, this.ReceiveItem22, this.ReceiveItem23, this.ReceiveItem24, this.ReceiveItem25,
				this.ReceiveItem26, this.ReceiveItem27, this.ReceiveItem28, this.ReceiveItem29, this.ReceiveItem30,
				this.ReceiveItem31, this.ReceiveItem32, this.ReceiveItem33, this.ReceiveItem34, this.ReceiveItem35,
				this.ReceiveItem36, this.ReceiveItem37, this.ReceiveItem38, this.ReceiveItem39, this.ReceiveItem40
			};
		}

		/// <summary>
		/// 取得所有的收入科目英文名稱陣列，陣列元素依序為由 ReceiveItemE01 至 ReceiveItemE40
		/// </summary>
		/// <returns>傳回所有的收入科目英文名稱陣列</returns>
		public string[] GetAllReceiveItemEngs()
		{
			return new string[] {
				this.ReceiveItemE01, this.ReceiveItemE02, this.ReceiveItemE03, this.ReceiveItemE04, this.ReceiveItemE05,
				this.ReceiveItemE06, this.ReceiveItemE07, this.ReceiveItemE08, this.ReceiveItemE09, this.ReceiveItemE10,
				this.ReceiveItemE11, this.ReceiveItemE12, this.ReceiveItemE13, this.ReceiveItemE14, this.ReceiveItemE15,
				this.ReceiveItemE16, this.ReceiveItemE17, this.ReceiveItemE18, this.ReceiveItemE19, this.ReceiveItemE20,
				this.ReceiveItemE21, this.ReceiveItemE22, this.ReceiveItemE23, this.ReceiveItemE24, this.ReceiveItemE25,
				this.ReceiveItemE26, this.ReceiveItemE27, this.ReceiveItemE28, this.ReceiveItemE29, this.ReceiveItemE30,
				this.ReceiveItemE31, this.ReceiveItemE32, this.ReceiveItemE33, this.ReceiveItemE34, this.ReceiveItemE35,
				this.ReceiveItemE36, this.ReceiveItemE37, this.ReceiveItemE38, this.ReceiveItemE39, this.ReceiveItemE40
			};
		}

		/// <summary>
		/// 取得指定編號的收入科目中文名稱
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <returns>找到則傳回收入科目中文名稱，否則傳回 null</returns>
		public string GetReceiveItemChtByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.ReceiveItem01;
				case 02:
					return this.ReceiveItem02;
				case 03:
					return this.ReceiveItem03;
				case 04:
					return this.ReceiveItem04;
				case 05:
					return this.ReceiveItem05;
				case 06:
					return this.ReceiveItem06;
				case 07:
					return this.ReceiveItem07;
				case 08:
					return this.ReceiveItem08;
				case 09:
					return this.ReceiveItem09;
				case 10:
					return this.ReceiveItem10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.ReceiveItem11;
				case 12:
					return this.ReceiveItem12;
				case 13:
					return this.ReceiveItem13;
				case 14:
					return this.ReceiveItem14;
				case 15:
					return this.ReceiveItem15;
				case 16:
					return this.ReceiveItem16;
				case 17:
					return this.ReceiveItem17;
				case 18:
					return this.ReceiveItem18;
				case 19:
					return this.ReceiveItem19;
				case 20:
					return this.ReceiveItem20;
				#endregion

				#region 21 ~ 30
				case 21:
					return this.ReceiveItem21;
				case 22:
					return this.ReceiveItem22;
				case 23:
					return this.ReceiveItem23;
				case 24:
					return this.ReceiveItem24;
				case 25:
					return this.ReceiveItem25;
				case 26:
					return this.ReceiveItem26;
				case 27:
					return this.ReceiveItem27;
				case 28:
					return this.ReceiveItem28;
				case 29:
					return this.ReceiveItem29;
				case 30:
					return this.ReceiveItem30;
				#endregion

				#region 31 ~ 40
				case 31:
					return this.ReceiveItem31;
				case 32:
					return this.ReceiveItem32;
				case 33:
					return this.ReceiveItem33;
				case 34:
					return this.ReceiveItem34;
				case 35:
					return this.ReceiveItem35;
				case 36:
					return this.ReceiveItem36;
				case 37:
					return this.ReceiveItem37;
				case 38:
					return this.ReceiveItem38;
				case 39:
					return this.ReceiveItem39;
				case 40:
					return this.ReceiveItem40;
				#endregion

				default:
					return null;
			}
		}

		/// <summary>
		/// 取得指定編號的收入科目英文名稱
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <returns>找到則傳回收入科目英文名稱，否則傳回 null</returns>
		public string GetReceiveItemEngByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.ReceiveItemE01;
				case 02:
					return this.ReceiveItemE02;
				case 03:
					return this.ReceiveItemE03;
				case 04:
					return this.ReceiveItemE04;
				case 05:
					return this.ReceiveItemE05;
				case 06:
					return this.ReceiveItemE06;
				case 07:
					return this.ReceiveItemE07;
				case 08:
					return this.ReceiveItemE08;
				case 09:
					return this.ReceiveItemE09;
				case 10:
					return this.ReceiveItemE10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.ReceiveItemE11;
				case 12:
					return this.ReceiveItemE12;
				case 13:
					return this.ReceiveItemE13;
				case 14:
					return this.ReceiveItemE14;
				case 15:
					return this.ReceiveItemE15;
				case 16:
					return this.ReceiveItemE16;
				case 17:
					return this.ReceiveItemE17;
				case 18:
					return this.ReceiveItemE18;
				case 19:
					return this.ReceiveItemE19;
				case 20:
					return this.ReceiveItemE20;
				#endregion

				#region 21 ~ 30
				case 21:
					return this.ReceiveItemE21;
				case 22:
					return this.ReceiveItemE22;
				case 23:
					return this.ReceiveItemE23;
				case 24:
					return this.ReceiveItemE24;
				case 25:
					return this.ReceiveItemE25;
				case 26:
					return this.ReceiveItemE26;
				case 27:
					return this.ReceiveItemE27;
				case 28:
					return this.ReceiveItemE28;
				case 29:
					return this.ReceiveItemE29;
				case 30:
					return this.ReceiveItemE30;
				#endregion

				#region 31 ~ 40
				case 31:
					return this.ReceiveItemE31;
				case 32:
					return this.ReceiveItemE32;
				case 33:
					return this.ReceiveItemE33;
				case 34:
					return this.ReceiveItemE34;
				case 35:
					return this.ReceiveItemE35;
				case 36:
					return this.ReceiveItemE36;
				case 37:
					return this.ReceiveItemE37;
				case 38:
					return this.ReceiveItemE38;
				case 39:
					return this.ReceiveItemE39;
				case 40:
					return this.ReceiveItemE40;
				#endregion

				default:
					return null;
			}
		}

		/// <summary>
		/// 取得指定編號的收入科目中文與英文名稱
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="chtValue">找到則傳回收入科目中文名稱，否則傳回 null</param>
		/// <param name="engValue">找到則傳回收入科目英文名稱，否則傳回 null</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool GetReceiveItemByNo(int no, out string chtValue, out string engValue)
		{
			chtValue = null;
			engValue = null;
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					chtValue = this.ReceiveItem01;
					engValue = this.ReceiveItemE01;
					return true;
				case 02:
					chtValue = this.ReceiveItem02;
					engValue = this.ReceiveItemE02;
					return true;
				case 03:
					chtValue = this.ReceiveItem03;
					engValue = this.ReceiveItemE03;
					return true;
				case 04:
					chtValue = this.ReceiveItem04;
					engValue = this.ReceiveItemE04;
					return true;
				case 05:
					chtValue = this.ReceiveItem05;
					engValue = this.ReceiveItemE05;
					return true;
				case 06:
					chtValue = this.ReceiveItem06;
					engValue = this.ReceiveItemE06;
					return true;
				case 07:
					chtValue = this.ReceiveItem07;
					engValue = this.ReceiveItemE07;
					return true;
				case 08:
					chtValue = this.ReceiveItem08;
					engValue = this.ReceiveItemE08;
					return true;
				case 09:
					chtValue = this.ReceiveItem09;
					engValue = this.ReceiveItemE09;
					return true;
				case 10:
					chtValue = this.ReceiveItem10;
					engValue = this.ReceiveItemE10;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					chtValue = this.ReceiveItem11;
					engValue = this.ReceiveItemE11;
					return true;
				case 12:
					chtValue = this.ReceiveItem12;
					engValue = this.ReceiveItemE12;
					return true;
				case 13:
					chtValue = this.ReceiveItem13;
					engValue = this.ReceiveItemE13;
					return true;
				case 14:
					chtValue = this.ReceiveItem14;
					engValue = this.ReceiveItemE14;
					return true;
				case 15:
					chtValue = this.ReceiveItem15;
					engValue = this.ReceiveItemE15;
					return true;
				case 16:
					chtValue = this.ReceiveItem16;
					engValue = this.ReceiveItemE16;
					return true;
				case 17:
					chtValue = this.ReceiveItem17;
					engValue = this.ReceiveItemE17;
					return true;
				case 18:
					chtValue = this.ReceiveItem18;
					engValue = this.ReceiveItemE18;
					return true;
				case 19:
					chtValue = this.ReceiveItem19;
					engValue = this.ReceiveItemE19;
					return true;
				case 20:
					chtValue = this.ReceiveItem20;
					engValue = this.ReceiveItemE20;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					chtValue = this.ReceiveItem21;
					engValue = this.ReceiveItemE21;
					return true;
				case 22:
					chtValue = this.ReceiveItem22;
					engValue = this.ReceiveItemE22;
					return true;
				case 23:
					chtValue = this.ReceiveItem23;
					engValue = this.ReceiveItemE23;
					return true;
				case 24:
					chtValue = this.ReceiveItem24;
					engValue = this.ReceiveItemE24;
					return true;
				case 25:
					chtValue = this.ReceiveItem25;
					engValue = this.ReceiveItemE25;
					return true;
				case 26:
					chtValue = this.ReceiveItem26;
					engValue = this.ReceiveItemE26;
					return true;
				case 27:
					chtValue = this.ReceiveItem27;
					engValue = this.ReceiveItemE27;
					return true;
				case 28:
					chtValue = this.ReceiveItem28;
					engValue = this.ReceiveItemE28;
					return true;
				case 29:
					chtValue = this.ReceiveItem29;
					engValue = this.ReceiveItemE29;
					return true;
				case 30:
					chtValue = this.ReceiveItem30;
					engValue = this.ReceiveItemE30;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					chtValue = this.ReceiveItem31;
					engValue = this.ReceiveItemE31;
					return true;
				case 32:
					chtValue = this.ReceiveItem32;
					engValue = this.ReceiveItemE32;
					return true;
				case 33:
					chtValue = this.ReceiveItem33;
					engValue = this.ReceiveItemE33;
					return true;
				case 34:
					chtValue = this.ReceiveItem34;
					engValue = this.ReceiveItemE34;
					return true;
				case 35:
					chtValue = this.ReceiveItem35;
					engValue = this.ReceiveItemE35;
					return true;
				case 36:
					chtValue = this.ReceiveItem36;
					engValue = this.ReceiveItemE36;
					return true;
				case 37:
					chtValue = this.ReceiveItem37;
					engValue = this.ReceiveItemE37;
					return true;
				case 38:
					chtValue = this.ReceiveItem38;
					engValue = this.ReceiveItemE38;
					return true;
				case 39:
					chtValue = this.ReceiveItem39;
					engValue = this.ReceiveItemE39;
					return true;
				case 40:
					chtValue = this.ReceiveItem40;
					engValue = this.ReceiveItemE40;
					return true;
				#endregion

				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的收入科目中文名稱
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="value">指定收入科目中文名稱</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetReceiveItemChtByNo(int no, string value)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.ReceiveItem01 = value;
					return true;
				case 02:
					this.ReceiveItem02 = value;
					return true;
				case 03:
					this.ReceiveItem03 = value;
					return true;
				case 04:
					this.ReceiveItem04 = value;
					return true;
				case 05:
					this.ReceiveItem05 = value;
					return true;
				case 06:
					this.ReceiveItem06 = value;
					return true;
				case 07:
					this.ReceiveItem07 = value;
					return true;
				case 08:
					this.ReceiveItem08 = value;
					return true;
				case 09:
					this.ReceiveItem09 = value;
					return true;
				case 10:
					this.ReceiveItem10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.ReceiveItem11 = value;
					return true;
				case 12:
					this.ReceiveItem12 = value;
					return true;
				case 13:
					this.ReceiveItem13 = value;
					return true;
				case 14:
					this.ReceiveItem14 = value;
					return true;
				case 15:
					this.ReceiveItem15 = value;
					return true;
				case 16:
					this.ReceiveItem16 = value;
					return true;
				case 17:
					this.ReceiveItem17 = value;
					return true;
				case 18:
					this.ReceiveItem18 = value;
					return true;
				case 19:
					this.ReceiveItem19 = value;
					return true;
				case 20:
					this.ReceiveItem20 = value;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					this.ReceiveItem21 = value;
					return true;
				case 22:
					this.ReceiveItem22 = value;
					return true;
				case 23:
					this.ReceiveItem23 = value;
					return true;
				case 24:
					this.ReceiveItem24 = value;
					return true;
				case 25:
					this.ReceiveItem25 = value;
					return true;
				case 26:
					this.ReceiveItem26 = value;
					return true;
				case 27:
					this.ReceiveItem27 = value;
					return true;
				case 28:
					this.ReceiveItem28 = value;
					return true;
				case 29:
					this.ReceiveItem29 = value;
					return true;
				case 30:
					this.ReceiveItem30 = value;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					this.ReceiveItem31 = value;
					return true;
				case 32:
					this.ReceiveItem32 = value;
					return true;
				case 33:
					this.ReceiveItem33 = value;
					return true;
				case 34:
					this.ReceiveItem34 = value;
					return true;
				case 35:
					this.ReceiveItem35 = value;
					return true;
				case 36:
					this.ReceiveItem36 = value;
					return true;
				case 37:
					this.ReceiveItem37 = value;
					return true;
				case 38:
					this.ReceiveItem38 = value;
					return true;
				case 39:
					this.ReceiveItem39 = value;
					return true;
				case 40:
					this.ReceiveItem40 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的收入科目英文名稱
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="value">指定收入科目英文名稱</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetReceiveItemEngByNo(int no, string value)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.ReceiveItemE01 = value;
					return true;
				case 02:
					this.ReceiveItemE02 = value;
					return true;
				case 03:
					this.ReceiveItemE03 = value;
					return true;
				case 04:
					this.ReceiveItemE04 = value;
					return true;
				case 05:
					this.ReceiveItemE05 = value;
					return true;
				case 06:
					this.ReceiveItemE06 = value;
					return true;
				case 07:
					this.ReceiveItemE07 = value;
					return true;
				case 08:
					this.ReceiveItemE08 = value;
					return true;
				case 09:
					this.ReceiveItemE09 = value;
					return true;
				case 10:
					this.ReceiveItemE10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.ReceiveItemE11 = value;
					return true;
				case 12:
					this.ReceiveItemE12 = value;
					return true;
				case 13:
					this.ReceiveItemE13 = value;
					return true;
				case 14:
					this.ReceiveItemE14 = value;
					return true;
				case 15:
					this.ReceiveItemE15 = value;
					return true;
				case 16:
					this.ReceiveItemE16 = value;
					return true;
				case 17:
					this.ReceiveItemE17 = value;
					return true;
				case 18:
					this.ReceiveItemE18 = value;
					return true;
				case 19:
					this.ReceiveItemE19 = value;
					return true;
				case 20:
					this.ReceiveItemE20 = value;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					this.ReceiveItemE21 = value;
					return true;
				case 22:
					this.ReceiveItemE22 = value;
					return true;
				case 23:
					this.ReceiveItemE23 = value;
					return true;
				case 24:
					this.ReceiveItemE24 = value;
					return true;
				case 25:
					this.ReceiveItemE25 = value;
					return true;
				case 26:
					this.ReceiveItemE26 = value;
					return true;
				case 27:
					this.ReceiveItemE27 = value;
					return true;
				case 28:
					this.ReceiveItemE28 = value;
					return true;
				case 29:
					this.ReceiveItemE29 = value;
					return true;
				case 30:
					this.ReceiveItemE30 = value;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					this.ReceiveItemE31 = value;
					return true;
				case 32:
					this.ReceiveItemE32 = value;
					return true;
				case 33:
					this.ReceiveItemE33 = value;
					return true;
				case 34:
					this.ReceiveItemE34 = value;
					return true;
				case 35:
					this.ReceiveItemE35 = value;
					return true;
				case 36:
					this.ReceiveItemE36 = value;
					return true;
				case 37:
					this.ReceiveItemE37 = value;
					return true;
				case 38:
					this.ReceiveItemE38 = value;
					return true;
				case 39:
					this.ReceiveItemE39 = value;
					return true;
				case 40:
					this.ReceiveItemE40 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的收入科目中文與英文名稱
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 40)</param>
		/// <param name="chtValue">指定收入科目中文名稱</param>
		/// <param name="engValue">指定收入科目英文名稱</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetReceiveItemByNo(int no, string chtValue, string engValue)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.ReceiveItem01 = chtValue;
					this.ReceiveItemE01 = engValue;
					return true;
				case 02:
					this.ReceiveItem02 = chtValue;
					this.ReceiveItemE02 = engValue;
					return true;
				case 03:
					this.ReceiveItem03 = chtValue;
					this.ReceiveItemE03 = engValue;
					return true;
				case 04:
					this.ReceiveItem04 = chtValue;
					this.ReceiveItemE04 = engValue;
					return true;
				case 05:
					this.ReceiveItem05 = chtValue;
					this.ReceiveItemE05 = engValue;
					return true;
				case 06:
					this.ReceiveItem06 = chtValue;
					this.ReceiveItemE06 = engValue;
					return true;
				case 07:
					this.ReceiveItem07 = chtValue;
					this.ReceiveItemE07 = engValue;
					return true;
				case 08:
					this.ReceiveItem08 = chtValue;
					this.ReceiveItemE08 = engValue;
					return true;
				case 09:
					this.ReceiveItem09 = chtValue;
					this.ReceiveItemE09 = engValue;
					return true;
				case 10:
					this.ReceiveItem10 = chtValue;
					this.ReceiveItemE10 = engValue;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.ReceiveItem11 = chtValue;
					this.ReceiveItemE11 = engValue;
					return true;
				case 12:
					this.ReceiveItem12 = chtValue;
					this.ReceiveItemE12 = engValue;
					return true;
				case 13:
					this.ReceiveItem13 = chtValue;
					this.ReceiveItemE13 = engValue;
					return true;
				case 14:
					this.ReceiveItem14 = chtValue;
					this.ReceiveItemE14 = engValue;
					return true;
				case 15:
					this.ReceiveItem15 = chtValue;
					this.ReceiveItemE15 = engValue;
					return true;
				case 16:
					this.ReceiveItem16 = chtValue;
					this.ReceiveItemE16 = engValue;
					return true;
				case 17:
					this.ReceiveItem17 = chtValue;
					this.ReceiveItemE17 = engValue;
					return true;
				case 18:
					this.ReceiveItem18 = chtValue;
					this.ReceiveItemE18 = engValue;
					return true;
				case 19:
					this.ReceiveItem19 = chtValue;
					this.ReceiveItemE19 = engValue;
					return true;
				case 20:
					this.ReceiveItem20 = chtValue;
					this.ReceiveItemE20 = engValue;
					return true;
				#endregion

				#region 21 ~ 30
				case 21:
					this.ReceiveItem21 = chtValue;
					this.ReceiveItemE21 = engValue;
					return true;
				case 22:
					this.ReceiveItem22 = chtValue;
					this.ReceiveItemE22 = engValue;
					return true;
				case 23:
					this.ReceiveItem23 = chtValue;
					this.ReceiveItemE23 = engValue;
					return true;
				case 24:
					this.ReceiveItem24 = chtValue;
					this.ReceiveItemE24 = engValue;
					return true;
				case 25:
					this.ReceiveItem25 = chtValue;
					this.ReceiveItemE25 = engValue;
					return true;
				case 26:
					this.ReceiveItem26 = chtValue;
					this.ReceiveItemE26 = engValue;
					return true;
				case 27:
					this.ReceiveItem27 = chtValue;
					this.ReceiveItemE27 = engValue;
					return true;
				case 28:
					this.ReceiveItem28 = chtValue;
					this.ReceiveItemE28 = engValue;
					return true;
				case 29:
					this.ReceiveItem29 = chtValue;
					this.ReceiveItemE29 = engValue;
					return true;
				case 30:
					this.ReceiveItem30 = chtValue;
					this.ReceiveItemE30 = engValue;
					return true;
				#endregion

				#region 31 ~ 40
				case 31:
					this.ReceiveItem31 = chtValue;
					this.ReceiveItemE31 = engValue;
					return true;
				case 32:
					this.ReceiveItem32 = chtValue;
					this.ReceiveItemE32 = engValue;
					return true;
				case 33:
					this.ReceiveItem33 = chtValue;
					this.ReceiveItemE33 = engValue;
					return true;
				case 34:
					this.ReceiveItem34 = chtValue;
					this.ReceiveItemE34 = engValue;
					return true;
				case 35:
					this.ReceiveItem35 = chtValue;
					this.ReceiveItemE35 = engValue;
					return true;
				case 36:
					this.ReceiveItem36 = chtValue;
					this.ReceiveItemE36 = engValue;
					return true;
				case 37:
					this.ReceiveItem37 = chtValue;
					this.ReceiveItemE37 = engValue;
					return true;
				case 38:
					this.ReceiveItem38 = chtValue;
					this.ReceiveItemE38 = engValue;
					return true;
				case 39:
					this.ReceiveItem39 = chtValue;
					this.ReceiveItemE39 = engValue;
					return true;
				case 40:
					this.ReceiveItem40 = chtValue;
					this.ReceiveItemE40 = engValue;
					return true;
				#endregion

				default:
					return false;
			}
		}


		/// <summary>
		/// 依據是否使用英文資料介面，取得所有的收入科目名稱陣列（英文資料介面使用時，沒有中文名稱就沒有英文名稱，沒有英文名稱就用中文名稱）
		/// </summary>
		/// <param name="useEngDataUI">指定是否英文資料介面使用，預設 false</param>
		/// <returns>傳回收入科目名稱陣列</returns>
		public string[] GetAllReceiveItems(bool useEngDataUI = false)
		{
			string[] chts = this.GetAllReceiveItemChts();
			if (useEngDataUI)
			{
				//英文資料介面使用時，以中文名稱為準，沒有中文名稱就沒有英文名稱，沒有英文名稱就用中文名稱
				string[] engs = this.GetAllReceiveItemEngs();
				for (int idx = 0; idx < chts.Length; idx++)
				{
					if ((String.IsNullOrEmpty(chts[idx]) && !String.IsNullOrEmpty(engs[idx]))
						|| (String.IsNullOrEmpty(engs[idx]) && !String.IsNullOrEmpty(chts[idx])))
					{
						engs[idx] = chts[idx];
					}
				}
				return engs;
			}
			else
			{
				return chts;
			}
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 備註項目相關 Method
		/// <summary>
		/// 取得所有的備註項目中文標題陣列，陣列元素依序為由 MemoTitle01 ~ MemoTitle21
		/// </summary>
		/// <returns>傳回所有的備註項目中文標題陣列</returns>
		public string[] GetAllMemoTitleChts()
		{
			return new string[MemoTitleMaxCount] {
				this.MemoTitle01, this.MemoTitle02, this.MemoTitle03, this.MemoTitle04, this.MemoTitle05,
				this.MemoTitle06, this.MemoTitle07, this.MemoTitle08, this.MemoTitle09, this.MemoTitle10,
				this.MemoTitle11, this.MemoTitle12, this.MemoTitle13, this.MemoTitle14, this.MemoTitle15,
				this.MemoTitle16, this.MemoTitle17, this.MemoTitle18, this.MemoTitle19, this.MemoTitle20,
				this.MemoTitle21
			};
		}

		/// <summary>
		/// 取得所有的備註項目英文標題陣列，陣列元素依序為由 MemoTitleE01 ~ MemoTitleE21
		/// </summary>
		/// <returns>傳回所有的備註項目英文標題陣列</returns>
		public string[] GetAllMemoTitleEngs()
		{
			return new string[MemoTitleMaxCount] {
				this.MemoTitleE01, this.MemoTitleE02, this.MemoTitleE03, this.MemoTitleE04, this.MemoTitleE05,
				this.MemoTitleE06, this.MemoTitleE07, this.MemoTitleE08, this.MemoTitleE09, this.MemoTitleE10,
				this.MemoTitleE11, this.MemoTitleE12, this.MemoTitleE13, this.MemoTitleE14, this.MemoTitleE15,
				this.MemoTitleE16, this.MemoTitleE17, this.MemoTitleE18, this.MemoTitleE19, this.MemoTitleE20,
				this.MemoTitleE21
			};
		}

		/// <summary>
		/// 取得指定編號的備註項目中文標題
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 21)</param>
		/// <returns>找到則傳回備註項目中文標題，否則傳回 null</returns>
		public string GetMemoTitleChtByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.MemoTitle01;
				case 02:
					return this.MemoTitle02;
				case 03:
					return this.MemoTitle03;
				case 04:
					return this.MemoTitle04;
				case 05:
					return this.MemoTitle05;
				case 06:
					return this.MemoTitle06;
				case 07:
					return this.MemoTitle07;
				case 08:
					return this.MemoTitle08;
				case 09:
					return this.MemoTitle09;
				case 10:
					return this.MemoTitle10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.MemoTitle11;
				case 12:
					return this.MemoTitle12;
				case 13:
					return this.MemoTitle13;
				case 14:
					return this.MemoTitle14;
				case 15:
					return this.MemoTitle15;
				case 16:
					return this.MemoTitle16;
				case 17:
					return this.MemoTitle17;
				case 18:
					return this.MemoTitle18;
				case 19:
					return this.MemoTitle19;
				case 20:
					return this.MemoTitle20;
				#endregion

				#region 21
				case 21:
					return this.MemoTitle21;
				#endregion

				default:
					return null;
			}
		}

		/// <summary>
		/// 取得指定編號的備註項目英文標題
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 21)</param>
		/// <returns>找到則傳回備註項目英文標題，否則傳回 null</returns>
		public string GetMemoTitleEngByNo(int no)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					return this.MemoTitleE01;
				case 02:
					return this.MemoTitleE02;
				case 03:
					return this.MemoTitleE03;
				case 04:
					return this.MemoTitleE04;
				case 05:
					return this.MemoTitleE05;
				case 06:
					return this.MemoTitleE06;
				case 07:
					return this.MemoTitleE07;
				case 08:
					return this.MemoTitleE08;
				case 09:
					return this.MemoTitleE09;
				case 10:
					return this.MemoTitleE10;
				#endregion

				#region 11 ~ 20
				case 11:
					return this.MemoTitleE11;
				case 12:
					return this.MemoTitleE12;
				case 13:
					return this.MemoTitleE13;
				case 14:
					return this.MemoTitleE14;
				case 15:
					return this.MemoTitleE15;
				case 16:
					return this.MemoTitleE16;
				case 17:
					return this.MemoTitleE17;
				case 18:
					return this.MemoTitleE18;
				case 19:
					return this.MemoTitleE19;
				case 20:
					return this.MemoTitleE20;
				#endregion

				#region 21
				case 21:
					return this.MemoTitleE21;
				#endregion

				default:
					return null;
			}
		}

		/// <summary>
		/// 取得指定編號的備註項目中文與英文標題
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 21)</param>
		/// <param name="chtValue">找到則傳回備註項目中文標題，否則傳回 null</param>
		/// <param name="engValue">找到則傳回備註項目英文標題，否則傳回 null</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool GetMemoTitleByNo(int no, out string chtValue, out string engValue)
		{
			chtValue = null;
			engValue = null;
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					chtValue = this.MemoTitle01;
					engValue = this.MemoTitleE01;
					return true;
				case 02:
					chtValue = this.MemoTitle02;
					engValue = this.MemoTitleE02;
					return true;
				case 03:
					chtValue = this.MemoTitle03;
					engValue = this.MemoTitleE03;
					return true;
				case 04:
					chtValue = this.MemoTitle04;
					engValue = this.MemoTitleE04;
					return true;
				case 05:
					chtValue = this.MemoTitle05;
					engValue = this.MemoTitleE05;
					return true;
				case 06:
					chtValue = this.MemoTitle06;
					engValue = this.MemoTitleE06;
					return true;
				case 07:
					chtValue = this.MemoTitle07;
					engValue = this.MemoTitleE07;
					return true;
				case 08:
					chtValue = this.MemoTitle08;
					engValue = this.MemoTitleE08;
					return true;
				case 09:
					chtValue = this.MemoTitle09;
					engValue = this.MemoTitleE09;
					return true;
				case 10:
					chtValue = this.MemoTitle10;
					engValue = this.MemoTitleE10;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					chtValue = this.MemoTitle11;
					engValue = this.MemoTitleE11;
					return true;
				case 12:
					chtValue = this.MemoTitle12;
					engValue = this.MemoTitleE12;
					return true;
				case 13:
					chtValue = this.MemoTitle13;
					engValue = this.MemoTitleE13;
					return true;
				case 14:
					chtValue = this.MemoTitle14;
					engValue = this.MemoTitleE14;
					return true;
				case 15:
					chtValue = this.MemoTitle15;
					engValue = this.MemoTitleE15;
					return true;
				case 16:
					chtValue = this.MemoTitle16;
					engValue = this.MemoTitleE16;
					return true;
				case 17:
					chtValue = this.MemoTitle17;
					engValue = this.MemoTitleE17;
					return true;
				case 18:
					chtValue = this.MemoTitle18;
					engValue = this.MemoTitleE18;
					return true;
				case 19:
					chtValue = this.MemoTitle19;
					engValue = this.MemoTitleE19;
					return true;
				case 20:
					chtValue = this.MemoTitle20;
					engValue = this.MemoTitleE20;
					return true;
				#endregion

				#region 21
				case 21:
					chtValue = this.MemoTitle21;
					engValue = this.MemoTitleE21;
					return true;
				#endregion

				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的備註項目中文備註
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 21)</param>
		/// <param name="value">指定備註項目中文備註</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetMemoTitleChtByNo(int no, string value)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.MemoTitle01 = value;
					return true;
				case 02:
					this.MemoTitle02 = value;
					return true;
				case 03:
					this.MemoTitle03 = value;
					return true;
				case 04:
					this.MemoTitle04 = value;
					return true;
				case 05:
					this.MemoTitle05 = value;
					return true;
				case 06:
					this.MemoTitle06 = value;
					return true;
				case 07:
					this.MemoTitle07 = value;
					return true;
				case 08:
					this.MemoTitle08 = value;
					return true;
				case 09:
					this.MemoTitle09 = value;
					return true;
				case 10:
					this.MemoTitle10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.MemoTitle11 = value;
					return true;
				case 12:
					this.MemoTitle12 = value;
					return true;
				case 13:
					this.MemoTitle13 = value;
					return true;
				case 14:
					this.MemoTitle14 = value;
					return true;
				case 15:
					this.MemoTitle15 = value;
					return true;
				case 16:
					this.MemoTitle16 = value;
					return true;
				case 17:
					this.MemoTitle17 = value;
					return true;
				case 18:
					this.MemoTitle18 = value;
					return true;
				case 19:
					this.MemoTitle19 = value;
					return true;
				case 20:
					this.MemoTitle20 = value;
					return true;
				#endregion

				#region 21
				case 21:
					this.MemoTitle21 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的備註項目英文備註
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 21)</param>
		/// <param name="value">指定備註項目英文備註</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetMemoTitleEngByNo(int no, string value)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.MemoTitleE01 = value;
					return true;
				case 02:
					this.MemoTitleE02 = value;
					return true;
				case 03:
					this.MemoTitleE03 = value;
					return true;
				case 04:
					this.MemoTitleE04 = value;
					return true;
				case 05:
					this.MemoTitleE05 = value;
					return true;
				case 06:
					this.MemoTitleE06 = value;
					return true;
				case 07:
					this.MemoTitleE07 = value;
					return true;
				case 08:
					this.MemoTitleE08 = value;
					return true;
				case 09:
					this.MemoTitleE09 = value;
					return true;
				case 10:
					this.MemoTitleE10 = value;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.MemoTitleE11 = value;
					return true;
				case 12:
					this.MemoTitleE12 = value;
					return true;
				case 13:
					this.MemoTitleE13 = value;
					return true;
				case 14:
					this.MemoTitleE14 = value;
					return true;
				case 15:
					this.MemoTitleE15 = value;
					return true;
				case 16:
					this.MemoTitleE16 = value;
					return true;
				case 17:
					this.MemoTitleE17 = value;
					return true;
				case 18:
					this.MemoTitleE18 = value;
					return true;
				case 19:
					this.MemoTitleE19 = value;
					return true;
				case 20:
					this.MemoTitleE20 = value;
					return true;
				#endregion

				#region 21
				case 21:
					this.MemoTitleE21 = value;
					return true;
				#endregion

				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的備註項目中文與英文備註
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 21)</param>
		/// <param name="chtValue">指定備註項目中文備註</param>
		/// <param name="engValue">指定備註項目英文備註</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetMemoTitleByNo(int no, string chtValue, string engValue)
		{
			switch (no)
			{
				#region 01 ~ 10
				case 01:
					this.MemoTitle01 = chtValue;
					this.MemoTitleE01 = engValue;
					return true;
				case 02:
					this.MemoTitle02 = chtValue;
					this.MemoTitleE02 = engValue;
					return true;
				case 03:
					this.MemoTitle03 = chtValue;
					this.MemoTitleE03 = engValue;
					return true;
				case 04:
					this.MemoTitle04 = chtValue;
					this.MemoTitleE04 = engValue;
					return true;
				case 05:
					this.MemoTitle05 = chtValue;
					this.MemoTitleE05 = engValue;
					return true;
				case 06:
					this.MemoTitle06 = chtValue;
					this.MemoTitleE06 = engValue;
					return true;
				case 07:
					this.MemoTitle07 = chtValue;
					this.MemoTitleE07 = engValue;
					return true;
				case 08:
					this.MemoTitle08 = chtValue;
					this.MemoTitleE08 = engValue;
					return true;
				case 09:
					this.MemoTitle09 = chtValue;
					this.MemoTitleE09 = engValue;
					return true;
				case 10:
					this.MemoTitle10 = chtValue;
					this.MemoTitleE10 = engValue;
					return true;
				#endregion

				#region 11 ~ 20
				case 11:
					this.MemoTitle11 = chtValue;
					this.MemoTitleE11 = engValue;
					return true;
				case 12:
					this.MemoTitle12 = chtValue;
					this.MemoTitleE12 = engValue;
					return true;
				case 13:
					this.MemoTitle13 = chtValue;
					this.MemoTitleE13 = engValue;
					return true;
				case 14:
					this.MemoTitle14 = chtValue;
					this.MemoTitleE14 = engValue;
					return true;
				case 15:
					this.MemoTitle15 = chtValue;
					this.MemoTitleE15 = engValue;
					return true;
				case 16:
					this.MemoTitle16 = chtValue;
					this.MemoTitleE16 = engValue;
					return true;
				case 17:
					this.MemoTitle17 = chtValue;
					this.MemoTitleE17 = engValue;
					return true;
				case 18:
					this.MemoTitle18 = chtValue;
					this.MemoTitleE18 = engValue;
					return true;
				case 19:
					this.MemoTitle19 = chtValue;
					this.MemoTitleE19 = engValue;
					return true;
				case 20:
					this.MemoTitle20 = chtValue;
					this.MemoTitleE20 = engValue;
					return true;
				#endregion

				#region 21
				case 21:
					this.MemoTitle21 = chtValue;
					this.MemoTitleE21 = engValue;
					return true;
				#endregion

				default:
					return false;
			}
		}


		/// <summary>
		/// 依據是否使用英文資料介面，取得所有的備註項目標題陣列（英文資料介面使用時，沒有中文標題就沒有英文標題，沒有英文標題就用中文標題）
		/// </summary>
		/// <param name="useEngDataUI">指定是否英文資料介面使用，預設 false</param>
		/// <returns>傳回備註項目標題陣列</returns>
		public string[] GetAllMemoTitle(bool useEngDataUI = false)
		{
			string[] chts = this.GetAllMemoTitleChts();
			if (useEngDataUI)
			{
				//英文資料介面使用時，以中文名稱為準，沒有中文名稱就沒有英文名稱，沒有英文名稱就用中文名稱
				string[] engs = this.GetAllMemoTitleEngs();
				for (int idx = 0; idx < chts.Length; idx++)
				{
					if ((String.IsNullOrEmpty(chts[idx]) && !String.IsNullOrEmpty(engs[idx]))
						|| (String.IsNullOrEmpty(engs[idx]) && !String.IsNullOrEmpty(chts[idx])))
					{
						engs[idx] = chts[idx];
					}
				}
				return engs;
			}
			else
			{
				return chts;
			}
		}
		#endregion

		#region [MDY:202203XX] 2022擴充案 注意事項相關 Method
		/// <summary>
		/// 取得所有的注意事項中文內容陣列，陣列元素依序為 Brief1 ~ Brief6
		/// </summary>
		/// <returns>傳回所有的注意事項中文內容陣列</returns>
		public string[] GetAllBriefChts()
		{
			return new string[BriefMaxCount] {
				this.Brief1, this.Brief2, this.Brief3, this.Brief4, this.Brief5,
				this.Brief6
			};
		}

		/// <summary>
		/// 取得所有的注意事項英文內容陣列，陣列元素依序為 BriefE1 ~ BriefE6
		/// </summary>
		/// <returns>傳回所有的注意事項英文內容陣列</returns>
		public string[] GetAllBriefEngs()
		{
			return new string[BriefMaxCount] {
				this.BriefE1, this.BriefE2, this.BriefE3, this.BriefE4, this.BriefE5,
				this.BriefE6
			};
		}

		/// <summary>
		/// 取得指定編號的注意事項中文內容
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 6)</param>
		/// <returns>找到則傳回注意事項中文內容，否則傳回 null</returns>
		public string GetBriefChtByNo(int no)
		{
			switch (no)
			{
				case 1:
					return this.Brief1;
				case 2:
					return this.Brief2;
				case 3:
					return this.Brief3;
				case 4:
					return this.Brief4;
				case 5:
					return this.Brief5;
				case 6:
					return this.Brief6;
				default:
					return null;
			}
		}

		/// <summary>
		/// 取得指定編號的注意事項英文內容
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 6)</param>
		/// <returns>找到則傳回注意事項英文內容，否則傳回 null</returns>
		public string GetBriefEngByNo(int no)
		{
			switch (no)
			{
				case 1:
					return this.BriefE1;
				case 2:
					return this.BriefE2;
				case 3:
					return this.BriefE3;
				case 4:
					return this.BriefE4;
				case 5:
					return this.BriefE5;
				case 6:
					return this.BriefE6;
				default:
					return null;
			}
		}

		/// <summary>
		/// 取得指定編號的注意事項中文與英文內容
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 6)</param>
		/// <param name="chtValue">找到則傳回注意事項中文內容，否則傳回 null</param>
		/// <param name="engValue">找到則傳回注意事項英文內容，否則傳回 null</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool GetBriefByNo(int no, out string chtValue, out string engValue)
		{
			chtValue = null;
			engValue = null;
			switch (no)
			{
				case 1:
					chtValue = this.Brief1;
					engValue = this.BriefE1;
					return true;
				case 2:
					chtValue = this.Brief2;
					engValue = this.BriefE2;
					return true;
				case 3:
					chtValue = this.Brief3;
					engValue = this.BriefE3;
					return true;
				case 4:
					chtValue = this.Brief4;
					engValue = this.BriefE4;
					return true;
				case 5:
					chtValue = this.Brief5;
					engValue = this.BriefE5;
					return true;
				case 6:
					chtValue = this.Brief6;
					engValue = this.BriefE6;
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的注意事項中文內容
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 6)</param>
		/// <param name="value">指定注意事項中文內容</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetBriefChtByNo(int no, string value)
		{
			switch (no)
			{
				case 1:
					this.Brief1 = value;
					return true;
				case 2:
					this.Brief2 = value;
					return true;
				case 3:
					this.Brief3 = value;
					return true;
				case 4:
					this.Brief4 = value;
					return true;
				case 5:
					this.Brief5 = value;
					return true;
				case 6:
					this.Brief6 = value;
					return true;
				default:
					 return false;
			}
		}

		/// <summary>
		/// 設定指定編號的注意事項英文內容
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 6)</param>
		/// <param name="value">指定注意事項英文內容</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetBriefEngByNo(int no, string value)
		{
			switch (no)
			{
				case 1:
					this.BriefE1 = value;
					return true;
				case 2:
					this.BriefE2 = value;
					return true;
				case 3:
					this.BriefE3 = value;
					return true;
				case 4:
					this.BriefE4 = value;
					return true;
				case 5:
					this.BriefE5 = value;
					return true;
				case 6:
					this.BriefE6 = value;
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// 設定指定編號的注意事項的中文與英文內容
		/// </summary>
		/// <param name="no">指定編號 (1 ~ 6)</param>
		/// <param name="chtValue">指定注意事項中文內容</param>
		/// <param name="engValue">指定注意事項英文內容</param>
		/// <returns>找到則傳回 true，否則傳回 false</returns>
		public bool SetBriefByNo(int no, string chtValue, string engValue)
		{
			switch (no)
			{
				case 1:
					this.Brief1 = chtValue;
					this.BriefE1 = engValue;
					return true;
				case 2:
					this.Brief2 = chtValue;
					this.BriefE2 = engValue;
					return true;
				case 3:
					this.Brief3 = chtValue;
					this.BriefE3 = engValue;
					return true;
				case 4:
					this.Brief4 = chtValue;
					this.BriefE4 = engValue;
					return true;
				case 5:
					this.Brief5 = chtValue;
					this.BriefE5 = engValue;
					return true;
				case 6:
					this.Brief6 = chtValue;
					this.BriefE6 = engValue;
					return true;
				default:
					return false;
			}
		}


		/// <summary>
		/// 依據是否使用英文資料介面，取得所有的備註項目標題陣列（英文資料介面使用時，沒有中文標題就沒有英文標題，沒有英文標題就用中文標題）
		/// </summary>
		/// <param name="useEngDataUI">指定是否英文資料介面使用，預設 false</param>
		/// <returns>傳回備註項目標題陣列</returns>
		public string[] GetAllBrief(bool useEngDataUI = false)
		{
			string[] chts = this.GetAllBriefChts();
			if (useEngDataUI)
			{
				//英文資料介面使用時，以中文名稱為準，沒有中文名稱就沒有英文名稱，沒有英文名稱就用中文名稱
				string[] engs = this.GetAllBriefEngs();
				for (int idx = 0; idx < chts.Length; idx++)
				{
					if ((String.IsNullOrEmpty(chts[idx]) && !String.IsNullOrEmpty(engs[idx]))
						|| (String.IsNullOrEmpty(engs[idx]) && !String.IsNullOrEmpty(chts[idx])))
					{
						engs[idx] = chts[idx];
					}
				}
				return engs;
			}
			else
			{
				return chts;
			}
		}
		#endregion
	}
}
