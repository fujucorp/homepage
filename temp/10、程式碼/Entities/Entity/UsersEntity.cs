/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:31:50
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
	/// Users 資料表 Entity 類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class UsersEntity : Entity
	{
		public const string TABLE_NAME = "Users";

		#region Field Name Const Class
		/// <summary>
		/// UsersEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
            /// 使用者帳號 (必須同一單位唯一)
			/// </summary>
			public const string UId = "U_ID";

			/// <summary>
            /// 授權的商家代號 (學校主管：星號 "*"，學校經辦：商家代號使用逗號分隔，行員：空白)
			/// </summary>
			public const string URt = "U_RT";

			/// <summary>
            /// 使用者所屬群組
			/// </summary>
			public const string UGrp = "U_Grp";

			/// <summary>
            /// 使用者所屬單位 (學校：學校代碼，行員：分行代碼 6 碼)
			/// </summary>
			public const string UBank = "U_Bank";
			#endregion

			#region Data
			/// <summary>
			/// U_Name 欄位名稱常數定義
			/// </summary>
			public const string UName = "U_Name";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// U_Pwd 欄位名稱常數定義
			/// </summary>
			public const string UPXX = "U_Pwd";
			#endregion
			#endregion

			/// <summary>
			/// Creator 欄位名稱常數定義
			/// </summary>
			public const string Creator = "Creator";

			/// <summary>
			/// Approver 欄位名稱常數定義
			/// </summary>
			public const string Approver = "Approver";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// Pwd_ChangeDate 欄位名稱常數定義
			/// </summary>
			public const string PXXChangeDate = "Pwd_ChangeDate";
			#endregion
			#endregion

			/// <summary>
			/// U_Lock 欄位名稱常數定義
			/// </summary>
			public const string ULock = "U_Lock";

			/// <summary>
			/// U_num 欄位名稱常數定義
			/// </summary>
			public const string UNum = "U_num";

			/// <summary>
			/// Title 欄位名稱常數定義
			/// </summary>
			public const string Title = "Title";

			/// <summary>
			/// Tel 欄位名稱常數定義
			/// </summary>
			public const string Tel = "Tel";

			/// <summary>
			/// EMail 欄位名稱常數定義
			/// </summary>
			public const string Email = "EMail";

			#region [MDY:20220530] Checkmarx 調整
			/// <summary>
			/// U_Pwd1 欄位名稱常數定義
			/// </summary>
			public const string UPXX1 = "U_Pwd1";
			#endregion

			/// <summary>
			/// sessionid 欄位名稱常數定義
			/// </summary>
			public const string Sessionid = "sessionid";

			/// <summary>
			/// CreateDate 欄位名稱常數定義
			/// </summary>
			public const string Createdate = "CreateDate";

			/// <summary>
			/// ApproveDate 欄位名稱常數定義
			/// </summary>
			public const string Approvedate = "ApproveDate";

			/// <summary>
			/// Reset_UID 欄位名稱常數定義
			/// </summary>
			public const string ResetUid = "Reset_UID";

			/// <summary>
			/// ResetDate 欄位名稱常數定義
			/// </summary>
			public const string Resetdate = "ResetDate";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// ResetPWD 欄位名稱常數定義
			/// </summary>
			public const string ResetPXX = "ResetPWD";
			#endregion
			#endregion

			/// <summary>
			/// data_status 欄位名稱常數定義
			/// </summary>
			public const string DataStatus = "data_status";

			/// <summary>
			/// process_status 欄位名稱常數定義
			/// </summary>
			public const string ProcessStatus = "process_status";

			/// <summary>
			/// remark 欄位名稱常數定義
			/// </summary>
			public const string Remark = "remark";

			/// <summary>
			/// last_modify_user 欄位名稱常數定義
			/// </summary>
			public const string LastModifyUser = "last_modify_user";

			/// <summary>
			/// last_modify_time 欄位名稱常數定義
			/// </summary>
			public const string LastModifyTime = "last_modify_time";

			/// <summary>
			/// last_approve_user 欄位名稱常數定義
			/// </summary>
			public const string LastApproveUser = "last_approve_user";

			/// <summary>
			/// last_approve_time 欄位名稱常數定義
			/// </summary>
			public const string LastApproveTime = "last_approve_time";

			/// <summary>
			/// flag 欄位名稱常數定義
			/// </summary>
			public const string Flag = "flag";

			#region [MDY:20220530] Checkmarx 調整
			#region [MDY:20210401] 原碼修正
			/// <summary>
			/// init_pwd 欄位名稱常數定義
			/// </summary>
			public const string InitPXX = "init_pwd";
			#endregion
			#endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// UsersEntity 類別建構式
		/// </summary>
		public UsersEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _UId = null;
		/// <summary>
        /// 使用者帳號 (必須同一單位唯一)
		/// </summary>
		[FieldSpec(Field.UId, true, FieldTypeEnum.VarChar, 20, false)]
		public string UId
		{
			get
			{
				return _UId;
			}
			set
			{
				_UId = value == null ? null : value.Trim();
			}
		}

		private string _URt = null;
		/// <summary>
        /// 授權的商家代號 (學校主管：星號 "*"，學校經辦：商家代號使用逗號分隔，行員：空白)
		/// </summary>
		[FieldSpec(Field.URt, true, FieldTypeEnum.VarChar, 100, false)]
		public string URt
		{
			get
			{
				return _URt;
			}
			set
			{
				_URt = value == null ? null : value.Trim();
			}
		}

		private string _UGrp = null;
		/// <summary>
        /// 使用者所屬群組
		/// </summary>
		[FieldSpec(Field.UGrp, true, FieldTypeEnum.VarChar, 8, false)]
		public string UGrp
		{
			get
			{
				return _UGrp;
			}
			set
			{
				_UGrp = value == null ? null : value.Trim();
			}
		}

		private string _UBank = null;
		/// <summary>
        /// 使用者所屬單位 (學校：學校代碼，行員：分行代碼 6 碼)
		/// </summary>
		[FieldSpec(Field.UBank, true, FieldTypeEnum.VarChar, 10, false)]
		public string UBank
		{
			get
			{
				return _UBank;
			}
			set
			{
				_UBank = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// U_Name 使用者名稱
		/// </summary>
		[FieldSpec(Field.UName, false, FieldTypeEnum.VarChar, 32, true)]
		public string UName
		{
			get;
			set;
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		/// <summary>
		/// U_Pwd 使用者密碼
		/// </summary>
		[FieldSpec(Field.UPXX, false, FieldTypeEnum.VarChar, 32, false)]
		public string UPXX
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// Creator 帳號建立人員
		/// </summary>
		[FieldSpec(Field.Creator, false, FieldTypeEnum.VarChar, 32, true)]
		public string Creator
		{
			get;
			set;
		}

		/// <summary>
		/// Approver 帳號覆核人員
		/// </summary>
		[FieldSpec(Field.Approver, false, FieldTypeEnum.VarChar, 32, true)]
		public string Approver
		{
			get;
			set;
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		/// <summary>
		/// Pwd_ChangeDate 最後一次密碼變更日期
		/// </summary>
		[FieldSpec(Field.PXXChangeDate, false, FieldTypeEnum.Char, 8, true)]
		public string PXXChangeDate
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// U_Lock 帳號是否鎖住 (Y/N)
		/// </summary>
		[FieldSpec(Field.ULock, false, FieldTypeEnum.Char, 1, true)]
		public string ULock
		{
			get;
			set;
		}

		/// <summary>
		/// U_num 帳號輸錯密碼次數
		/// </summary>
		[FieldSpec(Field.UNum, false, FieldTypeEnum.Char, 1, true)]
		public string UNum
		{
			get;
			set;
		}

		/// <summary>
		/// Title 職稱
		/// </summary>
		[FieldSpec(Field.Title, false, FieldTypeEnum.VarChar, 50, false)]
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Tel 電話
		/// </summary>
		[FieldSpec(Field.Tel, false, FieldTypeEnum.VarChar, 20, false)]
		public string Tel
		{
			get;
			set;
		}

		/// <summary>
		/// EMail 電子郵件
		/// </summary>
		[FieldSpec(Field.Email, false, FieldTypeEnum.VarChar, 64, false)]
		public string Email
		{
			get;
			set;
		}

		#region [MDY:20220530] Checkmarx 調整
		/// <summary>
		/// U_Pwd1 上一次密碼
		/// </summary>
		[FieldSpec(Field.UPXX1, false, FieldTypeEnum.VarChar, 32, true)]
		public string UPXX1
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// sessionid session值(沒用)
		/// </summary>
		[FieldSpec(Field.Sessionid, false, FieldTypeEnum.VarChar, 128, true)]
		public string Sessionid
		{
			get;
			set;
		}

		/// <summary>
		/// CreateDate 建立日期
		/// </summary>
		[FieldSpec(Field.Createdate, false, FieldTypeEnum.VarChar, 8, true)]
		public string Createdate
		{
			get;
			set;
		}

		/// <summary>
		/// ApproveDate 覆核日期
		/// </summary>
		[FieldSpec(Field.Approvedate, false, FieldTypeEnum.Char, 8, true)]
		public string Approvedate
		{
			get;
			set;
		}

		/// <summary>
		/// Reset_UID 密碼重置人員
		/// </summary>
		[FieldSpec(Field.ResetUid, false, FieldTypeEnum.VarChar, 32, true)]
		public string ResetUid
		{
			get;
			set;
		}

		/// <summary>
		/// ResetDate 密碼重置日期
		/// </summary>
		[FieldSpec(Field.Resetdate, false, FieldTypeEnum.Char, 8, true)]
		public string Resetdate
		{
			get;
			set;
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		/// <summary>
		/// ResetPWD 重置密碼
		/// </summary>
		[FieldSpec(Field.ResetPXX, false, FieldTypeEnum.VarChar, 32, true)]
		public string ResetPXX
		{
			get;
			set;
		}
		#endregion
		#endregion

		/// <summary>
		/// data_status 資料狀態
		/// </summary>
		[FieldSpec(Field.DataStatus, false, FieldTypeEnum.Char, 1, false)]
		public string DataStatus
		{
			get;
			set;
		}

		/// <summary>
		/// process_status 處理狀態
		/// </summary>
		[FieldSpec(Field.ProcessStatus, false, FieldTypeEnum.Char, 1, false)]
		public string ProcessStatus
		{
			get;
			set;
		}

		/// <summary>
		/// remark 備註
		/// </summary>
		[FieldSpec(Field.Remark, false, FieldTypeEnum.VarChar, 200, false)]
		public string Remark
		{
			get;
			set;
		}

		/// <summary>
		/// last_modify_user 最後一次修改人員
		/// </summary>
		[FieldSpec(Field.LastModifyUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string LastModifyUser
		{
			get;
			set;
		}

		/// <summary>
		/// last_modify_time 最後一次修改時間
		/// </summary>
		[FieldSpec(Field.LastModifyTime, false, FieldTypeEnum.DateTime, true)]
		public DateTime? LastModifyTime
		{
			get;
			set;
		}

		/// <summary>
		/// last_approve_user 最後一次覆核人員
		/// </summary>
		[FieldSpec(Field.LastApproveUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string LastApproveUser
		{
			get;
			set;
		}

		/// <summary>
		/// last_approve_time 最後一次覆核時間
		/// </summary>
		[FieldSpec(Field.LastApproveTime, false, FieldTypeEnum.DateTime, true)]
		public DateTime? LastApproveTime
		{
			get;
			set;
		}

		/// <summary>
		/// flag 欄位屬性
		/// </summary>
		[FieldSpec(Field.Flag, false, FieldTypeEnum.Char, 1, false)]
		public string Flag
		{
			get;
			set;
		}

		#region [MDY:20220530] Checkmarx 調整
		#region [MDY:20210401] 原碼修正
		/// <summary>
		/// init_pwd 重置初始密碼
		/// </summary>
		[FieldSpec(Field.InitPXX, false, FieldTypeEnum.VarChar, 32, false)]
		public string InitPXX
		{
			get;
			set;
		}
		#endregion
		#endregion
		#endregion
		#endregion

		#region Method
		/// <summary>
		/// 取得登入錯誤次數
		/// </summary>
		/// <returns></returns>
		public int GetWrongTimes()
        {
            int value = 0;
            if (int.TryParse(this.UNum, out value) && value >= 0)
            {
                return value;
            }
            return 0;
        }

		#region [MDY:20220530] Checkmarx 調整
		/// <summary>
		/// 取得上次變更密碼的日期
		/// </summary>
		/// <returns>有修改則傳回日期，否則傳回 null。</returns>
		public DateTime? GetPXXChangeDate()
		{
			DateTime date;
			string dateTxt = this.PXXChangeDate == null ? null : this.PXXChangeDate.Trim();
			if (!String.IsNullOrEmpty(dateTxt) && Fuju.Common.TryConvertTWDate7(dateTxt, out date))
			{
				return date;
			}
			return null;
		}
		#endregion

		/// <summary>
		/// 取得資料建立日期
		/// </summary>
		/// <returns>有則傳回日期時間，否則傳回 null。</returns>
		public DateTime? GetCreateDate()
        {
            DateTime date;
            string dateTxt = this.Createdate == null ? null : this.Createdate.Trim();
            if (!String.IsNullOrEmpty(dateTxt) && Fuju.Common.TryConvertTWDate7(dateTxt, out date))
            {
                return date;
            }
            return null;
        }

        /// <summary>
        /// 取得密碼重置日期
        /// </summary>
        /// <returns>有則傳回日期時間，否則傳回 null。</returns>
        public DateTime? GetRestDate()
        {
            DateTime date;
            string dateTxt = this.Resetdate == null ? null : this.Resetdate.Trim();
            if (!String.IsNullOrEmpty(dateTxt) && Fuju.Common.TryConvertTWDate7(dateTxt, out date))
            {
                return date;
            }
            return null;
        }

        public bool IsLocked()
        {
            return (this.ULock == "Y");
        }

        public bool IsBank()
        {
            #region [MDY:20160814] 修正分行代碼判斷 (因為總行類的分行第6碼為英文)
            #region [Old]
            //if (String.IsNullOrEmpty(this.URt)
            //    && Fuju.Common.IsNumber(this.UBank, 6) && this.UBank.StartsWith(DataFormat.MyBankID))
            //{
            //    //沒有授權商家代號，且所屬單位代碼為6碼數字且005開頭
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            #endregion

            if (String.IsNullOrEmpty(this.URt) && DataFormat.IsMyBankCode(this.UBank))
            {
                //沒有授權商家代號，且所屬單位代碼為6碼數字且005開頭
                return true;
            }
            else
            {
                return false;
            }
            #endregion
        }
        #endregion
	}
}
