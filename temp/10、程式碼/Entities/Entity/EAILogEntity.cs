/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2016/03/08 13:44:50
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
	/// 電文日誌記錄檔
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class EAILogEntity : Entity
	{
        #region Static Readonly
        public static readonly string KIND_CHANGEKEY = "CHGKEY";
        public static readonly string KIND_D3800 = "D3800";
        public static readonly string KIND_D0070 = "D0070";
        public static readonly string KIND_D0071 = "D0071";
        #endregion

		public const string TABLE_NAME = "EAI_Log";

		#region Field Name Const Class
		/// <summary>
		/// EAILogEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 流水號 (Identity)
			/// </summary>
			public const string SN = "sn";
			#endregion

			#region Data
			/// <summary>
			/// 電文類別代碼
			/// </summary>
			public const string Kind = "Kind";

			/// <summary>
			/// 電文的 MsgId
			/// </summary>
			public const string MsgId = "Msg_Id";

			/// <summary>
			/// 電文的 RqUID
			/// </summary>
			public const string RqUID = "Rq_UID";

			/// <summary>
			/// 發送電文的內容
			/// </summary>
			public const string RqXml = "Rq_Xml";

			/// <summary>
			/// 接收電文的內容
			/// </summary>
			public const string RsXml = "Rs_Xml";

			/// <summary>
			/// 接收電文的 StatusCode
			/// </summary>
			public const string RsStatusCode = "Rs_Status_Code";

			/// <summary>
			/// 接收電文的 StatusDesc
			/// </summary>
			public const string RsStatusDesc = "Rs_Status_Desc";

			/// <summary>
			/// 伺服器名稱
			/// </summary>
			public const string MachineName = "Machine_Name";

			/// <summary>
			/// 發送/接收時間
			/// </summary>
			public const string SendTime = "Send_Time";

			/// <summary>
			/// 發送/接收結果
			/// </summary>
			public const string SendResult = "Send_Result";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// EAILogEntity 類別建構式
		/// </summary>
		public EAILogEntity()
			: base()
		{
			this.MachineName = Environment.MachineName;
		}

		/// <summary>
		/// EAILogEntity 類別建構式
		/// </summary>
		public EAILogEntity(string kind, DateTime sendTime)
			: base()
		{
			this.Kind = kind;
			this.SendTime = sendTime;
			this.MachineName = Environment.MachineName;
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 流水號 (Identity)
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.Identity, false)]
		public Int64 SN
		{
			get;
			set;
		}
		#endregion

		#region Data
		private string _Kind = null;
		/// <summary>
		/// 電文類別代碼
		/// </summary>
		[FieldSpec(Field.Kind, false, FieldTypeEnum.VarChar, 20, false)]
		public string Kind
		{
			get
			{
				return _Kind;
			}
			set
			{
				_Kind = value == null ? null : value.Trim();
			}
		}

		private string _MsgId = String.Empty;
		/// <summary>
		/// 電文的 MsgId
		/// </summary>
		[FieldSpec(Field.MsgId, false, FieldTypeEnum.VarChar, 20, true)]
		public string MsgId
		{
			get
			{
				return _MsgId;
			}
			set
			{
				_MsgId = value == null ? String.Empty : value.Trim();
			}
		}

		private string _RqUID = String.Empty;
		/// <summary>
		/// 電文的 RqUID
		/// </summary>
		[FieldSpec(Field.RqUID, false, FieldTypeEnum.VarChar, 40, true)]
		public string RqUID
		{
			get
			{
				return _RqUID;
			}
			set
			{
				_RqUID = value == null ? String.Empty : value.Trim();
			}
		}

		private string _RqXml = String.Empty;
		/// <summary>
		/// 發送電文的內容
		/// </summary>
		[FieldSpec(Field.RqXml, false, FieldTypeEnum.NVarChar, 2000, true)]
		public string RqXml
		{
			get
			{
				return _RqXml;
			}
			set
			{
				_RqXml = value == null ? String.Empty : value.Trim();
			}
		}

		private string _RsXml = String.Empty;
		/// <summary>
		/// 接收電文的內容
		/// </summary>
		[FieldSpec(Field.RsXml, false, FieldTypeEnum.NVarCharMax, true)]
		public string RsXml
		{
			get
			{
				return _RsXml;
			}
			set
			{
				_RsXml = value == null ? String.Empty : value.Trim();
			}
		}

		private string _RsStatusCode = String.Empty;
		/// <summary>
		/// 接收電文的 StatusCode
		/// </summary>
		[FieldSpec(Field.RsStatusCode, false, FieldTypeEnum.VarChar, 20, true)]
		public string RsStatusCode
		{
			get
			{
				return _RsStatusCode;
			}
			set
			{
				_RsStatusCode = value == null ? String.Empty : value.Trim();
			}
		}

		private string _RsStatusDesc = String.Empty;
		/// <summary>
		/// 接收電文的 StatusDesc
		/// </summary>
		[FieldSpec(Field.RsStatusDesc, false, FieldTypeEnum.NVarChar, 100, true)]
		public string RsStatusDesc
		{
			get
			{
				return _RsStatusDesc;
			}
			set
			{
				_RsStatusDesc = value == null ? String.Empty : value.Trim();
			}
		}

		private string _MachineName = String.Empty;
		/// <summary>
		/// 伺服器名稱
		/// </summary>
		[FieldSpec(Field.MachineName, false, FieldTypeEnum.NVarChar, 32, true)]
		public string MachineName
		{
			get
			{
				return _MachineName;
			}
			set
			{
				_MachineName = value == null ? String.Empty : value.Trim();
			}
		}

		/// <summary>
		/// 發送/接收時間
		/// </summary>
		[FieldSpec(Field.SendTime, false, FieldTypeEnum.DateTime, false)]
		public DateTime SendTime
		{
			get;
			set;
		}

		private string _SendResult = String.Empty;
		/// <summary>
		/// 發送/接收結果
		/// </summary>
		[FieldSpec(Field.SendResult, false, FieldTypeEnum.NVarChar, 200, false)]
		public string SendResult
		{
			get
			{
				return _SendResult;
			}
			set
			{
				_SendResult = value == null ? String.Empty : value.Trim();
			}
		}
		#endregion
		#endregion

		#region Readonly Property
        [XmlIgnore]
        public string KindText
        {
            get
            {
                return this.GetKindText(this.Kind);
            }
        }
		#endregion

        #region static Method
        private static readonly Fuju.CodeTextList _KindList = InitKindList();

        private static Fuju.CodeTextList InitKindList()
        {
            Fuju.CodeTextList list = new Fuju.CodeTextList();
            list.Add(KIND_CHANGEKEY, "EAI 換 KEY");
            list.Add(KIND_D3800, "上傳中心異動檔");
            list.Add(KIND_D0070, "下載虛擬帳號交易明細");

            #region [MDY:20161124] 增加 D0071 選項
            list.Add(KIND_D0071, "虛擬帳號轉存帳號明細查詢");
            #endregion

            return list;
        }

        public static Fuju.CodeTextList GetAllKindList()
        {
            return _KindList.Copy();
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得電文類別文字
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public string GetKindText(string kind)
        {
            int index = _KindList.CodeIndexOf(kind);
            if (index > -1)
            {
                return _KindList[index].Text;
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion
	}
}
