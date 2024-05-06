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

namespace eSchoolWeb.S
{
    /// <summary>
    /// 學校設定檔管理
    /// </summary>
    public partial class S5500001 : BasePage
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
        /// 編輯的商家代號參數
        /// </summary>
        private string QueryReceiveType
        {
            get
            {
                return ViewState["QueryReceiveType"] as string;
            }
            set
            {
                ViewState["QueryReceiveType"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的學校代碼參數
        /// </summary>
        private string EditMySchIdenty
        {
            get
            {
                return ViewState["EditMySchIdenty"] as string;
            }
            set
            {
                ViewState["EditMySchIdenty"] = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 編輯的同學校代碼的商家代號
        /// </summary>
        private string[] EditMyReceiveTypes
        {
            get
            {
                return ViewState["EditMyReceiveTypes"] as string[];
            }
            set
            {
                ViewState["EditMyReceiveTypes"] = value;
            }
        }
        #endregion

        /// <summary>
        /// 介面初始話
        /// </summary>
        private void InitialUI()
        {
            this.tbxGiree11.Text = string.Empty;
            this.tbxGiree12.Text = string.Empty;
            this.tbxGiree21.Text = string.Empty;
            this.tbxGiree22.Text = string.Empty;
            this.tbxGiree31.Text = string.Empty;
            this.tbxGiree32.Text = string.Empty;
            this.tbxGiree41.Text = string.Empty;
            this.tbxGiree42.Text = string.Empty;
            this.tbxGiree51.Text = string.Empty;
            this.tbxGiree52.Text = string.Empty;
            this.tbxGiree61.Text = string.Empty;
            this.tbxGiree62.Text = string.Empty;

            if (!this.IsPostBack)
            {
                //學年選項與學生登入依據選項是固定的，所以只有非 PostBack 才需要初始化
                this.GetAndBindYearIdOptions();
                CodeText[] options = new CodeText[] { new CodeText("Y", "開放"), new CodeText("N", "不開放") };
                WebHelper.SetRadioButtonListItems(this.rdoOpenStudentArea, options, true, 2, options[0].Code);
                WebHelper.SetRadioButtonListItems(this.rdoLoginType, new LoginKeyTypeCodeTexts(), true, 2, LoginKeyTypeCodeTexts.Default.Code);

                #region [MDY:20160201] 審核階層
                WebHelper.SetDropDownListItems(this.ddlFlowKind, DefaultItem.Kind.Select, false, new FlowKindCodeTexts(), true, true, 0, null);
                #endregion

                #region [MDY:2018xxxx] 獨立成 S5600018 功能，所以 MARK
                //#region [MDY:20160206] 線上資料保留年數、歷史資料保留年數
                //{
                //    CodeTextList items = new CodeTextList();
                //    items.Add("0", "不指定");
                //    for (int year = 1; year <= SchoolRTypeEntity.MaxKeepDataYear; year++)
                //    {
                //        string code = year.ToString();
                //        string text = String.Format("{0:00}學年", year);
                //        items.Add(code, text);
                //    }
                //    WebHelper.SetDropDownListItems(this.ddlKeepDataYear, DefaultItem.Kind.Select, false, items, false, false, 0, null);

                //    items.Clear();
                //    items.Add("0", "不指定");
                //    for (int year = 1; year <= SchoolRTypeEntity.MaxKeepHistoryYear; year++)
                //    {
                //        string code = year.ToString();
                //        string text = String.Format("{0:00}學年", year);
                //        items.Add(code, text);
                //    }
                //    WebHelper.SetDropDownListItems(this.ddlKeepHistoryYear, DefaultItem.Kind.Select, false, items, false, false, 0, null);
                //}
                //#endregion
                #endregion
            }

            string openStudentArea = null;
            string flowKind = null;
            string keepDataYear = null;
            string keepHistoryYear = null;
            string[] myReceiveTypes = null;
            this.EditMySchIdenty = this.GetEditSchoolRType(this.QueryReceiveType, out openStudentArea, out flowKind, out keepDataYear, out keepHistoryYear, out myReceiveTypes);
            this.EditMyReceiveTypes = myReceiveTypes;

            ReceiveConfigEntity config = this.GetEditReceiveConfig(this.QueryReceiveType);
            if (config == null)
            {
                this.Action = ActionMode.Insert;
                this.GetAndBindTermIdOptions(this.QueryReceiveType, this.ddlYearId.SelectedValue);
            }
            else
            {
                this.Action = ActionMode.Modify;
                this.BindEditData(config, openStudentArea, flowKind, keepDataYear, keepHistoryYear);
            }
        }

        protected void GetAndBindYearIdOptions()
        {
            CodeText[] datas = null;

            Expression where = new Expression();
            KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
            orderbys.Add(YearListEntity.Field.YearId, OrderByEnum.Asc);

            string[] codeFieldNames = new string[] { YearListEntity.Field.YearId };
            string codeCombineFormat = null;
            string[] textFieldNames = new string[] { YearListEntity.Field.YearName };
            string textCombineFormat = null;

            XmlResult xmlResult = DataProxy.Current.GetEntityOptions<YearListEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
            if (!xmlResult.IsSuccess)
            {
                this.ShowErrorMessage(xmlResult.Code, "無法取得學年選項資料");
            }

            WebHelper.SetDropDownListItems(this.ddlYearId, DefaultItem.Kind.None, false, datas, true, false, 0, null);
        }

        protected void GetAndBindTermIdOptions(string receiveType, string yearId)
        {
            CodeText[] datas = null;

            if (!String.IsNullOrEmpty(receiveType) && !String.IsNullOrEmpty(yearId))
            {
                Expression where = new Expression(TermListEntity.Field.ReceiveType, receiveType)
                    .And(TermListEntity.Field.YearId, yearId);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(TermListEntity.Field.TermId, OrderByEnum.Asc);

                string[] codeFieldNames = new string[] { TermListEntity.Field.TermId };
                string codeCombineFormat = null;
                string[] textFieldNames = new string[] { TermListEntity.Field.TermName };
                string textCombineFormat = null;

                XmlResult xmlResult = DataProxy.Current.GetEntityOptions<TermListEntity>(this.Page, where, orderbys, codeFieldNames, codeCombineFormat, textFieldNames, textCombineFormat, out datas);
                if (!xmlResult.IsSuccess)
                {
                    this.ShowErrorMessage(xmlResult.Code, "無法取得學期選項資料");
                }
            }
            WebHelper.SetDropDownListItems(this.ddlTermId, DefaultItem.Kind.None, false, datas, true, false, 0, null);
        }

        /// <summary>
        /// 取得學校代碼、是否開放學生專區與同學校代號的所有商家代號
        /// </summary>
        /// <param name="receiveType"></param>
        /// <param name="openStudentArea"></param>
        /// <param name="myReceiveTypes"></param>
        /// <returns>傳回學校代碼或 null</returns>
        private string GetEditSchoolRType(string receiveType, out string openStudentArea, out string flowKind, out string keepDataYear, out string keepHistoryYear, out string[] myReceiveTypes)
        {
            openStudentArea = null;
            flowKind = null;
            keepDataYear = null;
            keepHistoryYear = null;
            myReceiveTypes = null;
            string mySchIdenty = null;

            string action = this.GetLocalized("讀取商家代號資料");
            #region 取得學校代碼與是否開放學生專區
            {
                SchoolRTypeEntity data = null;
                Expression where = new Expression(SchoolRTypeEntity.Field.ReceiveType, this.QueryReceiveType);
                XmlResult result = DataProxy.Current.SelectFirst<SchoolRTypeEntity>(this, where, null, out data);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return null;
                }
                if (data == null)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "查無商家代號資料");
                    return null;
                }
                mySchIdenty = data.SchIdenty.Trim();
                openStudentArea = data.OpenStudentArea;

                #region [MDY:20160201] 審核階層
                flowKind = data.FlowKind;
                #endregion

                #region [MDY:2018xxxx] 獨立成 S5600018 功能，所以 MARK
                //#region [MDY:20160206] 線上資料保留年數、歷史資料保留年數
                //keepDataYear = data.KeepDataYear == null ? String.Empty : data.KeepDataYear.ToString();
                //keepHistoryYear = data.KeepHistoryYear == null ? String.Empty : data.KeepHistoryYear.ToString();
                //#endregion
                #endregion
            }
            #endregion

            #region 取得同學校代號的所有商家代號
            {
                SchoolRTypeEntity[] datas = null;
                Expression where = new Expression(SchoolRTypeEntity.Field.SchIdenty, mySchIdenty);
                KeyValueList<OrderByEnum> orderbys = new KeyValueList<OrderByEnum>(1);
                orderbys.Add(SchoolRTypeEntity.Field.ReceiveType, OrderByEnum.Asc);
                XmlResult result = DataProxy.Current.SelectAll<SchoolRTypeEntity>(this, where, orderbys, out datas);
                if (!result.IsSuccess)
                {
                    this.ShowActionFailureMessage(action, result.Code, result.Message);
                    return null;
                }
                if (datas == null || datas.Length == 0)
                {
                    this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "查無相關商家代號資料");
                    return null;
                }
                myReceiveTypes = new string[datas.Length];
                for (int idx = 0; idx < datas.Length; idx++)
                {
                    myReceiveTypes[idx] = datas[idx].ReceiveType.Trim();
                }
            }
            #endregion

            return mySchIdenty;
        }

        /// <summary>
        /// 顯示單筆明細資料
        /// </summary>
        /// <param name="config"></param>
        private void BindEditData(ReceiveConfigEntity config, string openStudentArea, string flowKind, string keepDataYear, string keepHistoryYear)
        {
            this.tbxGiree11.Text = config.Giree11 == null ? String.Empty : config.Giree11.Trim();
            this.tbxGiree12.Text = config.Giree12 == null ? String.Empty : config.Giree12.Trim();
            this.tbxGiree21.Text = config.Giree21 == null ? String.Empty : config.Giree21.Trim();
            this.tbxGiree22.Text = config.Giree22 == null ? String.Empty : config.Giree22.Trim();
            this.tbxGiree31.Text = config.Giree31 == null ? String.Empty : config.Giree31.Trim();
            this.tbxGiree32.Text = config.Giree32 == null ? String.Empty : config.Giree32.Trim();
            this.tbxGiree41.Text = config.Giree41 == null ? String.Empty : config.Giree41.Trim();
            this.tbxGiree42.Text = config.Giree42 == null ? String.Empty : config.Giree42.Trim();
            this.tbxGiree51.Text = config.Giree51 == null ? String.Empty : config.Giree51.Trim();
            this.tbxGiree52.Text = config.Giree52 == null ? String.Empty : config.Giree52.Trim();
            this.tbxGiree61.Text = config.Giree61 == null ? String.Empty : config.Giree61.Trim();
            this.tbxGiree62.Text = config.Giree62 == null ? String.Empty : config.Giree62.Trim();

            WebHelper.SetDropDownListSelectedValue(this.ddlYearId, config.DefaultYearId);

            this.GetAndBindTermIdOptions(config.ReceiveType, this.ddlYearId.SelectedValue);
            WebHelper.SetDropDownListSelectedValue(this.ddlTermId, config.DefaultTermId);

            WebHelper.SetRadioButtonListSelectedValue(this.rdoOpenStudentArea, openStudentArea);
            WebHelper.SetRadioButtonListSelectedValue(this.rdoLoginType, config.CheckBirthday);

            #region [MDY:20160201] 審核階層
            WebHelper.SetDropDownListSelectedValue(this.ddlFlowKind, flowKind);
            #endregion

            #region [MDY:2018xxxx] 獨立成 S5600018 功能，所以 MARK
            //#region [MDY:20160206] 線上資料保留年數、歷史資料保留年數
            //WebHelper.SetDropDownListSelectedValue(this.ddlKeepDataYear, keepDataYear);
            //WebHelper.SetDropDownListSelectedValue(this.ddlKeepHistoryYear, keepHistoryYear);
            //#endregion
            #endregion
        }

        private ReceiveConfigEntity GetEditReceiveConfig(string receiveType)
        {
            ReceiveConfigEntity data = null;

            string action = this.GetLocalized("讀取商家代號(學校)設定資料");

            Expression where = new Expression(ReceiveConfigEntity.Field.ReceiveType, this.QueryReceiveType);
            XmlResult result = DataProxy.Current.SelectFirst<ReceiveConfigEntity>(this, where, null, out data);
            if (!result.IsSuccess)
            {
                this.ShowActionFailureMessage(action, result.Code, result.Message);
                return null;
            }
            if (data == null)
            {
                //this.ShowErrorMessage(ErrorCode.D_DATA_NOT_FOUND, "無該商家代號(學校)設定資料");
                return null;
            }

            return data;
        }

        protected ReceiveConfigEntity GetInputData()
        {
            ReceiveConfigEntity entity = new ReceiveConfigEntity();
            entity.ReceiveType = this.QueryReceiveType;
            entity.DefaultYearId = ddlYearId.SelectedValue;
            entity.DefaultTermId = ddlTermId.SelectedValue;
            entity.CheckBirthday = this.rdoLoginType.SelectedValue;
            entity.Giree11 = this.tbxGiree11.Text.Trim();
            entity.Giree12 = this.tbxGiree12.Text.Trim();
            entity.Giree21 = this.tbxGiree21.Text.Trim();
            entity.Giree22 = this.tbxGiree22.Text.Trim();
            entity.Giree31 = this.tbxGiree31.Text.Trim();
            entity.Giree32 = this.tbxGiree32.Text.Trim();
            entity.Giree41 = this.tbxGiree41.Text.Trim();
            entity.Giree42 = this.tbxGiree42.Text.Trim();
            entity.Giree51 = this.tbxGiree51.Text.Trim();
            entity.Giree52 = this.tbxGiree52.Text.Trim();
            entity.Giree61 = this.tbxGiree61.Text.Trim();
            entity.Giree62 = this.tbxGiree62.Text.Trim();

            switch (this.Action)
            {
                case "M":   //修改
                    entity.MdyDate = DateTime.Now;
                    entity.MdyUser = this.GetLogonUser().UserId;
                    break;
                case "A":   //新增
                    entity.Status = DataStatusCodeTexts.NORMAL;
                    entity.CrtUser = this.GetLogonUser().UserId;
                    entity.CrtDate = DateTime.Now;
                    break;
            }

            return entity;
        }

        /// <summary>
        /// 檢查資料，以作為使用介面輸入是否正確 (前台用)
        /// </summary>
        /// <returns>傳回錯誤訊息</returns>
        public string CheckDataForInputUI()
        {
            if (ddlYearId.SelectedValue == string.Empty)
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("目前學年");
            }

            if (ddlTermId.SelectedValue == string.Empty)
            {
                return this.GetLocalized("請輸入") + this.GetLocalized("目前學期");
            }

            #region [MDY:20160201] 審核階層
            {
                string flowKind = this.ddlFlowKind.SelectedValue;
                if (String.IsNullOrEmpty(flowKind) || FlowKindCodeTexts.GetCodeText(flowKind) == null)
                {
                    return this.GetLocalized("請選擇") + this.GetLocalized("審核階層");
                }
            }
            #endregion

            #region [MDY:2018xxxx] 獨立成 S5600018 功能，所以 MARK
            //#region [MDY:20160206] 線上資料保留年數、歷史資料保留年數
            //{
            //    int dataYear;
            //    string keepDataYear = this.ddlKeepDataYear.SelectedValue;
            //    if (String.IsNullOrEmpty(keepDataYear) || !Int32.TryParse(keepDataYear, out dataYear) || dataYear < 0 || dataYear > SchoolRTypeEntity.MaxKeepDataYear)
            //    {
            //        return this.GetLocalized("請選擇") + this.GetLocalized("線上資料保留年數");
            //    }

            //    int historyYear;
            //    string keepHistoryYear = this.ddlKeepHistoryYear.SelectedValue;
            //    if (String.IsNullOrEmpty(keepHistoryYear) || !Int32.TryParse(keepHistoryYear, out historyYear) || historyYear < 0 || historyYear > SchoolRTypeEntity.MaxKeepHistoryYear)
            //    {
            //        return this.GetLocalized("請選擇") + this.GetLocalized("歷史資料保留年數");
            //    }

            //    if (dataYear > 0 && historyYear > 0 && dataYear >= historyYear)
            //    {
            //        return this.GetLocalized("歷史資料保留年數不可以小於線上資料保留年數");
            //    }
            //}
            //#endregion
            #endregion

            return String.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 處理五個下拉選項
                string receiveType = null;
                string yearID = null;
                string termID = null;
                string depID = null;
                string receiveID = null;
                if (!WebHelper.GetFilterArguments(out receiveType, out yearID, out termID, out depID, out receiveID)
                    || String.IsNullOrEmpty(receiveType))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("無法取得商家代號參數");
                    this.ShowJsAlert(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                this.QueryReceiveType = receiveType;

                this.InitialUI();
            }
        }

        protected void ucFilter1_ItemSelectedIndexChanged(object sender, FilterEventArgs e)
        {
            this.QueryReceiveType = ucFilter1.SelectedReceiveType;

            this.InitialUI();
        }

        protected void ddlYearId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetAndBindTermIdOptions(this.QueryReceiveType, this.ddlYearId.SelectedValue);
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

            ReceiveConfigEntity entity = this.GetInputData();

            bool hasError = false;
            List<string> msgs = new List<string>(3);

            #region ReceiveConfigEntity
            {
                switch (this.Action)
                {
                    case "M":   //修改
                        #region 修改
                        {
                            #region 更新條件
                            Expression where = new Expression(ReceiveConfigEntity.Field.ReceiveType, entity.ReceiveType);
                            #endregion

                            #region 更新欄位
                            KeyValueList fieldValues = new KeyValueList();
                            fieldValues.Add(ReceiveConfigEntity.Field.DefaultYearId, entity.DefaultYearId);
                            fieldValues.Add(ReceiveConfigEntity.Field.DefaultTermId, entity.DefaultTermId);
                            fieldValues.Add(ReceiveConfigEntity.Field.CheckBirthday, entity.CheckBirthday);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree11, entity.Giree11);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree12, entity.Giree12);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree21, entity.Giree21);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree22, entity.Giree22);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree31, entity.Giree31);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree32, entity.Giree32);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree41, entity.Giree41);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree42, entity.Giree42);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree51, entity.Giree51);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree52, entity.Giree52);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree61, entity.Giree61);
                            fieldValues.Add(ReceiveConfigEntity.Field.Giree62, entity.Giree62);
                            fieldValues.Add(ReceiveConfigEntity.Field.MdyUser, this.GetLogonUser().UserId);
                            fieldValues.Add(ReceiveConfigEntity.Field.MdyDate, DateTime.Now);
                            #endregion

                            int count = 0;
                            XmlResult xmlResult = DataProxy.Current.UpdateFields<ReceiveConfigEntity>(this, where, fieldValues, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    #region [Old] 後面還有資料要處理，所以改成收集執行結果
                                    //string msg = String.Format("{0}，{1}", this.GetLocalized("更新學校設定資料失敗"), this.GetLocalized("無資料被更新"));
                                    //this.ShowSystemMessage(msg);
                                    #endregion

                                    msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新學校設定資料失敗"), this.GetLocalized("無資料被更新")));
                                    hasError = true;
                                }
                                else
                                {
                                    #region [Old] 後面還有資料要處理，所以改成收集執行結果
                                    //string msg = this.GetLocalized("更新學校設定資料成功");
                                    //this.ShowSystemMessage(msg);
                                    #endregion

                                    msgs.Add(this.GetLocalized("更新學校設定資料成功"));
                                }
                            }
                            else
                            {
                                #region [Old] 後面還有資料要處理，所以改成收集執行結果
                                //this.ShowSystemMessage(xmlResult.Message);
                                #endregion

                                msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新學校設定資料失敗"), xmlResult.Message));
                                hasError = true;
                            }
                        }
                        #endregion
                        break;
                    case "A":   //新增
                        #region 新增
                        {
                            int count = 0;

                            XmlResult xmlResult = DataProxy.Current.Insert<ReceiveConfigEntity>(this, entity, out count);
                            if (xmlResult.IsSuccess)
                            {
                                if (count < 1)
                                {
                                    #region [Old] 後面還有資料要處理，所以改成收集執行結果
                                    //string msg = String.Format("{0}，{1}", this.GetLocalized("新增學校設定資料失敗"), this.GetLocalized("無資料被新增"));
                                    //this.ShowSystemMessage(msg);
                                    #endregion

                                    msgs.Add(String.Format("{0}，{1}", this.GetLocalized("新增學校設定資料失敗"), this.GetLocalized("無資料被更新")));
                                    hasError = true;
                                }
                                else
                                {
                                    #region [Old] 後面還有資料要處理，所以改成收集執行結果
                                    //string msg = this.GetLocalized("新增學校設定資料成功");
                                    //this.ShowSystemMessage(msg);
                                    #endregion

                                    msgs.Add(this.GetLocalized("新增學校設定資料成功"));
                                }
                            }
                            else
                            {
                                #region [Old] 後面還有資料要處理，所以改成收集執行結果
                                //this.ShowSystemMessage(result.Message);
                                #endregion

                                msgs.Add(String.Format("{0}，{1}", this.GetLocalized("新增學校設定資料失敗"), xmlResult.Message));
                                hasError = true;
                            }
                        }
                        #endregion
                        break;
                }
            }
            #endregion

            string mySchIdenty = this.EditMySchIdenty;
            string[] myReceiveTypes = this.EditMyReceiveTypes;

            #region 更新該學校所有的 ReceiveConfigEntity.Field.CheckBirthday
            if (myReceiveTypes != null && myReceiveTypes.Length > 1)
            {
                Expression where = new Expression(ReceiveConfigEntity.Field.ReceiveType, myReceiveTypes);
                KeyValueList fieldValues = new KeyValueList();
                fieldValues.Add(ReceiveConfigEntity.Field.CheckBirthday, entity.CheckBirthday);
                int count = 0;
                XmlResult xmlResult = DataProxy.Current.UpdateFields<ReceiveConfigEntity>(this, where, fieldValues, out count);
                if (xmlResult.IsSuccess)
                {
                    if (count < 1)
                    {
                        msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新其他商家代號的學生登入依據資料失敗"), this.GetLocalized("無資料被更新")));
                        hasError = true;
                    }
                    else
                    {
                        msgs.Add(this.GetLocalized("更新其他商家代號的學生登入依據資料成功"));
                    }
                }
                else
                {
                    msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新其他商家代號的學生登入依據資料失敗"), xmlResult.Message));
                    hasError = true;
                }
            }
            #endregion

            #region 更新該學校所有的 SchoolRType.OpenStudentArea、SchoolRType.FlowKind
            if (!string.IsNullOrEmpty(mySchIdenty) && myReceiveTypes != null && myReceiveTypes.Length > 0)
            {
                string openStudentArea = this.rdoOpenStudentArea.SelectedValue;
                if (openStudentArea != "N")
                {
                    openStudentArea = "Y";
                }

                #region [MDY:20160201] 審核階層
                string flowKind = this.ddlFlowKind.SelectedValue;
                if (flowKind != FlowKindCodeTexts.MULTI)
                {
                    flowKind = FlowKindCodeTexts.SINGLE;
                }
                #endregion

                #region [MDY:2018xxxx] 獨立成 S5600018 功能，所以 MARK
                //#region [MDY:20160206] 線上資料保留年數、歷史資料保留年數
                //int keepDataYear = 0;
                //int keepHistoryYear = 0;
                //{
                //    int value;
                //    if (Int32.TryParse(this.ddlKeepDataYear.SelectedValue, out value) && value >= 0 && value <= SchoolRTypeEntity.MaxKeepDataYear)
                //    {
                //        keepDataYear = value;
                //    }

                //    if (Int32.TryParse(this.ddlKeepHistoryYear.SelectedValue, out value) && value >= 0 && value <= SchoolRTypeEntity.MaxKeepHistoryYear)
                //    {
                //        keepHistoryYear = value;
                //    }
                //}
                //#endregion
                #endregion

                Expression where = new Expression(SchoolRTypeEntity.Field.SchIdenty, mySchIdenty)
                    .And(SchoolRTypeEntity.Field.ReceiveType, myReceiveTypes);
                KeyValueList fieldValues = new KeyValueList();
                fieldValues.Add(SchoolRTypeEntity.Field.OpenStudentArea, openStudentArea);

                #region [MDY:20160201] 審核階層
                fieldValues.Add(SchoolRTypeEntity.Field.FlowKind, flowKind);
                #endregion

                #region [MDY:2018xxxx] 獨立成 S5600018 功能，所以 MARK
                //#region [MDY:20160206] 線上資料保留年數、歷史資料保留年數
                //fieldValues.Add(SchoolRTypeEntity.Field.KeepDataYear, keepDataYear);
                //fieldValues.Add(SchoolRTypeEntity.Field.KeepHistoryYear, keepHistoryYear);
                //#endregion
                #endregion

                int count = 0;
                XmlResult xmlResult = DataProxy.Current.UpdateFields<SchoolRTypeEntity>(this, where, fieldValues, out count);

                #region [MDY:2018xxxx] 獨立成 S5600018 功能
                #region [OLD]
                //if (xmlResult.IsSuccess)
                //{
                //    if (count < 1)
                //    {
                //        msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新是否開放學生專區、審核階層、線上資料保留年數、歷史資料保留年數資料失敗"), this.GetLocalized("無資料被更新")));
                //        hasError = true;
                //    }
                //    else
                //    {
                //        msgs.Add(this.GetLocalized("更新是否開放學生專區、審核階層、線上資料保留年數、歷史資料保留年數資料成功"));
                //    }
                //}
                //else
                //{
                //    msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新是否開放學生專區、審核階層、線上資料保留年數、歷史資料保留年數資料失敗"), xmlResult.Message));
                //    hasError = true;
                //}
                #endregion

                if (xmlResult.IsSuccess)
                {
                    if (count < 1)
                    {
                        msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新是否開放學生專區、審核階層資料失敗"), this.GetLocalized("無資料被更新")));
                        hasError = true;
                    }
                    else
                    {
                        msgs.Add(this.GetLocalized("更新是否開放學生專區、審核階層資料成功"));
                    }
                }
                else
                {
                    msgs.Add(String.Format("{0}，{1}", this.GetLocalized("更新是否開放學生專區、審核階層資料失敗"), xmlResult.Message));
                    hasError = true;
                }
                #endregion
            }
            #endregion

            if (hasError)
            {
                string msg = String.Join("<br/>", msgs.ToArray());
                this.ShowSystemMessage(msg, false);
                this.ShowJsAlert("ccbtnOK_Click", "部分資料更新失敗");
            }
            else
            {
                this.ShowSystemMessage("所有資料更新成功");
            }
        }
    }
}