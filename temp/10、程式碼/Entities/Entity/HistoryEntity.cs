﻿/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2016/01/19 11:58:43
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
	/// 歷史資料承載類別
	/// </summary>
	[Serializable]
	[EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
	public partial class HistoryEntity : Entity
	{
		public const string TABLE_NAME = "History";

		#region Field Name Const Class
		/// <summary>
		/// HistoryEntity 欄位名稱定義抽象類別
		/// </summary>
		public abstract class Field
		{
			#region PKey
			/// <summary>
			/// 流水號 (Identity)
			/// </summary>
			public const string SN = "sn";
			#endregion

			#region Data
			#region StudentReceive PKey
			/// <summary>
			/// 商家代號
			/// </summary>
			public const string ReceiveType = "Receive_Type";

			/// <summary>
			/// 學年代碼
			/// </summary>
			public const string YearId = "Year_Id";

			/// <summary>
			/// 學期代碼
			/// </summary>
			public const string TermId = "Term_Id";

			/// <summary>
			/// 部別代碼 (土銀不使用，固定為空白)
			/// </summary>
			public const string DepId = "Dep_Id";

			/// <summary>
			/// 代收費用別代碼
			/// </summary>
			public const string ReceiveId = "Receive_Id";

			/// <summary>
			/// 學號
			/// </summary>
			public const string StuId = "Stu_Id";

			/// <summary>
			/// 舊資料序號
			/// </summary>
			public const string OldSeq = "Old_Seq";
			#endregion

			#region Query Key
			/// <summary>
			/// 上傳資料批號
			/// </summary>
			public const string UpNo = "Up_No";

			/// <summary>
			/// 上傳該批資料的順序編號
			/// </summary>
			public const string UpOrder = "Up_Order";

			/// <summary>
			/// 部別代碼
			/// </summary>
			public const string DeptId = "Dept_Id";

			/// <summary>
			/// 院別代碼
			/// </summary>
			public const string CollegeId = "College_Id";

			/// <summary>
			/// 科系代碼
			/// </summary>
			public const string MajorId = "Major_Id";

			/// <summary>
			/// 年級代碼
			/// </summary>
			public const string StuGrade = "Stu_Grade";

			/// <summary>
			/// 班別代碼
			/// </summary>
			public const string ClassId = "Class_Id";

			/// <summary>
			/// 虛擬帳號
			/// </summary>
			public const string CancelNo = "Cancel_No";

			/// <summary>
			/// 應繳金額
			/// </summary>
			public const string ReceiveAmount = "Receive_Amount";

			/// <summary>
			/// 代收日期
			/// </summary>
			public const string ReceiveDate = "Receive_Date";

			/// <summary>
			/// 入帳日期
			/// </summary>
			public const string AccountDate = "Account_Date";

			/// <summary>
			/// 代收管道代碼
			/// </summary>
			public const string ReceiveWay = "Receive_Way";

			/// <summary>
			/// 學校代碼
			/// </summary>
			public const string SchIdenty = "Sch_Identy";

			/// <summary>
			/// 學校所屬分行代碼
			/// </summary>
			public const string SchBankId = "Sch_Bank_Id";
			#endregion

			#region 代碼名稱
			/// <summary>
			/// 學期名稱
			/// </summary>
			public const string TermName = "Term_Name";

			/// <summary>
			/// 代收費用別名稱
			/// </summary>
			public const string ReceiveName = "Receive_Name";

			/// <summary>
			/// 學生名稱
			/// </summary>
			public const string StuName = "Stu_Name";

			/// <summary>
			/// 部別名稱
			/// </summary>
			public const string DeptName = "Dept_Name";

			/// <summary>
			/// 院別名稱
			/// </summary>
			public const string CollegeName = "College_Name";

			/// <summary>
			/// 科系名稱
			/// </summary>
			public const string MajorName = "Major_Name";

			/// <summary>
			/// 班別名稱
			/// </summary>
			public const string ClassName = "Class_Name";

			/// <summary>
			/// 代收管道名稱
			/// </summary>
			public const string ReceiveWayName = "Receive_Way_Name";
			#endregion

			/// <summary>
			/// 商家代號(學校)資料XML
			/// </summary>
			public const string SchoolXml = "School_Xml";

			/// <summary>
			/// 學生資料XML
			/// </summary>
			public const string StudentXml = "Student_Xml";

			/// <summary>
			/// 費用別資料XML
			/// </summary>
			public const string SchoolRidXml = "School_Rid_Xml";

			/// <summary>
			/// 繳費資料XML
			/// </summary>
			public const string StudentReceiveXml = "Student_Receive_Xml";

			/// <summary>
			/// 小計資料
			/// </summary>
			public const string ReceiveSumData = "ReceiveSum_Data";

			/// <summary>
			/// 繳費資料修改日期
			/// </summary>
			public const string StudentReceiveMdyDate = "Student_Receive_MdyDate";

			/// <summary>
			/// 歷史資料建立日期
			/// </summary>
			public const string CrtDate = "crt_date";
			#endregion
		}
		#endregion

		#region Constructor
		/// <summary>
		/// HistoryEntity 類別建構式
		/// </summary>
		public HistoryEntity()
			: base()
		{
		}
		#endregion

		#region Property
		#region PKey
		/// <summary>
		/// 流水號 (Identity)
		/// </summary>
		[FieldSpec(Field.SN, true, FieldTypeEnum.Identity, false)]
		public Int64 SN
		{
			get;
			set;
		}
		#endregion

		#region Data
		#region StudentReceive PKey
		private string _ReceiveType = null;
		/// <summary>
		/// 商家代號
		/// </summary>
		[FieldSpec(Field.ReceiveType, false, FieldTypeEnum.VarChar, 6, false)]
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

		private string _YearId = null;
		/// <summary>
		/// 學年代碼
		/// </summary>
		[FieldSpec(Field.YearId, false, FieldTypeEnum.VarChar, 3, false)]
		public string YearId
		{
			get
			{
				return _YearId;
			}
			set
			{
				_YearId = value == null ? null : value.Trim();
			}
		}

		private string _TermId = null;
		/// <summary>
		/// 學期代碼
		/// </summary>
		[FieldSpec(Field.TermId, false, FieldTypeEnum.Char, 1, false)]
		public string TermId
		{
			get
			{
				return _TermId;
			}
			set
			{
				_TermId = value == null ? null : value.Trim();
			}
		}

		private string _DepId = null;
		/// <summary>
		/// 部別代碼 (土銀不使用，固定為空白)
		/// </summary>
		[FieldSpec(Field.DepId, false, FieldTypeEnum.Char, 1, false)]
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

		private string _ReceiveId = null;
		/// <summary>
		/// 代收費用別代碼
		/// </summary>
		[FieldSpec(Field.ReceiveId, false, FieldTypeEnum.VarChar, 2, false)]
		public string ReceiveId
		{
			get
			{
				return _ReceiveId;
			}
			set
			{
				_ReceiveId = value == null ? null : value.Trim();
			}
		}

		private string _StuId = null;
		/// <summary>
		/// 學號
		/// </summary>
		[FieldSpec(Field.StuId, false, FieldTypeEnum.VarChar, 20, false)]
		public string StuId
		{
			get
			{
				return _StuId;
			}
			set
			{
				_StuId = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 舊資料序號
		/// </summary>
		[FieldSpec(Field.OldSeq, false, FieldTypeEnum.Integer, false)]
		public int OldSeq
		{
			get;
			set;
		}
		#endregion

		#region Query Key
		private string _UpNo = null;
		/// <summary>
		/// 上傳資料批號
		/// </summary>
		[FieldSpec(Field.UpNo, false, FieldTypeEnum.VarChar, 4, false)]
		public string UpNo
		{
			get
			{
				return _UpNo;
			}
			set
			{
				_UpNo = value == null ? String.Empty : value.Trim();
			}
		}

		private string _UpOrder = null;
		/// <summary>
		/// 上傳該批資料的順序編號
		/// </summary>
		[FieldSpec(Field.UpOrder, false, FieldTypeEnum.VarChar, 6, false)]
		public string UpOrder
		{
			get
			{
				return _UpOrder;
			}
			set
			{
				_UpOrder = value == null ? String.Empty : value.Trim();
			}
		}

		private string _DeptId = null;
		/// <summary>
		/// 部別代碼
		/// </summary>
		[FieldSpec(Field.DeptId, false, FieldTypeEnum.NVarChar, 20, true)]
		public string DeptId
		{
			get
			{
				return _DeptId;
			}
			set
			{
				_DeptId = value == null ? null : value.Trim();
			}
		}

		private string _CollegeId = null;
		/// <summary>
		/// 院別代碼
		/// </summary>
		[FieldSpec(Field.CollegeId, false, FieldTypeEnum.NVarChar, 20, true)]
		public string CollegeId
		{
			get
			{
				return _CollegeId;
			}
			set
			{
				_CollegeId = value == null ? null : value.Trim();
			}
		}

		#region [MDY:20200810] M202008_02 科系名稱長度放大到40個中文字
		private string _MajorId = null;
		/// <summary>
		/// 科系代碼
		/// </summary>
		[FieldSpec(Field.MajorId, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MajorId
		{
			get
			{
				return _MajorId;
			}
			set
			{
				_MajorId = value == null ? null : value.Trim();
			}
		}
		#endregion

		private string _StuGrade = null;
		/// <summary>
		/// 年級代碼
		/// </summary>
		[FieldSpec(Field.StuGrade, false, FieldTypeEnum.VarChar, 2, true)]
		public string StuGrade
		{
			get
			{
				return _StuGrade;
			}
			set
			{
				_StuGrade = value == null ? null : value.Trim();
			}
		}

		private string _ClassId = null;
		/// <summary>
		/// 班別代碼
		/// </summary>
		[FieldSpec(Field.ClassId, false, FieldTypeEnum.NVarChar, 20, true)]
		public string ClassId
		{
			get
			{
				return _ClassId;
			}
			set
			{
				_ClassId = value == null ? null : value.Trim();
			}
		}

		private string _CancelNo = null;
		/// <summary>
		/// 虛擬帳號
		/// </summary>
		[FieldSpec(Field.CancelNo, false, FieldTypeEnum.VarChar, 16, true)]
		public string CancelNo
		{
			get
			{
				return _CancelNo;
			}
			set
			{
				_CancelNo = value == null ? null : value.Trim();
			}
		}

		/// <summary>
		/// 應繳金額
		/// </summary>
		[FieldSpec(Field.ReceiveAmount, false, FieldTypeEnum.Decimal, true)]
		public decimal? ReceiveAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 代收日期
		/// </summary>
		[FieldSpec(Field.ReceiveDate, false, FieldTypeEnum.Char, 7, true)]
		public string ReceiveDate
		{
			get;
			set;
		}

		/// <summary>
		/// 入帳日期
		/// </summary>
		[FieldSpec(Field.AccountDate, false, FieldTypeEnum.Char, 7, true)]
		public string AccountDate
		{
			get;
			set;
		}

		private string _ReceiveWay = null;
		/// <summary>
		/// 代收管道代碼
		/// </summary>
		[FieldSpec(Field.ReceiveWay, false, FieldTypeEnum.VarChar, 4, true)]
		public string ReceiveWay
		{
			get
			{
				return _ReceiveWay;
			}
			set
			{
				_ReceiveWay = value == null ? null : value.Trim();
			}
		}

		private string _SchIdenty = null;
		/// <summary>
		/// 學校代碼
		/// </summary>
		[FieldSpec(Field.SchIdenty, false, FieldTypeEnum.VarChar, 10, false)]
		public string SchIdenty
		{
			get
			{
				return _SchIdenty;
			}
			set
			{
				_SchIdenty = value == null ? null : value.Trim();
			}
		}

		private string _SchBankId = null;
		/// <summary>
		/// 學校所屬分行代碼
		/// </summary>
		[FieldSpec(Field.SchBankId, false, FieldTypeEnum.Char, 7, false)]
		public string SchBankId
		{
			get
			{
				return _SchBankId;
			}
			set
			{
				_SchBankId = value == null ? null : value.Trim();
			}
		}
		#endregion

		#region 代碼名稱
		/// <summary>
		/// 學期名稱
		/// </summary>
		[FieldSpec(Field.TermName, false, FieldTypeEnum.NVarChar, 20, true)]
		public string TermName
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

		/// <summary>
		/// 學生名稱
		/// </summary>
		[FieldSpec(Field.StuName, false, FieldTypeEnum.NVarChar, 60, true)]
		public string StuName
		{
			get;
			set;
		}

		/// <summary>
		/// 部別名稱
		/// </summary>
		[FieldSpec(Field.DeptName, false, FieldTypeEnum.NVarChar, 20, true)]
		public string DeptName
		{
			get;
			set;
		}

		/// <summary>
		/// 院別名稱
		/// </summary>
		[FieldSpec(Field.CollegeName, false, FieldTypeEnum.NVarChar, 40, true)]
		public string CollegeName
		{
			get;
			set;
		}

		/// <summary>
		/// 科系名稱
		/// </summary>
		[FieldSpec(Field.MajorName, false, FieldTypeEnum.NVarChar, 40, true)]
		public string MajorName
		{
			get;
			set;
		}

		/// <summary>
		/// 班別名稱
		/// </summary>
		[FieldSpec(Field.ClassName, false, FieldTypeEnum.NVarChar, 40, true)]
		public string ClassName
		{
			get;
			set;
		}

		/// <summary>
		/// 代收管道名稱
		/// </summary>
		[FieldSpec(Field.ReceiveWayName, false, FieldTypeEnum.NVarChar, 50, true)]
		public string ReceiveWayName
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// 商家代號(學校)資料XML
		/// </summary>
		[FieldSpec(Field.SchoolXml, false, FieldTypeEnum.NVarCharMax, false)]
		public string SchoolXml
		{
			get;
			set;
		}

		/// <summary>
		/// 學生資料XML
		/// </summary>
		[FieldSpec(Field.StudentXml, false, FieldTypeEnum.NVarChar, 4000, false)]
		public string StudentXml
		{
			get;
			set;
		}

		/// <summary>
		/// 費用別資料XML
		/// </summary>
		[FieldSpec(Field.SchoolRidXml, false, FieldTypeEnum.NVarCharMax, false)]
		public string SchoolRidXml
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費資料XML
		/// </summary>
		[FieldSpec(Field.StudentReceiveXml, false, FieldTypeEnum.NVarCharMax, false)]
		public string StudentReceiveXml
		{
			get;
			set;
		}

		/// <summary>
		/// 小計資料
		/// </summary>
		[FieldSpec(Field.ReceiveSumData, false, FieldTypeEnum.NVarChar, 4000, false)]
		public string ReceiveSumData
		{
			get;
			set;
		}

		/// <summary>
		/// 繳費資料修改日期
		/// </summary>
		[FieldSpec(Field.StudentReceiveMdyDate, false, FieldTypeEnum.DateTime, true)]
		public DateTime? StudentReceiveMdyDate
		{
			get;
			set;
		}

		/// <summary>
		/// 歷史資料建立日期
		/// </summary>
		[FieldSpec(Field.CrtDate, false, FieldTypeEnum.DateTime, false)]
		public DateTime CrtDate
		{
			get;
			set;
		}
		#endregion
		#endregion
	}
}
