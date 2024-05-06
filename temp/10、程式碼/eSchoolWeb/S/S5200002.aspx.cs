using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 權限管理
    /// </summary>
    public partial class S5200002 : BasePage
    {
        #region [權限邏輯：20161010] 依據【土銀學雜使用者帳號管理說明.doc】文件
        // 總行 (AD0、AD5、AD6)
        //   群組選項：所有行員群組 + 所有學校的主管群組
        // 分行主控、會計主管
        //   群組選項：自己分行的特定群組(含AD5、AD6) + 自己分行的學校的主管群組
        // 分行主管、經辦
        //   群組選項：自己分行的學校的主管群組
        // 學校主管、經辦
        //   群組選項：自己學校的經辦群組
        #endregion

        #region inner class
        /// <summary>
        /// 功能權限類別
        /// </summary>
        [Serializable]
        private class FuncRight
        {
            #region Property
            private string _FuncId = null;
            /// <summary>
            /// 功能代碼
            /// </summary>
            public string FuncId
            {
                get
                {
                    return _FuncId;
                }
                set
                {
                    _FuncId = value == null ? null : value.Trim();
                }
            }

            private string _FuncName = null;
            /// <summary>
            /// 功能名稱
            /// </summary>
            public string FuncName
            {
                get
                {
                    return _FuncName;
                }
                set
                {
                    _FuncName = value == null ? String.Empty : value.Trim();
                }
            }

            /// <summary>
            /// 是否勾選新增權限
            /// </summary>
            public bool CheckInsertRight
            {
                get;
                set;
            }

            /// <summary>
            /// 是否勾選修改權限
            /// </summary>
            public bool CheckUpdateRight
            {
                get;
                set;
            }

            /// <summary>
            /// 是否勾選刪除權限
            /// </summary>
            public bool CheckDeleteRight
            {
                get;
                set;
            }

            /// <summary>
            /// 是否勾選查詢權限
            /// </summary>
            public bool CheckSelectRight
            {
                get;
                set;
            }

            /// <summary>
            /// 是否勾選列印權限
            /// </summary>
            public bool CheckPrintRight
            {
                get;
                set;
            }

            public AuthCodeEnum MaxAuthCode
            {
                get;
                set;
            }
            #endregion

            #region Constructor
            /// <summary>
            /// 建構式
            /// </summary>
            public FuncRight()
            {
            }

            /// <summary>
            /// 建構式
            /// </summary>
            /// <param name="funcId">功能代碼</param>
            /// <param name="funcName">功能名稱</param>
            /// <param name="rightCode">權限代碼 (1=編緝 / 2=查詢)</param>
            public FuncRight(string funcId, string funcName, string rightCode, AuthCodeEnum maxAuthCode)
            {
                this.FuncId = funcId;
                this.FuncName = funcName;

                rightCode = GroupRightEntity.FormatRightCode(rightCode == null ? null : rightCode.Trim());
                this.CheckInsertRight = (rightCode[0] == 'Y');
                this.CheckUpdateRight = (rightCode[1] == 'Y');
                this.CheckDeleteRight = (rightCode[2] == 'Y');
                this.CheckSelectRight = (rightCode[3] == 'Y');
                this.CheckPrintRight = (rightCode[4] == 'Y');

                #region [MDY:20160116] 濾掉自己沒有的授權
                this.MaxAuthCode = maxAuthCode;
                if (!AuthCodeHelper.HasInsert(maxAuthCode))
                {
                    this.CheckInsertRight = false;
                }
                if (!AuthCodeHelper.HasUpdate(maxAuthCode))
                {
                    this.CheckUpdateRight = false;
                }
                if (!AuthCodeHelper.HasDelete(maxAuthCode))
                {
                    this.CheckDeleteRight = false;
                }
                if (!AuthCodeHelper.HasSelect(maxAuthCode))
                {
                    this.CheckSelectRight = false;
                }
                if (!AuthCodeHelper.HasPrint(maxAuthCode))
                {
                    this.CheckPrintRight = false;
                }
                #endregion
            }
            #endregion

            #region Method
            /// <summary>
            /// 取得有效的權限代碼
            /// </summary>
            /// <returns></returns>
            public string GetRightCode()
            {
                return String.Format("{0}{1}{2}{3}{4}"
                    , this.CheckInsertRight ? "Y" : "N"
                    , this.CheckUpdateRight ? "Y" : "N"
                    , this.CheckDeleteRight ? "Y" : "N"
                    , this.CheckSelectRight ? "Y" : "N"
                    , this.CheckPrintRight ? "Y" : "N"
                );
            }

            /// <summary>
            /// 取得有效的權限文字
            /// </summary>
            /// <returns></returns>
            public string GetRightText()
            {
                return String.Format("{0}{1}{2}{3}{4}"
                    , this.CheckInsertRight ? "增" : String.Empty
                    , this.CheckUpdateRight ? "修" : String.Empty
                    , this.CheckDeleteRight ? "刪" : String.Empty
                    , this.CheckSelectRight ? "查" : String.Empty
                    , this.CheckPrintRight ? "印" : String.Empty
                );
            }

            /// <summary>
            /// 取得是否有權限
            /// </summary>
            /// <returns></returns>
            public bool HasRight()
            {
                return (this.CheckInsertRight || this.CheckUpdateRight || this.CheckDeleteRight || this.CheckSelectRight || this.CheckPrintRight);
            }

            public bool IsMenu()
            {
                return (String.IsNullOrEmpty(this.FuncId) || this.FuncId.Length <= 3);
            }
            #endregion
        }
        #endregion

        #region Property
        /// <summary>
        /// 儲存群組代碼的查詢條件
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
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ddlGroup.Enabled = false;
            }
            #endregion

            bool isOK = this.GetAndBindGroupOption();
            if (isOK)
            {
                isOK = this.GetAndBindQueryData(this.ddlGroup.SelectedValue);
            }

            this.ccbtnOK.Visible = isOK && (this.gvResult.Rows.Count > 0);

            this.ccbtnExport.OnClientClick = String.Format("return confirm('{0}');", this.GetLocalized("匯出資料不會自動儲存修改過的設定，請先按【確定】儲存資料，確定要匯出資料"));
            this.ccbtnExport.Visible = isOK;

            #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
            this.ccbtnExportODS.OnClientClick = String.Format("return confirm('{0}');", this.GetLocalized("匯出資料不會自動儲存修改過的設定，請先按【確定】儲存資料，確定要匯出資料"));
            this.ccbtnExportODS.Visible = isOK;
            #endregion
        }

        /// <summary>
        /// 取得並結繫群組選項
        /// </summary>
        private bool GetAndBindGroupOption()
        {
            bool IsOK = true;
            this.ddlGroup.Items.Clear();

            #region 取得群組選項
            CodeText[] datas = null;
            {
                LogonUser logonUser = this.GetLogonUser();

                Expression where = null;
                if (logonUser.IsBankManager)
                {
                    //總行：所有行員群組 + 所有學校的主管群組
                    where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.STAFF)     //行員群組
                        .Or(new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL).And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER));   //學校主管群組
                }
                else if (logonUser.IsBankUser)
                {
                    if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                    {
                        //分行主控、會計主管：自己分行的特定群組(含AD5、AD6) + 自己分行的學校的主管群組
                        Expression w1 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.STAFF)     //行員
                            .And(GroupListEntity.Field.Branchs, logonUser.UnitId);                          //自己分行
                        Expression w2 = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                            .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER)                 //主管
                            .And(GroupListEntity.Field.Branchs, logonUser.MySchIdentys);                    //自己分行的學校
                        where = new Expression();
                        where.And(w1.Or(w2));
                    }
                    else
                    {
                        //分行主管、經辦：自己分行的學校的主管群組
                        where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                            .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER)         //主管
                            .And(GroupListEntity.Field.Branchs, logonUser.MySchIdentys);            //自己分行的學校
                    }
                }
                else if (logonUser.IsSchoolUser)
                {
                    //學校主管、經辦：自己學校的經辦群組
                    where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.SCHOOL)    //學校
                        .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.USER)            //主管
                        .And(GroupListEntity.Field.Branchs, logonUser.UnitId);                  //自己學校
                }
                else
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return false;
                }

                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(GroupListEntity.Field.GroupId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { GroupListEntity.Field.GroupId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { GroupListEntity.Field.GroupName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<GroupListEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    IsOK = false;
                    string action = this.GetLocalized("查詢群組資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            WebHelper.SetDropDownListItems(this.ddlGroup, DefaultItem.Kind.Select, false, datas, true, false, 0, null);

            return IsOK;
        }

        /// <summary>
        /// 取得並結繫查詢資料
        /// </summary>
        /// <returns></returns>
        private bool GetAndBindQueryData(string groupId)
        {
            this.QueryGroupId = groupId;

            if (String.IsNullOrEmpty(groupId))
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataBind();
                return true;
            }

            bool isOK = true;

            #region 取得功能資料
            FuncMenuEntity[] funcMenus = null;
            {
                string action = this.GetLocalized("查詢功能資料");
                Expression where = new Expression(FuncMenuEntity.Field.Status, DataStatusCodeTexts.NORMAL);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(FuncMenuEntity.Field.FuncId, OrderByEnum.Asc);
                XmlResult xmlResult = DataProxy.Current.SelectAll<FuncMenuEntity>(this, where, orderbys, out funcMenus);
                if (!xmlResult.IsSuccess)
                {
                    isOK = false;
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
                else if (funcMenus == null || funcMenus.Length == 0)
                {
                    isOK = false;
                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "未設定任何功能資料");
                }
            }
            #endregion

            #region 取得群組權限資料
            GroupRightEntity[] groupRights = null;
            if (isOK)
            {
                string action = this.GetLocalized("查詢群組權限資料");
                Expression where = new Expression(GroupRightEntity.Field.GroupId, groupId)
                    .And(GroupRightEntity.Field.RightCode, RelationEnum.NotIn, new string[] { GroupRightEntity.None_RightCode, String.Empty }); //只取有權限的就好
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(GroupRightEntity.Field.FuncId, OrderByEnum.Asc);
                XmlResult xmlResult = DataProxy.Current.SelectAll<GroupRightEntity>(this, where, orderbys, out groupRights);
                if (!xmlResult.IsSuccess)
                {
                    isOK = false;
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }
            #endregion

            #region 組成 FuncRigths
            List<FuncRight> funcRights = null;
            if (isOK && funcMenus != null && funcMenus.Length > 0)
            {
                funcRights = new List<FuncRight>(funcMenus.Length);
                LogonUser logonUser = this.GetLogonUser();

                #region [MDY:20160116] 非總行人員只能授權自己有的功能
                bool checkMyAuth = !logonUser.IsBankManager;
                MenuAuth[] myAuthMenus = checkMyAuth ? logonUser.AuthMenus : null;
                #endregion

                foreach (FuncMenuEntity funcMenu in funcMenus)
                {
                    string funcId = funcMenu.FuncId;
                    if (String.IsNullOrEmpty(funcId) || funcId.Length <= 3)
                    {
                        //沒有代碼或長度小於等於 3 (視為 Menu) 不處理
                        continue;
                    }

                    #region [MDY:20160116] 濾掉自己沒有的功能
                    AuthCodeEnum maxAuthCode = AuthCodeEnum.None;
                    if (checkMyAuth)
                    {
                        MenuAuth myAuthMenu = myAuthMenus.FirstOrDefault<MenuAuth>(x => x.MenuID.Equals(funcId));
                        if (myAuthMenu == null || !myAuthMenu.HasAnyone())
                        {
                            continue;
                        }
                        maxAuthCode = myAuthMenu.AuthCode;
                    }
                    else
                    {
                        maxAuthCode = AuthCodeEnum.All;
                    }
                    #endregion

                    string rightCode = null;
                    if (groupRights != null && groupRights.Length > 0)
                    {
                        foreach (GroupRightEntity groupRight in groupRights)
                        {
                            if (groupRight.GroupId == groupId && groupRight.FuncId == funcId)
                            {
                                rightCode = groupRight.RightCode.Trim();
                                break;
                            }
                        }
                    }

                    funcRights.Add(new FuncRight(funcMenu.FuncId, funcMenu.FuncName, rightCode, maxAuthCode));
                }
            }
            #endregion

            this.gvResult.DataSource = funcRights.ToArray();
            this.gvResult.DataBind();
            return isOK;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string groupId = this.ddlGroup.SelectedValue;
            bool isOK = this.GetAndBindQueryData(groupId);

            this.ccbtnOK.Visible = isOK && (this.gvResult.Rows.Count > 0);
            this.ccbtnExport.Visible = isOK;
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            FuncRight[] datas = this.gvResult.DataSource as FuncRight[];
            if (datas == null || datas.Length == 0)
            {
                return;
            }

            #region 檢查維護權限
            bool enabled = this.HasMaintainAuth();
            #endregion

            foreach (GridViewRow row in gvResult.Rows)
            {
                FuncRight data = datas[row.RowIndex];
                string rightCode = data.GetRightCode();

                CheckBox chkInsert = row.FindControl("chkInsert") as CheckBox;
                if (chkInsert != null)
                {
                    chkInsert.Checked = data.CheckInsertRight;
                    chkInsert.Enabled = enabled && AuthCodeHelper.HasInsert(data.MaxAuthCode);
                }

                CheckBox chkUpdate = row.FindControl("chkUpdate") as CheckBox;
                if (chkUpdate != null)
                {
                    chkUpdate.Checked = data.CheckUpdateRight;
                    chkUpdate.Enabled = enabled && AuthCodeHelper.HasUpdate(data.MaxAuthCode);
                }
                CheckBox chkDelete = row.FindControl("chkDelete") as CheckBox;
                if (chkDelete != null)
                {
                    chkDelete.Checked = data.CheckDeleteRight;
                    chkDelete.Enabled = enabled && AuthCodeHelper.HasDelete(data.MaxAuthCode);
                }
                CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                if (chkSelect != null)
                {
                    chkSelect.Checked = data.CheckSelectRight;
                    chkSelect.Enabled = enabled && AuthCodeHelper.HasSelect(data.MaxAuthCode);
                }
                CheckBox chkPrint = row.FindControl("chkPrint") as CheckBox;
                if (chkPrint != null)
                {
                    chkPrint.Checked = data.CheckPrintRight;
                    chkPrint.Enabled = enabled && AuthCodeHelper.HasPrint(data.MaxAuthCode);
                }
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 檢查維護權限
            if (!this.HasMaintainAuth())
            {
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                return;
            }
            #endregion

            string groupId = this.QueryGroupId;
            if (String.IsNullOrEmpty(groupId))
            {
                this.ShowMustInputAlert("群組");
                return;
            }

            #region 取得群組資料
            GroupListEntity group = null;
            {
                Expression where = new Expression(GroupListEntity.Field.GroupId, groupId);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<GroupListEntity>(this.Page, where, null, out group);
                if (xmlResult.IsSuccess)
                {
                    if (group == null)
                    {
                        string msg = this.GetLocalized("查無該群組資料");
                        this.ShowSystemMessage(msg);
                        return;
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(this.GetLocalized("讀取群組資料"), xmlResult.Code, xmlResult.Message);
                    return;
                }
            }
            #endregion

            LogonUser logonUser = this.GetLogonUser();

            #region 先判斷是否需要審核流程 (學校使用者由 SchoolRTypeEntity.FlowKind 決定，其他一律要審核)
            bool isNeedFlow = true;
            if (logonUser.GroupId == BankADGroupCodeTexts.AD0)
            {
                //AD0 維護的資料不用審
                isNeedFlow = false;
            }
            else if (logonUser.IsSchoolUser)
            {
                SchoolRTypeEntity school = null;

                //只要 FlowKindCodeTexts.SINGLE, FlowKindCodeTexts.MULTI，其他視為 FlowKindCodeTexts.SINGLE
                Expression where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUser.UnitId)
                    .And(SchoolRTypeEntity.Field.FlowKind, new string[] { FlowKindCodeTexts.SINGLE, FlowKindCodeTexts.MULTI });
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(SchoolRTypeEntity.Field.MdyDate, OrderByEnum.Desc);
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where, orderbys, out school);
                if (xmlResult.IsSuccess)
                {
                    if (school != null)
                    {
                        isNeedFlow = school.IsNeedFlow();
                    }
                }
                else
                {
                    this.ShowActionFailureMessage(this.GetLocalized("讀取審核階層資料"), xmlResult.Code, xmlResult.Message);
                    return;
                }
            }
            #endregion

            if (isNeedFlow)
            {
                #region [MDY:20160116] 改用審核機制
                string action = this.GetLocalized("修改權限資料申請");

                #region 表單資訊
                string groupName = null;
                int hasRightCount = 0;
                StringBuilder formInfos = new StringBuilder();
                {
                    ListItem item = this.ddlGroup.Items.FindByValue(groupId);
                    if (item != null)
                    {
                        groupName = item.Text;
                    }
                    else
                    {
                        groupName = groupId;
                    }
                    formInfos.AppendFormat("群組：{0};", groupName);
                }
                #endregion

                #region 組參數
                KeyValueList<string> arguments = new KeyValueList<string>();
                arguments.Add("groupID", groupId);
                string funcId = string.Empty;
                string rightCode = string.Empty;
                FuncRight right = new FuncRight();
                foreach (GridViewRow row in gvResult.Rows)
                {
                    funcId = row.Cells[0].Text.Trim();

                    CheckBox chkInsert = row.FindControl("chkInsert") as CheckBox;
                    right.CheckInsertRight = chkInsert != null ? chkInsert.Checked : false;

                    CheckBox chkUpdate = row.FindControl("chkUpdate") as CheckBox;
                    right.CheckUpdateRight = chkUpdate != null ? chkUpdate.Checked : false;

                    CheckBox chkDelete = row.FindControl("chkDelete") as CheckBox;
                    right.CheckDeleteRight = chkDelete != null ? chkDelete.Checked : false;

                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                    right.CheckSelectRight = chkSelect != null ? chkSelect.Checked : false;

                    CheckBox chkPrint = row.FindControl("chkPrint") as CheckBox;
                    right.CheckPrintRight = chkPrint != null ? chkPrint.Checked : false;

                    rightCode = right.GetRightCode();

                    arguments.Add(funcId, rightCode);

                    #region 表單資訊
                    if (right.HasRight())
                    {
                        hasRightCount++;
                        string funcName = row.Cells[1].Text.Trim();
                        string rightText = right.GetRightText();
                        formInfos.AppendFormat("{0}：{1};", funcName, rightText);
                    }
                    #endregion
                }

                #region 表單資訊
                arguments.Add("formInfos", formInfos.ToString());
                #endregion
                #endregion

                FlowDataHelper helper = new FlowDataHelper();

                #region 先檢查是否有審核中的資料
                {
                    Expression where = new Expression(FlowDataEntity.Field.FormId, FormCodeTexts.S5200002)
                        .And(FlowDataEntity.Field.DataKey, groupId)
                        .And(FlowDataEntity.Field.Status, new string[] { FlowStatusCodeTexts.FLOWING, FlowStatusCodeTexts.PROCESSING });
                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.SelectCount<FlowDataEntity>(this.Page, where, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count > 0)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("該群組權限資料審核中");
                            this.ShowSystemMessage(msg);
                            return;
                        }
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = String.Format("{0}，{1}", this.GetLocalized("查詢審核中的群組權限資料失敗"), xmlResult.Message);
                        this.ShowSystemMessage(xmlResult.Code, msg);
                        return;
                    }
                }
                #endregion

                #region 新增流程資料
                {
                    #region 流程資料
                    FlowDataEntity flowData = new FlowDataEntity();
                    flowData.FormId = FormCodeTexts.S5200002;
                    flowData.FormData = helper.GetS5200002FormData(arguments);
                    if (String.IsNullOrEmpty(flowData.FormData))
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("序列化申請資料失敗");
                        this.ShowSystemMessage(msg);
                        return;
                    }
                    flowData.FormDesc = String.Format("群組：{0}; 授權功能：{1} 項", groupName, hasRightCount);

                    flowData.ApplyDate = DateTime.Now;
                    flowData.ApplyKind = ApplyKindCodeTexts.UPDATE;
                    flowData.ApplyUnitId = logonUser.UnitId;
                    flowData.ApplyUserId = logonUser.UserId;
                    flowData.ApplyUserName = logonUser.UserName;
                    flowData.ApplyUserQual = logonUser.UserQual;

                    flowData.DataKey = group.GroupId;
                    flowData.DataUnitId = group.Branchs;
                    flowData.DataRole = group.Role;
                    flowData.DataRoleType = group.RoleType;
                    flowData.DataReceiveType = null;
                    flowData.DataYearId = null;
                    flowData.DataTermId = null;
                    flowData.DataDepId = null;
                    flowData.DataReceiveId = null;

                    flowData.Guid = helper.GetNewFlowGuid();
                    flowData.Status = FlowStatusCodeTexts.FLOWING;
                    #endregion

                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.Insert<FlowDataEntity>(this, flowData, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count < 1)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "新增流程資料失敗，無資料被新增");
                        }
                        else
                        {
                            this.ShowActionSuccessMessage(action);
                        }
                    }
                    else
                    {
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    }
                }
                #endregion
                #endregion
            }
            else
            {
                #region 直接修改資料
                string action = this.GetLocalized("更新權限資料");

                KeyValueList<string> arguments = new KeyValueList<string>();
                arguments.Add("groupID", groupId);
                string funcId = string.Empty;
                string rightCode = string.Empty;
                FuncRight right = new FuncRight();
                foreach (GridViewRow row in gvResult.Rows)
                {
                    funcId = row.Cells[0].Text.Trim();

                    CheckBox chkInsert = row.FindControl("chkInsert") as CheckBox;
                    right.CheckInsertRight = chkInsert != null ? chkInsert.Checked : false;

                    CheckBox chkUpdate = row.FindControl("chkUpdate") as CheckBox;
                    right.CheckUpdateRight = chkUpdate != null ? chkUpdate.Checked : false;

                    CheckBox chkDelete = row.FindControl("chkDelete") as CheckBox;
                    right.CheckDeleteRight = chkDelete != null ? chkDelete.Checked : false;

                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                    right.CheckSelectRight = chkSelect != null ? chkSelect.Checked : false;

                    CheckBox chkPrint = row.FindControl("chkPrint") as CheckBox;
                    right.CheckPrintRight = chkPrint != null ? chkPrint.Checked : false;

                    rightCode = right.GetRightCode();

                    arguments.Add(funcId, rightCode);
                }

                object returnData = null;
                XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.RenewGroupRight, arguments, out returnData);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    return;
                }
                else
                {
                    this.ShowActionSuccessMessage(action);
                }
                #endregion
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

            string groupId = this.ddlGroup.SelectedValue;
            bool isOK = this.GetAndBindQueryData(groupId);
            if (isOK)
            {
                FuncRight[] funcRights = this.gvResult.DataSource as FuncRight[];
                if (funcRights == null || funcRights.Length == 0)
                {
                    this.ShowJsAlert("無資料");
                    return;
                }

                #region DataTable 初始化
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add(new System.Data.DataColumn("功能代碼"));
                dt.Columns.Add(new System.Data.DataColumn("功能名稱"));
                dt.Columns.Add(new System.Data.DataColumn("新增"));
                dt.Columns.Add(new System.Data.DataColumn("修改"));
                dt.Columns.Add(new System.Data.DataColumn("刪除"));
                dt.Columns.Add(new System.Data.DataColumn("查詢"));
                dt.Columns.Add(new System.Data.DataColumn("列印"));
                #endregion

                #region funcRights 轉成 DataTable
                System.Data.DataRow dRow = null;
                foreach (FuncRight funcRight in funcRights)
                {
                    dRow = dt.NewRow();
                    dRow["功能代碼"] = funcRight.FuncId;
                    dRow["功能名稱"] = funcRight.FuncName;
                    dRow["新增"] = funcRight.CheckInsertRight ? "Y" : "N";
                    dRow["修改"] = funcRight.CheckUpdateRight ? "Y" : "N";
                    dRow["刪除"] = funcRight.CheckDeleteRight ? "Y" : "N";
                    dRow["查詢"] = funcRight.CheckSelectRight ? "Y" : "N";
                    dRow["列印"] = funcRight.CheckPrintRight ? "Y" : "N";
                    dt.Rows.Add(dRow);
                }
                #endregion

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                byte[] fileContent = null;
                if (extName == "ODS")
                {
                    #region DataTable 轉 ODS
                    ODSHelper helper = new ODSHelper();
                    fileContent = helper.DataTable2ODS(dt);
                    #endregion
                }
                else
                {
                    #region DataTable 轉 Xls
                    ConvertFileHelper helper = new ConvertFileHelper();
                    fileContent = helper.Dt2Xls(dt);
                    #endregion
                }
                #endregion

                dt.Clear();
                dt.Dispose();
                dt = null;

                #region [MDY:20190906] (2019擴充案) 匯出檔增加 ODS 格式
                if (fileContent == null)
                {
                    this.ShowSystemMessage(String.Format("將匯出資料存成 {0} 檔失敗", extName));
                }
                else
                {
                    #region [MDY:20210401] 原碼修正
                    string fileName = String.Format("{0}_權限資料查詢結果.{1}", HttpUtility.UrlEncode(groupId), extName);
                    #endregion

                    this.ResponseFile(fileName, fileContent, extName);
                }
                #endregion
            }

        }
    }
}