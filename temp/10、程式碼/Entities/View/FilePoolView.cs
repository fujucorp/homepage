using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public class FilePoolView : Entity
    {
        protected const string VIEWSQL = @"
    select sn, explain, file_name, ext_name, file_qual, url, status from file_pool ";

        #region Constructor
		/// <summary>
        /// ContactView 類別建構式
		/// </summary>
        public FilePoolView()
			: base()
		{
		}
		#endregion

        #region Field Name Const Class
        /// <summary>
        /// file pool 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 序號 (Identity)
            /// </summary>
            public const string Sn = "sn";
            #endregion

            #region Data
            /// <summary>
            /// 說明
            /// </summary>
            public const string Explain = "explain";

            /// <summary>
            /// 檔名
            /// </summary>
            public const string FileName = "file_name";

            /// <summary>
            /// 副檔名
            /// </summary>
            public const string ExtName = "ext_name";

            /// <summary>
            /// 型態 (1=連結; 2=檔案)
            /// </summary>
            public const string FileQual = "file_qual";

            /// <summary>
            /// 連結
            /// </summary>
            public const string Url = "url";

            /// <summary>
            /// 狀態
            /// </summary>
            public const string Status = "status";
            #endregion
        }
        #endregion

        #region Property

        #region PKey
        /// <summary>
        /// 序號 (Identity)
        /// </summary>
        [FieldSpec(Field.Sn, true, FieldTypeEnum.Identity, false)]
        public int Sn
        {
            get;
            set;
        }
        #endregion

        #region Data
        /// <summary>
        /// 說明
        /// </summary>
        [FieldSpec(Field.Explain, false, FieldTypeEnum.NVarChar, 1000, false)]
        public string Explain
        {
            get;
            set;
        }

        /// <summary>
        /// 檔名
        /// </summary>
        [FieldSpec(Field.FileName, false, FieldTypeEnum.NVarChar, 100, false)]
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 副檔名
        /// </summary>
        [FieldSpec(Field.ExtName, false, FieldTypeEnum.VarChar, 10, false)]
        public string ExtName
        {
            get;
            set;
        }

        /// <summary>
        /// 型態 (1=連結; 2=檔案)
        /// </summary>
        [FieldSpec(Field.FileQual, false, FieldTypeEnum.Char, 1, false)]
        public string FileQual
        {
            get;
            set;
        }

        /// <summary>
        /// 連結
        /// </summary>
        [FieldSpec(Field.Url, false, FieldTypeEnum.NVarChar, 1000, false)]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// 狀態
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
        public string Status
        {
            get;
            set;
        }
        #endregion
        #endregion
    }
}
