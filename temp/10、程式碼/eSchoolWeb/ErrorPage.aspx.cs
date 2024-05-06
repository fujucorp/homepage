using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchoolWeb
{
    public partial class ErrorPage : LocalizedPage  //System.Web.UI.Page
    {
        /// <summary>
        /// 顯示例外的錯誤訊息
        /// </summary>
        /// <param name="ex">發生的例外</param>
        private void ShowExceptionErrorMessage(Exception ex)
        {
            //[TODO] 可以在這裡加儲存錯誤 Log
            if (ex is HttpRequestValidationException)
            {
                //[TODO] 要收集的錯誤代碼
                this.litMessage.Text = "[WE001] 請勿輸入具有潛在危險的字串";
            }
            else if (ex is WebException)
            {
                //[TODO] 要收集的錯誤代碼
                this.litMessage.Text = "[WE002] 資料存取服務連線失敗";
            }
            else if (ex is HttpException)
            {
                //[TODO] 要收集的錯誤代碼
                #region [MDY:20220618] Checkmarx 調整 (Information Exposure Through an Error Message)
                #region [OLD] 不要直接把例外訊息傳回，改成一般的錯誤訊息
                //this.litMessage.Text = "[WE008] " + ex.Message;
                #endregion

                this.litMessage.Text = "[WE008] 網頁處理發生錯誤";
                #endregion
            }
            else
            {
                //[TODO] 要收集的錯誤代碼
                this.litMessage.Text = "[WE009] 網頁發生未預期錯誤";
            }

            #region [New] 寫 Exception 日誌檔
            try
            {
                string logPath = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_PATH");
                if (!String.IsNullOrEmpty(logPath))
                {
                    DateTime now = DateTime.Now;
                    string logFileName = String.Format("WebException_{0:yyyyMMdd}.log", now);
                    string logFileFullName = System.IO.Path.Combine(logPath, logFileName);
                    Entities.LogonUser logonUser = WebHelper.GetLogonUser();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb
                        .AppendFormat("[{0:yyyy/MM/dd HH:mm:ss}] 網頁執行發生例外 (LogonUser：LogonSN={1}, UnitId={2}, UnitName={3}, UserId={4}, UserName={5}, UserQual={6}, GroupId={7}, RoleType={8}, ReceiveType={9})", now, logonUser.LogonSN, logonUser.UnitId, logonUser.UnitName, logonUser.UserId, logonUser.UserName, logonUser.UserQual, logonUser.GroupId, logonUser.RoleType, logonUser.ReceiveType).AppendLine()
                        .AppendFormat("ex.Message = {0}", ex.Message).AppendLine()
                        .AppendFormat("ex.Source = {0}", ex.Source).AppendLine()
                        .AppendFormat("ex.TargetSite = {0}", ex.TargetSite).AppendLine()
                        .AppendFormat("ex.StackTrace = {0}", ex.StackTrace).AppendLine();
                    if (ex.InnerException != null)
                    {
                        Exception ex2 = ex.InnerException;
                        sb
                        .AppendFormat("InnerException.Message = {0}", ex2.Message).AppendLine()
                        .AppendFormat("InnerException.Source = {0}", ex2.Source).AppendLine()
                        .AppendFormat("InnerException.TargetSite = {0}", ex2.TargetSite).AppendLine()
                        .AppendFormat("InnerException.StackTrace = {0}", ex2.StackTrace).AppendLine();
                    }
                    sb.AppendLine();

                    System.IO.File.AppendAllText(logFileFullName, sb.ToString());
                }
            }
            catch(Exception)
            {
            }
            #endregion
        }

        /// <summary>
        /// 顯示一般的錯誤訊息
        /// </summary>
        /// <param name="errorCode">發生的錯誤代碼</param>
        /// <param name="errorMessage">發生的錯誤訊息</param>
        public void ShowErrorMessage(string errorCode, string errorMessage)
        {
            if (String.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = "頁面發生錯誤";
            }
            if (String.IsNullOrWhiteSpace(errorCode))
            {
                this.litMessage.Text = errorMessage;
            }
            else
            {
                //[TODO] 一般錯誤應該不用存 Log，但是否要轉成模糊錯誤訊息??
                this.litMessage.Text = String.Format("[{0}] {1}", errorCode, errorMessage);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ErrorPageInfo pageInfo = WebHelper.GetErrorPageInfo();
                if (pageInfo == null || pageInfo.IsNoMessage())
                {
                    this.litMessage.Text = "發生未知的錯誤";
                    return;
                }

                //[TODO] 要不要自動轉到登入頁
                //if (pageInfo.ErrorCode == ErrorCode.S_SESSION_TIMEOUT)
                //{
                //    Session.Clear();
                //    Session.Abandon();
                //    Response.Redirect("~/index.aspx", true);
                //}

                if (pageInfo.Exception != null)
                {
                    this.ShowExceptionErrorMessage(pageInfo.Exception);
                }
                else
                {
                    this.ShowErrorMessage(pageInfo.ErrorCode, pageInfo.ErrorMessage);
                }
            }
        }
    }
}