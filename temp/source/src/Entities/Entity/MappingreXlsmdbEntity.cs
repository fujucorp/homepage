/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:35:58
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
	/// 上傳學生繳費資料 XLS 對照檔 資料承載類別
	/// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class MappingreXlsmdbEntity : Entity
    {
        #region Const
        /// <summary>
        /// 備註數量 (21)
        /// </summary>
        public const int MemoCount = 21;
        #endregion

        public const string TABLE_NAME = "MappingRe_Xlsmdb";

        #region Field Name Const Class
        /// <summary>
        /// MappingreXlsmdbEntity 欄位名稱定義抽象類別
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
            /// 學號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuId = "Stu_Id";

            /// <summary>
            /// 學生姓名欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuName = "Stu_Name";

            /// <summary>
            /// 學生生日欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuBirthday = "Stu_Birthday";

            /// <summary>
            /// 學生身份證欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdNumber = "Id_Number";

            /// <summary>
            /// 學生電話欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuTel = "Stu_Tel";

            /// <summary>
            /// 學生郵遞區號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuAddcode = "Stu_Addcode";

            /// <summary>
            /// 學生地址欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuAdd = "Stu_Add";

            /// <summary>
            /// 學生Email欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Email = "Email";

            /// <summary>
            /// 學生家長名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuParent = "Stu_Parent";
            #endregion

            #region [MDY:20160131] 增加資料序號與繳款期限對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 資料序號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string OldSeq = "Old_Seq";

            /// <summary>
            /// 繳款期限欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string PayDueDate = "Pay_Due_Date";
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 資料序號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string NCCardFlag = "NCCardFlag";
            #endregion

            #region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
            /// <summary>
            /// 年級代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuGrade = "Stu_Grade";

            /// <summary>
            /// 部別代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string DeptId = "Dept_Id";

            /// <summary>
            /// 部別名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string DeptName = "Dept_Name";

            /// <summary>
            /// 院別代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string CollegeId = "College_Id";

            /// <summary>
            /// 院別名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string CollegeName = "College_Name";

            /// <summary>
            /// 系所代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string MajorId = "Major_Id";

            /// <summary>
            /// 系所名稱欄位對照設定
            /// </summary>
            public const string MajorName = "Major_Name";

            /// <summary>
            /// 班別代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ClassId = "Class_Id";

            /// <summary>
            /// 班別名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ClassName = "Class_Name";

            /// <summary>
            /// 總學分數欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuCredit = "Stu_Credit";

            /// <summary>
            /// 上課時數欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuHour = "Stu_Hour";
            #endregion

            #region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
            /// <summary>
            /// 減免代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ReduceId = "Reduce_Id";

            /// <summary>
            /// 減免名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ReduceName = "Reduce_Name";

            /// <summary>
            /// 住宿代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string DormId = "Dorm_Id";

            /// <summary>
            /// 住宿名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string DormName = "Dorm_Name";

            /// <summary>
            /// 就貸代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string LoanId = "Loan_Id";

            /// <summary>
            /// 就貸名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string LoanName = "Loan_Name";
            #endregion

            #region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
            /// <summary>
            /// 身份註記01代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyId1 = "Identify_Id1";

            /// <summary>
            /// 身份註記01名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName1 = "Identify_Name1";

            /// <summary>
            /// 身份註記02代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyId2 = "Identify_Id2";

            /// <summary>
            /// 身份註記02名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName2 = "Identify_Name2";

            /// <summary>
            /// 身份註記03代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyId3 = "Identify_Id3";

            /// <summary>
            /// 身份註記03名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName3 = "Identify_Name3";

            /// <summary>
            /// 身份註記04代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyId4 = "Identify_Id4";

            /// <summary>
            /// 身份註記04名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName4 = "Identify_Name4";

            /// <summary>
            /// 身份註記05代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyId5 = "Identify_Id5";

            /// <summary>
            /// 身份註記05名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName5 = "Identify_Name5";

            /// <summary>
            /// 身份註記06代碼欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyId6 = "Identify_Id6";

            /// <summary>
            /// 身份註記06名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyName6 = "Identify_Name6";
            #endregion

            #region 繳費資料對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 收入科目01金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive1 = "Receive_1";

            /// <summary>
            /// 收入科目02金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive2 = "Receive_2";

            /// <summary>
            /// 收入科目03金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive3 = "Receive_3";

            /// <summary>
            /// 收入科目04金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive4 = "Receive_4";

            /// <summary>
            /// 收入科目05金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive5 = "Receive_5";

            /// <summary>
            /// 收入科目06金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive6 = "Receive_6";

            /// <summary>
            /// 收入科目07金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive7 = "Receive_7";

            /// <summary>
            /// 收入科目08金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive8 = "Receive_8";

            /// <summary>
            /// 收入科目09金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive9 = "Receive_9";

            /// <summary>
            /// 收入科目10金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive10 = "Receive_10";

            /// <summary>
            /// 收入科目11金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive11 = "Receive_11";

            /// <summary>
            /// 收入科目12金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive12 = "Receive_12";

            /// <summary>
            /// 收入科目13金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive13 = "Receive_13";

            /// <summary>
            /// 收入科目14金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive14 = "Receive_14";

            /// <summary>
            /// 收入科目15金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive15 = "Receive_15";

            /// <summary>
            /// 收入科目16金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive16 = "Receive_16";

            /// <summary>
            /// 收入科目17金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive17 = "Receive_17";

            /// <summary>
            /// 收入科目18金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive18 = "Receive_18";

            /// <summary>
            /// 收入科目19金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive19 = "Receive_19";

            /// <summary>
            /// 收入科目20金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive20 = "Receive_20";

            /// <summary>
            /// 收入科目21金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive21 = "Receive_21";

            /// <summary>
            /// 收入科目22金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive22 = "Receive_22";

            /// <summary>
            /// 收入科目23金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive23 = "Receive_23";

            /// <summary>
            /// 收入科目24金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive24 = "Receive_24";

            /// <summary>
            /// 收入科目25金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive25 = "Receive_25";

            /// <summary>
            /// 收入科目26金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive26 = "Receive_26";

            /// <summary>
            /// 收入科目27金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive27 = "Receive_27";

            /// <summary>
            /// 收入科目28金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive28 = "Receive_28";

            /// <summary>
            /// 收入科目29金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive29 = "Receive_29";

            /// <summary>
            /// 收入科目30金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive30 = "Receive_30";

            /// <summary>
            /// 收入科目31金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive31 = "Receive_31";

            /// <summary>
            /// 收入科目32金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive32 = "Receive_32";

            /// <summary>
            /// 收入科目33金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive33 = "Receive_33";

            /// <summary>
            /// 收入科目34金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive34 = "Receive_34";

            /// <summary>
            /// 收入科目35金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive35 = "Receive_35";

            /// <summary>
            /// 收入科目36金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive36 = "Receive_36";

            /// <summary>
            /// 收入科目37金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive37 = "Receive_37";

            /// <summary>
            /// 收入科目38金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive38 = "Receive_38";

            /// <summary>
            /// 收入科目39金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive39 = "Receive_39";

            /// <summary>
            /// 收入科目40金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Receive40 = "Receive_40";

            /// <summary>
            /// 可貸金額欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string LoanAmount = "Loan_Amount";

            /// <summary>
            /// 繳費金額合計欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// 座號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string StuHid = "Stu_Hid";

            /// <summary>
            /// Remark (土銀沒用到) 欄位名稱常數定義
            /// </summary>
            public const string Remark = "Remark";

            /// <summary>
            /// 流水號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string SeriorNo = "Serior_No";

            /// <summary>
            /// 銷帳編號欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string CancelNo = "Cancel_No";
            #endregion

            #region 其他繳費資料對照欄位
            /// <summary>
            /// Credit_Id1 欄位名稱常數定義
            /// </summary>
            public const string CreditId1 = "Credit_Id1";

            /// <summary>
            /// Course_Id1 欄位名稱常數定義
            /// </summary>
            public const string CourseId1 = "Course_Id1";

            /// <summary>
            /// Credit1 欄位名稱常數定義
            /// </summary>
            public const string Credit1 = "Credit1";

            /// <summary>
            /// Credit_Id2 欄位名稱常數定義
            /// </summary>
            public const string CreditId2 = "Credit_Id2";

            /// <summary>
            /// Course_Id2 欄位名稱常數定義
            /// </summary>
            public const string CourseId2 = "Course_Id2";

            /// <summary>
            /// Credit2 欄位名稱常數定義
            /// </summary>
            public const string Credit2 = "Credit2";

            /// <summary>
            /// Credit_Id3 欄位名稱常數定義
            /// </summary>
            public const string CreditId3 = "Credit_Id3";

            /// <summary>
            /// Course_Id3 欄位名稱常數定義
            /// </summary>
            public const string CourseId3 = "Course_Id3";

            /// <summary>
            /// Credit3 欄位名稱常數定義
            /// </summary>
            public const string Credit3 = "Credit3";

            /// <summary>
            /// Credit_Id4 欄位名稱常數定義
            /// </summary>
            public const string CreditId4 = "Credit_Id4";

            /// <summary>
            /// Course_Id4 欄位名稱常數定義
            /// </summary>
            public const string CourseId4 = "Course_Id4";

            /// <summary>
            /// Credit4 欄位名稱常數定義
            /// </summary>
            public const string Credit4 = "Credit4";

            /// <summary>
            /// Credit_Id5 欄位名稱常數定義
            /// </summary>
            public const string CreditId5 = "Credit_Id5";

            /// <summary>
            /// Course_Id5 欄位名稱常數定義
            /// </summary>
            public const string CourseId5 = "Course_Id5";

            /// <summary>
            /// Credit5 欄位名稱常數定義
            /// </summary>
            public const string Credit5 = "Credit5";

            /// <summary>
            /// Credit_Id6 欄位名稱常數定義
            /// </summary>
            public const string CreditId6 = "Credit_Id6";

            /// <summary>
            /// Course_Id6 欄位名稱常數定義
            /// </summary>
            public const string CourseId6 = "Course_Id6";

            /// <summary>
            /// Credit6 欄位名稱常數定義
            /// </summary>
            public const string Credit6 = "Credit6";

            /// <summary>
            /// Credit_Id7 欄位名稱常數定義
            /// </summary>
            public const string CreditId7 = "Credit_Id7";

            /// <summary>
            /// Course_Id7 欄位名稱常數定義
            /// </summary>
            public const string CourseId7 = "Course_Id7";

            /// <summary>
            /// Credit7 欄位名稱常數定義
            /// </summary>
            public const string Credit7 = "Credit7";

            /// <summary>
            /// Credit_Id8 欄位名稱常數定義
            /// </summary>
            public const string CreditId8 = "Credit_Id8";

            /// <summary>
            /// Course_Id8 欄位名稱常數定義
            /// </summary>
            public const string CourseId8 = "Course_Id8";

            /// <summary>
            /// Credit8 欄位名稱常數定義
            /// </summary>
            public const string Credit8 = "Credit8";

            /// <summary>
            /// Credit_Id9 欄位名稱常數定義
            /// </summary>
            public const string CreditId9 = "Credit_Id9";

            /// <summary>
            /// Course_Id9 欄位名稱常數定義
            /// </summary>
            public const string CourseId9 = "Course_Id9";

            /// <summary>
            /// Credit9 欄位名稱常數定義
            /// </summary>
            public const string Credit9 = "Credit9";

            /// <summary>
            /// Credit_Id10 欄位名稱常數定義
            /// </summary>
            public const string CreditId10 = "Credit_Id10";

            /// <summary>
            /// Course_Id10 欄位名稱常數定義
            /// </summary>
            public const string CourseId10 = "Course_Id10";

            /// <summary>
            /// Credit10 欄位名稱常數定義
            /// </summary>
            public const string Credit10 = "Credit10";

            /// <summary>
            /// Credit_Id11 欄位名稱常數定義
            /// </summary>
            public const string CreditId11 = "Credit_Id11";

            /// <summary>
            /// Course_Id11 欄位名稱常數定義
            /// </summary>
            public const string CourseId11 = "Course_Id11";

            /// <summary>
            /// Credit11 欄位名稱常數定義
            /// </summary>
            public const string Credit11 = "Credit11";

            /// <summary>
            /// Credit_Id12 欄位名稱常數定義
            /// </summary>
            public const string CreditId12 = "Credit_Id12";

            /// <summary>
            /// Course_Id12 欄位名稱常數定義
            /// </summary>
            public const string CourseId12 = "Course_Id12";

            /// <summary>
            /// Credit12 欄位名稱常數定義
            /// </summary>
            public const string Credit12 = "Credit12";

            /// <summary>
            /// Credit_Id13 欄位名稱常數定義
            /// </summary>
            public const string CreditId13 = "Credit_Id13";

            /// <summary>
            /// Course_Id13 欄位名稱常數定義
            /// </summary>
            public const string CourseId13 = "Course_Id13";

            /// <summary>
            /// Credit13 欄位名稱常數定義
            /// </summary>
            public const string Credit13 = "Credit13";

            /// <summary>
            /// Credit_Id14 欄位名稱常數定義
            /// </summary>
            public const string CreditId14 = "Credit_Id14";

            /// <summary>
            /// Course_Id14 欄位名稱常數定義
            /// </summary>
            public const string CourseId14 = "Course_Id14";

            /// <summary>
            /// Credit14 欄位名稱常數定義
            /// </summary>
            public const string Credit14 = "Credit14";

            /// <summary>
            /// Credit_Id15 欄位名稱常數定義
            /// </summary>
            public const string CreditId15 = "Credit_Id15";

            /// <summary>
            /// Course_Id15 欄位名稱常數定義
            /// </summary>
            public const string CourseId15 = "Course_Id15";

            /// <summary>
            /// Credit15 欄位名稱常數定義
            /// </summary>
            public const string Credit15 = "Credit15";

            /// <summary>
            /// Credit_Id16 欄位名稱常數定義
            /// </summary>
            public const string CreditId16 = "Credit_Id16";

            /// <summary>
            /// Course_Id16 欄位名稱常數定義
            /// </summary>
            public const string CourseId16 = "Course_Id16";

            /// <summary>
            /// Credit16 欄位名稱常數定義
            /// </summary>
            public const string Credit16 = "Credit16";

            /// <summary>
            /// Credit_Id17 欄位名稱常數定義
            /// </summary>
            public const string CreditId17 = "Credit_Id17";

            /// <summary>
            /// Course_Id17 欄位名稱常數定義
            /// </summary>
            public const string CourseId17 = "Course_Id17";

            /// <summary>
            /// Credit17 欄位名稱常數定義
            /// </summary>
            public const string Credit17 = "Credit17";

            /// <summary>
            /// Credit_Id18 欄位名稱常數定義
            /// </summary>
            public const string CreditId18 = "Credit_Id18";

            /// <summary>
            /// Course_Id18 欄位名稱常數定義
            /// </summary>
            public const string CourseId18 = "Course_Id18";

            /// <summary>
            /// Credit18 欄位名稱常數定義
            /// </summary>
            public const string Credit18 = "Credit18";

            /// <summary>
            /// Credit_Id19 欄位名稱常數定義
            /// </summary>
            public const string CreditId19 = "Credit_Id19";

            /// <summary>
            /// Course_Id19 欄位名稱常數定義
            /// </summary>
            public const string CourseId19 = "Course_Id19";

            /// <summary>
            /// Credit19 欄位名稱常數定義
            /// </summary>
            public const string Credit19 = "Credit19";

            /// <summary>
            /// Credit_Id20 欄位名稱常數定義
            /// </summary>
            public const string CreditId20 = "Credit_Id20";

            /// <summary>
            /// Course_Id20 欄位名稱常數定義
            /// </summary>
            public const string CourseId20 = "Course_Id20";

            /// <summary>
            /// Credit20 欄位名稱常數定義
            /// </summary>
            public const string Credit20 = "Credit20";

            /// <summary>
            /// Credit_Id21 欄位名稱常數定義
            /// </summary>
            public const string CreditId21 = "Credit_Id21";

            /// <summary>
            /// Course_Id21 欄位名稱常數定義
            /// </summary>
            public const string CourseId21 = "Course_Id21";

            /// <summary>
            /// Credit21 欄位名稱常數定義
            /// </summary>
            public const string Credit21 = "Credit21";

            /// <summary>
            /// Credit_Id22 欄位名稱常數定義
            /// </summary>
            public const string CreditId22 = "Credit_Id22";

            /// <summary>
            /// Course_Id22 欄位名稱常數定義
            /// </summary>
            public const string CourseId22 = "Course_Id22";

            /// <summary>
            /// Credit22 欄位名稱常數定義
            /// </summary>
            public const string Credit22 = "Credit22";

            /// <summary>
            /// Credit_Id23 欄位名稱常數定義
            /// </summary>
            public const string CreditId23 = "Credit_Id23";

            /// <summary>
            /// Course_Id23 欄位名稱常數定義
            /// </summary>
            public const string CourseId23 = "Course_Id23";

            /// <summary>
            /// Credit23 欄位名稱常數定義
            /// </summary>
            public const string Credit23 = "Credit23";

            /// <summary>
            /// Credit_Id24 欄位名稱常數定義
            /// </summary>
            public const string CreditId24 = "Credit_Id24";

            /// <summary>
            /// Course_Id24 欄位名稱常數定義
            /// </summary>
            public const string CourseId24 = "Course_Id24";

            /// <summary>
            /// Credit24 欄位名稱常數定義
            /// </summary>
            public const string Credit24 = "Credit24";

            /// <summary>
            /// Credit_Id25 欄位名稱常數定義
            /// </summary>
            public const string CreditId25 = "Credit_Id25";

            /// <summary>
            /// Course_Id25 欄位名稱常數定義
            /// </summary>
            public const string CourseId25 = "Course_Id25";

            /// <summary>
            /// Credit25 欄位名稱常數定義
            /// </summary>
            public const string Credit25 = "Credit25";

            /// <summary>
            /// Credit_Id26 欄位名稱常數定義
            /// </summary>
            public const string CreditId26 = "Credit_Id26";

            /// <summary>
            /// Course_Id26 欄位名稱常數定義
            /// </summary>
            public const string CourseId26 = "Course_Id26";

            /// <summary>
            /// Credit26 欄位名稱常數定義
            /// </summary>
            public const string Credit26 = "Credit26";

            /// <summary>
            /// Credit_Id27 欄位名稱常數定義
            /// </summary>
            public const string CreditId27 = "Credit_Id27";

            /// <summary>
            /// Course_Id27 欄位名稱常數定義
            /// </summary>
            public const string CourseId27 = "Course_Id27";

            /// <summary>
            /// Credit27 欄位名稱常數定義
            /// </summary>
            public const string Credit27 = "Credit27";

            /// <summary>
            /// Credit_Id28 欄位名稱常數定義
            /// </summary>
            public const string CreditId28 = "Credit_Id28";

            /// <summary>
            /// Course_Id28 欄位名稱常數定義
            /// </summary>
            public const string CourseId28 = "Course_Id28";

            /// <summary>
            /// Credit28 欄位名稱常數定義
            /// </summary>
            public const string Credit28 = "Credit28";

            /// <summary>
            /// Credit_Id29 欄位名稱常數定義
            /// </summary>
            public const string CreditId29 = "Credit_Id29";

            /// <summary>
            /// Course_Id29 欄位名稱常數定義
            /// </summary>
            public const string CourseId29 = "Course_Id29";

            /// <summary>
            /// Credit29 欄位名稱常數定義
            /// </summary>
            public const string Credit29 = "Credit29";

            /// <summary>
            /// Credit_Id30 欄位名稱常數定義
            /// </summary>
            public const string CreditId30 = "Credit_Id30";

            /// <summary>
            /// Course_Id30 欄位名稱常數定義
            /// </summary>
            public const string CourseId30 = "Course_Id30";

            /// <summary>
            /// Credit30 欄位名稱常數定義
            /// </summary>
            public const string Credit30 = "Credit30";

            /// <summary>
            /// Credit_Id31 欄位名稱常數定義
            /// </summary>
            public const string CreditId31 = "Credit_Id31";

            /// <summary>
            /// Course_Id31 欄位名稱常數定義
            /// </summary>
            public const string CourseId31 = "Course_Id31";

            /// <summary>
            /// Credit31 欄位名稱常數定義
            /// </summary>
            public const string Credit31 = "Credit31";

            /// <summary>
            /// Credit_Id32 欄位名稱常數定義
            /// </summary>
            public const string CreditId32 = "Credit_Id32";

            /// <summary>
            /// Course_Id32 欄位名稱常數定義
            /// </summary>
            public const string CourseId32 = "Course_Id32";

            /// <summary>
            /// Credit32 欄位名稱常數定義
            /// </summary>
            public const string Credit32 = "Credit32";

            /// <summary>
            /// Credit_Id33 欄位名稱常數定義
            /// </summary>
            public const string CreditId33 = "Credit_Id33";

            /// <summary>
            /// Course_Id33 欄位名稱常數定義
            /// </summary>
            public const string CourseId33 = "Course_Id33";

            /// <summary>
            /// Credit33 欄位名稱常數定義
            /// </summary>
            public const string Credit33 = "Credit33";

            /// <summary>
            /// Credit_Id34 欄位名稱常數定義
            /// </summary>
            public const string CreditId34 = "Credit_Id34";

            /// <summary>
            /// Course_Id34 欄位名稱常數定義
            /// </summary>
            public const string CourseId34 = "Course_Id34";

            /// <summary>
            /// Credit34 欄位名稱常數定義
            /// </summary>
            public const string Credit34 = "Credit34";

            /// <summary>
            /// Credit_Id35 欄位名稱常數定義
            /// </summary>
            public const string CreditId35 = "Credit_Id35";

            /// <summary>
            /// Course_Id35 欄位名稱常數定義
            /// </summary>
            public const string CourseId35 = "Course_Id35";

            /// <summary>
            /// Credit35 欄位名稱常數定義
            /// </summary>
            public const string Credit35 = "Credit35";

            /// <summary>
            /// Credit_Id36 欄位名稱常數定義
            /// </summary>
            public const string CreditId36 = "Credit_Id36";

            /// <summary>
            /// Course_Id36 欄位名稱常數定義
            /// </summary>
            public const string CourseId36 = "Course_Id36";

            /// <summary>
            /// Credit36 欄位名稱常數定義
            /// </summary>
            public const string Credit36 = "Credit36";

            /// <summary>
            /// Credit_Id37 欄位名稱常數定義
            /// </summary>
            public const string CreditId37 = "Credit_Id37";

            /// <summary>
            /// Course_Id37 欄位名稱常數定義
            /// </summary>
            public const string CourseId37 = "Course_Id37";

            /// <summary>
            /// Credit37 欄位名稱常數定義
            /// </summary>
            public const string Credit37 = "Credit37";

            /// <summary>
            /// Credit_Id38 欄位名稱常數定義
            /// </summary>
            public const string CreditId38 = "Credit_Id38";

            /// <summary>
            /// Course_Id38 欄位名稱常數定義
            /// </summary>
            public const string CourseId38 = "Course_Id38";

            /// <summary>
            /// Credit38 欄位名稱常數定義
            /// </summary>
            public const string Credit38 = "Credit38";

            /// <summary>
            /// Credit_Id39 欄位名稱常數定義
            /// </summary>
            public const string CreditId39 = "Credit_Id39";

            /// <summary>
            /// Course_Id39 欄位名稱常數定義
            /// </summary>
            public const string CourseId39 = "Course_Id39";

            /// <summary>
            /// Credit39 欄位名稱常數定義
            /// </summary>
            public const string Credit39 = "Credit39";

            /// <summary>
            /// Credit_Id40 欄位名稱常數定義
            /// </summary>
            public const string CreditId40 = "Credit_Id40";

            /// <summary>
            /// Course_Id40 欄位名稱常數定義
            /// </summary>
            public const string CourseId40 = "Course_Id40";

            /// <summary>
            /// Credit40 欄位名稱常數定義
            /// </summary>
            public const string Credit40 = "Credit40";
            #endregion

            #region 轉帳資料對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 扣款轉帳銀行代碼 欄位名稱常數定義
            /// </summary>
            public const string DeductBankid = "Deduct_BankID";

            /// <summary>
            /// 扣款轉帳銀行帳號 欄位名稱常數定義
            /// </summary>
            public const string DeductAccountno = "Deduct_AccountNo";

            /// <summary>
            /// 扣款轉帳銀行帳號戶名 欄位名稱常數定義
            /// </summary>
            public const string DeductAccountname = "Deduct_AccountName";

            /// <summary>
            /// 扣款轉帳銀行帳戶ＩＤ 欄位名稱常數定義
            /// </summary>
            public const string DeductAccountid = "Deduct_AccountId";
            #endregion

            #region 備註資料對照欄位 (StudentReceiveEntity)
            /// <summary>
            /// 備註01欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo01 = "Memo01";

            /// <summary>
            /// 備註02欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo02 = "Memo02";

            /// <summary>
            /// 備註03欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo03 = "Memo03";

            /// <summary>
            /// 備註04欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo04 = "Memo04";

            /// <summary>
            /// 備註05欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo05 = "Memo05";

            /// <summary>
            /// 備註06欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo06 = "Memo06";

            /// <summary>
            /// 備註07欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo07 = "Memo07";

            /// <summary>
            /// 備註08欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo08 = "Memo08";

            /// <summary>
            /// 備註09欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo09 = "Memo09";

            /// <summary>
            /// 備註10欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo10 = "Memo10";

            /// <summary>
            /// 備註11欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo11 = "Memo11";

            /// <summary>
            /// 備註12欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo12 = "Memo12";

            /// <summary>
            /// 備註13欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo13 = "Memo13";

            /// <summary>
            /// 備註14欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo14 = "Memo14";

            /// <summary>
            /// 備註15欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo15 = "Memo15";

            /// <summary>
            /// 備註16欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo16 = "Memo16";

            /// <summary>
            /// 備註17欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo17 = "Memo17";

            /// <summary>
            /// 備註18欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo18 = "Memo18";

            /// <summary>
            /// 備註19欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo19 = "Memo19";

            /// <summary>
            /// 備註20欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo20 = "Memo20";

            /// <summary>
            /// 備註21欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string Memo21 = "Memo21";
            #endregion

            #region [MDY:20220808] 2022擴充案 英文名稱相關對照 新增欄位
            #region 學籍資料英文名稱對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
            /// <summary>
            /// 部別英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string DeptEName = "Dept_EName";

            /// <summary>
            /// 院別英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string CollegeEName = "College_EName";

            /// <summary>
            /// 系所英文名稱欄位對照設定
            /// </summary>
            public const string MajorEName = "Major_EName";

            /// <summary>
            /// 班別英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ClassEName = "Class_EName";
            #endregion

            #region 減免、就貸、住宿英文名稱對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
            /// <summary>
            /// 減免英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string ReduceEName = "Reduce_EName";

            /// <summary>
            /// 住宿英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string DormEName = "Dorm_EName";

            /// <summary>
            /// 就貸英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string LoanEName = "Loan_EName";
            #endregion

            #region 身分註記英文名稱對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
            /// <summary>
            /// 身份註記01英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyEName1 = "Identify_EName1";

            /// <summary>
            /// 身份註記02英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyEName2 = "Identify_EName2";

            /// <summary>
            /// 身份註記03英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyEName3 = "Identify_EName3";

            /// <summary>
            /// 身份註記04英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyEName4 = "Identify_EName4";

            /// <summary>
            /// 身份註記05英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyEName5 = "Identify_EName5";

            /// <summary>
            /// 身份註記06英文名稱欄位對照設定 欄位名稱常數定義
            /// </summary>
            public const string IdentifyEName6 = "Identify_EName6";
            #endregion
            #endregion

            #region [MDY:20220808] 2022擴充案 英文名稱相關 新增欄位
            #region 40 項收入科目英文名稱 (StudentRidEntity)
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

            #region 21 項備註項目英文標題 (StudentRidEntity)
            /// <summary>
            /// 備註項目01英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE01 = "Memo_TitleE01";

            /// <summary>
            /// 備註項目02英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE02 = "Memo_TitleE02";

            /// <summary>
            /// 備註項目03英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE03 = "Memo_TitleE03";

            /// <summary>
            /// 備註項目04英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE04 = "Memo_TitleE04";

            /// <summary>
            /// 備註項目05英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE05 = "Memo_TitleE05";

            /// <summary>
            /// 備註項目06英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE06 = "Memo_TitleE06";

            /// <summary>
            /// 備註項目07英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE07 = "Memo_TitleE07";

            /// <summary>
            /// 備註項目08英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE08 = "Memo_TitleE08";

            /// <summary>
            /// 備註項目09英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE09 = "Memo_TitleE09";

            /// <summary>
            /// 備註項目10英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE10 = "Memo_TitleE10";

            /// <summary>
            /// 備註項目11英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE11 = "Memo_TitleE11";

            /// <summary>
            /// 備註項目12英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE12 = "Memo_TitleE12";

            /// <summary>
            /// 備註項目13英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE13 = "Memo_TitleE13";

            /// <summary>
            /// 備註項目14英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE14 = "Memo_TitleE14";

            /// <summary>
            /// 備註項目15英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE15 = "Memo_TitleE15";

            /// <summary>
            /// 備註項目16英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE16 = "Memo_TitleE16";

            /// <summary>
            /// 備註項目17英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE17 = "Memo_TitleE17";

            /// <summary>
            /// 備註項目18英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE18 = "Memo_TitleE18";

            /// <summary>
            /// 備註項目19英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE19 = "Memo_TitleE19";

            /// <summary>
            /// 備註項目20英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE20 = "Memo_TitleE20";

            /// <summary>
            /// 備註項目21英文標題 欄位名稱常數定義
            /// </summary>
            public const string MemoTitleE21 = "Memo_TitleE21";
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
        /// MappingreXlsmdbEntity 類別建構式
        /// </summary>
        public MappingreXlsmdbEntity()
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
        private string _StuId = null;
        /// <summary>
        /// 學號欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, true)]
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

        private string _StuName = null;
        /// <summary>
        /// 學生姓名欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuName, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuName
        {
            get
            {
                return _StuName;
            }
            set
            {
                _StuName = value == null ? null : value.Trim();
            }
        }

        private string _StuBirthday = null;
        /// <summary>
        /// 學生生日欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuBirthday, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuBirthday
        {
            get
            {
                return _StuBirthday;
            }
            set
            {
                _StuBirthday = value == null ? null : value.Trim();
            }
        }

        private string _IdNumber = null;
        /// <summary>
        /// 學生身份證欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdNumber, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdNumber
        {
            get
            {
                return _IdNumber;
            }
            set
            {
                _IdNumber = value == null ? null : value.Trim();
            }
        }

        private string _StuTel = null;
        /// <summary>
        /// 學生電話欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuTel, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuTel
        {
            get
            {
                return _StuTel;
            }
            set
            {
                _StuTel = value == null ? null : value.Trim();
            }
        }

        private string _StuAddcode = null;
        /// <summary>
        /// 學生郵遞區號欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuAddcode, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuAddcode
        {
            get
            {
                return _StuAddcode;
            }
            set
            {
                _StuAddcode = value == null ? null : value.Trim();
            }
        }

        private string _StuAdd = null;
        /// <summary>
        /// 學生地址欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuAdd, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuAdd
        {
            get
            {
                return _StuAdd;
            }
            set
            {
                _StuAdd = value == null ? null : value.Trim();
            }
        }

        private string _Email = null;
        /// <summary>
        /// 學生Email欄位對照設定
        /// </summary>
        [FieldSpec(Field.Email, false, FieldTypeEnum.VarChar, 64, true)]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value == null ? null : value.Trim();
            }
        }

        private string _StuParent = null;
        /// <summary>
        /// 學生家長名稱欄位對照設定
        /// </summary>
        /// <remarks>資料庫的欄位長度開錯了，這裡調整為 VarChar(40)，實際上用 NVarChar(20) 最好</remarks>
        [FieldSpec(Field.StuParent, false, FieldTypeEnum.VarChar, 40, true)]
        public string StuParent
        {
            get
            {
                return _StuParent;
            }
            set
            {
                _StuParent = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region [MDY:20160131] 增加資料序號與繳款期限對照欄位 (StudentReceiveEntity)
        private string _OldSeq = null;
        /// <summary>
        /// 資料序號欄位對照設定
        /// </summary>
        [FieldSpec(Field.OldSeq, false, FieldTypeEnum.NVarChar, 20, true)]
        public string OldSeq
        {
            get
            {
                return _OldSeq;
            }
            set
            {
                _OldSeq = value == null ? null : value.Trim();
            }
        }

        private string _PayDueDate = null;
        /// <summary>
        /// 繳款期限欄位對照設定
        /// </summary>
        [FieldSpec(Field.PayDueDate, false, FieldTypeEnum.NVarChar, 20, true)]
        public string PayDueDate
        {
            get
            {
                return _PayDueDate;
            }
            set
            {
                _PayDueDate = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位 (StudentReceiveEntity)
        private string _NCCardFlag = null;
        /// <summary>
        /// 是否啟用國際信用卡繳費旗標欄位對照設定
        /// </summary>
        [FieldSpec(Field.NCCardFlag, false, FieldTypeEnum.NVarChar, 20, true)]
        public string NCCardFlag
        {
            get
            {
                return _NCCardFlag;
            }
            set
            {
                _NCCardFlag = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
        private string _StuGrade = null;
        /// <summary>
        /// 年級代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuGrade, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuGrade
        {
            get
            {
                return _StuGrade;
            }
            set
            {
                _StuGrade = value == null ? null : value.Trim();
            }
        }

        private string _DeptId = null;
        /// <summary>
        /// 部別代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.DeptId, false, FieldTypeEnum.VarChar, 20, true)]
        public string DeptId
        {
            get
            {
                return _DeptId;
            }
            set
            {
                _DeptId = value == null ? null : value.Trim();
            }
        }

        private string _DeptName = null;
        /// <summary>
        /// 部別名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.DeptName, false, FieldTypeEnum.VarChar, 20, true)]
        public string DeptName
        {
            get
            {
                return _DeptName;
            }
            set
            {
                _DeptName = value == null ? null : value.Trim();
            }
        }

        private string _CollegeId = null;
        /// <summary>
        /// 院別代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.CollegeId, false, FieldTypeEnum.VarChar, 20, true)]
        public string CollegeId
        {
            get
            {
                return _CollegeId;
            }
            set
            {
                _CollegeId = value == null ? null : value.Trim();
            }
        }

        private string _CollegeName = null;
        /// <summary>
        /// 院別名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.CollegeName, false, FieldTypeEnum.VarChar, 20, true)]
        public string CollegeName
        {
            get
            {
                return _CollegeName;
            }
            set
            {
                _CollegeName = value == null ? null : value.Trim();
            }
        }

        private string _MajorId = null;
        /// <summary>
        /// 系所代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.MajorId, false, FieldTypeEnum.VarChar, 20, true)]
        public string MajorId
        {
            get
            {
                return _MajorId;
            }
            set
            {
                _MajorId = value == null ? null : value.Trim();
            }
        }

        private string _MajorName = null;
        /// <summary>
        /// 系所名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.MajorName, false, FieldTypeEnum.VarChar, 20, true)]
        public string MajorName
        {
            get
            {
                return _MajorName;
            }
            set
            {
                _MajorName = value == null ? null : value.Trim();
            }
        }

        private string _ClassId = null;
        /// <summary>
        /// 班別代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.ClassId, false, FieldTypeEnum.VarChar, 20, true)]
        public string ClassId
        {
            get
            {
                return _ClassId;
            }
            set
            {
                _ClassId = value == null ? null : value.Trim();
            }
        }

        private string _ClassName = null;
        /// <summary>
        /// 班別名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.ClassName, false, FieldTypeEnum.VarChar, 20, true)]
        public string ClassName
        {
            get
            {
                return _ClassName;
            }
            set
            {
                _ClassName = value == null ? null : value.Trim();
            }
        }

        private string _StuCredit = null;
        /// <summary>
        /// 總學分數欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuCredit, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuCredit
        {
            get
            {
                return _StuCredit;
            }
            set
            {
                _StuCredit = value == null ? null : value.Trim();
            }
        }

        private string _StuHour = null;
        /// <summary>
        /// 上課時數欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuHour, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuHour
        {
            get
            {
                return _StuHour;
            }
            set
            {
                _StuHour = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
        private string _ReduceId = null;
        /// <summary>
        /// 減免代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.ReduceId, false, FieldTypeEnum.VarChar, 20, true)]
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

        private string _ReduceName = null;
        /// <summary>
        /// 減免名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.ReduceName, false, FieldTypeEnum.VarChar, 20, true)]
        public string ReduceName
        {
            get
            {
                return _ReduceName;
            }
            set
            {
                _ReduceName = value == null ? null : value.Trim();
            }
        }

        private string _DormId = null;
        /// <summary>
        /// 住宿代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.DormId, false, FieldTypeEnum.VarChar, 20, true)]
        public string DormId
        {
            get
            {
                return _DormId;
            }
            set
            {
                _DormId = value == null ? null : value.Trim();
            }
        }

        private string _DormName = null;
        /// <summary>
        /// 住宿名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.DormName, false, FieldTypeEnum.VarChar, 20, true)]
        public string DormName
        {
            get
            {
                return _DormName;
            }
            set
            {
                _DormName = value == null ? null : value.Trim();
            }
        }

        private string _LoanId = null;
        /// <summary>
        /// 就貸代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoanId, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoanId
        {
            get
            {
                return _LoanId;
            }
            set
            {
                _LoanId = value == null ? null : value.Trim();
            }
        }

        private string _LoanName = null;
        /// <summary>
        /// 就貸名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoanName, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoanName
        {
            get
            {
                return _LoanName;
            }
            set
            {
                _LoanName = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
        private string _IdentifyId1 = null;
        /// <summary>
        /// 身份註記01代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyId1, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId1
        {
            get
            {
                return _IdentifyId1;
            }
            set
            {
                _IdentifyId1 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName1 = null;
        /// <summary>
        /// 身份註記01名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyName1, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyName1
        {
            get
            {
                return _IdentifyName1;
            }
            set
            {
                _IdentifyName1 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId2 = null;
        /// <summary>
        /// 身份註記02代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyId2, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId2
        {
            get
            {
                return _IdentifyId2;
            }
            set
            {
                _IdentifyId2 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName2 = null;
        /// <summary>
        /// 身份註記02名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyName2, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyName2
        {
            get
            {
                return _IdentifyName2;
            }
            set
            {
                _IdentifyName2 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId3 = null;
        /// <summary>
        /// 身份註記03代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyId3, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId3
        {
            get
            {
                return _IdentifyId3;
            }
            set
            {
                _IdentifyId3 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName3 = null;
        /// <summary>
        /// 身份註記03名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyName3, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyName3
        {
            get
            {
                return _IdentifyName3;
            }
            set
            {
                _IdentifyName3 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId4 = null;
        /// <summary>
        /// 身份註記04代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyId4, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId4
        {
            get
            {
                return _IdentifyId4;
            }
            set
            {
                _IdentifyId4 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName4 = null;
        /// <summary>
        /// 身份註記04名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyName4, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyName4
        {
            get
            {
                return _IdentifyName4;
            }
            set
            {
                _IdentifyName4 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId5 = null;
        /// <summary>
        /// 身份註記05代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyId5, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId5
        {
            get
            {
                return _IdentifyId5;
            }
            set
            {
                _IdentifyId5 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName5 = null;
        /// <summary>
        /// 身份註記05名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyName5, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyName5
        {
            get
            {
                return _IdentifyName5;
            }
            set
            {
                _IdentifyName5 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId6 = null;
        /// <summary>
        /// 身份註記06代碼欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyId6, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId6
        {
            get
            {
                return _IdentifyId6;
            }
            set
            {
                _IdentifyId6 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyName6 = null;
        /// <summary>
        /// 身份註記06名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyName6, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyName6
        {
            get
            {
                return _IdentifyName6;
            }
            set
            {
                _IdentifyName6 = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 繳費資料對照欄位 (StudentReceiveEntity)
        #region [MDY:202203XX] 2022擴充案 收入科目對照欄位型別 VarChar(40) 調整為 NVarChar(40)
        private string _Receive1 = null;
        /// <summary>
        /// 收入科目01金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive1, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive1
        {
            get
            {
                return _Receive1;
            }
            set
            {
                _Receive1 = value == null ? null : value.Trim();
            }
        }

        private string _Receive2 = null;
        /// <summary>
        /// 收入科目02金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive2, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive2
        {
            get
            {
                return _Receive2;
            }
            set
            {
                _Receive2 = value == null ? null : value.Trim();
            }
        }

        private string _Receive3 = null;
        /// <summary>
        /// 收入科目03金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive3, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive3
        {
            get
            {
                return _Receive3;
            }
            set
            {
                _Receive3 = value == null ? null : value.Trim();
            }
        }

        private string _Receive4 = null;
        /// <summary>
        /// 收入科目04金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive4, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive4
        {
            get
            {
                return _Receive4;
            }
            set
            {
                _Receive4 = value == null ? null : value.Trim();
            }
        }

        private string _Receive5 = null;
        /// <summary>
        /// 收入科目05金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive5, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive5
        {
            get
            {
                return _Receive5;
            }
            set
            {
                _Receive5 = value == null ? null : value.Trim();
            }
        }

        private string _Receive6 = null;
        /// <summary>
        /// 收入科目06金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive6, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive6
        {
            get
            {
                return _Receive6;
            }
            set
            {
                _Receive6 = value == null ? null : value.Trim();
            }
        }

        private string _Receive7 = null;
        /// <summary>
        /// 收入科目07金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive7, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive7
        {
            get
            {
                return _Receive7;
            }
            set
            {
                _Receive7 = value == null ? null : value.Trim();
            }
        }

        private string _Receive8 = null;
        /// <summary>
        /// 收入科目08金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive8, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive8
        {
            get
            {
                return _Receive8;
            }
            set
            {
                _Receive8 = value == null ? null : value.Trim();
            }
        }

        private string _Receive9 = null;
        /// <summary>
        /// 收入科目09金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive9, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive9
        {
            get
            {
                return _Receive9;
            }
            set
            {
                _Receive9 = value == null ? null : value.Trim();
            }
        }

        private string _Receive10 = null;
        /// <summary>
        /// 收入科目10金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive10, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive10
        {
            get
            {
                return _Receive10;
            }
            set
            {
                _Receive10 = value == null ? null : value.Trim();
            }
        }

        private string _Receive11 = null;
        /// <summary>
        /// 收入科目11金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive11, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive11
        {
            get
            {
                return _Receive11;
            }
            set
            {
                _Receive11 = value == null ? null : value.Trim();
            }
        }

        private string _Receive12 = null;
        /// <summary>
        /// 收入科目12金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive12, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive12
        {
            get
            {
                return _Receive12;
            }
            set
            {
                _Receive12 = value == null ? null : value.Trim();
            }
        }

        private string _Receive13 = null;
        /// <summary>
        /// 收入科目13金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive13, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive13
        {
            get
            {
                return _Receive13;
            }
            set
            {
                _Receive13 = value == null ? null : value.Trim();
            }
        }

        private string _Receive14 = null;
        /// <summary>
        /// 收入科目14金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive14, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive14
        {
            get
            {
                return _Receive14;
            }
            set
            {
                _Receive14 = value == null ? null : value.Trim();
            }
        }

        private string _Receive15 = null;
        /// <summary>
        /// 收入科目15金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive15, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive15
        {
            get
            {
                return _Receive15;
            }
            set
            {
                _Receive15 = value == null ? null : value.Trim();
            }
        }

        private string _Receive16 = null;
        /// <summary>
        /// 收入科目16金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive16, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive16
        {
            get
            {
                return _Receive16;
            }
            set
            {
                _Receive16 = value == null ? null : value.Trim();
            }
        }

        private string _Receive17 = null;
        /// <summary>
        /// 收入科目17金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive17, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive17
        {
            get
            {
                return _Receive17;
            }
            set
            {
                _Receive17 = value == null ? null : value.Trim();
            }
        }

        private string _Receive18 = null;
        /// <summary>
        /// 收入科目18金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive18, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive18
        {
            get
            {
                return _Receive18;
            }
            set
            {
                _Receive18 = value == null ? null : value.Trim();
            }
        }

        private string _Receive19 = null;
        /// <summary>
        /// 收入科目19金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive19, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive19
        {
            get
            {
                return _Receive19;
            }
            set
            {
                _Receive19 = value == null ? null : value.Trim();
            }
        }

        private string _Receive20 = null;
        /// <summary>
        /// 收入科目20金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive20, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive20
        {
            get
            {
                return _Receive20;
            }
            set
            {
                _Receive20 = value == null ? null : value.Trim();
            }
        }

        private string _Receive21 = null;
        /// <summary>
        /// 收入科目21金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive21, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive21
        {
            get
            {
                return _Receive21;
            }
            set
            {
                _Receive21 = value == null ? null : value.Trim();
            }
        }

        private string _Receive22 = null;
        /// <summary>
        /// 收入科目22金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive22, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive22
        {
            get
            {
                return _Receive22;
            }
            set
            {
                _Receive22 = value == null ? null : value.Trim();
            }
        }

        private string _Receive23 = null;
        /// <summary>
        /// 收入科目23金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive23, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive23
        {
            get
            {
                return _Receive23;
            }
            set
            {
                _Receive23 = value == null ? null : value.Trim();
            }
        }

        private string _Receive24 = null;
        /// <summary>
        /// 收入科目24金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive24, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive24
        {
            get
            {
                return _Receive24;
            }
            set
            {
                _Receive24 = value == null ? null : value.Trim();
            }
        }

        private string _Receive25 = null;
        /// <summary>
        /// 收入科目25金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive25, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive25
        {
            get
            {
                return _Receive25;
            }
            set
            {
                _Receive25 = value == null ? null : value.Trim();
            }
        }

        private string _Receive26 = null;
        /// <summary>
        /// 收入科目26金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive26, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive26
        {
            get
            {
                return _Receive26;
            }
            set
            {
                _Receive26 = value == null ? null : value.Trim();
            }
        }

        private string _Receive27 = null;
        /// <summary>
        /// 收入科目27金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive27, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive27
        {
            get
            {
                return _Receive27;
            }
            set
            {
                _Receive27 = value == null ? null : value.Trim();
            }
        }

        private string _Receive28 = null;
        /// <summary>
        /// 收入科目28金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive28, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive28
        {
            get
            {
                return _Receive28;
            }
            set
            {
                _Receive28 = value == null ? null : value.Trim();
            }
        }

        private string _Receive29 = null;
        /// <summary>
        /// 收入科目29金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive29, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive29
        {
            get
            {
                return _Receive29;
            }
            set
            {
                _Receive29 = value == null ? null : value.Trim();
            }
        }

        private string _Receive30 = null;
        /// <summary>
        /// 收入科目30金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive30, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive30
        {
            get
            {
                return _Receive30;
            }
            set
            {
                _Receive30 = value == null ? null : value.Trim();
            }
        }

        private string _Receive31 = null;
        /// <summary>
        /// 收入科目31金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive31, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive31
        {
            get
            {
                return _Receive31;
            }
            set
            {
                _Receive31 = value == null ? null : value.Trim();
            }
        }

        private string _Receive32 = null;
        /// <summary>
        /// 收入科目32金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive32, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive32
        {
            get
            {
                return _Receive32;
            }
            set
            {
                _Receive32 = value == null ? null : value.Trim();
            }
        }

        private string _Receive33 = null;
        /// <summary>
        /// 收入科目33金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive33, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive33
        {
            get
            {
                return _Receive33;
            }
            set
            {
                _Receive33 = value == null ? null : value.Trim();
            }
        }

        private string _Receive34 = null;
        /// <summary>
        /// 收入科目34金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive34, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive34
        {
            get
            {
                return _Receive34;
            }
            set
            {
                _Receive34 = value == null ? null : value.Trim();
            }
        }

        private string _Receive35 = null;
        /// <summary>
        /// 收入科目35金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive35, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive35
        {
            get
            {
                return _Receive35;
            }
            set
            {
                _Receive35 = value == null ? null : value.Trim();
            }
        }

        private string _Receive36 = null;
        /// <summary>
        /// 收入科目36金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive36, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive36
        {
            get
            {
                return _Receive36;
            }
            set
            {
                _Receive36 = value == null ? null : value.Trim();
            }
        }

        private string _Receive37 = null;
        /// <summary>
        /// 收入科目37金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive37, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive37
        {
            get
            {
                return _Receive37;
            }
            set
            {
                _Receive37 = value == null ? null : value.Trim();
            }
        }

        private string _Receive38 = null;
        /// <summary>
        /// 收入科目38金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive38, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive38
        {
            get
            {
                return _Receive38;
            }
            set
            {
                _Receive38 = value == null ? null : value.Trim();
            }
        }

        private string _Receive39 = null;
        /// <summary>
        /// 收入科目39金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive39, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive39
        {
            get
            {
                return _Receive39;
            }
            set
            {
                _Receive39 = value == null ? null : value.Trim();
            }
        }

        private string _Receive40 = null;
        /// <summary>
        /// 收入科目40金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.Receive40, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Receive40
        {
            get
            {
                return _Receive40;
            }
            set
            {
                _Receive40 = value == null ? null : value.Trim();
            }
        }
        #endregion

        private string _LoanAmount = null;
        /// <summary>
        /// 可貸金額欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoanAmount, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoanAmount
        {
            get
            {
                return _LoanAmount;
            }
            set
            {
                _LoanAmount = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveAmount = null;
        /// <summary>
        /// 繳費金額合計欄位對照設定
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.VarChar, 20, true)]
        public string ReceiveAmount
        {
            get
            {
                return _ReceiveAmount;
            }
            set
            {
                _ReceiveAmount = value == null ? null : value.Trim();
            }
        }

        private string _StuHid = null;
        /// <summary>
        /// 座號欄位對照設定
        /// </summary>
        [FieldSpec(Field.StuHid, false, FieldTypeEnum.VarChar, 20, true)]
        public string StuHid
        {
            get
            {
                return _StuHid;
            }
            set
            {
                _StuHid = value == null ? null : value.Trim();
            }
        }

        private string _Remark = null;
        /// <summary>
        /// Remark (土銀沒用到) 欄位屬性
        /// </summary>
        /// <remarks>資料庫的欄位開錯了，這裡調整為 VarChar(40)，實際上用 NVarChar(20) 最好</remarks>
        [FieldSpec(Field.Remark, false, FieldTypeEnum.VarChar, 40, true)]
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                _Remark = value == null ? null : value.Trim();
            }
        }

        private string _SeriorNo = null;
        /// <summary>
        /// 流水號欄位對照設定
        /// </summary>
        [FieldSpec(Field.SeriorNo, false, FieldTypeEnum.NVarChar, 20, true)]
        public string SeriorNo
        {
            get
            {
                return _SeriorNo;
            }
            set
            {
                _SeriorNo = value == null ? null : value.Trim();
            }
        }

        private string _CancelNo = null;
        /// <summary>
        /// 銷帳編號欄位對照設定
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.NVarChar, 20, true)]
        public string CancelNo
        {
            get
            {
                return _CancelNo;
            }
            set
            {
                _CancelNo = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 其他繳費資料對照欄位
        /// <summary>
        /// Credit_Id1 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId1, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId1
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id1 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId1, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId1
        {
            get;
            set;
        }

        /// <summary>
        /// Credit1 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit1, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit1
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id2 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId2, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId2
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id2 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId2, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId2
        {
            get;
            set;
        }

        /// <summary>
        /// Credit2 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit2, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit2
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id3 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId3, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId3
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id3 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId3, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId3
        {
            get;
            set;
        }

        /// <summary>
        /// Credit3 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit3, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit3
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id4 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId4, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId4
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id4 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId4, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId4
        {
            get;
            set;
        }

        /// <summary>
        /// Credit4 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit4, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit4
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id5 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId5, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId5
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id5 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId5, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId5
        {
            get;
            set;
        }

        /// <summary>
        /// Credit5 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit5, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit5
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id6 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId6, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId6
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id6 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId6, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId6
        {
            get;
            set;
        }

        /// <summary>
        /// Credit6 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit6, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit6
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id7 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId7, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId7
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id7 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId7, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId7
        {
            get;
            set;
        }

        /// <summary>
        /// Credit7 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit7, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit7
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id8 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId8, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId8
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id8 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId8, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId8
        {
            get;
            set;
        }

        /// <summary>
        /// Credit8 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit8, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit8
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id9 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId9, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId9
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id9 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId9, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId9
        {
            get;
            set;
        }

        /// <summary>
        /// Credit9 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit9, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit9
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id10 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId10, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId10
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id10 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId10, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId10
        {
            get;
            set;
        }

        /// <summary>
        /// Credit10 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit10, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit10
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id11 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId11, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId11
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id11 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId11, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId11
        {
            get;
            set;
        }

        /// <summary>
        /// Credit11 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit11, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit11
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id12 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId12, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId12
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id12 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId12, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId12
        {
            get;
            set;
        }

        /// <summary>
        /// Credit12 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit12, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit12
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id13 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId13, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId13
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id13 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId13, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId13
        {
            get;
            set;
        }

        /// <summary>
        /// Credit13 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit13, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit13
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id14 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId14, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId14
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id14 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId14, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId14
        {
            get;
            set;
        }

        /// <summary>
        /// Credit14 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit14, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit14
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id15 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId15, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId15
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id15 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId15, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId15
        {
            get;
            set;
        }

        /// <summary>
        /// Credit15 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit15, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit15
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id16 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId16, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId16
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id16 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId16, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId16
        {
            get;
            set;
        }

        /// <summary>
        /// Credit16 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit16, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit16
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id17 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId17, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId17
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id17 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId17, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId17
        {
            get;
            set;
        }

        /// <summary>
        /// Credit17 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit17, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit17
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id18 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId18, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId18
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id18 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId18, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId18
        {
            get;
            set;
        }

        /// <summary>
        /// Credit18 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit18, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit18
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id19 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId19, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId19
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id19 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId19, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId19
        {
            get;
            set;
        }

        /// <summary>
        /// Credit19 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit19, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit19
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id20 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId20, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId20
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id20 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId20, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId20
        {
            get;
            set;
        }

        /// <summary>
        /// Credit20 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit20, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit20
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id21 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId21, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId21
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id21 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId21, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId21
        {
            get;
            set;
        }

        /// <summary>
        /// Credit21 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit21, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit21
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id22 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId22, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId22
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id22 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId22, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId22
        {
            get;
            set;
        }

        /// <summary>
        /// Credit22 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit22, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit22
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id23 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId23, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId23
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id23 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId23, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId23
        {
            get;
            set;
        }

        /// <summary>
        /// Credit23 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit23, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit23
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id24 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId24, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId24
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id24 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId24, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId24
        {
            get;
            set;
        }

        /// <summary>
        /// Credit24 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit24, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit24
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id25 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId25, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId25
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id25 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId25, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId25
        {
            get;
            set;
        }

        /// <summary>
        /// Credit25 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit25, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit25
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id26 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId26, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId26
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id26 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId26, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId26
        {
            get;
            set;
        }

        /// <summary>
        /// Credit26 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit26, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit26
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id27 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId27, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId27
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id27 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId27, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId27
        {
            get;
            set;
        }

        /// <summary>
        /// Credit27 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit27, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit27
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id28 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId28, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId28
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id28 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId28, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId28
        {
            get;
            set;
        }

        /// <summary>
        /// Credit28 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit28, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit28
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id29 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId29, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId29
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id29 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId29, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId29
        {
            get;
            set;
        }

        /// <summary>
        /// Credit29 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit29, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit29
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id30 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId30, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId30
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id30 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId30, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId30
        {
            get;
            set;
        }

        /// <summary>
        /// Credit30 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit30, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit30
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id31 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId31, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId31
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id31 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId31, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId31
        {
            get;
            set;
        }

        /// <summary>
        /// Credit31 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit31, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit31
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id32 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId32, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId32
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id32 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId32, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId32
        {
            get;
            set;
        }

        /// <summary>
        /// Credit32 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit32, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit32
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id33 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId33, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId33
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id33 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId33, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId33
        {
            get;
            set;
        }

        /// <summary>
        /// Credit33 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit33, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit33
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id34 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId34, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId34
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id34 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId34, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId34
        {
            get;
            set;
        }

        /// <summary>
        /// Credit34 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit34, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit34
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id35 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId35, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId35
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id35 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId35, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId35
        {
            get;
            set;
        }

        /// <summary>
        /// Credit35 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit35, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit35
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id36 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId36, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId36
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id36 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId36, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId36
        {
            get;
            set;
        }

        /// <summary>
        /// Credit36 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit36, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit36
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id37 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId37, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId37
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id37 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId37, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId37
        {
            get;
            set;
        }

        /// <summary>
        /// Credit37 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit37, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit37
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id38 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId38, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId38
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id38 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId38, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId38
        {
            get;
            set;
        }

        /// <summary>
        /// Credit38 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit38, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit38
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id39 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId39, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId39
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id39 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId39, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId39
        {
            get;
            set;
        }

        /// <summary>
        /// Credit39 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit39, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit39
        {
            get;
            set;
        }

        /// <summary>
        /// Credit_Id40 欄位屬性
        /// </summary>
        [FieldSpec(Field.CreditId40, false, FieldTypeEnum.VarChar, 40, true)]
        public string CreditId40
        {
            get;
            set;
        }

        /// <summary>
        /// Course_Id40 欄位屬性
        /// </summary>
        [FieldSpec(Field.CourseId40, false, FieldTypeEnum.VarChar, 40, true)]
        public string CourseId40
        {
            get;
            set;
        }

        /// <summary>
        /// Credit40 欄位屬性
        /// </summary>
        [FieldSpec(Field.Credit40, false, FieldTypeEnum.VarChar, 40, true)]
        public string Credit40
        {
            get;
            set;
        }
        #endregion

        #region 轉帳資料對照欄位 (StudentReceiveEntity)
        private string _DeductBankid = null;
        /// <summary>
        /// 扣款轉帳銀行代碼
        /// </summary>
        [FieldSpec(Field.DeductBankid, false, FieldTypeEnum.VarChar, 20, true)]
        public string DeductBankid
        {
            get
            {
                return _DeductBankid;
            }
            set
            {
                _DeductBankid = value == null ? null : value.Trim();
            }
        }

        private string _DeductAccountno = null;
        /// <summary>
        /// 扣款轉帳銀行帳號
        /// </summary>
        [FieldSpec(Field.DeductAccountno, false, FieldTypeEnum.VarChar, 20, true)]
        public string DeductAccountno
        {
            get
            {
                return _DeductAccountno;
            }
            set
            {
                _DeductAccountno = value == null ? null : value.Trim();
            }
        }

        private string _DeductAccountname = null;
        /// <summary>
        /// 扣款轉帳銀行帳號戶名
        /// </summary>
        [FieldSpec(Field.DeductAccountname, false, FieldTypeEnum.NVarChar, 20, true)]
        public string DeductAccountname
        {
            get
            {
                return _DeductAccountname;
            }
            set
            {
                _DeductAccountname = value == null ? null : value.Trim();
            }
        }

        private string _DeductAccountid = null;
        /// <summary>
        /// 扣款轉帳銀行帳戶ＩＤ
        /// </summary>
        [FieldSpec(Field.DeductAccountid, false, FieldTypeEnum.NVarChar, 20, true)]
        public string DeductAccountid
        {
            get
            {
                return _DeductAccountid;
            }
            set
            {
                _DeductAccountid = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 備註對照欄位 (StudentReceiveEntity)
        #region [MDY:202203XX] 2022擴充案 備註對照欄位型別 NVarChar(20) 調整為 NVarChar(40)
        private string _Memo01 = null;
        /// <summary>
        /// 備註01欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo01, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo01
        {
            get
            {
                return _Memo01;
            }
            set
            {
                _Memo01 = value == null ? null : value.Trim();
            }
        }

        private string _Memo02 = null;
        /// <summary>
        /// 備註02欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo02, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo02
        {
            get
            {
                return _Memo02;
            }
            set
            {
                _Memo02 = value == null ? null : value.Trim();
            }
        }

        private string _Memo03 = null;
        /// <summary>
        /// 備註03欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo03, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo03
        {
            get
            {
                return _Memo03;
            }
            set
            {
                _Memo03 = value == null ? null : value.Trim();
            }
        }

        private string _Memo04 = null;
        /// <summary>
        /// 備註04欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo04, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo04
        {
            get
            {
                return _Memo04;
            }
            set
            {
                _Memo04 = value == null ? null : value.Trim();
            }
        }

        private string _Memo05 = null;
        /// <summary>
        /// 備註05欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo05, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo05
        {
            get
            {
                return _Memo05;
            }
            set
            {
                _Memo05 = value == null ? null : value.Trim();
            }
        }

        private string _Memo06 = null;
        /// <summary>
        /// 備註06欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo06, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo06
        {
            get
            {
                return _Memo06;
            }
            set
            {
                _Memo06 = value == null ? null : value.Trim();
            }
        }

        private string _Memo07 = null;
        /// <summary>
        /// 備註07欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo07, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo07
        {
            get
            {
                return _Memo07;
            }
            set
            {
                _Memo07 = value == null ? null : value.Trim();
            }
        }

        private string _Memo08 = null;
        /// <summary>
        /// 備註08欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo08, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo08
        {
            get
            {
                return _Memo08;
            }
            set
            {
                _Memo08 = value == null ? null : value.Trim();
            }
        }

        private string _Memo09 = null;
        /// <summary>
        /// 備註09欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo09, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo09
        {
            get
            {
                return _Memo09;
            }
            set
            {
                _Memo09 = value == null ? null : value.Trim();
            }
        }

        private string _Memo10 = null;
        /// <summary>
        /// 備註10欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo10, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo10
        {
            get
            {
                return _Memo10;
            }
            set
            {
                _Memo10 = value == null ? null : value.Trim();
            }
        }

        private string _Memo11 = null;
        /// <summary>
        /// 備註11欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo11, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo11
        {
            get
            {
                return _Memo11;
            }
            set
            {
                _Memo11 = value == null ? null : value.Trim();
            }
        }

        private string _Memo12 = null;
        /// <summary>
        /// 備註12欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo12, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo12
        {
            get
            {
                return _Memo12;
            }
            set
            {
                _Memo12 = value == null ? null : value.Trim();
            }
        }

        private string _Memo13 = null;
        /// <summary>
        /// 備註13欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo13, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo13
        {
            get
            {
                return _Memo13;
            }
            set
            {
                _Memo13 = value == null ? null : value.Trim();
            }
        }

        private string _Memo14 = null;
        /// <summary>
        /// 備註14欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo14, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo14
        {
            get
            {
                return _Memo14;
            }
            set
            {
                _Memo14 = value == null ? null : value.Trim();
            }
        }

        private string _Memo15 = null;
        /// <summary>
        /// 備註15欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo15, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo15
        {
            get
            {
                return _Memo15;
            }
            set
            {
                _Memo15 = value == null ? null : value.Trim();
            }
        }

        private string _Memo16 = null;
        /// <summary>
        /// 備註16欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo16, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo16
        {
            get
            {
                return _Memo16;
            }
            set
            {
                _Memo16 = value == null ? null : value.Trim();
            }
        }

        private string _Memo17 = null;
        /// <summary>
        /// 備註17欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo17, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo17
        {
            get
            {
                return _Memo17;
            }
            set
            {
                _Memo17 = value == null ? null : value.Trim();
            }
        }

        private string _Memo18 = null;
        /// <summary>
        /// 備註18欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo18, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo18
        {
            get
            {
                return _Memo18;
            }
            set
            {
                _Memo18 = value == null ? null : value.Trim();
            }
        }

        private string _Memo19 = null;
        /// <summary>
        /// 備註19欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo19, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo19
        {
            get
            {
                return _Memo19;
            }
            set
            {
                _Memo19 = value == null ? null : value.Trim();
            }
        }

        private string _Memo20 = null;
        /// <summary>
        /// 備註20欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo20, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo20
        {
            get
            {
                return _Memo20;
            }
            set
            {
                _Memo20 = value == null ? null : value.Trim();
            }
        }

        private string _Memo21 = null;
        /// <summary>
        /// 備註21欄位對照設定
        /// </summary>
        [FieldSpec(Field.Memo21, false, FieldTypeEnum.NVarChar, 40, true)]
        public string Memo21
        {
            get
            {
                return _Memo21;
            }
            set
            {
                _Memo21 = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region [MDY:20220808] 2022擴充案 英文名稱相關對照 新增欄位
        #region 學籍資料英文名稱對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
        private string _DeptEName = null;
        /// <summary>
        /// 部別英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.DeptEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string DeptEName
        {
            get
            {
                return _DeptEName;
            }
            set
            {
                _DeptEName = value == null ? null : value.Trim();
            }
        }

        private string _CollegeEName = null;
        /// <summary>
        /// 院別英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.CollegeEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string CollegeEName
        {
            get
            {
                return _CollegeEName;
            }
            set
            {
                _CollegeEName = value == null ? null : value.Trim();
            }
        }

        private string _MajorEName = null;
        /// <summary>
        /// 系所英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.MajorEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string MajorEName
        {
            get
            {
                return _MajorEName;
            }
            set
            {
                _MajorEName = value == null ? null : value.Trim();
            }
        }

        private string _ClassEName = null;
        /// <summary>
        /// 班別英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.ClassEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string ClassEName
        {
            get
            {
                return _ClassEName;
            }
            set
            {
                _ClassEName = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 減免、就貸、住宿英文名稱對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
        private string _ReduceEName = null;
        /// <summary>
        /// 減免英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.ReduceEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string ReduceEName
        {
            get
            {
                return _ReduceEName;
            }
            set
            {
                _ReduceEName = value == null ? null : value.Trim();
            }
        }

        private string _DormEName = null;
        /// <summary>
        /// 住宿英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.DormEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string DormEName
        {
            get
            {
                return _DormEName;
            }
            set
            {
                _DormEName = value == null ? null : value.Trim();
            }
        }

        private string _LoanEName = null;
        /// <summary>
        /// 就貸英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoanEName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string LoanEName
        {
            get
            {
                return _LoanEName;
            }
            set
            {
                _LoanEName = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 身分註記英文名稱對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
        private string _IdentifyEName1 = null;
        /// <summary>
        /// 身份註記01英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyEName1, false, FieldTypeEnum.NVarChar, 20, true)]
        public string IdentifyEName1
        {
            get
            {
                return _IdentifyEName1;
            }
            set
            {
                _IdentifyEName1 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyEName2 = null;
        /// <summary>
        /// 身份註記02英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyEName2, false, FieldTypeEnum.NVarChar, 20, true)]
        public string IdentifyEName2
        {
            get
            {
                return _IdentifyEName2;
            }
            set
            {
                _IdentifyEName2 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyEName3 = null;
        /// <summary>
        /// 身份註記03英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyEName3, false, FieldTypeEnum.NVarChar, 20, true)]
        public string IdentifyEName3
        {
            get
            {
                return _IdentifyEName3;
            }
            set
            {
                _IdentifyEName3 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyEName4 = null;
        /// <summary>
        /// 身份註記04英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyEName4, false, FieldTypeEnum.NVarChar, 20, true)]
        public string IdentifyEName4
        {
            get
            {
                return _IdentifyEName4;
            }
            set
            {
                _IdentifyEName4 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyEName5 = null;
        /// <summary>
        /// 身份註記05英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyEName5, false, FieldTypeEnum.NVarChar, 20, true)]
        public string IdentifyEName5
        {
            get
            {
                return _IdentifyEName5;
            }
            set
            {
                _IdentifyEName5 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyEName6 = null;
        /// <summary>
        /// 身份註記06英文名稱欄位對照設定
        /// </summary>
        [FieldSpec(Field.IdentifyEName6, false, FieldTypeEnum.NVarChar, 20, true)]
        public string IdentifyEName6
        {
            get
            {
                return _IdentifyEName6;
            }
            set
            {
                _IdentifyEName6 = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region [MDY:20220808] 2022擴充案 英文名稱相關 新增欄位並調整為 NVarChar(140)
        #region 40 項收入科目英文名稱 (StudentRidEntity)
        private string _ReceiveItemE01 = null;
        /// <summary>
        /// 收入科目01英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE01, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE01
        {
            get
            {
                return _ReceiveItemE01;
            }
            set
            {
                _ReceiveItemE01 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE02 = null;
        /// <summary>
        /// 收入科目02英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE02, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE02
        {
            get
            {
                return _ReceiveItemE02;
            }
            set
            {
                _ReceiveItemE02 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE03 = null;
        /// <summary>
        /// 收入科目03英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE03, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE03
        {
            get
            {
                return _ReceiveItemE03;
            }
            set
            {
                _ReceiveItemE03 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE04 = null;
        /// <summary>
        /// 收入科目04英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE04, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE04
        {
            get
            {
                return _ReceiveItemE04;
            }
            set
            {
                _ReceiveItemE04 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE05 = null;
        /// <summary>
        /// 收入科目05英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE05, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE05
        {
            get
            {
                return _ReceiveItemE05;
            }
            set
            {
                _ReceiveItemE05 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE06 = null;
        /// <summary>
        /// 收入科目06英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE06, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE06
        {
            get
            {
                return _ReceiveItemE06;
            }
            set
            {
                _ReceiveItemE06 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE07 = null;
        /// <summary>
        /// 收入科目07英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE07, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE07
        {
            get
            {
                return _ReceiveItemE07;
            }
            set
            {
                _ReceiveItemE07 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE08 = null;
        /// <summary>
        /// 收入科目08英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE08, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE08
        {
            get
            {
                return _ReceiveItemE08;
            }
            set
            {
                _ReceiveItemE08 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE09 = null;
        /// <summary>
        /// 收入科目09英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE09, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE09
        {
            get
            {
                return _ReceiveItemE09;
            }
            set
            {
                _ReceiveItemE09 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE10 = null;
        /// <summary>
        /// 收入科目10英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE10, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE10
        {
            get
            {
                return _ReceiveItemE10;
            }
            set
            {
                _ReceiveItemE10 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE11 = null;
        /// <summary>
        /// 收入科目11英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE11, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE11
        {
            get
            {
                return _ReceiveItemE11;
            }
            set
            {
                _ReceiveItemE11 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE12 = null;
        /// <summary>
        /// 收入科目12英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE12, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE12
        {
            get
            {
                return _ReceiveItemE12;
            }
            set
            {
                _ReceiveItemE12 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE13 = null;
        /// <summary>
        /// 收入科目13英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE13, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE13
        {
            get
            {
                return _ReceiveItemE13;
            }
            set
            {
                _ReceiveItemE13 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE14 = null;
        /// <summary>
        /// 收入科目14英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE14, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE14
        {
            get
            {
                return _ReceiveItemE14;
            }
            set
            {
                _ReceiveItemE14 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE15 = null;
        /// <summary>
        /// 收入科目15英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE15, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE15
        {
            get
            {
                return _ReceiveItemE15;
            }
            set
            {
                _ReceiveItemE15 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE16 = null;
        /// <summary>
        /// 收入科目16英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE16, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE16
        {
            get
            {
                return _ReceiveItemE16;
            }
            set
            {
                _ReceiveItemE16 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE17 = null;
        /// <summary>
        /// 收入科目17英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE17, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE17
        {
            get
            {
                return _ReceiveItemE17;
            }
            set
            {
                _ReceiveItemE17 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE18 = null;
        /// <summary>
        /// 收入科目18英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE18, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE18
        {
            get
            {
                return _ReceiveItemE18;
            }
            set
            {
                _ReceiveItemE18 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE19 = null;
        /// <summary>
        /// 收入科目19英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE19, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE19
        {
            get
            {
                return _ReceiveItemE19;
            }
            set
            {
                _ReceiveItemE19 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE20 = null;
        /// <summary>
        /// 收入科目20英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE20, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE20
        {
            get
            {
                return _ReceiveItemE20;
            }
            set
            {
                _ReceiveItemE20 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE21 = null;
        /// <summary>
        /// 收入科目21英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE21, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE21
        {
            get
            {
                return _ReceiveItemE21;
            }
            set
            {
                _ReceiveItemE21 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE22 = null;
        /// <summary>
        /// 收入科目22英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE22, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE22
        {
            get
            {
                return _ReceiveItemE22;
            }
            set
            {
                _ReceiveItemE22 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE23 = null;
        /// <summary>
        /// 收入科目23英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE23, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE23
        {
            get
            {
                return _ReceiveItemE23;
            }
            set
            {
                _ReceiveItemE23 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE24 = null;
        /// <summary>
        /// 收入科目24英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE24, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE24
        {
            get
            {
                return _ReceiveItemE24;
            }
            set
            {
                _ReceiveItemE24 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE25 = null;
        /// <summary>
        /// 收入科目25英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE25, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE25
        {
            get
            {
                return _ReceiveItemE25;
            }
            set
            {
                _ReceiveItemE25 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE26 = null;
        /// <summary>
        /// 收入科目26英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE26, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE26
        {
            get
            {
                return _ReceiveItemE26;
            }
            set
            {
                _ReceiveItemE26 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE27 = null;
        /// <summary>
        /// 收入科目27英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE27, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE27
        {
            get
            {
                return _ReceiveItemE27;
            }
            set
            {
                _ReceiveItemE27 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE28 = null;
        /// <summary>
        /// 收入科目28英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE28, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE28
        {
            get
            {
                return _ReceiveItemE28;
            }
            set
            {
                _ReceiveItemE28 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE29 = null;
        /// <summary>
        /// 收入科目29英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE29, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE29
        {
            get
            {
                return _ReceiveItemE29;
            }
            set
            {
                _ReceiveItemE29 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE30 = null;
        /// <summary>
        /// 收入科目30英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE30, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE30
        {
            get
            {
                return _ReceiveItemE30;
            }
            set
            {
                _ReceiveItemE30 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE31 = null;
        /// <summary>
        /// 收入科目31英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE31, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE31
        {
            get
            {
                return _ReceiveItemE31;
            }
            set
            {
                _ReceiveItemE31 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE32 = null;
        /// <summary>
        /// 收入科目32英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE32, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE32
        {
            get
            {
                return _ReceiveItemE32;
            }
            set
            {
                _ReceiveItemE32 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE33 = null;
        /// <summary>
        /// 收入科目33英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE33, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE33
        {
            get
            {
                return _ReceiveItemE33;
            }
            set
            {
                _ReceiveItemE33 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE34 = null;
        /// <summary>
        /// 收入科目34英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE34, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE34
        {
            get
            {
                return _ReceiveItemE34;
            }
            set
            {
                _ReceiveItemE34 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE35 = null;
        /// <summary>
        /// 收入科目35英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE35, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE35
        {
            get
            {
                return _ReceiveItemE35;
            }
            set
            {
                _ReceiveItemE35 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE36 = null;
        /// <summary>
        /// 收入科目36英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE36, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE36
        {
            get
            {
                return _ReceiveItemE36;
            }
            set
            {
                _ReceiveItemE36 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE37 = null;
        /// <summary>
        /// 收入科目37英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE37, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE37
        {
            get
            {
                return _ReceiveItemE37;
            }
            set
            {
                _ReceiveItemE37 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE38 = null;
        /// <summary>
        /// 收入科目38英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE38, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE38
        {
            get
            {
                return _ReceiveItemE38;
            }
            set
            {
                _ReceiveItemE38 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE39 = null;
        /// <summary>
        /// 收入科目39英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE39, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE39
        {
            get
            {
                return _ReceiveItemE39;
            }
            set
            {
                _ReceiveItemE39 = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveItemE40 = null;
        /// <summary>
        /// 收入科目40英文名稱
        /// </summary>
        [FieldSpec(Field.ReceiveItemE40, false, FieldTypeEnum.NVarChar, 140, true)]
        public string ReceiveItemE40
        {
            get
            {
                return _ReceiveItemE40;
            }
            set
            {
                _ReceiveItemE40 = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 21 項備註項目英文標題 (StudentRidEntity)
        private string _MemoTitleE01 = null;
        /// <summary>
        /// 備註項目01英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE01, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目02英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE02, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目03英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE03, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目04英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE04, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目05英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE05, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目06英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE06, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目07英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE07, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目08英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE08, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目09英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE09, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目10英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE10, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目11英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE11, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目12英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE12, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目13英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE13, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目14英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE14, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目15英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE15, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目16英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE16, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目17英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE17, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目18英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE18, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目19英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE19, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目20英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE20, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 備註項目21英文標題
        /// </summary>
        [FieldSpec(Field.MemoTitleE21, false, FieldTypeEnum.NVarChar, 140, true)]
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
        /// 取得有設定的 XlsMapField 設定陣列
        /// </summary>
        /// <returns>傳回 XlsMapField 設定陣列</returns>
        public XlsMapField[] GetMapFields()
        {
            List<XlsMapField> mapFields = new List<XlsMapField>();

            #region 學生資料對照欄位 (StudentMasterEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.StuId))
                {
                    mapFields.Add(new XlsMapField(Field.StuId, this.StuId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuName))
                {
                    mapFields.Add(new XlsMapField(Field.StuName, this.StuName, new WordChecker(1, 60)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuBirthday))
                {
                    mapFields.Add(new XlsMapField(Field.StuBirthday, this.StuBirthday, new DateTimeChecker(DateTimeChecker.FormatEnum.DateText)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdNumber))
                {
                    #region [Old] 土銀不檢查身份證格式，改檢查字元長度限制
                    //mapFields.Add(new XlsMapField(Field.IdNumber, this.IdNumber, new PersonalIDChecker()));
                    #endregion

                    mapFields.Add(new XlsMapField(Field.IdNumber, this.IdNumber, new CharChecker(0, 10)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuTel))
                {
                    mapFields.Add(new XlsMapField(Field.StuTel, this.StuTel, new CharChecker(0, 14)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuAddcode))
                {
                    mapFields.Add(new XlsMapField(Field.StuAddcode, this.StuAddcode, new CharChecker(0, 5)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuAdd))
                {
                    mapFields.Add(new XlsMapField(Field.StuAdd, this.StuAdd, new WordChecker(0, 50)));
                }
                if (!String.IsNullOrWhiteSpace(this.Email))
                {
                    mapFields.Add(new XlsMapField(Field.Email, this.Email, new CharChecker(0, 50)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuParent))
                {
                    mapFields.Add(new XlsMapField(Field.StuParent, this.StuParent, new WordChecker(0, 60)));
                }
            }
            #endregion

            #region [MDY:20160131] 增加資料序號與繳款期限對照欄位 (StudentReceiveEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.OldSeq))
                {
                    mapFields.Add(new XlsMapField(Field.OldSeq, this.OldSeq, new IntegerChecker(0, StudentReceiveEntity.MaxOldSeq)));
                }
                if (!String.IsNullOrWhiteSpace(this.PayDueDate))
                {
                    mapFields.Add(new XlsMapField(Field.PayDueDate, this.PayDueDate, new DateTimeChecker(DateTimeChecker.FormatEnum.DateText)));
                }
            }
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標對照欄位 (StudentReceiveEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.NCCardFlag))
                {
                    mapFields.Add(new XlsMapField(Field.NCCardFlag, this.NCCardFlag, new RegexChecker(1, 1, new System.Text.RegularExpressions.Regex("^[YN是否]$", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase), "必須為 Y、N、是、否")));
                }
            }
            #endregion

            #region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, CollegeListEntity, MajorListEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.StuGrade))
                {
                    mapFields.Add(new XlsMapField(Field.StuGrade, this.StuGrade, new IntegerChecker(0, 12)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuHid))
                {
                    mapFields.Add(new XlsMapField(Field.StuHid, this.StuHid, new CodeChecker(1, 10)));
                }

                #region [MDY:20220808] 2022擴充案 名稱檢核 WordChecker(1, 20) 調整為 WordChecker(1, 40) (名稱欄位值最大40字)
                if (!String.IsNullOrWhiteSpace(this.ClassId))   //ClassListEntity
                {
                    mapFields.Add(new XlsMapField(Field.ClassId, this.ClassId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.ClassName)) //ClassListEntity
                {
                    mapFields.Add(new XlsMapField(Field.ClassName, this.ClassName, new WordChecker(1, 40)));
                }
                if (!String.IsNullOrWhiteSpace(this.DeptId))   //DeptListEntity
                {
                    mapFields.Add(new XlsMapField(Field.DeptId, this.DeptId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.DeptName))   //DeptListEntity
                {
                    mapFields.Add(new XlsMapField(Field.DeptName, this.DeptName, new WordChecker(1, 40)));
                }
                if (!String.IsNullOrWhiteSpace(this.CollegeId)) //CollegeListEntity
                {
                    mapFields.Add(new XlsMapField(Field.CollegeId, this.CollegeId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.CollegeName)) //CollegeListEntity
                {
                    mapFields.Add(new XlsMapField(Field.CollegeName, this.CollegeName, new WordChecker(1, 40)));
                }
                #endregion

                #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
                if (!String.IsNullOrWhiteSpace(this.MajorId)) //MajorListEntity
                {
                    mapFields.Add(new XlsMapField(Field.MajorId, this.MajorId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.MajorName)) //MajorListEntity
                {
                    mapFields.Add(new XlsMapField(Field.MajorName, this.MajorName, new WordChecker(1, 40)));
                }
                #endregion
            }
            #endregion

            #region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
            {
                #region [MDY:20220808] 2022擴充案 名稱檢核 WordChecker(1, 20) 調整為 WordChecker(1, 40)
                if (!String.IsNullOrWhiteSpace(this.ReduceId))  //ReduceListEntity
                {
                    mapFields.Add(new XlsMapField(Field.ReduceId, this.ReduceId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.ReduceName)) //ReduceListEntity
                {
                    mapFields.Add(new XlsMapField(Field.ReduceName, this.ReduceName, new WordChecker(1, 40)));
                }
                if (!String.IsNullOrWhiteSpace(this.LoanId))  //LoanListEntity
                {
                    mapFields.Add(new XlsMapField(Field.LoanId, this.LoanId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.LoanName)) //LoanListEntity
                {
                    mapFields.Add(new XlsMapField(Field.LoanName, this.LoanName, new WordChecker(1, 40)));
                }
                if (!String.IsNullOrWhiteSpace(this.DormId))  //DormListEntity
                {
                    mapFields.Add(new XlsMapField(Field.DormId, this.DormId, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.DormName)) //DormListEntity
                {
                    mapFields.Add(new XlsMapField(Field.DormName, this.DormName, new WordChecker(1, 40)));
                }
                #endregion
            }
            #endregion

            #region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
            {
                #region [MDY:20220808] 2022擴充案 名稱檢核 WordChecker(1, 20) 調整為 WordChecker(1, 40)
                if (!String.IsNullOrWhiteSpace(this.IdentifyId1))  //IdentifyList1Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyId1, this.IdentifyId1, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdentifyName1)) //IdentifyList1Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyName1, this.IdentifyName1, new WordChecker(1, 40)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyId2))  //IdentifyList2Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyId2, this.IdentifyId2, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdentifyName2)) //IdentifyList2Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyName2, this.IdentifyName2, new WordChecker(1, 40)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyId3))  //IdentifyList3Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyId3, this.IdentifyId3, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdentifyName3)) //IdentifyList3Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyName3, this.IdentifyName3, new WordChecker(1, 40)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyId4))  //IdentifyList4Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyId4, this.IdentifyId4, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdentifyName4)) //IdentifyList4Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyName4, this.IdentifyName4, new WordChecker(1, 40)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyId5))  //IdentifyList5Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyId5, this.IdentifyId5, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdentifyName5)) //IdentifyList5Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyName5, this.IdentifyName5, new WordChecker(1, 40)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyId6))  //IdentifyList6Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyId6, this.IdentifyId6, new CodeChecker(1, 20)));
                }
                if (!String.IsNullOrWhiteSpace(this.IdentifyName6)) //IdentifyList6Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyName6, this.IdentifyName6, new WordChecker(1, 40)));
                }
                #endregion
            }
            #endregion

            #region 收入科目金額對照欄位 (StudentReceiveEntity)
            {
                string[] fields = new string[] {
                    Field.Receive1, Field.Receive2, Field.Receive3, Field.Receive4, Field.Receive5,
                    Field.Receive6, Field.Receive7, Field.Receive8, Field.Receive9, Field.Receive10,
                    Field.Receive11, Field.Receive12, Field.Receive13, Field.Receive14, Field.Receive15,
                    Field.Receive16, Field.Receive17, Field.Receive18, Field.Receive19, Field.Receive20,
                    Field.Receive21, Field.Receive22, Field.Receive23, Field.Receive24, Field.Receive25,
                    Field.Receive26, Field.Receive27, Field.Receive28, Field.Receive29, Field.Receive30,
                    Field.Receive31, Field.Receive32, Field.Receive33, Field.Receive34, Field.Receive35,
                    Field.Receive36, Field.Receive37, Field.Receive38, Field.Receive39, Field.Receive40
                };
                string[] values = new string[] {
                    this.Receive1, this.Receive2, this.Receive3, this.Receive4, this.Receive5,
                    this.Receive6, this.Receive7, this.Receive8, this.Receive9, this.Receive10,
                    this.Receive11, this.Receive12, this.Receive13, this.Receive14, this.Receive15,
                    this.Receive16, this.Receive17, this.Receive18, this.Receive19, this.Receive20,
                    this.Receive21, this.Receive22, this.Receive23, this.Receive24, this.Receive25,
                    this.Receive26, this.Receive27, this.Receive28, this.Receive29, this.Receive30,
                    this.Receive31, this.Receive32, this.Receive33, this.Receive34, this.Receive35,
                    this.Receive36, this.Receive37, this.Receive38, this.Receive39, this.Receive40
                };
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (!String.IsNullOrWhiteSpace(values[idx]))
                    {
                        mapFields.Add(new XlsMapField(fields[idx], values[idx], new DecimalChecker(-999999999M, 999999999M)));
                    }
                }
            }
            #endregion

            #region 其他對照欄位 (StudentReceiveEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.StuCredit))
                {
                    mapFields.Add(new XlsMapField(Field.StuCredit, this.StuCredit, new DecimalChecker(0M, 999.99M)));
                }
                if (!String.IsNullOrWhiteSpace(this.StuHour))
                {
                    mapFields.Add(new XlsMapField(Field.StuHour, this.StuHour, new WordChecker(0, 10)));
                }
                if (!String.IsNullOrWhiteSpace(this.LoanAmount))
                {
                    mapFields.Add(new XlsMapField(Field.LoanAmount, this.LoanAmount, new DecimalChecker(0M, 999999999M)));
                }
                if (!String.IsNullOrWhiteSpace(this.ReceiveAmount))
                {
                    mapFields.Add(new XlsMapField(Field.ReceiveAmount, this.ReceiveAmount, new DecimalChecker(0M, 999999999M)));
                }
            }
            #endregion

            #region 學分基準、課程、學分數對照欄位 (StudentCourseEntity)
            {
                string[] creditIdFileds = new string[] {
                    Field.CreditId1, Field.CreditId2, Field.CreditId3, Field.CreditId4, Field.CreditId5,
                    Field.CreditId6, Field.CreditId7, Field.CreditId8, Field.CreditId9, Field.CreditId10,
                    Field.CreditId11, Field.CreditId12, Field.CreditId13, Field.CreditId14, Field.CreditId15,
                    Field.CreditId16, Field.CreditId17, Field.CreditId18, Field.CreditId19, Field.CreditId20,
                    Field.CreditId21, Field.CreditId22, Field.CreditId23, Field.CreditId24, Field.CreditId25,
                    Field.CreditId26, Field.CreditId27, Field.CreditId28, Field.CreditId29, Field.CreditId30,
                    Field.CreditId31, Field.CreditId32, Field.CreditId33, Field.CreditId34, Field.CreditId35,
                    Field.CreditId36, Field.CreditId37, Field.CreditId38, Field.CreditId39, Field.CreditId40
                };
                string[] creditIdValues = new string[] {
                    this.CreditId1, this.CreditId2, this.CreditId3, this.CreditId4, this.CreditId5,
                    this.CreditId6, this.CreditId7, this.CreditId8, this.CreditId9, this.CreditId10,
                    this.CreditId11, this.CreditId12, this.CreditId13, this.CreditId14, this.CreditId15,
                    this.CreditId16, this.CreditId17, this.CreditId18, this.CreditId19, this.CreditId20,
                    this.CreditId21, this.CreditId22, this.CreditId23, this.CreditId24, this.CreditId25,
                    this.CreditId26, this.CreditId27, this.CreditId28, this.CreditId29, this.CreditId30,
                    this.CreditId31, this.CreditId32, this.CreditId33, this.CreditId34, this.CreditId35,
                    this.CreditId36, this.CreditId37, this.CreditId38, this.CreditId39, this.CreditId40
                };
                string[] courseIdFileds = new string[] {
                    Field.CourseId1, Field.CourseId2, Field.CourseId3, Field.CourseId4, Field.CourseId5,
                    Field.CourseId6, Field.CourseId7, Field.CourseId8, Field.CourseId9, Field.CourseId10,
                    Field.CourseId11, Field.CourseId12, Field.CourseId13, Field.CourseId14, Field.CourseId15,
                    Field.CourseId16, Field.CourseId17, Field.CourseId18, Field.CourseId19, Field.CourseId20,
                    Field.CourseId21, Field.CourseId22, Field.CourseId23, Field.CourseId24, Field.CourseId25,
                    Field.CourseId26, Field.CourseId27, Field.CourseId28, Field.CourseId29, Field.CourseId30,
                    Field.CourseId31, Field.CourseId32, Field.CourseId33, Field.CourseId34, Field.CourseId35,
                    Field.CourseId36, Field.CourseId37, Field.CourseId38, Field.CourseId39, Field.CourseId40
                };
                string[] courseIdValues = new string[] {
                    this.CourseId1, this.CourseId2, this.CourseId3, this.CourseId4, this.CourseId5,
                    this.CourseId6, this.CourseId7, this.CourseId8, this.CourseId9, this.CourseId10,
                    this.CourseId11, this.CourseId12, this.CourseId13, this.CourseId14, this.CourseId15,
                    this.CourseId16, this.CourseId17, this.CourseId18, this.CourseId19, this.CourseId20,
                    this.CourseId21, this.CourseId22, this.CourseId23, this.CourseId24, this.CourseId25,
                    this.CourseId26, this.CourseId27, this.CourseId28, this.CourseId29, this.CourseId30,
                    this.CourseId31, this.CourseId32, this.CourseId33, this.CourseId34, this.CourseId35,
                    this.CourseId36, this.CourseId37, this.CourseId38, this.CourseId39, this.CourseId40
                };
                string[] creditFileds = new string[] {
                    Field.Credit1, Field.Credit2, Field.Credit3, Field.Credit4, Field.Credit5,
                    Field.Credit6, Field.Credit7, Field.Credit8, Field.Credit9, Field.Credit10,
                    Field.Credit11, Field.Credit12, Field.Credit13, Field.Credit14, Field.Credit15,
                    Field.Credit16, Field.Credit17, Field.Credit18, Field.Credit19, Field.Credit20,
                    Field.Credit21, Field.Credit22, Field.Credit23, Field.Credit24, Field.Credit25,
                    Field.Credit26, Field.Credit27, Field.Credit28, Field.Credit29, Field.Credit30,
                    Field.Credit31, Field.Credit32, Field.Credit33, Field.Credit34, Field.Credit35,
                    Field.Credit36, Field.Credit37, Field.Credit38, Field.Credit39, Field.Credit40
                };
                string[] creditValues = new string[] {
                    this.Credit1, this.Credit2, this.Credit3, this.Credit4, this.Credit5,
                    this.Credit6, this.Credit7, this.Credit8, this.Credit9, this.Credit10,
                    this.Credit11, this.Credit12, this.Credit13, this.Credit14, this.Credit15,
                    this.Credit16, this.Credit17, this.Credit18, this.Credit19, this.Credit20,
                    this.Credit21, this.Credit22, this.Credit23, this.Credit24, this.Credit25,
                    this.Credit26, this.Credit27, this.Credit28, this.Credit29, this.Credit30,
                    this.Credit31, this.Credit32, this.Credit33, this.Credit34, this.Credit35,
                    this.Credit36, this.Credit37, this.Credit38, this.Credit39, this.Credit40
                };
                for (int idx = 0; idx < creditIdValues.Length; idx++)
                {
                    if (!String.IsNullOrWhiteSpace(creditIdValues[idx]))
                    {
                        mapFields.Add(new XlsMapField(creditIdFileds[idx], creditIdValues[idx], new CodeChecker(0, 8)));
                    }
                    if (!String.IsNullOrWhiteSpace(courseIdValues[idx]))
                    {
                        mapFields.Add(new XlsMapField(courseIdFileds[idx], courseIdValues[idx], new CodeChecker(0, 8)));
                    }
                    if (!String.IsNullOrWhiteSpace(creditValues[idx]))
                    {
                        mapFields.Add(new XlsMapField(creditFileds[idx], creditValues[idx], new DecimalChecker(0M, 999.99M)));
                    }
                }
            }
            #endregion

            #region Remark (StudentReceiveEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.Remark))
                {
                    mapFields.Add(new XlsMapField(Field.Remark, this.Remark, null));
                }
            }
            #endregion

            #region 扣款資料相關對照欄位 (StudentReceiveEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.DeductBankid))
                {
                    //TODO:測試
                    //mapFields.Add(new XlsMapField(Field.DeductBankid, this.DeductBankid, new NumberChecker(7, 7, "7碼的銀行代碼")));
                    mapFields.Add(new XlsMapField(Field.DeductBankid, this.DeductBankid, null));
                }
                if (!String.IsNullOrWhiteSpace(this.DeductAccountno))
                {
                    mapFields.Add(new XlsMapField(Field.DeductAccountno, this.DeductAccountno, new NumberChecker(0, 21, "0~21碼數字")));
                }
                if (!String.IsNullOrWhiteSpace(this.DeductAccountname))
                {
                    mapFields.Add(new XlsMapField(Field.DeductAccountname, this.DeductAccountname, new WordChecker(0, 60)));
                }
                if (!String.IsNullOrWhiteSpace(this.DeductAccountid))
                {
                    mapFields.Add(new XlsMapField(Field.DeductAccountid, this.DeductAccountid, new CharChecker(0, 10)));
                }
            }
            #endregion

            #region 虛擬帳號資料相關對照欄位 (StudentReceiveEntity)
            {
                if (!String.IsNullOrWhiteSpace(this.SeriorNo))
                {
                    mapFields.Add(new XlsMapField(Field.SeriorNo, this.SeriorNo, new NumberChecker(1, 11, "流水號必須為數字")));
                }
                if (!String.IsNullOrWhiteSpace(this.CancelNo))
                {
                    mapFields.Add(new XlsMapField(Field.CancelNo, this.CancelNo, new NumberChecker(12, 16, "12、14或16碼的虛擬帳號數字")));
                }
            }
            #endregion

            #region 備註對照欄位 (StudentReceiveEntity)
            {
                string[] memoFileds = new string[MemoCount] {
                    Field.Memo01, Field.Memo02, Field.Memo03, Field.Memo04, Field.Memo05,
                    Field.Memo06, Field.Memo07, Field.Memo08, Field.Memo09, Field.Memo10,
                    Field.Memo11, Field.Memo12, Field.Memo13, Field.Memo14, Field.Memo15,
                    Field.Memo16, Field.Memo17, Field.Memo18, Field.Memo19, Field.Memo20,
                    Field.Memo21
                };
                string[] memoValues = new string[MemoCount] {
                    this.Memo01, this.Memo02, this.Memo03, this.Memo04, this.Memo05,
                    this.Memo06, this.Memo07, this.Memo08, this.Memo09, this.Memo10,
                    this.Memo11, this.Memo12, this.Memo13, this.Memo14, this.Memo15,
                    this.Memo16, this.Memo17, this.Memo18, this.Memo19, this.Memo20,
                    this.Memo21
                };

                for (int idx = 0; idx < MemoCount; idx++)
                {
                    if (!String.IsNullOrWhiteSpace(memoValues[idx]))
                    {
                        mapFields.Add(new XlsMapField(memoFileds[idx], memoValues[idx], new WordChecker(0, 50)));
                    }
                }
            }
            #endregion

            #region [MDY:20220808] 2022擴充案 英文名稱相關對照欄位 (名稱欄位值最大140字)
            #region 學籍資料英文名稱對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
            if (!String.IsNullOrWhiteSpace(this.DeptEName))   //DeptListEntity
            {
                mapFields.Add(new XlsMapField(Field.DeptEName, this.DeptEName, new WordChecker(1, 140)));
            }

            if (!String.IsNullOrWhiteSpace(this.CollegeEName)) //CollegeListEntity
            {
                mapFields.Add(new XlsMapField(Field.CollegeEName, this.CollegeEName, new WordChecker(1, 140)));
            }

            if (!String.IsNullOrWhiteSpace(this.MajorEName)) //MajorListEntity
            {
                mapFields.Add(new XlsMapField(Field.MajorEName, this.MajorEName, new WordChecker(1, 140)));
            }

            if (!String.IsNullOrWhiteSpace(this.ClassEName)) //ClassListEntity
            {
                mapFields.Add(new XlsMapField(Field.ClassEName, this.ClassEName, new WordChecker(1, 140)));
            }
            #endregion

            #region 減免、就貸、住宿英文名稱對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
            if (!String.IsNullOrWhiteSpace(this.ReduceEName)) //ReduceListEntity
            {
                mapFields.Add(new XlsMapField(Field.ReduceEName, this.ReduceEName, new WordChecker(1, 140)));
            }

            if (!String.IsNullOrWhiteSpace(this.DormEName)) //DormListEntity
            {
                mapFields.Add(new XlsMapField(Field.DormEName, this.DormEName, new WordChecker(1, 140)));
            }

            if (!String.IsNullOrWhiteSpace(this.LoanEName)) //LoanListEntity
            {
                mapFields.Add(new XlsMapField(Field.LoanEName, this.LoanEName, new WordChecker(1, 140)));
            }
            #endregion

            #region 身分註記英文名稱對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
            {
                if (!String.IsNullOrWhiteSpace(this.IdentifyEName1)) //IdentifyList1Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyEName1, this.IdentifyEName1, new WordChecker(1, 140)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyEName2)) //IdentifyList2Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyEName2, this.IdentifyEName2, new WordChecker(1, 140)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyEName3)) //IdentifyList3Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyEName3, this.IdentifyEName3, new WordChecker(1, 140)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyEName4)) //IdentifyList4Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyEName4, this.IdentifyEName4, new WordChecker(1, 140)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyEName5)) //IdentifyList5Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyEName5, this.IdentifyEName5, new WordChecker(1, 140)));
                }

                if (!String.IsNullOrWhiteSpace(this.IdentifyEName6)) //IdentifyList6Entity
                {
                    mapFields.Add(new XlsMapField(Field.IdentifyEName6, this.IdentifyEName6, new WordChecker(1, 140)));
                }
            }
            #endregion
            #endregion

            return mapFields.ToArray();
        }

        #region [MDY:20220808] 2022擴充案 收入科目英文名稱相關 Method
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
        #endregion

        #region [MDY:20220808] 2022擴充案 備註項目英文標題相關 Method
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
        #endregion


        #region 目前沒人用，暫時不提供
        ///// <summary>
        ///// 設定上傳資料相關欄位的值，未指定的欄位將被設為 null
        ///// </summary>
        ///// <param name="mapFields"></param>
        ///// <returns></returns>
        //internal bool SetMapFields(ICollection<XlsMapField> mapFields)
        //{
        //    if (mapFields == null)
        //    {
        //        mapFields = new XlsMapField[0];
        //    }

        //    bool isOK = true;
        //    XlsMapField mapField = null;

        //    #region 學生資料對照欄位
        //    {
        //        #region StuId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuId);
        //        this.StuId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuName);
        //        this.StuName = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuBirthday
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuBirthday);
        //        this.StuBirthday = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdNumber
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdNumber);
        //        this.IdNumber = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuTel
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuTel);
        //        this.StuTel = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuAddcode
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuAddcode);
        //        this.StuAddcode = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuAdd
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuAdd);
        //        this.StuAdd = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region Email
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.Email);
        //        this.Email = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuParent
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuParent);
        //        this.StuParent = mapField == null ? null : mapField.CellName;
        //        #endregion
        //    }
        //    #endregion

        //    #region 學籍資料對照欄位
        //    {
        //        #region StuGrade
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuGrade);
        //        this.StuGrade = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuHid
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuHid);
        //        this.StuHid = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region ClassId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ClassId);
        //        this.ClassId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region ClassName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ClassName);
        //        this.ClassName = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DeptId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeptId);
        //        this.DeptId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DeptName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeptName);
        //        this.DeptName = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region CollegeId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.CollegeId);
        //        this.CollegeId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region CollegeName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.CollegeName);
        //        this.CollegeName = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region MajorId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.MajorId);
        //        this.MajorId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region MajorName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.MajorName);
        //        this.MajorName = mapField == null ? null : mapField.CellName;
        //        #endregion
        //    }
        //    #endregion

        //    #region 減免、就貸、住宿對照欄位
        //    {
        //        #region ReduceId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ReduceId);
        //        this.ReduceId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region ReduceName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ReduceName);
        //        this.ReduceName = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region LoanId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.LoanId);
        //        this.LoanId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region LoanName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.LoanName);
        //        this.LoanName = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DormId
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DormId);
        //        this.DormId = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DormName
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DormName);
        //        this.DormName = mapField == null ? null : mapField.CellName;
        //        #endregion
        //    }
        //    #endregion

        //    #region 身分註記對照欄位
        //    {
        //        #region IdentifyId1
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId1);
        //        this.IdentifyId1 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyName1
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName1);
        //        this.IdentifyName1 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyId2
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId2);
        //        this.IdentifyId2 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyName2
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName2);
        //        this.IdentifyName2 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyId3
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId3);
        //        this.IdentifyId3 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyName3
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName3);
        //        this.IdentifyName3 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyId4
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId4);
        //        this.IdentifyId4 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyName4
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName4);
        //        this.IdentifyName4 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyId5
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId5);
        //        this.IdentifyId5 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyName5
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName5);
        //        this.IdentifyName5 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyId6
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyId6);
        //        this.IdentifyId6 = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region IdentifyName6
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.IdentifyName6);
        //        this.IdentifyName6 = mapField == null ? null : mapField.CellName;
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
        //            mapField = mapFields.First(x => x.Key == field);
        //            string fieldName = string.Format("Receive{0}", no);
        //            object fieldValue = mapField == null ? null : mapField.CellName;
        //            isOK &= this.SetValue(fieldName, fieldValue).IsSuccess;
        //        }
        //    }
        //    #endregion

        //    #region 其他對照欄位 (StudentReceiveEntity)
        //    {
        //        #region StuCredit
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuCredit);
        //        this.StuCredit = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region StuHour
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.StuHour);
        //        this.StuHour = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region LoanAmount
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.LoanAmount);
        //        this.LoanAmount = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region ReceiveAmount
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.ReceiveAmount);
        //        this.ReceiveAmount = mapField == null ? null : mapField.CellName;
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
        //                mapField = mapFields.First(x => x.Key == field);
        //                string fieldName = string.Format("CreditId{0}", no);
        //                object fieldValue = mapField == null ? null : mapField.CellName;
        //                isOK &= this.SetValue(fieldName, fieldValue).IsSuccess;
        //            }
        //            #endregion

        //            #region CourseId
        //            {
        //                string field = creditIdFileds[idx];
        //                mapField = mapFields.First(x => x.Key == field);
        //                string fieldName = string.Format("CourseId{0}", no);
        //                object fieldValue = mapField == null ? null : mapField.CellName;
        //                isOK &= this.SetValue(fieldName, fieldValue).IsSuccess;
        //            }
        //            #endregion

        //            #region Credit
        //            {
        //                string field = creditIdFileds[idx];
        //                mapField = mapFields.First(x => x.Key == field);
        //                string fieldName = string.Format("Credit{0}", no);
        //                object fieldValue = mapField == null ? null : mapField.CellName;
        //                isOK &= this.SetValue(fieldName, fieldValue).IsSuccess;
        //            }
        //            #endregion
        //        }
        //    }
        //    #endregion

        //    #region Remark (StudentReceiveEntity)
        //    {
        //        #region Remark
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.Remark);
        //        this.Remark = mapField == null ? null : mapField.CellName;
        //        #endregion
        //    }
        //    #endregion

        //    #region 扣款資料相關對照欄位 (StudentReceiveEntity)
        //    {
        //        #region DeductBankid
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductBankid);
        //        this.DeductBankid = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DeductAccountno
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductAccountno);
        //        this.DeductAccountno = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DeductAccountname
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductAccountname);
        //        this.DeductAccountname = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region DeductAccountid
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.DeductAccountid);
        //        this.DeductAccountid = mapField == null ? null : mapField.CellName;
        //        #endregion
        //    }
        //    #endregion

        //    #region 虛擬帳號資料相關對照欄位 (StudentReceiveEntity)
        //    {
        //        #region SeriorNo
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.SeriorNo);
        //        this.SeriorNo = mapField == null ? null : mapField.CellName;
        //        #endregion

        //        #region CancelNo
        //        mapField = mapFields.First(x => x.Key == MappingreXlsmdbEntity.Field.CancelNo);
        //        this.CancelNo = mapField == null ? null : mapField.CellName;
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
        //            mapField = mapFields.First(x => x.Key == field);
        //            string fieldName = string.Format("Memo{0}", no);
        //            object fieldValue = mapField == null ? null : mapField.CellName;
        //            isOK &= this.SetValue(fieldName, fieldValue).IsSuccess;
        //        }
        //    }
        //    #endregion

        //    return isOK;
        //}
        #endregion
        #endregion
    }
}
