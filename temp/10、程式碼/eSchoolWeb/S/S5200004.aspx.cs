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
    /// 待辦事項
    /// </summary>
    public partial class S5200004 : PagingBasePage
    {
        #region [權限邏輯：20161010] 依據【土銀學雜使用者帳號管理說明.doc】文件
        // 1. 行員申請的行員審核，學校申請的學校審核
        // 2. 非自己申請的資料
        // 3. 資料權限
        // 總行 (AD0、AD5、AD6)
        //   帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
        // 分行主控、會計主管
        //   帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
        // 分行主管、經辦
        //   帳號維護：自己分行的學校的主管帳號
        // 學校主管、經辦
        //   帳號維護：自己學校的經辦帳號
        #endregion

        #region Property
        /// <summary>
        /// 儲存查詢的待辦事項 (表單代碼)
        /// </summary>
        private string QueryFormId
        {
            get
            {
                return ViewState["QueryFormId"] as string;
            }
            set
            {
                ViewState["QueryFormId"] = value == null ? String.Empty : value.Trim();
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
            LogonUser logonUser = this.GetLogonUser();

            #region 基本條件
            where = new Expression(FlowDataEntity.Field.Status, FlowStatusCodeTexts.FLOWING);   //待處理
            #endregion

            #region [MDY:20161010] 依據權限邏輯修正
            if (logonUser.IsBankUser)
            {
                where.And(FlowDataEntity.Field.ApplyUserQual, UserQualCodeTexts.BANK)                   //行員申請的行員審核
                    .And(FlowDataEntity.Field.ApplyUserId, RelationEnum.NotEqual, logonUser.UserId);    //非自己申請 (因為行員帳號是全銀行唯一，所以不用看分行代碼)

                if (logonUser.IsBankManager)
                {
                    //總行 - 帳號維護：所有分行 + 所有學校主管
                    Expression w1 = new Expression(FlowDataEntity.Field.DataRole , RoleCodeTexts.STAFF);    //行員
                    Expression w2 = new Expression(FlowDataEntity.Field.DataRole, RoleCodeTexts.SCHOOL)     //學校
                        .And(FlowDataEntity.Field.DataRoleType, RoleTypeCodeTexts.MANAGER);                 //主管
                    where.And(w1.Or(w2));    //所有行員 or 所有學校主管 的資料
                }
                else if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管 - 自己分行 + 自己分行的學校主管
                    Expression w1 = new Expression(FlowDataEntity.Field.DataRole, RoleCodeTexts.STAFF)      //行員
                        .And(FlowDataEntity.Field.DataUnitId, logonUser.UnitId);                            //自己分行
                    Expression w2 = new Expression(FlowDataEntity.Field.DataRole, RoleCodeTexts.SCHOOL)     //學校
                        .And(FlowDataEntity.Field.DataUnitId, logonUser.MySchIdentys)                       //自己分行的學校
                        .And(FlowDataEntity.Field.DataRoleType, RoleTypeCodeTexts.MANAGER);                 //主管
                    where.And(w1.Or(w2));    //自己分行的行員 or 自己分行的學校主管 的資料
                }
                else
                {
                    //分行主管、經辦 - 自己分行的學校主管
                    where.And(FlowDataEntity.Field.DataRole, RoleCodeTexts.SCHOOL)              //學校
                        .And(FlowDataEntity.Field.DataUnitId, logonUser.MySchIdentys)           //自己分行的學校
                        .And(FlowDataEntity.Field.DataRoleType, RoleTypeCodeTexts.MANAGER);     //主管
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                where
                    .And(FlowDataEntity.Field.ApplyUserQual, UserQualCodeTexts.SCHOOL)                  //學校申請的學校審核
                    .And(FlowDataEntity.Field.ApplyUnitId, logonUser.UnitId)                            //自己學校申請
                    .And(FlowDataEntity.Field.ApplyUserId, RelationEnum.NotEqual, logonUser.UserId);    //非自己申請

                where
                    .And(FlowDataEntity.Field.DataRole, RoleCodeTexts.SCHOOL)                           //學校
                    .And(FlowDataEntity.Field.DataUnitId, logonUser.UnitId)                             //自己學校
                    .And(FlowDataEntity.Field.DataRoleType, RoleTypeCodeTexts.USER);                    //經辦
            }
            else
            {
                orderbys = null;
                return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            }
            #endregion

            #region [Old]
            //if (logonUser.IsBankUser)
            //{
            //    where.And(FlowDataEntity.Field.ApplyUserQual, UserQualCodeTexts.BANK);  //行員
            //    if (!logonUser.IsBankManager)
            //    {
            //        where
            //            .And(FlowDataEntity.Field.ApplyUnitId, logonUser.UnitId)    //分行代碼
            //            .And(FlowDataEntity.Field.ApplyUserId, RelationEnum.NotEqual, logonUser.UserId);    //不可自己申請自審核
            //    }
            //    else
            //    {
            //        Expression w1 = new Expression(FlowDataEntity.Field.ApplyUnitId, RelationEnum.NotEqual, logonUser.UnitId);  //非自己分行
            //        Expression w2 = new Expression(FlowDataEntity.Field.ApplyUnitId, logonUser.UnitId)      //不可自己申請自審核
            //            .And(FlowDataEntity.Field.ApplyUserId, RelationEnum.NotEqual, logonUser.UserId);
            //        where.And(w1.Or(w2)); //非自己分行，或非自己申請
            //    }
            //}
            //else if (logonUser.IsSchoolUser)
            //{
            //    where
            //        .And(FlowDataEntity.Field.ApplyUserQual, UserQualCodeTexts.SCHOOL)  //學校
            //        .And(FlowDataEntity.Field.ApplyUnitId, logonUser.UnitId)            //學校統編
            //        .And(FlowDataEntity.Field.ApplyUserId, RelationEnum.NotEqual, logonUser.UserId);    //不可自己申請自審核
            //}
            //else
            //{
            //    orderbys = null;
            //    return new XmlResult(false, "無權限", ErrorCode.S_NO_AUTHORIZE, null);
            //}
            #endregion

            #region 待辦事項 (表單代碼) 條件
            string qFormId = this.QueryFormId;
            if (!String.IsNullOrEmpty(qFormId))
            {
                where.And(FlowDataEntity.Field.FormId, qFormId);
            }
            #endregion

            orderbys = new KeyValueList<OrderByEnum>(3);
            orderbys.Add(FlowDataEntity.Field.ApplyDate, OrderByEnum.Asc);
            orderbys.Add(FlowDataEntity.Field.DataUnitId, OrderByEnum.Asc);
            orderbys.Add(FlowDataEntity.Field.DataReceiveType, OrderByEnum.Asc);
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
            XmlResult xmlResult = base.QueryDataAndBind<FlowDataEntity>(pagingInfo, ucPagings, this.gvResult);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Query);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
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
        private void InitialUI()
        {
            #region 待辦事項 (表單代碼)
            LogonUser user = this.GetLogonUser();
            WebHelper.SetDropDownListItems(this.ddlFormId, DefaultItem.Kind.All, false, new FormCodeTexts(), false, false, 0, null);
            #endregion
        }

        /// <summary>
        /// 取得查詢條件並紀錄到 ViewStatus (記錄到 ViewStatus 是為了避免翻頁時取介面上的條件)
        /// </summary>
        /// <returns>成功傳回 true，否則傳回 false</returns>
        private bool GetAndKeepQueryCondition()
        {
            //查詢的銷帳選擇
            this.QueryFormId = this.ddlFormId.SelectedValue;

            return true;
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

            if (this.GetAndKeepQueryCondition())
            {
                PagingInfo pagingInfo = new PagingInfo(10, 0, 0);
                XmlResult xmlResult = this.CallQueryDataAndBind(pagingInfo);

                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, xmlResult.Message);
                }
            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            FlowDataEntity[] datas = this.gvResult.DataSource as FlowDataEntity[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            bool isVisible = this.HasMaintainAuth();

            foreach (GridViewRow row in this.gvResult.Rows)
            {
                FlowDataEntity data = datas[row.RowIndex];
                //資料參數
                string argument = data.Guid;

                MyLinkButton lbtnApprove = row.FindControl("lbtnApprove") as MyLinkButton;
                if (lbtnApprove != null)
                {
                    lbtnApprove.CommandArgument = argument;
                    lbtnApprove.Visible = isVisible;
                }

                MyLinkButton lbtnReject = row.FindControl("lbtnReject") as MyLinkButton;
                if (lbtnApprove != null)
                {
                    lbtnReject.CommandArgument = argument;
                    lbtnReject.Visible = isVisible;
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

            string guid = argument;
            #endregion

            switch (e.CommandName)
            {
                case "Approve":
                case "Reject":
                    #region 處理
                    {
                        string processKind = e.CommandName == "Approve" ? ProcessKindCodeTexts.APPROVE : ProcessKindCodeTexts.REJECT;
                        XmlResult xmlResult = DataProxy.Current.ProcessFlowData(this.Page, this.GetLogonUser(), guid, processKind, String.Empty);

                        string action = this.GetLocalized(ProcessKindCodeTexts.GetText(processKind));
                        if (xmlResult.IsSuccess)
                        {
                            this.ShowActionSuccessMessage(action);
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                        }

                        if (this.GetAndKeepQueryCondition())
                        {
                            XmlResult xmlResult2 = this.CallQueryDataAndBind(this.ucPaging1.PagingInfo);
                            if (!xmlResult2.IsSuccess)
                            {
                                this.ShowErrorMessage(xmlResult2.Code, xmlResult2.Message);
                            }
                        }
                    }
                    #endregion
                    break;
            }
        }
    }
}