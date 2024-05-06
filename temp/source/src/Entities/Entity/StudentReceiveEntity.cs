/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:32:32
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
    /// 學生繳費單資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class StudentReceiveEntity : Entity
    {
        #region Const
        /// <summary>
        /// 備註數量 (21)
        /// </summary>
        public const int MemoCount = 21;

        #region [MDY:20160131] 最大可指定的資料序號
        /// <summary>
        /// 最大可指定的資料序號 (999)
        /// </summary>
        public const int MaxOldSeq = 999;
        #endregion
        #endregion

        public const string TABLE_NAME = "Student_Receive";

        #region Field Name Const Class
        /// <summary>
        /// StudentReceiveEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 商家代號
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
            /// 部別代碼 (土銀不使用此部別欄位，固定設為空字串)
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

            #region [MDY:20160131] 開放指定資料序號
            /// <summary>
            /// 同5KEY下同學號的資料序號 (不指定則預設為 0，指定時只允許1~999，超過則為轉置的舊資料序號) 欄位名稱常數定義
            /// </summary>
            public const string OldSeq = "Old_Seq";
            #endregion
            #endregion

            #region Data
            /// <summary>
            /// 上傳資料批號
            /// </summary>
            public const string UpNo = "Up_No";

            /// <summary>
            /// 上傳批資料序號 (批資料流水號)
            /// </summary>
            public const string UpOrder = "Up_Order";

            /// <summary>
            /// 年級代碼
            /// </summary>
            public const string StuGrade = "Stu_Grade";

            /// <summary>
            /// 院別代碼
            /// </summary>
            public const string CollegeId = "College_Id";

            /// <summary>
            /// 科系代碼
            /// </summary>
            public const string MajorId = "Major_Id";

            /// <summary>
            /// 班別代碼
            /// </summary>
            public const string ClassId = "Class_Id";

            /// <summary>
            /// 總學分數
            /// </summary>
            public const string StuCredit = "Stu_Credit";

            /// <summary>
            /// 上課時數
            /// </summary>
            public const string StuHour = "Stu_Hour";

            /// <summary>
            /// 減免代碼
            /// </summary>
            public const string ReduceId = "Reduce_Id";

            /// <summary>
            /// 住宿代碼
            /// </summary>
            public const string DormId = "Dorm_Id";

            /// <summary>
            /// 就貸代碼
            /// </summary>
            public const string LoanId = "Loan_Id";

            /// <summary>
            /// 代辦費明細項目 (土銀好像沒用到)
            /// </summary>
            public const string AgencyList = "Agency_List";

            /// <summary>
            /// 計算方式 (1=依收費標準計算 / 2=依輸入金額計算)
            /// </summary>
            public const string BillingType = "Billing_type";

            /// <summary>
            /// 身份註記01代碼
            /// </summary>
            public const string IdentifyId01 = "Identify_Id01";

            /// <summary>
            /// 身份註記02代碼
            /// </summary>
            public const string IdentifyId02 = "Identify_Id02";

            /// <summary>
            /// 身份註記03代碼
            /// </summary>
            public const string IdentifyId03 = "Identify_Id03";

            /// <summary>
            /// 身份註記04代碼
            /// </summary>
            public const string IdentifyId04 = "Identify_Id04";

            /// <summary>
            /// 身份註記05代碼
            /// </summary>
            public const string IdentifyId05 = "Identify_Id05";

            /// <summary>
            /// 身份註記06代碼
            /// </summary>
            public const string IdentifyId06 = "Identify_Id06";


            #region [MDY:2018xxxx] 收入科目金額 相關
            /// <summary>
            /// 收入科目01金額
            /// </summary>
            public const string Receive01 = "Receive_01";

            /// <summary>
            /// 收入科目02金額
            /// </summary>
            public const string Receive02 = "Receive_02";

            /// <summary>
            /// 收入科目03金額
            /// </summary>
            public const string Receive03 = "Receive_03";

            /// <summary>
            /// 收入科目04金額
            /// </summary>
            public const string Receive04 = "Receive_04";

            /// <summary>
            /// 收入科目05金額
            /// </summary>
            public const string Receive05 = "Receive_05";

            /// <summary>
            /// 收入科目06金額
            /// </summary>
            public const string Receive06 = "Receive_06";

            /// <summary>
            /// 收入科目07金額
            /// </summary>
            public const string Receive07 = "Receive_07";

            /// <summary>
            /// 收入科目08金額
            /// </summary>
            public const string Receive08 = "Receive_08";

            /// <summary>
            /// 收入科目09金額
            /// </summary>
            public const string Receive09 = "Receive_09";

            /// <summary>
            /// 收入科目10金額
            /// </summary>
            public const string Receive10 = "Receive_10";

            /// <summary>
            /// 收入科目11金額
            /// </summary>
            public const string Receive11 = "Receive_11";

            /// <summary>
            /// 收入科目12金額
            /// </summary>
            public const string Receive12 = "Receive_12";

            /// <summary>
            /// 收入科目13金額
            /// </summary>
            public const string Receive13 = "Receive_13";

            /// <summary>
            /// 收入科目14金額
            /// </summary>
            public const string Receive14 = "Receive_14";

            /// <summary>
            /// 收入科目15金額
            /// </summary>
            public const string Receive15 = "Receive_15";

            /// <summary>
            /// 收入科目16金額
            /// </summary>
            public const string Receive16 = "Receive_16";

            /// <summary>
            /// 收入科目17金額
            /// </summary>
            public const string Receive17 = "Receive_17";

            /// <summary>
            /// 收入科目18金額
            /// </summary>
            public const string Receive18 = "Receive_18";

            /// <summary>
            /// 收入科目19金額
            /// </summary>
            public const string Receive19 = "Receive_19";

            /// <summary>
            /// 收入科目20金額
            /// </summary>
            public const string Receive20 = "Receive_20";

            /// <summary>
            /// 收入科目21金額
            /// </summary>
            public const string Receive21 = "Receive_21";

            /// <summary>
            /// 收入科目22金額
            /// </summary>
            public const string Receive22 = "Receive_22";

            /// <summary>
            /// 收入科目23金額
            /// </summary>
            public const string Receive23 = "Receive_23";

            /// <summary>
            /// 收入科目24金額
            /// </summary>
            public const string Receive24 = "Receive_24";

            /// <summary>
            /// 收入科目25金額
            /// </summary>
            public const string Receive25 = "Receive_25";

            /// <summary>
            /// 收入科目26金額
            /// </summary>
            public const string Receive26 = "Receive_26";

            /// <summary>
            /// 收入科目27金額
            /// </summary>
            public const string Receive27 = "Receive_27";

            /// <summary>
            /// 收入科目28金額
            /// </summary>
            public const string Receive28 = "Receive_28";

            /// <summary>
            /// 收入科目29金額
            /// </summary>
            public const string Receive29 = "Receive_29";

            /// <summary>
            /// 收入科目30金額
            /// </summary>
            public const string Receive30 = "Receive_30";

            /// <summary>
            /// 收入科目31金額
            /// </summary>
            public const string Receive31 = "Receive_31";

            /// <summary>
            /// 收入科目32金額
            /// </summary>
            public const string Receive32 = "Receive_32";

            /// <summary>
            /// 收入科目33金額
            /// </summary>
            public const string Receive33 = "Receive_33";

            /// <summary>
            /// 收入科目34金額
            /// </summary>
            public const string Receive34 = "Receive_34";

            /// <summary>
            /// 收入科目35金額
            /// </summary>
            public const string Receive35 = "Receive_35";

            /// <summary>
            /// 收入科目36金額
            /// </summary>
            public const string Receive36 = "Receive_36";

            /// <summary>
            /// 收入科目37金額
            /// </summary>
            public const string Receive37 = "Receive_37";

            /// <summary>
            /// 收入科目38金額
            /// </summary>
            public const string Receive38 = "Receive_38";

            /// <summary>
            /// 收入科目39金額
            /// </summary>
            public const string Receive39 = "Receive_39";

            /// <summary>
            /// 收入科目40金額
            /// </summary>
            public const string Receive40 = "Receive_40";
            #endregion


            /// <summary>
            /// 繳費金額合計
            /// </summary>
            public const string ReceiveAmount = "Receive_Amount";

            /// <summary>
            /// ATM繳費金額
            /// </summary>
            public const string ReceiveAtmamount = "Receive_ATMAmount";

            /// <summary>
            /// EBank1繳費金額 (土銀沒使用)
            /// </summary>
            public const string ReceiveEb1amount = "Receive_EB1Amount";

            /// <summary>
            /// EBank2繳費金額 (土銀沒使用)
            /// </summary>
            public const string ReceiveEb2amount = "Receive_EB2Amount";

            /// <summary>
            /// 超商繳費金額
            /// </summary>
            public const string ReceiveSmamount = "Receive_SMAmount";

            /// <summary>
            /// 可貸金額 (BUA或頁面上輸入的預估就貸金額) (原就學貸款金額)
            /// </summary>
            public const string LoanAmount = "Loan_Amount";

            /// <summary>
            /// 補單註記 (9=原公式 / 7=原公式但已銷帳 / 8=強迫更改金額 / 6=強迫更改但已銷帳)
            /// </summary>
            public const string ReissueFlag = "Reissue_Flag";

            /// <summary>
            /// 銷帳編號中的流水號
            /// </summary>
            public const string SeriorNo = "Serior_No";

            /// <summary>
            /// 銷帳編號 (虛擬帳號)
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// ATM銷帳編號
            /// </summary>
            public const string CancelAtmno = "Cancel_ATMNo";

            /// <summary>
            /// EBank1銷帳編號 (土銀沒使用)
            /// </summary>
            public const string CancelEb1no = "Cancel_EB1No";

            /// <summary>
            /// EBank2銷帳編號 (土銀沒使用)
            /// </summary>
            public const string CancelEb2no = "Cancel_EB2No";

            /// <summary>
            /// 郵局銷帳編號 (土銀沒使用。土銀學雜費的郵局資料不歸系統管)
            /// </summary>
            public const string CancelPono = "Cancel_PONo";

            /// <summary>
            /// 超商銷帳編號
            /// </summary>
            public const string CancelSmno = "Cancel_SMNo";

            /// <summary>
            /// 銷帳註記 (1=連線 / 2=金額不符 / 3=檢碼不符 / 7=銷問題檔 / 8=人工銷帳 / 9=離線) (參考 CancelFlagCodeTexts) 欄位名稱常數定義
            /// </summary>
            public const string CancelFlag = "Cancel_Flag";

            /// <summary>
            /// 代收銀行/分行
            /// </summary>
            public const string ReceivebankId = "Receivebank_Id";

            /// <summary>
            /// 代收日期 (民國年日期7碼)
            /// </summary>
            public const string ReceiveDate = "Receive_Date";

            /// <summary>
            /// 代收時間 (HHmmdd / HHmm)
            /// </summary>
            public const string ReceiveTime = "Receive_Time";

            /// <summary>
            /// 入帳日期 (民國年日期7碼)
            /// </summary>
            public const string AccountDate = "Account_Date";

            /// <summary>
            /// 繳費方式 (參考管道代碼)
            /// </summary>
            public const string ReceiveWay = "Receive_Way";

            /// <summary>
            /// 手續費 (限 09-支付寶、NC-國際信用卡)
            /// </summary>
            public const string ProcessFee = "Process_Fee";

            /// <summary>
            /// Real_Receive (土銀沒用到)
            /// </summary>
            public const string RealReceive = "Real_Receive";

            /// <summary>
            /// Cancel_Receive (土銀沒用到)
            /// </summary>
            public const string CancelReceive = "Cancel_Receive";

            /// <summary>
            /// Fee_Receivable (土銀沒用到)
            /// </summary>
            public const string FeeReceivable = "Fee_Receivable";

            /// <summary>
            /// Fee_Payable (土銀用來紀錄減免金額)
            /// </summary>
            public const string FeePayable = "Fee_Payable";

            /// <summary>
            /// Postseq (土銀沒用到)
            /// </summary>
            public const string Postseq = "Postseq";

            /// <summary>
            /// Atm_meno (土銀沒用到)
            /// </summary>
            public const string AtmMeno = "Atm_meno";

            /// <summary>
            /// 座號 
            /// </summary>
            public const string StuHid = "stu_hid";

            /// <summary>
            /// e網通 (土銀沒用到)
            /// </summary>
            public const string EFlag = "e_flag";

            /// <summary>
            /// c_flag (土銀用來記錄是否已送給CTCB)
            /// </summary>
            public const string CFlag = "c_flag";

            /// <summary>
            /// 建立日期時間
            /// </summary>
            public const string CreateDate = "create_date";

            /// <summary>
            /// 最後修改日期時間
            /// </summary>
            public const string UpdateDate = "update_date";

            /// <summary>
            /// 上傳就學貸款金額 (BUD上傳的就貸金額)
            /// </summary>
            public const string Loan = "loan";

            /// <summary>
            /// 實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)
            /// </summary>
            public const string RealLoan = "real_loan";

            /// <summary>
            /// 銷帳日期 (YYYYMMDD)
            /// </summary>
            public const string CancelDate = "Cancel_Date";

            /// <summary>
            /// PrintCreate_Date (土銀沒用到)
            /// </summary>
            public const string PrintcreateDate = "PrintCreate_Date";

            /// <summary>
            /// Print_Date (土銀沒用到)
            /// </summary>
            public const string PrintDate = "Print_Date";

            /// <summary>
            /// exportReceiveData (土銀用來存放是否送過 D38 Y=是)
            /// </summary>
            public const string Exportreceivedata = "exportReceiveData";

            /// <summary>
            /// InvCreate_Date (土銀沒用到)
            /// </summary>
            public const string InvcreateDate = "InvCreate_Date";

            /// <summary>
            /// Inv_Date (土銀沒用到)
            /// </summary>
            public const string InvDate = "Inv_Date";

            /// <summary>
            /// 匯入此資料的對照檔類型 (Excel:2, Txt:1)
            /// </summary>
            public const string MappingType = "MAPPING_TYPE";

            /// <summary>
            /// 匯入此資料的對照檔代碼
            /// </summary>
            public const string MappingId = "MAPPING_ID";

            /// <summary>
            /// remark (土銀沒用到)
            /// </summary>
            public const string Remark = "remark";

            /// <summary>
            /// 扣款轉帳銀行代碼
            /// </summary>
            public const string DeductBankid = "Deduct_BankId";

            /// <summary>
            /// 扣款轉帳銀行帳號
            /// </summary>
            public const string DeductAccountno = "Deduct_AccountNo";

            /// <summary>
            /// 扣款轉帳銀行帳號戶名
            /// </summary>
            public const string DeductAccountname = "Deduct_AccountName";

            /// <summary>
            /// 扣款轉帳銀行帳戶ＩＤ
            /// </summary>
            public const string DeductAccountid = "Deduct_AccountId";

            #region 上傳資料註記 相關
            /// <summary>
            /// 上傳資料註記 (1=含流水號; 2=含銷帳編號; 3=含流水號與銷帳編號; 4=含總金額; 5=含流水號與總金額; 6=含銷帳編號與總金額; 7=含流水號、銷帳編號與總金額)
            /// </summary>
            public const string Uploadflag = "UploadFlag";
            #endregion

            /// <summary>
            /// 儲存消帳註記相關欄位值
            /// </summary>
            public const string CancelFlagFields = "CancelFlagFields";
            #endregion

            #region 土銀專用部別代碼
            /// <summary>
            /// 部別代號 (土銀專用)
            /// </summary>
            public const string DeptId = "Dept_Id";
            #endregion

            #region [MDY:2018xxxx] 備註 相關
            #region 備註
            /// <summary>
            /// 備註01
            /// </summary>
            public const string Memo01 = "Memo01";

            /// <summary>
            /// 備註02
            /// </summary>
            public const string Memo02 = "Memo02";

            /// <summary>
            /// 備註03
            /// </summary>
            public const string Memo03 = "Memo03";

            /// <summary>
            /// 備註04
            /// </summary>
            public const string Memo04 = "Memo04";

            /// <summary>
            /// 備註05
            /// </summary>
            public const string Memo05 = "Memo05";

            /// <summary>
            /// 備註06
            /// </summary>
            public const string Memo06 = "Memo06";

            /// <summary>
            /// 備註07
            /// </summary>
            public const string Memo07 = "Memo07";

            /// <summary>
            /// 備註08
            /// </summary>
            public const string Memo08 = "Memo08";

            /// <summary>
            /// 備註09
            /// </summary>
            public const string Memo09 = "Memo09";

            /// <summary>
            /// 備註10
            /// </summary>
            public const string Memo10 = "Memo10";
            #endregion

            #region 土銀增加備註數量
            /// <summary>
            /// 備註11
            /// </summary>
            public const string Memo11 = "Memo11";

            /// <summary>
            /// 備註12
            /// </summary>
            public const string Memo12 = "Memo12";

            /// <summary>
            /// 備註13
            /// </summary>
            public const string Memo13 = "Memo13";

            /// <summary>
            /// 備註14
            /// </summary>
            public const string Memo14 = "Memo14";

            /// <summary>
            /// 備註15
            /// </summary>
            public const string Memo15 = "Memo15";

            /// <summary>
            /// 備註16
            /// </summary>
            public const string Memo16 = "Memo16";

            /// <summary>
            /// 備註17
            /// </summary>
            public const string Memo17 = "Memo17";

            /// <summary>
            /// 備註18
            /// </summary>
            public const string Memo18 = "Memo18";

            /// <summary>
            /// 備註19
            /// </summary>
            public const string Memo19 = "Memo19";

            /// <summary>
            /// 備註20
            /// </summary>
            public const string Memo20 = "Memo20";

            /// <summary>
            /// 備註21
            /// </summary>
            public const string Memo21 = "Memo21";
            #endregion
            #endregion

            #region [MDY:20160131] 增加繳款期限
            /// <summary>
            /// 繳款期限 (格式：民國年7碼)
            /// </summary>
            public const string PayDueDate = "Pay_Due_Date";
            #endregion

            #region [MDY:20160305] 連動製單服務 相關欄位
            /// <summary>
            /// 學校端惟一序號 (參加連動製單服務一定要有這個值)
            /// </summary>
            public const string ServiceSeqNo = "Service_Seq_No";

            /// <summary>
            /// 學校端操作人員
            /// </summary>
            public const string ServiceSchUser = "Service_Sch_User";

            /// <summary>
            /// 學校端操作日期 (格式：yyyyMMdd)
            /// </summary>
            public const string ServiceSchMDate = "Service_Sch_MDate";

            /// <summary>
            /// 學校端操作時間 (格式：HHmmss)
            /// </summary>
            public const string ServiceSchMTime = "Service_Sch_MTime";
            #endregion

            #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
            /// <summary>
            /// 是否啟用國際信用卡繳費旗標
            /// </summary>
            public const string NCCardFlag = "NCCardFlag";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// StudentReceiveEntity 類別建構式
        /// </summary>
        public StudentReceiveEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ReceiveType = null;
        /// <summary>
        /// 商家代號
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

        private string _DepId = String.Empty;
        /// <summary>
        /// 部別代碼 (土銀不使用此部別欄位，固定設為空字串)
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

        #region [MDY:20160131] 開放指定資料序號
        private int _OldSeq = 0;
        /// <summary>
        /// 同5KEY下同學號的資料序號 (不指定則預設為 0，指定時只允許1~999，超過則為轉置的舊資料序號)
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
        #endregion

        #region Data
        private string _UpNo = null;
        /// <summary>
        /// 上傳資料批號
        /// </summary>
        [FieldSpec(Field.UpNo, false, FieldTypeEnum.VarChar, 4, true)]
        public string UpNo
        {
            get
            {
                return _UpNo;
            }
            set
            {
                _UpNo = value == null ? null : value.Trim();
            }
        }

        private string _UpOrder = null;
        /// <summary>
        /// 上傳批資料序號 (批資料流水號)
        /// </summary>
        [FieldSpec(Field.UpOrder, false, FieldTypeEnum.Char, 6, true)]
        public string UpOrder
        {
            get
            {
                return _UpOrder;
            }
            set
            {
                _UpOrder = value == null ? null : value.Trim();
            }
        }

        private string _StuGrade = null;
        /// <summary>
        /// 年級代碼
        /// </summary>
        [FieldSpec(Field.StuGrade, false, FieldTypeEnum.VarChar, 2, true)]
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

        private string _CollegeId = null;
        /// <summary>
        /// 院別代碼
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

        #region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
        private string _MajorId = null;
        /// <summary>
        /// 科系代碼
        /// </summary>
        [FieldSpec(Field.MajorId, false, FieldTypeEnum.NVarChar, 40, true)]
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
        #endregion

        private string _ClassId = null;
        /// <summary>
        /// 班別代碼
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

        /// <summary>
        /// 總學分數
        /// </summary>
        [FieldSpec(Field.StuCredit, false, FieldTypeEnum.Decimal, true)]
        public decimal? StuCredit
        {
            get;
            set;
        }

        /// <summary>
        /// 上課時數
        /// </summary>
        [FieldSpec(Field.StuHour, false, FieldTypeEnum.Decimal, true)]
        public decimal? StuHour
        {
            get;
            set;
        }

        private string _ReduceId = null;
        /// <summary>
        /// 減免代碼
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

        private string _DormId = null;
        /// <summary>
        /// 住宿代碼
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

        private string _LoanId = null;
        /// <summary>
        /// 就貸代碼
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

        /// <summary>
        /// 代辦費明細項目 (土銀好像沒用到)
        /// </summary>
        [FieldSpec(Field.AgencyList, false, FieldTypeEnum.VarChar, 300, true)]
        public string AgencyList
        {
            get;
            set;
        }

        private string _BillingType = null;
        /// <summary>
        /// 計算方式 (1=依收費標準計算 / 2=依輸入金額計算)
        /// </summary>
        [FieldSpec(Field.BillingType, false, FieldTypeEnum.Char, 1, true)]
        public string BillingType
        {
            get
            {
                return _BillingType;
            }
            set
            {
                _BillingType = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId01 = null;
        /// <summary>
        /// 身份註記01代碼
        /// </summary>
        [FieldSpec(Field.IdentifyId01, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId01
        {
            get
            {
                return _IdentifyId01;
            }
            set
            {
                _IdentifyId01 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId02 = null;
        /// <summary>
        /// 身份註記02代碼
        /// </summary>
        [FieldSpec(Field.IdentifyId02, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId02
        {
            get
            {
                return _IdentifyId02;
            }
            set
            {
                _IdentifyId02 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId031 = null;
        /// <summary>
        /// 身份註記03代碼
        /// </summary>
        [FieldSpec(Field.IdentifyId03, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId03
        {
            get
            {
                return _IdentifyId031;
            }
            set
            {
                _IdentifyId031 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId04 = null;
        /// <summary>
        /// 身份註記04代碼
        /// </summary>
        [FieldSpec(Field.IdentifyId04, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId04
        {
            get
            {
                return _IdentifyId04;
            }
            set
            {
                _IdentifyId04 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId05 = null;
        /// <summary>
        /// 身份註記05代碼
        /// </summary>
        [FieldSpec(Field.IdentifyId05, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId05
        {
            get
            {
                return _IdentifyId05;
            }
            set
            {
                _IdentifyId05 = value == null ? null : value.Trim();
            }
        }

        private string _IdentifyId06 = null;
        /// <summary>
        /// 身份註記06代碼
        /// </summary>
        [FieldSpec(Field.IdentifyId06, false, FieldTypeEnum.VarChar, 20, true)]
        public string IdentifyId06
        {
            get
            {
                return _IdentifyId06;
            }
            set
            {
                _IdentifyId06 = value == null ? null : value.Trim();
            }
        }


        #region [MDY:2018xxxx] 收入科目金額 相關
        #region Member
        /// <summary>
        /// 儲存所有收入科目金額 (避免陣列索引與項目編號的轉換，陣列開 41 個元素 index 0 不使用，項目編號等於索引值)
        /// </summary>
        private decimal?[] _AllReceiveItemAmounts = new decimal?[41];
        #endregion

        /// <summary>
        /// 收入科目01金額
        /// </summary>
        [FieldSpec(Field.Receive01, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive01
        {
            get
            {
                return _AllReceiveItemAmounts[01];
            }
            set
            {
                _AllReceiveItemAmounts[01] = value;
            }
        }

        /// <summary>
        /// 收入科目02金額
        /// </summary>
        [FieldSpec(Field.Receive02, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive02
        {
            get
            {
                return _AllReceiveItemAmounts[02];
            }
            set
            {
                _AllReceiveItemAmounts[02] = value;
            }
        }

        /// <summary>
        /// 收入科目03金額
        /// </summary>
        [FieldSpec(Field.Receive03, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive03
        {
            get
            {
                return _AllReceiveItemAmounts[03];
            }
            set
            {
                _AllReceiveItemAmounts[03] = value;
            }
        }

        /// <summary>
        /// 收入科目04金額
        /// </summary>
        [FieldSpec(Field.Receive04, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive04
        {
            get
            {
                return _AllReceiveItemAmounts[04];
            }
            set
            {
                _AllReceiveItemAmounts[04] = value;
            }
        }

        /// <summary>
        /// 收入科目05金額
        /// </summary>
        [FieldSpec(Field.Receive05, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive05
        {
            get
            {
                return _AllReceiveItemAmounts[05];
            }
            set
            {
                _AllReceiveItemAmounts[05] = value;
            }
        }

        /// <summary>
        /// 收入科目06金額
        /// </summary>
        [FieldSpec(Field.Receive06, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive06
        {
            get
            {
                return _AllReceiveItemAmounts[06];
            }
            set
            {
                _AllReceiveItemAmounts[06] = value;
            }
        }

        /// <summary>
        /// 收入科目07金額
        /// </summary>
        [FieldSpec(Field.Receive07, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive07
        {
            get
            {
                return _AllReceiveItemAmounts[07];
            }
            set
            {
                _AllReceiveItemAmounts[07] = value;
            }
        }

        /// <summary>
        /// 收入科目08金額
        /// </summary>
        [FieldSpec(Field.Receive08, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive08
        {
            get
            {
                return _AllReceiveItemAmounts[08];
            }
            set
            {
                _AllReceiveItemAmounts[08] = value;
            }
        }

        /// <summary>
        /// 收入科目09金額
        /// </summary>
        [FieldSpec(Field.Receive09, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive09
        {
            get
            {
                return _AllReceiveItemAmounts[09];
            }
            set
            {
                _AllReceiveItemAmounts[09] = value;
            }
        }

        /// <summary>
        /// 收入科目10金額
        /// </summary>
        [FieldSpec(Field.Receive10, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive10
        {
            get
            {
                return _AllReceiveItemAmounts[10];
            }
            set
            {
                _AllReceiveItemAmounts[10] = value;
            }
        }

        /// <summary>
        /// 收入科目11金額
        /// </summary>
        [FieldSpec(Field.Receive11, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive11
        {
            get
            {
                return _AllReceiveItemAmounts[11];
            }
            set
            {
                _AllReceiveItemAmounts[11] = value;
            }
        }

        /// <summary>
        /// 收入科目12金額
        /// </summary>
        [FieldSpec(Field.Receive12, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive12
        {
            get
            {
                return _AllReceiveItemAmounts[12];
            }
            set
            {
                _AllReceiveItemAmounts[12] = value;
            }
        }

        /// <summary>
        /// 收入科目13金額
        /// </summary>
        [FieldSpec(Field.Receive13, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive13
        {
            get
            {
                return _AllReceiveItemAmounts[13];
            }
            set
            {
                _AllReceiveItemAmounts[13] = value;
            }
        }

        /// <summary>
        /// 收入科目14金額
        /// </summary>
        [FieldSpec(Field.Receive14, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive14
        {
            get
            {
                return _AllReceiveItemAmounts[14];
            }
            set
            {
                _AllReceiveItemAmounts[14] = value;
            }
        }

        /// <summary>
        /// 收入科目15金額
        /// </summary>
        [FieldSpec(Field.Receive15, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive15
        {
            get
            {
                return _AllReceiveItemAmounts[15];
            }
            set
            {
                _AllReceiveItemAmounts[15] = value;
            }
        }

        /// <summary>
        /// 收入科目16金額
        /// </summary>
        [FieldSpec(Field.Receive16, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive16
        {
            get
            {
                return _AllReceiveItemAmounts[16];
            }
            set
            {
                _AllReceiveItemAmounts[16] = value;
            }
        }

        /// <summary>
        /// 收入科目17金額
        /// </summary>
        [FieldSpec(Field.Receive17, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive17
        {
            get
            {
                return _AllReceiveItemAmounts[17];
            }
            set
            {
                _AllReceiveItemAmounts[17] = value;
            }
        }

        /// <summary>
        /// 收入科目18金額
        /// </summary>
        [FieldSpec(Field.Receive18, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive18
        {
            get
            {
                return _AllReceiveItemAmounts[18];
            }
            set
            {
                _AllReceiveItemAmounts[18] = value;
            }
        }

        /// <summary>
        /// 收入科目19金額
        /// </summary>
        [FieldSpec(Field.Receive19, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive19
        {
            get
            {
                return _AllReceiveItemAmounts[19];
            }
            set
            {
                _AllReceiveItemAmounts[19] = value;
            }
        }

        /// <summary>
        /// 收入科目20金額
        /// </summary>
        [FieldSpec(Field.Receive20, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive20
        {
            get
            {
                return _AllReceiveItemAmounts[20];
            }
            set
            {
                _AllReceiveItemAmounts[20] = value;
            }
        }

        /// <summary>
        /// 收入科目21金額
        /// </summary>
        [FieldSpec(Field.Receive21, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive21
        {
            get
            {
                return _AllReceiveItemAmounts[21];
            }
            set
            {
                _AllReceiveItemAmounts[21] = value;
            }
        }

        /// <summary>
        /// 收入科目22金額
        /// </summary>
        [FieldSpec(Field.Receive22, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive22
        {
            get
            {
                return _AllReceiveItemAmounts[22];
            }
            set
            {
                _AllReceiveItemAmounts[22] = value;
            }
        }

        /// <summary>
        /// 收入科目23金額
        /// </summary>
        [FieldSpec(Field.Receive23, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive23
        {
            get
            {
                return _AllReceiveItemAmounts[23];
            }
            set
            {
                _AllReceiveItemAmounts[23] = value;
            }
        }

        /// <summary>
        /// 收入科目24金額
        /// </summary>
        [FieldSpec(Field.Receive24, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive24
        {
            get
            {
                return _AllReceiveItemAmounts[24];
            }
            set
            {
                _AllReceiveItemAmounts[24] = value;
            }
        }

        /// <summary>
        /// 收入科目25金額
        /// </summary>
        [FieldSpec(Field.Receive25, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive25
        {
            get
            {
                return _AllReceiveItemAmounts[25];
            }
            set
            {
                _AllReceiveItemAmounts[25] = value;
            }
        }

        /// <summary>
        /// 收入科目26金額
        /// </summary>
        [FieldSpec(Field.Receive26, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive26
        {
            get
            {
                return _AllReceiveItemAmounts[26];
            }
            set
            {
                _AllReceiveItemAmounts[26] = value;
            }
        }

        /// <summary>
        /// 收入科目27金額
        /// </summary>
        [FieldSpec(Field.Receive27, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive27
        {
            get
            {
                return _AllReceiveItemAmounts[27];
            }
            set
            {
                _AllReceiveItemAmounts[27] = value;
            }
        }

        /// <summary>
        /// 收入科目28金額
        /// </summary>
        [FieldSpec(Field.Receive28, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive28
        {
            get
            {
                return _AllReceiveItemAmounts[28];
            }
            set
            {
                _AllReceiveItemAmounts[28] = value;
            }
        }

        /// <summary>
        /// 收入科目29金額
        /// </summary>
        [FieldSpec(Field.Receive29, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive29
        {
            get
            {
                return _AllReceiveItemAmounts[29];
            }
            set
            {
                _AllReceiveItemAmounts[29] = value;
            }
        }

        /// <summary>
        /// 收入科目30金額
        /// </summary>
        [FieldSpec(Field.Receive30, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive30
        {
            get
            {
                return _AllReceiveItemAmounts[30];
            }
            set
            {
                _AllReceiveItemAmounts[30] = value;
            }
        }

        /// <summary>
        /// 收入科目31金額
        /// </summary>
        [FieldSpec(Field.Receive31, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive31
        {
            get
            {
                return _AllReceiveItemAmounts[31];
            }
            set
            {
                _AllReceiveItemAmounts[31] = value;
            }
        }

        /// <summary>
        /// 收入科目32金額
        /// </summary>
        [FieldSpec(Field.Receive32, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive32
        {
            get
            {
                return _AllReceiveItemAmounts[32];
            }
            set
            {
                _AllReceiveItemAmounts[32] = value;
            }
        }

        /// <summary>
        /// 收入科目33金額
        /// </summary>
        [FieldSpec(Field.Receive33, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive33
        {
            get
            {
                return _AllReceiveItemAmounts[33];
            }
            set
            {
                _AllReceiveItemAmounts[33] = value;
            }
        }

        /// <summary>
        /// 收入科目34金額
        /// </summary>
        [FieldSpec(Field.Receive34, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive34
        {
            get
            {
                return _AllReceiveItemAmounts[34];
            }
            set
            {
                _AllReceiveItemAmounts[34] = value;
            }
        }

        /// <summary>
        /// 收入科目35金額
        /// </summary>
        [FieldSpec(Field.Receive35, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive35
        {
            get
            {
                return _AllReceiveItemAmounts[35];
            }
            set
            {
                _AllReceiveItemAmounts[35] = value;
            }
        }

        /// <summary>
        /// 收入科目36金額
        /// </summary>
        [FieldSpec(Field.Receive36, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive36
        {
            get
            {
                return _AllReceiveItemAmounts[36];
            }
            set
            {
                _AllReceiveItemAmounts[36] = value;
            }
        }

        /// <summary>
        /// 收入科目37金額
        /// </summary>
        [FieldSpec(Field.Receive37, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive37
        {
            get
            {
                return _AllReceiveItemAmounts[37];
            }
            set
            {
                _AllReceiveItemAmounts[37] = value;
            }
        }

        /// <summary>
        /// 收入科目38金額
        /// </summary>
        [FieldSpec(Field.Receive38, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive38
        {
            get
            {
                return _AllReceiveItemAmounts[38];
            }
            set
            {
                _AllReceiveItemAmounts[38] = value;
            }
        }

        /// <summary>
        /// 收入科目39金額
        /// </summary>
        [FieldSpec(Field.Receive39, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive39
        {
            get
            {
                return _AllReceiveItemAmounts[39];
            }
            set
            {
                _AllReceiveItemAmounts[39] = value;
            }
        }

        /// <summary>
        /// 收入科目40金額
        /// </summary>
        [FieldSpec(Field.Receive40, false, FieldTypeEnum.Decimal, true)]
        public decimal? Receive40
        {
            get
            {
                return _AllReceiveItemAmounts[40];
            }
            set
            {
                _AllReceiveItemAmounts[40] = value;
            }
        }
        #endregion


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
        /// ATM繳費金額 欄位屬性
        /// </summary>
        [FieldSpec(Field.ReceiveAtmamount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveAtmamount
        {
            get;
            set;
        }

        /// <summary>
        /// EBank1繳費金額 (土銀沒使用)
        /// </summary>
        [FieldSpec(Field.ReceiveEb1amount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveEb1amount
        {
            get;
            set;
        }

        /// <summary>
        /// EBank2繳費金額 (土銀沒使用)
        /// </summary>
        [FieldSpec(Field.ReceiveEb2amount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveEb2amount
        {
            get;
            set;
        }

        /// <summary>
        /// 超商繳費金額
        /// </summary>
        [FieldSpec(Field.ReceiveSmamount, false, FieldTypeEnum.Decimal, true)]
        public decimal? ReceiveSmamount
        {
            get;
            set;
        }

        /// <summary>
        /// 可貸金額 (BUA或頁面上輸入的預估就貸金額) (原就學貸款金額)
        /// </summary>
        [FieldSpec(Field.LoanAmount, false, FieldTypeEnum.Decimal, true)]
        public decimal? LoanAmount
        {
            get;
            set;
        }

        private string _ReissueFlag = null;
        /// <summary>
        /// 補單註記 (9=原公式 / 7=原公式但已銷帳 / 8=強迫更改金額 / 6=強迫更改但已銷帳)
        /// </summary>
        [FieldSpec(Field.ReissueFlag, false, FieldTypeEnum.Char, 1, true)]
        public string ReissueFlag
        {
            get
            {
                return _ReissueFlag;
            }
            set
            {
                _ReissueFlag = value == null ? null : value.Trim();
            }
        }

        private string _SeriorNo = null;
        /// <summary>
        /// 銷帳編號中的流水號
        /// </summary>
        [FieldSpec(Field.SeriorNo, false, FieldTypeEnum.VarChar, 11, true)]
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
        /// 銷帳編號 (虛擬帳號)
        /// </summary>
        [FieldSpec(Field.CancelNo, false, FieldTypeEnum.Char, 16, true)]
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

        private string _CancelAtmno = null;
        /// <summary>
        /// ATM銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelAtmno, false, FieldTypeEnum.Char, 16, true)]
        public string CancelAtmno
        {
            get
            {
                return _CancelAtmno;
            }
            set
            {
                _CancelAtmno = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// EBank1銷帳編號 (土銀沒使用)
        /// </summary>
        [FieldSpec(Field.CancelEb1no, false, FieldTypeEnum.Char, 16, true)]
        public string CancelEb1no
        {
            get;
            set;
        }

        /// <summary>
        /// EBank2銷帳編號 (土銀沒使用)
        /// </summary>
        [FieldSpec(Field.CancelEb2no, false, FieldTypeEnum.Char, 16, true)]
        public string CancelEb2no
        {
            get;
            set;
        }

        private string _CancelPono = null;
        /// <summary>
        /// 郵局銷帳編號 (土銀沒使用。土銀學雜費的郵局資料不歸系統管)
        /// </summary>
        [FieldSpec(Field.CancelPono, false, FieldTypeEnum.Char, 16, true)]
        public string CancelPono
        {
            get
            {
                return _CancelPono;
            }
            set
            {
                _CancelPono = value == null ? null : value.Trim();
            }
        }

        private string _CancelSmno = null;
        /// <summary>
        /// 超商銷帳編號
        /// </summary>
        [FieldSpec(Field.CancelSmno, false, FieldTypeEnum.Char, 16, true)]
        public string CancelSmno
        {
            get
            {
                return _CancelSmno;
            }
            set
            {
                _CancelSmno = value == null ? null : value.Trim();
            }
        }

        private string _CancelFlag = null;
        /// <summary>
        /// 銷帳註記 (1=連線 / 2=金額不符 / 3=檢碼不符 / 7=銷問題檔 / 8=人工銷帳 / 9=離線) (參考 CancelFlagCodeTexts)
        /// </summary>
        [FieldSpec(Field.CancelFlag, false, FieldTypeEnum.Char, 1, true)]
        public string CancelFlag
        {
            get
            {
                return _CancelFlag;
            }
            set
            {
                _CancelFlag = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveBankId = null;
        /// <summary>
        /// 代收銀行/分行
        /// </summary>
        [FieldSpec(Field.ReceivebankId, false, FieldTypeEnum.Char, 7, true)]
        public string ReceivebankId
        {
            get
            {
                return _ReceiveBankId;
            }
            set
            {
                _ReceiveBankId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveDate = null;
        /// <summary>
        /// 代收日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Char, 7, true)]
        public string ReceiveDate
        {
            get
            {
                return _ReceiveDate;
            }
            set
            {
                _ReceiveDate = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 代收時間 (HHmmdd / HHmm)
        /// </summary>
        [FieldSpec(Field.ReceiveTime, false, FieldTypeEnum.Char, 6, true)]
        public string ReceiveTime
        {
            get;
            set;
        }

        private string _AccountDate = null;
        /// <summary>
        /// 入帳日期 (民國年日期7碼)
        /// </summary>
        [FieldSpec(Field.AccountDate, false, FieldTypeEnum.Char, 7, true)]
        public string AccountDate
        {
            get
            {
                return _AccountDate;
            }
            set
            {
                _AccountDate = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveWay = null;
        /// <summary>
        /// 繳費方式 (參考管道代碼)
        /// </summary>
        [FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, true)]
        public string ReceiveWay
        {
            get
            {
                return _ReceiveWay;
            }
            set
            {
                _ReceiveWay = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 手續費 (限 09-支付寶、NC-國際信用卡)
        /// </summary>
        [FieldSpec(Field.ProcessFee, false, FieldTypeEnum.Decimal, true)]
        public decimal? ProcessFee
        {
            get;
            set;
        }

        /// <summary>
        /// Real_Receive (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.RealReceive, false, FieldTypeEnum.Decimal, true)]
        public decimal? RealReceive
        {
            get;
            set;
        }

        /// <summary>
        /// Cancel_Receive (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.CancelReceive, false, FieldTypeEnum.Decimal, true)]
        public decimal? CancelReceive
        {
            get;
            set;
        }

        /// <summary>
        /// Fee_Receivable (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.FeeReceivable, false, FieldTypeEnum.Decimal, true)]
        public decimal? FeeReceivable
        {
            get;
            set;
        }

        /// <summary>
        /// Fee_Payable (土銀用來紀錄減免金額)
        /// </summary>
        [FieldSpec(Field.FeePayable, false, FieldTypeEnum.Decimal, true)]
        public decimal? FeePayable
        {
            get;
            set;
        }

        /// <summary>
        /// Postseq (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.Postseq, false, FieldTypeEnum.Char, 10, true)]
        public string Postseq
        {
            get;
            set;
        }

        /// <summary>
        /// Atm_meno (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.AtmMeno, false, FieldTypeEnum.Char, 50, true)]
        public string AtmMeno
        {
            get;
            set;
        }

        private string _StuHid = null;
        /// <summary>
        /// 座號
        /// </summary>
        [FieldSpec(Field.StuHid, false, FieldTypeEnum.Char, 10, true)]
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

        /// <summary>
        /// e網通 (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.EFlag, false, FieldTypeEnum.Char, 1, true)]
        public string EFlag
        {
            get;
            set;
        }

        private string _CFlag = null;
        /// <summary>
        /// c_flag (土銀用來記錄是否已送給CTCB，null 表示未送，0 表示資料被更新過可能需要重送，1 表示已送)
        /// </summary>
        [FieldSpec(Field.CFlag, false, FieldTypeEnum.Char, 1, true)]
        public string CFlag
        {
            get
            {
                return _CFlag;
            }
            set
            {
                _CFlag = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 建立日期時間
        /// </summary>
        [FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 最後修改日期時間
        /// </summary>
        [FieldSpec(Field.UpdateDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? UpdateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 上傳就學貸款金額 (BUD上傳的就貸金額)
        /// </summary>
        [FieldSpec(Field.Loan, false, FieldTypeEnum.Decimal, false)]
        public decimal Loan
        {
            get;
            set;
        }

        /// <summary>
        /// 實際貸款金額 (BUD上傳的就貸金額或由BUD上傳的就貸明細總額)
        /// </summary>
        [FieldSpec(Field.RealLoan, false, FieldTypeEnum.Decimal, false)]
        public decimal RealLoan
        {
            get;
            set;
        }

        private string _CancelDate = null;
        /// <summary>
        /// 銷帳日期 (YYYYMMDD)
        /// </summary>
        [FieldSpec(Field.CancelDate, false, FieldTypeEnum.Char, 8, true)]
        public string CancelDate
        {
            get
            {
                return _CancelDate;
            }
            set
            {
                _CancelDate = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// PrintCreate_Date (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.PrintcreateDate, false, FieldTypeEnum.Char, 8, true)]
        public string PrintcreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// Print_Date (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.PrintDate, false, FieldTypeEnum.Char, 8, true)]
        public string PrintDate
        {
            get;
            set;
        }

        private string _Exportreceivedata = null;
        /// <summary>
        /// exportReceiveData (土銀用來存放是否送過 D38 Y=是)
        /// </summary>
        [FieldSpec(Field.Exportreceivedata, false, FieldTypeEnum.Char, 1, false)]
        public string Exportreceivedata
        {
            get
            {
                return _Exportreceivedata;
            }
            set
            {
                _Exportreceivedata = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// InvCreate_Date (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.InvcreateDate, false, FieldTypeEnum.Char, 8, true)]
        public string InvcreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// Inv_Date (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.InvDate, false, FieldTypeEnum.Char, 8, true)]
        public string InvDate
        {
            get;
            set;
        }

        private string _MappingType = null;
        /// <summary>
        /// 匯入此資料的對照檔類型 (Excel:2, Txt:1)
        /// </summary>
        [FieldSpec(Field.MappingType, false, FieldTypeEnum.Char, 1, false)]
        public string MappingType
        {
            get
            {
                return _MappingType;
            }
            set
            {
                _MappingType = value == null ? null : value.Trim();
            }
        }

        private string _MappingId = null;
        /// <summary>
        /// 匯入此資料的對照檔代碼
        /// </summary>
        [FieldSpec(Field.MappingId, false, FieldTypeEnum.Char, 2, false)]
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

        /// <summary>
        /// remark (土銀沒用到)
        /// </summary>
        [FieldSpec(Field.Remark, false, FieldTypeEnum.VarCharMax, false)]
        public string Remark
        {
            get;
            set;
        }

        private string _DeductBankid = null;
        /// <summary>
        /// 扣款轉帳銀行代碼
        /// </summary>
        [FieldSpec(Field.DeductBankid, false, FieldTypeEnum.VarChar, 7, true)]
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
        [FieldSpec(Field.DeductAccountno, false, FieldTypeEnum.VarChar, 21, true)]
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
        [FieldSpec(Field.DeductAccountname, false, FieldTypeEnum.NVarChar, 60, true)]
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
        [FieldSpec(Field.DeductAccountid, false, FieldTypeEnum.VarChar, 10, true)]
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

        #region [MDY:2018xxxx] 上傳資料註記
        private string _Uploadflag = null;
        /// <summary>
        /// 上傳資料註記 (1=含流水號; 2=含銷帳編號; 3=含流水號與銷帳編號; 4=含總金額; 5=含流水號與總金額; 6=含銷帳編號與總金額; 7=含流水號、銷帳編號與總金額)
        /// </summary>
        [FieldSpec(Field.Uploadflag, false, FieldTypeEnum.Char, 1, true)]
        public string Uploadflag
        {
            get
            {
                return _Uploadflag;
            }
            set
            {
                _Uploadflag = value == null ? null : value.Trim();
                _UploadFlagValue = null;
            }
        }
        #endregion

        private string _CancelFlagFields = null;
        /// <summary>
        /// 儲存消帳註記相關欄位值
        /// </summary>
        [FieldSpec(Field.CancelFlagFields, false, FieldTypeEnum.VarChar, 100, true)]
        public string CancelFlagFields
        {
            get
            {
                return _CancelFlagFields;
            }
            set
            {
                _CancelFlagFields = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region 土銀專用部別代碼
        private string _DeptId = String.Empty;
        /// <summary>
        /// 部別代碼 (土銀專用)
        /// </summary>
        [FieldSpec(Field.DeptId, false, FieldTypeEnum.NVarChar, 20, true)]
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
        #endregion

        #region [MDY:2018xxxx] 備註 相關
        #region Member
        /// <summary>
        /// 儲存所有備註項目 (避免陣列索引與項目編號的轉換，陣列開 22 個元素 index 0 不使用，項目編號等於索引值)
        /// </summary>
        private string[] _AllMemoItemValues = new string[22];
        #endregion

        #region 備註
        /// <summary>
        /// 備註01
        /// </summary>
        [FieldSpec(Field.Memo01, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo01
        {
            get
            {
                return GetMemoItemValue(01);
            }
            set
            {
                this.SetMemoItemValue(01, value);
            }
        }

        /// <summary>
        /// 備註02
        /// </summary>
        [FieldSpec(Field.Memo02, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo02
        {
            get
            {
                return GetMemoItemValue(02);
            }
            set
            {
                this.SetMemoItemValue(02, value);
            }
        }

        /// <summary>
        /// 備註03
        /// </summary>
        [FieldSpec(Field.Memo03, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo03
        {
            get
            {
                return GetMemoItemValue(03);
            }
            set
            {
                this.SetMemoItemValue(03, value);
            }
        }

        /// <summary>
        /// 備註04
        /// </summary>
        [FieldSpec(Field.Memo04, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo04
        {
            get
            {
                return GetMemoItemValue(04);
            }
            set
            {
                this.SetMemoItemValue(04, value);
            }
        }

        /// <summary>
        /// 備註05
        /// </summary>
        [FieldSpec(Field.Memo05, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo05
        {
            get
            {
                return GetMemoItemValue(05);
            }
            set
            {
                this.SetMemoItemValue(05, value);
            }
        }

        /// <summary>
        /// 備註06
        /// </summary>
        [FieldSpec(Field.Memo06, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo06
        {
            get
            {
                return GetMemoItemValue(06);
            }
            set
            {
                this.SetMemoItemValue(06, value);
            }
        }

        /// <summary>
        /// 備註07
        /// </summary>
        [FieldSpec(Field.Memo07, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo07
        {
            get
            {
                return GetMemoItemValue(07);
            }
            set
            {
                this.SetMemoItemValue(07, value);
            }
        }

        /// <summary>
        /// 備註08
        /// </summary>
        [FieldSpec(Field.Memo08, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo08
        {
            get
            {
                return GetMemoItemValue(08);
            }
            set
            {
                this.SetMemoItemValue(08, value);
            }
        }

        /// <summary>
        /// 備註09
        /// </summary>
        [FieldSpec(Field.Memo09, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo09
        {
            get
            {
                return GetMemoItemValue(09);
            }
            set
            {
                this.SetMemoItemValue(09, value);
            }
        }

        /// <summary>
        /// 備註10
        /// </summary>
        [FieldSpec(Field.Memo10, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo10
        {
            get
            {
                return GetMemoItemValue(10);
            }
            set
            {
                this.SetMemoItemValue(10, value);
            }
        }
        #endregion

        #region 土銀增加備註數量
        /// <summary>
        /// 備註11
        /// </summary>
        [FieldSpec(Field.Memo11, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo11
        {
            get
            {
                return GetMemoItemValue(11);
            }
            set
            {
                this.SetMemoItemValue(11, value);
            }
        }

        /// <summary>
        /// 備註12
        /// </summary>
        [FieldSpec(Field.Memo12, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo12
        {
            get
            {
                return GetMemoItemValue(12);
            }
            set
            {
                this.SetMemoItemValue(12, value);
            }
        }

        /// <summary>
        /// 備註13
        /// </summary>
        [FieldSpec(Field.Memo13, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo13
        {
            get
            {
                return GetMemoItemValue(13);
            }
            set
            {
                this.SetMemoItemValue(13, value);
            }
        }

        /// <summary>
        /// 備註14
        /// </summary>
        [FieldSpec(Field.Memo14, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo14
        {
            get
            {
                return GetMemoItemValue(14);
            }
            set
            {
                this.SetMemoItemValue(14, value);
            }
        }

        /// <summary>
        /// 備註15
        /// </summary>
        [FieldSpec(Field.Memo15, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo15
        {
            get
            {
                return GetMemoItemValue(15);
            }
            set
            {
                this.SetMemoItemValue(15, value);
            }
        }

        /// <summary>
        /// 備註16
        /// </summary>
        [FieldSpec(Field.Memo16, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo16
        {
            get
            {
                return GetMemoItemValue(16);
            }
            set
            {
                this.SetMemoItemValue(16, value);
            }
        }

        /// <summary>
        /// 備註17
        /// </summary>
        [FieldSpec(Field.Memo17, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo17
        {
            get
            {
                return GetMemoItemValue(17);
            }
            set
            {
                this.SetMemoItemValue(17, value);
            }
        }

        /// <summary>
        /// 備註18
        /// </summary>
        [FieldSpec(Field.Memo18, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo18
        {
            get
            {
                return GetMemoItemValue(18);
            }
            set
            {
                this.SetMemoItemValue(18, value);
            }
        }

        /// <summary>
        /// 備註19
        /// </summary>
        [FieldSpec(Field.Memo19, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo19
        {
            get
            {
                return GetMemoItemValue(19);
            }
            set
            {
                this.SetMemoItemValue(19, value);
            }
        }

        /// <summary>
        /// 備註20
        /// </summary>
        [FieldSpec(Field.Memo20, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo20
        {
            get
            {
                return GetMemoItemValue(20);
            }
            set
            {
                this.SetMemoItemValue(20, value);
            }
        }

        /// <summary>
        /// 備註21
        /// </summary>
        [FieldSpec(Field.Memo21, false, FieldTypeEnum.NVarChar, 50, true)]
        public string Memo21
        {
            get
            {
                return GetMemoItemValue(21);
            }
            set
            {
                this.SetMemoItemValue(21, value);
            }
        }
        #endregion
        #endregion

        #region [MDY:20160131] 增加繳款期限
        private string _PayDueDate = String.Empty;
        /// <summary>
        /// 繳款期限 (格式：民國年7碼)
        /// </summary>
        [FieldSpec(Field.PayDueDate, false, FieldTypeEnum.Char, 7, true)]
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

        #region [MDY:20160305] 連動製單服務 相關欄位
        private string _ServiceSeqNo = String.Empty;
        /// <summary>
        /// 學校端惟一序號 (參加連動製單服務一定要有這個值)
        /// </summary>
        [FieldSpec(Field.ServiceSeqNo, false, FieldTypeEnum.VarChar, 32, true)]
        public string ServiceSeqNo
        {
            get
            {
                return _ServiceSeqNo;
            }
            set
            {
                _ServiceSeqNo = value == null ? null : value.Trim();
            }
        }

        private string _ServiceSchUser = String.Empty;
        /// <summary>
        /// 學校端操作人員
        /// </summary>
        [FieldSpec(Field.ServiceSchUser, false, FieldTypeEnum.NVarChar, 32, true)]
        public string ServiceSchUser
        {
            get
            {
                return _ServiceSchUser;
            }
            set
            {
                _ServiceSchUser = value == null ? null : value.Trim();
            }
        }

        private string _ServiceSchMDate = String.Empty;
        /// <summary>
        /// 學校端操作日期 (格式：yyyyMMdd)
        /// </summary>
        [FieldSpec(Field.ServiceSchMDate, false, FieldTypeEnum.VarChar, 8, true)]
        public string ServiceSchMDate
        {
            get
            {
                return _ServiceSchMDate;
            }
            set
            {
                _ServiceSchMDate = value == null ? null : value.Trim();
            }
        }

        private string _ServiceSchMTime = String.Empty;
        /// <summary>
        /// 學校端操作時間 (格式：HHmmss)
        /// </summary>
        [FieldSpec(Field.ServiceSchMTime, false, FieldTypeEnum.VarChar, 6, true)]
        public string ServiceSchMTime
        {
            get
            {
                return _ServiceSchMTime;
            }
            set
            {
                _ServiceSchMTime = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
        private string _NCCardFlag = "N";
        /// <summary>
        /// 是否啟用國際信用卡繳費旗標 (Y=是; N=否; 預設值 N)
        /// </summary>
        [FieldSpec(Field.NCCardFlag, false, FieldTypeEnum.Char, 1, true)]
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
        #endregion

        #region Method
        #region [MDY:2018xxxx] 銷帳狀態 相關
        /// <summary>
        /// 取得銷帳狀態代碼
        /// </summary>
        /// <returns>傳回銷帳狀態代碼</returns>
        public string GetCancelStatus()
        {
            if (!String.IsNullOrWhiteSpace(this.AccountDate))
            {
                return CancelStatusCodeTexts.CANCELED;
            }
            else if (!String.IsNullOrWhiteSpace(this.ReceiveDate) || !String.IsNullOrWhiteSpace(this.ReceiveWay))
            {
                return CancelStatusCodeTexts.PAYED;
            }
            else
            {
                return CancelStatusCodeTexts.NONPAY;
            }
        }
        #endregion

        #region [MDY:2018xxxx] 上傳資料註記 相關
        #region Static Readonly (上傳資料註記值定義)
        /// <summary>
        /// 上傳資料包含流水號旗標 : 1
        /// </summary>
        public static readonly int UploadSeriorNoFlag = 1;
        /// <summary>
        /// 上傳資料包含虛擬帳號旗標 : 2
        /// </summary>
        public static readonly int UploadCancelNoFlag = 2;
        /// <summary>
        /// 上傳資料包含應繳金額旗標 : 4
        /// </summary>
        public static readonly int UploadAmountFlag = 4;
        #endregion

        #region Member
        private int? _UploadFlagValue = null;
        #endregion

        /// <summary>
        /// 取得上傳資料註記的數值 (1=含流水號; 2=含銷帳編號; 3=含流水號與銷帳編號; 4=含總金額; 5=含流水號與總金額; 6=含銷帳編號與總金額; 7=含流水號、銷帳編號與總金額)
        /// </summary>
        /// <returns>傳回上傳資料註記的數值，或 0</returns>
        public int GetUploadFlagValue()
        {
            if (_UploadFlagValue == null)
            {
                int value = 0;
                if (Int32.TryParse(this.Uploadflag, out value) && value >= 0)
                {
                    _UploadFlagValue = value;
                }
                else
                {
                    _UploadFlagValue = 0;
                }
            }
            return _UploadFlagValue.Value;
        }

        /// <summary>
        /// 取得此資料是否有上傳流水號
        /// </summary>
        /// <returns></returns>
        public bool HasUploadSeriorNo()
        {
            int value = this.GetUploadFlagValue();
            return ((value & UploadSeriorNoFlag) == UploadSeriorNoFlag);
        }

        /// <summary>
        /// 取得此資料是否有上傳銷帳編號
        /// </summary>
        /// <returns></returns>
        public bool HasUploadCancelNo()
        {
            int value = this.GetUploadFlagValue();
            return ((value & UploadCancelNoFlag) == UploadCancelNoFlag);
        }

        /// <summary>
        /// 取得此資料是否有上傳總金額
        /// </summary>
        /// <returns></returns>
        public bool HasUploadAmount()
        {
            int value = this.GetUploadFlagValue();
            return ((value & UploadAmountFlag) == UploadAmountFlag);
        }
        #endregion

        #region [MDY:2018xxxx] 收入科目金額 相關
        /// <summary>
        /// 取得所有收入科目金額的陣列 (依序 Receive01 ~ Receive40)
        /// </summary>
        /// <returns>傳回所有收入科目金額的陣列</returns>
        public decimal?[] GetAllReceiveItemAmounts()
        {
            decimal?[] amounts = new decimal?[_AllReceiveItemAmounts.Length - 1];
            Array.Copy(_AllReceiveItemAmounts, 1, amounts, 0, amounts.Length);
            return amounts;
        }

        /// <summary>
        /// 設定指定項目編號(1 ~ 40)的收入科目金額
        /// </summary>
        /// <param name="itemNo">指定收入科目的項目編號(1 ~ 40)</param>
        /// <param name="amount">指定收入科目金額</param>
        /// <returns>成功傳回 true。否則傳回 false</returns>
        public bool SetReceiveItemAmount(int itemNo, decimal? amount)
        {
            if (itemNo > 0 && itemNo < _AllReceiveItemAmounts.Length)
            {
                _AllReceiveItemAmounts[itemNo] = amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定項目編號(1 ~ 40)的收入科目金額
        /// </summary>
        /// <param name="itemNo">指定收入科目的項目編號(1 ~ 40)</param>
        /// <returns>成功傳回收入科目金額，無金額或失敗則傳回 null</returns>
        public decimal? GetReceiveItemAmount(int itemNo)
        {
            if (itemNo > 0 && itemNo < _AllReceiveItemAmounts.Length)
            {
                return _AllReceiveItemAmounts[itemNo];
            }
            return null;
        }
        #endregion

        #region [MDY:2018xxxx] 備註 相關
        /// <summary>
        /// 取得所有備註內容的陣列 (依序 Memo01 ~ Memo21)
        /// </summary>
        /// <returns>傳回所有備註內容的陣列</returns>
        public string[] GetAllMemoItemValues()
        {
            string[] values = new string[_AllMemoItemValues.Length - 1];
            Array.Copy(_AllMemoItemValues, 1, values, 0, values.Length);
            return values;
        }

        /// <summary>
        /// 設定指定項目編號(1 ~ 21)的備註內容
        /// </summary>
        /// <param name="itemNo">指定備註的項目編號(1 ~ 21)</param>
        /// <param name="value">指定備註內容</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool SetMemoItemValue(int itemNo, string value)
        {
            if (itemNo > 0 && itemNo < _AllMemoItemValues.Length)
            {
                _AllMemoItemValues[itemNo] = value == null ? null : value.Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定項目編號(1 ~ 21)的備註內容
        /// </summary>
        /// <param name="itemNo">指定備註的項目編號(1 ~ 21)</param>
        /// <returns>成功傳回備註內容，無內容或失敗則傳回 null</returns>
        public string GetMemoItemValue(int itemNo)
        {
            if (itemNo > 0 && itemNo < _AllMemoItemValues.Length)
            {
                return _AllMemoItemValues[itemNo];
            }
            return null;
        }
        #endregion
        #endregion
    }
}
