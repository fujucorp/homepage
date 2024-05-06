using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.C
{
    /// <summary>
    /// 學校自收整批上傳
    /// </summary>
    public partial class C3200002 : BasePage
    {
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號、學年或學期參數");
                this.ShowSystemMessage(msg);
                return;
            }

            //this.lbReceiveType.Text = receiveType;
            QueryReceiveType = receiveType;

            this.tbxReceiveDate.Text = String.Empty;
            this.tbxAccountDate.Text = String.Empty;
            this.lbtnUpload.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                this.lbtnUpload.Visible = true;
            }
        }

        #region [MDY:20190906] (2019擴充案) 匯入檔增加 ODS 格式
        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            #region 上傳銷帳媒體檔案
            if (!this.fileUpload.HasFile || this.fileUpload.FileBytes == null || this.fileUpload.FileBytes.Length == 0)
            {
                this.ShowMustInputAlert("上傳銷帳媒體檔案");
                return;
            }
            #endregion

            #region 代收日
            string receiveDate = null;
            {
                if (String.IsNullOrWhiteSpace(this.tbxReceiveDate.Text))
                {
                    this.ShowMustInputAlert("代收日");
                    return;
                }
                DateTime? date = DataFormat.ConvertDateText(this.tbxReceiveDate.Text);
                if (date == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「代收日」不是合法的民國年7碼 (YYYYMMDD) 的日期格式");
                    this.ShowJsAlert(msg);
                    return;
                }
                else
                {
                    receiveDate = Common.GetTWDate7(date.Value);
                    this.tbxReceiveDate.Text = receiveDate;
                }
            }
            #endregion

            #region 入帳日
            string accountDate = null;
            {
                if (String.IsNullOrWhiteSpace(tbxAccountDate.Text))
                {
                    this.ShowMustInputAlert("入帳日");
                    return;
                }
                DateTime? date = DataFormat.ConvertDateText(tbxAccountDate.Text);
                if (date == null)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「入帳日」不是合法的民國年7碼 (YYYYMMDD) 的日期格式");
                    this.ShowJsAlert(msg);
                    return;
                }
                else
                {
                    accountDate = Common.GetTWDate7(date.Value);
                    this.tbxAccountDate.Text = accountDate;
                }
            }
            #endregion

            StringBuilder content = new StringBuilder();

            string extName = Path.GetExtension(this.fileUpload.FileName).ToLower();
            if (extName != ".xls" && extName != ".xlsx" && extName != ".ods")
            {
                string msg = this.GetLocalized("僅支援 Excel 的 xls | xlsx 或 Calc 的 ods 檔案");
                this.ShowSystemMessage(msg);
                return;
            }
            else
            {
                byte[] fileContent = this.fileUpload.FileBytes;
                string sheetName = "Sheet1";
                ConvertFileHelper helper = new ConvertFileHelper();

                #region 取得上傳檔案的表頭
                List<XlsMapField> mapFields = new List<XlsMapField>(2);
                {
                    List<string> headers = null;
                    try
                    {
                        string errmsg = null;
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            if (extName == ".xls")
                            {
                                errmsg = helper.GetXlsHeader(ms, sheetName, out headers);
                            }
                            else if (extName == ".xlsx")
                            {
                                errmsg = helper.GetXlsxHeader(ms, sheetName, out headers);
                            }
                            else if (extName == ".ods")
                            {
                                errmsg = helper.GetOdsHeader(ms, sheetName, out headers);
                            }
                            else
                            {
                                this.ShowSystemMessage(String.Format("不支援 {0} 格式", extName));
                                return;
                            }
                        }
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            this.ShowSystemMessage(String.Format("讀取上傳檔案表頭失敗，{0}", errmsg));
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowSystemMessage(String.Format("讀取上傳檔案表頭發生例外，錯誤訊息：{0}", ex.Message));
                        return;
                    }
                    if (headers != null && headers.Count > 0)
                    {
                        #region 虛擬帳號
                        if (headers.Contains("虛擬帳號"))
                        {
                            mapFields.Add(new XlsMapField("CancelNo", "虛擬帳號", new NumberChecker(12, 16, "12、14或16碼的虛擬帳號數字")));
                        }
                        else
                        {
                            this.ShowSystemMessage("上傳檔案表頭缺少 虛擬帳號");
                            return;
                        }
                        #endregion

                        #region 繳費金額
                        if (headers.Contains("繳費金額"))
                        {
                            mapFields.Add(new XlsMapField("ReceiveAmount", "繳費金額", new DecimalChecker(0M, 999999999M)));
                        }
                        else
                        {
                            this.ShowSystemMessage("上傳檔案表頭缺少 繳費金額");
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        this.ShowSystemMessage("上傳檔案無表頭資料");
                        return;
                    }
                }
                #endregion

                #region 檔案內容轉成 DataTable
                System.Data.DataTable table = null;
                {
                    bool isBatch = true;
                    bool isOK = false;
                    int totalCount = 0;
                    int successCount = 0;
                    string errmsg = null;
                    if (extName == ".xls")
                    {
                        #region Xls 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            isOK = helper.Xls2DataTable(ms, sheetName, mapFields.ToArray(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        #endregion
                    }
                    else if (extName == ".xlsx")
                    {
                        #region Xlsx 轉 DataTable
                        using (MemoryStream ms = new MemoryStream(fileContent))
                        {
                            isOK = helper.Xlsx2DataTable(ms, sheetName, mapFields.ToArray(), isBatch, true, 0, out table, out totalCount, out successCount, out errmsg);
                        }
                        #endregion
                    }
                    else if (extName == ".ods")
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
                        errmsg = String.Format("不支援 {0} 格式", extName);
                        isOK = false;
                    }

                    if (!isOK)
                    {
                        this.ShowSystemMessage(errmsg);
                        return;
                    }
                }
                #endregion

                if (table != null && table.Rows.Count > 0)
                {
                    int rowNo = 0;
                    foreach (System.Data.DataRow row in table.Rows)
                    {
                        #region 檢查是否有資料錯誤
                        rowNo++;
                        string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
                        if (!String.IsNullOrEmpty(failMsg))
                        {
                            this.ShowSystemMessage(String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg));
                            return;
                        }
                        #endregion

                        //轉成後端的文字格式 (0, 16):cancelNo (16, 9):receiveAmount (25, 7):receiveDate (32, 7):AccountDate
                        string cancelNo = row["CancelNo"].ToString().Trim().PadRight(16, ' ');
                        decimal receiveAmount = Convert.ToDecimal(row["ReceiveAmount"]);
                        content.AppendFormat("{0}{1:000000000}{2}{3}", cancelNo, receiveAmount, receiveDate, accountDate).AppendLine();
                    }
                }
                else
                {
                    this.ShowSystemMessage("上傳檔案無資料");
                    return;
                }
            }

            string action = this.GetLocalized("自收處理");

            object returnData = null;
            KeyValue<string>[] arguments = new KeyValue<string>[] {
                new KeyValue<string>("Kind", "2"),
                new KeyValue<string>("ReceiveType", QueryReceiveType),
                new KeyValue<string>("Content", content.ToString())
            };
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.UpdateCancelDatas, arguments, out returnData);
            if (xmlResult.IsSuccess)
            {
                string[] logs = returnData as string[];
                if (logs == null)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.S_INVALID_RETURN_VALUE, "不正確的回傳資料");
                }
                else
                {
                    this.labLog.Text = String.Join("<br/>", logs);
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }
        #endregion

        #region [OLD]
        //protected void lbtnUpload_Click(object sender, EventArgs e)
        //{
        //    #region 上傳銷帳媒體檔案
        //    if (!this.fileUpload.HasFile || this.fileUpload.FileBytes == null || this.fileUpload.FileBytes.Length == 0)
        //    {
        //        this.ShowMustInputAlert("上傳銷帳媒體檔案");
        //        return;
        //    }
        //    #endregion

        //    #region 代收日
        //    string receiveDate = null;
        //    {
        //        if (String.IsNullOrWhiteSpace(this.tbxReceiveDate.Text))
        //        {
        //            this.ShowMustInputAlert("代收日");
        //            return;
        //        }
        //        DateTime? date = DataFormat.ConvertDateText(this.tbxReceiveDate.Text);
        //        if (date == null)
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("「代收日」不是合法的民國年7碼 (YYYYMMDD) 的日期格式");
        //            this.ShowJsAlert(msg);
        //            return;
        //        }
        //        else
        //        {
        //            receiveDate = Common.GetTWDate7(date.Value);
        //            this.tbxReceiveDate.Text = receiveDate;
        //        }
        //    }
        //    #endregion

        //    #region 入帳日
        //    string accountDate = null;
        //    {
        //        if (String.IsNullOrWhiteSpace(tbxAccountDate.Text))
        //        {
        //            this.ShowMustInputAlert("入帳日");
        //            return;
        //        }
        //        DateTime? date = DataFormat.ConvertDateText(tbxAccountDate.Text);
        //        if (date == null)
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("「入帳日」不是合法的民國年7碼 (YYYYMMDD) 的日期格式");
        //            this.ShowJsAlert(msg);
        //            return;
        //        }
        //        else
        //        {
        //            accountDate = Common.GetTWDate7(date.Value);
        //            this.tbxAccountDate.Text = accountDate;
        //        }
        //    }
        //    #endregion

        //    StringBuilder content = new StringBuilder();

        //    #region [MDY:20160921] 改上傳 Excel 檔
        //    #region [Old]
        //    //using (StreamReader reader = new StreamReader(this.fileUpload.FileContent))
        //    //{
        //    //    string line = reader.ReadLine();
        //    //    while (line != null)
        //    //    {
        //    //        content.AppendFormat("{0}{1}{2}", line, receiveDate, accountDate).AppendLine();
        //    //        line = reader.ReadLine();
        //    //    }
        //    //}
        //    #endregion

        //    string extName = Path.GetExtension(this.fileUpload.FileName).ToUpper();
        //    if (extName != ".XLS" && extName != ".XLSX")
        //    {
        //        this.ShowSystemMessage("上傳檔案限 Excel 檔 (.XLS 或 .XLSX)");
        //        return;
        //    }
        //    else
        //    {
        //        byte[] fileBytes = this.fileUpload.FileBytes;
        //        string sheetName = "Sheet1";
        //        ConvertFileHelper helper = new ConvertFileHelper();

        //        #region 取得上傳檔案的 XLS 表頭
        //        List<XlsMapField> mapFields = new List<XlsMapField>(2);
        //        {
        //            List<string> headers = null;
        //            try
        //            {
        //                string errmsg = null;
        //                using (MemoryStream ms = new MemoryStream(fileBytes))
        //                {
        //                    if (extName == ".XLS")
        //                    {
        //                        errmsg = helper.GetXlsHeader(this.fileUpload.FileContent, sheetName, out headers);
        //                    }
        //                    else
        //                    {
        //                        errmsg = helper.GetXlsxHeader(this.fileUpload.FileContent, sheetName, out headers);
        //                    }
        //                }
        //                if (!String.IsNullOrEmpty(errmsg))
        //                {
        //                    this.ShowSystemMessage(String.Format("讀取上傳檔案表頭失敗，{0}", errmsg));
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                this.ShowSystemMessage(String.Format("讀取上傳檔案表頭發生例外，錯誤訊息：{0}", ex.Message));
        //                return;
        //            }
        //            if (headers != null && headers.Count > 0)
        //            {
        //                #region 虛擬帳號
        //                if (headers.Contains("虛擬帳號"))
        //                {
        //                    mapFields.Add(new XlsMapField("CancelNo", "虛擬帳號", new NumberChecker(12, 16, "12、14或16碼的虛擬帳號數字")));
        //                }
        //                else
        //                {
        //                    this.ShowSystemMessage("上傳檔案表頭缺少 虛擬帳號");
        //                    return;
        //                }
        //                #endregion

        //                #region 繳費金額
        //                if (headers.Contains("繳費金額"))
        //                {
        //                    mapFields.Add(new XlsMapField("ReceiveAmount", "繳費金額", new DecimalChecker(0M, 999999999M)));
        //                }
        //                else
        //                {
        //                    this.ShowSystemMessage("上傳檔案表頭缺少 繳費金額");
        //                    return;
        //                }
        //                #endregion
        //            }
        //            else
        //            {
        //                this.ShowSystemMessage("上傳檔案無表頭資料");
        //                return;
        //            }
        //        }
        //        #endregion

        //        #region 檔案內容轉成 DataTable
        //        System.Data.DataTable table = null;
        //        {
        //            int totalCount = 0;
        //            int successCount = 0;
        //            string errmsg = null;

        //            #region Xls 轉 DataTable
        //            if (extName == ".XLS")
        //            {
        //                using (MemoryStream ms = new MemoryStream(fileBytes))
        //                {
        //                    if (!helper.Xls2DataTable(ms, sheetName, mapFields.ToArray(), true, true, 0, out table, out totalCount, out successCount, out errmsg))
        //                    {
        //                        this.ShowSystemMessage(errmsg);
        //                        return;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                using (MemoryStream ms = new MemoryStream(fileBytes))
        //                {
        //                    if (!helper.Xlsx2DataTable(ms, sheetName, mapFields.ToArray(), true, true, 0, out table, out totalCount, out successCount, out errmsg))
        //                    {
        //                        this.ShowSystemMessage(errmsg);
        //                        return;
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        #endregion

        //        if (table != null && table.Rows.Count > 0)
        //        {
        //            int rowNo = 0;
        //            foreach (System.Data.DataRow row in table.Rows)
        //            {
        //                #region 檢查是否有資料錯誤
        //                rowNo++;
        //                string failMsg = row.IsNull(ConvertFileHelper.DataLineFailureFieldName) ? null : row[ConvertFileHelper.DataLineFailureFieldName].ToString();
        //                if (!String.IsNullOrEmpty(failMsg))
        //                {
        //                    this.ShowSystemMessage(String.Format("第 {0} 筆資料不正確，錯誤訊息：{1}", rowNo, failMsg));
        //                    return;
        //                }
        //                #endregion

        //                //轉成後端的文字格式 (0, 16):cancelNo (16, 9):receiveAmount (25, 7):receiveDate (32, 7):AccountDate
        //                string cancelNo = row["CancelNo"].ToString().Trim().PadRight(16, ' ');
        //                decimal receiveAmount = Convert.ToDecimal(row["ReceiveAmount"]);
        //                content.AppendFormat("{0}{1:000000000}{2}{3}", cancelNo, receiveAmount, receiveDate, accountDate).AppendLine();
        //            }
        //        }
        //        else
        //        {
        //            this.ShowSystemMessage("上傳檔案無資料");
        //            return;
        //        }
        //    }
        //    #endregion

        //    string action = this.GetLocalized("自收處理");

        //    object returnData = null;
        //    KeyValue<string>[] arguments = new KeyValue<string>[] {
        //        new KeyValue<string>("Kind", "2"),
        //        new KeyValue<string>("ReceiveType", QueryReceiveType),
        //        new KeyValue<string>("Content", content.ToString())
        //    };
        //    XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.UpdateCancelDatas, arguments, out returnData);
        //    if (xmlResult.IsSuccess)
        //    {
        //        string[] logs = returnData as string[];
        //        if (logs == null)
        //        {
        //            this.ShowActionFailureMessage(action, ErrorCode.S_INVALID_RETURN_VALUE, "不正確的回傳資料");
        //        }
        //        else
        //        {
        //            this.labLog.Text = String.Join("<br/>", logs);
        //        }
        //    }
        //    else
        //    {
        //        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //    }
        //}
        #endregion

        #region [Old]
        //#region Keep 頁面參數
        ///// <summary>
        ///// 編輯的學生資料主檔
        ///// </summary>
        //private List<StudentReceiveEntity> EditStudentReceives
        //{
        //    get
        //    {
        //        return ViewState["EditStudentReceives"] as List<StudentReceiveEntity>;
        //    }
        //    set
        //    {
        //        ViewState["EditStudentReceives"] = value == null ? null : value;
        //    }
        //}
        //#endregion

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        this.InitialUI();

        //        #region 檢查維護權限
        //        if (!this.HasMaintainAuth())
        //        {
        //            this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //            return;
        //        }
        //        #endregion
        //    }
        //}

        ///// <summary>
        ///// 初始化使用介面
        ///// </summary>
        //private void InitialUI()
        //{
        //    this.tbxReceiveDate.Text = String.Empty;
        //    this.tbxAccountDate.Text = String.Empty;
        //    this.lbtnUpload.Visible = true;
        //}

        //protected void lbtnUpload_Click(object sender, EventArgs e)
        //{
        //    if (!this.CheckEditData())
        //    {
        //        return;
        //    }


        //    KeyValueList<string> cancelDatas = new KeyValueList<string>();
        //    this.EditStudentReceives = GetEditData();
        //    if (this.EditStudentReceives.Count > 0)
        //    {
        //        //執行多筆銷帳
        //        foreach(StudentReceiveEntity data in this.EditStudentReceives)
        //        {
        //            string argument = String.Format("{0},{1},{2},{3},{4}", data.ReceiveType, data.CancelNo, data.ReceiveAmount, data.ReceiveDate, data.AccountDate);
        //            cancelDatas.Add("args", argument);
        //        }

        //        object returnData = null;
        //        XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.UpdateCancelDatas, cancelDatas, out returnData);
        //        if (xmlResult.IsSuccess)
        //        {
        //            string[] rtnMsg = (string[])returnData;
        //            string message = string.Empty;
        //            if(rtnMsg.Length > 0)
        //            {
        //                message = String.Join("\r\n", rtnMsg);
        //            }

        //            this.ShowSystemMessage(message);
        //            return;
        //        }
        //        else
        //        {
        //            this.ShowSystemMessage(this.GetLocalized("資料更新失敗") + "，" + xmlResult.Message);
        //            return;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 取得輸入的維護資料
        ///// </summary>
        ///// <returns>傳回輸入的維護資料</returns>
        //private List<StudentReceiveEntity> GetEditData()
        //{
        //    List<StudentReceiveEntity> datas = new List<StudentReceiveEntity>();

        //    try
        //    {
        //        if (fileUpload.HasFile)
        //        {
        //            StreamReader reader = new StreamReader(fileUpload.FileContent);
        //            do
        //            {
        //                string line = reader.ReadLine();

        //                StudentReceiveEntity data = new StudentReceiveEntity();

        //                data.ReceiveType = ucFilter1.SelectedReceiveType;

        //                data.CancelNo = line.Substring(0, 16);
        //                string receiveAmount = line.Substring(16, 9);
        //                decimal amount = 0;
        //                decimal.TryParse(receiveAmount, out amount);
        //                data.ReceiveAmount = amount;
        //                data.ReceiveDate = this.tbxReceiveDate.Text.Trim();
        //                data.AccountDate = this.tbxAccountDate.Text.Trim();

        //                datas.Add(data);

        //            } while (reader.Peek() != -1);
        //            reader.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ShowSystemMessage(ex.Message);
        //        return null;
        //    }


        //    return datas;
        //}

        ///// <summary>
        ///// 檢查輸入的維護資料
        ///// </summary>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //private bool CheckEditData()
        //{
        //    if (String.IsNullOrEmpty(fileUpload.FileName))
        //    {
        //        this.ShowMustInputAlert("上傳銷帳媒體檔案");
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(tbxReceiveDate.Text.Trim()))
        //    {
        //        this.ShowMustInputAlert("代收日");
        //        return false;
        //    }

        //    if ((tbxReceiveDate.Text.Trim().Length != 7))
        //    {
        //        this.ShowSystemMessage("代收日日期為7碼");
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(tbxAccountDate.Text.Trim()))
        //    {
        //        this.ShowMustInputAlert("入帳日");
        //        return false;
        //    }

        //    if ((tbxAccountDate.Text.Trim().Length != 7))
        //    {
        //        this.ShowSystemMessage("入帳日日期為7碼");
        //        return false;
        //    }
        //    return true;
        //}
        #endregion
    }
}