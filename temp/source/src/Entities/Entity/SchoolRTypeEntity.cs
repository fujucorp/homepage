/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/12/08 17:25:54
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
	/// 商家代號資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class SchoolRTypeEntity : Entity
	{
        #region Const
        #region [MDY:20160206] 最大可指定的線上資料保留年數、最大可指定的歷史資料保留年數
        /// <summary>
        /// 最大可指定的線上資料保留年數 (10)
        /// </summary>
        public const int MaxKeepDataYear = 10;

        /// <summary>
        /// 最大可指定的歷史資料保留年數 (10)
        /// </summary>
        public const int MaxKeepHistoryYear = 10;
        #endregion
        #endregion

        public const string TABLE_NAME = "School_Rtype";

		#region Field Name Const Class
		/// <summary>
		/// SchoolRTypeEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 代收類別代碼
			/// </summary>
			public const string ReceiveType = "Receive_Type";
			#endregion

			#region Data
			/// <summary>
			/// 代收類別名稱 (學校全名)
			/// </summary>
			public const string SchName = "Sch_Name";

			/// <summary>
			/// 學校識別編號 (土銀沒使用)
			/// </summary>
			public const string SchId = "Sch_ID";

			/// <summary>
			/// 學校地址
			/// </summary>
			public const string SchAddress = "Sch_Address";

			/// <summary>
			/// 校長名稱
			/// </summary>
			public const string SchPrincipal = "Sch_Principal";

			/// <summary>
			/// 銀行帳戶名稱
			/// </summary>
			public const string AccountName = "Account_Name";

			/// <summary>
            /// 所屬(主辦)分行代碼 (7碼) (土銀只給六碼)
			/// </summary>
			public const string BankId = "Bank_Id";

			/// <summary>
			/// 是否檢查繳費期限旗標 (y=是; n=否) (注意資料庫存的是小寫)
			/// </summary>
			public const string PayOver = "Pay_over";

			/// <summary>
			/// 學校網址
			/// </summary>
			public const string Url = "Url";

			/// <summary>
			/// 學校統一編號 (土銀用來存學校代碼)
			/// </summary>
			public const string SchIdenty = "Sch_Identy";

			/// <summary>
			/// 銀行帳戶號碼
			/// </summary>
			public const string SchAccount = "Sch_Account";

            #region [MDY:20211001] M202110_01 設定 FTP 種類 (2021擴充案先做)
            /// <summary>
            /// FTP 種類 (FTP / FTPS / SFTP)
            /// </summary>
            public const string FtpKind = "FTP_Kind";
            #endregion

			/// <summary>
			/// FTP 伺服器的位置
			/// </summary>
			public const string FtpLocation = "FTP_location";

			/// <summary>
			/// FTP 伺服器的 Port 號
			/// </summary>
			public const string FtpPort = "FTP_port";

			/// <summary>
			/// FTP 的 登入帳號
			/// </summary>
			public const string FtpAccount = "FTP_Account";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// FTP 的 登入密碼
			/// </summary>
			public const string FtpPXX = "FTP_PWD";
			#endregion
			#endregion

			/// <summary>
			/// 優先次序編號 (1~5)
			/// </summary>
			public const string SchPri = "Sch_Pri";

			/// <summary>
            /// 預估處理量
			/// </summary>
			public const string SchHAmount = "Sch_H_Amount";

			/// <summary>
			/// 學校郵遞區號
			/// </summary>
			public const string SchPostal = "Sch_Postal";

			/// <summary>
			/// 學校主要聯絡人名稱
			/// </summary>
			public const string SchContract = "Sch_Contract";

			/// <summary>
			/// 學校主要聯絡人電話
			/// </summary>
			public const string SchConTel = "Sch_Con_Tel";

			/// <summary>
			/// 學校次要聯絡人名稱
			/// </summary>
			public const string SchContract1 = "Sch_Contract1";

			/// <summary>
			/// 學校次要聯絡人電話
			/// </summary>
			public const string SchConTel1 = "Sch_Con_Tel1";

			/// <summary>
            /// 是否參加其他代收管道 (0~25) (土銀停用，固定設為空字串)
			/// </summary>
			public const string EFlag = "e_flag";

			/// <summary>
            /// 是否可銷帳完帳和產生銷帳編號後、再上傳學生資料 (1=是; 0=否)
			/// </summary>
			public const string AFlag = "a_flag";

			/// <summary>
			/// 學校 Email
			/// </summary>
			public const string SchMail = "Sch_Mail";

			/// <summary>
			/// 銀行 Email
			/// </summary>
			public const string BankMail = "Bank_Mail";

			/// <summary>
            /// 是否需分行控管才可新增代收費用別  (1=是; 0=否)
			/// </summary>
			public const string CFlag = "c_flag";

			/// <summary>
            /// (土銀停用，固定設為 null)
			/// </summary>
			public const string MailFlag = "mail_flag";

			/// <summary>
            /// (土銀停用，固定設為 null)
			/// </summary>
			public const string DistrictNo = "District_No";

			/// <summary>
            /// (土銀停用，固定設為 null)
			/// </summary>
			public const string SchLvNo = "Sch_Lv_No";

			/// <summary>
            /// (土銀停用，固定設為 null)
			/// </summary>
			public const string ApplyDate = "Apply_Date";

			/// <summary>
            /// 繳費單格式代碼 (0=二聯式; 1=三聯式; 8=學制、類別公版; 9=專屬) (土銀只使用 0, 1, 9)
			/// </summary>
			public const string PayStyle = "PayStyle";

			/// <summary>
            /// 是否提供分項繳費單旗標 (1=是; 0=否) (限三聯式繳費單格式)
			/// </summary>
			public const string DiviFlag = "Divi_Flag";

			/// <summary>
            /// 專屬繳費單模板檔名 (土銀停用，固定設為空字串)
			/// </summary>
			public const string SpecialPayStyleName = "SpecialPayStyleName";

            ///// <summary>
            ///// 專屬繳費單模板檔案內容 byte (土銀停用，固定設為 null)
            ///// </summary>
            //public const string SpecialPayStyle = "SpecialPayStyle";

			/// <summary>
            /// 超商4~6萬使用的識別碼 (土銀停用，固定設為空字串)
			/// </summary>
			public const string SpecBarCode = "SpecBarCode";

			/// <summary>
            /// 學校代理(同業)銀行代碼 (3碼) (土銀停用，固定設為空字串)
			/// </summary>
			public const string AgentBank = "Agent_Bank";

			/// <summary>
            /// 是否為企業戶旗標 (Y=帳單代收的代收類別; N=學雜費的代收類別) (土銀停用，固定設為 N)
			/// </summary>
			public const string IsCorp = "isCorp";

			/// <summary>
            /// 是否使用簡易網站旗標 (Y=是; N=否) (土銀停用，固定設為 N)
			/// </summary>
			public const string UseEZTwbank = "useEZTwbank";

			/// <summary>
            /// 是否在學校申貸名冊上傳後重新計算金額  (Y=是; N=否)
			/// </summary>
			public const string CalcSchoolLoan = "calcSchoolLoan";

			/// <summary>
            /// 郵局手續費是否內含旗標 (Y=是; N=否)
			/// </summary>
			public const string PostFeeInclude = "PostFeeInclude";

			/// <summary>
            /// 郵局手續費金額 (土銀停用，固定設為 0)
			/// </summary>
			public const string PostFee = "PostFee";

			/// <summary>
            /// 是否採新郵局手續費計算(外加)旗標 (Y=是; N=否) (土銀停用，固定設為空字串)
			/// </summary>
			public const string UseNewPostCharge = "UseNewPostCharge";

			/// <summary>
            /// 2萬(含)以下郵局手續費金額
			/// </summary>
			public const string PostFee1 = "PostFee1";

			/// <summary>
            /// 2萬以上郵局手續費金額
			/// </summary>
			public const string PostFee2 = "PostFee2";

			/// <summary>
            /// 學制、類別 (1:大專院校 2:高中職 3:國中小 4:幼兒園)
			/// </summary>
			public const string CorpType = "CorpType";

			/// <summary>
            /// 是否使用20碼的學號旗標 (Y=是; N=否)
			/// </summary>
			public const string UseStuId20 = "UseStuID20";

			/// <summary>
            /// 是否使用教育部補助旗標 (Y=是; N=否)
			/// </summary>
			public const string UseEduSubsidy = "UseEduSubsidy";

			/// <summary>
            /// 教育部補助的字樣
			/// </summary>
			public const string EduSubsidyLabel = "EduSubsidyLabel";

			/// <summary>
            /// 繳費清冊(新版)是否使用「MAC值驗證」功能 (Y=是; N=否) (土銀停用，固定設為空字串)
			/// </summary>
			public const string UseVerify = "useVerify";

			/// <summary>
            /// 是否可使用印鑑旗標 (Y=是; N=否) (土銀停用，固定設為 N)
			/// </summary>
			public const string UseStamp = "useStamp";

			/// <summary>
            /// 是否可使用郵局繳䥗期限旗標 (土銀停用，固定設為 N)
			/// </summary>
			public const string UsePostDueDate = "usePostDueDate";

			/// <summary>
            /// 是否可使用多聯繳費單功能旗標 (Y=是; N=否) (限三聯式繳費單格式) (土銀停用，固定設為 N)
			/// </summary>
			public const string UseMultiCopy = "useMultiCopy";

            ///// <summary>
            ///// 浮水印圖檔 byte (土銀停用，固定設為 null)
            ///// </summary>
            //public const string Watermark = "watermark";

			/// <summary>
            /// 是否可使用UTF8功能 (Y=是; N=否) (土銀停用，固定設為空字串)
			/// </summary>
			public const string UseUTF8 = "useUTF8";

			/// <summary>
            /// 印鑑圖檔大小限制 (KB) (土銀停用，固定設為 0)
			/// </summary>
			public const string StampSize = "StampSize";

			/// <summary>
            /// 繳費清冊(新版)是否可使用「PDF格式」功能 (Y=是; N=否) (土銀停用，固定設為空字串)
			/// </summary>
			public const string UseP16PDF = "useP16PDF";

			/// <summary>
            /// 浮水印 Size (土銀停用，固定設為 0)
			/// </summary>
			public const string WatermarkSize = "watermarksize";

			/// <summary>
            /// 是否可使用「個人備註」功能 (Y=是; N=否) (土銀停用，固定設為空字串)
			/// </summary>
			public const string UseStuRemark = "UseStuRemark";

            /// <summary>
            /// 銷帳編號規則
            /// </summary>
            public const string CancelNoRule = "CancelNoRule";

            #region 財金特店資料相關欄位
            /// <summary>
            /// 土銀的客戶委託代號
            /// </summary>
            public const string DeductId = "Deduct_Id";

            /// <summary>
            /// 土銀的學校財金特店代碼參數
            /// </summary>
            public const string MerchantId = "MerchantId";

            /// <summary>
            /// 土銀的學校財金端末機代號參數
            /// </summary>
            public const string TerminalId = "TerminalId";

            /// <summary>
            /// 土銀的學校財金特店編號參數
            /// </summary>
            public const string MerId = "merId";
            #endregion

            /// <summary>
            /// 是否使用2位數的費用別代碼旗標 (預設 N，一律轉大寫，且非 Y 值一律轉為 N)
            /// </summary>
            public const String BigReceiveIdFlag = "Big_ReceiveId_Fg";

            /// <summary>
            /// 是否開放學生專區 (Y/N，預設 Y，一律轉大寫，且非 N 值一律轉為 Y)
            /// </summary>
            public const String OpenStudentArea = "Open_Student_Area";

            /// <summary>
            /// 代收種類 (1:學雜費 2:代收各項費用，預設 1，請參考 ReceiveKindCodeTexts)
            /// </summary>
            public const String ReceiveKind = "Receive_Kind";

            #region [MDY:20160201] 流程(階層)種類
            /// <summary>
            /// 流程(階層)種類 (S:單階 (免審核) M:多階 (需審核)，預設 S，請參考 FlowKindCodeTexts)
            /// </summary>
            public const String FlowKind = "Flow_Kind";
            #endregion

            #region [MDY:20160202] 新增線上資料保留年數、歷史資料保留年數
            /// <summary>
            /// 線上資料保留年數
            /// </summary>
            public const String KeepDataYear = "Keep_Data_Year";

            /// <summary>
            /// 歷史資料保留年數
            /// </summary>
            public const String KeepHistoryYear = "Keep_History_Year";
            #endregion

            #region [MDY:20191214] 2019擴充案 國際信用卡 - 財金特店參數相關欄位
            /// <summary>
            /// 土銀的國際信用卡-學校財金特店代碼參數
            /// </summary>
            public const string MerchantId2 = "MerchantId2";

            /// <summary>
            /// 土銀的國際信用卡-學校財金端末機代號參數
            /// </summary>
            public const string TerminalId2 = "TerminalId2";

            /// <summary>
            /// 土銀的國際信用卡-學校財金特店編號參數
            /// </summary>
            public const string MerId2 = "merId2";

            /// <summary>
            /// 土銀的國際信用卡-學校手續費率參數
            /// </summary>
            public const string HandlingFeeRate = "Handling_Fee_Rate";
			#endregion

			#region [MDY:20220808] 2022擴充案 英文資料啟用、學校英文全名 新增欄位
			/// <summary>
			/// 英文資料啟用 (Y:啟用 N:停用，預設 N，一律轉大寫，且非 Y 值一律轉為 N)
			/// </summary>
			public const String EngEnabled = "Eng_Enabled";

			/// <summary>
			/// 學校英文全名
			/// </summary>
			public const string SchEName = "Sch_EName";
			#endregion
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
		/// SchoolRTypeEntity 類別建構式
		/// </summary>
		public SchoolRTypeEntity()
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
		#endregion

		#region Data
		/// <summary>
		/// 代收類別名稱 (學校全名)
		/// </summary>
		[FieldSpec(Field.SchName, false, FieldTypeEnum.VarChar, 54, false)]
		public string SchName
		{
			get;
			set;
		}

		/// <summary>
		/// 學校識別編號 (土銀沒使用)
		/// </summary>
		[FieldSpec(Field.SchId, false, FieldTypeEnum.Char, 6, false)]
		public string SchId
		{
			get;
			set;
		}

		/// <summary>
		/// 學校地址
		/// </summary>
		[FieldSpec(Field.SchAddress, false, FieldTypeEnum.VarChar, 100, false)]
		public string SchAddress
		{
			get;
			set;
		}

		/// <summary>
		/// 校長名稱
		/// </summary>
		[FieldSpec(Field.SchPrincipal, false, FieldTypeEnum.VarChar, 10, true)]
		public string SchPrincipal
		{
			get;
			set;
		}

		/// <summary>
		/// 銀行帳戶名稱
		/// </summary>
		[FieldSpec(Field.AccountName, false, FieldTypeEnum.Char, 54, true)]
		public string AccountName
		{
			get;
			set;
		}

		/// <summary>
		/// 所屬(主辦)分行代碼 (7碼) (土銀只給六碼)
		/// </summary>
		[FieldSpec(Field.BankId, false, FieldTypeEnum.Char, 7, false)]
		public string BankId
		{
			get;
			set;
		}

		/// <summary>
		/// 是否檢查繳費期限旗標 (y=是; n=否) (注意資料庫存的是小寫)
		/// </summary>
		[FieldSpec(Field.PayOver, false, FieldTypeEnum.Char, 1, true)]
		public string PayOver
		{
			get;
			set;
		}

		/// <summary>
		/// 學校網址
		/// </summary>
		[FieldSpec(Field.Url, false, FieldTypeEnum.VarChar, 40, true)]
		public string Url
		{
			get;
			set;
		}

		private string _SchIdenty = null;
		/// <summary>
		/// 學校統一編號 (土銀用來存學校代碼)
		/// </summary>
		[FieldSpec(Field.SchIdenty, false, FieldTypeEnum.Char, 8, true)]
		public string SchIdenty
		{
            get
            {
                return _SchIdenty;
            }
            set
            {
                _SchIdenty = value == null ? null : value.Trim();
            }
		}

		/// <summary>
		/// 銀行帳戶號碼
		/// </summary>
		[FieldSpec(Field.SchAccount, false, FieldTypeEnum.Char, 21, true)]
		public string SchAccount
		{
			get;
			set;
		}

        #region [MDY:20211001] M202110_01 設定 FTP 種類 (2021擴充案先做)
        /// <summary>
        /// FTP 種類 (FTP / FTPS / SFTP)
        /// </summary>
        [FieldSpec(Field.FtpKind, false, FieldTypeEnum.VarChar, 8, true)]
        public string FtpKind
        {
            get;
            set;
        }
        #endregion

		/// <summary>
		/// FTP 伺服器的位置
		/// </summary>
		[FieldSpec(Field.FtpLocation, false, FieldTypeEnum.VarChar, 40, true)]
		public string FtpLocation
		{
			get;
			set;
		}

		/// <summary>
		/// FTP 伺服器的 Port 號
		/// </summary>
		[FieldSpec(Field.FtpPort, false, FieldTypeEnum.VarChar, 5, true)]
		public string FtpPort
		{
			get;
			set;
		}

		/// <summary>
		/// FTP 的 登入帳號
		/// </summary>
		[FieldSpec(Field.FtpAccount, false, FieldTypeEnum.VarChar, 15, true)]
		public string FtpAccount
		{
			get;
			set;
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		/// <summary>
		/// FTP 的 登入密碼
		/// </summary>
		[FieldSpec(Field.FtpPXX, false, FieldTypeEnum.VarChar, 16, true)]
		public string FtpPXX
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// 優先次序編號 (1~5)
		/// </summary>
		[FieldSpec(Field.SchPri, false, FieldTypeEnum.Char, 3, true)]
		public string SchPri
		{
			get;
			set;
		}

		/// <summary>
        /// 預估處理量
		/// </summary>
		[FieldSpec(Field.SchHAmount, false, FieldTypeEnum.Char, 5, true)]
		public string SchHAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 學校郵遞區號
		/// </summary>
		[FieldSpec(Field.SchPostal, false, FieldTypeEnum.VarChar, 5, true)]
		public string SchPostal
		{
			get;
			set;
		}

		/// <summary>
		/// 學校主要聯絡人名稱
		/// </summary>
		[FieldSpec(Field.SchContract, false, FieldTypeEnum.Char, 10, true)]
		public string SchContract
		{
			get;
			set;
		}

		/// <summary>
		/// 學校主要聯絡人電話
		/// </summary>
		[FieldSpec(Field.SchConTel, false, FieldTypeEnum.Char, 20, true)]
		public string SchConTel
		{
			get;
			set;
		}

		/// <summary>
		/// 學校次要聯絡人名稱
		/// </summary>
		[FieldSpec(Field.SchContract1, false, FieldTypeEnum.Char, 10, true)]
		public string SchContract1
		{
			get;
			set;
		}

		/// <summary>
		/// 學校次要聯絡人電話
		/// </summary>
		[FieldSpec(Field.SchConTel1, false, FieldTypeEnum.Char, 20, true)]
		public string SchConTel1
		{
			get;
			set;
		}

		/// <summary>
        /// 是否參加其他代收管道 (0~25) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.EFlag, false, FieldTypeEnum.Char, 2, true)]
		public string EFlag
		{
			get;
			set;
		}

		/// <summary>
        /// 是否可銷帳完帳和產生銷帳編號後、再上傳學生資料 (1=是; 0=否) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.AFlag, false, FieldTypeEnum.Char, 2, true)]
		public string AFlag
		{
			get;
			set;
		}

		/// <summary>
		/// 學校 Email
		/// </summary>
		[FieldSpec(Field.SchMail, false, FieldTypeEnum.VarChar, 500, true)]
		public string SchMail
		{
			get;
			set;
		}

		/// <summary>
		/// 銀行 Email
		/// </summary>
		[FieldSpec(Field.BankMail, false, FieldTypeEnum.VarChar, 500, true)]
		public string BankMail
		{
			get;
			set;
		}

		/// <summary>
        /// 是否需分行控管才可新增代收費用別  (1=是; 0=否)
		/// </summary>
		[FieldSpec(Field.CFlag, false, FieldTypeEnum.Integer, true)]
		public int? CFlag
		{
			get;
			set;
		}

		/// <summary>
        /// (土銀停用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.MailFlag, false, FieldTypeEnum.Integer, true)]
		public int? MailFlag
		{
			get;
			set;
		}

		/// <summary>
        /// (土銀停用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.DistrictNo, false, FieldTypeEnum.Integer, true)]
		public int? DistrictNo
		{
			get;
			set;
		}

		/// <summary>
        /// (土銀停用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.SchLvNo, false, FieldTypeEnum.Integer, true)]
		public int? SchLvNo
		{
			get;
			set;
		}

		/// <summary>
        /// (土銀停用，固定設為 null)
		/// </summary>
		[FieldSpec(Field.ApplyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? ApplyDate
		{
			get;
			set;
		}

		/// <summary>
        /// 繳費單格式代碼 (0=二聯式; 1=三聯式; 8=學制、類別公版; 9=專屬) (土銀只使用 0, 1, 9)
		/// </summary>
		[FieldSpec(Field.PayStyle, false, FieldTypeEnum.Char, 1, true)]
		public string PayStyle
		{
			get;
			set;
		}

		/// <summary>
        /// 是否提供分項繳費單旗標 (1=是; 0=否) (限三聯式繳費單格式) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.DiviFlag, false, FieldTypeEnum.Char, 1, true)]
		public string DiviFlag
		{
			get;
			set;
		}

		/// <summary>
        /// 專屬繳費單模板檔名 (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.SpecialPayStyleName, false, FieldTypeEnum.VarChar, 100, true)]
		public string SpecialPayStyleName
		{
			get;
			set;
		}

        ///// <summary>
        ///// 專屬繳費單模板檔案內容 byte (土銀停用，固定設為 null)
        ///// </summary>
        //[FieldSpec(Field.SpecialPayStyle, false, FieldTypeEnum.Binary, true)]
        //public byte[] SpecialPayStyle
        //{
        //    get;
        //    set;
        //}

		/// <summary>
        /// 超商4~6萬使用的識別碼 (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.SpecBarCode, false, FieldTypeEnum.VarChar, 3, true)]
		public string SpecBarCode
		{
			get;
			set;
		}

		/// <summary>
        /// 學校代理(同業)銀行代碼 (3碼) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.AgentBank, false, FieldTypeEnum.VarChar, 7, true)]
		public string AgentBank
		{
			get;
			set;
		}

		/// <summary>
        /// 是否為企業戶旗標 (Y=帳單代收的代收類別; N=學雜費的代收類別) (土銀停用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.IsCorp, false, FieldTypeEnum.Char, 1, true)]
		public string IsCorp
		{
			get;
			set;
		}

		/// <summary>
        /// 是否使用簡易網站旗標 (Y=是; N=否) (土銀停用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.UseEZTwbank, false, FieldTypeEnum.Char, 1, true)]
		public string UseEZTwbank
		{
			get;
			set;
		}

		/// <summary>
        /// 是否在學校申貸名冊上傳後重新計算金額  (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.CalcSchoolLoan, false, FieldTypeEnum.Char, 1, true)]
		public string CalcSchoolLoan
		{
			get;
			set;
		}

		/// <summary>
        /// 郵局手續費是否內含旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.PostFeeInclude, false, FieldTypeEnum.Char, 1, false)]
		public string PostFeeInclude
		{
			get;
			set;
		}

		/// <summary>
        /// 郵局手續費金額 土銀停用，固定設為 0)
		/// </summary>
		[FieldSpec(Field.PostFee, false, FieldTypeEnum.Decimal, false)]
		public decimal PostFee
		{
			get;
			set;
		}

		/// <summary>
        /// 是否採新郵局手續費計算(外加)旗標 (Y=是; N=否) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.UseNewPostCharge, false, FieldTypeEnum.Char, 1, false)]
		public string UseNewPostCharge
		{
			get;
			set;
		}

		/// <summary>
        /// 2萬(含)以下郵局手續費金額
		/// </summary>
		[FieldSpec(Field.PostFee1, false, FieldTypeEnum.Decimal, false)]
		public decimal PostFee1
		{
			get;
			set;
		}

		/// <summary>
        /// 2萬以上郵局手續費金額
		/// </summary>
		[FieldSpec(Field.PostFee2, false, FieldTypeEnum.Decimal, false)]
		public decimal PostFee2
		{
			get;
			set;
		}

		/// <summary>
        /// 學制、類別 (1:大專院校 2:高中職 3:國中小 4:幼兒園)
		/// </summary>
		[FieldSpec(Field.CorpType, false, FieldTypeEnum.VarChar, 3, false)]
		public string CorpType
		{
			get;
			set;
		}

		/// <summary>
        /// 是否使用20碼的學號旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.UseStuId20, false, FieldTypeEnum.Char, 1, false)]
		public string UseStuId20
		{
			get;
			set;
		}

		/// <summary>
        /// 是否使用教育部補助旗標 (Y=是; N=否)
		/// </summary>
		[FieldSpec(Field.UseEduSubsidy, false, FieldTypeEnum.Char, 1, false)]
		public string UseEduSubsidy
		{
			get;
			set;
		}

		/// <summary>
        /// 教育部補助的字樣
		/// </summary>
		[FieldSpec(Field.EduSubsidyLabel, false, FieldTypeEnum.VarChar, 40, false)]
		public string EduSubsidyLabel
		{
			get;
			set;
		}

		/// <summary>
        /// 繳費清冊(新版)是否使用「MAC值驗證」功能 (Y=是; N=否) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.UseVerify, false, FieldTypeEnum.Char, 1, false)]
		public string UseVerify
		{
			get;
			set;
		}

		/// <summary>
        /// 是否可使用印鑑旗標 (Y=是; N=否) (土銀停用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.UseStamp, false, FieldTypeEnum.Char, 1, false)]
		public string UseStamp
		{
			get;
			set;
		}

		/// <summary>
        /// 是否可使用郵局繳䥗期限旗標 (土銀停用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.UsePostDueDate, false, FieldTypeEnum.Char, 1, false)]
		public string UsePostDueDate
		{
			get;
			set;
		}

		/// <summary>
        /// 是否可使用多聯繳費單功能旗標 (Y=是; N=否) (限三聯式繳費單格式) (土銀停用，固定設為 N)
		/// </summary>
		[FieldSpec(Field.UseMultiCopy, false, FieldTypeEnum.Char, 1, false)]
		public string UseMultiCopy
		{
			get;
			set;
		}

        ///// <summary>
        ///// 浮水印圖檔 byte (土銀停用，固定設為 null)
        ///// </summary>
        //[FieldSpec(Field.Watermark, false, FieldTypeEnum.Binary, true)]
        //public byte[] Watermark
        //{
        //    get;
        //    set;
        //}

		/// <summary>
        /// 是否可可使用UTF8功能 (Y=是; N=否) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.UseUTF8, false, FieldTypeEnum.Char, 1, false)]
		public string UseUTF8
		{
			get;
			set;
		}

		/// <summary>
        /// 印鑑圖檔大小限制 (KB) (土銀停用，固定設為 0)
		/// </summary>
		[FieldSpec(Field.StampSize, false, FieldTypeEnum.Integer, false)]
		public int StampSize
		{
			get;
			set;
		}

		/// <summary>
        /// 繳費清冊(新版)是否可使用「PDF格式」功能 (Y=是; N=否) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.UseP16PDF, false, FieldTypeEnum.Char, 1, false)]
		public string UseP16PDF
		{
			get;
			set;
		}

		/// <summary>
        /// 浮水印 Size (土銀停用，固定設為 0)
		/// </summary>
		[FieldSpec(Field.WatermarkSize, false, FieldTypeEnum.Integer, false)]
		public int WatermarkSize
		{
			get;
			set;
		}

		/// <summary>
        /// 是否可使用「個人備註」功能 (Y=是; N=否) (土銀停用，固定設為空字串)
		/// </summary>
		[FieldSpec(Field.UseStuRemark, false, FieldTypeEnum.Char, 1, false)]
		public string UseStuRemark
		{
			get;
			set;
		}

        /// <summary>
        /// 銷帳編號規則 (請參考 CancelNoHelper)
        /// </summary>
        [FieldSpec(Field.CancelNoRule, false, FieldTypeEnum.VarChar, 6, false)]
        public string CancelNoRule
        {
            get;
            set;
        }

        #region 財金特店資料相關欄位
        private string _DeductId = String.Empty;
        /// <summary>
        /// 土銀的客戶委託代號
        /// </summary>
        [FieldSpec(Field.DeductId, false, FieldTypeEnum.VarChar, 10, true)]
        public string DeductId
        {
            get
            {
                return _DeductId;
            }
            set
            {
                _DeductId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _MerchantId = String.Empty;
        /// <summary>
        /// 土銀的客戶委託代號
        /// </summary>
        [FieldSpec(Field.MerchantId, false, FieldTypeEnum.VarChar, 15, true)]
        public string MerchantId
        {
            get
            {
                return _MerchantId;
            }
            set
            {
                _MerchantId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _TerminalId = String.Empty;
        /// <summary>
        /// 土銀的學校財金端末機代號參數
        /// </summary>
        [FieldSpec(Field.TerminalId, false, FieldTypeEnum.VarChar, 8, true)]
        public string TerminalId
        {
            get
            {
                return _TerminalId;
            }
            set
            {
                _TerminalId = value == null ? String.Empty : value.Trim();
            }
        }

        private string _MerId = String.Empty;
        /// <summary>
        /// 土銀的學校財金特店編號參數
        /// </summary>
        [FieldSpec(Field.MerId, false, FieldTypeEnum.VarChar, 10, true)]
        public string MerId
        {
            get
            {
                return _MerId;
            }
            set
            {
                _MerId = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion

        private string _BigReceiveIdFlag = "N";
        /// <summary>
        /// 是否使用2位數的費用別代碼旗標 (預設 N，一律轉大寫，且非 Y 值一律轉為 N)
        /// </summary>
        [FieldSpec(Field.BigReceiveIdFlag, false, FieldTypeEnum.Char, 1, false)]
        public string BigReceiveIdFlag
        {
            get
            {
                return _BigReceiveIdFlag;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _BigReceiveIdFlag = "N";
                }
                else
                {
                    _BigReceiveIdFlag = value.Trim().ToUpper();
                    if (_BigReceiveIdFlag != "Y" && _BigReceiveIdFlag != "N")
                    {
                        _BigReceiveIdFlag = "N";
                    }
                }
            }
        }

        private string _OpenStudentArea = "Y";
        /// <summary>
        /// 是否開放學生專區 (Y/N，預設 Y，一律轉大寫，且非 N 值一律轉為 Y)
        /// </summary>
        [FieldSpec(Field.OpenStudentArea, false, FieldTypeEnum.Char, 1, false)]
        public string OpenStudentArea
        {
            get
            {
                return _OpenStudentArea;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _OpenStudentArea = "Y";
                }
                else
                {
                    _OpenStudentArea = value.Trim().ToUpper();
                    if (_OpenStudentArea != "Y" && _OpenStudentArea != "N")
                    {
                        _OpenStudentArea = "Y";
                    }
                }
            }
        }

        private string _ReceiveKind = ReceiveKindCodeTexts.SCHOOL;
        /// <summary>
        /// 代收種類 (1:學雜費 2:代收各項費用，預設 1，請參考 ReceiveKindCodeTexts)
        /// </summary>
        [FieldSpec(Field.ReceiveKind, false, FieldTypeEnum.Char, 1, false)]
        public string ReceiveKind
        {
            get
            {
                return _ReceiveKind;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _ReceiveKind = "";
                }
                else
                {
                    _ReceiveKind = value.Trim().ToUpper();
                }
            }
        }

        #region [MDY:20160201] 流程(階層)種類
        private string _FlowKind = FlowKindCodeTexts.SINGLE;
        /// <summary>
        /// 流程(階層)種類 (S:單階 (免審核) M:多階 (需審核)，預設 S，請參考 FlowKindCodeTexts)
        /// </summary>
        [FieldSpec(Field.FlowKind, false, FieldTypeEnum.Char, 1, false)]
        public string FlowKind
        {
            get
            {
                return _FlowKind;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _FlowKind = "";
                }
                else
                {
                    _FlowKind = value.Trim().ToUpper();
                }
            }
        }
        #endregion

        #region [MDY:20160202] 新增線上資料保留年數、歷史資料保留年數
        /// <summary>
        /// 線上資料保留年數
        /// </summary>
        [FieldSpec(Field.KeepDataYear, false, FieldTypeEnum.Integer, false)]
        public int? KeepDataYear
        {
            get;
            set;
        }

        /// <summary>
        /// 歷史資料保留年數
        /// </summary>
        [FieldSpec(Field.KeepHistoryYear, false, FieldTypeEnum.Integer, false)]
        public int? KeepHistoryYear
        {
            get;
            set;
        }
        #endregion

        #region [MDY:20191214] 2019擴充案 國際信用卡 - 財金特店參數相關欄位
        private string _MerchantId2 = String.Empty;
        /// <summary>
        /// 土銀的國際信用卡-學校財金特店代碼參數
        /// </summary>
        [FieldSpec(Field.MerchantId2, false, FieldTypeEnum.VarChar, 15, true)]
        public string MerchantId2
        {
            get
            {
                return _MerchantId2;
            }
            set
            {
                _MerchantId2 = value == null ? String.Empty : value.Trim();
            }
        }

        private string _TerminalId2 = String.Empty;
        /// <summary>
        /// 土銀的國際信用卡-學校財金端末機代號參數
        /// </summary>
        [FieldSpec(Field.TerminalId2, false, FieldTypeEnum.VarChar, 8, true)]
        public string TerminalId2
        {
            get
            {
                return _TerminalId2;
            }
            set
            {
                _TerminalId2 = value == null ? String.Empty : value.Trim();
            }
        }

        private string _MerId2 = String.Empty;
        /// <summary>
        /// 土銀的國際信用卡-學校財金特店編號參數
        /// </summary>
        [FieldSpec(Field.MerId2, false, FieldTypeEnum.VarChar, 10, true)]
        public string MerId2
        {
            get
            {
                return _MerId2;
            }
            set
            {
                _MerId2 = value == null ? String.Empty : value.Trim();
            }
        }

        private decimal? _HandlingFeeRate = null;
        /// <summary>
        /// 土銀的國際信用卡-學校手續費率參數
        /// </summary>
        [FieldSpec(Field.HandlingFeeRate, false, FieldTypeEnum.Decimal, true)]
        public decimal? HandlingFeeRate
        {
            get
            {
                return _HandlingFeeRate;
            }
            set
            {
                if (value.HasValue && value.Value >= 0)
                {
                    _HandlingFeeRate = value.Value;
                }
                else
                {
                    _HandlingFeeRate = null;
                }
            }
        }
		#endregion

		#region [MDY:202203XX] 2022擴充案 英文資料啟用、學校英文全名 新增欄位並調整學校英文為 NVarChar(140)
		private string _EngEnabled = null;
		/// <summary>
		/// 英文資料啟用 (Y:啟用 N:停用，預設 N，一律轉大寫，且非 Y 值一律轉為 N)
		/// </summary>
		[FieldSpec(Field.EngEnabled, false, FieldTypeEnum.Char, 1, true)]
		public string EngEnabled
		{
			get
			{
				return _EngEnabled;
			}
			set
			{
				if (String.IsNullOrWhiteSpace(value))
				{
					_EngEnabled = "N";
				}
				else
				{
					_EngEnabled = "Y".Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase) ? "Y" : "N";
				}
			}
		}

		/// <summary>
		/// 學校英文全名
		/// </summary>
		[FieldSpec(Field.SchEName, false, FieldTypeEnum.NVarChar, 140, true)]
		public string SchEName
		{
			get;
			set;
		}
		#endregion

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

        #region method
        /// <summary>
        /// 取得是否使用兩位數的費用別代碼
        /// </summary>
        /// <returns></returns>
        public bool IsBigReceiveId()
        {
            return this.BigReceiveIdFlag == "Y";
        }

        /// <summary>
        /// 取得是否開放學生專區
        /// </summary>
        /// <returns></returns>
        public bool IsOpenStudentArea()
        {
            return this.OpenStudentArea == "Y";
        }

        /// <summary>
        /// 取得是否需要審核流程
        /// </summary>
        /// <returns></returns>
        public bool IsNeedFlow()
        {
            return (this.FlowKind == FlowKindCodeTexts.MULTI);
        }

		#region [MDY:202203XX] 2022擴充案 英文資料啟用 欄位
		/// <summary>
		/// 取得是否英文資料啟用
		/// </summary>
		/// <returns></returns>
		public bool IsEngEnabled()
		{
			return this.EngEnabled == "Y";
		}
		#endregion
		#endregion

		#region Static Method
		/// <summary>
		/// 取得學制、類別的文字
		/// </summary>
		/// <param name="CorpType"></param>
		/// <returns></returns>
		public static string GetCorpTypeText(string CorpType)
        {
            string CorpTypeText = "";
            if(CorpType=="1")
            {
                CorpTypeText = "大專院校";
            }
            else if (CorpType == "2")
            {
                CorpTypeText = "高中職";
            }
            else if (CorpType == "3")
            {
                CorpTypeText = "國中小";
            }
            else if (CorpType == "4")
            {
                CorpTypeText = "幼兒園";
            }
            return CorpTypeText;
        }
        #endregion
    }
}
