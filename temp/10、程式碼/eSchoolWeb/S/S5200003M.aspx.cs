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
    /// 群組管理 (維護)
    /// </summary>
    public partial class S5200003M : BasePage
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

        #region Keep 頁面參數
        /// <summary>
        /// 編輯模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return ViewState["ACTION"] as string;
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 編輯的群組代碼參數
        /// </summary>
        private string EditGroupId
        {
            get
            {
                return ViewState["EditGroupId"] as string;
            }
            set
            {
                ViewState["EditGroupId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的群組角色參數
        /// </summary>
        private string EditRole
        {
            get
            {
                return ViewState["EditRole"] as string;
            }
            set
            {
                ViewState["EditRole"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的特定單位代碼參數
        /// </summary>
        private string EditBranchs
        {
            get
            {
                return ViewState["EditBranchs"] as string;
            }
            set
            {
                ViewState["EditBranchs"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            LogonUser logonUser = this.GetLogonUser();

            #region [MDY:20161010] 依據資料處理邏輯修正
            if (logonUser.IsBankManager)
            {
                //總行：可維護 所有行員群組 + 所有學校的主管群組
                #region 群組角色
                this.labRole.Text = String.Empty;
                this.labRole.Visible = false;
                this.BindRoleOptions(true);
                #endregion

                #region 權限角色
                this.labRoleType.Text = String.Empty;
                this.labRoleType.Visible = false;
                this.BindRoleTypeOptions(true);
                #endregion

                #region 特定分行選項
                this.labBank.Text = String.Empty;
                this.labBank.Visible = false;
                this.GetAndBindBankOptions(true);
                #endregion

                #region 學校選項
                this.labSchIdenty.Text = String.Empty;
                this.labSchIdenty.Visible = false;
                this.GetAndBindSchIdentyOption(true);
                #endregion
            }
            else if (logonUser.IsBankUser)
            {
                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管：可維護 自己分行的特定群組 + 自己分行的學校的主管群組
                    #region 群組角色
                    this.labRole.Text = String.Empty;
                    this.labRole.Visible = false;
                    this.BindRoleOptions(true);
                    #endregion

                    #region 權限角色
                    this.labRoleType.Text = String.Empty;
                    this.labRoleType.Visible = false;
                    this.BindRoleTypeOptions(true);
                    #endregion

                    #region 特定分行選項
                    this.labBank.Text = String.Format("{0} - {1}", logonUser.UnitId, logonUser.UnitName);
                    this.labBank.Visible = true;
                    this.GetAndBindBankOptions(false);
                    #endregion

                    #region 學校選項
                    this.labSchIdenty.Text = String.Empty;
                    this.labSchIdenty.Visible = false;
                    this.GetAndBindSchIdentyOption(true);
                    #endregion
                }
                else
                {
                    //分行主管、經辦：可維護 自己分行的學校的主管群組
                    #region 群組角色
                    this.labRole.Text = String.Empty;
                    this.labRole.Visible = true;
                    this.BindRoleOptions(false);
                    #endregion

                    #region 權限角色
                    this.labRoleType.Text = String.Empty;
                    this.labRoleType.Visible = true;
                    this.BindRoleTypeOptions(false);
                    #endregion

                    #region 特定分行選項
                    this.labBank.Text = String.Empty;
                    this.labBank.Visible = false;
                    this.GetAndBindBankOptions(false);
                    #endregion

                    #region 學校選項
                    this.labSchIdenty.Text = String.Empty;
                    this.labSchIdenty.Visible = false;
                    this.GetAndBindSchIdentyOption(true);
                    #endregion
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦：可維護 自己學校的經辦群組
                #region 群組角色
                this.labRole.Text = String.Empty;
                this.labRole.Visible = true;
                this.BindRoleOptions(false);
                #endregion

                #region 權限角色
                this.labRoleType.Text = String.Empty;
                this.labRoleType.Visible = true;
                this.BindRoleTypeOptions(false);
                #endregion

                #region 特定分行選項
                this.labBank.Text = String.Empty;
                this.labBank.Visible = false;
                this.GetAndBindBankOptions(false);
                #endregion

                #region 學校選項
                this.labSchIdenty.Text = String.Format("{0} - {1}", logonUser.UnitId, logonUser.UnitName);
                this.labSchIdenty.Visible = true;
                this.GetAndBindSchIdentyOption(false);
                #endregion
            }
            else
            {
                string msg = this.GetLocalized("無維護權限");
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, msg);
                this.ccbtnOK.Visible = false;
                return false;
            }
            #endregion

            this.labGroupPrefix.Text = String.Empty;
            this.labGroupPrefix.Visible = false;
            this.tbxGroupId.Text = String.Empty;
            this.tbxGroupName.Text = String.Empty;
            return true;
        }

        /// <summary>
        /// 結繫群組角色選項
        /// </summary>
        private void BindRoleOptions(bool isVisible)
        {
            this.ddlRole.Visible = isVisible;
            if (isVisible)
            {
                //CodeText[] items = new CodeText[] { new CodeText(RoleCodeTexts.STAFF, RoleCodeTexts.STAFF_TEXT), new CodeText(RoleCodeTexts.SCHOOL, RoleCodeTexts.SCHOOL_TEXT) };
                RoleCodeTexts items = new RoleCodeTexts();
                WebHelper.SetDropDownListItems(this.ddlRole, DefaultItem.Kind.Select, false, items, false, true, 0, items[0].Code);
            }
            else
            {
                this.ddlRole.Items.Clear();
            }
        }

        /// <summary>
        /// 結繫權限角色選項
        /// </summary>
        private void BindRoleTypeOptions(bool isVisible)
        {
            this.rblRoleType.Visible = isVisible;
            if (isVisible)
            {
                //CodeText[] items = new CodeText[] { new CodeText(RoleTypeCodeTexts.MANAGER, "總行"), new CodeText(RoleTypeCodeTexts.USER, "分行") };
                RoleTypeCodeTexts items = new RoleTypeCodeTexts();
                WebHelper.SetRadioButtonListItems(this.rblRoleType, items, true, 2, items[0].Code);
            }
            else
            {
                this.rblRoleType.Items.Clear();
            }
        }

        /// <summary>
        /// 取得並結繫分行選項
        /// </summary>
        private void GetAndBindBankOptions(bool isVisible)
        {
            this.ddlBank.Visible = isVisible;
            if (isVisible)
            {
                Expression where = new Expression();    //因為只有總行會執行此方法，所以取所有分行資料
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(BankEntity.Field.BankNo, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { BankEntity.Field.BankNo };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { BankEntity.Field.BankSName };
                string textCombineFormat = null;

                CodeText[] items = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<BankEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢分行資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }

                WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.All, false, items, true, false, 0, null);
            }
            else
            {
                this.ddlBank.Items.Clear();
            }
        }

        /// <summary>
        /// 取得並結繫學校選項
        /// </summary>
        private void GetAndBindSchIdentyOption(bool isVisible)
        {
            this.ddlSchIdenty.Visible = isVisible;
            if (isVisible)
            {
                LogonUser logonUser = this.GetLogonUser();

                Expression where = null;
                if (logonUser.IsBankManager)
                {
                    //總行：所有學校
                    where = new Expression();
                }
                else if (logonUser.IsBankUser)
                {
                    //分行：自己分行的學校
                    where = new Expression(SchoolRTypeEntity.Field.BankId, logonUser.BankId);
                }
                else if (logonUser.IsSchoolUser)
                {
                    //學校：自己學校
                    where = new Expression(SchoolRTypeEntity.Field.SchIdenty, logonUser.UnitId);
                }

                if (where != null)
                {
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
                        string action = this.GetLocalized("查詢學校資料");
                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                    }
                    WebHelper.SetDropDownListItems(this.ddlSchIdenty, DefaultItem.Kind.Select, false, datas, true, false, 0, null);
                }
                else
                {
                    this.ddlSchIdenty.Items.Clear();
                }
            }
            else
            {
                this.ddlSchIdenty.Items.Clear();
            }
        }

        private void ChangeUIByRole(string role)
        {
            LogonUser logonUser = this.GetLogonUser();
            this.ccbtnOK.Visible = true;
            if (role == RoleCodeTexts.STAFF)
            {
                this.trBank.Visible = true;
                this.trSchIdenty.Visible = !this.trBank.Visible;
                this.labGroupPrefix.Visible = this.trSchIdenty.Visible;

                this.labRoleType.Visible = false;
                this.rblRoleType.Visible = !this.labRoleType.Visible;

                if (logonUser.IsBankManager)
                {
                    this.labRoleType.Visible = false;
                    this.rblRoleType.Visible = !this.labRoleType.Visible;
                }
                else if (logonUser.IsBankUser)
                {
                    if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                    {
                        this.labRoleType.Visible = false;
                        this.rblRoleType.Visible = !this.labRoleType.Visible;
                    }
                    else
                    {
                        this.labRoleType.Visible = true;
                        this.rblRoleType.Visible = !this.labRoleType.Visible;
                    }
                }
                else
                {
                    this.trBank.Visible = false;
                    this.labRoleType.Visible = true;
                    this.rblRoleType.Visible = false;
                    this.ccbtnOK.Visible = false;
                }
            }
            else if (role == RoleCodeTexts.SCHOOL)
            {
                this.trBank.Visible = false;
                this.trSchIdenty.Visible = !this.trBank.Visible;
                this.labGroupPrefix.Visible = this.trSchIdenty.Visible;

                this.labRoleType.Visible = true;
                this.rblRoleType.Visible = !this.labRoleType.Visible;

                if (logonUser.IsBankManager)
                {
                    //this.ddlSchIdenty.Visible = true;
                }
                else if (logonUser.IsBankUser)
                {
                    if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                    {
                        
                    }
                    else
                    {
                        
                    }

                    //this.ddlSchIdenty.Visible = true;
                }
                else if (logonUser.IsSchoolUser)
                {
                    //this.ddlSchIdenty.Visible = false;
                }
                else
                {
                    this.trSchIdenty.Visible = false;
                    this.labRoleType.Visible = true;
                    this.rblRoleType.Visible = false;
                    this.ccbtnOK.Visible = false;
                }
            }
            else
            {
                this.trBank.Visible = false;
                this.trSchIdenty.Visible = false;

                this.labRoleType.Visible = false;
                this.rblRoleType.Visible = false;
                this.ccbtnOK.Visible = false;
            }
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(GroupListEntity data)
        {
            if (data == null)
            {
                this.labRole.Text = String.Empty;
                this.ddlRole.SelectedIndex = -1;
                this.ddlBank.SelectedIndex = -1;
                this.ddlSchIdenty.SelectedIndex = -1;
                this.labGroupPrefix.Text = String.Empty;
                this.tbxGroupId.Text = String.Empty;
                this.tbxGroupName.Text = String.Empty;
                this.labRoleType.Text = String.Empty;
                this.rblRoleType.SelectedIndex = -1;
                this.ChangeUIByRole(null);
                this.ccbtnOK.Visible = false;
                return;
            }

            bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
            bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);

            string role = data.Role == null ? String.Empty : data.Role.Trim();
            string groupId = data.GroupId == null ? String.Empty : data.GroupId.Trim();
            string unitId = data.Branchs == null ? String.Empty : data.Branchs.Trim();
            string roleType = data.RoleType == null ? null : data.RoleType.Trim();

            #region 群組角色
            this.labRole.Text = String.Format("{0} - {1}", role, RoleCodeTexts.GetText(role));
            WebHelper.SetDropDownListSelectedValue(this.ddlRole, role);
            this.ChangeUIByRole(data.Role);
            #endregion

            #region 特定分行 + 學校代碼 + 群組代碼
            if (role == RoleCodeTexts.STAFF)
            {
                WebHelper.SetDropDownListSelectedValue(this.ddlBank, unitId);
                this.ddlSchIdenty.SelectedIndex = -1;

                this.labGroupPrefix.Text = String.Empty;
                this.tbxGroupId.Text = groupId;
            }
            else if (role == RoleCodeTexts.SCHOOL)
            {
                this.ddlBank.SelectedIndex = -1;
                WebHelper.SetDropDownListSelectedValue(this.ddlSchIdenty, unitId);

                if (groupId.Length > unitId.Length && groupId.StartsWith(unitId))
                {
                    this.labGroupPrefix.Text = unitId;
                    this.tbxGroupId.Text = groupId.Substring(unitId.Length);
                }
                else
                {
                    this.labGroupPrefix.Text = String.Empty;
                    this.tbxGroupId.Text = groupId;
                }
            }
            else
            {
                this.ddlBank.SelectedIndex = -1;
                this.ddlSchIdenty.SelectedIndex = -1;
                this.labGroupPrefix.Text = String.Empty;
                this.tbxGroupId.Text = groupId;
            }
            #endregion

            #region 群組名稱
            this.tbxGroupName.Text = data.GroupName == null ? String.Empty : data.GroupName.Trim();
            #endregion

            #region 權限角色
            this.labRoleType.Text = String.Format("{0} - {1}", roleType, RoleTypeCodeTexts.GetText(roleType));
            WebHelper.SetRadioButtonListSelectedValue(this.rblRoleType, roleType);
            #endregion

            #region PKey 欄位
            this.ddlRole.Enabled = isPKeyEditable;
            this.ddlBank.Enabled = isPKeyEditable;
            this.ddlSchIdenty.Enabled = isPKeyEditable;
            this.tbxGroupId.Enabled = isPKeyEditable;
            #endregion

            #region Data 欄位
            this.tbxGroupName.Enabled = isDataEditable;
            this.rblRoleType.Enabled = isDataEditable;
            #endregion

            #region [Old] GroupId
            //if (this.labSchIdenty.Visible)
            //{
            //    string schIdenty = this.labSchIdenty.Text.Trim();
            //    if (groupId.Length > 4 && groupId.StartsWith(schIdenty))
            //    {
            //        this.tbxGroupId.Text = groupId.Substring(4);
            //    }
            //    else
            //    {
            //        this.tbxGroupId.Text = groupId;
            //    }
            //}

            //if (role == RoleCodeTexts.SCHOOL)
            //{
            //    string schIdenty = null;
            //    if (groupId.Length > 4)
            //    {
            //        schIdenty = groupId.Substring(0, 4);
            //        groupId = groupId.Substring(4);
            //    }
            //    else
            //    {
            //        groupId = String.Empty;
            //    }
            //    WebHelper.SetDropDownListSelectedValue(this.ddlSchIdenty, schIdenty);
            //    this.tbxGroupId.Text = groupId;
            //}
            //else
            //{
            //    WebHelper.SetDropDownListSelectedValue(this.ddlSchIdenty, null);
            //    this.tbxGroupId.Text = groupId;
            //}
            #endregion

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得並檢查輸入的編輯資料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool GetAndCheckEditData(out GroupListEntity data)
        {
            LogonUser logonUser = this.GetLogonUser();

            data = new GroupListEntity();

            data.Branchs = String.Empty;

            if (ActionMode.IsPKeyEditableMode(this.Action))
            {
                string groupId = this.tbxGroupId.Text.Trim();
                if (String.IsNullOrEmpty(groupId))
                {
                    this.ShowMustInputAlert("群組代碼");
                    return false;
                }
                if (!Common.IsEnglishNumber(groupId, 1, 4))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「群組代碼」限輸入 1 ~ 4 碼的英文、數字或英數混合字");
                    this.ShowJsAlert(msg);
                    return false;
                }

                if (logonUser.IsBankUser)
                {
                    if (logonUser.IsBankManager)
                    {
                        #region 總行：維護所有行員群組 + 所有學校的經辦群組
                        data.Role = this.ddlRole.SelectedValue;
                        if (String.IsNullOrEmpty(data.Role))
                        {
                            this.ShowMustInputAlert("群組角色");
                            return false;
                        }
                        if (!RoleCodeTexts.IsDefine(data.Role))
                        {
                            string msg = this.GetLocalized("群組角色不正確");
                            this.ShowSystemMessage(msg);
                            return false;
                        }

                        if (data.Role == RoleCodeTexts.STAFF)
                        {
                            //行員
                            data.GroupId = groupId;
                            data.Branchs = this.ddlBank.SelectedValue;
                            data.RoleType = this.rblRoleType.SelectedValue;
                            if (String.IsNullOrEmpty(data.RoleType))
                            {
                                this.ShowMustInputAlert("權限角色");
                                return false;
                            }
                        }
                        else
                        {
                            //學校
                            string schIdenty = this.ddlSchIdenty.SelectedValue;
                            data.GroupId = String.Concat(schIdenty, groupId);
                            data.Branchs = schIdenty;
                            data.RoleType = RoleTypeCodeTexts.MANAGER;          //主管
                        }
                        #endregion
                    }
                    else if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                    {
                        #region 分行主控、會計主管：維護自己分行的行員群組 + 自己分行的學校的經辦群組
                        data.Role = this.ddlRole.SelectedValue;
                        if (String.IsNullOrEmpty(data.Role))
                        {
                            this.ShowMustInputAlert("群組角色");
                            return false;
                        }
                        if (!RoleCodeTexts.IsDefine(data.Role))
                        {
                            string msg = this.GetLocalized("群組角色不正確");
                            this.ShowSystemMessage(msg);
                            return false;
                        }

                        if (data.Role == RoleCodeTexts.STAFF)
                        {
                            //行員
                            data.GroupId = groupId;
                            data.Branchs = logonUser.UnitId;    //自己的分行
                            data.RoleType = this.rblRoleType.SelectedValue;
                            if (String.IsNullOrEmpty(data.RoleType))
                            {
                                this.ShowMustInputAlert("權限角色");
                                return false;
                            }
                        }
                        else
                        {
                            //學校
                            string schIdenty = this.ddlSchIdenty.SelectedValue;
                            data.GroupId = String.Concat(schIdenty, groupId);
                            data.Branchs = schIdenty;
                            data.RoleType = RoleTypeCodeTexts.MANAGER;          //主管
                        }
                        #endregion
                    }
                    else
                    {
                        #region 分行主管、經辦：自己分行的學校的主管群組
                        string schIdenty = this.ddlSchIdenty.SelectedValue;
                        data.Role = RoleCodeTexts.SCHOOL;                   //學校
                        data.GroupId = String.Concat(schIdenty, groupId);
                        data.Branchs = schIdenty;
                        data.RoleType = RoleTypeCodeTexts.MANAGER;          //主管
                        #endregion
                    }
                }
                else if (logonUser.IsSchoolUser)
                {
                    #region 學校：維護自己學校的經辦群組
                    string schIdenty = logonUser.UnitId;
                    data.GroupId = String.Concat(schIdenty, groupId);
                    data.Role = RoleCodeTexts.SCHOOL;           //學校
                    data.Branchs = schIdenty;                   //自己學校
                    data.RoleType = RoleTypeCodeTexts.USER;     //經辦
                    #endregion
                }

                #region 檢查特定的群組代碼
                if (data.GroupId.StartsWith("AD"))
                {
                    string msg = this.GetLocalized("群組代碼不可以 AD 開頭");
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region [Old]
                //if (logonUser.IsSchoolUser)
                //{
                //    #region 學校使用者只能維護學校的使用者群組
                //    data.Role = RoleCodeTexts.SCHOOL;
                //    if (!Common.IsEnglishNumber(groupId, 1, 4))
                //    {
                //        //[TODO] 固定顯示訊息的收集
                //        string msg = this.GetLocalized("「群組代碼」限輸入 1 ~ 4 碼的英文、數字或英數混合字");
                //        this.ShowJsAlert(msg);
                //        return false;
                //    }

                //    string schIdenty = logonUser.BankId;
                //    data.GroupId = String.Concat(schIdenty, groupId);
                //    data.Branchs = schIdenty;
                //    #endregion
                //}
                //else
                //{
                //    #region 銀行使用者可維護銀行的群組與學校的管理者群組
                //    if (logonUser.IsBankManager)
                //    {
                //        data.Role = this.ddlRole.SelectedValue;
                //    }
                //    else
                //    {
                //        data.Role = RoleCodeTexts.SCHOOL;   //分行只能處理學校群組
                //    }
                //    if (String.IsNullOrEmpty(data.Role))
                //    {
                //        this.ShowMustInputAlert("群組角色");
                //        return false;
                //    }

                //    if (data.Role == RoleCodeTexts.SCHOOL)
                //    {
                //        #region 學校群組
                //        if (!Common.IsEnglishNumber(groupId, 1, 4))
                //        {
                //            //[TODO] 固定顯示訊息的收集
                //            string msg = this.GetLocalized("「群組代碼」限輸入 1 ~ 4 碼的英文、數字或英數混合字");
                //            this.ShowJsAlert(msg);
                //            return false;
                //        }

                //        string schIdenty = this.ddlSchIdenty.SelectedValue;
                //        if (String.IsNullOrEmpty(schIdenty))
                //        {
                //            this.ShowMustInputAlert("學校");
                //            return false;
                //        }

                //        data.GroupId = String.Concat(schIdenty, groupId);
                //        data.Branchs = schIdenty;
                //        #endregion
                //    }
                //    else
                //    {
                //        #region 銀行群組
                //        if (!Common.IsEnglishNumber(groupId, 1, 3))
                //        {
                //            //[TODO] 固定顯示訊息的收集
                //            string msg = this.GetLocalized("「群組代碼」限輸入 1 ~ 3 碼的英文、數字或英數混合字");
                //            this.ShowJsAlert(msg);
                //            return false;
                //        }

                //        data.GroupId = groupId;
                //        data.Branchs = String.Empty;
                //        #endregion
                //    }
                //    #endregion
                //}
                #endregion
            }
            else
            {
                data.GroupId = this.EditGroupId;
                if (String.IsNullOrEmpty(data.GroupId))
                {
                    this.ShowMustInputAlert("群組代碼");
                    return false;
                }

                data.Role = this.EditRole;
                data.Branchs = this.EditBranchs;
                if (data.Role == RoleCodeTexts.STAFF)
                {
                    data.RoleType = this.rblRoleType.SelectedValue;
                    if (String.IsNullOrEmpty(data.RoleType))
                    {
                        this.ShowMustInputAlert("權限角色");
                        return false;
                    }
                }
                else
                {
                    if (logonUser.IsSchoolUser)
                    {
                        //學校使用者只能維護學校的經辦群組
                        data.RoleType = RoleTypeCodeTexts.USER;
                    }
                    else
                    {
                        //銀行使用者只能維護學校的主管群組
                        data.RoleType = RoleTypeCodeTexts.MANAGER;
                    }
                }
            }

            

            #region [Old]
            //if (data.Role == RoleCodeTexts.SCHOOL)
            //{
            //    if (logonUser.IsSchoolUser)
            //    {
            //        //學校使用者只能維護學校的使用者群組
            //        data.RoleType = RoleTypeCodeTexts.USER;
            //    }
            //    else
            //    {
            //        //銀行使用者只能維護學校的管理者群組
            //        data.RoleType = RoleTypeCodeTexts.MANAGER;
            //    }
            //}
            //else
            //{
            //    data.RoleType = this.rblRoleType.SelectedValue;
            //    if (String.IsNullOrEmpty(data.RoleType))
            //    {
            //        this.ShowMustInputAlert("權限角色");
            //        return false;
            //    }
            //}
            #endregion

            data.GroupName = this.tbxGroupName.Text.Trim();
            if (String.IsNullOrEmpty(data.GroupName))
            {
                this.ShowMustInputAlert("群組名稱");
                return false;
            }

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LogonUser logonUser = this.GetLogonUser();

                if (!this.InitialUI())
                {
                    return;
                }

                #region 檢查維護權限
                if (!this.HasMaintainAuth())
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
                    return;
                }
                #endregion

                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditGroupId = QueryString.TryGetValue("GroupId", String.Empty);
                bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);

                if (!ActionMode.IsMaintinaMode(this.Action)
                    || (!isPKeyEditable && String.IsNullOrEmpty(this.EditGroupId)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                if (!isPKeyEditable && BankADGroupCodeTexts.IsDefine(this.EditGroupId))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護此資料的權限");
                    return;
                }
                #endregion

                #region 取得維護資料
                GroupListEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new GroupListEntity();
                            if (logonUser.IsBankManager)
                            {
                                //總行 - 預設行員、主管
                                data.Role = RoleCodeTexts.STAFF;
                                data.RoleType = RoleTypeCodeTexts.MANAGER;
                            }
                            else if (logonUser.IsBankUser)
                            {
                                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                                {
                                    //分行主控、會計主管 - 預設行員、主管
                                    data.Role = RoleCodeTexts.STAFF;
                                    data.RoleType = RoleTypeCodeTexts.MANAGER;
                                }
                                else
                                {
                                    //分行主管、經辦 - 預設學校、主管
                                    data.Role = RoleCodeTexts.SCHOOL;
                                    data.RoleType = RoleTypeCodeTexts.MANAGER;
                                }
                            }
                            else if (logonUser.IsSchoolUser)
                            {
                                //學校主管、經辦 - 預設學校、經辦
                                data.Role = RoleCodeTexts.SCHOOL;
                                data.RoleType = RoleTypeCodeTexts.USER;
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢資料
                            Expression where = new Expression(GroupListEntity.Field.GroupId, this.EditGroupId);
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<GroupListEntity>(this, where, null, out data);
                            if (!xmlResult.IsSuccess)
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                return;
                            }
                            if (data == null)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                return;
                            }
                            #endregion

                            this.EditRole = data.Role;
                            this.EditBranchs = data.Branchs;

                            bool isHasAuth = false;
                            #region 檢查資料權限
                            if (logonUser.IsBankManager)
                            {
                                //總行：可維護 所有行員群組 + 所有學校的主管群組
                                isHasAuth = (data.Role == RoleCodeTexts.STAFF
                                    || (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.MANAGER));
                             }
                            else if (logonUser.IsBankUser)
                            {
                                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                                {
                                    //分行主控、會計主管 - 可維護 自己分行的特定群組 + 自己分行的學校的主管群組
                                    isHasAuth = ((data.Role == RoleCodeTexts.STAFF && data.Branchs == logonUser.UnitId)
                                        || (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.MANAGER && logonUser.IsMySchIdenty(data.Branchs)));
                                }
                                else
                                {
                                    //分行主管、經辦 - 可維護 自己分行的學校的主管群組
                                    isHasAuth = (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.MANAGER && logonUser.IsMySchIdenty(data.Branchs));
                                }
                            }
                            else if (logonUser.IsSchoolUser)
                            {
                                //學校主管、經辦 - 可維護 自己學校的經辦群組
                                isHasAuth = (data.Role == RoleCodeTexts.SCHOOL && data.RoleType == RoleTypeCodeTexts.USER && data.Branchs == logonUser.UnitId);
                            }
                            else
                            {
                                isHasAuth = false;
                            }

                            if (!isHasAuth)
                            {
                                //[TODO] 固定顯示訊息的收集
                                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護此資料的權限");
                                return;
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    default:
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("網頁參數不正確");
                        this.ShowSystemMessage(msg);
                        return;
                }
                #endregion

                this.BindEditData(data);
                this.ccbtnOK.Visible = true;
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region [Old]
            //string role = this.ddlRole.SelectedValue;
            //if (role == RoleCodeTexts.STAFF)
            //{
            //    this.tbxGroupId.MaxLength = 3;
            //    this.trRoleType.Visible = true;
            //    this.ddlSchIdenty.Visible = false;
            //}
            //else if (role == RoleCodeTexts.SCHOOL)
            //{
            //    this.tbxGroupId.MaxLength = 4;
            //    this.trRoleType.Visible = false;
            //    this.ddlSchIdenty.Visible = true;
            //}
            //else
            //{
            //    //this.tbxGroupId.MaxLength = 4;
            //    this.trRoleType.Visible = false;
            //    this.ddlSchIdenty.Visible = false;
            //}
            #endregion

            if (this.ddlRole.Visible)
            {
                string role = this.ddlRole.SelectedValue;
                this.ChangeUIByRole(role);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            GroupListEntity data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            string backUrl = "S5200003.aspx";

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
                //只取最後一次修改的資料，避免同學校代號的資料不同步的問題
                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this.Page, where, orderbys, out school);
                if (xmlResult.IsSuccess)
                {
                    if (school != null)
                    {
                        isNeedFlow = school.IsNeedFlow();
                    }
                    else
                    {
                        //無資料則視為 FlowKindCodeTexts.SINGLE
                        isNeedFlow = false;
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
                FlowDataHelper helper = new FlowDataHelper();

                #region 先檢查是否有審核中的資料
                {
                    Expression where = new Expression(FlowDataEntity.Field.FormId, FormCodeTexts.S5200003)
                        .And(FlowDataEntity.Field.DataKey, data.GroupId)
                        .And(FlowDataEntity.Field.Status, new string[] { FlowStatusCodeTexts.FLOWING, FlowStatusCodeTexts.PROCESSING });
                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.SelectCount<FlowDataEntity>(this.Page, where, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count > 0)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("該群組資料審核中");
                            this.ShowSystemMessage(msg);
                            return;
                        }
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = String.Format("{0}，{1}", this.GetLocalized("查詢審核中的群組資料失敗"), xmlResult.Message);
                        this.ShowSystemMessage(xmlResult.Code, msg);
                        return;
                    }
                }
                #endregion

                switch (this.Action)
                {
                    case ActionMode.Insert:     //新增
                        #region 新增
                        {
                            #region [MDY:20161013] 檢查群組代碼唯一
                            {
                                Expression where2 = new Expression(GroupListEntity.Field.GroupId, data.GroupId);
                                int count2 = 0;
                                XmlResult xmlResult2 = DataProxy.Current.SelectCount<GroupListEntity>(this.Page, where2, out count2);
                                if (xmlResult2.IsSuccess)
                                {
                                    if (count2 > 0)
                                    {
                                        //[TODO] 固定顯示訊息的收集
                                        string msg = this.GetLocalized("此群組代碼已存在");
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }
                                }
                                else
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = String.Format("{0}，{1}", this.GetLocalized("查詢群組代碼資料失敗"), xmlResult2.Message);
                                    this.ShowSystemMessage(xmlResult2.Code, msg);
                                    return;
                                }
                            }
                            #endregion

                            string action = this.GetLocalized("新增群組資料申請");

                            #region 補齊表單資料
                            data.Status = DataStatusCodeTexts.NORMAL;
                            data.CrtUser = this.GetLogonUser().UserId;
                            data.CrtDate = DateTime.Now;
                            data.MdyUser = null;
                            data.MdyDate = null;
                            #endregion

                            #region 流程資料
                            FlowDataEntity flowData = new FlowDataEntity();
                            flowData.FormId = FormCodeTexts.S5200003;
                            flowData.FormData = helper.GetS5200003FormData(data);
                            if (String.IsNullOrEmpty(flowData.FormData))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("序列化申請資料失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            flowData.FormDesc = String.Format("群組代碼：{0}; 群組名稱：{1}; 群組角色：{2}; 權限角色：{3}; ", data.GroupId, data.GroupName, data.RoleText, data.RoleTypeText);

                            flowData.ApplyDate = DateTime.Now;
                            flowData.ApplyKind = ApplyKindCodeTexts.INSERT;
                            flowData.ApplyUnitId = logonUser.UnitId;
                            flowData.ApplyUserId = logonUser.UserId;
                            flowData.ApplyUserName = logonUser.UserName;
                            flowData.ApplyUserQual = logonUser.UserQual;

                            flowData.DataKey = data.GroupId;
                            flowData.DataUnitId = data.Branchs;
                            flowData.DataRole = data.Role;
                            flowData.DataRoleType = data.RoleType;
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
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:     //修改
                        #region 修改
                        {
                            string action = this.GetLocalized("修改群組資料申請");

                            #region 補齊表單資料
                            data.Status = DataStatusCodeTexts.NORMAL;
                            //data.CrtUser = null;  //流程更新資料不會處理此欄位
                            //data.CrtDate = null;  //流程更新資料不會處理此欄位
                            data.MdyUser = this.GetLogonUser().UserId;
                            data.MdyDate = DateTime.Now;
                            #endregion

                            #region 流程資料
                            FlowDataEntity flowData = new FlowDataEntity();
                            flowData.FormId = FormCodeTexts.S5200003;
                            flowData.FormData = helper.GetS5200003FormData(data);
                            if (String.IsNullOrEmpty(flowData.FormData))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("序列化申請資料失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            flowData.FormDesc = String.Format("群組代碼：{0}; 群組名稱：{1}; 群組角色：{2}; 權限角色：{3}; ", data.GroupId, data.GroupName, data.RoleText, data.RoleTypeText);

                            flowData.ApplyDate = DateTime.Now;
                            flowData.ApplyKind = ApplyKindCodeTexts.UPDATE;
                            flowData.ApplyUnitId = logonUser.UnitId;
                            flowData.ApplyUserId = logonUser.UserId;
                            flowData.ApplyUserName = logonUser.UserName;
                            flowData.ApplyUserQual = logonUser.UserQual;

                            flowData.DataKey = data.GroupId;
                            flowData.DataUnitId = data.Branchs;
                            flowData.DataRole = data.Role;
                            flowData.DataRoleType = data.RoleType;
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
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Delete:     //刪除
                        #region 刪除
                        {
                            string action = this.GetLocalized("刪除群組資料申請");

                            #region 流程資料
                            FlowDataEntity flowData = new FlowDataEntity();
                            flowData.FormId = FormCodeTexts.S5200003;
                            flowData.FormData = helper.GetS5200003FormData(data);
                            //刪除其實無須表單資料
                            if (String.IsNullOrEmpty(flowData.FormData))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("序列化申請資料失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            flowData.FormDesc = String.Format("群組代碼：{0}; 群組名稱：{1}; 群組角色：{2}; 權限角色：{3}; ", data.GroupId, data.GroupName, data.RoleText, data.RoleTypeText);

                            flowData.ApplyDate = DateTime.Now;
                            flowData.ApplyKind = ApplyKindCodeTexts.DELETE;
                            flowData.ApplyUnitId = logonUser.UnitId;
                            flowData.ApplyUserId = logonUser.UserId;
                            flowData.ApplyUserName = logonUser.UserName;
                            flowData.ApplyUserQual = logonUser.UserQual;

                            flowData.DataKey = data.GroupId;
                            flowData.DataUnitId = data.Branchs;
                            flowData.DataRole = data.Role;
                            flowData.DataRoleType = data.RoleType;
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
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                        #endregion
                        break;
                }
                #endregion
            }
            else
            {
                #region 直接處理資料
                string action = ActionMode.GetActionLocalized(this.Action);
                switch (this.Action)
                {
                    case ActionMode.Insert:     //新增
                        #region 新增
                        {
                            #region 補齊資料
                            data.Status = DataStatusCodeTexts.NORMAL;
                            data.CrtUser = this.GetLogonUser().UserId;
                            data.CrtDate = DateTime.Now;
                            #endregion

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.Insert<GroupListEntity>(this, data, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                                }
                                else
                                {
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:     //修改
                        #region 修改
                        {
                            #region 更新條件
                            Expression where = new Expression(GroupListEntity.Field.GroupId, data.GroupId)
                                .And(GroupListEntity.Field.Role, data.Role);
                            #endregion

                            #region 更新欄位
                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(GroupListEntity.Field.GroupName, data.GroupName);
                            fieldValues.Add(GroupListEntity.Field.RoleType, data.RoleType);
                            //fieldValues.Add(GroupListEntity.Field.Branchs, data.Branchs);
                            fieldValues.Add(GroupListEntity.Field.MdyUser, this.GetLogonUser().UserId);
                            fieldValues.Add(GroupListEntity.Field.MdyDate, DateTime.Now);
                            #endregion

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.UpdateFields<GroupListEntity>(this, where, fieldValues.ToArray(), out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                }
                                else
                                {
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                        #endregion
                        break;
                    case ActionMode.Delete:     //刪除
                        #region 刪除
                        {
                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.Delete<GroupListEntity>(this, data, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                }
                                else
                                {
                                    this.ShowActionSuccessAlert(action, backUrl);
                                }
                            }
                            else
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                            }
                        }
                        #endregion
                        break;
                }
                #endregion
            }
        }
    }
}