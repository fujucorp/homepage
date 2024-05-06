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
    /// 系統訊息公告(最新消息) (維護)
    /// </summary>
    public partial class S5600007M : BasePage
    {
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
        /// 編輯的公告流水號
        /// </summary>
        private int EditBoardId
        {
            get
            {
                object value = ViewState["EditBoardId"];
                if (value is int)
                {
                    return (int)value;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["EditBoardId"] = value < 0 ? 0 : value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxBoardSubject.Text = String.Empty;
            this.tbxStartDate.Text = String.Empty;
            this.tbxEndDate.Text = String.Empty;
            this.tbxBoardContent.Text = String.Empty;

            #region [MDY:2018xxxx] 公告內容型態選項 (土銀只使用 html)
            {
                //CodeText[] items = new CodeText[] { new CodeText("1", "純文字"), new CodeText("2", "Html") };
                CodeText[] items = new CodeText[] { new CodeText("2", "Html") };
                WebHelper.SetDropDownListItems(this.ddlBoardType, DefaultItem.Kind.None, false, items, false, true, 0, items[0].Code);
            }
            #endregion

            LogonUser logonUser = this.GetLogonUser();

            #region [MDY:2018xxxx] 公告位置選項增加權限判斷，並改用 BoardTypeCodeTexts
            {
                #region [Old]
                ////CodeText[] items = new CodeText[] { new CodeText("1", "純文字"), new CodeText("2", "Html") };
                //items = new CodeText[] { new CodeText("1", "首頁"), new CodeText("2", "學校專區"), new CodeText("3", "學生專區")
                //    , new CodeText("4", "銀行專區"), new CodeText("5", "信用卡繳費"), new CodeText("6", "銀聯卡繳費")
                //    , new CodeText("7", "查詢繳費狀態"), new CodeText("8", "查詢列印繳費單"), new CodeText("9", "列印收據")};
                //WebHelper.SetDropDownListItems(this.ddlSchId, DefaultItem.Kind.None, false, items, false, true, 0, items[0].Code);
                #endregion

                if (logonUser.IsBankManager)
                {
                    //總行使用者可以選擇所有選項
                    CodeTextList items = new BoardTypeCodeTexts();
                    WebHelper.SetDropDownListItems(this.ddlSchId, DefaultItem.Kind.None, false, items, false, true, 0, items[0].Code);
                }
                else
                {
                    //分行使用者只能選擇學校專區或學生專區
                    CodeText[] items = new CodeText[2];
                    items[0] = new CodeText(BoardTypeCodeTexts.SCHOOL, BoardTypeCodeTexts.SCHOOL_TEXT);

                    #region [MDY:20190218] 修正索引值錯誤 (20190212_02)
                    items[1] = new CodeText(BoardTypeCodeTexts.STUDENT, BoardTypeCodeTexts.STUDENT_TEXT);
                    #endregion

                    WebHelper.SetDropDownListItems(this.ddlSchId, DefaultItem.Kind.None, false, items, false, true, 0, items[0].Code);
                }
            }
            #endregion

            #region [MDY:2018xxxx] 增加公告對象
            {
                CodeText[] items = null;
                XmlResult xmlResult = DataProxy.Current.GetMySchoolRTypeCodeTextsByBank(this.Page, out items);
                if (this.GetLogonUser().IsBankManager)
                {
                    //總行使用者可以選擇全部
                    WebHelper.SetDropDownListItems(this.ddlTarget, DefaultItem.Kind.All, false, items, false, false, 0, "");
                }
                else
                {
                    //分行使用者只能選擇自己分行的學校
                    WebHelper.SetDropDownListItems(this.ddlTarget, DefaultItem.Kind.Select, false, items, false, false, 0, "");
                }
            }
            #endregion

            #region [MDY:2018xxxx] 新增社群分享
            {
                CodeText[] items = new CodeText[] { new CodeText("Y", "啟用分享按鈕"), new CodeText("N", "停用分享按鈕") };
                WebHelper.SetDropDownListItems(this.ddlShareFlag, DefaultItem.Kind.None, false, items, false, false, 0, "N");
            }
            #endregion

            this.ccbtnOK.Visible = false;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(BoardEntity data)
        {
            if (data == null)
            {
                this.tbxBoardSubject.Text = String.Empty;
                this.tbxStartDate.Text = String.Empty;
                this.tbxEndDate.Text = String.Empty;
                this.tbxBoardContent.Text = String.Empty;
                this.ddlBoardType.SelectedIndex = 0;
                this.ddlSchId.SelectedIndex = 0;

                #region [MDY:2018xxxx] 增加公告對象
                this.ddlTarget.SelectedIndex = 0;
                #endregion

                #region [MDY:2018xxxx] 新增社群分享
                WebHelper.SetDropDownListSelectedValue(this.ddlShareFlag, "N");
                #endregion

                this.ccbtnOK.Visible = false;
                return;
            }

            bool isEditable = ActionMode.IsDataEditableMode(this.Action);
            this.tbxBoardSubject.Enabled = isEditable;
            this.tbxStartDate.Enabled = isEditable;
            this.tbxEndDate.Enabled = isEditable;
            this.tbxBoardContent.Enabled = isEditable;
            this.ddlBoardType.Enabled = isEditable;
            this.ddlSchId.Enabled = isEditable;

            this.tbxBoardSubject.Text = data.BoardSubject;
            this.tbxStartDate.Text = data.StartDate == null ? String.Empty : DataFormat.GetDateText(data.StartDate.Value);
            this.tbxEndDate.Text = data.EndDate == null ? String.Empty : DataFormat.GetDateText(data.EndDate.Value);
            this.tbxBoardContent.Text = data.BoardContent;
            WebHelper.SetDropDownListSelectedValue(this.ddlBoardType, data.BoardType);
            WebHelper.SetDropDownListSelectedValue(this.ddlSchId, data.SchId);

            #region [MDY:2018xxxx] 增加公告對象
            {
                this.ddlTarget.Enabled = isEditable;
                WebHelper.SetDropDownListSelectedValue(this.ddlTarget, data.ReceiveType);
            }
            #endregion

            #region [MDY:2018xxxx] 新增社群分享
            {
                this.ddlShareFlag.Enabled = isEditable;
                string shareFlag = "Y".Equals(data.ShareFlag, StringComparison.CurrentCultureIgnoreCase) ? "Y" : "N";
                WebHelper.SetDropDownListSelectedValue(this.ddlShareFlag, shareFlag);
            }
            #endregion

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool GetAndCheckEditData(out BoardEntity data)
        {
            data = new BoardEntity();

            if (!ActionMode.IsPKeyEditableMode(this.Action))
            {
                data.BoardId = this.EditBoardId;
            }

            data.BoardSubject = this.tbxBoardSubject.Text.Trim();
            if (String.IsNullOrWhiteSpace(data.BoardSubject))
            {
                this.ShowMustInputAlert("公告主旨");
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.tbxStartDate.Text))
            {
                this.ShowMustInputAlert("公告日期");
                return false;
            }
            DateTime sDate;
            if (DateTime.TryParse(this.tbxStartDate.Text.Trim(), out sDate))
            {
                data.StartDate = sDate;
            }
            else
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("公告日期不是有效的日期格式");
                this.ShowJsAlert(msg);
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.tbxEndDate.Text))
            {
                this.ShowMustInputAlert("有效日期");
                return false;
            }
            DateTime eDate;
            if (DateTime.TryParse(this.tbxEndDate.Text.Trim(), out eDate))
            {
                data.EndDate = eDate;
            }
            else
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("有效日期不是有效的日期格式");
                this.ShowJsAlert(msg);
                return false;
            }

            data.BoardContent = Server.HtmlDecode(this.hidHtmlContent.Value.Trim());
            if (String.IsNullOrWhiteSpace(data.BoardContent))
            {
                this.ShowMustInputAlert("公告內容");
                return false;
            }

            data.BoardType = this.ddlBoardType.SelectedValue;

            LogonUser logonUser = this.GetLogonUser();

            #region [MDY:2018xxxx] 檢查公告位置權限
            {
                data.SchId = this.ddlSchId.SelectedValue;
                //分行使用者只能選擇學校專區或學生專區
                if (!logonUser.IsBankManager && data.SchId != BoardTypeCodeTexts.SCHOOL && data.SchId != BoardTypeCodeTexts.STUDENT)
                {
                    string msg = this.GetLocalized("指定的公告位置未授權");
                    this.ShowJsAlert(msg);
                    return false;
                }
            }
            #endregion

            #region [MDY:2018xxxx] 增加公告對象
            {
                if (data.SchId == BoardTypeCodeTexts.STUDENT || data.SchId == BoardTypeCodeTexts.SCHOOL)
                {
                    data.ReceiveType = this.ddlTarget.SelectedValue;
                    if (String.IsNullOrEmpty(data.ReceiveType) && !logonUser.IsBankManager) //總行使用者才可以選擇全部
                    {
                        this.ShowMustInputAlert("公告對象");
                        return false;
                    }
                    else if (!logonUser.IsMySchIdenty(data.ReceiveType))
                    {
                        string msg = this.GetLocalized("指定的公告對象未授權");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                }
                else
                {
                    data.ReceiveType = String.Empty;
                }
            }
            #endregion

            #region [MDY:2018xxxx] 新增社群分享
            {
                data.ShareFlag = "Y".Equals(this.ddlShareFlag.SelectedValue, StringComparison.CurrentCultureIgnoreCase) ? "Y" : "N" ;
            }
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.InitialUI();

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
                int boardId = 0;
                int.TryParse(QueryString.TryGetValue("BoardId", "0"), out boardId);
                this.EditBoardId = boardId;
                if (!ActionMode.IsMaintinaMode(this.Action)
                    || (!ActionMode.IsPKeyEditableMode(this.Action) && this.EditBoardId < 1))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    return;
                }
                #endregion

                #region 取得維護資料
                BoardEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new BoardEntity();
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(BoardEntity.Field.BoardId, this.EditBoardId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<BoardEntity>(this, where, null, out data);
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
                        }
                        #endregion
                        break;
                }
                #endregion

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            BoardEntity data = null;
            if (!this.GetAndCheckEditData(out data))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600007.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;

                        #region [MDY:2018xxxx] 公告對象
                        //data.ReceiveType = string.Empty;
                        #endregion

                        data.BoardDep = string.Empty;
                        data.BoardUser = string.Empty;
                        data.BoardTel = string.Empty;
                        data.BoardEmail = string.Empty;

                        #region [MDY:2018xxxx] 更正為 UserId
                        #region [Old]
                        //data.UId = this.GetLogonUser().UnitId;
                        #endregion

                        data.UId = this.GetLogonUser().UserId;
                        #endregion

                        data.AddDate = DateTime.Now;
                        data.UpdateUId = null;
                        data.UpdateDate = null;

                        XmlResult xmlResult = DataProxy.Current.Insert<BoardEntity>(this, data, out count);
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
                        Expression where = new Expression(BoardEntity.Field.BoardId, data.BoardId);
                        #endregion

                        #region 更新欄位
                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(BoardEntity.Field.BoardSubject, data.BoardSubject);
                        fieldValues.Add(BoardEntity.Field.StartDate, data.StartDate);
                        fieldValues.Add(BoardEntity.Field.EndDate, data.EndDate);
                        fieldValues.Add(BoardEntity.Field.SchId, data.SchId);
                        fieldValues.Add(BoardEntity.Field.BoardContent, data.BoardContent);

                        #region [MDY:2018xxxx] 增加公告對象 & 更新者 & 更新日期
                        fieldValues.Add(BoardEntity.Field.ReceiveType, data.ReceiveType);
                        fieldValues.Add(BoardEntity.Field.UpdateUId, this.GetLogonUser().UserId);
                        fieldValues.Add(BoardEntity.Field.UpdateDate, DateTime.Now);
                        #endregion

                        #region [MDY:2018xxxx] 新增社群分享
                        fieldValues.Add(BoardEntity.Field.ShareFlag, data.ShareFlag);
                        #endregion

                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.UpdateFields<BoardEntity>(this, where, fieldValues, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<BoardEntity>(this, data, out count);
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
        }

        #region [Old]
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        this.InitialUI();

        //        #region 檢查維護權限
        //        if (!this.HasMaintainAuth())
        //        {
        //            this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //            return;
        //        }
        //        #endregion

        //        #region 處理參數
        //        KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
        //        if (QueryString == null || QueryString.Count == 0)
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("缺少網頁參數");
        //            this.ShowSystemMessage(msg);
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }

        //        this.Action = QueryString.TryGetValue("Action", String.Empty);

        //        this.EditBoardId = QueryString.TryGetValue("BoardId", "0");

        //        if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete)
        //            && String.IsNullOrEmpty(this.EditBoardId)))
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("網頁參數不正確");
        //            this.ShowSystemMessage(msg);
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }
        //        #endregion

        //        #region 取得維護資料
        //        BoardEntity data = null;
        //        switch (this.Action)
        //        {
        //            case ActionMode.Insert:   //新增
        //                #region 新增
        //                {
        //                    //空的資料
        //                    data = new BoardEntity();
        //                    //data.BoardId = ;
        //                }
        //                #endregion
        //                break;
        //            case ActionMode.Modify:   //修改
        //            case ActionMode.Delete:   //刪除
        //                #region 修改 | 刪除
        //                {
        //                    string action = this.GetLocalized("查詢要維護的資料");

        //                    #region 查詢條件
        //                    Expression where = new Expression(BoardEntity.Field.BoardId, this.EditBoardId);
        //                    #endregion

        //                    #region 查詢資料
        //                    XmlResult xmlResult = DataProxy.Current.SelectFirst<BoardEntity>(this, where, null, out data);
        //                    if (!xmlResult.IsSuccess)
        //                    {
        //                        this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                        this.ccbtnOK.Visible = false;
        //                        return;
        //                    }
        //                    if (data == null)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                        this.ccbtnOK.Visible = false;
        //                        return;
        //                    }
        //                    #endregion
        //                }
        //                #endregion
        //                break;
        //        }
        //        #endregion

        //        this.BindEditData(data);
        //    }
        //}

        ///// <summary>
        ///// 初始化使用介面
        ///// </summary>
        //private void InitialUI()
        //{
        //    this.tbxBoardContent.Text = String.Empty;
        //    this.tbxEndDate.Text = String.Empty;
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 結繫維護資料
        ///// </summary>
        ///// <param name="data">維護資料</param>
        //private void BindEditData(BoardEntity data)
        //{
        //    if (data == null)
        //    {
        //        this.tbxBoardContent.Text = String.Empty;
        //        this.tbxEndDate.Text = String.Empty;
        //        this.ccbtnOK.Visible = false;
        //        return;
        //    }

        //    switch (this.Action)
        //    {
        //        case ActionMode.Delete:
        //            this.tbxBoardContent.Enabled = false;
        //            this.tbxEndDate.Enabled = false;
        //            break;
        //        default:
        //            this.tbxBoardContent.Enabled = true;
        //            this.tbxEndDate.Enabled = true;
        //            break;
        //    }
        //    this.tbxBoardContent.Text = data.BoardContent;
        //    if (data.EndDate != null)
        //    {
        //        DateTime dt = (DateTime)data.EndDate;
        //        this.tbxEndDate.Text = dt.ToString("yyyy/MM/dd");
        //    }
        //    else
        //    {
        //        this.tbxEndDate.Text = string.Empty;
        //    }
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 檢查輸入的維護資料
        ///// </summary>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //private bool CheckEditData()
        //{
        //    if (String.IsNullOrEmpty(tbxBoardContent.Text.Trim()))
        //    {
        //        this.ShowMustInputAlert("公告內容");
        //        return false;
        //    }

        //    if (String.IsNullOrEmpty(tbxEndDate.Text.Trim()))
        //    {
        //        this.ShowMustInputAlert("有效日期");
        //        return false;
        //    }

        //    if (!Common.IsDate(tbxEndDate.Text.Trim()))
        //    {
        //        this.ShowSystemMessage("有效日期格式錯誤yyyy/MM/dd");
        //        return false;
        //    }
        //    return true;
        //}

        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    if (!this.CheckEditData())
        //    {
        //        return;
        //    }

        //    BoardEntity data = this.GetEditData();

        //    string action = ActionMode.GetActionLocalized(this.Action);
        //    string backUrl = "S5600007.aspx";
        //    switch (this.Action)
        //    {
        //        case ActionMode.Insert:     //新增
        //            #region 新增
        //            {
        //                int count = 0;
                        
        //                //不能為 Null
        //                data.ReceiveType = string.Empty;
        //                data.StartDate = DateTime.Now;

        //                XmlResult xmlResult = DataProxy.Current.Insert<BoardEntity>(this, data, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
        //                    }
        //                    else
        //                    {
        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //        case ActionMode.Modify:     //修改
        //            #region 修改
        //            {
        //                #region 更新條件
        //                Expression where = new Expression(BoardEntity.Field.BoardId, data.BoardId);
        //                #endregion

        //                #region 更新欄位
        //                KeyValueList fieldValues = new KeyValueList();
        //                fieldValues.Add(BoardEntity.Field.BoardContent, data.BoardContent);
        //                fieldValues.Add(BoardEntity.Field.EndDate, data.EndDate);
        //                #endregion

        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.UpdateFields<BoardEntity>(this, where, fieldValues, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    }
        //                    else
        //                    {
        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //        case ActionMode.Delete:     //刪除
        //            #region 刪除
        //            {
        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.Delete<BoardEntity>(this, data, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    }
        //                    else
        //                    {
        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //    }
        //}

        ///// <summary>
        ///// 取得輸入的維護資料
        ///// </summary>
        ///// <returns>傳回輸入的維護資料</returns>
        //private BoardEntity GetEditData()
        //{
        //    BoardEntity data = new BoardEntity();
        //    data.BoardId = Convert.ToInt16(this.EditBoardId);
        //    data.BoardContent = tbxBoardContent.Text.Trim();
        //    data.EndDate = Convert.ToDateTime(this.tbxEndDate.Text.Trim());
        //    return data;
        //}
        #endregion
    }
}