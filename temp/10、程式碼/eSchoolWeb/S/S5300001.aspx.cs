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
    /// 使用者管理
    /// </summary>
    public partial class S5300001 : PagingBasePage
    {
        #region [權限邏輯：20161010] 依據【土銀學雜使用者帳號管理說明.doc】文件
        // 總行 (AD0、AD5、AD6)
        //   群組選項：除 AD0、AD1、AD2 以外的所有群組 (因為這些群組的使用者不存在 USERS 中)
        //   帳號查詢：所有行員的帳號 (除 AD0、AD1、AD2 群組的帳號) + 所有學校的帳號
        //   帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
        // 分行主控、會計主管
        //   群組選項：AD3、AD4 與自己分行的特定分行主管、經辦(含AD5、AD6)群組 + 自己分行的學校的群組
        //   帳號查詢：自己分行的帳號 + 自己分行的學校的帳號
        //   帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
        // 分行主管、經辦
        //   群組選項：自己分行的學校的群組
        //   帳號查詢：自己分行的學校的帳號
        //   帳號維護：自己分行的學校的主管帳號
        // 學校主管、經辦
        //   群組選項：自己學校的群組
        //   帳號查詢：自己學校的帳號
        //   帳號維護：自己學校的經辦帳號
        #endregion

        #region Property
        /// <summary>
        /// 儲存查詢的群組代碼
        /// </summary>
        private string QueryGroupId
        {
            get
            {
                return ViewState["QueryGroupId"] as string;
            }
            set
            {
                ViewState["QueryGroupId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的分行代碼
        /// </summary>
        private string QueryBankCode
        {
            get
            {
                return ViewState["QueryBankCode"] as string;
            }
            set
            {
                ViewState["QueryBankCode"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的學校代碼
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

        /// <summary>
        /// 儲存查詢的資料類別
        /// </summary>
        private string QueryDataType
        {
            get
            {
                return ViewState["QueryDataType"] as string;
            }
            set
            {
                ViewState["QueryDataType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存查詢的資料值
        /// </summary>
        private string QueryDataValue
        {
            get
            {
                return ViewState["QueryDataValue"] as string;
            }
            set
            {
                ViewState["QueryDataValue"] = value == null ? null : value.Trim();
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

            LogonUser logonUser = this.GetLogonUser();

            #region 基本條件
            #region [Old]
            //if (logonUser.IsBankManager)
            //{
            //    Expression w1 = new Expression(UsersFlowView.Field.URt, String.Empty) //行員
            //        .And(UsersFlowView.Field.UBank, RelationEnum.Like, DataFormat.MyBankID + "%");
            //    Expression w2 = new Expression(UsersFlowView.Field.URt, RelationEnum.NotEqual, String.Empty); //學校
            //    where = new Expression().And(w1.Or(w2));
            //}
            //else if (logonUser.IsBankUser)
            //{
            //    Expression w1 = new Expression(UsersFlowView.Field.URt, String.Empty) //行員
            //        .And(UsersFlowView.Field.UBank, logonUser.BankId);    //自己分行
            //    Expression w2 = new Expression(UsersFlowView.Field.URt, RelationEnum.NotEqual, String.Empty) //學校
            //        .And(UsersFlowView.Field.UBank, logonUser.MySchIdentys);  //自己分行學校
            //    where = new Expression().And(w1.Or(w2));
            //}
            //else if (logonUser.IsSchoolUser)
            //{
            //    where = new Expression(UsersFlowView.Field.UBank, logonUser.UnitId);
            //}
            //else
            //{
            //    return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            //}
            #endregion

            #region [MDY:20161010] 依據權限邏輯修正基本條件
            if (logonUser.IsBankManager)
            {
                //總行 (AD0、AD5、AD6) -帳號查詢：所有行員的帳號 (除 AD0、AD1、AD2 群組的帳號) + 所有學校的帳號
                where = new Expression(UsersFlowView.Field.GroupRole, RelationEnum.NotIn, BankADGroupCodeTexts.GetInADCodes());
            }
            else if (logonUser.IsBankUser)
            {
                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管 - 帳號查詢：自己分行的帳號 + 自己分行的學校的帳號
                    Expression w1 = new Expression(UsersFlowView.Field.GroupRole, RoleCodeTexts.STAFF)      //行員
                        .And(UsersFlowView.Field.UBank, logonUser.BankId);                                  //自己分行
                    Expression w2 = new Expression(UsersFlowView.Field.GroupRole, RoleCodeTexts.SCHOOL)     //學校
                        .And(UsersFlowView.Field.UBank, logonUser.MySchIdentys);                            //自己分行學校
                    where = new Expression().And(w1.Or(w2));
                }
                else
                {
                    //分行主管、經辦 - 帳號查詢：自己分行的學校的帳號
                    where = new Expression(UsersFlowView.Field.GroupRole, RoleCodeTexts.SCHOOL)     //學校
                        .And(UsersFlowView.Field.UBank, logonUser.MySchIdentys);                    //自己分行學校
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦 - 帳號查詢：自己學校的帳號
                where = new Expression(UsersFlowView.Field.GroupRole, RoleCodeTexts.SCHOOL)     //學校
                    .And(UsersFlowView.Field.UBank, logonUser.UnitId);                          //自己學校
            }
            else
            {
                return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            }
            #endregion
            #endregion

            #region 群組 條件
            string qGroupId = this.QueryGroupId;
            if (!String.IsNullOrEmpty(qGroupId))
            {
                where.And(UsersFlowView.Field.UGrp, qGroupId);
            }
            #endregion

            #region 分行 條件
            string qBankCode = this.QueryBankCode;
            if (!String.IsNullOrEmpty(qBankCode))
            {
                where.And(UsersFlowView.Field.UBank, qBankCode);
            }
            #endregion

            #region 學校 條件
            string qSchIdenty = this.QuerySchIdenty;
            if (!String.IsNullOrEmpty(qSchIdenty))
            {
                where.And(UsersFlowView.Field.UBank, qSchIdenty);
            }
            #endregion

            #region 商家代號 條件
            string qReceiveType = this.QueryReceiveType;
            if (!String.IsNullOrEmpty(qReceiveType))
            {
                qReceiveType = "%," + qReceiveType + ",%";
                where.And(UsersFlowView.Field.URt, RelationEnum.Like, qReceiveType);
            }
            #endregion

            #region 使用者帳號 | 使用者名稱 條件
            {
                string qData = this.QueryDataValue;
                if (!String.IsNullOrEmpty(qData))
                {
                    switch (this.QueryDataType)
                    {
                        case "UserID":
                            where.And(UsersFlowView.Field.UId, qData);
                            break;
                        case "UserName":
                            where.And(UsersFlowView.Field.UName, RelationEnum.Like, "%" + qData + "%");
                            break;
                    }
                }
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(UsersFlowView.Field.UBank, OrderByEnum.Asc);
            orderbys.Add(UsersFlowView.Field.UId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<UsersFlowView>(pagingInfo, ucPagings, this.gvResult);
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
            #region 群組選項
            this.GetAndBindGroupOptions();
            #endregion

            #region 分行選項
            this.GetAndBindBankOptions();
            #endregion

            #region 學校選項
            this.GetAndBindSchoolOptions();
            #endregion

            #region 商家代號
            this.GetAndBindReceiveTypeOptions();
            #endregion

            #region rbtnlQDataType 選項
            {
                CodeText[] qDataOptions = new CodeText[2];
                qDataOptions[0] = new CodeText("UserID", "依使用者帳號");
                qDataOptions[1] = new CodeText("UserName", "依使用者名稱");
                WebHelper.SetRadioButtonListItems(this.rbtnlQDataType, qDataOptions, true, 2, "UserID");
            }
            #endregion

            #region 查詢結果初始化
            {
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
        /// 清除查詢結果
        /// </summary>
        private void ClearQueryResult()
        {
            string emptyDataText = this.gvResult.EmptyDataText;
            this.gvResult.EmptyDataText = "";
            this.gvResult.DataSource = null;
            this.gvResult.DataBind();
            this.gvResult.EmptyDataText = emptyDataText;
            Paging[] ucPagings = this.GetPagingControls();
            foreach (Paging ucPaging in ucPagings)
            {
                ucPaging.Visible = false;
            }
        }

        /// <summary>
        /// 取得並結繫群組選項
        /// </summary>
        private void GetAndBindGroupOptions()
        {
            LogonUser logonUser = this.GetLogonUser();

            Expression where = null;

            #region [MDY:20161010] 依據權限邏輯修正取群組資料邏輯
            #region [Old]
            //if (logonUser.IsBankManager)
            //{
            //    //總行可看非學校經辦的所有群組
            //    Expression w1 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.STAFF)
            //        .And(GroupListEntity.Field.GroupId, RelationEnum.NotIn, BankADGroupCodeTexts.GetInADCodes());   //濾掉存在 AD 的群組，因為這些群組的使用者不存在系統中
            //    Expression w2 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)
            //        .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER);
            //    where = new Expression().And(w1.Or(w2));
            //}
            //else if (logonUser.IsBankUser)
            //{
            //    //分行可看分行經辦群組與自己分行學校的主管群組
            //    Expression w1 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.STAFF)
            //        .And(GroupListEntity.Field.GroupId, RelationEnum.NotIn, BankADGroupCodeTexts.GetInADCodes())    //濾掉存在 AD 的群組，因為這些群組的使用者不存在系統中
            //        .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.USER);
            //    Expression w2 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)
            //        .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER)
            //        .And(GroupListEntity.Field.Branchs, logonUser.MySchIdentys);
            //    where = new Expression().And(w1.Or(w2));
            //}
            //else if (logonUser.IsSchoolUser)
            //{
            //    //學校可看自己學校的經辦群組
            //    where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)
            //        .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.USER)
            //        .And(GroupListEntity.Field.Branchs, logonUser.UnitId);
            //}
            #endregion

            if (logonUser.IsBankManager)
            {
                //總行 (AD0、AD5、AD6) - 群組選項：除 AD0、AD1、AD2 以外的所有群組
                where = new Expression(GroupListEntity.Field.GroupId, RelationEnum.NotIn, BankADGroupCodeTexts.GetInADCodes());     //除 AD0, AD1, AD2 群組
            }
            else if (logonUser.IsBankUser)
            {
                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管 - 群組選項：AD3、AD4 與自己分行的特定分行主管、經辦(含AD5、AD6)群組 + 自己分行的學校的群組
                    Expression w1b = new Expression(GroupListEntity.Field.GroupId, new string[] { BankADGroupCodeTexts.AD3, BankADGroupCodeTexts.AD4 }) //AD3、AD4
                        .Or(GroupListEntity.Field.Branchs, logonUser.UnitId);   //自己分行的特定群組
                    Expression w1 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.STAFF).And(w1b); //行員 and AD3、AD4 或 自己分行的特定群組
                    Expression w2 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                        .And(GroupListEntity.Field.Branchs, logonUser.MySchIdentys);                    //自己分行的學校
                    where = new Expression().And(w1.Or(w2));
                }
                else
                {
                    //分行主管、經辦 - 群組選項：自己分行的學校的群組
                    where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                        .And(GroupListEntity.Field.Branchs, logonUser.MySchIdentys);            //自己分行的學校
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦 - 群組選項：自己學校的群組
                where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                     .And(GroupListEntity.Field.Branchs, logonUser.UnitId);                 //自己學校
            }
            #endregion

            CodeText[] items = null;
            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(GroupListEntity.Field.GroupId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { GroupListEntity.Field.GroupId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { GroupListEntity.Field.GroupName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<GroupListEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢群組資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            WebHelper.SetDropDownListItems(this.ddlGroup, DefaultItem.Kind.Select, false, items, true, false, 0, null);
        }

        /// <summary>
        /// 取得並結繫分行選項
        /// </summary>
        private void GetAndBindBankOptions()
        {
            LogonUser logonUser = this.GetLogonUser();

            if (logonUser.IsBankManager)
            {
                //總行則顯示分行選項
                this.trBank.Visible = true;

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
                    string action = this.GetLocalized("查詢分行資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
                WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.Select, false, items, true, false, 0, null);
            }
            else
            {
                //否則不顯示分行選項
                this.trBank.Visible = false;
            }
        }

        /// <summary>
        /// 取得並結繫學校選項
        /// </summary>
        private void GetAndBindSchoolOptions()
        {
            LogonUser logonUser = this.GetLogonUser();

            Expression where = null;
            if (logonUser.IsBankManager)
            {
                //總行：所有學校
                where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL);
            }
            else if (logonUser.IsBankUser)
            {
                //分行：自己分行學校
                where = new Expression(SchoolRTypeEntity.Field.Status, DataStatusCodeTexts.NORMAL)
                    .And(SchoolRTypeEntity.Field.BankId, logonUser.UnitId);
            }

            if (where != null)
            {
                //有查詢條件則顯示學校選項
                this.trSchool.Visible = true;

                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.SchIdenty, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { SchoolRTypeEntity.Field.SchIdenty };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { SchoolRTypeEntity.Field.SchName };
                string textCombineFormat = null;

                CodeText[] datas = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<SchoolRTypeEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("讀取學校資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
                WebHelper.SetDropDownListItems(this.ddlSchool, DefaultItem.Kind.Select, false, datas, true, false, 0, null);
            }
            else
            {
                //否則不顯示學校選項
                this.trSchool.Visible = false;
            }
        }

        /// <summary>
        /// 取得並結繫商家代號選項
        /// </summary>
        private void GetAndBindReceiveTypeOptions()
        {
            LogonUser logonUser = this.GetLogonUser();

            if (logonUser.IsSchoolUser)
            {
                //學校才顯示商家代號選項
                this.trReceiveType.Visible = true;

                CodeText[] items = null;

                XmlResult xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTexts(this, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢所屬學校的商家代號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
                WebHelper.SetDropDownListItems(this.ddlReceiveType, DefaultItem.Kind.All, false, items, true, false, 0, null);
            }
            else
            {
                //否則不顯示商家代號選項
                this.trReceiveType.Visible = false;
            }
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus
        /// </summary>
        /// <returns>傳回處理結果</returns>
        private XmlResult GetAndKeepQueryCondition()
        {
            #region 群組
            this.QueryGroupId = this.ddlGroup.SelectedValue;
            #endregion

            #region 分行
            if (this.trBank.Visible)
            {
                this.QueryBankCode = this.ddlBank.SelectedValue;
            }
            else
            {
                this.QueryBankCode = null;
            }
            #endregion

            #region 學校
            if (this.trSchool.Visible)
            {
                this.QuerySchIdenty = this.ddlSchool.SelectedValue;
            }
            else
            {
                this.QuerySchIdenty = null;
            }
            #endregion

            #region 商家代號
            if (this.trReceiveType.Visible)
            {
                this.QueryReceiveType = this.ddlReceiveType.SelectedValue;
            }
            else
            {
                this.QueryReceiveType = null;
            }
            #endregion

            #region 使用者帳號 | 使用者名稱
            this.QueryDataType = this.rbtnlQDataType.SelectedValue;
            this.QueryDataValue = this.tbxQDataValue.Text.Trim();
            #endregion

            if (!String.IsNullOrEmpty(this.QueryBankCode))
            {
                if (!String.IsNullOrEmpty(this.QuerySchIdenty) || !String.IsNullOrEmpty(this.QueryReceiveType))
                {
                    return new XmlResult(false, "已選擇分行，不可再選擇學校或商家代號");
                }
            }

            return new XmlResult(true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
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
                //查詢條件有錯，需要清除查詢結果，避免翻頁執行到錯誤的查詢條件
                this.ClearQueryResult();

                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
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
            //QueryString.Add("BankId", this.GetLogonUser().UnitId);
            Session["QueryString"] = QueryString;
            Response.Redirect("S5300001M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            UsersFlowView[] datas = this.gvResult.DataSource as UsersFlowView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            LogonUser logonUser = this.GetLogonUser();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                UsersFlowView data = datas[row.RowIndex];
                //資料參數 (因為 ReceiveType 包含逗號，所以改用 | 分隔參數)
                string argument = String.Format("{0}|{1}|{2}|{3}", data.UId, data.URt, data.UGrp, data.UBank);

                bool isEditable = false;            //是否可維護
                bool isBankData = data.IsBank();    //是否為行員帳號
                if (data.FlowStatus == FlowStatusCodeTexts.ENDING)  //已覆核的資料才可能被修改
                {
                    if (logonUser.IsBankManager)
                    {
                        //總行 (AD0、AD5、AD6) - 帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
                        if (isBankData) //所有行員的帳號
                        {
                            isEditable = true;
                        }
                        else if (!isBankData && data.GroupRoleType == RoleTypeCodeTexts.MANAGER)   //所有學校的主管帳號
                        {
                            isEditable = true;
                        }
                    }
                    else if (logonUser.IsBankUser)
                    {
                        if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                        {
                            //分行主控、會計主管 - 帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
                            if (isBankData && data.UBank == logonUser.BankId)   //自己分行的帳號
                            {
                                isEditable = true;
                            }
                            else if (!isBankData && logonUser.IsMySchIdenty(data.UBank) && data.GroupRoleType == RoleTypeCodeTexts.MANAGER)    //自己分行的學校的主管帳號
                            {
                                isEditable = true;
                            }
                        }
                        else
                        {
                            //分行主管、經辦 - 帳號維護：自己分行的學校的主管帳號
                            if (!isBankData && logonUser.IsMySchIdenty(data.UBank) && data.GroupRoleType == RoleTypeCodeTexts.MANAGER)    //自己分行的學校的主管帳號
                            {
                                isEditable = true;
                            }
                        }
                    }
                    else if (logonUser.IsSchoolUser)
                    {
                        //學校主管、經辦 - 帳號維護：自己學校的經辦帳號
                        if (!isBankData && logonUser.UnitId == data.UBank && data.GroupRoleType == RoleTypeCodeTexts.USER)    //自己學校的經辦帳號
                        {
                            isEditable = true;
                        }
                    }
                }

                row.Cells[0].Text = HttpUtility.HtmlEncode(data.URt.Trim(new char[] { ' ', ',' }));

                MyModifyButton ccbtnModify = row.FindControl("ccbtnModify") as MyModifyButton;
                if (ccbtnModify != null)
                {
                    ccbtnModify.CommandArgument = argument;
                    ccbtnModify.Visible = isEditable;
                }

                MyDeleteButton ccbtnDelete = row.FindControl("ccbtnDelete") as MyDeleteButton;
                if (ccbtnDelete != null)
                {
                    ccbtnDelete.CommandArgument = argument;
                    ccbtnDelete.Visible = isEditable;
                }
            }
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
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }
            //(因為 ReceiveType 包含逗號，所以改用 | 分隔參數)
            string[] args = argument.Split(new char[] { '|' }, StringSplitOptions.None);
            if (args.Length != 4)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("取無法取得要處理資料的參數");
                this.ShowSystemMessage(msg);
                return;
            }

            string userId = args[0];
            string receiveType = args[1];
            string groupId = args[2];
            string bankId = args[3];
            #endregion

            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("UserId", userId);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("GroupId", groupId);
                        QueryString.Add("BankId", bankId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5300001M.aspx"));
                        #endregion
                    }
                    #endregion
                    break;
                case ButtonCommandName.Delete:
                    #region 刪除資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Delete);
                        QueryString.Add("UserId", userId);
                        QueryString.Add("ReceiveType", receiveType);
                        QueryString.Add("GroupId", groupId);
                        QueryString.Add("BankId", bankId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl("S5300001M.aspx"));
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