/*
Entity Version: 3.0
程式產生器：Entity 類別程式碼產生器 (For FujuV3.0) 
產生日期時間：2015/01/26 10:42:58
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
	/// 批次處理佇列 資料承載類別
	/// </summary>
    [Serializable]
    [EntitySpec(TABLE_NAME, TableTypeEnum.Table)]
    public partial class JobcubeEntity : Entity
    {
        public const string TABLE_NAME = "JobCube";

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
            /// 處理開始時間 欄位名稱常數定義
            /// </summary>
            public const string Jstd = "JSTD";

            /// <summary>
            /// 處理結束時間 欄位名稱常數定義
            /// </summary>
            public const string Jetd = "JETD";

            /// <summary>
            /// 處理的DLL (土銀沒作用) 欄位名稱常數定義 (BUA：clsUploadDataBUA.dll BUB：clsUploadDataBUB.dll BUC：clsUploadDataBUC.dll BUD：clsUploadDataBUD.dll BUE：clsUploadDataBUE.dll)
            /// </summary>
            public const string Jdll = "JDLL";

            /// <summary>
            /// 處理的類別 (土銀沒作用) 欄位名稱常數定義 (BUA：clsUploadDataBUA.Mapping BUB：clsUploadDataBUB.Mapping BUC：clsUploadDataBUC.Mapping BUD：clsUploadDataBUD.Mapping BUE：clsUploadDataBUE.Mapping)
            /// </summary>
            public const string Jclass = "JCLASS";

            /// <summary>
            /// 處理的方法 (土銀沒作用) 欄位名稱常數定義
            /// </summary>
            public const string Jmethod = "JMETHOD";

            /// <summary>
            /// 處理的參數 (不同作業有不同的參數格式，參考 Static Method) 欄位名稱常數定義
            /// </summary>
            public const string Jparam = "JPARAM";

            /// <summary>
            /// Process ID (土銀用來存放執行作業的伺服器名稱) 欄位名稱常數定義
            /// </summary>
            public const string Jpid = "JPID";

            /// <summary>
            /// 發動者帳號 (建立此作業的使用者帳號或 SYSTEM) 欄位名稱常數定義
            /// </summary>
            public const string Jowner = "JOWNER";

            /// <summary>
            /// 商家代號 (索引用，參數以 Jparam 為準) 欄位名稱常數定義
            /// </summary>
            public const string Jrid = "JRID";

            /// <summary>
            /// 年度代碼 (索引用，參數以 Jparam 為準) 欄位名稱常數定義
            /// </summary>
            public const string Jyear = "JYEAR";

            /// <summary>
            /// 期別代碼 (索引用，參數以 Jparam 為準) 欄位名稱常數定義
            /// </summary>
            public const string Jterm = "JTERM";

            /// <summary>
            /// 部別代碼 (索引用，參數以 Jparam 為準) 欄位名稱常數定義
            /// </summary>
            public const string Jdep = "JDEP";

            /// <summary>
            /// 費用別代碼 (索引用，參數以 Jparam 為準) 欄位名稱常數定義
            /// </summary>
            public const string Jrecid = "JRECID";

            /// <summary>
            /// 優先順序 (土銀不使用，固定為 0) 欄位名稱常數定義
            /// </summary>
            public const string Jprity = "JPRITY";

            /// <summary>
            /// 作業類別代碼 欄位名稱常數定義 (請參考 JobCubeTypeCodeTexts)
            /// (BUA：上傳學生繳費資料 / BUB：上傳學分費退費資料 / BUC：上傳課程收費標準 / BUD：上傳已產生銷帳編號就貸資料 / BUE：上傳已產生銷帳編號減免資料)
            /// (BUF：上傳簡易學生繳費資料 / BUG：上傳學生基本資料)
            /// (CP：產生繳費金額 / CI：產生銷帳編號)
            /// </summary>
            public const string Jtypeid = "JTYPEID";

            /// <summary>
            /// 處理狀態代碼 欄位名稱常數定義 (請參考 JobCubeStatusCodeTexts)
            /// </summary>
            public const string Jstatusid = "JSTATUSID";

            /// <summary>
            /// 處理結果代碼 欄位名稱常數定義 (請參考 JobCubeResultCodeTexts)
            /// </summary>
            public const string Jresultid = "JRESULTID";

            /// <summary>
            /// 處理日誌 欄位名稱常數定義
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
            /// 備註 (待處理：空字串, 處理成功：處理成功 / 處裡中：戳記 / 處理失敗：錯誤訊息) 欄位名稱常數定義
            /// </summary>
            public const string Memo = "Memo";

            /// <summary>
            /// 檔案庫序號 (bankPM.cancel) 欄位名稱常數定義
            /// </summary>
            public const string Chancel = "chancel";
            #endregion
        }
        #endregion

        #region Constructor
        /// <summary>
        /// JobcubeEntity 類別建構式
        /// </summary>
        public JobcubeEntity()
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

        private string _JDll = String.Empty;
        /// <summary>
        /// 處理的DLL (土銀沒作用) (BUA：clsUploadDataBUA.dll BUB：clsUploadDataBUB.dll BUC：clsUploadDataBUC.dll BUD：clsUploadDataBUD.dll BUE：clsUploadDataBUE.dll)
        /// </summary>
        [FieldSpec(Field.Jdll, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jdll
        {
            get
            {
                return _JDll;
            }
            set
            {
                _JDll = value == null ? String.Empty : value.Trim();
            }
        }

        private string _JClass = String.Empty;
        /// <summary>
        /// 處理的類別 (土銀沒作用) (BUA：clsUploadDataBUA.Mapping BUB：clsUploadDataBUB.Mapping BUC：clsUploadDataBUC.Mapping BUD：clsUploadDataBUD.Mapping BUE：clsUploadDataBUE.Mapping)
        /// </summary>
        [FieldSpec(Field.Jclass, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jclass
        {
            get
            {
                return _JClass;
            }
            set
            {
                _JClass = value == null ? String.Empty : value.Trim();
            }
        }

        private string _JMethod = String.Empty;
        /// <summary>
        /// 處理的方法 (土銀沒作用)
        /// </summary>
        [FieldSpec(Field.Jmethod, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jmethod
        {
            get
            {
                return _JMethod;
            }
            set
            {
                _JMethod = value == null ? String.Empty : value.Trim();
            }
        }

        private string _JParam = String.Empty;
        /// <summary>
        /// 處理的參數 (不同作業有不同的參數格式，參考 Static Method)
        /// </summary>
        [FieldSpec(Field.Jparam, false, FieldTypeEnum.VarCharMax, true)]
        public string Jparam
        {
            get
            {
                return _JParam;
            }
            set
            {
                _JParam = value == null ? String.Empty : value.Trim();
            }
        }

        private string _JPid = String.Empty;
        /// <summary>
        /// Process ID (土銀用來存放執行作業的伺服器名稱)
        /// </summary>
        [FieldSpec(Field.Jpid, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jpid
        {
            get
            {
                return _JPid;
            }
            set
            {
                _JPid = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Jowner = String.Empty;
        /// <summary>
        /// 發動者帳號 (建立此作業的使用者帳號或 SYSTEM)
        /// </summary>
        [FieldSpec(Field.Jowner, false, FieldTypeEnum.VarChar, 50, true)]
        public string Jowner
        {
            get
            {
                return _Jowner;
            }
            set
            {
                _Jowner = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Jrid = String.Empty;
        /// <summary>
        /// 商家代號 (索引用，參數以 Jparam 為準)
        /// </summary>
        [FieldSpec(Field.Jrid, false, FieldTypeEnum.VarChar, 6, true)]
        public string Jrid
        {
            get
            {
                return _Jrid;
            }
            set
            {
                _Jrid = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Jyear = String.Empty;
        /// <summary>
        /// 年度代碼 (索引用，參數以 Jparam 為準)
        /// </summary>
        [FieldSpec(Field.Jyear, false, FieldTypeEnum.VarChar, 3, true)]
        public string Jyear
        {
            get
            {
                return _Jyear;
            }
            set
            {
                _Jyear = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Jterm = String.Empty;
        /// <summary>
        /// 期別代碼 (索引用，參數以 Jparam 為準)
        /// </summary>
        [FieldSpec(Field.Jterm, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jterm
        {
            get
            {
                return _Jterm;
            }
            set
            {
                _Jterm = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Jdep = String.Empty;
        /// <summary>
        /// 部別代碼 (索引用，參數以 Jparam 為準)
        /// </summary>
        [FieldSpec(Field.Jdep, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jdep
        {
            get
            {
                return _Jdep;
            }
            set
            {
                _Jdep = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Jrecid = String.Empty;
        /// <summary>
        /// 費用別代碼 (索引用，參數以 Jparam 為準)
        /// </summary>
        [FieldSpec(Field.Jrecid, false, FieldTypeEnum.VarChar, 2, true)]
        public string Jrecid
        {
            get
            {
                return _Jrecid;
            }
            set
            {
                _Jrecid = value == null ? String.Empty : value.Trim();
            }
        }

        private int? JPrity = 0;
        /// <summary>
        /// 優先順序 (土銀不使用，固定為 0)
        /// </summary>
        [FieldSpec(Field.Jprity, false, FieldTypeEnum.Integer, true)]
        public int? Jprity
        {
            get
            {
                return JPrity;
            }
            set
            {
                JPrity = value;
            }
        }

        private string _JTypeId = null;
        /// <summary>
        /// 作業類別代碼 (請參考 JobCubeTypeCodeTexts)
        /// (BUA：上傳學生繳費資料 / BUB：上傳學分費退費資料 / BUC：上傳課程收費標準 / BUD：上傳已產生銷帳編號就貸資料 / BUE：上傳已產生銷帳編號減免資料)
        /// (BUF：上傳簡易學生繳費資料 / BUG：上傳學生基本資料)
        /// (CP：產生繳費金額 / CI：產生銷帳編號)
        /// </summary>
        [FieldSpec(Field.Jtypeid, false, FieldTypeEnum.VarChar, 3, true)]
        public string Jtypeid
        {
            get
            {
                return _JTypeId;
            }
            set
            {
                _JTypeId = value == null ? null : value.Trim();
            }
        }

        private string _JStatusId = null;
        /// <summary>
        /// 處理狀態代碼 (請參考 JobCubeStatusCodeTexts)
        /// </summary>
        [FieldSpec(Field.Jstatusid, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jstatusid
        {
            get
            {
                return _JStatusId;
            }
            set
            {
                _JStatusId = value == null ? null : value.Trim();
            }
        }

        private string _Jresultid = null;
        /// <summary>
        /// 處理結果代碼 (請參考 JobCubeResultCodeTexts)
        /// </summary>
        [FieldSpec(Field.Jresultid, false, FieldTypeEnum.VarChar, 1, true)]
        public string Jresultid
        {
            get
            {
                return _Jresultid;
            }
            set
            {
                _Jresultid = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 處理日誌
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

        private string _SeriorNo = String.Empty;
        /// <summary>
        /// 批號 (不是所有作業都有此參數) (by JRID + JYEAR + JTERM + JDEP + JRECID)
        /// </summary>
        [FieldSpec(Field.SeriorNo, false, FieldTypeEnum.VarChar, 4, true)]
        public string SeriorNo
        {
            get
            {
                return _SeriorNo;
            }
            set
            {
                _SeriorNo = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 備註 (待處理：空字串, 處理成功：處理成功 / 處裡中：戳記 / 處理失敗：錯誤訊息)
        /// </summary>
        [FieldSpec(Field.Memo, false, FieldTypeEnum.NVarChar, 2000, true)]
        public string Memo
        {
            get;
            set;
        }

        private string _Chancel = String.Empty;
        /// <summary>
        /// 檔案庫序號 (不是所有作業都有此參數) (bankPM.cancel)
        /// </summary>
        [FieldSpec(Field.Chancel, false, FieldTypeEnum.Char, 10, true)]
        public string Chancel
        {
            get
            {
                return _Chancel;
            }
            set
            {
                _Chancel = value == null ? String.Empty : value.Trim();
            }
        }
        #endregion
        #endregion

        #region Method
        #region 取得 BUA、BUB、BUC、BUD、BUE、BUF、BUG 的上傳檔案名稱
        /// <summary>
        /// 取得 BUA、BUB、BUC、BUD、BUE、BUF、BUG 的上傳檔案名稱參數值
        /// </summary>
        /// <returns></returns>
        public string GetUploadFileName()
        {
            return TryGetUploadFileName(this.Jtypeid, this.Jparam) ?? String.Empty;
        }
        #endregion
        #endregion

        #region Static Method
        #region BUA (上傳學生繳費資料) 作業類別相關
        /// <summary>
        /// 組合 BUA (上傳學生繳費資料) 作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="mappingId">對照檔代碼</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="cancel">上傳檔案的序號</param>
        /// <param name="seriorNo">批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUAParameter(string owner, string receiveType, string yearId, string termId, string depId, string receiveId
            , string mappingId, string fileName, string sheetName, string cancel, string seriorNo)
        {
            string typeId = JobCubeTypeCodeTexts.BUA;
            string tableName = "MappingRe"; //要寫入的資料表名稱
            string logPath = String.Empty;  //log 路徑
            //格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + 作業類別代碼 + bankPM.cancel + sheet_name

            #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
            #region [OLD]
            //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
            //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
            //    , fileName, tableName, logPath, seriorNo, typeId, cancel, sheetName);
            #endregion

            return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{typeId},{cancel},{sheetName}";
            #endregion
        }

        /// <summary>
        /// 拆解 BUA (上傳學生繳費資料) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="mappingId">傳回對照檔代碼</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="cancel">傳回上傳檔案的序號</param>
        /// <param name="seriorNo">傳回批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUAParameter(string parameter
            , out string owner, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string mappingId, out string fileName, out string sheetName, out string cancel, out string seriorNo)
        {
            owner = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            mappingId = null;
            fileName = null;
            sheetName = null;
            cancel = null;
            seriorNo = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null && parts.Length == 14)
            {
                owner = parts[0].Trim();
                receiveType = parts[1].Trim();
                yearId = parts[2].Trim();
                termId = parts[3].Trim();
                mappingId = parts[4].Trim();
                depId = parts[5].Trim();
                receiveId = parts[6].Trim();

                fileName = parts[7].Trim();
                seriorNo = parts[10].Trim();
                cancel = parts[12].Trim();
                sheetName = parts[13].Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 產生 BUA (上傳學生繳費資料) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="method">處理的方法</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUAEmpty(string owner, string receiveType, string yearId, string termId, string depId, string receiveId, string method)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUA;
            jobCube.Jdll = "clsUploadDataBUA.dll";
            jobCube.Jclass = "clsUploadDataBUA.Mapping";
            jobCube.Jmethod = method;
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = yearId;
            jobCube.Jterm = termId;
            jobCube.Jdep = depId;
            jobCube.Jrecid = receiveId;
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 取得 BUA (上傳學生繳費資料) 作業類別的 檔案類型對應的處理方法名稱
        /// </summary>
        /// <param name="fileType">指定檔案類型</param>
        /// <returns>傳回對應的處理方法名稱，或空字串</returns>
        public static string GetBUAMethodName(string fileType)
        {
            fileType = fileType == null ? null : fileType.Trim().ToUpper();
            switch (fileType)
            {
                case "TXT":
                case ".TXT":
                    return "ImportToReTxt";
                case "XLS":
                case ".XLS":
                case "XLSX":
                case ".XLSX":
                    return "ImportToReXSL";
                default:
                    return String.Empty;
            }
        }
        #endregion

        #region BUB (上傳學分費退費資料) 作業類別相關
        /// <summary>
        /// 組合 BUB (上傳學分費退費資料) 作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="mappingId">對照檔代碼</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="cancel">上傳檔案的序號</param>
        /// <param name="seriorNo">批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUBParameter(string owner, string receiveType, string yearId, string termId, string depId, string receiveId
            , string mappingId, string fileName, string sheetName, string cancel, string seriorNo)
        {
            //string typeId = JobCubeTypeCodeTexts.BUB;
            string tableName = "MappingRt"; //要寫入的資料表名稱
            string logPath = String.Empty;  //log 路徑
                                            //格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + sheet_name + 種類 (1 ~ 4) + "" + bankPM.cancel

            #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
            #region [OLD]
            //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}"
            //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
            //    , fileName, tableName, logPath, seriorNo, sheetName, "", "", cancel);
            #endregion

            return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{sheetName},{""},{""},{cancel}";
            #endregion
        }

        /// <summary>
        /// 拆解 BUB (上傳學分費退費資料) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="mappingId">傳回對照檔代碼</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="cancel">傳回上傳檔案的序號</param>
        /// <param name="seriorNo">傳回批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUBParameter(string parameter
            , out string owner, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string mappingId, out string fileName, out string sheetName, out string cancel, out string seriorNo)
        {
            owner = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            mappingId = null;
            fileName = null;
            sheetName = null;
            cancel = null;
            seriorNo = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null && parts.Length == 15)
            {
                owner = parts[0].Trim();
                receiveType = parts[1].Trim();
                yearId = parts[2].Trim();
                termId = parts[3].Trim();
                mappingId = parts[4].Trim();
                depId = parts[5].Trim();
                receiveId = parts[6].Trim();

                fileName = parts[7].Trim();
                seriorNo = parts[10].Trim();
                sheetName = parts[11].Trim();
                cancel = parts[14].Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 產生 BUB (上傳學分費退費資料) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="method">處理的方法</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUBEmpty(string owner, string receiveType, string yearId, string termId, string depId, string receiveId, string method)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUB;
            jobCube.Jdll = "clsUploadDataBUB.dll";
            jobCube.Jclass = "clsUploadDataBUB.Mapping";
            jobCube.Jmethod = method;
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = yearId;
            jobCube.Jterm = termId;
            jobCube.Jdep = depId;
            jobCube.Jrecid = receiveId;
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 取得 BUB (上傳學分費退費資料) 作業類別的 檔案類型對應的處理方法名稱
        /// </summary>
        /// <param name="fileType">指定檔案類型</param>
        /// <returns>傳回對應的處理方法名稱，或空字串</returns>
        public static string GetBUBMethodName(string fileType)
        {
            fileType = fileType == null ? null : fileType.Trim().ToUpper();
            switch (fileType)
            {
                case "TXT":
                case ".TXT":
                    return "ImportToReTxt";
                case "XLS":
                case ".XLS":
                case "XLSX":
                case ".XLSX":
                    return "ImportToReXSL";
                default:
                    return String.Empty;
            }
        }
        #endregion

        #region BUC (上傳課程收費標準) 作業類別相關
        /// <summary>
        /// 組合 BUC (上傳課程收費標準) 作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="mappingId">對照檔代碼</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="cancel">上傳檔案的序號</param>
        /// <param name="seriorNo">批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUCParameter(string owner, string receiveType, string yearId, string termId, string depId, string receiveId
            , string mappingId, string fileName, string sheetName, string cancel, string seriorNo)
        {
            //string typeId = JobCubeTypeCodeTexts.BUC;
            string tableName = "MappingCS"; //要寫入的資料表名稱
            string logPath = String.Empty;  //log 路徑
            if (String.IsNullOrEmpty(sheetName))
            {
                //TXT 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + bankPM.cancel

                #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
                #region [OLD]
                //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
                //    , fileName, tableName, logPath, seriorNo, cancel);
                #endregion

                return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{cancel}";
                #endregion
            }
            else
            {
                //XLS 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + sheet_name + bankPM.cancel

                #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
                #region [OLD]
                //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
                //    , fileName, tableName, logPath, seriorNo, sheetName, cancel);
                #endregion

                return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{sheetName},{cancel}";
                #endregion
            }
        }

        /// <summary>
        /// 拆解 BUC (上傳課程收費標準) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="mappingId">傳回對照檔代碼</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="cancel">傳回上傳檔案的序號</param>
        /// <param name="seriorNo">傳回批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUCParameter(string parameter
            , out string owner, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string mappingId, out string fileName, out string sheetName, out string cancel, out string seriorNo)
        {
            owner = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            mappingId = null;
            fileName = null;
            sheetName = null;
            cancel = null;
            seriorNo = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null)
            {
                if (parts.Length == 12)
                {
                    //TXT 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    cancel = parts[11].Trim();
                    return true;
                }
                if (parts.Length == 13)
                {
                    //XLS 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    sheetName = parts[11].Trim();
                    cancel = parts[12].Trim();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 產生 BUC (上傳課程收費標準) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="method">處理的方法</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUCEmpty(string owner, string receiveType, string yearId, string termId, string depId, string receiveId, string method)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUC;
            jobCube.Jdll = "clsUploadDataBUC.dll";
            jobCube.Jclass = "clsUploadDataBUC.Mapping";
            jobCube.Jmethod = method;
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = yearId;
            jobCube.Jterm = termId;
            jobCube.Jdep = depId;
            jobCube.Jrecid = receiveId;
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 取得 BUC (上傳課程收費標準) 作業類別的 檔案類型對應的處理方法名稱
        /// </summary>
        /// <param name="fileType">指定檔案類型</param>
        /// <returns>傳回對應的處理方法名稱，或空字串</returns>
        public static string GetBUCMethodName(string fileType)
        {
            fileType = fileType == null ? null : fileType.Trim().ToUpper();
            switch (fileType)
            {
                case "TXT":
                case ".TXT":
                    return "ImportToReTxt";
                case "XLS":
                case ".XLS":
                case "XLSX":
                case ".XLSX":
                    return "ImportToReXSL";
                default:
                    return String.Empty;
            }
        }
        #endregion

        #region BUD (上傳已產生銷帳編號就貸資料) 作業類別相關
        /// <summary>
        /// 組合 BUD (上傳已產生銷帳編號就貸資料) 作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="mappingId">對照檔代碼</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="cancel">上傳檔案的序號</param>
        /// <param name="seriorNo">批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUDParameter(string owner, string receiveType, string yearId, string termId, string depId, string receiveId
            , string mappingId, string fileName, string sheetName, string cancel, string seriorNo)
        {
            //string typeId = JobCubeTypeCodeTexts.BUD;
            string tableName = "MappingLO"; //要寫入的資料表名稱
            string logPath = String.Empty;  //log 路徑
            if (String.IsNullOrEmpty(sheetName))
            {
                //TXT 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + bankPM.cancel

                #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
                #region [OLD]
                //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
                //    , fileName, tableName, logPath, seriorNo, cancel);
                #endregion

                return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{cancel}";
                #endregion
            }
            else
            {
                //XLS 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + sheet_name + bankPM.cancel

                #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
                #region [OLD]
                //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
                //    , fileName, tableName, logPath, seriorNo, sheetName, cancel);
                #endregion

                return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{sheetName},{cancel}";
                #endregion
            }
        }

        /// <summary>
        /// 拆解 BUD (上傳已產生銷帳編號就貸資料) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="mappingId">傳回對照檔代碼</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="cancel">傳回上傳檔案的序號</param>
        /// <param name="seriorNo">傳回批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUDParameter(string parameter
            , out string owner, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string mappingId, out string fileName, out string sheetName, out string cancel, out string seriorNo)
        {
            owner = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            mappingId = null;
            fileName = null;
            sheetName = null;
            cancel = null;
            seriorNo = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null)
            {
                if (parts.Length == 12)
                {
                    //TXT 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    cancel = parts[11].Trim();
                    return true;
                }
                if (parts.Length == 13)
                {
                    //XLS 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    sheetName = parts[11].Trim();
                    cancel = parts[12].Trim();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 產生 BUD (上傳已產生銷帳編號就貸資料) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="method">處理的方法</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUDEmpty(string owner, string receiveType, string yearId, string termId, string depId, string receiveId, string method)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUD;
            jobCube.Jdll = "clsUploadDataBUD.dll";
            jobCube.Jclass = "clsUploadDataBUD.Mapping";
            jobCube.Jmethod = method;
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = yearId;
            jobCube.Jterm = termId;
            jobCube.Jdep = depId;
            jobCube.Jrecid = receiveId;
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 取得 BUD (上傳已產生銷帳編號就貸資料) 作業類別的 檔案類型對應的處理方法名稱
        /// </summary>
        /// <param name="fileType">指定檔案類型</param>
        /// <returns>傳回對應的處理方法名稱，或空字串</returns>
        public static string GetBUDMethodName(string fileType)
        {
            fileType = fileType == null ? null : fileType.Trim().ToUpper();
            switch (fileType)
            {
                case "TXT":
                case ".TXT":
                    return "ImportToReTxt";
                case "XLS":
                case ".XLS":
                case "XLSX":
                case ".XLSX":
                    return "ImportToReXSL";
                default:
                    return String.Empty;
            }
        }
        #endregion

        #region BUE (上傳已產生銷帳編號減免資料) 作業類別相關
        /// <summary>
        /// 組合 BUE (上傳已產生銷帳編號減免資料) 作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="mappingId">對照檔代碼</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="cancel">上傳檔案的序號</param>
        /// <param name="seriorNo">批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUEParameter(string owner, string receiveType, string yearId, string termId, string depId, string receiveId
            , string mappingId, string fileName, string sheetName, string cancel, string seriorNo)
        {
            //string typeId = JobCubeTypeCodeTexts.BUE;
            string tableName = "MappingRR"; //要寫入的資料表名稱
            string logPath = String.Empty;  //log 路徑
            if (String.IsNullOrEmpty(sheetName))
            {
                //TXT 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + bankPM.cancel

                #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
                #region [OLD]
                //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
                //    , fileName, tableName, logPath, seriorNo, cancel);
                #endregion

                return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{cancel}";
                #endregion
            }
            else
            {
                //XLS 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + sheet_name + bankPM.cancel

                #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
                #region [OLD]
                //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
                //    , fileName, tableName, logPath, seriorNo, sheetName, cancel);
                #endregion

                return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{sheetName},{cancel} ";
                #endregion
            }
        }

        /// <summary>
        /// 拆解 BUE (上傳已產生銷帳編號減免資料) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="mappingId">傳回對照檔代碼</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="cancel">傳回上傳檔案的序號</param>
        /// <param name="seriorNo">傳回批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUEParameter(string parameter
            , out string owner, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string mappingId, out string fileName, out string sheetName, out string cancel, out string seriorNo)
        {
            owner = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            mappingId = null;
            fileName = null;
            sheetName = null;
            cancel = null;
            seriorNo = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null)
            {
                if (parts.Length == 12)
                {
                    //TXT 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    cancel = parts[11].Trim();
                    return true;
                }
                if (parts.Length == 13)
                {
                    //XLS 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    sheetName = parts[11].Trim();
                    cancel = parts[12].Trim();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 產生 BUE (上傳已產生銷帳編號減免資料) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="method">處理的方法</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUEEmpty(string owner, string receiveType, string yearId, string termId, string depId, string receiveId, string method)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUE;
            jobCube.Jdll = "clsUploadDataBUE.dll";
            jobCube.Jclass = "clsUploadDataBUE.Mapping";
            jobCube.Jmethod = method;
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = yearId;
            jobCube.Jterm = termId;
            jobCube.Jdep = depId;
            jobCube.Jrecid = receiveId;
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 取得 BUE (上傳已產生銷帳編號減免資料) 作業類別的 檔案類型對應的處理方法名稱
        /// </summary>
        /// <param name="fileType">指定檔案類型</param>
        /// <returns>傳回對應的處理方法名稱，或空字串</returns>
        public static string GetBUEMethodName(string fileType)
        {
            fileType = fileType == null ? null : fileType.Trim().ToUpper();
            switch (fileType)
            {
                case "TXT":
                case ".TXT":
                    return "ImportToReTxt";
                case "XLS":
                case ".XLS":
                case "XLSX":
                case ".XLSX":
                    return "ImportToReXSL";
                default:
                    return String.Empty;
            }
        }
        #endregion

        #region BUF (簡易上傳) 作業類別相關
        /// <summary>
        /// 產生 BUF (簡易上傳) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">學年代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="method">處理的方法</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUFEmpty(string owner, string receiveType, string yearId, string termId, string depId, string receiveId)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUF;
            jobCube.Jdll = "clsUploadDataBUF.dll";
            jobCube.Jclass = "clsUploadDataBUF.Mapping";
            jobCube.Jmethod = "";
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = yearId;
            jobCube.Jterm = termId;
            jobCube.Jdep = depId;
            jobCube.Jrecid = receiveId;
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 組合 BUF (簡易上傳) 作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="cancel">上傳檔案的序號</param>
        /// <param name="seriorNo">批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUFParameter(string owner, string receiveType, string yearId, string termId, string depId, string receiveId
            , string fileName, string sheetName, string cancel, string seriorNo)
        {
            string typeId = JobCubeTypeCodeTexts.BUF;
            string tableName = ""; //要寫入的資料表名稱
            string logPath = String.Empty;  //log 路徑
            string mappingId = String.Empty;    //對照檔代碼

            //XLS 格式： 發動者帳號 + 代收類別  + 學年代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼 + 檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + 作業類別代碼 + bankPM.cancel + sheet_name

            #region [MDY:20220910] Checkmarx - Potential ReDoS 誤判調整
            #region [OLD]
            //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
            //    , owner, receiveType, yearId, termId, mappingId, depId, receiveId
            //    , fileName, tableName, logPath, seriorNo, typeId, cancel, sheetName);
            #endregion

            return $"{owner},{receiveType},{yearId},{termId},{mappingId},{depId},{receiveId},{fileName},{tableName},{logPath},{seriorNo},{typeId},{cancel},{sheetName}";
            #endregion
        }

        /// <summary>
        /// 拆解 BUF (簡易上傳) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="cancel">傳回上傳檔案的序號</param>
        /// <param name="seriorNo">傳回批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUFParameter(string parameter
            , out string owner, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string fileName, out string sheetName, out string cancel, out string seriorNo)
        {
            owner = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            //mappingId = null;
            fileName = null;
            sheetName = null;
            cancel = null;
            seriorNo = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null)
            {
                if (parts.Length == 14)
                {
                    //XLS 格式
                    owner = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    //mappingId = parts[4].Trim();
                    depId = parts[5].Trim();
                    receiveId = parts[6].Trim();

                    fileName = parts[7].Trim();
                    seriorNo = parts[10].Trim();
                    sheetName = parts[13].Trim();
                    cancel = parts[12].Trim();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region BUG (上傳學生基本資料)  作業類別相關
        /// <summary>
        /// 產生 BUG (上傳學生基本資料) 作業類別的 空的批次處理佇列物件
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <returns>傳回批次處理佇列物件</returns>
        public static JobcubeEntity CreateBUGEmpty(string owner, string receiveType)
        {
            JobcubeEntity jobCube = new JobcubeEntity();
            jobCube.Jno = 0;
            jobCube.Jtypeid = JobCubeTypeCodeTexts.BUG;
            jobCube.Jdll = "clsUploadDataBUG.dll";
            jobCube.Jclass = "clsUploadDataBUG.Mapping";
            jobCube.Jmethod = "";
            jobCube.Jowner = owner;
            jobCube.Jrid = receiveType;
            jobCube.Jyear = "";
            jobCube.Jterm = "";
            jobCube.Jdep = "";
            jobCube.Jrecid = "";
            jobCube.Jprity = 0;

            jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            jobCube.CDate = DateTime.Now;
            jobCube.SeriorNo = String.Empty;
            jobCube.Memo = String.Empty;
            jobCube.Chancel = String.Empty;
            jobCube.Jparam = String.Empty;
            return jobCube;
        }

        /// <summary>
        /// 組合 BUG (上傳學生基本資料)  作業類別的參數字串
        /// </summary>
        /// <param name="owner">發動者帳號</param>
        /// <param name="receiveType">商家代號</param>
        /// <param name="fileName">上傳的檔案名稱</param>
        /// <param name="sheetName">Excel的工作表名稱</param>
        /// <param name="bankPMCancel">存放上傳資料檔案庫序號 (bankPM.Cancel)</param>
        /// <param name="seriorNo">上傳批號</param>
        /// <returns>傳回參數字串</returns>
        public static string JoinBUGParameter(string owner, string receiveType, string fileName, string sheetName, string bankPMCancel, string upNo)
        {
            string typeId = JobCubeTypeCodeTexts.BUG;
            //string mappingId = String.Empty;    //固定格式，所以沒有對照檔代碼

            Fuju.KeyValueList<string> args = new Fuju.KeyValueList<string>();
            args.Add("TypeId", typeId);          //作業類別代碼
            args.Add("Owner", owner == null ? String.Empty : owner.Trim());                         //發動者帳號
            args.Add("ReceiveType", receiveType == null ? String.Empty : receiveType.Trim());       //代收類別
            args.Add("FileName", fileName == null ? String.Empty : fileName.Trim());                //上傳檔案名稱
            args.Add("SheetName", sheetName == null ? String.Empty : sheetName.Trim());             //Excel的工作表名稱
            args.Add("BankPMCancel", bankPMCancel == null ? String.Empty : bankPMCancel.Trim());    //存放上傳檔案的bankPM序號
            args.Add("UpNo", upNo == null ? String.Empty : upNo.Trim());                            //上傳批號

            //格式：Key=value;
            return args.ToString("=", ";");
        }

        /// <summary>
        /// 拆解 BUG (上傳學生基本資料) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter">指定參數字串</param>
        /// <param name="owner">傳回發動者帳號</param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="fileName">傳回上傳的檔案名稱</param>
        /// <param name="sheetName">傳回Excel的工作表名稱</param>
        /// <param name="bankPMCancel">傳回上傳資料檔案庫序號 (bankPM.Cancel)</param>
        /// <param name="upNo">傳回上傳批號</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool ParseBUGParameter(string parameter
            , out string owner, out string receiveType, out string fileName, out string sheetName, out string bankPMCancel, out string upNo)
        {
            string typeId = null;
            owner = null;
            receiveType = null;
            fileName = null;
            sheetName = null;
            bankPMCancel = null;
            upNo = null;

            if (!String.IsNullOrWhiteSpace(parameter))
            {
                Fuju.KeyValueList<string> args = Fuju.KeyValueList<string>.Split(parameter, "=", ";");
                foreach (Fuju.KeyValue<string> arg in args)
                {
                    switch (arg.Key)
                    {
                        case "TypeId":
                            typeId = arg.Value.Trim();
                            if (typeId != JobCubeTypeCodeTexts.BUG)
                            {
                                return false;
                            }
                            break;
                        case "Owner":
                            owner = arg.Value.Trim();
                            break;
                        case "ReceiveType":
                            receiveType = arg.Value.Trim();
                            break;
                        case "FileName":
                            fileName = arg.Value.Trim();
                            break;
                        case "SheetName":
                            sheetName = arg.Value.Trim();
                            break;
                        case "BankPMCancel":
                            bankPMCancel = arg.Value.Trim();
                            break;
                        case "UpNo":
                            upNo = arg.Value.Trim();
                            break;
                    }
                }
                if (owner == null || receiveType == null || fileName == null || sheetName == null || bankPMCancel == null || upNo == null)
                {
                    //null 表示缺少該參數
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 嘗試從指定作業參數中取得的上傳檔案名稱
        /// </summary>
        /// <param name="jobcubeTypeId">指定作業類別代碼</param>
        /// <param name="jobcubeParameter">指定作業參數</param>
        /// <returns>有上傳檔案名稱參數則傳回該參數值，否則傳回 null</returns>
        public static string TryGetUploadFileName(string jobcubeTypeId, string jobcubeParameter)
        {
            string fileName = null;
            if (!String.IsNullOrWhiteSpace(jobcubeTypeId) && !String.IsNullOrWhiteSpace(jobcubeParameter))
            {
                string owner = null;
                string receiveType = null;
                string yearId = null;
                string termId = null;
                string depId = null;
                string receiveId = null;
                string mappingId = null;
                string sheetName = null;
                string cancel = null;
                string seriorNo = null;
                switch (jobcubeTypeId.Trim().ToUpper())
                {
                    case JobCubeTypeCodeTexts.BUA:
                        JobcubeEntity.ParseBUAParameter(jobcubeParameter, out owner, out receiveType, out yearId, out termId, out depId, out receiveId
                            , out mappingId, out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                    case JobCubeTypeCodeTexts.BUB:
                        JobcubeEntity.ParseBUBParameter(jobcubeParameter, out owner, out receiveType, out yearId, out termId, out depId, out receiveId
                            , out mappingId, out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                    case JobCubeTypeCodeTexts.BUC:
                        JobcubeEntity.ParseBUCParameter(jobcubeParameter, out owner, out receiveType, out yearId, out termId, out depId, out receiveId
                            , out mappingId, out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                    case JobCubeTypeCodeTexts.BUD:
                        JobcubeEntity.ParseBUDParameter(jobcubeParameter, out owner, out receiveType, out yearId, out termId, out depId, out receiveId
                            , out mappingId, out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                    case JobCubeTypeCodeTexts.BUE:
                        JobcubeEntity.ParseBUEParameter(jobcubeParameter, out owner, out receiveType, out yearId, out termId, out depId, out receiveId
                            , out mappingId, out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                    case JobCubeTypeCodeTexts.BUF:
                        JobcubeEntity.ParseBUFParameter(jobcubeParameter, out owner, out receiveType, out yearId, out termId, out depId, out receiveId
                            , out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                    case JobCubeTypeCodeTexts.BUG:
                        JobcubeEntity.ParseBUGParameter(jobcubeParameter, out owner, out receiveType, out fileName, out sheetName, out cancel, out seriorNo);
                        break;
                }
            }
            return fileName == null ? null : fileName.Trim();
        }

        #region CP (產生金額) 作業類別相關
        /// <summary>
        /// 組合 CI (產生銷帳編號) 作業類別的參數字串
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="upNo">上傳繳費資料的批號</param>
        /// <param name="billingType">計算方式</param>
        /// <returns></returns>
        public static string JoinCPParameter(string receiveType, string yearId, string termId, string depId, string receiveId
            , string upNo, string billingType)
        {
            return String.Format("{0},{1},{2},{3},{4},{5},{6}", receiveType, yearId, termId, depId, receiveId, upNo, billingType);
        }

        /// <summary>
        /// 拆解 CP (產生金額) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="upNo">上傳繳費資料的批號</param>
        /// <param name="type">傳回計算方式。1:依收費標準 2:依輸入金額</param>
        /// <returns></returns>
        public static bool ParseCPParameter(string parameter
            , out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string upNo, out string type)
        {
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            upNo = null; //批號
            type = null; //1:依收費標準 2:依輸入金額

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null && parts.Length == 7)
            {
                receiveType = parts[0].Trim();
                yearId = parts[1].Trim();
                termId = parts[2].Trim();
                depId = parts[3].Trim();
                receiveId = parts[4].Trim();
                upNo = parts[5].Trim();
                type = parts[6].Trim();
                return true;
            }
            return false;
        }
        #endregion

        #region CI (產生銷帳編號) 作業類別相關
        /// <summary>
        /// 組合 CI (產生銷帳編號) 作業類別的參數字串
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="upNo">上傳繳費資料的批號</param>
        /// <param name="startSeriorNo">流水號起始位置</param>
        /// <param name="sortType">排序原則</param>
        /// <param name="sortFields">排序欄位陣列</param>
        /// <returns></returns>
        public static string JoinCIParameter(string receiveType, string yearId, string termId, string depId, string receiveId
            , string upNo, string startSeriorNo, string sortType, string[] sortFields)
        {
            string sortFieldsTxt = (sortFields == null || sortFields.Length == 0) ? String.Empty : String.Join("|", sortFields);
            return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", sortType, startSeriorNo, yearId, receiveType, termId, depId, receiveId, sortFieldsTxt, upNo);
        }

        /// <summary>
        /// 拆解 CI (產生金額) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="upNo">上傳繳費資料的批號</param>
        /// <param name="startSeriorNo">流水號起始位置</param>
        /// <param name="sortType">排序原則</param>
        /// <param name="sortFields">排序欄位陣列</param>
        /// <returns></returns>
        public static bool ParseCIParameter(string parameter
            , out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string upNo, out string startSeriorNo, out string sortType, out string[] sortFields)
        {
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            upNo = null;
            startSeriorNo = null;
            sortType = null;
            sortFields = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null && parts.Length == 9)
            {
                sortType = parts[0].Trim();
                startSeriorNo = parts[1].Trim();
                yearId = parts[2].Trim();
                receiveType = parts[3].Trim();
                termId = parts[4].Trim();
                depId = parts[5].Trim();
                receiveId = parts[6].Trim();
                sortFields = parts[7].Trim().Split(new char[] { '|' });
                upNo = parts[8].Trim();
                return true;
            }
            return false;
        }
        #endregion

        #region PDFB 與 PDFR (產生繳費單 與 收據) 作業類別相關
        #region [MDY:202203XX] 2022擴充案 是否英文介面
        /// <summary>
        /// 組合 PDFB 與 PDFR (產生繳費單 與 收據) 作業類別的參數字串
        /// </summary>
        /// <param name="receiveType">商家代號</param>
        /// <param name="yearId">年度代碼</param>
        /// <param name="termId">學期代碼</param>
        /// <param name="depId">部別代碼</param>
        /// <param name="receiveId">代收費用別代碼</param>
        /// <param name="qType">條件類別</param>
        /// <param name="startSNo">起始流水號</param>
        /// <param name="endSNo">結束流水號</param>
        /// <param name="upNo">上傳資料批號</param>
        /// <param name="studentId">學號</param>
        /// <param name="majorId">科系代號</param>
        /// <param name="stuGrade">年級代碼</param>
        /// <param name="allAmount">是否所有金額的資料都要產生</param>
        /// <param name="isEngUI">是否英文介面</param>
        /// <returns></returns>
        public static string JoinPDFxParameter(string receiveType, string yearId, string termId, string depId, string receiveId
            , string qType, string startSNo, string endSNo, string upNo, string studentId, string majorId = null, string stuGrade = null
            , bool allAmount = false, bool isEngUI = false)
        {
            //[TODO] 沒有照舊格式
            string qValue = "";
            switch (qType)
            {
                case "2":   //自訂產生繳費單流水號
                    qValue = String.Format("{0}|{1}", startSNo, endSNo);
                    break;
                case "3":   //依批號產生
                    qValue = upNo;
                    break;
                case "4":   //依學號產生
                    qValue = studentId;
                    break;
                case "5":   //依科系產生
                    qValue = majorId;
                    break;
                case "6":   //依年級產生
                    qValue = stuGrade;
                    break;
            }
            //return String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", receiveType, yearId, termId, depId, receiveId, qType, qValue, allAmount ? "Y" : "N");
            return $"{receiveType},{yearId},{termId},{depId},{receiveId},{qType},{qValue},{(allAmount ? "Y" : "N")},{(isEngUI ? "Y" : "N")}";
        }

        /// <summary>
        /// 拆解 組合 PDFB 與 PDFR (產生繳費單 與 收據) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="qType">條件類別</param>
        /// <param name="qValue">條件值</param>
        /// <param name="allAmount">是否所有金額的資料都要產生</param>
        /// <param name="isEngUI">是否英文介面</param>
        /// <returns></returns>
        public static bool ParsePDBxParameter(string parameter
            , out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string qType, out string qValue, out bool allAmount, out bool isEngUI)
        {
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            qType = null;
            qValue = null;
            allAmount = false;
            isEngUI = false;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null)
            {
                if (parts.Length == 7)
                {
                    receiveType = parts[0].Trim();
                    yearId = parts[1].Trim();
                    termId = parts[2].Trim();
                    depId = parts[3].Trim();
                    receiveId = parts[4].Trim();
                    qType = parts[5].Trim();
                    qValue = parts[6].Trim();
                    return true;
                }
                else if (parts.Length == 8)
                {
                    receiveType = parts[0].Trim();
                    yearId = parts[1].Trim();
                    termId = parts[2].Trim();
                    depId = parts[3].Trim();
                    receiveId = parts[4].Trim();
                    qType = parts[5].Trim();
                    qValue = parts[6].Trim();
                    allAmount = (parts[7].Trim() == "Y");
                    return true;
                }
                else if (parts.Length == 9)
                {
                    receiveType = parts[0].Trim();
                    yearId = parts[1].Trim();
                    termId = parts[2].Trim();
                    depId = parts[3].Trim();
                    receiveId = parts[4].Trim();
                    qType = parts[5].Trim();
                    qValue = parts[6].Trim();
                    allAmount = (parts[7].Trim() == "Y");
                    isEngUI = (parts[8].Trim() == "Y");
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region D38 作業類別相關
        /// <summary>
        /// 上傳資料至中心
        /// </summary>
        public static readonly string D38UpdKind_Update = "U";
        /// <summary>
        /// 刪除已上傳資料
        /// </summary>
        public static readonly string D38UpdKind_Delete = "D";

        public static string JoinD38Parameter(string updKind, string receiveType, string yearId, string termId, string depId, string receiveId
            , string qType, string sSerialNo, string eSerialNo, string upNo, string studentId)
        {
            string qValue = "";
            switch (qType)
            {
                case "2":   //自訂產生繳費單流水號
                    qValue = String.Format("{0}|{1}", sSerialNo, eSerialNo);
                    break;
                case "3":   //依批號產生
                    qValue = upNo;
                    break;
                case "4":   //依學號產生
                    qValue = studentId;
                    break;
            }
            return String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", updKind, receiveType, yearId, termId, depId, receiveId, qType, qValue);
        }

        /// <summary>
        /// 拆解 組合 PDFB 與 PDFR (產生繳費單 與 收據) 作業類別的參數字串
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="receiveType">傳回商家代號</param>
        /// <param name="yearId">傳回學年代碼</param>
        /// <param name="termId">傳回學期代碼</param>
        /// <param name="depId">傳回部別代碼</param>
        /// <param name="receiveId">傳回代收費用別代碼</param>
        /// <param name="qType">條件類別</param>
        /// <param name="qValue">條件值</param>
        /// <returns></returns>
        public static bool ParseD38Parameter(string parameter
            , out string updKind, out string receiveType, out string yearId, out string termId, out string depId, out string receiveId
            , out string qType, out string qValue)
        {
            updKind = null;
            receiveType = null;
            yearId = null;
            termId = null;
            depId = null;
            receiveId = null;
            qType = null;
            qValue = null;

            string[] parts = String.IsNullOrEmpty(parameter) ? null : parameter.Split(new char[] { ',' });
            if (parts != null)
            {
                if (parts.Length == 7)
                {
                    updKind = D38UpdKind_Update;
                    receiveType = parts[0].Trim();
                    yearId = parts[1].Trim();
                    termId = parts[2].Trim();
                    depId = parts[3].Trim();
                    receiveId = parts[4].Trim();
                    qType = parts[5].Trim();
                    qValue = parts[6].Trim();
                    return true;
                }
                else if (parts.Length == 8)
                {
                    updKind = parts[0].Trim();
                    receiveType = parts[1].Trim();
                    yearId = parts[2].Trim();
                    termId = parts[3].Trim();
                    depId = parts[4].Trim();
                    receiveId = parts[5].Trim();
                    qType = parts[6].Trim();
                    qValue = parts[7].Trim();
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion
    }
}
