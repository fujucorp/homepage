using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb.R
{
    /// <summary>
    /// 選單(功能)代碼 R43 的首頁 (產生退費媒體檔的次選單(功能)頁面)
    /// </summary>
    public partial class R4300000 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ucSubMenu_MenuClick(object sender, SubMenuEventArgs e)
        {
            if (String.IsNullOrEmpty(e.MenuID) || String.IsNullOrEmpty(e.MenuUrl))
            {
                //[TODO] 固定顯示訊息的收集
                this.ShowJsAlert("無法取得點選的子選單參數");
                return;
            }

            #region 處理業務別碼、學年、學期參數
            {
                string receiveType = this.ucFilter1.SelectedReceiveType;
                string yearID = this.ucFilter1.SelectedYearID;
                string termID = this.ucFilter1.SelectedTermID;

                #region 不同的頁面必要參數可能不同，可以在這裡判斷
                //業務別碼、學年、學期為必要參數
                if (String.IsNullOrEmpty(receiveType))
                {
                    this.ShowMustInputAlert("Filter1", "ReceiveType", "業務別碼");
                    return;
                }
                if (String.IsNullOrEmpty(yearID))
                {
                    this.ShowMustInputAlert("Filter1", "Year", "學年");
                    return;
                }
                if (String.IsNullOrEmpty(termID))
                {
                    this.ShowMustInputAlert("Filter1", "Term", "學期");
                    return;
                }
                #endregion

                //一定要用這個方法將業務別碼、學年、學期、部別、代收費用別參數傳給下一頁
                //否則下一頁的 Filter1 與 Filter2 無法正確自動
                WebHelper.SetFilterArguments(receiveType, yearID, termID, null, null);
            }
            #endregion

            Response.Redirect(this.GetResolveUrl(e.MenuUrl));
        }
    }
}