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
    /// 系統訊息公告(最新消息)
    /// </summary>
    public partial class S5600007 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        /// <summary>
        /// 初始化介面
        /// </summary>
        private void InitialUI()
        {
            DateTime today = DateTime.Today;

            this.tbxStartDate.Text = DataFormat.GetDateText(today.AddMonths(-3));
            this.tbxEndDate.Text = DataFormat.GetDateText(today.AddMonths(3));

            if (!this.GetDataAndBind())
            {
                return;
            }
        }


        protected void ccbtnQuery_Click(object sender, EventArgs e)
        {
            if (!this.GetDataAndBind())
            {
                return;
            }
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
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            #region 查詢的公告日期的起日
            DateTime? sDate = null;
            {
                string txt = this.tbxStartDate.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                    {
                        sDate = date;
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「公告日期的起日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
            }
            #endregion

            #region 查詢的公告日期的迄日
            DateTime? eDate = null;
            {
                string txt = this.tbxEndDate.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date) && date.Year >= 1911)
                    {
                        eDate = date;
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「公告日期的迄日」不是合法的日期格式 (必須是大於1910年的西元年 YYYY/MM/DD 格式)");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
            }
            #endregion

            Expression where = new Expression();
            if (sDate != null)
            {
                where.And(BoardEntity.Field.StartDate, RelationEnum.GreaterEqual, sDate.Value.Date);
            }
            if (eDate != null)
            {
                where.And(BoardEntity.Field.StartDate, RelationEnum.LessEqual, eDate.Value.Date);
            }

            #region [MDY:20190218] 公告位置、公告對象的權限條件
            {
                LogonUser logonUser = this.GetLogonUser();
                if (logonUser.IsBankManager)
                {
                    //授權所有公告位置、公告對象
                }
                else if (logonUser.IsBankUser)
                {
                    //授權學校專區或學生專區且自己的學校
                    where.And(BoardEntity.Field.SchId, new string[] { BoardTypeCodeTexts.SCHOOL, BoardTypeCodeTexts.STUDENT });
                    where.And(BoardEntity.Field.ReceiveType, logonUser.MySchIdentys);
                }
                else
                {
                    //不授權
                    string msg = this.GetLocalized("未授權任何公告位置");
                    this.ShowJsAlert(msg);
                    this.gvResult.DataSource = null;
                    this.gvResult.DataBind();
                    return false;
                }
            }
            #endregion

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(BoardEntity.Field.BoardId, OrderByEnum.Asc);

            BoardEntity[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<BoardEntity>(this, where, orderbys, out datas);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            this.gvResult.DataSource = datas;
            this.gvResult.DataBind();
            return xmlResult.IsSuccess;
        }

        ///// <summary>
        ///// 取得並結繫查詢資料
        ///// </summary>
        ///// <returns>成功則傳回 true</returns>
        //private bool GetDataAndBind()
        //{
        //    #region 檢查查詢權限
        //    if (!this.HasQueryAuth())
        //    {
        //        this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_QUERY, "無查詢權限");
        //        return false;
        //    }
        //    #endregion

        //    Expression where = new Expression();
        //    #region 排序條件
        //    KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
        //    orderbys.Add(BoardEntity.Field.BoardId, OrderByEnum.Asc);
        //    #endregion

        //    BoardEntity[] datas = null;
        //    XmlResult xmlResult = DataProxy.Current.SelectAll<BoardEntity>(this, where, orderbys, out datas);
        //    if (!xmlResult.IsSuccess)
        //    {
        //        string action = ActionMode.GetActionLocalized(ActionMode.Query);
        //        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //    }

        //    this.gvResult.DataSource = datas;
        //    this.gvResult.DataBind();
        //    return xmlResult.IsSuccess;
        //}

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

            Server.Transfer("S5600007M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            BoardEntity[] datas = this.gvResult.DataSource as BoardEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                BoardEntity data = datas[row.RowIndex];
                //資料參數
                string argument = String.Format("{0}", data.BoardId);

                #region [Old]
                //#region 公告日期
                //if (data.StartDate != null)
                //{
                //    DateTime dt = (DateTime)data.StartDate;
                //    row.Cells[1].Text = dt.ToString("yyyy/MM/dd");
                //}
                //#endregion

                //#region 有效日期
                //if (data.EndDate != null)
                //{
                //    DateTime dt = (DateTime)data.EndDate;
                //    row.Cells[2].Text = dt.ToString("yyyy/MM/dd");
                //}
                //#endregion
                #endregion

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
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
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            #endregion

            string editUrl = "S5600007M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("BoardId", argument);
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
                        QueryString.Add("BoardId", argument);
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