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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 使用者操作記錄查詢
    /// </summary>
    public partial class S5400001 : PagingBasePage
    {
        #region Property
        /// <summary>
        /// 儲存查詢的單位類別
        /// </summary>
        private string QueryUserQual
        {
            get
            {
                return ViewState["QueryUserQual"] as string;
            }
            set
            {
                ViewState["QueryUserQual"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的使用者單位代碼
        /// </summary>
        private string QueryUserUnitId
        {
            get
            {
                return ViewState["QueryUserUnitId"] as string;
            }
            set
            {
                ViewState["QueryUserUnitId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的使用者帳號
        /// </summary>
        private string QueryUserId
        {
            get
            {
                return ViewState["QueryUserId"] as string;
            }
            set
            {
                ViewState["QueryUserId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的日誌日期區間起日
        /// </summary>
        private string QuerySDate
        {
            get
            {
                return ViewState["QuerySDate"] as string;
            }
            set
            {
                ViewState["QuerySDate"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的日誌日期區間迄日
        /// </summary>
        private string QueryEDate
        {
            get
            {
                return ViewState["QueryEDate"] as string;
            }
            set
            {
                ViewState["QueryEDate"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的功能
        /// </summary>
        private string QueryFunctionId
        {
            get
            {
                return ViewState["QueryFunctionId"] as string;
            }
            set
            {
                ViewState["QueryFunctionId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的操作
        /// </summary>
        private string QueryLogType
        {
            get
            {
                return ViewState["QueryLogType"] as string;
            }
            set
            {
                ViewState["QueryLogType"] = value == null ? null : value.Trim();
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

            #region 視為操作的的資料邏輯
            //1. CHECK_LOGON 不是功能不撈
            //2. LOGON 的登入交易也視為操作
            //3. 其他則 LogType 不為 E 才視為操作
            #endregion

            Expression w2 = new Expression(LogTableView.Field.LogType, "E")
                .And(LogTableView.Field.FunctionId, "LOGON");
            Expression w3 = new Expression(LogTableView.Field.LogType, RelationEnum.NotEqual, "E")
                .And(LogTableView.Field.FunctionId, RelationEnum.NotEqual, "LOGON");

            where = new Expression(LogTableView.Field.FunctionId, RelationEnum.NotEqual, "CHECK_LOGON")
                .And(w2.Or(w3));

            #region 單位類別 條件
            string qUserQual = this.QueryUserQual;
            switch (qUserQual)
            {
                case UserQualCodeTexts.BANK:
                case UserQualCodeTexts.SCHOOL:
                    where.And(LogTableView.Field.Role, qUserQual);
                    break;
                default:
                    return new XmlResult(false, "單位類別查詢條件錯誤");
            }
            #endregion

            #region 使用者單位代碼 條件
            if (!String.IsNullOrEmpty(this.QueryUserUnitId))
            {
                where.And(LogTableView.Field.ReceiveType, this.QueryUserUnitId);
            }
            #endregion

            #region 使用者帳號 條件
            if (!String.IsNullOrEmpty(this.QueryUserId))
            {
                where.And(LogTableView.Field.UserId, this.QueryUserId);
            }
            #endregion

            #region 日期區間 條件
            if (!String.IsNullOrEmpty(this.QuerySDate))
            {
                where.And(LogTableView.Field.LogDate, RelationEnum.GreaterEqual, this.QuerySDate);
            }
            if (!String.IsNullOrEmpty(this.QueryEDate))
            {
                where.And(LogTableView.Field.LogDate, RelationEnum.LessEqual, this.QueryEDate);
            }
            #endregion

            #region 功能 條件
            if (!String.IsNullOrEmpty(this.QueryFunctionId))
            {
                where.And(LogTableView.Field.FunctionId, this.QueryFunctionId);
            }
            #endregion

            #region 操作 條件
            if (!String.IsNullOrEmpty(this.QueryLogType))
            {
                where.And(LogTableView.Field.LogType, this.QueryLogType);
            }
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
        private bool InitialUI()
        {
            LogonUser logonUser = this.GetLogonUser();

            #region ddlUserQual
            {
                if (logonUser.IsBankUser)
                {
                    this.trUserQual.Visible = true;
                    CodeText[] items = new CodeText[] { new CodeText(UserQualCodeTexts.BANK, UserQualCodeTexts.BANK_TEXT), new CodeText(UserQualCodeTexts.SCHOOL, UserQualCodeTexts.SCHOOL_TEXT) };
                    WebHelper.SetDropDownListItems(this.ddlUserQual, DefaultItem.Kind.None, false, items, false, true, 0, items[0].Code);
                }
                else
                {
                    this.trUserQual.Visible = false;
                }
            }
            #endregion

            #region ddlBank
            {
                this.ddlBank.Items.Clear();
                if (logonUser.IsBankManager)
                {
                    this.trBank.Visible = true;
                    this.GetAndBindBankOption(logonUser);
                }
                else
                {
                    this.trBank.Visible = false;
                }
            }
            #endregion

            #region ddlSchIdenty
            {
                this.ddlSchIdenty.Items.Clear();
                if (logonUser.IsBankUser)
                {
                    this.GetAndBindSchIdentyOption(logonUser, null, null);
                    if (this.ddlUserQual.SelectedValue == UserQualCodeTexts.SCHOOL)
                    {
                        this.trSchIdenty.Visible = true;
                    }
                    else
                    {
                        this.trSchIdenty.Visible = false;
                    }
                }
                else
                {
                    this.trSchIdenty.Visible = false;
                }
            }
            #endregion

            #region ddlFunction
            {
                this.GetAndBindFunctionOption();
            }
            #endregion

            #region ddlLogType
            {
                //LogType 不為 E 才視為操作，所以要去掉 E
                CodeTextList items = new LogTypeCodeTexts();
                items.RemoveAt(items.CodeIndexOf(LogTypeCodeTexts.EXECUTE));
                WebHelper.SetDropDownListItems(this.ddlLogType, DefaultItem.Kind.All, false, items, true, true, 0, null);
            }
            #endregion

            #region 查詢結果初始化
            {
                this.divResult.Visible = false;
                this.gvResult.DataSource = null;
                this.gvResult.DataBind();
                Paging[] ucPagings = this.GetPagingControls();
                foreach (Paging ucPaging in ucPagings)
                {
                    ucPaging.Visible = false;
                }
            }
            #endregion

            return true;
        }

        private void GetAndBindBankOption(LogonUser logonUser)
        {
            Expression where = null;
            if (logonUser.IsBankManager)
            {
                where = new Expression();
            }
            else if (logonUser.IsBankUser)
            {
                where = new Expression(BankEntity.Field.BankNo, logonUser.BankId);
            }

            CodeText[] datas = null;
            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(BankEntity.Field.BankNo, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { BankEntity.Field.BankNo };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { BankEntity.Field.BankSName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BankEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "無法取得分行選項資料");
                }
            }
            WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.None, false, datas, true, false, 0, null);
        }

        private void GetAndBindSchIdentyOption(LogonUser logonUser, string bankId, string selectedValue)
        {
            Expression where = null;

            if (logonUser.IsBankManager)
            {
                //銀行管理者可以查指定分行的學校
                if (!String.IsNullOrEmpty(bankId))
                {
                    if (bankId.Length == 7)
                    {
                        bankId = bankId.Substring(0, 6);
                    }
                    else if (bankId.Length == 3)
                    {
                        bankId = DataFormat.MyBankID + bankId;
                    }
                    where = new Expression(SchoolRTypeEntity.Field.BankId, bankId);
                }
                else
                {
                    where = new Expression();
                }
            }
            else if (logonUser.IsBankUser)
            {
                bankId = logonUser.BankId;
                if (bankId.Length == 7)
                {
                    bankId = bankId.Substring(0, 6);
                }
                else if (bankId.Length == 3)
                {
                    bankId = DataFormat.MyBankID + bankId;
                }
                //非銀行管理者只查自己分行的學校
                where = new Expression(SchoolRTypeEntity.Field.BankId, bankId);
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校只能查自己的學校
                where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUser.UnitId);
            }

            CodeText[] datas = null;
            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "無法取得學校選項資料");
                }
            }
            WebHelper.SetDropDownListItems(this.ddlSchIdenty, DefaultItem.Kind.None, false, datas, true, false, 0, selectedValue);
        }

        private void GetAndBindFunctionOption()
        {
            CodeTextList items = null;

            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(FuncMenuView2.Field.FuncId, OrderByEnum.Asc);

            FuncMenuView2[] datas = null;
            XmlResult xmlResult = DataProxy.Current.SelectAll<FuncMenuView2>(this.Page, where, orderbys, out datas);
            if (xmlResult.IsSuccess)
            {
                #region LOGON 的登入交易也視為操作
                CodeText[] others = new CodeText[1] { new CodeText("LOGON", "使用者登入") };
                items = new CodeTextList((datas == null ? 0 : datas.Length) + others.Length);
                items.AddRange(others);
                #endregion

                if (datas != null && datas.Length > 0)
                {
                    foreach (FuncMenuView2 data in datas)
                    {
                        items.Add(data.FuncId, data.FuncName);
                    }
                }
            }
            else
            {
                this.ShowErrorMessage(xmlResult.Code, "無法取得功能選項資料");
            }

            WebHelper.SetDropDownListItems(this.ddlFunction, DefaultItem.Kind.All, false, items, true, true, 0, null);
        }

        private void ChangeUIByUserQual(string userQual)
        {
            if (userQual == UserQualCodeTexts.BANK)
            {
                this.trBank.Visible = true;
                this.trSchIdenty.Visible = false;
            }
            else if (userQual == UserQualCodeTexts.SCHOOL)
            {
                this.trBank.Visible = false;
                this.trSchIdenty.Visible = true;
            }
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            LogonUser logonUser = this.GetLogonUser();

            #region ddlUserQual
            string qUserQual = null;
            if (this.trUserQual.Visible)
            {
                //一定是行員
                qUserQual = this.ddlUserQual.SelectedValue;
                if (String.IsNullOrEmpty(qUserQual))
                {
                    this.ShowMustInputAlert("單位類別");
                    return false;
                }
            }
            else
            {
                //一定是學校的使用者
                if (logonUser.IsSchoolUser)
                {
                    qUserQual = UserQualCodeTexts.SCHOOL;
                }
                else
                {
                    this.ShowMustInputAlert("單位類別");
                    return false;
                }
            }
            #endregion

            #region ddlBank
            string qBankNo = null;
            if (this.trBank.Visible)
            {
                qBankNo = this.ddlBank.SelectedValue;
                if (String.IsNullOrEmpty(qBankNo))
                {
                    this.ShowMustInputAlert("分行");
                    return false;
                }
            }
            else
            {
                //一定是非管理者的銀行使用者
                if (logonUser.IsBankUser)
                {
                    qBankNo = logonUser.BankId;
                }
                else
                {
                    this.ShowMustInputAlert("分行");
                    return false;
                }
            }
            #endregion

            #region ddlSchIdenty
            string qSchIdenty = null;
            if (qUserQual == UserQualCodeTexts.SCHOOL)
            {
                if (this.trSchIdenty.Visible)
                {
                    qSchIdenty = this.ddlSchIdenty.SelectedValue;
                    if (String.IsNullOrEmpty(qSchIdenty))
                    {
                        this.ShowMustInputAlert("學校");
                        return false;
                    }
                }
                else
                {
                    //一定是學校的使用者
                    if (logonUser.IsSchoolUser)
                    {
                        qSchIdenty = logonUser.UnitId;
                    }
                    else
                    {
                        this.ShowMustInputAlert("學校");
                        return false;
                    }
                }
            }
            else
            {
                qSchIdenty = null;
            }
            #endregion

            #region 查詢的代收日區間起日
            DateTime sDate = DateTime.MinValue;
            string qSDate = tbxDateS.Text.Trim();
            if (!String.IsNullOrEmpty(qSDate))
            {
                if (DateTime.TryParse(qSDate, out sDate))
                {
                    qSDate = Common.GetTWDate7(sDate);
                }
                else
                {
                    string msg = this.GetLocalized("日期區間的起日不是有效的日期格式");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 查詢的代收日區間迄日
            DateTime eDate = DateTime.MinValue;
            string qEDate = tbxDateE.Text.Trim();
            if (!String.IsNullOrEmpty(qEDate))
            {
                if (DateTime.TryParse(qEDate, out eDate))
                {
                    qEDate = Common.GetTWDate7(eDate);
                }
                else
                {
                    string msg = this.GetLocalized("日期區間的迄日不是有效的日期格式");
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            if (!String.IsNullOrEmpty(qSDate) && !String.IsNullOrEmpty(qEDate) && sDate > eDate)
            {
                string msg = this.GetLocalized("日期區間的起日不可以大於迄日");
                this.ShowSystemMessage(msg);
                return false;
            }

            this.QueryUserQual = qUserQual;
            switch (qUserQual)
            {
                case UserQualCodeTexts.BANK:
                    this.QueryUserUnitId = qBankNo;
                    break;
                case UserQualCodeTexts.SCHOOL:
                    this.QueryUserUnitId = qSchIdenty;
                    break;
                default:
                    this.QueryUserUnitId = null;
                    break;
            }

            this.QuerySDate = qSDate;
            this.QueryEDate = qEDate;

            //查詢的使用者帳號
            this.QueryUserId = this.tbxUserId.Text.Trim();

            this.QueryFunctionId = this.ddlFunction.SelectedValue;
            this.QueryLogType = this.ddlLogType.SelectedValue;

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ddlUserQual_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userQual = this.ddlUserQual.SelectedValue;
            this.ChangeUIByUserQual(userQual);
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

                row.Cells[0].Text = RoleCodeTexts.GetText(data.Role);
                row.Cells[1].Text = String.Format("{0}-{1}", data.ReceiveType, data.UserId);
                row.Cells[2].Text = String.Format("{0}-{1}", data.LogDate, data.LogTime);
                if (String.IsNullOrEmpty(data.FuncName))
                {
                    switch (data.FunctionId)
                    {
                        case "LOGON":
                            row.Cells[3].Text = "使用者登入";
                            break;
                        case "CHECK_LOGON":
                            row.Cells[3].Text = "驗證登入狀態";
                            break;
                    }
                }
                row.Cells[4].Text = LogTypeCodeTexts.GetText(data.LogType);
            }
        }

        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}