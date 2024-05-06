/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/03/24 15:49:30
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
    /// 代收類別管道手續費設定資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class ReceiveChannelEntity : Entity
    {
        public const string TABLE_NAME = "Receive_Channel";

        #region Field Name Const Class
        /// <summary>
        /// ReceiveChannelEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 代收類別代碼
            /// </summary>
            public const string ReceiveType = "RC_type";

            /// <summary>
            /// 管道代碼
            /// </summary>
            public const string ChannelId = "RC_Channel";

            /// <summary>
            /// 手序費代碼
            /// </summary>
            public const string BarcodeId = "BarCode_Id";
            #endregion

            #region Data
            /// <summary>
            /// 手續費
            /// </summary>
            public const string ChannelCharge = "RC_Charge";

            /// <summary>
            /// 繳費單是否包含手續費旗標 (0=否 / 1=是)
            /// </summary>
            public const string IncludePay = "IncludePay";

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
            /// 保留
            /// </summary>
            public const string RCWhom = "RC_Whom";

            /// <summary>
            /// 保留
            /// </summary>
            public const string RCDPay = "RCD_Pay";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// ReceiveChannelEntity 類別建構式
        /// </summary>
        public ReceiveChannelEntity()
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

        /// <summary>
        /// 繳費單是否包含手續費旗標 (0=否 / 1=是)
        /// </summary>
        [FieldSpec(Field.IncludePay, false, FieldTypeEnum.Char, 1, false)]
        public string IncludePay
        {
            get;
            set;
        }

        /// <summary>
        /// 手續費負擔別 (2=學校負擔 / 3=繳款人負擔)
        /// </summary>
        [FieldSpec(Field.RCFlag, false, FieldTypeEnum.Char, 1, true)]
        public string RCFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 學校/繳款人的負擔手續費
        /// </summary>
        [FieldSpec(Field.RCSPay, false, FieldTypeEnum.Decimal, true)]
        public decimal? RCSPay
        {
            get;
            set;
        }

        /// <summary>
        /// 銀行負擔收序費
        /// </summary>
        [FieldSpec(Field.RCBPay, false, FieldTypeEnum.Decimal, true)]
        public decimal? RCBPay
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
        [FieldSpec(Field.MaxMoney, false, FieldTypeEnum.Decimal, true)]
        public decimal? MaxMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 保留
        /// </summary>
        [FieldSpec(Field.RCWhom, false, FieldTypeEnum.Char, 7, true)]
        public string RCWhom
        {
            get;
            set;
        }

        /// <summary>
        /// 保留
        /// </summary>
        [FieldSpec(Field.RCDPay, false, FieldTypeEnum.Decimal, true)]
        public decimal? RCDPay
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 取得是否為郵局管道 (因為台企銀沒有郵局管道，所以永遠傳回 false)
        /// </summary>
        /// <returns></returns>
        public bool IsPOChannel()
        {
            return false;
        }

        /// <summary>
        /// 取得是否為超商管道
        /// </summary>
        /// <returns></returns>
        public bool IsSMChannel()
        {
            return ChannelHelper.IsSMChannel(this.ChannelId);
        }

        /// <summary>
        /// 取得是否為臨櫃管道
        /// </summary>
        /// <returns></returns>
        public bool IsCashChannel()
        {
            return ChannelHelper.IsCashChannel(this.ChannelId);
        }
        #endregion
    }
}
