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
    public partial class D1400004M1 : BasePage
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
        private MappingloXlsmdbEntity EditMappingloXlsmdbEntity
        {
            get
            {
                return ViewState["EditMappingloXlsmdbEntity"] as MappingloXlsmdbEntity;
            }
            set
            {
                ViewState["EditMappingloXlsmdbEntity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 儲存對照表Txt
        /// </summary>
        private MappingloTxtEntity EditMappingloTxtEntity
        {
            get
            {
                return ViewState["EditMappingloTxtEntity"] as MappingloTxtEntity;
            }
            set
            {
                ViewState["EditMappingloTxtEntity"] = value == null ? null : value;
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

                MappingloXlsmdbEntity XlsData = null;
                MappingloTxtEntity TxtData = null;

                switch (this.Action)
                {
                    case "M":   //修改
                        #region 取得修改的資料
                        {
                            XmlResult result = new XmlResult();
                            if (this.EditFileType == "xls")
                            {
                                Expression where = new Expression(MappingloXlsmdbEntity.Field.ReceiveType, this.EditReceiveType);
                                where.And(MappingloXlsmdbEntity.Field.MappingId, this.EditMappingId);
                                result = DataProxy.Current.SelectFirst<MappingloXlsmdbEntity>(this, where, null, out XlsData);

                            }
                            else
                            {
                                Expression where = new Expression(MappingloTxtEntity.Field.ReceiveType, this.EditReceiveType);
                                where.And(MappingloTxtEntity.Field.MappingId, this.EditMappingId);
                                result = DataProxy.Current.SelectFirst<MappingloTxtEntity>(this, where, null, out TxtData);
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
                            XlsData = new MappingloXlsmdbEntity();
                            TxtData = new MappingloTxtEntity();
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
        private void BindXlsData(MappingloXlsmdbEntity entity)
        {
            if (entity == null && Action == "M")
            {
                this.lbtnNext.Visible = false;
                return;
            }
            else
            {
                this.lbtnNext.Visible = true;
                this.EditMappingloXlsmdbEntity = entity;
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

                    #region 就貸金額項目欄位另外處理
                    int iCount = 0;
                    for (int i = 1; i <= 40; i++)
                    {
                        string strFieldName = "Lo_" + i.ToString();
                        object ob = entity.GetValue(strFieldName, out rt);
                        if (ob != null)
                        {
                            if (!String.IsNullOrEmpty(ob.ToString()))
                            {
                                iCount++;
                            }
                        }
                    }
                    if (iCount > 0)
                    {
                        CheckBox ck = (CheckBox)cph.FindControl("chkLo");
                        if (ck != null)
                        {
                            ck.Checked = true;
                        }
                        tbxLoCount.Text = iCount.ToString();
                    }
                    #endregion

                    break;
            }

            this.lbtnNext.Visible = true;
        }

        /// <summary>
        /// 結繫主要條件選項資料 Txt
        /// </summary>
        /// <param name="option">主要條件選項資料</param>
        private void BindTxtData(MappingloTxtEntity entity)
        {
            if (entity == null && Action == "M")
            {
                this.lbtnNext.Visible = false;
                return;
            }
            else
            {
                this.lbtnNext.Visible = true;
                this.EditMappingloTxtEntity = entity;
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

                    #region 就貸金額項目欄位另外處理
                    int iCount = 0;
                    for (int i = 1; i <= 40; i++)
                    {
                        string strFieldName = "Lo_" + i.ToString() + "_S";
                        object ob = entity.GetValue(strFieldName, out rt);
                        if (ob != null)
                        {
                            if (!String.IsNullOrEmpty(ob.ToString()))
                            {
                                iCount++;
                            }
                        }
                    }
                    if (iCount > 0)
                    {
                        CheckBox ck = (CheckBox)cph.FindControl("chkLo");
                        if (ck != null)
                        {
                            ck.Checked = true;
                        }
                        tbxLoCount.Text = iCount.ToString();
                    }
                    #endregion

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
                    if (ck.ID != "chkS_Id" && ck.ID != "chkCancel_No")
                    {
                        ck.Checked = false;
                    }
                }
            }
            tbxMappingName.Text = "";
            tbxLoCount.Text = "";

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
                            int iCount = 0;
                            switch (ck.ID)
                            {
                                case "chkLo":
                                    iCount = Convert.ToInt16(((TextBox)cph.FindControl("tbxLoCount")).Text);
                                    for (int i = 1; i <= iCount; i++)
                                    {
                                        strID = "Lo_" + i.ToString();
                                        strName = ck.Text.Trim() + i.ToString();
                                        lists.Add(strID, strName);
                                    }
                                    break;
                                default:
                                    strID = ck.ID.Replace("chk", "");
                                    strName = ck.Text.Trim();
                                    lists.Add(strID, strName);
                                    break;
                            }
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

                //組 Html
                initStep2B(this.EditMappingloXlsmdbEntity);
            }
            else
            {
                divStep1.Visible = false;
                divStep2A.Visible = true;
                divStep2B.Visible = false;

                //組 Html
                initStep2A(this.EditMappingloTxtEntity);
            }
        }

        protected void initStep2B(MappingloXlsmdbEntity entity)
        {
            List<MappingItem> items = this.EditMappingItems;

            labMappingNameB.Text = HttpUtility.HtmlEncode(this.EditMappingName);

            #region 組 Html

            KeyValueList<string> itemNames = this.GetCheckedItems();
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

                    if (sID == MappingloXlsmdbEntity.Field.CancelNo)
                    {
                        sb
                            .AppendLine("<tr>")
                            .AppendFormat("<td>{0}</td>", sName).AppendLine()
                            .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" readonly=\"readonly\" disabled=\"disabled\" /></td>", sID, sValue).AppendLine()
                            .AppendLine("</tr>");
                    }
                    else
                    {
                        sb
                            .AppendLine("<tr>")
                            .AppendFormat("<td>{0}</td>", sName).AppendLine()
                            .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" maxlength=\"20\" id=\"ContentPlaceHolder1_tbx{0}\" /></td>", sID, sValue).AppendLine()
                            .AppendLine("</tr>");
                    }
                }
                this.litHtmlB.Text = sb.ToString();
            }
            #endregion

        }

        protected void initStep2A(MappingloTxtEntity entity)
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

                    if (sID == MappingloXlsmdbEntity.Field.CancelNo)
                    {
                        sb
                            .AppendLine("<tr>")
                            .AppendFormat("<td>{0}</td>", sName).AppendLine()
                            .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" readonly=\"readonly\" disabled=\"disabled\" /></td>", fieldName_Start, fieldValue_Start).AppendLine()
                            .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" readonly=\"readonly\" disabled=\"disabled\" /></td>", fieldName_Length, fieldValue_Length).AppendLine()
                            .AppendLine("</tr>");
                    }
                    else
                    {
                        sb
                            .AppendLine("<tr>")
                            .AppendFormat("<td>{0}</td>", sName).AppendLine()
                            .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" /></td>", fieldName_Start, fieldValue_Start).AppendLine()
                            .AppendFormat("<td><input name=\"ctl00$ContentPlaceHolder1$tbx{0}\" type=\"text\" value=\"{1}\" id=\"ContentPlaceHolder1_tbx{0}\" /></td>", fieldName_Length, fieldValue_Length).AppendLine()
                            .AppendLine("</tr>");
                    }
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

                                if (sID == MappingloXlsmdbEntity.Field.CancelNo)
                                {
                                    //虛擬帳號為固定值不可修改，所以不取輸入值
                                    sValue = MappingloXlsmdbEntity.CancelNo_FIX_VALUE;
                                }
                                else
                                {
                                    string txtValue = GetValue("tbx" + sID);
                                    if (txtValue != null)
                                    {
                                        if (String.IsNullOrEmpty(txtValue))
                                        {
                                            sValue = string.Empty;
                                        }
                                        sValue = txtValue;
                                    }
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
                                string tbxValue_Start = null;   // GetValue(tbxFieldName_Start);
                                string tbxValue_Length = null;  // GetValue(tbxFieldName_Length);

                                if (tmpKey == MappingloXlsmdbEntity.Field.CancelNo)
                                {
                                    //虛擬帳號為固定值不可修改，所以不取輸入值
                                    tbxValue_Start = MappingloTxtEntity.CancelNoS_FIX_VALUE.ToString();
                                    tbxValue_Length = MappingloTxtEntity.CancelNoL_FIX_VALUE.ToString();
                                }
                                else
                                {
                                    tbxValue_Start = GetValue(tbxFieldName_Start);
                                    tbxValue_Length = GetValue(tbxFieldName_Length);
                                }

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

        protected void lbtnOK2A_Click(object sender, EventArgs e)
        {
            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1400004.aspx";

            KeyValueList<string> itemNames = this.GetCheckedItems();
            if (itemNames != null && itemNames.Count > 0)
            {
                int cancelNoIndex = -1;
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    string key = itemNames[idx].Key;
                    string sFieldName_Start = key + "_S";
                    string sFieldName_Length = key + "_L";

                    itemNames[idx].Key = sFieldName_Start + ", " + sFieldName_Length;

                    string tbxFieldName_Start = "tbx" + sFieldName_Start;
                    string tbxFieldName_Length = "tbx" + sFieldName_Length;
                    string tbxValue_Start = null;   // GetValue(tbxFieldName_Start);
                    string tbxValue_Length = null;  // GetValue(tbxFieldName_Length);

                    if (key == MappingloXlsmdbEntity.Field.CancelNo)
                    {
                        //虛擬帳號不存在資料庫，所以掠過
                        cancelNoIndex = idx;
                        continue;
                    }
                    else
                    {
                        tbxValue_Start = GetValue(tbxFieldName_Start);
                        tbxValue_Length = GetValue(tbxFieldName_Length);
                    }

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

                //移除虛擬帳號項目，因為虛擬帳號不存在資料庫
                itemNames.RemoveAt(cancelNoIndex);
            }

            string receiveType = this.EditReceiveType;
            string mappingId = string.Empty;

            //如果是新增要取一個新的 mappingID
            if (Action == "A")
            {
                mappingId = GetNewMappingID(MappingloTxtEntity.TABLE_NAME);
            }
            else
            {
                mappingId = this.EditMappingId;
            }

            itemNames.Add("tableName", MappingloTxtEntity.TABLE_NAME);
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
            string backUrl = "D1400004.aspx";

            KeyValueList<string> itemNames = this.GetCheckedItems();

            int cancelNoIndex = -1;
            if (itemNames != null && itemNames.Count > 0)
            {
                Encoding encoding = Encoding.Default;
                for (int idx = 0; idx < itemNames.Count; idx++)
                {
                    string key = itemNames[idx].Key;
                    if (key == MappingloXlsmdbEntity.Field.CancelNo)
                    {
                        //虛擬帳號不存在資料庫，所以掠過
                        cancelNoIndex = idx;
                        continue;
                    }
                    else
                    {
                        string txtValue = GetValue("tbx" + key);
                        if (txtValue != null)
                        {
                            if (String.IsNullOrEmpty(txtValue))
                            {
                                this.ShowMustInputAlert(itemNames[idx].Value);
                                return;
                            }
                            else if (encoding.GetByteCount(txtValue) > 20)
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = String.Format("{0} 的試算表欄位名稱不可超過 20 個 byte", itemNames[idx].Value);
                                this.ShowJsAlert(msg);
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

                //移除虛擬帳號項目，因為虛擬帳號不存在資料庫
                itemNames.RemoveAt(cancelNoIndex);
            }

            string receiveType = this.EditReceiveType;
            string mappingId = string.Empty;

            //如果是新增要取一個新的 mappingID
            if (Action == "A")
            {
                mappingId = GetNewMappingID(MappingloXlsmdbEntity.TABLE_NAME);
            }
            else
            {
                mappingId = this.EditMappingId;
            }

            itemNames.Add("tableName", MappingloXlsmdbEntity.TABLE_NAME);
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

        /// <summary>
        /// 取得對照檔代碼
        /// </summary>
        /// <returns></returns>
        protected string GetNewMappingID(string tableName)
        {
            if (tableName == MappingloTxtEntity.TABLE_NAME)
            {
                MappingloTxtEntity data = null;
                Expression where = new Expression(MappingloTxtEntity.Field.ReceiveType, this.EditReceiveType);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(MappingloTxtEntity.Field.MappingId, OrderByEnum.Desc);

                XmlResult result = DataProxy.Current.SelectFirst<MappingloTxtEntity>(this, where, orderbys, out data);
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
                MappingloXlsmdbEntity XlsData = null;
                Expression where = new Expression(MappingloXlsmdbEntity.Field.ReceiveType, this.EditReceiveType);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
                orderbys.Add(MappingloXlsmdbEntity.Field.MappingId, OrderByEnum.Desc);

                XmlResult result = DataProxy.Current.SelectFirst<MappingloXlsmdbEntity>(this, where, orderbys, out XlsData);
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

        /// <summary>
        /// 檢查資料，以作為使用介面輸入是否正確 (前台用)
        /// </summary>
        /// <returns>傳回錯誤訊息</returns>
        private string CheckDataForInputUI()
        {
            if (String.IsNullOrEmpty(tbxMappingName.Text))
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("對照表名稱");
            }
            if (chkLo.Checked)
            {
                if (String.IsNullOrEmpty(tbxLoCount.Text))
                {
                    return this.GetLocalized("請輸入") + this.GetLocalized("就貸金額項目");
                }
            }

            #region [Old] 土銀說上傳的就貸金額項目不用去異動 Student_Receive 的收入科目金額，所以不鎖了 (20150908)
            //if (this.chkLo_amount.Checked && this.chkLo.Checked)
            //{
            //    return this.GetLocalized("不可同時上傳「就貸金額」與「就貸金額項目」");
            //}
            #endregion

            if (!this.chkLo_amount.Checked && !this.chkLo.Checked)
            {
                return this.GetLocalized("請選擇「就貸金額」或「就貸金額項目」");
            }

            return String.Empty;
        }
    }
}