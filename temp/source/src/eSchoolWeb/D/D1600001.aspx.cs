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
    /// 上傳中國信託繳費單資料
    /// </summary>
    public partial class D1600001 : BasePage
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
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無權限");
                return false;
            }
            #endregion

            #region 處理商家代號
            string receiveType = null;
            string yearId = null;
            string termId = null;
            string depId = null;
            string receiveId = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearId, out termId, out depId, out receiveId)
                || String.IsNullOrEmpty(receiveType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號參數");
                this.ShowSystemMessage(msg);
                return false;
            }

            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearId, termId, depId, receiveId);
            if (xmlResult.IsSuccess)
            {
                this.EditReceiveType = receiveType;
            }
            #endregion

            #region 檢查商家代號授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該商家代號");
                this.lbtnUpload.Visible = false;
                return false;
            }
            #endregion

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
            #region 商家代號
            string receiveType = this.EditReceiveType;
            if (String.IsNullOrEmpty(receiveType))
            {
                this.ShowMustInputAlert("商家代號");
                return;
            }
            #endregion

            #region 繳款期限
            DateTime? payDueDate = null;
            {
                string txt = this.tbxPayDueDate.Text.Trim();
                if (String.IsNullOrEmpty(txt))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowMustInputAlert("繳款期限");
                    return;
                }

                DateTime date;
                if (DateTime.TryParse(txt, out date))
                {
                    if (date < DateTime.Today)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「繳款期限」不可小於今日");
                        this.ShowJsAlert(msg);
                        return;
                    }
                    else
                    {
                        payDueDate = date;
                    }
                }
                else
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「繳款期限」不是合法的日期格式");
                    this.ShowJsAlert(msg);
                    return;
                }
            }
            #endregion

            #region 上傳檔案
            string fileName = this.fileUpload.FileName;
            Byte[] fileContent = this.fileUpload.FileBytes;
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
                this.ShowMustInputAlert("上傳檔案");
                return;
            }
            #endregion
            #endregion

            #region 除錯日誌紀錄器
            DebugLogger debugger = DebugLogger.Create(this.GetLogonUser(), this.Session.SessionID);
            debugger.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] D1600001 開始處理上傳檔案 ", DateTime.Now).AppendLine(false);
            #endregion

            System.Diagnostics.Stopwatch watchA = new System.Diagnostics.Stopwatch();
            watchA.Start();

            #region 上傳
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Insert);
                string dataInfo = null;

                #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
                string fileType = Path.GetExtension(fileName).TrimStart('.').ToLower();
                XmlResult xmlResult = DataProxy.Current.ImportQueueCTCBFile(this.Page, receiveType, fileContent, fileType, payDueDate.Value, out dataInfo);
                #endregion

                //為了傳回 dataInfo，所以資料錯誤仍傳回成功
                if (xmlResult.IsSuccess && String.IsNullOrEmpty(xmlResult.Message))
                {
                    this.ShowActionSuccessAlert(action, null);
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
                this.labResult.Text = dataInfo;
            }
            #endregion

            watchA.Stop();
            debugger.AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] D1600001 結束處理上傳檔案 ，共耗時 {1} 秒", DateTime.Now, (watchA.ElapsedMilliseconds / 1000M)).AppendLine(false).Flush();
            debugger.Dispose();
            debugger = null;
        }
    }
}