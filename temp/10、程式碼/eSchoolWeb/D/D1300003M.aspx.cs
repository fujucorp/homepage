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
    public partial class D1300003M : BasePage
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
        /// 編輯的學分基準代碼參數
        /// </summary>
        private string EditCreditbId
        {
            get
            {
                return ViewState["EditCreditbId"] as string;
            }
            set
            {
                ViewState["EditCreditbId"] = value == null ? null : value.Trim();
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
                this.EditCreditbId = QueryString.TryGetValue("CreditbId", String.Empty);

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                CreditbStandardEntity entity = null;
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = new Expression(CreditbStandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(CreditbStandardEntity.Field.YearId, this.EditYearId);
                            where.And(CreditbStandardEntity.Field.TermId, this.EditTermId);
                            where.And(CreditbStandardEntity.Field.DepId, this.EditDepId);
                            where.And(CreditbStandardEntity.Field.ReceiveId, this.EditReceiveId);
                            where.And(CreditbStandardEntity.Field.CreditbId, this.EditCreditbId);
                            XmlResult result = DataProxy.Current.SelectFirst<CreditbStandardEntity>(this, where, null, out entity);
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
                            entity = new CreditbStandardEntity();
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
            this.tbxCreditbId.Text = String.Empty;
            this.tbxCreditbName.Text = String.Empty;
            this.tbxCreditbCprice.Text = String.Empty;
            BindReceiveItemOptions();
        }

        private void BindEditData(CreditbStandardEntity entity)
        {
            if (entity == null)
            {
                this.tbxCreditbId.Text = String.Empty;
                this.tbxCreditbName.Text = String.Empty;
                this.tbxCreditbCprice.Text = String.Empty;
                this.ccbtnOK.Visible = false;
            }
            else
            {
                if (this.Action == "A")
                {
                    this.tbxCreditbId.Enabled = true;
                    this.tbxCreditbName.Enabled = true;
                    this.tbxCreditbCprice.Enabled = true;
                }
                else if (this.Action == "M")
                {
                    this.tbxCreditbId.Enabled = false;
                    this.tbxCreditbName.Enabled = true;
                    this.tbxCreditbCprice.Enabled = true;
                    tbxCreditbId.Text = entity.CreditbId;
                    tbxCreditbCprice.Text = entity.CreditbCprice.ToString();
                    tbxCreditbName.Text = entity.CreditbName;

                    #region [MDY:20210401] 原碼修正
                    #region [OLD]
                    //ddlReceiveItem.SelectedValue = entity.CreditItem;
                    #endregion

                    WebHelper.SetDropDownListSelectedValue(this.ddlReceiveItem, entity.CreditItem);
                    #endregion
                }
                this.ccbtnOK.Visible = true;
            }
        }

        private CreditbStandardEntity GetEditData()
        {
            CreditbStandardEntity entity = new CreditbStandardEntity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":   //修改
                    entity.CreditbId = this.EditCreditbId;
                    entity.CreditbName = this.tbxCreditbName.Text;
                    entity.CreditbCprice = Convert.ToDecimal(this.tbxCreditbCprice.Text);
                    entity.CreditItem = ddlReceiveItem.SelectedValue;
                    break;
                case "A":   //新增
                    entity.CreditbId = this.tbxCreditbId.Text;
                    entity.CreditbName = this.tbxCreditbName.Text;
                    entity.CreditbCprice = Convert.ToDecimal(this.tbxCreditbCprice.Text);
                    entity.CreditItem = ddlReceiveItem.SelectedValue;
                    break;
            }
            return entity;
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string errmsg = this.CheckDataForInputUI();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }

            CreditbStandardEntity entity = this.GetEditData();

            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        Expression where = new Expression(CreditbStandardEntity.Field.ReceiveType, entity.ReceiveType);
                        where.And(CreditbStandardEntity.Field.YearId, this.EditYearId);
                        where.And(CreditbStandardEntity.Field.TermId, this.EditTermId);
                        where.And(CreditbStandardEntity.Field.DepId, this.EditDepId);
                        where.And(CreditbStandardEntity.Field.ReceiveId, this.EditReceiveId);
                        where.And(CreditbStandardEntity.Field.CreditbId, this.EditCreditbId);

                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(CreditbStandardEntity.Field.CreditbName, entity.CreditbName);
                        fieldValues.Add(CreditbStandardEntity.Field.CreditbCprice, entity.CreditbCprice);
                        fieldValues.Add(CreditbStandardEntity.Field.CreditItem, entity.CreditItem);
                        fieldValues.Add(CreditbStandardEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(CreditbStandardEntity.Field.MdyDate, DateTime.Now);
                        int count = 0;
                        XmlResult result = DataProxy.Current.UpdateFields<CreditbStandardEntity>(this, where, fieldValues.ToArray(), out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300003.aspx");
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
                        entity.Status = DataStatusCodeTexts.NORMAL;
                        entity.CrtUser = this.GetLogonUser().UserId;
                        entity.CrtDate = DateTime.Now;
                        int count = 0;

                        XmlResult result = DataProxy.Current.Insert<CreditbStandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300003.aspx");
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
            if (String.IsNullOrEmpty(tbxCreditbId.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("學分基準代碼");
            }

            if (String.IsNullOrEmpty(tbxCreditbName.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("學分基準名稱");
            }

            if (!Common.IsMoney(tbxCreditbCprice.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("每學分基準單價");
            }

            return String.Empty;
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
                        //string text = String.Format("{0}({1})", entity.ReceiveItem01, data.CollegeId);
                        list.Add(new ListItem(ob.ToString(), i.ToString("00")));
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
    }
}