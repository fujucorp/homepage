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
    /// Q&A (維護)
    /// </summary>
    public partial class S5600013M : BasePage
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
        /// 編輯的序號
        /// </summary>
        private string EditSn
        {
            get
            {
                return ViewState["EditSn"] as string;
            }
            set
            {
                ViewState["EditSn"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的Entity
        /// </summary>
        private QnaEntity EditQnaEntity
        {
            get
            {
                return ViewState["EditQnaEntity"] as QnaEntity;
            }
            set
            {
                ViewState["EditQnaEntity"] = value == null ? null : value;
            }
        }
        #endregion

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
                    this.ccbtnOK.Visible = false;
                    return;
                }

                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditSn = QueryString.TryGetValue("Sn", String.Empty);

                if (((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete) && String.IsNullOrEmpty(this.EditSn)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                QnaEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new QnaEntity();
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(QnaEntity.Field.Sn, this.EditSn);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<QnaEntity>(this, where, null, out data);
                            if (!xmlResult.IsSuccess)
                            {
                                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            if (data == null)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                this.ccbtnOK.Visible = false;
                                return;
                            }
                            this.EditQnaEntity = data;
                            #endregion
                        }
                        #endregion
                        break;
                }
                #endregion

                this.BindEditData(data);
            }
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxQ.Text = String.Empty;
            this.tbxA.Text = String.Empty;
            this.tbxSort.Text = String.Empty;
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        private void BindEditData(QnaEntity data)
        {
            if (data == null)
            {
                this.tbxQ.Text = String.Empty;
                this.tbxA.Text = String.Empty;
                this.tbxSort.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.Action)
            {
                case ActionMode.Insert:
                case ActionMode.Modify:
                    this.tbxQ.Enabled = true;
                    this.tbxA.Enabled = true;
                    this.tbxSort.Enabled = true;
                    this.ddlType.Enabled = true;
                    break;
                default:
                    this.tbxQ.Enabled = false;
                    this.tbxA.Enabled = false;
                    this.tbxSort.Enabled = false;
                    this.ddlType.Enabled = false;
                    break;
            }
            this.tbxQ.Text = HttpUtility.HtmlDecode(data.Q);
            this.tbxA.Text = HttpUtility.HtmlDecode(data.A);
            this.tbxSort.Text = data.Sort.ToString();
            WebHelper.SetDropDownListSelectedValue(this.ddlType, data.Type);
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 檢查輸入的維護資料
        /// </summary>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool CheckEditData()
        {
            //if (String.IsNullOrEmpty(tbxQ.Text.Trim()))
            //{
            //    this.ShowMustInputAlert("問題");
            //    return false;
            //}

            if (String.IsNullOrEmpty(tbxSort.Text.Trim()))
            {
                this.ShowMustInputAlert("排序");
                return false;
            }
            return true;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            if (!this.CheckEditData())
            {
                return;
            }

            QnaEntity data = this.GetEditData();

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600013.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<QnaEntity>(this, data, out count);
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
                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<QnaEntity>(this, data, out count);
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
                        XmlResult xmlResult = DataProxy.Current.Delete<QnaEntity>(this, data, out count);
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

        /// <summary>
        /// 取得輸入的維護資料
        /// </summary>
        /// <returns>傳回輸入的維護資料</returns>
        private QnaEntity GetEditData()
        {
            QnaEntity data = new QnaEntity();

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.Status = DataStatusCodeTexts.NORMAL;
                    data.CrtDate = DateTime.Now;
                    data.CrtUser = this.GetLogonUser().UserId;
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data = this.EditQnaEntity;
                    data.MdyDate = DateTime.Now;
                    data.MdyUser = this.GetLogonUser().UserId;
                    break;
            }

            data.Q = Server.HtmlDecode(this.hidHtmlContentQ.Value.Trim());
            data.A = Server.HtmlDecode(this.hidHtmlContentA.Value.Trim());

            //data.Q = HttpUtility.HtmlEncode(tbxQ.Text);
            //data.A = HttpUtility.HtmlEncode(tbxA.Text);
            int sort = 0;
            int.TryParse(tbxSort.Text, out sort);
            data.Sort = sort;
            data.Type = ddlType.SelectedValue;

            return data;
        }
    }
}