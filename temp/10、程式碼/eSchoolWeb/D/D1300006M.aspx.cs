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
    public partial class D1300006M : BasePage
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

        /// <summary>
        /// 編輯的小於基準其他收費標準檔
        /// </summary>
        private Credit2StandardEntity EditCredit2StandardEntity
        {
            get
            {
                return ViewState["EditCredit2StandardEntity"] as Credit2StandardEntity;
            }
            set
            {
                ViewState["EditCredit2StandardEntity"] = value == null ? null : value;
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

                if (this.Action != "A" && this.Action != "M")
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                Credit2StandardEntity entity = null;
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            Expression where = new Expression(Credit2StandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(Credit2StandardEntity.Field.YearId, this.EditYearId);
                            where.And(Credit2StandardEntity.Field.TermId, this.EditTermId);
                            where.And(Credit2StandardEntity.Field.DepId, this.EditDepId);
                            where.And(Credit2StandardEntity.Field.ReceiveId, this.EditReceiveId);
                            XmlResult result = DataProxy.Current.SelectFirst<Credit2StandardEntity>(this, where, null, out entity);
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
                            entity = new Credit2StandardEntity();
                            EditReceiveId = ddlReceiveId.SelectedValue;
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
            ddlReceiveId.Enabled = false;

            BindReceiveIdOptions();

        }

        /// <summary>
        /// 顯示單筆明細資料
        /// </summary>
        /// <param name="entity"></param>
        private void BindEditData(Credit2StandardEntity entity)
        {
            if (entity == null)
            {
                this.ccbtnOK.Visible = false;
                return;
            }

            EditCredit2StandardEntity = entity;

            #region 組 Html
            ListItem[] ReceiveItems = GetReceiveItemOptions();
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
                string fieldName = "Credit2_" + (idx + 1).ToString("00");
                string chkName = "chkItem" + (idx + 1).ToString();
                bool chkChecked = false;

                sb.AppendLine("<tr>");
                sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", listitem.Text).AppendLine();
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

            #region [MDY:20210713] FIX BUG
            WebHelper.SetDropDownListSelectedValue(this.ddlReceiveId, entity.ReceiveId);
            #endregion

            if (this.Action == "A")
            {
                this.ddlReceiveId.Enabled = true;
            }
            else if (this.Action == "M")
            {
                this.ddlReceiveId.Enabled = false;
            }
            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫
        /// </summary>
        /// <returns></returns>
        private Credit2StandardEntity GetEditData()
        {
            Credit2StandardEntity entity = new Credit2StandardEntity();
            entity.ReceiveType = this.EditReceiveType;
            entity.YearId = this.EditYearId;
            entity.TermId = this.EditTermId;
            entity.DepId = this.EditDepId;
            entity.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case "M":   //修改
                    entity = EditCredit2StandardEntity;
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                case "A":   //新增
                    entity.ReceiveId = ddlReceiveId.SelectedValue;;
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
            }

            //處理收入科目
            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Result rt = new Result();

            ListItem[] ReceiveItems = GetReceiveItemOptions();
            for (int i = 0; i < ReceiveItems.Length; i++)
            {
                string fieldName = "Credit2_" + (i + 1).ToString("00");
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
            Credit2StandardEntity entity = this.GetEditData();

            switch (this.Action)
            {
                case "M":   //修改
                    #region 修改
                    {
                        int count = 0;
                        //XmlResult result = DataProxy.Current.UpdateFields<Credit2StandardEntity>(this, where, fieldValues.ToArray(), out count);
                        XmlResult result = DataProxy.Current.Update<Credit2StandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300006.aspx");
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

                        XmlResult result = DataProxy.Current.Insert<Credit2StandardEntity>(this, entity, out count);
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
                                this.ShowJsAlertAndGoUrl(msg, "D1300006.aspx");
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
        private void BindReceiveIdOptions()
        {
            List<ListItem> list = new List<ListItem>();

            Expression where = new Expression();
            where.And(ReceiveListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(ReceiveListEntity.Field.YearId, this.EditYearId);
            where.And(ReceiveListEntity.Field.TermId, this.EditTermId);
            where.And(ReceiveListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ReceiveListEntity.Field.ReceiveId, OrderByEnum.Asc);

            ReceiveListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<ReceiveListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (ReceiveListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.ReceiveName, data.ReceiveId);
                    list.Add(new ListItem(text, data.ReceiveId));
                }
            }

            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlReceiveId.Items.Clear();
                this.ddlReceiveId.Items.AddRange(items);
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
                        ;
                        list.Add(new ListItem(ob.ToString(), i.ToString()));
                    }
                }
            }

            return list.ToArray();
        }

        protected void ddlReceiveId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EditReceiveId = ddlReceiveId.SelectedValue;

            #region [MDY:20210713] FIX BUG
            this.BindEditData(new Credit2StandardEntity() { ReceiveId = EditReceiveId });
            #endregion
        }
    }
}