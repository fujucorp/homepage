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
    public partial class M0002 : LocalizedPage  //System.Web.UI.Page, IMenuPage
    {
        #region Override LocalizedPage's IMenuPage
        /// <summary>
        /// 取得選單(功能)代碼
        /// </summary>
        public override string MenuID
        {
            get
            {
                return "M0002";
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

        private bool getStudentReceive(string cancel_no, string stuId, out StudentReceiveView3[] student_receives, out string msg)
        {
            bool rc = false;
            student_receives = null;
            msg = "";

            #region [MDY:20191023] M201910_02 調整效能
            Expression where = new Expression(StudentReceiveView3.Field.CancelNo, cancel_no)
                .And(StudentReceiveView3.Field.ReceiveType, cancel_no.Substring(0, 4));
            #endregion

            #region [MDY:20200916] 2020901_01 增加學號的輸入作為驗證欄位
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

        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string cancel_no = this.txtCancelNo.Text.Trim();

            #region [MDY:20191023] M201910_02 修正圖形驗證碼弱點 & 統一參數檢查處理
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
            //    return;
            //}
            //#endregion

            //#region 檢查銷帳編號
            //if (cancel_no == "")
            //{
            //    StringBuilder js = new StringBuilder();
            //    js.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode("虛擬帳號錯誤，請重新輸入.")).AppendLine();

            //    ClientScriptManager cs = this.ClientScript;
            //    Type myType = this.GetType();
            //    if (!cs.IsClientScriptBlockRegistered(myType, "SHOW_JS_ALERT"))
            //    {
            //        cs.RegisterClientScriptBlock(myType, "SHOW_JS_ALERT", js.ToString(), true);
            //    }

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

            #region [MDY:20200916] 2020901_01 增加學號的輸入作為驗證欄位
            string stuId = this.tbxStuId.Text.Trim();
            if (String.IsNullOrEmpty(stuId))
            {
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
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
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
        }
    }
}