
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
	/// <summary>
	/// 廣告版位 資料承載類別
	/// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class AdEntity : Entity
    {
        public const string TABLE_NAME = "Ad";

        #region Field Name Const Class
        /// <summary>
        /// AdEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 廣告版位代碼 (PKey 請參考 AdCodeTexts)
            /// </summary>
            public const string Id = "Id";
            #endregion

            #region Data
            /// <summary>
            /// 廣告種類 (C=圖檔內容; U=圖檔網址 請參考 AdKindCodeTexts)
            /// </summary>
            public const string Kind = "Kind";

            /// <summary>
            /// 圖檔網址
            /// </summary>
            public const string ImgUrl = "Img_Url";

            /// <summary>
            /// 圖檔內容
            /// </summary>
            public const string ImgContent = "Img_Content";

            /// <summary>
            /// 廣告連結網址
            /// </summary>
            public const string LinkUrl = "Link_Url";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// AdEntity 類別建構式
        /// </summary>
        public AdEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _Id = null;
        /// <summary>
        /// 廣告版位代碼 (PKey 請參考 AdCodeTexts)
        /// </summary>
        [FieldSpec(Field.Id, true, FieldTypeEnum.VarChar, 5, false)]
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        private string _Kind = null;
        /// <summary>
        /// 廣告種類 (C=圖檔內容; U=圖檔網址 請參考 AdKindCodeTexts)
        /// </summary>
        [FieldSpec(Field.Kind, false, FieldTypeEnum.VarChar, 3, false)]
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

        private string _ImgUrl = null;
        /// <summary>
        /// 圖檔網址
        /// </summary>
        [FieldSpec(Field.ImgUrl, false, FieldTypeEnum.VarChar, 50, true)]
        public string ImgUrl
        {
            get
            {
                return _ImgUrl;
            }
            set
            {
                _ImgUrl = value == null ? null : value.Trim();
            }
        }

        private byte[] _ImgContent = null;
        /// <summary>
        /// 圖檔內容
        /// </summary>
        [FieldSpec(Field.ImgContent, false, FieldTypeEnum.Binary, true)]
        public byte[] ImgContent
        {
            get
            {
                return _ImgContent;
            }
            set
            {
                _ImgContent = value;
            }
        }

        private string _LinkUrl = null;
        /// <summary>
        /// 廣告連結網址
        /// </summary>
        [FieldSpec(Field.LinkUrl, false, FieldTypeEnum.VarChar, 50, false)]
        public string LinkUrl
        {
            get
            {
                return _LinkUrl;
            }
            set
            {
                _LinkUrl = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion
    }
}
