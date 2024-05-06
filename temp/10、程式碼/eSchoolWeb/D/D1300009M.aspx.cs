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
    public partial class D1300009M : BasePage
    {
        public const string pFieldName1 = "Num_";
        public const string pFieldName2 = "Dno_";
        public const string pFieldName3 = "limit_";
        public const string aFieldName1 = "Reduce_Amount";
        public const string rFieldName1 = "Reduce_order";

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
        /// 編輯的減免類別代碼參數
        /// </summary>
        private string EditReduceId
        {
            get
            {
                return ViewState["EditReduceId"] as string;
            }
            set
            {
                ViewState["EditReduceId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的減免類別標準
        /// </summary>
        private ReduceStandardEntity EditReduceStandardEntity
        {
            get
            {
                return ViewState["EditReduceStandardEntity"] as ReduceStandardEntity;
            }
            set
            {
                ViewState["EditReduceStandardEntity"] = value == null ? null : value;
            }
        }
        #endregion

        ListItem[] gReceiveItems = null;

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
                this.EditReduceId = QueryString.TryGetValue("ReduceId", String.Empty);

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                ReduceStandardEntity entity = null;
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = new Expression(ReduceStandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(ReduceStandardEntity.Field.YearId, this.EditYearId);
                            where.And(ReduceStandardEntity.Field.TermId, this.EditTermId);
                            where.And(ReduceStandardEntity.Field.DepId, this.EditDepId);
                            where.And(ReduceStandardEntity.Field.ReceiveId, this.EditReceiveId);
                            where.And(ReduceStandardEntity.Field.ReduceId, this.EditReduceId);
                            XmlResult result = DataProxy.Current.SelectFirst<ReduceStandardEntity>(this, where, null, out entity);
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
                            entity = new ReduceStandardEntity();
                            entity.ReduceWay = "1";
                            this.EditReduceId = ddlReduceId.SelectedValue;
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
            gReceiveItems = GetReceiveItemOptions();

            ddlReduceId.Enabled = false;
            BindReduceIdOptions();
            BindReceiveItemOptions();
            ddlReceiveItem.Enabled = false;
        }

        /// <summary>
        /// 顯示單筆明細資料
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData(ReduceStandardEntity entity)
        {
            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            this.EditReduceStandardEntity = entity;
            this.litHtml.Text = String.Empty;

            #region 組 Html
            //ListItem[] ReceiveItems = this.EditReceiveItems;
            ListItem[] ReceiveItems = GetReceiveItemOptions();

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (entity.ReduceWay)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent(ReceiveItems, this.EditReduceStandardEntity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount(ReceiveItems, this.EditReduceStandardEntity);
                    break;
                case "3":   //金額依次減免
                    this.litHtml.Text = GetHtmlbyReduce(ReceiveItems, this.EditReduceStandardEntity);
                    break;
            }
            #endregion

            if (this.Action == "A")
            {
                this.ddlReduceId.Enabled = true;
                ddlMemo.SelectedValue = "1";
                ShowReceiveItemOption(false);
            }
            else if (this.Action == "M")
            {
                this.ddlReduceId.Enabled = false;
                ddlReduceWay.SelectedValue = entity.ReduceWay;
                if (entity.ReduceWay == "3")  //依金額依次減免，不顯示 ddlMemo
                {
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                }
                else
                {
                    ddlReceiveItem.SelectedValue = entity.ReduceItem.Trim();
                    if (entity.ReduceItem.Trim().Length > 0)
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
        private string GetHtmlbyPercent(ListItem[] ReceiveItems, ReduceStandardEntity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">減免分子</div></th>");
            sb.AppendLine("<th><div align=\"center\">減免分母</div></th>");
            sb.AppendLine("<th><div align=\"center\">減免上限</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //減免分子
                string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //減免分母
                string fieldName3 = pFieldName3 + (idx + 1).ToString("00");   //減免上限金額
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                string fieldValue3 = string.Empty;

                sb.AppendLine("<tr>");
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 減免分子
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

                #region 減免分母
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

                #region 減免上限金額
                object ob3 = entity.GetValue(fieldName3, out rt);
                if (ob3 != null)
                {
                    if (!String.IsNullOrEmpty(ob3.ToString()))
                    {
                        fieldValue3 = ob3.ToString();
                    }
                }
                sb.AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1${0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_{0}\" /></td>", fieldName3, fieldValue3).AppendLine();
                #endregion


                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyAmount(ListItem[] ReceiveItems, ReduceStandardEntity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">減免項目金額</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //減免項目金額
                string fieldValue1 = string.Empty;

                sb.AppendLine("<tr>");
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 減免項目金額
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

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 依金額依次減免計算 動態產生 Html
        /// </summary>
        /// <returns></returns>
        private string GetHtmlbyReduce(ListItem[] ReceiveItems, ReduceStandardEntity entity)
        {
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<tr><th><div align=\"center\">收入科目</div></th>");
            sb.AppendLine("<th><div align=\"center\">減免次序</div></th></tr>");

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = rFieldName1 + (idx + 1).ToString();   //減免次序
                string fieldValue1 = string.Empty;

                sb.AppendLine("<tr>");
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();

                #region 減免次序
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

                sb.AppendLine("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫
        /// </summary>
        /// <returns></returns>
        private ReduceStandardEntity GetEditData()
        {
            ReduceStandardEntity entity = new ReduceStandardEntity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":   //修改
                    entity = EditReduceStandardEntity;
                    entity.ReduceWay = ddlReduceWay.SelectedValue;
                    //依百分比計算 及 依百分比計算 ddlMemo 顯示
                    if (entity.ReduceWay == "1" || entity.ReduceWay == "2")
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.ReduceItem = string.Empty;
                        }
                        else
                        {
                            entity.ReduceItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.ReduceItem = string.Empty;
                    }
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                case "A":   //新增
                    entity.ReduceWay = ddlReduceWay.SelectedValue;
                    //依百分比計算 及 依百分比計算 ddlMemo 顯示
                    if (entity.ReduceWay == "1" || entity.ReduceWay == "2")
                    {
                        if (ddlMemo.SelectedValue == "1")
                        {
                            entity.ReduceItem = string.Empty;
                        }
                        else
                        {
                            entity.ReduceItem = ddlReceiveItem.SelectedValue;
                        }
                    }
                    else
                    {
                        entity.ReduceItem = string.Empty;
                    }
                    entity.ReduceId = ddlReduceId.SelectedValue; ;
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
            }

            //處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();


            for (int idx = 0; idx < ddlReceiveItem.Items.Count; idx++)
            {
                switch (ddlReduceWay.SelectedValue)
                {
                    case "1":   //依百分比計算                        
                        string fieldName1 = pFieldName1 + (idx + 1).ToString("00");   //減免分子
                        string fieldName2 = pFieldName2 + (idx + 1).ToString("00");   //減免分母
                        string fieldName3 = pFieldName3 + (idx + 1).ToString("00");   //減免上限金額
                        string fieldValue1 = string.Empty;
                        string fieldValue2 = string.Empty;
                        string fieldValue3 = string.Empty;

                        fieldValue1 = GetValue(fieldName1);
                        fieldValue2 = GetValue(fieldName2);
                        fieldValue3 = GetValue(fieldName3);
                        entity.SetValue(fieldName1, fieldValue1);
                        entity.SetValue(fieldName2, fieldValue2);
                        entity.SetValue(fieldName3, fieldValue3);
                        break;
                    case "2":   //依金額計算                    
                        fieldName1 = aFieldName1 + (idx + 1).ToString("00");   //減免金額
                        fieldValue1 = string.Empty;

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                    case "3":   //金額依次減免計算 
                        fieldName1 = rFieldName1 + (idx + 1).ToString();   //減免次序
                        fieldValue1 = string.Empty;

                        fieldValue1 = GetValue(fieldName1);
                        entity.SetValue(fieldName1, fieldValue1);
                        break;
                }
            }

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
        /// 儲存就貸收費資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            ReduceStandardEntity entity = this.GetEditData();

            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        int count = 0;
                        XmlResult result = DataProxy.Current.Update<ReduceStandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300009.aspx");
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

                        XmlResult result = DataProxy.Current.Insert<ReduceStandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300009.aspx");
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
        /// 繫結 代收費用別 下拉選單
        /// </summary>
        private void BindReduceIdOptions()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(ReduceListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(ReduceListEntity.Field.YearId, this.EditYearId);
            where.And(ReduceListEntity.Field.TermId, this.EditTermId);
            where.And(ReduceListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ReduceListEntity.Field.ReduceId, OrderByEnum.Asc);

            ReduceListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<ReduceListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (ReduceListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.ReduceName, data.ReduceId);
                    list.Add(new ListItem(text, data.ReduceId));
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlReduceId.Items.Clear();
                this.ddlReduceId.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 所屬收入科目 下拉選單
        /// </summary>
        private void BindReceiveItemOptions()
        {
            if (gReceiveItems != null)
            {
                ListItem[] items = gReceiveItems;
                if (items != null && items.Length > 0)
                {
                    this.ddlReceiveItem.Items.Clear();
                    this.ddlReceiveItem.Items.Add(new ListItem("", ""));
                    this.ddlReceiveItem.Items.AddRange(items);
                }
            }
        }

        /// <summary>
        /// 取得 所屬收入科目 下拉選單
        /// </summary>
        private ListItem[] GetReceiveItemOptions()
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
                        list.Add(new ListItem(ob.ToString(), i.ToString()));
                    }
                }
            }

            return list.ToArray();
        }

        protected void ddlReduceWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.litHtml.Text = String.Empty;

            switch (ddlReduceWay.SelectedValue)
            {
                case "1":   //依百分比計算 show ddlMemo
                case "2":   //依金額計算  show ddlMemo
                    ddlMemo.Visible = true;
                    ddlMemo.SelectedValue = "1";
                    ShowReceiveItemOption(false);
                    break;
                case "3":   //依金額計算
                    ddlMemo.Visible = false;
                    ShowReceiveItemOption(false);
                    break;
            }

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItemOptions();

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            switch (ddlReduceWay.SelectedValue)
            {
                case "1":   //依百分比計算
                    this.litHtml.Text = GetHtmlbyPercent(ReceiveItems, this.EditReduceStandardEntity);
                    break;
                case "2":   //依金額計算
                    this.litHtml.Text = GetHtmlbyAmount(ReceiveItems, this.EditReduceStandardEntity);
                    break;
                case "3":   //金額依次減免
                    this.litHtml.Text = GetHtmlbyReduce(ReceiveItems, this.EditReduceStandardEntity);
                    break;
            }
            #endregion

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
    }
}