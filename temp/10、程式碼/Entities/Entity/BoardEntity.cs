/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/04/14 08:13:40
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
	/// 公告(最新消息) 資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class BoardEntity : Entity
	{
		public const string TABLE_NAME = "Board";

		#region Field Name Const Class
		/// <summary>
		/// BoardEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 公告流水號 (Identity)
			/// </summary>
			public const string BoardId = "Board_Id";
			#endregion

			#region Data
			/// <summary>
            /// 公告對象的學校代碼 改放 公告位置
            /// 1:首頁 2:學校專區 3:學生專區 4:銀行專區 5:信用卡繳費 6:銀聯卡繳費 7:查詢繳費狀態 8:查詢列印繳費單 9:列印收據
			/// </summary>
			public const string SchId = "Sch_Id";

			/// <summary>
            /// 公告對象的代收類別 (保留) [MDY:2018xxxx] 改放公告對象學校代碼
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 公告單位 (保留)
			/// </summary>
			public const string BoardDep = "Board_Dep";

			/// <summary>
            /// 公告內容型態 (1=純文字 2=Html) (土銀只用 2)
			/// </summary>
			public const string BoardType = "Board_Type";

			/// <summary>
			/// 公告張貼人 (保留)
			/// </summary>
			public const string BoardUser = "Board_User";

			/// <summary>
			/// 公告主旨
			/// </summary>
			public const string BoardSubject = "Board_Subject";

			/// <summary>
			/// 聯絡電話 (保留)
			/// </summary>
			public const string BoardTel = "Board_Tel";

			/// <summary>
			/// 聯絡信項 (保留)
			/// </summary>
			public const string BoardEmail = "Board_Email";

			/// <summary>
			/// 公告開始日
			/// </summary>
			public const string StartDate = "Start_Date";

			/// <summary>
			/// 公告結束日
			/// </summary>
			public const string EndDate = "End_Date";

			/// <summary>
			/// 公告內容
			/// </summary>
			public const string BoardContent = "Board_Content";

			/// <summary>
			/// 連結網址
			/// </summary>
			public const string BoardUrl = "Board_Url";

			/// <summary>
			/// 建立者
			/// </summary>
			public const string UId = "U_ID";

			/// <summary>
			/// 最後更新者
			/// </summary>
			public const string UpdateUId = "Update_U_ID";

			/// <summary>
			/// 建立日期
			/// </summary>
			public const string AddDate = "Add_Date";

			/// <summary>
			/// 最後更新日期
			/// </summary>
			public const string UpdateDate = "Update_Date";

			#region [MDY:2018xxxx] 增加分享旗標
			/// <summary>
			/// 分享旗標
			/// </summary>
			public const string ShareFlag = "Share_Flag";
			#endregion
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// BoardEntity 類別建構式
		/// </summary>
		public BoardEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 公告流水號 (Identity)
		/// </summary>
		[FieldSpec(Field.BoardId, true, FieldTypeEnum.Identity, false)]
		public int BoardId
		{
			get;
			set;
		}
		#endregion

		#region Data
		/// <summary>
        /// 公告對象的學校代碼 改放 公告位置
        /// 1:首頁 2:學校專區 3:學生專區 4:銀行專區 5:信用卡繳費 6:銀聯卡繳費 7:查詢繳費狀態 8:查詢列印繳費單 9:列印收據
		/// </summary>
		[FieldSpec(Field.SchId, false, FieldTypeEnum.Char, 6, true)]
		public string SchId
		{
			get;
			set;
		}

		/// <summary>
        /// 公告對象的代收類別 (保留) [MDY:2018xxxx] 改放公告對象學校代碼
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
		public string ReceiveType
		{
			get;
			set;
		}

		/// <summary>
		/// 公告單位 (保留)
		/// </summary>
		[FieldSpec(Field.BoardDep, false, FieldTypeEnum.VarChar, 30, true)]
		public string BoardDep
		{
			get;
			set;
		}

		/// <summary>
		/// 公告內容型態 (1=純文字 2=Html)
		/// </summary>
		[FieldSpec(Field.BoardType, false, FieldTypeEnum.Char, 1, true)]
		public string BoardType
		{
			get;
			set;
		}

		/// <summary>
		/// 公告張貼人 (保留)
		/// </summary>
		[FieldSpec(Field.BoardUser, false, FieldTypeEnum.VarChar, 20, true)]
		public string BoardUser
		{
			get;
			set;
		}

		/// <summary>
		/// 公告主旨
		/// </summary>
		[FieldSpec(Field.BoardSubject, false, FieldTypeEnum.VarChar, 300, true)]
		public string BoardSubject
		{
			get;
			set;
		}

		/// <summary>
		/// 聯絡電話 (保留)
		/// </summary>
		[FieldSpec(Field.BoardTel, false, FieldTypeEnum.VarChar, 14, true)]
		public string BoardTel
		{
			get;
			set;
		}

		/// <summary>
		/// 聯絡信項 (保留)
		/// </summary>
		[FieldSpec(Field.BoardEmail, false, FieldTypeEnum.VarChar, 50, true)]
		public string BoardEmail
		{
			get;
			set;
		}

		/// <summary>
		/// 公告開始日
		/// </summary>
		[FieldSpec(Field.StartDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? StartDate
		{
			get;
			set;
		}

		/// <summary>
		/// 公告結束日
		/// </summary>
		[FieldSpec(Field.EndDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? EndDate
		{
			get;
			set;
		}

		/// <summary>
		/// 公告內容
		/// </summary>
		[FieldSpec(Field.BoardContent, false, FieldTypeEnum.NVarCharMax, true)]
		public string BoardContent
		{
			get;
			set;
		}

		/// <summary>
		/// 連結網址
		/// </summary>
		[FieldSpec(Field.BoardUrl, false, FieldTypeEnum.VarChar, 50, true)]
		public string BoardUrl
		{
			get;
			set;
		}

		/// <summary>
		/// 建立者
		/// </summary>
		[FieldSpec(Field.UId, false, FieldTypeEnum.VarChar, 20, true)]
		public string UId
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新者
		/// </summary>
		[FieldSpec(Field.UpdateUId, false, FieldTypeEnum.VarChar, 20, true)]
		public string UpdateUId
		{
			get;
			set;
		}

		/// <summary>
		/// 建立日期
		/// </summary>
		[FieldSpec(Field.AddDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? AddDate
		{
			get;
			set;
		}

		/// <summary>
		/// 最後更新日期
		/// </summary>
		[FieldSpec(Field.UpdateDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? UpdateDate
		{
			get;
			set;
		}

		#region [MDY:2018xxxx] 增加分享旗標
		/// <summary>
		/// 分享旗標
		/// </summary>
		[FieldSpec(Field.ShareFlag, false, FieldTypeEnum.VarChar, 3, true)]
		public string ShareFlag
		{
			get;
			set;
		}
		#endregion
		#endregion
		#endregion
	}
}
