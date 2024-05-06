using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 查詢繳費狀態
    /// </summary>
    public partial class check_bill_status : LocalizedPage  //System.Web.UI.Page, IMenuPage
    {
        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "check_bill_status";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "查詢繳費狀態";
            }
        }

        /// <summary>
        /// 取得是否為編輯頁面
        /// </summary>
        public override bool IsEditPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得是否為延伸頁面
        /// </summary>
        public override bool IsSubPage
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得選單(功能)代碼是否符合命名規則
        /// </summary>
        public override bool IsMatchMenuID
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region [MDY:20190506] 處理 WebLog 相關
        /// <summary>
        /// Request 時間
        /// </summary>
        public WebLogEntity WebLog
        {
            get;
            set;
        }

        #region Override Page Method
        protected override void OnLoad(EventArgs e)
        {
            if (this.IsPostBack)
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                this.WebLog = new WebLogEntity()
                    {
                        TaskNo = Guid.NewGuid().ToString().ToUpper(),
                        RequestId = this.MenuID,
                        RequestTime = DateTime.Now,
                        WebMachine = Environment.MachineName,
                        SessionId = Session.SessionID,
                        ClientIp = Request.UserHostAddress,
                        UserUnitKind = logonUser.UserQual,
                        UserUnitId = logonUser.UnitId,
                        UserLoginId = logonUser.UserId,
                    };
            }

            base.OnLoad(e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            #region 處理網頁日誌
            if (this.WebLog != null && !String.IsNullOrEmpty(this.WebLog.RequestKind))
            {
                this.WebLog.LogTime = DateTime.Now;
                XmlResult xmlResult = null;
                try
                {
                    if (!this.WebLog.ResponseTime.HasValue)
                    {
                        this.WebLog.ResponseTime = this.WebLog.LogTime;
                    }
                    int count = 0;
                    xmlResult = DataProxy.Current.Insert<WebLogEntity>(this.Page, this.WebLog, out count);
                }
                catch (Exception ex)
                {
                    xmlResult = new XmlResult(false, "新增網站日誌資料發生例外。" + ex.Message, BaseStatusCode.UNKNOWN_EXCEPTION);
                }
                finally
                {
                    #region 失敗寫 Log File
                    if (xmlResult != null && !xmlResult.IsSuccess)
                    {
                        string logPath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
                        if (!String.IsNullOrEmpty(logPath))
                        {
                            try
                            {
                                string xmlWebLog = null;
                                Fuju.Common.TryToXmlExplicitly<WebLogEntity>(this.WebLog, out xmlWebLog);

                                string logFileName = String.Format("WebLogFail_{0:yyyyMMdd}.log", this.WebLog.LogTime);
                                string logFileFullName = System.IO.Path.Combine(logPath, logFileName);
                                StringBuilder sb = new StringBuilder();
                                sb
                                    .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 處理網頁日誌失敗。", this.WebLog.LogTime).AppendLine()
                                    .AppendFormat("  錯誤代碼：{0}", xmlResult.Code).AppendLine()
                                    .AppendFormat("  錯誤訊息：{0}", xmlResult.Message).AppendLine();
                                if (!String.IsNullOrEmpty(xmlWebLog))
                                {
                                    sb.AppendLine("  網頁日誌：").AppendLine(xmlWebLog).AppendLine();
                                }
                                System.IO.File.AppendAllText(logFileFullName, sb.ToString());
                            }
                            catch
                            {
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion
        #endregion

        private bool getStudentReceive(string cancel_no, string stuId, out StudentReceiveView3[] student_receives, out string msg)
        {
            bool rc = false;
            student_receives = null;
            msg = "";

            #region [MDY:20191023] M201910_02 調整效能
            Expression where = new Expression(StudentReceiveView3.Field.CancelNo, cancel_no)
                .And(StudentReceiveView3.Field.ReceiveType, cancel_no.Substring(0, 4));
            #endregion

            #region [MDY:20200902] 2020901_01 增加學號的輸入作為驗證欄位
            where.And(StudentReceiveView3.Field.StuId, stuId);
            #endregion

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(StudentReceiveView3.Field.YearId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView3.Field.TermId, OrderByEnum.Desc);
            orderbys.Add(StudentReceiveView3.Field.ReceiveId, OrderByEnum.Desc);

            XmlResult xmlResult = DataProxy.Current.SelectAll<StudentReceiveView3>(this.Page, where, orderbys, out student_receives);
            if (!xmlResult.IsSuccess)
            {
                msg = string.Format("[getStudentReceive] 取得學生繳費單資料發生錯誤，錯誤訊息={0}", xmlResult.Message);
            }
            else
            {
                rc = true;
            }

            return rc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UCPageNews.PageId = BoardTypeCodeTexts.QUERYPAY;
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region [MDY:20190506] 紀錄網頁日誌的 Request 相關資訊
            {
                this.WebLog.RequestKind = WebLogRequestKindCodeTexts.QUERY_CODE;
                this.WebLog.RequestDesc = this.MenuName;
                string cancelNo = this.txtCancelNo.Text.Trim();
                this.WebLog.IndexCancelNo = cancelNo;
                if ((cancelNo.Length == 14 || cancelNo.Length == 16) && Common.IsNumber(cancelNo))
                {
                    this.WebLog.IndexReceiveType = cancelNo.Substring(0, 4);
                }
                this.WebLog.RequestArgs = String.Format("CancelNo={0}", cancelNo);
            }
            #endregion

            string cancel_no = this.txtCancelNo.Text.Trim();

            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點 & 統一虛擬帳號檢查處理
            #region [OLD]
            //string validate_number = this.txtValidateNum.Text.Trim();
            //string real_validate_number = (string)Session["ValidateNum"];
            //#region 先檢查圖形驗證碼
            //if (validate_number.ToLower() != real_validate_number.ToLower())
            //{
            //    this.txtValidateNum.Text = "";

            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("驗證碼錯誤，請重新輸入.")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }

            //    //Response.Write(string.Format("<script>alert('驗證碼錯誤，請從新輸入。{0} : {1})</script>", real_validate_number, validate_number));

            //    #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
            //    {
            //        this.WebLog.ResponseTime = DateTime.Now;
            //        this.WebLog.ResponseData = "驗證碼錯誤，請重新輸入.";

            //        this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
            //        this.WebLog.StatusMessage = String.Empty;
            //    }
            //    #endregion

            //    return;
            //}
            //#endregion

            //#region 檢查銷帳編號
            //if (cancel_no == "")
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("請指定虛擬帳號錯誤")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }

            //    #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
            //    {
            //        this.WebLog.ResponseTime = DateTime.Now;
            //        this.WebLog.ResponseData = "請指定虛擬帳號錯誤";

            //        this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
            //        this.WebLog.StatusMessage = String.Empty;
            //    }
            //    #endregion

            //    return;
            //}
            //if (cancel_no.Length != 14 && cancel_no.Length != 16)
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("虛擬帳號錯誤，請重新輸入.")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }

            //    #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
            //    {
            //        this.WebLog.ResponseTime = DateTime.Now;
            //        this.WebLog.ResponseData = "虛擬帳號錯誤，請重新輸入.";

            //        this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
            //        this.WebLog.StatusMessage = String.Empty;
            //    }
            //    #endregion

            //    return;
            //}
            //#endregion
            #endregion

            #region 檢查圖形驗證碼
            {
                string validateCode = this.txtValidateCode.Text.Trim();
                this.txtValidateCode.Text = String.Empty;
                if (!(new ValidatePic()).CheckValidateCode(validateCode))
                {
                    string errmsg = "驗證碼錯誤，請重新輸入";

                    #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                    {
                        this.WebLog.ResponseTime = DateTime.Now;
                        this.WebLog.ResponseData = errmsg;

                        this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
                        this.WebLog.StatusMessage = String.Empty;
                    }
                    #endregion

                    this.ShowJsAlert(errmsg);
                    return;
                }
            }
            #endregion

            #region 檢查銷帳編號
            if (String.IsNullOrEmpty(cancel_no))
            {
                #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                {
                    this.WebLog.ResponseTime = DateTime.Now;
                    this.WebLog.ResponseData = "請指定虛擬帳號";

                    this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
                    this.WebLog.StatusMessage = String.Empty;
                }
                #endregion

                this.ShowMustInputAlert("虛擬帳號");
                return;
            }
            if (!Common.IsNumber(cancel_no) && cancel_no.Length != 16 && cancel_no.Length != 14)
            {
                string errmsg = "輸入的虛擬帳號有誤，請重新輸入";

                #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                {
                    this.WebLog.ResponseTime = DateTime.Now;
                    this.WebLog.ResponseData = errmsg;

                    this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
                    this.WebLog.StatusMessage = String.Empty;
                }
                #endregion

                this.ShowJsAlert(errmsg);
                return;
            }
            #endregion
            #endregion

            #region [MDY:20200902] 2020901_01 增加學號的輸入作為驗證欄位
            string stuId = this.tbxStuId.Text.Trim();
            if (String.IsNullOrEmpty(stuId))
            {
                #region 紀錄網頁日誌的 Response & Status 相關資訊
                {
                    this.WebLog.ResponseTime = DateTime.Now;
                    this.WebLog.ResponseData = "請指定學號";

                    this.WebLog.StatusCode = BaseStatusCode.INVALID_PARAMETER;
                    this.WebLog.StatusMessage = String.Empty;
                }
                #endregion

                this.ShowMustInputAlert("學號");
                return;
            }
            #endregion

            StudentReceiveView3[] student_receives = null;
            string msg = "";
            if (this.getStudentReceive(cancel_no, stuId, out student_receives, out msg))
            {
                if (student_receives != null && student_receives.Length > 0)
                {
                    gvResult.DataSource = student_receives;
                    gvResult.DataBind();
                    gvResult.Visible = true;

                    #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                    {
                        this.WebLog.ResponseTime = DateTime.Now;

                        #region 組 ResponseData 改寫在 gvResult_PreRender()
                        //try
                        //{
                        //    StringBuilder sb = new StringBuilder();
                        //    foreach (StudentReceiveView3 data in student_receives)
                        //    {
                        //        sb.AppendFormat("虛擬帳號={0}; 應繳金額={1}; 學號={2}; 學生姓名={3}; 繳費狀態={4};", data.CancelNo, data.ReceiveAmount, data.StuId, data.MaskedStuName, data.CancelStatus).AppendLine();
                        //    }
                        //    this.WebLog.ResponseData = sb.ToString();
                        //}
                        //catch
                        //{

                        //}
                        #endregion

                        this.WebLog.StatusCode = BaseStatusCode.NORMAL_STATUS;
                        this.WebLog.StatusMessage = String.Empty;
                    }
                    #endregion
                }
                else
                {
                    //沒有資料，顯示彈跳視窗
                    gvResult.Visible = false;

                    #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                    #region [OLD]
                    //StringBuilder js = new StringBuilder();
                    //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("查無符合的繳費單資料，請重新輸入虛擬帳號.")).AppendLine();

                    //ClientScriptManager cs = this.ClientScript;
                    //Type myType = this.GetType();
                    //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                    //{
                    //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                    //}
                    //this.txtCancelNo.Text = "";
                    //this.txtValidateNum.Text = "";

                    //#region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                    //{
                    //    this.WebLog.ResponseTime = DateTime.Now;
                    //    this.WebLog.ResponseData = "查無符合的繳費單資料，請重新輸入虛擬帳號.";

                    //    this.WebLog.StatusCode = ErrorCode.D_DATA_NOT_FOUND;
                    //    this.WebLog.StatusMessage = String.Empty;
                    //}
                    //#endregion
                    #endregion

                    this.txtCancelNo.Text = String.Empty;

                    string errmsg = "查無符合的繳費單資料，請重新輸入虛擬帳號";

                    #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                    {
                        this.WebLog.ResponseTime = DateTime.Now;
                        this.WebLog.ResponseData = errmsg;

                        this.WebLog.StatusCode = ErrorCode.D_DATA_NOT_FOUND;
                        this.WebLog.StatusMessage = String.Empty;
                    }
                    #endregion

                    this.ShowJsAlert(errmsg);
                    #endregion
                }
            }
            else
            {
                //讀取資料失敗
                gvResult.Visible = false;

                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                //StringBuilder js = new StringBuilder();
                //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("讀取繳費單發生錯誤，請稍後再試.")).AppendLine();

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                //this.txtCancelNo.Text = "";
                //this.txtValidateNum.Text = "";

                //#region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                //{
                //    this.WebLog.ResponseTime = DateTime.Now;
                //    this.WebLog.ResponseData = "讀取繳費單發生錯誤，請稍後再試.";

                //    this.WebLog.StatusCode = BaseStatusCode.UNKNOWN_ERROR;
                //    this.WebLog.StatusMessage = msg;
                //}
                //#endregion
                #endregion

                this.txtCancelNo.Text = String.Empty;

                string errmsg = "讀取繳費單資料失敗，請稍後再試";

                #region [MDY:20190506] 紀錄網頁日誌的 Response & Status 相關資訊
                {
                    this.WebLog.ResponseTime = DateTime.Now;
                    this.WebLog.ResponseData = errmsg;

                    this.WebLog.StatusCode = BaseStatusCode.UNKNOWN_ERROR;
                    this.WebLog.StatusMessage = msg;
                }
                #endregion

                this.ShowJsAlert(errmsg);
                #endregion
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            #region [OLD]
            //StudentReceiveView3[] datas = this.gvResult.DataSource as StudentReceiveView3[];
            //if (datas == null || datas.Length == 0)
            //{
            //    return;
            //}

            //foreach (GridViewRow row in this.gvResult.Rows)
            //{
            //    StudentReceiveView3 data = datas[row.RowIndex];

            //    CodeText cancelStatus = CancelStatusCodeTexts.GetCancelStatus(data.ReceiveDate, data.AccountDate);

            //    row.Cells[2].Text = DataFormat.MaskText(data.StuId, DataFormat.MaskDataType.ID);  //學號
            //    row.Cells[3].Text = DataFormat.MaskText(data.StuName, DataFormat.MaskDataType.Name);  //學生姓名
            //    row.Cells[4].Text = cancelStatus.Text;  //繳費狀態

            //}
            #endregion

            #region [MDY:20190506] 紀錄網頁日誌的 ResponseData
            {
                try
                {
                    DataControlFieldCollection columns = this.gvResult.Columns;
                    KeyValueList<int> headers = new KeyValueList<int>(columns.Count);
                    TableCellCollection headerCells = this.gvResult.HeaderRow.Cells;
                    for (int idx = 0; idx < columns.Count; idx++)
                    {
                        if (columns[idx].Visible)
                        {
                            headers.Add(headerCells[idx].Text, idx);
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (GridViewRow row in this.gvResult.Rows)
                    {
                        foreach (KeyValue<int> header in headers)
                        {
                            sb.AppendFormat("{0}={1};", header.Key, row.Cells[header.Value].Text);
                        }
                        sb.AppendLine();

                        if (sb.Length >= 2000)
                        {
                            break;
                        }
                    }
                    this.WebLog.ResponseData = sb.ToString();
                }
                catch(Exception ex)
                {
                    this.WebLog.ResponseData = "組 Response 資料失敗，" + ex.Message;
                }
            }
            #endregion
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}