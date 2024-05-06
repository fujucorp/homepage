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
    public partial class D1300008M : BasePage
    {
        public const string pFieldName1 = "Id_Num";
        public const string pFieldName2 = "Id_Dno";
        public const string aFieldName1 = "Id_Amount";

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
        /// 編輯的身分註記類別
        /// </summary>
        private string EditIdentifyType
        {
            get
            {
                return ViewState["EditIdentifyType"] as string;
            }
            set
            {
                ViewState["EditIdentifyType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的身分註記代碼參數
        /// </summary>
        private string EditIdentifyId
        {
            get
            {
                return ViewState["EditIdentifyId"] as string;
            }
            set
            {
                ViewState["EditIdentifyId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的身分註記1
        /// </summary>
        private IdentifyStandard1Entity EditIdentifyStandard1Entity
        {
            get
            {
                return ViewState["EditIdentifyStandard1Entity"] as IdentifyStandard1Entity;
            }
            set
            {
                ViewState["EditIdentifyStandard1Entity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的身分註記2
        /// </summary>
        private IdentifyStandard2Entity EditIdentifyStandard2Entity
        {
            get
            {
                return ViewState["EditIdentifyStandard2Entity"] as IdentifyStandard2Entity;
            }
            set
            {
                ViewState["EditIdentifyStandard2Entity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的身分註記3
        /// </summary>
        private IdentifyStandard3Entity EditIdentifyStandard3Entity
        {
            get
            {
                return ViewState["EditIdentifyStandard3Entity"] as IdentifyStandard3Entity;
            }
            set
            {
                ViewState["EditIdentifyStandard3Entity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的身分註記4
        /// </summary>
        private IdentifyStandard4Entity EditIdentifyStandard4Entity
        {
            get
            {
                return ViewState["EditIdentifyStandard4Entity"] as IdentifyStandard4Entity;
            }
            set
            {
                ViewState["EditIdentifyStandard4Entity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的身分註記5
        /// </summary>
        private IdentifyStandard5Entity EditIdentifyStandard5Entity
        {
            get
            {
                return ViewState["EditIdentifyStandard5Entity"] as IdentifyStandard5Entity;
            }
            set
            {
                ViewState["EditIdentifyStandard5Entity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的身分註記6
        /// </summary>
        private IdentifyStandard6Entity EditIdentifyStandard6Entity
        {
            get
            {
                return ViewState["EditIdentifyStandard6Entity"] as IdentifyStandard6Entity;
            }
            set
            {
                ViewState["EditIdentifyStandard6Entity"] = value == null ? null : value;
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
                this.EditIdentifyId = QueryString.TryGetValue("IdentifyId", String.Empty);
                this.EditIdentifyType = QueryString.TryGetValue("IdentifyType", String.Empty);

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                IdentifyStandard1Entity entity1 = null;
                IdentifyStandard2Entity entity2 = null;
                IdentifyStandard3Entity entity3 = null;
                IdentifyStandard4Entity entity4 = null;
                IdentifyStandard5Entity entity5 = null;
                IdentifyStandard6Entity entity6 = null;
                bool hasData = true;

                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = null;
                            XmlResult result = null;
                            switch (this.EditIdentifyType)
                            {
                                case "1":
                                    where = getIdentify1Where(true);
                                    result = DataProxy.Current.SelectFirst<IdentifyStandard1Entity>(this, where, null, out entity1);
                                    if (entity1 == null) hasData = false;
                                    break;
                                case "2":
                                    where = getIdentify2Where(true);
                                    result = DataProxy.Current.SelectFirst<IdentifyStandard2Entity>(this, where, null, out entity2);
                                    if (entity2 == null) hasData = false;
                                    break;
                                case "3":
                                    where = getIdentify3Where(true);
                                    result = DataProxy.Current.SelectFirst<IdentifyStandard3Entity>(this, where, null, out entity3);
                                    if (entity3 == null) hasData = false;
                                    break;
                                case "4":
                                    where = getIdentify4Where(true);
                                    result = DataProxy.Current.SelectFirst<IdentifyStandard4Entity>(this, where, null, out entity4);
                                    if (entity4 == null) hasData = false;
                                    break;
                                case "5":
                                    where = getIdentify5Where(true);
                                    result = DataProxy.Current.SelectFirst<IdentifyStandard5Entity>(this, where, null, out entity5);
                                    if (entity5 == null) hasData = false;
                                    break;
                                case "6":
                                    where = getIdentify6Where(true);
                                    result = DataProxy.Current.SelectFirst<IdentifyStandard6Entity>(this, where, null, out entity6);
                                    if (entity6 == null) hasData = false;
                                    break;
                            }

                            if (result.IsSuccess)
                            {
                                if (!hasData)
                                {
                                    this.ShowSystemMessage(this.GetLocalized("查無指定要修改的資料"));
                                    return;
                                }
                            }
                            else
                            {
                                this.ShowSystemMessage(this.GetLocalized("查詢指定要修改的資料失敗") + "，" + result.Message);
                                return;
                            }
                        }
                        #endregion
                        break;
                    case "A":   //新增
                        #region 產生空的新增資料
                        {
                            switch (this.EditIdentifyType)
                            {
                                case "1":
                                    entity1 = new IdentifyStandard1Entity();
                                    entity1.IdWay = "1";    //預設值
                                    this.EditIdentifyId = ddlIdentifyId.SelectedValue;
                                    break;
                                case "2":
                                    entity2 = new IdentifyStandard2Entity();
                                    entity2.IdWay = "1";    //預設值
                                    this.EditIdentifyId = ddlIdentifyId.SelectedValue;
                                    break;
                                case "3":
                                    entity3 = new IdentifyStandard3Entity();
                                    entity3.IdWay = "1";    //預設值
                                    this.EditIdentifyId = ddlIdentifyId.SelectedValue;
                                    break;
                                case "4":
                                    entity4 = new IdentifyStandard4Entity();
                                    entity4.IdWay = "1";    //預設值
                                    this.EditIdentifyId = ddlIdentifyId.SelectedValue;
                                    break;
                                case "5":
                                    entity5 = new IdentifyStandard5Entity();
                                    entity5.IdWay = "1";    //預設值
                                    this.EditIdentifyId = ddlIdentifyId.SelectedValue;
                                    break;
                                case "6":
                                    entity6 = new IdentifyStandard6Entity();
                                    entity6.IdWay = "1";    //預設值
                                    this.EditIdentifyId = ddlIdentifyId.SelectedValue;
                                    break;
                            }
                        }
                        #endregion
                        break;
                }

                switch (this.EditIdentifyType)
                {
                    case "1":
                        this.BindEditData1(entity1);
                        break;
                    case "2":
                        this.BindEditData2(entity2);
                        break;
                    case "3":
                        this.BindEditData3(entity3);
                        break;
                    case "4":
                        this.BindEditData4(entity4);
                        break;
                    case "5":
                        this.BindEditData5(entity5);
                        break;
                    case "6":
                        this.BindEditData6(entity6);
                        break;
                }
            }
        }

        #region getIdentifyWhere 1~6
        protected Expression getIdentify1Where(bool hasIdentifyId)
        {
            Expression where = new Expression();
            where.And(IdentifyStandard1Entity.Field.ReceiveType, this.EditReceiveType);
            where.And(IdentifyStandard1Entity.Field.YearId, this.EditYearId);
            where.And(IdentifyStandard1Entity.Field.TermId, this.EditTermId);
            where.And(IdentifyStandard1Entity.Field.DepId, this.EditDepId);
            if (hasIdentifyId)
            {
                where.And(IdentifyStandard1Entity.Field.ReceiveId, this.EditReceiveId);
                where.And(IdentifyStandard1Entity.Field.IdentifyId, this.EditIdentifyId);
            }

            return where;
        }

        protected Expression getIdentify2Where(bool hasIdentifyId)
        {
            Expression where = new Expression();
            where.And(IdentifyStandard2Entity.Field.ReceiveType, this.EditReceiveType);
            where.And(IdentifyStandard2Entity.Field.YearId, this.EditYearId);
            where.And(IdentifyStandard2Entity.Field.TermId, this.EditTermId);
            where.And(IdentifyStandard2Entity.Field.DepId, this.EditDepId);
            if (hasIdentifyId)
            {
                where.And(IdentifyStandard2Entity.Field.ReceiveId, this.EditReceiveId);
                where.And(IdentifyStandard2Entity.Field.IdentifyId, this.EditIdentifyId);
            }

            return where;
        }

        protected Expression getIdentify3Where(bool hasIdentifyId)
        {
            Expression where = new Expression();
            where.And(IdentifyStandard3Entity.Field.ReceiveType, this.EditReceiveType);
            where.And(IdentifyStandard3Entity.Field.YearId, this.EditYearId);
            where.And(IdentifyStandard3Entity.Field.TermId, this.EditTermId);
            where.And(IdentifyStandard3Entity.Field.DepId, this.EditDepId);
            if (hasIdentifyId)
            {
                where.And(IdentifyStandard3Entity.Field.ReceiveId, this.EditReceiveId);
                where.And(IdentifyStandard3Entity.Field.IdentifyId, this.EditIdentifyId);
            }

            return where;
        }

        protected Expression getIdentify4Where(bool hasIdentifyId)
        {
            Expression where = new Expression();
            where.And(IdentifyStandard4Entity.Field.ReceiveType, this.EditReceiveType);
            where.And(IdentifyStandard4Entity.Field.YearId, this.EditYearId);
            where.And(IdentifyStandard4Entity.Field.TermId, this.EditTermId);
            where.And(IdentifyStandard4Entity.Field.DepId, this.EditDepId);
            if (hasIdentifyId)
            {
                where.And(IdentifyStandard4Entity.Field.ReceiveId, this.EditReceiveId);
                where.And(IdentifyStandard4Entity.Field.IdentifyId, this.EditIdentifyId);
            }

            return where;
        }

        protected Expression getIdentify5Where(bool hasIdentifyId)
        {
            Expression where = new Expression();
            where.And(IdentifyStandard5Entity.Field.ReceiveType, this.EditReceiveType);
            where.And(IdentifyStandard5Entity.Field.YearId, this.EditYearId);
            where.And(IdentifyStandard5Entity.Field.TermId, this.EditTermId);
            where.And(IdentifyStandard5Entity.Field.DepId, this.EditDepId);
            if (hasIdentifyId)
            {
                where.And(IdentifyStandard5Entity.Field.ReceiveId, this.EditReceiveId);
                where.And(IdentifyStandard5Entity.Field.IdentifyId, this.EditIdentifyId);
            }

            return where;
        }

        protected Expression getIdentify6Where(bool hasIdentifyId)
        {
            Expression where = new Expression();
            where.And(IdentifyStandard6Entity.Field.ReceiveType, this.EditReceiveType);
            where.And(IdentifyStandard6Entity.Field.YearId, this.EditYearId);
            where.And(IdentifyStandard6Entity.Field.TermId, this.EditTermId);
            where.And(IdentifyStandard6Entity.Field.DepId, this.EditDepId);
            if (hasIdentifyId)
            {
                where.And(IdentifyStandard6Entity.Field.ReceiveId, this.EditReceiveId);
                where.And(IdentifyStandard6Entity.Field.IdentifyId, this.EditIdentifyId);
            }

            return where;
        }
        #endregion

        /// <summary>
        /// 介面初始話
        /// </summary>
        private void InitialUI()
        {
            ddlIdentifyId.Enabled = false;
            BindIdentifyIdOptions();
            BindReceiveItemOptions();
            ddlReceiveItem.Enabled = false;
        }

        /// <summary>
        /// 繫結 所屬收入科目 下拉選單
        /// </summary>
        private void BindReceiveItemOptions()
        {
            ListItem[] items = GetReceiveItems();
            if (items != null && items.Length > 0)
            {
                this.ddlReceiveItem.Items.Clear();
                //this.ddlReceiveItem.Items.Add(new ListItem("", ""));
                this.ddlReceiveItem.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 顯示單筆明細資料1
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData1(IdentifyStandard1Entity entity)
        {
            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            this.EditIdentifyStandard1Entity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();
            this.litHtml.Text = String.Empty;

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.IdWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent1(ReceiveItems, this.EditIdentifyStandard1Entity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount1(ReceiveItems, this.EditIdentifyStandard1Entity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlIdentifyId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlIdentifyId.Enabled = false;
                ddlIdWay.SelectedValue = entity.IdWay;
                if (entity.IdWay == "2")  //依金額，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.IdItem.Trim();
                    if (entity.IdItem.Trim().Length > 0)
                    {
                        ddlMemo.SelectedValue = "2";
                        ShowReceiveItemOption(true);
                        ddlReceiveItem.Enabled = true;
                    }
                    else
                    {
                        ddlMemo.SelectedValue = "1";
                        ShowReceiveItemOption(false);
                    }
                }
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 顯示單筆明細資料2
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData2(IdentifyStandard2Entity entity)
        {

            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditIdentifyStandard2Entity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();
            this.litHtml.Text = String.Empty;

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.IdWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent2(ReceiveItems, this.EditIdentifyStandard2Entity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount2(ReceiveItems, this.EditIdentifyStandard2Entity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlIdentifyId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlIdentifyId.Enabled = false;
                ddlIdWay.SelectedValue = entity.IdWay;
                if (entity.IdWay == "2")  //依金額，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.IdItem.Trim();
                    if (entity.IdItem.Trim().Length > 0)
                    {
                        ddlMemo.SelectedValue = "2";
                        ShowReceiveItemOption(true);
                        ddlReceiveItem.Enabled = true;
                    }
                    else
                    {
                        ddlMemo.SelectedValue = "1";
                        ShowReceiveItemOption(false);
                    }
                }
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 顯示單筆明細資料3
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData3(IdentifyStandard3Entity entity)
        {

            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditIdentifyStandard3Entity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();
            this.litHtml.Text = String.Empty;

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.IdWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent3(ReceiveItems, this.EditIdentifyStandard3Entity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount3(ReceiveItems, this.EditIdentifyStandard3Entity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlIdentifyId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlIdentifyId.Enabled = false;
                ddlIdWay.SelectedValue = entity.IdWay;
                if (entity.IdWay == "2")  //依金額，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.IdItem.Trim();
                    if (entity.IdItem.Trim().Length > 0)
                    {
                        ddlMemo.SelectedValue = "2";
                        ShowReceiveItemOption(true);
                        ddlReceiveItem.Enabled = true;
                    }
                    else
                    {
                        ddlMemo.SelectedValue = "1";
                        ShowReceiveItemOption(false);
                    }
                }
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 顯示單筆明細資料4
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData4(IdentifyStandard4Entity entity)
        {

            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditIdentifyStandard4Entity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();
            this.litHtml.Text = String.Empty;

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.IdWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent4(ReceiveItems, this.EditIdentifyStandard4Entity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount4(ReceiveItems, this.EditIdentifyStandard4Entity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlIdentifyId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlIdentifyId.Enabled = false;
                ddlIdWay.SelectedValue = entity.IdWay;
                if (entity.IdWay == "2")  //依金額，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.IdItem.Trim();
                    if (entity.IdItem.Trim().Length > 0)
                    {
                        ddlMemo.SelectedValue = "2";
                        ShowReceiveItemOption(true);
                        ddlReceiveItem.Enabled = true;
                    }
                    else
                    {
                        ddlMemo.SelectedValue = "1";
                        ShowReceiveItemOption(false);
                    }
                }
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 顯示單筆明細資料5
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData5(IdentifyStandard5Entity entity)
        {

            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditIdentifyStandard5Entity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();
            this.litHtml.Text = String.Empty;

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.IdWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent5(ReceiveItems, this.EditIdentifyStandard5Entity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount5(ReceiveItems, this.EditIdentifyStandard5Entity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlIdentifyId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlIdentifyId.Enabled = false;
                ddlIdWay.SelectedValue = entity.IdWay;
                if (entity.IdWay == "2")  //依金額，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.IdItem.Trim();
                    if (entity.IdItem.Trim().Length > 0)
                    {
                        ddlMemo.SelectedValue = "2";
                        ShowReceiveItemOption(true);
                        ddlReceiveItem.Enabled = true;
                    }
                    else
                    {
                        ddlMemo.SelectedValue = "1";
                        ShowReceiveItemOption(false);
                    }
                }
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 顯示單筆明細資料6
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData6(IdentifyStandard6Entity entity)
        {

            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditIdentifyStandard6Entity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();
            this.litHtml.Text = String.Empty;

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.IdWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent6(ReceiveItems, this.EditIdentifyStandard6Entity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount6(ReceiveItems, this.EditIdentifyStandard6Entity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlIdentifyId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlIdentifyId.Enabled = false;
                ddlIdWay.SelectedValue = entity.IdWay;
                if (entity.IdWay == "2")  //依金額，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.IdItem.Trim();
                    if (entity.IdItem.Trim().Length > 0)
                    {
                        ddlMemo.SelectedValue = "2";
                        ShowReceiveItemOption(true);
                        ddlReceiveItem.Enabled = true;
                    }
                    else
                    {
                        ddlMemo.SelectedValue = "1";
                        ShowReceiveItemOption(false);
                    }
                }
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 依百分比計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyPercent1(ListItem[] ReceiveItems, IdentifyStandard1Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分母</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身分註記分子
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        fieldValue1 = ob1.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                #region 身分註記分母
                object ob2 = entity.GetValue(fieldName2, out rt);
                if (ob2 != null)
                {
                    if (!String.IsNullOrEmpty(ob2.ToString()))
                    {
                        fieldValue2 = ob2.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName2, fieldValue2).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount1(ListItem[] ReceiveItems, IdentifyStandard1Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身份註記項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //身份註記項目金額
                string fieldValue1 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身份註記項目金額
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        decimal d = 0;
                        if (decimal.TryParse(ob1.ToString(), out d))
                        {
                            fieldValue1 = d.ToString("##0");
                        }
                        else
                        {
                            fieldValue1 = ob1.ToString();
                        }
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依百分比計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyPercent2(ListItem[] ReceiveItems, IdentifyStandard2Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分母</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身分註記分子
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        fieldValue1 = ob1.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                #region 身分註記分母
                object ob2 = entity.GetValue(fieldName2, out rt);
                if (ob2 != null)
                {
                    if (!String.IsNullOrEmpty(ob2.ToString()))
                    {
                        fieldValue2 = ob2.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName2, fieldValue2).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount2(ListItem[] ReceiveItems, IdentifyStandard2Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身份註記項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //身份註記項目金額
                string fieldValue1 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身份註記項目金額
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        decimal d = 0;
                        if (decimal.TryParse(ob1.ToString(), out d))
                        {
                            fieldValue1 = d.ToString("##0");
                        }
                        else
                        {
                            fieldValue1 = ob1.ToString();
                        }
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依百分比計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyPercent3(ListItem[] ReceiveItems, IdentifyStandard3Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分母</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身分註記分子
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        fieldValue1 = ob1.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                #region 身分註記分母
                object ob2 = entity.GetValue(fieldName2, out rt);
                if (ob2 != null)
                {
                    if (!String.IsNullOrEmpty(ob2.ToString()))
                    {
                        fieldValue2 = ob2.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName2, fieldValue2).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount3(ListItem[] ReceiveItems, IdentifyStandard3Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身份註記項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //身份註記項目金額
                string fieldValue1 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身份註記項目金額
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        decimal d = 0;
                        if (decimal.TryParse(ob1.ToString(), out d))
                        {
                            fieldValue1 = d.ToString("##0");
                        }
                        else
                        {
                            fieldValue1 = ob1.ToString();
                        }
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依百分比計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyPercent4(ListItem[] ReceiveItems, IdentifyStandard4Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分母</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身分註記分子
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        fieldValue1 = ob1.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                #region 身分註記分母
                object ob2 = entity.GetValue(fieldName2, out rt);
                if (ob2 != null)
                {
                    if (!String.IsNullOrEmpty(ob2.ToString()))
                    {
                        fieldValue2 = ob2.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName2, fieldValue2).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount4(ListItem[] ReceiveItems, IdentifyStandard4Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身份註記項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //身份註記項目金額
                string fieldValue1 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身份註記項目金額
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        decimal d = 0;
                        if (decimal.TryParse(ob1.ToString(), out d))
                        {
                            fieldValue1 = d.ToString("##0");
                        }
                        else
                        {
                            fieldValue1 = ob1.ToString();
                        }
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依百分比計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyPercent5(ListItem[] ReceiveItems, IdentifyStandard5Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分母</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身分註記分子
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        fieldValue1 = ob1.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                #region 身分註記分母
                object ob2 = entity.GetValue(fieldName2, out rt);
                if (ob2 != null)
                {
                    if (!String.IsNullOrEmpty(ob2.ToString()))
                    {
                        fieldValue2 = ob2.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName2, fieldValue2).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount5(ListItem[] ReceiveItems, IdentifyStandard5Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身份註記項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //身份註記項目金額
                string fieldValue1 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身份註記項目金額
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        decimal d = 0;
                        if (decimal.TryParse(ob1.ToString(), out d))
                        {
                            fieldValue1 = d.ToString("##0");
                        }
                        else
                        {
                            fieldValue1 = ob1.ToString();
                        }
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依百分比計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyPercent6(ListItem[] ReceiveItems, IdentifyStandard6Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">身分註記分母</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身分註記分子
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        fieldValue1 = ob1.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                #region 身分註記分母
                object ob2 = entity.GetValue(fieldName2, out rt);
                if (ob2 != null)
                {
                    if (!String.IsNullOrEmpty(ob2.ToString()))
                    {
                        fieldValue2 = ob2.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName2, fieldValue2).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount6(ListItem[] ReceiveItems, IdentifyStandard6Entity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">身份註記項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //身份註記項目金額
                string fieldValue1 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 身份註記項目金額
                object ob1 = entity.GetValue(fieldName1, out rt);
                if (ob1 != null)
                {
                    if (!String.IsNullOrEmpty(ob1.ToString()))
                    {
                        decimal d = 0;
                        if (decimal.TryParse(ob1.ToString(), out d))
                        {
                            fieldValue1 = d.ToString("##0");
                        }
                        else
                        {
                            fieldValue1 = ob1.ToString();
                        }
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName1, fieldValue1).AppendLine();
                #endregion

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫1
        /// </summary>
        /// <returns></returns>
        private IdentifyStandard1Entity GetEditData1()
        {
            IdentifyStandard1Entity entity = new IdentifyStandard1Entity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditIdentifyStandard1Entity;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.IdentifyId = ddlIdentifyId.SelectedValue;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            #region 處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                switch (ddlIdWay.SelectedValue)
                {
                    case "1":   //依百分比計算 
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                        string fieldValue1 = "0";
                        string fieldValue2 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        break;
                    case "2":   //依金額計算
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");    //身份註記項目金額
                        fieldValue1 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }
            #endregion

            return entity;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫2
        /// </summary>
        /// <returns></returns>
        private IdentifyStandard2Entity GetEditData2()
        {
            IdentifyStandard2Entity entity = new IdentifyStandard2Entity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditIdentifyStandard2Entity;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.IdentifyId = ddlIdentifyId.SelectedValue;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            #region 處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                switch (ddlIdWay.SelectedValue)
                {
                    case "1":   //依百分比計算 
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                        string fieldValue1 = "0";
                        string fieldValue2 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        break;
                    case "2":   //依金額計算
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");    //身份註記項目金額
                        fieldValue1 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }
            #endregion

            return entity;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫3
        /// </summary>
        /// <returns></returns>
        private IdentifyStandard3Entity GetEditData3()
        {
            IdentifyStandard3Entity entity = new IdentifyStandard3Entity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditIdentifyStandard3Entity;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.IdentifyId = ddlIdentifyId.SelectedValue;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            #region 處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                switch (ddlIdWay.SelectedValue)
                {
                    case "1":   //依百分比計算 
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                        string fieldValue1 = "0";
                        string fieldValue2 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        break;
                    case "2":   //依金額計算
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");    //身份註記項目金額
                        fieldValue1 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }
            #endregion

            return entity;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫4
        /// </summary>
        /// <returns></returns>
        private IdentifyStandard4Entity GetEditData4()
        {
            IdentifyStandard4Entity entity = new IdentifyStandard4Entity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditIdentifyStandard4Entity;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.IdentifyId = ddlIdentifyId.SelectedValue;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            #region 處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                switch (ddlIdWay.SelectedValue)
                {
                    case "1":   //依百分比計算 
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                        string fieldValue1 = "0";
                        string fieldValue2 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        break;
                    case "2":   //依金額計算
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");    //身份註記項目金額
                        fieldValue1 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }
            #endregion

            return entity;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫5
        /// </summary>
        /// <returns></returns>
        private IdentifyStandard5Entity GetEditData5()
        {
            IdentifyStandard5Entity entity = new IdentifyStandard5Entity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditIdentifyStandard5Entity;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.IdentifyId = ddlIdentifyId.SelectedValue;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            #region 處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                switch (ddlIdWay.SelectedValue)
                {
                    case "1":   //依百分比計算 
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                        string fieldValue1 = "0";
                        string fieldValue2 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        break;
                    case "2":   //依金額計算
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");    //身份註記項目金額
                        fieldValue1 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }
            #endregion

            return entity;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫6
        /// </summary>
        /// <returns></returns>
        private IdentifyStandard6Entity GetEditData6()
        {
            IdentifyStandard6Entity entity = new IdentifyStandard6Entity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":
                    #region 修改
                    entity = EditIdentifyStandard6Entity;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                    #endregion
                case "A":
                    #region 新增
                    entity.IdentifyId = ddlIdentifyId.SelectedValue;
                    entity.IdItem = ddlReceiveItem.SelectedValue;
                    entity.IdWay = ddlIdWay.SelectedValue;
                    if (entity.IdWay == "1")  //依百分比計算 ddlMemo 顯示
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.IdItem = string.Empty;
                        }
                        else
                        {
                            entity.IdItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.IdItem = string.Empty;
                    }
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
                    #endregion
            }

            #region 處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                switch (ddlIdWay.SelectedValue)
                {
                    case "1":   //依百分比計算 
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //身分註記分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //身分註記分母
                        string fieldValue1 = "0";
                        string fieldValue2 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        break;
                    case "2":   //依金額計算
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");    //身份註記項目金額
                        fieldValue1 = "0";

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }
            #endregion

            return entity;
        }

        /// <summary>
        /// 取得控制項的值
        /// </summary>
        /// <param name="ControlID"></param>
        /// <returns></returns>
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
        /// 檢查資料，以作為使用介面輸入是否正確 (前台用)
        /// </summary>
        /// <returns>傳回錯誤訊息</returns>
        public string CheckDataForInputUI()
        {
            if (ddlIdentifyId.SelectedValue.Trim() == "")
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("身分註記別");
            }

            return String.Empty;
        }

        /// <summary>
        /// 儲存就貸收費資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            //檢查輸入資料正確性
            string errmsg = this.CheckDataForInputUI();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }

            IdentifyStandard1Entity entity1 = null;
            IdentifyStandard2Entity entity2 = null;
            IdentifyStandard3Entity entity3 = null;
            IdentifyStandard4Entity entity4 = null;
            IdentifyStandard5Entity entity5 = null;
            IdentifyStandard6Entity entity6 = null;
            switch (this.EditIdentifyType)
            {
                case "1":
                    entity1 = this.GetEditData1();
                    break;
                case "2":
                    entity2 = this.GetEditData2();
                    break;
                case "3":
                    entity3 = this.GetEditData3();
                    break;
                case "4":
                    entity4 = this.GetEditData4();
                    break;
                case "5":
                    entity5 = this.GetEditData5();
                    break;
                case "6":
                    entity6 = this.GetEditData6();
                    break;
            }


            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        int count = 0;
                        XmlResult result = null;
                        switch (this.EditIdentifyType)
                        {
                            case "1":
                                result = DataProxy.Current.Update<IdentifyStandard1Entity>(this, entity1, out count);
                                break;
                            case "2":
                                result = DataProxy.Current.Update<IdentifyStandard2Entity>(this, entity2, out count);
                                break;
                            case "3":
                                result = DataProxy.Current.Update<IdentifyStandard3Entity>(this, entity3, out count);
                                break;
                            case "4":
                                result = DataProxy.Current.Update<IdentifyStandard4Entity>(this, entity4, out count);
                                break;
                            case "5":
                                result = DataProxy.Current.Update<IdentifyStandard5Entity>(this, entity5, out count);
                                break;
                            case "6":
                                result = DataProxy.Current.Update<IdentifyStandard6Entity>(this, entity6, out count);
                                break;
                        }
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300008.aspx?IdentifyType=" + this.EditIdentifyType);
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

                        XmlResult result = null;

                        switch (this.EditIdentifyType)
                        {
                            case "1":
                                result = DataProxy.Current.Insert<IdentifyStandard1Entity>(this, entity1, out count);
                                break;
                            case "2":
                                result = DataProxy.Current.Insert<IdentifyStandard2Entity>(this, entity2, out count);
                                break;
                            case "3":
                                result = DataProxy.Current.Insert<IdentifyStandard3Entity>(this, entity3, out count);
                                break;
                            case "4":
                                result = DataProxy.Current.Insert<IdentifyStandard4Entity>(this, entity4, out count);
                                break;
                            case "5":
                                result = DataProxy.Current.Insert<IdentifyStandard5Entity>(this, entity5, out count);
                                break;
                            case "6":
                                result = DataProxy.Current.Insert<IdentifyStandard6Entity>(this, entity6, out count);
                                break;
                        }

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
                                this.ShowJsAlertAndGoUrl(msg, "D1300008.aspx?IdentifyType=" + this.EditIdentifyType);
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
        /// 繫結 身分註記 下拉選單
        /// </summary>
        private void BindIdentifyIdOptions()
        {
            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            XmlResult result = null;

            List<ListItem> list = new List<ListItem>();
            switch (this.EditIdentifyType)
            {
                case "1":
                    where = getIdentify1Where(false);
                    orderbys.Add(IdentifyList1Entity.Field.IdentifyId, OrderByEnum.Asc);
                    IdentifyList1Entity[] datas1 = null;
                    result = DataProxy.Current.SelectAll<IdentifyList1Entity>(this, where, orderbys, out datas1);
                    if (result.IsSuccess)
                    {
                        foreach (IdentifyList1Entity data in datas1)
                        {
                            string text = String.Format("{0}({1})", data.IdentifyName, data.IdentifyId);
                            list.Add(new ListItem(text, data.IdentifyId));
                        }
                    }
                    break;
                case "2":
                    where = getIdentify2Where(false);
                    orderbys.Add(IdentifyList2Entity.Field.IdentifyId, OrderByEnum.Asc);
                    IdentifyList2Entity[] datas2 = null;
                    result = DataProxy.Current.SelectAll<IdentifyList2Entity>(this, where, orderbys, out datas2);
                    if (result.IsSuccess)
                    {
                        foreach (IdentifyList2Entity data in datas2)
                        {
                            string text = String.Format("{0}({1})", data.IdentifyName, data.IdentifyId);
                            list.Add(new ListItem(text, data.IdentifyId));
                        }
                    }
                    break;
                case "3":
                    where = getIdentify1Where(false);
                    orderbys.Add(IdentifyList3Entity.Field.IdentifyId, OrderByEnum.Asc);
                    IdentifyList3Entity[] datas3 = null;
                    result = DataProxy.Current.SelectAll<IdentifyList3Entity>(this, where, orderbys, out datas3);
                    if (result.IsSuccess)
                    {
                        foreach (IdentifyList3Entity data in datas3)
                        {
                            string text = String.Format("{0}({1})", data.IdentifyName, data.IdentifyId);
                            list.Add(new ListItem(text, data.IdentifyId));
                        }
                    }
                    break;
                case "4":
                    where = getIdentify4Where(false);
                    orderbys.Add(IdentifyList4Entity.Field.IdentifyId, OrderByEnum.Asc);
                    IdentifyList4Entity[] datas4 = null;
                    result = DataProxy.Current.SelectAll<IdentifyList4Entity>(this, where, orderbys, out datas4);
                    if (result.IsSuccess)
                    {
                        foreach (IdentifyList4Entity data in datas4)
                        {
                            string text = String.Format("{0}({1})", data.IdentifyName, data.IdentifyId);
                            list.Add(new ListItem(text, data.IdentifyId));
                        }
                    }
                    break;
                case "5":
                    where = getIdentify5Where(false);
                    orderbys.Add(IdentifyList5Entity.Field.IdentifyId, OrderByEnum.Asc);
                    IdentifyList5Entity[] datas5 = null;
                    result = DataProxy.Current.SelectAll<IdentifyList5Entity>(this, where, orderbys, out datas5);
                    if (result.IsSuccess)
                    {
                        foreach (IdentifyList5Entity data in datas5)
                        {
                            string text = String.Format("{0}({1})", data.IdentifyName, data.IdentifyId);
                            list.Add(new ListItem(text, data.IdentifyId));
                        }
                    }
                    break;
                case "6":
                    where = getIdentify6Where(false);
                    orderbys.Add(IdentifyList6Entity.Field.IdentifyId, OrderByEnum.Asc);
                    IdentifyList6Entity[] datas6 = null;
                    result = DataProxy.Current.SelectAll<IdentifyList6Entity>(this, where, orderbys, out datas6);
                    if (result.IsSuccess)
                    {
                        foreach (IdentifyList6Entity data in datas6)
                        {
                            string text = String.Format("{0}({1})", data.IdentifyName, data.IdentifyId);
                            list.Add(new ListItem(text, data.IdentifyId));
                        }
                    }
                    break;
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlIdentifyId.Items.Clear();
                this.ddlIdentifyId.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 取得 所屬收入科目 下拉選單
        /// </summary>
        private ListItem[] GetReceiveItems()
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
                return null;
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

            return list.ToArray();
        }

        protected void ddlMemo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMemo.SelectedValue == "2")
            {
                ShowReceiveItemOption(true);
                ddlReceiveItem.Enabled = true;
            }
            else
            {
                ShowReceiveItemOption(false);
            }
        }

        protected void ddlIdWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.litHtml.Text = String.Empty;

            switch (ddlIdWay.SelectedValue)
            {
                case "1":   //依百分比計算 show ddlMemo
                    ddlMemo.Visible = true;
                    ddlMemo.SelectedValue = "1";
                    ShowReceiveItemOption(false);
                    break;
                case "2":   //依金額計算
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                    break;
            }

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (this.EditIdentifyType)
            {
                case "1":
                    switch (ddlIdWay.SelectedValue)
                    {
                        case "1":   //依百分比計算
                            this.litHtml.Text = GetHtmlbyPercent1(ReceiveItems, this.EditIdentifyStandard1Entity);
                            break;
                        case "2":   //依金額計算
                            this.litHtml.Text = GetHtmlbyAmount1(ReceiveItems, this.EditIdentifyStandard1Entity);
                            break;
                    }
                    break;
                case "2":
                    switch (ddlIdWay.SelectedValue)
                    {
                        case "1":   //依百分比計算
                            this.litHtml.Text = GetHtmlbyPercent2(ReceiveItems, this.EditIdentifyStandard2Entity);
                            break;
                        case "2":   //依金額計算
                            this.litHtml.Text = GetHtmlbyAmount2(ReceiveItems, this.EditIdentifyStandard2Entity);
                            break;
                    }
                    break;
                case "3":
                    switch (ddlIdWay.SelectedValue)
                    {
                        case "1":   //依百分比計算
                            this.litHtml.Text = GetHtmlbyPercent3(ReceiveItems, this.EditIdentifyStandard3Entity);
                            break;
                        case "2":   //依金額計算
                            this.litHtml.Text = GetHtmlbyAmount3(ReceiveItems, this.EditIdentifyStandard3Entity);
                            break;
                    }
                    break;
                case "4":
                    switch (ddlIdWay.SelectedValue)
                    {
                        case "1":   //依百分比計算
                            this.litHtml.Text = GetHtmlbyPercent4(ReceiveItems, this.EditIdentifyStandard4Entity);
                            break;
                        case "2":   //依金額計算
                            this.litHtml.Text = GetHtmlbyAmount4(ReceiveItems, this.EditIdentifyStandard4Entity);
                            break;
                    }
                    break;
                case "5":
                    switch (ddlIdWay.SelectedValue)
                    {
                        case "1":   //依百分比計算
                            this.litHtml.Text = GetHtmlbyPercent5(ReceiveItems, this.EditIdentifyStandard5Entity);
                            break;
                        case "2":   //依金額計算
                            this.litHtml.Text = GetHtmlbyAmount5(ReceiveItems, this.EditIdentifyStandard5Entity);
                            break;
                    }
                    break;
                case "6":
                    switch (ddlIdWay.SelectedValue)
                    {
                        case "1":   //依百分比計算
                            this.litHtml.Text = GetHtmlbyPercent6(ReceiveItems, this.EditIdentifyStandard6Entity);
                            break;
                        case "2":   //依金額計算
                            this.litHtml.Text = GetHtmlbyAmount6(ReceiveItems, this.EditIdentifyStandard6Entity);
                            break;
                    }
                    break;
            }

            #endregion
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

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("D1300008.aspx?IdentifyType=" + this.EditIdentifyType);
        }

    }
}