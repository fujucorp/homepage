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
    /// 上傳學生基本資料
    /// </summary>
    public partial class D1500008 : BasePage
    {
        #region Property
        /// <summary>
        /// 儲存商家代號代碼的條件
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
            }

            #region 檢查商家代號授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該商家代號");
                this.lbtnUpload.Visible = false;
                return false;
            }
            #endregion

            this.labSeriorNo.Text = "";    //上傳批號
            this.tbxSheetName.Text = "";

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.lbtnUpload.Visible = this.InitialUI();
            }
        }

        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            #region 檢查資料正確性
            string receiveType = this.EditReceiveType;
            string sheetName = this.tbxSheetName.Text.Trim();
            string fileName = this.fileUpload.FileName;
            Byte[] fileContent = this.fileUpload.FileBytes;

            string jMethod = string.Empty;

            #region 商家代號
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            #endregion

            #region 工作表名稱
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

                #region [MDY:20171115] AP 端已支援 XLSX 所以不要轉了
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

            string owner = this.GetLogonUser().UserId;

            #region 要新增的 JobCube
            JobcubeEntity jobCube = JobcubeEntity.CreateBUGEmpty(owner, receiveType);
            jobCube.Jprity = 0;

            jobCube.Jparam = JobcubeEntity.JoinBUGParameter(owner, receiveType, fileName, sheetName, "[@Data.Cancel]", "[@Job.SeriorNo]");
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

        protected void lbtnSample_Click(object sender, EventArgs e)
        {
            #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
            string extName = "xls";
            LinkButton lbtnSample = sender as LinkButton;
            if (lbtnSample.CommandArgument == "ODS")
            {
                extName = "ods";
            }

            #region 產生試算表的 bytes
            byte[] content = null;
            {
                XlsMapField[] mapFields = StudentMasterEntity.GetXlsMapFields();
                List<string> headTexts = new List<string>(mapFields.Length);
                foreach (XlsMapField mapField in mapFields)
                {
                    headTexts.Add(mapField.CellName);
                }

                string sheetName = "Sheet1";
                if (extName == "xls")
                {
                    #region 產生 xls 的範本
                    #region [MDY:20220503] 改用 ConvertFileHelper.XlsMapField2XlsSample()
                    #region [OLD]
                    //ExcelHelper.ConvertFileHelper helper = new ExcelHelper.ConvertFileHelper();
                    //content = helper.GenXlsSample(headTexts, sheetName);
                    #endregion

                    ConvertFileHelper helper = new ConvertFileHelper();
                    content = helper.XlsMapField2XlsSample(mapFields, sheetName);
                    #endregion
                    #endregion
                }
                else
                {
                    #region 產生 Ods 的範本
                    ODSHelper helper = new ODSHelper();
                    content = helper.GenOdsSample(headTexts, sheetName);
                    #endregion
                }
            }
            #endregion

            if (content == null)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("產生範本失敗");
                this.ShowSystemMessage(msg);
                return;
            }
            else
            {
                string fileName = String.Concat("上傳學生基本資料_範本.", extName);
                this.ResponseFile(fileName, content);
            }
            #endregion
        }
    }
}