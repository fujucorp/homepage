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
    /// 使用者管理 (維護頁)
    /// </summary>
    public partial class S5300001M : BasePage
    {
        #region [權限邏輯：20161010] 依據【土銀學雜使用者帳號管理說明.doc】文件
        // 總行 (AD0、AD5、AD6)
        //   群組選項：除 AD0、AD1、AD2 以外的所有行員群組 (因為這些群組的使用者不存在 USERS 中) + 所有學校的主管帳號
        //   帳號查詢：所有行員的帳號 (除 AD0、AD1、AD2 群組的帳號) + 所有學校的帳號
        //   帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
        // 分行主控、會計主管
        //   群組選項：AD3、AD4 與自己分行的特定分行主管、經辦(含AD5、AD6)群組 + 自己分行的學校的主管群組
        //   帳號查詢：自己分行的帳號 + 自己分行的學校的帳號
        //   帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
        // 分行主管、經辦
        //   群組選項：自己分行的學校的主管群組
        //   帳號查詢：自己分行的學校的帳號
        //   帳號維護：自己分行的學校的主管帳號
        // 學校主管、經辦
        //   群組選項：自己學校的經辦群組
        //   帳號查詢：自己學校的帳號
        //   帳號維護：自己學校的經辦帳號
        #endregion

        #region [Old] 土銀不使用使用者權限
        //#region inner class
        ///// <summary>
        ///// 功能選單權限類別
        ///// </summary>
        //[Serializable]
        //private class FuncRight
        //{
        //    #region Property
        //    private string _FuncId = null;
        //    /// <summary>
        //    /// 功能選單代碼
        //    /// </summary>
        //    public string FuncId
        //    {
        //        get
        //        {
        //            return _FuncId;
        //        }
        //        set
        //        {
        //            _FuncId = value == null ? null : value.Trim();
        //        }
        //    }

        //    private string _FuncName = null;
        //    /// <summary>
        //    /// 功能選單名稱
        //    /// </summary>
        //    public string FuncName
        //    {
        //        get
        //        {
        //            return _FuncName;
        //        }
        //        set
        //        {
        //            _FuncName = value == null ? String.Empty : value.Trim();
        //        }
        //    }

        //    /// <summary>
        //    /// 是否有查詢權限
        //    /// </summary>
        //    public bool HasQueryRight
        //    {
        //        get;
        //        set;
        //    }

        //    /// <summary>
        //    /// 是否有編輯權限
        //    /// </summary>
        //    public bool HasEditRight
        //    {
        //        get;
        //        set;
        //    }

        //    /// <summary>
        //    /// 是否勾選查詢權限
        //    /// </summary>
        //    public bool CheckQueryRight
        //    {
        //        get;
        //        set;
        //    }

        //    /// <summary>
        //    /// 是否勾選編輯權限
        //    /// </summary>
        //    public bool CheckEditRight
        //    {
        //        get;
        //        set;
        //    }
        //    #endregion

        //    #region Constructor
        //    /// <summary>
        //    /// 建構式
        //    /// </summary>
        //    public FuncRight()
        //    {
        //    }

        //    /// <summary>
        //    /// 建構式
        //    /// </summary>
        //    /// <param name="funcId">功能選單代碼</param>
        //    /// <param name="funcName">功能選單名稱</param>
        //    /// <param name="hasRightCode">擁有的權限代碼 (1=編緝 / 2=查詢)</param>
        //    /// <param name="rightCode">勾選的權限代碼 (1=編緝 / 2=查詢)</param>
        //    public FuncRight(string funcId, string funcName, string hasRightCode, string rightCode)
        //    {
        //        this.FuncId = funcId;
        //        this.FuncName = funcName;

        //        hasRightCode = hasRightCode == null ? null : hasRightCode.Trim();
        //        this.HasEditRight = (hasRightCode == FuncRightCodeTexts.MAINTAIN);
        //        this.HasQueryRight = FuncRightCodeTexts.IsDefine(hasRightCode);

        //        rightCode = rightCode == null ? null : rightCode.Trim();
        //        this.CheckEditRight = (rightCode == FuncRightCodeTexts.MAINTAIN);
        //        this.CheckQueryRight = FuncRightCodeTexts.IsDefine(rightCode);
        //    }
        //    #endregion

        //    #region Method
        //    /// <summary>
        //    /// 取得有效的勾選權限代碼 (1=編緝 / 2=查詢)
        //    /// </summary>
        //    /// <returns></returns>
        //    public string GetReallyRightCode()
        //    {
        //        if (this.HasEditRight)
        //        {
        //            if (this.CheckEditRight)
        //            {
        //                return "1";
        //            }
        //            else if (this.CheckQueryRight)
        //            {
        //                return "2";
        //            }
        //        }
        //        else if (this.HasQueryRight)
        //        {
        //            if (this.CheckEditRight || this.CheckQueryRight)
        //            {
        //                return "1";
        //            }
        //        }

        //        return "";
        //    }
        //    #endregion
        //}
        //#endregion
        #endregion

        #region Keep 頁面參數
        /// <summary>
        /// 操作模式參數
        /// </summary>
        private string Action
        {
            get
            {
                return HttpUtility.HtmlEncode(ViewState["ACTION"] as string);
            }
            set
            {
                ViewState["ACTION"] = value == null ? null : value.Trim().ToUpper();
            }
        }

        ///// <summary>
        ///// 編輯的學校統編
        ///// </summary>
        //private string EditBankId
        //{
        //    get
        //    {
        //        return ViewState["EditBankId"] as string;
        //    }
        //    set
        //    {
        //        ViewState["EditBankId"] = value == null ? null : value.Trim();
        //    }
        //}

        /// <summary>
        /// 編輯的使用者參數
        /// </summary>
        private UsersEntity EditUser
        {
            get
            {
                return ViewState["EditUser"] as UsersEntity;
            }
            set
            {
                ViewState["EditUser"] = value;
            }
        }

        /// <summary>
        /// 編輯的使用者群組參數
        /// </summary>
        private GroupListEntity EditGroup
        {
            get
            {
                return ViewState["EditGroup"] as GroupListEntity;
            }
            set
            {
                ViewState["EditGroup"] = value;
            }
        }

        #region [Old] 土銀不使用使用者權限
        ///// <summary>
        ///// 編輯的功能選單資料
        ///// </summary>
        //private CodeText[] EditFuncMenus
        //{
        //    get
        //    {
        //        return ViewState["EditFuncMenus"] as CodeText[];
        //    }
        //    set
        //    {
        //        ViewState["EditFuncMenus"] = value;
        //    }
        //}

        ///// <summary>
        ///// 編輯的群組權限資料
        ///// </summary>
        //private GroupRightEntity[] EditGroupRights
        //{
        //    get
        //    {
        //        return ViewState["EditGroupRights"] as GroupRightEntity[];
        //    }
        //    set
        //    {
        //        ViewState["EditGroupRights"] = value;
        //    }
        //}
        #endregion
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            #region [MDY:20161010] 依據權限邏輯修正
            bool isOK = true;
            LogonUser logonUser = this.GetLogonUser();
            if (logonUser.IsBankManager)
            {
                //總行 (AD0、AD5、AD6) - 帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
                this.trRole.Visible = true;         //群組角色可選則
                this.trSchIdenty.Visible = true;    //學校代碼可選則
                this.trBankId.Visible = true;       //分行代號可選則
                this.ddlBank.Visible = true;
                this.labBank.Visible = false;
                this.trReceiveType.Visible = false; //商家代號不可選，因為只有學校可以管理學校經辦

                this.BindRoleOptions();
                this.GetAndBindSchIdentyOptions(logonUser, null);
                this.GetAndBindBankOptions(null);

                //初始化不用執行，因為結繫資料的時候會執行
                //this.ChangeRole();
            }
            else if (logonUser.IsBankUser)
            {
                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管 - 帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
                    this.trRole.Visible = true;         //群組角色可選則
                    this.trSchIdenty.Visible = true;    //學校代碼可選則
                    this.trBankId.Visible = true;       //分行代號固定值
                    this.ddlBank.Visible = false;
                    this.labBank.Visible = true;
                    this.trReceiveType.Visible = false; //商家代號不可選，因為只有學校可以管理學校經辦

                    this.BindRoleOptions();
                    this.GetAndBindSchIdentyOptions(logonUser, null);
                    this.labBank.Text = String.Format("{0} - {1}", logonUser.BankId, logonUser.UnitName);

                    //初始化不用執行，因為結繫資料的時候會執行
                    //this.ChangeRole();
                }
                else
                {
                    //分行主管、經辦 - 帳號維護：自己分行的學校的主管帳號
                    this.trRole.Visible = false;        //群組角色不能選 (固定為學校)
                    this.trSchIdenty.Visible = true;    //學校代碼可選則
                    this.trBankId.Visible = false;      //分行代號不能選
                    this.trReceiveType.Visible = false; //商家代號不可選，因為只有學校可以管理學校經辦

                    this.GetAndBindSchIdentyOptions(logonUser, null);
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦 - 帳號維護：自己學校的經辦帳號
                this.trRole.Visible = false;        //群組角色不能選
                this.trSchIdenty.Visible = false;   //學校代碼不能選
                this.trBankId.Visible = false;      //分行代號不能選
                this.trReceiveType.Visible = true;  //商家代號可選擇，因為只有學校可以管理學校經辦
            }
            else
            {
                //無權限
                isOK = false;
                this.trRole.Visible = false;
                this.trSchIdenty.Visible = false;
                this.trBankId.Visible = false;
                this.trReceiveType.Visible = false;
                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
            }

            this.tbxUserId.Text = String.Empty;
            this.tbxUserName.Text = String.Empty;

            #region [MDY:20220530] Checkmarx 調整
            this.tbxNewPXX.Text = String.Empty;
            this.tbxConfirmPXX.Text = String.Empty;
            #endregion

            this.tbxTel.Text = String.Empty;
            this.tbxEmail.Text = String.Empty;
            this.chkLock.Checked = false;

            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            this.trNewPXX.Visible = false;
            this.trConfirmPXX.Visible = false;
            #endregion
            #endregion

            this.trLock.Visible = false;

            //土銀不使用使用者權限，永遠不顯示
            this.gvUserRight.Visible = false;

            this.ccbtnOK.Visible = isOK;
            return isOK;
            #endregion
        }

        /// <summary>
        /// 結繫群組角色選項
        /// </summary>
        private void BindRoleOptions()
        {
            CodeText[] items = new CodeText[] { new CodeText(RoleCodeTexts.STAFF, RoleCodeTexts.STAFF_TEXT), new CodeText(RoleCodeTexts.SCHOOL, RoleCodeTexts.SCHOOL_TEXT) };
            WebHelper.SetDropDownListItems(this.ddlRole, DefaultItem.Kind.Select, false, items, false, true, 0, items[0].Code);
        }

        /// <summary>
        /// 取得並結繫分行選項
        /// </summary>
        /// <param name="selectedValue"></param>
        private void GetAndBindBankOptions(string selectedValue)
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

            WebHelper.SetDropDownListItems(this.ddlBank, DefaultItem.Kind.Select, false, items, true, false, 0, selectedValue);
        }

        /// <summary>
        /// 取得並結繫學校選項
        /// </summary>
        /// <param name="logonUser"></param>
        /// <param name="selectedValue"></param>
        private void GetAndBindSchIdentyOptions(LogonUser logonUser, string selectedValue)
        {
            Expression where = null;
            if (logonUser.IsBankManager)
            {
                //總行 (AD0、AD5、AD6) - 帳號維護：所有學校的主管帳號
                where = new Expression();
            }
            else if (logonUser.IsBankUser)
            {
                //分行 - 帳號維護：自己分行的學校的主管帳號
                where = new Expression(SchoolRTypeEntity.Field.BankId, logonUser.BankId);
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校 - 帳號維護：自己學校的經辦帳號
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

                WebHelper.SetDropDownListItems(this.ddlSchIdenty, DefaultItem.Kind.Select, false, datas, true, false, 0, selectedValue);
            }
            else
            {
                this.ddlSchIdenty.Items.Clear();
            }

            this.ddlSchIdenty_SelectedIndexChanged(this.ddlSchIdenty, EventArgs.Empty);
        }

        /// <summary>
        /// 取得並結繫群組選項
        /// </summary>
        /// <param name="role"></param>
        /// <param name="schIdenty"></param>
        /// <param name="selectedValue"></param>
        private void GetAndBindGroupOptions(string role, string schIdenty, string selectedValue)
        {
            #region [MDY:20161010] 依據權限邏輯修正
            LogonUser logonUser = this.GetLogonUser();

            Expression where = null;
            if (logonUser.IsBankManager)
            {
                //總行 (AD0、AD5、AD6) - 群組選項：除 AD0、AD1、AD2 以外的所有行員群組 (因為這些群組的使用者不存在 USERS 中) + 所有學校的主管帳號
                switch (role)
                {
                    case RoleCodeTexts.STAFF:   //行員
                        where = new Expression(GroupListEntity.Field.Role, role)                                            //行員
                            .And(GroupListEntity.Field.GroupId, RelationEnum.NotIn, BankADGroupCodeTexts.GetInADCodes());   //除 AD0、AD1、AD2 外
                        break;
                    case RoleCodeTexts.SCHOOL:  //學校
                        if (!String.IsNullOrEmpty(schIdenty))   //因為群組選項會被學校連動，所以只取指定學校代碼的資料
                        {
                            where = new Expression(GroupListEntity.Field.Role, role)                //學校
                                .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER)     //主管
                                .And(GroupListEntity.Field.Branchs, schIdenty);                     //指定學校代碼
                        }
                        break;
                }
            }
            else if (logonUser.IsBankUser)
            {
                //分行主控、會計主管 - 群組選項：AD3、AD4 與自己分行的特定分行主管、經辦(含AD5、AD6)群組 + 自己分行的學校的主管群組
                //分行主管、經辦 - 群組選項：自己學校的主管群組
                switch (role)
                {
                    case RoleCodeTexts.STAFF:   //行員
                        if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                        {
                            //分行主控、會計主管：AD3、AD4 或 自己分行的特定群組
                            Expression w1 = new Expression(GroupListEntity.Field.GroupId, new string[] { BankADGroupCodeTexts.AD3, BankADGroupCodeTexts.AD4 })  //AD3、AD4 群組
                                .Or(GroupListEntity.Field.Branchs, logonUser.UnitId);    //或自己分行的特定群組
                            where = new Expression(GroupListEntity.Field.Role, RoleCodeTexts.STAFF)     //行員
                                .And(w1);                                                               //AD3、AD4 或 自己分行的特定群組
                        }
                        else
                        {
                            //分行主管、經辦：沒有行員群組的權限
                        }
                        break;
                    case RoleCodeTexts.SCHOOL:  //學校
                        if (!String.IsNullOrEmpty(schIdenty))   //因為群組選項會被學校連動，所以只取指定學校代碼的資料
                        {
                            where = new Expression(GroupListEntity.Field.Role, role)                //學校
                                .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.MANAGER)     //主管
                                .And(GroupListEntity.Field.Branchs, schIdenty);                     //指定學校代碼
                        }
                        break;
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦 - 群組選項：自己學校的經辦群組
                role = RoleCodeTexts.SCHOOL;
                schIdenty = logonUser.UnitId;
                where = new Expression(GroupListEntity.Field.Role, role)            //學校
                    .And(GroupListEntity.Field.RoleType, RoleTypeCodeTexts.USER)    //經辦
                    .And(GroupListEntity.Field.Branchs, schIdenty);                 //自己學校
            }

            if (where != null)
            {
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(GroupListEntity.Field.GroupId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { GroupListEntity.Field.GroupId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { GroupListEntity.Field.GroupName };
                string textCombineFormat = null;

                CodeText[] items = null;
                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<GroupListEntity>(this, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out items);
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢群組代碼資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }

                WebHelper.SetDropDownListItems(this.ddlGroup, DefaultItem.Kind.Select, false, items, true, false, 0, selectedValue);
            }
            else
            {
                this.ddlGroup.Items.Clear();
            }
            #endregion
        }

        /// <summary>
        /// 取得並結繫商家代號選項
        /// </summary>
        /// <param name="schIdenty"></param>
        /// <param name="selectedValues"></param>
        private void GetAndBindReceiveTypeOptions(string schIdenty, string[] selectedValues)
        {
            LogonUser logonUser = this.GetLogonUser();

            if (logonUser.IsBankUser)
            {
                if (!logonUser.IsMySchIdenty(schIdenty))
                {
                    schIdenty = null;
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                schIdenty = logonUser.UnitId;
            }
            else
            {
                schIdenty = null;
            }

            CodeText[] items = null;
            if (!String.IsNullOrEmpty(schIdenty))
            {
                XmlResult xmlResult = null;
                if (logonUser.IsBankUser)
                {
                    xmlResult = DataProxy.Current.GetReceiveTypeCodeTextsBySchool(this, schIdenty, out items);
                }
                else
                {
                    xmlResult = DataProxy.Current.GetMyReceiveTypeCodeTexts(this, out items);
                }
                if (!xmlResult.IsSuccess)
                {
                    string action = this.GetLocalized("查詢學校所有商家代號資料");
                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                }
            }

            if (items != null)
            {
                foreach (CodeText item in items)
                {
                    item.Text = String.Format("{0}({1})", item.Text, item.Code);
                }
            }
            WebHelper.SetCheckBoxListItems(this.cblReceiveType, items, false, 2, selectedValues);
        }

        /// <summary>
        /// 依據群組角色改變輸入介面
        /// </summary>
        private void ChangeUIByRole()
        {
            #region [MDY:20220530] Checkmarx 調整
            #region [MDY:20210401] 原碼修正
            LogonUser logonUser = this.GetLogonUser();
            string role = this.ddlRole.SelectedValue;
            if (role == RoleCodeTexts.STAFF)
            {
                this.trSchIdenty.Visible = false;
                this.trNewPXX.Visible = false;     //行員的密碼在 AD
                this.trConfirmPXX.Visible = false; //行員的密碼在 AD
                this.trBankId.Visible = true;
                this.tbxUserId.MaxLength = 6;           //行員的帳號只有6個字
                this.labUserIdComment.Text = this.GetLocalized(DataFormat.GetBankUserIdComment());
                this.trLock.Visible = false;    //行員沒有鎖定
            }
            else if (role == RoleCodeTexts.SCHOOL)
            {
                this.trSchIdenty.Visible = logonUser.IsBankUser;        //行員才能指定學校
                this.trNewPXX.Visible = ActionMode.IsDataEditableMode(this.Action);    //維護模式才可以設定密碼
                this.trConfirmPXX.Visible = this.trNewPXX.Visible;
                this.trBankId.Visible = false;
                this.tbxUserId.MaxLength = DataFormat.UserIdMaxSize;    //學校帳號最大長度
                this.labUserIdComment.Text = this.GetLocalized(DataFormat.GetUserIdComment());
                this.trLock.Visible = this.Action != ActionMode.Insert; //非新增模式的時候才能顯示 Lock
            }
            else
            {
                this.trSchIdenty.Visible = false;
                this.trNewPXX.Visible = false;
                this.trConfirmPXX.Visible = false;
                this.trBankId.Visible = false;
                this.tbxUserId.MaxLength = 0;
                this.labUserIdComment.Text = null;
                this.trLock.Visible = false;
            }
            #endregion
            #endregion
        }

        /// <summary>
        /// 結繫編輯的資料
        /// </summary>
        /// <param name="action"></param>
        /// <param name="schoolName"></param>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <param name="funcMenus"></param>
        /// <param name="groupRights"></param>
        /// <param name="userRights"></param>
        private void BindEditData(string action, UsersEntity user, GroupListEntity group, CodeText[] funcMenus, GroupRightEntity[] groupRights, UsersRightEntity[] userRights)
        {
            this.tbxUserId.Text = user.UId;
            this.tbxUserName.Text = user.UName;

            #region [MDY:20220530] Checkmarx 調整
            this.tbxNewPXX.Text = String.Empty;
            this.tbxConfirmPXX.Text = String.Empty;
            #endregion

            this.tbxTel.Text = user.Tel;
            this.tbxEmail.Text = user.Email;
            this.chkLock.Checked = user.IsLocked();

            #region 群組角色
            string role = group.Role == null ? null : group.Role.Trim();
            string roleType = group.RoleType == null ? null : group.RoleType.Trim();
            WebHelper.SetDropDownListSelectedValue(this.ddlRole, role);
            #endregion

            switch (role)
            {
                case RoleCodeTexts.STAFF:
                    #region 行員
                    {
                        #region 群組
                        this.GetAndBindGroupOptions(role, null, user.UGrp);
                        #endregion

                        #region 分行
                        WebHelper.SetDropDownListSelectedValue(this.ddlBank, user.UBank);
                        #endregion
                    }
                    #endregion
                    break;
                case RoleCodeTexts.SCHOOL:
                    #region 學校
                    {
                        #region 學校代碼 & 群組
                        WebHelper.SetDropDownListSelectedValue(this.ddlSchIdenty, user.UBank);
                        this.ddlSchIdenty_SelectedIndexChanged(this.ddlSchIdenty, EventArgs.Empty);
                        WebHelper.SetDropDownListSelectedValue(this.ddlGroup, user.UGrp);
                        #endregion

                        #region 商家代號
                        if (roleType == RoleTypeCodeTexts.USER)     //學校經辦才需要指定商家代號
                        {
                            string[] receiveTypes = null;
                            if (user.URt != null)
                            {
                                receiveTypes = user.URt.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (receiveTypes.Length > 0)
                                {
                                    for (int idx = 0; idx < receiveTypes.Length; idx++)
                                    {
                                        receiveTypes[idx] = receiveTypes[idx].Trim();
                                    }
                                }
                            }
                            WebHelper.SetCheckBoxListSelectedValues(this.cblReceiveType, receiveTypes);

                            //因為只有學校可以建立學校經辦的使用者，所以 trReceiveType 是否顯示與 user.URt 脫鉤
                            //this.trReceiveType.Visible = true;
                        }
                        else
                        {
                            //因為只有學校可以建立學校經辦的使用者，所以 trReceiveType 是否顯示與 user.URt 脫鉤
                            //this.trReceiveType.Visible = false;
                        }
                        #endregion
                    }
                    #endregion
                    break;
            }

            switch (action)
            {
                case ActionMode.Insert:
                    #region 新增
                    {
                        this.ddlRole.Enabled = true;
                        this.ddlSchIdenty.Enabled = true;
                        this.ddlGroup.Enabled = true;
                        this.cblReceiveType.Enabled = true;
                        this.ddlBank.Enabled = true;
                        this.tbxUserId.Enabled = true;
                        this.tbxUserName.Enabled = true;
                        this.tbxTel.Enabled = true;
                        this.tbxEmail.Enabled = true;
                        this.chkLock.Enabled = true;

                        //this.trLock.Visible = false;

                        this.ChangeUIByRole();
                        //this.gvUserRight.Visible = false;
                    }
                    #endregion
                    break;
                case ActionMode.Modify:
                    #region 修改
                    {
                        this.ddlRole.Enabled = false;
                        this.ddlSchIdenty.Enabled = false;
                        this.ddlGroup.Enabled = false;
                        this.cblReceiveType.Enabled = true;
                        this.ddlBank.Enabled = false;
                        this.tbxUserId.Enabled = false;
                        this.tbxUserName.Enabled = true;
                        this.tbxTel.Enabled = true;
                        this.tbxEmail.Enabled = true;
                        this.chkLock.Enabled = true;

                        //this.trLock.Visible = true;
                        this.ChangeUIByRole();

                        #region [Old] 土銀不使用使用者權限
                        //this.gvUserRight.Visible = true;

                        //this.BindUserRight(funcMenus, groupRights, userRights);
                        #endregion

                        //this.gvUserRight.Visible = false;

                        //if (this.ddlBank.Visible && group.Role == RoleCodeTexts.STAFF)
                        //{
                        //    WebHelper.SetDropDownListSelectedValue(this.ddlBank, user.UBank);
                        //}
                        //this.ddlBank.Enabled = false;
                    }
                    #endregion
                    break;
                case ActionMode.Delete:
                    #region 刪除
                    {
                        this.ddlRole.Enabled = false;
                        this.ddlSchIdenty.Enabled = false;
                        this.ddlGroup.Enabled = false;
                        this.cblReceiveType.Enabled = false;
                        this.ddlBank.Enabled = false;
                        this.tbxUserId.Enabled = false;
                        this.tbxUserName.Enabled = false;
                        this.tbxTel.Enabled = false;
                        this.tbxEmail.Enabled = false;
                        this.chkLock.Enabled = false;

                        this.ChangeUIByRole();

                        //this.trNewPXX.Visible = false;
                        //this.trConfirmPXX.Visible = false;
                        //this.trLock.Visible = false;
                        //this.gvUserRight.Visible = false;

                        //if (this.ddlBank.Visible && group.Role == RoleCodeTexts.STAFF)
                        //{
                        //    WebHelper.SetDropDownListSelectedValue(this.ddlBank, user.UBank);
                        //}
                        //this.ddlBank.Enabled = false;
                    }
                    #endregion
                    break;
            }

            //WebHelper.SetDropDownListSelectedValue(this.ddlGroup, user.UGrp);
            //this.ddlGroup_SelectedIndexChanged(this.ddlGroup, EventArgs.Empty);
        }

        #region [Old] 土銀不使用使用者權限
        ///// <summary>
        ///// 結繫使用者權限
        ///// </summary>
        ///// <param name="funcMenus"></param>
        ///// <param name="groupRights"></param>
        ///// <param name="userRights"></param>
        //private void BindUserRight(CodeText[] funcMenus, GroupRightEntity[] groupRights, UsersRightEntity[] userRights)
        //{
        //    List<FuncRight> funcRights = null;
        //    if (funcMenus != null && funcMenus.Length > 0)
        //    {
        //        funcRights = new List<FuncRight>(funcMenus.Length);
        //        foreach (CodeText funcMenu in funcMenus)
        //        {
        //            string funcId = funcMenu.Code;
        //            if (String.IsNullOrEmpty(funcId) || funcId.Length <= 3)
        //            {
        //                //沒有代碼或長度小於等於 3 (視為 Menu) 不處理
        //                continue;
        //            }

        //            string gorupRightCode = null;
        //            if (groupRights != null && groupRights.Length > 0)
        //            {
        //                GroupRightEntity groupRight = Array.Find<GroupRightEntity>(groupRights, (x => x.FuncId == funcId));
        //                if (groupRight != null)
        //                {
        //                    gorupRightCode = groupRight.RightCode;
        //                }
        //            }
        //            if (gorupRightCode == null)
        //            {
        //                continue;
        //            }

        //            string userRightCode = null;
        //            if (userRights != null && userRights.Length > 0)
        //            {
        //                UsersRightEntity userRight = Array.Find<UsersRightEntity>(userRights, (x => x.FuncId == funcId));
        //                if (userRight != null)
        //                {
        //                    userRightCode = userRight.RightCode;
        //                }
        //            }

        //            funcRights.Add(new FuncRight(funcMenu.Code, funcMenu.Text, gorupRightCode, userRightCode));
        //        }
        //    }

        //    this.gvUserRight.DataSource = funcRights.ToArray();
        //    this.gvUserRight.DataBind();
        //}
        #endregion

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <returns></returns>
        private bool InsertData()
        {
            LogonUser logonUser = this.GetLogonUser();

            UsersEntity data = this.EditUser;

            #region 群組角色
            string role = null;
            if (logonUser.IsBankManager)
            {
                //總行 (AD0、AD5、AD6) - 帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
                role = this.ddlRole.SelectedValue;
            }
            else if (logonUser.IsBankUser)
            {
                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                {
                    //分行主控、會計主管 - 帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
                    role = this.ddlRole.SelectedValue;
                }
                else
                {
                    //分行主管、經辦 - 帳號維護：自己分行的學校的主管帳號
                    role = RoleCodeTexts.SCHOOL;
                }
            }
            else if (logonUser.IsSchoolUser)
            {
                //學校主管、經辦 - 帳號維護：自己學校的經辦帳號
                role = RoleCodeTexts.SCHOOL;
            }
            if (String.IsNullOrEmpty(role))
            {
                this.ShowMustInputAlert("群組角色");
                return false;
            }
            #endregion

            #region 所屬單位
            string bankId = null;
            if (role == RoleCodeTexts.STAFF)
            {
                if (logonUser.IsBankManager)
                {
                    bankId = this.ddlBank.SelectedValue;
                    if (String.IsNullOrEmpty(bankId))
                    {
                        this.ShowMustInputAlert("分行代號");
                        return false;
                    }
                }
                else
                {
                    bankId = logonUser.BankId;
                }
            }
            else if (role == RoleCodeTexts.SCHOOL)
            {
                if (logonUser.IsBankUser)
                {
                    bankId = this.ddlSchIdenty.SelectedValue;
                    if (String.IsNullOrEmpty(bankId))
                    {
                        this.ShowMustInputAlert("學校代碼");
                        return false;
                    }
                }
                else
                {
                    bankId = logonUser.BankId;
                }
            }
            else
            {
                this.ShowSystemMessage("群組角色不正確");
                return false;
            }
            #endregion

            #region [Old] 所屬單位
            //string role = this.ddlRole.SelectedValue;
            //string bankId = null;
            //if (logonUser.IsBankUser)
            //{
            //    if (role == RoleCodeTexts.SCHOOL)
            //    {
            //        bankId = this.ddlSchIdenty.SelectedValue;
            //        if (String.IsNullOrEmpty(bankId))
            //        {
            //            this.ShowMustInputAlert("學校代碼");
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        //TODO 應該要有分行選項
            //        if (logonUser.IsBankManager)
            //        {
            //            bankId = this.ddlBank.SelectedValue;
            //            if (String.IsNullOrEmpty(bankId))
            //            {
            //                this.ShowMustInputAlert("分行代號");
            //                return false;
            //            }
            //        }
            //        else
            //        {
            //            bankId = logonUser.BankId;
            //        }
            //    }
            //}
            //else
            //{
            //    bankId = logonUser.BankId;
            //}
            #endregion

            #region [Old]
            //#region 所屬學校統編
            ////string bankId = this.EditBankId;
            //string bankId = data.UBank;
            //#endregion
            #endregion

            #region 所屬群組
            string groupId = this.ddlGroup.SelectedValue;
            if (String.IsNullOrEmpty(groupId))
            {
                this.ShowMustInputAlert("群組");
                return false;
            }
            #endregion

            #region 商家代號
            string receiveType = null;
            if (role == RoleCodeTexts.STAFF)
            {
                receiveType = String.Empty;
            }
            else if (role == RoleCodeTexts.SCHOOL)
            {
                if (this.trReceiveType.Visible)
                {
                    //trReceiveType 顯示表示使用者是學校經辦
                    List<string> list = new List<string>(this.cblReceiveType.Items.Count);
                    foreach (ListItem item in this.cblReceiveType.Items)
                    {
                        if (item.Selected)
                        {
                            list.Add(item.Value);
                        }
                    }
                    if (list.Count == 0)
                    {
                        this.ShowMustInputAlert("商家代號");
                        return false;
                    }
                    else
                    {
                        receiveType = String.Concat(",", String.Join(",", list.ToArray()), ",");
                    }
                }
                else
                {
                    //trReceiveType 不顯示表示使用者是學校主管
                    receiveType = "*";  //學校主管
                }
            }
            else
            {
                this.ShowSystemMessage("群組角色不正確");
                return false;
            }
            #endregion

            #region [Old] 商家代號
            //string receiveType = null;
            //if (this.trReceiveType.Visible)
            //{
            //    //trReceiveType 顯示表示使用者是學校經辦
            //    receiveType = ",";
            //    foreach (ListItem item in this.cblReceiveType.Items)
            //    {
            //        if (item.Selected)
            //        {
            //            receiveType += item.Value + ",";
            //        }
            //    }
            //    if (receiveType.Length <= 1)
            //    {
            //        this.ShowMustInputAlert("商家代號");
            //        return false;
            //    }
            //}
            //else
            //{
            //    //trReceiveType 不顯示表示使用者是學校主管或行員
            //    if (role == RoleCodeTexts.SCHOOL)
            //    {
            //        receiveType = "*";  //學校主管
            //    }
            //    else
            //    {
            //        receiveType = String.Empty; //行員
            //    }
            //}
            #endregion

            #region 使用者帳號
            string userId = this.tbxUserId.Text.Trim();
            if (String.IsNullOrEmpty(userId))
            {
                this.ShowMustInputAlert("使用者帳號");
                return false;
            }
            if (role == RoleCodeTexts.STAFF)
            {
                if (!DataFormat.CheckBankUserIDFormat(userId))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「使用者帳號」限輸入" + DataFormat.GetBankUserIdComment());
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            else
            {
                if (!DataFormat.CheckUserIDFormat(userId))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「使用者帳號」限輸入" + DataFormat.GetUserIdComment());
                    this.ShowSystemMessage(msg);
                    return false;
                }
            }
            #endregion

            #region 使用者名稱
            string userName = this.tbxUserName.Text.Trim();
            if (String.IsNullOrEmpty(userName))
            {
                this.ShowMustInputAlert("使用者名稱");
                return false;
            }
            #endregion

            #region 密碼
            #region [MDY:20220530] Checkmarx 調整
            string newPXX = String.Empty;
            if (role == RoleCodeTexts.SCHOOL)
            {
                newPXX = this.tbxNewPXX.Text.Trim();
                if (String.IsNullOrEmpty(newPXX))
                {
                    this.ShowMustInputAlert("使用者密碼");
                    return false;
                }
                if (newPXX != this.tbxConfirmPXX.Text.Trim())
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("「確認密碼」與「使用者密碼」不相同");
                    this.ShowSystemMessage(msg);
                    return false;
                }

                #region [MDY:20181206] 改為 不可3個以上(連續)相同或連號的英數字元 (20181201_03)
                if (!DataFormat.CheckUserPXXFormat(newPXX))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized(String.Format("「使用者密碼」限輸入 {0} ~ {1} 碼英數字混合字串，且不可含連續3個(或以上)相同或連號的英文或數字", DataFormat.UserPXXMinSize, DataFormat.UserPXXMaxSize));
                    this.ShowSystemMessage(msg);
                    return false;
                }
                #endregion

                #region [MDY:20181206] 增加不可與帳號相同 (20181201_03)
                if (newPXX.Equals(userId))
                {
                    this.ShowSystemMessage("密碼不可與帳號相同");
                    return false;
                }
                #endregion
            }
            #endregion
            #endregion

            #region 電話
            string tel = this.tbxTel.Text.Trim();
            #endregion

            #region E-Mail
            string email = this.tbxEmail.Text.Trim();
            #endregion

            XmlResult xmlResult = DataProxy.Current.S530001InsertData(this, userId, groupId, bankId, receiveType, newPXX, userName, tel, email);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Insert);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            return xmlResult.IsSuccess;
        }

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <returns></returns>
        private bool UpdateData()
        {
            #region 使用者帳號
            UsersEntity user = this.EditUser;
            if (user == null)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得被修改的使用者資料");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region 所屬群組
            //GroupListEntity group = this.EditGroup;
            string role = this.EditGroup.Role;
            if (!RoleCodeTexts.IsDefine(role))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得被修改的使用者所屬群組資料");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            #region 商家代號
            string newReceiveType = null;
            if (role == RoleCodeTexts.STAFF)
            {
                newReceiveType = String.Empty;
            }
            else if (role == RoleCodeTexts.SCHOOL)
            {
                if (this.trReceiveType.Visible)
                {
                    //trReceiveType 顯示表示使用者是學校經辦
                    List<string> list = new List<string>(this.cblReceiveType.Items.Count);
                    foreach (ListItem item in this.cblReceiveType.Items)
                    {
                        if (item.Selected)
                        {
                            list.Add(item.Value);
                        }
                    }
                    if (list.Count == 0)
                    {
                        this.ShowMustInputAlert("商家代號");
                        return false;
                    }
                    else
                    {
                        newReceiveType = String.Concat(",", String.Join(",", list.ToArray()), ",");
                    }
                }
                else
                {
                    //trReceiveType 不顯示表示使用者是學校主管
                    newReceiveType = "*";  //學校主管
                }
            }
            else
            {
                this.ShowSystemMessage("群組角色不正確");
                return false;
            }
            #endregion

            #region [Old] 商家代號
            //string newReceiveType = null;
            //if (this.trReceiveType.Visible)
            //{
            //    //trReceiveType 顯示表示使用者是學校經辦
            //    newReceiveType = ",";
            //    foreach (ListItem item in this.cblReceiveType.Items)
            //    {
            //        if (item.Selected)
            //        {
            //            newReceiveType += item.Value + ",";
            //        }
            //    }
            //    if (newReceiveType.Length <= 1)
            //    {
            //        this.ShowMustInputAlert("商家代號");
            //        return false;
            //    }
            //}
            //else
            //{
            //    //trReceiveType 不顯示表示使用者是學校主管或行員
            //    if (role == RoleCodeTexts.SCHOOL)
            //    {
            //        newReceiveType = "*";  //學校主管
            //    }
            //    else
            //    {
            //        newReceiveType = String.Empty; //行員
            //    }
            //}
            #endregion

            #region 使用者名稱
            string userName = this.tbxUserName.Text.Trim();
            if (String.IsNullOrEmpty(userName))
            {
                this.ShowMustInputAlert("使用者名稱");
                return false;
            }
            #endregion

            #region 密碼
            #region [MDY:20220530] Checkmarx 調整
            string newPXX = null;
            if (role == RoleCodeTexts.SCHOOL)
            {
                newPXX = this.tbxNewPXX.Text.Trim();
                if (!String.IsNullOrEmpty(newPXX))
                {
                    if (newPXX != this.tbxConfirmPXX.Text.Trim())
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「確認密碼」與「使用者密碼」不相同");
                        this.ShowSystemMessage(msg);
                        return false;
                    }

                    #region [MDY:20181206] 改為 不可3個以上(連續)相同或連號的英數字元 (20181201_03)
                    if (!DataFormat.CheckUserPXXFormat(newPXX))
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized(String.Format("「使用者密碼」限輸入 {0} ~ {1} 碼英數字混合字串，且不可含連續3個(或以上)相同或連號的英文或數字", DataFormat.UserPXXMinSize, DataFormat.UserPXXMaxSize));
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    #endregion

                    #region [MDY:20181206] 增加不可與帳號相同 (20181201_03)
                    if (newPXX.Equals(user.UId))
                    {
                        this.ShowSystemMessage("密碼不可與帳號相同");
                        return false;
                    }
                    #endregion

                    #region [MDY:20160921] 使用者密碼加密
                    string encodePWrod = DataFormat.GetUserPXXEncode(newPXX);
                    if (String.IsNullOrEmpty(encodePWrod))
                    {
                        string msg = this.GetLocalized("「使用者密碼」加解密處理失敗");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    if (encodePWrod == user.UPXX1 || encodePWrod == user.UPXX)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("「使用者密碼」不可以與前兩次相同");
                        this.ShowSystemMessage(msg);
                        return false;
                    }
                    #endregion
                }
            }
            #endregion
            #endregion

            #region 電話
            string tel = this.tbxTel.Text.Trim();
            #endregion

            #region 職稱
            string title = String.Empty;
            #endregion

            #region EMail
            string email = this.tbxEmail.Text.Trim();
            #endregion

            #region isLocked
            bool isLocked = false;
            if (role == RoleCodeTexts.SCHOOL)
            {
                isLocked = this.chkLock.Checked;
            }
            #endregion

            #region [Old] 土銀不使用使用者權限
            //#region 使用者權限
            //KeyValueList<string> userRights = null;
            //{
            //    userRights = new KeyValueList<string>(gvUserRight.Rows.Count);
            //    foreach (GridViewRow row in gvUserRight.Rows)
            //    {
            //        string funcId = row.Cells[0].Text.Trim();
            //        string rightCode = null;
            //        CheckBox chkEdit = row.FindControl("chkEdit") as CheckBox;
            //        CheckBox chkQuery = row.FindControl("chkQuery") as CheckBox;
            //        if (chkEdit != null && chkEdit.Enabled && chkEdit.Checked)
            //        {
            //            rightCode = "1";    //編輯
            //        }
            //        else if (chkQuery != null && chkQuery.Enabled && chkQuery.Checked)
            //        {
            //            rightCode = "2";    //查詢
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //        userRights.Add(new KeyValue<string>(funcId, rightCode));
            //    }
            //}
            //#endregion
            #endregion

            KeyValueList<string> userRights = null;

            #region [MDY:20220530] Checkmarx 調整
            XmlResult xmlResult = DataProxy.Current.S530001UpdateData(this, user.UId, user.UGrp, user.UBank, user.URt
                , newReceiveType, newPXX, userName, title, tel, email, isLocked, userRights);
            #endregion

            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Modify);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            return xmlResult.IsSuccess;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <returns></returns>
        private bool DeleteData()
        {
            #region 使用者帳號
            UsersEntity user = this.EditUser;
            if (user == null)
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得被刪除的使用者資料");
                this.ShowSystemMessage(msg);
                return false;
            }
            #endregion

            XmlResult xmlResult = DataProxy.Current.S530001DeleteData(this, user.UId, user.UGrp, user.UBank, user.URt);
            if (!xmlResult.IsSuccess)
            {
                string action = ActionMode.GetActionLocalized(ActionMode.Delete);
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
            return xmlResult.IsSuccess;
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
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無維護權限");
                    this.ShowJsAlert(msg);
                    this.ccbtnOK.Visible = false;
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
                    this.ccbtnOK.Visible = false;
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                string bankId = QueryString.TryGetValue("BankId", String.Empty);
                string groupId = QueryString.TryGetValue("GroupId", String.Empty);
                string userId = QueryString.TryGetValue("UserId", String.Empty);
                string receiveType = QueryString.TryGetValue("ReceiveType", String.Empty);

                if (!ActionMode.IsMaintinaMode(this.Action)
                    || (!ActionMode.IsPKeyEditableMode(this.Action) && (String.IsNullOrEmpty(bankId) || String.IsNullOrEmpty(groupId) || String.IsNullOrEmpty(userId)))
                    )
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                UsersEntity user = null;
                GroupListEntity group = null;
                CodeText[] funcMenus = null;
                GroupRightEntity[] groupRights = null;
                UsersRightEntity[] userRights = null;

                switch (this.Action)
                {
                    case ActionMode.Insert:
                        #region 新增
                        {
                            user = new UsersEntity();
                            if (logonUser.IsSchoolUser)
                            {
                                user.UBank = logonUser.UnitId;
                            }
                            user.UGrp = null;
                            user.UId = null;
                            user.URt = null;

                            group = new GroupListEntity();
                            if (logonUser.IsBankManager)
                            {
                                //總行 - 預設行員、主管
                                group.Role = RoleCodeTexts.STAFF;
                                group.RoleType = RoleTypeCodeTexts.MANAGER;
                            }
                            else if (logonUser.IsBankUser)
                            {
                                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                                {
                                    //分行主控、會計主管 - 預設行員、主管
                                    group.Role = RoleCodeTexts.STAFF;
                                    group.RoleType = RoleTypeCodeTexts.MANAGER;
                                }
                                else
                                {
                                    //分行主管、經辦 - 預設學校、主管
                                    group.Role = RoleCodeTexts.SCHOOL;
                                    group.RoleType = RoleTypeCodeTexts.MANAGER;
                                }
                            }
                            else if (logonUser.IsSchoolUser)
                            {
                                //學校主管、經辦 - 預設學校、經辦
                                group.Role = RoleCodeTexts.SCHOOL;
                                group.RoleType = RoleTypeCodeTexts.USER;
                            }

                            #region [Old]
                            //group.Role = logonUser.IsSchoolUser ? RoleCodeTexts.SCHOOL : this.ddlRole.SelectedValue;
                            //if (group.Role == RoleCodeTexts.SCHOOL)
                            //{
                            //    if (logonUser.IsBankUser)
                            //    {
                            //        group.RoleType = RoleTypeCodeTexts.MANAGER;
                            //    }
                            //    else if (logonUser.IsSchoolUser)
                            //    {
                            //        group.RoleType = RoleTypeCodeTexts.USER;
                            //    }
                            //}
                            //else
                            //{
                            //    group.RoleType = RoleTypeCodeTexts.USER;
                            //}
                            #endregion

                            this.EditUser = user;
                            this.EditGroup = null;

                            #region [Old] 土銀不使用使用者權限
                            //this.EditFuncMenus = null;
                            //this.EditGroupRights = null;
                            #endregion
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:
                    case ActionMode.Delete:
                        #region 修改 / 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            //取得使用者、群組、功能選單、群組權限、使用者權限資料
                            XmlResult xmlResult = DataProxy.Current.S530001GetEditData(this, userId, groupId, receiveType, bankId,
                                out user, out group, out funcMenus, out groupRights, out userRights);
                            if (!xmlResult.IsSuccess)
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            if (user == null)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "使用者資料不存在");
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            if (group == null)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "使用者所屬群組資料不存在");
                                this.ccbtnOK.Visible = false;
                                return;
                            }

                            this.EditUser = user;
                            this.EditGroup = group;

                            #region [Old] 土銀不使用使用者權限
                            //this.EditFuncMenus = funcMenus;
                            //this.EditGroupRights = groupRights;
                            #endregion

                            #region [Old] 20150605 取消前擋後，改回後踢前
                            //if (user != null && this.Action == ActionMode.Modify)
                            //{
                            //    this.cclbtnLogout.Visible = true;
                            //}
                            //else
                            //{
                            //    this.cclbtnLogout.Visible = false;
                            //}
                            #endregion

                            #region 檢查資料權限
                            bool isHasAuth = false;
                            if (logonUser.IsBankManager)
                            {
                                //總行 (AD0、AD5、AD6) - 帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
                                isHasAuth = (group.Role == RoleCodeTexts.STAFF
                                    || (group.Role == RoleCodeTexts.SCHOOL && group.RoleType == RoleTypeCodeTexts.MANAGER));
                            }
                            else if (logonUser.IsBankUser)
                            {
                                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                                {
                                    //分行主控、會計主管 - 自己分行的帳號 + 自己分行的學校的主管帳號
                                    isHasAuth = ((group.Role == RoleCodeTexts.STAFF && user.UBank == logonUser.BankId)
                                        || (group.Role == RoleCodeTexts.SCHOOL && group.RoleType == RoleTypeCodeTexts.MANAGER && logonUser.IsMySchIdenty(user.UBank)));
                                }
                                else
                                {
                                    //分行主管、經辦 - 帳號維護：自己分行的學校的主管帳號
                                    isHasAuth = (group.Role == RoleCodeTexts.SCHOOL && group.RoleType == RoleTypeCodeTexts.MANAGER && logonUser.IsMySchIdenty(user.UBank));
                                }
                            }
                            else if (logonUser.IsSchoolUser)
                            {
                                //學校主管、經辦 - 帳號維護：自己學校的經辦帳號
                                isHasAuth = (group.Role == RoleCodeTexts.SCHOOL && group.RoleType == RoleTypeCodeTexts.USER && user.UBank == logonUser.UnitId);
                            }
                            else
                            {
                                isHasAuth = false;
                            }

                            if (!isHasAuth)
                            {
                                //[TODO] 固定顯示訊息的收集
                                this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護此資料的權限");
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            #endregion

                            #region [Old]
                            //if (logonUser.IsBankUser)
                            //{
                            //    if (group.Role == RoleCodeTexts.SCHOOL && group.RoleType != RoleTypeCodeTexts.MANAGER)
                            //    {
                            //        string msg = this.GetLocalized("無權限維護此使用者資料");
                            //        this.ShowJsAlert(msg);
                            //        this.ccbtnOK.Visible = false;
                            //        return;
                            //    }
                            //    if (!logonUser.IsBankManager && group.Role == RoleCodeTexts.STAFF && group.RoleType == RoleTypeCodeTexts.MANAGER)
                            //    {
                            //        string msg = this.GetLocalized("無權限維護此使用者資料");
                            //        this.ShowJsAlert(msg);
                            //        this.ccbtnOK.Visible = false;
                            //        return;
                            //    }
                            //}
                            //if (logonUser.IsSchoolUser && 
                            //    (user.UBank != logonUser.UnitId || group.Role != RoleCodeTexts.SCHOOL || group.RoleType != RoleTypeCodeTexts.USER)
                            //)
                            //{
                            //    //[TODO] 固定顯示訊息的收集
                            //    string msg = this.GetLocalized("無權限維護此使用者資料");
                            //    this.ShowJsAlert(msg);
                            //    this.ccbtnOK.Visible = false;
                            //    return;
                            //}
                            #endregion
                        }
                        #endregion
                        break;
                }
                #endregion

                #region [Old]
                //if (logonUser.IsBankUser)
                //{
                //    string gRole = null;
                //    string gGroupId = null;
                //    if (group != null && group.Role != null)
                //    {
                //        gRole = group.Role == null ? null : group.Role.Trim();
                //        gGroupId = group.GroupId;
                //    }
                //    WebHelper.SetDropDownListSelectedValue(this.ddlRole, gRole);
                //    this.ChangeRole();

                //    string schIdenty = null;
                //    if (user != null && gRole == RoleCodeTexts.SCHOOL)
                //    {
                //        schIdenty = user.UBank;
                //        //this.GetAndBindSchIdentyOptions(logonUser, schIdenty);
                //        WebHelper.SetDropDownListSelectedValue(this.ddlSchIdenty, schIdenty);
                //        this.ddlSchIdenty_SelectedIndexChanged(this.ddlSchIdenty, EventArgs.Empty);
                //        //this.GetAndBindReceiveTypeOptions(schIdenty, null);
                //    }
                //    else
                //    {
                //        this.GetAndBindGroupOptions(gRole, schIdenty, gGroupId);
                //    }
                //}
                //else
                //{
                //    string role = RoleCodeTexts.SCHOOL; //學校只能是這個 Role
                //    WebHelper.SetDropDownListSelectedValue(this.ddlRole, role);
                //    this.ChangeRole();

                //    //this.trSchIdenty.Visible = false;

                //    string schIdenty = logonUser.UnitId;

                //    //string[] receiveTypes = null;
                //    //if (!String.IsNullOrWhiteSpace(user.URt))
                //    //{
                //    //    receiveTypes = user.URt.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //    //    if (receiveTypes.Length > 0)
                //    //    {
                //    //        for (int idx = 0; idx < receiveTypes.Length; idx++)
                //    //        {
                //    //            receiveTypes[idx] = receiveTypes[idx].Trim();
                //    //        }
                //    //    }
                //    //}

                //    this.GetAndBindGroupOptions(RoleTypeCodeTexts.USER, schIdenty, group == null ? null : group.GroupId);

                //    this.GetAndBindReceiveTypeOptions(schIdenty, null);
                //}

                //if (logonUser.IsBankUser)
                //{
                //    if (group != null && user != null && group.Role == RoleCodeTexts.SCHOOL && group.RoleType == RoleTypeCodeTexts.USER)
                //    {
                //        this.GetAndBindReceiveTypeOption(user.UBank, null);
                //        this.trReceiveType.Visible = true;
                //    }
                //    else
                //    {
                //        this.trReceiveType.Visible = false;
                //    }
                //}
                //else
                //{
                //    this.GetAndBindReceiveTypeOption(bankId, null);
                //}
                #endregion

                this.BindEditData(this.Action, user, group, funcMenus, groupRights, userRights);
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = this.ddlRole.SelectedValue;
            string schIdenty = this.ddlSchIdenty.SelectedValue;
            this.ChangeUIByRole();
            this.GetAndBindGroupOptions(role, schIdenty, null);
        }

        protected void ddlSchIdenty_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = this.ddlRole.SelectedValue;
            string schIdenty = this.ddlSchIdenty.SelectedValue;
            //因為只由學校可以建立學校經辦的使用者，所以 trReceiveType 是否顯示與 user.URt 脫鉤
            //this.trReceiveType.Visible = !String.IsNullOrEmpty(schIdenty);
            this.GetAndBindReceiveTypeOptions(schIdenty, null);
            this.GetAndBindGroupOptions(role, schIdenty, null);
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string groupId = this.ddlGroup.SelectedValue;

            //if (String.IsNullOrEmpty(groupId))
            //{
            //    this.trReceiveType.Visible = false;
            //    return;
            //}

            //GroupListEntity group = null;
            //Expression where = new Expression(GroupListEntity.Field.GroupId, groupId);
            //XmlResult xmlResult = DataProxy.Current.SelectFirst<GroupListEntity>(this.Page, where, null, out group);
            //if (xmlResult.IsSuccess)
            //{
            //    //this.EditGroup = group;
            //    //if (group.Role == RoleCodeTexts.SCHOOL && group.RoleType != RoleTypeCodeTexts.MANAGER)
            //    if (group.Role == RoleCodeTexts.SCHOOL)
            //    {
            //        //if (group.RoleType == RoleTypeCodeTexts.USER)
            //        //{
            //        //    this.trReceiveType.Visible = true;
            //        //}
            //        //else
            //        //{
            //        //    this.trReceiveType.Visible = false;
            //        //}
            //        //WebHelper.SetCheckBoxListSelectedValues(this.cblReceiveType, null);
            //        this.trReceiveType.Visible = (group.RoleType == RoleTypeCodeTexts.USER);
            //    }
            //    else
            //    {
            //        this.trReceiveType.Visible = false;
            //    }
            //}
            //else
            //{
            //    this.trReceiveType.Visible = false;
            //    string action = this.GetLocalized("讀取群組資料");
            //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            //    this.ccbtnOK.Enabled = false;
            //}
        }

        protected void gvUserRight_PreRender(object sender, EventArgs e)
        {
            #region [Old] 土銀不使用使用者權限
            //FuncRight[] funcRights = this.gvUserRight.DataSource as FuncRight[];
            //if (funcRights == null || funcRights.Length == 0)
            //{
            //    return;
            //}

            //#region 檢查維護權限
            //bool visible = this.Action != ActionMode.Delete;
            //#endregion

            //foreach (GridViewRow row in this.gvUserRight.Rows)
            //{
            //    FuncRight funcRight = funcRights[row.RowIndex];

            //    CheckBox chkEdit = row.FindControl("chkEdit") as CheckBox;
            //    CheckBox chkQuery = row.FindControl("chkQuery") as CheckBox;
            //    if (chkEdit != null)
            //    {
            //        chkEdit.Enabled = funcRight.HasEditRight;
            //        chkEdit.Checked = chkEdit.Enabled && funcRight.CheckEditRight;
            //        chkEdit.Visible = visible;
            //    }
            //    if (chkQuery != null)
            //    {
            //        chkQuery.Enabled = funcRight.HasQueryRight;
            //        chkQuery.Checked = chkQuery.Enabled && funcRight.CheckQueryRight;
            //        chkQuery.Visible = visible;
            //    }
            //}
            #endregion
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string backUrl = "S5300001.aspx";

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

                #region 檢查與處理輸入資料
                string role = null;
                string userId = null;
                string dataKey = null;
                string formDesc = null;
                string dataUnitId = null;
                string dataRoleType = null;
                KeyValueList<string> args = new KeyValueList<string>();
                switch (this.Action)
                {
                    case ActionMode.Insert:
                        #region 新增
                        {
                            string action = this.GetLocalized("新增使用者資料申請");

                            #region 群組角色
                            if (logonUser.IsBankManager)
                            {
                                //總行 (AD0、AD5、AD6) - 帳號維護：所有行員的帳號 (除 AD0, AD1, AD2 群組的帳號) + 所有學校的主管帳號
                                role = this.ddlRole.SelectedValue;
                            }
                            else if (logonUser.IsBankUser)
                            {
                                if (logonUser.GroupId == BankADGroupCodeTexts.AD1 || logonUser.GroupId == BankADGroupCodeTexts.AD2)
                                {
                                    //分行主控、會計主管 - 帳號維護：自己分行的帳號 + 自己分行的學校的主管帳號
                                    role = this.ddlRole.SelectedValue;
                                }
                                else
                                {
                                    //分行主管、經辦 - 帳號維護：自己分行的學校的主管帳號
                                    role = RoleCodeTexts.SCHOOL;
                                }
                            }
                            else if (logonUser.IsSchoolUser)
                            {
                                //學校主管、經辦 - 帳號維護：自己學校的經辦帳號
                                role = RoleCodeTexts.SCHOOL;
                            }
                            if (String.IsNullOrEmpty(role))
                            {
                                this.ShowMustInputAlert("群組角色");
                                return;
                            }
                            #endregion

                            #region 所屬單位
                            string bankId = null;
                            if (role == RoleCodeTexts.STAFF)
                            {
                                if (logonUser.IsBankManager)
                                {
                                    bankId = this.ddlBank.SelectedValue;
                                    if (String.IsNullOrEmpty(bankId))
                                    {
                                        this.ShowMustInputAlert("分行代號");
                                        return;
                                    }
                                }
                                else
                                {
                                    bankId = logonUser.BankId;
                                }
                            }
                            else if (role == RoleCodeTexts.SCHOOL)
                            {
                                if (logonUser.IsBankUser)
                                {
                                    bankId = this.ddlSchIdenty.SelectedValue;
                                    if (String.IsNullOrEmpty(bankId))
                                    {
                                        this.ShowMustInputAlert("學校代碼");
                                        return;
                                    }
                                }
                                else
                                {
                                    bankId = logonUser.BankId;
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage("群組角色不正確");
                                return;
                            }
                            args.Add("BankId", bankId);
                            dataUnitId = bankId;
                            #endregion

                            #region [Old] 所屬單位
                            //string role = this.ddlRole.SelectedValue;
                            //string bankId = null;
                            //if (logonUser.IsBankUser)
                            //{
                            //    if (role == RoleCodeTexts.SCHOOL)
                            //    {
                            //        bankId = this.ddlSchIdenty.SelectedValue;
                            //        if (String.IsNullOrEmpty(bankId))
                            //        {
                            //            this.ShowMustInputAlert("學校代碼");
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        //TODO 應該要有分行選項
                            //        if (logonUser.IsBankManager)
                            //        {
                            //            bankId = this.ddlBank.SelectedValue;
                            //            if (String.IsNullOrEmpty(bankId))
                            //            {
                            //                this.ShowMustInputAlert("分行代號");
                            //                return;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            bankId = logonUser.BankId;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    bankId = logonUser.BankId;
                            //}
                            //args.Add("BankId", bankId);
                            #endregion

                            #region 所屬群組
                            string groupId = this.ddlGroup.SelectedValue;
                            if (String.IsNullOrEmpty(groupId))
                            {
                                this.ShowMustInputAlert("群組");
                                return;
                            }
                            args.Add("GroupId", groupId);

                            #region 取得 RolrType
                            {
                                Expression where = new Expression(GroupListEntity.Field.GroupId, groupId);
                                GroupListEntity group = null;
                                XmlResult xmlResult = DataProxy.Current.SelectFirst<GroupListEntity>(this.Page, where, null, out group);
                                if (xmlResult.IsSuccess)
                                {
                                    if (group == null)
                                    {
                                        this.ShowSystemMessage(ErrorCode.D_DATA_NOT_FOUND, "查無選取的群組資料");
                                        return;
                                    }
                                    else
                                    {
                                        dataRoleType = group.RoleType;
                                    }
                                }
                                else
                                {
                                    this.ShowActionFailureMessage(this.GetLocalized("讀取群組資料"), xmlResult.Code, xmlResult.Message);
                                    return;
                                }
                            }
                            #endregion
                            #endregion

                            #region 商家代號
                            string receiveType = null;
                            if (role == RoleCodeTexts.STAFF)
                            {
                                receiveType = String.Empty;
                            }
                            else if (role == RoleCodeTexts.SCHOOL)
                            {
                                if (this.trReceiveType.Visible)
                                {
                                    //trReceiveType 顯示表示使用者是學校經辦
                                    List<string> list = new List<string>(this.cblReceiveType.Items.Count);
                                    foreach (ListItem item in this.cblReceiveType.Items)
                                    {
                                        if (item.Selected)
                                        {
                                            list.Add(item.Value);
                                        }
                                    }
                                    if (list.Count == 0)
                                    {
                                        this.ShowMustInputAlert("商家代號");
                                        return;
                                    }
                                    else
                                    {
                                        receiveType = String.Concat(",", String.Join(",", list.ToArray()), ",");
                                    }
                                }
                                else
                                {
                                    //trReceiveType 不顯示表示使用者是學校主管
                                    receiveType = "*";  //學校主管
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage("群組角色不正確");
                                return;
                            }
                            args.Add("ReceiveType", receiveType);
                            #endregion

                            #region [Old] 商家代號
                            //string receiveType = null;
                            //if (this.trReceiveType.Visible)
                            //{
                            //    //trReceiveType 顯示表示使用者是學校經辦
                            //    receiveType = ",";
                            //    foreach (ListItem item in this.cblReceiveType.Items)
                            //    {
                            //        if (item.Selected)
                            //        {
                            //            receiveType += item.Value + ",";
                            //        }
                            //    }
                            //    if (receiveType.Length <= 1)
                            //    {
                            //        this.ShowMustInputAlert("商家代號");
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    //trReceiveType 不顯示表示使用者是學校主管或行員
                            //    if (role == RoleCodeTexts.SCHOOL)
                            //    {
                            //        receiveType = "*";  //學校主管
                            //    }
                            //    else
                            //    {
                            //        receiveType = String.Empty; //行員
                            //    }
                            //}
                            //args.Add("ReceiveType", receiveType);
                            #endregion

                            #region 使用者帳號
                            userId = this.tbxUserId.Text.Trim();
                            if (String.IsNullOrEmpty(userId))
                            {
                                this.ShowMustInputAlert("使用者帳號");
                                return;
                            }
                            if (role == RoleCodeTexts.STAFF)
                            {
                                if (!DataFormat.CheckBankUserIDFormat(userId))
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = this.GetLocalized("「使用者帳號」限輸入" + DataFormat.GetBankUserIdComment());
                                    this.ShowSystemMessage(msg);
                                    return;
                                }
                            }
                            else
                            {
                                if (!DataFormat.CheckUserIDFormat(userId))
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = this.GetLocalized("「使用者帳號」限輸入" + DataFormat.GetUserIdComment());
                                    this.ShowSystemMessage(msg);
                                    return;
                                }
                            }
                            args.Add("UserId", userId);
                            #endregion

                            #region 使用者名稱
                            string userName = this.tbxUserName.Text.Trim();
                            if (String.IsNullOrEmpty(userName))
                            {
                                this.ShowMustInputAlert("使用者名稱");
                                return;
                            }
                            args.Add("UserName", userName);
                            #endregion

                            #region 密碼
                            #region [MDY:20220530] Checkmarx 調整
                            if (role == RoleCodeTexts.SCHOOL)
                            {
                                string newPXX = this.tbxNewPXX.Text.Trim();
                                if (String.IsNullOrEmpty(newPXX))
                                {
                                    this.ShowMustInputAlert("使用者密碼");
                                    return;
                                }
                                if (newPXX != this.tbxConfirmPXX.Text.Trim())
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = this.GetLocalized("「確認密碼」與「使用者密碼」不相同");
                                    this.ShowSystemMessage(msg);
                                    return;
                                }

                                #region [MDY:20181206] 改為 不可3個以上(連續)相同或連號的英數字元 (20181201_03)
                                if (!DataFormat.CheckUserPXXFormat(newPXX))
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = this.GetLocalized(String.Format("「使用者密碼」限輸入 {0} ~ {1} 碼英數字混合字串，且不可含連續3個(或以上)相同或連號的英文或數字", DataFormat.UserPXXMinSize, DataFormat.UserPXXMaxSize));
                                    this.ShowSystemMessage(msg);
                                    return;
                                }
                                #endregion

                                #region [MDY:20181206] 增加不可與帳號相同 (20181201_03)
                                if (newPXX.Equals(userId))
                                {
                                    this.ShowSystemMessage("密碼不可與帳號相同");
                                    return;
                                }
                                #endregion

                                #region [MDY:20160921] 使用者密碼加密
                                string encodePXX = DataFormat.GetUserPXXEncode(newPXX);
                                if (String.IsNullOrEmpty(encodePXX))
                                {
                                    string msg = this.GetLocalized("「使用者密碼」加解密處理失敗");
                                    this.ShowSystemMessage(msg);
                                    return;
                                }
                                #endregion

                                args.Add("NewPXX", encodePXX);
                            }
                            else
                            {
                                args.Add("NewPXX", String.Empty);
                            }
                            #endregion
                            #endregion

                            #region 電話
                            string tel = this.tbxTel.Text.Trim();
                            args.Add("Tel", tel);
                            #endregion

                            #region E-Mail
                            string email = this.tbxEmail.Text.Trim();
                            args.Add("Email", email);
                            #endregion

                            string unitType = role == RoleCodeTexts.SCHOOL ? "學校" : "分行";
                            dataKey = helper.GetS5300001DataKey(userId, groupId, bankId);
                            formDesc = String.Format("{0}代碼：{1}; 群組：{2}; 帳號：{3}; 名稱：{4}; {5}", unitType, bankId, groupId, userId, userName, (String.IsNullOrEmpty(receiveType) ? "" : "商家代號：" + receiveType + ";"));

                            #region [MDY:20161013] 檢查行員的帳號為所有銀行唯一，學校的帳號為該校唯一
                            if (role == RoleCodeTexts.STAFF)
                            {
                                Expression where = new Expression(UsersEntity.Field.UBank, RelationEnum.Like, DataFormat.MyBankID + "___")
                                    .And(UsersEntity.Field.UId, userId);
                                int count = 0;
                                XmlResult xmlResult = DataProxy.Current.SelectCount<UsersEntity>(this.Page, where, out count);
                                if (xmlResult.IsSuccess)
                                {
                                    if (count > 0)
                                    {
                                        //[TODO] 固定顯示訊息的收集
                                        string msg = this.GetLocalized("此行員帳號已存在");
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }
                                }
                                else
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = String.Format("{0}，{1}", this.GetLocalized("查詢使用者帳號資料失敗"), xmlResult.Message);
                                    this.ShowSystemMessage(xmlResult.Code, msg);
                                    return;
                                }
                            }
                            else
                            {
                                Expression where = new Expression(UsersEntity.Field.UBank, bankId)
                                    .And(UsersEntity.Field.UId, userId);
                                int count = 0;
                                XmlResult xmlResult = DataProxy.Current.SelectCount<UsersEntity>(this.Page, where, out count);
                                if (xmlResult.IsSuccess)
                                {
                                    if (count > 0)
                                    {
                                        //[TODO] 固定顯示訊息的收集
                                        string msg = this.GetLocalized("該單位已有此使用者帳號");
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }
                                }
                                else
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = String.Format("{0}，{1}", this.GetLocalized("查詢該單位使用者帳號失敗"), xmlResult.Message);
                                    this.ShowSystemMessage(xmlResult.Code, msg);
                                    return;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:
                        #region 修改
                        {
                            #region 使用者帳號
                            UsersEntity user = this.EditUser;
                            if (user == null)
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("無法取得被修改的使用者資料");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            userId = user.UId;
                            args.Add("BankId", user.UBank);
                            args.Add("GroupId", user.UGrp);
                            args.Add("UserId", user.UId);
                            args.Add("ReceiveType", user.URt);
                            dataUnitId = user.UBank;
                            #endregion

                            #region 所屬群組
                            //GroupListEntity group = this.EditGroup;
                            role = this.EditGroup == null ? null : this.EditGroup.Role;
                            if (!RoleCodeTexts.IsDefine(role))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("無法取得被修改的使用者所屬群組資料");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            dataRoleType = this.EditGroup.RoleType;
                            #endregion

                            #region 商家代號
                            string newReceiveType = null;
                            if (role == RoleCodeTexts.STAFF)
                            {
                                newReceiveType = String.Empty;
                            }
                            else if (role == RoleCodeTexts.SCHOOL)
                            {
                                if (this.trReceiveType.Visible)
                                {
                                    //trReceiveType 顯示表示使用者是學校經辦
                                    List<string> list = new List<string>(this.cblReceiveType.Items.Count);
                                    foreach (ListItem item in this.cblReceiveType.Items)
                                    {
                                        if (item.Selected)
                                        {
                                            list.Add(item.Value);
                                        }
                                    }
                                    if (list.Count == 0)
                                    {
                                        this.ShowMustInputAlert("商家代號");
                                        return;
                                    }
                                    else
                                    {
                                        newReceiveType = String.Concat(",", String.Join(",", list.ToArray()), ",");
                                    }
                                }
                                else
                                {
                                    //trReceiveType 不顯示表示使用者是學校主管
                                    newReceiveType = "*";  //學校主管
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage("群組角色不正確");
                                return;
                            }
                            args.Add("NewReceiveType", newReceiveType);
                            #endregion

                            #region [Old] 商家代號
                            //string newReceiveType = null;
                            //if (this.trReceiveType.Visible)
                            //{
                            //    //trReceiveType 顯示表示使用者是學校經辦
                            //    newReceiveType = ",";
                            //    foreach (ListItem item in this.cblReceiveType.Items)
                            //    {
                            //        if (item.Selected)
                            //        {
                            //            newReceiveType += item.Value + ",";
                            //        }
                            //    }
                            //    if (newReceiveType.Length <= 1)
                            //    {
                            //        this.ShowMustInputAlert("商家代號");
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    //trReceiveType 不顯示表示使用者是學校主管或行員
                            //    if (role == RoleCodeTexts.SCHOOL)
                            //    {
                            //        newReceiveType = "*";  //學校主管
                            //    }
                            //    else
                            //    {
                            //        newReceiveType = String.Empty; //行員
                            //    }
                            //}
                            //args.Add("NewReceiveType", newReceiveType);
                            #endregion

                            #region 使用者名稱
                            string userName = this.tbxUserName.Text.Trim();
                            if (String.IsNullOrEmpty(userName))
                            {
                                this.ShowMustInputAlert("使用者名稱");
                                return;
                            }
                            args.Add("UserName", userName);
                            #endregion

                            #region 密碼
                            #region [MDY:20220530] Checkmarx 調整
                            if (role == RoleCodeTexts.SCHOOL)
                            {
                                string newPXX = this.tbxNewPXX.Text.Trim();
                                if (!String.IsNullOrEmpty(newPXX))
                                {
                                    if (newPXX != this.tbxConfirmPXX.Text.Trim())
                                    {
                                        //[TODO] 固定顯示訊息的收集
                                        string msg = this.GetLocalized("「確認密碼」與「使用者密碼」不相同");
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }

                                    #region [MDY:20181206] 改為 不可3個以上(連續)相同或連號的英數字元 (20181201_03)
                                    if (!DataFormat.CheckUserPXXFormat(newPXX))
                                    {
                                        //[TODO] 固定顯示訊息的收集
                                        string msg = this.GetLocalized(String.Format("「使用者密碼」限輸入 {0} ~ {1} 碼英數字混合字串，且不可含連續3個(或以上)相同或連號的英文或數字", DataFormat.UserPXXMinSize, DataFormat.UserPXXMaxSize));
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }
                                    #endregion

                                    #region [MDY:20181206] 增加不可與帳號相同 (20181201_03)
                                    if (newPXX.Equals(userId))
                                    {
                                        this.ShowSystemMessage("密碼不可與帳號相同");
                                        return;
                                    }
                                    #endregion

                                    #region [MDY:20160921] 使用者密碼加密
                                    string encodePWrod = DataFormat.GetUserPXXEncode(newPXX);
                                    if (String.IsNullOrEmpty(encodePWrod))
                                    {
                                        string msg = this.GetLocalized("「使用者密碼」加解密處理失敗");
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }
                                    if (encodePWrod == user.UPXX1 || encodePWrod == user.UPXX)
                                    {
                                        //[TODO] 固定顯示訊息的收集
                                        string msg = this.GetLocalized("「使用者密碼」不可以與前兩次相同");
                                        this.ShowSystemMessage(msg);
                                        return;
                                    }
                                    #endregion

                                    args.Add("NewPXX", encodePWrod);
                                }
                            }
                            #endregion
                            #endregion

                            #region 電話
                            string tel = this.tbxTel.Text.Trim();
                            args.Add("Tel", tel);
                            #endregion

                            #region EMail
                            string email = this.tbxEmail.Text.Trim();
                            args.Add("Email", email);
                            #endregion

                            #region isLocked
                            string lockStatus = null;
                            if (role == RoleCodeTexts.SCHOOL)
                            {
                                if (this.chkLock.Checked)
                                {
                                    lockStatus = "鎖定;";
                                    args.Add("IsLocked", "Y");
                                }
                                else
                                {
                                    lockStatus = "正常;";
                                    args.Add("IsLocked", "N");
                                }
                            }
                            else
                            {
                                args.Add("IsLocked", "N");
                            }
                            #endregion

                            string unitType = role == RoleCodeTexts.SCHOOL ? "學校" : "分行";
                            dataKey = helper.GetS5300001DataKey(user.UId, user.UGrp, user.UBank);
                            formDesc = String.Format("{0}代碼：{1}; 群組：{2}; 帳號：{3}; 名稱：{4}; {5}{6}", unitType, user.UBank, user.UGrp, user.UId, userName, (String.IsNullOrEmpty(user.URt) ? "" : "商家代號：" + user.URt + ";"), lockStatus);
                        }
                        #endregion
                        break;
                    case ActionMode.Delete:
                        #region 刪除
                        {
                            #region 使用者帳號
                            UsersEntity user = this.EditUser;
                            if (user == null)
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("無法取得被刪除的使用者資料");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            userId = user.UId;
                            args.Add("BankId", user.UBank);
                            args.Add("GroupId", user.UGrp);
                            args.Add("UserId", user.UId);
                            args.Add("ReceiveType", user.URt);
                            dataUnitId = user.UBank;
                            #endregion

                            #region 所屬群組
                            //GroupListEntity group = this.EditGroup;
                            role = this.EditGroup == null ? null : this.EditGroup.Role;
                            if (!RoleCodeTexts.IsDefine(role))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("無法取得被刪除的使用者所屬群組資料");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            dataRoleType = this.EditGroup.RoleType;
                            #endregion

                            string unitType = role == RoleCodeTexts.SCHOOL ? "學校" : "分行";
                            dataKey = helper.GetS5300001DataKey(user.UId, user.UGrp, user.UBank);
                            formDesc = String.Format("{0}代碼：{1}; 群組：{2}; 帳號：{3}; 名稱：{4}; ", unitType, user.UBank, user.UGrp, user.UId, user.UName);
                        }
                        #endregion
                        break;
                    default:
                        #region
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("不支援的操作模式");
                            this.ShowSystemMessage(msg);
                        }
                        #endregion
                        return;
                }
                #endregion

                #region 檢查審核中是否有該帳號
                {
                    Expression where = new Expression(FlowDataEntity.Field.FormId, FormCodeTexts.S5300001)
                        .And(FlowDataEntity.Field.Status, new string[] { FlowStatusCodeTexts.FLOWING, FlowStatusCodeTexts.PROCESSING });

                    #region [MDY:20161013] 行員的帳號為所有銀行唯一，學校的帳號為該校唯一
                    if (role == RoleCodeTexts.STAFF)
                    {
                        string likeKey = helper.GetS5300001DataKey(userId, "%", DataFormat.MyBankID + "___");
                        where.And(FlowDataEntity.Field.DataKey, RelationEnum.Like, likeKey);
                    }
                    else
                    {
                        where.And(FlowDataEntity.Field.DataKey, dataKey);
                    }
                    #endregion

                    int count = 0;
                    XmlResult xmlResult = DataProxy.Current.SelectCount<FlowDataEntity>(this.Page, where, out count);
                    if (xmlResult.IsSuccess)
                    {
                        if (count > 0)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("該使用者資料審核中");
                            this.ShowSystemMessage(msg);
                            return;
                        }
                    }
                    else
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = String.Format("{0}，{1}", this.GetLocalized("查詢審核中的使用者資料失敗"), xmlResult.Message);
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
                            string action = this.GetLocalized("新增使用者資料申請");

                            #region 流程資料
                            FlowDataEntity flowData = new FlowDataEntity();
                            flowData.FormId = FormCodeTexts.S5300001;
                            flowData.FormData = helper.GetS5300001FormData(args);
                            if (String.IsNullOrEmpty(flowData.FormData))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("序列化申請資料失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            flowData.FormDesc = formDesc;

                            flowData.ApplyDate = DateTime.Now;
                            flowData.ApplyKind = ApplyKindCodeTexts.INSERT;
                            flowData.ApplyUnitId = logonUser.UnitId;
                            flowData.ApplyUserId = logonUser.UserId;
                            flowData.ApplyUserName = logonUser.UserName;
                            flowData.ApplyUserQual = logonUser.UserQual;

                            flowData.DataKey = dataKey;
                            flowData.DataUnitId = dataUnitId;
                            flowData.DataRole = role;
                            flowData.DataRoleType = dataRoleType;
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
                            string action = this.GetLocalized("修改使用者資料申請");

                            #region 流程資料
                            FlowDataEntity flowData = new FlowDataEntity();
                            flowData.FormId = FormCodeTexts.S5300001;
                            flowData.FormData = helper.GetS5300001FormData(args);
                            if (String.IsNullOrEmpty(flowData.FormData))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("序列化申請資料失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            flowData.FormDesc = formDesc;

                            flowData.ApplyDate = DateTime.Now;
                            flowData.ApplyKind = ApplyKindCodeTexts.UPDATE;
                            flowData.ApplyUnitId = logonUser.UnitId;
                            flowData.ApplyUserId = logonUser.UserId;
                            flowData.ApplyUserName = logonUser.UserName;
                            flowData.ApplyUserQual = logonUser.UserQual;

                            flowData.DataKey = dataKey;
                            flowData.DataUnitId = dataUnitId;
                            flowData.DataRole = role;
                            flowData.DataRoleType = dataRoleType;
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
                            string action = this.GetLocalized("刪除使用者資料申請");

                            #region 流程資料
                            FlowDataEntity flowData = new FlowDataEntity();
                            flowData.FormId = FormCodeTexts.S5300001;
                            flowData.FormData = helper.GetS5300001FormData(args);
                            if (String.IsNullOrEmpty(flowData.FormData))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("序列化申請資料失敗");
                                this.ShowSystemMessage(msg);
                                return;
                            }
                            flowData.FormDesc = formDesc;

                            flowData.ApplyDate = DateTime.Now;
                            flowData.ApplyKind = ApplyKindCodeTexts.DELETE;
                            flowData.ApplyUnitId = logonUser.UnitId;
                            flowData.ApplyUserId = logonUser.UserId;
                            flowData.ApplyUserName = logonUser.UserName;
                            flowData.ApplyUserQual = logonUser.UserQual;

                            flowData.DataKey = dataKey;
                            flowData.DataUnitId = dataUnitId;
                            flowData.DataRole = role;
                            flowData.DataRoleType = dataRoleType;
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
                bool isOk = false;
                string msg = null;
                string action = ActionMode.GetActionLocalized(this.Action);
                switch (this.Action)
                {
                    case ActionMode.Insert:
                        isOk = this.InsertData();
                        break;
                    case ActionMode.Modify:
                        isOk = this.UpdateData();
                        break;
                    case ActionMode.Delete:
                        isOk = this.DeleteData();
                        break;
                    default:
                        msg = this.GetLocalized("不支援的操作模式");
                        break;
                }
                if (isOk)
                {
                    this.ShowActionSuccessAlert(action, backUrl);
                }
                else if (String.IsNullOrEmpty(msg))
                {
                    //[TODO] 固定顯示訊息的收集
                    this.ShowSystemMessage(msg);
                }
                #endregion
            }
        }

        protected void cclbtnLogout_Click(object sender, EventArgs e)
        {
            #region [Old] 20150605 取消前擋後，改回後踢前
            //UsersEntity user = this.EditUser;
            //if (user == null)
            //{
            //    //[TODO] 固定顯示訊息的收集
            //    string msg = this.GetLocalized("無法取得被修改的使用者資料");
            //    this.ShowSystemMessage(msg);
            //    return;
            //}

            //string action = this.GetLocalized("強迫登出");
            ////TODO: 不確定 userQual 怎麽分
            //string userQual = (user.UBank.Length == 4 ? UserQualCodeTexts.SCHOOL : UserQualCodeTexts.BANK);
            //XmlResult xmlResult = DataProxy.Current.ForcedLogoutUser(this.Page, user.UBank, user.UId, userQual);

            //if (xmlResult.IsSuccess)
            //{
            //    this.ShowActionSuccessAlert(action, null);
            //}
            //else
            //{
            //    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            //}
            #endregion
        }

    }
}