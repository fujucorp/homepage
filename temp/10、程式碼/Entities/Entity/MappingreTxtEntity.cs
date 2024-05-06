/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:35:53
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
    /// 上傳學生繳費資料 TXT 對照檔 資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class MappingreTxtEntity : Entity
	{
        #region Const
        /// <summary>
        /// 備註數量 (21)
        /// </summary>
        public const int MemoCount = 21;
        #endregion

		public const string TABLE_NAME = "MappingRe_Txt";

		#region Field Name Const Class
		/// <summary>
		/// MappingreTxtEntity 欄位名稱定義抽象類別
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
            /// 所屬年別代號 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
            /// 所屬期別代號 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
            /// 所屬部別代碼 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
            /// 所屬代收費用別 欄位名稱常數定義 (土銀不使用)
			/// </summary>
			public const string ReceiveId = "Receive_Id";
			#endregion

			#region 上傳資料相關欄位
			#region 學生基本資料對照欄位 (StudentMasterEntity)
			/// <summary>
			/// 學號欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuIdS = "Stu_Id_S";

			/// <summary>
            /// 學號欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuIdL = "Stu_Id_L";

			/// <summary>
            /// 學生姓名欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuNameS = "Stu_Name_S";

			/// <summary>
            /// 學生姓名欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuNameL = "Stu_Name_L";

			/// <summary>
            /// 學生生日欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuBirthdayS = "Stu_Birthday_S";

			/// <summary>
            /// 學生生日欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuBirthdayL = "Stu_Birthday_L";

			/// <summary>
            /// 學生身份證欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdNumberS = "Id_Number_S";

			/// <summary>
            /// 學生身份證欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdNumberL = "Id_Number_L";

			/// <summary>
			/// Stu_Tel_S 欄位名稱常數定義
			/// </summary>
			public const string StuTelS = "Stu_Tel_S";

			/// <summary>
			/// Stu_Tel_L 欄位名稱常數定義
			/// </summary>
			public const string StuTelL = "Stu_Tel_L";

			/// <summary>
			/// Stu_Addcode_S 欄位名稱常數定義
			/// </summary>
			public const string StuAddcodeS = "Stu_Addcode_S";

			/// <summary>
			/// Stu_Addcode_L 欄位名稱常數定義
			/// </summary>
			public const string StuAddcodeL = "Stu_Addcode_L";

			/// <summary>
			/// Stu_Add_S 欄位名稱常數定義
			/// </summary>
			public const string StuAddS = "Stu_Add_S";

			/// <summary>
			/// Stu_Add_L 欄位名稱常數定義
			/// </summary>
			public const string StuAddL = "Stu_Add_L";

            /// <summary>
            /// Email_S 欄位名稱常數定義
            /// </summary>
            public const string EmailS = "Email_S";

            /// <summary>
            /// Email_L 欄位名稱常數定義
            /// </summary>
            public const string EmailL = "Email_L";

            /// <summary>
            /// Stu_Parent_S 欄位名稱常數定義
            /// </summary>
            public const string StuParentS = "Stu_Parent_S";

            /// <summary>
            /// Stu_Parent_L 欄位名稱常數定義
            /// </summary>
            public const string StuParentL = "Stu_Parent_L";
            #endregion

            #region [MDY:20160131] 增加資料序號與繳款期限對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 資料序號欄位起始位置
            /// </summary>
            public const string OldSeqS = "Old_Seq_S";

            /// <summary>
            /// 資料序號欄位字元長度
            /// </summary>
            public const string OldSeqL = "Old_Seq_L";

            /// <summary>
            /// 繳款期限欄位起始位置
            /// </summary>
            public const string PayDueDateS = "Pay_Due_Date_S";

            /// <summary>
            /// 繳款期限欄位字元長度
            /// </summary>
            public const string PayDueDateL = "Pay_Due_Date_L";
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 是否啟用國際信用卡繳費欄位起始位置
            /// </summary>
            public const string NCCardFlagS = "NCCardFlag_S";

            /// <summary>
            /// 是否啟用國際信用卡繳費旗標欄位字元長度
            /// </summary>
            public const string NCCardFlagL = "NCCardFlag_L";
			#endregion

			#region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
			/// <summary>
			/// 學年欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuGradeS = "Stu_Grade_S";

			/// <summary>
			/// 學年欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuGradeL = "Stu_Grade_L";

			/// <summary>
			/// 部別代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeptIdS = "Dept_Id_S";

			/// <summary>
			/// 部別代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeptIdL = "Dept_Id_L";

			/// <summary>
			/// 部別名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeptNameS = "Dept_Name_S";

			/// <summary>
			/// 部別名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeptNameL = "Dept_Name_L";

			/// <summary>
			/// 院別代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string CollegeIdS = "College_Id_S";

			/// <summary>
			/// 院別代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string CollegeIdL = "College_Id_L";

			/// <summary>
			/// 院別名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string CollegeNameS = "College_Name_S";

			/// <summary>
			/// 院別名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string CollegeNameL = "College_Name_L";

			/// <summary>
			/// 系所代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string MajorIdS = "Major_Id_S";

			/// <summary>
			/// 系所代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string MajorIdL = "Major_Id_L";

			/// <summary>
			/// 系所名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string MajorNameS = "Major_Name_S";

			/// <summary>
			/// 系所名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string MajorNameL = "Major_Name_L";

			/// <summary>
			/// 班別代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ClassIdS = "Class_Id_S";

			/// <summary>
			/// 班別代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ClassIdL = "Class_Id_L";

			/// <summary>
			/// 班別名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ClassNameS = "Class_Name_S";

			/// <summary>
			/// 班別名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ClassNameL = "Class_Name_L";

			/// <summary>
			/// 總學分數欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuCreditS = "Stu_Credit_S";

			/// <summary>
			/// 總學分數欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuCreditL = "Stu_Credit_L";

			/// <summary>
			/// 上課時數欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuHourS = "Stu_Hour_S";

			/// <summary>
			/// 上課時數欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuHourL = "Stu_Hour_L";
			#endregion

			#region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
			/// <summary>
			/// 減免代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ReduceIdS = "Reduce_Id_S";

			/// <summary>
			/// 減免代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ReduceIdL = "Reduce_Id_L";

			/// <summary>
			/// 減免名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ReduceNameS = "Reduce_Name_S";

			/// <summary>
			/// 減免名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ReduceNameL = "Reduce_Name_L";

			/// <summary>
			/// 住宿代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DormIdS = "Dorm_Id_S";

			/// <summary>
			/// 住宿代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DormIdL = "Dorm_Id_L";

			/// <summary>
			/// 住宿名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DormNameS = "Dorm_Name_S";

			/// <summary>
			/// 住宿名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DormNameL = "Dorm_Name_L";

			/// <summary>
			/// 就貸代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoanIdS = "Loan_Id_S";

			/// <summary>
			/// 就貸代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string LoanIdL = "Loan_Id_L";

			/// <summary>
			/// 就貸名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoanNameS = "Loan_Name_S";

			/// <summary>
			/// 就貸名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string LoanNameL = "Loan_Name_L";
			#endregion

			#region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
			/// <summary>
			/// 身份註記01代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId1S = "Identify_Id1_S";

			/// <summary>
			/// 身份註記01代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId1L = "Identify_Id1_L";

			/// <summary>
			/// 身份註記01名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName1S = "Identify_Name1_S";

			/// <summary>
			/// 身份註記01名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName1L = "Identify_Name1_L";

			/// <summary>
			/// 身份註記02代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId2S = "Identify_Id2_S";

			/// <summary>
			/// 身份註記02代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId2L = "Identify_Id2_L";

			/// <summary>
			/// 身份註記02名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName2S = "Identify_Name2_S";

			/// <summary>
			/// 身份註記02名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName2L = "Identify_Name2_L";

			/// <summary>
			/// 身份註記03代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId3S = "Identify_Id3_S";

			/// <summary>
			/// 身份註記03代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId3L = "Identify_Id3_L";

			/// <summary>
			/// 身份註記03名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName3S = "Identify_Name3_S";

			/// <summary>
			/// 身份註記03名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName3L = "Identify_Name3_L";

			/// <summary>
			/// 身份註記04代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId4S = "Identify_Id4_S";

			/// <summary>
			/// 身份註記04代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId4L = "Identify_Id4_L";

			/// <summary>
			/// 身份註記04名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName4S = "Identify_Name4_S";

			/// <summary>
			/// 身份註記04名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName4L = "Identify_Name4_L";

			/// <summary>
			/// 身份註記05代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId5S = "Identify_Id5_S";

			/// <summary>
			/// 身份註記05代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId5L = "Identify_Id5_L";

			/// <summary>
			/// 身份註記05名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName5S = "Identify_Name5_S";

			/// <summary>
			/// 身份註記05名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName5L = "Identify_Name5_L";

			/// <summary>
			/// 身份註記06代碼欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId6S = "Identify_Id6_S";

			/// <summary>
			/// 身份註記06代碼欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyId6L = "Identify_Id6_L";

			/// <summary>
			/// 身份註記06名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName6S = "Identify_Name6_S";

			/// <summary>
			/// 身份註記06名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyName6L = "Identify_Name6_L";
			#endregion

			#region 繳費資料對照欄位 (StudentReceiveEntity)
			/// <summary>
			/// 收入科目01金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive1S = "Receive_1_S";

			/// <summary>
			/// 收入科目01金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive1L = "Receive_1_L";

			/// <summary>
			/// 收入科目02金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive2S = "Receive_2_S";

			/// <summary>
			/// 收入科目02金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive2L = "Receive_2_L";

			/// <summary>
			/// 收入科目03金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive3S = "Receive_3_S";

			/// <summary>
			/// 收入科目03金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive3L = "Receive_3_L";

			/// <summary>
			/// 收入科目04金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive4S = "Receive_4_S";

			/// <summary>
			/// 收入科目04金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive4L = "Receive_4_L";

			/// <summary>
			/// 收入科目05金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive5S = "Receive_5_S";

			/// <summary>
			/// 收入科目05金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive5L = "Receive_5_L";

			/// <summary>
			/// 收入科目06金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive6S = "Receive_6_S";

			/// <summary>
			/// 收入科目06金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive6L = "Receive_6_L";

			/// <summary>
			/// 收入科目07金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive7S = "Receive_7_S";

			/// <summary>
			/// 收入科目07金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive7L = "Receive_7_L";

			/// <summary>
			/// 收入科目08金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive8S = "Receive_8_S";

			/// <summary>
			/// 收入科目08金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive8L = "Receive_8_L";

			/// <summary>
			/// 收入科目09金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive9S = "Receive_9_S";

			/// <summary>
			/// 收入科目09金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive9L = "Receive_9_L";

			/// <summary>
			/// 收入科目10金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive10S = "Receive_10_S";

			/// <summary>
			/// 收入科目10金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive10L = "Receive_10_L";

			/// <summary>
			/// 收入科目11金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive11S = "Receive_11_S";

			/// <summary>
			/// 收入科目11金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive11L = "Receive_11_L";

			/// <summary>
			/// 收入科目12金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive12S = "Receive_12_S";

			/// <summary>
			/// 收入科目12金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive12L = "Receive_12_L";

			/// <summary>
			/// 收入科目13金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive13S = "Receive_13_S";

			/// <summary>
			/// 收入科目13金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive13L = "Receive_13_L";

			/// <summary>
			/// 收入科目14金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive14S = "Receive_14_S";

			/// <summary>
			/// 收入科目14金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive14L = "Receive_14_L";

			/// <summary>
			/// 收入科目15金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive15S = "Receive_15_S";

			/// <summary>
			/// 收入科目15金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive15L = "Receive_15_L";

			/// <summary>
			/// 收入科目16金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive16S = "Receive_16_S";

			/// <summary>
			/// 收入科目16金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive16L = "Receive_16_L";

			/// <summary>
			/// 收入科目17金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive17S = "Receive_17_S";

			/// <summary>
			/// 收入科目17金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive17L = "Receive_17_L";

			/// <summary>
			/// 收入科目18金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive18S = "Receive_18_S";

			/// <summary>
			/// 收入科目18金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive18L = "Receive_18_L";

			/// <summary>
			/// 收入科目19金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive19S = "Receive_19_S";

			/// <summary>
			/// 收入科目19金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive19L = "Receive_19_L";

			/// <summary>
			/// 收入科目20金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive20S = "Receive_20_S";

			/// <summary>
			/// 收入科目20金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive20L = "Receive_20_L";

			/// <summary>
			/// 收入科目21金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive21S = "Receive_21_S";

			/// <summary>
			/// 收入科目21金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive21L = "Receive_21_L";

			/// <summary>
			/// 收入科目22金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive22S = "Receive_22_S";

			/// <summary>
			/// 收入科目22金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive22L = "Receive_22_L";

			/// <summary>
			/// 收入科目23金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive23S = "Receive_23_S";

			/// <summary>
			/// 收入科目23金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive23L = "Receive_23_L";

			/// <summary>
			/// 收入科目24金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive24S = "Receive_24_S";

			/// <summary>
			/// 收入科目24金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive24L = "Receive_24_L";

			/// <summary>
			/// 收入科目25金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive25S = "Receive_25_S";

			/// <summary>
			/// 收入科目25金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive25L = "Receive_25_L";

			/// <summary>
			/// 收入科目26金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive26S = "Receive_26_S";

			/// <summary>
			/// 收入科目26金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive26L = "Receive_26_L";

			/// <summary>
			/// 收入科目27金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive27S = "Receive_27_S";

			/// <summary>
			/// 收入科目27金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive27L = "Receive_27_L";

			/// <summary>
			/// 收入科目28金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive28S = "Receive_28_S";

			/// <summary>
			/// 收入科目28金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive28L = "Receive_28_L";

			/// <summary>
			/// 收入科目29金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive29S = "Receive_29_S";

			/// <summary>
			/// 收入科目29金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive29L = "Receive_29_L";

			/// <summary>
			/// 收入科目30金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive30S = "Receive_30_S";

			/// <summary>
			/// 收入科目30金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive30L = "Receive_30_L";

			/// <summary>
			/// 收入科目31金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive31S = "Receive_31_S";

			/// <summary>
			/// 收入科目31金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive31L = "Receive_31_L";

			/// <summary>
			/// 收入科目32金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive32S = "Receive_32_S";

			/// <summary>
			/// 收入科目32金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive32L = "Receive_32_L";

			/// <summary>
			/// 收入科目33金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive33S = "Receive_33_S";

			/// <summary>
			/// 收入科目33金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive33L = "Receive_33_L";

			/// <summary>
			/// 收入科目34金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive34S = "Receive_34_S";

			/// <summary>
			/// 收入科目34金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive34L = "Receive_34_L";

			/// <summary>
			/// 收入科目35金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive35S = "Receive_35_S";

			/// <summary>
			/// 收入科目35金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive35L = "Receive_35_L";

			/// <summary>
			/// 收入科目36金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive36S = "Receive_36_S";

			/// <summary>
			/// 收入科目36金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive36L = "Receive_36_L";

			/// <summary>
			/// 收入科目37金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive37S = "Receive_37_S";

			/// <summary>
			/// 收入科目37金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive37L = "Receive_37_L";

			/// <summary>
			/// 收入科目38金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive38S = "Receive_38_S";

			/// <summary>
			/// 收入科目38金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive38L = "Receive_38_L";

			/// <summary>
			/// 收入科目39金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive39S = "Receive_39_S";

			/// <summary>
			/// 收入科目39金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive39L = "Receive_39_L";

			/// <summary>
			/// 收入科目40金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string Receive40S = "Receive_40_S";

			/// <summary>
			/// 收入科目40金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string Receive40L = "Receive_40_L";

			/// <summary>
			/// 可貸金額欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoanAmountS = "Loan_Amount_S";

			/// <summary>
			/// 可貸金額欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string LoanAmountL = "Loan_Amount_L";

			/// <summary>
			/// 繳費金額合計欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ReceiveAmountS = "Receive_Amount_S";

			/// <summary>
			/// 繳費金額合計欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ReceiveAmountL = "Receive_Amount_L";

			/// <summary>
			/// 座號欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string StuHidS = "Stu_Hid_S";

			/// <summary>
			/// 座號欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string StuHidL = "Stu_Hid_L";

			/// <summary>
			/// Remark (土銀沒用到) 欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string RemarkS = "Remark_S";

			/// <summary>
			/// Remark (土銀沒用到) 欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string RemarkL = "Remark_L";

			/// <summary>
			/// 流水號欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string SeriorNoS = "Serior_No_S";

			/// <summary>
			/// 流水號欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string SeriorNoL = "Serior_No_L";

			/// <summary>
			/// 銷帳編號欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string CancelNoS = "Cancel_No_S";

			/// <summary>
			/// 銷帳編號欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string CancelNoL = "Cancel_No_L";
			#endregion

			#region 其他繳費資料對照欄位
			/// <summary>
			/// Credit_Id1_S 欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string CreditId1S = "Credit_Id1_S";

			/// <summary>
			/// Credit_Id1_L 欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string CreditId1L = "Credit_Id1_L";

			/// <summary>
			/// Course_Id1_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId1S = "Course_Id1_S";

			/// <summary>
			/// Course_Id1_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId1L = "Course_Id1_L";

			/// <summary>
			/// Credit1_S 欄位名稱常數定義
			/// </summary>
			public const string Credit1S = "Credit1_S";

			/// <summary>
			/// Credit1_L 欄位名稱常數定義
			/// </summary>
			public const string Credit1L = "Credit1_L";

			/// <summary>
			/// Credit_Id2_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId2S = "Credit_Id2_S";

			/// <summary>
			/// Credit_Id2_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId2L = "Credit_Id2_L";

			/// <summary>
			/// Course_Id2_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId2S = "Course_Id2_S";

			/// <summary>
			/// Course_Id2_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId2L = "Course_Id2_L";

			/// <summary>
			/// Credit2_S 欄位名稱常數定義
			/// </summary>
			public const string Credit2S = "Credit2_S";

			/// <summary>
			/// Credit2_L 欄位名稱常數定義
			/// </summary>
			public const string Credit2L = "Credit2_L";

			/// <summary>
			/// Credit_Id3_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId3S = "Credit_Id3_S";

			/// <summary>
			/// Credit_Id3_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId3L = "Credit_Id3_L";

			/// <summary>
			/// Course_Id3_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId3S = "Course_Id3_S";

			/// <summary>
			/// Course_Id3_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId3L = "Course_Id3_L";

			/// <summary>
			/// Credit3_S 欄位名稱常數定義
			/// </summary>
			public const string Credit3S = "Credit3_S";

			/// <summary>
			/// Credit3_L 欄位名稱常數定義
			/// </summary>
			public const string Credit3L = "Credit3_L";

			/// <summary>
			/// Credit_Id4_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId4S = "Credit_Id4_S";

			/// <summary>
			/// Credit_Id4_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId4L = "Credit_Id4_L";

			/// <summary>
			/// Course_Id4_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId4S = "Course_Id4_S";

			/// <summary>
			/// Course_Id4_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId4L = "Course_Id4_L";

			/// <summary>
			/// Credit4_S 欄位名稱常數定義
			/// </summary>
			public const string Credit4S = "Credit4_S";

			/// <summary>
			/// Credit4_L 欄位名稱常數定義
			/// </summary>
			public const string Credit4L = "Credit4_L";

			/// <summary>
			/// Credit_Id5_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId5S = "Credit_Id5_S";

			/// <summary>
			/// Credit_Id5_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId5L = "Credit_Id5_L";

			/// <summary>
			/// Course_Id5_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId5S = "Course_Id5_S";

			/// <summary>
			/// Course_Id5_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId5L = "Course_Id5_L";

			/// <summary>
			/// Credit5_S 欄位名稱常數定義
			/// </summary>
			public const string Credit5S = "Credit5_S";

			/// <summary>
			/// Credit5_L 欄位名稱常數定義
			/// </summary>
			public const string Credit5L = "Credit5_L";

			/// <summary>
			/// Credit_Id6_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId6S = "Credit_Id6_S";

			/// <summary>
			/// Credit_Id6_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId6L = "Credit_Id6_L";

			/// <summary>
			/// Course_Id6_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId6S = "Course_Id6_S";

			/// <summary>
			/// Course_Id6_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId6L = "Course_Id6_L";

			/// <summary>
			/// Credit6_S 欄位名稱常數定義
			/// </summary>
			public const string Credit6S = "Credit6_S";

			/// <summary>
			/// Credit6_L 欄位名稱常數定義
			/// </summary>
			public const string Credit6L = "Credit6_L";

			/// <summary>
			/// Credit_Id7_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId7S = "Credit_Id7_S";

			/// <summary>
			/// Credit_Id7_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId7L = "Credit_Id7_L";

			/// <summary>
			/// Course_Id7_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId7S = "Course_Id7_S";

			/// <summary>
			/// Course_Id7_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId7L = "Course_Id7_L";

			/// <summary>
			/// Credit7_S 欄位名稱常數定義
			/// </summary>
			public const string Credit7S = "Credit7_S";

			/// <summary>
			/// Credit7_L 欄位名稱常數定義
			/// </summary>
			public const string Credit7L = "Credit7_L";

			/// <summary>
			/// Credit_Id8_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId8S = "Credit_Id8_S";

			/// <summary>
			/// Credit_Id8_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId8L = "Credit_Id8_L";

			/// <summary>
			/// Course_Id8_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId8S = "Course_Id8_S";

			/// <summary>
			/// Course_Id8_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId8L = "Course_Id8_L";

			/// <summary>
			/// Credit8_S 欄位名稱常數定義
			/// </summary>
			public const string Credit8S = "Credit8_S";

			/// <summary>
			/// Credit8_L 欄位名稱常數定義
			/// </summary>
			public const string Credit8L = "Credit8_L";

			/// <summary>
			/// Credit_Id9_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId9S = "Credit_Id9_S";

			/// <summary>
			/// Credit_Id9_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId9L = "Credit_Id9_L";

			/// <summary>
			/// Course_Id9_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId9S = "Course_Id9_S";

			/// <summary>
			/// Course_Id9_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId9L = "Course_Id9_L";

			/// <summary>
			/// Credit9_S 欄位名稱常數定義
			/// </summary>
			public const string Credit9S = "Credit9_S";

			/// <summary>
			/// Credit9_L 欄位名稱常數定義
			/// </summary>
			public const string Credit9L = "Credit9_L";

			/// <summary>
			/// Credit_Id10_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId10S = "Credit_Id10_S";

			/// <summary>
			/// Credit_Id10_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId10L = "Credit_Id10_L";

			/// <summary>
			/// Course_Id10_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId10S = "Course_Id10_S";

			/// <summary>
			/// Course_Id10_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId10L = "Course_Id10_L";

			/// <summary>
			/// Credit10_S 欄位名稱常數定義
			/// </summary>
			public const string Credit10S = "Credit10_S";

			/// <summary>
			/// Credit10_L 欄位名稱常數定義
			/// </summary>
			public const string Credit10L = "Credit10_L";

			/// <summary>
			/// Credit_Id11_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId11S = "Credit_Id11_S";

			/// <summary>
			/// Credit_Id11_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId11L = "Credit_Id11_L";

			/// <summary>
			/// Course_Id11_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId11S = "Course_Id11_S";

			/// <summary>
			/// Course_Id11_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId11L = "Course_Id11_L";

			/// <summary>
			/// Credit11_S 欄位名稱常數定義
			/// </summary>
			public const string Credit11S = "Credit11_S";

			/// <summary>
			/// Credit11_L 欄位名稱常數定義
			/// </summary>
			public const string Credit11L = "Credit11_L";

			/// <summary>
			/// Credit_Id12_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId12S = "Credit_Id12_S";

			/// <summary>
			/// Credit_Id12_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId12L = "Credit_Id12_L";

			/// <summary>
			/// Course_Id12_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId12S = "Course_Id12_S";

			/// <summary>
			/// Course_Id12_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId12L = "Course_Id12_L";

			/// <summary>
			/// Credit12_S 欄位名稱常數定義
			/// </summary>
			public const string Credit12S = "Credit12_S";

			/// <summary>
			/// Credit12_L 欄位名稱常數定義
			/// </summary>
			public const string Credit12L = "Credit12_L";

			/// <summary>
			/// Credit_Id13_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId13S = "Credit_Id13_S";

			/// <summary>
			/// Credit_Id13_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId13L = "Credit_Id13_L";

			/// <summary>
			/// Course_Id13_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId13S = "Course_Id13_S";

			/// <summary>
			/// Course_Id13_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId13L = "Course_Id13_L";

			/// <summary>
			/// Credit13_S 欄位名稱常數定義
			/// </summary>
			public const string Credit13S = "Credit13_S";

			/// <summary>
			/// Credit13_L 欄位名稱常數定義
			/// </summary>
			public const string Credit13L = "Credit13_L";

			/// <summary>
			/// Credit_Id14_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId14S = "Credit_Id14_S";

			/// <summary>
			/// Credit_Id14_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId14L = "Credit_Id14_L";

			/// <summary>
			/// Course_Id14_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId14S = "Course_Id14_S";

			/// <summary>
			/// Course_Id14_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId14L = "Course_Id14_L";

			/// <summary>
			/// Credit14_S 欄位名稱常數定義
			/// </summary>
			public const string Credit14S = "Credit14_S";

			/// <summary>
			/// Credit14_L 欄位名稱常數定義
			/// </summary>
			public const string Credit14L = "Credit14_L";

			/// <summary>
			/// Credit_Id15_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId15S = "Credit_Id15_S";

			/// <summary>
			/// Credit_Id15_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId15L = "Credit_Id15_L";

			/// <summary>
			/// Course_Id15_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId15S = "Course_Id15_S";

			/// <summary>
			/// Course_Id15_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId15L = "Course_Id15_L";

			/// <summary>
			/// Credit15_S 欄位名稱常數定義
			/// </summary>
			public const string Credit15S = "Credit15_S";

			/// <summary>
			/// Credit15_L 欄位名稱常數定義
			/// </summary>
			public const string Credit15L = "Credit15_L";

			/// <summary>
			/// Credit_Id16_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId16S = "Credit_Id16_S";

			/// <summary>
			/// Credit_Id16_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId16L = "Credit_Id16_L";

			/// <summary>
			/// Course_Id16_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId16S = "Course_Id16_S";

			/// <summary>
			/// Course_Id16_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId16L = "Course_Id16_L";

			/// <summary>
			/// Credit16_S 欄位名稱常數定義
			/// </summary>
			public const string Credit16S = "Credit16_S";

			/// <summary>
			/// Credit16_L 欄位名稱常數定義
			/// </summary>
			public const string Credit16L = "Credit16_L";

			/// <summary>
			/// Credit_Id17_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId17S = "Credit_Id17_S";

			/// <summary>
			/// Credit_Id17_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId17L = "Credit_Id17_L";

			/// <summary>
			/// Course_Id17_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId17S = "Course_Id17_S";

			/// <summary>
			/// Course_Id17_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId17L = "Course_Id17_L";

			/// <summary>
			/// Credit17_S 欄位名稱常數定義
			/// </summary>
			public const string Credit17S = "Credit17_S";

			/// <summary>
			/// Credit17_L 欄位名稱常數定義
			/// </summary>
			public const string Credit17L = "Credit17_L";

			/// <summary>
			/// Credit_Id18_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId18S = "Credit_Id18_S";

			/// <summary>
			/// Credit_Id18_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId18L = "Credit_Id18_L";

			/// <summary>
			/// Course_Id18_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId18S = "Course_Id18_S";

			/// <summary>
			/// Course_Id18_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId18L = "Course_Id18_L";

			/// <summary>
			/// Credit18_S 欄位名稱常數定義
			/// </summary>
			public const string Credit18S = "Credit18_S";

			/// <summary>
			/// Credit18_L 欄位名稱常數定義
			/// </summary>
			public const string Credit18L = "Credit18_L";

			/// <summary>
			/// Credit_Id19_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId19S = "Credit_Id19_S";

			/// <summary>
			/// Credit_Id19_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId19L = "Credit_Id19_L";

			/// <summary>
			/// Course_Id19_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId19S = "Course_Id19_S";

			/// <summary>
			/// Course_Id19_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId19L = "Course_Id19_L";

			/// <summary>
			/// Credit19_S 欄位名稱常數定義
			/// </summary>
			public const string Credit19S = "Credit19_S";

			/// <summary>
			/// Credit19_L 欄位名稱常數定義
			/// </summary>
			public const string Credit19L = "Credit19_L";

			/// <summary>
			/// Credit_Id20_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId20S = "Credit_Id20_S";

			/// <summary>
			/// Credit_Id20_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId20L = "Credit_Id20_L";

			/// <summary>
			/// Course_Id20_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId20S = "Course_Id20_S";

			/// <summary>
			/// Course_Id20_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId20L = "Course_Id20_L";

			/// <summary>
			/// Credit20_S 欄位名稱常數定義
			/// </summary>
			public const string Credit20S = "Credit20_S";

			/// <summary>
			/// Credit20_L 欄位名稱常數定義
			/// </summary>
			public const string Credit20L = "Credit20_L";

			/// <summary>
			/// Credit_Id21_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId21S = "Credit_Id21_S";

			/// <summary>
			/// Credit_Id21_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId21L = "Credit_Id21_L";

			/// <summary>
			/// Course_Id21_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId21S = "Course_Id21_S";

			/// <summary>
			/// Course_Id21_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId21L = "Course_Id21_L";

			/// <summary>
			/// Credit21_S 欄位名稱常數定義
			/// </summary>
			public const string Credit21S = "Credit21_S";

			/// <summary>
			/// Credit21_L 欄位名稱常數定義
			/// </summary>
			public const string Credit21L = "Credit21_L";

			/// <summary>
			/// Credit_Id22_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId22S = "Credit_Id22_S";

			/// <summary>
			/// Credit_Id22_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId22L = "Credit_Id22_L";

			/// <summary>
			/// Course_Id22_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId22S = "Course_Id22_S";

			/// <summary>
			/// Course_Id22_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId22L = "Course_Id22_L";

			/// <summary>
			/// Credit22_S 欄位名稱常數定義
			/// </summary>
			public const string Credit22S = "Credit22_S";

			/// <summary>
			/// Credit22_L 欄位名稱常數定義
			/// </summary>
			public const string Credit22L = "Credit22_L";

			/// <summary>
			/// Credit_Id23_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId23S = "Credit_Id23_S";

			/// <summary>
			/// Credit_Id23_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId23L = "Credit_Id23_L";

			/// <summary>
			/// Course_Id23_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId23S = "Course_Id23_S";

			/// <summary>
			/// Course_Id23_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId23L = "Course_Id23_L";

			/// <summary>
			/// Credit23_S 欄位名稱常數定義
			/// </summary>
			public const string Credit23S = "Credit23_S";

			/// <summary>
			/// Credit23_L 欄位名稱常數定義
			/// </summary>
			public const string Credit23L = "Credit23_L";

			/// <summary>
			/// Credit_Id24_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId24S = "Credit_Id24_S";

			/// <summary>
			/// Credit_Id24_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId24L = "Credit_Id24_L";

			/// <summary>
			/// Course_Id24_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId24S = "Course_Id24_S";

			/// <summary>
			/// Course_Id24_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId24L = "Course_Id24_L";

			/// <summary>
			/// Credit24_S 欄位名稱常數定義
			/// </summary>
			public const string Credit24S = "Credit24_S";

			/// <summary>
			/// Credit24_L 欄位名稱常數定義
			/// </summary>
			public const string Credit24L = "Credit24_L";

			/// <summary>
			/// Credit_Id25_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId25S = "Credit_Id25_S";

			/// <summary>
			/// Credit_Id25_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId25L = "Credit_Id25_L";

			/// <summary>
			/// Course_Id25_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId25S = "Course_Id25_S";

			/// <summary>
			/// Course_Id25_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId25L = "Course_Id25_L";

			/// <summary>
			/// Credit25_S 欄位名稱常數定義
			/// </summary>
			public const string Credit25S = "Credit25_S";

			/// <summary>
			/// Credit25_L 欄位名稱常數定義
			/// </summary>
			public const string Credit25L = "Credit25_L";

			/// <summary>
			/// Credit_Id26_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId26S = "Credit_Id26_S";

			/// <summary>
			/// Credit_Id26_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId26L = "Credit_Id26_L";

			/// <summary>
			/// Course_Id26_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId26S = "Course_Id26_S";

			/// <summary>
			/// Course_Id26_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId26L = "Course_Id26_L";

			/// <summary>
			/// Credit26_S 欄位名稱常數定義
			/// </summary>
			public const string Credit26S = "Credit26_S";

			/// <summary>
			/// Credit26_L 欄位名稱常數定義
			/// </summary>
			public const string Credit26L = "Credit26_L";

			/// <summary>
			/// Credit_Id27_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId27S = "Credit_Id27_S";

			/// <summary>
			/// Credit_Id27_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId27L = "Credit_Id27_L";

			/// <summary>
			/// Course_Id27_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId27S = "Course_Id27_S";

			/// <summary>
			/// Course_Id27_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId27L = "Course_Id27_L";

			/// <summary>
			/// Credit27_S 欄位名稱常數定義
			/// </summary>
			public const string Credit27S = "Credit27_S";

			/// <summary>
			/// Credit27_L 欄位名稱常數定義
			/// </summary>
			public const string Credit27L = "Credit27_L";

			/// <summary>
			/// Credit_Id28_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId28S = "Credit_Id28_S";

			/// <summary>
			/// Credit_Id28_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId28L = "Credit_Id28_L";

			/// <summary>
			/// Course_Id28_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId28S = "Course_Id28_S";

			/// <summary>
			/// Course_Id28_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId28L = "Course_Id28_L";

			/// <summary>
			/// Credit28_S 欄位名稱常數定義
			/// </summary>
			public const string Credit28S = "Credit28_S";

			/// <summary>
			/// Credit28_L 欄位名稱常數定義
			/// </summary>
			public const string Credit28L = "Credit28_L";

			/// <summary>
			/// Credit_Id29_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId29S = "Credit_Id29_S";

			/// <summary>
			/// Credit_Id29_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId29L = "Credit_Id29_L";

			/// <summary>
			/// Course_Id29_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId29S = "Course_Id29_S";

			/// <summary>
			/// Course_Id29_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId29L = "Course_Id29_L";

			/// <summary>
			/// Credit29_S 欄位名稱常數定義
			/// </summary>
			public const string Credit29S = "Credit29_S";

			/// <summary>
			/// Credit29_L 欄位名稱常數定義
			/// </summary>
			public const string Credit29L = "Credit29_L";

			/// <summary>
			/// Credit_Id30_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId30S = "Credit_Id30_S";

			/// <summary>
			/// Credit_Id30_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId30L = "Credit_Id30_L";

			/// <summary>
			/// Course_Id30_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId30S = "Course_Id30_S";

			/// <summary>
			/// Course_Id30_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId30L = "Course_Id30_L";

			/// <summary>
			/// Credit30_S 欄位名稱常數定義
			/// </summary>
			public const string Credit30S = "Credit30_S";

			/// <summary>
			/// Credit30_L 欄位名稱常數定義
			/// </summary>
			public const string Credit30L = "Credit30_L";

			/// <summary>
			/// Credit_Id31_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId31S = "Credit_Id31_S";

			/// <summary>
			/// Credit_Id31_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId31L = "Credit_Id31_L";

			/// <summary>
			/// Course_Id31_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId31S = "Course_Id31_S";

			/// <summary>
			/// Course_Id31_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId31L = "Course_Id31_L";

			/// <summary>
			/// Credit31_S 欄位名稱常數定義
			/// </summary>
			public const string Credit31S = "Credit31_S";

			/// <summary>
			/// Credit31_L 欄位名稱常數定義
			/// </summary>
			public const string Credit31L = "Credit31_L";

			/// <summary>
			/// Credit_Id32_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId32S = "Credit_Id32_S";

			/// <summary>
			/// Credit_Id32_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId32L = "Credit_Id32_L";

			/// <summary>
			/// Course_Id32_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId32S = "Course_Id32_S";

			/// <summary>
			/// Course_Id32_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId32L = "Course_Id32_L";

			/// <summary>
			/// Credit32_S 欄位名稱常數定義
			/// </summary>
			public const string Credit32S = "Credit32_S";

			/// <summary>
			/// Credit32_L 欄位名稱常數定義
			/// </summary>
			public const string Credit32L = "Credit32_L";

			/// <summary>
			/// Credit_Id33_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId33S = "Credit_Id33_S";

			/// <summary>
			/// Credit_Id33_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId33L = "Credit_Id33_L";

			/// <summary>
			/// Course_Id33_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId33S = "Course_Id33_S";

			/// <summary>
			/// Course_Id33_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId33L = "Course_Id33_L";

			/// <summary>
			/// Credit33_S 欄位名稱常數定義
			/// </summary>
			public const string Credit33S = "Credit33_S";

			/// <summary>
			/// Credit33_L 欄位名稱常數定義
			/// </summary>
			public const string Credit33L = "Credit33_L";

			/// <summary>
			/// Credit_Id34_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId34S = "Credit_Id34_S";

			/// <summary>
			/// Credit_Id34_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId34L = "Credit_Id34_L";

			/// <summary>
			/// Course_Id34_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId34S = "Course_Id34_S";

			/// <summary>
			/// Course_Id34_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId34L = "Course_Id34_L";

			/// <summary>
			/// Credit34_S 欄位名稱常數定義
			/// </summary>
			public const string Credit34S = "Credit34_S";

			/// <summary>
			/// Credit34_L 欄位名稱常數定義
			/// </summary>
			public const string Credit34L = "Credit34_L";

			/// <summary>
			/// Credit_Id35_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId35S = "Credit_Id35_S";

			/// <summary>
			/// Credit_Id35_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId35L = "Credit_Id35_L";

			/// <summary>
			/// Course_Id35_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId35S = "Course_Id35_S";

			/// <summary>
			/// Course_Id35_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId35L = "Course_Id35_L";

			/// <summary>
			/// Credit35_S 欄位名稱常數定義
			/// </summary>
			public const string Credit35S = "Credit35_S";

			/// <summary>
			/// Credit35_L 欄位名稱常數定義
			/// </summary>
			public const string Credit35L = "Credit35_L";

			/// <summary>
			/// Credit_Id36_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId36S = "Credit_Id36_S";

			/// <summary>
			/// Credit_Id36_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId36L = "Credit_Id36_L";

			/// <summary>
			/// Course_Id36_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId36S = "Course_Id36_S";

			/// <summary>
			/// Course_Id36_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId36L = "Course_Id36_L";

			/// <summary>
			/// Credit36_S 欄位名稱常數定義
			/// </summary>
			public const string Credit36S = "Credit36_S";

			/// <summary>
			/// Credit36_L 欄位名稱常數定義
			/// </summary>
			public const string Credit36L = "Credit36_L";

			/// <summary>
			/// Credit_Id37_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId37S = "Credit_Id37_S";

			/// <summary>
			/// Credit_Id37_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId37L = "Credit_Id37_L";

			/// <summary>
			/// Course_Id37_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId37S = "Course_Id37_S";

			/// <summary>
			/// Course_Id37_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId37L = "Course_Id37_L";

			/// <summary>
			/// Credit37_S 欄位名稱常數定義
			/// </summary>
			public const string Credit37S = "Credit37_S";

			/// <summary>
			/// Credit37_L 欄位名稱常數定義
			/// </summary>
			public const string Credit37L = "Credit37_L";

			/// <summary>
			/// Credit_Id38_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId38S = "Credit_Id38_S";

			/// <summary>
			/// Credit_Id38_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId38L = "Credit_Id38_L";

			/// <summary>
			/// Course_Id38_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId38S = "Course_Id38_S";

			/// <summary>
			/// Course_Id38_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId38L = "Course_Id38_L";

			/// <summary>
			/// Credit38_S 欄位名稱常數定義
			/// </summary>
			public const string Credit38S = "Credit38_S";

			/// <summary>
			/// Credit38_L 欄位名稱常數定義
			/// </summary>
			public const string Credit38L = "Credit38_L";

			/// <summary>
			/// Credit_Id39_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId39S = "Credit_Id39_S";

			/// <summary>
			/// Credit_Id39_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId39L = "Credit_Id39_L";

			/// <summary>
			/// Course_Id39_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId39S = "Course_Id39_S";

			/// <summary>
			/// Course_Id39_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId39L = "Course_Id39_L";

			/// <summary>
			/// Credit39_S 欄位名稱常數定義
			/// </summary>
			public const string Credit39S = "Credit39_S";

			/// <summary>
			/// Credit39_L 欄位名稱常數定義
			/// </summary>
			public const string Credit39L = "Credit39_L";

			/// <summary>
			/// Credit_Id40_S 欄位名稱常數定義
			/// </summary>
			public const string CreditId40S = "Credit_Id40_S";

			/// <summary>
			/// Credit_Id40_L 欄位名稱常數定義
			/// </summary>
			public const string CreditId40L = "Credit_Id40_L";

			/// <summary>
			/// Course_Id40_S 欄位名稱常數定義
			/// </summary>
			public const string CourseId40S = "Course_Id40_S";

			/// <summary>
			/// Course_Id40_L 欄位名稱常數定義
			/// </summary>
			public const string CourseId40L = "Course_Id40_L";

			/// <summary>
			/// Credit40_S 欄位名稱常數定義
			/// </summary>
			public const string Credit40S = "Credit40_S";

			/// <summary>
			/// Credit40_L 欄位名稱常數定義
			/// </summary>
			public const string Credit40L = "Credit40_L";
			#endregion

			#region 轉帳資料對照欄位 (StudentReceiveEntity)
			/// <summary>
			/// 扣款轉帳銀行代碼 欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeductBankidS = "Deduct_BankID_S";

			/// <summary>
			/// 扣款轉帳銀行代碼 欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeductBankidL = "Deduct_BankID_L";

			/// <summary>
			/// 扣款轉帳銀行帳號 欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeductAccountnoS = "Deduct_AccountNo_S";

			/// <summary>
			/// 扣款轉帳銀行帳號 欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeductAccountnoL = "Deduct_AccountNo_L";

			/// <summary>
			/// 扣款轉帳銀行帳號戶名 欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeductAccountnameS = "Deduct_AccountName_S";

			/// <summary>
			/// 扣款轉帳銀行帳號戶名 欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeductAccountnameL = "Deduct_AccountName_L";

			/// <summary>
			/// 扣款轉帳銀行帳戶ＩＤ 欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeductAccountidS = "Deduct_AccountId_S";

			/// <summary>
			/// 扣款轉帳銀行帳戶ＩＤ 欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeductAccountidL = "Deduct_AccountId_L";
            #endregion

            #region 備註對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 備註01欄位起始位置
            /// </summary>
            public const string Memo01S = "Memo01_S";

            /// <summary>
            /// 備註01欄位字元長度
            /// </summary>
            public const string Memo01L = "Memo01_L";

            /// <summary>
            /// 備註02欄位起始位置
            /// </summary>
            public const string Memo02S = "Memo02_S";

            /// <summary>
            /// 備註02欄位字元長度
            /// </summary>
            public const string Memo02L = "Memo02_L";

            /// <summary>
            /// 備註03欄位起始位置
            /// </summary>
            public const string Memo03S = "Memo03_S";

            /// <summary>
            /// 備註03欄位字元長度
            /// </summary>
            public const string Memo03L = "Memo03_L";

            /// <summary>
            /// 備註04欄位起始位置
            /// </summary>
            public const string Memo04S = "Memo04_S";

            /// <summary>
            /// 備註04欄位字元長度
            /// </summary>
            public const string Memo04L = "Memo04_L";

            /// <summary>
            /// 備註05欄位起始位置
            /// </summary>
            public const string Memo05S = "Memo05_S";

            /// <summary>
            /// 備註05欄位字元長度
            /// </summary>
            public const string Memo05L = "Memo05_L";

            /// <summary>
            /// 備註06欄位起始位置
            /// </summary>
            public const string Memo06S = "Memo06_S";

            /// <summary>
            /// 備註06欄位字元長度
            /// </summary>
            public const string Memo06L = "Memo06_L";

            /// <summary>
            /// 備註07欄位起始位置
            /// </summary>
            public const string Memo07S = "Memo07_S";

            /// <summary>
            /// 備註07欄位字元長度
            /// </summary>
            public const string Memo07L = "Memo07_L";

            /// <summary>
            /// 備註08欄位起始位置
            /// </summary>
            public const string Memo08S = "Memo08_S";

            /// <summary>
            /// 備註08欄位字元長度
            /// </summary>
            public const string Memo08L = "Memo08_L";

            /// <summary>
            /// 備註09欄位起始位置
            /// </summary>
            public const string Memo09S = "Memo09_S";

            /// <summary>
            /// 備註09欄位字元長度
            /// </summary>
            public const string Memo09L = "Memo09_L";

            /// <summary>
            /// 備註10欄位起始位置
            /// </summary>
            public const string Memo10S = "Memo10_S";

            /// <summary>
            /// 備註10欄位字元長度
            /// </summary>
            public const string Memo10L = "Memo10_L";

            /// <summary>
            /// 備註11欄位起始位置
            /// </summary>
            public const string Memo11S = "Memo11_S";

            /// <summary>
            /// 備註11欄位字元長度
            /// </summary>
            public const string Memo11L = "Memo11_L";

            /// <summary>
            /// 備註12欄位起始位置
            /// </summary>
            public const string Memo12S = "Memo12_S";

            /// <summary>
            /// 備註12欄位字元長度
            /// </summary>
            public const string Memo12L = "Memo12_L";

            /// <summary>
            /// 備註13欄位起始位置
            /// </summary>
            public const string Memo13S = "Memo13_S";

            /// <summary>
            /// 備註13欄位字元長度
            /// </summary>
            public const string Memo13L = "Memo13_L";

            /// <summary>
            /// 備註14欄位起始位置
            /// </summary>
            public const string Memo14S = "Memo14_S";

            /// <summary>
            /// 備註14欄位字元長度
            /// </summary>
            public const string Memo14L = "Memo14_L";

            /// <summary>
            /// 備註15欄位起始位置
            /// </summary>
            public const string Memo15S = "Memo15_S";

            /// <summary>
            /// 備註15欄位字元長度
            /// </summary>
            public const string Memo15L = "Memo15_L";

            /// <summary>
            /// 備註16欄位起始位置
            /// </summary>
            public const string Memo16S = "Memo16_S";

            /// <summary>
            /// 備註16欄位字元長度
            /// </summary>
            public const string Memo16L = "Memo16_L";

            /// <summary>
            /// 備註17欄位起始位置
            /// </summary>
            public const string Memo17S = "Memo17_S";

            /// <summary>
            /// 備註17欄位字元長度
            /// </summary>
            public const string Memo17L = "Memo17_L";

            /// <summary>
            /// 備註18欄位起始位置
            /// </summary>
            public const string Memo18S = "Memo18_S";

            /// <summary>
            /// 備註18欄位字元長度
            /// </summary>
            public const string Memo18L = "Memo18_L";

            /// <summary>
            /// 備註19欄位起始位置
            /// </summary>
            public const string Memo19S = "Memo19_S";

            /// <summary>
            /// 備註19欄位字元長度
            /// </summary>
            public const string Memo19L = "Memo19_L";

            /// <summary>
            /// 備註20欄位起始位置
            /// </summary>
            public const string Memo20S = "Memo20_S";

            /// <summary>
            /// 備註20欄位字元長度
            /// </summary>
            public const string Memo20L = "Memo20_L";

            /// <summary>
            /// 備註21欄位起始位置
            /// </summary>
            public const string Memo21S = "Memo21_S";

            /// <summary>
            /// 備註21欄位字元長度
            /// </summary>
            public const string Memo21L = "Memo21_L";
			#endregion

			#region [MDY:202203XX] 2022擴充案 英文名稱相關對照欄位
			#region 學籍資料英文名稱對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
			/// <summary>
			/// 部別英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DeptENameS = "Dept_EName_S";

			/// <summary>
			/// 部別英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DeptENameL = "Dept_EName_L";

			/// <summary>
			/// 院別英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string CollegeENameS = "College_EName_S";

			/// <summary>
			/// 院別英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string CollegeENameL = "College_EName_L";

			/// <summary>
			/// 系所英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string MajorENameS = "Major_EName_S";

			/// <summary>
			/// 系所英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string MajorENameL = "Major_EName_L";

			/// <summary>
			/// 班別英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ClassENameS = "Class_EName_S";

			/// <summary>
			/// 班別英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ClassENameL = "Class_EName_L";
			#endregion

			#region 減免、就貸、住宿英文名稱對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
			/// <summary>
			/// 減免英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string ReduceENameS = "Reduce_EName_S";

			/// <summary>
			/// 減免英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string ReduceENameL = "Reduce_EName_L";

			/// <summary>
			/// 住宿英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string DormENameS = "Dorm_EName_S";

			/// <summary>
			/// 住宿英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string DormENameL = "Dorm_EName_L";

			/// <summary>
			/// 就貸英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string LoanENameS = "Loan_EName_S";

			/// <summary>
			/// 就貸英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string LoanENameL = "Loan_EName_L";
			#endregion

			#region 身分註記英文名稱對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
			/// <summary>
			/// 身份註記01英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName1S = "Identify_EName1_S";

			/// <summary>
			/// 身份註記01英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName1L = "Identify_EName1_L";

			/// <summary>
			/// 身份註記02英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName2S = "Identify_EName2_S";

			/// <summary>
			/// 身份註記02英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName2L = "Identify_EName2_L";

			/// <summary>
			/// 身份註記03英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName3S = "Identify_EName3_S";

			/// <summary>
			/// 身份註記03英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName3L = "Identify_EName3_L";

			/// <summary>
			/// 身份註記04英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName4S = "Identify_EName4_S";

			/// <summary>
			/// 身份註記04英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName4L = "Identify_EName4_L";

			/// <summary>
			/// 身份註記05英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName5S = "Identify_EName5_S";

			/// <summary>
			/// 身份註記05英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName5L = "Identify_EName5_L";

			/// <summary>
			/// 身份註記06英文名稱欄位起始位置 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName6S = "Identify_EName6_S";

			/// <summary>
			/// 身份註記06英文名稱欄位字元長度 欄位名稱常數定義
			/// </summary>
			public const string IdentifyEName6L = "Identify_EName6_L";
			#endregion
			#endregion
			#endregion

			#region 狀態相關欄位
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
		/// MappingreTxtEntity 類別建構式
		/// </summary>
		public MappingreTxtEntity()
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
        /// 所屬年別代號 (土銀不使用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.Char, 3, true)]
		public string YearId
		{
			get;
			set;
		}

		/// <summary>
        /// 所屬期別代號 (土銀不使用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, true)]
		public string TermId
		{
			get;
			set;
		}

		/// <summary>
        /// 所屬部別代碼 (土銀不使用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, true)]
		public string DepId
		{
			get;
			set;
		}

		/// <summary>
        /// 所屬代收費用別 (土銀不使用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.Char, 1, true)]
		public string ReceiveId
		{
			get;
			set;
		}
        #endregion

        #region 上傳資料相關欄位
        #region 學生基本資料對照欄位 (StudentMasterEntity)
        /// <summary>
        /// 學號欄位起始位置
		/// </summary>
		[FieldSpec(Field.StuIdS, false, FieldTypeEnum.Integer, true)]
		public int? StuIdS
		{
			get;
			set;
		}

		/// <summary>
        /// 學號欄位字元長度
		/// </summary>
		[FieldSpec(Field.StuIdL, false, FieldTypeEnum.Integer, true)]
		public int? StuIdL
		{
			get;
			set;
		}

		/// <summary>
        /// 學生姓名欄位起始位置
		/// </summary>
		[FieldSpec(Field.StuNameS, false, FieldTypeEnum.Integer, true)]
		public int? StuNameS
		{
			get;
			set;
		}

		/// <summary>
        /// 學生姓名欄位字元長度
		/// </summary>
		[FieldSpec(Field.StuNameL, false, FieldTypeEnum.Integer, true)]
		public int? StuNameL
		{
			get;
			set;
		}

		/// <summary>
        /// 學生生日欄位起始位置
		/// </summary>
		[FieldSpec(Field.StuBirthdayS, false, FieldTypeEnum.Integer, true)]
		public int? StuBirthdayS
		{
			get;
			set;
		}

		/// <summary>
        /// 學生生日欄位字元長度
		/// </summary>
		[FieldSpec(Field.StuBirthdayL, false, FieldTypeEnum.Integer, true)]
		public int? StuBirthdayL
		{
			get;
			set;
		}

		/// <summary>
        /// 學生身份證欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdNumberS, false, FieldTypeEnum.Integer, true)]
		public int? IdNumberS
		{
			get;
			set;
		}

		/// <summary>
        /// 學生身份證欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdNumberL, false, FieldTypeEnum.Integer, true)]
		public int? IdNumberL
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Tel_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuTelS, false, FieldTypeEnum.Integer, true)]
		public int? StuTelS
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Tel_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuTelL, false, FieldTypeEnum.Integer, true)]
		public int? StuTelL
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Addcode_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuAddcodeS, false, FieldTypeEnum.Integer, true)]
		public int? StuAddcodeS
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Addcode_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuAddcodeL, false, FieldTypeEnum.Integer, true)]
		public int? StuAddcodeL
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Add_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuAddS, false, FieldTypeEnum.Integer, true)]
		public int? StuAddS
		{
			get;
			set;
		}

		/// <summary>
		/// Stu_Add_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.StuAddL, false, FieldTypeEnum.Integer, true)]
		public int? StuAddL
		{
			get;
			set;
		}

        /// <summary>
        /// Email_S 欄位屬性
        /// </summary>
        [FieldSpec(Field.EmailS, false, FieldTypeEnum.Integer, true)]
        public int? EmailS
        {
            get;
            set;
        }

        /// <summary>
        /// Email_L 欄位屬性
        /// </summary>
        [FieldSpec(Field.EmailL, false, FieldTypeEnum.Integer, true)]
        public int? EmailL
        {
            get;
            set;
        }

        /// <summary>
        /// Stu_Parent_S 欄位屬性
        /// </summary>
        [FieldSpec(Field.StuParentS, false, FieldTypeEnum.Integer, true)]
        public int? StuParentS
        {
            get;
            set;
        }

        /// <summary>
        /// Stu_Parent_L 欄位屬性
        /// </summary>
        [FieldSpec(Field.StuParentL, false, FieldTypeEnum.Integer, true)]
        public int? StuParentL
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20160131] 增加資料序號與繳款期限對照欄位 (StudentReceiveEntity)
        /// <summary>
        /// 資料序號欄位起始位置
        /// </summary>
        [FieldSpec(Field.OldSeqS, false, FieldTypeEnum.Integer, true)]
        public int? OldSeqS
        {
            get;
            set;
        }

        /// <summary>
        /// 資料序號欄位字元長度
        /// </summary>
        [FieldSpec(Field.OldSeqL, false, FieldTypeEnum.Integer, true)]
        public int? OldSeqL
        {
            get;
            set;
        }

        /// <summary>
        /// 繳款期限欄位起始位置
        /// </summary>
        [FieldSpec(Field.PayDueDateS, false, FieldTypeEnum.Integer, true)]
        public int? PayDueDateS
        {
            get;
            set;
        }

        /// <summary>
        /// 繳款期限欄位字元長度
        /// </summary>
        [FieldSpec(Field.PayDueDateL, false, FieldTypeEnum.Integer, true)]
        public int? PayDueDateL
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位 (StudentReceiveEntity)
        /// <summary>
        /// 是否啟用國際信用卡繳費旗標欄位起始位置
        /// </summary>
        [FieldSpec(Field.NCCardFlagS, false, FieldTypeEnum.Integer, true)]
        public int? NCCardFlagS
        {
            get;
            set;
        }

        /// <summary>
        /// 是否啟用國際信用卡繳費旗標欄位字元長度
        /// </summary>
        [FieldSpec(Field.NCCardFlagL, false, FieldTypeEnum.Integer, true)]
        public int? NCCardFlagL
        {
            get;
            set;
        }
        #endregion

        #region 學籍資料對照欄位 (StudentReceiveEntity + ClassListEntity + DeptListEntity + CollegeListEntity + MajorListEntity)
        /// <summary>
        /// 學年欄位起始位置
		/// </summary>
		[FieldSpec(Field.StuGradeS, false, FieldTypeEnum.Integer, true)]
		public int? StuGradeS
		{
			get;
			set;
		}

		/// <summary>
        /// 學年欄位字元長度
		/// </summary>
		[FieldSpec(Field.StuGradeL, false, FieldTypeEnum.Integer, true)]
		public int? StuGradeL
		{
			get;
			set;
		}

        /// <summary>
        /// 部別代碼欄位起始位置
        /// </summary>
        [FieldSpec(Field.DeptIdS, false, FieldTypeEnum.Integer, true)]
        public int? DeptIdS
        {
            get;
            set;
        }

        /// <summary>
        /// 部別代碼欄位字元長度
        /// </summary>
        [FieldSpec(Field.DeptIdL, false, FieldTypeEnum.Integer, true)]
        public int? DeptIdL
        {
            get;
            set;
        }

		/// <summary>
		/// 部別名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.DeptNameS, false, FieldTypeEnum.Integer, true)]
		public int? DeptNameS
        {
            get;
            set;
        }

		/// <summary>
		/// 部別名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.DeptNameL, false, FieldTypeEnum.Integer, true)]
		public int? DeptNameL
        {
            get;
            set;
        }

		/// <summary>
        /// 院別代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.CollegeIdS, false, FieldTypeEnum.Integer, true)]
		public int? CollegeIdS
		{
			get;
			set;
		}

		/// <summary>
		/// 院別代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.CollegeIdL, false, FieldTypeEnum.Integer, true)]
		public int? CollegeIdL
		{
			get;
			set;
		}

		/// <summary>
        /// 院別名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.CollegeNameS, false, FieldTypeEnum.Integer, true)]
		public int? CollegeNameS
		{
			get;
			set;
		}

		/// <summary>
		/// 院別名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.CollegeNameL, false, FieldTypeEnum.Integer, true)]
		public int? CollegeNameL
		{
			get;
			set;
		}

		/// <summary>
        /// 系所代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.MajorIdS, false, FieldTypeEnum.Integer, true)]
		public int? MajorIdS
		{
			get;
			set;
		}

		/// <summary>
		/// 系所代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.MajorIdL, false, FieldTypeEnum.Integer, true)]
		public int? MajorIdL
		{
			get;
			set;
		}

		/// <summary>
        /// 系所名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.MajorNameS, false, FieldTypeEnum.Integer, true)]
		public int? MajorNameS
		{
			get;
			set;
		}

		/// <summary>
		/// 系所名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.MajorNameL, false, FieldTypeEnum.Integer, true)]
		public int? MajorNameL
		{
			get;
			set;
		}

		/// <summary>
        /// 班別代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.ClassIdS, false, FieldTypeEnum.Integer, true)]
		public int? ClassIdS
		{
			get;
			set;
		}

		/// <summary>
		/// 班別代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.ClassIdL, false, FieldTypeEnum.Integer, true)]
		public int? ClassIdL
		{
			get;
			set;
		}

		/// <summary>
		/// 班別名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.ClassNameS, false, FieldTypeEnum.Integer, true)]
		public int? ClassNameS
		{
			get;
			set;
		}

		/// <summary>
		/// 班別名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.ClassNameL, false, FieldTypeEnum.Integer, true)]
		public int? ClassNameL
		{
			get;
			set;
		}

		/// <summary>
		/// 總學分數欄位起始位置
		/// </summary>
		[FieldSpec(Field.StuCreditS, false, FieldTypeEnum.Integer, true)]
		public int? StuCreditS
		{
			get;
			set;
		}

		/// <summary>
		/// 總學分數欄位字元長度
		/// </summary>
		[FieldSpec(Field.StuCreditL, false, FieldTypeEnum.Integer, true)]
		public int? StuCreditL
		{
			get;
			set;
		}

		/// <summary>
		/// 上課時數欄位起始位置
		/// </summary>
		[FieldSpec(Field.StuHourS, false, FieldTypeEnum.Integer, true)]
		public int? StuHourS
		{
			get;
			set;
		}

		/// <summary>
		/// 上課時數欄位字元長度
		/// </summary>
		[FieldSpec(Field.StuHourL, false, FieldTypeEnum.Integer, true)]
		public int? StuHourL
		{
			get;
			set;
		}
		#endregion

		#region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
		/// <summary>
		/// 減免代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.ReduceIdS, false, FieldTypeEnum.Integer, true)]
		public int? ReduceIdS
		{
			get;
			set;
		}

		/// <summary>
		/// 減免代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.ReduceIdL, false, FieldTypeEnum.Integer, true)]
		public int? ReduceIdL
		{
			get;
			set;
		}

		/// <summary>
		/// 減免名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.ReduceNameS, false, FieldTypeEnum.Integer, true)]
		public int? ReduceNameS
		{
			get;
			set;
		}

		/// <summary>
		/// 減免名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.ReduceNameL, false, FieldTypeEnum.Integer, true)]
		public int? ReduceNameL
		{
			get;
			set;
		}

		/// <summary>
		/// 住宿代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.DormIdS, false, FieldTypeEnum.Integer, true)]
		public int? DormIdS
		{
			get;
			set;
		}

		/// <summary>
		/// 住宿代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.DormIdL, false, FieldTypeEnum.Integer, true)]
		public int? DormIdL
		{
			get;
			set;
		}

		/// <summary>
		/// 住宿名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.DormNameS, false, FieldTypeEnum.Integer, true)]
		public int? DormNameS
		{
			get;
			set;
		}

		/// <summary>
		/// 住宿名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.DormNameL, false, FieldTypeEnum.Integer, true)]
		public int? DormNameL
		{
			get;
			set;
		}

		/// <summary>
		/// 就貸代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.LoanIdS, false, FieldTypeEnum.Integer, true)]
		public int? LoanIdS
		{
			get;
			set;
		}

		/// <summary>
		/// 就貸代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.LoanIdL, false, FieldTypeEnum.Integer, true)]
		public int? LoanIdL
		{
			get;
			set;
		}

		/// <summary>
		/// 就貸名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.LoanNameS, false, FieldTypeEnum.Integer, true)]
		public int? LoanNameS
		{
			get;
			set;
		}

		/// <summary>
		/// 就貸名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.LoanNameL, false, FieldTypeEnum.Integer, true)]
		public int? LoanNameL
		{
			get;
			set;
		}
		#endregion

		#region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
		/// <summary>
		/// 身份註記01代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyId1S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId1S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記01代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyId1L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId1L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記01名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyName1S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName1S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記01名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyName1L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName1L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記02代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyId2S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId2S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記02代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyId2L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId2L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記02名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyName2S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName2S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記02名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyName2L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName2L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記03代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyId3S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId3S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記03代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyId3L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId3L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記03名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyName3S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName3S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記03名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyName3L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName3L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記04代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyId4S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId4S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記04代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyId4L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId4L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記04名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyName4S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName4S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記04名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyName4L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName4L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記05代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyId5S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId5S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記05代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyId5L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId5L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記05名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyName5S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName5S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記05名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyName5L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName5L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記06代碼欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyId6S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId6S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記06代碼欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyId6L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyId6L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記06名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyName6S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName6S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記06名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyName6L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyName6L
		{
			get;
			set;
		}
		#endregion

        #region 繳費資料對照欄位 (StudentReceiveEntity)
        /// <summary>
		/// Receive_1_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive1S, false, FieldTypeEnum.Integer, true)]
		public int? Receive1S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_1_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive1L, false, FieldTypeEnum.Integer, true)]
		public int? Receive1L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_2_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive2S, false, FieldTypeEnum.Integer, true)]
		public int? Receive2S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_2_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive2L, false, FieldTypeEnum.Integer, true)]
		public int? Receive2L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_3_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive3S, false, FieldTypeEnum.Integer, true)]
		public int? Receive3S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_3_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive3L, false, FieldTypeEnum.Integer, true)]
		public int? Receive3L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_4_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive4S, false, FieldTypeEnum.Integer, true)]
		public int? Receive4S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_4_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive4L, false, FieldTypeEnum.Integer, true)]
		public int? Receive4L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_5_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive5S, false, FieldTypeEnum.Integer, true)]
		public int? Receive5S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_5_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive5L, false, FieldTypeEnum.Integer, true)]
		public int? Receive5L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_6_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive6S, false, FieldTypeEnum.Integer, true)]
		public int? Receive6S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_6_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive6L, false, FieldTypeEnum.Integer, true)]
		public int? Receive6L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_7_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive7S, false, FieldTypeEnum.Integer, true)]
		public int? Receive7S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_7_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive7L, false, FieldTypeEnum.Integer, true)]
		public int? Receive7L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_8_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive8S, false, FieldTypeEnum.Integer, true)]
		public int? Receive8S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_8_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive8L, false, FieldTypeEnum.Integer, true)]
		public int? Receive8L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_9_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive9S, false, FieldTypeEnum.Integer, true)]
		public int? Receive9S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_9_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive9L, false, FieldTypeEnum.Integer, true)]
		public int? Receive9L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive10S, false, FieldTypeEnum.Integer, true)]
		public int? Receive10S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive10L, false, FieldTypeEnum.Integer, true)]
		public int? Receive10L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive11S, false, FieldTypeEnum.Integer, true)]
		public int? Receive11S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive11L, false, FieldTypeEnum.Integer, true)]
		public int? Receive11L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive12S, false, FieldTypeEnum.Integer, true)]
		public int? Receive12S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive12L, false, FieldTypeEnum.Integer, true)]
		public int? Receive12L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive13S, false, FieldTypeEnum.Integer, true)]
		public int? Receive13S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive13L, false, FieldTypeEnum.Integer, true)]
		public int? Receive13L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive14S, false, FieldTypeEnum.Integer, true)]
		public int? Receive14S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive14L, false, FieldTypeEnum.Integer, true)]
		public int? Receive14L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive15S, false, FieldTypeEnum.Integer, true)]
		public int? Receive15S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive15L, false, FieldTypeEnum.Integer, true)]
		public int? Receive15L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive16S, false, FieldTypeEnum.Integer, true)]
		public int? Receive16S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive16L, false, FieldTypeEnum.Integer, true)]
		public int? Receive16L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive17S, false, FieldTypeEnum.Integer, true)]
		public int? Receive17S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive17L, false, FieldTypeEnum.Integer, true)]
		public int? Receive17L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive18S, false, FieldTypeEnum.Integer, true)]
		public int? Receive18S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive18L, false, FieldTypeEnum.Integer, true)]
		public int? Receive18L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive19S, false, FieldTypeEnum.Integer, true)]
		public int? Receive19S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive19L, false, FieldTypeEnum.Integer, true)]
		public int? Receive19L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive20S, false, FieldTypeEnum.Integer, true)]
		public int? Receive20S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive20L, false, FieldTypeEnum.Integer, true)]
		public int? Receive20L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive21S, false, FieldTypeEnum.Integer, true)]
		public int? Receive21S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive21L, false, FieldTypeEnum.Integer, true)]
		public int? Receive21L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive22S, false, FieldTypeEnum.Integer, true)]
		public int? Receive22S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive22L, false, FieldTypeEnum.Integer, true)]
		public int? Receive22L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive23S, false, FieldTypeEnum.Integer, true)]
		public int? Receive23S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive23L, false, FieldTypeEnum.Integer, true)]
		public int? Receive23L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive24S, false, FieldTypeEnum.Integer, true)]
		public int? Receive24S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive24L, false, FieldTypeEnum.Integer, true)]
		public int? Receive24L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive25S, false, FieldTypeEnum.Integer, true)]
		public int? Receive25S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive25L, false, FieldTypeEnum.Integer, true)]
		public int? Receive25L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive26S, false, FieldTypeEnum.Integer, true)]
		public int? Receive26S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive26L, false, FieldTypeEnum.Integer, true)]
		public int? Receive26L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive27S, false, FieldTypeEnum.Integer, true)]
		public int? Receive27S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive27L, false, FieldTypeEnum.Integer, true)]
		public int? Receive27L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive28S, false, FieldTypeEnum.Integer, true)]
		public int? Receive28S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive28L, false, FieldTypeEnum.Integer, true)]
		public int? Receive28L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive29S, false, FieldTypeEnum.Integer, true)]
		public int? Receive29S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive29L, false, FieldTypeEnum.Integer, true)]
		public int? Receive29L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive30S, false, FieldTypeEnum.Integer, true)]
		public int? Receive30S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive30L, false, FieldTypeEnum.Integer, true)]
		public int? Receive30L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive31S, false, FieldTypeEnum.Integer, true)]
		public int? Receive31S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive31L, false, FieldTypeEnum.Integer, true)]
		public int? Receive31L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive32S, false, FieldTypeEnum.Integer, true)]
		public int? Receive32S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive32L, false, FieldTypeEnum.Integer, true)]
		public int? Receive32L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive33S, false, FieldTypeEnum.Integer, true)]
		public int? Receive33S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive33L, false, FieldTypeEnum.Integer, true)]
		public int? Receive33L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive34S, false, FieldTypeEnum.Integer, true)]
		public int? Receive34S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive34L, false, FieldTypeEnum.Integer, true)]
		public int? Receive34L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive35S, false, FieldTypeEnum.Integer, true)]
		public int? Receive35S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive35L, false, FieldTypeEnum.Integer, true)]
		public int? Receive35L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive36S, false, FieldTypeEnum.Integer, true)]
		public int? Receive36S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive36L, false, FieldTypeEnum.Integer, true)]
		public int? Receive36L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive37S, false, FieldTypeEnum.Integer, true)]
		public int? Receive37S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive37L, false, FieldTypeEnum.Integer, true)]
		public int? Receive37L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive38S, false, FieldTypeEnum.Integer, true)]
		public int? Receive38S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive38L, false, FieldTypeEnum.Integer, true)]
		public int? Receive38L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive39S, false, FieldTypeEnum.Integer, true)]
		public int? Receive39S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive39L, false, FieldTypeEnum.Integer, true)]
		public int? Receive39L
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive40S, false, FieldTypeEnum.Integer, true)]
		public int? Receive40S
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Receive40L, false, FieldTypeEnum.Integer, true)]
		public int? Receive40L
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_Amount_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.LoanAmountS, false, FieldTypeEnum.Integer, true)]
		public int? LoanAmountS
		{
			get;
			set;
		}

		/// <summary>
		/// Loan_Amount_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.LoanAmountL, false, FieldTypeEnum.Integer, true)]
		public int? LoanAmountL
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_Amount_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReceiveAmountS, false, FieldTypeEnum.Integer, true)]
		public int? ReceiveAmountS
		{
			get;
			set;
		}

		/// <summary>
		/// Receive_Amount_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.ReceiveAmountL, false, FieldTypeEnum.Integer, true)]
		public int? ReceiveAmountL
		{
			get;
			set;
		}

        /// <summary>
        /// Stu_Hid_S 欄位屬性
        /// </summary>
        [FieldSpec(Field.StuHidS, false, FieldTypeEnum.Integer, true)]
        public int? StuHidS
        {
            get;
            set;
        }

        /// <summary>
        /// Stu_Hid_L 欄位屬性
        /// </summary>
        [FieldSpec(Field.StuHidL, false, FieldTypeEnum.Integer, true)]
        public int? StuHidL
        {
            get;
            set;
        }

        /// <summary>
        /// Remark_S 欄位屬性
        /// </summary>
        [FieldSpec(Field.RemarkS, false, FieldTypeEnum.Integer, true)]
        public int? RemarkS
        {
            get;
            set;
        }

        /// <summary>
        /// Remark_L 欄位屬性
        /// </summary>
        [FieldSpec(Field.RemarkL, false, FieldTypeEnum.Integer, true)]
        public int? RemarkL
        {
            get;
            set;
        }

        /// <summary>
        /// Serior_No_S 欄位屬性
        /// </summary>
        [FieldSpec(Field.SeriorNoS, false, FieldTypeEnum.Integer, true)]
        public int? SeriorNoS
        {
            get;
            set;
        }

        /// <summary>
        /// Serior_No_L 欄位屬性
        /// </summary>
        [FieldSpec(Field.SeriorNoL, false, FieldTypeEnum.Integer, true)]
        public int? SeriorNoL
        {
            get;
            set;
        }

        /// <summary>
        /// Cancel_No_S 欄位屬性
        /// </summary>
        [FieldSpec(Field.CancelNoS, false, FieldTypeEnum.Integer, true)]
        public int? CancelNoS
        {
            get;
            set;
        }

        /// <summary>
        /// Cancel_No_L 欄位屬性
        /// </summary>
        [FieldSpec(Field.CancelNoL, false, FieldTypeEnum.Integer, true)]
        public int? CancelNoL
        {
            get;
            set;
        }
        #endregion

        #region 其他繳費資料對照欄位
        /// <summary>
		/// Credit_Id1_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId1S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId1S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id1_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId1L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId1L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id1_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId1S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId1S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id1_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId1L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId1L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit1_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit1S, false, FieldTypeEnum.Integer, true)]
		public int? Credit1S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit1_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit1L, false, FieldTypeEnum.Integer, true)]
		public int? Credit1L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id2_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId2S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId2S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id2_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId2L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId2L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id2_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId2S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId2S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id2_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId2L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId2L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit2S, false, FieldTypeEnum.Integer, true)]
		public int? Credit2S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit2_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit2L, false, FieldTypeEnum.Integer, true)]
		public int? Credit2L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id3_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId3S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId3S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id3_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId3L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId3L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id3_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId3S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId3S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id3_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId3L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId3L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit3_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit3S, false, FieldTypeEnum.Integer, true)]
		public int? Credit3S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit3_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit3L, false, FieldTypeEnum.Integer, true)]
		public int? Credit3L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id4_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId4S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId4S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id4_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId4L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId4L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id4_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId4S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId4S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id4_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId4L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId4L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit4_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit4S, false, FieldTypeEnum.Integer, true)]
		public int? Credit4S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit4_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit4L, false, FieldTypeEnum.Integer, true)]
		public int? Credit4L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id5_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId5S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId5S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id5_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId5L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId5L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id5_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId5S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId5S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id5_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId5L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId5L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit5_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit5S, false, FieldTypeEnum.Integer, true)]
		public int? Credit5S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit5_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit5L, false, FieldTypeEnum.Integer, true)]
		public int? Credit5L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id6_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId6S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId6S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id6_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId6L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId6L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id6_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId6S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId6S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id6_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId6L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId6L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit6_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit6S, false, FieldTypeEnum.Integer, true)]
		public int? Credit6S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit6_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit6L, false, FieldTypeEnum.Integer, true)]
		public int? Credit6L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id7_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId7S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId7S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id7_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId7L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId7L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id7_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId7S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId7S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id7_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId7L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId7L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit7_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit7S, false, FieldTypeEnum.Integer, true)]
		public int? Credit7S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit7_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit7L, false, FieldTypeEnum.Integer, true)]
		public int? Credit7L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id8_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId8S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId8S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id8_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId8L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId8L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id8_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId8S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId8S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id8_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId8L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId8L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit8_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit8S, false, FieldTypeEnum.Integer, true)]
		public int? Credit8S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit8_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit8L, false, FieldTypeEnum.Integer, true)]
		public int? Credit8L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id9_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId9S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId9S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id9_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId9L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId9L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id9_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId9S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId9S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id9_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId9L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId9L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit9_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit9S, false, FieldTypeEnum.Integer, true)]
		public int? Credit9S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit9_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit9L, false, FieldTypeEnum.Integer, true)]
		public int? Credit9L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId10S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId10S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId10L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId10L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId10S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId10S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId10L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId10L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit10_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit10S, false, FieldTypeEnum.Integer, true)]
		public int? Credit10S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit10_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit10L, false, FieldTypeEnum.Integer, true)]
		public int? Credit10L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId11S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId11S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId11L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId11L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId11S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId11S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId11L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId11L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit11_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit11S, false, FieldTypeEnum.Integer, true)]
		public int? Credit11S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit11_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit11L, false, FieldTypeEnum.Integer, true)]
		public int? Credit11L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId12S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId12S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId12L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId12L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId12S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId12S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId12L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId12L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit12_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit12S, false, FieldTypeEnum.Integer, true)]
		public int? Credit12S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit12_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit12L, false, FieldTypeEnum.Integer, true)]
		public int? Credit12L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId13S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId13S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId13L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId13L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId13S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId13S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId13L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId13L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit13_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit13S, false, FieldTypeEnum.Integer, true)]
		public int? Credit13S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit13_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit13L, false, FieldTypeEnum.Integer, true)]
		public int? Credit13L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId14S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId14S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId14L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId14L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId14S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId14S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId14L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId14L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit14_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit14S, false, FieldTypeEnum.Integer, true)]
		public int? Credit14S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit14_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit14L, false, FieldTypeEnum.Integer, true)]
		public int? Credit14L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId15S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId15S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId15L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId15L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId15S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId15S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId15L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId15L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit15_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit15S, false, FieldTypeEnum.Integer, true)]
		public int? Credit15S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit15_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit15L, false, FieldTypeEnum.Integer, true)]
		public int? Credit15L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId16S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId16S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId16L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId16L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId16S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId16S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId16L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId16L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit16_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit16S, false, FieldTypeEnum.Integer, true)]
		public int? Credit16S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit16_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit16L, false, FieldTypeEnum.Integer, true)]
		public int? Credit16L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId17S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId17S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId17L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId17L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId17S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId17S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId17L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId17L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit17_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit17S, false, FieldTypeEnum.Integer, true)]
		public int? Credit17S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit17_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit17L, false, FieldTypeEnum.Integer, true)]
		public int? Credit17L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId18S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId18S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId18L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId18L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId18S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId18S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId18L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId18L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit18_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit18S, false, FieldTypeEnum.Integer, true)]
		public int? Credit18S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit18_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit18L, false, FieldTypeEnum.Integer, true)]
		public int? Credit18L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId19S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId19S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId19L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId19L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId19S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId19S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId19L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId19L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit19_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit19S, false, FieldTypeEnum.Integer, true)]
		public int? Credit19S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit19_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit19L, false, FieldTypeEnum.Integer, true)]
		public int? Credit19L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId20S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId20S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId20L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId20L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId20S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId20S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId20L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId20L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit20_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit20S, false, FieldTypeEnum.Integer, true)]
		public int? Credit20S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit20_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit20L, false, FieldTypeEnum.Integer, true)]
		public int? Credit20L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId21S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId21S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId21L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId21L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId21S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId21S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId21L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId21L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit21_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit21S, false, FieldTypeEnum.Integer, true)]
		public int? Credit21S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit21_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit21L, false, FieldTypeEnum.Integer, true)]
		public int? Credit21L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId22S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId22S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId22L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId22L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId22S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId22S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId22L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId22L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit22_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit22S, false, FieldTypeEnum.Integer, true)]
		public int? Credit22S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit22_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit22L, false, FieldTypeEnum.Integer, true)]
		public int? Credit22L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId23S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId23S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId23L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId23L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId23S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId23S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId23L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId23L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit23_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit23S, false, FieldTypeEnum.Integer, true)]
		public int? Credit23S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit23_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit23L, false, FieldTypeEnum.Integer, true)]
		public int? Credit23L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId24S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId24S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId24L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId24L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId24S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId24S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId24L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId24L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit24_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit24S, false, FieldTypeEnum.Integer, true)]
		public int? Credit24S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit24_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit24L, false, FieldTypeEnum.Integer, true)]
		public int? Credit24L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId25S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId25S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId25L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId25L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId25S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId25S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId25L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId25L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit25_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit25S, false, FieldTypeEnum.Integer, true)]
		public int? Credit25S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit25_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit25L, false, FieldTypeEnum.Integer, true)]
		public int? Credit25L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId26S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId26S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId26L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId26L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId26S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId26S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId26L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId26L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit26_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit26S, false, FieldTypeEnum.Integer, true)]
		public int? Credit26S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit26_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit26L, false, FieldTypeEnum.Integer, true)]
		public int? Credit26L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId27S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId27S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId27L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId27L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId27S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId27S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId27L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId27L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit27_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit27S, false, FieldTypeEnum.Integer, true)]
		public int? Credit27S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit27_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit27L, false, FieldTypeEnum.Integer, true)]
		public int? Credit27L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId28S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId28S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId28L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId28L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId28S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId28S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId28L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId28L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit28_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit28S, false, FieldTypeEnum.Integer, true)]
		public int? Credit28S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit28_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit28L, false, FieldTypeEnum.Integer, true)]
		public int? Credit28L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId29S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId29S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId29L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId29L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId29S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId29S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId29L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId29L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit29_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit29S, false, FieldTypeEnum.Integer, true)]
		public int? Credit29S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit29_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit29L, false, FieldTypeEnum.Integer, true)]
		public int? Credit29L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId30S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId30S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId30L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId30L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId30S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId30S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId30L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId30L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit30_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit30S, false, FieldTypeEnum.Integer, true)]
		public int? Credit30S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit30_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit30L, false, FieldTypeEnum.Integer, true)]
		public int? Credit30L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId31S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId31S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId31L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId31L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId31S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId31S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId31L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId31L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit31_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit31S, false, FieldTypeEnum.Integer, true)]
		public int? Credit31S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit31_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit31L, false, FieldTypeEnum.Integer, true)]
		public int? Credit31L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId32S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId32S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId32L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId32L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId32S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId32S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId32L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId32L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit32_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit32S, false, FieldTypeEnum.Integer, true)]
		public int? Credit32S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit32_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit32L, false, FieldTypeEnum.Integer, true)]
		public int? Credit32L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId33S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId33S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId33L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId33L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId33S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId33S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId33L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId33L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit33_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit33S, false, FieldTypeEnum.Integer, true)]
		public int? Credit33S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit33_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit33L, false, FieldTypeEnum.Integer, true)]
		public int? Credit33L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId34S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId34S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId34L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId34L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId34S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId34S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId34L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId34L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit34_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit34S, false, FieldTypeEnum.Integer, true)]
		public int? Credit34S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit34_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit34L, false, FieldTypeEnum.Integer, true)]
		public int? Credit34L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId35S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId35S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId35L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId35L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId35S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId35S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId35L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId35L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit35_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit35S, false, FieldTypeEnum.Integer, true)]
		public int? Credit35S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit35_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit35L, false, FieldTypeEnum.Integer, true)]
		public int? Credit35L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId36S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId36S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId36L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId36L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId36S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId36S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId36L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId36L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit36_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit36S, false, FieldTypeEnum.Integer, true)]
		public int? Credit36S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit36_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit36L, false, FieldTypeEnum.Integer, true)]
		public int? Credit36L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId37S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId37S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId37L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId37L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId37S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId37S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId37L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId37L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit37_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit37S, false, FieldTypeEnum.Integer, true)]
		public int? Credit37S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit37_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit37L, false, FieldTypeEnum.Integer, true)]
		public int? Credit37L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId38S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId38S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId38L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId38L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId38S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId38S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId38L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId38L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit38_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit38S, false, FieldTypeEnum.Integer, true)]
		public int? Credit38S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit38_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit38L, false, FieldTypeEnum.Integer, true)]
		public int? Credit38L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId39S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId39S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId39L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId39L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId39S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId39S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId39L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId39L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit39_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit39S, false, FieldTypeEnum.Integer, true)]
		public int? Credit39S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit39_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit39L, false, FieldTypeEnum.Integer, true)]
		public int? Credit39L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId40S, false, FieldTypeEnum.Integer, true)]
		public int? CreditId40S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit_Id40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CreditId40L, false, FieldTypeEnum.Integer, true)]
		public int? CreditId40L
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId40S, false, FieldTypeEnum.Integer, true)]
		public int? CourseId40S
		{
			get;
			set;
		}

		/// <summary>
		/// Course_Id40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.CourseId40L, false, FieldTypeEnum.Integer, true)]
		public int? CourseId40L
		{
			get;
			set;
		}

		/// <summary>
		/// Credit40_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit40S, false, FieldTypeEnum.Integer, true)]
		public int? Credit40S
		{
			get;
			set;
		}

		/// <summary>
		/// Credit40_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.Credit40L, false, FieldTypeEnum.Integer, true)]
		public int? Credit40L
		{
			get;
			set;
		}
        #endregion

        #region 轉帳資料對照欄位 (StudentReceiveEntity)
        /// <summary>
		/// Deduct_BankID_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductBankidS, false, FieldTypeEnum.Integer, true)]
		public int? DeductBankidS
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_BankID_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductBankidL, false, FieldTypeEnum.Integer, true)]
		public int? DeductBankidL
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_AccountNo_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductAccountnoS, false, FieldTypeEnum.Integer, true)]
		public int? DeductAccountnoS
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_AccountNo_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductAccountnoL, false, FieldTypeEnum.Integer, true)]
		public int? DeductAccountnoL
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_AccountName_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductAccountnameS, false, FieldTypeEnum.Integer, true)]
		public int? DeductAccountnameS
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_AccountName_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductAccountnameL, false, FieldTypeEnum.Integer, true)]
		public int? DeductAccountnameL
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_AccountId_S 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductAccountidS, false, FieldTypeEnum.Integer, true)]
		public int? DeductAccountidS
		{
			get;
			set;
		}

		/// <summary>
		/// Deduct_AccountId_L 欄位屬性
		/// </summary>
		[FieldSpec(Field.DeductAccountidL, false, FieldTypeEnum.Integer, true)]
		public int? DeductAccountidL
		{
			get;
			set;
		}
        #endregion

        #region 備註資料對照欄位 (StudentReceiveEntity)
        /// <summary>
        /// 備註01欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo01S, false, FieldTypeEnum.Integer, true)]
        public int? Memo01S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註01欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo01L, false, FieldTypeEnum.Integer, true)]
        public int? Memo01L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註02欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo02S, false, FieldTypeEnum.Integer, true)]
        public int? Memo02S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註02欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo02L, false, FieldTypeEnum.Integer, true)]
        public int? Memo02L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註03欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo03S, false, FieldTypeEnum.Integer, true)]
        public int? Memo03S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註03欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo03L, false, FieldTypeEnum.Integer, true)]
        public int? Memo03L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註04欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo04S, false, FieldTypeEnum.Integer, true)]
        public int? Memo04S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註04欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo04L, false, FieldTypeEnum.Integer, true)]
        public int? Memo04L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註05欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo05S, false, FieldTypeEnum.Integer, true)]
        public int? Memo05S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註05欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo05L, false, FieldTypeEnum.Integer, true)]
        public int? Memo05L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註06欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo06S, false, FieldTypeEnum.Integer, true)]
        public int? Memo06S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註06欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo06L, false, FieldTypeEnum.Integer, true)]
        public int? Memo06L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註07欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo07S, false, FieldTypeEnum.Integer, true)]
        public int? Memo07S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註07欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo07L, false, FieldTypeEnum.Integer, true)]
        public int? Memo07L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註08欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo08S, false, FieldTypeEnum.Integer, true)]
        public int? Memo08S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註08欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo08L, false, FieldTypeEnum.Integer, true)]
        public int? Memo08L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註09欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo09S, false, FieldTypeEnum.Integer, true)]
        public int? Memo09S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註09欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo09L, false, FieldTypeEnum.Integer, true)]
        public int? Memo09L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註10欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo10S, false, FieldTypeEnum.Integer, true)]
        public int? Memo10S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註10欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo10L, false, FieldTypeEnum.Integer, true)]
        public int? Memo10L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註11欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo11S, false, FieldTypeEnum.Integer, true)]
        public int? Memo11S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註11欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo11L, false, FieldTypeEnum.Integer, true)]
        public int? Memo11L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註12欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo12S, false, FieldTypeEnum.Integer, true)]
        public int? Memo12S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註12欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo12L, false, FieldTypeEnum.Integer, true)]
        public int? Memo12L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註13欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo13S, false, FieldTypeEnum.Integer, true)]
        public int? Memo13S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註13欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo13L, false, FieldTypeEnum.Integer, true)]
        public int? Memo13L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註14欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo14S, false, FieldTypeEnum.Integer, true)]
        public int? Memo14S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註14欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo14L, false, FieldTypeEnum.Integer, true)]
        public int? Memo14L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註15欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo15S, false, FieldTypeEnum.Integer, true)]
        public int? Memo15S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註15欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo15L, false, FieldTypeEnum.Integer, true)]
        public int? Memo15L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註16欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo16S, false, FieldTypeEnum.Integer, true)]
        public int? Memo16S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註16欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo16L, false, FieldTypeEnum.Integer, true)]
        public int? Memo16L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註17欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo17S, false, FieldTypeEnum.Integer, true)]
        public int? Memo17S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註17欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo17L, false, FieldTypeEnum.Integer, true)]
        public int? Memo17L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註18欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo18S, false, FieldTypeEnum.Integer, true)]
        public int? Memo18S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註18欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo18L, false, FieldTypeEnum.Integer, true)]
        public int? Memo18L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註19欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo19S, false, FieldTypeEnum.Integer, true)]
        public int? Memo19S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註19欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo19L, false, FieldTypeEnum.Integer, true)]
        public int? Memo19L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註20欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo20S, false, FieldTypeEnum.Integer, true)]
        public int? Memo20S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註20欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo20L, false, FieldTypeEnum.Integer, true)]
        public int? Memo20L
        {
            get;
            set;
        }

        /// <summary>
        /// 備註21欄位起始位置
        /// </summary>
        [FieldSpec(Field.Memo21S, false, FieldTypeEnum.Integer, true)]
        public int? Memo21S
        {
            get;
            set;
        }

        /// <summary>
        /// 備註21欄位字元長度
        /// </summary>
        [FieldSpec(Field.Memo21L, false, FieldTypeEnum.Integer, true)]
        public int? Memo21L
        {
            get;
            set;
        }
		#endregion

		#region [MDY:202203XX] 2022擴充案 英文名稱相關對照欄位
		#region 學籍資料英文名稱對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
		/// <summary>
		/// 部別英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.DeptENameS, false, FieldTypeEnum.Integer, true)]
		public int? DeptENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 部別英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.DeptENameL, false, FieldTypeEnum.Integer, true)]
		public int? DeptENameL
		{
			get;
			set;
		}

		/// <summary>
		/// 院別英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.CollegeENameS, false, FieldTypeEnum.Integer, true)]
		public int? CollegeENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 院別英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.CollegeENameL, false, FieldTypeEnum.Integer, true)]
		public int? CollegeENameL
		{
			get;
			set;
		}

		/// <summary>
		/// 系所英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.MajorENameS, false, FieldTypeEnum.Integer, true)]
		public int? MajorENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 系所英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.MajorENameL, false, FieldTypeEnum.Integer, true)]
		public int? MajorENameL
		{
			get;
			set;
		}

		/// <summary>
		/// 班別英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.ClassENameS, false, FieldTypeEnum.Integer, true)]
		public int? ClassENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 班別英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.ClassENameL, false, FieldTypeEnum.Integer, true)]
		public int? ClassENameL
		{
			get;
			set;
		}
		#endregion

		#region 減免、就貸、住宿英文名稱對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
		/// <summary>
		/// 減免英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.ReduceENameS, false, FieldTypeEnum.Integer, true)]
		public int? ReduceENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 減免英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.ReduceENameL, false, FieldTypeEnum.Integer, true)]
		public int? ReduceENameL
		{
			get;
			set;
		}

		/// <summary>
		/// 住宿英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.DormENameS, false, FieldTypeEnum.Integer, true)]
		public int? DormENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 住宿英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.DormENameL, false, FieldTypeEnum.Integer, true)]
		public int? DormENameL
		{
			get;
			set;
		}

		/// <summary>
		/// 就貸英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.LoanENameS, false, FieldTypeEnum.Integer, true)]
		public int? LoanENameS
		{
			get;
			set;
		}

		/// <summary>
		/// 就貸英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.LoanENameL, false, FieldTypeEnum.Integer, true)]
		public int? LoanENameL
		{
			get;
			set;
		}
		#endregion

		#region 身分註記英文名稱對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
		/// <summary>
		/// 身份註記01英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyEName1S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName1S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記01英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyEName1L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName1L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記02英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyEName2S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName2S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記02英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyEName2L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName2L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記03英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyEName3S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName3S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記03英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyEName3L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName3L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記04英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyEName4S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName4S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記04英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyEName4L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName4L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記05英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyEName5S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName5S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記05英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyEName5L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName5L
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記06英文名稱欄位起始位置
		/// </summary>
		[FieldSpec(Field.IdentifyEName6S, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName6S
		{
			get;
			set;
		}

		/// <summary>
		/// 身份註記06英文名稱欄位字元長度
		/// </summary>
		[FieldSpec(Field.IdentifyEName6L, false, FieldTypeEnum.Integer, true)]
		public int? IdentifyEName6L
		{
			get;
			set;
		}
		#endregion
		#endregion
		#endregion

		#region 狀態相關欄位
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
		/// 取得上傳資料相關欄位中有設定資料的 TxtMapField 設定陣列 (這裡的 Key 沿用 MappingreXlsmdbEntity 的欄位名稱，避免要另外處理 Key)
		/// </summary>
		/// <returns>傳回 TxtMapField 設定陣列</returns>
		internal TxtMapField[] GetMapFields()
		{
			List<TxtMapField> mapFields = new List<TxtMapField>();

			#region 學生資料對照欄位 (StudentMasterEntity)
			{
				if (this.StuIdS != null && this.StuIdL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuId, this.StuIdS.Value, this.StuIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.StuNameS != null && this.StuNameL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuName, this.StuNameS.Value, this.StuNameL.Value, new WordChecker(1, 60)));
				}
				if (this.StuBirthdayS != null && this.StuBirthdayL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuBirthday, this.StuBirthdayS.Value, this.StuBirthdayL.Value, new DateTimeChecker(DateTimeChecker.FormatEnum.DateText)));
				}
				if (this.IdNumberS != null && this.IdNumberL != null)
				{
					#region [Old] 土銀不檢查身份證格式，改檢查字元長度限制
					//mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdNumber, this.IdNumberS.Value, this.IdNumberL.Value, new PersonalIDChecker()));
					#endregion

					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdNumber, this.IdNumberS.Value, this.IdNumberL.Value, new CharChecker(0, 10)));
				}
				if (this.StuTelS != null && this.StuTelL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuTel, this.StuTelS.Value, this.StuTelL.Value, new CharChecker(0, 14)));
				}
				if (this.StuAddcodeS != null && this.StuAddcodeL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuAddcode, this.StuAddcodeS.Value, this.StuAddcodeL.Value, new CharChecker(0, 5)));
				}
				if (this.StuAddS != null && this.StuAddL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuAdd, this.StuAddS.Value, this.StuAddL.Value, new WordChecker(0, 50)));
				}
				if (this.EmailS != null && this.EmailL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.Email, this.EmailS.Value, this.EmailL.Value, new CharChecker(0, 50)));
				}
				if (this.StuParentS != null && this.StuParentL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuParent, this.StuParentS.Value, this.StuParentL.Value, new WordChecker(0, 60)));
				}
			}
			#endregion

			#region [MDY:20160131] 增加資料序號與繳款期限對照欄位 (StudentReceiveEntity)
			{
				if (this.OldSeqS != null && this.OldSeqL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.OldSeq, this.OldSeqS.Value, this.OldSeqL.Value, new IntegerChecker(0, StudentReceiveEntity.MaxOldSeq)));
				}
				if (this.PayDueDateS != null && this.PayDueDateL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.PayDueDate, this.PayDueDateS.Value, this.PayDueDateL.Value, new DateTimeChecker(DateTimeChecker.FormatEnum.DateText)));
				}
			}
			#endregion

			#region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位 (StudentReceiveEntity)
			{
				if (this.NCCardFlagS.HasValue && this.NCCardFlagL.HasValue)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.NCCardFlag, this.NCCardFlagS.Value, this.NCCardFlagL.Value, new RegexChecker(1, 1, new System.Text.RegularExpressions.Regex("^[YN是否]$", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase), "必須為 Y、N、是、否")));
				}
			}
			#endregion

			#region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, CollegeListEntity, MajorListEntity)
			{
				if (this.StuGradeS != null && this.StuGradeL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuGrade, this.StuGradeS.Value, this.StuGradeL.Value, new CodeChecker(1, 2)));
				}
				if (this.StuHidS != null && this.StuHidL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuHid, this.StuHidS.Value, this.StuHidL.Value, new CodeChecker(1, 10)));
				}

				#region [MDY:202203XX] 2022擴充案 名稱檢核 CodeChecker(1, 20) 調整為 WordChecker(1, 40)
				if (this.ClassIdS != null && this.ClassIdL != null)   //ClassListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ClassId, this.ClassIdS.Value, this.ClassIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.ClassNameS != null && this.ClassNameL != null) //ClassListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ClassName, this.ClassNameS.Value, this.ClassNameL.Value, new WordChecker(1, 40)));
				}
				if (this.DeptIdS != null && this.DeptIdL != null) //DepListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeptId, this.DeptIdS.Value, this.DeptIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.DeptNameS != null && this.DeptNameL != null) //DepListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeptName, this.DeptNameS.Value, this.DeptNameL.Value, new WordChecker(1, 40)));
				}
				if (this.CollegeIdS != null && this.CollegeIdL != null) //CollegeListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.CollegeId, this.CollegeIdS.Value, this.CollegeIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.CollegeNameS != null && this.CollegeNameL != null) //CollegeListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.CollegeName, this.CollegeNameS.Value, this.CollegeNameL.Value, new WordChecker(1, 40)));
				}
				#endregion

				#region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
				if (this.MajorIdS != null && this.MajorIdL != null) //MajorListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.MajorId, this.MajorIdS.Value, this.MajorIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.MajorNameS != null && this.MajorNameL != null) //MajorListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.MajorName, this.MajorNameS.Value, this.MajorNameL.Value, new WordChecker(1, 40)));
				}
				#endregion
			}
			#endregion

			#region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
			{
				#region [MDY:202203XX] 2022擴充案 名稱檢核 WordChecker(1, 20) 調整為 WordChecker(1, 40)
				if (this.ReduceIdS != null && this.ReduceIdL != null)  //+ ReduceListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ReduceId, this.ReduceIdS.Value, this.ReduceIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.ReduceNameS != null && this.ReduceNameL != null) //ReduceListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ReduceName, this.ReduceNameS.Value, this.ReduceNameL.Value, new WordChecker(1, 40)));
				}
				if (this.LoanIdS != null && this.LoanIdL != null)  //+ LoanListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.LoanId, this.LoanIdS.Value, this.LoanIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.LoanNameS != null && this.LoanNameL != null) //LoanListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.LoanName, this.LoanNameS.Value, this.LoanNameL.Value, new WordChecker(1, 40)));
				}
				if (this.DormIdS != null && this.DormIdL != null)  //+ DormListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DormId, this.DormIdS.Value, this.DormIdL.Value, new CodeChecker(1, 20)));
				}
				if (this.DormNameS != null && this.DormNameL != null) //DormListEntity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DormName, this.DormNameS.Value, this.DormNameL.Value, new WordChecker(1, 40)));
				}
				#endregion
			}
			#endregion

			#region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
			{
				#region [MDY:202203XX] 2022擴充案 名稱檢核 WordChecker(1, 20) 調整為 WordChecker(1, 40)
				if (this.IdentifyId1S != null && this.IdentifyId1L != null)  //IdentifyList1Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyId1, this.IdentifyId1S.Value, this.IdentifyId1L.Value, new CodeChecker(1, 20)));
				}
				if (this.IdentifyName1S != null && this.IdentifyName1L != null) //IdentifyList1Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyName1, this.IdentifyName1S.Value, this.IdentifyName1L.Value, new WordChecker(1, 40)));
				}

				if (this.IdentifyId2S != null && this.IdentifyId2L != null)  //IdentifyList2Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyId2, this.IdentifyId2S.Value, this.IdentifyId2L.Value, new CodeChecker(1, 20)));
				}
				if (this.IdentifyName2S != null && this.IdentifyName2L != null) //IdentifyList2Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyName2, this.IdentifyName2S.Value, this.IdentifyName2L.Value, new WordChecker(1, 40)));
				}

				if (this.IdentifyId3S != null && this.IdentifyId3L != null)  //IdentifyList3Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyId3, this.IdentifyId3S.Value, this.IdentifyId3L.Value, new CodeChecker(1, 20)));
				}
				if (this.IdentifyName3S != null && this.IdentifyName3L != null) //IdentifyList3Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyName3, this.IdentifyName3S.Value, this.IdentifyName3L.Value, new WordChecker(1, 40)));
				}

				if (this.IdentifyId4S != null && this.IdentifyId4L != null)  //IdentifyList4Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyId4, this.IdentifyId4S.Value, this.IdentifyId4L.Value, new CodeChecker(1, 20)));
				}
				if (this.IdentifyName4S != null && this.IdentifyName4L != null) //IdentifyList4Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyName4, this.IdentifyName4S.Value, this.IdentifyName4L.Value, new WordChecker(1, 40)));
				}

				if (this.IdentifyId5S != null && this.IdentifyId5L != null)  //IdentifyList5Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyId5, this.IdentifyId5S.Value, this.IdentifyId5L.Value, new CodeChecker(1, 20)));
				}
				if (this.IdentifyName5S != null && this.IdentifyName5L != null) //IdentifyList5Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyName5, this.IdentifyName5S.Value, this.IdentifyName5L.Value, new WordChecker(1, 40)));
				}

				if (this.IdentifyName6S != null && this.IdentifyName6L != null)  //IdentifyList6Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyId6, this.IdentifyId6S.Value, this.IdentifyId6L.Value, new CodeChecker(1, 20)));
				}
				if (this.IdentifyName6S != null && this.IdentifyName6L != null) //IdentifyList6Entity
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyName6, this.IdentifyName6S.Value, this.IdentifyName6L.Value, new WordChecker(1, 40)));
				}
				#endregion
			}
			#endregion

			#region 收入科目金額對照欄位 (StudentReceiveEntity)
			{
				string[] fields = new string[] {
					MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3, MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5,
					MappingreXlsmdbEntity.Field.Receive6, MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9, MappingreXlsmdbEntity.Field.Receive10,
					MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12, MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
					MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18, MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20,
					MappingreXlsmdbEntity.Field.Receive21, MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24, MappingreXlsmdbEntity.Field.Receive25,
					MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27, MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
					MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33, MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35,
					MappingreXlsmdbEntity.Field.Receive36, MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39, MappingreXlsmdbEntity.Field.Receive40
				};
				int?[] starts = new int?[] {
					this.Receive1S, this.Receive2S, this.Receive3S, this.Receive4S, this.Receive5S,
					this.Receive6S, this.Receive7S, this.Receive8S, this.Receive9S, this.Receive10S,
					this.Receive11S, this.Receive12S, this.Receive13S, this.Receive14S, this.Receive15S,
					this.Receive16S, this.Receive17S, this.Receive18S, this.Receive19S, this.Receive20S,
					this.Receive21S, this.Receive22S, this.Receive23S, this.Receive24S, this.Receive25S,
					this.Receive26S, this.Receive27S, this.Receive28S, this.Receive29S, this.Receive30S,
					this.Receive31S, this.Receive32S, this.Receive33S, this.Receive34S, this.Receive35S,
					this.Receive36S, this.Receive37S, this.Receive38S, this.Receive39S, this.Receive40S
				};
				int?[] lengths = new int?[] {
					this.Receive1L, this.Receive2L, this.Receive3L, this.Receive4L, this.Receive5L,
					this.Receive6L, this.Receive7L, this.Receive8L, this.Receive9L, this.Receive10L,
					this.Receive11L, this.Receive12L, this.Receive13L, this.Receive14L, this.Receive15L,
					this.Receive16L, this.Receive17L, this.Receive18L, this.Receive19L, this.Receive20L,
					this.Receive21L, this.Receive22L, this.Receive23L, this.Receive24L, this.Receive25L,
					this.Receive26L, this.Receive27L, this.Receive28L, this.Receive29L, this.Receive30L,
					this.Receive31L, this.Receive32L, this.Receive33L, this.Receive34L, this.Receive35L,
					this.Receive36L, this.Receive37L, this.Receive38L, this.Receive39L, this.Receive40L
				};
				for (int idx = 0; idx < starts.Length; idx++)
				{
					if (starts[idx] != null && lengths[idx] != null)
					{
						mapFields.Add(new TxtMapField(fields[idx], starts[idx].Value, lengths[idx].Value, new DecimalChecker(-999999999M, 999999999M)));
					}
				}
			}
			#endregion

			#region 其他對照欄位 (StudentReceiveEntity)
			{
				if (this.StuCreditS != null && this.StuCreditL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuCredit, this.StuCreditS.Value, this.StuCreditL.Value, new DecimalChecker(0M, 999.99M)));
				}
				if (this.StuHourS != null && this.StuHourL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.StuHour, this.StuHourS.Value, this.StuHourL.Value, new WordChecker(0, 10)));
				}
				if (this.LoanAmountS != null && this.LoanAmountL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.LoanAmount, this.LoanAmountS.Value, this.LoanAmountL.Value, new DecimalChecker(0M, 999999999M)));
				}
				if (this.ReceiveAmountS != null && this.ReceiveAmountL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ReceiveAmount, this.ReceiveAmountS.Value, this.ReceiveAmountL.Value, new DecimalChecker(0M, 999999999M)));
				}
			}
			#endregion

			#region 學分基準、課程、學分數對照欄位 (StudentCourseEntity)
			{
				string[] creditIdFileds = new string[] {
					MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2,
					MappingreXlsmdbEntity.Field.CreditId3, MappingreXlsmdbEntity.Field.CreditId4,
					MappingreXlsmdbEntity.Field.CreditId5, MappingreXlsmdbEntity.Field.CreditId6,
					MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8,
					MappingreXlsmdbEntity.Field.CreditId9, MappingreXlsmdbEntity.Field.CreditId10,
					MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12,
					MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14,
					MappingreXlsmdbEntity.Field.CreditId15, MappingreXlsmdbEntity.Field.CreditId16,
					MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18,
					MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20,
					MappingreXlsmdbEntity.Field.CreditId21, MappingreXlsmdbEntity.Field.CreditId22,
					MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24,
					MappingreXlsmdbEntity.Field.CreditId25, MappingreXlsmdbEntity.Field.CreditId26,
					MappingreXlsmdbEntity.Field.CreditId27, MappingreXlsmdbEntity.Field.CreditId28,
					MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
					MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32,
					MappingreXlsmdbEntity.Field.CreditId33, MappingreXlsmdbEntity.Field.CreditId34,
					MappingreXlsmdbEntity.Field.CreditId35, MappingreXlsmdbEntity.Field.CreditId36,
					MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38,
					MappingreXlsmdbEntity.Field.CreditId39, MappingreXlsmdbEntity.Field.CreditId40
				};
				int?[] creditIdStarts = new int?[] {
					this.CreditId1S, this.CreditId2S, this.CreditId3S, this.CreditId4S, this.CreditId5S,
					this.CreditId6S, this.CreditId7S, this.CreditId8S, this.CreditId9S, this.CreditId10S,
					this.CreditId11S, this.CreditId12S, this.CreditId13S, this.CreditId14S, this.CreditId15S,
					this.CreditId16S, this.CreditId17S, this.CreditId18S, this.CreditId19S, this.CreditId20S,
					this.CreditId21S, this.CreditId22S, this.CreditId23S, this.CreditId24S, this.CreditId25S,
					this.CreditId26S, this.CreditId27S, this.CreditId28S, this.CreditId29S, this.CreditId30S,
					this.CreditId31S, this.CreditId32S, this.CreditId33S, this.CreditId34S, this.CreditId35S,
					this.CreditId36S, this.CreditId37S, this.CreditId38S, this.CreditId39S, this.CreditId40S
				};
				int?[] creditIdLengths = new int?[] {
					this.CreditId1L, this.CreditId2L, this.CreditId3L, this.CreditId4L, this.CreditId5L,
					this.CreditId6L, this.CreditId7L, this.CreditId8L, this.CreditId9L, this.CreditId10L,
					this.CreditId11L, this.CreditId12L, this.CreditId13L, this.CreditId14L, this.CreditId15L,
					this.CreditId16L, this.CreditId17L, this.CreditId18L, this.CreditId19L, this.CreditId20L,
					this.CreditId21L, this.CreditId22L, this.CreditId23L, this.CreditId24L, this.CreditId25L,
					this.CreditId26L, this.CreditId27L, this.CreditId28L, this.CreditId29L, this.CreditId30L,
					this.CreditId31L, this.CreditId32L, this.CreditId33L, this.CreditId34L, this.CreditId35L,
					this.CreditId36L, this.CreditId37L, this.CreditId38L, this.CreditId39L, this.CreditId40L
				};
				string[] courseIdFileds = new string[] {
					MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2,
					MappingreXlsmdbEntity.Field.CourseId3, MappingreXlsmdbEntity.Field.CourseId4,
					MappingreXlsmdbEntity.Field.CourseId5, MappingreXlsmdbEntity.Field.CourseId6,
					MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8,
					MappingreXlsmdbEntity.Field.CourseId9, MappingreXlsmdbEntity.Field.CourseId10,
					MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12,
					MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14,
					MappingreXlsmdbEntity.Field.CourseId15, MappingreXlsmdbEntity.Field.CourseId16,
					MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18,
					MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20,
					MappingreXlsmdbEntity.Field.CourseId21, MappingreXlsmdbEntity.Field.CourseId22,
					MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24,
					MappingreXlsmdbEntity.Field.CourseId25, MappingreXlsmdbEntity.Field.CourseId26,
					MappingreXlsmdbEntity.Field.CourseId27, MappingreXlsmdbEntity.Field.CourseId28,
					MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
					MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32,
					MappingreXlsmdbEntity.Field.CourseId33, MappingreXlsmdbEntity.Field.CourseId34,
					MappingreXlsmdbEntity.Field.CourseId35, MappingreXlsmdbEntity.Field.CourseId36,
					MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38,
					MappingreXlsmdbEntity.Field.CourseId39, MappingreXlsmdbEntity.Field.CourseId40
				};
				int?[] courseIdStarts = new int?[] {
					this.CourseId1S, this.CourseId2S, this.CourseId3S, this.CourseId4S, this.CourseId5S,
					this.CourseId6S, this.CourseId7S, this.CourseId8S, this.CourseId9S, this.CourseId10S,
					this.CourseId11S, this.CourseId12S, this.CourseId13S, this.CourseId14S, this.CourseId15S,
					this.CourseId16S, this.CourseId17S, this.CourseId18S, this.CourseId19S, this.CourseId20S,
					this.CourseId21S, this.CourseId22S, this.CourseId23S, this.CourseId24S, this.CourseId25S,
					this.CourseId26S, this.CourseId27S, this.CourseId28S, this.CourseId29S, this.CourseId30S,
					this.CourseId31S, this.CourseId32S, this.CourseId33S, this.CourseId34S, this.CourseId35S,
					this.CourseId36S, this.CourseId37S, this.CourseId38S, this.CourseId39S, this.CourseId40S
				};
				int?[] courseIdLengths = new int?[] {
					this.CourseId1L, this.CourseId2L, this.CourseId3L, this.CourseId4L, this.CourseId5L,
					this.CourseId6L, this.CourseId7L, this.CourseId8L, this.CourseId9L, this.CourseId10L,
					this.CourseId11L, this.CourseId12L, this.CourseId13L, this.CourseId14L, this.CourseId15L,
					this.CourseId16L, this.CourseId17L, this.CourseId18L, this.CourseId19L, this.CourseId20L,
					this.CourseId21L, this.CourseId22L, this.CourseId23L, this.CourseId24L, this.CourseId25L,
					this.CourseId26L, this.CourseId27L, this.CourseId28L, this.CourseId29L, this.CourseId30L,
					this.CourseId31L, this.CourseId32L, this.CourseId33L, this.CourseId34L, this.CourseId35L,
					this.CourseId36L, this.CourseId37L, this.CourseId38L, this.CourseId39L, this.CourseId40L
				};
				string[] creditFileds = new string[] {
					MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2,
					MappingreXlsmdbEntity.Field.Credit3, MappingreXlsmdbEntity.Field.Credit4,
					MappingreXlsmdbEntity.Field.Credit5, MappingreXlsmdbEntity.Field.Credit6,
					MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8,
					MappingreXlsmdbEntity.Field.Credit9, MappingreXlsmdbEntity.Field.Credit10,
					MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12,
					MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14,
					MappingreXlsmdbEntity.Field.Credit15, MappingreXlsmdbEntity.Field.Credit16,
					MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18,
					MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20,
					MappingreXlsmdbEntity.Field.Credit21, MappingreXlsmdbEntity.Field.Credit22,
					MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24,
					MappingreXlsmdbEntity.Field.Credit25, MappingreXlsmdbEntity.Field.Credit26,
					MappingreXlsmdbEntity.Field.Credit27, MappingreXlsmdbEntity.Field.Credit28,
					MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
					MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32,
					MappingreXlsmdbEntity.Field.Credit33, MappingreXlsmdbEntity.Field.Credit34,
					MappingreXlsmdbEntity.Field.Credit35, MappingreXlsmdbEntity.Field.Credit36,
					MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38,
					MappingreXlsmdbEntity.Field.Credit39, MappingreXlsmdbEntity.Field.Credit40
				};
				int?[] creditStarts = new int?[] {
					this.Credit1S, this.Credit2S, this.Credit3S, this.Credit4S, this.Credit5S,
					this.Credit6S, this.Credit7S, this.Credit8S, this.Credit9S, this.Credit10S,
					this.Credit11S, this.Credit12S, this.Credit13S, this.Credit14S, this.Credit15S,
					this.Credit16S, this.Credit17S, this.Credit18S, this.Credit19S, this.Credit20S,
					this.Credit21S, this.Credit22S, this.Credit23S, this.Credit24S, this.Credit25S,
					this.Credit26S, this.Credit27S, this.Credit28S, this.Credit29S, this.Credit30S,
					this.Credit31S, this.Credit32S, this.Credit33S, this.Credit34S, this.Credit35S,
					this.Credit36S, this.Credit37S, this.Credit38S, this.Credit39S, this.Credit40S
				};
				int?[] creditLengths = new int?[] {
					this.Credit1L, this.Credit2L, this.Credit3L, this.Credit4L, this.Credit5L,
					this.Credit6L, this.Credit7L, this.Credit8L, this.Credit9L, this.Credit10L,
					this.Credit11L, this.Credit12L, this.Credit13L, this.Credit14L, this.Credit15L,
					this.Credit16L, this.Credit17L, this.Credit18L, this.Credit19L, this.Credit20L,
					this.Credit21L, this.Credit22L, this.Credit23L, this.Credit24L, this.Credit25L,
					this.Credit26L, this.Credit27L, this.Credit28L, this.Credit29L, this.Credit30L,
					this.Credit31L, this.Credit32L, this.Credit33L, this.Credit34L, this.Credit35L,
					this.Credit36L, this.Credit37L, this.Credit38L, this.Credit39L, this.Credit40L
				};
				for (int idx = 0; idx < creditIdStarts.Length; idx++)
				{
					if (creditIdStarts[idx] != null && creditIdLengths[idx] != null)
					{
						mapFields.Add(new TxtMapField(creditIdFileds[idx], creditIdStarts[idx].Value, creditIdLengths[idx].Value, new CodeChecker(0, 8)));
					}
					if (courseIdStarts[idx] != null && courseIdLengths[idx] != null)
					{
						mapFields.Add(new TxtMapField(courseIdFileds[idx], courseIdStarts[idx].Value, courseIdLengths[idx].Value, new CodeChecker(0, 8)));
					}
					if (creditStarts[idx] != null && creditLengths[idx] != null)
					{
						mapFields.Add(new TxtMapField(creditFileds[idx], creditStarts[idx].Value, creditLengths[idx].Value, new DecimalChecker(0M, 999.99M)));
					}
				}
			}
			#endregion

			#region Remark (StudentReceiveEntity)
			{
				if (this.RemarkS != null && this.RemarkL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.Remark, this.RemarkS.Value, this.RemarkL.Value, null));
				}
			}
			#endregion

			#region 扣款資料相關對照欄位 (StudentReceiveEntity)
			{
				if (this.DeductBankidS != null && this.DeductBankidL != null)
				{
					//mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeductBankid, this.DeductBankidS.Value, this.DeductBankidL.Value, new NumberChecker(7, 7, "7碼的銀行代碼")));
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeductBankid, this.DeductBankidS.Value, this.DeductBankidL.Value, null));
				}
				if (this.DeductAccountnoS != null && this.DeductAccountnoL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeductAccountno, this.DeductAccountnoS.Value, this.DeductAccountnoL.Value, new NumberChecker(0, 21, "0~21碼數字")));
				}
				if (this.DeductAccountnameS != null && this.DeductAccountnameL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeductAccountname, this.DeductAccountnameS.Value, this.DeductAccountnameL.Value, new WordChecker(0, 60)));
				}
				if (this.DeductAccountidS != null && this.DeductAccountidL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeductAccountid, this.DeductAccountidS.Value, this.DeductAccountidL.Value, new CharChecker(0, 10)));
				}
			}
			#endregion

			#region 虛擬帳號資料相關對照欄位 (StudentReceiveEntity)
			{
				#region [Old]
				//int seriorSize = 11;
				//int cancelNoSize = 0;
				//if (!String.IsNullOrEmpty(this.ReceiveType))
				//{
				//    CancelNoHelper.Module module = CancelNoHelper.Module.GetByReceiveType(this.ReceiveType);
				//    if (module != null)
				//    {
				//        seriorSize = module.SeriorNoSize;
				//        cancelNoSize = module.CancelNoSize;
				//    }
				//}

				//if (this.SeriorNoS != null && this.SeriorNoL != null)
				//{
				//    mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.SeriorNo, this.SeriorNoS.Value, this.SeriorNoL.Value, new NumberChecker(1, seriorSize, String.Format("1~{0}碼的流水號數字", seriorSize))));
				//}
				//if (this.CancelNoS != null && this.CancelNoL != null)
				//{
				//    if (cancelNoSize == 0)
				//    {
				//        mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.CancelNo, this.SeriorNoS.Value, this.SeriorNoL.Value, new NumberChecker(15, 16, "15或16碼的虛擬帳號數字")));
				//    }
				//    else
				//    {
				//        mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.CancelNo, this.SeriorNoS.Value, this.SeriorNoL.Value, new NumberChecker(cancelNoSize, cancelNoSize, String.Format("{0}碼的虛擬帳號數字", cancelNoSize))));
				//    }
				//}
				#endregion

				if (this.SeriorNoS != null && this.SeriorNoL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.SeriorNo, this.SeriorNoS.Value, this.SeriorNoL.Value, new NumberChecker(1, 11, "流水號必須為數字")));
				}
				if (this.CancelNoS != null && this.CancelNoL != null)
				{
					mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.CancelNo, this.SeriorNoS.Value, this.SeriorNoL.Value, new NumberChecker(12, 16, "12、14或16碼的虛擬帳號數字")));
				}
			}
			#endregion

			#region 備註對照欄位 (StudentReceiveEntity)
			{
				string[] memoFileds = new string[MemoCount] {
					MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
					MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
					MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
					MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
					MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
					MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
					MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
					MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
					MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
					MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
					MappingreXlsmdbEntity.Field.Memo21
				};
				int?[] memoStarts = new int?[MemoCount] {
					this.Memo01S, this.Memo02S, this.Memo03S, this.Memo04S, this.Memo05S,
					this.Memo06S, this.Memo07S, this.Memo08S, this.Memo09S, this.Memo10S,
					this.Memo11S, this.Memo12S, this.Memo13S, this.Memo14S, this.Memo15S,
					this.Memo16S, this.Memo17S, this.Memo18S, this.Memo19S, this.Memo20S,
					this.Memo21S
				};
				int?[] memoLengths = new int?[MemoCount] {
					this.Memo01L, this.Memo02L, this.Memo03L, this.Memo04L, this.Memo05L,
					this.Memo06L, this.Memo07L, this.Memo08L, this.Memo09L, this.Memo10L,
					this.Memo11L, this.Memo12L, this.Memo13L, this.Memo14L, this.Memo15L,
					this.Memo16L, this.Memo17L, this.Memo18L, this.Memo19L, this.Memo20L,
					this.Memo21L
				};
				for (int idx = 0; idx < MemoCount; idx++)
				{
					if (memoStarts[idx] != null && memoLengths[idx] != null)
					{
						mapFields.Add(new TxtMapField(memoFileds[idx], memoStarts[idx].Value, memoLengths[idx].Value, new WordChecker(0, 50)));
					}
				}
			}
			#endregion

			#region [MDY:202203XX] 2022擴充案 英文名稱相關對照欄位
			#region 學籍資料英文名稱對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
			if (this.ClassENameS != null && this.ClassENameL != null) //ClassListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ClassEName, this.ClassENameS.Value, this.ClassENameL.Value, new WordChecker(1, 40)));
			}

			if (this.DeptENameS != null && this.DeptENameL != null) //DepListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DeptEName, this.DeptENameS.Value, this.DeptENameL.Value, new WordChecker(1, 40)));
			}

			if (this.MajorENameS != null && this.MajorENameL != null) //MajorListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.MajorEName, this.MajorENameS.Value, this.MajorENameL.Value, new WordChecker(1, 40)));
			}

			if (this.CollegeENameS != null && this.CollegeENameL != null) //CollegeListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.CollegeEName, this.CollegeENameS.Value, this.CollegeENameL.Value, new WordChecker(1, 40)));
			}
			#endregion

			#region 減免、就貸、住宿英文名稱對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
			if (this.ReduceENameS != null && this.ReduceENameL != null) //ReduceListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.ReduceEName, this.ReduceENameS.Value, this.ReduceENameL.Value, new WordChecker(1, 40)));
			}

			if (this.LoanENameS != null && this.LoanENameL != null) //LoanListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.LoanEName, this.LoanENameS.Value, this.LoanENameL.Value, new WordChecker(1, 40)));
			}

			if (this.DormENameS != null && this.DormENameL != null) //DormListEntity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.DormEName, this.DormENameS.Value, this.DormENameL.Value, new WordChecker(1, 40)));
			}
			#endregion

			#region 身分註記英文名稱對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
			if (this.IdentifyEName1S != null && this.IdentifyEName1L != null) //IdentifyList1Entity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyEName1, this.IdentifyEName1S.Value, this.IdentifyEName1L.Value, new WordChecker(1, 40)));
			}

			if (this.IdentifyEName2S != null && this.IdentifyEName2L != null) //IdentifyList2Entity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyEName2, this.IdentifyEName2S.Value, this.IdentifyEName2L.Value, new WordChecker(1, 40)));
			}

			if (this.IdentifyEName3S != null && this.IdentifyEName3L != null) //IdentifyList3Entity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyEName3, this.IdentifyEName3S.Value, this.IdentifyEName3L.Value, new WordChecker(1, 40)));
			}

			if (this.IdentifyEName4S != null && this.IdentifyEName4L != null) //IdentifyList4Entity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyEName4, this.IdentifyEName4S.Value, this.IdentifyEName4L.Value, new WordChecker(1, 40)));
			}

			if (this.IdentifyEName5S != null && this.IdentifyEName5L != null) //IdentifyList5Entity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyEName5, this.IdentifyEName5S.Value, this.IdentifyEName5L.Value, new WordChecker(1, 40)));
			}

			if (this.IdentifyEName6S != null && this.IdentifyEName6L != null) //IdentifyList6Entity
			{
				mapFields.Add(new TxtMapField(MappingreXlsmdbEntity.Field.IdentifyEName6, this.IdentifyEName6S.Value, this.IdentifyEName6L.Value, new WordChecker(1, 40)));
			}
			#endregion
			#endregion

			return mapFields.ToArray();
		}

        #region 目前沒人用，暫時不提供
        ///// <summary>
        ///// 設定上傳資料相關欄位的值，未指定的欄位將被設為 null
        ///// </summary>
        ///// <param name="mapFields"></param>
        ///// <returns></returns>
        //internal bool SetMapFields(ICollection<TxtMapField> mapFields)
        //{
        //    if (mapFields == null)
        //    {
        //        mapFields = new TxtMapField[0];
        //    }

        //    bool isOK = true;
        //    TxtMapField mapField = null;

        //    #region 學生資料對照欄位
        //    {
        //        #region StuId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuId);
        //        if (mapField == null)
        //        {
        //            this.StuIdS = null;
        //            this.StuIdL = null;
        //        }
        //        else
        //        {
        //            this.StuIdS = mapField.Start;
        //            this.StuIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuName);
        //        if (mapField == null)
        //        {
        //            this.StuNameS = null;
        //            this.StuNameL = null;
        //        }
        //        else
        //        {
        //            this.StuNameS = mapField.Start;
        //            this.StuNameL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuBirthday
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuBirthday);
        //        if (mapField == null)
        //        {
        //            this.StuBirthdayS = null;
        //            this.StuBirthdayL = null;
        //        }
        //        else
        //        {
        //            this.StuBirthdayS = mapField.Start;
        //            this.StuBirthdayL = mapField.Length;
        //        }
        //        #endregion

        //        #region IdNumber
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdNumber);
        //        if (mapField == null)
        //        {
        //            this.IdNumberS = null;
        //            this.IdNumberL = null;
        //        }
        //        else
        //        {
        //            this.IdNumberS = mapField.Start;
        //            this.IdNumberL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuTel
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuTel);
        //        if (mapField == null)
        //        {
        //            this.StuTelS = null;
        //            this.StuTelL = null;
        //        }
        //        else
        //        {
        //            this.StuTelS = mapField.Start;
        //            this.StuTelL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuAddcode
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuAddcode);
        //        if (mapField == null)
        //        {
        //            this.StuAddcodeS = null;
        //            this.StuAddcodeL = null;
        //        }
        //        else
        //        {
        //            this.StuAddcodeS = mapField.Start;
        //            this.StuAddcodeL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuAdd
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuAdd);
        //        if (mapField == null)
        //        {
        //            this.StuAddS = null;
        //            this.StuAddL = null;
        //        }
        //        else
        //        {
        //            this.StuAddS = mapField.Start;
        //            this.StuAddL = mapField.Length;
        //        }
        //        #endregion

        //        #region Email
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.Email);
        //        if (mapField == null)
        //        {
        //            this.EmailS = null;
        //            this.EmailL = null;
        //        }
        //        else
        //        {
        //            this.EmailS = mapField.Start;
        //            this.EmailL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuParent
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuParent);
        //        if (mapField == null)
        //        {
        //            this.StuParentS = null;
        //            this.StuParentL = null;
        //        }
        //        else
        //        {
        //            this.StuParentS = mapField.Start;
        //            this.StuParentL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 學籍資料對照欄位
        //    {
        //        #region StuGrade
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuGrade);
        //        if (mapField == null)
        //        {
        //            this.StuGradeS = null;
        //            this.StuGradeL = null;
        //        }
        //        else
        //        {
        //            this.StuGradeS = mapField.Start;
        //            this.StuGradeL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuHid
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuHid);
        //        if (mapField == null)
        //        {
        //            this.StuHidS = null;
        //            this.StuHidL = null;
        //        }
        //        else
        //        {
        //            this.StuHidS = mapField.Start;
        //            this.StuHidL = mapField.Length;
        //        }
        //        #endregion

        //        #region ClassId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ClassId);
        //        if (mapField == null)
        //        {
        //            this.ClassIdS = null;
        //            this.ClassIdL = null;
        //        }
        //        else
        //        {
        //            this.ClassIdS = mapField.Start;
        //            this.ClassIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region ClassName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ClassName);
        //        if (mapField == null)
        //        {
        //            this.ClassNameS = null;
        //            this.ClassNameL = null;
        //        }
        //        else
        //        {
        //            this.ClassNameS = mapField.Start;
        //            this.ClassNameL = mapField.Length;
        //        }
        //        #endregion

        //        #region DeptId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeptId);
        //        if (mapField == null)
        //        {
        //            this.DeptIdS = null;
        //            this.DeptIdL = null;
        //        }
        //        else
        //        {
        //            this.DeptIdS = mapField.Start;
        //            this.DeptIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region DeptName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeptName);
        //        if (mapField == null)
        //        {
        //            this.DeptNameS = null;
        //            this.DeptNameL = null;
        //        }
        //        else
        //        {
        //            this.DeptNameS = mapField.Start;
        //            this.DeptNameL = mapField.Length;
        //        }
        //        #endregion

        //        #region CollegeId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.CollegeId);
        //        if (mapField == null)
        //        {
        //            this.CollegeIdS = null;
        //            this.CollegeIdL = null;
        //        }
        //        else
        //        {
        //            this.CollegeIdS = mapField.Start;
        //            this.CollegeIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region CollegeName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.CollegeName);
        //        if (mapField == null)
        //        {
        //            this.CollegeNameS = null;
        //            this.CollegeNameL = null;
        //        }
        //        else
        //        {
        //            this.CollegeNameS = mapField.Start;
        //            this.CollegeNameL = mapField.Length;
        //        }
        //        #endregion

        //        #region MajorId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.MajorId);
        //        if (mapField == null)
        //        {
        //            this.MajorIdS = null;
        //            this.MajorIdL = null;
        //        }
        //        else
        //        {
        //            this.MajorIdS = mapField.Start;
        //            this.MajorIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region MajorName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.MajorName);
        //        if (mapField == null)
        //        {
        //            this.MajorNameS = null;
        //            this.MajorNameL = null;
        //        }
        //        else
        //        {
        //            this.MajorNameS = mapField.Start;
        //            this.MajorNameL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 減免、就貸、住宿對照欄位
        //    {
        //        #region ReduceId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ReduceId);
        //        if (mapField == null)
        //        {
        //            this.ReduceIdS = null;
        //            this.ReduceIdL = null;
        //        }
        //        else
        //        {
        //            this.ReduceIdS = mapField.Start;
        //            this.ReduceIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region ReduceName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ReduceName);
        //        if (mapField == null)
        //        {
        //            this.ReduceNameS = null;
        //            this.ReduceNameL = null;
        //        }
        //        else
        //        {
        //            this.ReduceNameS = mapField.Start;
        //            this.ReduceNameL = mapField.Length;
        //        }
        //        #endregion

        //        #region LoanId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.LoanId);
        //        if (mapField == null)
        //        {
        //            this.LoanIdS = null;
        //            this.LoanIdL = null;
        //        }
        //        else
        //        {
        //            this.LoanIdS = mapField.Start;
        //            this.LoanIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region LoanName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.LoanName);
        //        if (mapField == null)
        //        {
        //            this.LoanNameS = null;
        //            this.LoanNameL = null;
        //        }
        //        else
        //        {
        //            this.LoanNameS = mapField.Start;
        //            this.LoanNameL = mapField.Length;
        //        }
        //        #endregion

        //        #region DormId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DormId);
        //        if (mapField == null)
        //        {
        //            this.DormIdS = null;
        //            this.DormIdL = null;
        //        }
        //        else
        //        {
        //            this.DormIdS = mapField.Start;
        //            this.DormIdL = mapField.Length;
        //        }
        //        #endregion

        //        #region DormName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DormName);
        //        if (mapField == null)
        //        {
        //            this.DormNameS = null;
        //            this.DormNameL = null;
        //        }
        //        else
        //        {
        //            this.DormNameS = mapField.Start;
        //            this.DormNameL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 身分註記對照欄位
        //    {
        //        #region IdentifyId1
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId1);
        //        if (mapField == null)
        //        {
        //            this.IdentifyId1S = null;
        //            this.IdentifyId1L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyId1S = mapField.Start;
        //            this.IdentifyId1L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyName1
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName1);
        //        if (mapField == null)
        //        {
        //            this.IdentifyName1S = null;
        //            this.IdentifyName1L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyName1S = mapField.Start;
        //            this.IdentifyName1L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyId2
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId2);
        //        if (mapField == null)
        //        {
        //            this.IdentifyId2S = null;
        //            this.IdentifyId2L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyId2S = mapField.Start;
        //            this.IdentifyId2L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyName2
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName2);
        //        if (mapField == null)
        //        {
        //            this.IdentifyName2S = null;
        //            this.IdentifyName2L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyName2S = mapField.Start;
        //            this.IdentifyName2L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyId3
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId3);
        //        if (mapField == null)
        //        {
        //            this.IdentifyId3S = null;
        //            this.IdentifyId3L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyId3S = mapField.Start;
        //            this.IdentifyId3L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyName3
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName3);
        //        if (mapField == null)
        //        {
        //            this.IdentifyName3S = null;
        //            this.IdentifyName3L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyName3S = mapField.Start;
        //            this.IdentifyName3L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyId4
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId4);
        //        if (mapField == null)
        //        {
        //            this.IdentifyId4S = null;
        //            this.IdentifyId4L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyId4S = mapField.Start;
        //            this.IdentifyId4L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyName4
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName4);
        //        if (mapField == null)
        //        {
        //            this.IdentifyName4S = null;
        //            this.IdentifyName4L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyName4S = mapField.Start;
        //            this.IdentifyName4L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyId5
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId5);
        //        if (mapField == null)
        //        {
        //            this.IdentifyId5S = null;
        //            this.IdentifyId5L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyId5S = mapField.Start;
        //            this.IdentifyId5L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyName5
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName5);
        //        if (mapField == null)
        //        {
        //            this.IdentifyName5S = null;
        //            this.IdentifyName5L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyName5S = mapField.Start;
        //            this.IdentifyName5L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyId6
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId6);
        //        if (mapField == null)
        //        {
        //            this.IdentifyId6S = null;
        //            this.IdentifyId6L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyId6S = mapField.Start;
        //            this.IdentifyId6L = mapField.Length;
        //        }
        //        #endregion

        //        #region IdentifyName6
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName6);
        //        if (mapField == null)
        //        {
        //            this.IdentifyName6S = null;
        //            this.IdentifyName6L = null;
        //        }
        //        else
        //        {
        //            this.IdentifyName6S = mapField.Start;
        //            this.IdentifyName6L = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 收入科目金額對照欄位 (StudentReceiveEntity)
        //    {
        //        string[] fields = new string[] {
        //            MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3, MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5,
        //            MappingreXlsmdbEntity.Field.Receive6, MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9, MappingreXlsmdbEntity.Field.Receive10,
        //            MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12, MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
        //            MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18, MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20,
        //            MappingreXlsmdbEntity.Field.Receive21, MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24, MappingreXlsmdbEntity.Field.Receive25,
        //            MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27, MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
        //            MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33, MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35,
        //            MappingreXlsmdbEntity.Field.Receive36, MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39, MappingreXlsmdbEntity.Field.Receive40
        //        };

        //        int no = 0;
        //        foreach (string field in fields)
        //        {
        //            no++;
        //            string fieldSName = string.Format("Receive{0}S", no);
        //            string fieldLName = string.Format("Receive{0}L", no);
        //            mapField = mapFields.First(x => x.Key == field);
        //            if (mapField == null)
        //            {
        //                isOK &= this.SetValue(fieldSName, null).IsSuccess;
        //                isOK &= this.SetValue(fieldLName, null).IsSuccess;
        //            }
        //            else
        //            {
        //                isOK &= this.SetValue(fieldSName, mapField.Start).IsSuccess;
        //                isOK &= this.SetValue(fieldLName, mapField.Length).IsSuccess;
        //            }
        //        }
        //    }
        //    #endregion

        //    #region 其他對照欄位 (StudentReceiveEntity)
        //    {
        //        #region StuCredit
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuCredit);
        //        if (mapField == null)
        //        {
        //            this.StuCreditS = null;
        //            this.StuCreditL = null;
        //        }
        //        else
        //        {
        //            this.StuCreditS = mapField.Start;
        //            this.StuCreditL = mapField.Length;
        //        }
        //        #endregion

        //        #region StuHour
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuHour);
        //        if (mapField == null)
        //        {
        //            this.StuHourS = null;
        //            this.StuHourL = null;
        //        }
        //        else
        //        {
        //            this.StuHourS = mapField.Start;
        //            this.StuHourL = mapField.Length;
        //        }
        //        #endregion

        //        #region LoanAmount
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.LoanAmount);
        //        if (mapField == null)
        //        {
        //            this.LoanAmountS = null;
        //            this.LoanAmountL = null;
        //        }
        //        else
        //        {
        //            this.LoanAmountS = mapField.Start;
        //            this.LoanAmountL = mapField.Length;
        //        }
        //        #endregion

        //        #region ReceiveAmount
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ReceiveAmount);
        //        if (mapField == null)
        //        {
        //            this.ReceiveAmountS = null;
        //            this.ReceiveAmountL = null;
        //        }
        //        else
        //        {
        //            this.ReceiveAmountS = mapField.Start;
        //            this.ReceiveAmountL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 學分基準、課程、學分數對照欄位 (StudentCourseEntity)
        //    {
        //        string[] creditIdFileds = new string[40] {
        //            MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2,
        //            MappingreXlsmdbEntity.Field.CreditId3, MappingreXlsmdbEntity.Field.CreditId4,
        //            MappingreXlsmdbEntity.Field.CreditId5, MappingreXlsmdbEntity.Field.CreditId6,
        //            MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8,
        //            MappingreXlsmdbEntity.Field.CreditId9, MappingreXlsmdbEntity.Field.CreditId10,
        //            MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12,
        //            MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14,
        //            MappingreXlsmdbEntity.Field.CreditId15, MappingreXlsmdbEntity.Field.CreditId16,
        //            MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18,
        //            MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20,
        //            MappingreXlsmdbEntity.Field.CreditId21, MappingreXlsmdbEntity.Field.CreditId22,
        //            MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24,
        //            MappingreXlsmdbEntity.Field.CreditId25, MappingreXlsmdbEntity.Field.CreditId26,
        //            MappingreXlsmdbEntity.Field.CreditId27, MappingreXlsmdbEntity.Field.CreditId28,
        //            MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
        //            MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32,
        //            MappingreXlsmdbEntity.Field.CreditId33, MappingreXlsmdbEntity.Field.CreditId34,
        //            MappingreXlsmdbEntity.Field.CreditId35, MappingreXlsmdbEntity.Field.CreditId36,
        //            MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38,
        //            MappingreXlsmdbEntity.Field.CreditId39, MappingreXlsmdbEntity.Field.CreditId40
        //        };
        //        string[] courseIdFileds = new string[40] {
        //            MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2,
        //            MappingreXlsmdbEntity.Field.CourseId3, MappingreXlsmdbEntity.Field.CourseId4,
        //            MappingreXlsmdbEntity.Field.CourseId5, MappingreXlsmdbEntity.Field.CourseId6,
        //            MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8,
        //            MappingreXlsmdbEntity.Field.CourseId9, MappingreXlsmdbEntity.Field.CourseId10,
        //            MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12,
        //            MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14,
        //            MappingreXlsmdbEntity.Field.CourseId15, MappingreXlsmdbEntity.Field.CourseId16,
        //            MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18,
        //            MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20,
        //            MappingreXlsmdbEntity.Field.CourseId21, MappingreXlsmdbEntity.Field.CourseId22,
        //            MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24,
        //            MappingreXlsmdbEntity.Field.CourseId25, MappingreXlsmdbEntity.Field.CourseId26,
        //            MappingreXlsmdbEntity.Field.CourseId27, MappingreXlsmdbEntity.Field.CourseId28,
        //            MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
        //            MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32,
        //            MappingreXlsmdbEntity.Field.CourseId33, MappingreXlsmdbEntity.Field.CourseId34,
        //            MappingreXlsmdbEntity.Field.CourseId35, MappingreXlsmdbEntity.Field.CourseId36,
        //            MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38,
        //            MappingreXlsmdbEntity.Field.CourseId39, MappingreXlsmdbEntity.Field.CourseId40
        //        };
        //        string[] creditFileds = new string[40] {
        //            MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2,
        //            MappingreXlsmdbEntity.Field.Credit3, MappingreXlsmdbEntity.Field.Credit4,
        //            MappingreXlsmdbEntity.Field.Credit5, MappingreXlsmdbEntity.Field.Credit6,
        //            MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8,
        //            MappingreXlsmdbEntity.Field.Credit9, MappingreXlsmdbEntity.Field.Credit10,
        //            MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12,
        //            MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14,
        //            MappingreXlsmdbEntity.Field.Credit15, MappingreXlsmdbEntity.Field.Credit16,
        //            MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18,
        //            MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20,
        //            MappingreXlsmdbEntity.Field.Credit21, MappingreXlsmdbEntity.Field.Credit22,
        //            MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24,
        //            MappingreXlsmdbEntity.Field.Credit25, MappingreXlsmdbEntity.Field.Credit26,
        //            MappingreXlsmdbEntity.Field.Credit27, MappingreXlsmdbEntity.Field.Credit28,
        //            MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
        //            MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32,
        //            MappingreXlsmdbEntity.Field.Credit33, MappingreXlsmdbEntity.Field.Credit34,
        //            MappingreXlsmdbEntity.Field.Credit35, MappingreXlsmdbEntity.Field.Credit36,
        //            MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38,
        //            MappingreXlsmdbEntity.Field.Credit39, MappingreXlsmdbEntity.Field.Credit40
        //        };
        //        for (int idx = 0; idx < 40; idx++)
        //        {
        //            int no = idx + 1;

        //            #region CreditId
        //            {
        //                string field = creditIdFileds[idx];
        //                string fieldSName = string.Format("CreditId{0}S", no);
        //                string fieldLName = string.Format("CreditId{0}L", no);
        //                mapField = mapFields.First(x => x.Key == field);
        //                if (mapField == null)
        //                {
        //                    isOK &= this.SetValue(fieldSName, null).IsSuccess;
        //                    isOK &= this.SetValue(fieldLName, null).IsSuccess;
        //                }
        //                else
        //                {
        //                    isOK &= this.SetValue(fieldSName, mapField.Start).IsSuccess;
        //                    isOK &= this.SetValue(fieldLName, mapField.Length).IsSuccess;
        //                }
        //            }
        //            #endregion

        //            #region CourseId
        //            {
        //                string field = creditIdFileds[idx];
        //                string fieldSName = string.Format("CourseId{0}S", no);
        //                string fieldLName = string.Format("CourseId{0}L", no);
        //                mapField = mapFields.First(x => x.Key == field);
        //                if (mapField == null)
        //                {
        //                    isOK &= this.SetValue(fieldSName, null).IsSuccess;
        //                    isOK &= this.SetValue(fieldLName, null).IsSuccess;
        //                }
        //                else
        //                {
        //                    isOK &= this.SetValue(fieldSName, mapField.Start).IsSuccess;
        //                    isOK &= this.SetValue(fieldLName, mapField.Length).IsSuccess;
        //                }
        //            }
        //            #endregion

        //            #region Credit
        //            {
        //                string field = creditIdFileds[idx];
        //                string fieldSName = string.Format("Credit{0}S", no);
        //                string fieldLName = string.Format("Credit{0}L", no);
        //                mapField = mapFields.First(x => x.Key == field);
        //                if (mapField == null)
        //                {
        //                    isOK &= this.SetValue(fieldSName, null).IsSuccess;
        //                    isOK &= this.SetValue(fieldLName, null).IsSuccess;
        //                }
        //                else
        //                {
        //                    isOK &= this.SetValue(fieldSName, mapField.Start).IsSuccess;
        //                    isOK &= this.SetValue(fieldLName, mapField.Length).IsSuccess;
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    #endregion

        //    #region Remark (StudentReceiveEntity)
        //    {
        //        #region Remark
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.Remark);
        //        if (mapField == null)
        //        {
        //            this.RemarkS = null;
        //            this.RemarkL = null;
        //        }
        //        else
        //        {
        //            this.RemarkS = mapField.Start;
        //            this.RemarkL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 扣款資料相關對照欄位 (StudentReceiveEntity)
        //    {
        //        #region DeductBankid
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductBankid);
        //        if (mapField == null)
        //        {
        //            this.DeductBankidS = null;
        //            this.DeductBankidL = null;
        //        }
        //        else
        //        {
        //            this.DeductBankidS = mapField.Start;
        //            this.DeductBankidL = mapField.Length;
        //        }
        //        #endregion

        //        #region DeductAccountno
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductAccountno);
        //        if (mapField == null)
        //        {
        //            this.DeductAccountnoS = null;
        //            this.DeductAccountnoL = null;
        //        }
        //        else
        //        {
        //            this.DeductAccountnoS = mapField.Start;
        //            this.DeductAccountnoL = mapField.Length;
        //        }
        //        #endregion

        //        #region DeductAccountname
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductAccountname);
        //        if (mapField == null)
        //        {
        //            this.DeductAccountnameS = null;
        //            this.DeductAccountnameL = null;
        //        }
        //        else
        //        {
        //            this.DeductAccountnameS = mapField.Start;
        //            this.DeductAccountnameL = mapField.Length;
        //        }
        //        #endregion

        //        #region DeductAccountid
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductAccountid);
        //        if (mapField == null)
        //        {
        //            this.DeductAccountidS = null;
        //            this.DeductAccountidL = null;
        //        }
        //        else
        //        {
        //            this.DeductAccountidS = mapField.Start;
        //            this.DeductAccountidL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 虛擬帳號資料相關對照欄位 (StudentReceiveEntity)
        //    {
        //        #region SeriorNo
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.SeriorNo);
        //        if (mapField == null)
        //        {
        //            this.SeriorNoS = null;
        //            this.SeriorNoL = null;
        //        }
        //        else
        //        {
        //            this.SeriorNoS = mapField.Start;
        //            this.SeriorNoL = mapField.Length;
        //        }
        //        #endregion

        //        #region CancelNo
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.CancelNo);
        //        if (mapField == null)
        //        {
        //            this.CancelNoS = null;
        //            this.CancelNoL = null;
        //        }
        //        else
        //        {
        //            this.CancelNoS = mapField.Start;
        //            this.CancelNoL = mapField.Length;
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    #region 備註對照欄位 (StudentReceiveEntity)
        //    {
        //        string[] memoFileds = new string[MemoCount] {
        //            MappingreXlsmdbEntity.Field.Memo01, MappingreXlsmdbEntity.Field.Memo02,
        //            MappingreXlsmdbEntity.Field.Memo03, MappingreXlsmdbEntity.Field.Memo04,
        //            MappingreXlsmdbEntity.Field.Memo05, MappingreXlsmdbEntity.Field.Memo06,
        //            MappingreXlsmdbEntity.Field.Memo07, MappingreXlsmdbEntity.Field.Memo08,
        //            MappingreXlsmdbEntity.Field.Memo09, MappingreXlsmdbEntity.Field.Memo10,
        //            MappingreXlsmdbEntity.Field.Memo11, MappingreXlsmdbEntity.Field.Memo12,
        //            MappingreXlsmdbEntity.Field.Memo13, MappingreXlsmdbEntity.Field.Memo14,
        //            MappingreXlsmdbEntity.Field.Memo15, MappingreXlsmdbEntity.Field.Memo16,
        //            MappingreXlsmdbEntity.Field.Memo17, MappingreXlsmdbEntity.Field.Memo18,
        //            MappingreXlsmdbEntity.Field.Memo19, MappingreXlsmdbEntity.Field.Memo20,
        //            MappingreXlsmdbEntity.Field.Memo21
        //        };
        //        int no = 0;
        //        foreach (string field in memoFileds)
        //        {
        //            no++;
        //            string fieldSName = string.Format("Memo{0}S", no);
        //            string fieldLName = string.Format("Memo{0}L", no);
        //            mapField = mapFields.First(x => x.Key == field);
        //            if (mapField == null)
        //            {
        //                isOK &= this.SetValue(fieldSName, null).IsSuccess;
        //                isOK &= this.SetValue(fieldLName, null).IsSuccess;
        //            }
        //            else
        //            {
        //                isOK &= this.SetValue(fieldSName, mapField.Start).IsSuccess;
        //                isOK &= this.SetValue(fieldLName, mapField.Length).IsSuccess;
        //            }
        //        }
        //    }
        //    #endregion

        //    return isOK;
        //}
        #endregion
        #endregion
    }
}
