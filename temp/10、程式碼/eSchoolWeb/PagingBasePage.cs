using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Fuju.Web;

using Entities;

namespace eSchoolWeb
{
    /// <summary>
    /// 含分頁控制項的網頁基底抽象類別
    /// </summary>
    public abstract class PagingBasePage : BasePage
    {
        #region 繼承頁面要實作的抽象方法
        /// <summary>
        /// 取得頁面中的分頁控制項陣列
        /// </summary>
        /// <returns>傳回分頁控制項陣列或 null</returns>
        protected abstract Paging[] GetPagingControls();

        /// <summary>
        /// 取得查詢條件與排序方法
        /// </summary>
        /// <param name="where">成功則傳回查詢條件，否則應該傳回 null</param>
        /// <param name="orderbys">成功則傳回查詢條件，否則應該傳回 null</param>
        /// <returns>傳回處理結果</returns>
        protected abstract XmlResult GetWhereAndOrderBys(out Expression where, out KeyValueList<OrderByEnum> orderbys);

        /// <summary>
        /// 呼叫 QueryDataAndBind 方法
        /// </summary>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <returns>傳回處理結果</returns>
        protected abstract XmlResult CallQueryDataAndBind(PagingInfo pagingInfo);
        #endregion

        #region Override BasePage's Method
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Paging[] ucPagings = this.GetPagingControls();
            if (ucPagings != null && ucPagings.Length > 0)
            {
                foreach(Paging ucPaging in ucPagings)
                {
                    ucPaging.PagingChanged += new EventHandler<PagingEventArgs>(ucPaging_Change);
                }
            }
        }
        #endregion

        #region Method
        protected XmlResult QueryDataAndBind<T>(PagingInfo pagingInfo, Paging[] ucPagings,Expression where,KeyValueList<OrderByEnum> orderbys, GridView gvControl) where T : class, IEntity
        {
            int startIndex = pagingInfo.GetStartIndex();
            int maxRecords = pagingInfo.PageSize;

            T[] datas = null;
            int totalCount = 0;

            XmlResult xmlResult = DataProxy.Current.Select<T>(this.Page, where, orderbys, startIndex, maxRecords, out datas, out totalCount);

            if (xmlResult.IsSuccess && ucPagings != null && ucPagings.Length > 0)
            {
                pagingInfo = PagingInfo.Calculate(pagingInfo.PageSize, pagingInfo.PageNo, totalCount);
                foreach (Paging paging in ucPagings)
                {
                    paging.BindData(pagingInfo);
                }
            }

            if (gvControl != null)
            {
                this.BindData(gvControl, datas);
            }

            return xmlResult;
        }

        /// <summary>
        /// 取得指定分頁訊息的資料並結繫
        /// </summary>
        /// <typeparam name="T">指定資料的 Entity 型別</typeparam>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <param name="ucPagings">指定分頁控制項陣列</param>
        /// <param name="gvControl">指定結繫資料的 GridView 控制項</param>
        /// <returns></returns>
        protected XmlResult QueryDataAndBind<T>(PagingInfo pagingInfo, Paging[] ucPagings, GridView gvControl) where T : class, IEntity
        {
            int startIndex = pagingInfo.GetStartIndex();
            int maxRecords = pagingInfo.PageSize;

            T[] datas = null;
            int totalCount = 0;

            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;
            XmlResult xmlResult = this.GetWhereAndOrderBys(out where, out orderbys);
            if (xmlResult.IsSuccess)
            {
                xmlResult = DataProxy.Current.Select<T>(this.Page, where, orderbys, startIndex, maxRecords, out datas, out totalCount);

                if (xmlResult.IsSuccess && ucPagings != null && ucPagings.Length > 0)
                {
                    pagingInfo = PagingInfo.Calculate(pagingInfo.PageSize, pagingInfo.PageNo, totalCount);
                    foreach (Paging paging in ucPagings)
                    {
                        paging.BindData(pagingInfo);
                    }
                }
            }

            if (gvControl != null)
            {
                this.BindData(gvControl, datas);
            }

            return xmlResult;
        }

        /// <summary>
        /// 取得指定分頁訊息的資料並結繫並傳回符合條件的資料總筆數
        /// </summary>
        /// <typeparam name="T">指定資料的 Entity 型別</typeparam>
        /// <param name="pagingInfo">指定分頁訊息</param>
        /// <param name="ucPagings">指定分頁控制項陣列</param>
        /// <param name="gvControl">指定結繫資料的 GridView 控制項</param>
        /// <param name="totalCount">傳回符合條件的資料總筆數</param>
        /// <returns></returns>
        protected XmlResult QueryDataAndBind2<T>(PagingInfo pagingInfo, Paging[] ucPagings, GridView gvControl, out int totalCount) where T : class, IEntity
        {
            int startIndex = pagingInfo.GetStartIndex();
            int maxRecords = pagingInfo.PageSize;

            T[] datas = null;
            totalCount = 0;

            Expression where = null;
            KeyValueList<OrderByEnum> orderbys = null;
            XmlResult xmlResult = this.GetWhereAndOrderBys(out where, out orderbys);
            if (xmlResult.IsSuccess)
            {
                xmlResult = DataProxy.Current.Select<T>(this.Page, where, orderbys, startIndex, maxRecords, out datas, out totalCount);

                if (xmlResult.IsSuccess && ucPagings != null && ucPagings.Length > 0)
                {
                    pagingInfo = PagingInfo.Calculate(pagingInfo.PageSize, pagingInfo.PageNo, totalCount);
                    foreach (Paging paging in ucPagings)
                    {
                        paging.BindData(pagingInfo);
                    }
                }
            }

            if (gvControl != null)
            {
                this.BindData(gvControl, datas);
            }

            return xmlResult;
        }

        /// <summary>
        /// 結繫資料
        /// </summary>
        /// <param name="bindControl"></param>
        /// <param name="datas"></param>
        private void BindData(Control bindControl, object datas)
        {
            if (bindControl is GridView)
            {
                GridView ctl = (GridView)bindControl;
                ctl.DataSource = datas;
                ctl.DataBind();
                return;
            }
            if (bindControl is Repeater)
            {
                Repeater ctl = (Repeater)bindControl;
                ctl.DataSource = datas;
                ctl.DataBind();
                return;
            }
            if (bindControl is ListView)
            {
                ListView ctl = (ListView)bindControl;
                ctl.DataSource = datas;
                ctl.DataBind();
                return;
            }
        }

        /// <summary>
        /// 分頁控制項的 PagingChanged 事件處理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucPaging_Change(object sender, PagingEventArgs e)
        {
            Paging[] ucPagings = this.GetPagingControls();
            if (ucPagings != null && ucPagings.Length > 0)
            {
                PagingInfo pagingInfo = e.PagingInfo.Calculate(e.CommandName, e.CommandArgument);
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
            }
        }
        #endregion
    }
}