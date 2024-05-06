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
    public partial class D1300008 : PagingBasePage
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
            orderbys = new KeyValueList<OrderByEnum>(1);
            switch (ddlIdentifyType.SelectedValue)
            {
                case "1":
                    where = getIdentify1Where();
                    orderbys.Add(IdentifyStandard1Entity.Field.IdentifyId, OrderByEnum.Asc);
                    break;
                case "2":
                    where = getIdentify2Where();
                    orderbys.Add(IdentifyStandard2Entity.Field.IdentifyId, OrderByEnum.Asc);
                    break;
                case "3":
                    where = getIdentify3Where();
                    orderbys.Add(IdentifyStandard3Entity.Field.IdentifyId, OrderByEnum.Asc);
                    break;
                case "4":
                    where = getIdentify4Where();
                    orderbys.Add(IdentifyStandard4Entity.Field.IdentifyId, OrderByEnum.Asc);
                    break;
                case "5":
                    where = getIdentify5Where();
                    orderbys.Add(IdentifyStandard5Entity.Field.IdentifyId, OrderByEnum.Asc);
                    break;
                case "6":
                    where = getIdentify6Where();
                    orderbys.Add(IdentifyStandard6Entity.Field.IdentifyId, OrderByEnum.Asc);
                    break;
            }

            return new XmlResult(true);
        }

        protected Expression getIdentify1Where()
        {
            Expression where = new Expression();
            where.And(IdentifyStandard1Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyStandard1Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyStandard1Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyStandard1Entity.Field.DepId, this.QueryDepID);
            where.And(IdentifyStandard1Entity.Field.ReceiveId, this.QueryReceiveID);

            return where;
        }

        protected Expression getIdentify2Where()
        {
            Expression where = new Expression();
            where.And(IdentifyStandard2Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyStandard2Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyStandard2Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyStandard2Entity.Field.DepId, this.QueryDepID);
            where.And(IdentifyStandard2Entity.Field.ReceiveId, this.QueryReceiveID);

            return where;
        }

        protected Expression getIdentify3Where()
        {
            Expression where = new Expression();
            where.And(IdentifyStandard3Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyStandard3Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyStandard3Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyStandard3Entity.Field.DepId, this.QueryDepID);
            where.And(IdentifyStandard3Entity.Field.ReceiveId, this.QueryReceiveID);

            return where;
        }

        protected Expression getIdentify4Where()
        {
            Expression where = new Expression();
            where.And(IdentifyStandard4Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyStandard4Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyStandard4Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyStandard4Entity.Field.DepId, this.QueryDepID);
            where.And(IdentifyStandard4Entity.Field.ReceiveId, this.QueryReceiveID);

            return where;
        }

        protected Expression getIdentify5Where()
        {
            Expression where = new Expression();
            where.And(IdentifyStandard5Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyStandard5Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyStandard5Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyStandard5Entity.Field.DepId, this.QueryDepID);
            where.And(IdentifyStandard5Entity.Field.ReceiveId, this.QueryReceiveID);

            return where;
        }

        protected Expression getIdentify6Where()
        {
            Expression where = new Expression();
            where.And(IdentifyStandard6Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyStandard6Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyStandard6Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyStandard6Entity.Field.DepId, this.QueryDepID);
            where.And(IdentifyStandard6Entity.Field.ReceiveId, this.QueryReceiveID);

            return where;
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
            XmlResult xmlResult = null;

            switch (ddlIdentifyType.SelectedValue)
            {
                case "1":
                    xmlResult = base.QueryDataAndBind<IdentifyStandard1Entity>(pagingInfo, ucPagings, this.gvResult);
                    break;
                case "2":
                    xmlResult = base.QueryDataAndBind<IdentifyStandard2Entity>(pagingInfo, ucPagings, this.gvResult);
                    break;
                case "3":
                    xmlResult = base.QueryDataAndBind<IdentifyStandard3Entity>(pagingInfo, ucPagings, this.gvResult);
                    break;
                case "4":
                    xmlResult = base.QueryDataAndBind<IdentifyStandard4Entity>(pagingInfo, ucPagings, this.gvResult);
                    break;
                case "5":
                    xmlResult = base.QueryDataAndBind<IdentifyStandard5Entity>(pagingInfo, ucPagings, this.gvResult);
                    break;
                case "6":
                    xmlResult = base.QueryDataAndBind<IdentifyStandard6Entity>(pagingInfo, ucPagings, this.gvResult);
                    break;
            }

            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
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

        /// <summary>
        /// 儲存身分註記別的查詢條件
        /// </summary>
        private string QueryIdentifyID
        {
            get
            {
                return ViewState["QueryIdentifyID"] as string;
            }
            set
            {
                ViewState["QueryIdentifyID"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存身分註記別的查詢條件
        /// </summary>
        private string QueryIdentifyType
        {
            get
            {
                return ViewState["QueryIdentifyType"] as string;
            }
            set
            {
                ViewState["QueryIdentifyType"] = value == null ? null : value.Trim();
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
                depID = "";
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

            SetddlIdentifyTypeOptions();

            #region 檢查業務別碼授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetLocalized("該業務別碼未授權");
                this.ShowJsAlert(msg);
                return;
            }
            #endregion

            if (this.Request.QueryString["IdentifyType"] != null)
            {
                this.QueryIdentifyType = this.Request.QueryString["IdentifyType"].ToString();
            }
            else
            {
                this.QueryIdentifyType = "1";
            }

            #region [MDY:20210401] 原碼修正
            #region [OLD]
            //this.ddlIdentifyType.SelectedValue = this.QueryIdentifyType;
            #endregion

            WebHelper.SetDropDownListSelectedValue(this.ddlIdentifyType, this.QueryIdentifyType);
            #endregion
        }

        /// <summary>
        /// 設定身分註記代碼下接選單
        /// </summary>
        protected void SetddlIdentifyTypeOptions()
        {
            this.ddlIdentifyType.Items.Clear();

            List<ListItem> list = new List<ListItem>();
            for (int i = 1; i < 7; i++)
            {
                string IdentifyText = this.GetLocalized("身分註記") + i.ToString();
                string text = String.Format("{0}({1})", IdentifyText, i.ToString());
                list.Add(new ListItem(text, i.ToString()));
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlIdentifyType.Items.AddRange(items);
            }
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
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string[] args = argument.Split(new char[] { ',' });
            if (args.Length != 6)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            string receiveType = args[0].Trim();
            string yearId = args[1].Trim();
            string termId = args[2].Trim();
            string depId = args[3].Trim();
            string receiveId = args[4].Trim();
            string identifyId = args[5].Trim();
            string identifyType = ddlIdentifyType.SelectedValue.Trim();
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
                        QueryString.Add("IdentifyType", identifyType);
                        QueryString.Add("IdentifyId", identifyId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("D1300008M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        int count = 0;
                        XmlResult result = new XmlResult();
                        #region 6 個 IdentifyList
                        switch (ddlIdentifyType.SelectedValue)
                        {
                            case "1":
                                IdentifyStandard1Entity data1 = new IdentifyStandard1Entity();
                                data1.ReceiveType = receiveType;
                                data1.YearId = yearId;
                                data1.TermId = termId;
                                data1.DepId = depId;
                                data1.ReceiveId = receiveId;
                                data1.IdentifyId = identifyId;
                                result = DataProxy.Current.Delete<IdentifyStandard1Entity>(this, data1, out count);
                                break;
                            case "2":
                                IdentifyStandard2Entity data2 = new IdentifyStandard2Entity();
                                data2.ReceiveType = receiveType;
                                data2.YearId = yearId;
                                data2.TermId = termId;
                                data2.DepId = depId;
                                data2.ReceiveId = receiveId;
                                data2.IdentifyId = identifyId;
                                result = DataProxy.Current.Delete<IdentifyStandard2Entity>(this, data2, out count);
                                break;
                            case "3":
                                IdentifyStandard3Entity data3 = new IdentifyStandard3Entity();
                                data3.ReceiveType = receiveType;
                                data3.YearId = yearId;
                                data3.TermId = termId;
                                data3.DepId = depId;
                                data3.ReceiveId = receiveId;
                                data3.IdentifyId = identifyId;
                                result = DataProxy.Current.Delete<IdentifyStandard3Entity>(this, data3, out count);
                                break;
                            case "4":
                                IdentifyStandard4Entity data4 = new IdentifyStandard4Entity();
                                data4.ReceiveType = receiveType;
                                data4.YearId = yearId;
                                data4.TermId = termId;
                                data4.DepId = depId;
                                data4.ReceiveId = receiveId;
                                data4.IdentifyId = identifyId;
                                result = DataProxy.Current.Delete<IdentifyStandard4Entity>(this, data4, out count);
                                break;
                            case "5":
                                IdentifyStandard5Entity data5 = new IdentifyStandard5Entity();
                                data5.ReceiveType = receiveType;
                                data5.YearId = yearId;
                                data5.TermId = termId;
                                data5.DepId = depId;
                                data5.ReceiveId = receiveId;
                                data5.IdentifyId = identifyId;
                                result = DataProxy.Current.Delete<IdentifyStandard5Entity>(this, data5, out count);
                                break;
                            case "6":
                                IdentifyStandard6Entity data6 = new IdentifyStandard6Entity();
                                data6.ReceiveType = receiveType;
                                data6.YearId = yearId;
                                data6.TermId = termId;
                                data6.DepId = depId;
                                data6.ReceiveId = receiveId;
                                data6.IdentifyId = identifyId;
                                result = DataProxy.Current.Delete<IdentifyStandard6Entity>(this, data6, out count);
                                break;
                        }
                        #endregion

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

        #region 取得身分註記代碼1~6 資料
        /// <summary>
        /// 取得身分註記代碼1資料
        /// </summary>
        protected IdentifyList1Entity[] GetIdentify1Datas()
        {
            IdentifyList1Entity[] datas = null;
            Expression where = new Expression();
            where.And(IdentifyList1Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyList1Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyList1Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyList1Entity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(IdentifyList1Entity.Field.IdentifyId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<IdentifyList1Entity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 取得身分註記代碼2資料
        /// </summary>
        protected IdentifyList2Entity[] GetIdentify2Datas()
        {
            IdentifyList2Entity[] datas = null;
            Expression where = new Expression();
            where.And(IdentifyList2Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyList2Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyList2Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyList2Entity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(IdentifyList2Entity.Field.IdentifyId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<IdentifyList2Entity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 取得身分註記代碼3資料
        /// </summary>
        protected IdentifyList3Entity[] GetIdentify3Datas()
        {
            IdentifyList3Entity[] datas = null;
            Expression where = new Expression();
            where.And(IdentifyList3Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyList3Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyList3Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyList3Entity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(IdentifyList3Entity.Field.IdentifyId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<IdentifyList3Entity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 取得身分註記代碼4資料
        /// </summary>
        protected IdentifyList4Entity[] GetIdentify4Datas()
        {
            IdentifyList4Entity[] datas = null;
            Expression where = new Expression();
            where.And(IdentifyList4Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyList4Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyList4Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyList4Entity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(IdentifyList4Entity.Field.IdentifyId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<IdentifyList4Entity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 取得身分註記代碼5資料
        /// </summary>
        protected IdentifyList5Entity[] GetIdentify5Datas()
        {
            IdentifyList5Entity[] datas = null;
            Expression where = new Expression();
            where.And(IdentifyList5Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyList5Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyList5Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyList5Entity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(IdentifyList5Entity.Field.IdentifyId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<IdentifyList5Entity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }

        /// <summary>
        /// 取得身分註記代碼6資料
        /// </summary>
        protected IdentifyList6Entity[] GetIdentify6Datas()
        {
            IdentifyList6Entity[] datas = null;
            Expression where = new Expression();
            where.And(IdentifyList6Entity.Field.ReceiveType, this.QueryReceiveType);
            where.And(IdentifyList6Entity.Field.YearId, this.QueryYearID);
            where.And(IdentifyList6Entity.Field.TermId, this.QueryTermID);
            where.And(IdentifyList6Entity.Field.DepId, this.QueryDepID);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(IdentifyList6Entity.Field.IdentifyId, OrderByEnum.Asc);

            XmlResult result = DataProxy.Current.SelectAll<IdentifyList6Entity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                return datas;
            }

            return null;
        }
        #endregion

        #region 取得身分註記代碼1~6 名稱
        /// <summary>
        /// 取得身分註記代碼1名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetIdentify1Text(IdentifyList1Entity[] allDatas, string code, string defaultText)
        {
            if (allDatas != null && code != null && code.Length > 0)
            {
                foreach (IdentifyList1Entity data in allDatas)
                {
                    if (data.IdentifyId == code)
                    {
                        return data.IdentifyName;
                    }
                }
            }
            return defaultText;
        }

        /// <summary>
        /// 取得身分註記代碼2名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetIdentify2Text(IdentifyList2Entity[] allDatas, string code, string defaultText)
        {
            if (allDatas != null && code != null && code.Length > 0)
            {
                foreach (IdentifyList2Entity data in allDatas)
                {
                    if (data.IdentifyId == code)
                    {
                        return data.IdentifyName;
                    }
                }
            }
            return defaultText;
        }

        /// <summary>
        /// 取得身分註記代碼3名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetIdentify3Text(IdentifyList3Entity[] allDatas, string code, string defaultText)
        {
            if (allDatas != null && code != null && code.Length > 0)
            {
                foreach (IdentifyList3Entity data in allDatas)
                {
                    if (data.IdentifyId == code)
                    {
                        return data.IdentifyName;
                    }
                }
            }
            return defaultText;
        }

        /// <summary>
        /// 取得身分註記代碼4名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetIdentify4Text(IdentifyList4Entity[] allDatas, string code, string defaultText)
        {
            if (allDatas != null && code != null && code.Length > 0)
            {
                foreach (IdentifyList4Entity data in allDatas)
                {
                    if (data.IdentifyId == code)
                    {
                        return data.IdentifyName;
                    }
                }
            }
            return defaultText;
        }

        /// <summary>
        /// 取得身分註記代碼5名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetIdentify5Text(IdentifyList5Entity[] allDatas, string code, string defaultText)
        {
            if (allDatas != null && code != null && code.Length > 0)
            {
                foreach (IdentifyList5Entity data in allDatas)
                {
                    if (data.IdentifyId == code)
                    {
                        return data.IdentifyName;
                    }
                }
            }
            return defaultText;
        }

        /// <summary>
        /// 取得身分註記代碼6名稱
        /// </summary>
        /// <param name="code"></param>
        /// <param name="renew"></param>
        /// <returns></returns>
        public static string GetIdentify6Text(IdentifyList6Entity[] allDatas, string code, string defaultText)
        {
            if (allDatas != null && code != null && code.Length > 0)
            {
                foreach (IdentifyList6Entity data in allDatas)
                {
                    if (data.IdentifyId == code)
                    {
                        return data.IdentifyName;
                    }
                }
            }
            return defaultText;
        }
        #endregion

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            //身分註記代碼1
            IdentifyList1Entity[] IdentifyList1Datas = null;
            IdentifyList2Entity[] IdentifyList2Datas = null;
            IdentifyList3Entity[] IdentifyList3Datas = null;
            IdentifyList4Entity[] IdentifyList4Datas = null;
            IdentifyList5Entity[] IdentifyList5Datas = null;
            IdentifyList6Entity[] IdentifyList6Datas = null;

            IdentifyStandard1Entity[] datas1 = null;
            IdentifyStandard2Entity[] datas2 = null;
            IdentifyStandard3Entity[] datas3 = null;
            IdentifyStandard4Entity[] datas4 = null;
            IdentifyStandard5Entity[] datas5 = null;
            IdentifyStandard6Entity[] datas6 = null;

            switch (ddlIdentifyType.SelectedValue)
            {
                case "1":
                    IdentifyList1Datas = GetIdentify1Datas();
                    datas1 = this.gvResult.DataSource as IdentifyStandard1Entity[];
                    if (datas1 == null || datas1.Length == 0)
                    {
                        return;
                    }
                    break;
                case "2":
                    IdentifyList2Datas = GetIdentify2Datas();
                    datas2 = this.gvResult.DataSource as IdentifyStandard2Entity[];
                    if (datas2 == null || datas2.Length == 0)
                    {
                        return;
                    }
                    break;
                case "3":
                    IdentifyList3Datas = GetIdentify3Datas();
                    datas3 = this.gvResult.DataSource as IdentifyStandard3Entity[];
                    if (datas3 == null || datas3.Length == 0)
                    {
                        return;
                    }
                    break;
                case "4":
                    IdentifyList4Datas = GetIdentify4Datas();
                    datas4 = this.gvResult.DataSource as IdentifyStandard4Entity[];
                    if (datas4 == null || datas4.Length == 0)
                    {
                        return;
                    }
                    break;
                case "5":
                    IdentifyList5Datas = GetIdentify5Datas();
                    datas5 = this.gvResult.DataSource as IdentifyStandard5Entity[];
                    if (datas5 == null || datas5.Length == 0)
                    {
                        return;
                    }
                    break;
                case "6":
                    IdentifyList6Datas = GetIdentify6Datas();
                    datas6 = this.gvResult.DataSource as IdentifyStandard6Entity[];
                    if (datas6 == null || datas6.Length == 0)
                    {
                        return;
                    }
                    break;
            }

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                //資料參數
                string argument = string.Empty;

                //註記方式: 1 - 依百分比計算 2 - 依金額計算

                switch (ddlIdentifyType.SelectedValue)
                {
                    case "1":
                        IdentifyStandard1Entity data1 = datas1[row.RowIndex];
                        row.Cells[1].Text = GetIdentify1Text(IdentifyList1Datas, data1.IdentifyId, string.Empty);
                        switch (data1.IdWay)
                        {
                            case "1":
                                row.Cells[2].Text = "依百分比計算";
                                break;
                            case "2":
                                row.Cells[2].Text = "依金額計算";
                                break;
                        }
                        argument = String.Format("{0},{1},{2},{3},{4},{5}", data1.ReceiveType, data1.YearId, data1.TermId, data1.DepId, data1.ReceiveId, data1.IdentifyId);
                        break;
                    case "2":
                        IdentifyStandard2Entity data2 = datas2[row.RowIndex];
                        row.Cells[1].Text = GetIdentify2Text(IdentifyList2Datas, data2.IdentifyId, string.Empty);
                        switch (data2.IdWay)
                        {
                            case "1":
                                row.Cells[2].Text = "依百分比計算";
                                break;
                            case "2":
                                row.Cells[2].Text = "依金額計算";
                                break;
                        }
                        argument = String.Format("{0},{1},{2},{3},{4},{5}", data2.ReceiveType, data2.YearId, data2.TermId, data2.DepId, data2.ReceiveId, data2.IdentifyId);
                        break;
                    case "3":
                        IdentifyStandard3Entity data3 = datas3[row.RowIndex];
                        row.Cells[1].Text = GetIdentify3Text(IdentifyList3Datas, data3.IdentifyId, string.Empty);
                        switch (data3.IdWay)
                        {
                            case "1":
                                row.Cells[2].Text = "依百分比計算";
                                break;
                            case "2":
                                row.Cells[2].Text = "依金額計算";
                                break;
                        }
                        argument = String.Format("{0},{1},{2},{3},{4},{5}", data3.ReceiveType, data3.YearId, data3.TermId, data3.DepId, data3.ReceiveId, data3.IdentifyId);
                        break;
                    case "4":
                        IdentifyStandard4Entity data4 = datas4[row.RowIndex];
                        row.Cells[1].Text = GetIdentify4Text(IdentifyList4Datas, data4.IdentifyId, string.Empty);
                        switch (data4.IdWay)
                        {
                            case "1":
                                row.Cells[2].Text = "依百分比計算";
                                break;
                            case "2":
                                row.Cells[2].Text = "依金額計算";
                                break;
                        }
                        argument = String.Format("{0},{1},{2},{3},{4},{5}", data4.ReceiveType, data4.YearId, data4.TermId, data4.DepId, data4.ReceiveId, data4.IdentifyId);
                        break;
                    case "5":
                        IdentifyStandard5Entity data5 = datas5[row.RowIndex];
                        row.Cells[1].Text = GetIdentify5Text(IdentifyList5Datas, data5.IdentifyId, string.Empty);
                        switch (data5.IdWay)
                        {
                            case "1":
                                row.Cells[2].Text = "依百分比計算";
                                break;
                            case "2":
                                row.Cells[2].Text = "依金額計算";
                                break;
                        }
                        argument = String.Format("{0},{1},{2},{3},{4},{5}", data5.ReceiveType, data5.YearId, data5.TermId, data5.DepId, data5.ReceiveId, data5.IdentifyId);
                        break;
                    case "6":
                        IdentifyStandard6Entity data6 = datas6[row.RowIndex];
                        row.Cells[1].Text = GetIdentify6Text(IdentifyList6Datas, data6.IdentifyId, string.Empty);
                        switch (data6.IdWay)
                        {
                            case "1":
                                row.Cells[2].Text = "依百分比計算";
                                break;
                            case "2":
                                row.Cells[2].Text = "依金額計算";
                                break;
                        }
                        argument = String.Format("{0},{1},{2},{3},{4},{5}", data6.ReceiveType, data6.YearId, data6.TermId, data6.DepId, data6.ReceiveId, data6.IdentifyId);
                        break;
                }


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
            this.QueryDepID = "";
            this.QueryReceiveID = ucFilter2.SelectedReceiveID;
            WebHelper.SetFilterArguments(this.QueryReceiveType, this.QueryYearID, this.QueryTermID, this.QueryDepID, this.QueryReceiveID);

            PagingInfo pagingInfo = new PagingInfo();
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
        }

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", "A");
            QueryString.Add("ReceiveType", this.QueryReceiveType);
            QueryString.Add("YearId", this.QueryYearID);
            QueryString.Add("TermId", this.QueryTermID);
            QueryString.Add("DepId", this.QueryDepID);
            QueryString.Add("ReceiveId", this.QueryReceiveID);
            QueryString.Add("IdentifyType", ddlIdentifyType.SelectedValue);
            Session["QueryString"] = QueryString;

            Server.Transfer("D1300008M.aspx");
        }

        protected void ddlIdentifyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PagingInfo pagingInfo = new PagingInfo();
            XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);
        }
    }
}