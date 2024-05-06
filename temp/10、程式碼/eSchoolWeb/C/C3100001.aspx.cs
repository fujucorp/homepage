using System;
using System.Collections.Generic;
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
    /// 自收單筆銷帳
    /// </summary>
    public partial class C3100001 : BasePage
    {
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxCancelNo.Text = String.Empty;
            this.tbxReceiveAmount.Text = String.Empty;
            this.tbxReceiveDate.Text = String.Empty;
            this.tbxAccountDate.Text = String.Empty;
            this.ccbtnOK.Visible = false;
        }

        /// <summary>
        /// 取得並檢查輸入的維護資料
        /// </summary>
        /// <param name="data">傳回銷帳資料行</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetAndCheckEditData(out string data)
        {
            data = null;

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
            cancelNo = cancelNo.PadRight(16, ' ');

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
            receiveAmount = amount.ToString("000000000");

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
            receiveDate = Common.GetTWDate7(rDate.Value);

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
            accountDate = Common.GetTWDate7(aDate.Value);

            data = String.Concat(cancelNo, receiveAmount, receiveDate, accountDate);
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

                this.ccbtnOK.Visible = true;
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            string action = this.GetLocalized("自收處理");

            object returnData = null;
            KeyValue<string>[] arguments = new KeyValue<string>[] {
                    new KeyValue<string>("Kind", "1"),
                    new KeyValue<string>("Content", data)
                };
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.UpdateCancelDatas, arguments, out returnData);
            if (xmlResult.IsSuccess)
            {
                string[] logs = returnData as string[];
                if (logs == null || logs.Length == 0)
                {
                    this.ShowActionFailureMessage(action, ErrorCode.S_INVALID_RETURN_VALUE, "不正確的回傳資料");
                }
                else
                {
                    this.ShowSystemMessage(logs[0]);
                    if (logs[0].IndexOf("成功") > -1)
                    {
                        this.tbxCancelNo.Text = String.Empty;
                        this.tbxReceiveAmount.Text = String.Empty;
                    }
                }
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }

        #region [Old]
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
        //    this.tbxCancelNo.Text = String.Empty;
        //    this.tbxReceiveAmount.Text = String.Empty;
        //    this.tbxReceiveDate.Text = String.Empty;
        //    this.tbxAccountDate.Text = String.Empty;
        //    this.ccbtnOK.Visible = true;
        //}

        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    if (!this.CheckEditData())
        //    {
        //        return;
        //    }

        //    string receiveType = ucFilter1.SelectedReceiveType;
        //    string cancelNo = tbxCancelNo.Text.Trim();
        //    string receiveAmount = tbxReceiveAmount.Text.Trim();
        //    string receiveDate = tbxReceiveDate.Text.Trim();
        //    string accountDate = tbxAccountDate.Text.Trim();

        //    //用代收類別+銷帳編號去查student_receive
        //    Expression where = new Expression();
        //    where.And(StudentReceiveEntity.Field.ReceiveType, receiveType);
        //    where.And(StudentReceiveEntity.Field.CancelNo, cancelNo);
            
        //    #region 排序條件
        //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        //    orderbys.Add(StudentReceiveEntity.Field.ReceiveAmount, OrderByEnum.Asc);
        //    #endregion

            
        //    StudentReceiveEntity[] datas = null;
        //    XmlResult xmlResult = DataProxy.Current.SelectAll<StudentReceiveEntity>(this, where, orderbys, out datas);
        //    if (!xmlResult.IsSuccess)
        //    {
        //        string action = ActionMode.GetActionLocalized(ActionMode.Query);
        //        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //        return;
        //    }

        //    #region 銷帳編號不存在，則傳回查無該銷帳編號
        //    if(datas.Length == 0)
        //    {
        //        string msg = this.GetLocalized("查無該銷帳編號");
        //        this.ShowSystemMessage(msg);
        //        return;
        //    }
        //    #endregion

        //    //檢查receive_way是否為null或空值 及 比對金額
        //    foreach(StudentReceiveEntity data in datas)
        //    {
        //        #region 銷帳編號存在但金額不合則傳回金額不合
        //        if (data.ReceiveAmount != Convert.ToDecimal(receiveAmount))
        //        {
        //            string msg = this.GetLocalized("金額不合");
        //            this.ShowSystemMessage(msg);
        //            return;
        //        }
        //        #endregion

        //        #region 銷編與金額都對但已有代收管道資料，則傳回已繳費
        //        if (!string.IsNullOrEmpty(data.ReceiveWay))
        //        {
        //            string msg = this.GetLocalized("已繳費");
        //            this.ShowSystemMessage(msg);
        //            return;
        //        }
        //        #endregion

        //        #region 如果無代收管道及代收金額相符，則執行銷帳
        //        data.CancelFlag = "9";
        //        data.ReceiveDate = receiveDate;
        //        data.AccountDate = accountDate;
        //        data.ReceiveTime = DateTime.Now.ToString("hhmmss");
        //        data.CancelDate = DateTime.Today.ToString("yyyyMMdd");
        //        if (GetLogonUser().UserQual == UserQualCodeTexts.BANK)
        //        {
        //            data.ReceiveWay = "S";  //(如果登入者是行員就是S不然就是C)
        //            data.ReceiveBankId = GetLogonUser().BankId;
        //        }
        //        else
        //        {
        //            data.ReceiveWay = "C";  //(如果登入者是行員就是S不然就是C)
        //            data.ReceiveBankId = string.Empty;
        //        }
        //        int count = 0;
        //        XmlResult result = DataProxy.Current.Update<StudentReceiveEntity>(this, data, out count);
        //        if (result.IsSuccess)
        //        {
        //            if (count < 1)
        //            {
        //                string msg = String.Format("{0}，{1}", this.GetLocalized("銷帳失敗"), this.GetLocalized("無資料被更新"));
        //                this.ShowSystemMessage(msg);
        //                return;
        //            }
        //            else
        //            {
        //                string msg = this.GetLocalized("銷帳成功");
        //                this.ShowSystemMessage(msg);
        //            }
        //        }
        //        else
        //        {
        //            this.ShowSystemMessage(result.Message);
        //        }
        //        #endregion
        //    }
        //}

        ///// <summary>
        ///// 取得輸入的維護資料
        ///// </summary>
        ///// <returns>傳回輸入的維護資料</returns>
        //private StudentReceiveEntity GetEditData()
        //{
        //    StudentReceiveEntity data = new StudentReceiveEntity();
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


        //    if (String.IsNullOrEmpty(tbxAccountDate.Text.Trim()))
        //    {
        //        this.ShowMustInputAlert("入帳日");
        //        return false;
        //    }

        //    return true;
        //}
        #endregion
    }
}