using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.C
{
    public partial class C3200003 : PagingBasePage
    {
        /// <summary>
        /// 這隻程式專門查 C3200002 學校自收整批上傳 的 Log (後台特別為 C3200002 額外增加 By 商家代號的紀錄，使用的 Function_Id 為 C3200002_LOG)
        /// </summary>
        private const string _QueryFunctionId = "C3200002_LOG";

        #region Property
        /// <summary>
        /// 儲存查詢的商家代號
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }
        #endregion

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
            where = null;
            orderbys = null;

            #region 商家代號 條件
            //雖然 LogTableView.Field.ReceiveType 應該是統編或分行代碼，
            //但是 C3200002 的功能特別另外用商家代號存了一筆資料，
            //所以這隻裡特別用商家代號查
            where = new Expression(LogTableView.Field.ReceiveType, QueryReceiveType);
            #endregion

            #region 功能 條件
            where.And(LogTableView.Field.FunctionId, _QueryFunctionId);
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(LogTableView.Field.LogDate, OrderByEnum.Desc);
            orderbys.Add(LogTableView.Field.LogTime, OrderByEnum.Desc);
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
            XmlResult xmlResult = base.QueryDataAndBind<LogTableView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + xmlResult.Message);
            }

            this.divResult.Visible = true;
            bool showPaging = this.gvResult.Rows.Count > 0;
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = showPaging;
            }

            return xmlResult;
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼、學年或學期參數");
                this.ShowSystemMessage(msg);
                return;
            }

            this.lbReceiveType.Text = receiveType;
            QueryReceiveType = receiveType;
        }

        private void BindData()
        {
            bool isOK = this.GetAndKeepQueryCondition();
            if (isOK)
            {
                PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
                if (!xmlResult.IsSuccess)
                {
                    //[TODO] 變動顯示訊息怎麼多語系
                    this.ShowSystemMessage(xmlResult.Message);
                }
            }
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            LogonUser logonUser = this.GetLogonUser();
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
                this.BindData();
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            LogTableView[] datas = this.gvResult.DataSource as LogTableView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                LogTableView data = datas[row.RowIndex];

                row.Cells[0].Text = String.Format("{0}-{1}", RoleCodeTexts.GetText(data.Role), data.UserId);
                row.Cells[1].Text = String.Format("{0}-{1}", data.LogDate, data.LogTime);
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}