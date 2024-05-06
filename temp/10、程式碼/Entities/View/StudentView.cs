using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 學生 View
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class StudentView : Entity
    {
        #region [Old]
//        protected const string VIEWSQL = @"
//SELECT sc.Pay_Date, sc.Bill_Valid_Date, sc.Bill_Close_Date, 
// M.Receive_Type, R.Year_Id, R.Term_Id, R.Dep_Id, R.Receive_Id, M.Stu_Id, M.Stu_Name, M.Stu_Birthday,
// M.Id_Number, M.Stu_Tel, M.Stu_Addcode, M.Stu_Address, M.Stu_Email, M.Stu_Account, M.create_date, M.update_date,
// R.Up_No, R.Up_Order, R.Stu_Grade, R.College_Id, R.Major_Id, R.Class_Id, R.Stu_Credit, R.Stu_Hour, R.Reduce_Id,
// R.Dorm_Id, R.Loan_Id, R.Agency_List, R.Billing_type, R.Identify_Id01, R.Identify_Id02, R.Identify_Id03, 
// R.Identify_Id04, R.Identify_Id05, R.Identify_Id06, R.Receive_01, R.Receive_02, R.Receive_03, R.Receive_04, 
// R.Receive_05, R.Receive_06, R.Receive_07, R.Receive_08, R.Receive_09, R.Receive_10, R.Receive_11, R.Receive_12, 
// R.Receive_13, R.Receive_14, R.Receive_15, R.Receive_16, R.Receive_Amount, R.Receive_ATMAmount, R.Receive_EB1Amount, 
// R.Receive_EB2Amount, R.Receive_SMAmount, R.Loan_Amount, R.Reissue_Flag, R.Serior_No, R.Cancel_No, R.Cancel_ATMNo, 
// R.Cancel_EB1No, R.Cancel_EB2No, R.Cancel_PONo, R.Cancel_SMNo, R.Cancel_Flag, R.Receivebank_Id, R.Receive_Date, R.Receive_Time, 
// R.Account_Date, R.Receive_Way, R.Process_Fee, R.Real_Receive, R.Cancel_Receive, R.Fee_Receivable, R.Fee_Payable, R.Postseq,
// R.Atm_meno, R.stu_hid, R.e_flag, R.c_flag, R.loan, R.real_loan, R.Cancel_Date, R.PrintCreate_Date, R.Print_Date, 
// R.exportReceiveData, R.InvCreate_Date, R.Inv_Date, R.MAPPING_TYPE, R.MAPPING_ID, R.remark
// From Student_Master M inner join Student_Receive R on M.Receive_Type = R.Receive_Type and M.Stu_Id = R.Stu_Id 
// inner join School_Rid sc on R.Receive_Type = sc.Receive_Type and R.Year_Id=sc.Year_Id and R.Term_Id=sc.Term_Id and R.Dep_Id=sc.Dep_Id and R.Receive_Id=sc.Receive_Id ";
        #endregion

        #region [Old]
//        protected const string VIEWSQL = @"
//SELECT sc.Pay_Date, sc.Bill_Valid_Date, sc.Bill_Close_Date
//     , M.Receive_Type, R.Year_Id, R.Term_Id, R.Dep_Id, R.Receive_Id, M.Stu_Id, M.Stu_Name, M.Stu_Birthday
//     , M.Id_Number, M.Stu_Tel, M.Stu_Addcode, M.Stu_Address, M.Stu_Email, M.Stu_Account, M.create_date, M.update_date
//     , R.Up_No, R.Up_Order, R.Stu_Grade, R.College_Id, R.Major_Id, R.Class_Id, R.Stu_Credit, R.Stu_Hour, R.Reduce_Id
//     , R.Dorm_Id, R.Loan_Id, R.Agency_List, R.Billing_type
//     , R.Identify_Id01, R.Identify_Id02, R.Identify_Id03, R.Identify_Id04, R.Identify_Id05, R.Identify_Id06
//     --, R.Receive_01, R.Receive_02, R.Receive_03, R.Receive_04, R.Receive_05, R.Receive_06, R.Receive_07, R.Receive_08, R.Receive_09, R.Receive_10
//     --, R.Receive_11, R.Receive_12, R.Receive_13, R.Receive_14, R.Receive_15, R.Receive_16
//     , R.Receive_Amount, R.Receive_ATMAmount, R.Receive_EB1Amount
//     , R.Receive_EB2Amount, R.Receive_SMAmount, R.Loan_Amount, R.Reissue_Flag, R.Serior_No, R.Cancel_No, R.Cancel_ATMNo
//     , R.Cancel_EB1No, R.Cancel_EB2No, R.Cancel_PONo, R.Cancel_SMNo, R.Cancel_Flag, R.Receivebank_Id, R.Receive_Date, R.Receive_Time
//     , R.Account_Date, R.Receive_Way, R.Process_Fee, R.Real_Receive, R.Cancel_Receive, R.Fee_Receivable, R.Fee_Payable, R.Postseq
//     , R.Atm_meno, R.stu_hid, R.e_flag, R.c_flag, R.loan, R.real_loan, R.Cancel_Date, R.PrintCreate_Date, R.Print_Date
//     , R.exportReceiveData, R.InvCreate_Date, R.Inv_Date, R.MAPPING_TYPE, R.MAPPING_ID, R.remark, R.CancelFlagFields
//  From Student_Master M 
// inner join Student_Receive R on M.Receive_Type = R.Receive_Type and M.Stu_Id = R.Stu_Id 
// inner join School_Rid sc on R.Receive_Type = sc.Receive_Type and R.Year_Id=sc.Year_Id and R.Term_Id=sc.Term_Id and R.Dep_Id=sc.Dep_Id and R.Receive_Id=sc.Receive_Id ";
        #endregion

        #region [Old]
//        protected const string VIEWSQL = @"SELECT A.Receive_Type, A.Year_Id, A.Term_Id, A.Dep_Id, A.Receive_Id, A.Stu_Id
//     , C.Stu_Name, C.Stu_Birthday, C.Id_Number, C.Stu_Tel, C.Stu_Addcode, C.Stu_Address, C.Stu_Email
//     , A.Up_No, A.Receive_Amount, A.Receive_ATMAmount, A.Receive_SMAmount, A.Cancel_No, A.Cancel_ATMNo, A.Cancel_SMNo
//     , A.Cancel_Flag, A.Receive_Date, A.Receive_Time, A.Receive_Way, A.Receivebank_Id, A.Account_Date
//     , A.CancelFlagFields
//   --, A.Up_Order, A.Stu_Grade, A.Dept_Id, A.College_Id, A.Major_Id, A.Class_Id, A.Stu_Credit, A.Stu_Hid, A.Reduce_Id
//     , B.Pay_Date, B.Bill_Valid_Date, B.Bill_Close_Date
//  FROM Student_Receive AS A
//  LEFT JOIN School_Rid AS B ON B.Receive_Type = A.Receive_Type AND B.Year_Id = A.Year_Id AND B.Term_Id = A.Term_Id AND B.Dep_Id = A.Dep_Id AND B.Receive_Id = A.Receive_Id
//  LEFT JOIN Student_Master AS C ON C.Receive_Type = A.Receive_Type AND C.Dep_Id = A.Dep_Id AND C.Stu_Id = A.Stu_Id ";
        #endregion

        #region [Old]
//        public const string VIEWSQL = @"SELECT A.Receive_Type, A.Year_Id, A.Term_Id, A.Dep_Id, A.Receive_Id, A.Stu_Id
//     , C.Stu_Name, C.Stu_Birthday, C.Id_Number, C.Stu_Tel, C.Stu_Addcode, C.Stu_Address, C.Stu_Email
//     , A.Major_Id, A.Stu_Grade
//     , A.Up_No, A.Receive_Amount, A.Receive_ATMAmount, A.Receive_SMAmount, A.Cancel_No, A.Cancel_ATMNo, A.Cancel_SMNo
//     , A.Cancel_Flag, A.Receive_Date, A.Receive_Time, A.Receive_Way, A.Receivebank_Id, A.Account_Date
//     , A.CancelFlagFields
//   --, A.Up_Order, A.Dept_Id, A.College_Id, A.Class_Id, A.Stu_Credit, A.Stu_Hid, A.Reduce_Id
//     , B.Pay_Date, B.Bill_Valid_Date, B.Bill_Close_Date
//  FROM Student_Receive AS A
//  LEFT JOIN School_Rid AS B ON B.Receive_Type = A.Receive_Type AND B.Year_Id = A.Year_Id AND B.Term_Id = A.Term_Id AND B.Dep_Id = A.Dep_Id AND B.Receive_Id = A.Receive_Id
//  LEFT JOIN Student_Master AS C ON C.Receive_Type = A.Receive_Type AND C.Dep_Id = A.Dep_Id AND C.Stu_Id = A.Stu_Id ";
        #endregion

        #region [Old]
//        public const string VIEWSQL = @"SELECT A.Receive_Type, A.Year_Id, A.Term_Id, A.Dep_Id, A.Receive_Id, A.Stu_Id, A.Old_Seq
//     , C.Stu_Name, C.Stu_Birthday, C.Id_Number, C.Stu_Tel, C.Stu_Addcode, C.Stu_Address, C.Stu_Email
//     , A.Major_Id, A.Stu_Grade
//     , A.Up_No, A.Receive_Amount, A.Receive_ATMAmount, A.Receive_SMAmount, A.Cancel_No, A.Cancel_ATMNo, A.Cancel_SMNo
//     , A.Cancel_Flag, A.Receive_Date, A.Receive_Time, A.Receive_Way, A.Receivebank_Id, A.Account_Date
//     , A.CancelFlagFields
//   --, A.Up_Order, A.Dept_Id, A.College_Id, A.Class_Id, A.Stu_Credit, A.Stu_Hid, A.Reduce_Id
//     , B.Pay_Date, B.Bill_Valid_Date, B.Bill_Close_Date
//  FROM Student_Receive AS A
//  LEFT JOIN School_Rid AS B ON B.Receive_Type = A.Receive_Type AND B.Year_Id = A.Year_Id AND B.Term_Id = A.Term_Id AND B.Dep_Id = A.Dep_Id AND B.Receive_Id = A.Receive_Id
//  LEFT JOIN Student_Master AS C ON C.Receive_Type = A.Receive_Type AND C.Dep_Id = A.Dep_Id AND C.Stu_Id = A.Stu_Id ";
        #endregion

        #region [Old]
//        public const string VIEWSQL = @"SELECT A.Receive_Type, A.Year_Id, A.Term_Id, A.Dep_Id, A.Receive_Id, A.Stu_Id, A.Old_Seq
//     , C.Stu_Name, C.Stu_Birthday, C.Id_Number, C.Stu_Tel, C.Stu_Addcode, C.Stu_Address, C.Stu_Email
//     , A.Major_Id, A.Stu_Grade
//     , A.Up_No, A.Receive_Amount, A.Receive_ATMAmount, A.Receive_SMAmount, A.Cancel_No, A.Cancel_ATMNo, A.Cancel_SMNo
//     , A.Cancel_Flag, A.Receive_Date, A.Receive_Time, A.Receive_Way, A.Receivebank_Id, A.Account_Date
//     , A.CancelFlagFields
//   --, A.Up_Order, A.Dept_Id, A.College_Id, A.Class_Id, A.Stu_Credit, A.Stu_Hid, A.Reduce_Id
//     , B.Pay_Date, B.Bill_Valid_Date, B.Bill_Close_Date
//     , ISNULL((SELECT Receive_Name FROM Receive_List AS RL WHERE RL.Receive_Type = A.Receive_Type AND RL.Year_Id = A.Year_Id AND RL.Term_Id = A.Term_Id AND RL.Dep_Id = A.Dep_Id AND RL.Receive_Id = A.Receive_Id), '') AS Receive_Name
//  FROM Student_Receive AS A
//  LEFT JOIN School_Rid AS B ON B.Receive_Type = A.Receive_Type AND B.Year_Id = A.Year_Id AND B.Term_Id = A.Term_Id AND B.Dep_Id = A.Dep_Id AND B.Receive_Id = A.Receive_Id
//  LEFT JOIN Student_Master AS C ON C.Receive_Type = A.Receive_Type AND C.Dep_Id = A.Dep_Id AND C.Stu_Id = A.Stu_Id ";
        #endregion

        public const string VIEWSQL = @"SELECT A.Receive_Type, A.Year_Id, A.Term_Id, A.Dep_Id, A.Receive_Id, A.Stu_Id, A.Old_Seq
     , C.Stu_Name, C.Stu_Birthday, C.Id_Number, C.Stu_Tel, C.Stu_Addcode, C.Stu_Address, C.Stu_Email, C.Stu_Parent
     , A.Major_Id, A.Stu_Grade
     , A.Up_No, A.Receive_Amount, A.Receive_ATMAmount, A.Receive_SMAmount, A.Cancel_No, A.Cancel_ATMNo, A.Cancel_SMNo
     , A.Cancel_Flag, A.Receive_Date, A.Receive_Time, A.Receive_Way, A.Receivebank_Id, A.Account_Date
     , A.CancelFlagFields
   --, A.Up_Order, A.Dept_Id, A.College_Id, A.Class_Id, A.Stu_Credit, A.Stu_Hid, A.Reduce_Id
     , B.Pay_Date, B.Bill_Valid_Date, B.Bill_Close_Date
     , ISNULL((SELECT Receive_Name FROM Receive_List AS RL WHERE RL.Receive_Type = A.Receive_Type AND RL.Year_Id = A.Year_Id AND RL.Term_Id = A.Term_Id AND RL.Dep_Id = A.Dep_Id AND RL.Receive_Id = A.Receive_Id), '') AS Receive_Name
  FROM Student_Receive AS A
  LEFT JOIN School_Rid AS B ON B.Receive_Type = A.Receive_Type AND B.Year_Id = A.Year_Id AND B.Term_Id = A.Term_Id AND B.Dep_Id = A.Dep_Id AND B.Receive_Id = A.Receive_Id
  LEFT JOIN Student_Master AS C ON C.Receive_Type = A.Receive_Type AND C.Dep_Id = A.Dep_Id AND C.Stu_Id = A.Stu_Id ";

        #region Field Name Const Class
        /// <summary>
        /// UsersView 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
			/// <summary>
			/// 代收類別代碼
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 學年代碼
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代碼
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代碼 (土銀不使用)
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 代收費用別代碼
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// 學號
			/// </summary>
			public const string StuId = "Stu_Id";

            /// <summary>
            /// 舊資料序號
            /// </summary>
            public const string OldSeq = "Old_Seq";
            #endregion

            #region Data
            #region 學生基本資料相關
            /// <summary>
			/// 姓名 欄位名稱常數定義
			/// </summary>
			public const string Name = "Stu_Name";

			/// <summary>
			/// 生日 (民國年月日 7 碼) 欄位名稱常數定義
			/// </summary>
			public const string Birthday = "Stu_Birthday";

			/// <summary>
			/// 身分證字號 欄位名稱常數定義
			/// </summary>
			public const string IdNumber = "Id_Number";

			/// <summary>
			/// 電話 欄位名稱常數定義
			/// </summary>
			public const string Tel = "Stu_Tel";

			/// <summary>
			/// 郵遞區號 欄位名稱常數定義
			/// </summary>
			public const string ZipCode = "Stu_Addcode";

			/// <summary>
			/// 地址 欄位名稱常數定義
			/// </summary>
			public const string Address = "Stu_Address";

			/// <summary>
			/// EMail 欄位名稱常數定義
			/// </summary>
			public const string Email = "Stu_Email";

            /// <summary>
            /// 家長名稱
            /// </summary>
            public const string StuParent = "Stu_Parent";
            #endregion

            #region [Old]
            ///// <summary>
            ///// Stu_Account 欄位名稱常數定義
            ///// </summary>
            //public const string Account = "Stu_Account";

            ///// <summary>
            ///// 資料建立日期
            ///// </summary>
            //public const string CrtDate = "create_date";

            ///// <summary>
            ///// 資料最後修改日期
            ///// </summary>
            //public const string MdyDate = "update_date";

            ///// <summary>
            ///// 接收順序
            ///// </summary>
            //public const string UpOrder = "Up_Order";

            ///// <summary>
            ///// 年級代碼
            ///// </summary>
            //public const string StuGrade = "Stu_Grade";

            ///// <summary>
            ///// 院別代碼
            ///// </summary>
            //public const string CollegeId = "College_Id";

            ///// <summary>
            ///// 班別代碼
            ///// </summary>
            //public const string ClassId = "Class_Id";

            ///// <summary>
            ///// 學分數
            ///// </summary>
            //public const string StuCredit = "Stu_Credit";

            ///// <summary>
            ///// 上課時數
            ///// </summary>
            //public const string StuHour = "Stu_Hour";

            ///// <summary>
            ///// 減免代碼
            ///// </summary>
            //public const string ReduceId = "Reduce_Id";

            ///// <summary>
            ///// 住宿代碼
            ///// </summary>
            //public const string DormId = "Dorm_Id";

            ///// <summary>
            ///// 就貸代碼
            ///// </summary>
            //public const string LoanId = "Loan_Id";

            ///// <summary>
            ///// 代辦費明細項目
            ///// </summary>
            //public const string AgencyList = "Agency_List";

            ///// <summary>
            ///// 計算方式 (1=依收費標準計算 / 2=依輸入金額計算)
            ///// </summary>
            //public const string BillingType = "Billing_type";

            ///// <summary>
            ///// 身份註記一代碼
            ///// </summary>
            //public const string IdentifyId01 = "Identify_Id01";

            ///// <summary>
            ///// 身份註記二代碼
            ///// </summary>
            //public const string IdentifyId02 = "Identify_Id02";

            ///// <summary>
            ///// 身份註記三代碼
            ///// </summary>
            //public const string IdentifyId03 = "Identify_Id03";

            ///// <summary>
            ///// 身份註記四代碼
            ///// </summary>
            //public const string IdentifyId04 = "Identify_Id04";

            ///// <summary>
            ///// 身份註記五代碼
            ///// </summary>
            //public const string IdentifyId05 = "Identify_Id05";

            ///// <summary>
            ///// 身份註記六代碼
            ///// </summary>
            //public const string IdentifyId06 = "Identify_Id06";

            ///// <summary>
            ///// 代收課目01金額
            ///// </summary>
            //public const string Receive01 = "Receive_01";

            ///// <summary>
            ///// 代收課目02金額
            ///// </summary>
            //public const string Receive02 = "Receive_02";

            ///// <summary>
            ///// 代收課目03金額
            ///// </summary>
            //public const string Receive03 = "Receive_03";

            ///// <summary>
            ///// 代收課目04金額
            ///// </summary>
            //public const string Receive04 = "Receive_04";

            ///// <summary>
            ///// 代收課目05金額
            ///// </summary>
            //public const string Receive05 = "Receive_05";

            ///// <summary>
            ///// 代收課目06金額
            ///// </summary>
            //public const string Receive06 = "Receive_06";

            ///// <summary>
            ///// 代收課目07金額
            ///// </summary>
            //public const string Receive07 = "Receive_07";

            ///// <summary>
            ///// 代收課目08金額
            ///// </summary>
            //public const string Receive08 = "Receive_08";

            ///// <summary>
            ///// 代收課目09金額
            ///// </summary>
            //public const string Receive09 = "Receive_09";

            ///// <summary>
            ///// 代收課目10金額
            ///// </summary>
            //public const string Receive10 = "Receive_10";

            ///// <summary>
            ///// 代收課目11金額
            ///// </summary>
            //public const string Receive11 = "Receive_11";

            ///// <summary>
            ///// 代收課目12金額
            ///// </summary>
            //public const string Receive12 = "Receive_12";

            ///// <summary>
            ///// 代收課目13金額
            ///// </summary>
            //public const string Receive13 = "Receive_13";

            ///// <summary>
            ///// 代收課目14金額
            ///// </summary>
            //public const string Receive14 = "Receive_14";

            ///// <summary>
            ///// 代收課目15金額
            ///// </summary>
            //public const string Receive15 = "Receive_15";

            ///// <summary>
            ///// 代收課目16金額
            ///// </summary>
            //public const string Receive16 = "Receive_16";
            #endregion

            #region 學籍資料
            /// <summary>
            /// 科系代碼
            /// </summary>
            public const string MajorId = "Major_Id";

            /// <summary>
            /// 年級代碼
            /// </summary>
            public const string StuGrade = "Stu_Grade";
            #endregion

            #region 繳費單資料相關
            /// <summary>
            /// 檔案上傳批號
            /// </summary>
            public const string UpNo = "Up_No";

            /// <summary>
			/// 繳費金額合計
			/// </summary>
			public const string ReceiveAmount = "Receive_Amount";

			/// <summary>
			/// ATM繳費金額
			/// </summary>
			public const string ReceiveATMAmount = "Receive_ATMAmount";

            /// <summary>
            /// 超商繳費金額
            /// </summary>
            public const string ReceiveSMAmount = "Receive_SMAmount";

            /// <summary>
            /// 銷帳編號
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// ATM銷帳編號
            /// </summary>
            public const string CancelATMNo = "Cancel_ATMNo";

            /// <summary>
            /// 超商銷帳編號
            /// </summary>
            public const string CancelSMNo = "Cancel_SMNo";
            #endregion

            #region [Old]
            ///// <summary>
            ///// EBank1繳費金額
            ///// </summary>
            //public const string ReceiveEB1Amount = "Receive_EB1Amount";

            ///// <summary>
            ///// EBank2繳費金額
            ///// </summary>
            //public const string ReceiveEB2Amount = "Receive_EB2Amount";

            ///// <summary>
            ///// 原就學貸款金額
            ///// </summary>
            //public const string LoanAmount = "Loan_Amount";

            ///// <summary>
            ///// 補單註記 (9=原公式 / 7=原公式但已銷帳 / 8=強迫更改金額 / 6=強迫更改但已銷帳)
            ///// </summary>
            //public const string ReissueFlag = "Reissue_Flag";

            ///// <summary>
            ///// 流水號
            ///// </summary>
            //public const string SeriorNo = "Serior_No";

            ///// <summary>
            ///// EBank1銷帳編號
            ///// </summary>
            //public const string CancelEB1No = "Cancel_EB1No";

            ///// <summary>
            ///// EBank2銷帳編號
            ///// </summary>
            //public const string CancelEB2No = "Cancel_EB2No";

            ///// <summary>
            ///// 郵局銷帳編號
            ///// </summary>
            //public const string CancelPONo = "Cancel_PONo";
            #endregion

            #region 銷帳(繳費)資料相關
            /// <summary>
            /// 銷帳註記 (1=連線 / 2=金額不符 / 3=檢碼不符 / 7=銷問題檔 / 8=人工銷帳 / 9=離線)
            /// </summary>
            public const string CancelFlag = "Cancel_Flag";

			/// <summary>
            /// 代收日期 (民國年日期7碼)
			/// </summary>
			public const string ReceiveDate = "Receive_Date";

			/// <summary>
			/// 代收時間
			/// </summary>
			public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 繳費方式 (參考管道代碼)
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 代收銀行/分行 (土銀 EAI 只給三碼)
            /// </summary>
            public const string ReceiveBankId = "Receivebank_Id";

			/// <summary>
            /// 入帳日期 (民國年日期7碼)
			/// </summary>
			public const string AccountDate = "Account_Date";

            /// <summary>
            /// 儲存消帳註記相關欄位
            /// </summary>
            public const string CancelFlagFields = "CancelFlagFields";
            #endregion

            #region [Old]
            ///// <summary>
            ///// 手續費
            ///// </summary>
            //public const string ProcessFee = "Process_Fee";

            ///// <summary>
            ///// Real_Receive 欄位名稱常數定義
            ///// </summary>
            //public const string RealReceive = "Real_Receive";

            ///// <summary>
            ///// Cancel_Receive 欄位名稱常數定義
            ///// </summary>
            //public const string CancelReceive = "Cancel_Receive";

            ///// <summary>
            ///// Fee_Receivable 欄位名稱常數定義
            ///// </summary>
            //public const string FeeReceivable = "Fee_Receivable";

            ///// <summary>
            ///// Fee_Payable 欄位名稱常數定義
            ///// </summary>
            //public const string FeePayable = "Fee_Payable";

            ///// <summary>
            ///// Postseq 欄位名稱常數定義
            ///// </summary>
            //public const string PostSeq = "Postseq";

            ///// <summary>
            ///// Atm_meno 欄位名稱常數定義
            ///// </summary>
            //public const string ATMMeno = "Atm_meno";

            ///// <summary>
            ///// 座號
            ///// </summary>
            //public const string StuHid = "stu_hid";

            ///// <summary>
            ///// e網通
            ///// </summary>
            //public const string EFlag = "e_flag";

            ///// <summary>
            ///// c_flag 欄位名稱常數定義
            ///// </summary>
            //public const string CFlag = "c_flag";

            ///// <summary>
            ///// 上傳就學貸款金額
            ///// </summary>
            //public const string Loan = "loan";

            ///// <summary>
            ///// 實際貸款金額
            ///// </summary>
            //public const string RealLoan = "real_loan";

            ///// <summary>
            ///// Cancel_Date 欄位名稱常數定義
            ///// </summary>
            //public const string CancelDate = "Cancel_Date";

            ///// <summary>
            ///// PrintCreate_Date 欄位名稱常數定義
            ///// </summary>
            //public const string PrintCreateDate = "PrintCreate_Date";

            ///// <summary>
            ///// Print_Date 欄位名稱常數定義
            ///// </summary>
            //public const string PrintDate = "Print_Date";

            ///// <summary>
            ///// exportReceiveData 欄位名稱常數定義
            ///// </summary>
            //public const string ExportReceiveData = "exportReceiveData";

            ///// <summary>
            ///// InvCreate_Date 欄位名稱常數定義
            ///// </summary>
            //public const string InvCreateDate = "InvCreate_Date";

            ///// <summary>
            ///// Inv_Date 欄位名稱常數定義
            ///// </summary>
            //public const string InvDate = "Inv_Date";

            ///// <summary>
            ///// MAPPING_TYPE 欄位名稱常數定義
            ///// </summary>
            //public const string MappingType = "MAPPING_TYPE";

            ///// <summary>
            ///// MAPPING_ID 欄位名稱常數定義
            ///// </summary>
            //public const string MappingId = "MAPPING_ID";

            ///// <summary>
            ///// 備註
            ///// </summary>
            //public const string Remark = "remark";
            #endregion

            #region 代收費用別資料相關
            /// <summary>
            /// 繳款期限 (民國年3碼+月2碼+日2碼)
            /// </summary>
            public const string PayDate = "Pay_Date";

            /// <summary>
            /// 開放列印日期 (格式：yyyyMMdd)
            /// </summary>
            public const string BillOpenDate = "Bill_Valid_Date";

            /// <summary>
            /// 關閉列印日期 (格式：yyyyMMdd)
            /// </summary>
            public const string BillCloseDate = "Bill_Close_Date";

            /// <summary>
            /// 代收費用別名稱
            /// </summary>
            public const string ReceiveName = "Receive_Name";
            #endregion

            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// UsersView 類別建構式
        /// </summary>
        public StudentView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 代收類別代碼
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

        private string _DepId = null;
        /// <summary>
        /// 部別代碼 (土銀不使用)
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
        /// 代收費用別代碼
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
        /// 學號
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
        /// 學號 欄位屬性
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
        #region 學生基本資料相關
        /// <summary>
        /// 姓名
        /// </summary>
        [FieldSpec(Field.Name, false, FieldTypeEnum.NVarChar, 60, true)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 生日 (民國年月日 7 碼)
        /// </summary>
        [FieldSpec(Field.Birthday, false, FieldTypeEnum.Char, 7, true)]
        public string Birthday
        {
            get;
            set;
        }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [FieldSpec(Field.IdNumber, false, FieldTypeEnum.Char, 12, true)]
        public string IdNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 電話
        /// </summary>
        [FieldSpec(Field.Tel, false, FieldTypeEnum.VarChar, 14, true)]
        public string Tel
        {
            get;
            set;
        }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        [FieldSpec(Field.ZipCode, false, FieldTypeEnum.VarChar, 5, true)]
        public string ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        [FieldSpec(Field.Address, false, FieldTypeEnum.VarChar, 100, true)]
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// EMail
        /// </summary>
        [FieldSpec(Field.Email, false, FieldTypeEnum.VarChar, 50, true)]
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 家長名稱
        /// </summary>
        [FieldSpec(Field.StuParent, false, FieldTypeEnum.NVarChar, 60, true)]
        public string StuParent
        {
            get;
            set;
        }
        #endregion

        #region [Old]
        ///// <summary>
        ///// Stu_Account 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.Account, false, FieldTypeEnum.VarChar, 21, true)]
        //public string Account
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 資料建立日期
        ///// </summary>
        //[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, true)]
        //public DateTime? CrtDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 資料最後修改日期
        ///// </summary>
        //[FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
        //public DateTime? MdyDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 接收順序
        ///// </summary>
        //[FieldSpec(Field.UpOrder, false, FieldTypeEnum.Char, 6, true)]
        //public string UpOrder
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 年級代碼
        ///// </summary>
        //[FieldSpec(Field.StuGrade, false, FieldTypeEnum.VarChar, 2, true)]
        //public string StuGrade
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 院別代碼
        ///// </summary>
        //[FieldSpec(Field.CollegeId, false, FieldTypeEnum.VarChar, 20, true)]
        //public string CollegeId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 班別代碼
        ///// </summary>
        //[FieldSpec(Field.ClassId, false, FieldTypeEnum.VarChar, 20, true)]
        //public string ClassId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 學分數
        ///// </summary>
        //[FieldSpec(Field.StuCredit, false, FieldTypeEnum.Decimal, true)]
        //public decimal? StuCredit
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 上課時數
        ///// </summary>
        //[FieldSpec(Field.StuHour, false, FieldTypeEnum.Decimal, true)]
        //public decimal? StuHour
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 減免代碼
        ///// </summary>
        //[FieldSpec(Field.ReduceId, false, FieldTypeEnum.VarChar, 20, true)]
        //public string ReduceId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 住宿代碼
        ///// </summary>
        //[FieldSpec(Field.DormId, false, FieldTypeEnum.VarChar, 20, true)]
        //public string DormId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 就貸代碼
        ///// </summary>
        //[FieldSpec(Field.LoanId, false, FieldTypeEnum.VarChar, 20, true)]
        //public string LoanId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代辦費明細項目
        ///// </summary>
        //[FieldSpec(Field.AgencyList, false, FieldTypeEnum.VarChar, 300, true)]
        //public string AgencyList
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 計算方式 (1=依收費標準計算 / 2=依輸入金額計算)
        ///// </summary>
        //[FieldSpec(Field.BillingType, false, FieldTypeEnum.Char, 1, true)]
        //public string BillingType
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 身份註記一代碼
        ///// </summary>
        //[FieldSpec(Field.IdentifyId01, false, FieldTypeEnum.VarChar, 20, true)]
        //public string IdentifyId01
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 身份註記二代碼
        ///// </summary>
        //[FieldSpec(Field.IdentifyId02, false, FieldTypeEnum.VarChar, 20, true)]
        //public string IdentifyId02
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 身份註記三代碼
        ///// </summary>
        //[FieldSpec(Field.IdentifyId03, false, FieldTypeEnum.VarChar, 20, true)]
        //public string IdentifyId03
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 身份註記四代碼
        ///// </summary>
        //[FieldSpec(Field.IdentifyId04, false, FieldTypeEnum.VarChar, 20, true)]
        //public string IdentifyId04
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 身份註記五代碼
        ///// </summary>
        //[FieldSpec(Field.IdentifyId05, false, FieldTypeEnum.VarChar, 20, true)]
        //public string IdentifyId05
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 身份註記六代碼
        ///// </summary>
        //[FieldSpec(Field.IdentifyId06, false, FieldTypeEnum.VarChar, 20, true)]
        //public string IdentifyId06
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目01金額
        ///// </summary>
        //[FieldSpec(Field.Receive01, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive01
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目02金額
        ///// </summary>
        //[FieldSpec(Field.Receive02, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive02
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目03金額
        ///// </summary>
        //[FieldSpec(Field.Receive03, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive03
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目04金額
        ///// </summary>
        //[FieldSpec(Field.Receive04, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive04
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目05金額
        ///// </summary>
        //[FieldSpec(Field.Receive05, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive05
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目06金額
        ///// </summary>
        //[FieldSpec(Field.Receive06, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive06
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目07金額
        ///// </summary>
        //[FieldSpec(Field.Receive07, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive07
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目08金額
        ///// </summary>
        //[FieldSpec(Field.Receive08, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive08
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目09金額
        ///// </summary>
        //[FieldSpec(Field.Receive09, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive09
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目10金額
        ///// </summary>
        //[FieldSpec(Field.Receive10, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive10
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目11金額
        ///// </summary>
        //[FieldSpec(Field.Receive11, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive11
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目12金額
        ///// </summary>
        //[FieldSpec(Field.Receive12, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive12
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目13金額
        ///// </summary>
        //[FieldSpec(Field.Receive13, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive13
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目14金額
        ///// </summary>
        //[FieldSpec(Field.Receive14, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive14
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目15金額
        ///// </summary>
        //[FieldSpec(Field.Receive15, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive15
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 代收課目16金額
        ///// </summary>
        //[FieldSpec(Field.Receive16, false, FieldTypeEnum.Decimal, true)]
        //public decimal? Receive16
        //{
        //    get;
        //    set;
        //}
        #endregion

        #region 學籍資料
        #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
        /// <summary>
        /// 科系代碼
        /// </summary>
        [FieldSpec(Field.MajorId, false, FieldTypeEnum.NVarChar, 40, true)]
        public string MajorId
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 年級代碼
        /// </summary>
        [FieldSpec(Field.StuGrade, false, FieldTypeEnum.VarChar, 2, true)]
        public string StuGrade
        {
            get;
            set;
        }
        #endregion

        #region 繳費單資料相關
        /// <summary>
        /// 檔案上傳批號
        /// </summary>
        [FieldSpec(Field.UpNo, false, FieldTypeEnum.VarChar, 4, true)]
        public string UpNo
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費金額合計
        /// </summary>
        [FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveAmount
        {
            get;
            set;
        }

        /// <summary>
        /// ATM繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveATMAmount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveATMAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 超商繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveSMAmount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveSMAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.Char, 16, true)]
        public string CancelNo
        {
            get;
            set;
        }

        /// <summary>
        /// ATM銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelATMNo, false, FieldTypeEnum.Char, 16, true)]
        public string CancelATMNo
        {
            get;
            set;
        }

        /// <summary>
        /// 超商銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelSMNo, false, FieldTypeEnum.Char, 16, true)]
        public string CancelSMNo
        {
            get;
            set;
        }
        #endregion

        #region [Old]
        ///// <summary>
        ///// EBank1繳費金額
        ///// </summary>
        //[FieldSpec(Field.ReceiveEB1Amount, false, FieldTypeEnum.Decimal, true)]
        //public decimal? ReceiveEB1Amount
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// EBank2繳費金額
        ///// </summary>
        //[FieldSpec(Field.ReceiveEB2Amount, false, FieldTypeEnum.Decimal, true)]
        //public decimal? ReceiveEB2Amount
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 原就學貸款金額
        ///// </summary>
        //[FieldSpec(Field.LoanAmount, false, FieldTypeEnum.Decimal, true)]
        //public decimal? LoanAmount
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 補單註記 (9=原公式 / 7=原公式但已銷帳 / 8=強迫更改金額 / 6=強迫更改但已銷帳)
        ///// </summary>
        //[FieldSpec(Field.ReissueFlag, false, FieldTypeEnum.Char, 1, true)]
        //public string ReissueFlag
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 流水號
        ///// </summary>
        //[FieldSpec(Field.SeriorNo, false, FieldTypeEnum.Char, 6, true)]
        //public string SeriorNo
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// EBank1銷帳編號
        ///// </summary>
        //[FieldSpec(Field.CancelEB1No, false, FieldTypeEnum.Char, 16, true)]
        //public string CancelEB1No
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// EBank2銷帳編號
        ///// </summary>
        //[FieldSpec(Field.CancelEB2No, false, FieldTypeEnum.Char, 16, true)]
        //public string CancelEB2No
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 郵局銷帳編號
        ///// </summary>
        //[FieldSpec(Field.CancelPONo, false, FieldTypeEnum.Char, 16, true)]
        //public string CancelPONo
        //{
        //    get;
        //    set;
        //}
        #endregion

        #region 銷帳(繳費)資料相關
        /// <summary>
        /// 銷帳註記 (1=連線 / 2=金額不符 / 3=檢碼不符 / 7=銷問題檔 / 8=人工銷帳 / 9=離線)
        /// </summary>
        [FieldSpec(Field.CancelFlag, false, FieldTypeEnum.Char, 1, true)]
        public string CancelFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 代收日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Char, 7, true)]
        public string ReceiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// 代收時間
        /// </summary>
        [FieldSpec(Field.ReceiveTime, false, FieldTypeEnum.Char, 6, true)]
        public string ReceiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 繳費方式 (參考管道代碼)
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 2, true)]
        public string ReceiveWay
        {
            get;
            set;
        }

        /// <summary>
        /// 代收銀行/分行 (土銀 EAI 只給三碼)
        /// </summary>
        [FieldSpec(Field.ReceiveBankId, false, FieldTypeEnum.Char, 7, true)]
        public string ReceiveBankId
        {
            get;
            set;
        }

        /// <summary>
        /// 代收日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Char, 7, true)]
        public string AccountDate
        {
            get;
            set;
        }

        /// <summary>
        /// UploadFlag 欄位屬性
        /// </summary>
        [FieldSpec(Field.CancelFlagFields, false, FieldTypeEnum.VarChar, 100, true)]
        public string CancelFlagFields
        {
            get;
            set;
        }
        #endregion

        #region [Old]
        ///// <summary>
        ///// 手續費
        ///// </summary>
        //[FieldSpec(Field.ProcessFee, false, FieldTypeEnum.Decimal, true)]
        //public decimal? ProcessFee
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Real_Receive 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.RealReceive, false, FieldTypeEnum.Decimal, true)]
        //public decimal? RealReceive
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Cancel_Receive 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.CancelReceive, false, FieldTypeEnum.Decimal, true)]
        //public decimal? CancelReceive
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Fee_Receivable 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.FeeReceivable, false, FieldTypeEnum.Decimal, true)]
        //public decimal? FeeReceivable
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Fee_Payable 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.FeePayable, false, FieldTypeEnum.Decimal, true)]
        //public decimal? FeePayable
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Postseq 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.PostSeq, false, FieldTypeEnum.Char, 10, true)]
        //public string PostSeq
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Atm_meno 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.ATMMeno, false, FieldTypeEnum.Char, 50, true)]
        //public string ATMMeno
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 座號
        ///// </summary>
        //[FieldSpec(Field.StuHid, false, FieldTypeEnum.Char, 10, true)]
        //public string StuHid
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// e網通
        ///// </summary>
        //[FieldSpec(Field.EFlag, false, FieldTypeEnum.Char, 1, true)]
        //public string EFlag
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// c_flag 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.CFlag, false, FieldTypeEnum.Char, 1, true)]
        //public string CFlag
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 上傳就學貸款金額
        ///// </summary>
        //[FieldSpec(Field.Loan, false, FieldTypeEnum.Decimal, false)]
        //public decimal Loan
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 實際貸款金額
        ///// </summary>
        //[FieldSpec(Field.RealLoan, false, FieldTypeEnum.Decimal, false)]
        //public decimal RealLoan
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Cancel_Date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.CancelDate, false, FieldTypeEnum.Char, 8, true)]
        //public string CancelDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// PrintCreate_Date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.PrintCreateDate, false, FieldTypeEnum.Char, 8, true)]
        //public string PrintCreateDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Print_Date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.PrintDate, false, FieldTypeEnum.Char, 8, true)]
        //public string PrintDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// exportReceiveData 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.ExportReceiveData, false, FieldTypeEnum.Char, 1, false)]
        //public string ExportReceiveData
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// InvCreate_Date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.InvCreateDate, false, FieldTypeEnum.Char, 8, true)]
        //public string InvCreateDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Inv_Date 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.InvDate, false, FieldTypeEnum.Char, 8, true)]
        //public string InvDate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// MAPPING_TYPE 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.MappingType, false, FieldTypeEnum.Char, 1, false)]
        //public string MappingType
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// MAPPING_ID 欄位屬性
        ///// </summary>
        //[FieldSpec(Field.MappingId, false, FieldTypeEnum.Char, 2, false)]
        //public string MappingId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 備註
        ///// </summary>
        //[FieldSpec(Field.Remark, false, FieldTypeEnum.VarCharMax, false)]
        //public string Remark
        //{
        //    get;
        //    set;
        //}
        #endregion

        #region 代收費用別資料相關
        /// <summary>
        /// 繳款期限 (民國年3碼+月2碼+日2碼)
        /// </summary>
        [FieldSpec(Field.PayDate, false, FieldTypeEnum.Char, 7, true)]
        public string PayDate
        {
            get;
            set;
        }

        /// <summary>
        /// 開放列印日期 (格式：yyyyMMdd)
        /// </summary>
        [FieldSpec(Field.BillOpenDate, false, FieldTypeEnum.Char, 8, true)]
        public string BillOpenDate
        {
            get;
            set;
        }

        /// <summary>
        /// 關閉列印日期 (格式：yyyyMMdd)
        /// </summary>
        [FieldSpec(Field.BillCloseDate, false, FieldTypeEnum.Char, 8, false)]
        public string BillCloseDate
        {
            get;
            set;
        }

        /// <summary>
        /// 代收費用別名稱
        /// </summary>
        [FieldSpec(Field.ReceiveName, false, FieldTypeEnum.NVarChar, 40, true)]
        public string ReceiveName
        {
            get;
            set;
        }
        #endregion

        #endregion
        #endregion
    }
}
