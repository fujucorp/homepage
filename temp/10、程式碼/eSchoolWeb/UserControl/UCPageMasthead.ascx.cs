using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 登入後主介面訊息的使用者控制項
    /// </summary>
    public partial class UCPageMasthead : BaseUserControl
    {
        /// <summary>
        /// 初始化使用者介面
        /// </summary>
        private void InitialUI()
        {
            #region 取得目前頁面的選單(功能)階層資料
            MenuInfo[] menus = null;
            MenuInfo myMenu = null;
            bool isEditPage = false;
            bool isSubPage = false;
            {
                BasePage myPage = this.Page as BasePage;
                if (myPage != null)
                {
                    menus = myPage.GetHistoryMenus(out isEditPage, out isSubPage);
                }
                else
                {
                    string menuID = WebHelper.GetPageMenuID(this.Page, out isEditPage, out isSubPage);
                    if (!String.IsNullOrEmpty(menuID))
                    {
                        menus = MenuHelper.Current.GetHistoryMenus(menuID);
                    }
                }
                if (menus != null && menus.Length > 0)
                {
                    myMenu = menus[menus.Length - 1];
                }
            }
            #endregion

            #region 判斷目前頁面是否要顯示 MastHead
            {
                string menuID = myMenu == null ? null : myMenu.ID;
                if (menuID == null || menuID.Length <= 3)
                {
                    //次選單頁面不顯示 MastHead
                    this.divMastHead.Visible = false;
                }
                if (Request.CurrentExecutionFilePath.Equals("Main.aspx"))
                {
                    //登入後的首頁不顯示 MastHead
                    this.divMastHead.Visible = false;
                }
                this.divMastHead.Visible = true;
            }
            #endregion

            #region 產生 MastHead 訊息
            if (this.divMastHead.Visible)
            {
                #region 目前頁面的選單(功能)名稱
                if (myMenu != null)
                {
                    if (isEditPage)
                    {
                        string editTitle = this.GetControlLocalized("UCPageMasthead", "Maintain", "維護");
                        if (myMenu.Name.StartsWith(editTitle))
                        {
                            this.labMenuName.Text =GetLocalized( myMenu.Name);
                        }
                        else
                        {
                            this.labMenuName.Text = String.Concat(editTitle, GetLocalized(myMenu.Name));
                        }
                    }
                    else
                    {
                        this.labMenuName.Text = GetLocalized(myMenu.Name);
                    }
                }
                else
                {
                    this.labMenuName.Text = String.Empty;
                }
                #endregion

                #region 目前頁面的選單(功能)階層資訊
                if (menus != null && menus.Length > 0)
                {
                    //目前頁面不用顯示在階層裡
                    string[] names = new string[menus.Length - 1];
                    for (int idx = 0; idx < menus.Length - 1; idx++)
                    {
                        names[idx] =GetLocalized( menus[idx].Name);
                    }
                    this.divMenuHistory.InnerText = String.Join(" ／ ", names);
                }
                else
                {
                    this.divMenuHistory.InnerText = String.Empty;
                }
                #endregion
            }
            #endregion

            #region 產生登入者資訊
            {
                LogonUser logonUser = WebHelper.GetLogonUser();
                if (logonUser != null)
                {
                    this.divLogonUser.InnerHtml = String.Format("<span>{0}</span>／{1}：{2}", logonUser.UnitName, this.GetControlLocalized("UCPageMasthead", "LogonUser", "登入帳號"), logonUser.UserName);
                }
                else
                {
                    this.divLogonUser.InnerText = String.Empty;
                }
            }
            #endregion

            #region 產生查詢日期 (含時間)
            {
                this.divNow.InnerText = String.Format("{0}：{1:yyyy/MM/dd HH:mm:ss}", this.GetControlLocalized("UCPageMasthead", "QueryDate", "查詢日期"), DateTime.Now);
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }
    }
}