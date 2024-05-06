using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 代收費用設定資料 + 學年、學期、部別、代收費用別名稱 View 的資料承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class SchoolRidView2 : SchoolRidEntity
    {
        protected const string VIEWSQL = @"SELECT *
     , ISNULL((SELECT [Year_Name]    FROM [Year_List]    AS YL WHERE YL.[Year_Id] = SR.[Year_Id]), '') AS [Year_Name]
     , ISNULL((SELECT [Term_Name]    FROM [Term_List]    AS TL WHERE TL.[Receive_Type] = SR.[Receive_Type] AND TL.[Year_Id] = SR.[Year_Id] AND TL.[Term_Id] = SR.[Term_Id]), '') AS [Term_Name]
     , ISNULL((SELECT [Dep_Name]     FROM [Dep_List]     AS DL WHERE DL.[Receive_Type] = SR.[Receive_Type] AND DL.[Year_Id] = SR.[Year_Id] AND DL.[Term_Id] = SR.[Term_Id] AND DL.[Dep_Id] = SR.[Dep_Id]), '') AS [Dep_Name]
     , ISNULL((SELECT [Receive_Name] FROM [Receive_List] AS RL WHERE RL.[Receive_Type] = SR.[Receive_Type] AND RL.[Year_Id] = SR.[Year_Id] AND RL.[Term_Id] = SR.[Term_Id] AND RL.[Dep_Id] = SR.[Dep_Id] AND RL.[Receive_Id] = SR.[Receive_Id]), '') AS [Receive_Name]
  FROM [" + SchoolRidEntity.TABLE_NAME + @"] AS SR";

        #region Field Name Const Class
        /// <summary>
        /// 代收費用設定資料 + 學年、學期、部別、代收費用別名稱 View 欄位名稱定義抽象類別
        /// </summary>
        public abstract new class Field : SchoolRidEntity.Field
        {
            #region 代碼名稱
            /// <summary>
            /// 學年名稱 欄位名稱常數定義
            /// </summary>
            public const string YearName = "Year_Name";

            /// <summary>
            /// 學期名稱 欄位名稱常數定義
            /// </summary>
            public const string TermName = "Term_Name";

            /// <summary>
            /// 部別名稱 欄位名稱常數定義
            /// </summary>
            public const string DepName = "Dep_Name";

            /// <summary>
            /// 代收費用別名稱 欄位名稱常數定義
            /// </summary>
            public const string ReceiveName = "Receive_Name";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構代收費用設定資料 + 學年、學期、部別、代收費用別名稱 View 的資料承載類別
        /// </summary>
        public SchoolRidView2()
            : base()
        {
        }
        #endregion

        #region Property
        #region 代碼名稱
        /// <summary>
        /// 學年名稱
        /// </summary>
        [FieldSpec(Field.YearName, false, FieldTypeEnum.Char, 8, false)]
        public string YearName
        {
            get;
            set;
        }

        /// <summary>
        /// 學期名稱
        /// </summary>
        [FieldSpec(Field.TermName, false, FieldTypeEnum.NVarChar, 20, false)]
        public string TermName
        {
            get;
            set;
        }

        /// <summary>
        /// 部別名稱
        /// </summary>
        [FieldSpec(Field.DepName, false, FieldTypeEnum.NVarChar, 20, true)]
        public string DepName
        {
            get;
            set;
        }

        /// <summary>
        /// 代收費用別名稱
        /// </summary>
        [FieldSpec(Field.ReceiveName, false, FieldTypeEnum.NVarChar, 40, true)]
        public string ReceiveName
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Readonly Property
        /// <summary>
        /// 繳費單格式名稱
        /// </summary>
        [XmlIgnore]
        public string PayStyleName
        {
            get
            {
                switch (this.Paystyle)
                {
                    case "0":
                        return "二聯式";
                    case "1":
                        return "三聯式預設";
                    case "2":
                        return "三聯式可調";
                    case "9":
                        return "專屬";
                }
                return String.Empty;
            }
        }
        #endregion
    }
}
