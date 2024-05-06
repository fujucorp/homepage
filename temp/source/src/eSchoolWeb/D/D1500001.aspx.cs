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
    /// 簡易上傳
    /// </summary>
    public partial class D1500001 : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存代收類別代碼的查詢條件
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.lbtnUpload.Visible = this.InitialUI();
            }
        }

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
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得代收類別");
                this.ShowJsAlert(msg);
                return false;
            }

            this.EditReceiveType = receiveType;
            this.EditYearId = yearID;
            this.EditTermId = termID;
            //this.EditDepId = depID;
            this.EditDepId = "";
            this.EditReceiveId = ReceiveID;
            this.ucFilter1.GetDataAndBind(this.EditReceiveType, EditYearId, EditTermId);

            labSeriorNo.Text = "";    //序號
            tbxSheetName.Text = "";

            #region 檢查代收類別授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetLocalized("該代收類別未授權");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            return true;
        }

        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            #region 檢查資料正確性
            string receiveType = this.EditReceiveType;
            string yearID = this.EditYearId;
            string termID = this.EditTermId;
            string depID = this.EditDepId;
            string receiveID = ucFilter2.SelectedReceiveID;
            string sheetName = Server.HtmlEncode(this.tbxSheetName.Text.Trim());
            string fileName = this.fileUpload.FileName;
            Byte[] fileContent = this.fileUpload.FileBytes;

            #region 檢查部別
            //if (String.IsNullOrEmpty(depID))
            //{
            //    string msg = this.GetLocalized("請選擇部別");
            //    this.ShowJsAlert(msg);
            //    return;
            //}
            #endregion

            #region 檢查代收費用別
            if (String.IsNullOrEmpty(receiveID))
            {
                string msg = this.GetLocalized("請選擇代收費用別");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region 檢查工作表名稱
            if (String.IsNullOrEmpty(sheetName))
            {
                string msg = this.GetLocalized("請填入工作表名稱");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            #region 檢查檔案
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

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                string extName = Path.GetExtension(fileName).ToLower();
                if (extName != ".xls" && extName != ".xlsx" && extName != ".ods")
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("僅支援 Excel 的 xls | xlsx 或 Calc 的 ods 檔案");
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

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式 (因為服務已支援 Xlsx 所以不再轉成 Xls)
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
                this.ShowMustInputAlert("檔案");
                return;
            }
            #endregion
            #endregion

            DateTime now = DateTime.Now;

            #region 新增到 BankPm
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
            //int jobNo = 0;   //系統會自動產生
            //string jobLogPath = ConfigManager.Current.GetProjectConfigValue("OperationLog", "Path");
            //jobCube.Jno = jobNo;
            //jobCube.Jdll = "clsUploadDataBUF.dll";
            //jobCube.Jclass = "clsUploadDataBUF.Mapping";
            //jobCube.Jmethod = "";
            //jobCube.Jowner = this.GetLogonUser().UserId;
            //jobCube.Jrid = this.EditReceiveType;
            //jobCube.Jyear = this.EditYearId;
            //jobCube.Jterm = this.EditTermId;
            //jobCube.Jdep = this.EditDepId;
            //jobCube.Jrecid = this.EditReceiveId;
            //jobCube.Jprity = GetSchoolRTypePri(this.EditReceiveType);
            //jobCube.Jtypeid = JobCubeTypeCodeTexts.BUF;
            //jobCube.Jlog = jobLogPath;
            //jobCube.CDate = DateTime.Now;
            //jobCube.SeriorNo = "[@Job.SeriorNo]";
            //jobCube.Jstatusid = DataStatusCodeTexts.NORMAL;

            //StringBuilder sb = new StringBuilder();
            //sb.Append(jobCube.Jowner + ",");        //發動者帳號
            //sb.Append(jobCube.Jrid + ",");          //代收類別
            //sb.Append(jobCube.Jyear + ",");         //年度代碼
            //sb.Append(jobCube.Jterm + ",");         //學期代碼
            //sb.Append("" + ",");                    //對照檔代碼
            //sb.Append(jobCube.Jdep + ",");          //部別代碼
            //sb.Append(jobCube.Jrecid + ",");        //費用別代碼
            //sb.Append(fileUpload.FileName + ",");   //檔案名稱
            //sb.Append("" + ",");                    //要寫入的資料表名稱
            //sb.Append(jobCube.Jlog + ",");          //log路徑
            //sb.Append(jobCube.SeriorNo + ",");      //批號 
            //sb.Append("BUF" + ",");                 //作業類別代碼
            //sb.Append("[@Data.Cancel]" + ",");      //bankPM.cancel
            //sb.Append(tbxSheetName.Text.Trim());    //sheet_name            
            //jobCube.Jparam = sb.ToString();
            #endregion

            JobcubeEntity jobCube = JobcubeEntity.CreateBUFEmpty(this.GetLogonUser().UserId, receiveType, yearID, termID, depID, receiveID);
            jobCube.Jprity = this.GetSchoolRTypePri(receiveType);

            jobCube.Jparam = JobcubeEntity.JoinBUFParameter(jobCube.Jowner, jobCube.Jrid, jobCube.Jyear, jobCube.Jterm, jobCube.Jdep, jobCube.Jrecid
                , fileData.Filename, sheetName, "[@Data.Cancel]", "[@Job.SeriorNo]");
            #endregion

            jobCube.Jprity = GetSchoolRTypePri(this.EditReceiveType);
            jobCube.SeriorNo = "[@Job.SeriorNo]";

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
    }
}