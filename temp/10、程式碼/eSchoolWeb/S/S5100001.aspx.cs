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
    /// 學校資料管理
    /// </summary>
    public partial class S5100001 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的業務別碼代碼
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
        /// 儲存查詢的資料類別
        /// </summary>
        private string QuerySchName
        {
            get
            {
                return ViewState["QuerySchName"] as string;
            }
            set
            {
                ViewState["QuerySchName"] = value == null ? null : value.Trim();
            }
        }

        ///// <summary>
        ///// 儲存查詢的縣市 
        ///// </summary>
        //private string QueryCity
        //{
        //    get
        //    {
        //        return ViewState["QueryCity"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QueryCity"] = value == null ? null : value.Trim();
        //    }
        //}

        ///// <summary>
        ///// 儲存查詢的校長姓名 
        ///// </summary>
        //private string QuerySchPrincipal
        //{
        //    get
        //    {
        //        return ViewState["QuerySchPrincipal"] as string;
        //    }
        //    set
        //    {
        //        ViewState["QuerySchPrincipal"] = value == null ? null : value.Trim();
        //    }
        //}

        /// <summary>
        /// 儲存查詢的統一編號
        /// </summary>
        private string QuerySchIdenty
        {
            get
            {
                return ViewState["QuerySchIdenty"] as string;
            }
            set
            {
                ViewState["QuerySchIdenty"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學制
        /// </summary>
        private string QueryCorpType
        {
            get
            {
                return ViewState["QueryCorpType"] as string;
            }
            set
            {
                ViewState["QueryCorpType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的主辦分行
        /// </summary>
        private string QueryBankId
        {
            get
            {
                return ViewState["QueryBankId"] as string;
            }
            set
            {
                ViewState["QueryBankId"] = value == null ? null : value.Trim();
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
            where = new Expression();
            orderbys = null;

            LogonUser logonUser = WebHelper.GetLogonUser();
            if (logonUser.IsBankUser)
            {
                if (!logonUser.IsBankManager)
                {
                    where.And(SchoolRTypeEntity.Field.BankId, logonUser.UnitId);
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                where.And(SchoolRTypeEntity.Field.SchIdenty, logonUser.UnitId);
                if (!logonUser.IsSchoolManager)
                {
                    where.And(SchoolRTypeEntity.Field.ReceiveType, logonUser.MyReceiveTypes);
                }
            }
            else
            {
                return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            }

            #region 商家代號 條件
            {
                string qReceiveType = this.QueryReceiveType;
                if (!String.IsNullOrEmpty(qReceiveType))
                {
                    where.And(SchoolRTypeEntity.Field.ReceiveType, RelationEnum.Equal, qReceiveType );
                }
            }
            #endregion

            #region 學校名稱 條件
            {
                string qSchName = this.QuerySchName;
                if (!String.IsNullOrEmpty(qSchName))
                {
                    where.And(SchoolRTypeEntity.Field.SchName, RelationEnum.Like, "%" + qSchName + "%");
                }
            }
            #endregion

            #region 學校代號 條件
            {
                string qSchIdenty = this.QuerySchIdenty;
                if (!String.IsNullOrEmpty(qSchIdenty))
                {
                    where.And(SchoolRTypeEntity.Field.SchIdenty, RelationEnum.Equal, qSchIdenty);
                }
            }
            #endregion

            #region 學制 條件
            {
                string qCorpType = this.QueryCorpType;
                if (!String.IsNullOrEmpty(qCorpType))
                {
                    where.And(SchoolRTypeEntity.Field.CorpType, RelationEnum.Equal, qCorpType );
                }
            }
            #endregion

            #region 主辦分行 條件
            {
                string qBankId = this.QueryBankId;
                if (!String.IsNullOrEmpty(qBankId))
                {
                    where.And(SchoolRTypeEntity.Field.BankId, RelationEnum.Equal, qBankId);
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);
            orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<SchoolRTypeEntity>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }

            //因為有新增與回上一頁的按鈕，所以無資料時不適合把 gvResult 隱藏
            //this.divResult.Visible = this.gvResult.Rows.Count > 0;
            bool showPaging = this.gvResult.Rows.Count > 0;
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = showPaging;
            }

            return xmlResult;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

                #region 檢查查詢權限
                if (!this.HasQueryAuth())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無查詢權限");
                    this.ShowJsAlert(msg);
                    return;
                }
                #endregion

                LogonUser logonUser = WebHelper.GetLogonUser();
                if (logonUser.IsSchoolUser)
                {
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
                }
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            this.GetAndBindReceiveTypeOptions();
            this.GetAndBindBankOptions();
            this.GetAndBindCorpTypeOptions();

            #region 查詢結果初始化
            {
                ////因為有新增與回上一頁的按鈕，所以不適合把 divResult 隱藏
                ////改為隱藏分頁按鈕，並結繫 null
                ////this.divResult.Visible = false;
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

        private void GetAndBindCorpTypeOptions()
        {
            CodeText[] items = new CodeText[5];

            List<CodeText> datas = new List<CodeText>();
            CodeText item = null;

            item = new CodeText();
            item.Code = "1";
            item.Text = "大專院校";
            datas.Add(item);

            item = new CodeText();
            item.Code = "2";
            item.Text = "高中職";
            datas.Add(item);

            item = new CodeText();
            item.Code = "3";
            item.Text = "國中小";
            datas.Add(item);

            item = new CodeText();
            item.Code = "4";
            item.Text = "幼兒園";
            datas.Add(item);

            items = datas.ToArray();
            WebHelper.SetDropDownListItems(this.ddlCorpType, DefaultItem.Kind.Select, false, items, false, false, 0, null);

        }

        /// <summary>
        /// 取得並結繫業務別碼選項
        /// </summary>
        private void GetAndBindReceiveTypeOptions()
        {
            CodeText[] items = null;

            #region [Old] 因為學校使用者可跨分商家代號且會變動，所以此方法不適用
            //XmlResult xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTextsBySchool(this, out items);
            #endregion

            XmlResult xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTexts(this, out items);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢所屬學校的業務別資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            WebHelper.SetDropDownListItems(this.ddlReceiveType, DefaultItem.Kind.Select, false, items, true, false, 0, null);
        }

        /// <summary>
        /// 取得並結繫銀行代碼選項
        /// </summary>
        private void GetAndBindBankOptions()
        {
            CodeText[] items = null;
            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(BankEntity.Field.BankNo, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { BankEntity.Field.BankNo };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { BankEntity.Field.BankSName };
            string textCombineFormat = null;

            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BankEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
            if (!xmlResult.IsSuccess)
            {
                string action = this.GetLocalized("查詢銀行代碼資料");
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            if (xmlResult.IsSuccess)
            {
                if (items != null)
                {
                    for (int idx = 0; idx < items.Length; idx++)
                    {
                        items[idx].Text = string.Format("{0}({1})", items[idx].Text, items[idx].Code);
                    }
                }
            }
            WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.Select, false, items, false, false, 0, null);
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private XmlResult GetAndKeepQueryCondition()
        {
            #region 業務別碼
            this.QueryReceiveType = this.ddlReceiveType.SelectedValue;
            #endregion

            #region 學校名稱
            this.QuerySchName = this.tbxSchName.Text.Trim();
            #endregion

            #region 縣市
            //this.QueryCity = this.ddlCity.SelectedValue;
            #endregion

            #region 校長姓名
            //this.QuerySchPrincipal = this.tbxSchPrincipal.Text.Trim();
            #endregion

            #region 學校代碼
            this.QuerySchIdenty = this.tbxSchIdenty.Text.Trim();
            #endregion

            #region 學制
            this.QueryCorpType = this.ddlCorpType.SelectedValue;
            #endregion

            #region 主辦分行
            this.QueryBankId = this.ddlBank.SelectedValue;
            #endregion

            return new XmlResult(true);
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無維護權限");
                this.ShowJsAlert(msg);
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
            string[] args = argument.Split(new char[] { ',' }, StringSplitOptions.None);
            if (args.Length != 1)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string receiveType = args[0];
            #endregion

            string editUrl = "S5100001M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Insert:
                    #region 新增
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Insert);
                        QueryString.Add("ReceiveType", receiveType); //新增代收類別
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("ReceiveType", receiveType);
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
                        //要增加刪除的檢查，如果有費用別了就不能刪除
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("ReceiveType", receiveType);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion
                    }
                    #endregion
                    break;
                case "ViewD0071":
                    #region 查詢商家代號的 D00I71 資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.View);
                        QueryString.Add("ReceiveType", receiveType);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5100001D.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            SchoolRTypeEntity[] datas = this.gvResult.DataSource as SchoolRTypeEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            string mainKey = String.Empty;
            GridViewRow mainRow = null;
            int[] spanCellIndexs = new int[] { 0, 1, 2, 3 };
            foreach (GridViewRow row in this.gvResult.Rows)
            {
                SchoolRTypeEntity data = datas[row.RowIndex];
                //資料參數
                //string argument = String.Format("{0}", data.ReceiveType);
                string argument = data.ReceiveType.Trim();

                string sch_type = SchoolRTypeEntity.GetCorpTypeText(data.CorpType);

                row.Cells[2].Text = sch_type;
                row.Cells[6].Text = data.SchContract + "\r\n" + data.SchConTel;

                MyInsertButton ccbtnAdd = row.FindControl("ccbtnAdd") as MyInsertButton;
                if (ccbtnAdd != null)
                {
                    ccbtnAdd.CommandArgument = String.Format("{0}", data.SchIdenty); //這裡的新增，是同一所學校新增新的代收類別
                }

                LinkButton lbtnViewD0071 = row.FindControl("lbtnViewD0071") as LinkButton;
                if (lbtnViewD0071 != null)
                {
                    lbtnViewD0071.Text = data.ReceiveType;
                    lbtnViewD0071.CommandArgument = argument;
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

                #region
                if (mainKey != data.SchIdenty)
                {
                    mainKey = data.SchIdenty;
                    mainRow = row;
                }
                else
                {
                    foreach (int idx in spanCellIndexs)
                    {
                        row.Cells[idx].Visible = false;
                        if (mainRow.Cells[idx].RowSpan == 0)
                        {
                            mainRow.Cells[idx].RowSpan = 2;
                        }
                        else
                        {
                            mainRow.Cells[idx].RowSpan++;
                        }
                    }
                }
                #endregion
            }
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
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }

        protected void ccbtnInsert_Click(object sender, EventArgs e)
        {
            KeyValueList<string> QueryString = new KeyValueList<string>();
            QueryString.Add("Action", ActionMode.Insert);
            Session["QueryString"] = QueryString;
            Response.Redirect("S5100001M.aspx");
        }

    }
}