/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2021/11/06 16:23:53
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
	/// 報送的KP3檔案資訊日誌
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class KP3RenderLogEntity : Entity
	{
		public const string TABLE_NAME = "KP3_Render_Log";

		#region Field Name Const Class
		/// <summary>
		/// Kp3RenderLogEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 序號。PKEY 16碼（3碼民國年+1碼Hex月+2碼日+2碼時+2碼分+5碼Hex(秒x100000+毫秒)）
			/// </summary>
			public const string SN = "SN";
			#endregion

			#region Data
			/// <summary>
			/// 報送日 (3碼民國年 YYYMMDD)
			/// </summary>
			public const string RenderDate = "Render_Date";

			/// <summary>
			/// 報送日檔案序號 (1 ～ 10+26)
			/// </summary>
			public const string RenderBatchNo = "Render_Batch_No";

			/// <summary>
			/// 報送檔案名稱
			/// </summary>
			public const string RenderFileName = "Render_File_Name";

			/// <summary>
			/// 報送檔案內容
			/// </summary>
			public const string RenderFileContent = "Render_File_Content";

			/// <summary>
			/// 報送處理狀態
			/// </summary>
			public const string Status = "Status";

			/// <summary>
			/// 上傳檔案日期時間
			/// </summary>
			public const string UploadDate = "Upload_Date";

			/// <summary>
			/// 上傳檔案處理狀態 (0=待處理; 1=處理中 2=成功; 3=失敗)
			/// </summary>
			public const string UploadStatus = "Upload_Status";

			/// <summary>
			/// 上傳檔案處理結果
			/// </summary>
			public const string UploadResult = "Upload_Result";

			/// <summary>
			/// 回饋檔案日期時間
			/// </summary>
			public const string FeedbackDate = "Feedback_Date";

			/// <summary>
			/// 回饋檔案名稱
			/// </summary>
			public const string FeedbackFileName = "Feedback_File_Name";

			/// <summary>
			/// 回饋檔案內容
			/// </summary>
			public const string FeedbackFileContent = "Feedback_File_Content";

			/// <summary>
			/// 回饋檔案處理狀態 (0=待處理; 1=處理中 2=成功; 3=失敗)
			/// </summary>
			public const string FeedbackStatus = "Feedback_Status";

			/// <summary>
			/// 回饋檔案處理結果
			/// </summary>
			public const string FeedbackResult = "Feedback_Result";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Kp3RenderLogEntity 類別建構式
		/// </summary>
		public KP3RenderLogEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _SN = null;
		/// <summary>
		/// 序號。PKEY 16碼（3碼民國年+1碼Hex月+2碼日+2碼時+2碼分+5碼Hex(秒x100000+毫秒)）
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.VarChar, 16, false)]
		public string SN
		{
			get
			{
				return _SN;
			}
			set
			{
				_SN = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// 報送日 (3碼民國年 YYYMMDD)
		/// </summary>
		[FieldSpec(Field.RenderDate, false, FieldTypeEnum.VarChar, 7, false)]
		public string RenderDate
		{
			get;
			set;
		}

		/// <summary>
		/// 報送日檔案序號 (1 ～ 10+26)
		/// </summary>
		[FieldSpec(Field.RenderBatchNo, false, FieldTypeEnum.Integer, false)]
		public int RenderBatchNo
		{
			get;
			set;
		}

		/// <summary>
		/// 報送檔案名稱
		/// </summary>
		[FieldSpec(Field.RenderFileName, false, FieldTypeEnum.VarChar, 20, false)]
		public string RenderFileName
		{
			get;
			set;
		}

		/// <summary>
		/// 報送檔案內容
		/// </summary>
		[FieldSpec(Field.RenderFileContent, false, FieldTypeEnum.NVarCharMax, false)]
		public string RenderFileContent
		{
			get;
			set;
		}

		/// <summary>
		/// 報送處理狀態
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 5, false)]
		public string Status
		{
			get;
			set;
		}

		/// <summary>
		/// 上傳檔案日期時間
		/// </summary>
		[FieldSpec(Field.UploadDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? UploadDate
		{
			get;
			set;
		}

		/// <summary>
		/// 上傳檔案處理狀態 (0=待處理; 1=處理中 2=成功; 3=失敗)
		/// </summary>
		[FieldSpec(Field.UploadStatus, false, FieldTypeEnum.VarChar, 1, false)]
		public string UploadStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 上傳檔案處理結果
		/// </summary>
		[FieldSpec(Field.UploadResult, false, FieldTypeEnum.NVarChar, 100, true)]
		public string UploadResult
		{
			get;
			set;
		}

		/// <summary>
		/// 回饋檔案日期時間
		/// </summary>
		[FieldSpec(Field.FeedbackDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? FeedbackDate
		{
			get;
			set;
		}

		/// <summary>
		/// 回饋檔案名稱
		/// </summary>
		[FieldSpec(Field.FeedbackFileName, false, FieldTypeEnum.VarChar, 20, false)]
		public string FeedbackFileName
		{
			get;
			set;
		}

		/// <summary>
		/// 回饋檔案內容
		/// </summary>
		[FieldSpec(Field.FeedbackFileContent, false, FieldTypeEnum.NVarCharMax, true)]
		public string FeedbackFileContent
		{
			get;
			set;
		}

		/// <summary>
		/// 回饋檔案處理狀態 (0=待處理; 1=處理中 2=成功; 3=失敗)
		/// </summary>
		[FieldSpec(Field.FeedbackStatus, false, FieldTypeEnum.VarChar, 1, false)]
		public string FeedbackStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 回饋檔案處理結果
		/// </summary>
		[FieldSpec(Field.FeedbackResult, false, FieldTypeEnum.NVarChar, 100, true)]
		public string FeedbackResult
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
