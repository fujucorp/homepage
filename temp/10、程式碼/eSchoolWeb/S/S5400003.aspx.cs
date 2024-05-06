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
    /// 排程作業查詢
    /// </summary>
    public partial class S5400003 : PagingBasePage
    {
        #region Property

        /// <summary>
        /// 儲存查詢的執行日期區間起日
        /// </summary>
        private DateTime? QuerySDate
        {
            get
            {
                return ViewState["QuerySDate"] as DateTime?;
            }
            set
            {
                ViewState["QuerySDate"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的執行日期區間迄日
        /// </summary>
        private  DateTime? QueryEDate
        {
            get
            {
                return ViewState["QueryEDate"] as DateTime?;
            }
            set
            {
                ViewState["QueryEDate"] = value;
            }
        }

        /// <summary>
        /// 儲存查詢的作業
        /// </summary>
        private string QueryJobCubeType
        {
            get
            {
                return ViewState["QueryJobCubeType"] as string;
            }
            set
            {
                ViewState["QueryJobCubeType"] = value == null ? null : value.Trim();
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

            where = new Expression();

            #region 執行日期區間 條件
            {
                DateTime? qSDate = this.QuerySDate;
                if (qSDate != null)
                {
                    where.And(JobcubeEntity.Field.Jstd, RelationEnum.GreaterEqual, qSDate.Value);
                }
                DateTime? qEDate = this.QueryEDate;
                if (qEDate != null)
                {
                    where.And(JobcubeEntity.Field.Jstd, RelationEnum.Less, qEDate.Value.AddDays(1));
                }
            }
            #endregion

            #region 作業 條件
            if (!String.IsNullOrEmpty(this.QueryJobCubeType))
            {
                where.And(JobcubeEntity.Field.Jtypeid, this.QueryJobCubeType);
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(JobcubeEntity.Field.Jstd, OrderByEnum.Desc);
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
            XmlResult xmlResult = base.QueryDataAndBind<JobcubeEntity>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + xmlResult.Message);
            }

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
        private bool InitialUI()
        {
            #region 服務名稱
            {
                #region [MDY:20160413] 排序
                CodeTextList items = JobCubeTypeCodeTexts.GetNonRealTimeJob();
                items.Sort(delegate(CodeText x, CodeText y)
                {
                    return x.Code.CompareTo(y.Code);
                });
                #endregion

                WebHelper.SetDropDownListItems(this.ddlJobCubeType, DefaultItem.Kind.Select, false, items, true, true, 0, null);
            }
            #endregion

            #region 查詢結果初始化
            {
                //this.divResult.Visible = false;
                //this.gvResult.DataSource = null;
                //this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private XmlResult GetAndKeepQueryCondition()
        {
            #region 查詢的執行日期區間起日
            DateTime? qSDate = null;
            {
                string txt = this.tbxDateS.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date))
                    {
                        qSDate = date;
                    }
                    else
                    {
                        return new XmlResult(false, this.GetLocalized("執行日期區間的起日不是合法的日期格式"));
                    }
                }
            }
            #endregion

            #region 查詢的執行日期區間迄日
            DateTime? qEDate = null;
            {
                string txt = this.tbxDateE.Text.Trim();
                if (!String.IsNullOrEmpty(txt))
                {
                    DateTime date;
                    if (DateTime.TryParse(txt, out date))
                    {
                        qEDate = date;
                    }
                    else
                    {
                        return new XmlResult(false, this.GetLocalized("執行日期區間的迄日不是合法的日期格式"));
                    }
                }
            }
            #endregion

            #region 查詢的作業名稱
            this.QueryJobCubeType = ddlJobCubeType.SelectedValue;
            #endregion

            this.QuerySDate = qSDate;
            this.QueryEDate = qEDate;
            return new XmlResult(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            JobcubeEntity[] datas = this.gvResult.DataSource as JobcubeEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                JobcubeEntity data = datas[row.RowIndex];

                row.Cells[0].Text = JobCubeTypeCodeTexts.GetText(data.Jtypeid);
                //row.Cells[1].Text = String.Format("{0}-{1}", data.jstd, data.UserId);
                //row.Cells[2].Text = String.Format("{0}-{1}", data.LogDate.ToString(), data.LogTime.ToString());
                //row.Cells[4].Text = LogTypeCodeTexts.GetText(data.LogType);
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ccbtnQuery_Click(object sender, EventArgs e)
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

            XmlResult xmlResult = this.GetAndKeepQueryCondition();
            if (xmlResult.IsSuccess)
            {
                PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
                xmlResult = this.CallQueryDataAndBind(pagingInfo);
            }
            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                this.ShowSystemMessage(xmlResult.Message);
            }
            else
            {
                this.gvResult.Visible = true;
            }
        }

        protected void ccbtnExport_Click(object sender, EventArgs e)
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

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            string extName = "XLS";
            {
                LinkButton control = sender as LinkButton;
                if (control.CommandArgument == "ODS")
                {
                    extName = "ODS";
                }
            }
            #endregion

            XmlResult xmlResult = this.GetAndKeepQueryCondition();
            if (xmlResult.IsSuccess)
            {
                Expression where = null;
                KeyValueList<OrderByEnum> orderbys = null;
                xmlResult = this.GetWhereAndOrderBys(out where, out orderbys);
                if (xmlResult.IsSuccess)
                {
                    string funcId = "S5400003";
                    byte[] fileContent = null;

                    #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                    xmlResult = DataProxy.Current.ExportQueryResult(this.Page, funcId, where, out fileContent, (extName == "ODS"));
                    #endregion

                    if (xmlResult.IsSuccess)
                    {
                        if (fileContent == null || fileContent.Length == 0)
                        {
                            this.ShowJsAlert("查無資料");
                        }
                        else
                        {
                            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                            string jobTypeName = JobCubeTypeCodeTexts.GetText(this.QueryJobCubeType);
                            string fileName = HttpUtility.HtmlEncode(String.Format("{0}排程作業查詢結果.{1}", (String.IsNullOrEmpty(jobTypeName) ? String.Empty : jobTypeName + "_"), extName));
                            this.ResponseFile(fileName, fileContent, extName);
                            #endregion
                        }
                    }
                }
            }

            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                string action = this.GetLocalized("匯出查詢結果");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }
    }
}