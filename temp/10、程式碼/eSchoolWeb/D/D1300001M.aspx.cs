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
    /// <summary>
    /// 一般收費標準檔 (維護)
    /// </summary>
    public partial class D1300001M : BasePage
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
        private string EditCollegeId
        {
            get
            {
                return ViewState["EditCollegeId"] as string;
            }
            set
            {
                ViewState["EditCollegeId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的科系代碼參數
        /// </summary>
        private string EditMajorId
        {
            get
            {
                return ViewState["EditMajorId"] as string;
            }
            set
            {
                ViewState["EditMajorId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的年級代碼參數
        /// </summary>
        private string EditStuGrade
        {
            get
            {
                return ViewState["EditStuGrade"] as string;
            }
            set
            {
                ViewState["EditStuGrade"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的班別代碼參數
        /// </summary>
        private string EditClassId
        {
            get
            {
                return ViewState["EditClassId"] as string;
            }
            set
            {
                ViewState["EditClassId"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的一般收費標準檔
        /// </summary>
        private GeneralStandardEntity EditGeneralStandardEntity
        {
            get
            {
                return ViewState["EditGeneralStandardEntity"] as GeneralStandardEntity;
            }
            set
            {
                ViewState["EditGeneralStandardEntity"] = value == null ? null : value;
            }
        }

        /// <summary>
        /// 編輯的收入科目繳費金額
        /// </summary>
        private string[] GeneralItemValues
        {
            get
            {
                return ViewState["GeneralItemValues"] as string[];
            }
            set
            {
                ViewState["GeneralItemValues"] = value;
            }
        }

        /// <summary>
        /// 收入科目
        /// </summary>
        private string[] ReceiveItemNames
        {
            get
            {
                return ViewState["ReceiveItemNames"] as string[];
            }
            set
            {
                ViewState["ReceiveItemNames"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            this.tbxOrder.Text = String.Empty;
        }

        /// <summary>
        /// 繫結 院別 下拉選單
        /// </summary>
        private void BindCollegeIdOptions()
        {
            List<ListItem> list = new List<ListItem>();
            //20150509 emil add
            list.Add(new ListItem("ALL", "all"));
            //20150509 emil add end

            Expression where = new Expression();
            where.And(CollegeListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(CollegeListEntity.Field.YearId, this.EditYearId);
            where.And(CollegeListEntity.Field.TermId, this.EditTermId);
            where.And(CollegeListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(CollegeListEntity.Field.CollegeId, OrderByEnum.Asc);

            CollegeListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<CollegeListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (CollegeListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.CollegeName, data.CollegeId);
                    list.Add(new ListItem(text, data.CollegeId));
                }
            }
            

            this.ddlCollegeId.Items.Clear();
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlCollegeId.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 科系 下拉選單
        /// </summary>
        private void BindMajorIdOptions()
        {
            List<ListItem> list = new List<ListItem>();
            //20150509 emil add
            list.Add(new ListItem("ALL", "all"));
            //20150509 emil add end

            Expression where = new Expression();
            where.And(MajorListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(MajorListEntity.Field.YearId, this.EditYearId);
            where.And(MajorListEntity.Field.TermId, this.EditTermId);
            where.And(MajorListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(MajorListEntity.Field.MajorId, OrderByEnum.Asc);

            MajorListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<MajorListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (MajorListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.MajorName, data.MajorId);
                    list.Add(new ListItem(text, data.MajorId));
                }
            }
            
            this.ddlMajorId.Items.Clear();
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlMajorId.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 年級 下拉選單
        /// </summary>
        private void BindStuGradeOptions()
        {
            List<ListItem> list = new List<ListItem>();
            //20150509 emil add
            list.Add(new ListItem("", "0"));
            //20150509 emil add end
            list.Add(new ListItem("一年級(1)", "1"));
            list.Add(new ListItem("二年級(2)", "2"));
            list.Add(new ListItem("三年級(3)", "3"));
            list.Add(new ListItem("四年級(4)", "4"));
            list.Add(new ListItem("五年級(5)", "5"));
            list.Add(new ListItem("六年級(6)", "6"));
            list.Add(new ListItem("七年級(7)", "7"));
            list.Add(new ListItem("八年級(8)", "8"));
            list.Add(new ListItem("九年級(9)", "9"));
            list.Add(new ListItem("十年級(10)", "10"));
            list.Add(new ListItem("十一年級(11)", "11"));
            list.Add(new ListItem("十二年級(12)", "12"));
            
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlStuGrade.Items.Clear();
                this.ddlStuGrade.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 繫結 班別 下拉選單
        /// </summary>
        private void BindClassIdOptions()
        {
            List<ListItem> list = new List<ListItem>();
            //20150509 emil add
            list.Add(new ListItem("ALL", "all"));
            //20150509 emil add end

            Expression where = new Expression();
            where.And(ClassListEntity.Field.ReceiveType, this.EditReceiveType);
            where.And(ClassListEntity.Field.YearId, this.EditYearId);
            where.And(ClassListEntity.Field.TermId, this.EditTermId);
            where.And(ClassListEntity.Field.DepId, this.EditDepId);

            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>();
            orderbys.Add(ClassListEntity.Field.ClassId, OrderByEnum.Asc);

            ClassListEntity[] datas = null;
            XmlResult result = DataProxy.Current.SelectAll<ClassListEntity>(this, where, orderbys, out datas);
            if (result.IsSuccess)
            {
                foreach (ClassListEntity data in datas)
                {
                    string text = String.Format("{0}({1})", data.ClassName, data.ClassId);
                    list.Add(new ListItem(text, data.ClassId));
                }
            }
            
            this.ddlClassId.Items.Clear();
            ListItem[] items = list.ToArray();
            if (items != null && items.Length > 0)
            {
                this.ddlClassId.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 取得收入科目名稱
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private XmlResult GetReceiveItemNames(out string[] items)
        {
            items = this.ReceiveItemNames;
            if (items == null)
            {
                SchoolRidEntity entity = null;
                Expression where = new Expression();
                where.And(SchoolRidEntity.Field.ReceiveType, this.EditReceiveType);
                where.And(SchoolRidEntity.Field.YearId, this.EditYearId);
                where.And(SchoolRidEntity.Field.TermId, this.EditTermId);
                where.And(SchoolRidEntity.Field.DepId, this.EditDepId);
                where.And(SchoolRidEntity.Field.ReceiveId, this.EditReceiveId);
                //ReceiveStatus代收費用型態
                //where.And(SchoolRidEntity.Field.ReceiveStatus, this.EditReceiveType);

                XmlResult xmlResult = DataProxy.Current.SelectFirst<SchoolRidEntity>(this, where, null, out entity);
                if (xmlResult.IsSuccess)
                {
                    if (entity == null)
                    {
                        items = new string[0];
                    }
                    else
                    {
                        items = new string[30];
                        items[0] = entity.ReceiveItem01;
                        items[1] = entity.ReceiveItem02;
                        items[2] = entity.ReceiveItem03;
                        items[3] = entity.ReceiveItem04;
                        items[4] = entity.ReceiveItem05;
                        items[5] = entity.ReceiveItem06;
                        items[6] = entity.ReceiveItem07;
                        items[7] = entity.ReceiveItem08;
                        items[8] = entity.ReceiveItem09;
                        items[9] = entity.ReceiveItem10;
                        items[10] = entity.ReceiveItem11;
                        items[11] = entity.ReceiveItem12;
                        items[12] = entity.ReceiveItem13;
                        items[13] = entity.ReceiveItem14;
                        items[14] = entity.ReceiveItem15;
                        items[15] = entity.ReceiveItem16;
                        items[16] = entity.ReceiveItem17;
                        items[17] = entity.ReceiveItem18;
                        items[18] = entity.ReceiveItem19;
                        items[19] = entity.ReceiveItem20;
                        items[20] = entity.ReceiveItem21;
                        items[21] = entity.ReceiveItem22;
                        items[22] = entity.ReceiveItem23;
                        items[23] = entity.ReceiveItem24;
                        items[24] = entity.ReceiveItem25;
                        items[25] = entity.ReceiveItem26;
                        items[26] = entity.ReceiveItem27;
                        items[27] = entity.ReceiveItem28;
                        items[28] = entity.ReceiveItem29;
                        items[29] = entity.ReceiveItem30;

                        bool hasItem = false;
                        foreach (string item in items)
                        {
                            if (!String.IsNullOrEmpty(item))
                            {
                                hasItem = true;
                                break;
                            }
                        }
                        if (!hasItem)
                        {
                            items = new String[0];
                        }
                    }
                }
                else
                {
                    return xmlResult;
                }

            }

            return new XmlResult(true);
        }

        /// <summary>
        /// 結繫維護資料
        /// </summary>
        /// <param name="data">維護資料</param>
        /// <param name="itemValues">收入科目資料</param>
        private void BindEditData(GeneralStandardEntity data, string[] itemValues)
        {
            if (data == null)
            {
                this.tbxOrder.Text = String.Empty;
                this.ddlCollegeId.SelectedIndex = -1;
                this.ddlMajorId.SelectedIndex = -1;
                this.ddlStuGrade.SelectedIndex = -1;
                this.ddlClassId.SelectedIndex = -1;
                this.litHtml.Text = String.Empty;
                this.ccbtnOK.Visible = false;
                return;
            }

            this.EditGeneralStandardEntity = data;

            WebHelper.SetDropDownListSelectedValue(this.ddlCollegeId, data.CollegeId);
            WebHelper.SetDropDownListSelectedValue(this.ddlMajorId, data.MajorId);
            WebHelper.SetDropDownListSelectedValue(this.ddlStuGrade, data.StuGrade);
            WebHelper.SetDropDownListSelectedValue(this.ddlClassId, data.ClassId);

            bool enabled = true;
            switch (this.Action)
            {
                case ActionMode.Insert:
                    this.ddlCollegeId.Enabled = true;
                    this.ddlClassId.Enabled = true;
                    this.ddlMajorId.Enabled = true;
                    this.ddlStuGrade.Enabled = true;
                    this.tbxOrder.Enabled = true;
                    enabled = true;
                    break;
                case ActionMode.Modify:
                    this.ddlCollegeId.Enabled = false;
                    this.ddlClassId.Enabled = false;
                    this.ddlMajorId.Enabled = false;
                    this.ddlStuGrade.Enabled = false;
                    this.tbxOrder.Enabled = true;
                    enabled = true;
                    break;
                default:
                    this.ddlCollegeId.Enabled = false;
                    this.ddlClassId.Enabled = false;
                    this.ddlMajorId.Enabled = false;
                    this.ddlStuGrade.Enabled = false;
                    this.tbxOrder.Enabled = false;
                    enabled = false;
                    break;
            }

            tbxOrder.Text = data.OrderId == null ? String.Empty : data.OrderId.ToString();

            #region 組收入科目 Html
            {
                StringBuilder sb = new StringBuilder();

                string[] itemNames = null;
                this.GetReceiveItemNames(out itemNames);
                if (itemNames != null && itemNames.Length > 0)
                {
                    Decimal?[] values = new Decimal?[] {
                        data.General01, data.General02, 
                        data.General03, data.General04, 
                        data.General05, data.General06, 
                        data.General07, data.General08, 
                        data.General09, data.General10, 
                        data.General11, data.General12, 
                        data.General13, data.General14, 
                        data.General15, data.General16,
                        data.General17, data.General18,
                        data.General19, data.General20,
                        data.General21, data.General22,
                        data.General23, data.General24,
                        data.General25, data.General26,
                        data.General27, data.General28,
                        data.General29, data.General30,
                    };

                    if (itemValues == null)
                    {
                        itemValues = new string[0];
                    }
                    for (int idx = 0; idx < values.Length; idx++)
                    {
                        string itemName = null;
                        if (idx < itemNames.Length)
                        {
                            itemName = itemNames[idx];
                        }
                        if (String.IsNullOrEmpty(itemName))
                        {
                            continue;
                        }

                        Decimal? value = values[idx];

                        string fieldName = "General_" + (idx + 1).ToString("00");
                        string tbxName = "tbxItem" + (idx + 1).ToString();
                        string fieldValue = DataFormat.GetAmountText(value);

                        sb.AppendLine("<tr>");
                        sb.AppendFormat("<td><div align=\"right\">{0}</div></td>", itemName).AppendLine();
                        if (enabled)
                        {
                            sb.AppendFormat("<td><input type=\"text\" name=\"tbxItem{0:00}\" id=\"tbxItem{0:00}\" value=\"{1}\" maxlength=\"8\" /></td>", idx + 1, fieldValue).AppendLine();
                        }
                        else
                        {
                            sb.AppendFormat("<td>{0}</td>", fieldValue).AppendLine();
                        }
                        sb.AppendLine("</tr>");
                    }
                }
                this.litHtml.Text = sb.ToString();
            }
            #endregion

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 將修改後的資料儲存至資料庫
        /// </summary>
        /// <returns></returns>
        private GeneralStandardEntity GetEditData(out string[] itemValues)
        {
            itemValues = null;

            GeneralStandardEntity data = this.EditGeneralStandardEntity;
            data.ReceiveType = this.EditReceiveType;
            data.YearId = this.EditYearId;
            data.TermId = this.EditTermId;
            data.DepId = this.EditDepId;
            data.ReceiveId = this.EditReceiveId;

            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    data.CollegeId = this.ddlCollegeId.SelectedValue;
                    data.MajorId = this.ddlMajorId.SelectedValue;
                    data.StuGrade = this.ddlStuGrade.SelectedValue;
                    data.ClassId = this.ddlClassId.SelectedValue;
                    break;
                case ActionMode.Modify:     //修改
                case ActionMode.Delete:     //刪除
                    data.CollegeId = this.EditCollegeId;
                    data.MajorId = this.EditMajorId;
                    data.StuGrade = this.EditStuGrade;
                    data.ClassId = this.EditClassId;
                    break;
            }

            if (this.Action != ActionMode.Delete)
            {
                Decimal order = 0;
                if (Decimal.TryParse(this.tbxOrder.Text.Trim(), out order))
                {
                    data.OrderId = (int)order;
                }
                else
                {
                    data.OrderId = null;
                }
            }

            #region 處理收入科目
            if (this.Action != ActionMode.Delete)
            {
                Result result = null;

                string[] fieldNames = new string[] {
                    GeneralStandardEntity.Field.General01, GeneralStandardEntity.Field.General02, 
                    GeneralStandardEntity.Field.General03, GeneralStandardEntity.Field.General04, 
                    GeneralStandardEntity.Field.General05, GeneralStandardEntity.Field.General06, 
                    GeneralStandardEntity.Field.General07, GeneralStandardEntity.Field.General08, 
                    GeneralStandardEntity.Field.General09, GeneralStandardEntity.Field.General10, 
                    GeneralStandardEntity.Field.General11, GeneralStandardEntity.Field.General12, 
                    GeneralStandardEntity.Field.General13, GeneralStandardEntity.Field.General14, 
                    GeneralStandardEntity.Field.General15, GeneralStandardEntity.Field.General16, 
                    GeneralStandardEntity.Field.General17, GeneralStandardEntity.Field.General18,
                    GeneralStandardEntity.Field.General19, GeneralStandardEntity.Field.General20,
                    GeneralStandardEntity.Field.General21, GeneralStandardEntity.Field.General22,
                    GeneralStandardEntity.Field.General23, GeneralStandardEntity.Field.General24,
                    GeneralStandardEntity.Field.General25, GeneralStandardEntity.Field.General26,
                    GeneralStandardEntity.Field.General27, GeneralStandardEntity.Field.General28,
                    GeneralStandardEntity.Field.General29, GeneralStandardEntity.Field.General30,
                };
                itemValues = new string[fieldNames.Length];

                string[] itemNames = null;
                this.GetReceiveItemNames(out itemNames);
                if (itemNames != null && itemNames.Length > 0)
                {
                    for (int idx = 0; idx < fieldNames.Length; idx++)
                    {
                        itemValues[idx] = null;
                        string fieldName = fieldNames[idx];
                        string itemName = null;
                        if (idx < itemNames.Length)
                        {
                            itemName = itemNames[idx];
                        }

                        if (String.IsNullOrEmpty(itemName))
                        {
                            data.SetValue(fieldName, null);
                        }
                        else
                        {
                            string controlID = String.Format("tbxItem{0:00}", idx + 1);
                            string tbxValue = GetValue(controlID);
                            itemValues[idx] = tbxValue;
                            decimal value = 0;
                            if (Decimal.TryParse(tbxValue, out value))
                            {
                                data.SetValue(fieldName, value);
                            }
                            else
                            {
                                result = data.SetValue(fieldName, null);
                                if (!result.IsSuccess)
                                {
                                    //[TODO] 固定顯示訊息的收集
                                    string msg = this.GetLocalized("處理收入科目發生錯誤");
                                    this.ShowSystemMessage(msg);
                                    this.ccbtnOK.Visible = false;
                                    return null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int idx = 0; idx < fieldNames.Length; idx++)
                    {
                        itemValues[idx] = null;
                        string fieldName = fieldNames[idx];
                        result = data.SetValue(fieldName, null);
                        if (!result.IsSuccess)
                        {
                            //[TODO] 固定顯示訊息的收集
                            string msg = this.GetLocalized("處理收入科目發生錯誤");
                            this.ShowSystemMessage(msg);
                            this.ccbtnOK.Visible = false;
                            return null;
                        }
                    }
                }
            }
            #endregion

            return data;
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
        public bool CheckDataForInputUI(string[] itemValues)
        {
            if (String.IsNullOrWhiteSpace(tbxOrder.Text))
            {
                this.ShowMustInputAlert("計算順序");
                return false;
            }
            if (!Common.IsNumber(tbxOrder.Text.Trim()))
            {
                //[TODO] 固定顯示訊息的收集
                string msg = this.GetLocalized("計算順序限輸入數字");
                this.ShowJsAlert(msg);
                return false;
            }
            string[] itemNames = null;
            this.GetReceiveItemNames(out itemNames);
            if (itemNames != null && itemNames.Length > 0)
            {
                if (itemValues == null)
                {
                    itemValues = new string[0];
                }
                for (int idx = 0; idx < itemNames.Length; idx++)
                {
                    string itemName = itemNames[idx];
                    if (!String.IsNullOrEmpty(itemName) && idx < itemValues.Length)
                    {
                        string itemValue = itemValues[idx];
                        /* 20150509 emil 可以允許不填值
                        if (String.IsNullOrEmpty(itemValue))
                        {
                            this.ShowMustInputAlert(itemName + "的繳費金額");
                            return false;
                        }
                        */
                        if (!String.IsNullOrEmpty(itemValue))
                        {
                            if (!Common.IsMoney(itemValue, 1, 8))
                            {
                                //[TODO] 固定顯示訊息的收集
                                string msg = this.GetLocalized("繳費金額最多輸入8位數的金額");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            return true;
        }

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
                this.EditReceiveType = QueryString.TryGetValue("ReceiveType", String.Empty);
                this.EditYearId = QueryString.TryGetValue("YearId", String.Empty);
                this.EditTermId = QueryString.TryGetValue("TermId", String.Empty);
                this.EditDepId = QueryString.TryGetValue("DepId", String.Empty);
                this.EditReceiveId = QueryString.TryGetValue("ReceiveId", String.Empty);
                this.EditCollegeId = QueryString.TryGetValue("CollegeId", String.Empty);
                this.EditMajorId = QueryString.TryGetValue("MajorId", String.Empty);
                this.EditStuGrade = QueryString.TryGetValue("StuGrade", String.Empty);
                this.EditClassId = QueryString.TryGetValue("ClassId", String.Empty);

                if (String.IsNullOrEmpty(this.EditReceiveType)
                    || String.IsNullOrEmpty(this.EditYearId)
                    || String.IsNullOrEmpty(this.EditTermId)
                    //|| String.IsNullOrEmpty(this.EditDepId)
                    || String.IsNullOrEmpty(this.EditReceiveId)
                    || !ActionMode.IsMaintinaMode(this.Action)
                    || ((this.Action == ActionMode.Modify || this.Action == ActionMode.Delete)
                        && (String.IsNullOrEmpty(this.EditCollegeId) || String.IsNullOrEmpty(this.EditMajorId) || String.IsNullOrEmpty(this.EditStuGrade) || String.IsNullOrEmpty(this.EditClassId))))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }

                #endregion

                #region 檢查業務別碼授權
                if (!this.GetLogonUser().IsAuthReceiveTypes(this.EditReceiveType))
                {
                    this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_RECEIVETYPE, "未授權該業務別");
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得收入科目
                {
                    string[] receiveItemNames = null;
                    XmlResult xmlResult = this.GetReceiveItemNames(out receiveItemNames);
                    if (!xmlResult.IsSuccess)
                    {
                        //[TODO] 固定顯示訊息的收集
                        string msg = this.GetLocalized("請先設定收入科目，再進入此網頁");
                        this.ShowSystemMessage(msg);
                        this.ccbtnOK.Visible = false;
                        return;
                    }
                }
                #endregion

                BindCollegeIdOptions();
                BindMajorIdOptions();
                BindStuGradeOptions();
                BindClassIdOptions();

                #region 取得維護資料
                GeneralStandardEntity data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new GeneralStandardEntity();
                            data.ReceiveType = this.EditReceiveType;
                            data.YearId = this.EditYearId;
                            data.TermId = this.EditTermId;
                            data.DepId = this.EditDepId;
                            data.ReceiveId = this.EditReceiveId;

                            data.CollegeId = String.Empty;
                            data.MajorId = String.Empty;
                            data.StuGrade = String.Empty;
                            data.ClassId = String.Empty;
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢條件
                            Expression where = new Expression(GeneralStandardEntity.Field.ReceiveType, this.EditReceiveType);
                            where.And(GeneralStandardEntity.Field.YearId, this.EditYearId);
                            where.And(GeneralStandardEntity.Field.TermId, this.EditTermId);
                            where.And(GeneralStandardEntity.Field.DepId, this.EditDepId);
                            where.And(GeneralStandardEntity.Field.ReceiveId, this.EditReceiveId);
                            where.And(GeneralStandardEntity.Field.CollegeId, this.EditCollegeId);
                            where.And(GeneralStandardEntity.Field.MajorId, this.EditMajorId);
                            where.And(GeneralStandardEntity.Field.StuGrade, this.EditStuGrade);
                            where.And(GeneralStandardEntity.Field.ClassId, this.EditClassId);
                            #endregion

                            #region 查詢資料
                            XmlResult xmlResult = DataProxy.Current.SelectFirst<GeneralStandardEntity>(this, where, null, out data);
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
                            #endregion
                        }
                        #endregion
                        break;
                }
                #endregion

                this.ucFilter1.GetDataAndBind(this.EditReceiveType, this.EditYearId, this.EditTermId, this.EditDepId, this.EditReceiveId);

                this.BindEditData(data, null);
            }
        }

        /// <summary>
        /// 儲存修改資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            string[] itemValues = null;
            GeneralStandardEntity data = this.GetEditData(out itemValues);
            if (!this.CheckDataForInputUI(itemValues))
            {
                return;
            }

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "D1300001.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        #region 補齊資料
                        data.Status = DataStatusCodeTexts.NORMAL;
                        data.CrtUser = this.GetLogonUser().UserId;
                        data.CrtDate = DateTime.Now;
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Insert<GeneralStandardEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);

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
                        #region 補齊資料
                        data.MdyUser = this.GetLogonUser().UserId;
                        data.MdyDate = DateTime.Now;
                        #endregion

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<GeneralStandardEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);

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
                        XmlResult xmlResult = DataProxy.Current.Delete<GeneralStandardEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                            }
                            else
                            {
                                WebHelper.SetFilterArguments(data.ReceiveType, data.YearId, data.TermId, data.DepId, data.ReceiveId);

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
    }
}