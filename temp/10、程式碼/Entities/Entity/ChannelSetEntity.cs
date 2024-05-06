/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/03/24 12:01:51
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
    /// 代收管道資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class ChannelSetEntity : Entity
    {
        public const string TABLE_NAME = "Channel_Set";

        #region Field Name Const Class
        /// <summary>
        /// ChannelSetEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 管道代碼
            /// </summary>
            public const string ChannelId = "Channel_Id";
            #endregion

            #region Data
            /// <summary>
            /// 管道名稱
            /// </summary>
            public const string ChannelName = "Channel_Name";

            /// <summary>
            /// 是否使用旗標
            /// </summary>
            public const string IsUsing = "IsUsing";

            /// <summary>
            /// 顯示排序編號
            /// </summary>
            public const string SortNo = "Sort";

            /// <summary>
            /// 彙總管道代碼
            /// </summary>
            public const string CategoryId = "Category_Id";

            /// <summary>
            /// 是否要處理手續費 (Y / N)
            /// </summary>
            public const string ProcessFee = "ProcessFee";

            /// <summary>
            /// 最小金額限制
            /// </summary>
            public const string MinMoney = "MoneyLowerLimit";

            /// <summary>
            /// 最大金額限制
            /// </summary>
            public const string MaxMoney = "MoneyLimit";

            /// <summary>
            /// 是否為學雜費預設管道旗標 (0=否 / 1=是)
            /// </summary>
            public const string DefaultFlag = "channel_default";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// ChannelSetEntity 類別建構式
        /// </summary>
        public ChannelSetEntity()
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
        #endregion

        #region Data
        /// <summary>
        /// 管道名稱
        /// </summary>
        [FieldSpec(Field.ChannelName, false, FieldTypeEnum.NVarChar, 50, false)]
        public string ChannelName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否使用旗標
        /// </summary>
        [FieldSpec(Field.IsUsing, false, FieldTypeEnum.Boolean, true)]
        public bool? IsUsing
        {
            get;
            set;
        }

        /// <summary>
        /// 顯示排序編號
        /// </summary>
        [FieldSpec(Field.SortNo, false, FieldTypeEnum.Integer, true)]
        public int? SortNo
        {
            get;
            set;
        }

        /// <summary>
        /// 彙總管道代碼
        /// </summary>
        [FieldSpec(Field.CategoryId, false, FieldTypeEnum.VarChar, 4, false)]
        public string CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否要處理手續費 (Y / N)
        /// </summary>
        [FieldSpec(Field.ProcessFee, false, FieldTypeEnum.Char, 1, false)]
        public string ProcessFee
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
        /// 是否為學雜費預設管道旗標 (0=否 / 1=是)
        /// </summary>
        [FieldSpec(Field.DefaultFlag, false, FieldTypeEnum.Char, 1, true)]
        public string DefaultFlag
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
