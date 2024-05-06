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
    /// 人工銷帳
    /// </summary>
    public partial class C3300001 : BasePage
    {
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
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxProblemCancelNo.Text = String.Empty;
            this.ccbtnOK.Visible = true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("ProblemCancelNo", tbxProblemCancelNo.Text.Trim());
            Session["QueryString"] = QueryString;
            Server.Transfer("C3300001M.aspx");
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            #region [MDY:20170721] FIX BUG
            {
                string cancelNo = tbxProblemCancelNo.Text.Trim();
                if (String.IsNullOrEmpty(cancelNo))
                {
                    this.ShowMustInputAlert("有問題虛擬帳號");
                    return false;
                }

                if (!Common.IsNumber(cancelNo) || (cancelNo.Length != 14 && cancelNo.Length != 16))
                {
                    this.ShowSystemMessage(this.GetLocalized("有問題虛擬帳號不合法"));
                    return false;
                }

                string receiveType = cancelNo.Substring(0, 4);
                if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "'該有問題虛擬帳號無權限");
                    return false;
                }
            }
            #endregion

            return true;
        }
    }
}