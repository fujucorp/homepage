using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using Fuju;
using Fuju.DB;
using Fuju.Web;
using Entities;
using System.IO;
namespace eSchoolWeb
{
    public partial class print_bill : LocalizedPage //System.Web.UI.Page, IMenuPage
    {
        #region Const
        /// <summary>
        /// 預設的 Javascript Alert 訊息 Key 的常數
        /// </summary>
        private const string SHOW_JS_ALERT_KEY = "SHOW_JS_ALERT";
        #endregion

        #region Keep 頁面參數
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return ViewState["ACTION"] as string;
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的業務別碼參數
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
        /// 編輯的學年參數
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
        /// 編輯的學期參數
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
        /// 編輯的部別參數
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
        /// 編輯的代收費用別參數
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

        /// <summary>
        /// 編輯的學號參數
        /// </summary>
        private string EditStuId
        {
            get
            {
                return ViewState["EditStuId"] as string;
            }
            set
            {
                ViewState["EditStuId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的舊資料序號參數
        /// </summary>
        private int EditOldSeq
        {
            get
            {
                object value = ViewState["EditOldSeq"];
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["EditOldSeq"] = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// 編輯資料的銷帳狀態
        /// </summary>
        private string EditCancelStatus
        {
            get
            {
                return ViewState["EditCancelStatus"] as string;
            }
            set
            {
                ViewState["EditCancelStatus"] = value;
            }
        }

        private string[] KeepReceiveItemNames
        {
            get
            {
                return ViewState["KeepReceiveItemNames"] as string[];
            }
            set
            {
                ViewState["KeepReceiveItemNames"] = value;
            }
        }
        #endregion

        #region [MDY:20200807] M202008_01 Keep 學生姓名是否要遮罩頁面參數
        /// <summary>
        /// 學生姓名是否要遮罩
        /// </summary>
        private bool KeepIsMaskReceiveType
        {
            get
            {
                object value = ViewState["KeepIsMaskReceiveType"];
                if (value is bool)
                {
                    return (bool)value;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["KeepIsMaskReceiveType"] = value;
            }
        }
        #endregion

        #region ResponseFile
        ///// <summary>
        ///// 回傳檔案內容
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="content"></param>
        //protected void ResponseFile(string fileName, byte[] content, string fileType = null)
        //{
        //    string browser = this.Request.Browser.Browser.ToUpper();
        //    if (browser == "IE" || browser == "INTERNETEXPLORER")
        //    {
        //        fileName = HttpUtility.UrlPathEncode(fileName);
        //    }
        //    if (String.IsNullOrEmpty(fileType))
        //    {
        //        fileType = Path.GetExtension(fileName);
        //    }
        //    else
        //    {
        //        fileType = fileType.Trim();
        //    }
        //    this.Response.Clear();
        //    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    this.Response.AddHeader("Content-Language", "utf-8");
        //    this.Response.ContentType = this.GetContentType(fileType);
        //    this.Response.BinaryWrite(content);
        //    this.Response.End();
        //}

        //protected string GetContentType(string extName)
        //{
        //    extName = extName == null ? string.Empty : extName.Trim().ToUpper();
        //    switch (extName)
        //    {
        //        case "PDF":
        //            return "application/pdf";
        //        case "TXT":
        //            return "application/txt";
        //        case "XLS":
        //            return "application/vnd.ms-excel";
        //        case "MDB":
        //            return "application/vnd.ms-access";
        //        case "DOC":
        //            return "application/msword";
        //    }
        //    return "application/octet-stream";
        //}
        #endregion

        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "print_receipt";
            }
        }

        /// <summary>
        /// 取得選單(功能)名稱
        /// </summary>
        public override string MenuName
        {
            get
            {
                return "列印收據";
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

        private bool getStudentReceive(string cancel_no, string stuId, out StudentReceiveView3[] student_receives, out string msg)
        {
            bool rc = false;
            student_receives = null;
            msg = "";

            #region [MDY:20210301] 改為商家代號 + 虛擬帳號 + 未繳條件 (View 已做 NULL 轉空字串 處理)
            Expression where = new Expression(StudentReceiveView3.Field.ReceiveType, cancel_no.Substring(0, 4))
                .And(StudentReceiveView3.Field.CancelNo, cancel_no)
                .And(StudentReceiveView3.Field.ReceiveWay, String.Empty);
            #endregion

            #region 繳款期限條件 (View 已做 NULL 轉空字串 處理)
            string twd7Today = Common.GetTWDate7(DateTime.Today);
            where.And(StudentReceiveView3.Field.PayDueDate, RelationEnum.GreaterEqual, twd7Today);
            #endregion

            #region 開放列印條件 (View 已做 NULL 轉空字串 處理)
            string d8Today = Common.GetDate8(DateTime.Today);
            //判斷 開放列印日期 (View 已做 ISNULL 處理)
            Expression w2 = new Expression(StudentReceiveView3.Field.BillOpenDate, String.Empty).Or(StudentReceiveView3.Field.BillOpenDate, RelationEnum.LessEqual, d8Today);

            //判斷 結束列印日期 (View 已做 ISNULL 處理)
            Expression w3 = new Expression(StudentReceiveView3.Field.BillCloseDate, String.Empty).Or(StudentReceiveView3.Field.BillCloseDate, RelationEnum.GreaterEqual, d8Today);

            where.And(w2.And(w3));
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

        #region [MDY:20200807] M202008_01 檢查指定商家代號是否學生姓名要遮罩 (2020806_01)
        /// <summary>
        /// 檢查指定商家代號是否學生姓名要遮罩
        /// </summary>
        /// <param name="receiveType">指定商家代號</param>
        /// <returns>是則傳回 true，否則傳回 false，檢查失敗也傳回 false</returns>
        private bool IsMaskReceiveType(string receiveType)
        {
            if (!String.IsNullOrWhiteSpace(receiveType) && receiveType.Length == 4)
            {
                #region 讀取學生姓名要遮罩的商家代號系統參數
                ConfigEntity data = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.MASK_RECEIVETYPE);
                KeyValueList<OrderByEnum> orderbys = null;
                XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this.Page, where, orderbys, out data);
                #endregion

                if (xmlResult.IsSuccess && data != null)
                {
                    if (("," + data.ConfigValue + ",").IndexOf("," + receiveType + ",") > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        private bool GenBill()
        {
            bool rc = false;
            byte[] pdfContent = null;

            #region [MDY:20200807] M202008_01 依據學生姓名要遮罩的商家代號系統參數決定是否要遮罩 (2020806_01)
            XmlResult xmlResult = DataProxy.Current.ExecB2100002Request(this.Page, "GENBILL"
                , this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId, this.EditStuId, this.EditOldSeq
                , this.KeepIsMaskReceiveType, this.isEngUI(), out pdfContent);
            #endregion

            if (xmlResult.IsSuccess)
            {
                #region [MDY:20210401] 原碼修正
                string fileName = String.Format("{0}繳費單.PDF", HttpUtility.UrlEncode(EditStuId));
                #endregion

                this.ResponseFile(fileName, pdfContent);
                rc = true;
            }
            else
            {
                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                ////this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);

                //StringBuilder js = new StringBuilder();
                //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("產生繳費單發生錯誤，請稍後再試.")).AppendLine();

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                #endregion

                this.ShowJsAlert("產生繳費單失敗，請稍後再試");
                #endregion
            }
            return rc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.ccbtnOK.Attributes.Add("onclick", "comingsoon();");
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string cancel_no = this.txtCancelNo.Text.Trim();

            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點  & 統一參數檢查處理
            #region [OLD]
            //string validate_number = this.txtValidateNum.Text.Trim();
            //string real_validate_number = (string)Session["ValidateNum"];
            ////string language = "tw-zh";

            //#region 檢查銷帳編號
            //if(cancel_no=="")
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("請輸入要查詢的虛擬帳號")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }
            //    return;
            //}
            //if(cancel_no.Length!=16 && cancel_no.Length!=14)
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("輸入的虛擬帳號有誤，請重新輸入")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }
            //    return;
            //}
            //#endregion

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
                    this.ShowJsAlert("驗證碼錯誤，請重新輸入");
                    return;
                }
            }
            #endregion

            #region 檢查銷帳編號
            if (String.IsNullOrEmpty(cancel_no))
            {
                this.ShowMustInputAlert("虛擬帳號");
                return;
            }
            if (!Common.IsNumber(cancel_no) && cancel_no.Length != 16 && cancel_no.Length != 14)
            {
                this.ShowJsAlert("輸入的虛擬帳號有誤，請重新輸入");
                return;
            }
            #endregion
            #endregion

            #region [MDY:20200902] 2020901_01 增加學號的輸入作為驗證欄位
            string stuId = this.tbxStuId.Text.Trim();
            if (String.IsNullOrEmpty(stuId))
            {
                this.ShowMustInputAlert("學號");
                return;
            }
            #endregion

            //先去查詢銷帳編號是否只有一筆，只有一筆就直接列印，如果多筆，就顯示grid
            StudentReceiveView3[] student_receives = null;
            string msg = "";
            if (this.getStudentReceive(cancel_no, stuId, out student_receives, out msg))
            {
                if (student_receives != null && student_receives.Length > 0)
                {
                    #region [MDY:20200807] M202008_01 取得指定商家代號是否學生姓名要遮罩 (2020806_01)
                    this.KeepIsMaskReceiveType = this.IsMaskReceiveType(student_receives[0].ReceiveType);
                    #endregion

                    if (student_receives.Length == 1)
                    {
                        this.EditReceiveType = student_receives[0].ReceiveType;
                        this.EditYearId = student_receives[0].YearId;
                        this.EditTermId = student_receives[0].TermId;
                        this.EditDepId = student_receives[0].DepId;
                        this.EditReceiveId = student_receives[0].ReceiveId;
                        this.EditStuId = student_receives[0].StuId;
                        this.EditOldSeq = student_receives[0].OldSeq;
                        gvResult.Visible = false;
                        GenBill();
                    }
                    else
                    {
                        //超過一筆顯示grid
                        gvResult.DataSource = student_receives;
                        gvResult.DataBind();
                        gvResult.Visible = true;
                    }
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
                    #endregion

                    this.txtCancelNo.Text = String.Empty;
                    this.ShowJsAlert("查無符合的繳費單資料，請重新輸入虛擬帳號");
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
                #endregion

                this.txtCancelNo.Text = String.Empty;
                this.ShowJsAlert("讀取繳費單資料失敗，請稍後再試");
                #endregion
            }
            return;
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                //StringBuilder js = new StringBuilder();
                //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("無法取得要處理資料的參數")).AppendLine();

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                #endregion

                this.ShowJsAlert("無法取得要處理資料的參數");
                #endregion

                return;
            }
            string[] args = argument.Split(new char[] { ',' });
            if (args.Length != 7)
            {
                #region [MDY:20191023] M201910_02 統一改用 ShowJsAlert() 方法顯示錯誤
                #region [OLD]
                //StringBuilder js = new StringBuilder();
                //js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("無法取得要處理資料的參數")).AppendLine();

                //ClientScriptManager cs = this.ClientScript;
                //Type myType = this.GetType();
                //if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
                //{
                //    cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
                //}
                #endregion

                this.ShowJsAlert("無法取得要處理資料的參數");
                #endregion

                return;
            }
            string receive_type = args[0];
            string year_id = args[1];
            string term_id = args[2];
            string dep_id = args[3];
            string receive_id = args[4];
            string stu_id = args[5];
            string old_seq = args[6];
            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Print:
                    #region 列印
                    {
                        this.EditReceiveType = receive_type;
                        this.EditYearId = year_id;
                        this.EditTermId = term_id;
                        this.EditDepId = dep_id;
                        this.EditReceiveId = receive_id;
                        this.EditStuId = stu_id;
                        this.EditOldSeq = Int32.Parse(old_seq);
                        GenBill();
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            StudentReceiveView3[] datas = this.gvResult.DataSource as StudentReceiveView3[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                StudentReceiveView3 data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5},{6}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.StuId, data.OldSeq);

                MyPrintButton ccbtnPrint = row.FindControl("ccbtnPrint") as MyPrintButton;
                if (ccbtnPrint != null)
                {
                    ccbtnPrint.CommandArgument = argument;
                }
            }
        }
    }
}