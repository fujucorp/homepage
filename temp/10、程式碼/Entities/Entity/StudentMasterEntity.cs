/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/24 13:27:39
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
	/// 學生基本資料承載類別
	/// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class StudentMasterEntity : Entity
    {
        public const string TABLE_NAME = "Student_Master";

        #region Field Name Const Class
        /// <summary>
        /// 學生基本資料 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 代收類別代碼 欄位名稱常數定義
            /// </summary>
            public const string ReceiveType = "Receive_Type";

            /// <summary>
            /// 部別代碼 欄位名稱常數定義
            /// </summary>
            public const string DepId = "Dep_Id";

            /// <summary>
            /// 學號 欄位名稱常數定義
            /// </summary>
            public const string Id = "Stu_Id";
            #endregion

            #region Data
            /// <summary>
            /// 姓名 欄位名稱常數定義
            /// </summary>
            public const string Name = "Stu_Name";

            /// <summary>
            /// 生日 (民國年月日 7 碼) 欄位名稱常數定義
            /// </summary>
            public const string Birthday = "Stu_Birthday";

            /// <summary>
            /// 身分證字號 欄位名稱常數定義
            /// </summary>
            public const string IdNumber = "Id_Number";

            /// <summary>
            /// 電話 欄位名稱常數定義
            /// </summary>
            public const string Tel = "Stu_Tel";

            /// <summary>
            /// 郵遞區號 欄位名稱常數定義
            /// </summary>
            public const string ZipCode = "Stu_Addcode";

            /// <summary>
            /// 地址 欄位名稱常數定義
            /// </summary>
            public const string Address = "Stu_Address";

            /// <summary>
            /// EMail 欄位名稱常數定義
            /// </summary>
            public const string Email = "Stu_Email";

            /// <summary>
            /// Stu_Account (土銀不使用) 欄位名稱常數定義
            /// </summary>
            public const string Account = "Stu_Account";

            /// <summary>
            /// 資料建立日期
            /// </summary>
            public const string CrtDate = "create_date";

            /// <summary>
            /// 資料最後修改日期
            /// </summary>
            public const string MdyDate = "update_date";

            /// <summary>
            /// 家長名稱
            /// </summary>
            public const string StuParent = "Stu_Parent";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構學生基本資料承載類別
        /// </summary>
        public StudentMasterEntity()
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

        private string _DepId = null;
        /// <summary>
        /// 部別代碼
        /// </summary>
        [FieldSpec(Field.DepId, true, FieldTypeEnum.Char, 1, false)]
        public string DepId
        {
            get
            {
                return _DepId;
            }
            set
            {
                _DepId = value == null ? null : value.Trim();
            }
        }

        private string _Id = null;
        /// <summary>
        /// 學號
        /// </summary>
        [FieldSpec(Field.Id, true, FieldTypeEnum.VarChar, 20, false)]
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
        private string _Name = null;
        /// <summary>
        /// 姓名
        /// </summary>
        [FieldSpec(Field.Name, false, FieldTypeEnum.NVarChar, 60, true)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value == null ? null : value.Trim();
            }
        }

        private string _Birthday = null;
        /// <summary>
        /// 生日 (民國年月日 7 碼)
        /// </summary>
        [FieldSpec(Field.Birthday, false, FieldTypeEnum.Char, 7, true)]
        public string Birthday
        {
            get
            {
                return _Birthday;
            }
            set
            {
                _Birthday = value == null ? null : value.Trim();
            }
        }

        private string _IdNumber = null;
        /// <summary>
        /// 身分證字號
        /// </summary>
        [FieldSpec(Field.IdNumber, false, FieldTypeEnum.Char, 12, true)]
        public string IdNumber
        {
            get
            {
                return _IdNumber;
            }
            set
            {
                _IdNumber = value == null ? null : value.Trim();
            }
        }

        private string _Tel = null;
        /// <summary>
        /// 電話
        /// </summary>
        [FieldSpec(Field.Tel, false, FieldTypeEnum.VarChar, 14, true)]
        public string Tel
        {
            get
            {
                return _Tel;
            }
            set
            {
                _Tel = value == null ? null : value.Trim();
            }
        }

        private string _ZipCode = null;
        /// <summary>
        /// 郵遞區號
        /// </summary>
        [FieldSpec(Field.ZipCode, false, FieldTypeEnum.VarChar, 5, true)]
        public string ZipCode
        {
            get
            {
                return _ZipCode;
            }
            set
            {
                _ZipCode = value == null ? null : value.Trim();
            }
        }

        private string _Address = null;
        /// <summary>
        /// 地址
        /// </summary>
        [FieldSpec(Field.Address, false, FieldTypeEnum.VarChar, 100, true)]
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value == null ? null : value.Trim();
            }
        }

        private string _Email = null;
        /// <summary>
        /// EMail
        /// </summary>
        [FieldSpec(Field.Email, false, FieldTypeEnum.VarChar, 50, true)]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value == null ? null : value.Trim();
            }
        }

        private string _Account = null;
        /// <summary>
        /// Stu_Account (土銀不使用) 欄位屬性
        /// </summary>
        [FieldSpec(Field.Account, false, FieldTypeEnum.VarChar, 21, true)]
        public string Account
        {
            get
            {
                return _Account;
            }
            set
            {
                _Account = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 資料建立日期
        /// </summary>
        [FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? CrtDate
        {
            get;
            set;
        }

        /// <summary>
        /// 資料最後修改日期
        /// </summary>
        [FieldSpec(Field.MdyDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? MdyDate
        {
            get;
            set;
        }

        private string _StuParent = null;
        /// <summary>
        /// 家長名稱
        /// </summary>
        [FieldSpec(Field.StuParent, false, FieldTypeEnum.NVarChar, 60, true)]
        public string StuParent
        {
            get
            {
                return _StuParent;
            }
            set
            {
                _StuParent = value == null ? null : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Static Method
        /// <summary>
        /// 取得上傳學生基本資料的 XlsMapField 設定陣列
        /// </summary>
        /// <returns>傳回 XlsMapField 設定陣列</returns>
        public static XlsMapField[] GetXlsMapFields()
        {
            //為了可以清除欄位值，所以除了學號 (PKey) 外要允許長度 0 或空字串

            XlsMapField[] mapFields = new XlsMapField[] {
                new XlsMapField(Field.Id, "學號", new CodeChecker(1, 20)),
                new XlsMapField(Field.Name, "姓名", new WordChecker(0, 60)),
                new XlsMapField(Field.Birthday, "生日", new DateTimeChecker(DateTimeChecker.FormatEnum.DateTextOrEmpty)),
                //土銀不檢查身份證格式，改檢查字元長度限制
                new XlsMapField(Field.IdNumber, "身分證字號", new CharChecker(0, 10)),
                new XlsMapField(Field.Tel, "電話", new CharChecker(0, 14)),
                new XlsMapField(Field.ZipCode, "郵遞區號", new CharChecker(0, 5)),
                new XlsMapField(Field.Address, "地址", new WordChecker(0, 50)),
                new XlsMapField(Field.Email, "EMail", new CharChecker(0, 50)),
                new XlsMapField(Field.StuParent, "家長名稱", new WordChecker(0, 60))
            };

            return mapFields;
        }
        #endregion
    }
}
