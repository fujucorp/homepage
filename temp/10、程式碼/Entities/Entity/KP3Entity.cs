/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2021/10/24 01:07:53
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// 聯徵KP3資料 承載類別
    /// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class KP3Entity : Entity
    {
        public const string TABLE_NAME = "KP3";

        #region Field Name Const Class
        /// <summary>
        /// KP3Entity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 序號
            /// </summary>
            public const string SN = "SN";
            #endregion

            #region Data
            /// <summary>
            /// 資料歸屬分行代碼
            /// </summary>
            public const string BranchCode = "Branch_Code";

            /// <summary>
            /// KP3 資料別 (固定10)
            /// </summary>
            public const string Item01 = "Item01";

            /// <summary>
            /// KP3 報送代碼 (A:新增／C:異動／D:刪除)
            /// </summary>
            public const string Item02 = "Item02";

            /// <summary>
            /// KP3 電子支付機構代號 (固定005)
            /// </summary>
            public const string Item03 = "Item03";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item04 = "Item04";

            /// <summary>
            /// KP3 特約機構屬性 (固定G)
            /// </summary>
            public const string Item05 = "Item05";

            /// <summary>
            /// KP3 特約機構 BAN/IDN
            /// </summary>
            public const string Item06 = "Item06";

            /// <summary>
            /// KP3 特約機構代號
            /// </summary>
            public const string Item07 = "Item07";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item08 = "Item08";

            /// <summary>
            /// KP3 特約機構類型 (1:第一類特約機構／2:第二類特約機構)
            /// </summary>
            public const string Item09 = "Item09";

            /// <summary>
            /// KP3 負責人/代表人 IDN
            /// </summary>
            public const string Item10 = "Item10";

            /// <summary>
            /// KP3 簽約日期 (民國YYYMMDD)
            /// </summary>
            public const string Item11 = "Item11";

            /// <summary>
            /// KP3 終止契約種類代號
            /// </summary>
            public const string Item12 = "Item12";

            /// <summary>
            /// KP3 終止契約原因代號
            /// </summary>
            public const string Item13 = "Item13";

            /// <summary>
            /// KP3 終止契約日期 (民國YYYMMDD)
            /// </summary>
            public const string Item14 = "Item14";

            /// <summary>
            /// KP3 終止契約後有應收未收取款項
            /// </summary>
            public const string Item15 = "Item15";

            /// <summary>
            /// KP3 終止契約後應收未收取款項金額
            /// </summary>
            public const string Item16 = "Item16";

            /// <summary>
            /// KP3 登記名稱
            /// </summary>
            public const string Item17 = "Item17";

            /// <summary>
            /// KP3 登記地址
            /// </summary>
            public const string Item18 = "Item18";

            /// <summary>
            /// KP3 招牌名稱
            /// </summary>
            public const string Item19 = "Item19";

            /// <summary>
            /// KP3 營業地址
            /// </summary>
            public const string Item20 = "Item20";

            /// <summary>
            /// KP3 英文名稱
            /// </summary>
            public const string Item21 = "Item21";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item22 = "Item22";

            /// <summary>
            /// KP3 營業型態 (1:實體／2:網路／3:實體及網路)
            /// </summary>
            public const string Item23 = "Item23";

            /// <summary>
            /// KP3 資本額
            /// </summary>
            public const string Item24 = "Item24";

            /// <summary>
            /// KP3 設立日期 (民國YYYMMDD)
            /// </summary>
            public const string Item25 = "Item25";

            /// <summary>
            /// KP3 業務行為
            /// </summary>
            public const string Item26 = "Item26";

            /// <summary>
            /// KP3 是否受理電子支付帳戶或儲值卡服務
            /// </summary>
            public const string Item27 = "Item27";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item28 = "Item28";

            /// <summary>
            /// KP3 是否受理信用卡服務 (Y:是／N:否)
            /// </summary>
            public const string Item29 = "Item29";

            /// <summary>
            /// KP3 營業性質
            /// </summary>
            public const string Item30 = "Item30";

            /// <summary>
            /// KP3 受理信用卡別名稱
            /// </summary>
            public const string Item31 = "Item31";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item32 = "Item32";

            /// <summary>
            /// KP3 是否有銷售遞延性商品或服務 (Y:是／N:否)
            /// </summary>
            public const string Item33 = "Item33";

            /// <summary>
            /// KP3 是否安裝端末設備 (Y:是／N:否)
            /// </summary>
            public const string Item34 = "Item34";

            /// <summary>
            /// KP3 是否安裝錄影設備 (Y:是／N:否)
            /// </summary>
            public const string Item35 = "Item35";

            /// <summary>
            /// KP3 連鎖店加盟或直營
            /// </summary>
            public const string Item36 = "Item36";

            /// <summary>
            /// KP3 保證人1 IDN/BAN
            /// </summary>
            public const string Item37 = "Item37";

            /// <summary>
            /// KP3 保證人2 IDN/BAN
            /// </summary>
            public const string Item38 = "Item38";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item39 = "Item39";

            /// <summary>
            /// KP3 資料更新日期 (民國YYYMMDD)
            /// </summary>
            public const string Item40 = "Item40";

            /// <summary>
            /// KP3 空白
            /// </summary>
            public const string Item41 = "Item41";

            /// <summary>
            /// KP3 狀態 (10=建立資料後待匯出 / 21=匯出資料中 / 20=匯出資料後待回饋 / 40=回饋完成)
            /// </summary>
            public const string Status = "Status";

            /// <summary>
            /// 資料建立日期時間
            /// </summary>
            public const string CreateDate = "Create_Date";

            /// <summary>
            /// 資料建立者單位代碼
            /// </summary>
            public const string CreateUnit = "Create_Unit";

            /// <summary>
            /// 資料建立者帳號代碼
            /// </summary>
            public const string CreateUser = "Create_User";

            /// <summary>
            /// 報送KP3資料序號
            /// </summary>
            public const string RenderSN = "Render_SN";

            /// <summary>
            /// 回饋處理狀態 (Y=成功／N=失敗)
            /// </summary>
            public const string FeedbackStatus = "Feedback_Status";

            /// <summary>
            /// 回饋處理結果
            /// </summary>
            public const string FeedbackResult = "Feedback_Result";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// KP3Entity 類別建構式
        /// </summary>
        public KP3Entity()
            : base()
        {
        }

        public KP3Entity(KP3Config config)
        {
            this.Item03 = config.Unit;
            this.Item05 = config.DataItem05;
            this.Item09 = config.DataItem09;
            this.Item23 = config.DataItem23;
            this.Item26 = config.DataItem26;
            this.Item27 = config.DataItem27;
            this.Item29 = config.DataItem29;
            this.Item33 = config.DataItem33;
            this.Item34 = config.DataItem34;
            this.Item35 = config.DataItem35;
            this.Item36 = config.DataItem36;
        }
        #endregion

        #region Property
        #region PKey
        private string _SN = null;
        /// <summary>
        /// 序號
        /// </summary>
        [FieldSpec(Field.SN, true, FieldTypeEnum.VarChar, 32, false)]
        public string SN
        {
            get
            {
                return _SN;
            }
            set
            {
                _SN = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region Data
        private string _BranchCode = null;
        /// <summary>
        /// 資料歸屬分行代碼
        /// </summary>
        [FieldSpec(Field.BranchCode, false, FieldTypeEnum.VarChar, 7, false)]
        public string BranchCode
        {
            get
            {
                return _BranchCode;
            }
            set
            {
                _BranchCode = value == null ? null : value.Trim();
            }
        }

        private string _Item01 = "10";
        /// <summary>
        /// KP3 資料別 (固定10)
        /// </summary>
        [FieldSpec(Field.Item01, false, FieldTypeEnum.VarChar, 2, false)]
        public string Item01
        {
            get
            {
                return _Item01;
            }
            set
            {
                _Item01 = value == null ? "10" : value.Trim();
            }
        }

        private string _Item02 = null;
        /// <summary>
        /// KP3 報送代碼 (A:新增／C:異動／D:刪除)
        /// </summary>
        [FieldSpec(Field.Item02, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item02
        {
            get
            {
                return _Item02;
            }
            set
            {
                _Item02 = value == null ? null : value.Trim();
            }
        }

        private string _Item03 = "005";
        /// <summary>
        /// KP3 電子支付機構代號 (固定005) (3碼阿拉伯數字或大寫英文字母)
        /// </summary>
        [FieldSpec(Field.Item03, false, FieldTypeEnum.VarChar, 3, false)]
        public string Item03
        {
            get
            {
                return _Item03;
            }
            set
            {
                _Item03 = value == null ? "005" : value.Trim().ToUpper();
            }
        }

        private string _Item04 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item04, false, FieldTypeEnum.VarChar, 4, false)]
        public string Item04
        {
            get
            {
                return _Item04;
            }
            set
            {
                _Item04 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item05 = "G";
        /// <summary>
        /// KP3 特約機構屬性 (固定G) (G:非個人特約機構 / H:個人特約機構)
        /// </summary>
        [FieldSpec(Field.Item05, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item05
        {
            get
            {
                return _Item05;
            }
            set
            {
                _Item05 = value == null ? "G" : value.Trim();
            }
        }

        private string _Item06 = null;
        /// <summary>
        /// KP3 特約機構 BAN/IDN (統一編號、稅籍編號或虛擬統編 / 個人之身分證號、統一證號或稅籍編號)
        /// </summary>
        [FieldSpec(Field.Item06, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item06
        {
            get
            {
                return _Item06;
            }
            set
            {
                _Item06 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item07 = null;
        /// <summary>
        /// KP3 特約機構代號
        /// </summary>
        [FieldSpec(Field.Item07, false, FieldTypeEnum.VarChar, 20, false)]
        public string Item07
        {
            get
            {
                return _Item07;
            }
            set
            {
                _Item07 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim().ToUpper();
            }
        }

        private string _Item08 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item08, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item08
        {
            get
            {
                return _Item08;
            }
            set
            {
                _Item08 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item09 = null;
        /// <summary>
        /// KP3 特約機構類型 (1:第一類特約機構／2:第二類特約機構)
        /// </summary>
        [FieldSpec(Field.Item09, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item09
        {
            get
            {
                return _Item09;
            }
            set
            {
                _Item09 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item10 = null;
        /// <summary>
        /// KP3 負責人/代表人 IDN (身分證號、統一證號或稅籍編號)
        /// </summary>
        [FieldSpec(Field.Item10, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item10
        {
            get
            {
                return _Item10;
            }
            set
            {
                _Item10 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item11 = null;
        /// <summary>
        /// KP3 簽約日期 (民國YYYMMDD)
        /// </summary>
        [FieldSpec(Field.Item11, false, FieldTypeEnum.VarChar, 7, false)]
        public string Item11
        {
            get
            {
                return _Item11;
            }
            set
            {
                _Item11 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item12 = null;
        /// <summary>
        /// KP3 終止契約種類代號
        /// </summary>
        [FieldSpec(Field.Item12, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item12
        {
            get
            {
                return _Item12;
            }
            set
            {
                _Item12 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item13 = null;
        /// <summary>
        /// KP3 終止契約原因代號
        /// </summary>
        [FieldSpec(Field.Item13, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item13
        {
            get
            {
                return _Item13;
            }
            set
            {
                _Item13 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim().ToUpper();
            }
        }

        private string _Item14 = null;
        /// <summary>
        /// KP3 終止契約日期 (民國YYYMMDD)
        /// </summary>
        [FieldSpec(Field.Item14, false, FieldTypeEnum.VarChar, 7, false)]
        public string Item14
        {
            get
            {
                return _Item14;
            }
            set
            {
                _Item14 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item15 = null;
        /// <summary>
        /// KP3 終止契約後有應收未收取款項
        /// </summary>
        [FieldSpec(Field.Item15, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item15
        {
            get
            {
                return _Item15;
            }
            set
            {
                _Item15 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item16 = null;
        /// <summary>
        /// KP3 終止契約後應收未收取款項金額
        /// </summary>
        [FieldSpec(Field.Item16, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item16
        {
            get
            {
                return _Item16;
            }
            set
            {
                _Item16 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item17 = null;
        /// <summary>
        /// KP3 登記名稱
        /// </summary>
        [FieldSpec(Field.Item17, false, FieldTypeEnum.NVarChar, 30, false)]
        public string Item17
        {
            get
            {
                return _Item17;
            }
            set
            {
                _Item17 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item18 = null;
        /// <summary>
        /// KP3 登記地址
        /// </summary>
        [FieldSpec(Field.Item18, false, FieldTypeEnum.NVarChar, 60, false)]
        public string Item18
        {
            get
            {
                return _Item18;
            }
            set
            {
                _Item18 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item19 = null;
        /// <summary>
        /// KP3 招牌名稱
        /// </summary>
        [FieldSpec(Field.Item19, false, FieldTypeEnum.NVarChar, 30, false)]
        public string Item19
        {
            get
            {
                return _Item19;
            }
            set
            {
                _Item19 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item20 = null;
        /// <summary>
        /// KP3 營業地址
        /// </summary>
        [FieldSpec(Field.Item20, false, FieldTypeEnum.NVarChar, 60, false)]
        public string Item20
        {
            get
            {
                return _Item20;
            }
            set
            {
                _Item20 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item21 = null;
        /// <summary>
        /// KP3 英文名稱
        /// </summary>
        [FieldSpec(Field.Item21, false, FieldTypeEnum.VarChar, 30, false)]
        public string Item21
        {
            get
            {
                return _Item21;
            }
            set
            {
                _Item21 = String.IsNullOrEmpty(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item22 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item22, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item22
        {
            get
            {
                return _Item22;
            }
            set
            {
                _Item22 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item23 = null;
        /// <summary>
        /// KP3 營業型態 (1:實體／2:網路／3:實體及網路)
        /// </summary>
        [FieldSpec(Field.Item23, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item23
        {
            get
            {
                return _Item23;
            }
            set
            {
                _Item23 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item24 = null;
        /// <summary>
        /// KP3 資本額
        /// </summary>
        [FieldSpec(Field.Item24, false, FieldTypeEnum.VarChar, 15, false)]
        public string Item24
        {
            get
            {
                return _Item24;
            }
            set
            {
                _Item24 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item25 = null;
        /// <summary>
        /// KP3 設立日期 (民國YYYMMDD)
        /// </summary>
        [FieldSpec(Field.Item25, false, FieldTypeEnum.VarChar, 7, false)]
        public string Item25
        {
            get
            {
                return _Item25;
            }
            set
            {
                _Item25 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item26 = null;
        /// <summary>
        /// KP3 業務行為
        /// </summary>
        [FieldSpec(Field.Item26, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item26
        {
            get
            {
                return _Item26;
            }
            set
            {
                _Item26 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item27 = null;
        /// <summary>
        /// KP3 是否受理電子支付帳戶或儲值卡服務
        /// </summary>
        [FieldSpec(Field.Item27, false, FieldTypeEnum.VarChar, 4, false)]
        public string Item27
        {
            get
            {
                return _Item27;
            }
            set
            {
                _Item27 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item28 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item28, false, FieldTypeEnum.VarChar, 52, false)]
        public string Item28
        {
            get
            {
                return _Item28;
            }
            set
            {
                _Item28 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item29 = null;
        /// <summary>
        /// KP3 是否受理信用卡服務 (Y:是／N:否)
        /// </summary>
        [FieldSpec(Field.Item29, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item29
        {
            get
            {
                return _Item29;
            }
            set
            {
                _Item29 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item30 = null;
        /// <summary>
        /// KP3 營業性質
        /// </summary>
        [FieldSpec(Field.Item30, false, FieldTypeEnum.VarChar, 4, false)]
        public string Item30
        {
            get
            {
                return _Item30;
            }
            set
            {
                _Item30 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item31 = null;
        /// <summary>
        /// KP3 受理信用卡別名稱
        /// </summary>
        [FieldSpec(Field.Item31, false, FieldTypeEnum.VarChar, 12, false)]
        public string Item31
        {
            get
            {
                return _Item31;
            }
            set
            {
                _Item31 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item32 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item32, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item32
        {
            get
            {
                return _Item32;
            }
            set
            {
                _Item32 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item33 = null;
        /// <summary>
        /// KP3 是否有銷售遞延性商品或服務 (Y:是／N:否)
        /// </summary>
        [FieldSpec(Field.Item33, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item33
        {
            get
            {
                return _Item33;
            }
            set
            {
                _Item33 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item34 = null;
        /// <summary>
        /// KP3 是否安裝端末設備 (Y:是／N:否)
        /// </summary>
        [FieldSpec(Field.Item34, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item34
        {
            get
            {
                return _Item34;
            }
            set
            {
                _Item34 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item35 = null;
        /// <summary>
        /// KP3 是否安裝錄影設備 (Y:是／N:否)
        /// </summary>
        [FieldSpec(Field.Item35, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item35
        {
            get
            {
                return _Item35;
            }
            set
            {
                _Item35 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item36 = null;
        /// <summary>
        /// KP3 連鎖店加盟或直營
        /// </summary>
        [FieldSpec(Field.Item36, false, FieldTypeEnum.VarChar, 1, false)]
        public string Item36
        {
            get
            {
                return _Item36;
            }
            set
            {
                _Item36 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item37 = null;
        /// <summary>
        /// KP3 保證人1 IDN/BAN
        /// </summary>
        [FieldSpec(Field.Item37, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item37
        {
            get
            {
                return _Item37;
            }
            set
            {
                _Item37 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item38 = null;
        /// <summary>
        /// KP3 保證人2 IDN/BAN
        /// </summary>
        [FieldSpec(Field.Item38, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item38
        {
            get
            {
                return _Item38;
            }
            set
            {
                _Item38 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item39 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item39, false, FieldTypeEnum.VarChar, 10, false)]
        public string Item39
        {
            get
            {
                return _Item39;
            }
            set
            {
                _Item39 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item40 = null;
        /// <summary>
        /// KP3 資料更新日期 (民國YYYMMDD)
        /// </summary>
        [FieldSpec(Field.Item40, false, FieldTypeEnum.VarChar, 7, false)]
        public string Item40
        {
            get
            {
                return _Item40;
            }
            set
            {
                _Item40 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Item41 = String.Empty;
        /// <summary>
        /// KP3 空白
        /// </summary>
        [FieldSpec(Field.Item41, false, FieldTypeEnum.VarChar, 90, false)]
        public string Item41
        {
            get
            {
                return _Item41;
            }
            set
            {
                _Item41 = String.IsNullOrWhiteSpace(value) ? String.Empty : value.Trim();
            }
        }

        private string _Status = null;
        /// <summary>
        /// KP3 狀態 (10=建立資料後待匯出 / 21=匯出資料中 / 20=匯出資料後待回饋 / 40=回饋完成)
        /// </summary>
        [FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 5, false)]
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 資料建立日期時間
        /// </summary>
        [FieldSpec(Field.CreateDate, false, FieldTypeEnum.DateTime, false)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        private string _CreateUnit = null;
        /// <summary>
        /// 資料建立者單位代碼
        /// </summary>
        [FieldSpec(Field.CreateUnit, false, FieldTypeEnum.VarChar, 7, false)]
        public string CreateUnit
        {
            get
            {
                return _CreateUnit;
            }
            set
            {
                _CreateUnit = value == null ? null : value.Trim();
            }
        }

        private string _CreateUser = null;
        /// <summary>
        /// 資料建立者帳號代碼
        /// </summary>
        [FieldSpec(Field.CreateUser, false, FieldTypeEnum.VarChar, 10, false)]
        public string CreateUser
        {
            get
            {
                return _CreateUser;
            }
            set
            {
                _CreateUser = value == null ? null : value.Trim();
            }
        }

        private string _RenderSN = null;
        /// <summary>
        /// 報送KP3資料序號
        /// </summary>
        [FieldSpec(Field.RenderSN, false, FieldTypeEnum.VarChar, 16, true)]
        public string RenderSN
        {
            get
            {
                return _RenderSN;
            }
            set
            {
                _RenderSN = value == null ? null : value.Trim();
            }
        }

        private string _FeedbackStatus = null;
        /// <summary>
        /// 回饋處理狀態 (Y=成功／N=失敗)
        /// </summary>
        [FieldSpec(Field.FeedbackStatus, false, FieldTypeEnum.VarChar, 20, true)]
        public string FeedbackStatus
        {
            get
            {
                return _FeedbackStatus;
            }
            set
            {
                _FeedbackStatus = value == null ? null : value.Trim();
            }
        }

        private string _FeedbackResult = null;
        /// <summary>
        /// 回饋處理結果
        /// </summary>
        [FieldSpec(Field.FeedbackResult, false, FieldTypeEnum.NVarChar, 100, true)]
        public string FeedbackResult
        {
            get
            {
                return _FeedbackResult;
            }
            set
            {
                _FeedbackResult = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Inner Enum
        /// <summary>
        /// 靠補方式
        /// </summary>
        private enum PadWay
        {
            /// <summary>
            /// 不靠不補（匹配長度）
            /// </summary>
            Match = 0,

            /// <summary>
            /// 左靠右補
            /// </summary>
            Right = 1,

            /// <summary>
            /// 右靠左補
            /// </summary>
            Left = 2
        }
        #endregion

        #region Private Method
        private Regex _ItemNameReg = null;
        /// <summary>
        /// 取得項目名稱正規表示式
        /// </summary>
        /// <returns>傳回項目名稱正規表示式</returns>
        private Regex GetItemNameRegex()
        {
            if (_ItemNameReg == null)
            {
                _ItemNameReg = new Regex(@"^Item([0-3]\d|4[01])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return _ItemNameReg;
        }

        Encoding _Encoding = null;

        private byte[] Repeat(char chr, int byteSize)
        {
            byte[] newBytes = new byte[byteSize];
            byte[] srcBytes = _Encoding.GetBytes(new Char[] { chr });
            int modVal = srcBytes.Length <= byteSize ? srcBytes.Length : byteSize;
            for (int idx = 0; idx < byteSize; idx++)
            {
                newBytes[idx] = srcBytes[idx % modVal];
            }
            return newBytes;
        }

        /// <summary>
        /// 取得指定文字的格式化後文字
        /// </summary>
        /// <param name="text">指定文字</param>
        /// <param name="byteSize">指定格式化後位元組（字節）長度</param>
        /// <param name="padChar">指定補齊位元組長度用的字元，預設為空白</param>
        /// <param name="padWay">指定補齊切除格式化的方式，預設為 PadWay.Match</param>
        /// <returns>成功則傳回格式化後文字，否則傳回 null</returns>
        private string GetFormatText(string text, int byteSize, char padChar = ' ', PadWay padWay = PadWay.Match)
        {
            if (_Encoding == null)
            {
                _Encoding = Encoding.GetEncoding("big5");
            }

            byte[] srcBytes = _Encoding.GetBytes(text);

            if (srcBytes.Length > byteSize)
            {
                #region 原始字串的位元組長度較大，表示要切字
                byte[] newBytes = new byte[byteSize];
                if (padWay == PadWay.Right)
                {
                    //左靠右切
                    Array.Copy(srcBytes, 0, newBytes, 0, byteSize);
                }
                else if (padWay == PadWay.Left)
                {
                    //右靠左切
                    Array.Copy(srcBytes, (srcBytes.Length - byteSize), newBytes, 0, byteSize);
                }
                else
                {
                    return null;  //失敗
                }
                return _Encoding.GetString(newBytes);
                #endregion
            }
            else if (srcBytes.Length < byteSize)
            {
                #region 原始字串的位元組長度較小，表示要補字
                byte[] newBytes = this.Repeat(padChar, byteSize);
                if (padWay == PadWay.Right)
                {
                    //左靠右補
                    Array.Copy(srcBytes, 0, newBytes, 0, srcBytes.Length);
                }
                else if (padWay == PadWay.Left)
                {
                    //右靠左補
                    Array.Copy(srcBytes, 0, newBytes, (byteSize - srcBytes.Length), srcBytes.Length);
                }
                else
                {
                    return null;
                }
                return _Encoding.GetString(newBytes);
                #endregion
            }
            else
            {
                return text;
            }
        }

        private string[] _ItemNos = null;

        private string[] GetItemNos()
        {
            if (_ItemNos == null)
            {
                _ItemNos = new string[41]
                {
                    "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                    "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                    "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                    "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
                    "41"
                };
            }
            return _ItemNos;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 取得 Item02 報送代碼 的文字
        /// </summary>
        /// <returns></returns>
        public string GetItem02Text()
        {
            switch (this.Item02)
            {
                case "A":
                    return "新增";
                case "C":
                    return "異動";
                case "D":
                    return "刪除";
                case null:
                case "":
                    return "";
                default:
                    return "N/A";
            }
        }

        /// <summary>
        /// 取得 是否受理電子支付帳戶或儲值卡服務（Item27） 選取值陣列
        /// </summary>
        /// <returns></returns>
        public string[] GetItem27()
        {
            string[] values = null;
            if (!String.IsNullOrEmpty(this.Item27))
            {
                char[] chars = this.Item27.ToCharArray();
                values = new string[chars.Length];
                for (int idx = 0; idx < chars.Length; idx++)
                {
                    values[idx] = chars[idx].ToString();
                }
            }
            return values;
        }

        /// <summary>
        /// 取得 受理信用卡別名稱（Item31） 選取值陣列
        /// </summary>
        /// <returns></returns>
        public string[] GetItem31()
        {
            string[] values = null;
            if (!String.IsNullOrEmpty(this.Item31))
            {
                char[] chars = this.Item31.ToCharArray();
                values = new string[chars.Length];
                for (int idx = 0; idx < chars.Length; idx++)
                {
                    values[idx] = chars[idx].ToString();
                }
            }
            return values;
        }

        /// <summary>
        ///指定名稱是否為項目名稱
        /// </summary>
        /// <param name="itemName">指定名稱</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsItemName(string itemName)
        {
            return this.GetItemNameRegex().IsMatch(itemName);
        }

        /// <summary>
        /// 指定編號是否為項目編號
        /// </summary>
        /// <param name="itemNo">指定編號</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsItemNo(string itemNo)
        {
            return this.IsItemName(String.Concat("Item", itemNo));
        }

        /// <summary>
        /// 取得指定項目名稱的項目內容
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetItemValueByName(string itemName)
        {
            Regex reg = this.GetItemNameRegex();
            Match m = reg.Match(itemName);
            if (m.Success)
            {
                string itemNo = m.Groups[1].Value;
                return GetItemValueByNo(itemNo);
            }
            return null;
        }

        /// <summary>
        /// 取得指定項目編號的項目值
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public string GetItemValueByNo(string itemNo)
        {
            switch (itemNo)
            {
                case "01":
                    return this.Item01;
                case "02":
                    return this.Item02;
                case "03":
                    return this.Item03;
                case "04":
                    return this.Item04;
                case "05":
                    return this.Item05;
                case "06":
                    return this.Item06;
                case "07":
                    return this.Item07;
                case "08":
                    return this.Item08;
                case "09":
                    return this.Item09;
                case "10":
                    return this.Item10;

                case "11":
                    return this.Item11;
                case "12":
                    return this.Item12;
                case "13":
                    return this.Item13;
                case "14":
                    return this.Item14;
                case "15":
                    return this.Item15;
                case "16":
                    return this.Item16;
                case "17":
                    return this.Item17;
                case "18":
                    return this.Item18;
                case "19":
                    return this.Item19;
                case "20":
                    return this.Item20;

                case "21":
                    return this.Item21;
                case "22":
                    return this.Item22;
                case "23":
                    return this.Item23;
                case "24":
                    return this.Item24;
                case "25":
                    return this.Item25;
                case "26":
                    return this.Item26;
                case "27":
                    return this.Item27;
                case "28":
                    return this.Item28;
                case "29":
                    return this.Item29;
                case "30":
                    return this.Item30;

                case "31":
                    return this.Item31;
                case "32":
                    return this.Item32;
                case "33":
                    return this.Item33;
                case "34":
                    return this.Item34;
                case "35":
                    return this.Item35;
                case "36":
                    return this.Item36;
                case "37":
                    return this.Item37;
                case "38":
                    return this.Item38;
                case "39":
                    return this.Item39;
                case "40":
                    return this.Item40;

                case "41":
                    return this.Item41;

                default:
                    return null;
            }
        }

        /// <summary>
        /// 取得指定項目編號的項目資料欄
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        public string GetItemDataText(string itemNo)
        {
            switch (itemNo)
            {
                case "01":
                    return this.GetFormatText(this.Item01, 2);
                case "02":
                    return this.GetFormatText(this.Item02, 1);
                case "03":
                    return this.GetFormatText(this.Item03, 3);
                case "04":
                    return this.GetFormatText(this.Item04, 4, ' ', PadWay.Right);
                case "05":
                    return this.GetFormatText(this.Item05, 1);
                case "06":
                    return this.GetFormatText(this.Item06, 10, ' ', PadWay.Right);
                case "07":
                    return this.GetFormatText(this.Item07, 20, ' ', PadWay.Right);
                case "08":
                    return this.GetFormatText(this.Item08, 10, ' ', PadWay.Right);
                case "09":
                    return this.GetFormatText(this.Item09, 1);
                case "10":
                    return this.GetFormatText(this.Item10, 10, ' ', PadWay.Right);

                case "11":
                    return this.GetFormatText(this.Item11, 7);
                case "12":
                    return this.GetFormatText(this.Item12, 1, ' ', PadWay.Right);
                case "13":
                    return this.GetFormatText(this.Item13, 1, ' ', PadWay.Right);
                case "14":
                    return this.GetFormatText(this.Item14, 7, ' ', PadWay.Right);
                case "15":
                    return this.GetFormatText(this.Item15, 1, ' ', PadWay.Right);
                case "16":
                    if (String.IsNullOrEmpty(this.Item14))
                    {
                        return this.GetFormatText(this.Item16, 10, ' ', PadWay.Left);
                    }
                    else
                    {
                        return this.GetFormatText(this.Item16, 10, '0', PadWay.Left);
                    }
                case "17":
                    return this.GetFormatText(this.Item17, 60, ' ', PadWay.Right);
                case "18":
                    return this.GetFormatText(this.Item18, 120, ' ', PadWay.Right);
                case "19":
                    return this.GetFormatText(this.Item19, 60, ' ', PadWay.Right);
                case "20":
                    return this.GetFormatText(this.Item20, 120, ' ', PadWay.Right);

                case "21":
                    return this.GetFormatText(this.Item21, 30, ' ', PadWay.Right);
                case "22":
                    return this.GetFormatText(this.Item22, 10, ' ', PadWay.Right);
                case "23":
                    return this.GetFormatText(this.Item23, 1);
                case "24":
                    return this.GetFormatText(this.Item24, 15, '0', PadWay.Left);
                case "25":
                    return this.GetFormatText(this.Item25, 7, '0', PadWay.Right);
                case "26":
                    return this.GetFormatText(this.Item26, 1);
                case "27":
                    return this.GetFormatText(this.Item27, 4, ' ', PadWay.Right);
                case "28":
                    return this.GetFormatText(this.Item28, 25, ' ', PadWay.Right);
                case "29":
                    return this.GetFormatText(this.Item29, 1);
                case "30":
                    return this.GetFormatText(this.Item30, 4, ' ', PadWay.Right);

                case "31":
                    return this.GetFormatText(this.Item31, 12, ' ', PadWay.Right);
                case "32":
                    return this.GetFormatText(this.Item32, 10, ' ', PadWay.Right);
                case "33":
                    return this.GetFormatText(this.Item33, 1);
                case "34":
                    return this.GetFormatText(this.Item34, 1, ' ', PadWay.Right);
                case "35":
                    return this.GetFormatText(this.Item35, 1, ' ', PadWay.Right);
                case "36":
                    return this.GetFormatText(this.Item36, 1, ' ', PadWay.Right);
                case "37":
                    return this.GetFormatText(this.Item37, 10, ' ', PadWay.Right);
                case "38":
                    return this.GetFormatText(this.Item38, 10, ' ', PadWay.Right);
                case "39":
                    return this.GetFormatText(this.Item39, 10, ' ', PadWay.Right);
                case "40":
                    return this.GetFormatText(this.Item40, 7, ' ', PadWay.Right);

                case "41":
                    return this.GetFormatText(this.Item41, 90, ' ', PadWay.Right);

                default:
                    return null;
            }
        }

        /// <summary>
        /// 取得 KP3 的資料行
        /// </summary>
        /// <returns></returns>
        public string GetDataLine()
        {
            StringBuilder sb = new StringBuilder();
            string[] itemNos = this.GetItemNos();
            foreach (string itemNo in itemNos)
            {
                string txt = this.GetItemDataText(itemNo);
                if (String.IsNullOrEmpty(txt))
                {
                    return null;
                }
                sb.Append(txt);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 檢查所有項目值
        /// </summary>
        /// <returns></returns>
        public string CheckItemValue()
        {
            #region Item01 資料別
            if (this.Item01 != "10")
            {
                return "『資料別』不正確";
            }
            #endregion

            #region Item02 報送代碼
            if (this.Item02 != "A" && this.Item02 != "C" && this.Item02 != "D")
            {
                return "『報送代碼』不正確";
            }
            #endregion

            #region Item03 電子支付機構代號
            if (!Common.IsEnglishNumber(this.Item03, 3))
            {
                return "『電子支付機構代號』必須為3碼的英文、數字或英數混合";
            }
            #endregion

            #region Item04 空白
            if (!String.IsNullOrWhiteSpace(this.Item04))
            {
                return "『項目04』必須為空白";
            }
            #endregion

            #region Item05 特約機構屬性
            if (this.Item05 != "G" && this.Item05 != "H")
            {
                return "『特約機構屬性』不正確";
            }
            #endregion

            #region Item06 特約機構 BAN/IDN
            if (!Common.IsEnglishNumber(this.Item06, 1, 10))
            {
                return "『特約機構 BAN/IDN』不可空白，且最多10個英文、數字或英數字混合";
            }
            #endregion

            #region Item07 特約機構代號
            if (!Common.IsEnglishNumber(this.Item07, 1, 20))
            {
                return "『特約機構代號』不可空白，且最多20個英文、數字或英數字混合";
            }
            #endregion

            #region Item08 空白
            if (!String.IsNullOrWhiteSpace(this.Item08))
            {
                return "『項目08』必須為空白";
            }
            #endregion

            #region Item09 特約機構類型
            if (this.Item09 != "1" && this.Item09 != "2")
            {
                return "『特約機構類型』不正確";
            }
            #endregion

            #region Item10 負責人/代表人 IDN
            if (!Common.IsEnglishNumber(this.Item10, 1, 10))
            {
                return "『負責人/代表人 IDN』不可空白，且最多10個英文、數字或英數字混合";
            }
            #endregion

            #region Item11 簽約日期
            DateTime item11;
            if (!Common.TryConvertTWDate7(this.Item11, out item11))
            {
                return "『簽約日期』必須為7碼民國年YYYMMDD的日期";
            }
            #endregion

            #region Item12 終止契約種類代號
            {
                string[] values = new string[] { "", "1", "2", "3", "4", "5", "6" };
                if (!values.Contains(this.Item12))
                {
                    return "『終止契約種類代號』不正確";
                }
            }
            #endregion

            #region Item13 終止契約原因代號
            if (String.IsNullOrEmpty(this.Item12))
            {
                if (!String.IsNullOrEmpty(this.Item13))
                {
                    return "無終止契約時不可指定『終止契約原因代號』";
                }
            }
            else
            {
                string msg = null;
                string[] values = null;
                switch (this.Item12)
                {
                    case "1":
                        values = new string[] { "A", "B", "C", "L", "N", "O", "V", "7", "8", "9", "D" };
                        msg = "『終止契約原因代號』為【1】 時『終止契約原因』必須為【A,B,C,L,N,O,V,7,8,9,D】其中之一";
                        break;
                    case "2":
                        values = new string[] { "D", "E", "M" };
                        msg = "『終止契約原因代號』為【1】 時『終止契約原因』必須為【D,E,M】其中之一";
                        break;
                    case "3":
                        values = new string[] { "F", "G" };
                        msg = "『終止契約原因代號』為【1】 時『終止契約原因』必須為【F,G】其中之一";
                        break;
                    case "4":
                        values = new string[] { "H", "I" };
                        msg = "『終止契約原因代號』為【1】 時『終止契約原因』必須為【H,I】其中之一";
                        break;
                    case "5":
                        values = new string[] { "J", "K", "R", "P", "Q", "S", "T", "U", "W", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "X" };
                        msg = "『終止契約原因代號』為【1】 時『終止契約原因』必須為【J,K,R,P,Q,S,T,U,W,Y,Z,1,2,3,4,5,6,7,8,9,A,X】其中之一";
                        break;
                    case "6":
                        values = new string[] { "X" };
                        msg = "『終止契約原因代號』為【1】 時『終止契約原因』必須為【X】";
                        break;
                }
                if (values != null && !values.Contains(this.Item13))
                {
                    return msg;
                }
            }
            #endregion

            #region Item14 終止契約日期
            DateTime? item14 = null;
            if (String.IsNullOrEmpty(this.Item12))
            {
                if (!String.IsNullOrEmpty(this.Item14))
                {
                    return "指定了『終止契約日期』時，必須指定『終止契約種類代號』";
                }
            }
            else
            {
                DateTime date;
                if (Common.TryConvertTWDate7(this.Item14, out date))
                {
                    item14 = date;
                }
                else
                {
                    return "『終止契約日期』必須為7碼民國年YYYMMDD的日期";
                }
            }
            #endregion

            #region Item15 終止契約後有應收未收取款項
            if (String.IsNullOrEmpty(this.Item14))
            {
                if (!String.IsNullOrEmpty(this.Item15))
                {
                    return "未指定『終止契約日期』時，不可指定『終止契約後有應收未收取款項』";
                }
            }
            else
            {
                if (this.Item15 != "Y" && this.Item15 != "N")
                {
                    return "『終止契約後有應收未收取款項』不正確";
                }
            }
            #endregion

            #region Item16 終止契約後應收未收取款項金額
            if (String.IsNullOrEmpty(this.Item14))
            {
                if (!String.IsNullOrEmpty(this.Item16))
                {
                    return "未指定『終止契約日期』時，時不可指定『終止契約後應收未收取款項金額』";
                }
            }
            else if (this.Item15 == "N")
            {
                if (this.Item16 != "0000000000")
                {
                    return "『終止契約後有應收未收取款項』選擇【否】時『終止契約後應收未收取款項金額』必須為 0000000000";
                }
            }
            else if (this.Item15 == "Y")
            {
                if (!Common.IsMoney(this.Item16, 1, 10))
                {
                    return "『終止契約後有應收未收取款項』選擇【是】時『終止契約後應收未收取款項金額』必須為 1 ~ 9999999999 數值";
                }
            }
            #endregion

            Regex _HasEngRex = new Regex("[a-zA-Z]+", RegexOptions.Compiled);
            Encoding encoding = Encoding.GetEncoding("big5");

            #region Item17 登記名稱
            if (String.IsNullOrEmpty(this.Item17) || _HasEngRex.IsMatch(this.Item17) || this.Item17.Length > 30 || encoding.GetByteCount(this.Item17) > 60)
            {
                return "『登記名稱』不可空白且最多30個中英文字（全形），且不可有半形英文";
            }
            #endregion

            #region Item18 登記地址
            if (!String.IsNullOrEmpty(this.Item18) && (_HasEngRex.IsMatch(this.Item18) || this.Item18.Length > 60 || encoding.GetByteCount(this.Item18) > 120))
            {
                return "『登記地址』最多60個中英文字（全形），且不可有半形英文";
            }
            #endregion

            #region Item19 招牌名稱
            if (!String.IsNullOrEmpty(this.Item19) && (_HasEngRex.IsMatch(this.Item19) || this.Item19.Length > 30 || encoding.GetByteCount(this.Item19) > 60))
            {
                return "『招牌名稱』最多30個中英文字（全形），且不可有半形英文";
            }
            #endregion

            #region Item20 營業地址
            if (!String.IsNullOrEmpty(this.Item20) && (_HasEngRex.IsMatch(this.Item20) || this.Item20.Length > 60 || encoding.GetByteCount(this.Item20) > 120))
            {
                return "『營業地址』最多60個中英文字（全形），且不可有半形英文";
            }
            #endregion

            #region Item21 英文名稱
            if (!String.IsNullOrEmpty(this.Item21))
            {
                if (this.Item21.Length > 30 || !Common.IsEnglish(this.Item21.Replace(" ", "")))
                {
                    return "『英文名稱』最多30個英文字";
                }
            }
            #endregion

            #region Item22 空白
            if (!String.IsNullOrWhiteSpace(this.Item22))
            {
                return "『項目22』必須為空白";
            }
            #endregion

            #region Item23 營業型態
            {
                string[] values = new string[] { "1", "2", "3" };
                if (!values.Contains(this.Item23))
                {
                    return "『營業型態』不正確";
                }
            }
            #endregion

            #region Item24 資本額
            if (!Common.IsNumber(this.Item24, 1, 15))
            {
                return "『資本額』不正確";
            }
            #endregion

            #region Item25 設立日期
            DateTime? item25 = null;
            if (this.Item25 != "0000000")
            {
                DateTime date;
                if (Common.TryConvertTWDate7(this.Item25, out date))
                {
                    item25 = date;
                }
                else
                {
                    return "『設立日期』必須為7碼民國年YYYMMDD的日期或 0000000";
                }
            }
            #endregion

            #region Item26 業務行為
            {
                string[] values = new string[] { "1", "2", "3", "4" };
                if (!values.Contains(this.Item26))
                {
                    return "『業務行為』不正確";
                }
            }
            #endregion

            #region Item27 是否受理電子支付帳戶或儲值卡服務
            {
                char[] chars = new char[] { '1', '2', '3', '4' };
                if (String.IsNullOrEmpty(this.Item27))
                {
                    return "『是否受理電子支付帳戶或儲值卡服務』必須有值";
                }
                else
                {
                    List<char> keepChars = new List<char>(4);
                    foreach (var chr in this.Item27)
                    {
                        if (!chars.Contains(chr) || keepChars.Contains(chr))
                        {
                            return "『是否受理電子支付帳戶或儲值卡服務』不正確";
                        }
                        keepChars.Add(chr);
                    }
                    keepChars.Sort();
                    this.Item27 = String.Join("", keepChars);
                }
            }
            #endregion

            #region Item28 空白
            if (!String.IsNullOrWhiteSpace(this.Item28))
            {
                return "『項目28』必須為空白";
            }
            #endregion

            #region Item29 是否受理信用卡服務
            if (this.Item29 != "Y" && this.Item29 != "N")
            {
                return "『是否受理信用卡服務』不正確";
            }
            #endregion

            #region Item30 營業性質
            if (this.Item29 == "Y" && !Common.IsEnglishNumber(this.Item30, 4))
            {
                return "『是否受理信用卡服務』選擇【是】時『營業性質』必須有值且最多4碼字元";
            }
            else if (this.Item29 == "N" && !String.IsNullOrEmpty(this.Item30))
            {
                return "『是否受理信用卡服務』選擇【否】時『營業性質』必須空白";
            }
            #endregion

            #region Item31 受理信用卡別名稱
            if (this.Item29 == "Y")
            {
                char[] chars = new char[] { 'A', 'C', 'D', 'E', 'J', 'M', 'N', 'V', 'O' };
                if (String.IsNullOrEmpty(this.Item31))
                {
                    return "『受理信用卡別名稱』不正確";
                }
                else
                {
                    foreach (var chr in this.Item31)
                    {
                        if (!chars.Contains(chr))
                        {
                            return "『受理信用卡別名稱』不正確";
                        }
                    }
                }
            }
            else if (this.Item29 == "N" && !String.IsNullOrEmpty(this.Item31))
            {
                return "『是否受理信用卡服務』選擇【否】時『受理信用卡別名稱』必須空白";
            }
            #endregion

            #region Item32 空白
            if (!String.IsNullOrWhiteSpace(this.Item32))
            {
                return "『項目32』必須為空白";
            }
            #endregion

            #region Item33 是否有銷售遞延性商品或服務
            if (this.Item33 != "Y" && this.Item33 != "N")
            {
                return "『是否有銷售遞延性商品或服務』不正確";
            }
            #endregion

            #region Item34 是否安裝端末設備
            if (this.Item34 != "" && this.Item34 != "Y" && this.Item34 != "N")
            {
                return "『是否安裝端末設備』不正確";
            }
            #endregion

            #region Item35 是否安裝錄影設備
            if (this.Item35 != "" && this.Item35 != "Y" && this.Item35 != "N")
            {
                return "『是否安裝錄影設備』不正確";
            }
            #endregion

            #region Item36 連鎖店加盟或直營
            if (this.Item36 != "" && this.Item36 != "1" && this.Item36 != "2" && this.Item36 != "3")
            {
                return "『連鎖店加盟或直營』不正確";
            }
            #endregion

            #region Item37 保證人1 IDN/BAN
            if (!Common.IsEnglishNumber(this.Item37, 0, 10))
            {
                return "『保證人1 IDN/BAN』最多10個英文、數字或英數字混合";
            }
            #endregion

            #region Item37 保證人2 IDN/BAN
            if (!Common.IsEnglishNumber(this.Item38, 0, 10))
            {
                return "『保證人2 IDN/BAN』且最多10個英文、數字或英數字混合";
            }
            #endregion

            #region Item39 空白
            if (!String.IsNullOrWhiteSpace(this.Item39))
            {
                return "『項目39』必須為空白";
            }
            #endregion

            #region item40 資料更新日期
            DateTime? item40 = null;
            if (!String.IsNullOrEmpty(this.Item40))
            {
                DateTime date;
                if (Common.TryConvertTWDate7(this.Item40, out date))
                {
                    item40 = date;
                }
                else
                {
                    return "『資料更新日期』必須為7碼民國年YYYMMDD的日期";
                }
            }
            #endregion

            #region Item41 空白
            if (!String.IsNullOrWhiteSpace(this.Item41))
            {
                return "『項目41』必須為空白";
            }
            #endregion

            #region 邏輯檢核 2
            if (item40.HasValue)
            {
                if (item40.Value > DateTime.Today)
                {
                    return "『資料更新日期』不可大於今天";
                }
                if (item25 > item40.Value)
                {
                    return "『設立日期』不可大於『資料更新日期』";
                }
                if (item11 > item40.Value)
                {
                    return "『簽約日期』不可大於『資料更新日期』";
                }
            }
            if (item11 < item25)
            {
                return "『簽約日期』不可小於『設立日期』";
            }
            if (item14.HasValue)
            {
                if (item40.HasValue && item14.Value > item40.Value)
                {
                    return "『終止契約日期』不可大於『資料更新日期』";
                }
                if (item14.Value < item11)
                {
                    return "『終止契約日期』不可小於『簽約日期』";
                }
            }
            #endregion

            #region 邏輯檢核 3
            if (this.Item05 == "G" && this.Item06.Length != 8)
            {
                return "『特約機構 BAN/IDN』不正確";
            }
            if (this.Item05 == "H" && this.Item06.Length != 10)
            {
                return "『特約機構 BAN/IDN』不正確";
            }
            #endregion

            #region 邏輯檢核 4
            if (this.Item05 == "G" && this.Item12 == "1")
            {
                if (this.Item13 == "7" || this.Item13 == "8" || this.Item13 == "9")
                {
                    return "非個人特約機構，不得報送個人特約機構專用之「終止契約原因代號」";
                }
            }
            #endregion

            #region 邏輯檢核 8
            {
                string[] values = null;
                string msg = null;
                switch (this.Item26)
                {
                    case "1":
                        values = new string[] { "1", "3", "13" };
                        msg = "『業務行為』是【1】時『是否受理電子支付帳戶或儲值卡服務』必須是【1,3,13】其中之一";
                        break;
                    case "2":
                        values = new string[] { "2", "4", "24" };
                        msg = "『業務行為』是【2】時『是否受理電子支付帳戶或儲值卡服務』必須是【2,4,24】其中之一";
                        break;
                    case "3":
                        values = new string[] { "12", "14", "23", "34", "123", "124", "134", "234", "1234" };
                        msg = "『業務行為』是【3】時『是否受理電子支付帳戶或儲值卡服務』必須是【12,14,23,34,123,124,134,234,1234】其中之一";
                        break;
                }
                if (values != null && !values.Contains(this.Item27))
                {
                    return msg;
                }
            }
            #endregion

            return null;
        }
        #endregion
    }
}
