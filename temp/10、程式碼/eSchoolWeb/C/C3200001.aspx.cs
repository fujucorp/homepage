using System;
using System.Collections.Generic;
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
    /// 自收多筆銷帳
    /// </summary>
    public partial class C3200001 : BasePage
    {
        [Serializable]
        private class CancelData
        {
            public string CancelNo
            {
                get;
                set;
            }
            public string ReceiveAmount
            {
                get;
                set;
            }
            public string ReceiveDate
            {
                get;
                set;
            }
            public string AccountDate
            {
                get;
                set;
            }
            public string Result
            {
                get;
                set;
            }

            public CancelData()
            {

            }

            public override string ToString()
            {
                return String.Concat(this.CancelNo, this.ReceiveAmount, this.ReceiveDate, this.AccountDate);
            }
        }

        #region Keep 頁面參數
        /// <summary>
        /// 編輯的銷帳資料
        /// </summary>
        private List<CancelData> EditCancelDatas
        {
            get
            {
                return ViewState["EditCancelDatas"] as List<CancelData>;
            }
            set
            {
                ViewState["EditCancelDatas"] = value == null ? null : value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxCancelNo.Text = String.Empty;
            this.tbxReceiveAmount.Text = String.Empty;
            this.tbxReceiveDate.Text = String.Empty;
            this.tbxAccountDate.Text = String.Empty;
            this.lbtnCancelBill.Visible = false;
            this.ccbtnOK.Visible = false;
        }

        /// <summary>
        /// 取得並檢查輸入的維護資料
        /// </summary>
        /// <param name="data">傳回銷帳資料</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetAndCheckEditData(out CancelData data)
        {
            data = new CancelData();

            string cancelNo = this.tbxCancelNo.Text.Trim();
            if (String.IsNullOrEmpty(cancelNo))
            {
                this.ShowMustInputAlert("虛擬帳號");
                return false;
            }
            if (!Common.IsNumber(cancelNo, 14, 16))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「虛擬帳號」不是合法，限輸入 14 ~ 16 的數字");
                this.ShowJsAlert(msg);
                return false;
            }
            data.CancelNo = cancelNo.PadRight(16, ' ');

            string receiveAmount = this.tbxReceiveAmount.Text.Trim();
            if (String.IsNullOrEmpty(receiveAmount))
            {
                this.ShowMustInputAlert("應繳金額");
                return false;
            }
            decimal amount = 0M;
            if (!Decimal.TryParse(receiveAmount, out amount) || amount < 0 || amount > 999999999 || Decimal.Truncate(amount) != amount)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「應繳金額」不是合法的金額，限輸入 1 ~ 999999999 的整數金額");
                this.ShowJsAlert(msg);
                return false;
            }
            data.ReceiveAmount = amount.ToString("000000000");

            string receiveDate = this.tbxReceiveDate.Text.Trim();
            if (String.IsNullOrEmpty(receiveDate))
            {
                this.ShowMustInputAlert("代收日");
                return false;
            }
            DateTime? rDate = DataFormat.ConvertDateText(receiveDate);
            if (rDate == null)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「代收日」不是合法的民國年7碼 (YYYYMMDD) 的日期格式");
                this.ShowJsAlert(msg);
                return false;
            }
            data.ReceiveDate = Common.GetTWDate7(rDate.Value);

            string accountDate = this.tbxAccountDate.Text.Trim();
            if (String.IsNullOrEmpty(accountDate))
            {
                this.ShowMustInputAlert("入帳日");
                return false;
            }
            DateTime? aDate = DataFormat.ConvertDateText(accountDate);
            if (accountDate == null)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("「入帳日」不是合法的民國年7碼 (YYYYMMDD) 的日期格式");
                this.ShowJsAlert(msg);
                return false;
            }
            data.AccountDate = Common.GetTWDate7(aDate.Value);

            return true;
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

                this.EditCancelDatas = new List<CancelData>();

                this.ccbtnOK.Visible = true;
            }
        }

        /// <summary>
        /// 將資料新增至 GridView 準備執行多筆銷帳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            CancelData data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            List<CancelData> datas = this.EditCancelDatas;
            if (datas == null)
            {
                datas = new List<CancelData>();
                datas.Add(data);
                this.EditCancelDatas = datas;
            }
            else
            {
                datas.Add(data);
                //this.EditCancelDatas = datas;
            }

            gvResult.DataSource = datas;
            gvResult.DataBind();
            this.lbtnCancelBill.Visible = (datas.Count > 0);
            this.tbxCancelNo.Text = String.Empty;
            this.tbxReceiveAmount.Text = String.Empty;
            this.tbxReceiveDate.Text = String.Empty;
            this.tbxAccountDate.Text = String.Empty;
        }

        /// <summary>
        /// 多筆銷帳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancelBill_Click(object sender, EventArgs e)
        {
            List<CancelData> datas = this.EditCancelDatas;
            if (datas == null || datas.Count > 0)
            {
                StringBuilder content = new StringBuilder();
                foreach (CancelData data in datas)
                {
                    content.AppendLine(data.ToString());
                }

                string action = this.GetLocalized("自收處理");

                object returnData = null;
                KeyValue<string>[] arguments = new KeyValue<string>[] {
                    new KeyValue<string>("Kind", "1"),
                    new KeyValue<string>("Content", content.ToString())
                };
                XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.UpdateCancelDatas, arguments, out returnData);
                if (xmlResult.IsSuccess)
                {
                    string[] logs = returnData as string[];
                    if (logs == null || logs.Length != datas.Count)
                    {
                        this.ShowActionFailureMessage(action, ErrorCode.S_INVALID_RETURN_VALUE, "不正確的回傳資料");
                    }
                    else
                    {
                        for (int idx = 0; idx < datas.Count; idx++)
                        {
                            datas[idx].Result = logs[idx];
                        }
                        this.gvResult.DataSource = datas;
                        this.gvResult.DataBind();
                        this.EditCancelDatas = new List<CancelData>();
                        this.lbtnCancelBill.Visible = false;
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            else
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("請先輸入要銷帳的資料");
                this.ShowSystemMessage(msg);
                return;
            }
        }


        protected void gvResult_PreRender(object sender, EventArgs e)
        {
        }

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

        //        this.EditStudentReceives = new List<StudentReceiveEntity>();

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
        //    this.tbxCancelNo.Text = String.Empty;
        //    this.tbxReceiveAmount.Text = String.Empty;
        //    this.tbxReceiveDate.Text = String.Empty;
        //    this.tbxAccountDate.Text = String.Empty;
        //    this.lbtnCancelBill.Visible = false;
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 將資料新增至 GridView 準備執行多筆銷帳 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    if (!this.CheckEditData())
        //    {
        //        return;
        //    }

        //    StudentReceiveEntity data = this.GetEditData();

        //    this.EditStudentReceives.Add(data);
            
        //    if (this.EditStudentReceives.Count > 0)
        //    {
        //        gvResult.DataSource = this.EditStudentReceives;
        //        gvResult.DataBind();
        //        gvResult.Visible = true;
        //        lbtnCancelBill.Visible = true;
        //    }
        //}

        ///// <summary>
        ///// 取得輸入的維護資料
        ///// </summary>
        ///// <returns>傳回輸入的維護資料</returns>
        //private StudentReceiveEntity GetEditData()
        //{
        //    StudentReceiveEntity data = new StudentReceiveEntity();
        //    data.ReceiveType = ucFilter1.SelectedReceiveType;
        //    data.CancelNo = tbxCancelNo.Text.Trim();
        //    data.ReceiveAmount = Convert.ToDecimal(this.tbxReceiveAmount.Text.Trim());
        //    data.ReceiveDate = this.tbxReceiveDate.Text.Trim();
        //    data.AccountDate = this.tbxAccountDate.Text.Trim();
        //    return data;
        //}

        ///// <summary>
        ///// 檢查輸入的維護資料
        ///// </summary>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //private bool CheckEditData()
        //{
        //    if (String.IsNullOrEmpty(tbxCancelNo.Text.Trim()))
        //    {
        //        this.ShowMustInputAlert("銷帳編號");
        //        return false;
        //    }

        //    if (!Common.IsMoney(tbxReceiveAmount.Text.Trim()))
        //    {
        //        //[TODO] 固定顯示訊息的收集
        //        string msg = this.GetLocalized("金額,請輸入數字");
        //        this.ShowJsAlert(msg);
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

        ///// <summary>
        ///// 多筆銷帳
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbtnCancelBill_Click(object sender, EventArgs e)
        //{
        //    KeyValueList<string> cancelDatas = new KeyValueList<string>();

        //    if (this.EditStudentReceives.Count > 0)
        //    {
        //        //執行多筆銷帳
        //        foreach (StudentReceiveEntity data in this.EditStudentReceives)
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
        //            if (rtnMsg.Length > 0)
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

        //private StudentReceiveEntity[] GetDatas(string ReceiveType, string CancelNo)
        //{            
        //    //用代收類別+銷帳編號去查student_receive
        //    Expression where = new Expression();
        //    where.And(StudentReceiveEntity.Field.ReceiveType, ReceiveType);
        //    where.And(StudentReceiveEntity.Field.CancelNo, CancelNo);
            
        //    #region 排序條件
        //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        //    orderbys.Add(StudentReceiveEntity.Field.ReceiveAmount, OrderByEnum.Asc);
        //    #endregion

            
        //    StudentReceiveEntity[] datas = null;
        //    XmlResult xmlResult = DataProxy.Current.SelectAll<StudentReceiveEntity>(this, where, orderbys, out datas);
        //    if (!xmlResult.IsSuccess)
        //    {
        //        this.ShowSystemMessage(xmlResult.Message);
        //        return null;
        //    }

        //    return datas;
        //}

        //protected void gvResult_PreRender(object sender, EventArgs e)
        //{
        //}
        #endregion
    }
}