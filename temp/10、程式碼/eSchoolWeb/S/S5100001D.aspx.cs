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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 學校資料管理 D00I71 明細頁
    /// </summary>
    public partial class S5100001D : BasePage
    {
        #region Keep 頁面參數
        /// <summary>
        /// 操作模式參數
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
        /// 查詢的業務別碼
        /// </summary>
        private string ViewReceiveType
        {
            get
            {
                return ViewState["ViewReceiveType"] as string;
            }
            set
            {
                ViewState["ViewReceiveType"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        private void GetAndBindData(string receiveType)
        {
            this.labAPPNO.Text = HttpUtility.HtmlEncode(receiveType);

            KeyValueList<string> datas = null;
            XmlResult xmlResult = DataProxy.Current.CallD00I71Request(this.Page, receiveType, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("發送 D00I71 電文");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                return;
            }

            if (datas == null || datas.Count == 0)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("發送 D00I71 電文成功，但查無資料");
                this.ShowSystemMessage(msg);
            }
            else
            {
                Label[] labels = new Label[] {
                    this.labACCTID, this.labUNINO, this.labAPYDAY, this.labCNLDAY,
                    this.labCHKTYPE, this.labTRNDT, this.labCUSTNAME, this.labIPADDR, this.labMAILADDR,
                    this.labSVCFLG, this.labRGDT, this.labSTARTDT, this.labSTPDT, this.labRTNDT,
                    this.labCNCLDT, this.labVIRFLG, this.labUPDBRNO, this.labUPDDATE, this.labUPDTIME,
                    this.labOPDATE, this.labOPTIME, this.labSTPDATE, this.labSTPTIME, this.labCMBFLG,
                    this.labKIND, this.labCHNLFGT, this.labCHNLFGR, this.labCHNLFGM, this.labFILLER
                };
                foreach (Label label in labels)
                {
                    string key = label.ID.Substring(3);
                    KeyValue<string> data = datas.Find(x => x.Key == key);
                    label.Text = data == null ? String.Empty : data.Value;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無查詢權限");
                    this.ShowJsAlert(msg);
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
                this.ViewReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                if (this.Action != ActionMode.View
                    || String.IsNullOrEmpty(this.ViewReceiveType))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                this.GetAndBindData(this.ViewReceiveType);
            }
        }
    }
}