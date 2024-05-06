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

namespace eSchoolWeb.D
{
    public partial class D1300007 : PagingBasePage
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
            where.And(DormStandardEntity.Field.ReceiveType, this.QueryReceiveType);
            where.And(DormStandardEntity.Field.YearId, this.QueryYearID);
            where.And(DormStandardEntity.Field.TermId, this.QueryTermID);
            where.And(DormStandardEntity.Field.DepId, this.QueryDepID);
            where.And(DormStandardEntity.Field.ReceiveId, this.QueryReceiveID);


            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(DormStandardEntity.Field.DormId, OrderByEnum.Asc);
            return new XmlResult(true);
        }

        /// <summary>
        /// 呼叫 QueryDataAndBind 方法
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <returns>傳回處理結果</returns>
        protected override XmlResult CallQueryDataAndBind(PagingInfo pagingInfo)
        {
            #region 檢查查詢權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無查詢權限");
                this.ShowJsAlert(msg);
                return null;
            }
            #endregion

            Paging[] ucPagings = this.GetPagingControls();
            XmlResult xmlResult = base.QueryDataAndBind<DormStandardEntity>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + xmlResult.Message);
            }
            return xmlResult;
        }
        #endregion

        #region Property
        /// <summary>
        /// 儲存業務別碼代碼的查詢條件
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

        /// <summary>
        /// 儲存學年代碼的查詢條件
        /// </summary>
        private string QueryYearID
        {
            get
            {
                return ViewState["QueryYearID"] as string;
            }
            set
            {
                ViewState["QueryYearID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存學期代碼的查詢條件
        /// </summary>
        private string QueryTermID
        {
            get
            {
                return ViewState["QueryTermID"] as string;
            }
            set
            {
                ViewState["QueryTermID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存部門別代碼的查詢條件
        /// </summary>
        private string QueryDepID
        {
            get
            {
                return ViewState["QueryDepID"] as string;
            }
            set
            {
                ViewState["QueryDepID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存收費別代碼的查詢條件
        /// </summary>
        private string QueryReceiveID
        {
            get
            {
                return ViewState["QueryReceiveID"] as string;
            }
            set
            {
                ViewState["QueryReceiveID"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                PagingInfo pagingInfo = new PagingInfo();
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 處理五個下拉選項
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string receiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out receiveID)
                || String.IsNullOrEmpty(receiveType)
                || String.IsNullOrEmpty(yearID))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得業務別碼或學年參數");
                this.ShowJsAlert(msg);
                return;
            }

            //因為控制項的 Page_Load 比頁面的 Page_Load 晚觸發，所以可以先執行 GetDataAndBind 方法，強迫 ucFilter1 結繫資料
            //因為 ucFilter1 有指定 Filter2ControlID 為 ucFilter2，所以 ucFilter2 頁會被自動結繫資料
            XmlResult xmlResult = this.ucFilter1.GetDataAndBind(receiveType, yearID, termID, depID, receiveID);
            if (xmlResult.IsSuccess)
            {
                //depID = ucFilter2.SelectedDepID;
                receiveID = ucFilter2.SelectedReceiveID;
            }

            //一定要用這個方法將業務別碼、學年、學期、部別、代收費用別參數傳給下一頁
            //否則下一頁的 Filter1 與 Filter2 無法正確自動取資料並結繫
            WebHelper.SetFilterArguments(receiveType, yearID, termID, depID, receiveID);
            #endregion

            this.QueryReceiveType = receiveType;
            this.QueryYearID = yearID;
            this.QueryTermID = termID;
            this.QueryDepID = depID;
            this.QueryReceiveID = receiveID;

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetLocalized("該業務別碼未授權");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                this.ShowJsAlert(this.GetLocalized("無維護權限"));
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
            string[] args = argument.Split(new char[] { ',' });
            if (args.Length != 6)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string receiveType = args[0].Trim();
            string yearId = args[1].Trim();
            string termId = args[2].Trim();
            string depId = args[3].Trim();
            string receiveId = args[4].Trim();
            string dormId = args[5].Trim();
            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", "M");
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("YearId", yearId);
                        QueryString.Add("TermId", termId);
                        QueryString.Add("DepId", depId);
                        QueryString.Add("ReceiveId", receiveId);
                        QueryString.Add("DormId", dormId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("D1300007M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        #region 刪除條件
                        DormStandardEntity entity = new DormStandardEntity();
                        entity.ReceiveType = receiveType;
                        entity.YearId = yearId;
                        entity.TermId = termId;
                        entity.DepId = depId;
                        entity.ReceiveId = receiveId;
                        entity.DormId = dormId;
                        #endregion

                        int count = 0;
                        XmlResult result = DataProxy.Current.Delete<DormStandardEntity>(this, entity, out count);
                        if (result.IsSuccess)
                        {
                            if (count == 0)
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("刪除資料失敗，無資料被刪除");
                                this.ShowSystemMessage(msg);
                            }
                            else
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("刪除資料成功");
                                this.ShowSystemMessage(msg);
                                this.CallQueryDataAndBind(this.ucPaging1.GetPagingInfo());
                            }
                        }
                        else
                        {
                            //[TODO] 變動顯示訊息怎麼多語系
                            this.ShowSystemMessage(this.GetLocalized("刪除資料失敗") + "，" + result.Message);
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 取得院別名稱
        /// </summary>
        protected DormListEntity[] getAllDatas()
        {
            DormListEntity[] datas = null;
            Expression where = new Expression();
            where.And(DormListEntity.Field.ReceiveType, this.QueryReceiveType);
            where.And(DormListEntity.Field.YearId, this.QueryYearID);
            where.And(DormListEntity.Field.TermId, this.QueryTermID);
            where.And(DormListEntity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(DormListEntity.Field.DormId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<DormListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 取得住宿收費代碼名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetDormText(DormListEntity[] allColleges, string code, string defaultText)
        {
            if (allColleges != null && code != null && code.Length > 0)
            {
                foreach (DormListEntity data in allColleges)
                {
                    if (data.DormId == code)
                    {
                        return data.DormName;
                    }
                }
            }
            return defaultText;
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            //院別名稱
            DormListEntity[] allDormList = getAllDatas();


            DormStandardEntity[] datas = this.gvResult.DataSource as DormStandardEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                DormStandardEntity data = datas[row.RowIndex];

                //顯示院別名稱
                row.Cells[1].Text = GetDormText(allDormList, data.DormId, string.Empty);


                //資料參數
                string argument = String.Format("{0},{1},{2},{3},{4},{5}", data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId, data.DormId);

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

        protected void ucFilter2_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            //this.QueryDepID = ucFilter2.SelectedDepID;
            this.QueryReceiveID = ucFilter2.SelectedReceiveID;
            WebHelper.SetFilterArguments(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, this.QueryDepID, this.QueryReceiveID);

            PagingInfo pagingInfo = new PagingInfo();
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
        }

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            //WebHelper.SetFilterArguments(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, this.QueryDepID, string.Empty);

            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", "A");
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            QueryString.Add("TermId", this.QueryTermID);
            QueryString.Add("DepId", this.QueryDepID);
            QueryString.Add("ReceiveId", this.QueryReceiveID);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1300007M.aspx");
        }
    }
}