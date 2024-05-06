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

namespace eSchoolWeb.D
{
    public partial class D1300005M : BasePage
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
        /// 編輯的業務別碼代碼參數
        /// </summary>
        private string EditReceiveType
        {
            get
            {
                return ViewState["EditReceiveType"] as string;
            }
            set
            {
                ViewState["EditReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學年代碼參數
        /// </summary>
        private string EditYearId
        {
            get
            {
                return ViewState["EditYearId"] as string;
            }
            set
            {
                ViewState["EditYearId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學期參數
        /// </summary>
        private string EditTermId
        {
            get
            {
                return ViewState["EditTermId"] as string;
            }
            set
            {
                ViewState["EditTermId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的部別參數
        /// </summary>
        private string EditDepId
        {
            get
            {
                return ViewState["EditDepId"] as string;
            }
            set
            {
                ViewState["EditDepId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的收費別代碼
        /// </summary>
        private string EditReceiveId
        {
            get
            {
                return ViewState["EditReceiveId"] as string;
            }
            set
            {
                ViewState["EditReceiveId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的院所代碼參數
        /// </summary>
        private string EditLoanId
        {
            get
            {
                return ViewState["EditLoanId"] as string;
            }
            set
            {
                ViewState["EditLoanId"] = value == null ? null : value.Trim();
            }
        }

        private LoanStandardEntity EditLoanStandardEntity
        {
            get
            {
                return ViewState["EditLoanStandardEntity"] as LoanStandardEntity;
            }
            set
            {
                ViewState["EditLoanStandardEntity"] = value == null ? null : value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 處理參數
                KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
                if (QueryString == null || QueryString.Count == 0)
                {
                    string msg = this.GetLocalized("缺少網頁參數");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);
                this.EditLoanId = QueryString.TryGetValue("LoanId", String.Empty);

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                LoanStandardEntity entity = null;
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = new Expression(LoanStandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(LoanStandardEntity.Field.YearId, this.EditYearId);
                            where.And(LoanStandardEntity.Field.TermId, this.EditTermId);
                            where.And(LoanStandardEntity.Field.DepId, this.EditDepId);
                            where.And(LoanStandardEntity.Field.ReceiveId, this.EditReceiveId);
                            where.And(LoanStandardEntity.Field.LoanId, this.EditLoanId);
                            XmlResult result = DataProxy.Current.SelectFirst<LoanStandardEntity>(this, where, null, out entity);
                            if (result.IsSuccess)
                            {
                                if (entity == null)
                                {
                                    this.ShowSystemMessage(this.GetLocalized("查無指定要修改的資料"));
                                }
                            }
                            else
                            {
                                entity = null;
                                this.ShowSystemMessage(this.GetLocalized("查詢指定要修改的資料失敗") + "，" + result.Message);
                            }
                        }
                        #endregion
                        break;
                    case "A":   //新增
                        #region 產生空的新增資料
                        {
                            entity = new LoanStandardEntity();
                        }
                        #endregion
                        break;
                }

                this.BindEditData(entity);
            }
        }

        /// <summary>
        /// 介面初始話
        /// </summary>
        private void InitialUI()
        {
            this.tbxLoanAmount.Text = String.Empty;
            ddlMemo.Enabled = false;
            ddlReceiveItem.Enabled = false;

            BindLoanIdOptions();
            BindReceiveItemOptions();

        }

        /// <summary>
        /// 顯示單筆明細資料
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData(LoanStandardEntity entity)
        {
            if (entity == null)
            {
                this.tbxLoanAmount.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            EditLoanStandardEntity = entity;

            #region 組 Html
            if (ddlReceiveItem.Items.Count == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            this.litHtml.Text = String.Empty;
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            for (int idx = 0; idx < ddlReceiveItem.Items.Count; idx++)
            {
                string fieldName = "Loan_" + (idx + 1).ToString("00");
                string chkName = "chkItem" + (idx + 1).ToString();
                bool chkChecked = false;

                sb.AppendLine("<tr>");
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", ddlReceiveItem.Items[idx].Text).AppendLine();
                object ob = entity.GetValue(fieldName, out rt);
                if (ob != null)
                {
                    if (!String.IsNullOrEmpty(ob.ToString()))
                    {
                        string fieldValue = ob.ToString();
                        if (fieldValue == "Y")
                        {
                            chkChecked = true;
                        }
                    }
                }
                if (chkChecked)
                {
                    sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$chkItem{0}\" type=\"checkbox\" checked=\"checked\" id=\"ContentPlaceHolder1_chkItem{0}\" /></td>", (idx + 1).ToString()).AppendLine();
                }
                else
                {
                    sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$chkItem{0}\" type=\"checkbox\" id=\"ContentPlaceHolder1_chkItem{0}\" /></td>", (idx + 1).ToString()).AppendLine();
                }
                sb.AppendLine("</tr>");
            }
            this.litHtml.Text = sb.ToString();

            #endregion

            if (this.Action == "A")
            {
                this.tbxLoanAmount.Enabled = true;
            }
            else if (this.Action == "M")
            {
                this.tbxLoanAmount.Enabled = true;
                this.ddlLoanId.Enabled = false;
                ddlLoanId.SelectedValue = entity.LoanId;
                tbxLoanAmount.Text = entity.LoanAmount.ToString();
                ddlReceiveItem.SelectedValue = entity.LoanItem.Trim();
                rdoPayFlag.SelectedValue = entity.PayFlag;
                if (rdoPayFlag.SelectedValue == "Y")
                {
                    #region 是否先行繳費(Y:顯示 ddlMemo)
                    ddlMemo.Enabled = true;
                    ddlReceiveItem.Enabled = true;
                    divLoanAmount.Visible = false;
                    if (String.IsNullOrEmpty(entity.LoanItem.Trim()))
                    {
                        ShowReceiveItemOption(false);
                    }
                    else
                    {
                        ShowReceiveItemOption(true);
                    }
                    #endregion
                }
                else
                {
                    ddlMemo.SelectedValue = "2";
                    ddlMemo.Enabled = false;
                    ddlReceiveItem.Enabled = false;
                    divLoanAmount.Visible = true;
                }
                rdoTakeOffReduce.SelectedValue = entity.TakeoffReduce;
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫
        /// </summary>
        /// <returns></returns>
        private LoanStandardEntity GetEditData()
        {
            LoanStandardEntity entity = new LoanStandardEntity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditLoanStandardEntity;
                    entity.LoanId = this.EditLoanId;
                    entity.PayFlag = rdoPayFlag.SelectedValue;
                    if (rdoPayFlag.SelectedValue == "Y")
                    {
                        entity.LoanAmount = 0;
                        if (ddlMemo.SelectedValue == "2")
                        {
                            entity.LoanItem = ddlReceiveItem.SelectedValue;
                        }
                        else
                        {
                            entity.LoanItem = "";
                        }
                    }
                    else
                    {
                        entity.LoanAmount = Convert.ToDecimal(this.tbxLoanAmount.Text);
                        entity.LoanItem = "";
                    }
                    entity.TakeoffReduce = rdoTakeOffReduce.SelectedValue;
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.LoanId = ddlLoanId.SelectedValue;
                    entity.PayFlag = rdoPayFlag.SelectedValue;
                    if (rdoPayFlag.SelectedValue == "Y")
                    {
                        entity.LoanAmount = 0;
                        if (ddlMemo.SelectedValue == "2")
                        {
                            entity.LoanItem = ddlReceiveItem.SelectedValue;
                        }
                        else
                        {
                            entity.LoanItem = "";
                        }
                    }
                    else
                    {
                        entity.LoanAmount = Convert.ToDecimal(this.tbxLoanAmount.Text);
                        entity.LoanItem = "";
                    }
                    entity.TakeoffReduce = rdoTakeOffReduce.SelectedValue;
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();
            for (int i = 0; i < ddlReceiveItem.Items.Count; i++)
            {
                string fieldName = "Loan_" + (i + 1).ToString("00");
                string chkName = "chkItem" + (i + 1).ToString();

                string chkValue = GetValue(chkName);
                if (chkValue == "on")
                {
                    entity.SetValue(fieldName, "Y");
                }
                else
                {
                    entity.SetValue(fieldName, "N");
                }
            }

            return entity;
        }

        private string GetValue(string ControlID)
        {
            string[] keys = Request.Form.AllKeys;
            string value = string.Empty;
            foreach (string key in keys)
            {
                if (key.IndexOf(ControlID) >= 0)
                {
                    value = Request.Form[key].ToString();
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// 儲存就貸收費資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string errmsg = this.CheckDataForInputUI();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }

            LoanStandardEntity entity = this.GetEditData();

            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        int count = 0;
                        //XmlResult result = DataProxy.Current.UpdateFields<LoanStandardEntity>(this, where, fieldValues.ToArray(), out count);
                        XmlResult result = DataProxy.Current.Update<LoanStandardEntity>(this, entity, out count);
                        if (result.IsSuccess)
                        {
                            if (count < 1)
                            {
                                string msg = String.Format("{0}，{1}", this.GetLocalized("更新資料失敗"), this.GetLocalized("無資料被更新"));
                                this.ShowSystemMessage(msg);
                            }
                            else
                            {
                                string msg = this.GetLocalized("更新資料成功");
                                this.ShowJsAlertAndGoUrl(msg, "D1300005.aspx");
                            }
                        }
                        else
                        {
                            this.ShowSystemMessage(result.Message);
                        }
                    }
                    #endregion
                    break;
                case "A":   //新增
                    #region 新增
                    {
                        int count = 0;

                        XmlResult result = DataProxy.Current.Insert<LoanStandardEntity>(this, entity, out count);
                        if (result.IsSuccess)
                        {
                            if (count < 1)
                            {
                                string msg = String.Format("{0}，{1}", this.GetLocalized("新增資料失敗"), this.GetLocalized("無資料被新增"));
                                this.ShowSystemMessage(msg);
                            }
                            else
                            {
                                string msg = this.GetLocalized("新增資料成功");
                                this.ShowJsAlertAndGoUrl(msg, "D1300005.aspx");
                            }
                        }
                        else
                        {
                            this.ShowSystemMessage(result.Message);
                        }
                    }
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// 檢查資料，以作為使用介面輸入是否正確 (前台用)
        /// </summary>
        /// <returns>傳回錯誤訊息</returns>
        public string CheckDataForInputUI()
        {
            if (!Common.IsMoney(tbxLoanAmount.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("額外之允許扣抵金額");
            }

            return String.Empty;
        }

        /// <summary>
        /// 繫結 院別 下拉選單
        /// </summary>
        private void BindLoanIdOptions()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(LoanListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(LoanListEntity.Field.YearId, this.EditYearId);
            where.And(LoanListEntity.Field.TermId, this.EditTermId);
            where.And(LoanListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(LoanListEntity.Field.LoanId, OrderByEnum.Asc);

            LoanListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<LoanListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (LoanListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.LoanName, data.LoanId);
                    list.Add(new ListItem(text, data.LoanId));
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlLoanId.Items.Clear();
                this.ddlLoanId.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 所屬收入科目 下拉選單
        /// </summary>
        private void BindReceiveItemOptions()
        {
            List<ListItem> list = new List<ListItem>();

            SchoolRidEntity entity = null;

            Expression where = new Expression();
            where.And(SchoolRidEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(SchoolRidEntity.Field.YearId, this.EditYearId);
            where.And(SchoolRidEntity.Field.TermId, this.EditTermId);
            where.And(SchoolRidEntity.Field.DepId, this.EditDepId);
            where.And(SchoolRidEntity.Field.ReceiveId, this.EditReceiveId);
            //ReceiveStatus代收費用型態
            //where.And(SchoolRidEntity.Field.ReceiveStatus, this.EditReceiveType);

            XmlResult result = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out entity);
            if (!result.IsSuccess || entity == null)
            {
                return;
            }

            for (int i = 1; i < 31; i++)
            {
                string fieldName = "Receive_Item" + i.ToString("00");

                Result rt = new Result();
                object ob = entity.GetValue(fieldName, out rt);
                if (ob != null)
                {
                    if (!String.IsNullOrEmpty(ob.ToString()))
                    {
                        ;
                        list.Add(new ListItem(ob.ToString(), i.ToString()));
                    }
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlReceiveItem.Items.Clear();
                //this.ddlReceiveItem.Items.Add(new ListItem("", ""));
                this.ddlReceiveItem.Items.AddRange(items);
            }
        }

        private void ShowReceiveItemOption(bool bShow)
        {
            if (bShow)
            {
                ddlReceiveItem.Visible = true;
                labReceiveItem.Visible = true;
            }
            else
            {
                ddlReceiveItem.Visible = false;
                labReceiveItem.Visible = false;
            }
        }

        protected void ddlMemo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMemo.SelectedValue == "2")
            {
                labReceiveItem.Visible = true;
                ddlReceiveItem.Visible = true;
            }
            else
            {
                labReceiveItem.Visible = false;
                ddlReceiveItem.Visible = false;
            }
        }

        protected void rdoPayFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoPayFlag.SelectedValue == "Y")
            {
                ShowReceiveItemOption(true);
                ddlMemo.Enabled = true;
                ddlReceiveItem.Enabled = true;
                divLoanAmount.Visible = false;
            }
            else
            {
                ShowReceiveItemOption(true);
                ddlMemo.SelectedValue = "2";
                ddlMemo.Enabled = false;
                ddlReceiveItem.Enabled = false;
                divLoanAmount.Visible = true;
            }
        }
    }
}