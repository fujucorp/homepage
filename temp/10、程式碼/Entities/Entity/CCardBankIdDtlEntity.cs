/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/09 12:22:09
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
    /// 信用卡繳費銀行清單 資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class CCardBankIdDtlEntity : Entity
    {
        public const string TABLE_NAME = "CCardBankIdDtl";

        #region Field Name Const Class
        /// <summary>
        /// 信用卡繳費銀行清單 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 銀行代碼
            /// </summary>
            public const string BankId = "bank_Id";
            #endregion

            #region Data
            /// <summary>
            /// 繳費平台代碼 (1=財金 [EZPOS]; 2=e 政府[eGov]; 3=中國信託[CTCB] ，請參考 CCardApCodeTexts)
            /// </summary>
            public const string ApNo = "AP_NO";

            /// <summary>
            /// 銀行名稱
            /// </summary>
            public const string BankName = "bank_Name";

            /// <summary>
            /// 建立日期時間
            /// </summary>
            public const string CreateDate = "Create_Date";

            /// <summary>
            /// 最後更新日期時間
            /// </summary>
            public const string UpdateDate = "Update_Date";

            /// <summary>
            /// 最後更新者(或建立者)
            /// </summary>
            public const string UpdateWho = "Update_Who";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// CCardBankIdDtlEntity 類別建構式
        /// </summary>
        public CCardBankIdDtlEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _BankId = null;
        /// <summary>
        /// 銀行代碼
        /// </summary>
        [FieldSpec(Field.BankId, true, FieldTypeEnum.VarChar, 5, false)]
        public string BankId
        {
            get
            {
                return _BankId;
            }
            set
            {
                _BankId = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// 繳費平台代碼 (1=財金 [EZPOS]; 2=e 政府[eGov]; 3=中國信託[CTCB] ，請參考 CCardApCodeTexts)
        /// </summary>
        [FieldSpec(Field.ApNo, false, FieldTypeEnum.Integer, false)]
        public int ApNo
        {
            get;
            set;
        }


        /// <summary>
        /// 銀行名稱
        /// </summary>
        [FieldSpec(Field.BankName, false, FieldTypeEnum.NVarChar, 50, false)]
        public string BankName
        {
            get;
            set;
        }

        /// <summary>
        /// 建立日期時間
        /// </summary>
        [FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 最後更新日期時間
        /// </summary>
        [FieldSpec(Field.UpdateDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? UpdateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 最後更新者(或建立者)
        /// </summary>
        [FieldSpec(Field.UpdateWho, false, FieldTypeEnum.VarChar, 20, true)]
        public string UpdateWho
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 繳費平台名稱
        /// </summary>
        [XmlIgnore]
        public string ApName
        {
            get
            {
                return CCardApCodeTexts.GetText(this.ApNo.ToString());
            }
        }
        #endregion
    }
}
