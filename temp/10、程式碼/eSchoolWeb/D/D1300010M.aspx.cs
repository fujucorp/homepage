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
    public partial class D1300010M : BasePage
    {
        public const string cFieldName1 = "Re_Num";
        public const string cFieldName2 = "Re_Dno";

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
        private string EditReturnId
        {
            get
            {
                return ViewState["EditReturnId"] as string;
            }
            set
            {
                ViewState["EditReturnId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的減免類別標準
        /// </summary>
        private ReturnStandardEntity EditReturnStandardEntity
        {
            get
            {
                return ViewState["EditReturnStandardEntity"] as ReturnStandardEntity;
            }
            set
            {
                ViewState["EditReturnStandardEntity"] = value == null ? null : value;
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
                this.EditReturnId = QueryString.TryGetValue("ReturnId", String.Empty);

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                ReturnStandardEntity entity = null;
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = new Expression(ReturnStandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(ReturnStandardEntity.Field.YearId, this.EditYearId);
                            where.And(ReturnStandardEntity.Field.TermId, this.EditTermId);
                            where.And(ReturnStandardEntity.Field.DepId, this.EditDepId);
                            where.And(ReturnStandardEntity.Field.ReceiveId, this.EditReceiveId);
                            where.And(ReturnStandardEntity.Field.ReturnId, this.EditReturnId);
                            XmlResult result = DataProxy.Current.SelectFirst<ReturnStandardEntity>(this, where, null, out entity);
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
                            entity = new ReturnStandardEntity();
                            this.EditReturnId = ddlReturnId.SelectedValue;
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
            ddlReturnId.Enabled = false;
            BindReturnIdOptions();
        }

        /// <summary>
        /// 顯示單筆明細資料
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData(ReturnStandardEntity entity)
        {
            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditReturnStandardEntity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItems();

            if (ReceiveItems == null || ReceiveItems.Length == 0)
            {
                string msg = this.GetLocalized("請將收入科目代碼輸入後再進入此網頁");
                this.ShowSystemMessage(msg);
                this.ccbtnOK.Visible = false;
                return;
            }

            this.litHtml.Text = String.Empty;
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();

            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                ListItem listitem = ReceiveItems[idx];
                string fieldName1 = cFieldName1 + (idx + 1).ToString("00");   //退費分子
                string fieldName2 = cFieldName2 + (idx + 1).ToString("00");   //退費分母
                string fieldValue1 = string.Empty;
                string fieldValue2 = string.Empty;
                sb.AppendLine("<tr>");

                //收入科目
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

                sb.AppendLine("</tr>");
            }
            this.litHtml.Text = sb.ToString();

            #endregion

            if (this.Action == "A")
            {
                this.ddlReturnId.Enabled = true;
            }
            else if (this.Action == "M")
            {
                this.ddlReturnId.Enabled = false;
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫
        /// </summary>
        /// <returns></returns>
        private ReturnStandardEntity GetEditData()
        {
            ReturnStandardEntity entity = new ReturnStandardEntity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":   //修改
                    entity = EditReturnStandardEntity;
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                case "A":   //新增
                    entity.ReturnId = ddlReturnId.SelectedValue; ;
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
            }

            //處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItems();
            for (int idx = 0; idx < ReceiveItems.Length; idx++)
            {
                string fieldName1 = cFieldName1 + (idx + 1).ToString("00");   //減免分子
                string fieldName2 = cFieldName2 + (idx + 1).ToString("00");   //減免分母
                string fieldValue1 = "0";
                string fieldValue2 = "0";

                fieldValue1 = GetValue(fieldName1);
                fieldValue2 = GetValue(fieldName2);
                entity.SetValue(fieldName1, fieldValue1);
                entity.SetValue(fieldName2, fieldValue2);
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
            ReturnStandardEntity entity = this.GetEditData();

            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        int count = 0;
                        XmlResult result = DataProxy.Current.Update<ReturnStandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300010.aspx");
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

                        XmlResult result = DataProxy.Current.Insert<ReturnStandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300010.aspx");
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
        private void BindReturnIdOptions()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(ReturnListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(ReturnListEntity.Field.YearId, this.EditYearId);
            where.And(ReturnListEntity.Field.TermId, this.EditTermId);
            where.And(ReturnListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ReturnListEntity.Field.ReturnId, OrderByEnum.Asc);

            ReturnListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<ReturnListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (ReturnListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.ReturnName, data.ReturnId);
                    list.Add(new ListItem(text, data.ReturnId));
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlReturnId.Items.Clear();
                this.ddlReturnId.Items.AddRange(items);
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
    }
}