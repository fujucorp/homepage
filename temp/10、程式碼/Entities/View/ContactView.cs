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
    public class ContactView : Entity
    {
        protected const string VIEWSQL = @"
select distinct sr.sch_identy sch_identy
      ,sr.sch_name sch_name
	  ,sr.CorpType CorpType
      ,sr.bank_id bank_id
	  ,b.BANKSNAME BANKSNAME
	  ,b.Tel Tel
	  ,b.fax fax
from school_rtype sr LEFT OUTER JOIN bank b ON sr.Bank_Id = b.BANKNO";

        #region Constructor
		/// <summary>
        /// ContactView 類別建構式
		/// </summary>
        public ContactView()
			: base()
		{
		}
		#endregion

        #region Field Name Const Class
        /// <summary>
        /// 模板資料 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 學校編號
            /// </summary>
            public const string sch_identy = "sch_identy";
            #endregion

            #region Data
            /// <summary>
            /// 學校名稱
            /// </summary>
            public const string sch_name = "sch_name";

            /// <summary>
            /// 主辦分行代碼
            /// </summary>
            public const string bank_id = "bank_id";

            /// <summary>
            /// 主辦分行簡稱
            /// </summary>
            public const string BANKSNAME = "BANKSNAME";

            /// <summary>
            /// 
            /// </summary>
            public const string CorpType = "CorpType";

            /// <summary>
            /// 學制、類別的代碼
            /// </summary>
            public const string Tel = "Tel";

            /// <summary>
            /// 
            /// </summary>
            public const string fax = "fax";

            #endregion
        }
        #endregion

        #region Property
        #region PKey
        private string _sch_identy = "";
        /// <summary>
        /// 模板編號
        /// </summary>
        [FieldSpec(Field.sch_identy, true, FieldTypeEnum.VarChar, 8, false)]
        public string sch_identy
        {
            get
            {
                return _sch_identy;
            }
            set
            {
                _sch_identy = value;
            }
        }
        #endregion

        #region Data
        private string _sch_name = null;
        /// <summary>
        /// 模板名稱
        /// </summary>
        [FieldSpec(Field.sch_name, false, FieldTypeEnum.NVarChar, 54, false)]
        public string sch_name
        {
            get
            {
                return _sch_name;
            }
            set
            {
                _sch_name = value == null ? null : value.Trim();
            }
        }

        private string _bank_id = "";
        /// <summary>
        /// 分行代碼
        /// </summary>
        [FieldSpec(Field.bank_id, false, FieldTypeEnum.VarChar, 7, true)]
        public string ReceiveType
        {
            get
            {
                return _bank_id;
            }
            set
            {
                _bank_id = value == null ? null : value.Trim();
            }
        }

        private string _BANKSNAME = "";
        /// <summary>
        /// 分行名稱
        /// </summary>
        [FieldSpec(Field.BANKSNAME, false, FieldTypeEnum.VarChar, 10, true)]
        public string BANKSNAME
        {
            get
            {
                return _BANKSNAME;
            }
            set
            {
                _BANKSNAME = value == null ? null : value.Trim();
            }
        }

        private string _CorpType = "";
        /// <summary>
        /// 學校類別
        /// </summary>
        [FieldSpec(Field.CorpType, false, FieldTypeEnum.VarChar, 3, false)]
        public string CorpType
        {
            get
            {
                return _CorpType;
            }
            set
            {
                _CorpType = value == null ? null : value.Trim();
            }
        }

        private string _Tel = null;
        /// <summary>
        /// 聯絡電話
        /// </summary>
        [FieldSpec(Field.Tel, false, FieldTypeEnum.VarChar, 20, false)]
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

        private string _fax = "";
        /// <summary>
        /// 傳真
        /// </summary>
        [FieldSpec(Field.fax, false, FieldTypeEnum.VarChar, 20, false)]
        public string fax
        {
            get
            {
                return _fax;
            }
            set
            {
                _fax = value == null ? null : value.Trim();
            }
        }
        #endregion

        #endregion
    }
}
