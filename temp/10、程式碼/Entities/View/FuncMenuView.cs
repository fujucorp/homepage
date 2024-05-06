/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2014/11/18 04:40:15
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
	/// 功能選單 View
	/// </summary>
	[Serializable]
	[EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
	public partial class FuncMenuView : Entity
	{
		protected const string VIEWSQL = @"
select Func_Id Func_Id1, Func_Name Func_Name1, '' Func_Id2, '' Func_Name2, '' Func_Id3, '' Func_Name3, status, Sort_No 
  from FuncMenu where Parent_Id=''
union all
select t1.Func_Id Func_Id1, t1.Func_Name Func_Name1, t2.Func_Id Func_Id2, t2.Func_Name Func_Name2, '' Func_Id3, '' Func_Name3, t2.status, t2.Sort_No
  from FuncMenu t1, FuncMenu t2 where t2.Parent_Id=t1.Func_Id and t1.Parent_Id = ''
union all 
select o.x1 Func_Id1, o.x2 Func_Name1, o.x3 Func_Id2, o.x4 Func_Name2, e.Func_Id Func_Id3, e.Func_Name Func_Name3, e.status, e.Sort_No 
  from FuncMenu e 
 inner join (select t1.Func_Id x1, t1.Func_Name x2, t2.Func_Id x3, t2.Func_Name x4
              from FuncMenu t1,FuncMenu t2 where t2.Parent_Id=t1.Func_Id and t1.Parent_Id='') o
       on e.Parent_Id=o.x3 ";

		#region Field Name Const Class
		/// <summary>
		/// FuncMenuView 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey

			#endregion

			#region Data
			/// <summary>
			/// 第一層功能代碼
			/// </summary>
			public const string FuncId1 = "Func_Id1";

			/// <summary>
			/// 第一層功能名稱
			/// </summary>
			public const string FuncName1 = "Func_Name1";

			/// <summary>
			/// 第二層功能代碼
			/// </summary>
			public const string FuncId2 = "Func_Id2";

			/// <summary>
			/// 第二層功能名稱
			/// </summary>
			public const string FuncName2 = "Func_Name2";

			/// <summary>
			/// 第三層功能代碼
			/// </summary>
			public const string FuncId3 = "Func_Id3";

			/// <summary>
			/// 第三層功能名稱
			/// </summary>
			public const string FuncName3 = "Func_Name3";

            /// <summary>
            /// 狀態
            /// </summary>
            public const string Status = "Status";

			/// <summary>
			/// 排序編號
			/// </summary>
			public const string SortNo = "Sort_No";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// FuncMenuView 類別建構式
		/// </summary>
		public FuncMenuView()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey

		#endregion

		#region Data
		private string _FuncId1 = null;
		/// <summary>
		/// 第一層功能代碼
		/// </summary>
		[FieldSpec(Field.FuncId1, false, FieldTypeEnum.VarCharMax, true)]
		public string FuncId1
		{
			get
			{
				return _FuncId1;
			}
			set
			{
				_FuncId1 = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 第一層功能名稱
		/// </summary>
		[FieldSpec(Field.FuncName1, false, FieldTypeEnum.VarCharMax, true)]
		public string FuncName1
		{
			get;
			set;
		}

		private string _FuncId2 = null;
		/// <summary>
		/// 第二層功能代碼
		/// </summary>
		[FieldSpec(Field.FuncId2, false, FieldTypeEnum.VarCharMax, true)]
		public string FuncId2
		{
			get
			{
				return _FuncId2;
			}
			set
			{
				_FuncId2 = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 第二層功能名稱
		/// </summary>
		[FieldSpec(Field.FuncName2, false, FieldTypeEnum.VarCharMax, true)]
		public string FuncName2
		{
			get;
			set;
		}

		private string _FuncId3 = null;
		/// <summary>
		/// 第三層功能代碼
		/// </summary>
		[FieldSpec(Field.FuncId3, false, FieldTypeEnum.VarCharMax, true)]
		public string FuncId3
		{
			get
			{
				return _FuncId3;
			}
			set
			{
				_FuncId3 = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 第三層功能名稱
		/// </summary>
		[FieldSpec(Field.FuncName3, false, FieldTypeEnum.VarCharMax, true)]
		public string FuncName3
		{
			get;
			set;
		}

		private string _Status = null;
		/// <summary>
		/// 狀態
		/// </summary>
		[FieldSpec(Field.Status, false, FieldTypeEnum.VarChar, 3, false)]
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
		/// 排序編號
		/// </summary>
		[FieldSpec(Field.SortNo, false, FieldTypeEnum.Integer, true)]
		public int? SortNo
		{
			get;
			set;
		}
		#endregion
		#endregion

        #region ReadOnly Property
        /// <summary>
        /// 第一層功能代碼與名稱
        /// </summary>
        [XmlIgnore]
        public string Func1IdName
        {
            get
            {
                if (String.IsNullOrEmpty(this.FuncId1))
                {
                    return String.Empty;
                }
                else
                {
                    return string.Format("{0}({1})", this.FuncId1, this.FuncName1);
                }
            }
        }

        /// <summary>
        /// 第二層功能代碼與名稱
        /// </summary>
        [XmlIgnore]
        public string Func2IdName
        {
            get
            {
                if (String.IsNullOrEmpty(this.FuncId2))
                {
                    return String.Empty;
                }
                else
                {
                    return string.Format("{0}({1})", this.FuncId2, this.FuncName2);
                }
            }
        }

        /// <summary>
        /// 第三層功能代碼與名稱
        /// </summary>
        [XmlIgnore]
        public string Func3IdName
        {
            get
            {
                if (String.IsNullOrEmpty(this.FuncId3))
                {
                    return String.Empty;
                }
                else
                {
                    return string.Format("{0}({1})", this.FuncId3, this.FuncName3);
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得 PKey
        /// </summary>
        /// <returns></returns>
        public string GetPKey()
        {
            if (!String.IsNullOrEmpty(this.FuncId3))
            {
                return this.FuncId3;
            }
            else if (!String.IsNullOrEmpty(this.FuncId2))
            {
                return this.FuncId2;
            }
            else
            {
                return this.FuncId1;
            }
        }
        #endregion
    }
}
