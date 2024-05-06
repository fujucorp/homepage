using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// FuncMenuEntity 的 View (取有頁面的功能，選項用)
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public class FuncMenuView2 : Entity
    {
        protected const string VIEWSQL = @"SELECT [" + FuncMenuEntity.Field.FuncId + @"], [" + FuncMenuEntity.Field.FuncName + @"]
  FROM [" + FuncMenuEntity.TABLE_NAME + @"]
 WHERE [" + FuncMenuEntity.Field.Status + @"] = '0' AND [" + FuncMenuEntity.Field.Url + @"] != '' AND LEN([" + FuncMenuEntity.Field.FuncId + @"]) > 3";

        #region Field Name Const Class
        /// <summary>
        /// FuncMenuView2 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 功能選單代碼 欄位名稱常數定義
            /// </summary>
            public const string FuncId = "Func_Id";
            #endregion

            #region Data
            /// <summary>
            /// 功能選單名稱 欄位名稱常數定義
            /// </summary>
            public const string FuncName = "Func_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// FuncMenuView2 類別建構式
        /// </summary>
        public FuncMenuView2()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _FuncId = null;
        /// <summary>
        /// 功能選單代碼
        /// </summary>
        [FieldSpec(Field.FuncId, true, FieldTypeEnum.VarChar, 32, false)]
        public string FuncId
        {
            get
            {
                return _FuncId;
            }
            set
            {
                _FuncId = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// 功能選單名稱
        /// </summary>
        [FieldSpec(Field.FuncName, false, FieldTypeEnum.NVarChar, 50, false)]
        public string FuncName
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
