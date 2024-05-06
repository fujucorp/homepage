using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 代收類別的代碼、名稱、統編與學制 + 代收類別(學校)設定的登入依據 的 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class SchoolConfigView : Entity
    {
        #region [Old] 增加是否開放學生專區
        //        protected const string VIEWSQL = @"SELECT SR.[Receive_Type], SR.[Sch_Name], SR.[Sch_Identy], SR.[CorpType]
        //     , ISNULL(RC.[Birthday_Chk], '" + LoginKeyTypeCodeTexts.DEFAULT_CODE + @"') AS LOGIN_KEY_TYPE
        //  FROM [" + SchoolRTypeEntity.TABLE_NAME + @"] AS SR
        //  LEFT JOIN [" + ReceiveConfigEntity.TABLE_NAME + @"] AS RC ON SR.[Receive_Type] = RC.[Receive_Type]
        // WHERE SR.[status] = '" + DataStatusCodeTexts.NORMAL + @"'";
        #endregion

        #region [MDY:202203XX] 2022擴充案 增加英文資料啟用、學校英文全名欄位
        protected const string VIEWSQL = @"SELECT SR.[Receive_Type], SR.[Sch_Name], SR.[Sch_Identy], SR.[CorpType]
     , ISNULL(SR.[Open_Student_Area], 'Y') AS [Open_Student_Area]
     , ISNULL(RC.[Birthday_Chk], '" + LoginKeyTypeCodeTexts.DEFAULT_CODE + @"') AS LOGIN_KEY_TYPE
     , SR.[Eng_Enabled], SR.[Sch_EName]
  FROM [" + SchoolRTypeEntity.TABLE_NAME + @"] AS SR
  LEFT JOIN [" + ReceiveConfigEntity.TABLE_NAME + @"] AS RC ON SR.[Receive_Type] = RC.[Receive_Type]
 WHERE SR.[status] = '" + DataStatusCodeTexts.NORMAL + @"'";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// 代收費用設定資料 + 學年、學期、部別、代收費用別名稱 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            /// <summary>
            /// 代收類別代碼 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 代收類別名稱 (學校全名) 欄位名稱常數定義
            /// </summary>
            public const string SchName = "Sch_Name";

            /// <summary>
            /// 學校統一編號 欄位名稱常數定義
            /// </summary>
            public const string SchIdenty = "Sch_Identy";

            /// <summary>
            /// 學制、類別 (1:大專院校 2:高中職 3:國中小 4:幼兒園)
            /// </summary>
            public const string CorpType = "CorpType";

            /// <summary>
            /// 是否開放學生專區 (Y/N)
            /// </summary>
            public const string OpenStudentArea = "Open_Student_Area";

            /// <summary>
            /// 登入依據類別代碼 欄位名稱常數定義
            /// </summary>
            public const string LoginKeyType = "LOGIN_KEY_TYPE";

            #region [MDY:202203XX] 2022擴充案 增加英文資料啟用、學校英文全名 欄位
            /// <summary>
            /// 代收類別 英文資料啟用 (Y:啟用 N:停用，預設 N，一律轉大寫，且非 Y 值一律轉為 N) 欄位名稱常數定義
            /// </summary>
            public const string EngEnabled = "Eng_Enabled";

            /// <summary>
            /// 代收類別 學校英文全名 欄位名稱常數定義
            /// </summary>
            public const string SchEName = "Sch_EName";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構代收費用設定資料 + 學年、學期、部別、代收費用別名稱 View 的資料承載類別
        /// </summary>
        public SchoolConfigView()
            : base()
        {
        }
        #endregion

        #region Property
        private string _ReceiveType = String.Empty;
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
                _ReceiveType = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SchName = String.Empty;
        /// <summary>
        /// 代收類別名稱 (學校全名)
        /// </summary>
        [FieldSpec(Field.SchName, false, FieldTypeEnum.VarChar, 54, false)]
        public string SchName
        {
            get
            {
                return _SchName;
            }
            set
            {
                _SchName = value == null ? String.Empty : value.Trim();
            }
        }

        private string _SchIdenty = String.Empty;
        /// <summary>
        /// 學校統一編號
        /// </summary>
        [FieldSpec(Field.SchIdenty, false, FieldTypeEnum.Char, 8, true)]
        public string SchIdenty
        {
            get
            {
                return _SchIdenty;
            }
            set
            {
                _SchIdenty = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 學制、類別 (1:大專院校 2:高中職 3:國中小 4:幼兒園)
        /// </summary>
        [FieldSpec(Field.CorpType, false, FieldTypeEnum.VarChar, 3, false)]
        public string CorpType
        {
            get;
            set;
        }

        private string _OpenStudentArea = String.Empty;
        /// <summary>
        /// 是否開放學生專區 (Y/N)
        /// </summary>
        [FieldSpec(Field.OpenStudentArea, false, FieldTypeEnum.Char, 1, false)]
        public string OpenStudentArea
        {
            get
            {
                return _OpenStudentArea;
            }
            set
            {
                _OpenStudentArea = value == null ? String.Empty : value.Trim();
            }
        }

        private string _LoginKeyType = String.Empty;
        /// <summary>
        /// 登入依據類別代碼
        /// </summary>
        [FieldSpec(Field.LoginKeyType, false, FieldTypeEnum.Char, 1, true)]
        public string LoginKeyType
        {
            get
            {
                return _LoginKeyType;
            }
            set
            {
                _LoginKeyType = value == null ? String.Empty : value.Trim();
            }
        }

        #region [MDY:202203XX] 2022擴充案 增加英文資料啟用、學校英文全名
        private string _EngEnabled = null;
        /// <summary>
        /// 代收類別 英文資料啟用 (Y:啟用 N:停用，預設 N，一律轉大寫，且非 Y 值一律轉為 N)
        /// </summary>
        [FieldSpec(Field.EngEnabled, false, FieldTypeEnum.Char, 1, true)]
        public string EngEnabled
        {
            get
            {
                return _EngEnabled;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    _EngEnabled = "N";
                }
                else
                {
                    _EngEnabled = "Y".Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase) ? "Y" : "N";
                }
            }
        }

        private string _SchEName = String.Empty;
        /// <summary>
        /// 代收類別 學校英文全名
        /// </summary>
        [FieldSpec(Field.SchEName, false, FieldTypeEnum.NVarChar, 40, true)]
        public string SchEName
        {
            get
            {
                return _SchEName;
            }
            set
            {
                _SchEName = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Public Method
        public string GetSchName(bool isEngUI)
        {
            if (isEngUI && "Y".Equals(this.EngEnabled) && !String.IsNullOrEmpty(this.SchEName))
            {
                return this.SchEName;
            }
            else
            {
                return this.SchName;
            }
        }
        #endregion
    }
}
