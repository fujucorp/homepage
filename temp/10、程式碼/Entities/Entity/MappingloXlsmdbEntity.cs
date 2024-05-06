/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/05/30 10:35:39
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
    /// MappingLO_Xlsmdb 資料表 Entity 類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class MappingloXlsmdbEntity : Entity
    {
        public const string TABLE_NAME = "MappingLO_Xlsmdb";

        #region Const
        /// <summary>
        /// 虛擬帳號欄位對照設定
        /// </summary>
        public const string CancelNo_FIX_VALUE = "虛擬帳號";
        #endregion

        #region Field Name Const Class
        /// <summary>
        /// MappingloXlsmdbEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 對照檔代碼 欄位名稱常數定義
            /// </summary>
            public const string MappingId = "Mapping_Id";

            /// <summary>
            /// 所屬商家代號 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = "Receive_Type";
            #endregion

            #region Data
            /// <summary>
            /// 對照檔名稱 欄位名稱常數定義
            /// </summary>
            public const string MappingName = "Mapping_Name";

            /// <summary>
            /// 學年代碼 欄位名稱常數定義 (土銀不使用)
            /// </summary>
            public const string YearId = "Year_Id";

            /// <summary>
            /// 學期代碼 欄位名稱常數定義 (土銀不使用)
            /// </summary>
            public const string TermId = "Term_Id";

            /// <summary>
            /// 部別代碼 欄位名稱常數定義 (土銀不使用)
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// 費用別代碼 欄位名稱常數定義 (土銀不使用)
            /// </summary>
            public const string ReceiveId = "Receive_Id";
            #endregion

            #region 上傳資料相關欄位
            /// <summary>
            /// 虛擬帳號 (此為後來額外加入的欄位，為了避免要異動資料庫，此欄位並不存在資料表)
            /// </summary>
            public const string CancelNo = "Cancel_No";

            /// <summary>
            /// 學號 欄位名稱常數定義
            /// </summary>
            public const string SId = "S_Id";

            /// <summary>
            /// 就貸代碼 欄位名稱常數定義
            /// </summary>
            public const string LoName = "Lo_Name";

            /// <summary>
            /// 就貸名稱 欄位名稱常數定義
            /// </summary>
            public const string LoCount = "Lo_Count";

            /// <summary>
            /// 可貸金額 欄位名稱常數定義
            /// </summary>
            public const string LoAmount = "Lo_amount";

            #region 就貸金額項目 1 ~ 40
            /// <summary>
            /// 就貸金額項目1 (收入科目01就貸金額) 欄位名稱常數定義
            /// </summary>
            public const string Lo1 = "Lo_1";

            /// <summary>
            /// Lo_2 欄位名稱常數定義
            /// </summary>
            public const string Lo2 = "Lo_2";

            /// <summary>
            /// Lo_3 欄位名稱常數定義
            /// </summary>
            public const string Lo3 = "Lo_3";

            /// <summary>
            /// Lo_4 欄位名稱常數定義
            /// </summary>
            public const string Lo4 = "Lo_4";

            /// <summary>
            /// Lo_5 欄位名稱常數定義
            /// </summary>
            public const string Lo5 = "Lo_5";

            /// <summary>
            /// Lo_6 欄位名稱常數定義
            /// </summary>
            public const string Lo6 = "Lo_6";

            /// <summary>
            /// Lo_7 欄位名稱常數定義
            /// </summary>
            public const string Lo7 = "Lo_7";

            /// <summary>
            /// Lo_8 欄位名稱常數定義
            /// </summary>
            public const string Lo8 = "Lo_8";

            /// <summary>
            /// Lo_9 欄位名稱常數定義
            /// </summary>
            public const string Lo9 = "Lo_9";

            /// <summary>
            /// Lo_10 欄位名稱常數定義
            /// </summary>
            public const string Lo10 = "Lo_10";

            /// <summary>
            /// Lo_11 欄位名稱常數定義
            /// </summary>
            public const string Lo11 = "Lo_11";

            /// <summary>
            /// Lo_12 欄位名稱常數定義
            /// </summary>
            public const string Lo12 = "Lo_12";

            /// <summary>
            /// Lo_13 欄位名稱常數定義
            /// </summary>
            public const string Lo13 = "Lo_13";

            /// <summary>
            /// Lo_14 欄位名稱常數定義
            /// </summary>
            public const string Lo14 = "Lo_14";

            /// <summary>
            /// Lo_15 欄位名稱常數定義
            /// </summary>
            public const string Lo15 = "Lo_15";

            /// <summary>
            /// Lo_16 欄位名稱常數定義
            /// </summary>
            public const string Lo16 = "Lo_16";

            /// <summary>
            /// Lo_17 欄位名稱常數定義
            /// </summary>
            public const string Lo17 = "Lo_17";

            /// <summary>
            /// Lo_18 欄位名稱常數定義
            /// </summary>
            public const string Lo18 = "Lo_18";

            /// <summary>
            /// Lo_19 欄位名稱常數定義
            /// </summary>
            public const string Lo19 = "Lo_19";

            /// <summary>
            /// Lo_20 欄位名稱常數定義
            /// </summary>
            public const string Lo20 = "Lo_20";

            /// <summary>
            /// Lo_21 欄位名稱常數定義
            /// </summary>
            public const string Lo21 = "Lo_21";

            /// <summary>
            /// Lo_22 欄位名稱常數定義
            /// </summary>
            public const string Lo22 = "Lo_22";

            /// <summary>
            /// Lo_23 欄位名稱常數定義
            /// </summary>
            public const string Lo23 = "Lo_23";

            /// <summary>
            /// Lo_24 欄位名稱常數定義
            /// </summary>
            public const string Lo24 = "Lo_24";

            /// <summary>
            /// Lo_25 欄位名稱常數定義
            /// </summary>
            public const string Lo25 = "Lo_25";

            /// <summary>
            /// Lo_26 欄位名稱常數定義
            /// </summary>
            public const string Lo26 = "Lo_26";

            /// <summary>
            /// Lo_27 欄位名稱常數定義
            /// </summary>
            public const string Lo27 = "Lo_27";

            /// <summary>
            /// Lo_28 欄位名稱常數定義
            /// </summary>
            public const string Lo28 = "Lo_28";

            /// <summary>
            /// Lo_29 欄位名稱常數定義
            /// </summary>
            public const string Lo29 = "Lo_29";

            /// <summary>
            /// Lo_30 欄位名稱常數定義
            /// </summary>
            public const string Lo30 = "Lo_30";

            /// <summary>
            /// Lo_31 欄位名稱常數定義
            /// </summary>
            public const string Lo31 = "Lo_31";

            /// <summary>
            /// Lo_32 欄位名稱常數定義
            /// </summary>
            public const string Lo32 = "Lo_32";

            /// <summary>
            /// Lo_33 欄位名稱常數定義
            /// </summary>
            public const string Lo33 = "Lo_33";

            /// <summary>
            /// Lo_34 欄位名稱常數定義
            /// </summary>
            public const string Lo34 = "Lo_34";

            /// <summary>
            /// Lo_35 欄位名稱常數定義
            /// </summary>
            public const string Lo35 = "Lo_35";

            /// <summary>
            /// Lo_36 欄位名稱常數定義
            /// </summary>
            public const string Lo36 = "Lo_36";

            /// <summary>
            /// Lo_37 欄位名稱常數定義
            /// </summary>
            public const string Lo37 = "Lo_37";

            /// <summary>
            /// Lo_38 欄位名稱常數定義
            /// </summary>
            public const string Lo38 = "Lo_38";

            /// <summary>
            /// Lo_39 欄位名稱常數定義
            /// </summary>
            public const string Lo39 = "Lo_39";

            /// <summary>
            /// Lo_40 欄位名稱常數定義
            /// </summary>
            public const string Lo40 = "Lo_40";
            #endregion

            /// <summary>
            /// Amount 欄位名稱常數定義
            /// </summary>
            public const string Amount = "Amount";
            #endregion

            #region 狀態相關欄位
            /// <summary>
            /// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts) 欄位名稱常數定義
            /// </summary>
            public const string Status = "status";

            /// <summary>
            /// 資料建立日期 (含時間) 欄位名稱常數定義
            /// </summary>
            public const string CrtDate = "crt_date";

            /// <summary>
            /// 資料建立者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
            /// </summary>
            public const string CrtUser = "crt_user";

            /// <summary>
            /// 資料最後修改日期 (含時間) 欄位名稱常數定義
            /// </summary>
            public const string MdyDate = "mdy_date";

            /// <summary>
            /// 資料最後修改者。暫時儲存使用者帳號 (UserId) 欄位名稱常數定義
            /// </summary>
            public const string MdyUser = "mdy_user";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// MappingloXlsmdbEntity 類別建構式
        /// </summary>
        public MappingloXlsmdbEntity()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        private string _MappingId = null;
        /// <summary>
        /// 對照檔代碼
        /// </summary>
        [FieldSpec(Field.MappingId, true, FieldTypeEnum.Char, 2, false)]
        public string MappingId
        {
            get
            {
                return _MappingId;
            }
            set
            {
                _MappingId = value == null ? null : value.Trim();
            }
        }

        private string _ReceiveType = null;
        /// <summary>
        /// 所屬商家代號
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
        #endregion

        #region Data
        /// <summary>
        /// 對照檔名稱
        /// </summary>
        [FieldSpec(Field.MappingName, false, FieldTypeEnum.VarChar, 50, true)]
        public string MappingName
        {
            get;
            set;
        }

        /// <summary>
        /// 學年代碼 (土銀不使用)
        /// </summary>
        [FieldSpec(Field.YearId, false, FieldTypeEnum.Char, 3, true)]
        public string YearId
        {
            get;
            set;
        }

        /// <summary>
        /// 學期代碼 (土銀不使用)
        /// </summary>
        [FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, true)]
        public string TermId
        {
            get;
            set;
        }

        /// <summary>
        /// 部別代碼 (土銀不使用)
        /// </summary>
        [FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, true)]
        public string DepId
        {
            get;
            set;
        }

        /// <summary>
        /// 費用別代碼 (土銀不使用)
        /// </summary>
        [FieldSpec(Field.ReceiveId, false, FieldTypeEnum.Char, 1, true)]
        public string ReceiveId
        {
            get;
            set;
        }
        #endregion

        #region 上傳資料相關欄位
        /// <summary>
        /// 虛擬帳號欄位對照設定 (此為後來額外加入的欄位，為了避免要異動資料庫，此欄位並不存在資料表。唯獨，固定傳回 "虛擬帳號")
        /// </summary>
        [XmlIgnore]
        public string CancelNo
        {
            get
            {
                return CancelNo_FIX_VALUE;
            }
        }

        /// <summary>
        /// 學號欄位對照設定
        /// </summary>
        [FieldSpec(Field.SId, false, FieldTypeEnum.VarChar, 20, true)]
        public string SId
        {
            get;
            set;
        }

        /// <summary>
        /// 就貸代碼 欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoName, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoName
        {
            get;
            set;
        }

        /// <summary>
        /// 就貸名稱 欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoCount, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoCount
        {
            get;
            set;
        }

        /// <summary>
        /// 可貸金額 欄位對照設定
        /// </summary>
        [FieldSpec(Field.LoAmount, false, FieldTypeEnum.VarChar, 20, true)]
        public string LoAmount
        {
            get;
            set;
        }

        #region 就貸金額項目 1 ~ 40
        /// <summary>
        /// 就貸金額項目1 (收入科目01就貸金額) 欄位對照設定
        /// </summary>
        [FieldSpec(Field.Lo1, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo1
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_2 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo2, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo2
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_3 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo3, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo3
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_4 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo4, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo4
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_5 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo5, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo5
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_6 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo6, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo6
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_7 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo7, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo7
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_8 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo8, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo8
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_9 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo9, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo9
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_10 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo10, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo10
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_11 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo11, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo11
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_12 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo12, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo12
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_13 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo13, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo13
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_14 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo14, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo14
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_15 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo15, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo15
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_16 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo16, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo16
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_17 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo17, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo17
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_18 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo18, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo18
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_19 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo19, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo19
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_20 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo20, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo20
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_21 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo21, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo21
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_22 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo22, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo22
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_23 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo23, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo23
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_24 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo24, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo24
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_25 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo25, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo25
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_26 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo26, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo26
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_27 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo27, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo27
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_28 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo28, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo28
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_29 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo29, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo29
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_30 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo30, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo30
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_31 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo31, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo31
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_32 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo32, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo32
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_33 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo33, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo33
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_34 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo34, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo34
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_35 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo35, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo35
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_36 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo36, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo36
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_37 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo37, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo37
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_38 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo38, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo38
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_39 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo39, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo39
        {
            get;
            set;
        }

        /// <summary>
        /// Lo_40 欄位屬性
        /// </summary>
        [FieldSpec(Field.Lo40, false, FieldTypeEnum.VarChar, 20, true)]
        public string Lo40
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Amount 欄位屬性
        /// </summary>
        [FieldSpec(Field.Amount, false, FieldTypeEnum.VarChar, 20, true)]
        public string Amount
        {
            get;
            set;
        }
        #endregion

        #region 狀態相關欄位
        /// <summary>
        /// 資料狀態 (0=正常 / D=停用) (請參考 DataStatusCodeTexts)
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// 資料建立日期 (含時間)
        /// </summary>
        [FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CrtDate
        {
            get;
            set;
        }

        /// <summary>
        /// 資料建立者。暫時儲存使用者帳號 (UserId)
        /// </summary>
        [FieldSpec(Field.CrtUser, false, FieldTypeEnum.VarChar, 20, false)]
        public string CrtUser
        {
            get;
            set;
        }

        /// <summary>
        /// 資料最後修改日期 (含時間)
        /// </summary>
        [FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? MdyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 資料最後修改者。暫時儲存使用者帳號 (UserId)
        /// </summary>
        [FieldSpec(Field.MdyUser, false, FieldTypeEnum.VarChar, 20, true)]
        public string MdyUser
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 取得有設定的 XlsMapField 設定陣列
        /// </summary>
        /// <returns>傳回 XlsMapField 設定陣列</returns>
        public XlsMapField[] GetMapFields()
        {
            List<XlsMapField> mapFields = new List<XlsMapField>();

            if (!String.IsNullOrWhiteSpace(this.CancelNo))
            {
                mapFields.Add(new XlsMapField(Field.CancelNo, this.CancelNo, new NumberChecker(12, 16, "12、14或16碼的虛擬帳號數字")));
            }

            if (!String.IsNullOrWhiteSpace(this.SId))
            {
                mapFields.Add(new XlsMapField(Field.SId, this.SId, new CodeChecker(1, 20)));
            }
            if (!String.IsNullOrWhiteSpace(this.LoCount))
            {
                mapFields.Add(new XlsMapField(Field.LoCount, this.LoCount, new WordChecker(1, 20)));
            }
            if (!String.IsNullOrWhiteSpace(this.LoName))
            {
                mapFields.Add(new XlsMapField(Field.LoName, this.LoName, new CodeChecker(1, 20)));
            }
            if (!String.IsNullOrWhiteSpace(this.LoAmount))
            {
                mapFields.Add(new XlsMapField(Field.LoAmount, this.LoAmount, new DecimalChecker(0, 999999999M)));
            }

            #region 就貸金額項目對照欄位 (Lo1 ~ Lo40)
            {
                string[] fields = new string[] {
                    Field.Lo1, Field.Lo2, Field.Lo3, Field.Lo4, Field.Lo5,
                    Field.Lo6, Field.Lo7, Field.Lo8, Field.Lo9, Field.Lo10,
                    Field.Lo11, Field.Lo12, Field.Lo13, Field.Lo14, Field.Lo15,
                    Field.Lo16, Field.Lo17, Field.Lo18, Field.Lo19, Field.Lo20,
                    Field.Lo21, Field.Lo22, Field.Lo23, Field.Lo24, Field.Lo25,
                    Field.Lo26, Field.Lo27, Field.Lo28, Field.Lo29, Field.Lo30,
                    Field.Lo31, Field.Lo32, Field.Lo33, Field.Lo34, Field.Lo35,
                    Field.Lo36, Field.Lo37, Field.Lo38, Field.Lo39, Field.Lo40
                };
                string[] values = new string[] {
                    this.Lo1, this.Lo2, this.Lo3, this.Lo4, this.Lo5,
                    this.Lo6, this.Lo7, this.Lo8, this.Lo9, this.Lo10,
                    this.Lo11, this.Lo12, this.Lo13, this.Lo14, this.Lo15,
                    this.Lo16, this.Lo17, this.Lo18, this.Lo19, this.Lo20,
                    this.Lo21, this.Lo22, this.Lo23, this.Lo24, this.Lo25,
                    this.Lo26, this.Lo27, this.Lo28, this.Lo29, this.Lo30,
                    this.Lo31, this.Lo32, this.Lo33, this.Lo34, this.Lo35,
                    this.Lo36, this.Lo37, this.Lo38, this.Lo39, this.Lo40
                };
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (!String.IsNullOrWhiteSpace(values[idx]))
                    {
                        mapFields.Add(new XlsMapField(fields[idx], values[idx], new DecimalChecker(0M, 999999999M)));
                    }
                }
            }
            #endregion

            return mapFields.ToArray();
        }
        #endregion

        #region Overrider Fuju.Entity's Method
        /// <summary>
        /// 取得指定的欄位名稱 (Field) 或屬性名稱 (Property) 的值
        /// </summary>
        /// <param name="nameOfFieldOrProperty">指定的欄位名稱 (Field) 或屬性名稱 (Property)</param>
        /// <param name="result">傳回執行結果</param>
        /// <returns>成功傳回值，否則傳回 null</returns>
        public override object GetValue(string nameOfFieldOrProperty, out Fuju.Result result)
        {
            if (nameOfFieldOrProperty == Field.CancelNo || nameOfFieldOrProperty == "CancelNo")
            {
                result = new Fuju.Result(true);
                return this.CancelNo;
            }
            else
            {
                return base.GetValue(nameOfFieldOrProperty, out result);
            }
        }
        #endregion
    }
}
