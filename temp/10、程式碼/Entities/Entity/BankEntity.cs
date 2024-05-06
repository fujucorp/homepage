/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/15 17:14:21
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
	/// 銀行資料
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class BankEntity : Entity
	{
		public const string TABLE_NAME = "BANK";

		#region Field Name Const Class
		/// <summary>
		/// BankEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 銀行代碼
			/// </summary>
			public const string BankNo = "BANKNO";
			#endregion

			#region Data
			/// <summary>
			/// 銀行簡稱
			/// </summary>
			public const string BankSName = "BANKSNAME";

			/// <summary>
			/// 銀行名稱
			/// </summary>
			public const string BankFName = "BANKFNAME";

			/// <summary>
			/// 分行旗標
			/// </summary>
			public const string Branch = "Branch";

            /// <summary>
            /// 銀行電話
            /// </summary>
            public const string Tel = "Tel";

            /// <summary>
            /// 銀行金融代號 (7碼)
            /// </summary>
            public const string FullCode = "FULL_CODE";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// BankEntity 類別建構式
		/// </summary>
		public BankEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		private string _BankNo = null;
		/// <summary>
		/// 銀行代碼
		/// </summary>
		[FieldSpec(Field.BankNo, true, FieldTypeEnum.NVarChar, 7, false)]
		public string BankNo
		{
			get
			{
				return _BankNo;
			}
			set
			{
				_BankNo = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region Data
		/// <summary>
		/// 銀行簡稱
		/// </summary>
		[FieldSpec(Field.BankSName, false, FieldTypeEnum.NVarChar, 10, true)]
		public string BankSName
		{
			get;
			set;
		}

		/// <summary>
		/// 銀行名稱
		/// </summary>
		[FieldSpec(Field.BankFName, false, FieldTypeEnum.NVarChar, 34, true)]
		public string BankFName
		{
			get;
			set;
		}

		/// <summary>
		/// 分行旗標
		/// </summary>
		[FieldSpec(Field.Branch, false, FieldTypeEnum.Char, 1, true)]
		public string Branch
		{
			get;
			set;
		}

        /// <summary>
        /// 銀行電話
        /// </summary>
        [FieldSpec(Field.Tel, false, FieldTypeEnum.VarChar, 20, true)]
        public string Tel
        {
            get;
            set;
        }

        /// <summary>
        /// 銀行金融代號 (7碼)
        /// </summary>
        [FieldSpec(Field.FullCode, false, FieldTypeEnum.VarChar, 7, true)]
        public string FullCode
        {
            get;
            set;
        }
		#endregion
		#endregion
	}
}
