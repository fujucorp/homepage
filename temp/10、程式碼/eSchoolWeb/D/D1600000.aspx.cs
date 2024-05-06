using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb.D
{
    /// <summary>
    /// 選單(功能)代碼 D16 的首頁 (上傳中國信託的次選單(功能)頁面)
    /// </summary>
    public partial class D1600000 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ucSubMenu_MenuClick(object sender, SubMenuEventArgs e)
        {
            if (String.IsNullOrEmpty(e.MenuID) || String.IsNullOrEmpty(e.MenuUrl))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得點選的子選單參數");
                this.ShowJsAlert(msg);
                return;
            }

            #region 處理商家代號
            {
                string receiveType = this.ucFilter1.SelectedReceiveType;

                #region 不同的頁面必要參數可能不同，可以在這裡判斷
                //商家代號為必要參數
                if (String.IsNullOrEmpty(receiveType))
                {
                    this.ShowMustInputAlert("Filter1", "ReceiveType", "商家代號");
                    return;
                }
                #endregion

                //一定要用這個方法將業務別碼、學年、學期、部別、代收費用別參數傳給下一頁
                //否則下一頁的 Filter1 與 Filter2 無法正確自動
                WebHelper.SetFilterArguments(receiveType, null, null, null, null);
            }
            #endregion

            Response.Redirect(this.GetResolveUrl(e.MenuUrl));
        }
    }
}