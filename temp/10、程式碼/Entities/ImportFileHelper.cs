using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;

using NPOI;
using NPOI.HPSF;
using NPOI.POIFS;
using NPOI.POIFS.FileSystem;
using NPOI.Util;

using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace Entities
{
    /// <summary>
    /// 匯入檔案處理工具類別
    /// </summary>
    public sealed class ImportFileHelper : IDisposable
    {
        #region Member
        /// <summary>
        /// 儲存 資料存取物件 的變數
        /// </summary>
        private EntityFactory _Factory = null;

        /// <summary>
        /// 儲存 此物件 Dispose 時 資料存取物件 是否需要一併 Dispose
        /// </summary>
        private bool _IsNeedFactoryDispose = false;
        #endregion

        #region Constructor
        /// <summary>
        /// 建構匯入檔案處理工具類別
        /// </summary>
        /// <param name="factory">指定資料存取物件</param>
        public ImportFileHelper(EntityFactory factory)
        {
            _Factory = factory;
            _IsNeedFactoryDispose = false;
        }

        /// <summary>
        /// 建構匯入檔案處理工具類別
        /// </summary>
        public ImportFileHelper()
        {
            _Factory = new EntityFactory();
            _IsNeedFactoryDispose = true;
        }
        #endregion

        #region Destructor
        /// <summary>
        /// 解構匯入檔案處理工具類別
        /// </summary>
        ~ImportFileHelper()
        {
            Dispose(false);
        }
        #endregion

        #region Implement IDisposable
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool _Disposed = false;

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">指定是否釋放資源。</param>
        private void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    if (_Factory != null && _IsNeedFactoryDispose)
                    {
                        _Factory.Dispose();
                        _Factory = null;
                    }
                }
                _Disposed = true;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此物件是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false。</returns>
        public bool IsReady()
        {
            return (_Factory != null && _Factory.IsReady());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobCube"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <param name="failMsg"></param>
        /// <returns></returns>
        public Result ImportByJobCube(JobcubeEntity jobCube, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;
            if (jobCube == null || !JobCubeTypeCodeTexts.IsImportFileJob(jobCube.Jtypeid))
            {
                return new Result(false, "缺少或無效的 jobCube 參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            switch (jobCube.Jtypeid)
            {
                case JobCubeTypeCodeTexts.BUA:
                    return ImportBUAJob(jobCube, encoding, isBatch, out logmsg, out totalCount, out successCount);
                case JobCubeTypeCodeTexts.BUB:
                    return ImportBUBJob(jobCube, encoding, isBatch, out logmsg, out totalCount, out successCount);
                case JobCubeTypeCodeTexts.BUC:
                    return ImportBUCJob(jobCube, encoding, isBatch, out logmsg, out totalCount, out successCount);
                case JobCubeTypeCodeTexts.BUD:
                    return ImportBUDJob(jobCube, encoding, isBatch, out logmsg, out totalCount, out successCount);
                case JobCubeTypeCodeTexts.BUE:
                    return ImportBUEJob(jobCube, encoding, isBatch, out logmsg, out totalCount, out successCount);
                case JobCubeTypeCodeTexts.BUF:
                    return ImportBUFJob(jobCube, encoding, isBatch, out logmsg, out totalCount, out successCount);
            }
            return new Result(false, "不支援的作業類別", CoreStatusCode.S_NO_SUPPORT, null);
        }
        #endregion

        #region Import BUA File(上傳學生繳費資料)
        /// <summary>
        /// 匯入 BUA (上傳學生繳費資料) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUAJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUA)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;

            #region [Old] 土銀不使用原部別，depId 固定為空字串
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼、學期代碼、部別代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
            //    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            CancelNoHelper cnoHelper = new CancelNoHelper();
            CancelNoHelper.Module module = cnoHelper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                logmsg = String.Format("無法取得商家代號 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            string owner = null;
            string mappingId = null;
            string sheetName = null;
            string fileType = null;
            int cancel = 0;
            int seriroNo = 0;
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pFileName = null;
                string pCancel = null;
                string pSeriroNo = null;
                isParamOK = JobcubeEntity.ParseBUAParameter(job.Jparam, out owner, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out mappingId, out pFileName, out sheetName, out pCancel, out pSeriroNo);
                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (String.IsNullOrEmpty(mappingId)
                    || (fileType != "xls" && fileType != "xlsx" && fileType != "txt" && fileType != "ods")
                    || String.IsNullOrEmpty(pCancel) || !Int32.TryParse(pCancel, out cancel) || cancel < 1
                    || String.IsNullOrEmpty(pSeriroNo) || !Int32.TryParse(pSeriroNo, out seriroNo) || seriroNo < 1)
                {
                    logmsg = "批次處理序列缺少對照表代碼、上傳檔案序號或批次號碼的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                {
                    fileContent = encoding.GetBytes(textContent);
                }
                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 因為要判斷是否啟用英文資料，所以取得商家代號資料程式段搬到這裡
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            bool isEngEnabled = school.IsEngEnabled();
            #endregion

            #region 取對照表
            MappingreTxtEntity txtMapping = null;
            MappingreXlsmdbEntity xlsMapping = null;

            string[] receiveItemNames = null;   //存放收集的代收科目名稱
            string[] memoTitles = null;         //存放收集的備註標題

            #region [MDY:202203XX] 2022擴充案 收入科目英文名稱、備註項目英文標題
            string[] receiveItemENames = null;  //存放收集的代收科目英文名稱
            string[] memoETitles = null;        //存放收集的備註項目英文標題
            #endregion
            {
                switch (fileType)
                {
                    case "txt":
                        #region 文字檔格式
                        {
                            Expression where = new Expression(MappingreTxtEntity.Field.MappingId, mappingId)
                                .And(MappingreTxtEntity.Field.ReceiveType, receiveType);
                            Result result = _Factory.SelectFirst<MappingreTxtEntity>(where, null, out txtMapping);
                            if (!result.IsSuccess)
                            {
                                logmsg = "讀取文字格式的上傳繳費資料對照表資料失敗，" + result.Message;
                                return new Result(false, logmsg, result.Code, result.Exception);
                            }
                            if (txtMapping == null)
                            {
                                logmsg = String.Format("查無 代碼 {0}、商家代號 {1} 的文字格式的上傳繳費資料對照表資料", mappingId, receiveType);
                                return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                            }
                        }
                        #endregion
                        break;
                    case "xls":
                    case "xlsx":
                        #region Excel檔格式
                        {
                            Expression where = new Expression(MappingreXlsmdbEntity.Field.MappingId, mappingId)
                                .And(MappingreXlsmdbEntity.Field.ReceiveType, receiveType);
                            Result result = _Factory.SelectFirst<MappingreXlsmdbEntity>(where, null, out xlsMapping);
                            if (!result.IsSuccess)
                            {
                                logmsg = "讀取 Excel 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                                return new Result(false, logmsg, result.Code, result.Exception);
                            }
                            if (xlsMapping == null)
                            {
                                logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Excel 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                                return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                            }

                            #region 收集收入科目名稱
                            {
                                bool hasReceiveItemName = false;
                                receiveItemNames = new string[] {
                                    xlsMapping.Receive1, xlsMapping.Receive2, xlsMapping.Receive3, xlsMapping.Receive4, xlsMapping.Receive5,
                                    xlsMapping.Receive6, xlsMapping.Receive7, xlsMapping.Receive8, xlsMapping.Receive9, xlsMapping.Receive10,
                                    xlsMapping.Receive11, xlsMapping.Receive12, xlsMapping.Receive13, xlsMapping.Receive14, xlsMapping.Receive15,
                                    xlsMapping.Receive16, xlsMapping.Receive17, xlsMapping.Receive18, xlsMapping.Receive19, xlsMapping.Receive20,
                                    xlsMapping.Receive21, xlsMapping.Receive22, xlsMapping.Receive23, xlsMapping.Receive24, xlsMapping.Receive25,
                                    xlsMapping.Receive26, xlsMapping.Receive27, xlsMapping.Receive28, xlsMapping.Receive29, xlsMapping.Receive30,
                                    xlsMapping.Receive31, xlsMapping.Receive32, xlsMapping.Receive33, xlsMapping.Receive34, xlsMapping.Receive35,
                                    xlsMapping.Receive36, xlsMapping.Receive37, xlsMapping.Receive38, xlsMapping.Receive39, xlsMapping.Receive40
                                };
                                for (int idx = 0; idx < receiveItemNames.Length; idx++)
                                {
                                    if (String.IsNullOrWhiteSpace(receiveItemNames[idx]))
                                    {
                                        receiveItemNames[idx] = String.Empty;
                                    }
                                    else
                                    {
                                        hasReceiveItemName = true;
                                        receiveItemNames[idx] = receiveItemNames[idx].Trim();
                                    }
                                }
                                if (!hasReceiveItemName)
                                {
                                    //沒有指定任何收入科目，就不用收集了
                                    receiveItemNames = null;
                                }
                            }
                            #endregion

                            #region 收集備註標題
                            {
                                bool hasMemoTitle = false;
                                memoTitles = new string[MappingreXlsmdbEntity.MemoCount] {
                                    xlsMapping.Memo01, xlsMapping.Memo02, xlsMapping.Memo03, xlsMapping.Memo04, xlsMapping.Memo05,
                                    xlsMapping.Memo06, xlsMapping.Memo07, xlsMapping.Memo08, xlsMapping.Memo09, xlsMapping.Memo10,
                                    xlsMapping.Memo11, xlsMapping.Memo12, xlsMapping.Memo13, xlsMapping.Memo14, xlsMapping.Memo15,
                                    xlsMapping.Memo16, xlsMapping.Memo17, xlsMapping.Memo18, xlsMapping.Memo19, xlsMapping.Memo20,
                                    xlsMapping.Memo21
                                };
                                for (int idx = 0; idx < memoTitles.Length; idx++)
                                {
                                    if (String.IsNullOrWhiteSpace(memoTitles[idx]))
                                    {
                                        memoTitles[idx] = String.Empty;
                                    }
                                    else
                                    {
                                        hasMemoTitle = true;
                                        memoTitles[idx] = memoTitles[idx].Trim();
                                    }
                                }
                                if (!hasMemoTitle)
                                {
                                    //沒有指定任何備註標題，就不用收集了
                                    memoTitles = null;
                                }
                            }
                            #endregion

                            #region [MDY:202203XX] 2022擴充案 收集收入科目英文名稱、備註項目英文標題
                            if (isEngEnabled)
                            {
                                #region 收集收入科目英文名稱（沒有中文就不要英文，有中文沒英文就用中文當英文）
                                if (receiveItemNames != null)
                                {
                                    receiveItemENames = new string[] {
                                        xlsMapping.ReceiveItemE01, xlsMapping.ReceiveItemE02, xlsMapping.ReceiveItemE03, xlsMapping.ReceiveItemE04, xlsMapping.ReceiveItemE05,
                                        xlsMapping.ReceiveItemE06, xlsMapping.ReceiveItemE07, xlsMapping.ReceiveItemE08, xlsMapping.ReceiveItemE09, xlsMapping.ReceiveItemE10,
                                        xlsMapping.ReceiveItemE11, xlsMapping.ReceiveItemE12, xlsMapping.ReceiveItemE13, xlsMapping.ReceiveItemE14, xlsMapping.ReceiveItemE15,
                                        xlsMapping.ReceiveItemE16, xlsMapping.ReceiveItemE17, xlsMapping.ReceiveItemE18, xlsMapping.ReceiveItemE19, xlsMapping.ReceiveItemE20,
                                        xlsMapping.ReceiveItemE21, xlsMapping.ReceiveItemE22, xlsMapping.ReceiveItemE23, xlsMapping.ReceiveItemE24, xlsMapping.ReceiveItemE25,
                                        xlsMapping.ReceiveItemE26, xlsMapping.ReceiveItemE27, xlsMapping.ReceiveItemE28, xlsMapping.ReceiveItemE29, xlsMapping.ReceiveItemE30,
                                        xlsMapping.ReceiveItemE31, xlsMapping.ReceiveItemE32, xlsMapping.ReceiveItemE33, xlsMapping.ReceiveItemE34, xlsMapping.ReceiveItemE35,
                                        xlsMapping.ReceiveItemE36, xlsMapping.ReceiveItemE37, xlsMapping.ReceiveItemE38, xlsMapping.ReceiveItemE39, xlsMapping.ReceiveItemE40
                                    };
                                    for (int idx = 0; idx < receiveItemNames.Length; idx++)
                                    {
                                        if (String.IsNullOrEmpty(receiveItemNames[idx]))
                                        {
                                            //沒有中文名稱，則不要英文名稱
                                            receiveItemNames[idx] = String.Empty;
                                        }
                                        else if (String.IsNullOrWhiteSpace(receiveItemENames[idx]))
                                        {
                                            //有中文名稱但沒英文名稱，則以中文名稱取代英文名稱
                                            receiveItemENames[idx] = receiveItemNames[idx];
                                        }
                                        else
                                        {
                                            //英文名稱去頭尾空白
                                            receiveItemENames[idx] = receiveItemENames[idx].Trim();
                                        }
                                    }
                                }
                                #endregion

                                #region 收集備註項目英文標題 （沒有中文就不要英文，有中文沒英文就用中文當英文）
                                if (memoTitles != null)
                                {
                                    memoETitles = new string[MappingreXlsmdbEntity.MemoCount] {
                                        xlsMapping.MemoTitleE01, xlsMapping.MemoTitleE02, xlsMapping.MemoTitleE03, xlsMapping.MemoTitleE04, xlsMapping.MemoTitleE05,
                                        xlsMapping.MemoTitleE06, xlsMapping.MemoTitleE07, xlsMapping.MemoTitleE08, xlsMapping.MemoTitleE09, xlsMapping.MemoTitleE10,
                                        xlsMapping.MemoTitleE11, xlsMapping.MemoTitleE12, xlsMapping.MemoTitleE13, xlsMapping.MemoTitleE14, xlsMapping.MemoTitleE15,
                                        xlsMapping.MemoTitleE16, xlsMapping.MemoTitleE17, xlsMapping.MemoTitleE18, xlsMapping.MemoTitleE19, xlsMapping.MemoTitleE20,
                                        xlsMapping.MemoTitleE21
                                    };
                                    for (int idx = 0; idx < memoTitles.Length; idx++)
                                    {
                                        if (String.IsNullOrEmpty(memoTitles[idx]))
                                        {
                                            //沒有中文標題，則不要英文標題
                                            memoETitles[idx] = String.Empty;
                                        }
                                        else if (String.IsNullOrWhiteSpace(receiveItemENames[idx]))
                                        {
                                            //有中文標題但沒英文標題，則以中文標題取代英文標題
                                            memoETitles[idx] = memoTitles[idx];
                                        }
                                        else
                                        {
                                            //英文標題去頭尾空白
                                            memoETitles[idx] = memoETitles[idx].Trim();
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                        break;

                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    case "ods":
                        #region Calc檔格式
                        {
                            Expression where = new Expression(MappingreXlsmdbEntity.Field.MappingId, mappingId)
                                .And(MappingreXlsmdbEntity.Field.ReceiveType, receiveType);
                            Result result = _Factory.SelectFirst<MappingreXlsmdbEntity>(where, null, out xlsMapping);
                            if (!result.IsSuccess)
                            {
                                logmsg = "讀取 Calc 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                                return new Result(false, logmsg, result.Code, result.Exception);
                            }
                            if (xlsMapping == null)
                            {
                                logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Calc 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                                return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                            }

                            #region 收集收入科目名稱
                            {
                                bool hasReceiveItemName = false;
                                receiveItemNames = new string[] {
                                    xlsMapping.Receive1, xlsMapping.Receive2, xlsMapping.Receive3, xlsMapping.Receive4, xlsMapping.Receive5,
                                    xlsMapping.Receive6, xlsMapping.Receive7, xlsMapping.Receive8, xlsMapping.Receive9, xlsMapping.Receive10,
                                    xlsMapping.Receive11, xlsMapping.Receive12, xlsMapping.Receive13, xlsMapping.Receive14, xlsMapping.Receive15,
                                    xlsMapping.Receive16, xlsMapping.Receive17, xlsMapping.Receive18, xlsMapping.Receive19, xlsMapping.Receive20,
                                    xlsMapping.Receive21, xlsMapping.Receive22, xlsMapping.Receive23, xlsMapping.Receive24, xlsMapping.Receive25,
                                    xlsMapping.Receive26, xlsMapping.Receive27, xlsMapping.Receive28, xlsMapping.Receive29, xlsMapping.Receive30,
                                    xlsMapping.Receive31, xlsMapping.Receive32, xlsMapping.Receive33, xlsMapping.Receive34, xlsMapping.Receive35,
                                    xlsMapping.Receive36, xlsMapping.Receive37, xlsMapping.Receive38, xlsMapping.Receive39, xlsMapping.Receive40
                                };
                                for (int idx = 0; idx < receiveItemNames.Length; idx++)
                                {
                                    if (String.IsNullOrWhiteSpace(receiveItemNames[idx]))
                                    {
                                        receiveItemNames[idx] = String.Empty;
                                    }
                                    else
                                    {
                                        hasReceiveItemName = true;
                                        receiveItemNames[idx] = receiveItemNames[idx].Trim();
                                    }
                                }
                                if (!hasReceiveItemName)
                                {
                                    //沒有指定任何收入科目，就不用收集了
                                    receiveItemNames = null;
                                }
                            }
                            #endregion

                            #region 收集備註標題
                            {
                                bool hasMemoTitle = false;
                                memoTitles = new string[MappingreXlsmdbEntity.MemoCount] {
                                    xlsMapping.Memo01, xlsMapping.Memo02, xlsMapping.Memo03, xlsMapping.Memo04, xlsMapping.Memo05,
                                    xlsMapping.Memo06, xlsMapping.Memo07, xlsMapping.Memo08, xlsMapping.Memo09, xlsMapping.Memo10,
                                    xlsMapping.Memo11, xlsMapping.Memo12, xlsMapping.Memo13, xlsMapping.Memo14, xlsMapping.Memo15,
                                    xlsMapping.Memo16, xlsMapping.Memo17, xlsMapping.Memo18, xlsMapping.Memo19, xlsMapping.Memo20,
                                    xlsMapping.Memo21
                                };
                                for (int idx = 0; idx < memoTitles.Length; idx++)
                                {
                                    if (String.IsNullOrWhiteSpace(memoTitles[idx]))
                                    {
                                        memoTitles[idx] = String.Empty;
                                    }
                                    else
                                    {
                                        hasMemoTitle = true;
                                        memoTitles[idx] = memoTitles[idx].Trim();
                                    }
                                }
                                if (!hasMemoTitle)
                                {
                                    //沒有指定任何備註標題，就不用收集了
                                    memoTitles = null;
                                }
                            }
                            #endregion

                            #region [MDY:202203XX] 2022擴充案 收集收入科目英文名稱、備註項目英文標題
                            if (isEngEnabled)
                            {
                                #region 收集收入科目英文名稱（沒有中文就不要英文，有中文沒英文就用中文當英文）
                                if (receiveItemNames != null)
                                {
                                    receiveItemENames = new string[] {
                                        xlsMapping.ReceiveItemE01, xlsMapping.ReceiveItemE02, xlsMapping.ReceiveItemE03, xlsMapping.ReceiveItemE04, xlsMapping.ReceiveItemE05,
                                        xlsMapping.ReceiveItemE06, xlsMapping.ReceiveItemE07, xlsMapping.ReceiveItemE08, xlsMapping.ReceiveItemE09, xlsMapping.ReceiveItemE10,
                                        xlsMapping.ReceiveItemE11, xlsMapping.ReceiveItemE12, xlsMapping.ReceiveItemE13, xlsMapping.ReceiveItemE14, xlsMapping.ReceiveItemE15,
                                        xlsMapping.ReceiveItemE16, xlsMapping.ReceiveItemE17, xlsMapping.ReceiveItemE18, xlsMapping.ReceiveItemE19, xlsMapping.ReceiveItemE20,
                                        xlsMapping.ReceiveItemE21, xlsMapping.ReceiveItemE22, xlsMapping.ReceiveItemE23, xlsMapping.ReceiveItemE24, xlsMapping.ReceiveItemE25,
                                        xlsMapping.ReceiveItemE26, xlsMapping.ReceiveItemE27, xlsMapping.ReceiveItemE28, xlsMapping.ReceiveItemE29, xlsMapping.ReceiveItemE30,
                                        xlsMapping.ReceiveItemE31, xlsMapping.ReceiveItemE32, xlsMapping.ReceiveItemE33, xlsMapping.ReceiveItemE34, xlsMapping.ReceiveItemE35,
                                        xlsMapping.ReceiveItemE36, xlsMapping.ReceiveItemE37, xlsMapping.ReceiveItemE38, xlsMapping.ReceiveItemE39, xlsMapping.ReceiveItemE40
                                    };
                                    for (int idx = 0; idx < receiveItemNames.Length; idx++)
                                    {
                                        if (String.IsNullOrEmpty(receiveItemNames[idx]))
                                        {
                                            //沒有中文名稱，則不要英文名稱
                                            receiveItemNames[idx] = String.Empty;
                                        }
                                        else if (String.IsNullOrWhiteSpace(receiveItemENames[idx]))
                                        {
                                            //有中文名稱但沒英文名稱，則以中文名稱取代英文名稱
                                            receiveItemENames[idx] = receiveItemNames[idx];
                                        }
                                        else
                                        {
                                            //英文名稱去頭尾空白
                                            receiveItemENames[idx] = receiveItemENames[idx].Trim();
                                        }
                                    }
                                }
                                #endregion

                                #region 收集備註項目英文標題 （沒有中文就不要英文，有中文沒英文就用中文當英文）
                                if (memoTitles != null)
                                {
                                    memoETitles = new string[MappingreXlsmdbEntity.MemoCount] {
                                        xlsMapping.MemoTitleE01, xlsMapping.MemoTitleE02, xlsMapping.MemoTitleE03, xlsMapping.MemoTitleE04, xlsMapping.MemoTitleE05,
                                        xlsMapping.MemoTitleE06, xlsMapping.MemoTitleE07, xlsMapping.MemoTitleE08, xlsMapping.MemoTitleE09, xlsMapping.MemoTitleE10,
                                        xlsMapping.MemoTitleE11, xlsMapping.MemoTitleE12, xlsMapping.MemoTitleE13, xlsMapping.MemoTitleE14, xlsMapping.MemoTitleE15,
                                        xlsMapping.MemoTitleE16, xlsMapping.MemoTitleE17, xlsMapping.MemoTitleE18, xlsMapping.MemoTitleE19, xlsMapping.MemoTitleE20,
                                        xlsMapping.MemoTitleE21
                                    };
                                    for (int idx = 0; idx < memoTitles.Length; idx++)
                                    {
                                        if (String.IsNullOrEmpty(memoTitles[idx]))
                                        {
                                            //沒有中文標題，則不要英文標題
                                            memoETitles[idx] = String.Empty;
                                        }
                                        else if (String.IsNullOrWhiteSpace(receiveItemENames[idx]))
                                        {
                                            //有中文標題但沒英文標題，則以中文標題取代英文標題
                                            memoETitles[idx] = memoTitles[idx];
                                        }
                                        else
                                        {
                                            //英文標題去頭尾空白
                                            memoETitles[idx] = memoETitles[idx].Trim();
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    #endregion
                }
            }
            #endregion

            #region 代收費用別設定
            bool isAddSchoolRid = false;
            SchoolRidEntity schoolRid = null;
            //有收集到收入科目或備註標題再取 SchoolRidEntity
            if ((receiveItemNames != null && receiveItemNames.Length > 0) || (memoTitles != null && memoTitles.Length > 0))
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRidEntity.Field.YearId, yearId)
                    .And(SchoolRidEntity.Field.TermId, termId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, receiveId);
                Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取代收費用別設定資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (schoolRid == null)
                {
                    #region 沒有代收費用別，就 New 一個預設的代收費用別，後面要新增
                    isAddSchoolRid = true;

                    schoolRid = new SchoolRidEntity();
                    schoolRid.ReceiveType = receiveType;
                    schoolRid.YearId = yearId;
                    schoolRid.TermId = termId;
                    schoolRid.DepId = depId;
                    schoolRid.ReceiveId = receiveId;
                    schoolRid.ReceiveStatus = String.Empty;

                    #region 收入科目名稱
                    if (receiveItemNames != null && receiveItemNames.Length > 0)
                    {
                        schoolRid.ReceiveItem01 = receiveItemNames[00];
                        schoolRid.ReceiveItem02 = receiveItemNames[01];
                        schoolRid.ReceiveItem03 = receiveItemNames[02];
                        schoolRid.ReceiveItem04 = receiveItemNames[03];
                        schoolRid.ReceiveItem05 = receiveItemNames[04];
                        schoolRid.ReceiveItem06 = receiveItemNames[05];
                        schoolRid.ReceiveItem07 = receiveItemNames[06];
                        schoolRid.ReceiveItem08 = receiveItemNames[07];
                        schoolRid.ReceiveItem09 = receiveItemNames[08];
                        schoolRid.ReceiveItem10 = receiveItemNames[09];

                        schoolRid.ReceiveItem11 = receiveItemNames[10];
                        schoolRid.ReceiveItem12 = receiveItemNames[11];
                        schoolRid.ReceiveItem13 = receiveItemNames[12];
                        schoolRid.ReceiveItem14 = receiveItemNames[13];
                        schoolRid.ReceiveItem15 = receiveItemNames[14];
                        schoolRid.ReceiveItem16 = receiveItemNames[15];
                        schoolRid.ReceiveItem17 = receiveItemNames[16];
                        schoolRid.ReceiveItem18 = receiveItemNames[17];
                        schoolRid.ReceiveItem19 = receiveItemNames[18];
                        schoolRid.ReceiveItem20 = receiveItemNames[19];

                        schoolRid.ReceiveItem21 = receiveItemNames[20];
                        schoolRid.ReceiveItem22 = receiveItemNames[21];
                        schoolRid.ReceiveItem23 = receiveItemNames[22];
                        schoolRid.ReceiveItem24 = receiveItemNames[23];
                        schoolRid.ReceiveItem25 = receiveItemNames[24];
                        schoolRid.ReceiveItem26 = receiveItemNames[25];
                        schoolRid.ReceiveItem27 = receiveItemNames[26];
                        schoolRid.ReceiveItem28 = receiveItemNames[27];
                        schoolRid.ReceiveItem29 = receiveItemNames[28];
                        schoolRid.ReceiveItem30 = receiveItemNames[29];

                        schoolRid.ReceiveItem31 = receiveItemNames[30];
                        schoolRid.ReceiveItem32 = receiveItemNames[31];
                        schoolRid.ReceiveItem33 = receiveItemNames[32];
                        schoolRid.ReceiveItem34 = receiveItemNames[33];
                        schoolRid.ReceiveItem35 = receiveItemNames[34];
                        schoolRid.ReceiveItem36 = receiveItemNames[35];
                        schoolRid.ReceiveItem37 = receiveItemNames[36];
                        schoolRid.ReceiveItem38 = receiveItemNames[37];
                        schoolRid.ReceiveItem39 = receiveItemNames[38];
                        schoolRid.ReceiveItem40 = receiveItemNames[39];
                    }
                    #endregion

                    #region 備註標題
                    if (memoTitles != null && memoTitles.Length > 0)
                    {
                        schoolRid.MemoTitle01 = memoTitles[00];
                        schoolRid.MemoTitle02 = memoTitles[01];
                        schoolRid.MemoTitle03 = memoTitles[02];
                        schoolRid.MemoTitle04 = memoTitles[03];
                        schoolRid.MemoTitle05 = memoTitles[04];
                        schoolRid.MemoTitle06 = memoTitles[05];
                        schoolRid.MemoTitle07 = memoTitles[06];
                        schoolRid.MemoTitle08 = memoTitles[07];
                        schoolRid.MemoTitle09 = memoTitles[08];
                        schoolRid.MemoTitle10 = memoTitles[09];

                        schoolRid.MemoTitle11 = memoTitles[10];
                        schoolRid.MemoTitle12 = memoTitles[11];
                        schoolRid.MemoTitle13 = memoTitles[12];
                        schoolRid.MemoTitle14 = memoTitles[13];
                        schoolRid.MemoTitle15 = memoTitles[14];
                        schoolRid.MemoTitle16 = memoTitles[15];
                        schoolRid.MemoTitle17 = memoTitles[16];
                        schoolRid.MemoTitle18 = memoTitles[17];
                        schoolRid.MemoTitle19 = memoTitles[18];
                        schoolRid.MemoTitle20 = memoTitles[19];

                        schoolRid.MemoTitle21 = memoTitles[20];
                    }
                    #endregion

                    #region [MDY:202203XX] 2022擴充案 收入科目英文名稱、備註項目英文標題
                    if (isEngEnabled)
                    {
                        #region 收入科目英文名稱
                        if (receiveItemENames != null && receiveItemENames.Length > 0)
                        {
                            schoolRid.ReceiveItemE01 = receiveItemENames[00];
                            schoolRid.ReceiveItemE02 = receiveItemENames[01];
                            schoolRid.ReceiveItemE03 = receiveItemENames[02];
                            schoolRid.ReceiveItemE04 = receiveItemENames[03];
                            schoolRid.ReceiveItemE05 = receiveItemENames[04];
                            schoolRid.ReceiveItemE06 = receiveItemENames[05];
                            schoolRid.ReceiveItemE07 = receiveItemENames[06];
                            schoolRid.ReceiveItemE08 = receiveItemENames[07];
                            schoolRid.ReceiveItemE09 = receiveItemENames[08];
                            schoolRid.ReceiveItemE10 = receiveItemENames[09];

                            schoolRid.ReceiveItemE11 = receiveItemENames[10];
                            schoolRid.ReceiveItemE12 = receiveItemENames[11];
                            schoolRid.ReceiveItemE13 = receiveItemENames[12];
                            schoolRid.ReceiveItemE14 = receiveItemENames[13];
                            schoolRid.ReceiveItemE15 = receiveItemENames[14];
                            schoolRid.ReceiveItemE16 = receiveItemENames[15];
                            schoolRid.ReceiveItemE17 = receiveItemENames[16];
                            schoolRid.ReceiveItemE18 = receiveItemENames[17];
                            schoolRid.ReceiveItemE19 = receiveItemENames[18];
                            schoolRid.ReceiveItemE20 = receiveItemENames[19];

                            schoolRid.ReceiveItemE21 = receiveItemENames[20];
                            schoolRid.ReceiveItemE22 = receiveItemENames[21];
                            schoolRid.ReceiveItemE23 = receiveItemENames[22];
                            schoolRid.ReceiveItemE24 = receiveItemENames[23];
                            schoolRid.ReceiveItemE25 = receiveItemENames[24];
                            schoolRid.ReceiveItemE26 = receiveItemENames[25];
                            schoolRid.ReceiveItemE27 = receiveItemENames[26];
                            schoolRid.ReceiveItemE28 = receiveItemENames[27];
                            schoolRid.ReceiveItemE29 = receiveItemENames[28];
                            schoolRid.ReceiveItemE30 = receiveItemENames[29];

                            schoolRid.ReceiveItemE31 = receiveItemENames[30];
                            schoolRid.ReceiveItemE32 = receiveItemENames[31];
                            schoolRid.ReceiveItemE33 = receiveItemENames[32];
                            schoolRid.ReceiveItemE34 = receiveItemENames[33];
                            schoolRid.ReceiveItemE35 = receiveItemENames[34];
                            schoolRid.ReceiveItemE36 = receiveItemENames[35];
                            schoolRid.ReceiveItemE37 = receiveItemENames[36];
                            schoolRid.ReceiveItemE38 = receiveItemENames[37];
                            schoolRid.ReceiveItemE39 = receiveItemENames[38];
                            schoolRid.ReceiveItemE40 = receiveItemENames[39];
                        }
                        #endregion

                        #region 備註項目英文標題
                        if (memoETitles != null && memoETitles.Length > 0)
                        {
                            schoolRid.MemoTitleE01 = memoETitles[00];
                            schoolRid.MemoTitleE02 = memoETitles[01];
                            schoolRid.MemoTitleE03 = memoETitles[02];
                            schoolRid.MemoTitleE04 = memoETitles[03];
                            schoolRid.MemoTitleE05 = memoETitles[04];
                            schoolRid.MemoTitleE06 = memoETitles[05];
                            schoolRid.MemoTitleE07 = memoETitles[06];
                            schoolRid.MemoTitleE08 = memoETitles[07];
                            schoolRid.MemoTitleE09 = memoETitles[08];
                            schoolRid.MemoTitleE10 = memoETitles[09];

                            schoolRid.MemoTitleE11 = memoETitles[10];
                            schoolRid.MemoTitleE12 = memoETitles[11];
                            schoolRid.MemoTitleE13 = memoETitles[12];
                            schoolRid.MemoTitleE14 = memoETitles[13];
                            schoolRid.MemoTitleE15 = memoETitles[14];
                            schoolRid.MemoTitleE16 = memoETitles[15];
                            schoolRid.MemoTitleE17 = memoETitles[16];
                            schoolRid.MemoTitleE18 = memoETitles[17];
                            schoolRid.MemoTitleE19 = memoETitles[18];
                            schoolRid.MemoTitleE20 = memoETitles[19];

                            schoolRid.MemoTitleE21 = memoETitles[20];
                        }
                        #endregion
                    }
                    #endregion

                    #region 收入科目是否助貸旗標
                    schoolRid.LoanItem01 = "N";
                    schoolRid.LoanItem02 = "N";
                    schoolRid.LoanItem03 = "N";
                    schoolRid.LoanItem04 = "N";
                    schoolRid.LoanItem05 = "N";
                    schoolRid.LoanItem06 = "N";
                    schoolRid.LoanItem07 = "N";
                    schoolRid.LoanItem08 = "N";
                    schoolRid.LoanItem09 = "N";
                    schoolRid.LoanItem10 = "N";

                    schoolRid.LoanItem11 = "N";
                    schoolRid.LoanItem12 = "N";
                    schoolRid.LoanItem13 = "N";
                    schoolRid.LoanItem14 = "N";
                    schoolRid.LoanItem15 = "N";
                    schoolRid.LoanItem16 = "N";
                    schoolRid.LoanItem17 = "N";
                    schoolRid.LoanItem18 = "N";
                    schoolRid.LoanItem19 = "N";
                    schoolRid.LoanItem20 = "N";

                    schoolRid.LoanItem21 = "N";
                    schoolRid.LoanItem22 = "N";
                    schoolRid.LoanItem23 = "N";
                    schoolRid.LoanItem24 = "N";
                    schoolRid.LoanItem25 = "N";
                    schoolRid.LoanItem26 = "N";
                    schoolRid.LoanItem27 = "N";
                    schoolRid.LoanItem28 = "N";
                    schoolRid.LoanItem29 = "N";
                    schoolRid.LoanItem30 = "N";

                    schoolRid.LoanItem31 = "N";
                    schoolRid.LoanItem32 = "N";
                    schoolRid.LoanItem33 = "N";
                    schoolRid.LoanItem34 = "N";
                    schoolRid.LoanItem35 = "N";
                    schoolRid.LoanItem36 = "N";
                    schoolRid.LoanItem37 = "N";
                    schoolRid.LoanItem38 = "N";
                    schoolRid.LoanItem39 = "N";
                    schoolRid.LoanItem40 = "N";

                    #region [MDY:202203XX] 2022擴充案 對應欄位已擴充，不再使用，固定設為 空白
                    #region [OLD]
                    //schoolRid.LoanItemOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    schoolRid.LoanItemOthers = String.Empty;
                    #endregion
                    #endregion

                    #region 收入科目是否代辦旗標
                    schoolRid.AgencyItem01 = "N";
                    schoolRid.AgencyItem02 = "N";
                    schoolRid.AgencyItem03 = "N";
                    schoolRid.AgencyItem04 = "N";
                    schoolRid.AgencyItem05 = "N";
                    schoolRid.AgencyItem06 = "N";
                    schoolRid.AgencyItem07 = "N";
                    schoolRid.AgencyItem08 = "N";
                    schoolRid.AgencyItem09 = "N";
                    schoolRid.AgencyItem10 = "N";

                    schoolRid.AgencyItem11 = "N";
                    schoolRid.AgencyItem12 = "N";
                    schoolRid.AgencyItem13 = "N";
                    schoolRid.AgencyItem14 = "N";
                    schoolRid.AgencyItem15 = "N";
                    schoolRid.AgencyItem16 = "N";
                    schoolRid.AgencyItem17 = "N";
                    schoolRid.AgencyItem18 = "N";
                    schoolRid.AgencyItem19 = "N";
                    schoolRid.AgencyItem20 = "N";

                    schoolRid.AgencyItem21 = "N";
                    schoolRid.AgencyItem22 = "N";
                    schoolRid.AgencyItem23 = "N";
                    schoolRid.AgencyItem24 = "N";
                    schoolRid.AgencyItem25 = "N";
                    schoolRid.AgencyItem26 = "N";
                    schoolRid.AgencyItem27 = "N";
                    schoolRid.AgencyItem28 = "N";
                    schoolRid.AgencyItem29 = "N";
                    schoolRid.AgencyItem30 = "N";

                    schoolRid.AgencyItem31 = "N";
                    schoolRid.AgencyItem32 = "N";
                    schoolRid.AgencyItem33 = "N";
                    schoolRid.AgencyItem34 = "N";
                    schoolRid.AgencyItem35 = "N";
                    schoolRid.AgencyItem36 = "N";
                    schoolRid.AgencyItem37 = "N";
                    schoolRid.AgencyItem38 = "N";
                    schoolRid.AgencyItem39 = "N";
                    schoolRid.AgencyItem40 = "N";

                    #region [MDY:202203XX] 2022擴充案 對應欄位已擴充，不再使用，固定設為 空白
                    #region [OLD]
                    //schoolRid.AgencyItemOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    schoolRid.AgencyItemOthers = String.Empty;
                    #endregion
                    #endregion

                    #region 收入科目是否可更改旗標
                    schoolRid.AgencyCheck01 = "N";
                    schoolRid.AgencyCheck02 = "N";
                    schoolRid.AgencyCheck03 = "N";
                    schoolRid.AgencyCheck04 = "N";
                    schoolRid.AgencyCheck05 = "N";
                    schoolRid.AgencyCheck06 = "N";
                    schoolRid.AgencyCheck07 = "N";
                    schoolRid.AgencyCheck08 = "N";
                    schoolRid.AgencyCheck09 = "N";
                    schoolRid.AgencyCheck10 = "N";

                    schoolRid.AgencyCheck11 = "N";
                    schoolRid.AgencyCheck12 = "N";
                    schoolRid.AgencyCheck13 = "N";
                    schoolRid.AgencyCheck14 = "N";
                    schoolRid.AgencyCheck15 = "N";
                    schoolRid.AgencyCheck16 = "N";
                    schoolRid.AgencyCheck17 = "N";
                    schoolRid.AgencyCheck18 = "N";
                    schoolRid.AgencyCheck19 = "N";
                    schoolRid.AgencyCheck20 = "N";

                    schoolRid.AgencyCheck21 = "N";
                    schoolRid.AgencyCheck22 = "N";
                    schoolRid.AgencyCheck23 = "N";
                    schoolRid.AgencyCheck24 = "N";
                    schoolRid.AgencyCheck25 = "N";
                    schoolRid.AgencyCheck26 = "N";
                    schoolRid.AgencyCheck27 = "N";
                    schoolRid.AgencyCheck28 = "N";
                    schoolRid.AgencyCheck29 = "N";
                    schoolRid.AgencyCheck30 = "N";

                    schoolRid.AgencyCheck31 = "N";
                    schoolRid.AgencyCheck32 = "N";
                    schoolRid.AgencyCheck33 = "N";
                    schoolRid.AgencyCheck34 = "N";
                    schoolRid.AgencyCheck35 = "N";
                    schoolRid.AgencyCheck36 = "N";
                    schoolRid.AgencyCheck37 = "N";
                    schoolRid.AgencyCheck38 = "N";
                    schoolRid.AgencyCheck39 = "N";
                    schoolRid.AgencyCheck40 = "N";

                    #region [MDY:202203XX] 2022擴充案 對應欄位已擴充，不再使用，固定設為 空白
                    #region [OLD]
                    //schoolRid.AgencyCheckOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    schoolRid.AgencyCheckOthers = String.Empty;
                    #endregion
                    #endregion

                    #region 收入科目是否教育部補助旗標
                    schoolRid.Issubsidy01 = "N";
                    schoolRid.Issubsidy02 = "N";
                    schoolRid.Issubsidy03 = "N";
                    schoolRid.Issubsidy04 = "N";
                    schoolRid.Issubsidy05 = "N";
                    schoolRid.Issubsidy06 = "N";
                    schoolRid.Issubsidy07 = "N";
                    schoolRid.Issubsidy08 = "N";
                    schoolRid.Issubsidy09 = "N";
                    schoolRid.Issubsidy10 = "N";

                    schoolRid.Issubsidy11 = "N";
                    schoolRid.Issubsidy12 = "N";
                    schoolRid.Issubsidy13 = "N";
                    schoolRid.Issubsidy14 = "N";
                    schoolRid.Issubsidy15 = "N";
                    schoolRid.Issubsidy16 = "N";
                    schoolRid.Issubsidy17 = "N";
                    schoolRid.Issubsidy18 = "N";
                    schoolRid.Issubsidy19 = "N";
                    schoolRid.Issubsidy20 = "N";

                    schoolRid.Issubsidy21 = "N";
                    schoolRid.Issubsidy22 = "N";
                    schoolRid.Issubsidy23 = "N";
                    schoolRid.Issubsidy24 = "N";
                    schoolRid.Issubsidy25 = "N";
                    schoolRid.Issubsidy26 = "N";
                    schoolRid.Issubsidy27 = "N";
                    schoolRid.Issubsidy28 = "N";
                    schoolRid.Issubsidy29 = "N";
                    schoolRid.Issubsidy30 = "N";

                    schoolRid.Issubsidy31 = "N";
                    schoolRid.Issubsidy32 = "N";
                    schoolRid.Issubsidy33 = "N";
                    schoolRid.Issubsidy34 = "N";
                    schoolRid.Issubsidy35 = "N";
                    schoolRid.Issubsidy36 = "N";
                    schoolRid.Issubsidy37 = "N";
                    schoolRid.Issubsidy38 = "N";
                    schoolRid.Issubsidy39 = "N";
                    schoolRid.Issubsidy40 = "N";

                    #region [MDY:202203XX] 2022擴充案 對應欄位已擴充，不再使用，固定設為 空白
                    #region [OLD]
                    //schoolRid.IssubsidyOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    schoolRid.IssubsidyOthers = String.Empty;
                    #endregion
                    #endregion

                    schoolRid.BillCloseDate = String.Empty;

                    schoolRid.BillingType = "1";
                    schoolRid.FlagRl = "1";
                    schoolRid.LoanQual = "1";
                    schoolRid.Brief1 = String.Empty;
                    schoolRid.Brief2 = String.Empty;
                    schoolRid.Brief3 = String.Empty;
                    schoolRid.Brief4 = String.Empty;
                    schoolRid.Brief5 = String.Empty;
                    schoolRid.Brief6 = String.Empty;
                    schoolRid.Hide = String.Empty;
                    schoolRid.SchLevel = String.Empty;
                    schoolRid.EnabledTax = "N";
                    schoolRid.EduTax = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
                    schoolRid.StayTax = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";

                    schoolRid.Usestamp = "N";
                    schoolRid.Usewatermark = "N";
                    schoolRid.Usepostdueday = String.Empty;

                    schoolRid.UpdateTime = DateTime.Now;
                    schoolRid.UpdateWho = owner;
                    #endregion
                }
            }
            #endregion

            #region [MDY:202203XX] 2022擴充案 因為要判斷是否啟用英文資料，所以取得商家代號資料程式段往前搬
            #region [OLD] 取得商家代號資料
            //SchoolRTypeEntity school = null;
            //{
            //    Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
            //    Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
            //    if (!result.IsSuccess)
            //    {
            //        logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
            //        return new Result(false, logmsg, result.Code, result.Exception);
            //    }
            //    if (school == null)
            //    {
            //        logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
            //        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
            //    }
            //}
            #endregion
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();
            {
                string errmsg = null;
                ConvertFileHelper fileHelper = new ConvertFileHelper();
                switch (fileType)
                {
                    case "txt":
                        #region Txt 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            using (StreamReader reader = new StreamReader(ms, encoding))
                            {
                                if (!fileHelper.Txt2DataTable(reader, txtMapping.GetMapFields(), isBatch, true, out table, out totalCount, out successCount, out errmsg))
                                {
                                    #region 轉失敗則收集每一筆資料的錯誤訊息
                                    if (table != null && table.Rows.Count > 0)
                                    {
                                        StringBuilder log = new StringBuilder();
                                        int rowNo = 0;
                                        foreach (DataRow row in table.Rows)
                                        {
                                            rowNo++;
                                            string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                            if (!String.IsNullOrEmpty(failMsg))
                                            {
                                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                            }
                                        }
                                        logmsg = log.ToString();
                                    }
                                    #endregion
                                    return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                                }
                            }
                        }
                        #endregion
                        break;
                    case "xls":
                        #region Xls 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            if (!fileHelper.Xls2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg))
                            {
                                #region 轉失敗則收集每一筆資料的錯誤訊息
                                if (table != null && table.Rows.Count > 0)
                                {
                                    StringBuilder log = new StringBuilder();
                                    int rowNo = 0;
                                    foreach (DataRow row in table.Rows)
                                    {
                                        rowNo++;
                                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                        if (!String.IsNullOrEmpty(failMsg))
                                        {
                                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        }
                                    }
                                    logmsg = log.ToString();
                                }
                                #endregion
                                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        #endregion
                        break;
                    case "xlsx":
                        #region Xlsx 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            if (!fileHelper.Xlsx2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg))
                            {
                                #region 轉失敗則收集每一筆資料的錯誤訊息
                                if (table != null && table.Rows.Count > 0)
                                {
                                    StringBuilder log = new StringBuilder();
                                    int rowNo = 0;
                                    foreach (DataRow row in table.Rows)
                                    {
                                        rowNo++;
                                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                        if (!String.IsNullOrEmpty(failMsg))
                                        {
                                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        }
                                    }
                                    logmsg = log.ToString();
                                }
                                #endregion
                                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        #endregion
                        break;

                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    case "ods":
                        #region Ods 轉 DataTable
                        {
                            bool isOK = false;
                            using (MemoryStream ms = new MemoryStream(fileContent))
                            {
                                isOK = fileHelper.Ods2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                            }
                            if (!isOK)
                            {
                                #region 轉失敗則收集每一筆資料的錯誤訊息
                                if (table != null && table.Rows.Count > 0)
                                {
                                    StringBuilder log = new StringBuilder();
                                    int rowNo = 0;
                                    foreach (DataRow row in table.Rows)
                                    {
                                        rowNo++;
                                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                        if (!String.IsNullOrEmpty(failMsg))
                                        {
                                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        }
                                    }
                                    logmsg = log.ToString();
                                }
                                #endregion
                                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        #endregion
                        break;
                    #endregion

                    default:
                        #region 不支援
                        {
                            logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                            return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        #endregion
                }

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }

                #region 檢查學號必要欄位
                if (!fieldNames.Contains(MappingreXlsmdbEntity.Field.StuId))
                {
                    logmsg = "此對照檔缺少學號欄位";
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                #endregion

                #region [MDY:20200815] M202008_02 學生身分證字號不可與學號相同 (2020806_01)
                if (table != null && table.Rows.Count > 0
                    && fieldNames.Contains(MappingreXlsmdbEntity.Field.StuId) && fieldNames.Contains(MappingreXlsmdbEntity.Field.IdNumber))
                {
                    foreach (DataRow dRow in table.Rows)
                    {
                        string failMsg = dRow.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : dRow[ConvertFileHelper.DataLineFailureFieldName].ToString();
                        if (String.IsNullOrEmpty(failMsg))
                        {
                            string stuId = dRow.IsNull(MappingreXlsmdbEntity.Field.StuId) ? null : dRow[MappingreXlsmdbEntity.Field.StuId].ToString();
                            string isNumber = dRow.IsNull(MappingreXlsmdbEntity.Field.IdNumber) ? null : dRow[MappingreXlsmdbEntity.Field.IdNumber].ToString();
                            if (!String.IsNullOrEmpty(isNumber) && isNumber.Equals(stuId))
                            {
                                dRow[ConvertFileHelper.DataLineFailureFieldName] = "學生身分證字號不可與學號相同";
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 使用交易將 DataTable 逐筆存入資料庫
            {
                successCount = 0;
                DateTime now = DateTime.Now;
                Result importResult = new Result(true);
                StringBuilder log = new StringBuilder();
                int insertCount = 0;    //新增 StudentReceive 的筆數
                int updateCount = 0;    //更新 StudentReceive 的筆數
                KeyValueList<int> updateUpNoCounts = new KeyValueList<int>();     //更新 StudentReceive 所屬批號的筆數
                using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
                {
                    #region [MDY:20160131] 改成同一批資料學號 + 序號不可學號重複
                    #region [Old]
                    //List<string> studentIds = new List<string>(table.Rows.Count);   //紀錄出現過的學號，用來檢查同一批資料不可學號重複
                    #endregion

                    List<string> studentKeys = new List<string>(table.Rows.Count);      //同一批資料學號 + 序號不可學號重複
                    #endregion

                    BillAmountHelper amountHelper = new BillAmountHelper();

                    List<ClassListEntity> classDatas = new List<ClassListEntity>();
                    List<DeptListEntity> deptDatas = new List<DeptListEntity>();
                    List<CollegeListEntity> collegeDatas = new List<CollegeListEntity>();
                    List<MajorListEntity> majorDatas = new List<MajorListEntity>();
                    List<ReduceListEntity> reduceDatas = new List<ReduceListEntity>();
                    List<LoanListEntity> loanDatas = new List<LoanListEntity>();
                    List<DormListEntity> dormDatas = new List<DormListEntity>();

                    List<IdentifyList1Entity> identify1Datas = new List<IdentifyList1Entity>();
                    List<IdentifyList2Entity> identify2Datas = new List<IdentifyList2Entity>();
                    List<IdentifyList3Entity> identify3Datas = new List<IdentifyList3Entity>();
                    List<IdentifyList4Entity> identify4Datas = new List<IdentifyList4Entity>();
                    List<IdentifyList5Entity> identify5Datas = new List<IdentifyList5Entity>();
                    List<IdentifyList6Entity> identify6Datas = new List<IdentifyList6Entity>();

                    bool hasPayDueDateField = fieldNames.Contains(MappingreXlsmdbEntity.Field.PayDueDate);

                    int rowNo = 0;
                    int okNo = 1;
                    foreach (DataRow row in table.Rows)
                    {
                        rowNo++;

                        #region 取得資料行錯誤訊息
                        {
                            string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                            if (!String.IsNullOrEmpty(failMsg))
                            {
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                continue;
                            }
                        }
                        #endregion

                        string studentId = row[MappingreXlsmdbEntity.Field.StuId].ToString();  //學號欄位一定要有

                        #region [MDY:20160131] 增加資料序號，未上傳則以 0 處理
                        int oldSeq = 0;
                        if (fieldNames.Contains(MappingreXlsmdbEntity.Field.OldSeq))
                        {
                            if (!int.TryParse(row[MappingreXlsmdbEntity.Field.OldSeq].ToString(), out oldSeq) || oldSeq < 0)
                            {
                                string failMsg = String.Format("序號 不是有效的數字");
                                row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        #endregion

                        #region [MDY:20160131] 改成同一批資料學號 + 序號不可學號重複
                        #region [Old]
                        //#region 同一批上傳資料學號不可重複
                        //if (studentIds.Contains(studentId))
                        //{
                        //    string failMsg = String.Format("學號 {0} 重複", studentId);
                        //    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                        //    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                        //    if (isBatch)
                        //    {
                        //        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }
                        //}
                        //else
                        //{
                        //    studentIds.Add(studentId);
                        //}
                        //#endregion
                        #endregion

                        #region 同一批資料學號 + 序號不可學號重複
                        string studentKey = String.Format("{0},{1}", studentId, oldSeq);
                        if (studentKeys.Contains(studentKey))
                        {
                            string failMsg = oldSeq == 0 ? String.Format("學號 {0} 重複", studentId) : String.Format("學號加序號 ({0}) 重複", studentKey);
                            row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                            if (isBatch)
                            {
                                importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            studentKeys.Add(studentKey);
                        }
                        #endregion
                        #endregion

                        #region [MDY:20160131] 增加繳款期限
                        string payDueDate = hasPayDueDateField ? row[MappingreXlsmdbEntity.Field.PayDueDate].ToString().Trim() : null;
                        if (!String.IsNullOrWhiteSpace(payDueDate))
                        {
                            //如果是日期的字串，格式化成 TWDate7
                            DateTime? date = DataFormat.ConvertDateText(payDueDate);
                            if (date != null)
                            {
                                payDueDate = Common.GetTWDate7(date.Value);
                            }
                            else
                            {
                                string failMsg = String.Format("繳款期限 不是有效的日期");
                                row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
                        string nCCardFlag = null;
                        if (fieldNames.Contains(MappingreXlsmdbEntity.Field.NCCardFlag))
                        {
                            string arg = row[MappingreXlsmdbEntity.Field.NCCardFlag].ToString().Trim();
                            if ("是".Equals(arg))
                            {
                                nCCardFlag = "Y";
                            }
                            else if ("否".Equals(arg))
                            {
                                nCCardFlag = "N";
                            }
                            else
                            {
                                nCCardFlag = arg.ToUpper();
                            }
                        }
                        #endregion

                        #region 產生各種 Entity
                        #region 學生資料對照欄位 (StudentMasterEntity)
                        StudentMasterEntity student = new StudentMasterEntity();
                        student.ReceiveType = receiveType;
                        student.DepId = depId;
                        student.Id = studentId;

                        //雖然目前是對照檔的欄位是固定的，但為了相容性以下的程式還是用 fieldNames.Contains 來決定是否取值
                        student.Name = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuName) ? row[MappingreXlsmdbEntity.Field.StuName].ToString() : String.Empty;
                        student.Birthday = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuBirthday) ? row[MappingreXlsmdbEntity.Field.StuBirthday].ToString() : String.Empty;
                        if (!String.IsNullOrWhiteSpace(student.Birthday))
                        {
                            //如果是日期的字串，格式化成 TWDate7
                            DateTime? date = DataFormat.ConvertDateText(student.Birthday.Trim());
                            if (date != null)
                            {
                                student.Birthday = Common.GetTWDate7(date.Value);
                            }
                        }
                        student.IdNumber = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdNumber) ? row[MappingreXlsmdbEntity.Field.IdNumber].ToString().ToUpper() : String.Empty;
                        student.Tel = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuTel) ? row[MappingreXlsmdbEntity.Field.StuTel].ToString() : String.Empty;
                        student.ZipCode = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuAddcode) ? row[MappingreXlsmdbEntity.Field.StuAddcode].ToString() : String.Empty;
                        student.Address = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuAdd) ? row[MappingreXlsmdbEntity.Field.StuAdd].ToString() : String.Empty;
                        student.Email = fieldNames.Contains(MappingreXlsmdbEntity.Field.Email) ? row[MappingreXlsmdbEntity.Field.Email].ToString() : String.Empty;
                        student.Account = String.Empty; //土銀不使用，所以固定為空字串
                        student.StuParent = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuParent) ? row[MappingreXlsmdbEntity.Field.StuParent].ToString() : String.Empty;
                        student.CrtDate = now;
                        student.MdyDate = null;
                        #endregion

                        #region [Old] 同一批上傳資料學號不可重複
                        //if (studentIds.Contains(student.Id))
                        //{
                        //    string failMsg = String.Format("學號 {0} 重複", student.Id);
                        //    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                        //    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                        //    if (isBatch)
                        //    {
                        //        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }
                        //}
                        //else
                        //{
                        //    studentIds.Add(student.Id);
                        //}
                        #endregion

                        StudentReceiveEntity receive = new StudentReceiveEntity();
                        receive.ReceiveType = receiveType;
                        receive.YearId = yearId;
                        receive.TermId = termId;
                        receive.DepId = depId;
                        receive.ReceiveId = receiveId;
                        receive.StuId = student.Id;

                        #region [MDY:20160131] 增加資料序號，未上傳則以 0 處理
                        #region [Old]
                        //receive.OldSeq = 0; //系統不處理舊學雜費轉置的資料，所以固定為 0
                        #endregion

                        receive.OldSeq = oldSeq;
                        #endregion

                        #region [MDY:20160131] 增加繳款期限
                        receive.PayDueDate = payDueDate;
                        #endregion

                        #region [MDY:20191214] (2019擴充案) 國際信用卡 - 是否啟用國際信用卡繳費旗標
                        receive.NCCardFlag = nCCardFlag;
                        #endregion

                        int uploadFlag = 0;

                        #region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
                        ClassListEntity classEntity = null;
                        DeptListEntity dept = null;
                        CollegeListEntity college = null;
                        MajorListEntity major = null;
                        {
                            receive.StuGrade = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuGrade) ? row[MappingreXlsmdbEntity.Field.StuGrade].ToString() : String.Empty;
                            receive.StuHid = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuHid) ? row[MappingreXlsmdbEntity.Field.StuHid].ToString() : String.Empty;

                            #region 班別資料 ClassListEntity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassId))
                            {
                                receive.ClassId = row[MappingreXlsmdbEntity.Field.ClassId].ToString();

                                classEntity = new ClassListEntity();
                                classEntity.ReceiveType = receiveType;
                                classEntity.YearId = yearId;
                                classEntity.TermId = termId;
                                classEntity.DepId = depId;
                                classEntity.ClassId = receive.ClassId;
                                classEntity.ClassName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassName) ? row[MappingreXlsmdbEntity.Field.ClassName].ToString() : receive.ClassId;

                                classEntity.Status = DataStatusCodeTexts.NORMAL;
                                classEntity.CrtDate = now;
                                classEntity.CrtUser = owner;
                                classEntity.MdyDate = null;
                                classEntity.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 班別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    classEntity.ClassEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassEName) ? row[MappingreXlsmdbEntity.Field.ClassEName].ToString() : classEntity.ClassName;
                                }
                                else
                                {
                                    classEntity.ClassEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassName))
                            {
                                string className = row[MappingreXlsmdbEntity.Field.ClassName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                classEntity = classDatas.Find(x => x.ClassName == className);
                                if (classEntity == null)
                                {
                                    Expression where = new Expression(ClassListEntity.Field.ReceiveType, receiveType)
                                        .And(ClassListEntity.Field.YearId, yearId)
                                        .And(ClassListEntity.Field.TermId, termId)
                                        .And(ClassListEntity.Field.DepId, depId)
                                        .And(ClassListEntity.Field.ClassName, className);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(ClassListEntity.Field.ClassId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<ClassListEntity>(where, orderbys, out classEntity);
                                }

                                if (classEntity == null)
                                {
                                    classEntity = new ClassListEntity();
                                    classEntity.ReceiveType = receiveType;
                                    classEntity.YearId = yearId;
                                    classEntity.TermId = termId;
                                    classEntity.DepId = depId;
                                    classEntity.ClassId = className;
                                    classEntity.ClassName = className;

                                    classEntity.Status = DataStatusCodeTexts.NORMAL;
                                    classEntity.CrtDate = now;
                                    classEntity.CrtUser = owner;
                                    classEntity.MdyDate = null;
                                    classEntity.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 班別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    classEntity.ClassEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassEName) ? row[MappingreXlsmdbEntity.Field.ClassEName].ToString() : classEntity.ClassName;
                                }
                                else
                                {
                                    classEntity.ClassEName = null;
                                }
                                #endregion

                                receive.ClassId = classEntity.ClassId;
                            }
                            else
                            {
                                receive.ClassId = String.Empty;
                            }
                            #endregion

                            #region 土銀專用的部別資料 DeptListEntity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptId))
                            {
                                receive.DeptId = row[MappingreXlsmdbEntity.Field.DeptId].ToString();

                                dept = new DeptListEntity();
                                dept.ReceiveType = receiveType;
                                dept.YearId = yearId;
                                dept.TermId = termId;
                                dept.DeptId = receive.DeptId;
                                dept.DeptName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptName) ? row[MappingreXlsmdbEntity.Field.DeptName].ToString() : receive.DeptId;

                                dept.Status = DataStatusCodeTexts.NORMAL;
                                dept.CrtDate = now;
                                dept.CrtUser = owner;
                                dept.MdyDate = null;
                                dept.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 部別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    dept.DeptEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptEName) ? row[MappingreXlsmdbEntity.Field.DeptEName].ToString() : dept.DeptName;
                                }
                                else
                                {
                                    dept.DeptEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptName))
                            {
                                string deptName = row[MappingreXlsmdbEntity.Field.DeptName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                dept = deptDatas.Find(x => x.DeptName == deptName);
                                if (dept == null)
                                {
                                    Expression where = new Expression(DeptListEntity.Field.ReceiveType, receiveType)
                                        .And(DeptListEntity.Field.YearId, yearId)
                                        .And(DeptListEntity.Field.TermId, termId)
                                        .And(DeptListEntity.Field.DeptName, deptName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(DeptListEntity.Field.DeptId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<DeptListEntity>(where, orderbys, out dept);
                                }

                                if (dept == null)
                                {
                                    dept = new DeptListEntity();
                                    dept.ReceiveType = receiveType;
                                    dept.YearId = yearId;
                                    dept.TermId = termId;
                                    dept.DeptId = deptName;
                                    dept.DeptName = deptName;

                                    dept.Status = DataStatusCodeTexts.NORMAL;
                                    dept.CrtDate = now;
                                    dept.CrtUser = owner;
                                    dept.MdyDate = null;
                                    dept.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 部別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    dept.DeptEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptEName) ? row[MappingreXlsmdbEntity.Field.DeptEName].ToString() : dept.DeptName;
                                }
                                else
                                {
                                    dept.DeptEName = null;
                                }
                                #endregion

                                receive.DeptId = dept.DeptId;
                            }
                            else
                            {
                                receive.DepId = String.Empty;
                            }
                            #endregion

                            #region 院別資料 CollegeListEntity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeId))
                            {
                                receive.CollegeId = row[MappingreXlsmdbEntity.Field.CollegeId].ToString();

                                college = new CollegeListEntity();
                                college.ReceiveType = receiveType;
                                college.YearId = yearId;
                                college.TermId = termId;
                                college.DepId = depId;
                                college.CollegeId = receive.CollegeId;
                                college.CollegeName = fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeName) ? row[MappingreXlsmdbEntity.Field.CollegeName].ToString() : receive.CollegeId;

                                college.Status = DataStatusCodeTexts.NORMAL;
                                college.CrtDate = now;
                                college.CrtUser = owner;
                                college.MdyDate = null;
                                college.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 院別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    college.CollegeEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeEName) ? row[MappingreXlsmdbEntity.Field.CollegeEName].ToString() : college.CollegeName;
                                }
                                else
                                {
                                    college.CollegeEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeName))
                            {
                                string collegeName = row[MappingreXlsmdbEntity.Field.CollegeName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                college = collegeDatas.Find(x => x.CollegeName == collegeName);
                                if (college == null)
                                {
                                    Expression where = new Expression(CollegeListEntity.Field.ReceiveType, receiveType)
                                        .And(CollegeListEntity.Field.YearId, yearId)
                                        .And(CollegeListEntity.Field.TermId, termId)
                                        .And(CollegeListEntity.Field.DepId, depId)
                                        .And(CollegeListEntity.Field.CollegeName, collegeName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(CollegeListEntity.Field.CollegeId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<CollegeListEntity>(where, orderbys, out college);
                                }

                                if (college == null)
                                {
                                    college = new CollegeListEntity();
                                    college.ReceiveType = receiveType;
                                    college.YearId = yearId;
                                    college.TermId = termId;
                                    college.DepId = depId;
                                    college.CollegeId = collegeName;
                                    college.CollegeName = collegeName;

                                    college.Status = DataStatusCodeTexts.NORMAL;
                                    college.CrtDate = now;
                                    college.CrtUser = owner;
                                    college.MdyDate = null;
                                    college.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 院別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    college.CollegeEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeEName) ? row[MappingreXlsmdbEntity.Field.CollegeEName].ToString() : college.CollegeName;
                                }
                                else
                                {
                                    college.CollegeEName = null;
                                }
                                #endregion

                                receive.CollegeId = college.CollegeId;
                            }
                            else
                            {
                                receive.CollegeId = String.Empty;
                            }
                            #endregion

                            #region 系別資料 MajorListEntity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorId))
                            {
                                receive.MajorId = row[MappingreXlsmdbEntity.Field.MajorId].ToString();

                                major = new MajorListEntity();
                                major.ReceiveType = receiveType;
                                major.YearId = yearId;
                                major.TermId = termId;
                                major.DepId = depId;
                                major.MajorId = receive.MajorId;
                                major.MajorName = fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorName) ? row[MappingreXlsmdbEntity.Field.MajorName].ToString() : major.MajorId;

                                major.Status = DataStatusCodeTexts.NORMAL;
                                major.CrtDate = now;
                                major.CrtUser = owner;
                                major.MdyDate = null;
                                major.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 系別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    major.MajorEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorEName) ? row[MappingreXlsmdbEntity.Field.MajorEName].ToString() : major.MajorName;
                                }
                                else
                                {
                                    major.MajorEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorName))
                            {
                                string majorName = row[MappingreXlsmdbEntity.Field.MajorName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                major = majorDatas.Find(x => x.MajorName == majorName);
                                if (major == null)
                                {
                                    Expression where = new Expression(MajorListEntity.Field.ReceiveType, receiveType)
                                        .And(MajorListEntity.Field.YearId, yearId)
                                        .And(MajorListEntity.Field.TermId, termId)
                                        .And(MajorListEntity.Field.DepId, depId)
                                        .And(MajorListEntity.Field.MajorName, majorName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(MajorListEntity.Field.MajorId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<MajorListEntity>(where, orderbys, out major);
                                }

                                if (major == null)
                                {
                                    major = new MajorListEntity();
                                    major.ReceiveType = receiveType;
                                    major.YearId = yearId;
                                    major.TermId = termId;
                                    major.DepId = depId;
                                    major.MajorId = majorName;
                                    major.MajorName = majorName;

                                    major.Status = DataStatusCodeTexts.NORMAL;
                                    major.CrtDate = now;
                                    major.CrtUser = owner;
                                    major.MdyDate = null;
                                    major.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 系別英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    major.MajorEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorEName) ? row[MappingreXlsmdbEntity.Field.MajorEName].ToString() : major.MajorName;
                                }
                                else
                                {
                                    major.MajorEName = null;
                                }
                                #endregion

                                receive.MajorId = major.MajorId;
                            }
                            else
                            {
                                receive.MajorId = String.Empty;
                            }
                            #endregion
                        }
                        #endregion

                        #region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
                        ReduceListEntity reduce = null;
                        LoanListEntity loan = null;
                        DormListEntity dorm = null;
                        {
                            #region 減免
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceId))
                            {
                                receive.ReduceId = row[MappingreXlsmdbEntity.Field.ReduceId].ToString();

                                reduce = new ReduceListEntity();
                                reduce.ReceiveType = receiveType;
                                reduce.YearId = yearId;
                                reduce.TermId = termId;
                                reduce.DepId = depId;
                                reduce.ReduceId = receive.ReduceId;
                                reduce.ReduceName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceName) ? row[MappingreXlsmdbEntity.Field.ReduceName].ToString() : reduce.ReduceId;

                                reduce.Status = DataStatusCodeTexts.NORMAL;
                                reduce.CrtDate = now;
                                reduce.CrtUser = owner;
                                reduce.MdyDate = null;
                                reduce.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 減免英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    reduce.ReduceEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceEName) ? row[MappingreXlsmdbEntity.Field.ReduceEName].ToString() : reduce.ReduceName;
                                }
                                else
                                {
                                    reduce.ReduceEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceName))
                            {
                                string reduceName = row[MappingreXlsmdbEntity.Field.ReduceName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                reduce = reduceDatas.Find(x => x.ReduceName == reduceName);
                                if (reduce == null)
                                {
                                    Expression where = new Expression(ReduceListEntity.Field.ReceiveType, receiveType)
                                       .And(ReduceListEntity.Field.YearId, yearId)
                                       .And(ReduceListEntity.Field.TermId, termId)
                                       .And(ReduceListEntity.Field.DepId, depId)
                                       .And(ReduceListEntity.Field.ReduceName, reduceName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(ReduceListEntity.Field.ReduceId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<ReduceListEntity>(where, orderbys, out reduce);
                                }

                                if (reduce == null)
                                {
                                    reduce = new ReduceListEntity();
                                    reduce.ReceiveType = receiveType;
                                    reduce.YearId = yearId;
                                    reduce.TermId = termId;
                                    reduce.DepId = depId;
                                    reduce.ReduceId = reduceName;
                                    reduce.ReduceName = reduceName;

                                    reduce.Status = DataStatusCodeTexts.NORMAL;
                                    reduce.CrtDate = now;
                                    reduce.CrtUser = owner;
                                    reduce.MdyDate = null;
                                    reduce.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 減免英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    reduce.ReduceEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceEName) ? row[MappingreXlsmdbEntity.Field.ReduceEName].ToString() : reduce.ReduceName;
                                }
                                else
                                {
                                    reduce.ReduceEName = null;
                                }
                                #endregion

                                receive.ReduceId = reduce.ReduceId;
                            }
                            else
                            {
                                receive.ReduceId = String.Empty;
                            }
                            #endregion

                            #region 就貸
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanId))
                            {
                                receive.LoanId = row[MappingreXlsmdbEntity.Field.LoanId].ToString();

                                loan = new LoanListEntity();
                                loan.ReceiveType = receiveType;
                                loan.YearId = yearId;
                                loan.TermId = termId;
                                loan.DepId = depId;
                                loan.LoanId = row[MappingreXlsmdbEntity.Field.LoanId].ToString();
                                loan.LoanName = fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanName) ? row[MappingreXlsmdbEntity.Field.LoanName].ToString() : loan.LoanId;

                                loan.Status = DataStatusCodeTexts.NORMAL;
                                loan.CrtDate = now;
                                loan.CrtUser = owner;
                                loan.MdyDate = null;
                                loan.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    loan.LoanEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanEName) ? row[MappingreXlsmdbEntity.Field.LoanEName].ToString() : loan.LoanName;
                                }
                                else
                                {
                                    loan.LoanEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanName))
                            {
                                string loanName = row[MappingreXlsmdbEntity.Field.LoanName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                loan = loanDatas.Find(x => x.LoanName == loanName);
                                if (loan == null)
                                {
                                    Expression where = new Expression(LoanListEntity.Field.ReceiveType, receiveType)
                                        .And(LoanListEntity.Field.YearId, yearId)
                                        .And(LoanListEntity.Field.TermId, termId)
                                        .And(LoanListEntity.Field.DepId, depId)
                                        .And(LoanListEntity.Field.LoanName, loanName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(LoanListEntity.Field.LoanId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<LoanListEntity>(where, orderbys, out loan);
                                }

                                if (loan == null)
                                {
                                    loan = new LoanListEntity();
                                    loan.ReceiveType = receiveType;
                                    loan.YearId = yearId;
                                    loan.TermId = termId;
                                    loan.DepId = depId;
                                    loan.LoanId = loanName;
                                    loan.LoanName = loanName;

                                    loan.Status = DataStatusCodeTexts.NORMAL;
                                    loan.CrtDate = now;
                                    loan.CrtUser = owner;
                                    loan.MdyDate = null;
                                    loan.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    loan.LoanEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanEName) ? row[MappingreXlsmdbEntity.Field.LoanEName].ToString() : loan.LoanName;
                                }
                                else
                                {
                                    loan.LoanEName = null;
                                }
                                #endregion

                                receive.LoanId = loan.LoanId;
                            }
                            else
                            {
                                receive.LoanId = String.Empty;
                            }
                            #endregion

                            #region 住宿
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DormId))
                            {
                                receive.DormId = row[MappingreXlsmdbEntity.Field.DormId].ToString();

                                dorm = new DormListEntity();
                                dorm.ReceiveType = receiveType;
                                dorm.YearId = yearId;
                                dorm.TermId = termId;
                                dorm.DepId = depId;
                                dorm.DormId = receive.DormId;
                                dorm.DormName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DormName) ? row[MappingreXlsmdbEntity.Field.DormName].ToString() : receive.DormId;

                                dorm.Status = DataStatusCodeTexts.NORMAL;
                                dorm.CrtDate = now;
                                dorm.CrtUser = owner;
                                dorm.MdyDate = null;
                                dorm.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    dorm.DormEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DormEName) ? row[MappingreXlsmdbEntity.Field.DormEName].ToString() : dorm.DormName;
                                }
                                else
                                {
                                    dorm.DormEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DormName))
                            {
                                string dormName = row[MappingreXlsmdbEntity.Field.DormName].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                dorm = dormDatas.Find(x => x.DormName == dormName);
                                if (dorm == null)
                                {
                                    Expression where = new Expression(DormListEntity.Field.ReceiveType, receiveType)
                                        .And(DormListEntity.Field.YearId, yearId)
                                        .And(DormListEntity.Field.TermId, termId)
                                        .And(DormListEntity.Field.DepId, depId)
                                        .And(DormListEntity.Field.DormName, dormName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(DormListEntity.Field.DormId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<DormListEntity>(where, orderbys, out dorm);
                                }

                                if (dorm == null)
                                {
                                    dorm = new DormListEntity();
                                    dorm.ReceiveType = receiveType;
                                    dorm.YearId = yearId;
                                    dorm.TermId = termId;
                                    dorm.DepId = depId;
                                    dorm.DormId = dormName;
                                    dorm.DormName = dormName;

                                    dorm.Status = DataStatusCodeTexts.NORMAL;
                                    dorm.CrtDate = now;
                                    dorm.CrtUser = owner;
                                    dorm.MdyDate = null;
                                    dorm.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 就貸英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    dorm.DormEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DormEName) ? row[MappingreXlsmdbEntity.Field.DormEName].ToString() : dorm.DormName;
                                }
                                else
                                {
                                    dorm.DormEName = null;
                                }
                                #endregion

                                receive.DormId = dorm.DormId;
                            }
                            else
                            {
                                receive.DormId = String.Empty;
                            }
                            #endregion
                        }
                        #endregion

                        #region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
                        IdentifyList1Entity identify1 = null;
                        IdentifyList2Entity identify2 = null;
                        IdentifyList3Entity identify3 = null;
                        IdentifyList4Entity identify4 = null;
                        IdentifyList5Entity identify5 = null;
                        IdentifyList6Entity identify6 = null;
                        {
                            #region [Old]
                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId1))
                            //{
                            //    receive.IdentifyId01 = row[MappingreXlsmdbEntity.Field.IdentifyId1].ToString();

                            //    identify1 = new IdentifyList1Entity();
                            //    identify1.ReceiveType = receiveType;
                            //    identify1.YearId = yearId;
                            //    identify1.TermId = termId;
                            //    identify1.DepId = depId;
                            //    identify1.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId1].ToString();
                            //    identify1.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName1) ? row[MappingreXlsmdbEntity.Field.IdentifyName1].ToString() : String.Empty;

                            //    identify1.Status = DataStatusCodeTexts.NORMAL;
                            //    identify1.CrtDate = now;
                            //    identify1.CrtUser = owner;
                            //    identify1.MdyDate = null;
                            //    identify1.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.IdentifyId01 = String.Empty;
                            //}

                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId2))
                            //{
                            //    receive.IdentifyId02 = row[MappingreXlsmdbEntity.Field.IdentifyId2].ToString();

                            //    identify2 = new IdentifyList2Entity();
                            //    identify2.ReceiveType = receiveType;
                            //    identify2.YearId = yearId;
                            //    identify2.TermId = termId;
                            //    identify2.DepId = depId;
                            //    identify2.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId2].ToString();
                            //    identify2.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName2) ? row[MappingreXlsmdbEntity.Field.IdentifyName2].ToString() : String.Empty;

                            //    identify2.Status = DataStatusCodeTexts.NORMAL;
                            //    identify2.CrtDate = now;
                            //    identify2.CrtUser = owner;
                            //    identify2.MdyDate = null;
                            //    identify2.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.IdentifyId02 = String.Empty;
                            //}

                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId3))
                            //{
                            //    receive.IdentifyId03 = row[MappingreXlsmdbEntity.Field.IdentifyId3].ToString();

                            //    identify3 = new IdentifyList3Entity();
                            //    identify3.ReceiveType = receiveType;
                            //    identify3.YearId = yearId;
                            //    identify3.TermId = termId;
                            //    identify3.DepId = depId;
                            //    identify3.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId3].ToString();
                            //    identify3.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName3) ? row[MappingreXlsmdbEntity.Field.IdentifyName3].ToString() : String.Empty;

                            //    identify3.Status = DataStatusCodeTexts.NORMAL;
                            //    identify3.CrtDate = now;
                            //    identify3.CrtUser = owner;
                            //    identify3.MdyDate = null;
                            //    identify3.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.IdentifyId03 = String.Empty;
                            //}

                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId4))
                            //{
                            //    receive.IdentifyId04 = row[MappingreXlsmdbEntity.Field.IdentifyId4].ToString();

                            //    identify4 = new IdentifyList4Entity();
                            //    identify4.ReceiveType = receiveType;
                            //    identify4.YearId = yearId;
                            //    identify4.TermId = termId;
                            //    identify4.DepId = depId;
                            //    identify4.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId4].ToString();
                            //    identify4.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName4) ? row[MappingreXlsmdbEntity.Field.IdentifyName4].ToString() : String.Empty;

                            //    identify4.Status = DataStatusCodeTexts.NORMAL;
                            //    identify4.CrtDate = now;
                            //    identify4.CrtUser = owner;
                            //    identify4.MdyDate = null;
                            //    identify4.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.IdentifyId04 = String.Empty;
                            //}

                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId5))
                            //{
                            //    receive.IdentifyId05 = row[MappingreXlsmdbEntity.Field.IdentifyId5].ToString();

                            //    identify5 = new IdentifyList5Entity();
                            //    identify5.ReceiveType = receiveType;
                            //    identify5.YearId = yearId;
                            //    identify5.TermId = termId;
                            //    identify5.DepId = depId;
                            //    identify5.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId5].ToString();
                            //    identify5.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName5) ? row[MappingreXlsmdbEntity.Field.IdentifyName5].ToString() : String.Empty;

                            //    identify5.Status = DataStatusCodeTexts.NORMAL;
                            //    identify5.CrtDate = now;
                            //    identify5.CrtUser = owner;
                            //    identify5.MdyDate = null;
                            //    identify5.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.IdentifyId05 = String.Empty;
                            //}

                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId6))
                            //{
                            //    receive.IdentifyId06 = row[MappingreXlsmdbEntity.Field.IdentifyId6].ToString();

                            //    identify6 = new IdentifyList6Entity();
                            //    identify6.ReceiveType = receiveType;
                            //    identify6.YearId = yearId;
                            //    identify6.TermId = termId;
                            //    identify6.DepId = depId;
                            //    identify6.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId6].ToString();
                            //    identify6.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName6) ? row[MappingreXlsmdbEntity.Field.IdentifyName6].ToString() : String.Empty;

                            //    identify6.Status = DataStatusCodeTexts.NORMAL;
                            //    identify6.CrtDate = now;
                            //    identify6.CrtUser = owner;
                            //    identify6.MdyDate = null;
                            //    identify6.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.IdentifyId06 = String.Empty;
                            //}
                            #endregion

                            #region IdentifyList1Entity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId1))
                            {
                                receive.IdentifyId01 = row[MappingreXlsmdbEntity.Field.IdentifyId1].ToString();

                                identify1 = new IdentifyList1Entity();
                                identify1.ReceiveType = receiveType;
                                identify1.YearId = yearId;
                                identify1.TermId = termId;
                                identify1.DepId = depId;
                                identify1.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId1].ToString();
                                identify1.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName1) ? row[MappingreXlsmdbEntity.Field.IdentifyName1].ToString() : String.Empty;

                                identify1.Status = DataStatusCodeTexts.NORMAL;
                                identify1.CrtDate = now;
                                identify1.CrtUser = owner;
                                identify1.MdyDate = null;
                                identify1.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 身分註記01英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify1.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName1) ? row[MappingreXlsmdbEntity.Field.IdentifyEName1].ToString() : identify1.IdentifyName;
                                }
                                else
                                {
                                    identify1.IdentifyEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName1))
                            {
                                string identifyName = row[MappingreXlsmdbEntity.Field.IdentifyName1].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                identify1 = identify1Datas.Find(x => x.IdentifyName == identifyName);
                                if (identify1 == null)
                                {
                                    Expression where = new Expression(IdentifyList1Entity.Field.ReceiveType, receiveType)
                                        .And(IdentifyList1Entity.Field.YearId, yearId)
                                        .And(IdentifyList1Entity.Field.TermId, termId)
                                        .And(IdentifyList1Entity.Field.DepId, depId)
                                        .And(IdentifyList1Entity.Field.IdentifyName, identifyName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(IdentifyList1Entity.Field.IdentifyId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<IdentifyList1Entity>(where, orderbys, out identify1);
                                }

                                if (identify1 == null)
                                {
                                    identify1 = new IdentifyList1Entity();
                                    identify1.ReceiveType = receiveType;
                                    identify1.YearId = yearId;
                                    identify1.TermId = termId;
                                    identify1.DepId = depId;
                                    identify1.IdentifyId = identifyName;
                                    identify1.IdentifyName = identifyName;

                                    identify1.Status = DataStatusCodeTexts.NORMAL;
                                    identify1.CrtDate = now;
                                    identify1.CrtUser = owner;
                                    identify1.MdyDate = null;
                                    identify1.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 身分註記01英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify1.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName1) ? row[MappingreXlsmdbEntity.Field.IdentifyEName1].ToString() : identify1.IdentifyName;
                                }
                                else
                                {
                                    identify1.IdentifyEName = null;
                                }
                                #endregion

                                receive.IdentifyId01 = identify1.IdentifyId;
                            }
                            else
                            {
                                receive.IdentifyId01 = String.Empty;
                            }
                            #endregion

                            #region IdentifyList2Entity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId2))
                            {
                                receive.IdentifyId02 = row[MappingreXlsmdbEntity.Field.IdentifyId2].ToString();

                                identify2 = new IdentifyList2Entity();
                                identify2.ReceiveType = receiveType;
                                identify2.YearId = yearId;
                                identify2.TermId = termId;
                                identify2.DepId = depId;
                                identify2.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId2].ToString();
                                identify2.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName2) ? row[MappingreXlsmdbEntity.Field.IdentifyName2].ToString() : String.Empty;

                                identify2.Status = DataStatusCodeTexts.NORMAL;
                                identify2.CrtDate = now;
                                identify2.CrtUser = owner;
                                identify2.MdyDate = null;
                                identify2.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 身分註記02英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify2.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName2) ? row[MappingreXlsmdbEntity.Field.IdentifyEName2].ToString() : identify2.IdentifyName;
                                }
                                else
                                {
                                    identify2.IdentifyEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName2))
                            {
                                string identifyName = row[MappingreXlsmdbEntity.Field.IdentifyName2].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                identify2 = identify2Datas.Find(x => x.IdentifyName == identifyName);
                                if (identify2 == null)
                                {
                                    Expression where = new Expression(IdentifyList2Entity.Field.ReceiveType, receiveType)
                                        .And(IdentifyList2Entity.Field.YearId, yearId)
                                        .And(IdentifyList2Entity.Field.TermId, termId)
                                        .And(IdentifyList2Entity.Field.DepId, depId)
                                        .And(IdentifyList2Entity.Field.IdentifyName, identifyName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(IdentifyList2Entity.Field.IdentifyId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<IdentifyList2Entity>(where, orderbys, out identify2);
                                }

                                if (identify2 == null)
                                {
                                    identify2 = new IdentifyList2Entity();
                                    identify2.ReceiveType = receiveType;
                                    identify2.YearId = yearId;
                                    identify2.TermId = termId;
                                    identify2.DepId = depId;
                                    identify2.IdentifyId = identifyName;
                                    identify2.IdentifyName = identifyName;

                                    identify2.Status = DataStatusCodeTexts.NORMAL;
                                    identify2.CrtDate = now;
                                    identify2.CrtUser = owner;
                                    identify2.MdyDate = null;
                                    identify2.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 身分註記02英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify2.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName2) ? row[MappingreXlsmdbEntity.Field.IdentifyEName2].ToString() : identify2.IdentifyName;
                                }
                                else
                                {
                                    identify2.IdentifyEName = null;
                                }
                                #endregion

                                receive.IdentifyId02 = identify2.IdentifyId;
                            }
                            else
                            {
                                receive.IdentifyId02 = String.Empty;
                            }
                            #endregion

                            #region IdentifyList3Entity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId3))
                            {
                                receive.IdentifyId03 = row[MappingreXlsmdbEntity.Field.IdentifyId3].ToString();

                                identify3 = new IdentifyList3Entity();
                                identify3.ReceiveType = receiveType;
                                identify3.YearId = yearId;
                                identify3.TermId = termId;
                                identify3.DepId = depId;
                                identify3.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId3].ToString();
                                identify3.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName3) ? row[MappingreXlsmdbEntity.Field.IdentifyName3].ToString() : String.Empty;

                                identify3.Status = DataStatusCodeTexts.NORMAL;
                                identify3.CrtDate = now;
                                identify3.CrtUser = owner;
                                identify3.MdyDate = null;
                                identify3.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 身分註記03英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify3.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName3) ? row[MappingreXlsmdbEntity.Field.IdentifyEName3].ToString() : identify3.IdentifyName;
                                }
                                else
                                {
                                    identify3.IdentifyEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName3))
                            {
                                string identifyName = row[MappingreXlsmdbEntity.Field.IdentifyName3].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                identify3 = identify3Datas.Find(x => x.IdentifyName == identifyName);
                                if (identify3 == null)
                                {
                                    Expression where = new Expression(IdentifyList3Entity.Field.ReceiveType, receiveType)
                                        .And(IdentifyList3Entity.Field.YearId, yearId)
                                        .And(IdentifyList3Entity.Field.TermId, termId)
                                        .And(IdentifyList3Entity.Field.DepId, depId)
                                        .And(IdentifyList3Entity.Field.IdentifyName, identifyName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(IdentifyList3Entity.Field.IdentifyId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<IdentifyList3Entity>(where, orderbys, out identify3);
                                }

                                if (identify3 == null)
                                {
                                    identify3 = new IdentifyList3Entity();
                                    identify3.ReceiveType = receiveType;
                                    identify3.YearId = yearId;
                                    identify3.TermId = termId;
                                    identify3.DepId = depId;
                                    identify3.IdentifyId = identifyName;
                                    identify3.IdentifyName = identifyName;

                                    identify3.Status = DataStatusCodeTexts.NORMAL;
                                    identify3.CrtDate = now;
                                    identify3.CrtUser = owner;
                                    identify3.MdyDate = null;
                                    identify3.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 身分註記03英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify3.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName3) ? row[MappingreXlsmdbEntity.Field.IdentifyEName3].ToString() : identify3.IdentifyName;
                                }
                                else
                                {
                                    identify3.IdentifyEName = null;
                                }
                                #endregion

                                receive.IdentifyId03 = identify3.IdentifyId;
                            }
                            else
                            {
                                receive.IdentifyId03 = String.Empty;
                            }
                            #endregion

                            #region IdentifyList4Entity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId4))
                            {
                                receive.IdentifyId04 = row[MappingreXlsmdbEntity.Field.IdentifyId4].ToString();

                                identify4 = new IdentifyList4Entity();
                                identify4.ReceiveType = receiveType;
                                identify4.YearId = yearId;
                                identify4.TermId = termId;
                                identify4.DepId = depId;
                                identify4.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId4].ToString();
                                identify4.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName4) ? row[MappingreXlsmdbEntity.Field.IdentifyName4].ToString() : String.Empty;

                                identify4.Status = DataStatusCodeTexts.NORMAL;
                                identify4.CrtDate = now;
                                identify4.CrtUser = owner;
                                identify4.MdyDate = null;
                                identify4.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 身分註記04英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify4.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName4) ? row[MappingreXlsmdbEntity.Field.IdentifyEName4].ToString() : identify4.IdentifyName;
                                }
                                else
                                {
                                    identify4.IdentifyEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName4))
                            {
                                string identifyName = row[MappingreXlsmdbEntity.Field.IdentifyName4].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                identify4 = identify4Datas.Find(x => x.IdentifyName == identifyName);
                                if (identify4 == null)
                                {
                                    Expression where = new Expression(IdentifyList4Entity.Field.ReceiveType, receiveType)
                                        .And(IdentifyList4Entity.Field.YearId, yearId)
                                        .And(IdentifyList4Entity.Field.TermId, termId)
                                        .And(IdentifyList4Entity.Field.DepId, depId)
                                        .And(IdentifyList4Entity.Field.IdentifyName, identifyName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(IdentifyList4Entity.Field.IdentifyId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<IdentifyList4Entity>(where, orderbys, out identify4);
                                }

                                if (identify4 == null)
                                {
                                    identify4 = new IdentifyList4Entity();
                                    identify4.ReceiveType = receiveType;
                                    identify4.YearId = yearId;
                                    identify4.TermId = termId;
                                    identify4.DepId = depId;
                                    identify4.IdentifyId = identifyName;
                                    identify4.IdentifyName = identifyName;

                                    identify4.Status = DataStatusCodeTexts.NORMAL;
                                    identify4.CrtDate = now;
                                    identify4.CrtUser = owner;
                                    identify4.MdyDate = null;
                                    identify4.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 身分註記04英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify4.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName4) ? row[MappingreXlsmdbEntity.Field.IdentifyEName4].ToString() : identify4.IdentifyName;
                                }
                                else
                                {
                                    identify4.IdentifyEName = null;
                                }
                                #endregion

                                receive.IdentifyId04 = identify4.IdentifyId;
                            }
                            else
                            {
                                receive.IdentifyId04 = String.Empty;
                            }
                            #endregion

                            #region IdentifyList5Entity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId5))
                            {
                                receive.IdentifyId05 = row[MappingreXlsmdbEntity.Field.IdentifyId5].ToString();

                                identify5 = new IdentifyList5Entity();
                                identify5.ReceiveType = receiveType;
                                identify5.YearId = yearId;
                                identify5.TermId = termId;
                                identify5.DepId = depId;
                                identify5.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId5].ToString();
                                identify5.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName5) ? row[MappingreXlsmdbEntity.Field.IdentifyName5].ToString() : String.Empty;

                                identify5.Status = DataStatusCodeTexts.NORMAL;
                                identify5.CrtDate = now;
                                identify5.CrtUser = owner;
                                identify5.MdyDate = null;
                                identify5.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 身分註記05英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify5.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName5) ? row[MappingreXlsmdbEntity.Field.IdentifyEName5].ToString() : identify5.IdentifyName;
                                }
                                else
                                {
                                    identify5.IdentifyEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName5))
                            {
                                string identifyName = row[MappingreXlsmdbEntity.Field.IdentifyName5].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                identify5 = identify5Datas.Find(x => x.IdentifyName == identifyName);
                                if (identify5 == null)
                                {
                                    Expression where = new Expression(IdentifyList5Entity.Field.ReceiveType, receiveType)
                                        .And(IdentifyList5Entity.Field.YearId, yearId)
                                        .And(IdentifyList5Entity.Field.TermId, termId)
                                        .And(IdentifyList5Entity.Field.DepId, depId)
                                        .And(IdentifyList5Entity.Field.IdentifyName, identifyName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(IdentifyList5Entity.Field.IdentifyId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<IdentifyList5Entity>(where, orderbys, out identify5);
                                }

                                if (identify5 == null)
                                {
                                    identify5 = new IdentifyList5Entity();
                                    identify5.ReceiveType = receiveType;
                                    identify5.YearId = yearId;
                                    identify5.TermId = termId;
                                    identify5.DepId = depId;
                                    identify5.IdentifyId = identifyName;
                                    identify5.IdentifyName = identifyName;

                                    identify5.Status = DataStatusCodeTexts.NORMAL;
                                    identify5.CrtDate = now;
                                    identify5.CrtUser = owner;
                                    identify5.MdyDate = null;
                                    identify5.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 身分註記05英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify5.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName5) ? row[MappingreXlsmdbEntity.Field.IdentifyEName5].ToString() : identify5.IdentifyName;
                                }
                                else
                                {
                                    identify5.IdentifyEName = null;
                                }
                                #endregion

                                receive.IdentifyId05 = identify5.IdentifyId;
                            }
                            else
                            {
                                receive.IdentifyId05 = String.Empty;
                            }
                            #endregion

                            #region IdentifyList6Entity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId6))
                            {
                                receive.IdentifyId06 = row[MappingreXlsmdbEntity.Field.IdentifyId6].ToString();

                                identify6 = new IdentifyList6Entity();
                                identify6.ReceiveType = receiveType;
                                identify6.YearId = yearId;
                                identify6.TermId = termId;
                                identify6.DepId = depId;
                                identify6.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId6].ToString();
                                identify6.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName6) ? row[MappingreXlsmdbEntity.Field.IdentifyName6].ToString() : String.Empty;

                                identify6.Status = DataStatusCodeTexts.NORMAL;
                                identify6.CrtDate = now;
                                identify6.CrtUser = owner;
                                identify6.MdyDate = null;
                                identify6.MdyUser = null;

                                #region [MDY:202203XX] 2022擴充案 身分註記06英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify6.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName6) ? row[MappingreXlsmdbEntity.Field.IdentifyEName6].ToString() : identify6.IdentifyName;
                                }
                                else
                                {
                                    identify6.IdentifyEName = null;
                                }
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName6))
                            {
                                string identifyName = row[MappingreXlsmdbEntity.Field.IdentifyName6].ToString();

                                #region 只有名稱就找出代碼，找不到代碼就把名稱當代碼
                                identify6 = identify6Datas.Find(x => x.IdentifyName == identifyName);
                                if (identify6 == null)
                                {
                                    Expression where = new Expression(IdentifyList6Entity.Field.ReceiveType, receiveType)
                                        .And(IdentifyList6Entity.Field.YearId, yearId)
                                        .And(IdentifyList6Entity.Field.TermId, termId)
                                        .And(IdentifyList6Entity.Field.DepId, depId)
                                        .And(IdentifyList6Entity.Field.IdentifyName, identifyName);
                                    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                                    orderbys.Add(IdentifyList6Entity.Field.IdentifyId, OrderByEnum.Asc);
                                    Result result = tsFactory.SelectFirst<IdentifyList6Entity>(where, orderbys, out identify6);
                                }

                                if (identify6 == null)
                                {
                                    identify6 = new IdentifyList6Entity();
                                    identify6.ReceiveType = receiveType;
                                    identify6.YearId = yearId;
                                    identify6.TermId = termId;
                                    identify6.DepId = depId;
                                    identify6.IdentifyId = identifyName;
                                    identify6.IdentifyName = identifyName;

                                    identify6.Status = DataStatusCodeTexts.NORMAL;
                                    identify6.CrtDate = now;
                                    identify6.CrtUser = owner;
                                    identify6.MdyDate = null;
                                    identify6.MdyUser = null;
                                }
                                #endregion

                                #region [MDY:202203XX] 2022擴充案 身分註記06英文名稱
                                if (isEngEnabled)
                                {
                                    //沒有英文名稱則以中文名稱取代英文名稱
                                    identify6.IdentifyEName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyEName6) ? row[MappingreXlsmdbEntity.Field.IdentifyEName6].ToString() : identify6.IdentifyName;
                                }
                                else
                                {
                                    identify6.IdentifyEName = null;
                                }
                                #endregion

                                receive.IdentifyId06 = identify6.IdentifyId;
                            }
                            else
                            {
                                receive.IdentifyId06 = String.Empty;
                            }
                            #endregion
                        }
                        #endregion

                        #region 收入科目金額對照欄位 (StudentReceiveEntity)
                        {
                            string[] fields = new string[] {
                                MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3, MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5,
                                MappingreXlsmdbEntity.Field.Receive6, MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9, MappingreXlsmdbEntity.Field.Receive10,
                                MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12, MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                                MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18, MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20,
                                MappingreXlsmdbEntity.Field.Receive21, MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24, MappingreXlsmdbEntity.Field.Receive25,
                                MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27, MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                                MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33, MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35,
                                MappingreXlsmdbEntity.Field.Receive36, MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39, MappingreXlsmdbEntity.Field.Receive40
                            };
                            System.Decimal?[] values = new System.Decimal?[] {
                                null, null, null, null, null, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null
                            };
                            bool isFail = false;
                            for (int idx = 0; idx < fields.Length; idx++)
                            {
                                string fieldName = fields[idx];
                                if (fieldNames.Contains(fieldName))
                                {
                                    System.Decimal value;
                                    if (System.Decimal.TryParse(row[fieldName].ToString(), out value))
                                    {
                                        values[idx] = value;
                                    }
                                    else
                                    {
                                        row[ConvertFileHelper.DataLineFailureFieldName] = String.Format("收入科目金額{0} 不是有效的金額", idx + 1);
                                        isFail = true;
                                        break;
                                    }
                                }
                            }
                            if (isFail)
                            {
                                string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            receive.Receive01 = values[0];
                            receive.Receive02 = values[1];
                            receive.Receive03 = values[2];
                            receive.Receive04 = values[3];
                            receive.Receive05 = values[4];
                            receive.Receive06 = values[5];
                            receive.Receive07 = values[6];
                            receive.Receive08 = values[7];
                            receive.Receive09 = values[8];
                            receive.Receive10 = values[9];

                            receive.Receive11 = values[10];
                            receive.Receive12 = values[11];
                            receive.Receive13 = values[12];
                            receive.Receive14 = values[13];
                            receive.Receive15 = values[14];
                            receive.Receive16 = values[15];
                            receive.Receive17 = values[16];
                            receive.Receive18 = values[17];
                            receive.Receive19 = values[18];
                            receive.Receive20 = values[19];

                            receive.Receive21 = values[20];
                            receive.Receive22 = values[21];
                            receive.Receive23 = values[22];
                            receive.Receive24 = values[23];
                            receive.Receive25 = values[24];
                            receive.Receive26 = values[25];
                            receive.Receive27 = values[26];
                            receive.Receive28 = values[27];
                            receive.Receive29 = values[28];
                            receive.Receive30 = values[29];

                            receive.Receive31 = values[30];
                            receive.Receive32 = values[31];
                            receive.Receive33 = values[32];
                            receive.Receive34 = values[33];
                            receive.Receive35 = values[34];
                            receive.Receive36 = values[35];
                            receive.Receive37 = values[36];
                            receive.Receive38 = values[37];
                            receive.Receive39 = values[38];
                            receive.Receive40 = values[39];
                        }
                        #endregion

                        #region 其他對照欄位 (StudentReceiveEntity)
                        {
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.StuCredit))
                            {
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.StuCredit].ToString(), out value))
                                {
                                    receive.StuCredit = value;
                                }
                                else
                                {
                                    string failMsg = "總學分數不是有效的數值";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.StuCredit = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.StuHour))
                            {
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.StuHour].ToString(), out value))
                                {
                                    receive.StuHour = value;
                                }
                                else
                                {
                                    string failMsg = "上課時數不是有效的數值";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.StuHour = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanAmount))
                            {
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.LoanAmount].ToString(), out value))
                                {
                                    receive.LoanAmount = value;
                                }
                                else
                                {
                                    string failMsg = "就貸可貸金額不是有效的金額";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.LoanAmount = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ReceiveAmount))
                            {
                                uploadFlag |= StudentReceiveEntity.UploadAmountFlag;
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.ReceiveAmount].ToString(), out value))
                                {
                                    receive.ReceiveAmount = value;
                                }
                                else
                                {
                                    string failMsg = "繳費金額合計不是有效的金額";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.ReceiveAmount = null;
                            }
                        }
                        #endregion

                        #region 學分基準、課程、學分數對照欄位 (StudentCourseEntity)
                        StudentCourseEntity course = null;
                        {
                            string[] creditIdFileds = new string[] {
                                MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2, MappingreXlsmdbEntity.Field.CreditId3, MappingreXlsmdbEntity.Field.CreditId4, MappingreXlsmdbEntity.Field.CreditId5,
                                MappingreXlsmdbEntity.Field.CreditId6, MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8, MappingreXlsmdbEntity.Field.CreditId9, MappingreXlsmdbEntity.Field.CreditId10,
                                MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12, MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14, MappingreXlsmdbEntity.Field.CreditId15,
                                MappingreXlsmdbEntity.Field.CreditId16, MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18, MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20,
                                MappingreXlsmdbEntity.Field.CreditId21, MappingreXlsmdbEntity.Field.CreditId22, MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24, MappingreXlsmdbEntity.Field.CreditId25,
                                MappingreXlsmdbEntity.Field.CreditId26, MappingreXlsmdbEntity.Field.CreditId27, MappingreXlsmdbEntity.Field.CreditId28, MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
                                MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32, MappingreXlsmdbEntity.Field.CreditId33, MappingreXlsmdbEntity.Field.CreditId34, MappingreXlsmdbEntity.Field.CreditId35,
                                MappingreXlsmdbEntity.Field.CreditId36, MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38, MappingreXlsmdbEntity.Field.CreditId39, MappingreXlsmdbEntity.Field.CreditId40
                            };
                            string[] courseIdFileds = new string[] {
                                MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2, MappingreXlsmdbEntity.Field.CourseId3, MappingreXlsmdbEntity.Field.CourseId4, MappingreXlsmdbEntity.Field.CourseId5,
                                MappingreXlsmdbEntity.Field.CourseId6, MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8, MappingreXlsmdbEntity.Field.CourseId9, MappingreXlsmdbEntity.Field.CourseId10,
                                MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12, MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14, MappingreXlsmdbEntity.Field.CourseId15,
                                MappingreXlsmdbEntity.Field.CourseId16, MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18, MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20,
                                MappingreXlsmdbEntity.Field.CourseId21, MappingreXlsmdbEntity.Field.CourseId22, MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24, MappingreXlsmdbEntity.Field.CourseId25,
                                MappingreXlsmdbEntity.Field.CourseId26, MappingreXlsmdbEntity.Field.CourseId27, MappingreXlsmdbEntity.Field.CourseId28, MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
                                MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32, MappingreXlsmdbEntity.Field.CourseId33, MappingreXlsmdbEntity.Field.CourseId34, MappingreXlsmdbEntity.Field.CourseId35,
                                MappingreXlsmdbEntity.Field.CourseId36, MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38, MappingreXlsmdbEntity.Field.CourseId39, MappingreXlsmdbEntity.Field.CourseId40
                            };
                            string[] creditFileds = new string[] {
                                MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2, MappingreXlsmdbEntity.Field.Credit3, MappingreXlsmdbEntity.Field.Credit4, MappingreXlsmdbEntity.Field.Credit5,
                                MappingreXlsmdbEntity.Field.Credit6, MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8, MappingreXlsmdbEntity.Field.Credit9, MappingreXlsmdbEntity.Field.Credit10,
                                MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12, MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14, MappingreXlsmdbEntity.Field.Credit15,
                                MappingreXlsmdbEntity.Field.Credit16, MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18, MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20,
                                MappingreXlsmdbEntity.Field.Credit21, MappingreXlsmdbEntity.Field.Credit22, MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24, MappingreXlsmdbEntity.Field.Credit25,
                                MappingreXlsmdbEntity.Field.Credit26, MappingreXlsmdbEntity.Field.Credit27, MappingreXlsmdbEntity.Field.Credit28, MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
                                MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32, MappingreXlsmdbEntity.Field.Credit33, MappingreXlsmdbEntity.Field.Credit34, MappingreXlsmdbEntity.Field.Credit35,
                                MappingreXlsmdbEntity.Field.Credit36, MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38, MappingreXlsmdbEntity.Field.Credit39, MappingreXlsmdbEntity.Field.Credit40
                            };
                            string[] creditIdValues = new string[40];
                            string[] courseIdValues = new string[40];
                            System.Decimal?[] creditValues = new System.Decimal?[40];
                            bool isFail = false;
                            for (int idx = 0; idx < creditIdFileds.Length; idx++)
                            {
                                string creditIdFiled = creditIdFileds[idx];
                                if (fieldNames.Contains(creditIdFiled))
                                {
                                    creditIdValues[idx] = row[creditIdFiled].ToString();
                                }
                                else
                                {
                                    creditIdValues[idx] = null;
                                }

                                string courseIdFiled = courseIdFileds[idx];
                                if (fieldNames.Contains(courseIdFiled))
                                {
                                    courseIdValues[idx] = row[courseIdFiled].ToString();
                                }
                                else
                                {
                                    courseIdValues[idx] = null;
                                }

                                string creditFiled = creditFileds[idx];
                                if (fieldNames.Contains(creditFiled))
                                {
                                    System.Decimal value;
                                    if (System.Decimal.TryParse(row[creditFiled].ToString(), out value))
                                    {
                                        creditValues[idx] = value;
                                    }
                                    else
                                    {
                                        row[ConvertFileHelper.DataLineFailureFieldName] = String.Format("學分數{0} 不是有效的數值", idx + 1);
                                        isFail = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    creditValues[idx] = null;
                                }
                            }

                            if (isFail)
                            {
                                string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            course = new StudentCourseEntity();
                            course.ReceiveType = receive.ReceiveType;
                            course.YearId = receive.YearId;
                            course.TermId = receive.TermId;
                            course.DepId = receive.DepId;
                            course.ReceiveId = receive.ReceiveId;
                            course.StuId = receive.StuId;
                            course.OldSeq = receive.OldSeq;

                            course.CreditId1 = creditIdValues[0];
                            course.CreditId2 = creditIdValues[1];
                            course.CreditId3 = creditIdValues[2];
                            course.CreditId4 = creditIdValues[3];
                            course.CreditId5 = creditIdValues[4];
                            course.CreditId6 = creditIdValues[5];
                            course.CreditId7 = creditIdValues[6];
                            course.CreditId8 = creditIdValues[7];
                            course.CreditId9 = creditIdValues[8];
                            course.CreditId10 = creditIdValues[9];

                            course.CreditId11 = creditIdValues[10];
                            course.CreditId12 = creditIdValues[11];
                            course.CreditId13 = creditIdValues[12];
                            course.CreditId14 = creditIdValues[13];
                            course.CreditId15 = creditIdValues[14];
                            course.CreditId16 = creditIdValues[15];
                            course.CreditId17 = creditIdValues[16];
                            course.CreditId18 = creditIdValues[17];
                            course.CreditId19 = creditIdValues[18];
                            course.CreditId20 = creditIdValues[19];

                            course.CreditId21 = creditIdValues[20];
                            course.CreditId22 = creditIdValues[21];
                            course.CreditId23 = creditIdValues[22];
                            course.CreditId24 = creditIdValues[23];
                            course.CreditId25 = creditIdValues[24];
                            course.CreditId26 = creditIdValues[25];
                            course.CreditId27 = creditIdValues[26];
                            course.CreditId28 = creditIdValues[27];
                            course.CreditId29 = creditIdValues[28];
                            course.CreditId30 = creditIdValues[29];

                            course.CreditId31 = creditIdValues[30];
                            course.CreditId32 = creditIdValues[31];
                            course.CreditId33 = creditIdValues[32];
                            course.CreditId34 = creditIdValues[33];
                            course.CreditId35 = creditIdValues[34];
                            course.CreditId36 = creditIdValues[35];
                            course.CreditId37 = creditIdValues[36];
                            course.CreditId38 = creditIdValues[37];
                            course.CreditId39 = creditIdValues[38];
                            course.CreditId40 = creditIdValues[39];

                            course.CourseId1 = courseIdValues[0];
                            course.CourseId2 = courseIdValues[1];
                            course.CourseId3 = courseIdValues[2];
                            course.CourseId4 = courseIdValues[3];
                            course.CourseId5 = courseIdValues[4];
                            course.CourseId6 = courseIdValues[5];
                            course.CourseId7 = courseIdValues[6];
                            course.CourseId8 = courseIdValues[7];
                            course.CourseId9 = courseIdValues[8];
                            course.CourseId10 = courseIdValues[9];

                            course.CourseId11 = courseIdValues[10];
                            course.CourseId12 = courseIdValues[11];
                            course.CourseId13 = courseIdValues[12];
                            course.CourseId14 = courseIdValues[13];
                            course.CourseId15 = courseIdValues[14];
                            course.CourseId16 = courseIdValues[15];
                            course.CourseId17 = courseIdValues[16];
                            course.CourseId18 = courseIdValues[17];
                            course.CourseId19 = courseIdValues[18];
                            course.CourseId20 = courseIdValues[19];

                            course.CourseId21 = courseIdValues[20];
                            course.CourseId22 = courseIdValues[21];
                            course.CourseId23 = courseIdValues[22];
                            course.CourseId24 = courseIdValues[23];
                            course.CourseId25 = courseIdValues[24];
                            course.CourseId26 = courseIdValues[25];
                            course.CourseId27 = courseIdValues[26];
                            course.CourseId28 = courseIdValues[27];
                            course.CourseId29 = courseIdValues[28];
                            course.CourseId30 = courseIdValues[29];

                            course.CourseId31 = courseIdValues[30];
                            course.CourseId32 = courseIdValues[31];
                            course.CourseId33 = courseIdValues[32];
                            course.CourseId34 = courseIdValues[33];
                            course.CourseId35 = courseIdValues[34];
                            course.CourseId36 = courseIdValues[35];
                            course.CourseId37 = courseIdValues[36];
                            course.CourseId38 = courseIdValues[37];
                            course.CourseId39 = courseIdValues[38];
                            course.CourseId40 = courseIdValues[39];

                            course.Credit1 = creditValues[0] ?? 0M;
                            course.Credit2 = creditValues[1] ?? 0M;
                            course.Credit3 = creditValues[2] ?? 0M;
                            course.Credit4 = creditValues[3] ?? 0M;
                            course.Credit5 = creditValues[4] ?? 0M;
                            course.Credit6 = creditValues[5] ?? 0M;
                            course.Credit7 = creditValues[6] ?? 0M;
                            course.Credit8 = creditValues[7] ?? 0M;
                            course.Credit9 = creditValues[8] ?? 0M;
                            course.Credit10 = creditValues[9] ?? 0M;

                            course.Credit11 = creditValues[10] ?? 0M;
                            course.Credit12 = creditValues[11] ?? 0M;
                            course.Credit13 = creditValues[12] ?? 0M;
                            course.Credit14 = creditValues[13] ?? 0M;
                            course.Credit15 = creditValues[14] ?? 0M;
                            course.Credit16 = creditValues[15] ?? 0M;
                            course.Credit17 = creditValues[16] ?? 0M;
                            course.Credit18 = creditValues[17] ?? 0M;
                            course.Credit19 = creditValues[18] ?? 0M;
                            course.Credit20 = creditValues[19] ?? 0M;

                            course.Credit21 = creditValues[20] ?? 0M;
                            course.Credit22 = creditValues[21] ?? 0M;
                            course.Credit23 = creditValues[22] ?? 0M;
                            course.Credit24 = creditValues[23] ?? 0M;
                            course.Credit25 = creditValues[24] ?? 0M;
                            course.Credit26 = creditValues[25] ?? 0M;
                            course.Credit27 = creditValues[26] ?? 0M;
                            course.Credit28 = creditValues[27] ?? 0M;
                            course.Credit29 = creditValues[28] ?? 0M;
                            course.Credit30 = creditValues[29] ?? 0M;

                            course.Credit31 = creditValues[30] ?? 0M;
                            course.Credit32 = creditValues[31] ?? 0M;
                            course.Credit33 = creditValues[32] ?? 0M;
                            course.Credit34 = creditValues[33] ?? 0M;
                            course.Credit35 = creditValues[34] ?? 0M;
                            course.Credit36 = creditValues[35] ?? 0M;
                            course.Credit37 = creditValues[36] ?? 0M;
                            course.Credit38 = creditValues[37] ?? 0M;
                            course.Credit39 = creditValues[38] ?? 0M;
                            course.Credit40 = creditValues[39] ?? 0M;
                        }
                        #endregion

                        #region Remark (StudentReceiveEntity)
                        {
                            receive.Remark = fieldNames.Contains(MappingreXlsmdbEntity.Field.Remark) ? row[MappingreXlsmdbEntity.Field.Remark].ToString() : String.Empty;
                        }
                        #endregion

                        #region 轉帳資料相關對照欄位 (StudentReceiveEntity)
                        {
                            receive.DeductBankid = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductBankid) ? row[MappingreXlsmdbEntity.Field.DeductBankid].ToString() : String.Empty;
                            receive.DeductAccountno = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductAccountno) ? row[MappingreXlsmdbEntity.Field.DeductAccountno].ToString() : String.Empty;
                            receive.DeductAccountname = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductAccountname) ? row[MappingreXlsmdbEntity.Field.DeductAccountname].ToString() : String.Empty;
                            receive.DeductAccountid = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductAccountid) ? row[MappingreXlsmdbEntity.Field.DeductAccountid].ToString() : String.Empty;
                        }
                        #endregion

                        #region 備註相關對照欄位 (StudentReceiveEntity)
                        {
                            receive.Memo01 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo01) ? row[MappingreXlsmdbEntity.Field.Memo01].ToString() : String.Empty;
                            receive.Memo02 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo02) ? row[MappingreXlsmdbEntity.Field.Memo02].ToString() : String.Empty;
                            receive.Memo03 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo03) ? row[MappingreXlsmdbEntity.Field.Memo03].ToString() : String.Empty;
                            receive.Memo04 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo04) ? row[MappingreXlsmdbEntity.Field.Memo04].ToString() : String.Empty;
                            receive.Memo05 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo05) ? row[MappingreXlsmdbEntity.Field.Memo05].ToString() : String.Empty;
                            receive.Memo06 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo06) ? row[MappingreXlsmdbEntity.Field.Memo06].ToString() : String.Empty;
                            receive.Memo07 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo07) ? row[MappingreXlsmdbEntity.Field.Memo07].ToString() : String.Empty;
                            receive.Memo08 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo08) ? row[MappingreXlsmdbEntity.Field.Memo08].ToString() : String.Empty;
                            receive.Memo09 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo09) ? row[MappingreXlsmdbEntity.Field.Memo09].ToString() : String.Empty;
                            receive.Memo10 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo10) ? row[MappingreXlsmdbEntity.Field.Memo10].ToString() : String.Empty;

                            receive.Memo11 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo11) ? row[MappingreXlsmdbEntity.Field.Memo11].ToString() : String.Empty;
                            receive.Memo12 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo12) ? row[MappingreXlsmdbEntity.Field.Memo12].ToString() : String.Empty;
                            receive.Memo13 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo13) ? row[MappingreXlsmdbEntity.Field.Memo13].ToString() : String.Empty;
                            receive.Memo14 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo14) ? row[MappingreXlsmdbEntity.Field.Memo14].ToString() : String.Empty;
                            receive.Memo15 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo15) ? row[MappingreXlsmdbEntity.Field.Memo15].ToString() : String.Empty;
                            receive.Memo16 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo16) ? row[MappingreXlsmdbEntity.Field.Memo16].ToString() : String.Empty;
                            receive.Memo17 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo17) ? row[MappingreXlsmdbEntity.Field.Memo17].ToString() : String.Empty;
                            receive.Memo18 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo18) ? row[MappingreXlsmdbEntity.Field.Memo18].ToString() : String.Empty;
                            receive.Memo19 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo19) ? row[MappingreXlsmdbEntity.Field.Memo19].ToString() : String.Empty;
                            receive.Memo20 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo20) ? row[MappingreXlsmdbEntity.Field.Memo20].ToString() : String.Empty;

                            receive.Memo21 = fieldNames.Contains(MappingreXlsmdbEntity.Field.Memo21) ? row[MappingreXlsmdbEntity.Field.Memo21].ToString() : String.Empty;
                        }
                        #endregion

                        #region 找出舊的 StudentReceiveEntity
                        StudentReceiveEntity oldStudentReceive = null;
                        {
                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive.ReceiveType)
                                .And(StudentReceiveEntity.Field.YearId, receive.YearId)
                                .And(StudentReceiveEntity.Field.TermId, receive.TermId)
                                .And(StudentReceiveEntity.Field.DepId, receive.DepId)
                                .And(StudentReceiveEntity.Field.ReceiveId, receive.ReceiveId)
                                .And(StudentReceiveEntity.Field.StuId, receive.StuId)
                                .And(StudentReceiveEntity.Field.OldSeq, receive.OldSeq);
                            Result result = tsFactory.SelectFirst<StudentReceiveEntity>(where, null, out oldStudentReceive);
                            if (!result.IsSuccess)
                            {
                                string failMsg = String.Format("查詢學號 {0} 的學生繳費資料失敗，{1}", receive.StuId, result.Message);
                                row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                log.AppendFormat("第 {0} 筆資料處理失敗，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料處理失敗，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        #endregion

                        #region 虛擬帳號資料相關對照欄位 (StudentReceiveEntity)
                        {
                            //bool hasSeriorNoField = false;
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.SeriorNo))
                            {
                                //有上傳流水號
                                uploadFlag |= StudentReceiveEntity.UploadSeriorNoFlag;
                                //hasSeriorNoField = true;
                                receive.SeriorNo = row[MappingreXlsmdbEntity.Field.SeriorNo].ToString().PadLeft(module.SeriorNoSize, '0');
                            }
                            else
                            {
                                //保持原有的流水號
                                receive.SeriorNo = oldStudentReceive == null ? null : oldStudentReceive.SeriorNo;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.CancelNo))
                            {
                                //有上傳自訂虛擬帳號
                                uploadFlag |= StudentReceiveEntity.UploadCancelNoFlag;
                                receive.CancelNo = row[MappingreXlsmdbEntity.Field.CancelNo].ToString();

                                #region [Old]
                                //string myReceiveType = null;
                                //string myCustomNo = null;
                                //string myChecksum = null;
                                //string myYearId = null;
                                //string myTermId = null;
                                //string myReceiveId = null;
                                //string mySeriorNo = null;
                                //if (module.TryParseCancelNo(receive.CancelNo, school.IsBigReceiveId(), out myReceiveType, out myCustomNo, out myChecksum, out myYearId, out myTermId, out myReceiveId, out mySeriorNo))
                                //{
                                //    string failMsg = null;
                                //    if (receiveType != myReceiveType)
                                //    {
                                //        failMsg = "指定的虛擬帳號與商家代號不相符";
                                //    }
                                //    else if (hasSeriorNoField && receive.SeriorNo.TrimStart('0') != mySeriorNo.TrimStart('0'))
                                //    {
                                //        failMsg = "指定的虛擬帳號與流水號不相符";
                                //    }

                                //    if (!hasSeriorNoField)
                                //    {
                                //        receive.SeriorNo = mySeriorNo;
                                //    }

                                //    if (!String.IsNullOrEmpty(failMsg))
                                //    {
                                //        row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                //        log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                //        if (isBatch)
                                //        {
                                //            importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                //            break;
                                //        }
                                //        else
                                //        {
                                //            continue;
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    string failMsg = "指定的虛擬帳號不正確，無法取得其中的流水號";
                                //    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                //    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                //    if (isBatch)
                                //    {
                                //        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                //        break;
                                //    }
                                //    else
                                //    {
                                //        continue;
                                //    }
                                //}
                                #endregion

                                //這裡只是為了計算收入科目總金額與繳費金額是否相同
                                if (!amountHelper.CheckBillAmount(receive))
                                {
                                    string failMsg = "收入科目總金額與繳費金額不和";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    string oldCancelNo = oldStudentReceive == null ? null : oldStudentReceive.CancelNo;
                                    decimal receiveAmount = receive.ReceiveAmount == null ? -1 : receive.ReceiveAmount.Value;
                                    string failMsg = cnoHelper.CheckCustomCancelNo(receive.CancelNo, module, oldCancelNo, receiveAmount);
                                    if (!String.IsNullOrEmpty(failMsg))
                                    {
                                        row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                        log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        if (isBatch)
                                        {
                                            importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                receive.CancelNo = null;
                            }
                        }
                        #endregion

                        receive.Uploadflag = uploadFlag == 0 ? String.Empty : uploadFlag.ToString();

                        receive.UpNo = seriroNo > 0 ? seriroNo.ToString() : String.Empty;
                        receive.UpOrder = okNo.ToString("000000");
                        receive.MappingId = mappingId;
                        receive.MappingType = fileType == "xls" || fileType == "xlsx" ? "2" : "1";
                        receive.Exportreceivedata = "N";

                        //[TODO]
                        receive.BillingType = "2";

                        receive.CreateDate = now;
                        //receive.UpdateDate = now;
                        #endregion

                        #region 寫入資料庫
                        string updateUpNo = null;   //紀錄此筆資料被更新的所屬批號 (非更新時為 null)
                        if (importResult.IsSuccess)
                        {
                            int count = 0;
                            Result result = new Result(false, "程式處理流程錯誤", CoreStatusCode.UNKNOWN_ERROR, null);

                            #region StudentReceiveEntity
                            {
                                if (oldStudentReceive == null)
                                {
                                    #region [MDY:20160131] 開放指定資料序號，所以不用檢查了
                                    #region [Old]
                                    //#region 檢查是否有相同學號的轉置資料
                                    //int oldDataCount = 0;
                                    //{
                                    //    Expression where2 = new Expression(StudentReceiveEntity.Field.ReceiveType, receive.ReceiveType)
                                    //        .And(StudentReceiveEntity.Field.YearId, receive.YearId)
                                    //        .And(StudentReceiveEntity.Field.TermId, receive.TermId)
                                    //        .And(StudentReceiveEntity.Field.DepId, receive.DepId)
                                    //        .And(StudentReceiveEntity.Field.ReceiveId, receive.ReceiveId)
                                    //        .And(StudentReceiveEntity.Field.StuId, receive.StuId)
                                    //        .And(StudentReceiveEntity.Field.OldSeq, RelationEnum.Greater, 0);
                                    //    result = tsFactory.SelectCount<StudentReceiveEntity>(where2, out oldDataCount);
                                    //    if (result.IsSuccess && oldDataCount > 0)
                                    //    {
                                    //        result = new Result(false, "該學生在此費用別已有舊的轉置資料，系統無法判斷處理方式", ErrorCode.D_DATA_EXISTS, null);
                                    //    }
                                    //}
                                    //#endregion

                                    //#region Insert
                                    //if (result.IsSuccess)
                                    //{
                                    //    result = tsFactory.Insert(receive, out count);
                                    //    if (result.IsSuccess && count == 0)
                                    //    {
                                    //        result = new Result(false, "學生繳費資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                    //    }
                                    //}
                                    //#endregion
                                    #endregion

                                    #region Insert
                                    result = tsFactory.Insert(receive, out count);
                                    if (result.IsSuccess && count == 0)
                                    {
                                        result = new Result(false, "學生繳費資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                    }
                                    #endregion
                                    #endregion
                                }
                                else
                                {
                                    #region Update
                                    updateUpNo = oldStudentReceive.UpNo;

                                    if (!String.IsNullOrWhiteSpace(oldStudentReceive.CancelFlag)
                                        || !String.IsNullOrEmpty(oldStudentReceive.ReceiveDate)
                                        || !String.IsNullOrEmpty(oldStudentReceive.ReceiveWay)
                                        || !String.IsNullOrEmpty(oldStudentReceive.AccountDate))
                                    {
                                        result = new Result(false, "該筆學生繳費資料已繳費", ErrorCode.D_DATA_EXISTS, null);
                                    }
                                    else if (!String.IsNullOrEmpty(oldStudentReceive.CancelNo) && !oldStudentReceive.HasUploadCancelNo())
                                    {
                                        result = new Result(false, "該筆學生繳費資料已產生銷編", ErrorCode.D_DATA_EXISTS, null);
                                    }
                                    else
                                    {
                                        #region 更新條件
                                        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive.ReceiveType)
                                            .And(StudentReceiveEntity.Field.YearId, receive.YearId)
                                            .And(StudentReceiveEntity.Field.TermId, receive.TermId)
                                            .And(StudentReceiveEntity.Field.DepId, receive.DepId)
                                            .And(StudentReceiveEntity.Field.ReceiveId, receive.ReceiveId)
                                            .And(StudentReceiveEntity.Field.StuId, receive.StuId)
                                            .And(StudentReceiveEntity.Field.OldSeq, receive.OldSeq);

                                        #region [20150915] 加強更新條件，避免更新到已繳或已銷的資料
                                        {
                                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
                                            where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
                                        }
                                        #endregion
                                        #endregion

                                        #region 更新欄位
                                        KeyValueList fieldValues = new KeyValueList();

                                        #region [MDY:20160131] 增加繳費期限
                                        if (hasPayDueDateField)
                                        {
                                            fieldValues.Add(StudentReceiveEntity.Field.PayDueDate, receive.PayDueDate);
                                        }
                                        #endregion

                                        #region 學籍資料
                                        fieldValues.Add(StudentReceiveEntity.Field.StuGrade, receive.StuGrade);
                                        fieldValues.Add(StudentReceiveEntity.Field.StuHid, receive.StuHid);

                                        fieldValues.Add(StudentReceiveEntity.Field.ClassId, receive.ClassId);
                                        fieldValues.Add(StudentReceiveEntity.Field.CollegeId, receive.CollegeId);
                                        fieldValues.Add(StudentReceiveEntity.Field.MajorId, receive.MajorId);

                                        fieldValues.Add(StudentReceiveEntity.Field.ReduceId, receive.ReduceId);
                                        fieldValues.Add(StudentReceiveEntity.Field.LoanId, receive.LoanId);
                                        fieldValues.Add(StudentReceiveEntity.Field.DormId, receive.DormId);

                                        fieldValues.Add(StudentReceiveEntity.Field.IdentifyId01, receive.IdentifyId01);
                                        fieldValues.Add(StudentReceiveEntity.Field.IdentifyId02, receive.IdentifyId02);
                                        fieldValues.Add(StudentReceiveEntity.Field.IdentifyId03, receive.IdentifyId03);
                                        fieldValues.Add(StudentReceiveEntity.Field.IdentifyId04, receive.IdentifyId04);
                                        fieldValues.Add(StudentReceiveEntity.Field.IdentifyId05, receive.IdentifyId05);
                                        fieldValues.Add(StudentReceiveEntity.Field.IdentifyId06, receive.IdentifyId06);
                                        #endregion

                                        #region 收入科目金額
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive01, receive.Receive01);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive02, receive.Receive02);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive03, receive.Receive03);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive04, receive.Receive04);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive05, receive.Receive05);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive06, receive.Receive06);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive07, receive.Receive07);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive08, receive.Receive08);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive09, receive.Receive09);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive10, receive.Receive10);

                                        fieldValues.Add(StudentReceiveEntity.Field.Receive11, receive.Receive11);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive12, receive.Receive12);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive13, receive.Receive13);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive14, receive.Receive14);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive15, receive.Receive15);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive16, receive.Receive16);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive17, receive.Receive17);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive18, receive.Receive18);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive19, receive.Receive19);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive20, receive.Receive20);

                                        fieldValues.Add(StudentReceiveEntity.Field.Receive21, receive.Receive21);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive22, receive.Receive22);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive23, receive.Receive23);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive24, receive.Receive24);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive25, receive.Receive25);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive26, receive.Receive26);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive27, receive.Receive27);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive28, receive.Receive28);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive29, receive.Receive29);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive30, receive.Receive30);

                                        fieldValues.Add(StudentReceiveEntity.Field.Receive31, receive.Receive31);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive32, receive.Receive32);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive33, receive.Receive33);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive34, receive.Receive34);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive35, receive.Receive35);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive36, receive.Receive36);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive37, receive.Receive37);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive38, receive.Receive38);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive39, receive.Receive39);
                                        fieldValues.Add(StudentReceiveEntity.Field.Receive40, receive.Receive40);
                                        #endregion

                                        #region 繳費資料
                                        fieldValues.Add(StudentReceiveEntity.Field.StuHour, receive.StuHour);
                                        fieldValues.Add(StudentReceiveEntity.Field.StuCredit, receive.StuCredit);
                                        fieldValues.Add(StudentReceiveEntity.Field.LoanAmount, receive.LoanAmount);
                                        fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, receive.ReceiveAmount);

                                        fieldValues.Add(StudentReceiveEntity.Field.SeriorNo, receive.SeriorNo);
                                        fieldValues.Add(StudentReceiveEntity.Field.CancelNo, receive.CancelNo);
                                        #endregion

                                        #region 轉帳資料
                                        fieldValues.Add(StudentReceiveEntity.Field.DeductBankid, receive.DeductBankid);
                                        fieldValues.Add(StudentReceiveEntity.Field.DeductAccountno, receive.DeductAccountno);
                                        fieldValues.Add(StudentReceiveEntity.Field.DeductAccountname, receive.DeductAccountname);
                                        fieldValues.Add(StudentReceiveEntity.Field.DeductAccountid, receive.DeductAccountid);
                                        #endregion

                                        #region 備註資料
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo01, receive.Memo01);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo02, receive.Memo02);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo03, receive.Memo03);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo04, receive.Memo04);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo05, receive.Memo05);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo06, receive.Memo06);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo07, receive.Memo07);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo08, receive.Memo08);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo09, receive.Memo09);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo10, receive.Memo10);

                                        fieldValues.Add(StudentReceiveEntity.Field.Memo11, receive.Memo11);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo12, receive.Memo12);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo13, receive.Memo13);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo14, receive.Memo14);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo15, receive.Memo15);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo16, receive.Memo16);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo17, receive.Memo17);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo18, receive.Memo18);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo19, receive.Memo19);
                                        fieldValues.Add(StudentReceiveEntity.Field.Memo20, receive.Memo20);

                                        fieldValues.Add(StudentReceiveEntity.Field.Memo21, receive.Memo21);
                                        #endregion

                                        #region 更新資料要把臨櫃與超商的金額與虛擬帳號清除，避免誤以為已產生了
                                        fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, null);
                                        fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, null);
                                        fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, null);
                                        fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, null);
                                        #endregion

                                        #region 有異動金額或虛擬帳號，中信資料發送旗標清為 0
                                        if (receive.CancelNo != oldStudentReceive.CancelNo || receive.ReceiveAmount != oldStudentReceive.ReceiveAmount)
                                        {
                                            receive.CFlag = "0";
                                            fieldValues.Add(StudentReceiveEntity.Field.CFlag, receive.CFlag);
                                        }
                                        #endregion

                                        #region D38資料發送旗標 (統一更新資料時不覆蓋此欄位，因為系統沒有提供 D38 回收機制)
                                        //fieldValues.Add(StudentReceiveEntity.Field.Exportreceivedata, receive.Exportreceivedata);
                                        #endregion

                                        fieldValues.Add(StudentReceiveEntity.Field.Uploadflag, receive.Uploadflag);
                                        fieldValues.Add(StudentReceiveEntity.Field.MappingId, receive.MappingId);
                                        fieldValues.Add(StudentReceiveEntity.Field.MappingType, receive.MappingType);

                                        fieldValues.Add(StudentReceiveEntity.Field.Remark, receive.Remark);

                                        fieldValues.Add(StudentReceiveEntity.Field.UpdateDate, now);
                                        #endregion

                                        result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生繳費資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region StudentMasterEntity
                            if (result.IsSuccess)
                            {
                                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, student.ReceiveType)
                                    .And(StudentMasterEntity.Field.DepId, student.DepId)
                                    .And(StudentMasterEntity.Field.Id, student.Id);
                                result = tsFactory.SelectCount<StudentMasterEntity>(where, out count);
                                if (result.IsSuccess)
                                {
                                    if (count == 0)
                                    {
                                        #region Insert
                                        result = tsFactory.Insert(student, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update
                                        KeyValueList fieldValues = new KeyValueList();
                                        fieldValues.Add(StudentMasterEntity.Field.Name, student.Name);
                                        fieldValues.Add(StudentMasterEntity.Field.Birthday, student.Birthday);
                                        fieldValues.Add(StudentMasterEntity.Field.IdNumber, student.IdNumber);
                                        fieldValues.Add(StudentMasterEntity.Field.Tel, student.Tel);
                                        fieldValues.Add(StudentMasterEntity.Field.ZipCode, student.ZipCode);
                                        fieldValues.Add(StudentMasterEntity.Field.Address, student.Address);
                                        fieldValues.Add(StudentMasterEntity.Field.Email, student.Email);
                                        fieldValues.Add(StudentMasterEntity.Field.Account, student.Account);
                                        fieldValues.Add(StudentMasterEntity.Field.StuParent, student.StuParent);

                                        fieldValues.Add(StudentMasterEntity.Field.MdyDate, now);

                                        result = tsFactory.UpdateFields<StudentMasterEntity>(fieldValues, where, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生繳費資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region StudentCourseEntity
                            if (result.IsSuccess && course != null)
                            {
                                Expression where = new Expression(StudentCourseEntity.Field.ReceiveType, course.ReceiveType)
                                    .And(StudentCourseEntity.Field.YearId, course.YearId)
                                    .And(StudentCourseEntity.Field.TermId, course.TermId)
                                    .And(StudentCourseEntity.Field.DepId, course.DepId)
                                    .And(StudentCourseEntity.Field.ReceiveId, course.ReceiveId)
                                    .And(StudentCourseEntity.Field.StuId, course.StuId)
                                    .And(StudentCourseEntity.Field.OldSeq, course.OldSeq);
                                result = tsFactory.SelectCount<StudentCourseEntity>(where, out count);
                                if (result.IsSuccess)
                                {
                                    if (count == 0)
                                    {
                                        #region Insert
                                        result = tsFactory.Insert(course, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生課程資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update
                                        result = tsFactory.Update(course, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生課程資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region StudentLoanEntity
                            if (result.IsSuccess)
                            {
                                Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, receive.ReceiveType)
                                    .And(StudentLoanEntity.Field.YearId, receive.YearId)
                                    .And(StudentLoanEntity.Field.TermId, receive.TermId)
                                    .And(StudentLoanEntity.Field.DepId, receive.DepId)
                                    .And(StudentLoanEntity.Field.ReceiveId, receive.ReceiveId)
                                    .And(StudentLoanEntity.Field.StuId, receive.StuId)
                                    .And(StudentLoanEntity.Field.OldSeq, receive.OldSeq)
                                    .And(StudentLoanEntity.Field.LoanId, receive.LoanId);
                                result = tsFactory.SelectCount<StudentLoanEntity>(where, out count);
                                if (result.IsSuccess)
                                {
                                    if (count == 0)
                                    {
                                        #region Insert
                                        StudentLoanEntity studentLoan = null;
                                        studentLoan = new StudentLoanEntity();
                                        studentLoan.ReceiveType = receive.ReceiveType;
                                        studentLoan.YearId = receive.YearId;
                                        studentLoan.TermId = receive.TermId;
                                        studentLoan.DepId = receive.DepId;
                                        studentLoan.ReceiveId = receive.ReceiveId;
                                        studentLoan.StuId = receive.StuId;
                                        studentLoan.OldSeq = receive.OldSeq;
                                        studentLoan.LoanId = receive.LoanId;
                                        studentLoan.LoanAmount = receive.LoanAmount ?? 0M;
                                        result = tsFactory.Insert(studentLoan, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生就貸資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update
                                        KeyValueList fieldValues = new KeyValueList();
                                        fieldValues.Add(StudentLoanEntity.Field.LoanAmount, receive.LoanAmount);

                                        result = tsFactory.UpdateFields<StudentLoanEntity>(fieldValues, where, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生就貸資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity, ReduceListEntity, LoanListEntity, DormListEntity
                            {
                                #region ClassListEntity
                                if (result.IsSuccess && classEntity != null && classDatas.Find(x => x.ClassId == classEntity.ClassId) == null)
                                {
                                    Expression where = new Expression(ClassListEntity.Field.ReceiveType, classEntity.ReceiveType)
                                        .And(ClassListEntity.Field.YearId, classEntity.YearId)
                                        .And(ClassListEntity.Field.TermId, classEntity.TermId)
                                        .And(ClassListEntity.Field.DepId, classEntity.DepId)
                                        .And(ClassListEntity.Field.ClassId, classEntity.ClassId);
                                    result = tsFactory.SelectCount<ClassListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(classEntity, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "班別就貸資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(ClassListEntity.Field.ClassName, classEntity.ClassName);

                                            fieldValues.Add(ClassListEntity.Field.MdyDate, classEntity.CrtDate);
                                            fieldValues.Add(ClassListEntity.Field.MdyUser, classEntity.CrtUser);

                                            result = tsFactory.UpdateFields<ClassListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "班別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        classDatas.Add(classEntity);
                                    }
                                }
                                #endregion

                                #region DeptListEntity
                                if (result.IsSuccess && dept != null && deptDatas.Find(x => x.DeptId == dept.DeptId) == null)
                                {
                                    Expression where = new Expression(DeptListEntity.Field.ReceiveType, dept.ReceiveType)
                                        .And(DeptListEntity.Field.YearId, dept.YearId)
                                        .And(DeptListEntity.Field.TermId, dept.TermId)
                                        .And(DeptListEntity.Field.DeptId, dept.DeptId);
                                    result = tsFactory.SelectCount<DeptListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(dept, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "部別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(DeptListEntity.Field.DeptName, dept.DeptName);

                                            fieldValues.Add(DeptListEntity.Field.MdyDate, dept.CrtDate);
                                            fieldValues.Add(DeptListEntity.Field.MdyUser, dept.CrtUser);

                                            result = tsFactory.UpdateFields<DeptListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "院別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        deptDatas.Add(dept);
                                    }
                                }
                                #endregion

                                #region CollegeListEntity
                                if (result.IsSuccess && college != null && collegeDatas.Find(x => x.CollegeId == college.CollegeId) == null)
                                {
                                    Expression where = new Expression(CollegeListEntity.Field.ReceiveType, college.ReceiveType)
                                        .And(CollegeListEntity.Field.YearId, college.YearId)
                                        .And(CollegeListEntity.Field.TermId, college.TermId)
                                        .And(CollegeListEntity.Field.DepId, college.DepId)
                                        .And(CollegeListEntity.Field.CollegeId, college.CollegeId);
                                    result = tsFactory.SelectCount<CollegeListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(college, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "院別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(CollegeListEntity.Field.CollegeName, college.CollegeName);

                                            fieldValues.Add(CollegeListEntity.Field.MdyDate, college.CrtDate);
                                            fieldValues.Add(CollegeListEntity.Field.MdyUser, college.CrtUser);

                                            result = tsFactory.UpdateFields<CollegeListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "院別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        collegeDatas.Add(college);
                                    }
                                }
                                #endregion

                                #region MajorListEntity
                                if (result.IsSuccess && major != null && majorDatas.Find(x => x.MajorId == major.MajorId) == null)
                                {
                                    Expression where = new Expression(MajorListEntity.Field.ReceiveType, major.ReceiveType)
                                        .And(MajorListEntity.Field.YearId, major.YearId)
                                        .And(MajorListEntity.Field.TermId, major.TermId)
                                        .And(MajorListEntity.Field.DepId, major.DepId)
                                        .And(MajorListEntity.Field.MajorId, major.MajorId);
                                    result = tsFactory.SelectCount<MajorListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(major, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "系所資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(MajorListEntity.Field.MajorName, major.MajorName);

                                            fieldValues.Add(MajorListEntity.Field.MdyDate, major.CrtDate);
                                            fieldValues.Add(MajorListEntity.Field.MdyUser, major.CrtUser);

                                            result = tsFactory.UpdateFields<MajorListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "系所資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        majorDatas.Add(major);
                                    }
                                }
                                #endregion

                                #region ReduceListEntity
                                if (result.IsSuccess && reduce != null && reduceDatas.Find(x => x.ReduceId == reduce.ReduceId) == null)
                                {
                                    Expression where = new Expression(ReduceListEntity.Field.ReceiveType, reduce.ReceiveType)
                                        .And(ReduceListEntity.Field.YearId, reduce.YearId)
                                        .And(ReduceListEntity.Field.TermId, reduce.TermId)
                                        .And(ReduceListEntity.Field.DepId, reduce.DepId)
                                        .And(ReduceListEntity.Field.ReduceId, reduce.ReduceId);
                                    result = tsFactory.SelectCount<ReduceListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(reduce, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "減免類別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(ReduceListEntity.Field.ReduceName, reduce.ReduceName);

                                            fieldValues.Add(ReduceListEntity.Field.MdyDate, reduce.CrtDate);
                                            fieldValues.Add(ReduceListEntity.Field.MdyUser, reduce.CrtUser);

                                            result = tsFactory.UpdateFields<ReduceListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "減免類別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        reduceDatas.Add(reduce);
                                    }
                                }
                                #endregion

                                #region LoanListEntity
                                if (result.IsSuccess && loan != null && loanDatas.Find(x => x.LoanId == loan.LoanId) == null)
                                {
                                    Expression where = new Expression(LoanListEntity.Field.ReceiveType, loan.ReceiveType)
                                        .And(LoanListEntity.Field.YearId, loan.YearId)
                                        .And(LoanListEntity.Field.TermId, loan.TermId)
                                        .And(LoanListEntity.Field.DepId, loan.DepId)
                                        .And(LoanListEntity.Field.LoanId, loan.LoanId);
                                    result = tsFactory.SelectCount<LoanListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(loan, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "就貸項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(LoanListEntity.Field.LoanName, loan.LoanName);

                                            fieldValues.Add(LoanListEntity.Field.MdyDate, loan.CrtDate);
                                            fieldValues.Add(LoanListEntity.Field.MdyUser, loan.CrtUser);

                                            result = tsFactory.UpdateFields<LoanListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "就貸項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        loanDatas.Add(loan);
                                    }
                                }
                                #endregion

                                #region DormListEntity
                                if (result.IsSuccess && dorm != null && dormDatas.Find(x => x.DormId == dorm.DormId) == null)
                                {
                                    Expression where = new Expression(DormListEntity.Field.ReceiveType, dorm.ReceiveType)
                                        .And(DormListEntity.Field.YearId, dorm.YearId)
                                        .And(DormListEntity.Field.TermId, dorm.TermId)
                                        .And(DormListEntity.Field.DepId, dorm.DepId)
                                        .And(DormListEntity.Field.DormId, dorm.DormId);
                                    result = tsFactory.SelectCount<DormListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(dorm, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(DormListEntity.Field.DormName, dorm.DormName);

                                            fieldValues.Add(DormListEntity.Field.MdyDate, dorm.CrtDate);
                                            fieldValues.Add(DormListEntity.Field.MdyUser, dorm.CrtUser);

                                            result = tsFactory.UpdateFields<DormListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        dormDatas.Add(dorm);
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity
                            {
                                #region IdentifyList1Entity
                                if (result.IsSuccess && identify1 != null && identify1Datas.Find(x => x.IdentifyId == identify1.IdentifyId) == null)
                                {
                                    Expression where = new Expression(IdentifyList1Entity.Field.ReceiveType, identify1.ReceiveType)
                                        .And(IdentifyList1Entity.Field.YearId, identify1.YearId)
                                        .And(IdentifyList1Entity.Field.TermId, identify1.TermId)
                                        .And(IdentifyList1Entity.Field.DepId, identify1.DepId)
                                        .And(IdentifyList1Entity.Field.IdentifyId, identify1.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList1Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify1, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記01資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList1Entity.Field.IdentifyName, identify1.IdentifyName);

                                            fieldValues.Add(IdentifyList1Entity.Field.MdyDate, identify1.CrtDate);
                                            fieldValues.Add(IdentifyList1Entity.Field.MdyUser, identify1.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList1Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        identify1Datas.Add(identify1);
                                    }
                                }
                                #endregion

                                #region IdentifyList2Entity
                                if (result.IsSuccess && identify2 != null && identify2Datas.Find(x => x.IdentifyId == identify2.IdentifyId) == null)
                                {
                                    Expression where = new Expression(IdentifyList2Entity.Field.ReceiveType, identify2.ReceiveType)
                                        .And(IdentifyList2Entity.Field.YearId, identify2.YearId)
                                        .And(IdentifyList2Entity.Field.TermId, identify2.TermId)
                                        .And(IdentifyList2Entity.Field.DepId, identify2.DepId)
                                        .And(IdentifyList2Entity.Field.IdentifyId, identify2.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList2Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify2, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記02資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList2Entity.Field.IdentifyName, identify2.IdentifyName);

                                            fieldValues.Add(IdentifyList2Entity.Field.MdyDate, identify2.CrtDate);
                                            fieldValues.Add(IdentifyList2Entity.Field.MdyUser, identify2.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList2Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        identify2Datas.Add(identify2);
                                    }
                                }
                                #endregion

                                #region IdentifyList3Entity
                                if (result.IsSuccess && identify3 != null && identify3Datas.Find(x => x.IdentifyId == identify3.IdentifyId) == null)
                                {
                                    Expression where = new Expression(IdentifyList3Entity.Field.ReceiveType, identify3.ReceiveType)
                                        .And(IdentifyList3Entity.Field.YearId, identify3.YearId)
                                        .And(IdentifyList3Entity.Field.TermId, identify3.TermId)
                                        .And(IdentifyList3Entity.Field.DepId, identify3.DepId)
                                        .And(IdentifyList3Entity.Field.IdentifyId, identify3.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList3Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify3, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記03資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList3Entity.Field.IdentifyName, identify3.IdentifyName);

                                            fieldValues.Add(IdentifyList3Entity.Field.MdyDate, identify3.CrtDate);
                                            fieldValues.Add(IdentifyList3Entity.Field.MdyUser, identify3.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList3Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        identify3Datas.Add(identify3);
                                    }
                                }
                                #endregion

                                #region IdentifyList4Entity
                                if (result.IsSuccess && identify4 != null && identify4Datas.Find(x => x.IdentifyId == identify4.IdentifyId) == null)
                                {
                                    Expression where = new Expression(IdentifyList4Entity.Field.ReceiveType, identify4.ReceiveType)
                                        .And(IdentifyList4Entity.Field.YearId, identify4.YearId)
                                        .And(IdentifyList4Entity.Field.TermId, identify4.TermId)
                                        .And(IdentifyList4Entity.Field.DepId, identify4.DepId)
                                        .And(IdentifyList4Entity.Field.IdentifyId, identify4.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList4Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify4, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記04資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList4Entity.Field.IdentifyName, identify4.IdentifyName);

                                            fieldValues.Add(IdentifyList4Entity.Field.MdyDate, identify4.CrtDate);
                                            fieldValues.Add(IdentifyList4Entity.Field.MdyUser, identify4.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList4Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        identify4Datas.Add(identify4);
                                    }
                                }
                                #endregion

                                #region IdentifyList5Entity
                                if (result.IsSuccess && identify5 != null && identify5Datas.Find(x => x.IdentifyId == identify5.IdentifyId) == null)
                                {
                                    Expression where = new Expression(IdentifyList5Entity.Field.ReceiveType, identify5.ReceiveType)
                                        .And(IdentifyList5Entity.Field.YearId, identify5.YearId)
                                        .And(IdentifyList5Entity.Field.TermId, identify5.TermId)
                                        .And(IdentifyList5Entity.Field.DepId, identify5.DepId)
                                        .And(IdentifyList5Entity.Field.IdentifyId, identify5.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList5Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify5, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記05資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList5Entity.Field.IdentifyName, identify5.IdentifyName);

                                            fieldValues.Add(IdentifyList5Entity.Field.MdyDate, identify5.CrtDate);
                                            fieldValues.Add(IdentifyList5Entity.Field.MdyUser, identify5.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList5Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        identify5Datas.Add(identify5);
                                    }
                                }
                                #endregion

                                #region IdentifyList6Entity
                                if (result.IsSuccess && identify6 != null && identify6Datas.Find(x => x.IdentifyId == identify6.IdentifyId) == null)
                                {
                                    Expression where = new Expression(IdentifyList6Entity.Field.ReceiveType, identify6.ReceiveType)
                                        .And(IdentifyList6Entity.Field.YearId, identify6.YearId)
                                        .And(IdentifyList6Entity.Field.TermId, identify6.TermId)
                                        .And(IdentifyList6Entity.Field.DepId, identify6.DepId)
                                        .And(IdentifyList6Entity.Field.IdentifyId, identify6.IdentifyId);
                                    result = tsFactory.SelectCount<IdentifyList6Entity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(identify6, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "身分註記06資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(IdentifyList6Entity.Field.IdentifyName, identify6.IdentifyName);

                                            fieldValues.Add(IdentifyList6Entity.Field.MdyDate, identify6.CrtDate);
                                            fieldValues.Add(IdentifyList6Entity.Field.MdyUser, identify6.CrtUser);

                                            result = tsFactory.UpdateFields<IdentifyList6Entity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "住宿項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        identify6Datas.Add(identify6);
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            if (result.IsSuccess)
                            {
                                tsFactory.Commit(); //每一筆(組)資料處理成功就 Ｃommit
                                successCount++;
                                okNo++;

                                #region 處理匯入資料所屬批號的統計
                                if (updateUpNo == null)
                                {
                                    insertCount++;
                                }
                                else
                                {
                                    updateCount++;
                                    KeyValue<int> updateUpNoCount = updateUpNoCounts.Find(x => x.Key == updateUpNo);
                                    if (updateUpNoCount == null)
                                    {
                                        updateUpNoCounts.Add(updateUpNo, 1);
                                    }
                                    else
                                    {
                                        updateUpNoCount.Value++;
                                    }
                                }
                                #endregion

                                //log.AppendFormat("第 {0} 筆資料新增成功", rowNo).AppendLine();
                            }
                            else
                            {
                                log.AppendFormat("第 {0} 筆資料新增失敗，錯誤訊息：{1}", rowNo, result.Message).AppendLine();
                                tsFactory.Rollback();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料新增失敗，錯誤訊息：：{1}", rowNo, result.Message), result.Code, result.Exception);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (!importResult.IsSuccess && isBatch)
                        {
                            break;
                        }
                        #endregion
                    }
                }

                #region 產生匯入資料所屬批號的統計的日誌
                {
                    log.AppendFormat("新增 {0} 筆資料 (批號 {1})，更新 {2} 筆資料", insertCount, seriroNo, updateCount).AppendLine();
                    foreach (KeyValue<int> updateUpNoCount in updateUpNoCounts)
                    {
                        log.AppendFormat("批號 {0} 更新 {1} 筆", updateUpNoCount.Key, updateUpNoCount.Value).AppendLine();
                    }
                }
                #endregion

                logmsg = log.ToString();
                if (importResult.IsSuccess && successCount == 0)
                {
                    importResult = new Result(false, "無資料被匯入成功", CoreStatusCode.UNKNOWN_ERROR, null);
                }

                #region 補代收科目名稱與備註標題
                if (importResult.IsSuccess && schoolRid != null)
                {
                    if (isAddSchoolRid)
                    {
                        #region 新增
                        int count = 0;
                        _Factory.Insert(schoolRid, out count);
                        #endregion
                    }
                    else
                    {
                        #region 更新
                        KeyValueList fieldValues = new KeyValueList(122);
                        if (receiveItemNames != null && receiveItemNames.Length > 0)
                        {
                            //因為要以上傳的欄位名稱為準，所以一律覆蓋，所以不用管舊資料
                            string[] fields = new string[] {
                                SchoolRidEntity.Field.ReceiveItem01, SchoolRidEntity.Field.ReceiveItem02, SchoolRidEntity.Field.ReceiveItem03, SchoolRidEntity.Field.ReceiveItem04, SchoolRidEntity.Field.ReceiveItem05,
                                SchoolRidEntity.Field.ReceiveItem06, SchoolRidEntity.Field.ReceiveItem07, SchoolRidEntity.Field.ReceiveItem08, SchoolRidEntity.Field.ReceiveItem09, SchoolRidEntity.Field.ReceiveItem10,
                                SchoolRidEntity.Field.ReceiveItem11, SchoolRidEntity.Field.ReceiveItem12, SchoolRidEntity.Field.ReceiveItem13, SchoolRidEntity.Field.ReceiveItem14, SchoolRidEntity.Field.ReceiveItem15,
                                SchoolRidEntity.Field.ReceiveItem16, SchoolRidEntity.Field.ReceiveItem17, SchoolRidEntity.Field.ReceiveItem18, SchoolRidEntity.Field.ReceiveItem19, SchoolRidEntity.Field.ReceiveItem20,
                                SchoolRidEntity.Field.ReceiveItem21, SchoolRidEntity.Field.ReceiveItem22, SchoolRidEntity.Field.ReceiveItem23, SchoolRidEntity.Field.ReceiveItem24, SchoolRidEntity.Field.ReceiveItem25,
                                SchoolRidEntity.Field.ReceiveItem26, SchoolRidEntity.Field.ReceiveItem27, SchoolRidEntity.Field.ReceiveItem28, SchoolRidEntity.Field.ReceiveItem29, SchoolRidEntity.Field.ReceiveItem30,
                                SchoolRidEntity.Field.ReceiveItem31, SchoolRidEntity.Field.ReceiveItem32, SchoolRidEntity.Field.ReceiveItem33, SchoolRidEntity.Field.ReceiveItem34, SchoolRidEntity.Field.ReceiveItem35,
                                SchoolRidEntity.Field.ReceiveItem36, SchoolRidEntity.Field.ReceiveItem37, SchoolRidEntity.Field.ReceiveItem38, SchoolRidEntity.Field.ReceiveItem39, SchoolRidEntity.Field.ReceiveItem40
                            };
                            for (int idx = 0; idx < fields.Length; idx++)
                            {
                                string value = idx < receiveItemNames.Length ? receiveItemNames[idx] : null;
                                fieldValues.Add(fields[idx], value);
                            }
                        }
                        if (memoTitles != null && memoTitles.Length > 0)
                        {
                            //因為要以上傳的欄位名稱為準，所以一律覆蓋，所以不用管舊資料
                            string[] fields = new string[SchoolRidEntity.MemoTitleMaxCount] {
                                SchoolRidEntity.Field.MemoTitle01, SchoolRidEntity.Field.MemoTitle02, SchoolRidEntity.Field.MemoTitle03, SchoolRidEntity.Field.MemoTitle04, SchoolRidEntity.Field.MemoTitle05,
                                SchoolRidEntity.Field.MemoTitle06, SchoolRidEntity.Field.MemoTitle07, SchoolRidEntity.Field.MemoTitle08, SchoolRidEntity.Field.MemoTitle09, SchoolRidEntity.Field.MemoTitle10,
                                SchoolRidEntity.Field.MemoTitle11, SchoolRidEntity.Field.MemoTitle12, SchoolRidEntity.Field.MemoTitle13, SchoolRidEntity.Field.MemoTitle14, SchoolRidEntity.Field.MemoTitle15,
                                SchoolRidEntity.Field.MemoTitle16, SchoolRidEntity.Field.MemoTitle17, SchoolRidEntity.Field.MemoTitle18, SchoolRidEntity.Field.MemoTitle19, SchoolRidEntity.Field.MemoTitle20,
                                SchoolRidEntity.Field.MemoTitle21
                            };
                            for (int idx = 0; idx < fields.Length; idx++)
                            {
                                string value = idx < memoTitles.Length ? memoTitles[idx] : null;
                                fieldValues.Add(fields[idx], value);
                            }
                        }
                        else
                        {
                            #region [MDY:20160215] 沒指定就清空
                            string[] fields = new string[SchoolRidEntity.MemoTitleMaxCount] {
                                SchoolRidEntity.Field.MemoTitle01, SchoolRidEntity.Field.MemoTitle02, SchoolRidEntity.Field.MemoTitle03, SchoolRidEntity.Field.MemoTitle04, SchoolRidEntity.Field.MemoTitle05,
                                SchoolRidEntity.Field.MemoTitle06, SchoolRidEntity.Field.MemoTitle07, SchoolRidEntity.Field.MemoTitle08, SchoolRidEntity.Field.MemoTitle09, SchoolRidEntity.Field.MemoTitle10,
                                SchoolRidEntity.Field.MemoTitle11, SchoolRidEntity.Field.MemoTitle12, SchoolRidEntity.Field.MemoTitle13, SchoolRidEntity.Field.MemoTitle14, SchoolRidEntity.Field.MemoTitle15,
                                SchoolRidEntity.Field.MemoTitle16, SchoolRidEntity.Field.MemoTitle17, SchoolRidEntity.Field.MemoTitle18, SchoolRidEntity.Field.MemoTitle19, SchoolRidEntity.Field.MemoTitle20,
                                SchoolRidEntity.Field.MemoTitle21
                            };
                            for (int idx = 0; idx < fields.Length; idx++)
                            {
                                fieldValues.Add(fields[idx], String.Empty);
                            }
                            #endregion
                        }

                        #region [MDY:202203XX] 2022擴充案 收入科目英文名稱
                        {
                            //比照中文名稱，以上傳的資料為準
                            string[] fields = new string[] {
                                SchoolRidEntity.Field.ReceiveItemE01, SchoolRidEntity.Field.ReceiveItemE02, SchoolRidEntity.Field.ReceiveItemE03, SchoolRidEntity.Field.ReceiveItemE04, SchoolRidEntity.Field.ReceiveItemE05,
                                SchoolRidEntity.Field.ReceiveItemE06, SchoolRidEntity.Field.ReceiveItemE07, SchoolRidEntity.Field.ReceiveItemE08, SchoolRidEntity.Field.ReceiveItemE09, SchoolRidEntity.Field.ReceiveItemE10,
                                SchoolRidEntity.Field.ReceiveItemE11, SchoolRidEntity.Field.ReceiveItemE12, SchoolRidEntity.Field.ReceiveItemE13, SchoolRidEntity.Field.ReceiveItemE14, SchoolRidEntity.Field.ReceiveItemE15,
                                SchoolRidEntity.Field.ReceiveItemE16, SchoolRidEntity.Field.ReceiveItemE17, SchoolRidEntity.Field.ReceiveItemE18, SchoolRidEntity.Field.ReceiveItemE19, SchoolRidEntity.Field.ReceiveItemE20,
                                SchoolRidEntity.Field.ReceiveItemE21, SchoolRidEntity.Field.ReceiveItemE22, SchoolRidEntity.Field.ReceiveItemE23, SchoolRidEntity.Field.ReceiveItemE24, SchoolRidEntity.Field.ReceiveItemE25,
                                SchoolRidEntity.Field.ReceiveItemE26, SchoolRidEntity.Field.ReceiveItemE27, SchoolRidEntity.Field.ReceiveItemE28, SchoolRidEntity.Field.ReceiveItemE29, SchoolRidEntity.Field.ReceiveItemE30,
                                SchoolRidEntity.Field.ReceiveItemE31, SchoolRidEntity.Field.ReceiveItemE32, SchoolRidEntity.Field.ReceiveItemE33, SchoolRidEntity.Field.ReceiveItemE34, SchoolRidEntity.Field.ReceiveItemE35,
                                SchoolRidEntity.Field.ReceiveItemE36, SchoolRidEntity.Field.ReceiveItemE37, SchoolRidEntity.Field.ReceiveItemE38, SchoolRidEntity.Field.ReceiveItemE39, SchoolRidEntity.Field.ReceiveItemE40
                            };
                            if (isEngEnabled)
                            {
                                for (int idx = 0; idx < fields.Length; idx++)
                                {
                                    string value = idx < receiveItemENames.Length ? receiveItemENames[idx] : null;
                                    fieldValues.Add(fields[idx], value);
                                }
                            }
                            else
                            {
                                //清空
                                for (int idx = 0; idx < fields.Length; idx++)
                                {
                                    fieldValues.Add(fields[idx], null);
                                }
                            }
                        }
                        #endregion

                        #region [MDY:202203XX] 2022擴充案 備註項目英文標題
                        {
                            //比照中文標題，以上傳的資料為準
                            string[] fields = new string[SchoolRidEntity.MemoTitleMaxCount] {
                                SchoolRidEntity.Field.MemoTitleE01, SchoolRidEntity.Field.MemoTitleE02, SchoolRidEntity.Field.MemoTitleE03, SchoolRidEntity.Field.MemoTitleE04, SchoolRidEntity.Field.MemoTitleE05,
                                SchoolRidEntity.Field.MemoTitleE06, SchoolRidEntity.Field.MemoTitleE07, SchoolRidEntity.Field.MemoTitleE08, SchoolRidEntity.Field.MemoTitleE09, SchoolRidEntity.Field.MemoTitleE10,
                                SchoolRidEntity.Field.MemoTitleE11, SchoolRidEntity.Field.MemoTitleE12, SchoolRidEntity.Field.MemoTitleE13, SchoolRidEntity.Field.MemoTitleE14, SchoolRidEntity.Field.MemoTitleE15,
                                SchoolRidEntity.Field.MemoTitleE16, SchoolRidEntity.Field.MemoTitleE17, SchoolRidEntity.Field.MemoTitleE18, SchoolRidEntity.Field.MemoTitleE19, SchoolRidEntity.Field.MemoTitleE20,
                                SchoolRidEntity.Field.MemoTitleE21
                            };
                            if (isEngEnabled)
                            {
                                for (int idx = 0; idx < fields.Length; idx++)
                                {
                                    string value = idx < memoETitles.Length ? memoETitles[idx] : null;
                                    fieldValues.Add(fields[idx], value);
                                }
                            }
                            else
                            {
                                //清空
                                for (int idx = 0; idx < fields.Length; idx++)
                                {
                                    fieldValues.Add(fields[idx], null);
                                }
                            }
                        }
                        #endregion

                        if (fieldValues.Count > 0)
                        {
                            fieldValues.Add(SchoolRidEntity.Field.UpdateTime, DateTime.Now);
                            fieldValues.Add(SchoolRidEntity.Field.UpdateWho, owner);

                            int count = 0;
                            Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, schoolRid.ReceiveType)
                                .And(SchoolRidEntity.Field.YearId, schoolRid.YearId)
                                .And(SchoolRidEntity.Field.TermId, schoolRid.TermId)
                                .And(SchoolRidEntity.Field.DepId, schoolRid.DepId)
                                .And(SchoolRidEntity.Field.ReceiveId, schoolRid.ReceiveId)
                                .And(SchoolRidEntity.Field.ReceiveStatus, schoolRid.ReceiveStatus);
                            _Factory.UpdateFields<SchoolRidEntity>(fieldValues, where, out count);
                        }
                        #endregion
                    }
                }
                #endregion

                return importResult;
            }
            #endregion
        }
        #endregion

        #region Import BUB File(上傳學分費退費資料)
        /// <summary>
        /// 匯入 BUB (上傳學分費退費資料) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUBJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUB)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;

            #region [Old] 土銀不使用原部別，depId 固定為空字串
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
            //    || String.IsNullOrEmpty(depId) || String.IsNullOrEmpty(receiveId))
            //{
            //    logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼、學期代碼、部別代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
            //    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId == null || String.IsNullOrEmpty(receiveId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            string owner = null;
            string mappingId = null;
            string sheetName = null;
            string fileType = null;
            int cancel = 0;
            int seriroNo = 0;
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pFileName = null;
                string pCancel = null;
                string pSeriroNo = null;
                isParamOK = JobcubeEntity.ParseBUBParameter(job.Jparam, out owner, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out mappingId, out pFileName, out sheetName, out pCancel, out pSeriroNo);
                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (String.IsNullOrEmpty(mappingId)
                    || (fileType != "xls" && fileType != "xlsx" && fileType != "txt" && fileType != "ods")
                    || String.IsNullOrEmpty(pCancel) || !Int32.TryParse(pCancel, out cancel) || cancel < 1
                    || String.IsNullOrEmpty(pSeriroNo) || !Int32.TryParse(pSeriroNo, out seriroNo) || seriroNo < 1)
                {
                    logmsg = "批次處理序列缺少對照表代碼、上傳檔案序號或批次號碼的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                {
                    fileContent = encoding.GetBytes(textContent);
                }
                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取對照表
            MappingrtTxtEntity txtMapping = null;
            MappingrtXlsmdbEntity xlsMapping = null;
            {
                if (fileType == "txt")
                {
                    Expression where = new Expression(MappingrtTxtEntity.Field.MappingId, mappingId)
                        .And(MappingrtTxtEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingrtTxtEntity>(where, null, out txtMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取文字格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (txtMapping == null)
                    {
                        logmsg = String.Format("查無 代碼 {0}、商家代號 {1} 的文字格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    Expression where = new Expression(MappingrtXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingrtXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingrtXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Excel 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Excel 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                }
                else if (fileType == "ods")
                {
                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    Expression where = new Expression(MappingrtXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingrtXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingrtXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Calc 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Calc 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();
            {
                string errmsg = null;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                ConvertFileHelper helper = new ConvertFileHelper();
                if (fileType == "txt")
                {
                    #region Txt 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        using (StreamReader reader = new StreamReader(ms, encoding))
                        {
                            isOK = helper.Txt2DataTable(reader, txtMapping.GetMapFields(), isBatch, true, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    #region Xls | Xlsx 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        if (fileType == "xls")
                        {
                            isOK = helper.Xls2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else
                        {
                            isOK = helper.Xlsx2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "ods")
                {
                    #region Ods 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Ods2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else
                {
                    #region 不支援
                    {
                        logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
                #endregion

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }
            }
            #endregion

            #region 判斷退費處理種類
            string processType = null;
            {
                if (fieldNames.Contains(MappingrtXlsmdbEntity.Field.RtCredit))
                {
                    processType = "1";  //有上傳 要退的學分數 資料
                }
                else if (fieldNames.Contains(MappingrtXlsmdbEntity.Field.ReCredit))
                {
                    processType = "2";  //有上傳 實際所修的學分數 資料
                }
                else if (fieldNames.Contains(MappingrtXlsmdbEntity.Field.RtAmount))
                {
                    processType = "3";  //有上傳 要退金額 資料
                }
                else
                {
                    processType = "4";
                }
            }
            #endregion

            #region 取整批退費所屬收入科目代碼 (第幾項)
            int returnItem = 0;
            if (processType == "3")
            {
                SchoolRidEntity schoolRid = null;
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRidEntity.Field.YearId, yearId)
                    .And(SchoolRidEntity.Field.TermId, termId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, receiveId);
                Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (schoolRid == null || String.IsNullOrEmpty(schoolRid.ReturnItem) || int.TryParse(schoolRid.ReturnItem, out returnItem) || returnItem < 1 || returnItem > 30)
                {
                    logmsg = String.Format("查無相關的代收費用別設定資料或未設定整批退費所屬收入科目", mappingId, receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region
            {
                successCount = 0;
                DateTime now = DateTime.Now;
                StringBuilder log = new StringBuilder();
                Result result = new Result(true);
                using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
                {
                    int rowNo = 0;
                    Result importResult = new Result(true);
                    foreach (DataRow row in table.Rows)
                    {
                        string stuId = row.IsNull(MappingrtXlsmdbEntity.Field.StuId) ? null : row[MappingrtXlsmdbEntity.Field.StuId].ToString().Trim();
                        if (String.IsNullOrEmpty(stuId))
                        {
                            log.AppendFormat("第 {0} 筆資料缺少學號資料", rowNo).AppendLine();
                            continue;
                        }

                        int oldSeq = 0; //系統不處理舊學雜費轉置的資料，所以固定為 0

                        switch (processType)
                        {
                            case "1":   //以要退的學分數處理
                                importResult = this.ImportBUBByProcess1(receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, row);
                                break;
                            case "2":   //以實際所修的學分數處理
                                importResult = this.ImportBUBByProcess2(receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, row);
                                break;
                            case "3":   //以要退金額處理
                                importResult = this.ImportBUBByProcess3(receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, returnItem, row);
                                break;
                            case "4":   //以退費項目金額處理
                                importResult = this.ImportBUBByProcess4(receiveType, yearId, termId, depId, receiveId, stuId, oldSeq, row);
                                break;
                        }
                        if (importResult.IsSuccess)
                        {
                            tsFactory.Commit();
                            successCount++;
                            //log.AppendFormat("第 {0} 筆資料新增成功", rowNo).AppendLine();
                        }
                        else
                        {
                            tsFactory.Rollback();
                            log.AppendFormat("第 {0} 筆資料處理失敗，錯誤訊息：{1}", rowNo, importResult.Message).AppendLine();
                            if (isBatch)
                            {
                                result = new Result(false, String.Format("第 {0} 筆資料新增失敗，錯誤訊息：：{1}", rowNo, result.Message), result.Code, result.Exception);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (!result.IsSuccess && isBatch)
                        {
                            break;
                        }
                    }
                }

                logmsg = log.ToString();
                if (result.IsSuccess && successCount == 0)
                {
                    result = new Result(false, "無資料被匯入成功", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 以要退的學分數處理
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private Result ImportBUBByProcess1(string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, DataRow row)
        {
            DataColumnCollection columns = row.Table.Columns;

            #region StudentReceiveEntity
            StudentReceiveEntity studentReceive = null;
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.StuId, stuId)
                    .And(StudentReceiveEntity.Field.OldSeq, oldSeq);
                Result result = _Factory.SelectFirst<StudentReceiveEntity>(where, null, out studentReceive);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (studentReceive == null || studentReceive.ReceiveAmount == null)
                {
                    return new Result(false, "查無對應的學生繳費資料或還未計算應繳金額，無法計算要退的學分數的退費金額", ErrorCode.D_DATA_NOT_FOUND, null);
                }

                #region [MDY:20190906] (2019擴充案) 修正 (已入帳才能做退費)
                if (String.IsNullOrEmpty(studentReceive.AccountDate))
                {
                    return new Result(false, "該筆學生繳費資料未入帳，不能處理退費", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                #endregion
            }
            #endregion

            #region CreditStandardEntity
            CreditStandardEntity creditStandard = null;
            {
                Expression where = new Expression(CreditStandardEntity.Field.ReceiveType, receiveType)
                    .And(CreditStandardEntity.Field.YearId, yearId)
                    .And(CreditStandardEntity.Field.TermId, termId)
                    .And(CreditStandardEntity.Field.DepId, depId)
                    .And(CreditStandardEntity.Field.ReceiveId, receiveId)
                    .And(CreditStandardEntity.Field.CollegeId, studentReceive.CollegeId);
                Result result = _Factory.SelectFirst<CreditStandardEntity>(where, null, out creditStandard);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (creditStandard == null || creditStandard.CreditPrice == null)
                {
                    return new Result(false, "查無對應的學分費收費標準資料或未設定學分費單價，無法計算要退的學分數的退費金額", ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 計算退費金額並檢查限制
            decimal returnAmount = 0M;
            decimal returnCredit = 0M;
            string rtCredit = row.IsNull(MappingrtXlsmdbEntity.Field.RtCredit) ? null : row[MappingrtXlsmdbEntity.Field.RtCredit].ToString().Trim();
            if (String.IsNullOrEmpty(rtCredit) || !System.Decimal.TryParse(rtCredit, out returnCredit) || returnCredit <= 0)
            {
                return new Result(false, "該筆資料的要退的學分數不是有效的數值", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            returnAmount = returnCredit * creditStandard.CreditPrice.Value;
            if (studentReceive.ReceiveAmount.Value < returnAmount)
            {
                return new Result(false, "該筆資料計算後的退費金額大於應繳金額，不匯入資料庫", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            #endregion

            #region 儲存 StudentReturnEntity
            {
                StudentReturnEntity studentReturn = null;
                Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, receiveType)
                    .And(StudentReturnEntity.Field.YearId, yearId)
                    .And(StudentReturnEntity.Field.TermId, termId)
                    .And(StudentReturnEntity.Field.DepId, depId)
                    .And(StudentReturnEntity.Field.ReceiveId, receiveId)
                    .And(StudentReturnEntity.Field.StuId, stuId)
                    .And(StudentReturnEntity.Field.OldSeq, oldSeq)
                    .And(StudentReturnEntity.Field.ReturnRemark, "n");
                Result result = _Factory.SelectFirst<StudentReturnEntity>(where, null, out studentReturn);
                if (!result.IsSuccess)
                {
                    return result;
                }

                #region 退費方式標準代碼
                string rtName = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtName))
                {
                    #region [MDY:20190906] (2019擴充案) 修正
                    rtName = row.IsNull(MappingrtXlsmdbEntity.Field.RtName) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtName].ToString().Trim();
                    #endregion
                }
                #endregion

                #region 匯款銀行代碼
                string rtBankid = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtBankId))
                {
                    rtBankid = row.IsNull(MappingrtXlsmdbEntity.Field.RtBankId) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtBankId].ToString().Trim();
                }
                #endregion

                #region 匯款帳號
                string rtAccount = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtAccount))
                {
                    rtAccount = row.IsNull(MappingrtXlsmdbEntity.Field.RtAccount) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtAccount].ToString().Trim();
                }
                #endregion

                if (studentReturn == null)
                {
                    studentReturn = new StudentReturnEntity();
                    studentReturn.ReceiveType = receiveType;
                    studentReturn.YearId = yearId;
                    studentReturn.TermId = termId;
                    studentReturn.DepId = depId;
                    studentReturn.ReceiveId = receiveId;
                    studentReturn.StuId = stuId;
                    studentReturn.OldSeq = 0;   //系統新增的資料，此欄位固定為 0

                    #region [MDY:20190906] (2019擴充案) 修正
                    studentReturn.SrNo = String.Empty;
                    #endregion

                    studentReturn.CancelNo = studentReceive.CancelNo;
                    studentReturn.ReturnCredit = returnCredit;
                    studentReturn.ReturnAmount = returnAmount;
                    studentReturn.ReturnRemark = "n";

                    //TODO: ???
                    studentReturn.ReturnId = rtName;
                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        studentReturn.RemitData = rtBankid + rtAccount;
                    }
                    else
                    {
                        studentReturn.RemitData = String.Empty;
                    }
                    //TODO: ???
                    studentReturn.ReturnWay = rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2");
                    studentReturn.ReturnDate = Common.GetTWDate7();
                    studentReturn.ReSeq = "1";

                    int count = 0;
                    result = _Factory.Insert(studentReturn, out count);
                    return result;
                }
                else
                {
                    KeyValueList fieldValues = new KeyValueList();
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnCredit, returnCredit);
                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnId, rtName);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnDate, Common.GetTWDate7());

                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, rtBankid + rtAccount);
                    }
                    else
                    {
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, String.Empty);
                    }
                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnWay, rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2"));

                    int count = 0;
                    result = _Factory.UpdateFields<StudentReturnEntity>(fieldValues, where, out count);
                    return result;
                }
            }
            #endregion
        }

        /// <summary>
        /// 以實際所修的學分數處理
        /// </summary>
        /// <returns></returns>
        private Result ImportBUBByProcess2(string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, DataRow row)
        {
            DataColumnCollection columns = row.Table.Columns;

            #region StudentReceiveEntity
            StudentReceiveEntity studentReceive = null;
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.StuId, stuId)
                    .And(StudentReceiveEntity.Field.OldSeq, oldSeq);
                Result result = _Factory.SelectFirst<StudentReceiveEntity>(where, null, out studentReceive);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (studentReceive == null || studentReceive.ReceiveAmount == null)
                {
                    return new Result(false, "查無對應的學生繳費資料或還未計算應繳金額，無法計算要退的學分數的退費金額", ErrorCode.D_DATA_NOT_FOUND, null);
                }

                #region [MDY:20190906] (2019擴充案) 修正 (已入帳才能做退費)
                if (String.IsNullOrEmpty(studentReceive.AccountDate))
                {
                    return new Result(false, "該筆學生繳費資料未入帳，不能處理退費", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                #endregion
            }
            #endregion

            #region CreditStandardEntity
            CreditStandardEntity creditStandard = null;
            {
                Expression where = new Expression(CreditStandardEntity.Field.ReceiveType, receiveType)
                    .And(CreditStandardEntity.Field.YearId, yearId)
                    .And(CreditStandardEntity.Field.TermId, termId)
                    .And(CreditStandardEntity.Field.DepId, depId)
                    .And(CreditStandardEntity.Field.ReceiveId, receiveId)
                    .And(CreditStandardEntity.Field.CollegeId, studentReceive.CollegeId);
                Result result = _Factory.SelectFirst<CreditStandardEntity>(where, null, out creditStandard);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (creditStandard == null || creditStandard.CreditPrice == null)
                {
                    return new Result(false, "查無對應的學分費收費標準資料或未設定學分費單價，無法計算要退的學分數的退費金額", ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 計算退費金額並檢查限制
            decimal returnAmount = 0M;
            decimal realCredit = 0M;
            string reCredit = row.IsNull(MappingrtXlsmdbEntity.Field.ReCredit) ? null : row[MappingrtXlsmdbEntity.Field.ReCredit].ToString().Trim();
            if (String.IsNullOrEmpty(reCredit) || !System.Decimal.TryParse(reCredit, out realCredit) || realCredit <= 0)
            {
                return new Result(false, "該筆資料的實際所修的學分數不是有效的數值", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            //TODO: ???  studentReceive.StuCredit * ??
            returnAmount = realCredit * creditStandard.CreditPrice.Value;
            if (studentReceive.ReceiveAmount.Value < returnAmount)
            {
                return new Result(false, "該筆資料計算後的退費金額大於應繳金額，不匯入資料庫", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            #endregion

            #region 儲存 StudentReturnEntity
            {
                StudentReturnEntity studentReturn = null;
                Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, receiveType)
                    .And(StudentReturnEntity.Field.YearId, yearId)
                    .And(StudentReturnEntity.Field.TermId, termId)
                    .And(StudentReturnEntity.Field.DepId, depId)
                    .And(StudentReturnEntity.Field.ReceiveId, receiveId)
                    .And(StudentReturnEntity.Field.StuId, stuId)
                    .And(StudentReturnEntity.Field.OldSeq, oldSeq)
                    .And(StudentReturnEntity.Field.ReturnRemark, "n");
                Result result = _Factory.SelectFirst<StudentReturnEntity>(where, null, out studentReturn);
                if (!result.IsSuccess)
                {
                    return result;
                }

                #region 退費方式標準代碼
                string rtName = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtName))
                {
                    #region [MDY:20190906] (2019擴充案) 修正
                    rtName = row.IsNull(MappingrtXlsmdbEntity.Field.RtName) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtName].ToString().Trim();
                    #endregion
                }
                #endregion

                #region 匯款銀行代碼
                string rtBankid = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtBankId))
                {
                    rtBankid = row.IsNull(MappingrtXlsmdbEntity.Field.RtBankId) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtBankId].ToString().Trim();
                }
                #endregion

                #region 匯款帳號
                string rtAccount = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtAccount))
                {
                    rtAccount = row.IsNull(MappingrtXlsmdbEntity.Field.RtAccount) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtAccount].ToString().Trim();
                }
                #endregion

                if (studentReturn == null)
                {
                    studentReturn = new StudentReturnEntity();
                    studentReturn.ReceiveType = receiveType;
                    studentReturn.YearId = yearId;
                    studentReturn.TermId = termId;
                    studentReturn.DepId = depId;
                    studentReturn.ReceiveId = receiveId;
                    studentReturn.StuId = stuId;
                    studentReturn.OldSeq = 0;   //系統新增的資料，此欄位固定為 0

                    #region [MDY:20190906] (2019擴充案) 修正
                    studentReturn.SrNo = String.Empty;
                    #endregion

                    studentReturn.CancelNo = studentReceive.CancelNo;
                    studentReturn.RealCredit = realCredit;
                    studentReturn.ReturnAmount = returnAmount;
                    studentReturn.ReturnRemark = "n";

                    //TODO: ???
                    studentReturn.ReturnId = rtName;
                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        studentReturn.RemitData = rtBankid + rtAccount;
                    }
                    else
                    {
                        studentReturn.RemitData = String.Empty;
                    }
                    //TODO: ???
                    studentReturn.ReturnWay = rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2");
                    studentReturn.ReturnDate = Common.GetTWDate7();
                    studentReturn.ReSeq = "1";

                    int count = 0;
                    result = _Factory.Insert(studentReturn, out count);
                    return result;
                }
                else
                {
                    KeyValueList fieldValues = new KeyValueList();
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);
                    fieldValues.Add(StudentReturnEntity.Field.RealCredit, realCredit);
                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnId, rtName);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnDate, Common.GetTWDate7());

                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, rtBankid + rtAccount);
                    }
                    else
                    {
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, String.Empty);
                    }
                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnWay, rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2"));

                    int count = 0;
                    result = _Factory.UpdateFields<StudentReturnEntity>(fieldValues, where, out count);
                    return result;
                }
            }
            #endregion
        }

        /// <summary>
        /// 以要退金額處理
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="returnItem"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private Result ImportBUBByProcess3(string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, int returnItem, DataRow row)
        {
            DataColumnCollection columns = row.Table.Columns;

            #region StudentReceiveEntity
            StudentReceiveEntity studentReceive = null;
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.StuId, stuId)
                    .And(StudentReceiveEntity.Field.OldSeq, oldSeq);
                Result result = _Factory.SelectFirst<StudentReceiveEntity>(where, null, out studentReceive);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (studentReceive == null || studentReceive.ReceiveAmount == null)
                {
                    return new Result(false, "查無對應的學生繳費資料或還未計算應繳金額，無法計算要退的學分數的退費金額", ErrorCode.D_DATA_NOT_FOUND, null);
                }

                #region [MDY:20190906] (2019擴充案) 修正 (已入帳才能做退費)
                if (String.IsNullOrEmpty(studentReceive.AccountDate))
                {
                    return new Result(false, "該筆學生繳費資料未入帳，不能處理退費", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                #endregion
            }
            #endregion

            #region 要退金額
            decimal rtAmount = 0M;
            if (row.IsNull(MappingrtXlsmdbEntity.Field.RtAmount) || !System.Decimal.TryParse(row[MappingrtXlsmdbEntity.Field.RtAmount].ToString().Trim(), out rtAmount))
            {
                return new Result(false, "要退金額不是有效的數值", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            #endregion

            #region 檢查各項退費金額不可大於收入科目金額，並計算退費金額
            #region [MDY:20190906] (2019擴充案) 修正
            decimal[] rtAmounts = new decimal[40];
            #endregion
            decimal returnAmount = 0M;
            {
                #region [MDY:20190906] (2019擴充案) 修正
                decimal?[] receiveAmounts = new decimal?[] {
                    studentReceive.Receive01 ?? 0M, studentReceive.Receive02 ?? 0M, studentReceive.Receive03 ?? 0M, studentReceive.Receive04 ?? 0M, studentReceive.Receive05 ?? 0M,
                    studentReceive.Receive06 ?? 0M, studentReceive.Receive07 ?? 0M, studentReceive.Receive08 ?? 0M, studentReceive.Receive09 ?? 0M, studentReceive.Receive10 ?? 0M,
                    studentReceive.Receive11 ?? 0M, studentReceive.Receive12 ?? 0M, studentReceive.Receive13 ?? 0M, studentReceive.Receive14 ?? 0M, studentReceive.Receive15 ?? 0M,
                    studentReceive.Receive16 ?? 0M, studentReceive.Receive17 ?? 0M, studentReceive.Receive18 ?? 0M, studentReceive.Receive19 ?? 0M, studentReceive.Receive20 ?? 0M,
                    studentReceive.Receive21 ?? 0M, studentReceive.Receive22 ?? 0M, studentReceive.Receive23 ?? 0M, studentReceive.Receive24 ?? 0M, studentReceive.Receive25 ?? 0M,
                    studentReceive.Receive26 ?? 0M, studentReceive.Receive27 ?? 0M, studentReceive.Receive28 ?? 0M, studentReceive.Receive29 ?? 0M, studentReceive.Receive30 ?? 0M,
                    studentReceive.Receive31 ?? 0M, studentReceive.Receive32 ?? 0M, studentReceive.Receive33 ?? 0M, studentReceive.Receive34 ?? 0M, studentReceive.Receive35 ?? 0M,
                    studentReceive.Receive36 ?? 0M, studentReceive.Receive37 ?? 0M, studentReceive.Receive38 ?? 0M, studentReceive.Receive39 ?? 0M, studentReceive.Receive40 ?? 0M
                };

                string[] rtFields = new string[40] {
                    MappingrtXlsmdbEntity.Field.Rt01, MappingrtXlsmdbEntity.Field.Rt02, MappingrtXlsmdbEntity.Field.Rt03, MappingrtXlsmdbEntity.Field.Rt04, MappingrtXlsmdbEntity.Field.Rt05,
                    MappingrtXlsmdbEntity.Field.Rt06, MappingrtXlsmdbEntity.Field.Rt07, MappingrtXlsmdbEntity.Field.Rt08, MappingrtXlsmdbEntity.Field.Rt09, MappingrtXlsmdbEntity.Field.Rt10,
                    MappingrtXlsmdbEntity.Field.Rt11, MappingrtXlsmdbEntity.Field.Rt12, MappingrtXlsmdbEntity.Field.Rt13, MappingrtXlsmdbEntity.Field.Rt14, MappingrtXlsmdbEntity.Field.Rt15,
                    MappingrtXlsmdbEntity.Field.Rt16, MappingrtXlsmdbEntity.Field.Rt17, MappingrtXlsmdbEntity.Field.Rt18, MappingrtXlsmdbEntity.Field.Rt19, MappingrtXlsmdbEntity.Field.Rt20,
                    MappingrtXlsmdbEntity.Field.Rt21, MappingrtXlsmdbEntity.Field.Rt22, MappingrtXlsmdbEntity.Field.Rt23, MappingrtXlsmdbEntity.Field.Rt24, MappingrtXlsmdbEntity.Field.Rt25,
                    MappingrtXlsmdbEntity.Field.Rt26, MappingrtXlsmdbEntity.Field.Rt27, MappingrtXlsmdbEntity.Field.Rt28, MappingrtXlsmdbEntity.Field.Rt29, MappingrtXlsmdbEntity.Field.Rt30,
                    MappingrtXlsmdbEntity.Field.Rt31, MappingrtXlsmdbEntity.Field.Rt32, MappingrtXlsmdbEntity.Field.Rt33, MappingrtXlsmdbEntity.Field.Rt34, MappingrtXlsmdbEntity.Field.Rt35,
                    MappingrtXlsmdbEntity.Field.Rt36, MappingrtXlsmdbEntity.Field.Rt37, MappingrtXlsmdbEntity.Field.Rt38, MappingrtXlsmdbEntity.Field.Rt39, MappingrtXlsmdbEntity.Field.Rt40
                };
                #endregion

                for (int idx = 0; idx < rtFields.Length; idx++)
                {
                    string rtField = rtFields[idx];
                    decimal amount = 0M;
                    if (columns.Contains(rtField) && !row.IsNull(rtField) && System.Decimal.TryParse(row[rtField].ToString().Trim(), out amount))
                    {
                        rtAmounts[idx] = amount;
                    }
                    else
                    {
                        rtAmounts[idx] = 0M;
                    }
                }

                int idxReturnItem  = returnItem - 1;
                rtAmounts[idxReturnItem] = rtAmount;

                for (int idx = 0; idx < receiveAmounts.Length; idx++)
                {
                    if (rtAmounts[idx] > receiveAmounts[idx])
                    {
                        if (idx == idxReturnItem)
                        {
                            return new Result(false, "要退金額大於該項的應繳金額，此筆資料不匯入檔案", CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                        else
                        {
                            return new Result(false, String.Format("第 {0} 項退費金額大於該項的應繳金額，此筆資料不匯入檔案", idx + 1), CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                    }
                    returnAmount += rtAmounts[idx];
                }
            }
            #endregion

            #region 儲存 StudentReturnEntity
            {
                StudentReturnEntity studentReturn = null;
                Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, receiveType)
                    .And(StudentReturnEntity.Field.YearId, yearId)
                    .And(StudentReturnEntity.Field.TermId, termId)
                    .And(StudentReturnEntity.Field.DepId, depId)
                    .And(StudentReturnEntity.Field.ReceiveId, receiveId)
                    .And(StudentReturnEntity.Field.StuId, stuId)
                    .And(StudentReturnEntity.Field.OldSeq, oldSeq)
                    .And(StudentReturnEntity.Field.ReturnRemark, "n");
                Result result = _Factory.SelectFirst<StudentReturnEntity>(where, null, out studentReturn);
                if (!result.IsSuccess)
                {
                    return result;
                }

                #region 退費方式標準代碼
                string rtName = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtName))
                {
                    #region [MDY:20190906] (2019擴充案) 修正
                    rtName = row.IsNull(MappingrtXlsmdbEntity.Field.RtName) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtName].ToString().Trim();
                    #endregion
                }
                #endregion

                #region 匯款銀行代碼
                string rtBankid = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtBankId))
                {
                    rtBankid = row.IsNull(MappingrtXlsmdbEntity.Field.RtBankId) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtBankId].ToString().Trim();
                }
                #endregion

                #region 匯款帳號
                string rtAccount = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtAccount))
                {
                    rtAccount = row.IsNull(MappingrtXlsmdbEntity.Field.RtAccount) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtAccount].ToString().Trim();
                }
                #endregion

                if (studentReturn == null)
                {
                    studentReturn = new StudentReturnEntity();
                    studentReturn.ReceiveType = receiveType;
                    studentReturn.YearId = yearId;
                    studentReturn.TermId = termId;
                    studentReturn.DepId = depId;
                    studentReturn.ReceiveId = receiveId;
                    studentReturn.StuId = stuId;
                    studentReturn.OldSeq = 0;   //系統新增的資料，此欄位固定為 0

                    #region [MDY:20190906] (2019擴充案) 修正
                    studentReturn.SrNo = String.Empty;
                    #endregion

                    studentReturn.CancelNo = studentReceive.CancelNo;
                    studentReturn.ReturnAmount = returnAmount;
                    studentReturn.ReturnRemark = "n";

                    #region [MDY:20190906] (2019擴充案) 修正
                    studentReturn.Return01 = rtAmounts[0];
                    studentReturn.Return02 = rtAmounts[1];
                    studentReturn.Return03 = rtAmounts[2];
                    studentReturn.Return04 = rtAmounts[3];
                    studentReturn.Return05 = rtAmounts[4];
                    studentReturn.Return06 = rtAmounts[5];
                    studentReturn.Return07 = rtAmounts[6];
                    studentReturn.Return08 = rtAmounts[7];
                    studentReturn.Return09 = rtAmounts[8];
                    studentReturn.Return10 = rtAmounts[9];

                    studentReturn.Return11 = rtAmounts[10];
                    studentReturn.Return12 = rtAmounts[11];
                    studentReturn.Return13 = rtAmounts[12];
                    studentReturn.Return14 = rtAmounts[13];
                    studentReturn.Return15 = rtAmounts[14];
                    studentReturn.Return16 = rtAmounts[15];
                    studentReturn.Return17 = rtAmounts[16];
                    studentReturn.Return18 = rtAmounts[17];
                    studentReturn.Return19 = rtAmounts[18];
                    studentReturn.Return20 = rtAmounts[19];

                    studentReturn.Return21 = rtAmounts[20];
                    studentReturn.Return22 = rtAmounts[21];
                    studentReturn.Return23 = rtAmounts[22];
                    studentReturn.Return24 = rtAmounts[23];
                    studentReturn.Return25 = rtAmounts[24];
                    studentReturn.Return26 = rtAmounts[25];
                    studentReturn.Return27 = rtAmounts[26];
                    studentReturn.Return28 = rtAmounts[27];
                    studentReturn.Return29 = rtAmounts[28];
                    studentReturn.Return30 = rtAmounts[29];

                    studentReturn.Return31 = rtAmounts[30];
                    studentReturn.Return32 = rtAmounts[31];
                    studentReturn.Return33 = rtAmounts[32];
                    studentReturn.Return34 = rtAmounts[33];
                    studentReturn.Return35 = rtAmounts[34];
                    studentReturn.Return36 = rtAmounts[35];
                    studentReturn.Return37 = rtAmounts[36];
                    studentReturn.Return38 = rtAmounts[37];
                    studentReturn.Return39 = rtAmounts[38];
                    studentReturn.Return40 = rtAmounts[39];
                    #endregion

                    //TODO: ???
                    studentReturn.ReturnId = rtName;
                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        studentReturn.RemitData = rtBankid + rtAccount;
                    }
                    else
                    {
                        studentReturn.RemitData = String.Empty;
                    }
                    //TODO: ???
                    studentReturn.ReturnWay = rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2");
                    studentReturn.ReturnDate = Common.GetTWDate7();
                    studentReturn.ReSeq = "1";

                    int count = 0;
                    result = _Factory.Insert(studentReturn, out count);
                    return result;
                }
                else
                {
                    KeyValueList fieldValues = new KeyValueList();
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);

                    string[] fields = new string[] {
                            StudentReturnEntity.Field.Return01, StudentReturnEntity.Field.Return02, StudentReturnEntity.Field.Return03, StudentReturnEntity.Field.Return04, StudentReturnEntity.Field.Return05,
                            StudentReturnEntity.Field.Return06, StudentReturnEntity.Field.Return07, StudentReturnEntity.Field.Return08, StudentReturnEntity.Field.Return09, StudentReturnEntity.Field.Return10,
                            StudentReturnEntity.Field.Return11, StudentReturnEntity.Field.Return12, StudentReturnEntity.Field.Return13, StudentReturnEntity.Field.Return14, StudentReturnEntity.Field.Return15,
                            StudentReturnEntity.Field.Return16, StudentReturnEntity.Field.Return17, StudentReturnEntity.Field.Return18, StudentReturnEntity.Field.Return19, StudentReturnEntity.Field.Return20,
                            StudentReturnEntity.Field.Return21, StudentReturnEntity.Field.Return22, StudentReturnEntity.Field.Return23, StudentReturnEntity.Field.Return24, StudentReturnEntity.Field.Return25,
                            StudentReturnEntity.Field.Return26, StudentReturnEntity.Field.Return27, StudentReturnEntity.Field.Return28, StudentReturnEntity.Field.Return29, StudentReturnEntity.Field.Return30
                    };
                    for (int idx = 0; idx < rtAmounts.Length; idx++)
                    {
                        fieldValues.Add(fields[idx], rtAccount[idx]);
                    }

                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnId, rtName);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnDate, Common.GetTWDate7());

                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, rtBankid + rtAccount);
                    }
                    else
                    {
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, String.Empty);
                    }
                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnWay, rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2"));

                    int count = 0;
                    result = _Factory.UpdateFields<StudentReturnEntity>(fieldValues, where, out count);
                    return result;
                }
            }
            #endregion
        }

        /// <summary>
        /// 以退費項目金額處理
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="yearId"></param>
        /// <param name="termId"></param>
        /// <param name="depId"></param>
        /// <param name="receiveId"></param>
        /// <param name="stuId"></param>
        /// <param name="returnItem"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private Result ImportBUBByProcess4(string receiveType, string yearId, string termId, string depId, string receiveId, string stuId, int oldSeq, DataRow row)
        {
            DataColumnCollection columns = row.Table.Columns;

            #region StudentReceiveEntity
            StudentReceiveEntity studentReceive = null;
            {
                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType)
                    .And(StudentReceiveEntity.Field.YearId, yearId)
                    .And(StudentReceiveEntity.Field.TermId, termId)
                    .And(StudentReceiveEntity.Field.DepId, depId)
                    .And(StudentReceiveEntity.Field.ReceiveId, receiveId)
                    .And(StudentReceiveEntity.Field.StuId, stuId)
                    .And(StudentReceiveEntity.Field.OldSeq, oldSeq);
                Result result = _Factory.SelectFirst<StudentReceiveEntity>(where, null, out studentReceive);
                if (!result.IsSuccess)
                {
                    return result;
                }
                if (studentReceive == null || studentReceive.ReceiveAmount == null)
                {
                    return new Result(false, "查無對應的學生繳費資料或還未計算應繳金額，無法計算要退的學分數的退費金額", ErrorCode.D_DATA_NOT_FOUND, null);
                }

                #region [MDY:20190906] (2019擴充案) 修正 (已入帳才能做退費)
                if (String.IsNullOrEmpty(studentReceive.AccountDate))
                {
                    return new Result(false, "該筆學生繳費資料未入帳，不能處理退費", ErrorCode.D_DATA_NOT_FOUND, null);
                }
                #endregion
            }
            #endregion

            #region 檢查各項退費金額不可大於收入科目金額，並計算退費金額
            #region [MDY:20190906] (2019擴充案) 修正
            decimal[] rtAmounts = new decimal[40];
            #endregion
            decimal returnAmount = 0M;
            {
                #region [MDY:20190906] (2019擴充案) 修正
                decimal?[] receiveAmounts = new decimal?[] {
                    studentReceive.Receive01 ?? 0M, studentReceive.Receive02 ?? 0M, studentReceive.Receive03 ?? 0M, studentReceive.Receive04 ?? 0M, studentReceive.Receive05 ?? 0M,
                    studentReceive.Receive06 ?? 0M, studentReceive.Receive07 ?? 0M, studentReceive.Receive08 ?? 0M, studentReceive.Receive09 ?? 0M, studentReceive.Receive10 ?? 0M,
                    studentReceive.Receive11 ?? 0M, studentReceive.Receive12 ?? 0M, studentReceive.Receive13 ?? 0M, studentReceive.Receive14 ?? 0M, studentReceive.Receive15 ?? 0M,
                    studentReceive.Receive16 ?? 0M, studentReceive.Receive17 ?? 0M, studentReceive.Receive18 ?? 0M, studentReceive.Receive19 ?? 0M, studentReceive.Receive20 ?? 0M,
                    studentReceive.Receive21 ?? 0M, studentReceive.Receive22 ?? 0M, studentReceive.Receive23 ?? 0M, studentReceive.Receive24 ?? 0M, studentReceive.Receive25 ?? 0M,
                    studentReceive.Receive26 ?? 0M, studentReceive.Receive27 ?? 0M, studentReceive.Receive28 ?? 0M, studentReceive.Receive29 ?? 0M, studentReceive.Receive30 ?? 0M,
                    studentReceive.Receive31 ?? 0M, studentReceive.Receive32 ?? 0M, studentReceive.Receive33 ?? 0M, studentReceive.Receive34 ?? 0M, studentReceive.Receive35 ?? 0M,
                    studentReceive.Receive36 ?? 0M, studentReceive.Receive37 ?? 0M, studentReceive.Receive38 ?? 0M, studentReceive.Receive39 ?? 0M, studentReceive.Receive40 ?? 0M
                };

                string[] rtFields = new string[] {
                    MappingrtXlsmdbEntity.Field.Rt01, MappingrtXlsmdbEntity.Field.Rt02, MappingrtXlsmdbEntity.Field.Rt03, MappingrtXlsmdbEntity.Field.Rt04, MappingrtXlsmdbEntity.Field.Rt05,
                    MappingrtXlsmdbEntity.Field.Rt06, MappingrtXlsmdbEntity.Field.Rt07, MappingrtXlsmdbEntity.Field.Rt08, MappingrtXlsmdbEntity.Field.Rt09, MappingrtXlsmdbEntity.Field.Rt10,
                    MappingrtXlsmdbEntity.Field.Rt11, MappingrtXlsmdbEntity.Field.Rt12, MappingrtXlsmdbEntity.Field.Rt13, MappingrtXlsmdbEntity.Field.Rt14, MappingrtXlsmdbEntity.Field.Rt15,
                    MappingrtXlsmdbEntity.Field.Rt16, MappingrtXlsmdbEntity.Field.Rt17, MappingrtXlsmdbEntity.Field.Rt18, MappingrtXlsmdbEntity.Field.Rt19, MappingrtXlsmdbEntity.Field.Rt20,
                    MappingrtXlsmdbEntity.Field.Rt21, MappingrtXlsmdbEntity.Field.Rt22, MappingrtXlsmdbEntity.Field.Rt23, MappingrtXlsmdbEntity.Field.Rt24, MappingrtXlsmdbEntity.Field.Rt25,
                    MappingrtXlsmdbEntity.Field.Rt26, MappingrtXlsmdbEntity.Field.Rt27, MappingrtXlsmdbEntity.Field.Rt28, MappingrtXlsmdbEntity.Field.Rt29, MappingrtXlsmdbEntity.Field.Rt30,
                    MappingrtXlsmdbEntity.Field.Rt31, MappingrtXlsmdbEntity.Field.Rt32, MappingrtXlsmdbEntity.Field.Rt33, MappingrtXlsmdbEntity.Field.Rt34, MappingrtXlsmdbEntity.Field.Rt35,
                    MappingrtXlsmdbEntity.Field.Rt36, MappingrtXlsmdbEntity.Field.Rt37, MappingrtXlsmdbEntity.Field.Rt38, MappingrtXlsmdbEntity.Field.Rt39, MappingrtXlsmdbEntity.Field.Rt40
                };
                #endregion

                for (int idx = 0; idx < rtFields.Length; idx++)
                {
                    string rtField = rtFields[idx];
                    decimal amount = 0M;
                    if (columns.Contains(rtField) && !row.IsNull(rtField) && System.Decimal.TryParse(row[rtField].ToString().Trim(), out amount))
                    {
                        rtAmounts[idx] = amount;
                    }
                    else
                    {
                        rtAmounts[idx] = 0M;
                    }
                }

                for (int idx = 0; idx < receiveAmounts.Length; idx++)
                {
                    if (rtAmounts[idx] > receiveAmounts[idx])
                    {
                        return new Result(false, String.Format("第 {0} 項退費金額大於該項的應繳金額，此筆資料不匯入檔案", idx + 1), CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    returnAmount += rtAmounts[idx];
                }
            }
            #endregion

            #region 儲存 StudentReturnEntity
            {
                StudentReturnEntity studentReturn = null;
                Expression where = new Expression(StudentReturnEntity.Field.ReceiveType, receiveType)
                    .And(StudentReturnEntity.Field.YearId, yearId)
                    .And(StudentReturnEntity.Field.TermId, termId)
                    .And(StudentReturnEntity.Field.DepId, depId)
                    .And(StudentReturnEntity.Field.ReceiveId, receiveId)
                    .And(StudentReturnEntity.Field.StuId, stuId)
                    .And(StudentReturnEntity.Field.OldSeq, oldSeq)
                    .And(StudentReturnEntity.Field.ReturnRemark, "n");
                Result result = _Factory.SelectFirst<StudentReturnEntity>(where, null, out studentReturn);
                if (!result.IsSuccess)
                {
                    return result;
                }

                #region 退費方式標準代碼
                string rtName = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtName))
                {
                    #region [MDY:20190906] (2019擴充案) 修正
                    rtName = row.IsNull(MappingrtXlsmdbEntity.Field.RtName) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtName].ToString().Trim();
                    #endregion
                }
                #endregion

                #region 匯款銀行代碼
                string rtBankid = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtBankId))
                {
                    rtBankid = row.IsNull(MappingrtXlsmdbEntity.Field.RtBankId) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtBankId].ToString().Trim();
                }
                #endregion

                #region 匯款帳號
                string rtAccount = null;
                if (columns.Contains(MappingrtXlsmdbEntity.Field.RtAccount))
                {
                    rtAccount = row.IsNull(MappingrtXlsmdbEntity.Field.RtAccount) ? String.Empty : row[MappingrtXlsmdbEntity.Field.RtAccount].ToString().Trim();
                }
                #endregion

                if (studentReturn == null)
                {
                    studentReturn = new StudentReturnEntity();
                    studentReturn.ReceiveType = receiveType;
                    studentReturn.YearId = yearId;
                    studentReturn.TermId = termId;
                    studentReturn.DepId = depId;
                    studentReturn.ReceiveId = receiveId;
                    studentReturn.StuId = stuId;
                    studentReturn.OldSeq = 0;   //系統新增的資料，此欄位固定為 0

                    #region [MDY:20190906] (2019擴充案) 修正
                    studentReturn.SrNo = String.Empty;
                    #endregion

                    studentReturn.CancelNo = studentReceive.CancelNo;
                    studentReturn.ReturnAmount = returnAmount;
                    studentReturn.ReturnRemark = "n";

                    #region [MDY:20190906] (2019擴充案) 修正
                    studentReturn.Return01 = rtAmounts[0];
                    studentReturn.Return02 = rtAmounts[1];
                    studentReturn.Return03 = rtAmounts[2];
                    studentReturn.Return04 = rtAmounts[3];
                    studentReturn.Return05 = rtAmounts[4];
                    studentReturn.Return06 = rtAmounts[5];
                    studentReturn.Return07 = rtAmounts[6];
                    studentReturn.Return08 = rtAmounts[7];
                    studentReturn.Return09 = rtAmounts[8];
                    studentReturn.Return10 = rtAmounts[9];

                    studentReturn.Return11 = rtAmounts[10];
                    studentReturn.Return12 = rtAmounts[11];
                    studentReturn.Return13 = rtAmounts[12];
                    studentReturn.Return14 = rtAmounts[13];
                    studentReturn.Return15 = rtAmounts[14];
                    studentReturn.Return16 = rtAmounts[15];
                    studentReturn.Return17 = rtAmounts[16];
                    studentReturn.Return18 = rtAmounts[17];
                    studentReturn.Return19 = rtAmounts[18];
                    studentReturn.Return20 = rtAmounts[19];

                    studentReturn.Return21 = rtAmounts[20];
                    studentReturn.Return22 = rtAmounts[21];
                    studentReturn.Return23 = rtAmounts[22];
                    studentReturn.Return24 = rtAmounts[23];
                    studentReturn.Return25 = rtAmounts[24];
                    studentReturn.Return26 = rtAmounts[25];
                    studentReturn.Return27 = rtAmounts[26];
                    studentReturn.Return28 = rtAmounts[27];
                    studentReturn.Return29 = rtAmounts[28];
                    studentReturn.Return30 = rtAmounts[29];

                    studentReturn.Return31 = rtAmounts[30];
                    studentReturn.Return32 = rtAmounts[31];
                    studentReturn.Return33 = rtAmounts[32];
                    studentReturn.Return34 = rtAmounts[33];
                    studentReturn.Return35 = rtAmounts[34];
                    studentReturn.Return36 = rtAmounts[35];
                    studentReturn.Return37 = rtAmounts[36];
                    studentReturn.Return38 = rtAmounts[37];
                    studentReturn.Return39 = rtAmounts[38];
                    studentReturn.Return40 = rtAmounts[39];
                    #endregion

                    //TODO: ???
                    studentReturn.ReturnId = rtName;
                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        studentReturn.RemitData = rtBankid + rtAccount;
                    }
                    else
                    {
                        studentReturn.RemitData = String.Empty;
                    }
                    //TODO: ???
                    studentReturn.ReturnWay = rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2");
                    studentReturn.ReturnDate = Common.GetTWDate7();
                    studentReturn.ReSeq = "1";

                    int count = 0;
                    result = _Factory.Insert(studentReturn, out count);
                    return result;
                }
                else
                {
                    KeyValueList fieldValues = new KeyValueList();
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);

                    string[] fields = new string[] {
                            StudentReturnEntity.Field.Return01, StudentReturnEntity.Field.Return02, StudentReturnEntity.Field.Return03, StudentReturnEntity.Field.Return04, StudentReturnEntity.Field.Return05,
                            StudentReturnEntity.Field.Return06, StudentReturnEntity.Field.Return07, StudentReturnEntity.Field.Return08, StudentReturnEntity.Field.Return09, StudentReturnEntity.Field.Return10,
                            StudentReturnEntity.Field.Return11, StudentReturnEntity.Field.Return12, StudentReturnEntity.Field.Return13, StudentReturnEntity.Field.Return14, StudentReturnEntity.Field.Return15,
                            StudentReturnEntity.Field.Return16, StudentReturnEntity.Field.Return17, StudentReturnEntity.Field.Return18, StudentReturnEntity.Field.Return19, StudentReturnEntity.Field.Return20,
                            StudentReturnEntity.Field.Return21, StudentReturnEntity.Field.Return22, StudentReturnEntity.Field.Return23, StudentReturnEntity.Field.Return24, StudentReturnEntity.Field.Return25,
                            StudentReturnEntity.Field.Return26, StudentReturnEntity.Field.Return27, StudentReturnEntity.Field.Return28, StudentReturnEntity.Field.Return29, StudentReturnEntity.Field.Return30
                    };
                    for (int idx = 0; idx < rtAmounts.Length; idx++)
                    {
                        fieldValues.Add(fields[idx], rtAccount[idx]);
                    }

                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnId, rtName);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnAmount, returnAmount);
                    fieldValues.Add(StudentReturnEntity.Field.ReturnDate, Common.GetTWDate7());

                    if (!String.IsNullOrEmpty(rtBankid) && !String.IsNullOrEmpty(rtAccount))
                    {
                        //TODO: ???
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, rtBankid + rtAccount);
                    }
                    else
                    {
                        fieldValues.Add(StudentReturnEntity.Field.RemitData, String.Empty);
                    }
                    //TODO: ???
                    fieldValues.Add(StudentReturnEntity.Field.ReturnWay, rtName == "1" ? "1" : (String.IsNullOrEmpty(studentReturn.RemitData) ? "1" : "2"));

                    int count = 0;
                    result = _Factory.UpdateFields<StudentReturnEntity>(fieldValues, where, out count);
                    return result;
                }
            }
            #endregion
        }

        #endregion

        #region Import BUC File(上傳課程收費標準)
        /// <summary>
        /// 匯入 BUC (上傳課程收費標準) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUCJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUC)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼或學期代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            string owner = null;
            string mappingId = null;
            string sheetName = null;
            string fileType = null;
            int cancel = 0;
            int seriroNo = 0;
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pFileName = null;
                string pCancel = null;
                string pSeriroNo = null;
                isParamOK = JobcubeEntity.ParseBUCParameter(job.Jparam, out owner, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out mappingId, out pFileName, out sheetName, out pCancel, out pSeriroNo);
                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (String.IsNullOrEmpty(mappingId)
                    || (fileType != "xls" && fileType != "xlsx" && fileType != "txt" && fileType != "ods")
                    || String.IsNullOrEmpty(pCancel) || !Int32.TryParse(pCancel, out cancel) || cancel < 1
                    || String.IsNullOrEmpty(pSeriroNo) || !Int32.TryParse(pSeriroNo, out seriroNo) || seriroNo < 1)
                {
                    logmsg = "批次處理序列缺少對照表代碼、上傳檔案序號或批次號碼的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                {
                    fileContent = encoding.GetBytes(textContent);
                }
                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取對照表
            MappingcsTxtEntity txtMapping = null;
            MappingcsXlsmdbEntity xlsMapping = null;
            {
                if (fileType == "txt")
                {
                    Expression where = new Expression(MappingcsTxtEntity.Field.MappingId, mappingId)
                        .And(MappingcsTxtEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingcsTxtEntity>(where, null, out txtMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取文字格式的上傳課程收費標準對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (txtMapping == null)
                    {
                        logmsg = String.Format("查無 代碼 {0}、商家代號 {1} 的文字格式的上傳課程收費標準對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    Expression where = new Expression(MappingcsXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingcsXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingcsXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Excel 格式的上傳課程收費標準對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Excel 格式的上傳課程收費標準對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                }
                else if (fileType == "ods")
                {
                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    Expression where = new Expression(MappingcsXlsmdbEntity.Field.MappingId, mappingId)
                       .And(MappingcsXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingcsXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Calc 格式的上傳課程收費標準對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Calc 格式的上傳課程收費標準對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();
            {
                string errmsg = null;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                ConvertFileHelper helper = new ConvertFileHelper();
                if (fileType == "txt")
                {
                    #region Txt 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        using (StreamReader reader = new StreamReader(ms, encoding))
                        {
                            isOK = helper.Txt2DataTable(reader, txtMapping.GetMapFields(), isBatch, true, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    #region Xls | Xlsx 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        if (fileType == "xls")
                        {
                            isOK = helper.Xls2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else
                        {
                            isOK = helper.Xlsx2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "ods")
                {
                    #region Ods 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Ods2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else
                {
                    #region 不支援
                    {
                        logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
                #endregion

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }
            }
            #endregion

            #region 使用交易將 DataTable 逐筆存入資料庫
            {
                successCount = 0;
                DateTime now = DateTime.Now;
                Result importResult = new Result(true);
                StringBuilder log = new StringBuilder();
                using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
                {
                    List<string> studentIds = new List<string>(table.Rows.Count);
                    List<string> saveClassIds = new List<string>();
                    List<string> saveCollegeIds = new List<string>();
                    List<string> saveMajorIds = new List<string>();
                    List<string> saveReduceIds = new List<string>();
                    List<string> saveLoanIds = new List<string>();
                    List<string> saveDormIds = new List<string>();

                    int rowNo = 0;
                    int okNo = 1;
                    foreach (DataRow row in table.Rows)
                    {
                        rowNo++;

                        #region 取得資料行錯誤訊息
                        {
                            string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                            if (!String.IsNullOrEmpty(failMsg))
                            {
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                continue;
                            }
                        }
                        #endregion

                        #region 課程標準 (ClassStandardEntity)
                        ClassStandardEntity class_standard = new ClassStandardEntity();
                        class_standard.ReceiveType = receiveType;
                        class_standard.YearId = yearId;
                        class_standard.TermId = termId;
                        class_standard.CourseId = fieldNames.Contains(MappingcsXlsmdbEntity.Field.CourseId) ? row[MappingcsXlsmdbEntity.Field.CourseId].ToString() : String.Empty;
                        class_standard.CourseName = fieldNames.Contains(MappingcsXlsmdbEntity.Field.CourseName) ? row[MappingcsXlsmdbEntity.Field.CourseName].ToString() : String.Empty;
                        class_standard.CourseCredit = fieldNames.Contains(MappingcsXlsmdbEntity.Field.CourseCredit) ? Convert.ToDecimal(row[MappingcsXlsmdbEntity.Field.CourseCredit]) : 0;
                        class_standard.CreditNo = fieldNames.Contains(MappingcsXlsmdbEntity.Field.CreditNo) ? row[MappingcsXlsmdbEntity.Field.CreditNo].ToString() : String.Empty;
                        class_standard.CourseCprice = fieldNames.Contains(MappingcsXlsmdbEntity.Field.CourseCprice) ? Convert.ToDecimal(row[MappingcsXlsmdbEntity.Field.CourseCprice]) : 0;
                        #endregion

                        #region 寫入資料庫
                        if (importResult.IsSuccess)
                        {
                            int count = 0;
                            Result result = null;

                            string actionName = null;
                            #region ClassStandardEntity
                            {
                                ClassStandardEntity oldClassStandard = null;
                                Expression where = new Expression(ClassStandardEntity.Field.ReceiveType, receiveType)
                                    .And(ClassStandardEntity.Field.YearId, yearId)
                                    .And(ClassStandardEntity.Field.TermId, termId)
                                    .And(ClassStandardEntity.Field.CourseId, class_standard.CourseId);
                                result = tsFactory.SelectFirst<ClassStandardEntity>(where, null, out oldClassStandard);
                                if (result.IsSuccess)
                                {
                                    if (oldClassStandard == null)
                                    {
                                        #region Insert
                                        actionName = "新增";
                                        result = tsFactory.Insert(class_standard, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "課程標準資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update
                                        actionName = "更新";
                                        KeyValueList fieldValues = new KeyValueList();
                                        fieldValues.Add(ClassStandardEntity.Field.CourseName, class_standard.CourseName);
                                        fieldValues.Add(ClassStandardEntity.Field.CourseCredit, class_standard.CourseCredit);
                                        fieldValues.Add(ClassStandardEntity.Field.CreditNo, class_standard.CreditNo);
                                        fieldValues.Add(ClassStandardEntity.Field.CourseCprice, class_standard.CourseCprice);

                                        result = tsFactory.UpdateFields<ClassStandardEntity>(fieldValues, where, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "課程標準不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            if (result.IsSuccess)
                            {
                                tsFactory.Commit();
                                successCount++;
                                okNo++;
                                log.AppendFormat("第 {0} 筆資料{1}成功", rowNo, actionName).AppendLine();
                            }
                            else
                            {
                                log.AppendFormat("第 {0} 筆資料{1}失敗，錯誤訊息：{2}", rowNo, actionName, result.Message).AppendLine();
                                tsFactory.Rollback();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料新增失敗，錯誤訊息：：{1}", rowNo, result.Message), result.Code, result.Exception);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (!importResult.IsSuccess && isBatch)
                        {
                            break;
                        }
                        #endregion
                    }
                }

                logmsg = log.ToString();
                if (importResult.IsSuccess && successCount == 0)
                {
                    importResult = new Result(false, "無資料被匯入成功", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                return importResult;
            }
            #endregion
        }
        #endregion

        #region Import BUD File(上傳已產生銷帳編號之就貸資料)
        /// <summary>
        /// 匯入 BUD (上傳已產生銷帳編號之就貸資料) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUDJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUD)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼或學期代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            string owner = null;
            string mappingId = null;
            string sheetName = null;
            string fileType = null;
            int cancel = 0;
            int seriroNo = 0;
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pFileName = null;
                string pCancel = null;
                string pSeriroNo = null;
                isParamOK = JobcubeEntity.ParseBUDParameter(job.Jparam, out owner, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out mappingId, out pFileName, out sheetName, out pCancel, out pSeriroNo);
                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (String.IsNullOrEmpty(mappingId)
                    || (fileType != "xls" && fileType != "xlsx" && fileType != "txt" && fileType != "ods")
                    || String.IsNullOrEmpty(pCancel) || !Int32.TryParse(pCancel, out cancel) || cancel < 1
                    || String.IsNullOrEmpty(pSeriroNo) || !Int32.TryParse(pSeriroNo, out seriroNo) || seriroNo < 1)
                {
                    logmsg = "批次處理序列缺少對照表代碼、上傳檔案序號或批次號碼的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion

                if (receiveType != pReceiveType || yearId != pYearId || termId != pTermId || depId != pDepId || receiveId != pReceiveId)
                {
                    logmsg = "批次處理序列參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 取得虛擬帳號模組資料
            CancelNoHelper cnoHelper = new CancelNoHelper();
            CancelNoHelper.Module module = cnoHelper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                logmsg = String.Format("無法取得商家代號 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
            }
            #endregion

            #region 代收管道設定
            bool hasSMChannel = false;
            bool hasCashChannel = false;
            {
                ChannelHelper helper = new ChannelHelper();
                string errmsg = helper.CheckReceiveChannel(receiveType, out hasSMChannel, out hasCashChannel);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    logmsg = String.Format("無法取得商家代號 {0} 的代收管道設定，錯誤訊息：{1}", receiveType, errmsg);
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                {
                    fileContent = encoding.GetBytes(textContent);
                }
                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取對照表
            MappingloTxtEntity txtMapping = null;
            MappingloXlsmdbEntity xlsMapping = null;
            {
                if (fileType == "txt")
                {
                    Expression where = new Expression(MappingloTxtEntity.Field.MappingId, mappingId)
                        .And(MappingloTxtEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingloTxtEntity>(where, null, out txtMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取文字格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (txtMapping == null)
                    {
                        logmsg = String.Format("查無 代碼 {0}、商家代號 {1} 的文字格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    Expression where = new Expression(MappingloXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingloXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingloXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Excel 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Excel 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                }
                else if (fileType == "ods")
                {
                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    Expression where = new Expression(MappingloXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingloXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingloXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Calc 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Calc 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();
            {
                string errmsg = null;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                ConvertFileHelper helper = new ConvertFileHelper();
                if (fileType == "txt")
                {
                    #region Txt 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        using (StreamReader reader = new StreamReader(ms, encoding))
                        {
                            isOK = helper.Txt2DataTable(reader, txtMapping.GetMapFields(), isBatch, true, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    #region Xls | Xlsx 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        if (fileType == "xls")
                        {
                            isOK = helper.Xls2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else
                        {
                            isOK = helper.Xlsx2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "ods")
                {
                    #region Ods 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Ods2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else
                {
                    #region 不支援
                    {
                        logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
                #endregion

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }
            }
            #endregion

            StringBuilder logs = new StringBuilder();
            Int32 line = 0;
            successCount = 0;
            #region 使用交易將 DataTable 逐筆存入資料庫
            using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
            {
                BillAmountHelper amountHelper = new BillAmountHelper();

                foreach (DataRow row in table.Rows)
                {
                    #region 處理上傳的每一筆資料
                    line++;

                    StudentLoanEntity new_student_loan = new StudentLoanEntity();
                    #region 讀資料
                    //代收類別
                    new_student_loan.ReceiveType = receiveType;
                    //學年
                    new_student_loan.YearId = yearId;
                    //學期
                    new_student_loan.TermId = termId;
                    //部別
                    new_student_loan.DepId = depId;
                    //費用別
                    new_student_loan.ReceiveId = receiveId;

                    #region 學號
                    string stu_id = "";
                    if (table.Columns.Contains("s_id"))
                    {
                        if (!Convert.IsDBNull(row["s_id"]))
                        {
                            stu_id = Convert.ToString(row["s_id"]).Trim();
                        }
                    }
                    new_student_loan.StuId = stu_id;
                    if (String.IsNullOrWhiteSpace(new_student_loan.StuId))  //必要欄位
                    {
                        logs.AppendLine(string.Format("第{0}筆缺少學號資料", line));
                        continue;
                    }
                    #endregion

                    #region 銷帳編號
                    string cancel_no = "";
                    if (table.Columns.Contains("cancel_no"))
                    {
                        if (!Convert.IsDBNull(row["cancel_no"]))
                        {
                            cancel_no = Convert.ToString(row["cancel_no"]).Trim();
                        }
                    }
                    if (String.IsNullOrWhiteSpace(cancel_no))   //必要欄位
                    {
                        logs.AppendLine(string.Format("第{0}筆缺少銷帳編號資料", line));
                        continue;
                    }
                    #endregion

                    //轉成table那裏好像寫反了
                    #region 就貸代碼
                    string loan_id = "";
                    if (table.Columns.Contains("lo_name"))
                    {
                        if (!Convert.IsDBNull(row["lo_name"]))
                        {
                            loan_id = Convert.ToString(row["lo_name"]).Trim();
                        }
                    }
                    new_student_loan.LoanId = loan_id;
                    #endregion

                    #region 就貸名稱
                    string loan_name = "";
                    if (table.Columns.Contains("lo_count"))
                    {
                        if (!Convert.IsDBNull(row["lo_count"]))
                        {
                            loan_name = Convert.ToString(row["lo_count"]).Trim();
                        }
                    }
                    #endregion

                    #region 可貸金額
                    Int32 loan_amount = 0;
                    if (table.Columns.Contains("lo_amount"))
                    {
                        if (!Convert.IsDBNull(row["lo_amount"]))
                        {
                            loan_amount = Convert.ToInt32(row["lo_amount"]);
                        }
                    }
                    new_student_loan.LoanFixamount = loan_amount;
                    #endregion

                    #region 就貸明細
                    Int32 loan_total = 0;
                    {
                        #region [Old]
                        //int loan_item_count = 0;
                        //string[] loan_item_names = new string[40];
                        //for (int i = 0; i < 40; i++)
                        //{
                        //    loan_item_names[i] = string.Format("lo_{0}", (i + 1).ToString());
                        //    Int32 loan_item_amount = 0;
                        //    if (table.Columns.Contains(loan_item_names[i]))
                        //    {
                        //        if (!Convert.IsDBNull(row[loan_item_names[i]]))
                        //        {
                        //            loan_item_amount = Convert.ToInt32(row[loan_item_names[i]]);
                        //        }
                        //        setLoanItemAmountofStudentLoan(loan_item_names[i], loan_item_amount, ref new_student_loan);
                        //        loan_item_count++;
                        //    }
                        //    else
                        //    {
                        //        setLoanItemAmountofStudentLoan(loan_item_names[i], loan_item_amount, ref new_student_loan);
                        //    }
                        //    loan_total += loan_item_amount;
                        //}
                        #endregion

                        for (int no = 1; no <= 40; no++)
                        {
                            int itemAmount = 0;
                            string fieldName = string.Format("lo_{0}", no);
                            if (table.Columns.Contains(fieldName) && !row.IsNull(fieldName))
                            {
                                itemAmount = Convert.ToInt32(row[fieldName]);
                            }
                            //null 或未上傳的項目都視為 0，以方便後面計算
                            new_student_loan.SetLoanItemAmount(no, itemAmount);
                            loan_total += itemAmount;
                        }
                    }
                    new_student_loan.LoanAmount = loan_total;
                    #endregion

                    #region 檢查明細與總金額
                    if (new_student_loan.LoanFixamount != 0 && new_student_loan.LoanAmount != 0)
                    {
                        if (new_student_loan.LoanFixamount != new_student_loan.LoanAmount)
                        {
                            logs.AppendLine(string.Format("第{0}筆可貸明細金額與總額不符", line));
                            continue;
                        }
                    }
                    #endregion
                    #endregion

                    StudentReceiveEntity studnet_receive = null;
                    StudentLoanEntity student_loan = null;
                    #region 讀取student_loan 及 student_receive
                    {
                        string log = "";
                        //因為怕有重複學號的，所以讀取的方式先去讀取student_receive確認銷編對應的old_seq再去讀取student_loan
                        studnet_receive = getStudentReceive(receiveType, yearId, termId, depId, receiveId, stu_id, cancel_no, out log);
                        if (studnet_receive != null)
                        {
                            new_student_loan.OldSeq = studnet_receive.OldSeq;

                            string loan_id_1 = studnet_receive.LoanId;
                            int old_seq = studnet_receive.OldSeq;
                            student_loan = getStudentLoan(receiveType, yearId, termId, depId, receiveId, stu_id, loan_id_1, old_seq, out log);
                        }
                        else
                        {
                            //讀不到student_receive就下一筆
                            logs.AppendLine(string.Format("第{0}筆讀取學生繳費資料發生錯誤,錯誤訊息={1}", line, log));
                            continue;
                        }
                    }
                    #endregion

                    #region 土銀說要自動計算金額與虛擬帳號
                    string orgSeriorNo = studnet_receive.SeriorNo;
                    string orgCancelNo = studnet_receive.CancelNo;
                    decimal? orgReceiveAmount = studnet_receive.ReceiveAmount;
                    string cancelNo = null;
                    string seriorNo = null;
                    string checksum = null;
                    string atmCancelNo = null;
                    string smCancelNo = null;
                    {
                        //先指定就貸金額，再計算
                        studnet_receive.Loan = new_student_loan.LoanFixamount.Value;
                        if (!amountHelper.CalcBillAmount(ref studnet_receive, BillAmountHelper.CalculateType.byAmount, hasCashChannel, hasSMChannel))
                        {
                            logs.AppendLine(string.Format("第{0}筆資料計算應繳金額失敗，錯誤訊息：{1}", line, amountHelper.err_mgs));
                            continue;
                        }

                        string errmsg = cnoHelper.TryGenCancelNo(_Factory, module, studnet_receive, school.IsBigReceiveId(), out cancelNo, out seriorNo, out checksum, out atmCancelNo, out smCancelNo, true);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            logs.AppendLine(string.Format("第{0}筆資料產生虛擬帳號失敗，錯誤訊息：{1}", line, errmsg));
                            continue;
                        }
                    }
                    #endregion

                    #region 寫入資料庫
                    bool commit = false;

                    #region [Old] 土銀說上傳的就貸金額項目不用去異動 Student_Receive 的收入科目金額，所以不用計算也不用更新收入科目
                    //#region 判斷studnet_loan是否存在，存在的話先還原金額，在更新，不存在就新增
                    //if (student_loan != null)
                    //{
                    //    #region 有舊的 studnet_loan
                    //    int count = 0;
                    //    Result result = null;
                    //    Expression where = null;
                    //    KeyValueList fieldValues = null;

                    //    #region 還原loan amount再扣掉新的loan amount studnet_receive
                    //    #region 設定where條件
                    //    where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType);
                    //    where.And(StudentReceiveEntity.Field.YearId, yearId);
                    //    where.And(StudentReceiveEntity.Field.TermId, termId);
                    //    where.And(StudentReceiveEntity.Field.DepId, depId);
                    //    where.And(StudentReceiveEntity.Field.ReceiveId, receiveId);
                    //    where.And(StudentReceiveEntity.Field.StuId, stu_id);
                    //    where.And(StudentReceiveEntity.Field.CancelNo, cancel_no);
                    //    Expression where1 = new Expression(StudentReceiveEntity.Field.ReceiveWay, "");
                    //    where1.Or(StudentReceiveEntity.Field.ReceiveWay, null);
                    //    where.And(where1);
                    //    #endregion
                    //    #region 設定要更新的欄位
                    //    //decimal new_receive_item_amount;
                    //    fieldValues = new KeyValueList();
                    //    #region 收入科目
                    //    #region [Old]
                    //    //if (studnet_receive.Receive01 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive01 + (decimal)student_loan.Loan01 - (decimal)new_student_loan.Loan01;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive01, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive02 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive02 + (decimal)student_loan.Loan02 - (decimal)new_student_loan.Loan02;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive02, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive03 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive03 + (decimal)student_loan.Loan03 - (decimal)new_student_loan.Loan03;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive03, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive04 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive04 + (decimal)student_loan.Loan04 - (decimal)new_student_loan.Loan04;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive04, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive05 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive05 + (decimal)student_loan.Loan05 - (decimal)new_student_loan.Loan05;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive05, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive06 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive06 + (decimal)student_loan.Loan06 - (decimal)new_student_loan.Loan06;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive06, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive07 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive07 + (decimal)student_loan.Loan07 - (decimal)new_student_loan.Loan07;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive07, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive08 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive08 + (decimal)student_loan.Loan08 - (decimal)new_student_loan.Loan08;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive08, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive09 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive09 + (decimal)student_loan.Loan09 - (decimal)new_student_loan.Loan09;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive09, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive10 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive10 + (decimal)student_loan.Loan10 - (decimal)new_student_loan.Loan10;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive10, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive11 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive11 + (decimal)student_loan.Loan11 - (decimal)new_student_loan.Loan11;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive11, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive12 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive12 + (decimal)student_loan.Loan12 - (decimal)new_student_loan.Loan12;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive12, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive13 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive13 + (decimal)student_loan.Loan13 - (decimal)new_student_loan.Loan13;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive13, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive14 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive14 + (decimal)student_loan.Loan14 - (decimal)new_student_loan.Loan14;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive14, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive15 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive15 + (decimal)student_loan.Loan15 - (decimal)new_student_loan.Loan15;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive15, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive16 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive16 + (decimal)student_loan.Loan16 - (decimal)new_student_loan.Loan16;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive16, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive17 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive17 + (decimal)student_loan.Loan17 - (decimal)new_student_loan.Loan17;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive17, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive18 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive18 + (decimal)student_loan.Loan18 - (decimal)new_student_loan.Loan18;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive18, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive19 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive19 + (decimal)student_loan.Loan19 - (decimal)new_student_loan.Loan19;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive19, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive20 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive20 + (decimal)student_loan.Loan20 - (decimal)new_student_loan.Loan20;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive20, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive21 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive21 + (decimal)student_loan.Loan21 - (decimal)new_student_loan.Loan21;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive21, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive22 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive22 + (decimal)student_loan.Loan22 - (decimal)new_student_loan.Loan22;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive22, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive23 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive23 + (decimal)student_loan.Loan23 - (decimal)new_student_loan.Loan23;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive23, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive24 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive24 + (decimal)student_loan.Loan24 - (decimal)new_student_loan.Loan24;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive24, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive25 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive25 + (decimal)student_loan.Loan25 - (decimal)new_student_loan.Loan25;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive25, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive26 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive26 + (decimal)student_loan.Loan26 - (decimal)new_student_loan.Loan26;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive26, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive27 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive27 + (decimal)student_loan.Loan27 - (decimal)new_student_loan.Loan27;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive27, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive28 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive28 + (decimal)student_loan.Loan28 - (decimal)new_student_loan.Loan28;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive28, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive29 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive29 + (decimal)student_loan.Loan29 - (decimal)new_student_loan.Loan29;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive29, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive30 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive30 + (decimal)student_loan.Loan30 - (decimal)new_student_loan.Loan30;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive30, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive31 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive31 + (decimal)student_loan.Loan31 - (decimal)new_student_loan.Loan31;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive31, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive32 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive32 + (decimal)student_loan.Loan32 - (decimal)new_student_loan.Loan32;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive32, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive33 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive33 + (decimal)student_loan.Loan33 - (decimal)new_student_loan.Loan33;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive33, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive34 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive34 + (decimal)student_loan.Loan34 - (decimal)new_student_loan.Loan34;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive34, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive35 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive35 + (decimal)student_loan.Loan35 - (decimal)new_student_loan.Loan35;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive35, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive36 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive36 + (decimal)student_loan.Loan36 - (decimal)new_student_loan.Loan36;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive36, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive37 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive37 + (decimal)student_loan.Loan37 - (decimal)new_student_loan.Loan37;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive37, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive38 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive38 + (decimal)student_loan.Loan38 - (decimal)new_student_loan.Loan38;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive38, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive39 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive39 + (decimal)student_loan.Loan39 - (decimal)new_student_loan.Loan39;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive39, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive40 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive40 + (decimal)student_loan.Loan40 - (decimal)new_student_loan.Loan40;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive40, new_receive_item_amount);
                    //    //}
                    //    #endregion

                    //    #region [New]
                    //    string[] receiveItemFieldNames = new string[] {
                    //        StudentReceiveEntity.Field.Receive01, StudentReceiveEntity.Field.Receive02, StudentReceiveEntity.Field.Receive03, StudentReceiveEntity.Field.Receive04, StudentReceiveEntity.Field.Receive05,
                    //        StudentReceiveEntity.Field.Receive06, StudentReceiveEntity.Field.Receive07, StudentReceiveEntity.Field.Receive08, StudentReceiveEntity.Field.Receive09, StudentReceiveEntity.Field.Receive10,
                    //        StudentReceiveEntity.Field.Receive11, StudentReceiveEntity.Field.Receive12, StudentReceiveEntity.Field.Receive13, StudentReceiveEntity.Field.Receive14, StudentReceiveEntity.Field.Receive15,
                    //        StudentReceiveEntity.Field.Receive16, StudentReceiveEntity.Field.Receive17, StudentReceiveEntity.Field.Receive18, StudentReceiveEntity.Field.Receive19, StudentReceiveEntity.Field.Receive20,
                    //        StudentReceiveEntity.Field.Receive21, StudentReceiveEntity.Field.Receive22, StudentReceiveEntity.Field.Receive23, StudentReceiveEntity.Field.Receive24, StudentReceiveEntity.Field.Receive25,
                    //        StudentReceiveEntity.Field.Receive26, StudentReceiveEntity.Field.Receive27, StudentReceiveEntity.Field.Receive28, StudentReceiveEntity.Field.Receive29, StudentReceiveEntity.Field.Receive30,
                    //        StudentReceiveEntity.Field.Receive31, StudentReceiveEntity.Field.Receive32, StudentReceiveEntity.Field.Receive33, StudentReceiveEntity.Field.Receive34, StudentReceiveEntity.Field.Receive35,
                    //        StudentReceiveEntity.Field.Receive36, StudentReceiveEntity.Field.Receive37, StudentReceiveEntity.Field.Receive38, StudentReceiveEntity.Field.Receive39, StudentReceiveEntity.Field.Receive40
                    //    };
                    //    decimal?[] oldReceiveItemAmounts = studnet_receive.GetAllReceiveAmounts();
                    //    for (int no = 1; no <= 40; no++)
                    //    {
                    //        decimal? oldReceiveItemAmount = oldReceiveItemAmounts[no - 1];
                    //        decimal? oldLoanItemAmount = student_loan.GetLoanItemAmount(no);
                    //        decimal? newLoanItemAmount = new_student_loan.GetLoanItemAmount(no);
                    //        if ((oldLoanItemAmount != null && oldLoanItemAmount.Value != 0) || (newLoanItemAmount != null && newLoanItemAmount.Value != 0))
                    //        {
                    //            decimal newReceiveItemAmount = (oldReceiveItemAmount == null ? 0 : oldReceiveItemAmount.Value)
                    //                + (oldLoanItemAmount == null ? 0 : oldLoanItemAmount.Value)
                    //                - (newLoanItemAmount == null ? 0 : newLoanItemAmount.Value);

                    //            fieldValues.Add(receiveItemFieldNames[no - 1], newReceiveItemAmount);
                    //        }
                    //    }
                    //    #endregion
                    //    #endregion
                    //    fieldValues.Add(StudentReceiveEntity.Field.LoanId, loan_id);
                    //    fieldValues.Add(StudentReceiveEntity.Field.Loan, (decimal)new_student_loan.LoanFixamount);
                    //    if (new_student_loan.LoanFixamount > 0)
                    //    {
                    //        fieldValues.Add(StudentReceiveEntity.Field.RealLoan, (decimal)new_student_loan.LoanFixamount);
                    //    }
                    //    else
                    //    {
                    //        if (new_student_loan.LoanAmount > 0)
                    //        {
                    //            fieldValues.Add(StudentReceiveEntity.Field.RealLoan, (decimal)new_student_loan.LoanAmount);
                    //        }
                    //        else
                    //        {
                    //            fieldValues.Add(StudentReceiveEntity.Field.RealLoan, 0);
                    //        }
                    //    }
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelNo, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelPono, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelEb1no, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelEb2no, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb1amount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb2amount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, 0);
                    //    #endregion
                    //    result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                    //    if (result.IsSuccess && count > 0)
                    //    {
                    //        #region 更新student_loan
                    //        #region 設定where條件
                    //        where = new Expression(StudentLoanEntity.Field.ReceiveType, receiveType);
                    //        where.And(StudentLoanEntity.Field.YearId, yearId);
                    //        where.And(StudentLoanEntity.Field.TermId, termId);
                    //        where.And(StudentLoanEntity.Field.DepId, depId);
                    //        where.And(StudentLoanEntity.Field.ReceiveId, receiveId);
                    //        where.And(StudentLoanEntity.Field.StuId, stu_id);
                    //        //where.And(StudentLoanEntity.Field.LoanId, loan_id); 同一個學生同一張繳單只會有一筆貸款
                    //        where.And(StudentLoanEntity.Field.OldSeq, new_student_loan.OldSeq);
                    //        #endregion
                    //        #region 設定要更新的欄位
                    //        fieldValues = new KeyValueList();
                    //        fieldValues.Add(StudentLoanEntity.Field.LoanId, loan_id);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan01, new_student_loan.Loan01);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan02, new_student_loan.Loan02);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan03, new_student_loan.Loan03);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan04, new_student_loan.Loan04);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan05, new_student_loan.Loan05);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan06, new_student_loan.Loan06);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan07, new_student_loan.Loan07);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan08, new_student_loan.Loan08);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan09, new_student_loan.Loan09);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan10, new_student_loan.Loan10);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan11, new_student_loan.Loan11);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan12, new_student_loan.Loan12);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan13, new_student_loan.Loan13);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan14, new_student_loan.Loan14);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan15, new_student_loan.Loan15);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan16, new_student_loan.Loan16);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan17, new_student_loan.Loan17);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan18, new_student_loan.Loan18);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan19, new_student_loan.Loan19);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan20, new_student_loan.Loan20);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan21, new_student_loan.Loan21);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan22, new_student_loan.Loan22);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan23, new_student_loan.Loan23);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan24, new_student_loan.Loan24);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan25, new_student_loan.Loan25);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan26, new_student_loan.Loan26);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan27, new_student_loan.Loan27);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan28, new_student_loan.Loan28);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan29, new_student_loan.Loan29);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan30, new_student_loan.Loan30);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan31, new_student_loan.Loan31);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan32, new_student_loan.Loan32);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan33, new_student_loan.Loan33);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan34, new_student_loan.Loan34);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan35, new_student_loan.Loan35);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan36, new_student_loan.Loan36);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan37, new_student_loan.Loan37);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan38, new_student_loan.Loan38);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan39, new_student_loan.Loan39);
                    //        fieldValues.Add(StudentLoanEntity.Field.Loan40, new_student_loan.Loan40);
                    //        fieldValues.Add(StudentLoanEntity.Field.LoanFixamount, new_student_loan.LoanFixamount);
                    //        fieldValues.Add(StudentLoanEntity.Field.LoanAmount, new_student_loan.LoanAmount);
                    //        #endregion
                    //        result = tsFactory.UpdateFields<StudentLoanEntity>(fieldValues, where, out count);
                    //        if (result.IsSuccess && count > 0)
                    //        {
                    //            commit = true;
                    //        }
                    //        else
                    //        {
                    //            logs.AppendLine(string.Format("第{0}筆更新學生就貸資料發生錯誤，錯誤訊息={1}", line, result.Message));
                    //        }
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        logs.AppendLine(string.Format("第{0}筆更新學生繳費資料發生錯誤，錯誤訊息={1}", line, result.Message));
                    //    }
                    //    #endregion

                    //    #endregion
                    //}
                    //else
                    //{
                    //    #region 無舊的 studnet_loan
                    //    int count = 0;
                    //    Result result = null;
                    //    Expression where = null;
                    //    KeyValueList fieldValues = null;

                    //    #region 設定where條件
                    //    where = new Expression(StudentReceiveEntity.Field.ReceiveType, receiveType);
                    //    where.And(StudentReceiveEntity.Field.YearId, yearId);
                    //    where.And(StudentReceiveEntity.Field.TermId, termId);
                    //    where.And(StudentReceiveEntity.Field.DepId, depId);
                    //    where.And(StudentReceiveEntity.Field.ReceiveId, receiveId);
                    //    where.And(StudentReceiveEntity.Field.StuId, stu_id);
                    //    where.And(StudentReceiveEntity.Field.CancelNo, cancel_no);
                    //    Expression where1 = new Expression(StudentReceiveEntity.Field.ReceiveWay, "");
                    //    where1.Or(StudentReceiveEntity.Field.ReceiveWay, null);
                    //    where.And(where1);
                    //    #endregion
                    //    #region 設定要更新的欄位
                    //    //decimal new_receive_item_amount;
                    //    fieldValues = new KeyValueList();
                    //    #region 更新收入科目
                    //    #region [Old]
                    //    //if (studnet_receive.Receive01 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive01 - (decimal)new_student_loan.Loan01;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive01, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive02 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive02 - (decimal)new_student_loan.Loan02;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive02, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive03 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive03 - (decimal)new_student_loan.Loan03;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive03, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive04 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive04 - (decimal)new_student_loan.Loan04;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive04, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive05 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive05 - (decimal)new_student_loan.Loan05;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive05, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive06 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive06 - (decimal)new_student_loan.Loan06;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive06, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive07 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive07 - (decimal)new_student_loan.Loan07;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive07, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive08 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive08 - (decimal)new_student_loan.Loan08;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive08, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive09 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive09 - (decimal)new_student_loan.Loan09;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive09, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive10 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive10 - (decimal)new_student_loan.Loan10;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive10, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive11 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive11 - (decimal)new_student_loan.Loan11;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive11, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive12 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive12 - (decimal)new_student_loan.Loan12;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive12, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive13 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive13 - (decimal)new_student_loan.Loan13;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive13, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive14 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive14 - (decimal)new_student_loan.Loan14;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive14, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive15 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive15 - (decimal)new_student_loan.Loan15;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive15, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive16 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive16 - (decimal)new_student_loan.Loan16;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive16, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive17 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive17 - (decimal)new_student_loan.Loan17;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive17, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive18 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive18 - (decimal)new_student_loan.Loan18;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive18, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive19 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive19 - (decimal)new_student_loan.Loan19;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive19, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive20 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive20 - (decimal)new_student_loan.Loan20;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive20, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive21 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive21 - (decimal)new_student_loan.Loan21;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive21, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive22 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive22 - (decimal)new_student_loan.Loan22;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive22, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive23 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive23 - (decimal)new_student_loan.Loan23;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive23, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive24 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive24 - (decimal)new_student_loan.Loan24;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive24, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive25 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive25 - (decimal)new_student_loan.Loan25;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive25, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive26 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive26 - (decimal)new_student_loan.Loan26;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive26, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive27 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive27 - (decimal)new_student_loan.Loan27;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive27, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive28 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive28 - (decimal)new_student_loan.Loan28;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive28, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive29 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive29 - (decimal)new_student_loan.Loan29;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive29, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive30 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive30 - (decimal)new_student_loan.Loan30;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive30, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive31 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive31 - (decimal)new_student_loan.Loan31;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive31, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive32 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive32 - (decimal)new_student_loan.Loan32;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive32, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive33 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive33 - (decimal)new_student_loan.Loan33;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive33, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive34 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive34 - (decimal)new_student_loan.Loan34;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive34, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive35 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive35 - (decimal)new_student_loan.Loan35;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive35, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive36 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive36 - (decimal)new_student_loan.Loan36;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive36, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive37 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive37 - (decimal)new_student_loan.Loan37;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive37, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive38 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive38 - (decimal)new_student_loan.Loan38;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive38, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive39 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive39 - (decimal)new_student_loan.Loan39;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive39, new_receive_item_amount);
                    //    //}
                    //    //if (studnet_receive.Receive40 != null)
                    //    //{
                    //    //    new_receive_item_amount = (decimal)studnet_receive.Receive40 - (decimal)new_student_loan.Loan40;
                    //    //    fieldValues.Add(StudentReceiveEntity.Field.Receive40, new_receive_item_amount);
                    //    //}
                    //    #endregion

                    //    #region [New]
                    //    string[] receiveItemFieldNames = new string[] {
                    //        StudentReceiveEntity.Field.Receive01, StudentReceiveEntity.Field.Receive02, StudentReceiveEntity.Field.Receive03, StudentReceiveEntity.Field.Receive04, StudentReceiveEntity.Field.Receive05,
                    //        StudentReceiveEntity.Field.Receive06, StudentReceiveEntity.Field.Receive07, StudentReceiveEntity.Field.Receive08, StudentReceiveEntity.Field.Receive09, StudentReceiveEntity.Field.Receive10,
                    //        StudentReceiveEntity.Field.Receive11, StudentReceiveEntity.Field.Receive12, StudentReceiveEntity.Field.Receive13, StudentReceiveEntity.Field.Receive14, StudentReceiveEntity.Field.Receive15,
                    //        StudentReceiveEntity.Field.Receive16, StudentReceiveEntity.Field.Receive17, StudentReceiveEntity.Field.Receive18, StudentReceiveEntity.Field.Receive19, StudentReceiveEntity.Field.Receive20,
                    //        StudentReceiveEntity.Field.Receive21, StudentReceiveEntity.Field.Receive22, StudentReceiveEntity.Field.Receive23, StudentReceiveEntity.Field.Receive24, StudentReceiveEntity.Field.Receive25,
                    //        StudentReceiveEntity.Field.Receive26, StudentReceiveEntity.Field.Receive27, StudentReceiveEntity.Field.Receive28, StudentReceiveEntity.Field.Receive29, StudentReceiveEntity.Field.Receive30,
                    //        StudentReceiveEntity.Field.Receive31, StudentReceiveEntity.Field.Receive32, StudentReceiveEntity.Field.Receive33, StudentReceiveEntity.Field.Receive34, StudentReceiveEntity.Field.Receive35,
                    //        StudentReceiveEntity.Field.Receive36, StudentReceiveEntity.Field.Receive37, StudentReceiveEntity.Field.Receive38, StudentReceiveEntity.Field.Receive39, StudentReceiveEntity.Field.Receive40
                    //    };
                    //    decimal?[] oldReceiveItemAmounts = studnet_receive.GetAllReceiveAmounts();
                    //    for (int no = 1; no <= 40; no++)
                    //    {
                    //        decimal? oldReceiveItemAmount = oldReceiveItemAmounts[no - 1];
                    //        decimal? newLoanItemAmount = new_student_loan.GetLoanItemAmount(no);
                    //        if ((newLoanItemAmount != null && newLoanItemAmount.Value != 0))
                    //        {
                    //            decimal newReceiveItemAmount = (oldReceiveItemAmount == null ? 0 : oldReceiveItemAmount.Value)
                    //                - (newLoanItemAmount == null ? 0 : newLoanItemAmount.Value);

                    //            fieldValues.Add(receiveItemFieldNames[no - 1], newReceiveItemAmount);
                    //        }
                    //    }
                    //    #endregion
                    //    #endregion
                    //    fieldValues.Add(StudentReceiveEntity.Field.LoanId, loan_id);
                    //    fieldValues.Add(StudentReceiveEntity.Field.Loan, (decimal)new_student_loan.LoanFixamount);
                    //    if (new_student_loan.LoanFixamount > 0)
                    //    {
                    //        fieldValues.Add(StudentReceiveEntity.Field.RealLoan, (decimal)new_student_loan.LoanFixamount);
                    //    }
                    //    else
                    //    {
                    //        if (new_student_loan.LoanAmount > 0)
                    //        {
                    //            fieldValues.Add(StudentReceiveEntity.Field.RealLoan, (decimal)new_student_loan.LoanAmount);
                    //        }
                    //        else
                    //        {
                    //            fieldValues.Add(StudentReceiveEntity.Field.RealLoan, 0);
                    //        }
                    //    }
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelNo, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelPono, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelEb1no, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelEb2no, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, "");
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb1amount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb2amount, 0);
                    //    fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, 0);
                    //    #endregion
                    //    result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                    //    if (result.IsSuccess && count > 0)
                    //    {
                    //        #region 新增student_loan
                    //        result = tsFactory.Insert(new_student_loan, out count);
                    //        if (result.IsSuccess && count > 0)
                    //        {
                    //            commit = true;
                    //        }
                    //        else
                    //        {
                    //            logs.AppendLine(string.Format("第{0}筆更新學生就貸資料發生錯誤，錯誤訊息={1}", line, result.Message));
                    //        }
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        logs.AppendLine(string.Format("第{0}筆更新學生繳費資料發生錯誤，錯誤訊息={1}", line, result.Message));
                    //    }

                    //    #endregion
                    //}
                    //#endregion
                    #endregion

                    #region [New] 土銀說上傳的就貸金額項目不用去異動 Student_Receive 的收入科目金額，所以不用計算也不用更新收入科目
                    {
                        Result result = new Result(false);
                        int count = 0;

                        #region 更新 StudentReceive
                        if (studnet_receive != null)
                        {
                            Expression w2 = new Expression(StudentReceiveEntity.Field.ReceiveWay, "").Or(StudentReceiveEntity.Field.ReceiveWay, null);
                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, studnet_receive.ReceiveType)
                                .And(StudentReceiveEntity.Field.YearId, studnet_receive.YearId)
                                .And(StudentReceiveEntity.Field.TermId, studnet_receive.TermId)
                                .And(StudentReceiveEntity.Field.DepId, studnet_receive.DepId)
                                .And(StudentReceiveEntity.Field.ReceiveId, studnet_receive.ReceiveId)
                                .And(StudentReceiveEntity.Field.StuId, studnet_receive.StuId)
                                .And(StudentReceiveEntity.Field.OldSeq, studnet_receive.OldSeq)
                                .And(StudentReceiveEntity.Field.CancelNo, studnet_receive.CancelNo)
                                .And(w2);

                            #region [20150915] 加強更新條件，避免更新到已繳或已銷的資料
                            {
                                where.And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
                                where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                //where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
                            }
                            #endregion

                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(StudentReceiveEntity.Field.LoanId, loan_id);
                            fieldValues.Add(StudentReceiveEntity.Field.Loan, new_student_loan.LoanFixamount.Value);

                            if (new_student_loan.LoanFixamount.Value > 0)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.RealLoan, new_student_loan.LoanFixamount.Value);
                            }
                            else if (new_student_loan.LoanAmount.Value > 0)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.RealLoan, new_student_loan.LoanAmount.Value);
                            }
                            else
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.RealLoan, 0);
                            }

                            if (seriorNo != orgSeriorNo)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.SeriorNo, seriorNo);
                            }
                            if (cancelNo != orgCancelNo)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.CancelNo, cancelNo);
                            }
                            fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, atmCancelNo);
                            fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, smCancelNo);
                            fieldValues.Add(StudentReceiveEntity.Field.CancelPono, "");
                            fieldValues.Add(StudentReceiveEntity.Field.CancelEb1no, "");
                            fieldValues.Add(StudentReceiveEntity.Field.CancelEb2no, "");
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, studnet_receive.ReceiveAmount);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, studnet_receive.ReceiveAtmamount);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, studnet_receive.ReceiveSmamount);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb1amount, 0);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb2amount, 0);

                            #region 虛擬帳號或應繳金額有異動則中信資料發送旗標清為 0，並更新 UpdateDate
                            if (orgCancelNo != cancelNo || orgReceiveAmount != studnet_receive.ReceiveAmount)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.CFlag, "0");

                                fieldValues.Add(StudentReceiveEntity.Field.UpdateDate, DateTime.Now);
                            }
                            #endregion

                            result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "無資料被更新", ErrorCode.D_DATA_NOT_FOUND, null);
                            }
                            if (!result.IsSuccess)
                            {
                                logs.AppendLine(string.Format("第{0}筆更新學生繳費資料發生錯誤，錯誤訊息：{1}", line, result.Message));
                            }
                        }
                        #endregion

                        #region StudnetLoan
                        if (result.IsSuccess)
                        {
                            if (student_loan != null)
                            {
                                #region 更新
                                Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, student_loan.ReceiveType)
                                    .And(StudentLoanEntity.Field.YearId, student_loan.YearId)
                                    .And(StudentLoanEntity.Field.TermId, student_loan.TermId)
                                    .And(StudentLoanEntity.Field.DepId, student_loan.DepId)
                                    .And(StudentLoanEntity.Field.ReceiveId, student_loan.ReceiveId)
                                    .And(StudentLoanEntity.Field.StuId, student_loan.StuId)
                                    .And(StudentLoanEntity.Field.LoanId, student_loan.LoanId)   //同一個學生同一張繳單只會有一筆貸款
                                    .And(StudentLoanEntity.Field.OldSeq, student_loan.OldSeq);

                                KeyValueList fieldValues = new KeyValueList();
                                fieldValues = new KeyValueList();
                                fieldValues.Add(StudentLoanEntity.Field.LoanId, loan_id);
                                fieldValues.Add(StudentLoanEntity.Field.Loan01, new_student_loan.Loan01);
                                fieldValues.Add(StudentLoanEntity.Field.Loan02, new_student_loan.Loan02);
                                fieldValues.Add(StudentLoanEntity.Field.Loan03, new_student_loan.Loan03);
                                fieldValues.Add(StudentLoanEntity.Field.Loan04, new_student_loan.Loan04);
                                fieldValues.Add(StudentLoanEntity.Field.Loan05, new_student_loan.Loan05);
                                fieldValues.Add(StudentLoanEntity.Field.Loan06, new_student_loan.Loan06);
                                fieldValues.Add(StudentLoanEntity.Field.Loan07, new_student_loan.Loan07);
                                fieldValues.Add(StudentLoanEntity.Field.Loan08, new_student_loan.Loan08);
                                fieldValues.Add(StudentLoanEntity.Field.Loan09, new_student_loan.Loan09);
                                fieldValues.Add(StudentLoanEntity.Field.Loan10, new_student_loan.Loan10);
                                fieldValues.Add(StudentLoanEntity.Field.Loan11, new_student_loan.Loan11);
                                fieldValues.Add(StudentLoanEntity.Field.Loan12, new_student_loan.Loan12);
                                fieldValues.Add(StudentLoanEntity.Field.Loan13, new_student_loan.Loan13);
                                fieldValues.Add(StudentLoanEntity.Field.Loan14, new_student_loan.Loan14);
                                fieldValues.Add(StudentLoanEntity.Field.Loan15, new_student_loan.Loan15);
                                fieldValues.Add(StudentLoanEntity.Field.Loan16, new_student_loan.Loan16);
                                fieldValues.Add(StudentLoanEntity.Field.Loan17, new_student_loan.Loan17);
                                fieldValues.Add(StudentLoanEntity.Field.Loan18, new_student_loan.Loan18);
                                fieldValues.Add(StudentLoanEntity.Field.Loan19, new_student_loan.Loan19);
                                fieldValues.Add(StudentLoanEntity.Field.Loan20, new_student_loan.Loan20);
                                fieldValues.Add(StudentLoanEntity.Field.Loan21, new_student_loan.Loan21);
                                fieldValues.Add(StudentLoanEntity.Field.Loan22, new_student_loan.Loan22);
                                fieldValues.Add(StudentLoanEntity.Field.Loan23, new_student_loan.Loan23);
                                fieldValues.Add(StudentLoanEntity.Field.Loan24, new_student_loan.Loan24);
                                fieldValues.Add(StudentLoanEntity.Field.Loan25, new_student_loan.Loan25);
                                fieldValues.Add(StudentLoanEntity.Field.Loan26, new_student_loan.Loan26);
                                fieldValues.Add(StudentLoanEntity.Field.Loan27, new_student_loan.Loan27);
                                fieldValues.Add(StudentLoanEntity.Field.Loan28, new_student_loan.Loan28);
                                fieldValues.Add(StudentLoanEntity.Field.Loan29, new_student_loan.Loan29);
                                fieldValues.Add(StudentLoanEntity.Field.Loan30, new_student_loan.Loan30);
                                fieldValues.Add(StudentLoanEntity.Field.Loan31, new_student_loan.Loan31);
                                fieldValues.Add(StudentLoanEntity.Field.Loan32, new_student_loan.Loan32);
                                fieldValues.Add(StudentLoanEntity.Field.Loan33, new_student_loan.Loan33);
                                fieldValues.Add(StudentLoanEntity.Field.Loan34, new_student_loan.Loan34);
                                fieldValues.Add(StudentLoanEntity.Field.Loan35, new_student_loan.Loan35);
                                fieldValues.Add(StudentLoanEntity.Field.Loan36, new_student_loan.Loan36);
                                fieldValues.Add(StudentLoanEntity.Field.Loan37, new_student_loan.Loan37);
                                fieldValues.Add(StudentLoanEntity.Field.Loan38, new_student_loan.Loan38);
                                fieldValues.Add(StudentLoanEntity.Field.Loan39, new_student_loan.Loan39);
                                fieldValues.Add(StudentLoanEntity.Field.Loan40, new_student_loan.Loan40);
                                fieldValues.Add(StudentLoanEntity.Field.LoanFixamount, new_student_loan.LoanFixamount);
                                fieldValues.Add(StudentLoanEntity.Field.LoanAmount, new_student_loan.LoanAmount);

                                result = tsFactory.UpdateFields<StudentLoanEntity>(fieldValues, where, out count);
                                if (result.IsSuccess && count == 0)
                                {
                                    result = new Result(false, "無資料被更新", ErrorCode.D_DATA_NOT_FOUND, null);
                                }
                                if (result.IsSuccess)
                                {
                                    commit = true;
                                }
                                else
                                {
                                    logs.AppendLine(string.Format("第{0}筆更新學生就貸資料發生錯誤，錯誤訊息：{1}", line, result.Message));
                                }
                                #endregion
                            }
                            else
                            {
                                #region 新增
                                result = tsFactory.Insert(new_student_loan, out count);
                                if (result.IsSuccess && count > 0)
                                {
                                    commit = true;
                                }
                                else
                                {
                                    logs.AppendLine(string.Format("第{0}筆更新學生就貸資料發生錯誤，錯誤訊息={1}", line, result.Message));
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 更新就貸代碼檔
                    if (commit)
                    {
                        string log = "";
                        if (updateLoanList(tsFactory, receiveType, yearId, termId, depId, loan_id, loan_name, out log))
                        {
                            tsFactory.Commit();
                            successCount++;
                        }
                        else
                        {
                            logs.AppendLine(string.Format("第{0}筆更新就貸代碼發生錯誤，錯誤訊息={1}", line, log));
                            tsFactory.Rollback();
                        }
                    }
                    else
                    {
                        tsFactory.Rollback();
                    }
                    #endregion

                    #endregion
                    #endregion
                }
            }

            logmsg = logs.ToString();
            return new Result(true, logmsg, "0000", null);
            #endregion
        }

        private void setLoanItemAmountofStudentLoan(string loan_item_name,decimal loan_item_amount,ref StudentLoanEntity student_loan)
        {
            if(loan_item_name.ToLower()=="lo_1")
            {
                student_loan.Loan01 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_2")
            {
                student_loan.Loan02 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_3")
            {
                student_loan.Loan03 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_4")
            {
                student_loan.Loan04 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_5")
            {
                student_loan.Loan05 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_6")
            {
                student_loan.Loan06 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_7")
            {
                student_loan.Loan07 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_8")
            {
                student_loan.Loan08 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_9")
            {
                student_loan.Loan09 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_10")
            {
                student_loan.Loan10 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_11")
            {
                student_loan.Loan11 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_12")
            {
                student_loan.Loan12 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_13")
            {
                student_loan.Loan13 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_14")
            {
                student_loan.Loan14 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_15")
            {
                student_loan.Loan15 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_16")
            {
                student_loan.Loan16 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_17")
            {
                student_loan.Loan17 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_18")
            {
                student_loan.Loan18 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_19")
            {
                student_loan.Loan19 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_20")
            {
                student_loan.Loan20 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_21")
            {
                student_loan.Loan21 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_22")
            {
                student_loan.Loan22 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_23")
            {
                student_loan.Loan23 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_24")
            {
                student_loan.Loan24 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_25")
            {
                student_loan.Loan25 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_26")
            {
                student_loan.Loan26 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_27")
            {
                student_loan.Loan27 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_28")
            {
                student_loan.Loan28 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_29")
            {
                student_loan.Loan29 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_30")
            {
                student_loan.Loan30 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_31")
            {
                student_loan.Loan31 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_32")
            {
                student_loan.Loan32 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_33")
            {
                student_loan.Loan33 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_34")
            {
                student_loan.Loan34 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_35")
            {
                student_loan.Loan35 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_36")
            {
                student_loan.Loan36 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_37")
            {
                student_loan.Loan37 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_38")
            {
                student_loan.Loan38 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_39")
            {
                student_loan.Loan39 = loan_item_amount;
            }
            if (loan_item_name.ToLower() == "lo_40")
            {
                student_loan.Loan40 = loan_item_amount;
            }
        }

        private StudentReceiveEntity getStudentReceive(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string stu_id, string cancel_no,out string errmsg)
        {
            errmsg = null;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},cancel_no={6},receive_way='' or null", receive_type, year_id, term_id, dep_id, receive_id, stu_id, cancel_no);

            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive_type)
                .And(StudentReceiveEntity.Field.YearId, year_id)
                .And(StudentReceiveEntity.Field.TermId, term_id)
                .And(StudentReceiveEntity.Field.DepId, dep_id)
                .And(StudentReceiveEntity.Field.ReceiveId, receive_id)
                .And(StudentReceiveEntity.Field.StuId, stu_id)
                .And(StudentReceiveEntity.Field.CancelNo, cancel_no)
                .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));  //未繳款

            KeyValueList<OrderByEnum> orderbys = null;

            StudentReceiveEntity student_receive = null;
            StudentReceiveEntity[] student_receives = null;

            Result result = _Factory.Select<StudentReceiveEntity>(where, orderbys, 0, 2, out student_receives);  //最多取兩筆就好，因為超過 1 筆就算錯
            if(result.IsSuccess)
            {
                if(student_receives!=null && student_receives.Length>0)
                {
                    if(student_receives.Length==1)
                    {
                        student_receive = student_receives[0];
                    }
                    else
                    {
                        student_receive = null;
                        errmsg = string.Format("查詢studnet_receive發生錯誤，錯誤訊息={0},key={1}", "查到超過一筆的student_receive",key);
                    }
                }
                else
                {
                    student_receive = null;
                    errmsg = string.Format("查詢studnet_receive發生錯誤，錯誤訊息={0},key={1}", "查無符合的資料",key);
                }
            }
            else
            {
                student_receive = null;
                errmsg = string.Format("查詢studnet_receive發生錯誤，錯誤訊息={0},key={1}", result.Message,key);
            }
            return student_receive;
        }

        private StudentLoanEntity getStudentLoan(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string stu_id, string loan_id, int old_seq, out string errmsg)
        {
            errmsg = null;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},loan_id={6},old_seq={7}", receive_type, year_id, term_id, dep_id, receive_id, stu_id, loan_id, old_seq);

            Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, receive_type)
                .And(StudentLoanEntity.Field.YearId, year_id)
                .And(StudentLoanEntity.Field.TermId, term_id)
                .And(StudentLoanEntity.Field.DepId, dep_id)
                .And(StudentLoanEntity.Field.ReceiveId, receive_id)
                .And(StudentLoanEntity.Field.StuId, stu_id)
                .And(StudentLoanEntity.Field.OldSeq, old_seq);
            if (!String.IsNullOrEmpty(loan_id))
            {
                where.And(StudentLoanEntity.Field.LoanId, loan_id);  //同一個學生同一張繳單只會有一筆貸款
            }

            KeyValueList<OrderByEnum> orderbys = null;

            StudentLoanEntity student_loan = null;
            Result result = _Factory.SelectFirst<StudentLoanEntity>(where, orderbys, out student_loan);
            if (result.IsSuccess)
            {
                if (student_loan == null)
                {
                    errmsg = string.Format("查詢student_loan發生錯誤，錯誤訊息={0},key={1}", "查無符合的資料", key);
                }
            }
            else
            {
                student_loan = null;
                errmsg = string.Format("查詢student_loan發生錯誤，錯誤訊息={0},key={1}", result.Message, key);
            }
            return student_loan;
        }

        private bool updateLoanList(EntityFactory tsFactory, string receive_type, string year_id, string term_id, string dep_id, string loan_id, string loan_name, out string errmsg)
        {
            errmsg = null;

            LoanListEntity loan_list = getLoanList(receive_type, year_id, term_id, dep_id, loan_id, out errmsg);
            if (!String.IsNullOrEmpty(errmsg))
            {
                return false;
            }

            bool isOK = false;
            int count = 0;
            if (loan_list != null)
            {
                if (!loan_list.LoanName.Equals(loan_name, StringComparison.InvariantCultureIgnoreCase))
                {
                    #region 更新
                    loan_list.LoanName = loan_name;
                    loan_list.MdyDate = DateTime.Now;
                    loan_list.MdyUser = "SYSTEM";
                    Result result = tsFactory.Update(loan_list, out count);
                    if(result.IsSuccess)
                    {
                        if (count > 0)
                        {
                            isOK = true;
                        }
                        else
                        {
                            errmsg = "無資料被更新";
                        }
                    }
                    else
                    {
                        errmsg = result.Message;
                    }
                    #endregion
                }
                else
                {
                    isOK = true;
                }
            }
            else
            {
                #region 新增
                loan_list = new LoanListEntity();
                loan_list.ReceiveType = receive_type;
                loan_list.YearId = year_id;
                loan_list.TermId = term_id;
                loan_list.DepId = dep_id;
                loan_list.LoanId = loan_id;
                loan_list.LoanName = loan_name;
                loan_list.Status = "0";
                loan_list.CrtDate = DateTime.Now;
                loan_list.CrtUser = "SYSTEM";
                Result result = tsFactory.Insert(loan_list, out count);
                if(result.IsSuccess)
                {
                    if (count > 0)
                    {
                        isOK = true;
                    }
                    else
                    {
                        errmsg = "無資料被新增";
                    }
                }
                else
                {
                    errmsg = string.Format(result.Message);
                }
                #endregion
            }
            return isOK;
        }

        private LoanListEntity getLoanList(string receive_type, string year_id, string term_id, string dep_id, string loan_id, out string errmsg)
        {
            errmsg = null;
            LoanListEntity loan_list = null;
            Expression where = new Expression(LoanListEntity.Field.ReceiveType, receive_type)
                .And(LoanListEntity.Field.YearId, year_id)
                .And(LoanListEntity.Field.TermId, term_id)
                .And(LoanListEntity.Field.DepId, dep_id)
                .And(LoanListEntity.Field.LoanId, loan_id);
            KeyValueList<OrderByEnum> orderbys = null;

            Result result = _Factory.SelectFirst<LoanListEntity>(where, orderbys, out loan_list);
            if (!result.IsSuccess)
            {
                errmsg = result.Message;
            }
            return loan_list;
        }
        #endregion

        #region Import BUE File(上傳已產生銷帳編號之減免資料)
        /// <summary>
        /// 匯入 BUE (上傳已產生銷帳編號之減免資料) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUEJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUE)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;
            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼或學期代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            string owner = null;
            string mappingId = null;
            string sheetName = null;
            string fileType = null;
            int cancel = 0;
            int seriroNo = 0;
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pFileName = null;
                string pCancel = null;
                string pSeriroNo = null;
                isParamOK = JobcubeEntity.ParseBUEParameter(job.Jparam, out owner, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out mappingId, out pFileName, out sheetName, out pCancel, out pSeriroNo);
                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (String.IsNullOrEmpty(mappingId)
                    || (fileType != "xls" && fileType != "xlsx" && fileType != "txt" && fileType != "ods")
                    || String.IsNullOrEmpty(pCancel) || !Int32.TryParse(pCancel, out cancel) || cancel < 1
                    || String.IsNullOrEmpty(pSeriroNo) || !Int32.TryParse(pSeriroNo, out seriroNo) || seriroNo < 1)
                {
                    logmsg = "批次處理序列缺少對照表代碼、上傳檔案序號或批次號碼的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 取得虛擬帳號模組資料
            CancelNoHelper cnoHelper = new CancelNoHelper();
            CancelNoHelper.Module module = cnoHelper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                logmsg = String.Format("無法取得商家代號 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
            }
            #endregion

            #region 代收管道設定
            bool hasSMChannel = false;
            bool hasCashChannel = false;
            {
                ChannelHelper helper = new ChannelHelper();
                string errmsg = helper.CheckReceiveChannel(receiveType, out hasSMChannel, out hasCashChannel);
                if (!String.IsNullOrEmpty(errmsg))
                {
                    logmsg = String.Format("無法取得商家代號 {0} 的代收管道設定，錯誤訊息：{1}", receiveType, errmsg);
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                {
                    fileContent = encoding.GetBytes(textContent);
                }
                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取對照表
            MappingrrTxtEntity txtMapping = null;
            MappingrrXlsmdbEntity xlsMapping = null;
            {
                if (fileType == "txt")
                {
                    Expression where = new Expression(MappingrrTxtEntity.Field.MappingId, mappingId)
                        .And(MappingrrTxtEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingrrTxtEntity>(where, null, out txtMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取文字格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (txtMapping == null)
                    {
                        logmsg = String.Format("查無 代碼 {0}、商家代號 {1} 的文字格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    Expression where = new Expression(MappingrrXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingrrXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingrrXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Excel 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Excel 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                }
                else if (fileType == "ods")
                {
                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    Expression where = new Expression(MappingrrXlsmdbEntity.Field.MappingId, mappingId)
                        .And(MappingrrXlsmdbEntity.Field.ReceiveType, receiveType);
                    Result result = _Factory.SelectFirst<MappingrrXlsmdbEntity>(where, null, out xlsMapping);
                    if (!result.IsSuccess)
                    {
                        logmsg = "讀取 Calc 格式的上傳繳費資料對照表資料失敗，" + result.Message;
                        return new Result(false, logmsg, result.Code, result.Exception);
                    }
                    if (xlsMapping == null)
                    {
                        logmsg = String.Format("查無 代碼{0}、商家代號{1} 的 Calc 格式的上傳繳費資料對照表資料", mappingId, receiveType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();
            {
                string errmsg = null;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                ConvertFileHelper helper = new ConvertFileHelper();
                if (fileType == "txt")
                {
                    #region Txt 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        using (StreamReader reader = new StreamReader(ms, encoding))
                        {
                            isOK = helper.Txt2DataTable(reader, txtMapping.GetMapFields(), isBatch, true, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    #region Xls | Xlsx 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        if (fileType == "xls")
                        {
                            isOK = helper.Xls2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else
                        {
                            isOK = helper.Xlsx2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else if (fileType == "ods")
                {
                    #region Ods 轉 DataTable
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Ods2DataTable(ms, sheetName, xlsMapping.GetMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            logmsg = log.ToString();
                        }
                        return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                    #endregion
                }
                else
                {
                    #region 不支援
                    {
                        logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
                #endregion

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }
            }
            #endregion

            #region 使用交易將 DataTable 逐筆存入資料庫
            #region [MDY:20190906] (2019擴充案) 修正
            StringBuilder logs = new StringBuilder();
            Int32 line = 0;
            successCount = 0;

            using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
            {
                BillAmountHelper amountHelper = new BillAmountHelper();

                foreach (DataRow row in table.Rows)
                {
                    #region 處理上傳的每一筆資料
                    line++;

                    StudentReduceEntity new_student_reduce = new StudentReduceEntity();
                    #region 讀資料
                    //代收類別
                    new_student_reduce.ReceiveType = receiveType;
                    //學年
                    new_student_reduce.YearId = yearId;
                    //學期
                    new_student_reduce.TermId = termId;
                    //部別
                    new_student_reduce.DepId = depId;
                    //費用別
                    new_student_reduce.ReceiveId = receiveId;

                    #region 學號
                    string stu_id = "";
                    if (table.Columns.Contains("s_id"))
                    {
                        if (!Convert.IsDBNull(row["s_id"]))
                        {
                            stu_id = Convert.ToString(row["s_id"]).Trim();
                        }
                    }
                    new_student_reduce.StuId = stu_id;
                    if (String.IsNullOrWhiteSpace(new_student_reduce.StuId))  //必要欄位
                    {
                        logs.AppendFormat("第{0}筆缺少學號資料", line).AppendLine();
                        continue;
                    }
                    #endregion

                    #region 減免不上傳銷帳編號
                    //string cancel_no = "";
                    //if (table.Columns.Contains("cancel_no"))
                    //{
                    //    if (!Convert.IsDBNull(row["cancel_no"]))
                    //    {
                    //        cancel_no = Convert.ToString(row["cancel_no"]).Trim();
                    //    }
                    //}
                    //if (String.IsNullOrWhiteSpace(cancel_no))   //必要欄位
                    //{
                    //    logs.AppendFormat("第{0}筆缺少銷帳編號資料", line).AppendLine();
                    //    continue;
                    //}
                    #endregion

                    //轉成table那裏好像寫反了
                    #region 減免代碼
                    string reduce_id = "";
                    if (table.Columns.Contains("rr_name"))
                    {
                        if (!Convert.IsDBNull(row["rr_name"]))
                        {
                            reduce_id = Convert.ToString(row["rr_name"]).Trim();
                        }
                    }
                    new_student_reduce.ReduceId = reduce_id;
                    #endregion

                    #region 減免名稱
                    string reduce_name = "";
                    if (table.Columns.Contains("rr_count"))
                    {
                        if (!Convert.IsDBNull(row["rr_count"]))
                        {
                            reduce_name = Convert.ToString(row["rr_count"]).Trim();
                        }
                    }
                    #endregion

                    #region 減免金額
                    Int32 reduce_amount = 0;
                    if (table.Columns.Contains("amount"))
                    {
                        if (!Convert.IsDBNull(row["amount"]))
                        {
                            reduce_amount = Convert.ToInt32(row["amount"]);
                        }
                    }
                    new_student_reduce.ReduceAmount = reduce_amount;
                    #endregion

                    #region 減免明細
                    Int32 reduce_total = 0;
                    {
                        for (int no = 1; no <= 40; no++)
                        {
                            int itemAmount = 0;
                            string fieldName = string.Format("rr_{0}", no);
                            if (table.Columns.Contains(fieldName) && !row.IsNull(fieldName))
                            {
                                itemAmount = Convert.ToInt32(row[fieldName]);
                            }
                            //null 或未上傳的項目都視為 0，以方便後面計算
                            new_student_reduce.SetReduceItemAmount(no, itemAmount);
                            reduce_total += itemAmount;
                        }
                    }
                    //new_student_reduce.ReduceAmount = reduce_total;
                    #endregion

                    #region 檢查明細與總金額
                    if (new_student_reduce.ReduceAmount == 0)
                    {
                        //ReduceAmount = 0 視為沒有上傳減免金額欄位
                        new_student_reduce.ReduceAmount = reduce_total;
                    }
                    else if (reduce_total != 0 && new_student_reduce.ReduceAmount != reduce_total)
                    {
                        //reduce_total != 0 視為有上傳減免明細
                        logs.AppendFormat("第{0}筆的減免明細金額與總額不符", line).AppendLine();
                        continue;
                    }
                    #endregion
                    #endregion

                    StudentReceiveEntity studnet_receive = null;
                    StudentReduceEntity student_reduce = null;
                    #region 讀取 student_reduce 及 student_receive
                    {
                        string errmsg = "";
                        //因為怕有重複學號的，所以讀取的方式先去讀取student_receive確認銷編對應的old_seq再去讀取student_reduce
                        studnet_receive = getStudentReceive(receiveType, yearId, termId, depId, receiveId, stu_id, out errmsg);
                        if (studnet_receive != null)
                        {
                            if (!String.IsNullOrEmpty(studnet_receive.ReceiveWay) || !String.IsNullOrEmpty(studnet_receive.ReceiveDate) || !String.IsNullOrEmpty(studnet_receive.AccountDate))
                            {
                                //錯誤鍵換下一筆
                                logs.AppendFormat("第{0}筆的學生繳費資料已繳款，不可在修改", line).AppendLine();
                                continue;
                            }

                            int old_seq = new_student_reduce.OldSeq = studnet_receive.OldSeq;
                            string reduce_id_1 = studnet_receive.ReduceId;
                            student_reduce = getStudentReduce(receiveType, yearId, termId, depId, receiveId, stu_id, old_seq, reduce_id_1, out errmsg);
                        }
                        else
                        {
                            //讀不到student_receive就換下一筆
                            logs.AppendFormat("讀取第{0}筆的學生繳費資料發生錯誤，錯誤訊息：{1}", line, errmsg).AppendLine();
                            continue;
                        }
                    }
                    #endregion

                    #region 土銀說要自動計算金額與虛擬帳號
                    string orgSeriorNo = studnet_receive.SeriorNo;
                    string orgCancelNo = studnet_receive.CancelNo;
                    decimal? orgReceiveAmount = studnet_receive.ReceiveAmount;
                    string cancelNo = null;
                    string seriorNo = null;
                    string checksum = null;
                    string atmCancelNo = null;
                    string smCancelNo = null;
                    {
                        #region 計算新的應繳金額 (因為土銀說減免項目金額不要去異動 Student_Receive 的收入科目金額，所以直接先加回舊減免總額再減掉新的減免總額)
                        studnet_receive.FeePayable = new_student_reduce.ReduceAmount.Value;
                        if (!amountHelper.CalcBillAmount(ref studnet_receive, BillAmountHelper.CalculateType.byAmount, hasCashChannel, hasSMChannel))
                        {
                            logs.AppendLine(string.Format("計算第{0}筆的應繳金額失敗，錯誤訊息：{1}", line, amountHelper.err_mgs));
                            continue;
                        }
                        #endregion

                        #region 計算新的虛擬帳號
                        string errmsg = cnoHelper.TryGenCancelNo(_Factory, module, studnet_receive, school.IsBigReceiveId(), out cancelNo, out seriorNo, out checksum, out atmCancelNo, out smCancelNo, true);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            logs.AppendLine(string.Format("產生第{0}筆的虛擬帳號失敗，錯誤訊息：{1}", line, errmsg));
                            continue;
                        }
                        #endregion
                    }
                    #endregion

                    #region 寫入資料庫
                    bool commit = false;

                    #region [Old] 土銀說上傳的減免項目金額不用去異動 Student_Receive 的收入科目金額，所以不用計算也不用更新收入科目
                    //#region 判斷 student_reduce 是否存在，存在的話先還原金額，再更新，不存在就新增
                    //if (student_reduce != null)
                    //{
                    //    #region 有舊的 student_reduce
                    //    int count = 0;
                    //    Result result = null;

                    //    #region 更新 StudentReceive
                    //    {
                    //        #region 設定 where 條件
                    //        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, studnet_receive.ReceiveType)
                    //            .And(StudentReceiveEntity.Field.YearId, studnet_receive.YearId)
                    //            .And(StudentReceiveEntity.Field.TermId, studnet_receive.TermId)
                    //            .And(StudentReceiveEntity.Field.DepId, studnet_receive.DepId)
                    //            .And(StudentReceiveEntity.Field.ReceiveId, studnet_receive.ReceiveId)
                    //            .And(StudentReceiveEntity.Field.StuId, studnet_receive.StuId)
                    //            .And(StudentReceiveEntity.Field.OldSeq, studnet_receive.OldSeq)
                    //            .And(StudentReceiveEntity.Field.CancelNo, studnet_receive.CancelNo);

                    //        #region 確保未繳款
                    //        where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                    //            .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                    //            .And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                    //        #endregion
                    //        #endregion

                    //        #region 設定要更新的欄位
                    //        KeyValueList fieldValues = new KeyValueList();

                    //        #region 收入科目 (還原 reduce amount 再扣掉新的 reduce amount)
                    //        string[] receiveItemFieldNames = new string[] {
                    //            StudentReceiveEntity.Field.Receive01, StudentReceiveEntity.Field.Receive02, StudentReceiveEntity.Field.Receive03, StudentReceiveEntity.Field.Receive04, StudentReceiveEntity.Field.Receive05,
                    //            StudentReceiveEntity.Field.Receive06, StudentReceiveEntity.Field.Receive07, StudentReceiveEntity.Field.Receive08, StudentReceiveEntity.Field.Receive09, StudentReceiveEntity.Field.Receive10,
                    //            StudentReceiveEntity.Field.Receive11, StudentReceiveEntity.Field.Receive12, StudentReceiveEntity.Field.Receive13, StudentReceiveEntity.Field.Receive14, StudentReceiveEntity.Field.Receive15,
                    //            StudentReceiveEntity.Field.Receive16, StudentReceiveEntity.Field.Receive17, StudentReceiveEntity.Field.Receive18, StudentReceiveEntity.Field.Receive19, StudentReceiveEntity.Field.Receive20,
                    //            StudentReceiveEntity.Field.Receive21, StudentReceiveEntity.Field.Receive22, StudentReceiveEntity.Field.Receive23, StudentReceiveEntity.Field.Receive24, StudentReceiveEntity.Field.Receive25,
                    //            StudentReceiveEntity.Field.Receive26, StudentReceiveEntity.Field.Receive27, StudentReceiveEntity.Field.Receive28, StudentReceiveEntity.Field.Receive29, StudentReceiveEntity.Field.Receive30,
                    //            StudentReceiveEntity.Field.Receive31, StudentReceiveEntity.Field.Receive32, StudentReceiveEntity.Field.Receive33, StudentReceiveEntity.Field.Receive34, StudentReceiveEntity.Field.Receive35,
                    //            StudentReceiveEntity.Field.Receive36, StudentReceiveEntity.Field.Receive37, StudentReceiveEntity.Field.Receive38, StudentReceiveEntity.Field.Receive39, StudentReceiveEntity.Field.Receive40
                    //        };
                    //        decimal?[] oldReceiveItemAmounts = studnet_receive.GetAllReceiveItemAmounts();
                    //        for (int no = 1; no <= 40; no++)
                    //        {
                    //            decimal? oldReceiveItemAmount = oldReceiveItemAmounts[no - 1];
                    //            if (oldReceiveItemAmount.HasValue)
                    //            {
                    //                decimal oldReduceItemAmount = student_reduce.GetReduceItemAmount(no) ?? 0;
                    //                decimal newReduceItemAmount = new_student_reduce.GetReduceItemAmount(no) ?? 0;
                    //                decimal newReceiveItemAmount = oldReceiveItemAmount.Value + oldReduceItemAmount - newReduceItemAmount;
                    //                fieldValues.Add(receiveItemFieldNames[no - 1], newReceiveItemAmount);
                    //            }
                    //        }
                    //        #endregion

                    //        fieldValues.Add(StudentReceiveEntity.Field.ReduceId, reduce_id);

                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelNo, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelPono, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelEb1no, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelEb2no, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb1amount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb2amount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, 0);
                    //        #endregion

                    //        result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                    //        if (result.IsSuccess && count == 0)
                    //        {
                    //            result = new Result(false, "更新學生繳費資料失敗，查無該資料或該資料已繳費", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                    //        }
                    //    }
                    //    #endregion

                    //    #region 更新 StudentReduce
                    //    if (result.IsSuccess)
                    //    {
                    //        #region 設定 where 條件
                    //        Expression where = new Expression(StudentReduceEntity.Field.ReceiveType, student_reduce.ReceiveType)
                    //            .And(StudentReduceEntity.Field.YearId, student_reduce.YearId)
                    //            .And(StudentReduceEntity.Field.TermId, student_reduce.TermId)
                    //            .And(StudentReduceEntity.Field.DepId, student_reduce.DepId)
                    //            .And(StudentReduceEntity.Field.ReceiveId, student_reduce.ReceiveId)
                    //            .And(StudentReduceEntity.Field.StuId, student_reduce.StuId)
                    //            .And(StudentReduceEntity.Field.OldSeq, student_reduce.OldSeq)
                    //            .And(StudentReduceEntity.Field.ReduceId, student_reduce.ReduceId);
                    //        #endregion

                    //        #region 設定要更新的欄位
                    //        KeyValueList fieldValues = new KeyValueList();
                    //        string[] reduceItemFieldNames = new string[] {
                    //            StudentReduceEntity.Field.Reduce01, StudentReduceEntity.Field.Reduce02, StudentReduceEntity.Field.Reduce03, StudentReduceEntity.Field.Reduce04, StudentReduceEntity.Field.Reduce05,
                    //            StudentReduceEntity.Field.Reduce06, StudentReduceEntity.Field.Reduce07, StudentReduceEntity.Field.Reduce08, StudentReduceEntity.Field.Reduce09, StudentReduceEntity.Field.Reduce10,
                    //            StudentReduceEntity.Field.Reduce11, StudentReduceEntity.Field.Reduce12, StudentReduceEntity.Field.Reduce13, StudentReduceEntity.Field.Reduce14, StudentReduceEntity.Field.Reduce15,
                    //            StudentReduceEntity.Field.Reduce16, StudentReduceEntity.Field.Reduce17, StudentReduceEntity.Field.Reduce18, StudentReduceEntity.Field.Reduce19, StudentReduceEntity.Field.Reduce20,
                    //            StudentReduceEntity.Field.Reduce21, StudentReduceEntity.Field.Reduce22, StudentReduceEntity.Field.Reduce23, StudentReduceEntity.Field.Reduce24, StudentReduceEntity.Field.Reduce25,
                    //            StudentReduceEntity.Field.Reduce26, StudentReduceEntity.Field.Reduce27, StudentReduceEntity.Field.Reduce28, StudentReduceEntity.Field.Reduce29, StudentReduceEntity.Field.Reduce30,
                    //            StudentReduceEntity.Field.Reduce31, StudentReduceEntity.Field.Reduce32, StudentReduceEntity.Field.Reduce33, StudentReduceEntity.Field.Reduce34, StudentReduceEntity.Field.Reduce35,
                    //            StudentReduceEntity.Field.Reduce36, StudentReduceEntity.Field.Reduce37, StudentReduceEntity.Field.Reduce38, StudentReduceEntity.Field.Reduce39, StudentReduceEntity.Field.Reduce40
                    //        };

                    //        fieldValues.Add(StudentReduceEntity.Field.ReduceId, new_student_reduce.ReduceId);
                    //        foreach (string reduceItemFieldName in reduceItemFieldNames)
                    //        {
                    //            fieldValues.Add(reduceItemFieldName, new_student_reduce.GetReduceItemAmount(reduceItemFieldName));
                    //        }

                    //        fieldValues.Add(StudentReduceEntity.Field.ReduceAmount, new_student_reduce.ReduceAmount);
                    //        #endregion

                    //        result = tsFactory.UpdateFields<StudentReduceEntity>(fieldValues, where, out count);
                    //        if (result.IsSuccess && count == 0)
                    //        {
                    //            result = new Result(false, "更新學生減免資料失敗，查無該資料", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                    //        }
                    //    }
                    //    #endregion

                    //    if (result.IsSuccess)
                    //    {
                    //        commit = true;
                    //    }
                    //    else
                    //    {
                    //        logs.AppendFormat("處理第{0}筆資料發生錯誤，錯誤訊息：{1}", line, result.Message).AppendLine();
                    //    }
                    //    #endregion
                    //}
                    //else
                    //{
                    //    #region 無舊的 studnet_reduce
                    //    int count = 0;
                    //    Result result = null;

                    //    #region 更新 StudentReceive
                    //    {
                    //        #region 設定 where 條件
                    //        Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, studnet_receive.ReceiveType)
                    //            .And(StudentReceiveEntity.Field.YearId, studnet_receive.YearId)
                    //            .And(StudentReceiveEntity.Field.TermId, studnet_receive.TermId)
                    //            .And(StudentReceiveEntity.Field.DepId, studnet_receive.DepId)
                    //            .And(StudentReceiveEntity.Field.ReceiveId, studnet_receive.ReceiveId)
                    //            .And(StudentReceiveEntity.Field.StuId, studnet_receive.StuId)
                    //            .And(StudentReceiveEntity.Field.OldSeq, studnet_receive.OldSeq);

                    //        #region 確保未繳款
                    //        where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                    //            .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                    //            .And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                    //        #endregion
                    //        #endregion

                    //        #region 設定要更新的欄位
                    //        KeyValueList fieldValues = new KeyValueList();

                    //        #region 收入科目 (還原 reduce amount 再扣掉新的 reduce amount)
                    //        string[] receiveItemFieldNames = new string[] {
                    //            StudentReceiveEntity.Field.Receive01, StudentReceiveEntity.Field.Receive02, StudentReceiveEntity.Field.Receive03, StudentReceiveEntity.Field.Receive04, StudentReceiveEntity.Field.Receive05,
                    //            StudentReceiveEntity.Field.Receive06, StudentReceiveEntity.Field.Receive07, StudentReceiveEntity.Field.Receive08, StudentReceiveEntity.Field.Receive09, StudentReceiveEntity.Field.Receive10,
                    //            StudentReceiveEntity.Field.Receive11, StudentReceiveEntity.Field.Receive12, StudentReceiveEntity.Field.Receive13, StudentReceiveEntity.Field.Receive14, StudentReceiveEntity.Field.Receive15,
                    //            StudentReceiveEntity.Field.Receive16, StudentReceiveEntity.Field.Receive17, StudentReceiveEntity.Field.Receive18, StudentReceiveEntity.Field.Receive19, StudentReceiveEntity.Field.Receive20,
                    //            StudentReceiveEntity.Field.Receive21, StudentReceiveEntity.Field.Receive22, StudentReceiveEntity.Field.Receive23, StudentReceiveEntity.Field.Receive24, StudentReceiveEntity.Field.Receive25,
                    //            StudentReceiveEntity.Field.Receive26, StudentReceiveEntity.Field.Receive27, StudentReceiveEntity.Field.Receive28, StudentReceiveEntity.Field.Receive29, StudentReceiveEntity.Field.Receive30,
                    //            StudentReceiveEntity.Field.Receive31, StudentReceiveEntity.Field.Receive32, StudentReceiveEntity.Field.Receive33, StudentReceiveEntity.Field.Receive34, StudentReceiveEntity.Field.Receive35,
                    //            StudentReceiveEntity.Field.Receive36, StudentReceiveEntity.Field.Receive37, StudentReceiveEntity.Field.Receive38, StudentReceiveEntity.Field.Receive39, StudentReceiveEntity.Field.Receive40
                    //        };
                    //        decimal?[] oldReceiveItemAmounts = studnet_receive.GetAllReceiveItemAmounts();
                    //        for (int no = 1; no <= 40; no++)
                    //        {
                    //            decimal? oldReceiveItemAmount = oldReceiveItemAmounts[no - 1];
                    //            if (oldReceiveItemAmount.HasValue)
                    //            {
                    //                decimal newReduceItemAmount = new_student_reduce.GetReduceItemAmount(no) ?? 0;
                    //                decimal newReceiveItemAmount = oldReceiveItemAmount.Value - newReduceItemAmount;
                    //                fieldValues.Add(receiveItemFieldNames[no - 1], newReceiveItemAmount);
                    //            }
                    //        }
                    //        #endregion

                    //        fieldValues.Add(StudentReceiveEntity.Field.ReduceId, reduce_id);

                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelNo, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelPono, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelEb1no, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelEb2no, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, "");
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb1amount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb2amount, 0);
                    //        fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, 0);
                    //        #endregion

                    //        result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                    //        if (result.IsSuccess && count == 0)
                    //        {
                    //            result = new Result(false, "更新學生繳費資料失敗，查無該資料或該資料已繳費", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                    //        }
                    //    }
                    //    #endregion

                    //    #region 新增 StudentReduce
                    //    if (result.IsSuccess)
                    //    {
                    //        result = tsFactory.Insert(new_student_reduce, out count);
                    //        if (result.IsSuccess && count == 0)
                    //        {
                    //            result = new Result(false, "新增學生減免資料失敗，無資料被新增", CoreStatusCode.D_NOT_DATA_INSERT, null);
                    //        }
                    //    }
                    //    #endregion

                    //    if (result.IsSuccess)
                    //    {
                    //        commit = true;
                    //    }
                    //    else
                    //    {
                    //        commit = false;
                    //        logs.AppendFormat("處理第{0}筆資料發生錯誤，錯誤訊息：{1}", line, result.Message).AppendLine();
                    //    }
                    //    #endregion
                    //}
                    //#endregion
                    #endregion

                    #region [New] 土銀說上傳的減免項目金額不用去異動 Student_Receive 的收入科目金額，所以不用計算也不用更新收入科目
                    {
                        Result result = new Result(false);
                        int count = 0;

                        #region 更新 StudentReceive
                        if (studnet_receive != null)
                        {
                            #region 設定 where 條件
                            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, studnet_receive.ReceiveType)
                                .And(StudentReceiveEntity.Field.YearId, studnet_receive.YearId)
                                .And(StudentReceiveEntity.Field.TermId, studnet_receive.TermId)
                                .And(StudentReceiveEntity.Field.DepId, studnet_receive.DepId)
                                .And(StudentReceiveEntity.Field.ReceiveId, studnet_receive.ReceiveId)
                                .And(StudentReceiveEntity.Field.StuId, studnet_receive.StuId)
                                .And(StudentReceiveEntity.Field.OldSeq, studnet_receive.OldSeq)
                                .And(StudentReceiveEntity.Field.CancelNo, studnet_receive.CancelNo);

                            #region 確保未繳款
                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty))
                                .And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty))
                                .And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                            #endregion
                            #endregion

                            #region 設定要更新的欄位
                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(StudentReceiveEntity.Field.ReduceId, reduce_id);

                            fieldValues.Add(StudentReceiveEntity.Field.FeePayable, new_student_reduce.ReduceAmount.Value);

                            if (seriorNo != orgSeriorNo)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.SeriorNo, seriorNo);
                            }
                            if (cancelNo != orgCancelNo)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.CancelNo, cancelNo);
                            }
                            fieldValues.Add(StudentReceiveEntity.Field.CancelAtmno, atmCancelNo);
                            fieldValues.Add(StudentReceiveEntity.Field.CancelSmno, smCancelNo);
                            fieldValues.Add(StudentReceiveEntity.Field.CancelPono, "");
                            fieldValues.Add(StudentReceiveEntity.Field.CancelEb1no, "");
                            fieldValues.Add(StudentReceiveEntity.Field.CancelEb2no, "");
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, studnet_receive.ReceiveAmount);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveAtmamount, studnet_receive.ReceiveAtmamount);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveSmamount, studnet_receive.ReceiveSmamount);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb1amount, 0);
                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveEb2amount, 0);

                            #region 虛擬帳號或應繳金額有異動則中信資料發送旗標清為 0
                            if (orgCancelNo != cancelNo || orgReceiveAmount != studnet_receive.ReceiveAmount)
                            {
                                fieldValues.Add(StudentReceiveEntity.Field.CFlag, "0");
                            }
                            #endregion

                            fieldValues.Add(StudentReceiveEntity.Field.UpdateDate, DateTime.Now);
                            #endregion

                            result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                            if (result.IsSuccess && count == 0)
                            {
                                result = new Result(false, "更新學生繳費資料失敗，查無該資料或該資料已繳費", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                            }
                        }
                        #endregion

                        #region 更新或新增 StudentReduce
                        if (result.IsSuccess)
                        {
                            if (student_reduce != null)
                            {
                                #region 更新 StudentReduce
                                #region 設定 where 條件
                                Expression where = new Expression(StudentReduceEntity.Field.ReceiveType, student_reduce.ReceiveType)
                                    .And(StudentReduceEntity.Field.YearId, student_reduce.YearId)
                                    .And(StudentReduceEntity.Field.TermId, student_reduce.TermId)
                                    .And(StudentReduceEntity.Field.DepId, student_reduce.DepId)
                                    .And(StudentReduceEntity.Field.ReceiveId, student_reduce.ReceiveId)
                                    .And(StudentReduceEntity.Field.StuId, student_reduce.StuId)
                                    .And(StudentReduceEntity.Field.OldSeq, student_reduce.OldSeq)
                                    .And(StudentReduceEntity.Field.ReduceId, student_reduce.ReduceId);
                                #endregion

                                #region 設定要更新的欄位
                                KeyValueList fieldValues = new KeyValueList();
                                string[] reduceItemFieldNames = new string[] {
                                    StudentReduceEntity.Field.Reduce01, StudentReduceEntity.Field.Reduce02, StudentReduceEntity.Field.Reduce03, StudentReduceEntity.Field.Reduce04, StudentReduceEntity.Field.Reduce05,
                                    StudentReduceEntity.Field.Reduce06, StudentReduceEntity.Field.Reduce07, StudentReduceEntity.Field.Reduce08, StudentReduceEntity.Field.Reduce09, StudentReduceEntity.Field.Reduce10,
                                    StudentReduceEntity.Field.Reduce11, StudentReduceEntity.Field.Reduce12, StudentReduceEntity.Field.Reduce13, StudentReduceEntity.Field.Reduce14, StudentReduceEntity.Field.Reduce15,
                                    StudentReduceEntity.Field.Reduce16, StudentReduceEntity.Field.Reduce17, StudentReduceEntity.Field.Reduce18, StudentReduceEntity.Field.Reduce19, StudentReduceEntity.Field.Reduce20,
                                    StudentReduceEntity.Field.Reduce21, StudentReduceEntity.Field.Reduce22, StudentReduceEntity.Field.Reduce23, StudentReduceEntity.Field.Reduce24, StudentReduceEntity.Field.Reduce25,
                                    StudentReduceEntity.Field.Reduce26, StudentReduceEntity.Field.Reduce27, StudentReduceEntity.Field.Reduce28, StudentReduceEntity.Field.Reduce29, StudentReduceEntity.Field.Reduce30,
                                    StudentReduceEntity.Field.Reduce31, StudentReduceEntity.Field.Reduce32, StudentReduceEntity.Field.Reduce33, StudentReduceEntity.Field.Reduce34, StudentReduceEntity.Field.Reduce35,
                                    StudentReduceEntity.Field.Reduce36, StudentReduceEntity.Field.Reduce37, StudentReduceEntity.Field.Reduce38, StudentReduceEntity.Field.Reduce39, StudentReduceEntity.Field.Reduce40
                                };

                                fieldValues.Add(StudentReduceEntity.Field.ReduceId, new_student_reduce.ReduceId);
                                foreach (string reduceItemFieldName in reduceItemFieldNames)
                                {
                                    fieldValues.Add(reduceItemFieldName, new_student_reduce.GetReduceItemAmount(reduceItemFieldName));
                                }

                                fieldValues.Add(StudentReduceEntity.Field.ReduceAmount, new_student_reduce.ReduceAmount);
                                #endregion

                                result = tsFactory.UpdateFields<StudentReduceEntity>(fieldValues, where, out count);
                                if (result.IsSuccess && count == 0)
                                {
                                    result = new Result(false, "更新學生減免資料失敗，查無該資料", CoreStatusCode.D_NOT_DATA_UPDATE, null);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 新增 StudentReduce
                                if (result.IsSuccess)
                                {
                                    result = tsFactory.Insert(new_student_reduce, out count);
                                    if (result.IsSuccess && count == 0)
                                    {
                                        result = new Result(false, "新增學生減免資料失敗，無資料被新增", CoreStatusCode.D_NOT_DATA_INSERT, null);
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        if (result.IsSuccess)
                        {
                            commit = true;
                        }
                        else
                        {
                            commit = false;
                            logs.AppendFormat("處理第{0}筆資料發生錯誤，錯誤訊息：{1}", line, result.Message).AppendLine();
                        }
                    }
                    #endregion

                    #region 更新減免代碼檔
                    if (commit)
                    {
                        string errmsg = "";
                        if (updateReduceList(tsFactory, receiveType, yearId, termId, depId, reduce_id, reduce_name, out errmsg))
                        {
                            commit = true;
                        }
                        else
                        {
                            commit = false;
                            logs.AppendFormat("處理第{0}筆資料發生錯誤，錯誤訊息：更新減免代碼資料失敗，{1}", line, errmsg).AppendLine();
                        }
                    }
                    #endregion

                    if (commit)
                    {
                        tsFactory.Commit();
                        successCount++;
                    }
                    else
                    {
                        tsFactory.Rollback();
                    }
                    #endregion
                    #endregion
                }
            }

            logmsg = logs.ToString();
            return new Result(true, logmsg, CoreStatusCode.NORMAL_STATUS, null);
            #endregion
            #endregion
        }

        private StudentReceiveEntity getStudentReceive(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string stu_id, out string errmsg)
        {
            errmsg = null;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},receive_way='' or null", receive_type, year_id, term_id, dep_id, receive_id, stu_id);

            StudentReceiveEntity[] student_receives = null;
            Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive_type)
                .And(StudentReceiveEntity.Field.YearId, year_id)
                .And(StudentReceiveEntity.Field.TermId, term_id)
                .And(StudentReceiveEntity.Field.DepId, dep_id)
                .And(StudentReceiveEntity.Field.ReceiveId, receive_id)
                .And(StudentReceiveEntity.Field.StuId, stu_id)
                .And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));  //未繳費

            KeyValueList<OrderByEnum> orderbys = null;

            StudentReceiveEntity student_receive = null;
            Result result = _Factory.Select<StudentReceiveEntity>(where, orderbys, 0, 2, out student_receives);  //最多取兩筆就好，因為超過 1 筆就算錯
            if (result.IsSuccess)
            {
                if (student_receives != null && student_receives.Length > 0)
                {
                    if (student_receives.Length == 1)
                    {
                        student_receive = student_receives[0];
                    }
                    else
                    {
                        student_receive = null;
                        errmsg = string.Format("查詢studnet_receive發生錯誤，錯誤訊息={0},key={1}", "查到超過一筆的student_receive", key);
                    }
                }
                else
                {
                    student_receive = null;
                    errmsg = string.Format("查詢studnet_receive發生錯誤，錯誤訊息={0},key={1}", "查無符合的資料", key);
                }
            }
            else
            {
                student_receive = null;
                errmsg = string.Format("查詢studnet_receive發生錯誤，錯誤訊息={0},key={1}", result.Message, key);
            }
            return student_receive;
        }

        private StudentReduceEntity getStudentReduce(string receive_type, string year_id, string term_id, string dep_id, string receive_id, string stu_id, int old_seq, string reduce_id, out string errmsg)
        {
            errmsg = null;
            string key = string.Format("receive_type={0},year_id={1},term_id={2},dep_id={3},receive_id={4},stu_id={5},old_seq={6},reduce_id={7}", receive_type, year_id, term_id, dep_id, receive_id, stu_id, old_seq, reduce_id);

            Expression where = new Expression(StudentReduceEntity.Field.ReceiveType, receive_type)
                .And(StudentReduceEntity.Field.YearId, year_id)
                .And(StudentReduceEntity.Field.TermId, term_id)
                .And(StudentReduceEntity.Field.DepId, dep_id)
                .And(StudentReduceEntity.Field.ReceiveId, receive_id)
                .And(StudentReduceEntity.Field.StuId, stu_id)
                .And(StudentLoanEntity.Field.OldSeq, old_seq);
            if (!String.IsNullOrEmpty(reduce_id))
            {
                where.And(StudentReduceEntity.Field.ReduceId, reduce_id); //同一個學生同一張繳單只應該有一筆減免
            }

            KeyValueList<OrderByEnum> orderbys = null;

            StudentReduceEntity student_reduce = null;
            Result result = _Factory.SelectFirst<StudentReduceEntity>(where, orderbys, out student_reduce);
            if (result.IsSuccess)
            {
                if (student_reduce != null)
                {

                }
                else
                {
                    student_reduce = null;
                    errmsg = string.Format("查詢student_reduce發生錯誤，錯誤訊息={0},key={1}", "查無符合的資料", key);
                }
            }
            else
            {
                student_reduce = null;
                errmsg = string.Format("查詢student_reduce發生錯誤，錯誤訊息={0},key={1}", result.Message, key);
            }
            return student_reduce;
        }

        private bool updateReduceList(EntityFactory tsFactory, string receive_type, string year_id, string term_id, string dep_id, string reduce_id, string reduce_name, out string errmsg)
        {
            errmsg = null;

            ReduceListEntity reduceList = getReduceList(receive_type, year_id, term_id, dep_id, reduce_id, out errmsg);
            if (!String.IsNullOrEmpty(errmsg))
            {
                return false;
            }

            bool isOK = false;
            int count = 0;
            if (reduceList != null)
            {
                if (!reduceList.ReduceName.Equals(reduce_name, StringComparison.InvariantCultureIgnoreCase))
                {
                    #region 更新
                    reduceList.ReduceName = reduce_name;
                    reduceList.MdyDate = DateTime.Now;
                    reduceList.MdyUser = "SYSTEM";
                    Result result = tsFactory.Update(reduceList, out count);
                    if (result.IsSuccess)
                    {
                        if (count > 0)
                        {
                            isOK = true;
                        }
                        else
                        {
                            errmsg = "無資料被更新";
                        }
                    }
                    else
                    {
                        errmsg = result.Message;
                    }
                    #endregion
                }
                else
                {
                    isOK = true;
                }
            }
            else
            {
                #region 新增
                reduceList = new ReduceListEntity();
                reduceList.ReceiveType = receive_type;
                reduceList.YearId = year_id;
                reduceList.TermId = term_id;
                reduceList.DepId = dep_id;
                reduceList.ReduceId = reduce_id;
                reduceList.ReduceName = reduce_name;
                reduceList.Status = "0";
                reduceList.CrtDate = DateTime.Now;
                reduceList.CrtUser = "SYSTEM";
                Result result = tsFactory.Insert(reduceList, out count);
                if (result.IsSuccess)
                {
                    if (count > 0)
                    {
                        isOK = true;
                    }
                    else
                    {
                        errmsg = "無資料被新增";
                    }
                }
                else
                {
                    errmsg = result.Message;
                }
                #endregion
            }
            return isOK;
        }

        private ReduceListEntity getReduceList(string receive_type, string year_id, string term_id, string dep_id, string reduce_id, out string errmsg)
        {
            errmsg = null;
            ReduceListEntity reduceList = null;
            Expression where = new Expression(ReduceListEntity.Field.ReceiveType, receive_type)
                .And(ReduceListEntity.Field.YearId, year_id)
                .And(ReduceListEntity.Field.TermId, term_id)
                .And(ReduceListEntity.Field.DepId, dep_id)
                .And(ReduceListEntity.Field.ReduceId, reduce_id);
            KeyValueList<OrderByEnum> orderbys = null;
            Result result = _Factory.SelectFirst<ReduceListEntity>(where, orderbys, out reduceList);
            if (!result.IsSuccess)
            {
                errmsg = result.Message;
            }
            return reduceList;
        }
        #endregion

        #region Import BUF File(簡易上傳)
        /// <summary>
        /// 匯入 BUF (簡易上傳) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch"></param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUFJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUF)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string yearId = job.Jyear;
            string termId = job.Jterm;
            string depId = job.Jdep;
            string receiveId = job.Jrecid;

            #region [Old] 土銀不使用部別，部別固定為空字串
            //if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId))
            //{
            //    logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼或學期代碼的資料參數或資料不正確", job.Jno);
            //    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            //}
            #endregion

            if (String.IsNullOrEmpty(receiveType) || String.IsNullOrEmpty(yearId) || String.IsNullOrEmpty(termId)
                || depId != null && String.IsNullOrEmpty(receiveId))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號、學年代碼、學期代碼或代收費用別代碼的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            CancelNoHelper cnoHelper = new CancelNoHelper();
            CancelNoHelper.Module module = cnoHelper.GetModuleByReceiveType(receiveType);
            if (module == null)
            {
                logmsg = String.Format("無法取得商家代號 {0} 的虛擬帳號模組資訊", receiveType);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            string owner = null;
            string mappingId = String.Empty;
            string sheetName = null;
            string fileType = null;
            int cancel = 0;
            int seriroNo = 0;
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            {
                string pReceiveType = null;
                string pYearId = null;
                string pTermId = null;
                string pDepId = null;
                string pReceiveId = null;
                string pFileName = null;
                string pCancel = null;
                string pSeriroNo = null;
                isParamOK = JobcubeEntity.ParseBUFParameter(job.Jparam, out owner, out pReceiveType, out pYearId, out pTermId, out pDepId, out pReceiveId
                                , out pFileName, out sheetName, out pCancel, out pSeriroNo);
                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if ((fileType != "xls" && fileType != "xlsx" && fileType != "ods")
                    || String.IsNullOrEmpty(pCancel) || !Int32.TryParse(pCancel, out cancel) || cancel < 1
                    || String.IsNullOrEmpty(pSeriroNo) || !Int32.TryParse(pSeriroNo, out seriroNo) || seriroNo < 1)
                {
                    logmsg = "批次處理序列缺少上傳檔案序號或批次號碼的參數或參數值不正確";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 修正 (不提供 TXT 所以 Mark)
                //if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                //{
                //    fileContent = encoding.GetBytes(textContent);
                //}
                #endregion

                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取對照表
            List<XlsMapField> mapFields = null;
            string[] receiveItemNames = new string[40];   //存放收集的代收科目名稱
            {
                #region 取得上傳檔案的表頭
                List<string> headers = null;
                try
                {
                    string errmsg = null;

                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式 (XLS 與 ODS 用的設定檔相同)
                    ConvertFileHelper helper = new ConvertFileHelper();
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        if (fileType == "xls")
                        {
                            errmsg = helper.GetXlsHeader(ms, sheetName, out headers);
                        }
                        else if (fileType == "xlsx")
                        {
                            errmsg = helper.GetXlsxHeader(ms, sheetName, out headers);
                        }
                        else if (fileType == "ods")
                        {
                            errmsg = helper.GetOdsHeader(ms, sheetName, out headers);
                        }
                        else
                        {
                            errmsg = String.Format("不支援 {0} 格式", fileType);
                        }
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        logmsg = String.Format("讀取上傳檔案表頭失敗，{0}", errmsg);
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                catch(Exception ex)
                {
                    logmsg = String.Format("讀取上傳檔案表頭發生例外，錯誤訊息：{0}", ex.Message);
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                if (headers == null || headers.Count == 0)
                {
                    logmsg = "上傳檔案無表頭資料";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion

                #region 將表頭轉成對照檔
                mapFields = new List<XlsMapField>(headers.Count);
                #region 1. 簡易上傳固定欄位名稱：學號, 身分證字號, 姓名, 年級, 班別, 生日, 減免, 座號, 電子郵件 (選擇性欄位)
                #region 學號
                if (!headers.Contains("學號"))
                {
                    logmsg = "上傳檔案表頭缺少學號";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.StuId, "學號", new CodeChecker(1, 20)));
                #endregion

                #region 身分證字號
                if (!headers.Contains("身分證字號"))
                {
                    logmsg = "上傳檔案表頭缺少身分證字號";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.IdNumber, "身分證字號", new CharChecker(0, 10)));
                #endregion

                #region 姓名
                if (!headers.Contains("姓名"))
                {
                    logmsg = "上傳檔案表頭缺少姓名";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.StuName, "姓名", new WordChecker(1, 60)));
                #endregion

                #region 生日
                if (!headers.Contains("生日"))
                {
                    logmsg = "上傳檔案表頭缺少生日";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.StuBirthday, "生日", new DateTimeChecker(DateTimeChecker.FormatEnum.DateText)));
                #endregion

                #region 電子郵件
                if (headers.Contains("電子郵件"))
                {
                    mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.Email, "電子郵件", new CharChecker(0, 50)));
                }
                #endregion

                #region 年級
                if (!headers.Contains("年級"))
                {
                    logmsg = "上傳檔案表頭缺少年級";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.StuGrade, "年級", new IntegerChecker(0, 12)));
                #endregion

                #region 班別
                if (!headers.Contains("班別"))
                {
                    logmsg = "上傳檔案表頭缺少班別";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.ClassName, "班別", new CodeChecker(1, 20)));
                #endregion

                #region 座號
                if (!headers.Contains("座號"))
                {
                    logmsg = "上傳檔案表頭缺少座號";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                mapFields.Add(new XlsMapField(MappingreXlsmdbEntity.Field.StuHid, "座號", new CodeChecker(1, 10)));
                #endregion

                #region [Old] 減免

                #endregion
                #endregion

                #region 2. 簡易上傳變動欄位名稱：其他名稱都算收入科目，且依序為 收入科目名稱01, 收入科目名稱02 ,,,,,,,,收入科目名稱40
                List<string> fixNames = new List<string>(new string[] { "學號", "身分證字號", "姓名", "年級", "班別", "生日", "減免", "座號", "電子郵件" });
                string[] fields = new string[] {
                    MappingreXlsmdbEntity.Field.Receive1,  MappingreXlsmdbEntity.Field.Receive2,  MappingreXlsmdbEntity.Field.Receive3,  MappingreXlsmdbEntity.Field.Receive4,  MappingreXlsmdbEntity.Field.Receive5,
                    MappingreXlsmdbEntity.Field.Receive6,  MappingreXlsmdbEntity.Field.Receive7,  MappingreXlsmdbEntity.Field.Receive8,  MappingreXlsmdbEntity.Field.Receive9,  MappingreXlsmdbEntity.Field.Receive10,
                    MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12, MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                    MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18, MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20,
                    MappingreXlsmdbEntity.Field.Receive21, MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24, MappingreXlsmdbEntity.Field.Receive25,
                    MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27, MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                    MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33, MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35,
                    MappingreXlsmdbEntity.Field.Receive36, MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39, MappingreXlsmdbEntity.Field.Receive40
                };
                int idx = 0;
                foreach (string header in headers)
                {
                    if (!fixNames.Contains(header))
                    {
                        #region 收集收入科目名稱
                        receiveItemNames[idx] = header;
                        #endregion

                        mapFields.Add(new XlsMapField(fields[idx], header, new DecimalChecker(-999999999M, 999999999M)));
                        idx++;
                    }
                }
                if (idx == 0)
                {
                    logmsg = "上傳檔案表頭無任何收入科目";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                if (idx > 40)
                {
                    logmsg = "上傳檔案表頭超過40項收入科目";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion
                #endregion

                if (mapFields == null || mapFields.Count == 0)
                {
                    logmsg = "無法取得對照欄位";
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
            }
            #endregion

            #region 代收費用別設定
            bool isAddSchoolRid = false;
            SchoolRidEntity schoolRid = null;
            //有收集到收入科目再取 SchoolRidEntity
            if (receiveItemNames != null && receiveItemNames.Length > 0)
            {
                Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
                    .And(SchoolRidEntity.Field.YearId, yearId)
                    .And(SchoolRidEntity.Field.TermId, termId)
                    .And(SchoolRidEntity.Field.DepId, depId)
                    .And(SchoolRidEntity.Field.ReceiveId, receiveId);
                Result result = _Factory.SelectFirst<SchoolRidEntity>(where, null, out schoolRid);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取代收費用別設定資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (schoolRid == null)
                {
                    isAddSchoolRid = true;

                    schoolRid = new SchoolRidEntity();
                    schoolRid.ReceiveType = receiveType;
                    schoolRid.YearId = yearId;
                    schoolRid.TermId = termId;
                    schoolRid.DepId = depId;
                    schoolRid.ReceiveId = receiveId;
                    schoolRid.ReceiveStatus = String.Empty;

                    #region 收入科目名稱
                    schoolRid.ReceiveItem01 = receiveItemNames[00];
                    schoolRid.ReceiveItem02 = receiveItemNames[01];
                    schoolRid.ReceiveItem03 = receiveItemNames[02];
                    schoolRid.ReceiveItem04 = receiveItemNames[03];
                    schoolRid.ReceiveItem05 = receiveItemNames[04];
                    schoolRid.ReceiveItem06 = receiveItemNames[05];
                    schoolRid.ReceiveItem07 = receiveItemNames[06];
                    schoolRid.ReceiveItem08 = receiveItemNames[07];
                    schoolRid.ReceiveItem09 = receiveItemNames[08];
                    schoolRid.ReceiveItem10 = receiveItemNames[09];

                    schoolRid.ReceiveItem11 = receiveItemNames[10];
                    schoolRid.ReceiveItem12 = receiveItemNames[11];
                    schoolRid.ReceiveItem13 = receiveItemNames[12];
                    schoolRid.ReceiveItem14 = receiveItemNames[13];
                    schoolRid.ReceiveItem15 = receiveItemNames[14];
                    schoolRid.ReceiveItem16 = receiveItemNames[15];
                    schoolRid.ReceiveItem17 = receiveItemNames[16];
                    schoolRid.ReceiveItem18 = receiveItemNames[17];
                    schoolRid.ReceiveItem19 = receiveItemNames[18];
                    schoolRid.ReceiveItem20 = receiveItemNames[19];

                    schoolRid.ReceiveItem21 = receiveItemNames[20];
                    schoolRid.ReceiveItem22 = receiveItemNames[21];
                    schoolRid.ReceiveItem23 = receiveItemNames[22];
                    schoolRid.ReceiveItem24 = receiveItemNames[23];
                    schoolRid.ReceiveItem25 = receiveItemNames[24];
                    schoolRid.ReceiveItem26 = receiveItemNames[25];
                    schoolRid.ReceiveItem27 = receiveItemNames[26];
                    schoolRid.ReceiveItem28 = receiveItemNames[27];
                    schoolRid.ReceiveItem29 = receiveItemNames[28];
                    schoolRid.ReceiveItem30 = receiveItemNames[29];

                    schoolRid.ReceiveItem31 = receiveItemNames[30];
                    schoolRid.ReceiveItem32 = receiveItemNames[31];
                    schoolRid.ReceiveItem33 = receiveItemNames[32];
                    schoolRid.ReceiveItem34 = receiveItemNames[33];
                    schoolRid.ReceiveItem35 = receiveItemNames[34];
                    schoolRid.ReceiveItem36 = receiveItemNames[35];
                    schoolRid.ReceiveItem37 = receiveItemNames[36];
                    schoolRid.ReceiveItem38 = receiveItemNames[37];
                    schoolRid.ReceiveItem39 = receiveItemNames[38];
                    schoolRid.ReceiveItem40 = receiveItemNames[39];
                    #endregion

                    #region 收入科目是否助貸旗標
                    schoolRid.LoanItem01 = "N";
                    schoolRid.LoanItem02 = "N";
                    schoolRid.LoanItem03 = "N";
                    schoolRid.LoanItem04 = "N";
                    schoolRid.LoanItem05 = "N";
                    schoolRid.LoanItem06 = "N";
                    schoolRid.LoanItem07 = "N";
                    schoolRid.LoanItem08 = "N";
                    schoolRid.LoanItem09 = "N";
                    schoolRid.LoanItem10 = "N";

                    schoolRid.LoanItem11 = "N";
                    schoolRid.LoanItem12 = "N";
                    schoolRid.LoanItem13 = "N";
                    schoolRid.LoanItem14 = "N";
                    schoolRid.LoanItem15 = "N";
                    schoolRid.LoanItem16 = "N";
                    schoolRid.LoanItem17 = "N";
                    schoolRid.LoanItem18 = "N";
                    schoolRid.LoanItem19 = "N";
                    schoolRid.LoanItem20 = "N";

                    schoolRid.LoanItem21 = "N";
                    schoolRid.LoanItem22 = "N";
                    schoolRid.LoanItem23 = "N";
                    schoolRid.LoanItem24 = "N";
                    schoolRid.LoanItem25 = "N";
                    schoolRid.LoanItem26 = "N";
                    schoolRid.LoanItem27 = "N";
                    schoolRid.LoanItem28 = "N";
                    schoolRid.LoanItem29 = "N";
                    schoolRid.LoanItem30 = "N";

                    schoolRid.LoanItem31 = "N";
                    schoolRid.LoanItem32 = "N";
                    schoolRid.LoanItem33 = "N";
                    schoolRid.LoanItem34 = "N";
                    schoolRid.LoanItem35 = "N";
                    schoolRid.LoanItem36 = "N";
                    schoolRid.LoanItem37 = "N";
                    schoolRid.LoanItem38 = "N";
                    schoolRid.LoanItem39 = "N";
                    schoolRid.LoanItem40 = "N";

                    schoolRid.LoanItemOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    #region 收入科目是否代辦旗標
                    schoolRid.AgencyItem01 = "N";
                    schoolRid.AgencyItem02 = "N";
                    schoolRid.AgencyItem03 = "N";
                    schoolRid.AgencyItem04 = "N";
                    schoolRid.AgencyItem05 = "N";
                    schoolRid.AgencyItem06 = "N";
                    schoolRid.AgencyItem07 = "N";
                    schoolRid.AgencyItem08 = "N";
                    schoolRid.AgencyItem09 = "N";
                    schoolRid.AgencyItem10 = "N";

                    schoolRid.AgencyItem11 = "N";
                    schoolRid.AgencyItem12 = "N";
                    schoolRid.AgencyItem13 = "N";
                    schoolRid.AgencyItem14 = "N";
                    schoolRid.AgencyItem15 = "N";
                    schoolRid.AgencyItem16 = "N";
                    schoolRid.AgencyItem17 = "N";
                    schoolRid.AgencyItem18 = "N";
                    schoolRid.AgencyItem19 = "N";
                    schoolRid.AgencyItem20 = "N";

                    schoolRid.AgencyItem21 = "N";
                    schoolRid.AgencyItem22 = "N";
                    schoolRid.AgencyItem23 = "N";
                    schoolRid.AgencyItem24 = "N";
                    schoolRid.AgencyItem25 = "N";
                    schoolRid.AgencyItem26 = "N";
                    schoolRid.AgencyItem27 = "N";
                    schoolRid.AgencyItem28 = "N";
                    schoolRid.AgencyItem29 = "N";
                    schoolRid.AgencyItem30 = "N";

                    schoolRid.AgencyItem31 = "N";
                    schoolRid.AgencyItem32 = "N";
                    schoolRid.AgencyItem33 = "N";
                    schoolRid.AgencyItem34 = "N";
                    schoolRid.AgencyItem35 = "N";
                    schoolRid.AgencyItem36 = "N";
                    schoolRid.AgencyItem37 = "N";
                    schoolRid.AgencyItem38 = "N";
                    schoolRid.AgencyItem39 = "N";
                    schoolRid.AgencyItem40 = "N";

                    schoolRid.AgencyItemOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    #region 收入科目是否可更改旗標
                    schoolRid.AgencyCheck01 = "N";
                    schoolRid.AgencyCheck02 = "N";
                    schoolRid.AgencyCheck03 = "N";
                    schoolRid.AgencyCheck04 = "N";
                    schoolRid.AgencyCheck05 = "N";
                    schoolRid.AgencyCheck06 = "N";
                    schoolRid.AgencyCheck07 = "N";
                    schoolRid.AgencyCheck08 = "N";
                    schoolRid.AgencyCheck09 = "N";
                    schoolRid.AgencyCheck10 = "N";

                    schoolRid.AgencyCheck11 = "N";
                    schoolRid.AgencyCheck12 = "N";
                    schoolRid.AgencyCheck13 = "N";
                    schoolRid.AgencyCheck14 = "N";
                    schoolRid.AgencyCheck15 = "N";
                    schoolRid.AgencyCheck16 = "N";
                    schoolRid.AgencyCheck17 = "N";
                    schoolRid.AgencyCheck18 = "N";
                    schoolRid.AgencyCheck19 = "N";
                    schoolRid.AgencyCheck20 = "N";

                    schoolRid.AgencyCheck21 = "N";
                    schoolRid.AgencyCheck22 = "N";
                    schoolRid.AgencyCheck23 = "N";
                    schoolRid.AgencyCheck24 = "N";
                    schoolRid.AgencyCheck25 = "N";
                    schoolRid.AgencyCheck26 = "N";
                    schoolRid.AgencyCheck27 = "N";
                    schoolRid.AgencyCheck28 = "N";
                    schoolRid.AgencyCheck29 = "N";
                    schoolRid.AgencyCheck30 = "N";

                    schoolRid.AgencyCheck31 = "N";
                    schoolRid.AgencyCheck32 = "N";
                    schoolRid.AgencyCheck33 = "N";
                    schoolRid.AgencyCheck34 = "N";
                    schoolRid.AgencyCheck35 = "N";
                    schoolRid.AgencyCheck36 = "N";
                    schoolRid.AgencyCheck37 = "N";
                    schoolRid.AgencyCheck38 = "N";
                    schoolRid.AgencyCheck39 = "N";
                    schoolRid.AgencyCheck40 = "N";

                    schoolRid.AgencyCheckOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion

                    #region 收入科目是否教育部補助旗標
                    schoolRid.Issubsidy01 = "N";
                    schoolRid.Issubsidy02 = "N";
                    schoolRid.Issubsidy03 = "N";
                    schoolRid.Issubsidy04 = "N";
                    schoolRid.Issubsidy05 = "N";
                    schoolRid.Issubsidy06 = "N";
                    schoolRid.Issubsidy07 = "N";
                    schoolRid.Issubsidy08 = "N";
                    schoolRid.Issubsidy09 = "N";
                    schoolRid.Issubsidy10 = "N";

                    schoolRid.Issubsidy11 = "N";
                    schoolRid.Issubsidy12 = "N";
                    schoolRid.Issubsidy13 = "N";
                    schoolRid.Issubsidy14 = "N";
                    schoolRid.Issubsidy15 = "N";
                    schoolRid.Issubsidy16 = "N";
                    schoolRid.Issubsidy17 = "N";
                    schoolRid.Issubsidy18 = "N";
                    schoolRid.Issubsidy19 = "N";
                    schoolRid.Issubsidy20 = "N";

                    schoolRid.Issubsidy21 = "N";
                    schoolRid.Issubsidy22 = "N";
                    schoolRid.Issubsidy23 = "N";
                    schoolRid.Issubsidy24 = "N";
                    schoolRid.Issubsidy25 = "N";
                    schoolRid.Issubsidy26 = "N";
                    schoolRid.Issubsidy27 = "N";
                    schoolRid.Issubsidy28 = "N";
                    schoolRid.Issubsidy29 = "N";
                    schoolRid.Issubsidy30 = "N";

                    schoolRid.Issubsidy31 = "N";
                    schoolRid.Issubsidy32 = "N";
                    schoolRid.Issubsidy33 = "N";
                    schoolRid.Issubsidy34 = "N";
                    schoolRid.Issubsidy35 = "N";
                    schoolRid.Issubsidy36 = "N";
                    schoolRid.Issubsidy37 = "N";
                    schoolRid.Issubsidy38 = "N";
                    schoolRid.Issubsidy39 = "N";
                    schoolRid.Issubsidy40 = "N";

                    schoolRid.IssubsidyOthers = "NNNNNNNNNNNNNNNNNNNNNNNN";
                    #endregion


                    schoolRid.BillCloseDate = String.Empty;

                    schoolRid.BillingType = "1";
                    schoolRid.LoanQual = String.Empty;
                    schoolRid.Brief1 = String.Empty;
                    schoolRid.Brief2 = String.Empty;
                    schoolRid.Brief3 = String.Empty;
                    schoolRid.Brief4 = String.Empty;
                    schoolRid.Brief5 = String.Empty;
                    schoolRid.Brief6 = String.Empty;
                    schoolRid.Hide = String.Empty;
                    schoolRid.SchLevel = String.Empty;
                    schoolRid.EnabledTax = "N";
                    schoolRid.EduTax = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
                    schoolRid.StayTax = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";

                    schoolRid.Usestamp = "N";
                    schoolRid.Usewatermark = "N";
                    schoolRid.Usepostdueday = String.Empty;
                }
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();
            {
                string errmsg = null;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                ConvertFileHelper helper = new ConvertFileHelper();
                bool isOK = false;
                if (fileType == "xls")
                {
                    #region Xls 轉 DataTable
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Xls2DataTable(ms, sheetName, mapFields.ToArray(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    #endregion
                }
                else if (fileType == "xlsx")
                {
                    #region Xlsx 轉 DataTable
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Xlsx2DataTable(ms, sheetName, mapFields.ToArray(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    #endregion
                }
                else if (fileType == "ods")
                {
                    #region Ods 轉 DataTable
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        isOK = helper.Ods2DataTable(ms, sheetName, mapFields.ToArray(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                    }
                    #endregion
                }
                else
                {
                    #region 不支援
                    {
                        logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                        return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                    }
                    #endregion
                }
                if (!isOK)
                {
                    if (table != null && table.Rows.Count > 0)
                    {
                        StringBuilder log = new StringBuilder();
                        int rowNo = 0;
                        foreach (DataRow row in table.Rows)
                        {
                            rowNo++;
                            string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                            if (!String.IsNullOrEmpty(failMsg))
                            {
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                            }
                        }
                        logmsg = log.ToString();
                    }
                    return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
                #endregion

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }

                #region [MDY:20200815] M202008_02 學生身分證字號不可與學號相同 (2020806_01)
                if (table != null && table.Rows.Count > 0
                    && fieldNames.Contains(MappingreXlsmdbEntity.Field.StuId) && fieldNames.Contains(MappingreXlsmdbEntity.Field.IdNumber))
                {
                    foreach (DataRow dRow in table.Rows)
                    {
                        string failMsg = dRow.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : dRow[ConvertFileHelper.DataLineFailureFieldName].ToString();
                        if (String.IsNullOrEmpty(failMsg))
                        {
                            string stuId = dRow.IsNull(MappingreXlsmdbEntity.Field.StuId) ? null : dRow[MappingreXlsmdbEntity.Field.StuId].ToString();
                            string isNumber = dRow.IsNull(MappingreXlsmdbEntity.Field.IdNumber) ? null : dRow[MappingreXlsmdbEntity.Field.IdNumber].ToString();
                            if (!String.IsNullOrEmpty(isNumber) && isNumber.Equals(stuId))
                            {
                                dRow[ConvertFileHelper.DataLineFailureFieldName] = "學生身分證字號不可與學號相同";
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 使用交易將 DataTable 逐筆存入資料庫
            {
                successCount = 0;
                DateTime now = DateTime.Now;
                Result importResult = new Result(true);
                StringBuilder log = new StringBuilder();
                using (EntityFactory tsFactory = _Factory.IsUseTransaction ? _Factory : _Factory.CloneForTransaction())
                {
                    List<string> studentIds = new List<string>(table.Rows.Count);   //紀錄出現過的學號，用來檢查同一批資料不可學號重複
                    List<string> saveClassIds = new List<string>();

                    #region [Old] 簡易上傳沒有這些欄位
                    //List<string> saveDeptIds = new List<string>();
                    //List<string> saveCollegeIds = new List<string>();
                    //List<string> saveMajorIds = new List<string>();
                    //List<string> saveReduceIds = new List<string>();
                    //List<string> saveLoanIds = new List<string>();
                    //List<string> saveDormIds = new List<string>();
                    #endregion

                    int rowNo = 0;
                    int okNo = 1;
                    foreach (DataRow row in table.Rows)
                    {
                        rowNo++;

                        #region 取得資料行錯誤訊息
                        {
                            string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                            if (!String.IsNullOrEmpty(failMsg))
                            {
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                continue;
                            }
                        }
                        #endregion

                        #region 產生各種 Entity
                        #region 學生資料對照欄位 (StudentMasterEntity)
                        StudentMasterEntity student = new StudentMasterEntity();
                        student.ReceiveType = receiveType;
                        student.DepId = depId;
                        student.Id = row[MappingreXlsmdbEntity.Field.StuId].ToString();
                        student.Name = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuName) ? row[MappingreXlsmdbEntity.Field.StuName].ToString() : String.Empty;
                        student.Birthday = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuBirthday) ? row[MappingreXlsmdbEntity.Field.StuBirthday].ToString() : String.Empty;
                        if (!String.IsNullOrWhiteSpace(student.Birthday))
                        {
                            DateTime? date = DataFormat.ConvertDateText(student.Birthday.Trim());
                            if (date != null)
                            {
                                student.Birthday = Common.GetTWDate7(date.Value);
                            }
                        }
                        student.IdNumber = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdNumber) ? row[MappingreXlsmdbEntity.Field.IdNumber].ToString().ToUpper() : String.Empty;
                        student.Tel = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuTel) ? row[MappingreXlsmdbEntity.Field.StuTel].ToString() : String.Empty;
                        student.ZipCode = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuAddcode) ? row[MappingreXlsmdbEntity.Field.StuAddcode].ToString() : String.Empty;
                        student.Address = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuAdd) ? row[MappingreXlsmdbEntity.Field.StuAdd].ToString() : String.Empty;
                        student.Email = fieldNames.Contains(MappingreXlsmdbEntity.Field.Email) ? row[MappingreXlsmdbEntity.Field.Email].ToString() : String.Empty;
                        student.Account = String.Empty;
                        student.CrtDate = now;
                        student.MdyDate = null;
                        #endregion

                        #region 同一批上傳資料學號不可重複
                        if (studentIds.Contains(student.Id))
                        {
                            string failMsg = String.Format("學號 {0} 重複", student.Id);
                            row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                            if (isBatch)
                            {
                                importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            studentIds.Add(student.Id);
                        }
                        #endregion

                        StudentReceiveEntity receive = new StudentReceiveEntity();
                        receive.ReceiveType = receiveType;
                        receive.YearId = yearId;
                        receive.TermId = termId;
                        receive.DepId = depId;
                        receive.ReceiveId = receiveId;
                        receive.StuId = student.Id;

                        receive.OldSeq = 0; //系統不處理舊學雜費轉置的資料，所以固定為 0

                        int uploadFlag = 0;

                        #region 學籍資料對照欄位 (StudentReceiveEntity, ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity)
                        ClassListEntity classEntity = null;
                        #region [Old] 簡易上傳沒有這些欄位
                        //DeptListEntity dept = null;
                        //CollegeListEntity college = null;
                        //MajorListEntity major = null;
                        #endregion
                        {
                            receive.StuGrade = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuGrade) ? row[MappingreXlsmdbEntity.Field.StuGrade].ToString() : String.Empty;
                            receive.StuHid = fieldNames.Contains(MappingreXlsmdbEntity.Field.StuHid) ? row[MappingreXlsmdbEntity.Field.StuHid].ToString() : String.Empty;

                            #region 班別資料 ClassListEntity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassId))
                            {
                                receive.ClassId = row[MappingreXlsmdbEntity.Field.ClassId].ToString();

                                classEntity = new ClassListEntity();
                                classEntity.ReceiveType = receiveType;
                                classEntity.YearId = yearId;
                                classEntity.TermId = termId;
                                classEntity.DepId = depId;
                                classEntity.ClassId = receive.ClassId;
                                classEntity.ClassName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassName) ? row[MappingreXlsmdbEntity.Field.ClassName].ToString() : receive.ClassId;

                                classEntity.Status = DataStatusCodeTexts.NORMAL;
                                classEntity.CrtDate = now;
                                classEntity.CrtUser = owner;
                                classEntity.MdyDate = null;
                                classEntity.MdyUser = null;
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ClassName))
                            {
                                //只有名稱，就把名稱當代碼
                                receive.ClassId = row[MappingreXlsmdbEntity.Field.ClassName].ToString();

                                classEntity = new ClassListEntity();
                                classEntity.ReceiveType = receiveType;
                                classEntity.YearId = yearId;
                                classEntity.TermId = termId;
                                classEntity.DepId = depId;
                                classEntity.ClassId = receive.ClassId;
                                classEntity.ClassName = receive.ClassId;

                                classEntity.Status = DataStatusCodeTexts.NORMAL;
                                classEntity.CrtDate = now;
                                classEntity.CrtUser = owner;
                                classEntity.MdyDate = null;
                                classEntity.MdyUser = null;
                            }
                            else
                            {
                                receive.ClassId = String.Empty;
                            }
                            #endregion

                            #region 土銀專用的部別資料 DeptListEntity
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptId))
                            {
                                receive.DeptId = row[MappingreXlsmdbEntity.Field.DeptId].ToString();

                                #region [Old] 簡易上傳沒有 DeptListEntity
                                //dept = new DeptListEntity();
                                //dept.ReceiveType = receiveType;
                                //dept.YearId = yearId;
                                //dept.TermId = termId;
                                //dept.DeptId = receive.DeptId;
                                //dept.DeptName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptName) ? row[MappingreXlsmdbEntity.Field.DeptName].ToString() : receive.DeptId;

                                //dept.Status = DataStatusCodeTexts.NORMAL;
                                //dept.CrtDate = now;
                                //dept.CrtUser = owner;
                                //dept.MdyDate = null;
                                //dept.MdyUser = null;
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DeptName))
                            {
                                //只有名稱，就把名稱當代碼
                                receive.DeptId = row[MappingreXlsmdbEntity.Field.DeptName].ToString();

                                #region [Old] 簡易上傳沒有 DeptListEntity
                                //dept = new DeptListEntity();
                                //dept.ReceiveType = receiveType;
                                //dept.YearId = yearId;
                                //dept.TermId = termId;
                                //dept.DeptId = receive.DeptId;
                                //dept.DeptName = receive.DeptId;

                                //dept.Status = DataStatusCodeTexts.NORMAL;
                                //dept.CrtDate = now;
                                //dept.CrtUser = owner;
                                //dept.MdyDate = null;
                                //dept.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.DepId = String.Empty;
                            }
                            #endregion

                            #region [Old] 簡易上傳沒有 院別資料 CollegeListEntity
                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeId))
                            //{
                            //    receive.CollegeId = row[MappingreXlsmdbEntity.Field.CollegeId].ToString();

                            //    college = new CollegeListEntity();
                            //    college.ReceiveType = receiveType;
                            //    college.YearId = yearId;
                            //    college.TermId = termId;
                            //    college.DepId = depId;
                            //    college.CollegeId = receive.CollegeId;
                            //    college.CollegeName = fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeName) ? row[MappingreXlsmdbEntity.Field.CollegeName].ToString() : receive.CollegeId;

                            //    college.Status = DataStatusCodeTexts.NORMAL;
                            //    college.CrtDate = now;
                            //    college.CrtUser = owner;
                            //    college.MdyDate = null;
                            //    college.MdyUser = null;
                            //}
                            //else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.CollegeName))
                            //{
                            //    //只有名稱，就把名稱當代碼
                            //    receive.CollegeId = row[MappingreXlsmdbEntity.Field.CollegeName].ToString();

                            //    college = new CollegeListEntity();
                            //    college.ReceiveType = receiveType;
                            //    college.YearId = yearId;
                            //    college.TermId = termId;
                            //    college.DepId = depId;
                            //    college.CollegeId = receive.CollegeId;
                            //    college.CollegeName = receive.CollegeId;

                            //    college.Status = DataStatusCodeTexts.NORMAL;
                            //    college.CrtDate = now;
                            //    college.CrtUser = owner;
                            //    college.MdyDate = null;
                            //    college.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.CollegeId = String.Empty;
                            //}
                            #endregion

                            #region [Old] 簡易上傳沒有 系別資料 MajorListEntity
                            //if (fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorId))
                            //{
                            //    receive.MajorId = row[MappingreXlsmdbEntity.Field.MajorId].ToString();

                            //    major = new MajorListEntity();
                            //    major.ReceiveType = receiveType;
                            //    major.YearId = yearId;
                            //    major.TermId = termId;
                            //    major.DepId = depId;
                            //    major.MajorId = row[MappingreXlsmdbEntity.Field.MajorId].ToString();
                            //    major.MajorName = fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorName) ? row[MappingreXlsmdbEntity.Field.MajorName].ToString() : major.MajorId;

                            //    major.Status = DataStatusCodeTexts.NORMAL;
                            //    major.CrtDate = now;
                            //    major.CrtUser = owner;
                            //    major.MdyDate = null;
                            //    major.MdyUser = null;
                            //}
                            //else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.MajorName))
                            //{
                            //    //只有名稱，就把名稱當代碼
                            //    receive.MajorId = row[MappingreXlsmdbEntity.Field.MajorName].ToString();

                            //    major = new MajorListEntity();
                            //    major.ReceiveType = receiveType;
                            //    major.YearId = yearId;
                            //    major.TermId = termId;
                            //    major.DepId = depId;
                            //    major.MajorId = receive.MajorId;
                            //    major.MajorName = receive.MajorId;

                            //    major.Status = DataStatusCodeTexts.NORMAL;
                            //    major.CrtDate = now;
                            //    major.CrtUser = owner;
                            //    major.MdyDate = null;
                            //    major.MdyUser = null;
                            //}
                            //else
                            //{
                            //    receive.MajorId = String.Empty;
                            //}
                            #endregion
                        }
                        #endregion

                        #region 減免、就貸、住宿對照欄位 (StudentReceiveEntity, ReduceListEntity, LoanListEntity, DormListEntity)
                        #region [Old] 簡易上傳沒有這些欄位
                        //ReduceListEntity reduce = null;
                        //LoanListEntity loan = null;
                        //DormListEntity dorm = null;
                        #endregion
                        {
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceId))
                            {
                                receive.ReduceId = row[MappingreXlsmdbEntity.Field.ReduceId].ToString();

                                #region [Old] 簡易上傳沒有 ReduceListEntity
                                //reduce = new ReduceListEntity();
                                //reduce.ReceiveType = receiveType;
                                //reduce.YearId = yearId;
                                //reduce.TermId = termId;
                                //reduce.DepId = depId;
                                //reduce.ReduceId = row[MappingreXlsmdbEntity.Field.ReduceId].ToString();
                                //reduce.ReduceName = fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceName) ? row[MappingreXlsmdbEntity.Field.ReduceName].ToString() : reduce.ReduceId;

                                //reduce.Status = DataStatusCodeTexts.NORMAL;
                                //reduce.CrtDate = now;
                                //reduce.CrtUser = owner;
                                //reduce.MdyDate = null;
                                //reduce.MdyUser = null;
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ReduceName))
                            {
                                //只有名稱，就把名稱當代碼
                                receive.ReduceId = row[MappingreXlsmdbEntity.Field.ReduceName].ToString();

                                #region [Old] 簡易上傳沒有 ReduceListEntity
                                //reduce = new ReduceListEntity();
                                //reduce.ReceiveType = receiveType;
                                //reduce.YearId = yearId;
                                //reduce.TermId = termId;
                                //reduce.DepId = depId;
                                //reduce.ReduceId = receive.ReduceId;
                                //reduce.ReduceName = receive.ReduceId;

                                //reduce.Status = DataStatusCodeTexts.NORMAL;
                                //reduce.CrtDate = now;
                                //reduce.CrtUser = owner;
                                //reduce.MdyDate = null;
                                //reduce.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.ReduceId = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanId))
                            {
                                receive.LoanId = row[MappingreXlsmdbEntity.Field.LoanId].ToString();

                                #region [Old] 簡易上傳沒有 LoanListEntity
                                //loan = new LoanListEntity();
                                //loan.ReceiveType = receiveType;
                                //loan.YearId = yearId;
                                //loan.TermId = termId;
                                //loan.DepId = depId;
                                //loan.LoanId = row[MappingreXlsmdbEntity.Field.LoanId].ToString();
                                //loan.LoanName = fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanName) ? row[MappingreXlsmdbEntity.Field.LoanName].ToString() : loan.LoanId;

                                //loan.Status = DataStatusCodeTexts.NORMAL;
                                //loan.CrtDate = now;
                                //loan.CrtUser = owner;
                                //loan.MdyDate = null;
                                //loan.MdyUser = null;
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanName))
                            {
                                //只有名稱，就把名稱當代碼
                                receive.LoanId = row[MappingreXlsmdbEntity.Field.LoanName].ToString();

                                #region [Old] 簡易上傳沒有 LoanListEntity
                                //loan = new LoanListEntity();
                                //loan.ReceiveType = receiveType;
                                //loan.YearId = yearId;
                                //loan.TermId = termId;
                                //loan.DepId = depId;
                                //loan.LoanId = receive.LoanId;
                                //loan.LoanName = receive.LoanId;

                                //loan.Status = DataStatusCodeTexts.NORMAL;
                                //loan.CrtDate = now;
                                //loan.CrtUser = owner;
                                //loan.MdyDate = null;
                                //loan.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.LoanId = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DormId))
                            {
                                receive.DormId = row[MappingreXlsmdbEntity.Field.DormId].ToString();

                                #region [Old] 簡易上傳沒有 DormListEntity
                                //dorm = new DormListEntity();
                                //dorm.ReceiveType = receiveType;
                                //dorm.YearId = yearId;
                                //dorm.TermId = termId;
                                //dorm.DepId = depId;
                                //dorm.DormId = receive.DormId;
                                //dorm.DormName = fieldNames.Contains(MappingreXlsmdbEntity.Field.DormName) ? row[MappingreXlsmdbEntity.Field.DormName].ToString() : receive.DormId;

                                //dorm.Status = DataStatusCodeTexts.NORMAL;
                                //dorm.CrtDate = now;
                                //dorm.CrtUser = owner;
                                //dorm.MdyDate = null;
                                //dorm.MdyUser = null;
                                #endregion
                            }
                            else if (fieldNames.Contains(MappingreXlsmdbEntity.Field.DormName))
                            {
                                //只有名稱，就把名稱當代碼
                                receive.DormId = row[MappingreXlsmdbEntity.Field.DormName].ToString();

                                #region [Old] 簡易上傳沒有 DormListEntity
                                //dorm = new DormListEntity();
                                //dorm.ReceiveType = receiveType;
                                //dorm.YearId = yearId;
                                //dorm.TermId = termId;
                                //dorm.DepId = depId;
                                //dorm.DormId = receive.DormId;
                                //dorm.DormName = dorm.DormId;

                                //dorm.Status = DataStatusCodeTexts.NORMAL;
                                //dorm.CrtDate = now;
                                //dorm.CrtUser = owner;
                                //dorm.MdyDate = null;
                                //dorm.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.DormId = String.Empty;
                            }
                        }
                        #endregion

                        #region 身分註記對照欄位 (StudentReceiveEntity, IdentifyList1Entity, IdentifyList2Entity, IdentifyList3Entity, IdentifyList4Entity, IdentifyList5Entity, IdentifyList6Entity)
                        #region [Old] 簡易上傳沒有這些欄位
                        //IdentifyList1Entity identify1 = null;
                        //IdentifyList2Entity identify2 = null;
                        //IdentifyList3Entity identify3 = null;
                        //IdentifyList4Entity identify4 = null;
                        //IdentifyList5Entity identify5 = null;
                        //IdentifyList6Entity identify6 = null;
                        #endregion
                        {
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId1))
                            {
                                receive.IdentifyId01 = row[MappingreXlsmdbEntity.Field.IdentifyId1].ToString();

                                #region [Old] 簡易上傳沒有 IdentifyList2Entity
                                //identify1 = new IdentifyList1Entity();
                                //identify1.ReceiveType = receiveType;
                                //identify1.YearId = yearId;
                                //identify1.TermId = termId;
                                //identify1.DepId = depId;
                                //identify1.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId1].ToString();
                                //identify1.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName1) ? row[MappingreXlsmdbEntity.Field.IdentifyName1].ToString() : String.Empty;

                                //identify1.Status = DataStatusCodeTexts.NORMAL;
                                //identify1.CrtDate = now;
                                //identify1.CrtUser = owner;
                                //identify1.MdyDate = null;
                                //identify1.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.IdentifyId01 = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId2))
                            {
                                receive.IdentifyId02 = row[MappingreXlsmdbEntity.Field.IdentifyId2].ToString();

                                #region [Old] 簡易上傳沒有 IdentifyList2Entity
                                //identify2 = new IdentifyList2Entity();
                                //identify2.ReceiveType = receiveType;
                                //identify2.YearId = yearId;
                                //identify2.TermId = termId;
                                //identify2.DepId = depId;
                                //identify2.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId2].ToString();
                                //identify2.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName2) ? row[MappingreXlsmdbEntity.Field.IdentifyName2].ToString() : String.Empty;

                                //identify2.Status = DataStatusCodeTexts.NORMAL;
                                //identify2.CrtDate = now;
                                //identify2.CrtUser = owner;
                                //identify2.MdyDate = null;
                                //identify2.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.IdentifyId02 = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId3))
                            {
                                receive.IdentifyId03 = row[MappingreXlsmdbEntity.Field.IdentifyId3].ToString();

                                #region [Old] 簡易上傳沒有 IdentifyList3Entity
                                //identify3 = new IdentifyList3Entity();
                                //identify3.ReceiveType = receiveType;
                                //identify3.YearId = yearId;
                                //identify3.TermId = termId;
                                //identify3.DepId = depId;
                                //identify3.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId3].ToString();
                                //identify3.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName3) ? row[MappingreXlsmdbEntity.Field.IdentifyName3].ToString() : String.Empty;

                                //identify3.Status = DataStatusCodeTexts.NORMAL;
                                //identify3.CrtDate = now;
                                //identify3.CrtUser = owner;
                                //identify3.MdyDate = null;
                                //identify3.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.IdentifyId03 = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId4))
                            {
                                receive.IdentifyId04 = row[MappingreXlsmdbEntity.Field.IdentifyId4].ToString();

                                #region [Old] 簡易上傳沒有 IdentifyList4Entity
                                //identify4 = new IdentifyList4Entity();
                                //identify4.ReceiveType = receiveType;
                                //identify4.YearId = yearId;
                                //identify4.TermId = termId;
                                //identify4.DepId = depId;
                                //identify4.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId4].ToString();
                                //identify4.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName4) ? row[MappingreXlsmdbEntity.Field.IdentifyName4].ToString() : String.Empty;

                                //identify4.Status = DataStatusCodeTexts.NORMAL;
                                //identify4.CrtDate = now;
                                //identify4.CrtUser = owner;
                                //identify4.MdyDate = null;
                                //identify4.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.IdentifyId04 = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId5))
                            {
                                receive.IdentifyId05 = row[MappingreXlsmdbEntity.Field.IdentifyId5].ToString();

                                #region [Old] 簡易上傳沒有 IdentifyList5Entity
                                //identify5 = new IdentifyList5Entity();
                                //identify5.ReceiveType = receiveType;
                                //identify5.YearId = yearId;
                                //identify5.TermId = termId;
                                //identify5.DepId = depId;
                                //identify5.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId5].ToString();
                                //identify5.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName5) ? row[MappingreXlsmdbEntity.Field.IdentifyName5].ToString() : String.Empty;

                                //identify5.Status = DataStatusCodeTexts.NORMAL;
                                //identify5.CrtDate = now;
                                //identify5.CrtUser = owner;
                                //identify5.MdyDate = null;
                                //identify5.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.IdentifyId05 = String.Empty;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyId6))
                            {
                                receive.IdentifyId06 = row[MappingreXlsmdbEntity.Field.IdentifyId6].ToString();

                                #region [Old] 簡易上傳沒有 IdentifyList6Entity
                                //identify6 = new IdentifyList6Entity();
                                //identify6.ReceiveType = receiveType;
                                //identify6.YearId = yearId;
                                //identify6.TermId = termId;
                                //identify6.DepId = depId;
                                //identify6.IdentifyId = row[MappingreXlsmdbEntity.Field.IdentifyId6].ToString();
                                //identify6.IdentifyName = fieldNames.Contains(MappingreXlsmdbEntity.Field.IdentifyName6) ? row[MappingreXlsmdbEntity.Field.IdentifyName6].ToString() : String.Empty;

                                //identify6.Status = DataStatusCodeTexts.NORMAL;
                                //identify6.CrtDate = now;
                                //identify6.CrtUser = owner;
                                //identify6.MdyDate = null;
                                //identify6.MdyUser = null;
                                #endregion
                            }
                            else
                            {
                                receive.IdentifyId06 = String.Empty;
                            }
                        }
                        #endregion

                        #region 收入科目金額對照欄位 (StudentReceiveEntity)
                        {
                            string[] fields = new string[] {
                                MappingreXlsmdbEntity.Field.Receive1, MappingreXlsmdbEntity.Field.Receive2, MappingreXlsmdbEntity.Field.Receive3, MappingreXlsmdbEntity.Field.Receive4, MappingreXlsmdbEntity.Field.Receive5,
                                MappingreXlsmdbEntity.Field.Receive6, MappingreXlsmdbEntity.Field.Receive7, MappingreXlsmdbEntity.Field.Receive8, MappingreXlsmdbEntity.Field.Receive9, MappingreXlsmdbEntity.Field.Receive10,
                                MappingreXlsmdbEntity.Field.Receive11, MappingreXlsmdbEntity.Field.Receive12, MappingreXlsmdbEntity.Field.Receive13, MappingreXlsmdbEntity.Field.Receive14, MappingreXlsmdbEntity.Field.Receive15,
                                MappingreXlsmdbEntity.Field.Receive16, MappingreXlsmdbEntity.Field.Receive17, MappingreXlsmdbEntity.Field.Receive18, MappingreXlsmdbEntity.Field.Receive19, MappingreXlsmdbEntity.Field.Receive20,
                                MappingreXlsmdbEntity.Field.Receive21, MappingreXlsmdbEntity.Field.Receive22, MappingreXlsmdbEntity.Field.Receive23, MappingreXlsmdbEntity.Field.Receive24, MappingreXlsmdbEntity.Field.Receive25,
                                MappingreXlsmdbEntity.Field.Receive26, MappingreXlsmdbEntity.Field.Receive27, MappingreXlsmdbEntity.Field.Receive28, MappingreXlsmdbEntity.Field.Receive29, MappingreXlsmdbEntity.Field.Receive30,
                                MappingreXlsmdbEntity.Field.Receive31, MappingreXlsmdbEntity.Field.Receive32, MappingreXlsmdbEntity.Field.Receive33, MappingreXlsmdbEntity.Field.Receive34, MappingreXlsmdbEntity.Field.Receive35,
                                MappingreXlsmdbEntity.Field.Receive36, MappingreXlsmdbEntity.Field.Receive37, MappingreXlsmdbEntity.Field.Receive38, MappingreXlsmdbEntity.Field.Receive39, MappingreXlsmdbEntity.Field.Receive40
                            };
                            System.Decimal?[] values = new System.Decimal?[] {
                                null, null, null, null, null, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, null
                            };
                            bool isFail = false;
                            for (int idx = 0; idx < fields.Length; idx++)
                            {
                                string fieldName = fields[idx];
                                if (fieldNames.Contains(fieldName))
                                {
                                    System.Decimal value;
                                    if (System.Decimal.TryParse(row[fieldName].ToString(), out value))
                                    {
                                        values[idx] = value;
                                    }
                                    else
                                    {
                                        row[ConvertFileHelper.DataLineFailureFieldName] = String.Format("收入科目金額{0} 不是有效的金額", idx + 1);
                                        isFail = true;
                                        break;
                                    }
                                }
                            }
                            if (isFail)
                            {
                                string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            receive.Receive01 = values[0];
                            receive.Receive02 = values[1];
                            receive.Receive03 = values[2];
                            receive.Receive04 = values[3];
                            receive.Receive05 = values[4];
                            receive.Receive06 = values[5];
                            receive.Receive07 = values[6];
                            receive.Receive08 = values[7];
                            receive.Receive09 = values[8];
                            receive.Receive10 = values[9];

                            receive.Receive11 = values[10];
                            receive.Receive12 = values[11];
                            receive.Receive13 = values[12];
                            receive.Receive14 = values[13];
                            receive.Receive15 = values[14];
                            receive.Receive16 = values[15];
                            receive.Receive17 = values[16];
                            receive.Receive18 = values[17];
                            receive.Receive19 = values[18];
                            receive.Receive20 = values[19];

                            receive.Receive21 = values[20];
                            receive.Receive22 = values[21];
                            receive.Receive23 = values[22];
                            receive.Receive24 = values[23];
                            receive.Receive25 = values[24];
                            receive.Receive26 = values[25];
                            receive.Receive27 = values[26];
                            receive.Receive28 = values[27];
                            receive.Receive29 = values[28];
                            receive.Receive30 = values[29];

                            receive.Receive31 = values[30];
                            receive.Receive32 = values[31];
                            receive.Receive33 = values[32];
                            receive.Receive34 = values[33];
                            receive.Receive35 = values[34];
                            receive.Receive36 = values[35];
                            receive.Receive37 = values[36];
                            receive.Receive38 = values[37];
                            receive.Receive39 = values[38];
                            receive.Receive40 = values[39];
                        }
                        #endregion

                        #region 簡易上傳沒有 其他對照欄位 (StudentReceiveEntity)
                        {
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.StuCredit))
                            {
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.StuCredit].ToString(), out value))
                                {
                                    receive.StuCredit = value;
                                }
                                else
                                {
                                    string failMsg = "總學分數不是有效的數值";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.StuCredit = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.StuHour))
                            {
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.StuHour].ToString(), out value))
                                {
                                    receive.StuHour = value;
                                }
                                else
                                {
                                    string failMsg = "上課時數不是有效的數值";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.StuHour = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.LoanAmount))
                            {
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.LoanAmount].ToString(), out value))
                                {
                                    receive.LoanAmount = value;
                                }
                                else
                                {
                                    string failMsg = "就貸可貸金額不是有效的金額";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.LoanAmount = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.ReceiveAmount))
                            {
                                uploadFlag |= StudentReceiveEntity.UploadAmountFlag;
                                decimal value;
                                if (System.Decimal.TryParse(row[MappingreXlsmdbEntity.Field.ReceiveAmount].ToString(), out value))
                                {
                                    receive.ReceiveAmount = value;
                                }
                                else
                                {
                                    string failMsg = "繳費金額合計不是有效的金額";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.ReceiveAmount = null;
                            }
                        }
                        #endregion

                        #region [Old] 簡易上傳沒有 學分基準、課程、學分數對照欄位 (StudentCourseEntity)
                        //StudentCourseEntity course = null;
                        //{
                        //    string[] creditIdFileds = new string[] {
                        //        MappingreXlsmdbEntity.Field.CreditId1, MappingreXlsmdbEntity.Field.CreditId2, MappingreXlsmdbEntity.Field.CreditId3, MappingreXlsmdbEntity.Field.CreditId4, MappingreXlsmdbEntity.Field.CreditId5,
                        //        MappingreXlsmdbEntity.Field.CreditId6, MappingreXlsmdbEntity.Field.CreditId7, MappingreXlsmdbEntity.Field.CreditId8, MappingreXlsmdbEntity.Field.CreditId9, MappingreXlsmdbEntity.Field.CreditId10,
                        //        MappingreXlsmdbEntity.Field.CreditId11, MappingreXlsmdbEntity.Field.CreditId12, MappingreXlsmdbEntity.Field.CreditId13, MappingreXlsmdbEntity.Field.CreditId14, MappingreXlsmdbEntity.Field.CreditId15,
                        //        MappingreXlsmdbEntity.Field.CreditId16, MappingreXlsmdbEntity.Field.CreditId17, MappingreXlsmdbEntity.Field.CreditId18, MappingreXlsmdbEntity.Field.CreditId19, MappingreXlsmdbEntity.Field.CreditId20,
                        //        MappingreXlsmdbEntity.Field.CreditId21, MappingreXlsmdbEntity.Field.CreditId22, MappingreXlsmdbEntity.Field.CreditId23, MappingreXlsmdbEntity.Field.CreditId24, MappingreXlsmdbEntity.Field.CreditId25,
                        //        MappingreXlsmdbEntity.Field.CreditId26, MappingreXlsmdbEntity.Field.CreditId27, MappingreXlsmdbEntity.Field.CreditId28, MappingreXlsmdbEntity.Field.CreditId29, MappingreXlsmdbEntity.Field.CreditId30,
                        //        MappingreXlsmdbEntity.Field.CreditId31, MappingreXlsmdbEntity.Field.CreditId32, MappingreXlsmdbEntity.Field.CreditId33, MappingreXlsmdbEntity.Field.CreditId34, MappingreXlsmdbEntity.Field.CreditId35,
                        //        MappingreXlsmdbEntity.Field.CreditId36, MappingreXlsmdbEntity.Field.CreditId37, MappingreXlsmdbEntity.Field.CreditId38, MappingreXlsmdbEntity.Field.CreditId39, MappingreXlsmdbEntity.Field.CreditId40
                        //    };
                        //    string[] courseIdFileds = new string[] {
                        //        MappingreXlsmdbEntity.Field.CourseId1, MappingreXlsmdbEntity.Field.CourseId2, MappingreXlsmdbEntity.Field.CourseId3, MappingreXlsmdbEntity.Field.CourseId4, MappingreXlsmdbEntity.Field.CourseId5,
                        //        MappingreXlsmdbEntity.Field.CourseId6, MappingreXlsmdbEntity.Field.CourseId7, MappingreXlsmdbEntity.Field.CourseId8, MappingreXlsmdbEntity.Field.CourseId9, MappingreXlsmdbEntity.Field.CourseId10,
                        //        MappingreXlsmdbEntity.Field.CourseId11, MappingreXlsmdbEntity.Field.CourseId12, MappingreXlsmdbEntity.Field.CourseId13, MappingreXlsmdbEntity.Field.CourseId14, MappingreXlsmdbEntity.Field.CourseId15,
                        //        MappingreXlsmdbEntity.Field.CourseId16, MappingreXlsmdbEntity.Field.CourseId17, MappingreXlsmdbEntity.Field.CourseId18, MappingreXlsmdbEntity.Field.CourseId19, MappingreXlsmdbEntity.Field.CourseId20,
                        //        MappingreXlsmdbEntity.Field.CourseId21, MappingreXlsmdbEntity.Field.CourseId22, MappingreXlsmdbEntity.Field.CourseId23, MappingreXlsmdbEntity.Field.CourseId24, MappingreXlsmdbEntity.Field.CourseId25,
                        //        MappingreXlsmdbEntity.Field.CourseId26, MappingreXlsmdbEntity.Field.CourseId27, MappingreXlsmdbEntity.Field.CourseId28, MappingreXlsmdbEntity.Field.CourseId29, MappingreXlsmdbEntity.Field.CourseId30,
                        //        MappingreXlsmdbEntity.Field.CourseId31, MappingreXlsmdbEntity.Field.CourseId32, MappingreXlsmdbEntity.Field.CourseId33, MappingreXlsmdbEntity.Field.CourseId34, MappingreXlsmdbEntity.Field.CourseId35,
                        //        MappingreXlsmdbEntity.Field.CourseId36, MappingreXlsmdbEntity.Field.CourseId37, MappingreXlsmdbEntity.Field.CourseId38, MappingreXlsmdbEntity.Field.CourseId39, MappingreXlsmdbEntity.Field.CourseId40
                        //    };
                        //    string[] creditFileds = new string[] {
                        //        MappingreXlsmdbEntity.Field.Credit1, MappingreXlsmdbEntity.Field.Credit2, MappingreXlsmdbEntity.Field.Credit3, MappingreXlsmdbEntity.Field.Credit4, MappingreXlsmdbEntity.Field.Credit5,
                        //        MappingreXlsmdbEntity.Field.Credit6, MappingreXlsmdbEntity.Field.Credit7, MappingreXlsmdbEntity.Field.Credit8, MappingreXlsmdbEntity.Field.Credit9, MappingreXlsmdbEntity.Field.Credit10,
                        //        MappingreXlsmdbEntity.Field.Credit11, MappingreXlsmdbEntity.Field.Credit12, MappingreXlsmdbEntity.Field.Credit13, MappingreXlsmdbEntity.Field.Credit14, MappingreXlsmdbEntity.Field.Credit15,
                        //        MappingreXlsmdbEntity.Field.Credit16, MappingreXlsmdbEntity.Field.Credit17, MappingreXlsmdbEntity.Field.Credit18, MappingreXlsmdbEntity.Field.Credit19, MappingreXlsmdbEntity.Field.Credit20,
                        //        MappingreXlsmdbEntity.Field.Credit21, MappingreXlsmdbEntity.Field.Credit22, MappingreXlsmdbEntity.Field.Credit23, MappingreXlsmdbEntity.Field.Credit24, MappingreXlsmdbEntity.Field.Credit25,
                        //        MappingreXlsmdbEntity.Field.Credit26, MappingreXlsmdbEntity.Field.Credit27, MappingreXlsmdbEntity.Field.Credit28, MappingreXlsmdbEntity.Field.Credit29, MappingreXlsmdbEntity.Field.Credit30,
                        //        MappingreXlsmdbEntity.Field.Credit31, MappingreXlsmdbEntity.Field.Credit32, MappingreXlsmdbEntity.Field.Credit33, MappingreXlsmdbEntity.Field.Credit34, MappingreXlsmdbEntity.Field.Credit35,
                        //        MappingreXlsmdbEntity.Field.Credit36, MappingreXlsmdbEntity.Field.Credit37, MappingreXlsmdbEntity.Field.Credit38, MappingreXlsmdbEntity.Field.Credit39, MappingreXlsmdbEntity.Field.Credit40
                        //    };
                        //    string[] creditIdValues = new string[40];
                        //    string[] courseIdValues = new string[40];
                        //    Decimal?[] creditValues = new Decimal?[40];
                        //    bool isFail = false;
                        //    for (int idx = 0; idx < creditIdFileds.Length; idx++)
                        //    {
                        //        string creditIdFiled = creditIdFileds[idx];
                        //        if (fieldNames.Contains(creditIdFiled))
                        //        {
                        //            creditIdValues[idx] = row[creditIdFiled].ToString();
                        //        }
                        //        else
                        //        {
                        //            creditIdValues[idx] = null;
                        //        }

                        //        string courseIdFiled = courseIdFileds[idx];
                        //        if (fieldNames.Contains(courseIdFiled))
                        //        {
                        //            courseIdValues[idx] = row[courseIdFiled].ToString();
                        //        }
                        //        else
                        //        {
                        //            courseIdValues[idx] = null;
                        //        }

                        //        string creditFiled = creditFileds[idx];
                        //        if (fieldNames.Contains(creditFiled))
                        //        {
                        //            Decimal value;
                        //            if (Decimal.TryParse(row[creditFiled].ToString(), out value))
                        //            {
                        //                creditValues[idx] = value;
                        //            }
                        //            else
                        //            {
                        //                row[ConvertFileHelper.DataLineFailureFieldName] = String.Format("學分數{0} 不是有效的數值", idx + 1);
                        //                isFail = true;
                        //                break;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            creditValues[idx] = null;
                        //        }
                        //    }

                        //    if (isFail)
                        //    {
                        //        string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                        //        log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                        //        if (isBatch)
                        //        {
                        //            importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            continue;
                        //        }
                        //    }

                        //    course = new StudentCourseEntity();
                        //    course.ReceiveType = receive.ReceiveType;
                        //    course.YearId = receive.YearId;
                        //    course.TermId = receive.TermId;
                        //    course.DepId = receive.DepId;
                        //    course.ReceiveId = receive.ReceiveId;
                        //    course.StuId = receive.StuId;

                        //    course.CreditId1 = creditIdValues[0];
                        //    course.CreditId2 = creditIdValues[1];
                        //    course.CreditId3 = creditIdValues[2];
                        //    course.CreditId4 = creditIdValues[3];
                        //    course.CreditId5 = creditIdValues[4];
                        //    course.CreditId6 = creditIdValues[5];
                        //    course.CreditId7 = creditIdValues[6];
                        //    course.CreditId8 = creditIdValues[7];
                        //    course.CreditId9 = creditIdValues[8];
                        //    course.CreditId10 = creditIdValues[9];

                        //    course.CreditId11 = creditIdValues[10];
                        //    course.CreditId12 = creditIdValues[11];
                        //    course.CreditId13 = creditIdValues[12];
                        //    course.CreditId14 = creditIdValues[13];
                        //    course.CreditId15 = creditIdValues[14];
                        //    course.CreditId16 = creditIdValues[15];
                        //    course.CreditId17 = creditIdValues[16];
                        //    course.CreditId18 = creditIdValues[17];
                        //    course.CreditId19 = creditIdValues[18];
                        //    course.CreditId20 = creditIdValues[19];

                        //    course.CreditId21 = creditIdValues[20];
                        //    course.CreditId22 = creditIdValues[21];
                        //    course.CreditId23 = creditIdValues[22];
                        //    course.CreditId24 = creditIdValues[23];
                        //    course.CreditId25 = creditIdValues[24];
                        //    course.CreditId26 = creditIdValues[25];
                        //    course.CreditId27 = creditIdValues[26];
                        //    course.CreditId28 = creditIdValues[27];
                        //    course.CreditId29 = creditIdValues[28];
                        //    course.CreditId30 = creditIdValues[29];

                        //    course.CreditId31 = creditIdValues[30];
                        //    course.CreditId32 = creditIdValues[31];
                        //    course.CreditId33 = creditIdValues[32];
                        //    course.CreditId34 = creditIdValues[33];
                        //    course.CreditId35 = creditIdValues[34];
                        //    course.CreditId36 = creditIdValues[35];
                        //    course.CreditId37 = creditIdValues[36];
                        //    course.CreditId38 = creditIdValues[37];
                        //    course.CreditId39 = creditIdValues[38];
                        //    course.CreditId40 = creditIdValues[39];

                        //    course.CourseId1 = courseIdValues[0];
                        //    course.CourseId2 = courseIdValues[1];
                        //    course.CourseId3 = courseIdValues[2];
                        //    course.CourseId4 = courseIdValues[3];
                        //    course.CourseId5 = courseIdValues[4];
                        //    course.CourseId6 = courseIdValues[5];
                        //    course.CourseId7 = courseIdValues[6];
                        //    course.CourseId8 = courseIdValues[7];
                        //    course.CourseId9 = courseIdValues[8];
                        //    course.CourseId10 = courseIdValues[9];

                        //    course.CourseId11 = courseIdValues[10];
                        //    course.CourseId12 = courseIdValues[11];
                        //    course.CourseId13 = courseIdValues[12];
                        //    course.CourseId14 = courseIdValues[13];
                        //    course.CourseId15 = courseIdValues[14];
                        //    course.CourseId16 = courseIdValues[15];
                        //    course.CourseId17 = courseIdValues[16];
                        //    course.CourseId18 = courseIdValues[17];
                        //    course.CourseId19 = courseIdValues[18];
                        //    course.CourseId20 = courseIdValues[19];

                        //    course.CourseId21 = courseIdValues[20];
                        //    course.CourseId22 = courseIdValues[21];
                        //    course.CourseId23 = courseIdValues[22];
                        //    course.CourseId24 = courseIdValues[23];
                        //    course.CourseId25 = courseIdValues[24];
                        //    course.CourseId26 = courseIdValues[25];
                        //    course.CourseId27 = courseIdValues[26];
                        //    course.CourseId28 = courseIdValues[27];
                        //    course.CourseId29 = courseIdValues[28];
                        //    course.CourseId30 = courseIdValues[29];

                        //    course.CourseId31 = courseIdValues[30];
                        //    course.CourseId32 = courseIdValues[31];
                        //    course.CourseId33 = courseIdValues[32];
                        //    course.CourseId34 = courseIdValues[33];
                        //    course.CourseId35 = courseIdValues[34];
                        //    course.CourseId36 = courseIdValues[35];
                        //    course.CourseId37 = courseIdValues[36];
                        //    course.CourseId38 = courseIdValues[37];
                        //    course.CourseId39 = courseIdValues[38];
                        //    course.CourseId40 = courseIdValues[39];

                        //    course.Credit1 = creditValues[0] ?? 0M;
                        //    course.Credit2 = creditValues[1] ?? 0M;
                        //    course.Credit3 = creditValues[2] ?? 0M;
                        //    course.Credit4 = creditValues[3] ?? 0M;
                        //    course.Credit5 = creditValues[4] ?? 0M;
                        //    course.Credit6 = creditValues[5] ?? 0M;
                        //    course.Credit7 = creditValues[6] ?? 0M;
                        //    course.Credit8 = creditValues[7] ?? 0M;
                        //    course.Credit9 = creditValues[8] ?? 0M;
                        //    course.Credit10 = creditValues[9] ?? 0M;

                        //    course.Credit11 = creditValues[10] ?? 0M;
                        //    course.Credit12 = creditValues[11] ?? 0M;
                        //    course.Credit13 = creditValues[12] ?? 0M;
                        //    course.Credit14 = creditValues[13] ?? 0M;
                        //    course.Credit15 = creditValues[14] ?? 0M;
                        //    course.Credit16 = creditValues[15] ?? 0M;
                        //    course.Credit17 = creditValues[16] ?? 0M;
                        //    course.Credit18 = creditValues[17] ?? 0M;
                        //    course.Credit19 = creditValues[18] ?? 0M;
                        //    course.Credit20 = creditValues[19] ?? 0M;

                        //    course.Credit21 = creditValues[20] ?? 0M;
                        //    course.Credit22 = creditValues[21] ?? 0M;
                        //    course.Credit23 = creditValues[22] ?? 0M;
                        //    course.Credit24 = creditValues[23] ?? 0M;
                        //    course.Credit25 = creditValues[24] ?? 0M;
                        //    course.Credit26 = creditValues[25] ?? 0M;
                        //    course.Credit27 = creditValues[26] ?? 0M;
                        //    course.Credit28 = creditValues[27] ?? 0M;
                        //    course.Credit29 = creditValues[28] ?? 0M;
                        //    course.Credit30 = creditValues[29] ?? 0M;

                        //    course.Credit31 = creditValues[30] ?? 0M;
                        //    course.Credit32 = creditValues[31] ?? 0M;
                        //    course.Credit33 = creditValues[32] ?? 0M;
                        //    course.Credit34 = creditValues[33] ?? 0M;
                        //    course.Credit35 = creditValues[34] ?? 0M;
                        //    course.Credit36 = creditValues[35] ?? 0M;
                        //    course.Credit37 = creditValues[36] ?? 0M;
                        //    course.Credit38 = creditValues[37] ?? 0M;
                        //    course.Credit39 = creditValues[38] ?? 0M;
                        //    course.Credit40 = creditValues[39] ?? 0M;
                        //}
                        #endregion

                        #region Remark (StudentReceiveEntity)
                        {
                            receive.Remark = fieldNames.Contains(MappingreXlsmdbEntity.Field.Remark) ? row[MappingreXlsmdbEntity.Field.Remark].ToString() : String.Empty;
                        }
                        #endregion

                        #region 扣款資料相關對照欄位 (StudentReceiveEntity)
                        {
                            receive.DeductBankid = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductBankid) ? row[MappingreXlsmdbEntity.Field.DeductBankid].ToString() : String.Empty;
                            receive.DeductAccountno = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductAccountno) ? row[MappingreXlsmdbEntity.Field.DeductAccountno].ToString() : String.Empty;
                            receive.DeductAccountname = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductAccountname) ? row[MappingreXlsmdbEntity.Field.DeductAccountname].ToString() : String.Empty;
                            receive.DeductAccountid = fieldNames.Contains(MappingreXlsmdbEntity.Field.DeductAccountid) ? row[MappingreXlsmdbEntity.Field.DeductAccountid].ToString() : String.Empty;
                        }
                        #endregion

                        #region 虛擬帳號資料相關對照欄位 (StudentReceiveEntity)
                        {
                            bool hasSeriorNoField = false;
                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.SeriorNo))
                            {
                                uploadFlag |= StudentReceiveEntity.UploadSeriorNoFlag;
                                hasSeriorNoField = true;
                                receive.SeriorNo = row[MappingreXlsmdbEntity.Field.SeriorNo].ToString().PadLeft(module.SeriorNoSize, '0');
                            }
                            else
                            {
                                receive.SeriorNo = null;
                            }

                            if (fieldNames.Contains(MappingreXlsmdbEntity.Field.CancelNo))
                            {
                                uploadFlag |= StudentReceiveEntity.UploadCancelNoFlag;
                                receive.CancelNo = row[MappingreXlsmdbEntity.Field.CancelNo].ToString();

                                string myReceiveType = null;
                                string myCustomNo = null;
                                string myChecksum = null;
                                string myYearId = null;
                                string myTermId = null;
                                string myReceiveId = null;
                                string mySeriorNo = null;
                                if (module.TryParseCancelNo(receive.CancelNo, school.IsBigReceiveId(), out myReceiveType, out myCustomNo, out myChecksum, out myYearId, out myTermId, out myReceiveId, out mySeriorNo))
                                {
                                    string failMsg = null;
                                    if (receiveType != myReceiveType)
                                    {
                                        failMsg = "指定的虛擬帳號與商家代號不相符";
                                    }
                                    else if (hasSeriorNoField && receive.SeriorNo.TrimStart('0') != mySeriorNo.TrimStart('0'))
                                    {
                                        failMsg = "指定的虛擬帳號與流水號不相符";
                                    }

                                    if (!hasSeriorNoField)
                                    {
                                        receive.SeriorNo = mySeriorNo;
                                    }

                                    if (!String.IsNullOrEmpty(failMsg))
                                    {
                                        row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                        log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        if (isBatch)
                                        {
                                            importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    string failMsg = "指定的虛擬帳號不正確，無法取得其中的流水號";
                                    row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                    if (isBatch)
                                    {
                                        importResult = new Result(false, String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg), CoreStatusCode.UNKNOWN_ERROR, null);
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                receive.CancelNo = null;
                            }
                        }
                        #endregion

                        receive.Uploadflag = uploadFlag == 0 ? String.Empty : uploadFlag.ToString();

                        receive.UpNo = seriroNo > 0 ? seriroNo.ToString() : String.Empty;
                        receive.UpOrder = okNo.ToString("000000");
                        receive.MappingId = mappingId;
                        receive.MappingType = fileType == "xls" || fileType == "xlsx" ? "2" : "1";
                        receive.Exportreceivedata = "N";

                        //[TODO]
                        receive.BillingType = "2";

                        receive.CreateDate = now;
                        //receive.UpdateDate = now;
                        #endregion

                        #region 寫入資料庫
                        if (importResult.IsSuccess)
                        {
                            int count = 0;
                            Result result = null;

                            #region StudentReceiveEntity
                            {
                                StudentReceiveEntity oldStudentReceive = null;
                                Expression where = new Expression(StudentReceiveEntity.Field.ReceiveType, receive.ReceiveType)
                                    .And(StudentReceiveEntity.Field.YearId, receive.YearId)
                                    .And(StudentReceiveEntity.Field.TermId, receive.TermId)
                                    .And(StudentReceiveEntity.Field.DepId, receive.DepId)
                                    .And(StudentReceiveEntity.Field.ReceiveId, receive.ReceiveId)
                                    .And(StudentReceiveEntity.Field.StuId, receive.StuId)
                                    .And(StudentReceiveEntity.Field.OldSeq, receive.OldSeq);
                                result = tsFactory.SelectFirst<StudentReceiveEntity>(where, null, out oldStudentReceive);
                                if (result.IsSuccess)
                                {
                                    if (oldStudentReceive == null)
                                    {
                                        #region 檢查是否有相同學號的轉置資料
                                        int oldDataCount = 0;
                                        {
                                            Expression where2 = new Expression(StudentReceiveEntity.Field.ReceiveType, receive.ReceiveType)
                                                .And(StudentReceiveEntity.Field.YearId, receive.YearId)
                                                .And(StudentReceiveEntity.Field.TermId, receive.TermId)
                                                .And(StudentReceiveEntity.Field.DepId, receive.DepId)
                                                .And(StudentReceiveEntity.Field.ReceiveId, receive.ReceiveId)
                                                .And(StudentReceiveEntity.Field.StuId, receive.StuId)
                                                .And(StudentReceiveEntity.Field.OldSeq, RelationEnum.Greater, 0);
                                            result = tsFactory.SelectCount<StudentReceiveEntity>(where2, out oldDataCount);
                                            if (result.IsSuccess && oldDataCount > 0)
                                            {
                                                result = new Result(false, "該學生在此費用別已有舊的轉置資料，系統無法判斷處理方式", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                        }
                                        #endregion

                                        #region Insert
                                        if (result.IsSuccess)
                                        {
                                            result = tsFactory.Insert(receive, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生繳費資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region [20150915] 加強更新條件，避免更新到已繳或已銷的資料
                                        {
                                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveDate, null).Or(StudentReceiveEntity.Field.ReceiveDate, String.Empty));
                                            where.And(new Expression(StudentReceiveEntity.Field.AccountDate, null).Or(StudentReceiveEntity.Field.AccountDate, String.Empty));
                                            where.And(new Expression(StudentReceiveEntity.Field.ReceiveWay, null).Or(StudentReceiveEntity.Field.ReceiveWay, String.Empty));
                                        }
                                        #endregion

                                        if (!String.IsNullOrEmpty(oldStudentReceive.CancelFlag) || !String.IsNullOrEmpty(oldStudentReceive.ReceiveDate))
                                        {
                                            result = new Result(false, "該筆學生繳費資料已繳費", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        else if (!String.IsNullOrEmpty(oldStudentReceive.CancelNo) && !oldStudentReceive.HasUploadCancelNo())
                                        {
                                            result = new Result(false, "該筆學生繳費資料已產生銷編", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(StudentReceiveEntity.Field.StuGrade, receive.StuGrade);
                                            fieldValues.Add(StudentReceiveEntity.Field.StuHid, receive.StuHid);

                                            fieldValues.Add(StudentReceiveEntity.Field.ClassId, receive.ClassId);
                                            fieldValues.Add(StudentReceiveEntity.Field.CollegeId, receive.CollegeId);
                                            fieldValues.Add(StudentReceiveEntity.Field.MajorId, receive.MajorId);

                                            fieldValues.Add(StudentReceiveEntity.Field.ReduceId, receive.ReduceId);
                                            fieldValues.Add(StudentReceiveEntity.Field.LoanId, receive.LoanId);
                                            fieldValues.Add(StudentReceiveEntity.Field.DormId, receive.DormId);

                                            fieldValues.Add(StudentReceiveEntity.Field.IdentifyId01, receive.IdentifyId01);
                                            fieldValues.Add(StudentReceiveEntity.Field.IdentifyId02, receive.IdentifyId02);
                                            fieldValues.Add(StudentReceiveEntity.Field.IdentifyId03, receive.IdentifyId03);
                                            fieldValues.Add(StudentReceiveEntity.Field.IdentifyId04, receive.IdentifyId04);
                                            fieldValues.Add(StudentReceiveEntity.Field.IdentifyId05, receive.IdentifyId05);
                                            fieldValues.Add(StudentReceiveEntity.Field.IdentifyId06, receive.IdentifyId06);

                                            fieldValues.Add(StudentReceiveEntity.Field.Receive01, receive.Receive01);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive02, receive.Receive02);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive03, receive.Receive03);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive04, receive.Receive04);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive05, receive.Receive05);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive06, receive.Receive06);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive07, receive.Receive07);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive08, receive.Receive08);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive09, receive.Receive09);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive10, receive.Receive10);

                                            fieldValues.Add(StudentReceiveEntity.Field.Receive11, receive.Receive11);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive12, receive.Receive12);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive13, receive.Receive13);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive14, receive.Receive14);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive15, receive.Receive15);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive16, receive.Receive16);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive17, receive.Receive17);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive18, receive.Receive18);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive19, receive.Receive19);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive20, receive.Receive20);

                                            fieldValues.Add(StudentReceiveEntity.Field.Receive21, receive.Receive21);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive22, receive.Receive22);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive23, receive.Receive23);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive24, receive.Receive24);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive25, receive.Receive25);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive26, receive.Receive26);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive27, receive.Receive27);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive28, receive.Receive28);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive29, receive.Receive29);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive30, receive.Receive30);

                                            fieldValues.Add(StudentReceiveEntity.Field.Receive31, receive.Receive31);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive32, receive.Receive32);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive33, receive.Receive33);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive34, receive.Receive34);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive35, receive.Receive35);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive36, receive.Receive36);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive37, receive.Receive37);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive38, receive.Receive38);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive39, receive.Receive39);
                                            fieldValues.Add(StudentReceiveEntity.Field.Receive40, receive.Receive40);

                                            fieldValues.Add(StudentReceiveEntity.Field.StuCredit, receive.StuCredit);
                                            fieldValues.Add(StudentReceiveEntity.Field.StuHour, receive.StuHour);
                                            fieldValues.Add(StudentReceiveEntity.Field.LoanAmount, receive.LoanAmount);
                                            fieldValues.Add(StudentReceiveEntity.Field.ReceiveAmount, receive.ReceiveAmount);

                                            fieldValues.Add(StudentReceiveEntity.Field.Remark, receive.Remark);

                                            fieldValues.Add(StudentReceiveEntity.Field.DeductBankid, receive.DeductBankid);
                                            fieldValues.Add(StudentReceiveEntity.Field.DeductAccountno, receive.DeductAccountno);
                                            fieldValues.Add(StudentReceiveEntity.Field.DeductAccountname, receive.DeductAccountname);
                                            fieldValues.Add(StudentReceiveEntity.Field.DeductAccountid, receive.DeductAccountid);

                                            fieldValues.Add(StudentReceiveEntity.Field.SeriorNo, receive.SeriorNo);
                                            fieldValues.Add(StudentReceiveEntity.Field.CancelNo, receive.CancelNo);

                                            fieldValues.Add(StudentReceiveEntity.Field.Uploadflag, receive.Uploadflag);
                                            fieldValues.Add(StudentReceiveEntity.Field.MappingId, receive.MappingId);
                                            fieldValues.Add(StudentReceiveEntity.Field.MappingType, receive.MappingType);
                                            fieldValues.Add(StudentReceiveEntity.Field.Exportreceivedata, receive.Exportreceivedata);

                                            fieldValues.Add(StudentReceiveEntity.Field.UpdateDate, now);

                                            result = tsFactory.UpdateFields<StudentReceiveEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "學生繳費資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region StudentMasterEntity
                            if (result.IsSuccess)
                            {
                                Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, student.ReceiveType)
                                    .And(StudentMasterEntity.Field.DepId, student.DepId)
                                    .And(StudentMasterEntity.Field.Id, student.Id);
                                result = tsFactory.SelectCount<StudentMasterEntity>(where, out count);
                                if (result.IsSuccess)
                                {
                                    if (count == 0)
                                    {
                                        #region Insert
                                        result = tsFactory.Insert(student, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update
                                        KeyValueList fieldValues = new KeyValueList();
                                        fieldValues.Add(StudentMasterEntity.Field.Name, student.Name);
                                        fieldValues.Add(StudentMasterEntity.Field.Birthday, student.Birthday);
                                        fieldValues.Add(StudentMasterEntity.Field.IdNumber, student.IdNumber);

                                        #region [Old] 簡易上傳沒有這欄位
                                        //fieldValues.Add(StudentMasterEntity.Field.Tel, student.Tel);
                                        //fieldValues.Add(StudentMasterEntity.Field.ZipCode, student.ZipCode);
                                        //fieldValues.Add(StudentMasterEntity.Field.Address, student.Address);
                                        #endregion

                                        fieldValues.Add(StudentMasterEntity.Field.Email, student.Email);

                                        #region [Old] 簡易上傳沒有這欄位
                                        //fieldValues.Add(StudentMasterEntity.Field.Account, student.Account);
                                        #endregion

                                        fieldValues.Add(StudentMasterEntity.Field.MdyDate, now);

                                        result = tsFactory.UpdateFields<StudentMasterEntity>(fieldValues, where, out count);
                                        if (result.IsSuccess && count == 0)
                                        {
                                            result = new Result(false, "學生繳費資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region [Old] 簡易上傳沒有 StudentCourseEntity
                            //if (result.IsSuccess && course != null)
                            //{
                            //    Expression where = new Expression(StudentCourseEntity.Field.ReceiveType, course.ReceiveType)
                            //        .And(StudentCourseEntity.Field.YearId, course.YearId)
                            //        .And(StudentCourseEntity.Field.TermId, course.TermId)
                            //        .And(StudentCourseEntity.Field.DepId, course.DepId)
                            //        .And(StudentCourseEntity.Field.ReceiveId, course.ReceiveId)
                            //        .And(StudentCourseEntity.Field.StuId, course.StuId);
                            //    result = tsFactory.SelectCount<StudentCourseEntity>(where, out count);
                            //    if (result.IsSuccess)
                            //    {
                            //        if (count == 0)
                            //        {
                            //            #region Insert
                            //            result = tsFactory.Insert(course, out count);
                            //            if (result.IsSuccess && count == 0)
                            //            {
                            //                result = new Result(false, "學生課程資料已存在", ErrorCode.D_DATA_EXISTS, null);
                            //            }
                            //            #endregion
                            //        }
                            //        else
                            //        {
                            //            #region Update
                            //            result = tsFactory.Update(course, out count);
                            //            if (result.IsSuccess && count == 0)
                            //            {
                            //                result = new Result(false, "學生課程資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                            //            }
                            //            #endregion
                            //        }
                            //    }
                            //}
                            #endregion

                            #region [Old] 簡易上傳沒有 StudentLoanEntity
                            //if (result.IsSuccess)
                            //{
                            //    Expression where = new Expression(StudentLoanEntity.Field.ReceiveType, receive.ReceiveType)
                            //        .And(StudentLoanEntity.Field.YearId, receive.YearId)
                            //        .And(StudentLoanEntity.Field.TermId, receive.TermId)
                            //        .And(StudentLoanEntity.Field.DepId, receive.DepId)
                            //        .And(StudentLoanEntity.Field.ReceiveId, receive.ReceiveId)
                            //        .And(StudentLoanEntity.Field.StuId, receive.StuId)
                            //        .And(StudentLoanEntity.Field.LoanId, receive.LoanId);
                            //    result = tsFactory.SelectCount<StudentLoanEntity>(where, out count);
                            //    if (result.IsSuccess)
                            //    {
                            //        if (count == 0)
                            //        {
                            //            #region Insert
                            //            StudentLoanEntity studentLoan = null;
                            //            studentLoan = new StudentLoanEntity();
                            //            studentLoan.ReceiveType = receive.ReceiveType;
                            //            studentLoan.YearId = receive.YearId;
                            //            studentLoan.TermId = receive.TermId;
                            //            studentLoan.DepId = receive.DepId;
                            //            studentLoan.ReceiveId = receive.ReceiveId;
                            //            studentLoan.StuId = receive.StuId;
                            //            studentLoan.LoanId = receive.LoanId;
                            //            studentLoan.LoanAmount = receive.LoanAmount ?? 0M;
                            //            result = tsFactory.Insert(studentLoan, out count);
                            //            if (result.IsSuccess && count == 0)
                            //            {
                            //                result = new Result(false, "學生就貸資料已存在", ErrorCode.D_DATA_EXISTS, null);
                            //            }
                            //            #endregion
                            //        }
                            //        else
                            //        {
                            //            #region Update
                            //            KeyValueList fieldValues = new KeyValueList();
                            //            fieldValues.Add(StudentLoanEntity.Field.LoanAmount, receive.LoanAmount);

                            //            result = tsFactory.UpdateFields<StudentLoanEntity>(fieldValues, where, out count);
                            //            if (result.IsSuccess && count == 0)
                            //            {
                            //                result = new Result(false, "學生就貸資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                            //            }
                            //            #endregion
                            //        }
                            //    }
                            //}
                            #endregion

                            #region ClassListEntity, DeptListEntity, CollegeListEntity, MajorListEntity, ReduceListEntity, LoanListEntity, DormListEntity
                            {
                                #region ClassListEntity
                                if (result.IsSuccess && classEntity != null && !saveClassIds.Contains(classEntity.ClassId))
                                {
                                    Expression where = new Expression(ClassListEntity.Field.ReceiveType, classEntity.ReceiveType)
                                        .And(ClassListEntity.Field.YearId, classEntity.YearId)
                                        .And(ClassListEntity.Field.TermId, classEntity.TermId)
                                        .And(ClassListEntity.Field.DepId, classEntity.DepId)
                                        .And(ClassListEntity.Field.ClassId, classEntity.ClassId);
                                    result = tsFactory.SelectCount<ClassListEntity>(where, out count);
                                    if (result.IsSuccess)
                                    {
                                        if (count == 0)
                                        {
                                            #region Insert
                                            result = tsFactory.Insert(classEntity, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "班別就貸資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Update
                                            KeyValueList fieldValues = new KeyValueList();
                                            fieldValues.Add(ClassListEntity.Field.ClassName, classEntity.ClassName);

                                            fieldValues.Add(ClassListEntity.Field.MdyDate, classEntity.CrtDate);
                                            fieldValues.Add(ClassListEntity.Field.MdyUser, classEntity.CrtUser);

                                            result = tsFactory.UpdateFields<ClassListEntity>(fieldValues, where, out count);
                                            if (result.IsSuccess && count == 0)
                                            {
                                                result = new Result(false, "班別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                            }
                                            #endregion
                                        }
                                    }
                                    if (result.IsSuccess)
                                    {
                                        saveClassIds.Add(classEntity.ClassId);
                                    }
                                }
                                #endregion

                                #region [Old] 簡易上傳沒有 DeptListEntity
                                //if (result.IsSuccess && dept != null && !saveDeptIds.Contains(dept.DeptId))
                                //{
                                //    Expression where = new Expression(DeptListEntity.Field.ReceiveType, dept.ReceiveType)
                                //        .And(DeptListEntity.Field.YearId, dept.YearId)
                                //        .And(DeptListEntity.Field.TermId, dept.TermId)
                                //        .And(DeptListEntity.Field.DeptId, dept.DeptId);
                                //    result = tsFactory.SelectCount<DeptListEntity>(where, out count);
                                //    if (result.IsSuccess)
                                //    {
                                //        if (count == 0)
                                //        {
                                //            #region Insert
                                //            result = tsFactory.Insert(dept, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "部別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                //            }
                                //            #endregion
                                //        }
                                //        else
                                //        {
                                //            #region Update
                                //            KeyValueList fieldValues = new KeyValueList();
                                //            fieldValues.Add(DeptListEntity.Field.DeptName, dept.DeptName);

                                //            fieldValues.Add(DeptListEntity.Field.MdyDate, dept.CrtDate);
                                //            fieldValues.Add(DeptListEntity.Field.MdyUser, dept.CrtUser);

                                //            result = tsFactory.UpdateFields<DeptListEntity>(fieldValues, where, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "院別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                //            }
                                //            #endregion
                                //        }
                                //    }
                                //    if (result.IsSuccess)
                                //    {
                                //        saveDeptIds.Add(dept.DeptId);
                                //    }
                                //}
                                #endregion

                                #region [Old] 簡易上傳沒有 CollegeListEntity
                                //if (result.IsSuccess && college != null && !saveCollegeIds.Contains(college.CollegeId))
                                //{
                                //    Expression where = new Expression(CollegeListEntity.Field.ReceiveType, college.ReceiveType)
                                //        .And(CollegeListEntity.Field.YearId, college.YearId)
                                //        .And(CollegeListEntity.Field.TermId, college.TermId)
                                //        .And(CollegeListEntity.Field.DepId, college.DepId)
                                //        .And(CollegeListEntity.Field.CollegeId, college.CollegeId);
                                //    result = tsFactory.SelectCount<CollegeListEntity>(where, out count);
                                //    if (result.IsSuccess)
                                //    {
                                //        if (count == 0)
                                //        {
                                //            #region Insert
                                //            result = tsFactory.Insert(college, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "院別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                //            }
                                //            #endregion
                                //        }
                                //        else
                                //        {
                                //            #region Update
                                //            KeyValueList fieldValues = new KeyValueList();
                                //            fieldValues.Add(CollegeListEntity.Field.CollegeName, college.CollegeName);

                                //            fieldValues.Add(CollegeListEntity.Field.MdyDate, college.CrtDate);
                                //            fieldValues.Add(CollegeListEntity.Field.MdyUser, college.CrtUser);

                                //            result = tsFactory.UpdateFields<CollegeListEntity>(fieldValues, where, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "院別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                //            }
                                //            #endregion
                                //        }
                                //    }
                                //    if (result.IsSuccess)
                                //    {
                                //        saveCollegeIds.Add(college.CollegeId);
                                //    }
                                //}
                                #endregion

                                #region [Old] 簡易上傳沒有 MajorListEntity
                                //if (result.IsSuccess && major != null && !saveMajorIds.Contains(major.MajorId))
                                //{
                                //    Expression where = new Expression(MajorListEntity.Field.ReceiveType, major.ReceiveType)
                                //        .And(MajorListEntity.Field.YearId, major.YearId)
                                //        .And(MajorListEntity.Field.TermId, major.TermId)
                                //        .And(MajorListEntity.Field.DepId, major.DepId)
                                //        .And(MajorListEntity.Field.MajorId, major.MajorId);
                                //    result = tsFactory.SelectCount<MajorListEntity>(where, out count);
                                //    if (result.IsSuccess)
                                //    {
                                //        if (count == 0)
                                //        {
                                //            #region Insert
                                //            result = tsFactory.Insert(major, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "系所資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                //            }
                                //            #endregion
                                //        }
                                //        else
                                //        {
                                //            #region Update
                                //            KeyValueList fieldValues = new KeyValueList();
                                //            fieldValues.Add(MajorListEntity.Field.MajorName, major.MajorName);

                                //            fieldValues.Add(MajorListEntity.Field.MdyDate, major.CrtDate);
                                //            fieldValues.Add(MajorListEntity.Field.MdyUser, major.CrtUser);

                                //            result = tsFactory.UpdateFields<MajorListEntity>(fieldValues, where, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "系所資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                //            }
                                //            #endregion
                                //        }
                                //    }
                                //    if (result.IsSuccess)
                                //    {
                                //        saveMajorIds.Add(major.MajorId);
                                //    }
                                //}
                                #endregion

                                #region [Old] 簡易上傳沒有 ReduceListEntity
                                //if (result.IsSuccess && reduce != null && !saveReduceIds.Contains(reduce.ReduceId))
                                //{
                                //    Expression where = new Expression(ReduceListEntity.Field.ReceiveType, reduce.ReceiveType)
                                //        .And(ReduceListEntity.Field.YearId, reduce.YearId)
                                //        .And(ReduceListEntity.Field.TermId, reduce.TermId)
                                //        .And(ReduceListEntity.Field.DepId, reduce.DepId)
                                //        .And(ReduceListEntity.Field.ReduceId, reduce.ReduceId);
                                //    result = tsFactory.SelectCount<ReduceListEntity>(where, out count);
                                //    if (result.IsSuccess)
                                //    {
                                //        if (count == 0)
                                //        {
                                //            #region Insert
                                //            result = tsFactory.Insert(reduce, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "減免類別資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                //            }
                                //            #endregion
                                //        }
                                //        else
                                //        {
                                //            #region Update
                                //            KeyValueList fieldValues = new KeyValueList();
                                //            fieldValues.Add(ReduceListEntity.Field.ReduceName, reduce.ReduceName);

                                //            fieldValues.Add(ReduceListEntity.Field.MdyDate, reduce.CrtDate);
                                //            fieldValues.Add(ReduceListEntity.Field.MdyUser, reduce.CrtUser);

                                //            result = tsFactory.UpdateFields<ReduceListEntity>(fieldValues, where, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "減免類別資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                //            }
                                //            #endregion
                                //        }
                                //    }
                                //    if (result.IsSuccess)
                                //    {
                                //        saveReduceIds.Add(reduce.ReduceId);
                                //    }
                                //}
                                #endregion

                                #region [Old] 簡易上傳沒有 LoanListEntity
                                //if (result.IsSuccess && loan != null && !saveLoanIds.Contains(loan.LoanId))
                                //{
                                //    Expression where = new Expression(LoanListEntity.Field.ReceiveType, loan.ReceiveType)
                                //        .And(LoanListEntity.Field.YearId, loan.YearId)
                                //        .And(LoanListEntity.Field.TermId, loan.TermId)
                                //        .And(LoanListEntity.Field.DepId, loan.DepId)
                                //        .And(LoanListEntity.Field.LoanId, loan.LoanId);
                                //    result = tsFactory.SelectCount<LoanListEntity>(where, out count);
                                //    if (result.IsSuccess)
                                //    {
                                //        if (count == 0)
                                //        {
                                //            #region Insert
                                //            result = tsFactory.Insert(loan, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "就貸項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                //            }
                                //            #endregion
                                //        }
                                //        else
                                //        {
                                //            #region Update
                                //            KeyValueList fieldValues = new KeyValueList();
                                //            fieldValues.Add(LoanListEntity.Field.LoanName, loan.LoanName);

                                //            fieldValues.Add(LoanListEntity.Field.MdyDate, loan.CrtDate);
                                //            fieldValues.Add(LoanListEntity.Field.MdyUser, loan.CrtUser);

                                //            result = tsFactory.UpdateFields<LoanListEntity>(fieldValues, where, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "就貸項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                //            }
                                //            #endregion
                                //        }
                                //    }
                                //    if (result.IsSuccess)
                                //    {
                                //        saveLoanIds.Add(loan.LoanId);
                                //    }
                                //}
                                #endregion

                                #region [Old] 簡易上傳沒有 DormListEntity
                                //if (result.IsSuccess && dorm != null && !saveDormIds.Contains(dorm.DormId))
                                //{
                                //    Expression where = new Expression(DormListEntity.Field.ReceiveType, dorm.ReceiveType)
                                //        .And(DormListEntity.Field.YearId, dorm.YearId)
                                //        .And(DormListEntity.Field.TermId, dorm.TermId)
                                //        .And(DormListEntity.Field.DepId, dorm.DepId)
                                //        .And(DormListEntity.Field.DormId, dorm.DormId);
                                //    result = tsFactory.SelectCount<DormListEntity>(where, out count);
                                //    if (result.IsSuccess)
                                //    {
                                //        if (count == 0)
                                //        {
                                //            #region Insert
                                //            result = tsFactory.Insert(dorm, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "就貸項目資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                //            }
                                //            #endregion
                                //        }
                                //        else
                                //        {
                                //            #region Update
                                //            KeyValueList fieldValues = new KeyValueList();
                                //            fieldValues.Add(DormListEntity.Field.DormName, dorm.DormName);

                                //            fieldValues.Add(DormListEntity.Field.MdyDate, dorm.CrtDate);
                                //            fieldValues.Add(DormListEntity.Field.MdyUser, dorm.CrtUser);

                                //            result = tsFactory.UpdateFields<DormListEntity>(fieldValues, where, out count);
                                //            if (result.IsSuccess && count == 0)
                                //            {
                                //                result = new Result(false, "就貸項目資料資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                //            }
                                //            #endregion
                                //        }
                                //    }
                                //    if (result.IsSuccess)
                                //    {
                                //        saveDormIds.Add(dorm.DormId);
                                //    }
                                //}
                                #endregion
                            }
                            #endregion

                            if (result.IsSuccess)
                            {
                                tsFactory.Commit();
                                successCount++;
                                okNo++;
                                //log.AppendFormat("第 {0} 筆資料新增成功", rowNo).AppendLine();
                            }
                            else
                            {
                                log.AppendFormat("第 {0} 筆資料新增失敗，錯誤訊息：{1}", rowNo, result.Message).AppendLine();
                                tsFactory.Rollback();
                                if (isBatch)
                                {
                                    importResult = new Result(false, String.Format("第 {0} 筆資料新增失敗，錯誤訊息：：{1}", rowNo, result.Message), result.Code, result.Exception);
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (!importResult.IsSuccess && isBatch)
                        {
                            break;
                        }
                        #endregion
                    }
                }

                logmsg = log.ToString();
                if (importResult.IsSuccess && successCount == 0)
                {
                    importResult = new Result(false, "無資料被匯入成功", CoreStatusCode.UNKNOWN_ERROR, null);
                }

                #region 補代收科目名稱
                if (importResult.IsSuccess && schoolRid != null && receiveItemNames != null && receiveItemNames.Length > 0)
                {
                    if (isAddSchoolRid)
                    {
                        int count = 0;
                        _Factory.Insert(schoolRid, out count);
                    }
                    else
                    {
                        string[] fields = new string[] {
                            SchoolRidEntity.Field.ReceiveItem01, SchoolRidEntity.Field.ReceiveItem02, SchoolRidEntity.Field.ReceiveItem03, SchoolRidEntity.Field.ReceiveItem04, SchoolRidEntity.Field.ReceiveItem05,
                            SchoolRidEntity.Field.ReceiveItem06, SchoolRidEntity.Field.ReceiveItem07, SchoolRidEntity.Field.ReceiveItem08, SchoolRidEntity.Field.ReceiveItem09, SchoolRidEntity.Field.ReceiveItem10,
                            SchoolRidEntity.Field.ReceiveItem11, SchoolRidEntity.Field.ReceiveItem12, SchoolRidEntity.Field.ReceiveItem13, SchoolRidEntity.Field.ReceiveItem14, SchoolRidEntity.Field.ReceiveItem15,
                            SchoolRidEntity.Field.ReceiveItem16, SchoolRidEntity.Field.ReceiveItem17, SchoolRidEntity.Field.ReceiveItem18, SchoolRidEntity.Field.ReceiveItem19, SchoolRidEntity.Field.ReceiveItem20,
                            SchoolRidEntity.Field.ReceiveItem21, SchoolRidEntity.Field.ReceiveItem22, SchoolRidEntity.Field.ReceiveItem23, SchoolRidEntity.Field.ReceiveItem24, SchoolRidEntity.Field.ReceiveItem25,
                            SchoolRidEntity.Field.ReceiveItem26, SchoolRidEntity.Field.ReceiveItem27, SchoolRidEntity.Field.ReceiveItem28, SchoolRidEntity.Field.ReceiveItem29, SchoolRidEntity.Field.ReceiveItem30,
                            SchoolRidEntity.Field.ReceiveItem31, SchoolRidEntity.Field.ReceiveItem32, SchoolRidEntity.Field.ReceiveItem33, SchoolRidEntity.Field.ReceiveItem34, SchoolRidEntity.Field.ReceiveItem35,
                            SchoolRidEntity.Field.ReceiveItem36, SchoolRidEntity.Field.ReceiveItem37, SchoolRidEntity.Field.ReceiveItem38, SchoolRidEntity.Field.ReceiveItem39, SchoolRidEntity.Field.ReceiveItem40
                        };

                        #region [Old] 改為一律覆蓋，所以不用取舊資料
                        //string[] values = new string[] {
                        //    schoolRid.ReceiveItem01, schoolRid.ReceiveItem02, schoolRid.ReceiveItem03, schoolRid.ReceiveItem04, schoolRid.ReceiveItem05,
                        //    schoolRid.ReceiveItem06, schoolRid.ReceiveItem07, schoolRid.ReceiveItem08, schoolRid.ReceiveItem09, schoolRid.ReceiveItem10,
                        //    schoolRid.ReceiveItem11, schoolRid.ReceiveItem12, schoolRid.ReceiveItem13, schoolRid.ReceiveItem14, schoolRid.ReceiveItem15,
                        //    schoolRid.ReceiveItem16, schoolRid.ReceiveItem17, schoolRid.ReceiveItem18, schoolRid.ReceiveItem19, schoolRid.ReceiveItem20,
                        //    schoolRid.ReceiveItem21, schoolRid.ReceiveItem22, schoolRid.ReceiveItem23, schoolRid.ReceiveItem24, schoolRid.ReceiveItem25,
                        //    schoolRid.ReceiveItem26, schoolRid.ReceiveItem27, schoolRid.ReceiveItem28, schoolRid.ReceiveItem29, schoolRid.ReceiveItem30,
                        //    schoolRid.ReceiveItem31, schoolRid.ReceiveItem32, schoolRid.ReceiveItem33, schoolRid.ReceiveItem34, schoolRid.ReceiveItem35,
                        //    schoolRid.ReceiveItem36, schoolRid.ReceiveItem37, schoolRid.ReceiveItem38, schoolRid.ReceiveItem39, schoolRid.ReceiveItem40
                        //};
                        #endregion

                        KeyValueList fieldValues = new KeyValueList(fields.Length);
                        for (int idx = 0; idx < fields.Length; idx++)
                        {
                            #region [Old] 改為一律覆蓋，所以不用判斷舊資料
                            //string value = idx < receiveItemNames.Length ? receiveItemNames[idx] : null;
                            //if (String.IsNullOrWhiteSpace(values[idx]) && !String.IsNullOrEmpty(value))
                            //{
                            //    fieldValues.Add(fields[idx], value);
                            //}
                            #endregion

                            string value = idx < receiveItemNames.Length ? receiveItemNames[idx] : null;
                            fieldValues.Add(fields[idx], value);
                        }
                        if (fieldValues.Count > 0)
                        {
                            int count = 0;
                            Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, schoolRid.ReceiveType)
                                .And(SchoolRidEntity.Field.YearId, schoolRid.YearId)
                                .And(SchoolRidEntity.Field.TermId, schoolRid.TermId)
                                .And(SchoolRidEntity.Field.DepId, schoolRid.DepId)
                                .And(SchoolRidEntity.Field.ReceiveId, schoolRid.ReceiveId)
                                .And(SchoolRidEntity.Field.ReceiveStatus, schoolRid.ReceiveStatus);
                            _Factory.UpdateFields<SchoolRidEntity>(fieldValues, where, out count);
                        }
                    }
                }
                #endregion

                return importResult;
            }
            #endregion
        }
        #endregion

        #region Import BUG File(上傳學生基本資料)
        /// <summary>
        /// 匯入 BUG (上傳學生基本資料) 批次處理序列的資料
        /// </summary>
        /// <param name="job"></param>
        /// <param name="encoding"></param>
        /// <param name="isBatch">指定是否批次處理檔案內容 (資料庫處理不使用交易機制)</param>
        /// <param name="logmsg"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <returns></returns>
        public Result ImportBUGJob(JobcubeEntity job, Encoding encoding, bool isBatch, out string logmsg, out Int32 totalCount, out Int32 successCount)
        {
            logmsg = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                logmsg = "資料存取物件未準備好";
                return new Result(false, logmsg, ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (job.Jtypeid != JobCubeTypeCodeTexts.BUG)
            {
                logmsg = String.Format("批次處理序列 {0} 的類別不符合", job.Jno);
                return new Result(false, logmsg, ErrorCode.S_INVALID_PARAMETER, null);
            }

            string receiveType = job.Jrid;
            string depId = String.Empty;    //土銀不使用原部別，所以學生的部別固定為空字串

            if (String.IsNullOrEmpty(receiveType))
            {
                logmsg = String.Format("批次處理序列 {0} 缺少商家代號的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            Int64 cancel = 0;
            if (String.IsNullOrEmpty(job.Chancel) || !Int64.TryParse(job.Chancel, out cancel) || cancel < 1)
            {
                logmsg = String.Format("批次處理序列 {0} 缺少上傳資料檔案庫序號的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            int seriroNo = 0;
            if (String.IsNullOrEmpty(job.SeriorNo) || !Int32.TryParse(job.SeriorNo, out seriroNo) || seriroNo < 1)
            {
                logmsg = String.Format("批次處理序列 {0} 缺少上傳批號的資料參數或資料不正確", job.Jno);
                return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 拆解 JobcubeEntity 參數
            bool isParamOK = false;
            string owner = null;
            string sheetName = null;
            string fileType = null;
            {
                string pReceiveType = null;
                string pFileName = null;
                string pBankPMCancel = null;
                string pUpNo = null;
                isParamOK = JobcubeEntity.ParseBUGParameter(job.Jparam, out owner, out pReceiveType, out pFileName, out sheetName, out pBankPMCancel, out pUpNo);
                if (!isParamOK)
                {
                    logmsg = String.Format("批次處理序列 {0} 的參數資料不正確，或處理失敗", job.Jno);
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }

                if (String.IsNullOrEmpty(pReceiveType) || pReceiveType != receiveType)
                {
                    logmsg = String.Format("批次處理序列 {0} 缺少商家代號參數或不正確", job.Jno);
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }

                if (!String.IsNullOrEmpty(pFileName))
                {
                    fileType = Path.GetExtension(pFileName).ToLower();
                    if (fileType.StartsWith("."))
                    {
                        fileType = fileType.Substring(1);
                    }
                }

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (fileType != "xls" && fileType != "xlsx" && fileType != "ods")
                {
                    logmsg = String.Format("批次處理序列 {0} 缺上傳檔案名稱參數或副檔名不正確", job.Jno);
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                #endregion

                Int64 bankPMCancel = 0;
                if (String.IsNullOrEmpty(pBankPMCancel) || !Int64.TryParse(pBankPMCancel, out bankPMCancel) || bankPMCancel != cancel)
                {
                    logmsg = String.Format("批次處理序列 {0} 缺少上傳資料檔案庫序號參數或不正確", job.Jno);
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }

                int upNo = 0;
                if (String.IsNullOrEmpty(pUpNo) || !Int32.TryParse(pUpNo, out upNo) || upNo != seriroNo)
                {
                    logmsg = String.Format("批次處理序列 {0} 缺少上傳批號參數或不正確", job.Jno);
                    return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
            }
            #endregion

            #region 取上傳檔案
            Byte[] fileContent = null;
            {
                BankpmEntity instance = null;
                Expression where = new Expression(BankpmEntity.Field.Cancel, cancel)
                    .And(BankpmEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<BankpmEntity>(where, null, out instance);
                if (!result.IsSuccess)
                {
                    logmsg = "讀取上傳檔案資料失敗，" + result.Message;
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (instance == null)
                {
                    logmsg = String.Format("查無序號 {0} 的上傳檔案資料", cancel);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                fileContent = instance.Tempfile;
                string textContent = instance.Filedetail;
                if (!String.IsNullOrEmpty(instance.Filename))
                {
                    string type = Path.GetExtension(instance.Filename).ToLower();
                    if (type.StartsWith("."))
                    {
                        type = type.Substring(1);
                    }
                    if (!String.IsNullOrEmpty(type) && type != fileType)
                    {
                        logmsg = "上傳檔案資料的檔案型別與批次處理序列指定的檔案型別不同";
                        return new Result(false, logmsg, CoreStatusCode.INVALID_PARAMETER, null);
                    }
                }
                if (fileType == "txt" && !String.IsNullOrEmpty(textContent) && (fileContent == null || fileContent.Length == 0))
                {
                    fileContent = encoding.GetBytes(textContent);
                }
                if (fileContent == null || fileContent.Length == 0)
                {
                    logmsg = "上傳檔案無資料";
                    return new Result(false, logmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    logmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, logmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    logmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            List<string> fieldNames = new List<string>();   //收集欄位名稱
            {
                string errmsg = null;
                ConvertFileHelper fileHelper = new ConvertFileHelper();
                switch (fileType)
                {
                    case "xls":
                        #region Xls 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            if (!fileHelper.Xls2DataTable(ms, sheetName, StudentMasterEntity.GetXlsMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg))
                            {
                                #region 轉失敗則收集每一筆資料的錯誤訊息
                                if (table != null && table.Rows.Count > 0)
                                {
                                    StringBuilder log = new StringBuilder();
                                    int rowNo = 0;
                                    foreach (DataRow row in table.Rows)
                                    {
                                        rowNo++;
                                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                        if (!String.IsNullOrEmpty(failMsg))
                                        {
                                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        }
                                    }
                                    logmsg = log.ToString();
                                }
                                #endregion
                                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        #endregion
                        break;
                    case "xlsx":
                        #region Xlsx 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            if (!fileHelper.Xlsx2DataTable(ms, sheetName, StudentMasterEntity.GetXlsMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg))
                            {
                                #region 轉失敗則收集每一筆資料的錯誤訊息
                                if (table != null && table.Rows.Count > 0)
                                {
                                    StringBuilder log = new StringBuilder();
                                    int rowNo = 0;
                                    foreach (DataRow row in table.Rows)
                                    {
                                        rowNo++;
                                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                        if (!String.IsNullOrEmpty(failMsg))
                                        {
                                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        }
                                    }
                                    logmsg = log.ToString();
                                }
                                #endregion
                                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        #endregion
                        break;

                    #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                    case "ods":
                        #region Ods 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            if (!fileHelper.Ods2DataTable(ms, sheetName, StudentMasterEntity.GetXlsMapFields(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg))
                            {
                                #region 轉失敗則收集每一筆資料的錯誤訊息
                                if (table != null && table.Rows.Count > 0)
                                {
                                    StringBuilder log = new StringBuilder();
                                    int rowNo = 0;
                                    foreach (DataRow row in table.Rows)
                                    {
                                        rowNo++;
                                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                        if (!String.IsNullOrEmpty(failMsg))
                                        {
                                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                        }
                                    }
                                    logmsg = log.ToString();
                                }
                                #endregion
                                return new Result(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                            }
                        }
                        #endregion
                        break;
                    #endregion

                    default:
                        #region 不支援
                        {
                            logmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                            return new Result(false, logmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                        #endregion
                }

                foreach (DataColumn column in table.Columns)
                {
                    fieldNames.Add(column.ColumnName);
                }

                #region [MDY:20200815] M202008_02 學生身分證字號不可與學號相同 (2020806_01)
                if (table != null && table.Rows.Count > 0
                    && fieldNames.Contains(StudentMasterEntity.Field.Id) && fieldNames.Contains(StudentMasterEntity.Field.IdNumber))
                {
                    foreach (DataRow dRow in table.Rows)
                    {
                        string failMsg = dRow.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : dRow[ConvertFileHelper.DataLineFailureFieldName].ToString();
                        if (String.IsNullOrEmpty(failMsg))
                        {
                            string stuId = dRow.IsNull(StudentMasterEntity.Field.Id) ? null : dRow[StudentMasterEntity.Field.Id].ToString();
                            string isNumber = dRow.IsNull(StudentMasterEntity.Field.IdNumber) ? null : dRow[StudentMasterEntity.Field.IdNumber].ToString();
                            if (!String.IsNullOrEmpty(isNumber) && isNumber.Equals(stuId))
                            {
                                dRow[ConvertFileHelper.DataLineFailureFieldName] = "學生身分證字號不可與學號相同";
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 使用非交易將 DataTable 逐筆存入資料庫
            {
                successCount = 0;
                DateTime now = DateTime.Now;
                StringBuilder log = new StringBuilder();
                int insertCount = 0;    //新增 StudentMaster 的筆數
                int updateCount = 0;    //更新 StudentMaster 的筆數
                using (EntityFactory factory = _Factory.CloneForNonTransaction())
                {
                    List<string> studentIds = new List<string>(table.Rows.Count);

                    int rowNo = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        rowNo++;

                        #region 取得資料行錯誤訊息
                        {
                            string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                            if (!String.IsNullOrEmpty(failMsg))
                            {
                                log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                continue;
                            }
                        }
                        #endregion

                        string studentId = row[StudentMasterEntity.Field.Id].ToString();  //這個是 PKey 一定要有

                        #region 同一批上傳資料學號不可重複
                        if (studentIds.Contains(studentId))
                        {
                            string failMsg = String.Format("學號 {0} 重複", studentId);
                            //row[ConvertFileHelper.DataLineFailureFieldName] = failMsg;
                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                            continue;
                        }
                        #endregion

                        #region 產生學生基本資料 Entity (StudentMasterEntity)
                        StudentMasterEntity student = new StudentMasterEntity();
                        student.ReceiveType = receiveType;
                        student.DepId = depId;
                        student.Id = studentId;

                        //雖然目前是對照檔的欄位是固定的，但為了相容性以下的程式還是用 fieldNames.Contains 來決定是否取值
                        student.Name = fieldNames.Contains(StudentMasterEntity.Field.Name) ? row[StudentMasterEntity.Field.Name].ToString() : String.Empty;
                        student.Birthday = fieldNames.Contains(StudentMasterEntity.Field.Birthday) ? row[StudentMasterEntity.Field.Birthday].ToString() : String.Empty;
                        if (!String.IsNullOrWhiteSpace(student.Birthday))
                        {
                            //如果是日期的字串，格式化成 TWDate7
                            DateTime? date = DataFormat.ConvertDateText(student.Birthday.Trim());
                            if (date != null)
                            {
                                student.Birthday = Common.GetTWDate7(date.Value);
                            }
                        }
                        student.IdNumber = fieldNames.Contains(StudentMasterEntity.Field.IdNumber) ? row[StudentMasterEntity.Field.IdNumber].ToString().ToUpper() : String.Empty;
                        student.Tel = fieldNames.Contains(StudentMasterEntity.Field.Tel) ? row[StudentMasterEntity.Field.Tel].ToString() : String.Empty;
                        student.ZipCode = fieldNames.Contains(StudentMasterEntity.Field.ZipCode) ? row[StudentMasterEntity.Field.ZipCode].ToString() : String.Empty;
                        student.Address = fieldNames.Contains(StudentMasterEntity.Field.Address) ? row[StudentMasterEntity.Field.Address].ToString() : String.Empty;
                        student.Email = fieldNames.Contains(StudentMasterEntity.Field.Email) ? row[StudentMasterEntity.Field.Email].ToString() : String.Empty;
                        student.Account = String.Empty; //土銀不使用，所以固定為空字串
                        student.StuParent = fieldNames.Contains(StudentMasterEntity.Field.StuParent) ? row[StudentMasterEntity.Field.StuParent].ToString() : String.Empty;
                        student.CrtDate = now;
                        student.MdyDate = null;
                        #endregion

                        #region 寫入資料庫
                        {
                            int count = 0;
                            string action = null;

                            Expression where = new Expression(StudentMasterEntity.Field.ReceiveType, student.ReceiveType)
                                .And(StudentMasterEntity.Field.DepId, student.DepId)
                                .And(StudentMasterEntity.Field.Id, student.Id);
                            Result result = factory.SelectCount<StudentMasterEntity>(where, out count);
                            if (result.IsSuccess)
                            {
                                if (count == 0)
                                {
                                    action = "新增";

                                    #region Insert
                                    result = factory.Insert(student, out count);
                                    if (result.IsSuccess && count == 0)
                                    {
                                        result = new Result(false, "學生資料已存在", ErrorCode.D_DATA_EXISTS, null);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    action = "更新";

                                    #region Update
                                    KeyValueList fieldValues = new KeyValueList();
                                    fieldValues.Add(StudentMasterEntity.Field.Name, student.Name);
                                    fieldValues.Add(StudentMasterEntity.Field.Birthday, student.Birthday);
                                    fieldValues.Add(StudentMasterEntity.Field.IdNumber, student.IdNumber);
                                    fieldValues.Add(StudentMasterEntity.Field.Tel, student.Tel);
                                    fieldValues.Add(StudentMasterEntity.Field.ZipCode, student.ZipCode);
                                    fieldValues.Add(StudentMasterEntity.Field.Address, student.Address);
                                    fieldValues.Add(StudentMasterEntity.Field.Email, student.Email);
                                    fieldValues.Add(StudentMasterEntity.Field.Account, student.Account);
                                    fieldValues.Add(StudentMasterEntity.Field.StuParent, student.StuParent);

                                    fieldValues.Add(StudentMasterEntity.Field.MdyDate, now);

                                    result = factory.UpdateFields<StudentMasterEntity>(fieldValues, where, out count);
                                    if (result.IsSuccess && count == 0)
                                    {
                                        result = new Result(false, "學生繳費資料不存在", ErrorCode.D_DATA_NOT_FOUND, null);
                                    }
                                    #endregion
                                }
                            }

                            if (result.IsSuccess)
                            {
                                successCount++;
                                if (action == "新增")
                                {
                                    insertCount++;
                                }
                                else
                                {
                                    updateCount++;
                                }
                            }
                            else
                            {
                                log.AppendFormat("第 {0} 筆資料{1}失敗，錯誤訊息：{2}", rowNo, action, result.Message).AppendLine();
                            }
                        }
                        #endregion
                    }
                }

                #region 產生統計的日誌
                {
                    log.AppendFormat("新增 {0} 筆資料，更新 {1} 筆資料", insertCount, updateCount).AppendLine();
                }
                #endregion

                logmsg = log.ToString();
                if (successCount == 0)
                {
                    return new Result(false, "無資料被匯入成功", CoreStatusCode.UNKNOWN_ERROR, null);
                }
                else
                {
                    return new Result(true);
                }
            }
            #endregion
        }
        #endregion

        #region Import QueueCTCB File
        #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
        /// <summary>
        /// 匯入上傳中國信託繳費單資料檔
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="payDueDate"></param>
        /// <param name="fileContent"></param>
        /// <param name="userId"></param>
        /// <param name="dataInfo"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public Result ImportQueueCTCBFile(string receiveType, DateTime payDueDate, byte[] fileContent, string userId, out string dataInfo, string fileType = "xls")
        {
            dataInfo = null;

            #region 檢查 IsReady
            if (!this.IsReady())
            {
                return new Result(false, "資料存取物件未準備好", ErrorCode.S_INVALID_FACTORY, null);
            }
            #endregion

            #region 檢查參數
            if (String.IsNullOrEmpty(receiveType))
            {
                return new Result(false, "缺少商家代號的參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (fileContent == null || fileContent.Length == 0)
            {
                return new Result(false, "缺少檔案內容的參數", CoreStatusCode.INVALID_PARAMETER, null);
            }

            if (fileType != "xls" && fileType != "xlsx" && fileType != "ods")
            {
                return new Result(false, "不支援的檔案格式", CoreStatusCode.INVALID_PARAMETER, null);
            }
            #endregion

            #region 取得商家代號資料
            SchoolRTypeEntity school = null;
            int? cancelNoSize = null;
            {
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
                Result result = _Factory.SelectFirst<SchoolRTypeEntity>(where, null, out school);
                if (!result.IsSuccess)
                {
                    string errmsg = String.Format("讀取商家代號 {0} 的資料失敗，{1}", receiveType, result.Message);
                    return new Result(false, errmsg, result.Code, result.Exception);
                }
                if (school == null)
                {
                    string errmsg = String.Format("查無商家代號 {0} 的資料", receiveType);
                    return new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                }
                if (school.ReceiveKind != ReceiveKindCodeTexts.UPCTCB)
                {
                    string errmsg = String.Format("此商家代號 {0} 非 [代收各項費用] 種類", receiveType);
                    return new Result(false, errmsg, CoreStatusCode.INVALID_PARAMETER, null);
                }
                CancelNoHelper helper = new CancelNoHelper();
                CancelNoHelper.Module module = helper.GetModuleById(school.CancelNoRule);
                if (module != null)
                {
                    cancelNoSize = module.CancelNoSize;
                }
            }
            #endregion

            #region 檔案內容轉成 DataTable
            DataTable table = null;
            {
                string sheetName = "sheet1";
                XlsMapField[] mapFields = new XlsMapField[] {
                    new XlsMapField(QueueCTCBEntity.Field.StuId, "學號", new CodeChecker(1, 10)),
                    new XlsMapField(QueueCTCBEntity.Field.StuName, "姓名", new WordChecker(0, 10)),
                    new XlsMapField(QueueCTCBEntity.Field.CancelNo, "虛擬帳號", new CancelNoChecker(false, receiveType, cancelNoSize)),
                    new XlsMapField(QueueCTCBEntity.Field.ReceiveAmount, "金額", new DecimalChecker(1, 9999999))
                };

                string errmsg = null;
                Int32 totalCount = 0;
                Int32 successCount = 0;
                ConvertFileHelper fileHelper = new ConvertFileHelper();
                try
                {
                    bool isOK = false;
                    using (MemoryStream ms = new MemoryStream(fileContent))
                    {
                        bool isBatch = false;
                        bool isCheckValue = true;
                        if (fileType == "xls")
                        {
                            isOK = fileHelper.Xls2DataTable(ms, sheetName, mapFields, isBatch, isCheckValue, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else if (fileType == "xlsx")
                        {
                            isOK = fileHelper.Xlsx2DataTable(ms, sheetName, mapFields, isBatch, isCheckValue, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else if (fileType == "ods")
                        {
                            isOK = fileHelper.Ods2DataTable(ms, sheetName, mapFields, isBatch, isCheckValue, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        else
                        {
                            errmsg = String.Format("不支援 {0} 格式的資料匯入", fileType);
                            return new Result(false, errmsg, ErrorCode.D_DATA_NOT_FOUND, null);
                        }
                    }
                    if (!isOK)
                    {
                        if (table != null && table.Rows.Count > 0)
                        {
                            StringBuilder log = new StringBuilder();
                            int rowNo = 0;
                            foreach (DataRow row in table.Rows)
                            {
                                rowNo++;
                                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                                if (!String.IsNullOrEmpty(failMsg))
                                {
                                    log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                                }
                            }
                            dataInfo = log.ToString();
                        }
                        //因為傳回 false 無法傳回 dataInfo，所以只能傳有錯誤訊息的 true，errmsg
                        return new Result(true, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                    }
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message, CoreStatusCode.UNKNOWN_ERROR, ex);
                }
            }
            #endregion

            #region 將 DataTable 逐筆存入資料庫
            if (table == null || table.Rows.Count == 0)
            {
                return new Result(false, "檔案內無資料", CoreStatusCode.UNKNOWN_ERROR, null);
            }
            else
            {
                Int32 successCount = 0;
                DateTime now = DateTime.Now;
                StringBuilder log = new StringBuilder();
                Result result = null;

                string sql = String.Format(@"INSERT INTO [{0}] 
([{1}], [{2}], [{3}], [{4}], [{5}], [{6}], [{7}], [{8}], [{9}], [{10}])
VALUES (@ReceiveType, @StuId, @StuName, @CancelNo, @ReceiveAmount, @PayDueDate, NULL, NULL, @CrtDate, @CrtUser)"
                    , QueueCTCBEntity.TABLE_NAME
                    , QueueCTCBEntity.Field.ReceiveType, QueueCTCBEntity.Field.StuId, QueueCTCBEntity.Field.StuName, QueueCTCBEntity.Field.CancelNo
                    , QueueCTCBEntity.Field.ReceiveAmount, QueueCTCBEntity.Field.PayDueDate, QueueCTCBEntity.Field.SendDate, QueueCTCBEntity.Field.SendStamp
                    , QueueCTCBEntity.Field.CrtDate, QueueCTCBEntity.Field.CrtUser);

                //注意，順序不能變
                KeyValue[] parameters = new KeyValue[] {
                    new KeyValue("@StuId", null),
                    new KeyValue("@StuName", null),
                    new KeyValue("@CancelNo", null),
                    new KeyValue("@ReceiveAmount", null),
                    new KeyValue("@ReceiveType", receiveType),
                    new KeyValue("@PayDueDate", Common.GetTWDate7(payDueDate)),
                    new KeyValue("@CrtDate", now),
                    new KeyValue("@CrtUser", userId)
                };

                int rowNo = 0;
                foreach (DataRow row in table.Rows)
                {
                    rowNo++;

                    #region 取得資料行錯誤訊息
                    {
                        string failMsg = row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                        if (!String.IsNullOrEmpty(failMsg))
                        {
                            log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg).AppendLine();
                            continue;
                        }
                    }
                    #endregion

                    string stuId = row.IsNull(QueueCTCBEntity.Field.StuId) ? null : row[QueueCTCBEntity.Field.StuId].ToString().Trim();
                    string stuName = row.IsNull(QueueCTCBEntity.Field.StuName) ? null : row[QueueCTCBEntity.Field.StuName].ToString().Trim();
                    string cancelNo = row.IsNull(QueueCTCBEntity.Field.CancelNo) ? null : row[QueueCTCBEntity.Field.CancelNo].ToString().Trim();
                    string receiveAmount = row.IsNull(QueueCTCBEntity.Field.ReceiveAmount) ? null : row[QueueCTCBEntity.Field.ReceiveAmount].ToString().Trim();
                    decimal amount = 0;
                    if (!System.Decimal.TryParse(receiveAmount, out amount))
                    {
                        log.AppendFormat("第 {0} 筆資料不正確，錯誤訊息：金額不是數值格式", rowNo).AppendLine();
                        continue;
                    }

                    //注意，順序不能變
                    parameters[0].Value = stuId;
                    parameters[1].Value = stuName ?? String.Empty;  //姓名允許空的
                    parameters[2].Value = cancelNo;
                    parameters[3].Value = amount;

                    #region 寫入資料庫
                    {
                        int count = 0;
                        result = _Factory.ExecuteNonQuery(sql, parameters, out count);
                        if (result.IsSuccess)
                        {
                            if (count == 0)
                            {
                                log.AppendFormat("第 {0} 筆資料新增失敗，錯誤訊息：無資料被新增", rowNo).AppendLine();
                            }
                            else
                            {
                                successCount++;
                            }
                        }
                        else
                        {
                            log.AppendFormat("第 {0} 筆資料新增失敗，錯誤訊息：{1}", rowNo, result.Message).AppendLine();
                        }
                    }
                    #endregion
                }

                log.AppendFormat("共 {0} 筆資料，新增 {1} 筆，失敗 {2} 筆", table.Rows.Count, successCount, table.Rows.Count - successCount).AppendLine();
                dataInfo = log.ToString();

                //不管成功幾筆，都傳回 true
                return new Result(true);
            }
            #endregion
        }
        #endregion
        #endregion
    }

    #region 對照欄位值檢查器
    /// <summary>
    /// 對照欄位值檢查器介面
    /// </summary>
    public interface IValueChecker
    {
        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        bool Check(string value, out string errmsg);
    }

    /// <summary>
    /// Int64 的欄位值檢查器
    /// </summary>
    public sealed class IntegerChecker : IValueChecker
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public Int64? MinValue
        {
            get;
            set;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public Int64? MaxValue
        {
            get;
            set;
        }

        public IntegerChecker()
        {

        }

        public IntegerChecker(Int64? minValue, Int64 maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public bool Check(string value, out string errmsg)
        {
            Int64 intValue = 0L;
            if (!Int64.TryParse(value, out intValue))
            {
                errmsg = "不是整數值";
                return false;
            }
            if (this.MinValue != null && intValue < this.MinValue.Value)
            {
                errmsg = String.Format("不可小於 {0}", this.MinValue.Value);
                return false;
            }
            if (this.MaxValue != null && intValue > this.MaxValue.Value)
            {
                errmsg = String.Format("不可大於 {0}", this.MaxValue.Value);
                return false;
            }
            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// Decimal 的欄位值檢查器
    /// </summary>
    public sealed class DecimalChecker : IValueChecker
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public System.Decimal? MinValue
        {
            get;
            set;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public System.Decimal? MaxValue
        {
            get;
            set;
        }

        public bool AllowEmpty
        {
            get;
            set;
        }

        public DecimalChecker(bool allowEmpty = true)
        {
            this.AllowEmpty = allowEmpty;
        }

        public DecimalChecker(System.Decimal? minValue, System.Decimal? maxValue, bool allowEmpty = true)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.AllowEmpty = allowEmpty;
        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public bool Check(string value, out string errmsg)
        {
            System.Decimal intValue = 0L;
            if (String.IsNullOrEmpty(value) && !this.AllowEmpty)
            {
                errmsg = "不允許無資料";
                return false;
            }

            if (!System.Decimal.TryParse(value, out intValue))
            {
                errmsg = "不是數值";
                return false;
            }
            if (this.MinValue != null && intValue < this.MinValue.Value)
            {
                errmsg = String.Format("不可小於 {0}", this.MinValue.Value);
                return false;
            }
            if (this.MaxValue != null && intValue > this.MaxValue.Value)
            {
                errmsg = String.Format("不可大於 {0}", this.MaxValue.Value);
                return false;
            }
            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// 規則運算式 的欄位值檢查器
    /// </summary>
    public class RegexChecker : IValueChecker
    {
        /// <summary>
        /// 最小長度
        /// </summary>
        public Int32? MinLength
        {
            get;
            set;
        }

        /// <summary>
        /// 最大長度
        /// </summary>
        public Int32? MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// 規則運算式
        /// </summary>
        protected Regex _Regular = null;
        internal Regex Regular
        {
            get
            {
                return _Regular;
            }
        }

        /// <summary>
        /// 規則運算式的描述
        /// </summary>
        protected string _RegularDesc = null;
        internal string RegularDesc
        {
            get
            {
                return _RegularDesc;
            }
        }

        public RegexChecker(Int32? minLength, Int32? maxLength, Regex regular, string regularDesc)
        {
            this.MinLength = minLength;
            this.MaxLength = maxLength;
            _Regular = regular;
            _RegularDesc = regularDesc == null ? String.Empty : regularDesc.Trim();
        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public virtual bool Check(string value, out string errmsg)
        {
            int length = value == null ? 0 : value.Length;
            if (this.MinLength != null && length < this.MinLength.Value)
            {
                errmsg = String.Format("不可小於 {0} 個字", this.MinLength.Value);
                return false;
            }
            if (this.MaxLength != null && length > this.MaxLength.Value)
            {
                errmsg = String.Format("不可大於 {0} 個字", this.MaxLength.Value);
                return false;
            }
            if (_Regular != null && !_Regular.IsMatch(value ?? String.Empty))
            {
                if (String.IsNullOrEmpty(_RegularDesc))
                {
                    errmsg = "不符合規則限制";
                }
                else
                {
                    errmsg = String.Format("不符合規則限制：{0}", _RegularDesc);
                }
                return false;
            }
            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// 數字 的欄位值檢查器
    /// </summary>
    public sealed class NumberChecker : RegexChecker
    {
        private static readonly Regex NumberRegex = new Regex("[0-9]*", RegexOptions.Compiled);

        public NumberChecker()
            : base(null, null, NumberRegex, "必須為數字")
        {

        }

        public NumberChecker(Int32? minLength, Int32? maxLength, string regularDesc)
            : base(minLength, maxLength, NumberRegex, regularDesc)
        {

        }
    }

    /// <summary>
    /// 文字 的欄位值檢查器
    /// </summary>
    public sealed class WordChecker : RegexChecker
    {
        public WordChecker(Int32? minLength, Int32? maxLength)
            : base(minLength, maxLength, null, null)
        {

        }
    }

    /// <summary>
    /// (字元)文字 的欄位值檢查器
    /// </summary>
    public sealed class CharChecker : IValueChecker
    {
        /// <summary>
        /// 最小長度
        /// </summary>
        public Int32? MinLength
        {
            get;
            set;
        }

        /// <summary>
        /// 最大長度
        /// </summary>
        public Int32? MaxLength
        {
            get;
            set;
        }

        public CharChecker(Int32? minLength, Int32? maxLength)
        {
            this.MinLength = minLength;
            this.MaxLength = maxLength;
        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public bool Check(string value, out string errmsg)
        {
            int length = value == null ? 0 : Encoding.Default.GetByteCount(value);
            if (this.MinLength != null && length < this.MinLength.Value)
            {
                errmsg = String.Format("不可小於 {0} 個字(字元)", this.MinLength.Value);
                return false;
            }
            if (this.MaxLength != null && length > this.MaxLength.Value)
            {
                errmsg = String.Format("不可大於 {0} 個字(字元)", this.MaxLength.Value);
                return false;
            }

            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// 代碼 (英數字) 的欄位值檢查器
    /// </summary>
    public sealed class CodeChecker : RegexChecker
    {
        public CodeChecker(Int32? minLength, Int32? maxLength)
            : base(minLength, maxLength, null, null)
        {

        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public override bool Check(string value, out string errmsg)
        {
            if (!base.Check(value, out errmsg))
            {
                return false;
            }

            if (!Common.IsEnglishNumber(value))
            {
                errmsg = "不符合規則限制：必須為全部英文、全部數字或全部由英數字混合";
                return false;
            }

            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// 日期時間 的欄位值檢查器
    /// </summary>
    public sealed class DateTimeChecker : IValueChecker
    {
        /// <summary>
        /// 日期時間格式列舉
        /// </summary>
        public enum FormatEnum
        {
            /// <summary>
            /// 西元年 yyyy/MM/dd
            /// </summary>
            Date10 = 1,
            /// <summary>
            /// 西元年 yyyyMMdd
            /// </summary>
            Date8 = 2,
            /// <summary>
            /// 民國年 yyyy/MM/dd
            /// </summary>
            TWDate10 = 3,
            /// <summary>
            /// 民國年 yyyyMMdd
            /// </summary>
            TWDate8 = 4,
            /// <summary>
            /// 民國年 yyyMMdd
            /// </summary>
            TWDate7 = 5,
            /// <summary>
            /// Date10 或 Date 8 或 TWDate7 或 TWDate6 或 符合 DateTime.TryParse()
            /// </summary>
            DateText = 6,
            /// <summary>
            /// HH:mm:ss
            /// </summary>
            Time8 = 7,
            /// <summary>
            /// HHmmss
            /// </summary>
            Time6 = 8,
            /// <summary>
            /// 西元年 yyyy/MM/dd HH:mm:ss
            /// </summary>
            DateTime = 9,

            /// <summary>
            /// Date10 或 Date 8 或 TWDate7 或 TWDate6 或 空字串 或 符合 DateTime.TryParse()
            /// </summary>
            DateTextOrEmpty = 10
        }

        private FormatEnum _Format = FormatEnum.Date8;
        /// <summary>
        /// 格式
        /// </summary>
        public FormatEnum Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        public DateTimeChecker()
        {

        }

        public DateTimeChecker(FormatEnum format)
        {
            this.Format = format;
        }

        Regex Time8Regex = new Regex("^([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", RegexOptions.Compiled);
        Regex Time6Regex = new Regex("^([0-1][0-9]|2[0-3])[0-5][0-9][0-5][0-9]$", RegexOptions.Compiled);

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public bool Check(string value, out string errmsg)
        {
            if (String.IsNullOrWhiteSpace(value) && this.Format != FormatEnum.DateTextOrEmpty)
            {
                errmsg = "不是日期時間格式";
                return false;
            }

            errmsg = null;
            switch (this.Format)
            {
                case FormatEnum.Date10:
                    #region
                    if (!Common.IsDate10(value))
                    {
                        errmsg = "不是西元年 YYYY/MM/DD 的日期格式";
                        return false;
                    }
                    return true;
                    #endregion
                case FormatEnum.Date8:
                    #region
                    if (!Common.IsDate8(value))
                    {
                        errmsg = "不是西元年 YYYYMMDD 的日期格式";
                        return false;
                    }
                    return true;
                    #endregion
                case FormatEnum.TWDate10:
                    #region
                    if (!Common.IsTWDate10(value))
                    {
                        errmsg = "不是民國年 YYYY/MM/DD 的日期格式";
                        return false;
                    }
                    return true;
                    #endregion
                case FormatEnum.TWDate8:
                    #region
                    if (!Common.IsTWDate8(value))
                    {
                        errmsg = "不是民國年 YYYYMMDD 的日期格式";
                        return false;
                    }
                    return true;
                    #endregion
                case FormatEnum.TWDate7:
                    #region
                    {
                        DateTime date;
                        if (!Common.TryConvertTWDate7(value, out date))
                        {
                            errmsg = "不是民國年 YYYMMDD 的日期格式";
                            return false;
                        }
                        return true;
                    }
                    #endregion
                case FormatEnum.DateText:
                    #region
                    {
                        DateTime? date = DataFormat.ConvertDateText(value);
                        if (date == null)
                        {
                            errmsg = "不是合法的日期格式";
                            return false;
                        }
                    }
                    return true;
                    #endregion
                case FormatEnum.Time8:
                    #region
                    if (!Time8Regex.IsMatch(value))
                    {
                        errmsg = "不是 HH:mm:ss 的日期格式";
                        return false;
                    }
                    return true;
                    #endregion
                case FormatEnum.Time6:
                    #region
                    if (!Time6Regex.IsMatch(value))
                    {
                        errmsg = "不是 HHmmss 的日期格式";
                        return false;
                    }
                    return true;
                    #endregion
                case FormatEnum.DateTime:
                    #region
                    {
                        DateTime date;
                        if (!DateTime.TryParse(value, out date))
                        {
                            errmsg = "不是 HHmmss 的日期格式";
                            return false;
                        }
                    }
                    return true;
                    #endregion

                case FormatEnum.DateTextOrEmpty:
                    #region
                    if (!String.IsNullOrEmpty(value))
                    {
                        DateTime? date = DataFormat.ConvertDateText(value);
                        if (date == null)
                        {
                            errmsg = "不是合法的日期格式";
                            return false;
                        }
                    }
                    return true;
                    #endregion
            }

            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// 中華民國身分證號(含居留證號) 的欄位值檢查器
    /// </summary>
    public sealed class PersonalIDChecker : IValueChecker
    {
        /// <summary>
        /// 是否驗證檢查碼
        /// </summary>
        public bool VerifyChecksum
        {
            get;
            set;
        }

        public PersonalIDChecker()
        {

        }

        public PersonalIDChecker(bool checksum)
        {
            this.VerifyChecksum = checksum;
        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public bool Check(string value, out string errmsg)
        {
            if (value != null)
            {
                value = value.ToUpper();
            }
            if (this.VerifyChecksum)
            {
                if (!Common.IsPersonalID2(value))
                {
                    errmsg = "不是合法的中華民國身分證號(含居留證號)";
                    return false;
                }
            }
            else
            {
                if (!Common.IsPersonalID(value))
                {
                    errmsg = "不符合中華民國身分證號(含居留證號)的格式";
                    return false;
                }
            }

            errmsg = null;
            return true;
        }
    }

    /// <summary>
    /// 虛擬帳號 的欄位值檢查器
    /// </summary>
    public sealed class CancelNoChecker : IValueChecker
    {
        /// <summary>
        /// 是否允無資料
        /// </summary>
        public bool AllowEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// 指定要符合的商家代號
        /// </summary>
        public string ReceiveType
        {
            get;
            private set;
        }

        public int? Length
        {
            get;
            private set;
        }

        public CancelNoChecker(bool allowEmpty, string receiveType = null, int? length = null)
        {
            this.ReceiveType = receiveType == null ? null : receiveType.Trim();
            this.Length = length;
        }

        /// <summary>
        /// 檢查指定對照欄位值是否符合限制
        /// </summary>
        /// <param name="value">指定對照欄位值</param>
        /// <param name="errmsg">符合則 null，否則傳回錯誤訊息</param>
        /// <returns>符合則傳回 true，否則傳回 false</returns>
        public bool Check(string value, out string errmsg)
        {
            errmsg = null;
            if (String.IsNullOrWhiteSpace(value))
            {
                if (this.AllowEmpty)
                {
                    return true;
                }
                else
                {
                    errmsg = "不允許無資料";
                    return false;
                }
            }

            value = value.Trim();
            if (this.Length != null)
            {
                if (this.Length != value.Length)
                {
                    errmsg = "資料長度不正確";
                    return false;
                }
            }
            else
            {
                if (value.Length != 14 && value.Length != 16)
                {
                    errmsg = "資料長度限制14或16碼";
                    return false;
                }
            }

            if (!String.IsNullOrEmpty(this.ReceiveType) && !value.StartsWith(this.ReceiveType))
            {
                errmsg = String.Format("不是商家代號 {0} 的資料", this.ReceiveType);
                return false;
            }

            errmsg = null;
            return true;
        }
    }
    #endregion

    #region MapField
    /// <summary>
    /// TXT 格式的對照欄位設定類別
    /// </summary>
    public sealed class TxtMapField
    {
        /// <summary>
        /// 對照欄位值檢查器
        /// </summary>
        private IValueChecker _ValueChecker = null;

        private string _Key = String.Empty;
        /// <summary>
        /// 對照欄位 Ky
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value == null ? String.Empty : value.Trim();
            }
        }

        private int _Start = 0;
        /// <summary>
        /// 對照欄位起始字元位置，最小值為 1，0 表示未設定
        /// </summary>
        public int Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value < 0 ? 0 : value;
            }
        }

        private int _Length = 0;
        /// <summary>
        /// 對照欄位起始字元位置，最小值為 1，0 表示未設定
        /// </summary>
        public int Length
        {
            get
            {
                return _Length;
            }
            set
            {
                _Length = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// 建構 TXT 格式的對照欄位設定類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="checker"></param>
        public TxtMapField(string key, int start, int length, IValueChecker checker)
        {
            this.Key = key;
            this.Start = start;
            this.Length = length;
            _ValueChecker = checker;
        }

        /// <summary>
        /// 檢查欄位值是否符合限制
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool Check(string value, out string errmsg)
        {
            if (_ValueChecker != null)
            {
                return _ValueChecker.Check(value, out errmsg);
            }
            else
            {
                errmsg = null;
                return true;
            }
        }

        /// <summary>
        /// 取得此物件的設定資料是否 Ready
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            return (!String.IsNullOrEmpty(this.Key) && this.Start > 0 && this.Length > 0);
        }
    }

    /// <summary>
    /// XLS 格式的對照欄位設定類別
    /// </summary>
    public sealed class XlsMapField
    {
        /// <summary>
        /// 對照欄位值檢查器
        /// </summary>
        private IValueChecker _ValueChecker = null;

        private string _Key = String.Empty;
        /// <summary>
        /// 對照欄位 Ky
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value == null ? String.Empty : value.Trim();
            }
        }

        private string _CellName = String.Empty;
        /// <summary>
        /// 對照欄位的Cell名稱
        /// </summary>
        public string CellName
        {
            get
            {
                return _CellName;
            }
            set
            {
                _CellName = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 建構 XLS 格式的對照欄位設定類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cellName"></param>
        /// <param name="checker"></param>
        public XlsMapField(string key, string cellName, IValueChecker checker)
        {
            this.Key = key;
            this.CellName = cellName;
            _ValueChecker = checker;
        }

        /// <summary>
        /// 檢查欄位值是否符合限制
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool Check(string value, out string errmsg)
        {
            if (_ValueChecker != null)
            {
                return _ValueChecker.Check(value, out errmsg);
            }
            else
            {
                errmsg = null;
                return true;
            }
        }

        /// <summary>
        /// 取得此物件的設定資料是否 Ready
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            return (!String.IsNullOrEmpty(this.Key) && !String.IsNullOrEmpty(CellName));
        }

        #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
        /// <summary>
        /// 取得此對照欄位設定類別的 Fuju.ODS.ODSMappingColumn 型別物件
        /// </summary>
        /// <param name="isValueChecker">指定是否要含 ValueChecker 物件。</param>
        /// <returns></returns>
        public Fuju.ODS.ODSMappingColumn GetMappingColumn(bool isValueChecker)
        {
            Fuju.ODS.ODSMappingColumn mappingColumn = new Fuju.ODS.ODSMappingColumn();
            mappingColumn.ColumnName = this.Key;
            mappingColumn.HeadCaption = this.CellName;
            if (isValueChecker && _ValueChecker != null)
            {
                if (_ValueChecker is IntegerChecker)
                {
                    IntegerChecker checker = _ValueChecker as IntegerChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.IntegerChecker(checker.MinValue, checker.MaxValue);
                }
                else if (_ValueChecker is DecimalChecker)
                {
                    DecimalChecker checker = _ValueChecker as DecimalChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.DecimalChecker(checker.MinValue, checker.MaxValue, checker.AllowEmpty);
                }
                else if (_ValueChecker is RegexChecker)
                {
                    RegexChecker checker = _ValueChecker as RegexChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.RegexChecker(checker.MinLength, checker.MaxLength, checker.Regular, checker.RegularDesc);
                }
                else if (_ValueChecker is NumberChecker)
                {
                    NumberChecker checker = _ValueChecker as NumberChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.NumberChecker(checker.MinLength, checker.MaxLength, checker.RegularDesc);
                }
                else if (_ValueChecker is WordChecker)
                {
                    WordChecker checker = _ValueChecker as WordChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.WordChecker(checker.MinLength, checker.MaxLength);
                }
                else if (_ValueChecker is CharChecker)
                {
                    CharChecker checker = _ValueChecker as CharChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.WordChecker(checker.MinLength, checker.MaxLength);
                }
                else if (_ValueChecker is CodeChecker)
                {
                    CodeChecker checker = _ValueChecker as CodeChecker;
                    mappingColumn.ValueChecker = new Fuju.ODS.CodeChecker(checker.MinLength, checker.MaxLength);
                }
                else if (_ValueChecker is DateTimeChecker)
                {
                    DateTimeChecker checker = _ValueChecker as DateTimeChecker;
                    switch(checker.Format)
                    {
                        case DateTimeChecker.FormatEnum.Date10:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.Date10);
                            break;
                        case DateTimeChecker.FormatEnum.Date8:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.Date8);
                            break;
                        case DateTimeChecker.FormatEnum.DateText:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.DateText);
                            break;
                        case DateTimeChecker.FormatEnum.DateTextOrEmpty:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.DateTextOrEmpty);
                            break;
                        case DateTimeChecker.FormatEnum.DateTime:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.DateTime);
                            break;
                        case DateTimeChecker.FormatEnum.Time6:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.Time6);
                            break;
                        case DateTimeChecker.FormatEnum.Time8:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.Time8);
                            break;
                        case DateTimeChecker.FormatEnum.TWDate10:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.TWDate10);
                            break;
                        case DateTimeChecker.FormatEnum.TWDate7:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.TWDate7);
                            break;
                        case DateTimeChecker.FormatEnum.TWDate8:
                            mappingColumn.ValueChecker = new Fuju.ODS.DateTimeChecker(Fuju.ODS.DateTimeChecker.FormatEnum.TWDate8);
                            break;
                    }
                }
            }
            return mappingColumn;
        }
        #endregion
    }

    /// <summary>
    /// 對照欄位值承載類別
    /// </summary>
    public sealed class MapFieldValue
    {
        private string _Key = String.Empty;
        /// <summary>
        /// 對照欄位 Ky
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value == null ? String.Empty : value.Trim();
            }
        }

        private string _Value = String.Empty;
        /// <summary>
        /// 對照欄位起始字元位置，最小值為 1，0 表示未設定
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value == null ? String.Empty : value.Trim();
            }
        }

        /// <summary>
        /// 建構對照欄位值承載類別
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public MapFieldValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
    #endregion

    #region ConvertHelper
    public sealed class ConvertFileHelper
    {
        #region Static Readonly
        /// <summary>
        /// 紀錄資料行原始文字的欄位名稱定義
        /// </summary>
        public static readonly string OriginalTextLineFieldName = "ORG_TXT_LINE";
        /// <summary>
        /// 紀錄資料行錯誤訊息的欄位名稱定義
        /// </summary>
        public static readonly string DataLineFailureFieldName = "FAIL_DATA_LINE";

        /// <summary>
        /// CSV 檔欄位中的逗號替代碼
        /// </summary>
        public static readonly string CsvCommaCode = "&#44;";
        #endregion

        #region Property
        //private string _comma = ",";
        //private string _comma_code = "&#44;";
        //public string comma_code
        //{
        //    get
        //    {
        //        return _comma_code;
        //    }
        //    set
        //    {
        //        _comma_code = value.Trim();
        //    }
        //}
        #endregion

        #region Constructor
        public ConvertFileHelper()
        {

        }
        #endregion

        #region Txt2DataTable
        /// <summary>
        /// 將指定的 Txt 檔讀入 DataTable
        /// </summary>
        /// <param name="txtFile">指定文字檔的路徑檔名</param>
        /// <param name="encoding">指定文字檔的編碼</param>
        /// <param name="mapFields">指定 TXT 格式的對照欄位設定陣列</param>
        /// <param name="isBatch">指定是否整批處理。指定 true 時任何一筆資料行處理失敗就算失敗，指定 false 時任何一筆資料行處理成功就算成功</param>
        /// <param name="isCheckValue">指定是否要檢查欄位值</param>
        /// <param name="table">成功則傳回 DataTable</param>
        /// <param name="totalCount">成功則傳回處理的總筆數</param>
        /// <param name="successCount">成功則傳回處理成功的筆數</param>
        /// <param name="errmsg">失敗則傳回錯誤訊息</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool Txt2DataTable(string txtFile, Encoding encoding, TxtMapField[] mapFields, bool isBatch, bool isCheckValue
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (String.IsNullOrEmpty(txtFile) || !File.Exists(txtFile))
            {
                errmsg = "未指定檔案參數或檔案不存在";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            #endregion

            #region 檢查對照欄位設定
            {
                foreach (TxtMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                }
            }
            #endregion

            bool isOK = false;
            try
            {
                using (StreamReader sr = new StreamReader(txtFile, encoding))
                {
                    isOK = this.Txt2DataTable(sr, mapFields, isBatch, isCheckValue, out table, out totalCount, out successCount, out errmsg);
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理檔案發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;
        }

        /// <summary>
        /// 將指定的 Txt 資料流讀入 DataTable
        /// </summary>
        /// <param name="reader">指定的 Txt 資料流</param>
        /// <param name="mapFields">指定 TXT 格式的對照欄位設定陣列</param>
        /// <param name="isBatch">指定是否整批處理。指定 true 時任何一筆資料行處理失敗就算失敗，指定 false 時任何一筆資料行處理成功就算成功</param>
        /// <param name="isCheckValue">指定是否要檢查欄位值</param>
        /// <param name="table">成功則傳回 DataTable</param>
        /// <param name="totalCount">成功則傳回處理的總筆數</param>
        /// <param name="successCount">成功則傳回處理成功的筆數</param>
        /// <param name="errmsg">失敗則傳回錯誤訊息</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool Txt2DataTable(StreamReader reader, TxtMapField[] mapFields, bool isBatch, bool isCheckValue
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (reader == null || reader.EndOfStream)
            {
                errmsg = "未指定資料流參數或資料流已結束";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            #endregion

            #region 檢查對照欄位設定，並取的資料行最小必要長度
            int minLengthLimit = 0;
            {
                int length = 0;
                foreach (TxtMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                    length = mapField.Start + mapField.Length - 1;
                    if (minLengthLimit < length)
                    {
                        minLengthLimit = length;
                    }
                }
            }
            #endregion

            errmsg = null;
            bool isOK = false;
            try
            {
                #region 初始化 DataTable (產生資料欄位)
                table = new DataTable();
                {
                    foreach (TxtMapField mapField in mapFields)
                    {
                        table.Columns.Add(new DataColumn(mapField.Key));
                    }
                    //紀錄原始文字的欄位
                    table.Columns.Add(new DataColumn(OriginalTextLineFieldName));
                    //紀錄資料錯誤的欄位
                    table.Columns.Add(new DataColumn(DataLineFailureFieldName));
                }
                #endregion

                #region 逐文字資料行處理
                {
                    Encoding encoding = reader.CurrentEncoding;
                    string line = null;
                    Byte[] buffer = null;
                    bool isBreak = false;
                    while ((line = reader.ReadLine()) != null && !isBreak)
                    {
                        totalCount++;

                        DataRow row = table.NewRow();
                        row[OriginalTextLineFieldName] = line;

                        #region 處理資料行
                        try
                        {
                            buffer = encoding.GetBytes(line);
                            if (buffer.Length < minLengthLimit)
                            {
                                row[DataLineFailureFieldName] = String.Format("資料行長度不足 {0} 字元", minLengthLimit);
                                if (isBatch)
                                {
                                    isBreak = true;
                                    errmsg = String.Format("第{0}行的資料長度不足 {1} 字元", totalCount, minLengthLimit);;
                                }
                            }
                            else
                            {
                                string valueError = null;
                                foreach (TxtMapField mapField in mapFields)
                                {
                                    string value = encoding.GetString(buffer, mapField.Start - 1, mapField.Length).Trim();
                                    row[mapField.Key] = value;

                                    if (isCheckValue && valueError == null)
                                    {
                                        if (!mapField.Check(value, out valueError))
                                        {
                                            valueError = String.Format("第 {0} 字元開始的值不正確，{1}", mapField.Start, valueError);
                                            row[DataLineFailureFieldName] = valueError;
                                        }
                                    }
                                }

                                if (String.IsNullOrEmpty(valueError))
                                {
                                    row[DataLineFailureFieldName] = String.Empty;
                                    successCount++;
                                }
                                else if (isBatch)
                                {
                                    isBreak = true;
                                    errmsg = String.Format("第{0}行的資料不正確，錯誤訊息：{1}", totalCount, valueError);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            row[DataLineFailureFieldName] = String.Format("處理資料行發生例外，錯誤訊息：{0}", ex.Message);
                            if (isBatch)
                            {
                                isBreak = true;
                                errmsg = String.Format("第{0}行的資料處理發生例外，錯誤訊息：{1}", totalCount, ex.Message);
                            }
                        }
                        #endregion

                        table.Rows.Add(row);

                        if (isBreak)
                        {
                            break;
                        }
                    }

                    if (isBatch)
                    {
                        //指定整批處理時，如被中斷表示失敗
                        isOK = !isBreak;
                    }
                    else
                    {
                        //非整批處理時，任一筆成功就算成功
                        isOK = (successCount > 0);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理資料發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;
        }
        #endregion

        #region Xls2DataTable
        /// <summary>
        /// 將指定的 Xls 檔讀入 DataTable
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetName"></param>
        /// <param name="encoding"></param>
        /// <param name="mapFields"></param>
        /// <param name="isBatch"></param>
        /// <param name="isCheckValue"></param>
        /// <param name="headRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool Xls2DataTable(string xlsFile, string sheetName, Encoding encoding, XlsMapField[] mapFields, bool isBatch, bool isCheckValue
            , Int32 headRowIndex
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (String.IsNullOrEmpty(xlsFile) || !File.Exists(xlsFile))
            {
                errmsg = "未指定檔案參數或檔案不存在";
                return false;
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                errmsg = "未指定 Excel 的 Sheet 名稱";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            #endregion

            #region 檢查對照欄位設定
            {
                foreach (XlsMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                }
            }
            #endregion

            bool isOK = false;
            try
            {
                using (FileStream fs = new FileStream(xlsFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    isOK = this.Xls2DataTable(fs, sheetName, mapFields, isBatch, isCheckValue, headRowIndex
                        , out table, out totalCount, out successCount, out errmsg);
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理檔案發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;

            #region [Old]
            //msg = "";
            //bool rc = false;

            //HSSFWorkbook wb = new HSSFWorkbook();
            ////NPOI.POIFS.FileSystem.POIFSDocument fs;
            //HSSFSheet sheet;
            //StreamWriter sw = null;
            //Int32 columns = 0;
            //Int32 rows = 0;
            //HSSFRow xlsRow;
            //HSSFCell cell;
            ////string cell_value = "";
            //string line = "";
            //StringBuilder log = new StringBuilder();
            //StringBuilder buff = new StringBuilder();
            //try
            //{
            //    //sr = new StreamReader(xls_file, coding);
            //    sw = new StreamWriter(csv_file, false, encoding);
            //    //fs = new POIFSDocument(sr.BaseStream);

            //    wb = new HSSFWorkbook(new FileStream(xlsFile, FileMode.Open, FileAccess.Read, FileShare.Read));
            //    sheet = wb.GetSheet(sheet_name);

            //    #region New (20120924 Joe)
            //    if (sheet == null)
            //    {
            //        throw new Exception(String.Format("sheet name {0} 不存在", sheet_name));
            //    }
            //    #endregion

            //    Int32 idxRow = startRow;
            //    rows = sheet.LastRowNum - sheet.FirstRowNum;
            //    xlsRow = sheet.GetRow(idxRow);
            //    columns = (xlsRow.LastCellNum - 1) - xlsRow.FirstCellNum + 1;
            //    for (Int32 i = idxRow; i <= sheet.LastRowNum; i++)
            //    {
            //        #region Old (20121001 Joe)
            //        //xlsRow = sheet.GetRow(i);
            //        //line = "";
            //        //#region column
            //        //for (Int32 j = 0; j < columns; j++)
            //        //{
            //        //    try
            //        //    {
            //        //        cell = xlsRow.GetCell(j);
            //        //        if (cell != null)
            //        //        {
            //        //            if (cell.CellType == HSSFCell.CELL_TYPE_FORMULA)
            //        //            {
            //        //                cell_value = cell.NumericCellValue.ToString();
            //        //            }
            //        //            else if (cell.CellType == HSSFCell.CELL_TYPE_NUMERIC)
            //        //            {
            //        //                cell_value = cell.NumericCellValue.ToString();
            //        //            }
            //        //            else if (cell.CellType == HSSFCell.CELL_TYPE_BLANK)
            //        //            {
            //        //                cell_value = "";
            //        //            }
            //        //            else
            //        //            {
            //        //                cell_value = cell.StringCellValue;
            //        //            }
            //        //        }
            //        //        else
            //        //        {
            //        //            cell_value = "";
            //        //        }

            //        //        if (i == idxRow && header_mapping!=null)
            //        //        {
            //        //            try
            //        //            {
            //        //                cell_value = (string)header_mapping[cell_value];
            //        //            }
            //        //            catch (Exception ex)
            //        //            {

            //        //            }
            //        //        }
            //        //        cell_value = cell_value.Replace(_comma, _comma_code);
            //        //        line += line != "" ? "," + cell_value : cell_value;
            //        //    }
            //        //    catch (Exception ex)
            //        //    {
            //        //        log.AppendLine(string.Format("處理第{0}列{1}行發生錯誤,err=[{2}] {3}", (i + 1).ToString(), (j + 1).ToString(), ex.Source, ex.Message));
            //        //    }
            //        //}
            //        //#endregion
            //        //buff.AppendLine(line);
            //        #endregion

            //        #region New (20121001 Joe)
            //        List<string> cellValues = new List<string>();
            //        xlsRow = sheet.GetRow(i);

            //        #region columns
            //        for (int cellIndex = 0; cellIndex < columns; cellIndex++)
            //        {
            //            try
            //            {
            //                cell = xlsRow.GetCell(cellIndex);
            //                string cellValue = null;
            //                if (cell != null)
            //                {
            //                    #region [Old] 20130702_1 改寫 CELL_TYPE_NUMERIC 格式的處理 (20130524_03)
            //                    //switch (cell.CellType)
            //                    //{
            //                    //    case HSSFCell.CELL_TYPE_FORMULA:
            //                    //        //檔Cell是參考其他欄位且設定為通用格式時，
            //                    //        //cell無法確定是 string 還是 numeric，
            //                    //        //所以先用StringCellValue取，如果失敗改用 NumericCellValue，
            //                    //        //在失敗就觸發外面的 catch
            //                    //        try
            //                    //        {
            //                    //            cellValue = cell.StringCellValue;
            //                    //        }
            //                    //        catch
            //                    //        {
            //                    //            cellValue = cell.NumericCellValue.ToString();
            //                    //        }
            //                    //        break;
            //                    //    case HSSFCell.CELL_TYPE_NUMERIC:
            //                    //        cellValue = cell.NumericCellValue.ToString();
            //                    //        break;
            //                    //    case HSSFCell.CELL_TYPE_BLANK:
            //                    //        cellValue = String.Empty;
            //                    //        break;
            //                    //    default:
            //                    //        cellValue = cell.StringCellValue;
            //                    //        break;
            //                    //}
            //                    #endregion

            //                    #region [New] 20130702_1 改寫 CELL_TYPE_NUMERIC 格式的處理 (20130524_03)
            //                    switch (cell.CellType)
            //                    {
            //                        case HSSFCell.CELL_TYPE_FORMULA:
            //                            NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate aggregate = cell.CellValueRecord as NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate;
            //                            if (aggregate.StringRecord != null)
            //                            {
            //                                cellValue = aggregate.StringValue;
            //                            }
            //                            else
            //                            {
            //                                cellValue = aggregate.FormulaRecord.Value.ToString();
            //                            }
            //                            break;
            //                        case HSSFCell.CELL_TYPE_NUMERIC:	//預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
            //                            cellValue = cell.ToString();
            //                            if (cellValue.StartsWith("_*"))	//會計格式會用這個字串開頭
            //                            {
            //                                cellValue = cellValue.Substring(2);
            //                            }
            //                            decimal val;
            //                            if (!Decimal.TryParse(cellValue, out val))
            //                            {
            //                                cellValue = cell.NumericCellValue.ToString();
            //                            }
            //                            break;
            //                        case HSSFCell.CELL_TYPE_BLANK:
            //                            cellValue = String.Empty;
            //                            break;
            //                        default:
            //                            cellValue = cell.StringCellValue;
            //                            break;
            //                    }
            //                    #endregion
            //                }
            //                else
            //                {
            //                    cellValue = String.Empty;
            //                }

            //                if (i == idxRow && header_mapping != null)
            //                {
            //                    object value = header_mapping[cellValue];
            //                    if (value is string)
            //                    {
            //                        cellValue = value as string;
            //                    }
            //                }
            //                cellValues.Add(cellValue.Replace(_comma, _comma_code));
            //            }
            //            catch (Exception ex)
            //            {
            //                log.AppendLine(string.Format("處理第{0}列{1}行發生錯誤,err=[{2}] {3}", (i + 1).ToString(), (cellIndex + 1).ToString(), ex.Source, ex.Message));
            //            }
            //        }
            //        #endregion

            //        if (cellValues.Count == columns)
            //        {
            //            buff.AppendLine(String.Join(",", cellValues.ToArray()));
            //        }
            //        #endregion
            //    }

            //    sw.Write(buff.ToString());
            //    sw.Close();
            //    sw.Dispose();
            //    sw = null;
            //    wb = null;
            //    rc = true;
            //}
            //catch (Exception ex)
            //{
            //    log.AppendLine(string.Format("轉檔發生錯誤,err=[{0}] {1}", ex.Source, ex.Message));
            //}
            //finally
            //{
            //    if (sw != null)
            //    {
            //        sw.Close();
            //        sw.Dispose();
            //        sw = null;
            //    }
            //    msg = log.ToString();
            //}

            //return rc;
            #endregion
        }

        /// <summary>
        /// 將指定的 Xls 資料流讀入 DataTable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="mapFields"></param>
        /// <param name="isBatch"></param>
        /// <param name="isCheckValue"></param>
        /// <param name="headRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool Xls2DataTable(Stream stream, string sheetName, XlsMapField[] mapFields, bool isBatch, bool isCheckValue
            , Int32 headRowIndex
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (stream == null || !stream.CanRead)
            {
                errmsg = "未指定資料流參數或資料流無法讀取";
                return false;
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                errmsg = "未指定 Excel 的 Sheet 名稱";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            #endregion

            #region 檢查對照欄位設定
            {
                List<string> cellNames = new List<string>(mapFields.Length);
                foreach (XlsMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                    else if (cellNames.Contains(mapField.CellName))
                    {
                        errmsg = "對照欄位設定的欄位名稱有重複";
                        return false;
                    }
                    cellNames.Add(mapField.CellName);
                }
            }
            #endregion

            errmsg = null;
            bool isOK = false;
            try
            {
                HSSFWorkbook wb = new HSSFWorkbook(stream);

                #region 檢查 Sheet 名稱
                HSSFSheet sheet = (HSSFSheet)wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    errmsg = String.Format("Sheet 名稱 {0} 不存在", sheetName);
                    return false;
                }
                #endregion

                #region 查 Head 的欄位數量
                HSSFRow headRow = (HSSFRow)sheet.GetRow(headRowIndex);
                int columnCount = (headRow.LastCellNum - 1) - headRow.FirstCellNum + 1;
                if (columnCount < mapFields.Length)
                {
                    errmsg = "Excel 檔的欄位數量與對照欄位設定數量不合";
                    return false;
                }
                #endregion

                #region 收集欄位索引與欄位名稱，並檢查欄位名稱是否有重複
                KeyValueList<string> columnIndexNames = new KeyValueList<string>(columnCount);
                {
                    for (int cellNum = headRow.FirstCellNum; cellNum < columnCount; cellNum++)
                    {
                        HSSFCell xlsCell = (HSSFCell)headRow.GetCell(cellNum);
                        string cellValue = null;
                        if (xlsCell != null)
                        {
                            switch (xlsCell.CellType)
                            {
                                case CellType.Formula:
                                    NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate aggregate = xlsCell.CellValueRecord as NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate;
                                    if (aggregate.StringRecord != null)
                                    {
                                        cellValue = aggregate.StringValue;
                                    }
                                    else
                                    {
                                        cellValue = aggregate.FormulaRecord.Value.ToString();
                                    }
                                    break;
                                case CellType.Numeric:  // HSSFCell.CELL_TYPE_NUMERIC:	//預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
                                    cellValue = xlsCell.ToString();
                                    if (cellValue.StartsWith("_*"))	//會計格式會用這個字串開頭
                                    {
                                        cellValue = cellValue.Substring(2);
                                    }
                                    decimal val;
                                    if (System.Decimal.TryParse(cellValue, System.Globalization.NumberStyles.Float, null, out val))
                                    {
                                        cellValue = val.ToString();
                                    }
                                    else
                                    {
                                        cellValue = xlsCell.NumericCellValue.ToString();
                                    }
                                    break;
                                case CellType.Blank: // HSSFCell.CELL_TYPE_BLANK:
                                    cellValue = String.Empty;
                                    break;
                                default:
                                    cellValue = xlsCell.StringCellValue;
                                    break;
                            }

                            if (cellValue != null)
                            {
                                cellValue = cellValue.Trim();
                            }
                        }
                        if (String.IsNullOrEmpty(cellValue))
                        {
                            continue;
                        }
                        if (columnIndexNames.Find(x => x.Value == cellValue) != null)
                        {
                            errmsg = "Excel 檔的欄位名稱有重複";
                            return false;
                        }
                        columnIndexNames.Add(xlsCell.ColumnIndex.ToString(), cellValue);
                    }
                }
                #endregion

                #region 檢查欄位名稱是否有缺少
                foreach (XlsMapField mapField in mapFields)
                {
                    KeyValue<string> columnIndexName = columnIndexNames.Find(x => x.Value == mapField.CellName);
                    if (columnIndexName == null)
                    {
                        errmsg = String.Format("Excel 檔缺少 {0} 的欄位名稱", mapField.CellName);
                        return false;
                    }
                    //把 Xls 的欄位名稱換成對應的對照欄位 Key
                    columnIndexName.Value = mapField.Key;
                }
                #endregion

                #region 初始化 DataTable (產生資料欄位)
                table = new DataTable();
                {
                    foreach (XlsMapField mapField in mapFields)
                    {
                        table.Columns.Add(new DataColumn(mapField.Key));
                    }
                    //紀錄資料錯誤的欄位
                    table.Columns.Add(new DataColumn(DataLineFailureFieldName));
                }
                #endregion

                #region 逐資料行處理
                {
                    bool isBreak = false;
                    for (int rowIndex = headRowIndex + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        totalCount++;

                        DataRow dataRow = table.NewRow();

                        #region 逐資料欄處理
                        try
                        {
                            HSSFRow xlsRow = (HSSFRow)sheet.GetRow(rowIndex);
                            string valueError = null;

                            #region [Old]
                            //for (int cellNum = xlsRow.FirstCellNum; cellNum < columnCount; cellNum++)
                            //{
                            //    HSSFCell xlsCell = xlsRow.GetCell(cellNum);
                            //    if (xlsCell == null)
                            //    {
                            //        continue;
                            //    }

                            //    #region 找出對應的對照欄位設定
                            //    XlsMapField mapField = null;
                            //    {
                            //        string columnIndex = xlsCell.ColumnIndex.ToString();
                            //        KeyValue<string> columnIndexName = columnIndexNames.Find(x => x.Key == columnIndex);
                            //        if (columnIndexName != null)
                            //        {
                            //            string columnName = columnIndexName.Value;
                            //            if (!String.IsNullOrEmpty(columnName))
                            //            {
                            //                columnName = columnName.Trim();
                            //                mapField = Array.Find<XlsMapField>(mapFields, x => x.Key == columnName);
                            //            }
                            //        }
                            //    }
                            //    if (mapField == null)
                            //    {
                            //        continue;
                            //    }
                            //    #endregion

                            //    #region 取 Cell 資料
                            //    string cellValue = null;
                            //    switch (xlsCell.CellType)
                            //    {
                            //        case HSSFCell.CELL_TYPE_FORMULA:
                            //            NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate aggregate = xlsCell.CellValueRecord as NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate;
                            //            if (aggregate.StringRecord != null)
                            //            {
                            //                cellValue = aggregate.StringValue;
                            //            }
                            //            else
                            //            {
                            //                cellValue = aggregate.FormulaRecord.Value.ToString();
                            //            }
                            //            break;
                            //        case HSSFCell.CELL_TYPE_NUMERIC:	//預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
                            //            switch (xlsCell.CellStyle.DataFormat)
                            //            {
                            //                case 0:     //"General"
                            //                    #region G/通用格式 (一律以數值處理)
                            //                    {
                            //                        cellValue = xlsCell.ToString();
                            //                        if (!Common.IsNumber(cellValue))
                            //                        {
                            //                            cellValue = xlsCell.NumericCellValue.ToString();
                            //                        }
                            //                    }
                            //                    break;
                            //                    #endregion

                            //                case 1:     //"0"
                            //                case 2:     //"0.00"
                            //                case 3:     //"#,##0"
                            //                case 4:     //"#,##0.00"
                            //                case 5:     //"($#,##0_);($#,##0)"
                            //                case 6:     //"($#,##0_);[Red]($#,##0)"
                            //                case 7:     //"($#,##0.00);($#,##0.00)"
                            //                case 8:     //"($#,##0.00_);[Red]($#,##0.00)"
                            //                case 9:     //"0%"
                            //                case 10:    //"0.00%"
                            //                case 11:    //"0.00E+00"
                            //                case 12:    //"# ?/?"
                            //                case 13:    //"# ??/??"
                            //                case 37:    //"(#,##0_);(#,##0)"
                            //                case 38:    //"(#,##0_);[Red](#,##0)"
                            //                case 39:    //"(#,##0.00_);(#,##0.00)"
                            //                case 40:    //"(#,##0.00_);[Red](#,##0.00)"
                            //                case 41:    //"_(*#,##0_);_(*(#,##0);_(* \"-\"_);_(@_)"
                            //                case 42:    //"_($*#,##0_);_($*(#,##0);_($* \"-\"_);_(@_)"
                            //                case 43:    //"_(*#,##0.00_);_(*(#,##0.00);_(*\"-\"??_);_(@_)"
                            //                case 44:    //"_($*#,##0.00_);_($*(#,##0.00);_($*\"-\"??_);_(@_)"
                            //                case 48:    //"##0.0E+0"
                            //                case 176:   //"mmdd" ??
                            //                    #region 數值
                            //                    {
                            //                        cellValue = xlsCell.ToString();
                            //                        if (!Common.IsNumber(cellValue))
                            //                        {
                            //                            cellValue = xlsCell.NumericCellValue.ToString();
                            //                        }
                            //                    }
                            //                    break;
                            //                    #endregion

                            //                case 14:    //"m/d/yy"
                            //                case 15:    //"d-mmm-yy"
                            //                case 16:    //"d-mmm"
                            //                case 17:    //"mmm-yy"
                            //                case 18:    //"h:mm AM/PM"
                            //                case 19:    //"h:mm:ss AM/PM"
                            //                case 20:    //"h:mm"
                            //                case 21:    //"h:mm:ss"
                            //                case 22:    //"m/d/yy h:mm"
                            //                case 45:    //"mm:ss"
                            //                case 46:    //"[h]:mm:ss"
                            //                case 47:    //"mm:ss.0"
                            //                    #region 日期
                            //                    {
                            //                        cellValue = xlsCell.DateCellValue.ToString("yyyy/MM/dd");
                            //                    }
                            //                    #endregion
                            //                    break;

                            //                case 23:    //"0x17"
                            //                case 24:    //"0x18"
                            //                case 25:    //"0x19"
                            //                case 26:    //"0x1a"
                            //                case 27:    //"0x1b"
                            //                case 28:    //"0x1c"
                            //                case 29:    //"0x1d"
                            //                case 30:    //"0x1e"
                            //                case 31:    //"0x1f"
                            //                case 32:    //"0x20"
                            //                case 33:    //"0x21"
                            //                case 34:    //"0x22"
                            //                case 35:    //"0x23"
                            //                case 36:    //"0x24"
                            //                    #region 無法處理的格式
                            //                    {
                            //                        cellValue = xlsCell.ToString();
                            //                        valueError = String.Format("{0} 欄位的格式不支援，(DataFormat={1}, GetDataFormatString={2})", mapField.CellName, xlsCell.CellStyle.DataFormat, xlsCell.CellStyle.GetDataFormatString());
                            //                        dataRow[DataLineFailureFieldName] = valueError;
                            //                    }
                            //                    #endregion
                            //                    break;

                            //                case 49:    //"@" (文字)
                            //                    #region 文字 (一律以文字處理)
                            //                    {
                            //                        cellValue = xlsCell.ToString();
                            //                    }
                            //                    #endregion
                            //                    break;

                            //                default:
                            //                    #region 其他 (一律以文字處理)
                            //                    {
                            //                        cellValue = xlsCell.ToString();

                            //                        //if (cellValue.StartsWith("_*"))	//會計格式會用這個字串開頭
                            //                        //{
                            //                        //    cellValue = cellValue.Substring(2);
                            //                        //}
                            //                        //decimal val;
                            //                        //if (Decimal.TryParse(cellValue, System.Globalization.NumberStyles.Float, null, out val))
                            //                        //{
                            //                        //    cellValue = val.ToString();
                            //                        //}
                            //                        //else
                            //                        //{
                            //                        //    cellValue = xlsCell.NumericCellValue.ToString();
                            //                        //}
                            //                    }
                            //                    #endregion
                            //                    break;
                            //            }
                            //            break;
                            //        case HSSFCell.CELL_TYPE_BLANK:
                            //            cellValue = String.Empty;
                            //            break;
                            //        default:
                            //            cellValue = xlsCell.StringCellValue;
                            //            break;
                            //    }
                            //    #endregion

                            //    dataRow[mapField.Key] = cellValue;

                            //    if (isCheckValue && valueError == null)
                            //    {
                            //        if (!mapField.Check(cellValue, out valueError))
                            //        {
                            //            valueError = String.Format("{0} 欄位的值不正確，{1}", mapField.CellName, valueError);
                            //            dataRow[DataLineFailureFieldName] = valueError;
                            //        }
                            //    }
                            //}
                            #endregion

                            #region [New]
                            foreach (KeyValue<string> columnIndexName in columnIndexNames)
                            {
                                int columnIndex = int.Parse(columnIndexName.Key);
                                string columnName = columnIndexName.Value;
                                if (String.IsNullOrWhiteSpace(columnName))
                                {
                                    continue;
                                }
                                columnName = columnName.Trim();
                                XlsMapField mapField = System.Array.Find<XlsMapField>(mapFields, x => x.Key == columnName);
                                if (mapField == null)
                                {
                                    continue;
                                }

                                HSSFCell xlsCell = null;
                                if (xlsRow.FirstCellNum <= columnIndex)
                                {
                                    xlsCell = (HSSFCell) xlsRow.GetCell(columnIndex);
                                }

                                #region 取 Cell 資料
                                string cellValue = null;
                                if (xlsCell != null)
                                {
                                    switch (xlsCell.CellType)
                                    {
                                        case CellType.Formula:
                                            NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate aggregate = xlsCell.CellValueRecord as NPOI.HSSF.Record.Aggregates.FormulaRecordAggregate;
                                            if (aggregate.StringRecord != null)
                                            {
                                                cellValue = aggregate.StringValue;
                                            }
                                            else
                                            {
                                                cellValue = aggregate.FormulaRecord.Value.ToString();
                                            }
                                            break;
                                        case CellType.Numeric:	//預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
                                            switch (xlsCell.CellStyle.DataFormat)
                                            {
                                                case 0:     //"General"
                                                    #region G/通用格式 (一律以數值處理)
                                                    {
                                                        cellValue = xlsCell.ToString();
                                                        if (!Common.IsNumber(cellValue))
                                                        {
                                                            cellValue = xlsCell.NumericCellValue.ToString();
                                                        }
                                                    }
                                                    break;
                                                    #endregion

                                                case 1:     //"0"
                                                case 2:     //"0.00"
                                                case 3:     //"#,##0"
                                                case 4:     //"#,##0.00"
                                                case 5:     //"($#,##0_);($#,##0)"
                                                case 6:     //"($#,##0_);[Red]($#,##0)"
                                                case 7:     //"($#,##0.00);($#,##0.00)"
                                                case 8:     //"($#,##0.00_);[Red]($#,##0.00)"
                                                case 9:     //"0%"
                                                case 10:    //"0.00%"
                                                case 11:    //"0.00E+00"
                                                case 12:    //"# ?/?"
                                                case 13:    //"# ??/??"
                                                case 37:    //"(#,##0_);(#,##0)"
                                                case 38:    //"(#,##0_);[Red](#,##0)"
                                                case 39:    //"(#,##0.00_);(#,##0.00)"
                                                case 40:    //"(#,##0.00_);[Red](#,##0.00)"
                                                case 41:    //"_(*#,##0_);_(*(#,##0);_(* \"-\"_);_(@_)"
                                                case 42:    //"_($*#,##0_);_($*(#,##0);_($* \"-\"_);_(@_)"
                                                case 43:    //"_(*#,##0.00_);_(*(#,##0.00);_(*\"-\"??_);_(@_)"
                                                case 44:    //"_($*#,##0.00_);_($*(#,##0.00);_($*\"-\"??_);_(@_)"
                                                case 48:    //"##0.0E+0"
                                                case 176:   //"mmdd" ??
                                                    #region 數值
                                                    {
                                                        cellValue = xlsCell.ToString();
                                                        if (!Common.IsNumber(cellValue))
                                                        {
                                                            cellValue = xlsCell.NumericCellValue.ToString();
                                                        }
                                                    }
                                                    break;
                                                    #endregion

                                                case 14:    //"m/d/yy"
                                                case 15:    //"d-mmm-yy"
                                                case 16:    //"d-mmm"
                                                case 17:    //"mmm-yy"
                                                case 18:    //"h:mm AM/PM"
                                                case 19:    //"h:mm:ss AM/PM"
                                                case 20:    //"h:mm"
                                                case 21:    //"h:mm:ss"
                                                case 22:    //"m/d/yy h:mm"
                                                case 45:    //"mm:ss"
                                                case 46:    //"[h]:mm:ss"
                                                case 47:    //"mm:ss.0"
                                                    #region 日期
                                                    {
                                                        cellValue = xlsCell.DateCellValue.ToString("yyyy/MM/dd");
                                                    }
                                                    #endregion
                                                    break;

                                                case 23:    //"0x17"
                                                case 24:    //"0x18"
                                                case 25:    //"0x19"
                                                case 26:    //"0x1a"
                                                case 27:    //"0x1b"
                                                case 28:    //"0x1c"
                                                case 29:    //"0x1d"
                                                case 30:    //"0x1e"
                                                case 31:    //"0x1f"
                                                case 32:    //"0x20"
                                                case 33:    //"0x21"
                                                case 34:    //"0x22"
                                                case 35:    //"0x23"
                                                case 36:    //"0x24"
                                                    #region 無法處理的格式
                                                    {
                                                        cellValue = xlsCell.ToString();
                                                        valueError = String.Format("{0} 欄位的格式不支援，(DataFormat={1}, GetDataFormatString={2})", mapField.CellName, xlsCell.CellStyle.DataFormat, xlsCell.CellStyle.GetDataFormatString());
                                                        dataRow[DataLineFailureFieldName] = valueError;
                                                    }
                                                    #endregion
                                                    break;

                                                case 49:    //"@" (文字)
                                                    #region 文字 (一律以文字處理)
                                                    {
                                                        cellValue = xlsCell.ToString();
                                                    }
                                                    #endregion
                                                    break;

                                                default:
                                                    #region 其他 (一律以文字處理)
                                                    {
                                                        cellValue = xlsCell.ToString();

                                                        //if (cellValue.StartsWith("_*"))	//會計格式會用這個字串開頭
                                                        //{
                                                        //    cellValue = cellValue.Substring(2);
                                                        //}
                                                        //decimal val;
                                                        //if (Decimal.TryParse(cellValue, System.Globalization.NumberStyles.Float, null, out val))
                                                        //{
                                                        //    cellValue = val.ToString();
                                                        //}
                                                        //else
                                                        //{
                                                        //    cellValue = xlsCell.NumericCellValue.ToString();
                                                        //}
                                                    }
                                                    #endregion
                                                    break;
                                            }
                                            break;
                                        case CellType.Unknown:   //.HSSFCell.CELL_TYPE_BLANK:
                                            cellValue = String.Empty;
                                            break;
                                        default:
                                            cellValue = xlsCell.StringCellValue;
                                            break;
                                    }
                                }
                                #endregion

                                dataRow[mapField.Key] = cellValue;

                                if (isCheckValue && valueError == null)
                                {
                                    if (!mapField.Check(cellValue, out valueError))
                                    {
                                        valueError = String.Format("{0} 欄位的值不正確，{1}", mapField.CellName, valueError);
                                        dataRow[DataLineFailureFieldName] = valueError;
                                    }
                                }
                            }
                            #endregion

                            if (String.IsNullOrEmpty(valueError))
                            {
                                dataRow[DataLineFailureFieldName] = String.Empty;
                                successCount++;
                            }
                            else if (isBatch)
                            {
                                isBreak = true;
                                errmsg = String.Format("第{0}行的資料不正確，錯誤訊息：{1}", totalCount, valueError);
                            }
                        }
                        catch (Exception ex)
                        {
                            dataRow[DataLineFailureFieldName] = String.Format("處理資料行發生例外，錯誤訊息：{0}", ex.Message);
                            if (isBatch)
                            {
                                isBreak = true;
                                errmsg = String.Format("第{0}行的資料處理發生例外，錯誤訊息：{1}", rowIndex + 1, ex.Message);
                            }
                        }
                        #endregion

                        table.Rows.Add(dataRow);

                        if (isBreak)
                        {
                            break;
                        }
                    }

                    if (isBatch)
                    {
                        //指定整批處理時，如被中斷表示失敗
                        isOK = !isBreak;
                    }
                    else
                    {
                        //非整批處理時，任一筆成功就算成功
                        isOK = (successCount > 0);
                        if (successCount > 0)
                        {
                            isOK = true;
                        }
                        else
                        {
                            errmsg = "無任何一筆有效的資料被匯入";
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理資料發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;
        }
        #endregion

        #region Xlsx2DataTable
        /// <summary>
        /// 將指定的 Xls 檔讀入 DataTable
        /// </summary>
        /// <param name="xlsxFile"></param>
        /// <param name="sheetName"></param>
        /// <param name="encoding"></param>
        /// <param name="mapFields"></param>
        /// <param name="isBatch"></param>
        /// <param name="isCheckValue"></param>
        /// <param name="headRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool Xlsx2DataTable(string xlsxFile, string sheetName, Encoding encoding, XlsMapField[] mapFields, bool isBatch, bool isCheckValue
            , Int32 headRowIndex
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (String.IsNullOrEmpty(xlsxFile) || !File.Exists(xlsxFile))
            {
                errmsg = "未指定檔案參數或檔案不存在";
                return false;
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                errmsg = "未指定 Excel 的 Sheet 名稱";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            #endregion

            #region 檢查對照欄位設定
            {
                foreach (XlsMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                }
            }
            #endregion

            bool isOK = false;
            try
            {
                using (FileStream fs = new FileStream(xlsxFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    isOK = this.Xlsx2DataTable(fs, sheetName, mapFields, isBatch, isCheckValue, headRowIndex
                        , out table, out totalCount, out successCount, out errmsg);
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理檔案發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;
        }

        /// <summary>
        /// 將指定的 Xls 資料流讀入 DataTable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="mapFields"></param>
        /// <param name="isBatch"></param>
        /// <param name="isCheckValue"></param>
        /// <param name="headRowIndex"></param>
        /// <param name="table"></param>
        /// <param name="totalCount"></param>
        /// <param name="successCount"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool Xlsx2DataTable(Stream stream, string sheetName, XlsMapField[] mapFields, bool isBatch, bool isCheckValue
            , Int32 headRowIndex
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (stream == null || !stream.CanRead)
            {
                errmsg = "未指定資料流參數或資料流無法讀取";
                return false;
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                errmsg = "未指定 Excel 的 Sheet 名稱";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            #endregion

            #region 檢查對照欄位設定
            {
                List<string> cellNames = new List<string>(mapFields.Length);
                foreach (XlsMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                    else if (cellNames.Contains(mapField.CellName))
                    {
                        errmsg = "對照欄位設定的欄位名稱有重複";
                        return false;
                    }
                    cellNames.Add(mapField.CellName);
                }
            }
            #endregion

            errmsg = null;
            bool isOK = false;
            try
            {
                XSSFWorkbook wb = new XSSFWorkbook(stream);
                XSSFFormulaEvaluator formulaHelper = null;

                #region 檢查 Sheet 名稱
                XSSFSheet sheet = (XSSFSheet)wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    errmsg = String.Format("Sheet 名稱 {0} 不存在", sheetName);
                    return false;
                }
                #endregion

                #region 查 Head 的欄位數量
                XSSFRow headRow = (XSSFRow)sheet.GetRow(headRowIndex);
                int columnCount = (headRow.LastCellNum - 1) - headRow.FirstCellNum + 1;
                if (columnCount < mapFields.Length)
                {
                    errmsg = "Excel 檔的欄位數量與對照欄位設定數量不合";
                    return false;
                }
                #endregion

                #region 收集欄位索引與欄位名稱，並檢查欄位名稱是否有重複
                KeyValueList<string> columnIndexNames = new KeyValueList<string>(columnCount);
                {
                    for (int cellNum = headRow.FirstCellNum; cellNum < columnCount; cellNum++)
                    {
                        XSSFCell xlsxCell = (XSSFCell)headRow.GetCell(cellNum);
                        string cellValue = null;
                        if (xlsxCell != null)
                        {
                            switch (xlsxCell.CellType)
                            {
                                case CellType.Formula:
                                    #region Formula
                                    {
                                        if (formulaHelper == null)
                                        {
                                            formulaHelper = (XSSFFormulaEvaluator)wb.GetCreationHelper().CreateFormulaEvaluator();
                                        }
                                        formulaHelper.EvaluateFormulaCell(xlsxCell);
                                        cellValue = xlsxCell.ToString();
                                    }
                                    #endregion
                                    break;
                                case CellType.Numeric:  // 預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
                                    #region Numeric
                                    {
                                        cellValue = xlsxCell.ToString();
                                        if (cellValue.StartsWith("_*"))	//會計格式會用這個字串開頭
                                        {
                                            cellValue = cellValue.Substring(2);
                                        }
                                        decimal val;
                                        if (System.Decimal.TryParse(cellValue, System.Globalization.NumberStyles.Float, null, out val))
                                        {
                                            cellValue = val.ToString();
                                        }
                                        else
                                        {
                                            cellValue = xlsxCell.NumericCellValue.ToString();
                                        }
                                    }
                                    #endregion
                                    break;
                                case CellType.Blank:
                                    cellValue = String.Empty;
                                    break;
                                default:
                                    cellValue = xlsxCell.StringCellValue;
                                    break;
                            }

                            if (cellValue != null)
                            {
                                cellValue = cellValue.Trim();
                            }
                        }
                        if (String.IsNullOrEmpty(cellValue))
                        {
                            continue;
                        }
                        if (columnIndexNames.Find(x => x.Value == cellValue) != null)
                        {
                            errmsg = "Excel 檔的欄位名稱有重複";
                            return false;
                        }
                        columnIndexNames.Add(xlsxCell.ColumnIndex.ToString(), cellValue);
                    }
                }
                #endregion

                #region 檢查欄位名稱是否有缺少
                foreach (XlsMapField mapField in mapFields)
                {
                    KeyValue<string> columnIndexName = columnIndexNames.Find(x => x.Value == mapField.CellName);
                    if (columnIndexName == null)
                    {
                        errmsg = String.Format("Excel 檔缺少 {0} 的欄位名稱", mapField.CellName);
                        return false;
                    }
                    //把 Xls 的欄位名稱換成對應的對照欄位 Key
                    columnIndexName.Value = mapField.Key;
                }
                #endregion

                #region 初始化 DataTable (產生資料欄位)
                table = new DataTable();
                {
                    foreach (XlsMapField mapField in mapFields)
                    {
                        table.Columns.Add(new DataColumn(mapField.Key));
                    }
                    //紀錄資料錯誤的欄位
                    table.Columns.Add(new DataColumn(DataLineFailureFieldName));
                }
                #endregion

                #region 逐資料行處理
                {
                    bool isBreak = false;
                    for (int rowIndex = headRowIndex + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        totalCount++;

                        DataRow dataRow = table.NewRow();

                        #region 逐資料欄處理
                        try
                        {
                            XSSFRow xlsRow = (XSSFRow)sheet.GetRow(rowIndex);
                            string valueError = null;

                            foreach (KeyValue<string> columnIndexName in columnIndexNames)
                            {
                                int columnIndex = int.Parse(columnIndexName.Key);

                                #region 找出對照欄位
                                XlsMapField mapField = null;
                                {
                                    string columnName = columnIndexName.Value;
                                    if (String.IsNullOrWhiteSpace(columnName))
                                    {
                                        continue;
                                    }
                                    columnName = columnName.Trim();
                                    mapField = System.Array.Find<XlsMapField>(mapFields, x => x.Key == columnName);
                                    if (mapField == null)
                                    {
                                        continue;
                                    }
                                }
                                #endregion

                                #region 找出 Cell
                                XSSFCell xlsxCell = null;
                                if (xlsRow.FirstCellNum <= columnIndex)
                                {
                                    xlsxCell = (XSSFCell)xlsRow.GetCell(columnIndex);
                                }
                                #endregion

                                #region 取 Cell 資料
                                string cellValue = null;
                                if (xlsxCell != null)
                                {
                                    switch (xlsxCell.CellType)
                                    {
                                        case CellType.Formula:
                                            #region Formula
                                            {
                                                if (formulaHelper == null)
                                                {
                                                    formulaHelper = (XSSFFormulaEvaluator)wb.GetCreationHelper().CreateFormulaEvaluator();
                                                }

                                                #region [MDY:20170818] M201708_01 修正讀取公式的欄位值 (20170731_01)
                                                #region [Old]
                                                //formulaHelper.EvaluateFormulaCell(xlsxCell);
                                                //cellValue = xlsxCell.ToString();
                                                #endregion

                                                //CellValue formulaCellValue = formulaHelper.Evaluate(xlsxCell);
                                                CellType cellType = formulaHelper.EvaluateFormulaCell(xlsxCell);
                                                switch (cellType)
                                                {
                                                    case CellType.Numeric:	//預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
                                                        #region Numeric
                                                        {
                                                            if (DateUtil.IsCellDateFormatted(xlsxCell))
                                                            {
                                                                cellValue = xlsxCell.DateCellValue.ToString("yyyy/MM/dd");
                                                            }
                                                            else
                                                            {
                                                                cellValue = xlsxCell.GetRawValue();
                                                                if (!Common.IsNumber(cellValue))
                                                                {
                                                                    cellValue = xlsxCell.NumericCellValue.ToString();
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                        break;
                                                    case CellType.Blank:
                                                    case CellType.Unknown:
                                                        cellValue = String.Empty;
                                                        break;
                                                    default:
                                                        cellValue = xlsxCell.GetRawValue();
                                                        break;
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            break;
                                        case CellType.Numeric:	//預設取 cell.ToString()，如果不能轉成 Decimal 則改取 NumericCellValue 值
                                            #region Numeric
                                            {
                                                switch (xlsxCell.CellStyle.DataFormat)
                                                {
                                                    case 0:     //"General"
                                                        #region G/通用格式 (一律以數值處理)
                                                        {
                                                            cellValue = xlsxCell.ToString();
                                                            if (!Common.IsNumber(cellValue))
                                                            {
                                                                cellValue = xlsxCell.NumericCellValue.ToString();
                                                            }
                                                        }
                                                        break;
                                                        #endregion

                                                    case 1:     //"0"
                                                    case 2:     //"0.00"
                                                    case 3:     //"#,##0"
                                                    case 4:     //"#,##0.00"
                                                    case 5:     //"($#,##0_);($#,##0)"
                                                    case 6:     //"($#,##0_);[Red]($#,##0)"
                                                    case 7:     //"($#,##0.00);($#,##0.00)"
                                                    case 8:     //"($#,##0.00_);[Red]($#,##0.00)"
                                                    case 9:     //"0%"
                                                    case 10:    //"0.00%"
                                                    case 11:    //"0.00E+00"
                                                    case 12:    //"# ?/?"
                                                    case 13:    //"# ??/??"
                                                    case 37:    //"(#,##0_);(#,##0)"
                                                    case 38:    //"(#,##0_);[Red](#,##0)"
                                                    case 39:    //"(#,##0.00_);(#,##0.00)"
                                                    case 40:    //"(#,##0.00_);[Red](#,##0.00)"
                                                    case 41:    //"_(*#,##0_);_(*(#,##0);_(* \"-\"_);_(@_)"
                                                    case 42:    //"_($*#,##0_);_($*(#,##0);_($* \"-\"_);_(@_)"
                                                    case 43:    //"_(*#,##0.00_);_(*(#,##0.00);_(*\"-\"??_);_(@_)"
                                                    case 44:    //"_($*#,##0.00_);_($*(#,##0.00);_($*\"-\"??_);_(@_)"
                                                    case 48:    //"##0.0E+0"
                                                    case 176:   //"mmdd" ??
                                                        #region 數值
                                                        {
                                                            cellValue = xlsxCell.ToString();
                                                            if (!Common.IsNumber(cellValue))
                                                            {
                                                                cellValue = xlsxCell.NumericCellValue.ToString();
                                                            }
                                                        }
                                                        break;
                                                        #endregion

                                                    case 14:    //"m/d/yy"
                                                    case 15:    //"d-mmm-yy"
                                                    case 16:    //"d-mmm"
                                                    case 17:    //"mmm-yy"
                                                    case 18:    //"h:mm AM/PM"
                                                    case 19:    //"h:mm:ss AM/PM"
                                                    case 20:    //"h:mm"
                                                    case 21:    //"h:mm:ss"
                                                    case 22:    //"m/d/yy h:mm"
                                                    case 45:    //"mm:ss"
                                                    case 46:    //"[h]:mm:ss"
                                                    case 47:    //"mm:ss.0"
                                                        #region 日期
                                                        {
                                                            cellValue = xlsxCell.DateCellValue.ToString("yyyy/MM/dd");
                                                        }
                                                        #endregion
                                                        break;

                                                    case 23:    //"0x17"
                                                    case 24:    //"0x18"
                                                    case 25:    //"0x19"
                                                    case 26:    //"0x1a"
                                                    case 27:    //"0x1b"
                                                    case 28:    //"0x1c"
                                                    case 29:    //"0x1d"
                                                    case 30:    //"0x1e"
                                                    case 31:    //"0x1f"
                                                    case 32:    //"0x20"
                                                    case 33:    //"0x21"
                                                    case 34:    //"0x22"
                                                    case 35:    //"0x23"
                                                    case 36:    //"0x24"
                                                        #region 無法處理的格式
                                                        {
                                                            cellValue = xlsxCell.ToString();
                                                            valueError = String.Format("{0} 欄位的格式不支援，(DataFormat={1}, GetDataFormatString={2})", mapField.CellName, xlsxCell.CellStyle.DataFormat, xlsxCell.CellStyle.GetDataFormatString());
                                                            dataRow[DataLineFailureFieldName] = valueError;
                                                        }
                                                        #endregion
                                                        break;

                                                    case 49:    //"@" (文字)
                                                        #region 文字 (一律以文字處理)
                                                        {
                                                            cellValue = xlsxCell.ToString();
                                                        }
                                                        #endregion
                                                        break;

                                                    default:
                                                        #region 其他 (一律以文字處理)
                                                        {
                                                            cellValue = xlsxCell.ToString();
                                                        }
                                                        #endregion
                                                        break;
                                                }
                                            }
                                            #endregion
                                            break;
                                        case CellType.Blank:
                                        case CellType.Unknown:
                                            cellValue = String.Empty;
                                            break;
                                        default:
                                            cellValue = xlsxCell.StringCellValue;
                                            break;
                                    }
                                }
                                #endregion

                                dataRow[mapField.Key] = cellValue;

                                if (isCheckValue && valueError == null)
                                {
                                    if (!mapField.Check(cellValue, out valueError))
                                    {
                                        valueError = String.Format("{0} 欄位的值不正確，{1}", mapField.CellName, valueError);
                                        dataRow[DataLineFailureFieldName] = valueError;
                                    }
                                }
                            }

                            if (String.IsNullOrEmpty(valueError))
                            {
                                dataRow[DataLineFailureFieldName] = String.Empty;
                                successCount++;
                            }
                            else if (isBatch)
                            {
                                isBreak = true;
                                errmsg = String.Format("第{0}行的資料不正確，錯誤訊息：{1}", totalCount, valueError);
                            }
                        }
                        catch (Exception ex)
                        {
                            dataRow[DataLineFailureFieldName] = String.Format("處理資料行發生例外，錯誤訊息：{0}", ex.Message);
                            if (isBatch)
                            {
                                isBreak = true;
                                errmsg = String.Format("第{0}行的資料處理發生例外，錯誤訊息：{1}", rowIndex + 1, ex.Message);
                            }
                        }
                        #endregion

                        table.Rows.Add(dataRow);

                        if (isBreak)
                        {
                            break;
                        }
                    }

                    if (isBatch)
                    {
                        //指定整批處理時，如被中斷表示失敗
                        isOK = !isBreak;
                    }
                    else
                    {
                        //非整批處理時，任一筆成功就算成功
                        isOK = (successCount > 0);
                        if (successCount > 0)
                        {
                            isOK = true;
                        }
                        else
                        {
                            errmsg = "無任何一筆有效的資料被匯入";
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理資料發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;
        }
        #endregion

        #region [MDY:20190906] (2019擴充案) Ods2DataTable (匯入檔增加 ODS 格式)
        public bool Ods2DataTable(Stream stream, string sheetName, XlsMapField[] mapFields, bool isBatch, bool isCheckValue
            , Int32 headRowIndex
            , out DataTable table, out Int32 totalCount, out Int32 successCount, out string errmsg)
        {
            table = null;
            totalCount = 0;
            successCount = 0;

            #region 檢查參數
            if (stream == null || !stream.CanRead)
            {
                errmsg = "未指定資料流參數或資料流無法讀取";
                return false;
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                errmsg = "未指定 Calc 的 Sheet 名稱";
                return false;
            }
            if (mapFields == null || mapFields.Length == 0)
            {
                errmsg = "未指定對照欄位設定參數";
                return false;
            }
            #endregion

            #region 檢查對照欄位設定
            {
                List<string> cellNames = new List<string>(mapFields.Length);

                foreach (XlsMapField mapField in mapFields)
                {
                    if (!mapField.IsReady())
                    {
                        errmsg = "對照欄位設定不正確";
                        return false;
                    }
                    else if (cellNames.Contains(mapField.CellName))
                    {
                        errmsg = "對照欄位設定的欄位名稱有重複";
                        return false;
                    }
                    cellNames.Add(mapField.CellName);
                }
            }
            #endregion

            errmsg = null;
            bool isOK = false;
            try
            {
                #region 查 Head 的欄位數量
                // reader.ReadToDataTable() 會檢查，這裡不用處理
                #endregion

                #region 收集欄位索引與欄位名稱，並檢查欄位名稱是否有重複
                // reader.ReadToDataTable() 自行處理欄位對照，這裡不用處理
                #endregion

                #region 檢查欄位名稱是否有缺少
                // reader.ReadToDataTable() 會檢查，這裡不用處理
                #endregion

                #region 將對照欄位設定 XlsMapField 轉成 Fuju.ODS.ODSMappingColumnList
                Fuju.ODS.ODSResultColumn odsResultColumn = new Fuju.ODS.ODSResultColumn();
                Fuju.ODS.ODSMappingColumnList mappingColumns = new Fuju.ODS.ODSMappingColumnList(mapFields.Length + 1);
                {
                    mappingColumns.HeadRowNo = headRowIndex + 1;
                    foreach (XlsMapField mapField in mapFields)
                    {
                        mappingColumns.Add(mapField.GetMappingColumn(isCheckValue));
                    }
                    mappingColumns.Add(odsResultColumn);
                }
                #endregion

                #region 讀取內容並轉成 DataTable
                try
                {
                    Fuju.ODS.ODSReader reader = new Fuju.ODS.ODSReader(stream);

                    #region 檢查 Sheet 名稱
                    if (!reader.IsExistSheetName(sheetName))
                    {
                        errmsg = String.Format("Sheet 名稱 {0} 不存在", sheetName);
                        return false;
                    }
                    #endregion

                    errmsg = reader.ReadToDataTable(sheetName, out table, mappingColumns: mappingColumns, isBatch: isBatch, rowEmptyWay: Fuju.ODS.RowEmptyWay.Skip);

                    #region 取得總筆數與計算成功筆數
                    totalCount = table.Rows.Count;
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.IsNull(odsResultColumn.ColumnName) || String.IsNullOrEmpty(row[odsResultColumn.ColumnName].ToString()))
                        {
                            successCount++;
                        }
                    }

                    //任一筆成功就算成功
                    if (successCount > 0)
                    {
                        isOK = true;
                    }
                    else
                    {
                        errmsg = "無任何一筆有效的資料被匯入";
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    isOK = false;
                    errmsg = String.Format("讀取 ODS 內容發生例外，錯誤訊息：{0}", ex.Message);
                }
                finally
                {
                    #region 把處理結果的蘭為名稱換回 ConvertFileHelper.DataLineFailureFieldName，其他處理方法統一名稱
                    if (table != null)
                    {
                        DataColumn column = table.Columns[odsResultColumn.ColumnName];
                        if (column != null)
                        {
                            column.ColumnName = ConvertFileHelper.DataLineFailureFieldName;
                        }
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理資料發生例外，錯誤訊息：{0}", ex.Message);
            }

            return isOK;
        }
        #endregion

        #region Csv2Xls
        /// <summary>
        /// 將指定的 Csv 檔轉成 Xls
        /// </summary>
        /// <param name="csvFileName">csv 來源檔的路徑檔名</param>
        /// <param name="xlsFileName">xls 目的檔的路徑檔名</param>
        /// <param name="xlsSheetName">xls 目的檔的 SheetName</param>
        /// <param name="csvCoding">csv 來源檔的編碼</param>
        /// <param name="csvCommaCode">csv 檔欄位中逗號替代碼</param>
        /// <param name="skipRowFail">指定當資料列處理失敗是否略過，傳入 true 表示略過該資料列，否則結束處理</param>
        /// <param name="errmsg">傳回錯誤訊息</param>
        /// <param name="logmsg">傳回處理日誌</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public bool Csv2Xls(string csvFileName, string xlsFileName, string xlsSheetName, Encoding csvCoding, string csvCommaCode, bool skipRowFail, out string errmsg, out string logmsg)
        {
            logmsg = null;

            #region 檢查參數
            if (String.IsNullOrEmpty(csvFileName))
            {
                errmsg = "未指定 csv 來源檔的路徑檔名";
                return false;
            }
            if (String.IsNullOrEmpty(xlsFileName))
            {
                errmsg = "未指定 xls 目的檔的路徑檔名";
                return false;
            }
            if (String.IsNullOrEmpty(xlsSheetName))
            {
                xlsSheetName = "sheet1";
            }
            if (csvCoding == null)
            {
                csvCoding = Encoding.Default;
            }

            FileInfo csvFInfo = null;
            FileInfo xlsFInfo = null;
            try
            {
                csvFInfo = new FileInfo(csvFileName);
                if (!csvFInfo.Exists)
                {
                    errmsg = "csv 來源檔不存在";
                    return false;
                }
                csvFileName = csvFInfo.FullName;

                xlsFInfo = new FileInfo(xlsFileName);
                if (xlsFInfo.Exists)
                {
                    try
                    {
                        xlsFInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        errmsg = "xls 目的檔已存在且無法刪除，錯誤訊息：" + ex.Message;
                        return false;
                    }
                }
                xlsFileName = xlsFInfo.FullName;
            }
            catch (Exception ex)
            {
                errmsg = "csv 來源檔 或 xls 目的檔不合法，錯誤訊息：" + ex.Message;
                return false;
            }
            #endregion

            errmsg = null;

            StringBuilder log = new StringBuilder();
            try
            {
                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(xlsSheetName);
                HSSFRow row = null; ;
                HSSFCell cell = null;

                using (StreamReader sr = new StreamReader(csvFileName, csvCoding))
                {
                    string[] columns = null;
                    int rowIdx = 0;
                    bool isBreak = false;
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        columns = line.Split(',');
                        try
                        {
                            row = (HSSFRow)sheet.CreateRow(rowIdx);
                            for (int idx = 0; idx < columns.Length; idx++)
                            {
                                cell = (HSSFCell)row.CreateCell(idx, CellType.String);
                                cell = (HSSFCell)row.CreateCell(idx, CellType.String);  //HSSFCell.CELL_TYPE_STRING);
                                if (String.IsNullOrEmpty(csvCommaCode))
                                {
                                    cell.SetCellValue(columns[idx].Replace(csvCommaCode, ","));
                                }
                                else
                                {
                                    cell.SetCellValue(columns[idx]);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            int rowNo = rowIdx + 1;
                            log.AppendFormat("處理第 {0} 列資料失敗，錯誤訊息：[{1}] {2}", rowNo, ex.Source, ex.Message).AppendLine();
                            if (!skipRowFail)
                            {
                                errmsg = String.Format("處理第 {0} 列資料失敗，所以中斷處理，錯誤訊息：{1}", rowNo, ex.Message);
                                isBreak = true;
                            }
                        }

                        if (isBreak)
                        {
                            break;
                        }
                        rowIdx++;
                    }
                }

                using (FileStream fs = new FileStream(xlsFileName, FileMode.CreateNew, FileAccess.Write))
                {
                    wb.Write(fs);
                }
                wb = null;
            }
            catch (Exception ex)
            {
                errmsg = String.Format("處理資料發生錯誤，錯誤訊息：{0}", ex.Message);
                return false;
            }

            logmsg = log.ToString();
            return String.IsNullOrEmpty(errmsg);
        }
        #endregion

        #region XlsMapField2XlsSample
        /// <summary>
        /// 取得 Xls 對照範例
        /// </summary>
        /// <param name="mapFields"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public byte[] XlsMapField2XlsSample(XlsMapField[] mapFields, string sheetName)
        {
            if (mapFields != null && mapFields.Length > 0)
            {
                if (String.IsNullOrEmpty(sheetName))
                {
                    sheetName = "sheet1";
                }

                try
                {
                    HSSFWorkbook wb = new HSSFWorkbook();
                    HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                    HSSFRow row = (HSSFRow)sheet.CreateRow(0);
                    HSSFCell cell = null;
                    int idx = 0;
                    foreach (XlsMapField mapField in mapFields)
                    {
                        cell = (HSSFCell)row.CreateCell(idx, CellType.String);
                        cell.SetCellValue(mapField.CellName);
                        idx++;
                    }

                    byte[] content = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        wb.Write(ms);
                        ms.Flush();
                        content = ms.ToArray();
                    }
                    row = null;
                    cell = null;
                    sheet = null;
                    wb = null;
                    return content;
                }
                catch (Exception)
                {
                }
            }
            return null;
        }
        #endregion

        #region DataTable2Xls
        /// <summary>
        /// 取得 DataTable 轉成 Xls 的內容
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isUseAmountFormat">Decimal 型別欄位是否使用金額格式 (預設為 false)</param>
        /// <param name="isDecimalTruncate">Decimal 型別欄位是否只取整數 (預設為 true)</param>
        /// <returns></returns>
        public byte[] Dt2Xls(DataTable dt, bool isUseAmountFormat = false, bool isDecimalTruncate = true, bool toCompression = false, string xlsFileName = null)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string sheetName = "sheet1";

                try
                {
                    HSSFWorkbook wb = new HSSFWorkbook();
                    HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                    HSSFRow row = (HSSFRow)sheet.CreateRow(0);
                    HSSFCell cell = null;
                    int rowIdx = 0;


                    #region Header 名稱列
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        cell = (HSSFCell)row.CreateCell(i, CellType.String);
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }
                    rowIdx++;
                    #endregion

                    #region Decimal 格式化用變數
                    Type decimalType = typeof(System.Decimal);
                    HSSFCellStyle amountCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                    amountCellStyle.Alignment = HorizontalAlignment.Right;
                    amountCellStyle.DataFormat = 3;
                    #endregion

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        row = (HSSFRow)sheet.CreateRow(rowIdx);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].DataType == decimalType)
                            {
                                if (isUseAmountFormat)
                                {
                                    cell = (HSSFCell)row.CreateCell(i, CellType.Numeric);
                                    cell.CellStyle = amountCellStyle;
                                    if (dtRow.IsNull(i))
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(Convert.ToDouble(dtRow[i]));
                                    }
                                }
                                else
                                {
                                    cell = (HSSFCell)row.CreateCell(i, CellType.String);
                                    if (dtRow.IsNull(i))
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else if (isDecimalTruncate)
                                    {
                                        cell.SetCellValue(Convert.ToDecimal(dtRow[i]).ToString("0"));
                                    }
                                    else
                                    {
                                        cell.SetCellValue(dtRow[i].ToString());
                                    }
                                }
                            }
                            else
                            {
                                cell = (HSSFCell)row.CreateCell(i, CellType.String);
                                if (dtRow.IsNull(i))
                                {
                                    cell.SetCellValue("");
                                }
                                else
                                {
                                    cell.SetCellValue(dtRow[i].ToString());
                                }
                            }
                        }
                        rowIdx++;
                    }

                    byte[] content = null;      //wb.GetBytes();
                    if (toCompression)
                    {
                        if (String.IsNullOrEmpty(xlsFileName))
                        {
                            xlsFileName = Path.GetTempFileName() + ".xls";
                        }
                        else
                        {
                            xlsFileName = Path.Combine(Path.GetTempPath(), xlsFileName);
                        }
                        using (FileStream fs = new FileStream(xlsFileName, FileMode.Create, FileAccess.Write))
                        {
                            wb.Write(fs);
                            fs.Flush();
                        }

                        string zipFileName = Path.GetTempFileName();
                        ZIPHelper.ZipFile(xlsFileName, zipFileName);
                        content = File.ReadAllBytes(zipFileName);
                        try
                        {
                            File.Delete(xlsFileName);
                            File.Delete(zipFileName);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream(1024 * 1024 * 64))
                        {
                            wb.Write(ms);
                            ms.Flush();
                            content = ms.ToArray();
                        }
                        GC.Collect();
                    }

                    row = null;
                    cell = null;
                    sheet = null;
                    wb = null;
                    return content;
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        #region [MDY:20190226] 新的 DataTable 轉成 Xls 檔，增加其他格式化參數
        /// <summary>
        /// 取得 DataTable 轉成 Xls 的內容
        /// </summary>
        /// <param name="dt">指定要轉換的 DataTable 資料</param>
        /// <param name="sheetName">指定工作表名稱 (預設為 sheet1)</param>
        /// <param name="isUseColumnCaption">指定是否改用 Column.Caption 作為欄位名稱的資料來源 (預設為 false)</param>
        /// <param name="isUseAmountStyle">指定 Decimal 型別欄位是否使用金額格式 (預設為 false)</param>
        /// <param name="isDecimalTruncate">指定 Decimal 型別欄位不使用金額格式時，是否只取整數值 (預設為 true)</param>
        /// <param name="isUseDateTimeStyle">指定 DateTime 型別欄位是否使用日期時間格式 (預設為 false)</param>
        /// <param name="dateColumns">指定 DateTime 型別欄位使用日期時間格式時，只有日期的欄位清單 (預設為 null)</param>
        /// <param name="toCompression">指定是否壓縮成 ZIP 檔 (預設為 false)</param>
        /// <param name="xlsFileName">指定壓縮檔內的 XLS 檔名 (預設為 null)</param>
        /// <returns>成功則傳回 Xls 檔內容，否則傳回 null</returns>
        public byte[] DataTable2Xls(DataTable dt, string sheetName = "sheet1", bool isUseColumnCaption = false
            , bool isUseAmountStyle = false, bool isDecimalTruncate = true
            , bool isUseDateTimeStyle = false, string[] dateColumns = null
            , bool toCompression = false, string xlsFileName = null)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (String.IsNullOrWhiteSpace(sheetName))
                {
                    sheetName = "sheet1";
                }
                else
                {
                    sheetName = sheetName.Trim();
                }

                try
                {
                    HSSFWorkbook wb = new HSSFWorkbook();
                    HSSFSheet sheet = (HSSFSheet)wb.CreateSheet(sheetName);
                    HSSFRow row = (HSSFRow)sheet.CreateRow(0);
                    HSSFCell cell = null;
                    int rowIdx = 0;

                    #region Header 名稱列
                    row = (HSSFRow)sheet.CreateRow(rowIdx);
                    for (int i=0; i < dt.Columns.Count; i++)
                    {
                        cell = (HSSFCell)row.CreateCell(i, CellType.String);
                        if (isUseColumnCaption)
                        {
                            cell.SetCellValue(dt.Columns[i].Caption);
                        }
                        else
                        {
                            cell.SetCellValue(dt.Columns[i].ColumnName);
                        }
                    }
                    rowIdx++;
                    #endregion

                    HSSFDataFormat format = (HSSFDataFormat)wb.CreateDataFormat();

                    #region Decimal 格式化用變數
                    Type decimalType = typeof(System.Decimal);
                    HSSFCellStyle amountCellStyle = (HSSFCellStyle) wb.CreateCellStyle();
                    amountCellStyle.Alignment = HorizontalAlignment.Right;
                    amountCellStyle.DataFormat = 3;
                    #endregion

                    #region DateTime 格式化用變數
                    Type datetimeType = typeof(System.DateTime);
                    HSSFCellStyle datetimeCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                    datetimeCellStyle.DataFormat = format.GetFormat("yyyy/mm/dd hh:mm:ss");

                    HSSFCellStyle dateCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                    dateCellStyle.DataFormat = format.GetFormat("yyyy/mm/dd");

                    List<int> datetimeColumnIndexs = null;   //使用日期時間格式的欄位索引集合
                    List<int> dateColumnIndexs = null;       //只有日期的欄位索引集合
                    if (isUseDateTimeStyle)
                    {
                        datetimeColumnIndexs = new List<int>(dt.Columns.Count);
                        bool hasDateColumn = (dateColumns != null && dateColumns.Length  > 0);
                        dateColumnIndexs = hasDateColumn ? new List<int>(dateColumns.Length) : new List<int>(0);
                        for (int idx = 0; idx < dt.Columns.Count; idx++)
                        {
                            DataColumn dColumn = dt.Columns[idx];
                            if (dColumn.DataType == datetimeType)
                            {
                                datetimeColumnIndexs.Add(idx);
                                if (hasDateColumn && System.Array.Exists<string>(dateColumns, x => dColumn.ColumnName.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    dateColumnIndexs.Add(idx);
                                }
                            }
                        }
                    }
                    else
                    {
                        datetimeColumnIndexs = new List<int>(0);
                        dateColumnIndexs = new List<int>(0);
                    }
                    #endregion

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        row = (HSSFRow)sheet.CreateRow(rowIdx);
                        for (int idx = 0; idx < dt.Columns.Count; idx++)
                        {
                            DataColumn dColumn = dt.Columns[idx];
                            if (dColumn.DataType == decimalType)
                            {
                                #region 處理 Decimal 型別欄位
                                if (isUseAmountStyle)
                                {
                                    cell = (HSSFCell)row.CreateCell(idx, CellType.Numeric);
                                    cell.CellStyle = amountCellStyle;
                                    if (dtRow.IsNull(idx))
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(Convert.ToDouble(dtRow[idx]));
                                    }
                                }
                                else
                                {
                                    cell = (HSSFCell)row.CreateCell(idx, CellType.String);
                                    if (dtRow.IsNull(idx))
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else if (isDecimalTruncate)
                                    {
                                        cell.SetCellValue(Convert.ToDecimal(dtRow[idx]).ToString("0"));
                                    }
                                    else
                                    {
                                        cell.SetCellValue(dtRow[idx].ToString());
                                    }
                                }
                                #endregion
                            }
                            else if (dColumn.DataType == datetimeType && datetimeColumnIndexs.Count > 0)
                            {
                                #region 處理 DateTime 型別欄位且有指定使用日期時間格式
                                if (datetimeColumnIndexs.Contains(idx))
                                {
                                    cell = (HSSFCell)row.CreateCell(idx, CellType.String);
                                    if (dateColumnIndexs.Count > 0 && dateColumnIndexs.Contains(idx))
                                    {
                                        cell.CellStyle = dateCellStyle;  //只有日期
                                    }
                                    else
                                    {
                                        cell.CellStyle = datetimeCellStyle;
                                    }
                                    if (dtRow.IsNull(idx))
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(Convert.ToDateTime(dtRow[idx]));
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                cell = (HSSFCell)row.CreateCell(idx, CellType.String);
                                if (dtRow.IsNull(idx))
                                {
                                    cell.SetCellValue("");
                                }
                                else
                                {
                                    cell.SetCellValue(dtRow[idx].ToString());
                                }
                            }
                        }
                        rowIdx++;
                    }

                    byte[] content = null;      //wb.GetBytes();
                    if (toCompression)
                    {
                        if (String.IsNullOrEmpty(xlsFileName))
                        {
                            xlsFileName = Path.GetTempFileName() + ".xls";
                        }
                        else
                        {
                            xlsFileName = Path.Combine(Path.GetTempPath(), xlsFileName);
                        }
                        using (FileStream fs = new FileStream(xlsFileName, FileMode.Create, FileAccess.Write))
                        {
                            wb.Write(fs);
                            fs.Flush();
                        }

                        string zipFileName = Path.GetTempFileName();
                        ZIPHelper.ZipFile(xlsFileName, zipFileName);
                        content = File.ReadAllBytes(zipFileName);
                        try
                        {
                            File.Delete(xlsFileName);
                            File.Delete(zipFileName);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream(1024 * 1024 * 64))
                        {
                            wb.Write(ms);
                            ms.Flush();
                            content = ms.ToArray();
                        }
                        GC.Collect();
                    }

                    row = null;
                    cell = null;
                    sheet = null;
                    wb = null;
                    return content;
                }
                catch (Exception)
                {
                }
            }
            return null;
        }
        #endregion
        #endregion

        #region 取得 Xls Header
        /// <summary>
        /// 取得 Xls 的 Header
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="headers"></param>
        /// <returns>傳回錯誤訊息</returns>
        public string GetXlsHeader(Stream stream, string sheetName, out List<string> headers)
        {
            headers = null;

            #region 檢查參數
            if (stream == null || !stream.CanRead)
            {
                return "未指定資料流參數或資料流無法讀取";
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                return "未指定工作表名稱";
            }
            #endregion

            string errmsg = null;
            try
            {
                HSSFWorkbook wb = new HSSFWorkbook(stream);

                #region 檢查 Sheet 名稱
                HSSFSheet sheet = (HSSFSheet)wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    return String.Format("儲存格名稱 {0} 不存在", sheetName);
                }
                #endregion

                #region 取 Head 的欄位
                HSSFRow headRow = (HSSFRow)sheet.GetRow(0);
                int columnCount = (headRow.LastCellNum - 1) - headRow.FirstCellNum + 1;
                if (columnCount > 0)
                {
                    headers = new List<string>(columnCount);
                    for (int cellNum = headRow.FirstCellNum; cellNum < columnCount; cellNum++)
                    {
                        HSSFCell xlsCell = (HSSFCell)headRow.GetCell(cellNum);
                        string cellValue = null;
                        if (xlsCell != null && xlsCell.CellType == CellType.String)
                        {
                            cellValue = xlsCell.StringCellValue;
                            if (!String.IsNullOrWhiteSpace(cellValue))
                            {
                                headers.Add(cellValue);
                            }
                        }
                    }
                }
                #endregion

                if (headers == null || headers.Count == 0)
                {
                    return "第一行無任何儲存格有字串型別的值";
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("開啟 Xls 檔發生例外，錯誤訊息：{0}", ex.Message);
            }
            return errmsg;
        }

        /// <summary>
        /// 取得 Xlsx 的 Header
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string GetXlsxHeader(Stream stream, string sheetName, out List<string> headers)
        {
            headers = null;

            #region 檢查參數
            if (stream == null || !stream.CanRead)
            {
                return "未指定資料流參數或資料流無法讀取";
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                return "未指定工作表名稱";
            }
            #endregion

            string errmsg = null;
            try
            {
                XSSFWorkbook wb = new XSSFWorkbook(stream);

                #region 檢查 Sheet 名稱
                XSSFSheet sheet = (XSSFSheet)wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    return String.Format("工作表名稱 {0} 不存在", sheetName);
                }
                #endregion

                #region 取 Head 的欄位
                XSSFRow headRow = (XSSFRow)sheet.GetRow(0);
                int columnCount = (headRow.LastCellNum - 1) - headRow.FirstCellNum + 1;
                if (columnCount > 0)
                {
                    headers = new List<string>(columnCount);
                    for (int cellNum = headRow.FirstCellNum; cellNum < columnCount; cellNum++)
                    {
                        XSSFCell xlsCell = (XSSFCell)headRow.GetCell(cellNum);
                        string cellValue = null;
                        if (xlsCell != null && xlsCell.CellType == CellType.String)
                        {
                            cellValue = xlsCell.StringCellValue;
                            if (!String.IsNullOrWhiteSpace(cellValue))
                            {
                                headers.Add(cellValue);
                            }
                        }
                    }
                }
                #endregion

                if (headers == null || headers.Count == 0)
                {
                    return "第一行無任何儲存格有字串型別的值";
                }
            }
            catch (Exception ex)
            {
                errmsg = String.Format("開啟 Xlsx 檔發生例外，錯誤訊息：{0}", ex.Message);
            }
            return errmsg;
        }

        /// <summary>
        /// 取得 Ods 的 Header
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string GetOdsHeader(Stream stream, string sheetName, out List<string> headers)
        {
            headers = null;

            #region 檢查參數
            if (stream == null || !stream.CanRead)
            {
                return "未指定資料流參數或資料流無法讀取";
            }
            if (String.IsNullOrEmpty(sheetName))
            {
                return "未指定工作表名稱";
            }
            #endregion

            string errmsg = null;
            try
            {
                Fuju.ODS.ODSReader reader = new Fuju.ODS.ODSReader(stream);

                #region 檢查 Sheet 名稱
                if (!reader.IsExistSheetName(sheetName))
                {
                    return String.Format("工作表名稱 {0} 不存在", sheetName);
                }
                #endregion

                errmsg = reader.GetRowCellValues(sheetName, 1, out headers);
            }
            catch (Exception ex)
            {
                errmsg = String.Format("開啟 Ods 檔發生例外，錯誤訊息：{0}", ex.Message);
            }
            return errmsg;
        }
        #endregion
    }
    #endregion
}
