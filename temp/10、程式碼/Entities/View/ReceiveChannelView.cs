using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 代收類別管道手續費設定 + 代收管道ChannelSet 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class ReceiveChannelView : ReceiveChannelEntity
    {
        protected const string VIEWSQL = @"SELECT *
     , ISNULL((SELECT [Channel_Name] FROM [Channel_Set] AS CS  WHERE CS.[Channel_Id] = RC.[RC_Channel]), '') AS [Channel_Name]
  FROM [" + ReceiveChannelEntity.TABLE_NAME + @"] AS RC";

        #region Field Name Const Class
        /// <summary>
        /// 代收類別管道手續費設定 + 代收管道ChannelSet 的資料承載類別
        /// </summary>
        public abstract new class Field : ReceiveChannelEntity.Field
        {
            #region 代碼名稱
            /// <summary>
            /// 繳費方式名稱 欄位名稱常數定義
            /// </summary>
            public const string ChannelName = "Channel_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 代收類別管道手續費設定 + 代收管道ChannelSet 的資料承載類別
        /// </summary>
        public ReceiveChannelView()
            : base()
        {
        }
        #endregion

        #region Property
        #region 代碼名稱
        /// <summary>
        /// 管道名稱
        /// </summary>
        [FieldSpec(Field.ChannelName, false, FieldTypeEnum.NVarChar, 50, false)]
        public string ChannelName
        {
            get;
            set;
        }
        #endregion
        #endregion

    }
}
