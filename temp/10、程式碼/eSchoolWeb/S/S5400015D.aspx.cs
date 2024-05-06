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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 首頁功能日誌查詢(明細)
    /// </summary>
    public partial class S5400015D : BasePage
    {
        #region Property
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
                ViewState["ACTION"] = String.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 儲存任務編號的查詢條件
        /// </summary>
        private string QueryTaskNo
        {
            get
            {
                return ViewState["QueryTaskNo"] as string;
            }
            set
            {
                ViewState["QueryTaskNo"] = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 取得資料並結繫
        /// </summary>
        private void GetDataAndBind()
        {
            string action = this.GetLocalized("查詢明細資料");

            #region 查詢條件
            Expression where = new Expression(WebLogEntity.Field.TaskNo, this.QueryTaskNo);
            #endregion

            #region 查詢資料
            WebLogEntity data = null;
            XmlResult xmlResult = DataProxy.Current.SelectFirst<WebLogEntity>(this, where, null, out data);
            if (!xmlResult.IsSuccess)
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }
            if (data == null)
            {
                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                return;
            }
            #endregion

            #region 結繫資料
            if (data != null)
            {
                WebLogRequestList list = new WebLogRequestList();
                WebLogRequestItem item = list.Find(data.RequestId);

                string requestKindName = WebLogRequestKindCodeTexts.GetText(data.RequestKind);
                string unitKindName = UserQualCodeTexts.GetText(data.UserUnitKind);

                this.labTaskNo.Text = Server.HtmlEncode(data.TaskNo);
                this.labRequestId.Text = Server.HtmlEncode(item == null ? data.RequestId : item.RequestName);
                this.labRequestKind.Text = Server.HtmlEncode(String.IsNullOrEmpty(requestKindName) ? data.RequestKind : requestKindName);
                this.labRequestDesc.Text = Server.HtmlEncode(data.RequestDesc);
                this.labRequestTime.Text = data.RequestTime.ToString("yyyy/MM/dd HH:mm:ss");
                this.labRequestArgs.Text = Server.HtmlEncode(data.RequestArgs); // String.Format("<pre>{0}</pre>", Server.HtmlEncode(data.RequestArgs));
                this.labWebMachine.Text = Server.HtmlEncode(data.WebMachine);
                this.labSessionId.Text = Server.HtmlEncode(data.SessionId);
                this.labClientIp.Text = Server.HtmlEncode(data.ClientIp);
                this.labUserUnitKind.Text = Server.HtmlEncode(String.IsNullOrEmpty(unitKindName) ? data.UserUnitKind : unitKindName);
                this.labUserUnitId.Text = Server.HtmlEncode(data.UserUnitId);
                this.labUserLoginId.Text = Server.HtmlEncode(data.UserLoginId);
                this.labResponseTime.Text = data.ResponseTime.HasValue ? data.ResponseTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : String.Empty;
                this.litResponseData.Text = Server.HtmlEncode(data.ResponseData);   // String.Format("<pre>{0}</pre>", Server.HtmlEncode(data.ResponseData));
                this.labStatusCode.Text = Server.HtmlEncode(data.StatusCode);
                this.labStatusMessage.Text = Server.HtmlEncode(data.StatusMessage);
                this.labLogTime.Text = data.LogTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
            #endregion
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無查詢權限");
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                string qTaskNo = this.QueryTaskNo = QueryString.TryGetValue("TaskNo", String.Empty);
                if (String.IsNullOrEmpty(qTaskNo)
                    || this.Action != ActionMode.View)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                this.GetDataAndBind();
            }
        }
    }
}