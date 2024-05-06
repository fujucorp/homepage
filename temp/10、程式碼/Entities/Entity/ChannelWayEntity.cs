/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/03/24 15:23:51
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
    /// 代收管道手續費級距資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class ChannelWayEntity : Entity
    {
        public const string TABLE_NAME = "Channel_Way";

        #region Field Name Const Class
        /// <summary>
        /// ChannelWayEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 管道代碼
            /// </summary>
            public const string ChannelId = "Channel_Id";

            /// <summary>
            /// 手序費代碼
            /// </summary>
            public const string BarcodeId = "Barcode_Id";
            #endregion

            #region Data
            /// <summary>
            /// 手續費
            /// </summary>
            public const string ChannelCharge = "Channel_Pay";

            /// <summary>
            /// 繳費單是否包含手續費旗標 (0=否 / 1=是)
            /// </summary>
            public const string IncludePay = "Channel_IncludePay";

            /// <summary>
            /// 手續費負擔別 (2=學校負擔 / 3=繳款人負擔)
            /// </summary>
            public const string RCFlag = "RC_Flag";

            /// <summary>
            /// 學校/繳款人的負擔手續費
            /// </summary>
            public const string RCSPay = "RCS_Pay";

            /// <summary>
            /// 銀行負擔收序費
            /// </summary>
            public const string RCBPay = "RCB_Pay";

            /// <summary>
            /// 最小金額限制
            /// </summary>
            public const string MinMoney = "MoneyLowerLimit";

            /// <summary>
            /// 最大金額限制
            /// </summary>
            public const string MaxMoney = "MoneyLimit";

            /// <summary>
            /// 預留 欄位名稱常數定義
            /// </summary>
            public const string OtherDef = "Channel_OtherDef";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// ChannelWayEntity 類別建構式
        /// </summary>
        public ChannelWayEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _ChannelId = null;
        /// <summary>
        /// 管道代碼
        /// </summary>
        [FieldSpec(Field.ChannelId, true, FieldTypeEnum.VarChar, 4, false)]
        public string ChannelId
        {
            get
            {
                return _ChannelId;
            }
            set
            {
                _ChannelId = value == null ? null : value.Trim();
            }
        }

        private string _BarcodeId = null;
        /// <summary>
        /// 手序費代碼
        /// </summary>
        [FieldSpec(Field.BarcodeId, true, FieldTypeEnum.VarChar, 3, false)]
        public string BarcodeId
        {
            get
            {
                return _BarcodeId;
            }
            set
            {
                _BarcodeId = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// 手續費
        /// </summary>
        [FieldSpec(Field.ChannelCharge, false, FieldTypeEnum.Decimal, true)]
        public decimal? ChannelCharge
        {
            get;
            set;
        }

        private string _IncludePay = null;
        /// <summary>
        /// 繳費單是否包含手續費旗標 (0=否 / 1=是)
        /// </summary>
        [FieldSpec(Field.IncludePay, false, FieldTypeEnum.Char, 1, true)]
        public string IncludePay
        {
            get
            {
                return _IncludePay;
            }
            set
            {
                _IncludePay = value == null ? null : value.Trim();
            }
        }

        private string _RCBPay = null;
        /// <summary>
        /// 手續費負擔別 (2=學校負擔 / 3=繳款人負擔)
        /// </summary>
        [FieldSpec(Field.RCFlag, false, FieldTypeEnum.Char, 1, false)]
        public string RCFlag
        {
            get
            {
                return _RCBPay;
            }
            set
            {
                _RCBPay = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 企業負擔手續費
        /// </summary>
        [FieldSpec(Field.RCSPay, false, FieldTypeEnum.Decimal, false)]
        public decimal RCSPay
        {
            get;
            set;
        }

        /// <summary>
        /// 銀行負擔收序費
        /// </summary>
        [FieldSpec(Field.RCBPay, false, FieldTypeEnum.Decimal, false)]
        public decimal RCBPay
        {
            get;
            set;
        }

        /// <summary>
        /// 最小金額限制
        /// </summary>
        [FieldSpec(Field.MinMoney, false, FieldTypeEnum.Decimal, false)]
        public decimal MinMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 最大金額限制
        /// </summary>
        [FieldSpec(Field.MaxMoney, false, FieldTypeEnum.Decimal, false)]
        public decimal MaxMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 預留 欄位屬性
        /// </summary>
        [FieldSpec(Field.OtherDef, false, FieldTypeEnum.VarChar, 500, true)]
        public string OtherDef
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 取得手續費負擔別的名稱
        /// </summary>
        [XmlIgnore]
        public string RCFlagName
        {
            get
            {
                switch (this.RCFlag)
                {
                    case "2":
                        return "學校負擔";
                    case "3":
                        return "繳款人負擔";
                    default:
                        return this.RCFlag;
                }
            }
        }
        #endregion
    }
}
