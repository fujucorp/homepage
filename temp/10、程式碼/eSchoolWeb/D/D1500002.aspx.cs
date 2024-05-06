using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.Configuration;

using Entities;
using Helpers;
//using ExcelHelper;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 上傳學生繳費資料
    /// </summary>
    public partial class D1500002 : BasePage
    {
        #region [Old] Const
        //private const string jDll = "clsUploadDataBUA.dll";
        //private const string jClass = "clsUploadDataBUA.Mapping";
        //private const string jTypeId = JobCubeTypeCodeTexts.BUA;
        //private const string JOBCUBE_TABLENAME = "MappingRe";
        #endregion

        #region Property
        /// <summary>
        /// 儲存商家代號代碼的查詢條件
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存年度代碼的查詢條件
        /// </summary>
        private string EditYearId
        {
            get
            {
                return ViewState["EditYearId"] as string;
            }
            set
            {
                ViewState["EditYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存期別代碼的查詢條件
        /// </summary>
        private string EditTermId
        {
            get
            {
                return ViewState["EditTermId"] as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存部門別代碼的查詢條件
        /// </summary>
        private string EditDepId
        {
            get
            {
                return ViewState["EditDepId"] as string;
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存費用別代碼的查詢條件
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return ViewState["EditReceiveId"] as string;
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return false;
            }
            #endregion

            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                || String.IsNullOrEmpty(receiveType)
                || (this.ucFilter1.YearVisible && String.IsNullOrEmpty(yearId))
                || (this.ucFilter1.TermVisible && String.IsNullOrEmpty(termId)))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號、學年或學期參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                this.EditReceiveType = receiveType;
                this.EditYearId = yearId;
                this.EditTermId = termId;
                //this.EditDepId = this.ucFilter2.SelectedDepID;
                this.EditReceiveId = this.ucFilter2.SelectedReceiveID;
            }

            #region 檢查商家代號授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該商家代號");
                this.lbtnUpload.Visible = false;
                return false;
            }
            #endregion

            this.ddlFileType.SelectedValue = "xls";
            this.labSeriorNo.Text = "";    //序號
            this.tbxSheetName.Text = "";

            return this.GetAndBindMappingOption(receiveType, this.ddlFileType.SelectedValue);
        }

        /// <summary>
        /// 取得並結繫對照檔選項資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetAndBindMappingOption(string receiveType, string fileType)
        {
            this.ddlMappingId.Items.Clear();

            if (String.IsNullOrEmpty(receiveType))
            {
                return true;
            }

            XmlResult xmlResult = null;
            CodeText[] items = null;
            switch (fileType)
            {
                case "xls":
                    #region 試算表
                    {
                        Expression where = new Expression(MappingreXlsmdbEntity.Field.ReceiveType, receiveType);
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                        orderbys.Add(MappingreXlsmdbEntity.Field.MappingId, OrderByEnum.Asc);

                        //因為 ReceiveType 是固定的，取 MappingId 即可
                        string[] codeFieldNames = new string[] { MappingreXlsmdbEntity.Field.MappingId };
                        string codeCombineFormat = null;
                        string[] textFieldNames = new string[] { MappingreXlsmdbEntity.Field.MappingName };
                        string textCombineFormat = null;

                        xmlResult = DataProxy.Current.GetEntityOptions<MappingreXlsmdbEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                    }
                    #endregion
                    break;
                case "txt":
                    #region 純文字
                    {
                        Expression where = new Expression(MappingreTxtEntity.Field.ReceiveType, receiveType);
                        KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                        orderbys.Add(MappingreTxtEntity.Field.MappingId, OrderByEnum.Asc);

                        //因為 ReceiveType 是固定的，取 MappingId 即可
                        string[] codeFieldNames = new string[] { MappingreTxtEntity.Field.MappingId };
                        string codeCombineFormat = null;
                        string[] textFieldNames = new string[] { MappingreTxtEntity.Field.MappingName };
                        string textCombineFormat = null;

                        xmlResult = DataProxy.Current.GetEntityOptions<MappingreTxtEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                    }
                    #endregion
                    break;
                default:
                    xmlResult = new XmlResult(false, "不支援的檔案類型", CoreStatusCode.S_NO_SUPPORT, null);
                    break;
            }

            if (xmlResult.IsSuccess && items != null && items.Length > 0)
            {
                WebHelper.SetDropDownListItems(this.ddlMappingId, DefaultItem.Kind.None, false, items, true, false, 0, null, true);
            }
            return xmlResult.IsSuccess;
        }

        /// <summary>
        /// 取得指定商家代號的優先次序編號
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <returns>傳回優先次序編號</returns>
        private int GetSchoolRTypePri(string receiveType)
        {
            SchoolRTypeEntity data = null;
            Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, receiveType);
            XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this, where, null, out data);
            if (xmlResult.IsSuccess)
            {
                int val = 0;
                if (data != null && int.TryParse(data.SchPri, out val))
                {
                    return val;
                }
            }
            return 0;
        }

        #region [Old]
        ////發動者帳號 + 代收類別  +年度代碼 + 學期代碼 + 對照檔代碼 + 部別代碼 + 費用別代碼
        ////檔案名稱 + 要寫入的資料表名稱 + log路徑 + 批號 + 作業類別代碼 + bankPM.cancel + sheet_name
        //private string GenJobParam(string jOwner, string jRid, string jYear, string jTerm, string mappingId, string jDep, string jRecid
        //    , string fileName, string tableName, string logPath, string seriorNo, string jTypeId, string cancel, string sheetName)
        //{
        //    return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
        //        , jOwner, jRid, jYear, jTerm, mappingId, jDep, jRecid
        //        , fileName, tableName, logPath, seriorNo, jTypeId, cancel, sheetName);
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.lbtnUpload.Visible = this.InitialUI();
            }
        }

        protected void ddlFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fileType = this.ddlFileType.SelectedValue;
            this.tbxSheetName.Visible = (fileType == "xls");
            bool isOK = this.GetAndBindMappingOption(this.EditReceiveType, fileType);
        }

        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            #region 檢查資料正確性
            string receiveType = this.EditReceiveType;
            string yearID = this.EditYearId;
            string termID = this.EditTermId;
            string depID = string.Empty;
            string receiveID = ucFilter2.SelectedReceiveID;

            string fileType = this.ddlFileType.SelectedValue;
            string mappingId = this.ddlMappingId.SelectedValue.Trim();
            string sheetName = Server.HtmlEncode(this.tbxSheetName.Text.Trim());
            string fileName = this.fileUpload.FileName;
            Byte[] fileContent = this.fileUpload.FileBytes;

            string jMethod = null;

            #region 部別
            //if (String.IsNullOrEmpty(depID))
            //{
            //    this.ShowMustInputAlert("部別");
            //    return;
            //}
            #endregion

            #region 代收費用別
            if (String.IsNullOrEmpty(receiveID))
            {
                this.ShowMustInputAlert("代收費用別");
                return;
            }
            #endregion

            #region 上傳檔案類型
            if (String.IsNullOrEmpty(fileType))
            {
                this.ShowMustInputAlert("上傳檔案類型");
                return;
            }
            jMethod = JobcubeEntity.GetBUAMethodName(fileType);
            if (String.IsNullOrEmpty(jMethod))
            {
                string msg = this.GetLocalized("不支援的檔案類型");
                this.ShowJsAlert(msg);
            }

            #region [Old]
            //switch (fileType)
            //{
            //    case "txt":
            //        jMethod = "ImportToReTxt";
            //        break;
            //    case "xls":
            //        jMethod = "ImportToReXSL";
            //        break;
            //    default:
            //    string msg = this.GetLocalized("不支援的檔案類型");
            //    this.ShowJsAlert(msg);
            //    return;
            //}
            #endregion
            #endregion

            #region 上傳對照檔
            if (String.IsNullOrEmpty(mappingId))
            {
                this.ShowMustInputAlert("上傳對照檔");
                return;
            }
            #endregion

            #region 工作表名稱
            if (fileType == "xls")
            {
                if (String.IsNullOrEmpty(sheetName))
                {
                    this.ShowMustInputAlert("工作表名稱");
                    return;
                }
                if (Common.HasSymbol(sheetName.Replace("_", "")))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「工作表名稱」不可含有底線以外的符號");
                    this.ShowJsAlert(msg);
                    return;
                }
            }
            #endregion

            #region 檔案
            if (this.fileUpload.HasFile)
            {
                if (fileContent == null || fileContent.Length == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("上傳的檔案無內容");
                    this.ShowJsAlert(msg);
                    return;
                }
                fileName = Path.GetFileName(fileName);
                string extName = Path.GetExtension(fileName).ToLower();

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                if (fileType == "xls" && (extName != ".xls" && extName != ".xlsx" && extName != ".ods"))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("副檔名與上傳檔案類型不合");
                    this.ShowJsAlert(msg);
                    return;
                }
                else if (fileType == "txt" && extName != ".txt")
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("副檔名與上傳檔案類型不合");
                    this.ShowJsAlert(msg);
                    return;
                }
                #endregion

                if (Common.HasSymbol(fileName.Replace("_", "").Replace(".", "")))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「上傳檔案」檔名不可含有底線以外的符號");
                    this.ShowJsAlert(msg);
                    return;
                }

                #region [Old] AP 端已支援 XLSX 所以不用轉換
                //#region 將 xlsx 轉存成 xls
                //if (extName == ".xlsx")
                //{
                //    try
                //    {
                //        string tempPath = ConfigManager.Current.GetProjectConfigValue("ExcelHelper", "TempPath");
                //        if (String.IsNullOrEmpty(tempPath))
                //        {
                //            tempPath = Path.GetTempPath();
                //        }
                //        string xlsxFile = Path.Combine(tempPath, String.Format("{0}_{1:yyyyMMddHHmmss}.xlsx", Path.GetFileNameWithoutExtension(fileName), DateTime.Now));
                //        this.fileUpload.SaveAs(xlsxFile);
                //        string xlsFile = Path.ChangeExtension(xlsxFile, "xls");
                //        string log = null;
                //        if (!ConvertXlsxToXls.ConvertXLSX2XLS(xlsxFile, xlsFile, out log))
                //        {
                //            string msg = this.GetLocalized("處理上傳的 XLSX 檔案失敗，") + log;
                //            this.ShowJsAlert(msg);
                //            return;
                //        }

                //        fileContent = File.ReadAllBytes(xlsFile);

                //        #region 刪除暫存檔
                //        try
                //        {
                //            File.Delete(xlsxFile);
                //            File.Delete(xlsFile);
                //        }
                //        catch (Exception)
                //        {

                //        }
                //        #endregion
                //    }
                //    catch (Exception ex)
                //    {
                //        string msg = this.GetLocalized("處理上傳的 XLSX 檔案失敗，") + ex.Message;
                //        this.ShowJsAlert(msg);
                //        return;
                //    }
                //}
                //#endregion
                #endregion
            }
            else
            {
                this.ShowMustInputAlert("上傳檔案");
                return;
            }
            #endregion
            #endregion

            #region [Old] 檢查代收費用別是否已設定
            //{
            //    int count = 0;
            //    Expression where = new Expression(SchoolRidEntity.Field.ReceiveType, receiveType)
            //        .And(SchoolRidEntity.Field.YearId, yearID)
            //        .And(SchoolRidEntity.Field.TermId, termID)
            //        .And(SchoolRidEntity.Field.DepId, depID)
            //        .And(SchoolRidEntity.Field.ReceiveId, receiveID);
            //    XmlResult xmlResult2 = DataProxy.Current.SelectCount<SchoolRidEntity>(this.Page, where, out count);
            //    if (xmlResult2.IsSuccess)
            //    {
            //        if (count == 0)
            //        {
            //            //[TODO] 固定顯示訊息的收集
            //            string msg = this.GetLocalized("未設定此代收費用別資料，請先至「維護代收費用檔」設定");
            //            this.ShowSystemMessage(msg);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        string action2 = this.GetLocalized("查詢代收費用別設定");
            //        this.ShowActionFailureMessage(action2, xmlResult2.Code, xmlResult2.Message);
            //        return;
            //    }
            //}
            #endregion

            DateTime now = DateTime.Now;

            #region 要新增的 BankPM
            BankpmEntity fileData = new BankpmEntity();
            fileData.Cancel = 0;
            fileData.Filename = fileName;
            fileData.Filedetail = null;
            fileData.Status = BankPMStatusCodeTexts.B;
            fileData.Cdate = now.ToString("yyyy/MM/dd");
            fileData.Udate = now.ToString("yyyy/MM/dd");
            fileData.Tempfile = fileContent;
            fileData.ReceiveType = receiveType;
            #endregion

            #region 要新增的 JobCube
            #region [Old]
            //JobcubeEntity jobCube = new JobcubeEntity();
            //jobCube.Jno = 0;
            //jobCube.Jdll = jDll;
            //jobCube.Jclass = jClass;
            //jobCube.Jmethod = jMethod;
            //jobCube.Jowner = this.GetLogonUser().UserId;
            //jobCube.Jrid = receiveType;
            //jobCube.Jyear = yearID;
            //jobCube.Jterm = termID;
            //jobCube.Jdep = depID;
            //jobCube.Jrecid = receiveID;
            //jobCube.Jprity = this.GetSchoolRTypePri(receiveType);
            //jobCube.Jtypeid = jTypeId;

            //jobCube.Jstatusid = JobCubeStatusCodeTexts.WAIT;
            //jobCube.Jresultid = JobCubeResultCodeTexts.WAIT;
            //jobCube.Jlog = JobCubeResultCodeTexts.WAIT_TEXT;
            //jobCube.CDate = DateTime.Now;
            //jobCube.SeriorNo = String.Empty;
            //jobCube.Memo = String.Empty;
            //jobCube.Chancel = "";

            //jobCube.Jparam = this.GenJobParam(jobCube.Jowner, jobCube.Jrid, jobCube.Jyear, jobCube.Jterm, mappingId, jobCube.Jdep, jobCube.Jrecid
            //    , fileData.Filename, JOBCUBE_TABLENAME, String.Empty, "[@Job.SeriorNo]", jobCube.Jtypeid, "[@Data.Cancel]", sheetName);
            #endregion

            JobcubeEntity jobCube = JobcubeEntity.CreateBUAEmpty(this.GetLogonUser().UserId, receiveType, yearID, termID, depID, receiveID, jMethod);
            jobCube.Jprity = this.GetSchoolRTypePri(receiveType);

            jobCube.Jparam = JobcubeEntity.JoinBUAParameter(jobCube.Jowner, jobCube.Jrid, jobCube.Jyear, jobCube.Jterm, jobCube.Jdep, jobCube.Jrecid
                , mappingId, fileData.Filename, sheetName, "[@Data.Cancel]", "[@Job.SeriorNo]");
            #endregion

            string action = ActionMode.GetActionLocalized(ActionMode.Insert);
            string fileSNo = null;
            string jobSNo = null;
            XmlResult xmlResult = DataProxy.Current.InsertJobCubeForD15(this, fileData, jobCube, out fileSNo, out jobSNo);
            if (xmlResult.IsSuccess)
            {
                this.ShowActionSuccessAlert(action, null);

                this.labSeriorNo.Text = jobSNo;
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }
    }
}