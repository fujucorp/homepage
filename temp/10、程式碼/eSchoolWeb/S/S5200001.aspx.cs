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
    /// 功能管理
    /// </summary>
    public partial class S5200001 : PagingBasePage
    {
        #region 實作 PagingBasePage's 抽象方法
        /// <summary>
        /// 取得頁面中的分頁控制項陣列
        /// </summary>
        /// <returns>傳回分頁控制項陣列或 null</returns>
        protected override Paging[] GetPagingControls()
        {
            return new Paging[] { this.ucPaging1, this.ucPaging2 };
        }

        /// <summary>
        /// 取得查詢條件與排序方法
        /// </summary>
        /// <param name="where">成功則傳回查詢條件，否則傳回 null</param>
        /// <param name="orderbys">成功則傳回查詢條件，否則傳回 null</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult GetWhereAndOrderBys(out Expression where, out KeyValueList<OrderByEnum> orderbys)
        {
            where = new Expression();
            orderbys = new KeyValueList<OrderByEnum>(4);
            orderbys.Add(FuncMenuView.Field.FuncId1, OrderByEnum.Asc);
            orderbys.Add(FuncMenuView.Field.FuncId2, OrderByEnum.Asc);
            orderbys.Add(FuncMenuView.Field.FuncId3, OrderByEnum.Asc);
            orderbys.Add(FuncMenuView.Field.SortNo, OrderByEnum.Asc);
            return new XmlResult(true);
        }

        /// <summary>
        /// 呼叫 QueryDataAndBind 方法
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult CallQueryDataAndBind(PagingInfo pagingInfo)
        {
            Paging[] ucPagings = this.GetPagingControls();
            XmlResult xmlResult = base.QueryDataAndBind<FuncMenuView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            return xmlResult;
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        /// <returns>成功則傳回 true</returns>
        private bool InitialUI()
        {
            PagingInfo pagingInfo = new PagingInfo();
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
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

            Server.Transfer("S5200001M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            FuncMenuView[] datas = this.gvResult.DataSource as FuncMenuView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                FuncMenuView data = datas[row.RowIndex];
                //資料參數
                string argument = data.GetPKey();

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                }

                MySwitchButton ccbtnSwitch = row.FindControl("ccbtnSwitch") as MySwitchButton;
                if (ccbtnSwitch != null)
                {
                    ccbtnSwitch.CommandArgument = argument;
                    switch (data.Status)
                    {
                        case DataStatusCodeTexts.NORMAL:
                            ccbtnSwitch.ShowOnButton = false;
                            break;
                        case DataStatusCodeTexts.DISABLED:
                            ccbtnSwitch.ShowOnButton = true;
                            break;
                        default:
                            ccbtnSwitch.Visible = false;
                            break;
                    }
                }
            }
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

            string funcId = argument.Trim();
            #endregion

            string editUrl = "S5200001M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("FuncId", funcId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Enable:
                case ButtonCommandName.Disable:
                    #region 啟用/停用資料
                    {
                        string commandText = null;

                        #region 更新條件
                        Expression where = new Expression(FuncMenuEntity.Field.FuncId, funcId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        if (e.CommandName == ButtonCommandName.Enable)
                        {
                            commandText = "啟用";
                            fieldValues.Add(FuncMenuEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                        }
                        else
                        {
                            commandText = "停用";
                            fieldValues.Add(FuncMenuEntity.Field.Status, DataStatusCodeTexts.DISABLED);
                        }
                        fieldValues.Add(FuncMenuEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(FuncMenuEntity.Field.MdyDate, DateTime.Now);
                        #endregion

                        string action = this.GetLocalized(commandText);
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<FuncMenuEntity>(this, where, fieldValues, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count == 0)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                this.ShowActionSuccessMessage(action);

                                this.CallQueryDataAndBind(this.ucPaging1.GetPagingInfo());
                            }
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }
    }
}