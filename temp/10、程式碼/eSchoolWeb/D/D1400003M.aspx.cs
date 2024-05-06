using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;
using Fuju.DB.Data;

using Entities;
using Helpers;

namespace eSchoolWeb.D
{
    public partial class D1400003M1 : BasePage
    {
        #region Property
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
        /// 檔案類型
        /// </summary>
        private string EditFileType
        {
            get
            {
                return ViewState["EditFileType"] as string;
            }
            set
            {
                ViewState["EditFileType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存商家代號代碼的查詢條件
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
        /// 儲存對照表代碼的查詢條件
        /// </summary>
        private string EditMappingId
        {
            get
            {
                return ViewState["EditMappingId"] as string;
            }
            set
            {
                ViewState["EditMappingId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存對照表名稱
        /// </summary>
        private string EditMappingName
        {
            get
            {
                return ViewState["EditMappingName"] as string;
            }
            set
            {
                ViewState["EditMappingName"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 儲存對照表Xlsmdb
        /// </summary>
        private MappingcsXlsmdbEntity EditMappingcsXlsmdbEntity
        {
            get
            {
                return ViewState["EditMappingcsXlsmdbEntity"] as MappingcsXlsmdbEntity;
            }
            set
            {
                ViewState["EditMappingcsXlsmdbEntity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 儲存對照表Txt
        /// </summary>
        private MappingcsTxtEntity EditMappingcsTxtEntity
        {
            get
            {
                return ViewState["EditMappingcsTxtEntity"] as MappingcsTxtEntity;
            }
            set
            {
                ViewState["EditMappingcsTxtEntity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 儲存編輯的 MappingItem 資料
        /// </summary>
        private List<MappingItem> EditMappingItems
        {
            get
            {
                return ViewState["EditMappingItems"] as List<MappingItem>;
            }
            set
            {
                ViewState["EditMappingItems"] = value;
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
                    this.lbtnNext.Visible = false;
                    return;
                }
                this.Action = QueryString.TryGetValue("Action", String.Empty);
                this.EditMappingId = QueryString.TryGetValue("MappingId", String.Empty);
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditFileType = QueryString.TryGetValue("FileType", String.Empty);
                this.EditMappingName = QueryString.TryGetValue("MappingName", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || (this.Action != "A" && this.Action != "M")
                    || (this.Action == "M" && String.IsNullOrEmpty(this.EditMappingId)))
                {
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.lbtnNext.Visible = false;
                    return;
                }
                #endregion

                this.InitialUI();

                MappingcsXlsmdbEntity XlsData = null;
                MappingcsTxtEntity TxtData = null;

                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            XmlResult result = new XmlResult();
                            if (this.EditFileType == "xls")
                            {
                                Expression where = new Expression(MappingcsXlsmdbEntity.Field.ReceiveType, this.EditReceiveType);
                                where.And(MappingcsXlsmdbEntity.Field.MappingId, this.EditMappingId);
                                result = DataProxy.Current.SelectFirst<MappingcsXlsmdbEntity>(this, where, null, out XlsData);

                            }
                            else
                            {
                                Expression where = new Expression(MappingcsTxtEntity.Field.ReceiveType, this.EditReceiveType);
                                where.And(MappingcsTxtEntity.Field.MappingId, this.EditMappingId);
                                result = DataProxy.Current.SelectFirst<MappingcsTxtEntity>(this, where, null, out TxtData);
                            }
                            if (result.IsSuccess)
                            {
                                if (XlsData == null && TxtData == null)
                                {
                                    this.ShowSystemMessage(this.GetLocalized("查無指定要修改的資料"));
                                }
                            }
                            else
                            {
                                XlsData = null;
                                TxtData = null;
                                this.ShowSystemMessage(this.GetLocalized("查詢指定要修改的資料失敗") + "，" + result.Message);
                            }

                        }
                        #endregion
                        break;
                    case "A":   //新增
                        #region 產生空的新增資料
                        {
                        }
                        #endregion
                        break;
                }

                switch (ddlFileType.SelectedValue)
                {
                    case "xls":
                        this.BindXlsData(XlsData);
                        break;
                    case "txt":
                        this.BindTxtData(TxtData);
                        break;
                }
            }
        }

        /// <summary>
        /// 結繫主要條件選項資料 Xls
        /// </summary>
        /// <param name="option">主要條件選項資料</param>
        private void BindXlsData(MappingcsXlsmdbEntity entity)
        {
            if (entity == null && Action == "M")
            {
                this.lbtnNext.Visible = false;
                return;
            }
            else
            {
                this.lbtnNext.Visible = true;
                this.EditMappingcsXlsmdbEntity = entity;
            }

            switch (this.Action)
            {
                case "A":
                    this.tbxMappingName.Enabled = true;
                    this.ddlFileType.Enabled = true;
                    this.ddlFileType.SelectedValue = "xls";  //default
                    break;
                case "M":
                    this.tbxMappingName.Enabled = true;
                    this.ddlFileType.Enabled = false;

                    #region [MDY:20210401] 原碼修正
                    #region [OLD]
                    //this.ddlFileType.SelectedValue = this.EditFileType;
                    #endregion

                    WebHelper.SetDropDownListSelectedValue(this.ddlFileType, this.EditFileType);
                    #endregion

                    this.tbxMappingName.Text = entity.MappingName;

                    ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                    Result rt = new Result();
                    FieldSpec[] fields = entity.FieldsSpec;
                    foreach (FieldSpec field in fields)
                    {
                        object ob = entity.GetValue(field.FieldName, out rt);
                        if (ob != null)
                        {
                            if (!String.IsNullOrEmpty(ob.ToString()))
                            {
                                CheckBox ck = (CheckBox)cph.FindControl("chk" + field.FieldName);
                                if (ck != null)
                                {
                                    ck.Checked = true;
                                }
                            }
                        }
                    }
                    break;
            }

            this.lbtnNext.Visible = true;
        }

        /// <summary>
        /// 結繫主要條件選項資料 Txt
        /// </summary>
        /// <param name="option">主要條件選項資料</param>
        private void BindTxtData(MappingcsTxtEntity entity)
        {
            if (entity == null && Action == "M")
            {
                this.lbtnNext.Visible = false;
                return;
            }
            else
            {
                this.lbtnNext.Visible = true;
                this.EditMappingcsTxtEntity = entity;
            }

            switch (this.Action)
            {
                case "A":
                    this.tbxMappingName.Enabled = true;
                    this.ddlFileType.Enabled = true;
                    this.ddlFileType.SelectedValue = "txt";
                    break;
                case "M":
                    this.tbxMappingName.Enabled = false;
                    this.ddlFileType.Enabled = false;

                    #region [MDY:20210401] 原碼修正
                    #region [OLD]
                    //this.ddlFileType.SelectedValue = this.EditFileType;
                    #endregion

                    WebHelper.SetDropDownListSelectedValue(this.ddlFileType, this.EditFileType);
                    #endregion

                    this.tbxMappingName.Text = entity.MappingName;

                    ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                    Result rt = new Result();
                    FieldSpec[] fields = entity.FieldsSpec;
                    foreach (FieldSpec field in fields)
                    {
                        object ob = entity.GetValue(field.FieldName, out rt);
                        if (ob != null)
                        {
                            if (!String.IsNullOrEmpty(ob.ToString()))
                            {
                                string strFieldName = field.FieldName;
                                if (strFieldName.LastIndexOf('_') > 0)
                                {
                                    strFieldName = strFieldName.Substring(0, strFieldName.LastIndexOf('_'));
                                    CheckBox ck = (CheckBox)cph.FindControl("chk" + strFieldName);
                                    if (ck != null)
                                    {
                                        ck.Checked = true;
                                    }
                                }
                            }
                        }
                    }

                    break;
            }

            this.lbtnNext.Visible = true;
        }

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private bool InitialUI()
        {
            string receiveType = null;
            string yearID = null;
            string termID = null;
            string depID = null;
            string ReceiveID = null;
            if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out ReceiveID)
                || String.IsNullOrEmpty(receiveType))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("無法取得商家代號");
                this.ShowJsAlert(msg);
                return false;
            }

            this.EditReceiveType = receiveType;
            this.ucFilter1.GetDataAndBind(this.EditReceiveType, "", "");

            #region [MDY:20210401] 原碼修正
            #region [OLD]
            //this.ddlFileType.SelectedValue = this.EditFileType;
            #endregion

            WebHelper.SetDropDownListSelectedValue(this.ddlFileType, this.EditFileType);
            #endregion

            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            foreach (Control control in cph.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox ck = (CheckBox)control;
                    ck.Checked = false;
                }
            }
            tbxMappingName.Text = "";

            #region 檢查商家代號授權
            if (!this.GetLogonUser().IsAuthReceiveTypes(receiveType))
            {
                string msg = this.GetLocalized("該商家代號未授權");
                this.ShowJsAlert(msg);
                return false;
            }
            #endregion

            divStep2A.Visible = false;
            divStep2B.Visible = false;
            return true;
        }

        /// <summary>
        /// 取得勾選的項目
        /// </summary>
        /// <returns></returns>
        protected KeyValueList<string> GetCheckedItems()
        {
            KeyValueList<string> lists = new KeyValueList<string>();

            ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
            Control cDiv = cph.FindControl("divStep1");
            if (cDiv != null)
            {
                foreach (Control control in cDiv.Controls)
                {
                    if (control is CheckBox)
                    {
                        CheckBox ck = (CheckBox)control;
                        if (ck.Checked)
                        {
                            string strID = string.Empty;
                            string strName = string.Empty;
                            strID = ck.ID.Replace("chk", "");
                            strName = ck.Text.Trim();
                            lists.Add(strID, strName);
                        }
                    }
                }
            }

            return lists;
        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            string errmsg = this.CheckDataForInputUI();
            if (!String.IsNullOrEmpty(errmsg))
            {
                this.ShowSystemMessage(errmsg);
                return;
            }

            string fileType = ddlFileType.SelectedValue;

            string strMappingName = string.Empty;
            this.EditMappingName = tbxMappingName.Text.Trim();

            if (ddlFileType.SelectedValue == "xls")  //xls
            {
                divStep1.Visible = false;
                divStep2A.Visible = false;
                divStep2B.Visible = true;
                //Response.Redirect("D1400002M2B.aspx");

                //組 Html
                initStep2B(this.EditMappingcsXlsmdbEntity);
            }
            else
            {
                divStep1.Visible = false;
                divStep2A.Visible = true;
                divStep2B.Visible = false;
                //Response.Redirect("D1400002M2A.aspx");

                //組 Html
                initStep2A(this.EditMappingcsTxtEntity);
            }
        }

        protected void initStep2B(MappingcsXlsmdbEntity entity)
        {
            List<MappingItem> items = this.EditMappingItems;

            labMappingNameB.Text = HttpUtility.HtmlEncode(this.EditMappingName);

            #region 組 Html

            KeyValueList<string> itemNames = GetCheckedItems();
            this.litHtmlB.Text = String.Empty;
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();
            if (itemNames != null && itemNames.Count > 0)
            {
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    string sID = itemNames[idx].Key;
                    string sName = itemNames[idx].Value;
                    string sValue = string.Empty;
                    if (entity != null)            //Action="M"
                    {
                        object ob = entity.GetValue(sID, out rt);
                        if (ob != null)
                        {
                            sValue = ob.ToString();
                        }
                    }

                    if (items != null)      //取得編輯中的值
                    {
                        foreach (MappingItem item in items)
                        {
                            if (sID == item.Key)
                            {
                                sValue = item.XlsName;
                                break;
                            }
                        }
                    }

                    sb
                        .AppendLine("<tr>")
                        .AppendFormat("<td>{0}</td>", sName).AppendLine()
                        .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" /></td>", sID, sValue).AppendLine()
                        .AppendLine("</tr>");
                }
                this.litHtmlB.Text = sb.ToString();
            }
            #endregion

        }

        protected void initStep2A(MappingcsTxtEntity entity)
        {
            List<MappingItem> items = this.EditMappingItems;
            
            labMappingNameA.Text = HttpUtility.HtmlEncode(this.EditMappingName);

            #region 組 Html

            KeyValueList<string> itemNames = this.GetCheckedItems();
            this.litHtmlA.Text = String.Empty;
            Result rt = new Result();
            StringBuilder sb = new StringBuilder();
            if (itemNames != null && itemNames.Count > 0)
            {
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    string sID = itemNames[idx].Key;
                    string fieldName_Start = sID + "_S";
                    string fieldName_Length = sID + "_L";
                    string sName = itemNames[idx].Value;
                    string fieldValue_Start = string.Empty;
                    string fieldValue_Length = string.Empty;
                    if (entity != null)            //Action="M"
                    {
                        object obS = entity.GetValue(fieldName_Start, out rt);
                        if (obS != null)
                        {
                            fieldValue_Start = obS.ToString();
                        }
                        object obL = entity.GetValue(fieldName_Length, out rt);
                        if (obL != null)
                        {
                            fieldValue_Length = obL.ToString();
                        }
                    }

                    if (items != null)      //取得編輯中的值
                    {
                        foreach (MappingItem item in items)
                        {
                            if (sID == item.Key)
                            {
                                fieldValue_Start = item.GetTxtStart() == null ? "0" : ((int)item.GetTxtStart()).ToString();
                                fieldValue_Length = item.GetTxtLength() == null ? "0" : ((int)item.GetTxtLength()).ToString();
                                break;
                            }
                        }
                    }

                    sb
                        .AppendLine("<tr>")
                        .AppendFormat("<td>{0}</td>", sName).AppendLine()
                        .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" /></td>", fieldName_Start, fieldValue_Start).AppendLine()
                        .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" /></td>", fieldName_Length, fieldValue_Length).AppendLine()
                        .AppendLine("</tr>");
                }
                this.litHtmlA.Text = sb.ToString();
            }
            #endregion

        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            //記住這頁的輸入值
            this.EditMappingItems = SaveStep2Data();

            divStep1.Visible = true;
            divStep2A.Visible = false;
            divStep2B.Visible = false;
        }

        /// <summary>
        /// 儲存 Step2 的輸入資料
        /// </summary>
        /// <returns></returns>
        private List<MappingItem> SaveStep2Data()
        {
            List<MappingItem> items = new List<MappingItem>();

            KeyValueList<string> itemNames = GetCheckedItems();
            if (itemNames != null && itemNames.Count > 0)
            {
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    switch (ddlFileType.SelectedValue)
                    {
                        case "xls":
                            #region 儲存 Xls 的編輯中的資料
                            {
                                string sID = itemNames[idx].Key;
                                string sName = itemNames[idx].Value;
                                string sValue = string.Empty;

                                string txtValue = GetValue("tbx" + sID);
                                if (txtValue != null)
                                {
                                    if (String.IsNullOrEmpty(txtValue))
                                    {
                                        sValue = string.Empty;
                                    }
                                    sValue = txtValue;
                                }

                                MappingItem item = MappingItem.CreateByXls(sID, sName, sName, sValue);
                                items.Add(item);
                            }
                            #endregion
                            break;
                        case "txt":
                            #region 儲存 Txt 的編輯中的資料
                            {
                                string tmpKey = itemNames[idx].Key;
                                string sFieldName_Start = itemNames[idx].Key + "_S";
                                string sFieldName_Length = itemNames[idx].Key + "_L";
                                itemNames[idx].Key = sFieldName_Start + ", " + sFieldName_Length;

                                string tbxFieldName_Start = "tbx" + sFieldName_Start;
                                string tbxFieldName_Length = "tbx" + sFieldName_Length;
                                string tbxValue_Start = GetValue(tbxFieldName_Start);
                                string tbxValue_Length = GetValue(tbxFieldName_Length);
                                if (tbxValue_Start != null && tbxValue_Length != null)
                                {
                                    MappingItem item = MappingItem.CreateByTxt(tmpKey, tmpKey, tmpKey, tbxValue_Start, tbxValue_Length);
                                    items.Add(item);
                                }
                                else
                                {
                                    MappingItem item = MappingItem.CreateByTxt(tmpKey, tmpKey, tmpKey, "0", "0");
                                    items.Add(item);
                                }
                            }
                            #endregion
                            break;
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// 檢查資料，以作為使用介面輸入是否正確 (前台用)
        /// </summary>
        /// <returns>傳回錯誤訊息</returns>
        public string CheckDataForInputUI()
        {
            if (String.IsNullOrEmpty(tbxMappingName.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("對照表名稱");
            }

            return String.Empty;
        }

        protected void lbtnOK2A_Click(object sender, EventArgs e)
        {
            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1400003.aspx";

            KeyValueList<string> itemNames = this.GetCheckedItems();

            if (itemNames != null && itemNames.Count > 0)
            {
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    string sFieldName_Start = itemNames[idx].Key + "_S";
                    string sFieldName_Length = itemNames[idx].Key + "_L";

                    itemNames[idx].Key = sFieldName_Start + ", " + sFieldName_Length;

                    string tbxFieldName_Start = "tbx" + sFieldName_Start;
                    string tbxFieldName_Length = "tbx" + sFieldName_Length;
                    string tbxValue_Start = GetValue(tbxFieldName_Start);
                    string tbxValue_Length = GetValue(tbxFieldName_Length);
                    if (tbxValue_Start != null && tbxValue_Length != null)
                    {
                        if (String.IsNullOrEmpty(tbxValue_Start))
                        {
                            this.ShowMustInputAlert(itemNames[idx].Value + "起始位置");
                            return;
                        }
                        if (String.IsNullOrEmpty(tbxValue_Length))
                        {
                            this.ShowMustInputAlert(itemNames[idx].Value + "長度");
                            return;
                        }
                        int start = 0;
                        if (!int.TryParse(tbxValue_Start, out start))
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(itemNames[idx].Value + "起始位置限輸入大於0的整數");
                            this.ShowJsAlert(msg);
                            return;
                        }
                        int length = 0;
                        if (!int.TryParse(tbxValue_Length, out length))
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized(itemNames[idx].Value + "長度限輸入大於0的整數");
                            this.ShowJsAlert(msg);
                            return;
                        }
                        itemNames[idx].Value = tbxValue_Start + ", " + tbxValue_Length;
                    }
                    else
                    {
                        itemNames[idx].Value = "0, 0";
                    }
                }
            }

            string receiveType = this.EditReceiveType;
            string mappingId = string.Empty;

            //如果是新增要取一個新的 mappingID
            if (Action == "A")
            {
                mappingId = GetNewMappingID(MappingcsTxtEntity.TABLE_NAME);
            }
            else
            {
                mappingId = this.EditMappingId;
            }

            itemNames.Add("tableName", MappingcsTxtEntity.TABLE_NAME);
            itemNames.Add("receiveType", receiveType);
            itemNames.Add("mappingId", mappingId);
            itemNames.Add("mappingName", labMappingNameA.Text);

            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyMappingTxtData, itemNames, out returnData);
            if (xmlResult.IsSuccess)
            {
                this.ShowActionSuccessAlert(action, backUrl);
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }

        protected void lbtnOK2B_Click(object sender, EventArgs e)
        {
            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1400003.aspx";

            KeyValueList<string> itemNames = this.GetCheckedItems();

            if (itemNames != null && itemNames.Count > 0)
            {
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    string txtValue = GetValue("tbx" + itemNames[idx].Key);
                    if (txtValue != null)
                    {
                        if (String.IsNullOrEmpty(txtValue))
                        {
                            this.ShowMustInputAlert(itemNames[idx].Value);
                            return;
                        }
                        itemNames[idx].Value = txtValue;
                    }
                    else
                    {
                        itemNames[idx].Value = string.Empty;
                    }
                }
            }

            string receiveType = this.EditReceiveType;
            string mappingId = string.Empty;

            //如果是新增要取一個新的 mappingID
            if (Action == "A")
            {
                mappingId = GetNewMappingID(MappingcsXlsmdbEntity.TABLE_NAME);
            }
            else
            {
                mappingId = this.EditMappingId;
            }

            itemNames.Add("tableName", MappingcsXlsmdbEntity.TABLE_NAME);
            itemNames.Add("receiveType", receiveType);
            itemNames.Add("mappingId", mappingId);
            itemNames.Add("mappingName", labMappingNameB.Text);

            object returnData = null;
            XmlResult xmlResult = DataProxy.Current.CallMethod(this.Page, CallMethodName.CopyMappingXlsData, itemNames, out returnData);
            if (xmlResult.IsSuccess)
            {
                this.ShowActionSuccessAlert(action, backUrl);
            }
            else
            {
                this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
            }
        }

        protected string GetNewMappingID(string tableName)
        {
            if (tableName == MappingcsTxtEntity.TABLE_NAME)
            {
                MappingcsTxtEntity data = null;
                Expression where = new Expression(MappingcsTxtEntity.Field.ReceiveType, this.EditReceiveType);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(MappingcsTxtEntity.Field.MappingId, OrderByEnum.Desc);

                XmlResult result = DataProxy.Current.SelectFirst<MappingcsTxtEntity>(this, where, orderbys, out data);
                if (result.IsSuccess)
                {
                    if (data == null)
                    {
                        return "01";
                    }
                    else
                    {
                        int iCount = Convert.ToInt16(data.MappingId);
                        return (iCount + 1).ToString("00");
                    }
                }

                return "01";
            }
            else
            {
                MappingcsXlsmdbEntity XlsData = null;
                Expression where = new Expression(MappingcsXlsmdbEntity.Field.ReceiveType, this.EditReceiveType);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(MappingcsXlsmdbEntity.Field.MappingId, OrderByEnum.Desc);

                XmlResult result = DataProxy.Current.SelectFirst<MappingcsXlsmdbEntity>(this, where, orderbys, out XlsData);
                if (result.IsSuccess)
                {
                    if (XlsData == null)
                    {
                        return "01";
                    }
                    else
                    {
                        int iCount = Convert.ToInt16(XlsData.MappingId);
                        return (iCount + 1).ToString("00");
                    }
                }

                return "01";
            }
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
    }
}