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
    /// 群組管理
    /// </summary>
    public partial class S5200003 : PagingBasePage
    {
        #region [資料處理邏輯：20161010] 依據【土銀學雜使用者帳號管理說明.doc】文件
        // 資料邏輯
        //   1. 行員群組代碼最多 4 碼 且不可以 AD 開頭，學校群組代碼至少 5 碼且以學校代碼 (4碼) 開頭
        //   2. 行員群組的 Branchs 存放特定分行代碼 (6碼) 或空字串，學校群組的 Branchs 存放學校代碼
        //   3. 所有維護的行員群組都是分行群組 (總行群組由 BankADGroupCodeTexts 定義)
        //
        // 權限邏輯
        // 非 BankADGroupCodeTexts 定義的群組才能維護
        // 總行：可維護 所有行員群組 + 所有學校的主管群組
        //   1. 取所有群組
        //   2. (Role = 1) 或 (Role = 2 且 RoleType = 3) 才能維護，其他只能看
        // 分行主控、會計主管：可維護 自己分行的特定群組 + 自己分行的學校的主管群組
        //   1. 取 (Role = 1 且 (AD3、AD4 或 Branchs = 自己分行代碼) 或 (Role = 2 且 Branchs = 自己分行的學校代碼) 的群組
        //   2. (Role = 1 且 Branchs = 自己分行代碼) 或 (Role = 2 且 RoleType = 3 且 Branchs = 自己分行的學校代碼) 才能維護，其他只能看
        // 分行主管、經辦：可維護 自己分行的學校的主管群組
        //   1. 取 (Role = 2 且 Branchs = 自己分行的學校代碼) 的群組
        //   2. (Role = 2 且 RoleType = 3 且 Branchs = 自己分行的學校代碼) 才能維護，其他只能看
        // 學校主管、經辦：可維護 自己學校的經辦群組
        //   1. 取 (Role = 2 且 Branchs = 學校代碼) 的群組
        //   2. (RoleType = 2 且 RoleType = 2 且 Branchs = 學校代碼) 才能維護，其他只能看
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
            #region [Old]
            //if (logonUser.IsSchoolUser)
            //{
            //    //學校
            //    where = new Expression(GroupListEntity.Field.GroupId, RelationEnum.Like, logonUser.UnitId + "%")
            //        .And(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL);
            //}
            //else if (logonUser.IsBankUser)
            //{
            //    where = new Expression();
            //}
            //else
            //{
            //    where = null;
            //    orderbys = null;
            //    return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            //}
            #endregion

            #region [MDY:20161010] 依據資料處理邏輯修正
            LogonUser logonUser = this.GetLogonUser();
            if (logonUser.IsBankManager)
            {
                //總行 - 查看所有群組
                where = new Expression();
            }
            else if (logonUser.IsBankUser)
            {
                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管 - 查看 (Role = 1 且 (AD3、AD4 或 Branchs = 自己分行代碼) 或 (Role = 2 且 Branchs = 自己分行的學校代碼) 的群組
                    Expression w1b = new Expression(GroupListFlowView.Field.GroupId, new string[] { BankADGroupCodeTexts.AD3, BankADGroupCodeTexts.AD4 }) //AD3、AD4
                        .Or(GroupListFlowView.Field.Branchs, logonUser.UnitId);   //自己分行的分行代碼
                    Expression w1 = new Expression(GroupListFlowView.Field.Role, RoleCodeTexts.STAFF).And(w1b); //行員 且 (AD3、AD4 或 自己分行的特定群組)
                    Expression w2 = new Expression(GroupListFlowView.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                        .And(GroupListFlowView.Field.Branchs, logonUser.MySchIdentys);                    //自己分行的學校代碼
                    where = new Expression().And(w1.Or(w2));
                }
                else
                {
                    //分行主管、經辦 - 查看 (Role = 2 且 Branchs = 自己分行的學校代碼) 的群組
                    where = new Expression(GroupListFlowView.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                        .And(GroupListFlowView.Field.Branchs, logonUser.MySchIdentys);            //自己分行的學校代碼
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦 - 查看 (Role = 2 且 Branchs = 學校代碼) 的群組
                where = new Expression(GroupListFlowView.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                    .And(GroupListFlowView.Field.Branchs, logonUser.UnitId);                  //自己學校的學校代碼
            }
            else
            {
                where = null;
                orderbys = null;
                return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(2);
            orderbys.Add(GroupListFlowView.Field.Role, OrderByEnum.Asc);
            orderbys.Add(GroupListFlowView.Field.GroupId, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<GroupListFlowView>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                //[TODO] 變動顯示訊息怎麼多語系
                this.ShowSystemMessage(this.GetLocalized("查詢資料失敗") + "，" + xmlResult.Message);
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

            this.ccbtnInsert.Enabled = xmlResult.IsSuccess;
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

            Server.Transfer("S5200003M.aspx");
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            GroupListFlowView[] datas = this.gvResult.DataSource as GroupListFlowView[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            LogonUser logonUser = this.GetLogonUser();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                GroupListFlowView data = datas[row.RowIndex];
                //資料參數
                string argument = data.GroupId;

                #region [MDY:20161010] 依據資料處理邏輯修正
                bool isEditable = !BankADGroupCodeTexts.IsDefine(data.GroupId);     //非 BankADGroupCodeTexts 定義的群組
                isEditable &= data.FlowStatus == FlowStatusCodeTexts.ENDING;        //已覆核的資料才可能被修改
                if (isEditable)
                {
                    if (logonUser.IsBankManager)
                    {
                        //總行 - 可維護 (Role = 1) 或 (Role = 2 且 RoleType = 3) 的群組
                        isEditable = (data.Role == RoleCodeTexts.STAFF 
                            || (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.MANAGER));
                    }
                    else if (logonUser.IsBankUser)
                    {
                        if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                        {
                            //分行主控、會計主管 - 可維護 (Role = 1 且 Branchs = 自己分行代碼) 或 (Role = 2 且 RoleType = 3 且 Branchs = 自己分行的學校代碼) 的群組
                            isEditable = ((data.Role == RoleCodeTexts.STAFF && data.Branchs == logonUser.UnitId)
                                || (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.MANAGER && logonUser.IsMySchIdenty(data.Branchs)));
                        }
                        else
                        {
                            //分行主管、經辦 - 可維護 (Role = 2 且 RoleType = 3 且 Branchs = 自己分行的學校代碼) 的群組
                            isEditable = (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.MANAGER && logonUser.IsMySchIdenty(data.Branchs));
                        }
                    }
                    else if (logonUser.IsSchoolUser)
                    {
                        //學校主管、經辦 - 可維護 (RoleType = 2 且 RoleType = 2 且 Branchs = 學校代碼) 的群組
                        isEditable = (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.USER && data.Branchs == logonUser.UnitId);
                    }
                    else
                    {
                        isEditable = false;
                    }
                }

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
                #endregion
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
            string groupId = argument;
            #endregion

            string editUrl = "S5200003M.aspx";
            switch (e.CommandName)
            {
                case ButtonCommandName.Modify:
                    #region 修改資料
                    {
                        KeyValueList<string> QueryString = new KeyValueList<string>();
                        QueryString.Add("Action", ActionMode.Modify);
                        QueryString.Add("GroupId", groupId);
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
                        QueryString.Add("GroupId", groupId);
                        Session["QueryString"] = QueryString;

                        #region [MDY:20210521] 原碼修正
                        Server.Transfer(WebHelper.GenRNUrl(editUrl));
                        #endregion

                        #region [Old]
                        //GroupListEntity data = new GroupListEntity();
                        //data.GroupId = groupId;

                        //string action = this.GetLocalized("刪除資料");
                        //int count = 0;
                        //XmlResult xmlResult = DataProxy.Current.Delete<GroupListEntity>(this, data, out count);
                        //if (xmlResult.IsSuccess)
                        //{
                        //    if (count == 0)
                        //    {
                        //        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                        //    }
                        //    else
                        //    {
                        //        this.ShowActionSuccessMessage(action);

                        //        this.CallQueryDataAndBind(this.ucPaging1.GetPagingInfo());
                        //    }
                        //}
                        //else
                        //{
                        //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        //}
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