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
    public partial class D1300007M : BasePage
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
        private string EditDormId
        {
            get
            {
                return ViewState["EditDormId"] as string;
            }
            set
            {
                ViewState["EditDormId"] = value == null ? null : value.Trim();
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
                this.EditDormId = QueryString.TryGetValue("DormId", String.Empty);

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                DormStandardEntity entity = null;
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = new Expression(DormStandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(DormStandardEntity.Field.YearId, this.EditYearId);
                            where.And(DormStandardEntity.Field.TermId, this.EditTermId);
                            where.And(DormStandardEntity.Field.DepId, this.EditDepId);
                            where.And(DormStandardEntity.Field.ReceiveId, this.EditReceiveId);
                            where.And(DormStandardEntity.Field.DormId, this.EditDormId);
                            XmlResult result = DataProxy.Current.SelectFirst<DormStandardEntity>(this, where, null, out entity);
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
                            entity = new DormStandardEntity();
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
            this.tbxDormAmount.Text = String.Empty;

            BindDormIdOptions();
            BindReceiveItemOptions();

        }

        private void BindEditData(DormStandardEntity entity)
        {
            if (entity == null)
            {
                this.tbxDormAmount.Text = String.Empty;
                this.ccbtnOK.Visible = false;
            }
            else
            {
                if (this.Action == "A")
                {
                    this.tbxDormAmount.Enabled = true;
                }
                else if (this.Action == "M")
                {
                    this.tbxDormAmount.Enabled = true;
                    this.ddlDormId.Enabled = false;
                    ddlDormId.SelectedValue = entity.DormId;
                    tbxDormAmount.Text = DataFormat.GetAmountText(entity.DormAmount);
                    ddlReceiveItem.SelectedValue = entity.DormItem;
                }
                this.ccbtnOK.Visible = true;
            }
        }

        private DormStandardEntity GetEditData()
        {
            DormStandardEntity entity = new DormStandardEntity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":   //修改
                    entity.DormId = this.EditDormId;
                    entity.DormAmount = Convert.ToDecimal(this.tbxDormAmount.Text);
                    entity.DormItem = ddlReceiveItem.SelectedValue;

                    break;
                case "A":   //新增
                    entity.DormId = ddlDormId.SelectedValue;
                    entity.DormAmount = Convert.ToDecimal(this.tbxDormAmount.Text);
                    entity.DormItem = ddlReceiveItem.SelectedValue;
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

            DormStandardEntity entity = this.GetEditData();

            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        Expression where = new Expression(DormStandardEntity.Field.ReceiveType, entity.ReceiveType);
                        where.And(DormStandardEntity.Field.YearId, this.EditYearId);
                        where.And(DormStandardEntity.Field.TermId, this.EditTermId);
                        where.And(DormStandardEntity.Field.DepId, this.EditDepId);
                        where.And(DormStandardEntity.Field.ReceiveId, this.EditReceiveId);
                        where.And(DormStandardEntity.Field.DormId, this.EditDormId);

                        KeyValueList fieldValues = new KeyValueList();
                        fieldValues.Add(DormStandardEntity.Field.DormItem, entity.DormItem);
                        fieldValues.Add(DormStandardEntity.Field.DormAmount, entity.DormAmount);
                        fieldValues.Add(DormStandardEntity.Field.MdyUser, this.GetLogonUser().UserId);
                        fieldValues.Add(DormStandardEntity.Field.MdyDate, DateTime.Now);
                        int count = 0;
                        XmlResult result = DataProxy.Current.UpdateFields<DormStandardEntity>(this, where, fieldValues.ToArray(), out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300007.aspx");
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

                        XmlResult result = DataProxy.Current.Insert<DormStandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300007.aspx");
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

        #region Method
        /// <summary>
        /// 檢查資料，以作為使用介面輸入是否正確 (前台用)
        /// </summary>
        /// <returns>傳回錯誤訊息</returns>
        public string CheckDataForInputUI()
        {
            if (!Common.IsMoney(tbxDormAmount.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("住宿金額");
            }

            return String.Empty;
        }
        #endregion

        /// <summary>
        /// 繫結 院別 下拉選單
        /// </summary>
        private void BindDormIdOptions()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(DormListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(DormListEntity.Field.YearId, this.EditYearId);
            where.And(DormListEntity.Field.TermId, this.EditTermId);
            where.And(DormListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(DormListEntity.Field.DormId, OrderByEnum.Asc);

            DormListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<DormListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (DormListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.DormName, data.DormId);
                    list.Add(new ListItem(text, data.DormId));
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlDormId.Items.Clear();
                this.ddlDormId.Items.AddRange(items);
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