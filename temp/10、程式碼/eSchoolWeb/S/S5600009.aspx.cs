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
    /// 排程作業管理
    /// </summary>
    public partial class S5600009 : BasePage
    {
        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            return this.GetDataAndBind();
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool GetDataAndBind()
        {
            #region 檢查查詢權限
            if (!this.HasQueryAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
                return false;
            }
            #endregion

            #region 查詢條件
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SERVICE_CONFIG);
            #endregion

            #region 排序條件
            KeyValueList<OrderByEnum> orderbys = null;
            #endregion

            #region 取得 ConfigEntity
            ConfigEntity config = null;
            XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this, where, orderbys, out config);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            #endregion

            #region [MDY:2018xxxx] 改用 ServiceConfig2 物件與新的轉換方法
            #region [OLD]
            //#region 轉換 ConfigValue
            //ServiceConfig[] datas = null;
            //if (config != null)
            //{
            //    ServiceConfigHelper helper = new ServiceConfigHelper();
            //    if (!helper.DeXmlString(config.ConfigValue, out datas))
            //    {
            //        xmlResult = new XmlResult(false, "服務項目設定解析失敗", CoreStatusCode.UNKNOWN_ERROR, null);
            //    }
            //}
            //#endregion

            //#region 排序
            //if (datas != null && datas.Length > 0)
            //{
            //    Array.Sort<ServiceConfig>(datas, delegate(ServiceConfig data1, ServiceConfig data2)
            //    {
            //        return data1.JobCubeType.CompareTo(data2.JobCubeType);
            //    });
            //}
            //#endregion
            #endregion

            #region 轉換 ConfigValue
            List<ServiceConfig2> datas = null;
            if (config != null)
            {
                string errmsg = null;
                ServiceConfigHelper helper = new ServiceConfigHelper();
                if (!helper.TryDeXml(config.ConfigValue, out errmsg, out datas))
                {
                    xmlResult = new XmlResult(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);

                    string action = ActionMode.GetActionLocalized(ActionMode.Query);
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region 排序
            if (datas != null && datas.Count > 0)
            {
                datas.Sort(ServiceConfig2.Comparison);
            }
            #endregion
            #endregion

            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Insert);
            Session["QueryString"] = QueryString;

            Server.Transfer("S5600009M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            #region [MDY:2018xxxx] 改用 ServiceConfig2 集合物件
            #region [OLD]
            //ServiceConfig[] datas = this.gvResult.DataSource as ServiceConfig[];
            //if (datas == null || datas.Length == 0)
            //{
            //    return;
            //}

            //foreach (GridViewRow row in this.gvResult.Rows)
            //{
            //    ServiceConfig data = datas[row.RowIndex];
            //    //資料參數
            //    string argument = data.JobCubeType;

            //    row.Cells[0].Text = String.Format("{0}-{1}", data.JobCubeType, data.GetJobCubeTypeName());
            //    row.Cells[1].Text = String.Format("{0}{1}", data.CycleValue, data.GetCycleUnitName());
            //    if (data.CycleUnit == ServiceCycleUnit.Minute)
            //    {
            //        row.Cells[2].Text = String.Format("{0}  {1}", data.CycleStartTime, data.CycleEndTime);
            //    }

            //    #region [MDY:20160413] 將空白轉成換行
            //    Literal litAppArguments = row.FindControl("litAppArguments") as Literal;
            //    if (litAppArguments != null)
            //    {
            //        litAppArguments.Text = data.AppArguments.Replace(" ", "<br/>");
            //    }
            //    #endregion

            //    MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
            //    if (ccbtnModify != null)
            //    {
            //        ccbtnModify.CommandArgument = argument;
            //    }

            //    MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
            //    if (ccbtnDelete != null)
            //    {
            //        ccbtnDelete.CommandArgument = argument;
            //    }
            //}
            #endregion

            List<ServiceConfig2> datas = this.gvResult.DataSource as List<ServiceConfig2>;
            if (datas == null || datas.Count == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                ServiceConfig2 data = datas[row.RowIndex];
                //資料參數
                string argument = data.JobCubeType;

                #region 服務名稱
                row.Cells[0].Text = String.Format("{0}<br/>{1}", Server.HtmlEncode(data.JobCubeType), Server.HtmlEncode(data.GetJobCubeTypeName()));
                #endregion

                #region 週期設定
                Literal litCycle = row.FindControl("litCycle") as Literal;
                if (litCycle != null)
                {
                    DaysCycle cycle = data.Cycle;
                    if (cycle != null)
                    {
                        CycleTime cycleTime = cycle.CycleTime;
                        litCycle.Text = String.Format("{0}<br/>{1}", Server.HtmlEncode(cycle.Info), cycleTime == null ? String.Empty : Server.HtmlEncode(cycleTime.Info));
                    }
                }
                #endregion

                #region 應用程式設定
                Literal litApp = row.FindControl("litApp") as Literal;
                if (litApp != null)
                {
                    litApp.Text = Server.HtmlEncode(data.AppArguments).Replace(" ", "<br/>");
                }
                #endregion

                #region 狀態
                row.Cells[3].Text = data.Enabled ? "啟用" : "停用";
                #endregion

                #region 修改
                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                }
                #endregion

                #region 刪除
                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                }
                #endregion
            }
            #endregion
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            #region 處理資料參數
            string argument = e.CommandArgument as string;
            if (String.IsNullOrEmpty(argument))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "S5600009M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("JobCubeType", argument);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("JobCubeType", argument);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }
}