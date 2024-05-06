using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Fuju.DB;
using Fuju.DB.Data;

namespace Entities
{
    /// <summary>
    /// JobCube View
    /// </summary>
    [Serializable]
    [EntitySpec(VIEWSQL, TableTypeEnum.ViewSql)]
    public partial class JobCubeView : Entity
    {
        protected const string VIEWSQL = @"
SELECT * 
     , ISNULL((SELECT Year_Name    FROM Year_List    AS Y WHERE Y.Year_Id = J.JYEAR), '') AS YearName
     , ISNULL((SELECT Term_Name    FROM Term_List    AS T WHERE T.Receive_Type = J.JRID AND T.Year_Id = J.JYEAR AND T.Term_Id = J.JTERM), '') AS TermName
     , ISNULL((SELECT Dep_Name     FROM Dep_List     AS D WHERE D.Receive_Type = J.JRID AND D.Year_Id = J.JYEAR AND D.Term_Id = J.JTERM AND D.Dep_Id = J.JDEP), '') AS DepName
     , ISNULL((SELECT Receive_Name FROM Receive_List AS R WHERE R.Receive_Type = J.JRID AND R.Year_Id = J.JYEAR AND R.Term_Id = J.JTERM AND R.Dep_Id = J.JDEP AND R.Receive_Id = J.JRECID), '') AS ReceiveName
  FROM JobCube AS J";

        #region Field Name Const Class
        /// <summary>
        /// JobcubeEntity 欄位名稱定義抽象類別
        /// </summary>
        public abstract class Field
        {
            #region PKey
            /// <summary>
            /// 序號 欄位名稱常數定義 (PK) (Identity)
            /// </summary>
            public const string Jno = "JNO";
            #endregion

            #region Data
            /// <summary>
            /// 處理開始使間 欄位名稱常數定義
            /// </summary>
            public const string Jstd = "JSTD";

            /// <summary>
            /// 處理結束時間 欄位名稱常數定義
            /// </summary>
            public const string Jetd = "JETD";

            /// <summary>
            /// 處理的DLL 欄位名稱常數定義 (BUA：clsUploadDataBUA.dll BUB：clsUploadDataBUB.dll BUC：clsUploadDataBUC.dll BUD：clsUploadDataBUD.dll BUE：clsUploadDataBUE.dll)
            /// </summary>
            public const string Jdll = "JDLL";

            /// <summary>
            /// 處理的類別 欄位名稱常數定義 (BUA：clsUploadDataBUA.Mapping BUB：clsUploadDataBUB.Mapping BUC：clsUploadDataBUC.Mapping BUD：clsUploadDataBUD.Mapping BUE：clsUploadDataBUE.Mapping)
            /// </summary>
            public const string Jclass = "JCLASS";

            /// <summary>
            /// 處理的方法 欄位名稱常數定義 (BUA：上傳學生繳費資料 BUB：上傳學分費退費資料 BUC：上傳課程收費標準 BUD：上傳已產生銷帳編號就貸資料 BUE：上傳已產生銷帳編號減免資料)
            /// </summary>
            public const string Jmethod = "JMETHOD";

            /// <summary>
            /// 參數 欄位名稱常數定義 (發動者帳號+代收類別+年度代碼+學期代碼+對照檔代碼+部別代碼+費用別代碼+檔案名稱+要寫入的資料表名稱+log路徑+批號+作業類別代碼+bankPM.cancel+sheet_name 用","格開)
            /// </summary>
            public const string Jparam = "JPARAM";

            /// <summary>
            /// Process ID 欄位名稱常數定義
            /// </summary>
            public const string Jpid = "JPID";

            /// <summary>
            /// 發動者帳號 欄位名稱常數定義
            /// </summary>
            public const string Jowner = "JOWNER";

            /// <summary>
            /// 商家代號 欄位名稱常數定義
            /// </summary>
            public const string Jrid = "JRID";

            /// <summary>
            /// 年度代碼 欄位名稱常數定義
            /// </summary>
            public const string Jyear = "JYEAR";

            /// <summary>
            /// 期別代碼 欄位名稱常數定義
            /// </summary>
            public const string Jterm = "JTERM";

            /// <summary>
            /// 部別代碼 欄位名稱常數定義
            /// </summary>
            public const string Jdep = "JDEP";

            /// <summary>
            /// 費用別代碼 欄位名稱常數定義
            /// </summary>
            public const string Jrecid = "JRECID";

            /// <summary>
            /// 優先順序 欄位名稱常數定義
            /// </summary>
            public const string Jprity = "JPRITY";

            /// <summary>
            /// 作業類別代碼 欄位名稱常數定義 (BUA：上傳學生繳費資料 BUB：上傳學分費退費資料 BUC：上傳課程收費標準 BUD：上傳已產生銷帳編號就貸資料 BUE：上傳已產生銷帳編號減免資料)
            /// </summary>
            public const string Jtypeid = "JTYPEID";

            /// <summary>
            /// 處理狀態 欄位名稱常數定義
            /// </summary>
            public const string Jstatusid = "JSTATUSID";

            /// <summary>
            /// 處理結果 欄位名稱常數定義
            /// </summary>
            public const string Jresultid = "JRESULTID";

            /// <summary>
            /// log 欄位名稱常數定義
            /// </summary>
            public const string Jlog = "JLOG";

            /// <summary>
            /// 建立日期 欄位名稱常數定義
            /// </summary>
            public const string CDate = "C_Date";

            /// <summary>
            /// 異動日期 欄位名稱常數定義
            /// </summary>
            public const string MDate = "M_Date";

            /// <summary>
            /// 批號 欄位名稱常數定義
            /// </summary>
            public const string SeriorNo = "Serior_No";

            /// <summary>
            /// 備註 欄位名稱常數定義
            /// </summary>
            public const string Memo = "Memo";

            /// <summary>
            /// 檔案庫序號 欄位名稱常數定義
            /// </summary>
            public const string Chancel = "chancel";
            #endregion

            #region Extra Data
            /// <summary>
            /// 學年名稱 欄位名稱常數定義
            /// </summary>
            public const string YearName = "YearName";

            /// <summary>
            /// 學期名稱 欄位名稱常數定義
            /// </summary>
            public const string TermName = "TermName";

            /// <summary>
            /// 部別名稱 欄位名稱常數定義
            /// </summary>
            public const string DepName = "DepName";

            /// <summary>
            /// 代收費用別名稱 欄位名稱常數定義
            /// </summary>
            public const string ReceiveName = "ReceiveName";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// JobCubeView 類別建構式
        /// </summary>
        public JobCubeView()
            : base()
        {
        }
        #endregion

        #region Property
        #region PKey
        /// <summary>
        /// 序號 (PK) (Identity)
        /// </summary>
        [FieldSpec(Field.Jno, true, FieldTypeEnum.Identity, false)]
        public int Jno
        {
            get;
            set;
        }
        #endregion

        #region Data
        /// <summary>
        /// 處理開始使間
        /// </summary>
        [FieldSpec(Field.Jstd, false, FieldTypeEnum.DateTime, true)]
        public DateTime? Jstd
        {
            get;
            set;
        }

        /// <summary>
        /// 處理結束時間
        /// </summary>
        [FieldSpec(Field.Jetd, false, FieldTypeEnum.DateTime, true)]
        public DateTime? Jetd
        {
            get;
            set;
        }

        /// <summary>
        /// 處理的DLL
        /// </summary>
        [FieldSpec(Field.Jdll, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jdll
        {
            get;
            set;
        }

        /// <summary>
        /// 處理的類別
        /// </summary>
        [FieldSpec(Field.Jclass, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jclass
        {
            get;
            set;
        }

        /// <summary>
        /// 處理的方法
        /// </summary>
        [FieldSpec(Field.Jmethod, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jmethod
        {
            get;
            set;
        }

        /// <summary>
        /// 參數 (發動者帳號+代收類別+年度代碼+學期代碼+對照檔代碼+部別代碼+費用別代碼+檔案名稱+要寫入的資料表名稱+log路徑+批號+作業類別代碼+bankPM.cancel+sheet_name 用","格開)
        /// </summary>
        [FieldSpec(Field.Jparam, false, FieldTypeEnum.VarCharMax, true)]
        public string Jparam
        {
            get;
            set;
        }

        /// <summary>
        /// Process ID
        /// </summary>
        [FieldSpec(Field.Jpid, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jpid
        {
            get;
            set;
        }

        /// <summary>
        /// 發動者帳號
        /// </summary>
        [FieldSpec(Field.Jowner, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jowner
        {
            get;
            set;
        }

        /// <summary>
        /// 商家代號
        /// </summary>
        [FieldSpec(Field.Jrid, false, FieldTypeEnum.VarChar, 6, true)]
        public string Jrid
        {
            get;
            set;
        }

        /// <summary>
        /// 年度代碼
        /// </summary>
        [FieldSpec(Field.Jyear, false, FieldTypeEnum.VarChar, 3, true)]
        public string Jyear
        {
            get;
            set;
        }

        /// <summary>
        /// 期別代碼
        /// </summary>
        [FieldSpec(Field.Jterm, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jterm
        {
            get;
            set;
        }

        /// <summary>
        /// 部別代碼
        /// </summary>
        [FieldSpec(Field.Jdep, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jdep
        {
            get;
            set;
        }

        /// <summary>
        /// 費用別代碼
        /// </summary>
        [FieldSpec(Field.Jrecid, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jrecid
        {
            get;
            set;
        }

        /// <summary>
        /// 優先順序
        /// </summary>
        [FieldSpec(Field.Jprity, false, FieldTypeEnum.Integer, true)]
        public int? Jprity
        {
            get;
            set;
        }

        /// <summary>
        /// 作業類別代碼 (BUA：上傳學生繳費資料 BUB：上傳學分費退費資料 BUC：上傳課程收費標準 BUD：上傳已產生銷帳編號就貸資料 BUE：上傳已產生銷帳編號減免資料)
        /// </summary>
        [FieldSpec(Field.Jtypeid, false, FieldTypeEnum.VarChar, 3, true)]
        public string Jtypeid
        {
            get;
            set;
        }

        /// <summary>
        /// 處理狀態
        /// </summary>
        [FieldSpec(Field.Jstatusid, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jstatusid
        {
            get;
            set;
        }

        /// <summary>
        /// 處理結果
        /// </summary>
        [FieldSpec(Field.Jresultid, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jresultid
        {
            get;
            set;
        }

        /// <summary>
        /// log
        /// </summary>
        [FieldSpec(Field.Jlog, false, FieldTypeEnum.VarCharMax, true)]
        public string Jlog
        {
            get;
            set;
        }

        /// <summary>
        /// 建立日期
        /// </summary>
        [FieldSpec(Field.CDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? CDate
        {
            get;
            set;
        }

        /// <summary>
        /// 異動日期
        /// </summary>
        [FieldSpec(Field.MDate, false, FieldTypeEnum.DateTime, true)]
        public DateTime? MDate
        {
            get;
            set;
        }

        /// <summary>
        /// 批號 (by JRID + JYEAR + JTERM + JDEP + JRECID)
        /// </summary>
        [FieldSpec(Field.SeriorNo, false, FieldTypeEnum.VarChar, 4, true)]
        public string SeriorNo
        {
            get;
            set;
        }

        /// <summary>
        /// 備註
        /// </summary>
        [FieldSpec(Field.Memo, false, FieldTypeEnum.VarChar, 200, true)]
        public string Memo
        {
            get;
            set;
        }

        /// <summary>
        /// 檔案庫序號
        /// </summary>
        [FieldSpec(Field.Chancel, false, FieldTypeEnum.Char, 10, true)]
        public string Chancel
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Extra Data
        /// <summary>
        /// 部別名稱
        /// </summary>
        [FieldSpec(Field.YearName, false, FieldTypeEnum.VarChar, 8, true)]
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
        [FieldSpec(Field.DepName, false, FieldTypeEnum.VarChar, 20, true)]
        public string DepName
        {
            get;
            set;
        }

        /// <summary>
        /// 代收費用別名稱
        /// </summary>
        [FieldSpec(Field.ReceiveName, false, FieldTypeEnum.VarChar, 40, true)]
        public string ReceiveName
        {
            get;
            set;
        }
        #endregion
    }
}
