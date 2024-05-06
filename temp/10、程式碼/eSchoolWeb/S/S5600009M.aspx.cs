using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fuju;
using Fuju.DB;
using Fuju.Web;

using Entities;
using Helpers;

namespace eSchoolWeb.S
{
    /// <summary>
    /// 排程作業管理 (維護)
    /// </summary>
    public partial class S5600009M : BasePage
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
        /// 編輯的作業類別代碼參數
        /// </summary>
        private string EditJobCubeType
        {
            get
            {
                return ViewState["EditJobCubeType"] as string;
            }
            set
            {
                ViewState["EditJobCubeType"] = value == null ? null : value.Trim();
            }
        }
        #endregion

        #region [MDY:2018xxxx] 改用 ServiceConfig2 物件，所以調整週期相關輸入項目
        #region [OLD]
        ///// <summary>
        ///// 初始化使用介面
        ///// </summary>
        //private void InitialUI()
        //{
        //    #region 服務名稱
        //    {
        //        #region [MDY:20160413] 排序
        //        CodeTextList items = JobCubeTypeCodeTexts.GetNonRealTimeJob();
        //        items.Sort(delegate(CodeText x, CodeText y)
        //        {
        //            return x.Code.CompareTo(y.Code);
        //        });
        //        #endregion

        //        WebHelper.SetDropDownListItems(this.ddlJobCubeType, DefaultItem.Kind.Select, false, items, true, true, 0, null);
        //    }
        //    #endregion

        //    #region 啟動週期
        //    {
        //        CodeTextList items = new CodeTextList();
        //        items.Add(ServiceCycleUnit.Minute.ToString(), "分鐘");
        //        items.Add(ServiceCycleUnit.Day.ToString(), "天");
        //        WebHelper.SetDropDownListItems(this.ddlCycleUnit, DefaultItem.Kind.Select, false, items, false, true, 0, null);
        //    }
        //    #endregion

        //    this.tbxCycleValue.Text = String.Empty;
        //    this.tbxStartTime.Text = String.Empty;
        //    this.TextEndTime.Text = String.Empty;
        //    this.tbxFileName.Text = String.Empty;
        //    this.tbxArguments.Text = String.Empty;
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 結繫維護資料
        ///// </summary>
        ///// <param name="data">維護資料</param>
        //private void BindEditData(ServiceConfig data)
        //{
        //    if (data == null)
        //    {
        //        this.ddlCycleUnit.SelectedIndex = -1;

        //        this.tbxCycleValue.Text = String.Empty;
        //        this.tbxStartTime.Text = String.Empty;
        //        this.TextEndTime.Text = String.Empty;
        //        this.tbxFileName.Text = String.Empty;
        //        this.tbxArguments.Text = String.Empty;
        //        this.ccbtnOK.Visible = false;
        //        return;
        //    }

        //    bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
        //    this.ddlJobCubeType.Enabled = isPKeyEditable;

        //    bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);
        //    this.ddlCycleUnit.Enabled = isDataEditable;
        //    this.tbxCycleValue.Enabled = isDataEditable;
        //    this.tbxStartTime.Enabled = isDataEditable;
        //    this.TextEndTime.Enabled = isDataEditable;
        //    this.tbxFileName.Enabled = isDataEditable;
        //    this.tbxArguments.Enabled = isDataEditable;

        //    WebHelper.SetDropDownListSelectedValue(this.ddlJobCubeType, data.JobCubeType);
        //    WebHelper.SetDropDownListSelectedValue(this.ddlCycleUnit, data.CycleUnit.ToString());
        //    this.tbxCycleValue.Text = data.CycleValue.ToString();
        //    this.tbxStartTime.Text = data.CycleStartTime.ToString();
        //    this.TextEndTime.Text = data.CycleEndTime.ToString();
        //    this.tbxFileName.Text = data.AppFileName;
        //    this.tbxArguments.Text = data.AppArguments;
        //    this.ccbtnOK.Visible = true;
        //}

        ///// <summary>
        ///// 取得輸入的維護資料
        ///// </summary>
        ///// <param name="data">傳回輸入的維護資料</param>
        ///// <returns>輸入資料正確則傳回 true，否則傳回 false</returns>
        //private bool GetAndCheckEditData(out ServiceConfig data)
        //{
        //    data = new ServiceConfig();
        //    data.Enabled = true;

        //    #region 服務名稱
        //    switch (this.Action)
        //    {
        //        case ActionMode.Insert:     //新增
        //            data.JobCubeType = this.ddlJobCubeType.SelectedValue;
        //            if (String.IsNullOrEmpty(data.JobCubeType))
        //            {
        //                this.ShowMustInputAlert("服務名稱");
        //                return false;
        //            }
        //            break;
        //        case ActionMode.Modify:     //修改
        //        case ActionMode.Delete:     //刪除
        //            data.JobCubeType = this.EditJobCubeType;
        //            break;
        //    }
        //    #endregion

        //    #region 啟動週期的單位
        //    {
        //        string txt = this.ddlCycleUnit.SelectedValue;
        //        ServiceCycleUnit cycleUnit = ServiceCycleUnit.Empty;
        //        if (!String.IsNullOrEmpty(txt) && Enum.TryParse<ServiceCycleUnit>(txt, out cycleUnit))
        //        {
        //            data.CycleUnit = cycleUnit;
        //        }
        //        else
        //        {
        //            data.CycleUnit = ServiceCycleUnit.Empty;
        //            this.ShowMustInputAlert("啟動週期的單位");
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 啟動週期的值
        //    {
        //        string txt = this.tbxCycleValue.Text.Trim();
        //        if (String.IsNullOrEmpty(txt))
        //        {
        //            this.ShowMustInputAlert("啟動週期的值");
        //            return false;
        //        }
        //        int value;
        //        if (Common.IsInt32(txt, 1, 999, out value))
        //        {
        //            data.CycleValue = value;
        //        }
        //        else
        //        {
        //            data.CycleValue = 0;
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("啟動週期的值限輸入1~999的數值");
        //            this.ShowJsAlert(msg);
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 啟動時間的起始時間
        //    {
        //        string txt = this.tbxStartTime.Text.Trim();
        //        if (String.IsNullOrEmpty(txt))
        //        {
        //            this.ShowMustInputAlert("啟動時間的起始時間");
        //            return false;
        //        }
        //        ServiceCycleTime value;
        //        if (ServiceCycleTime.TryParse(txt, out value))
        //        {
        //            data.CycleStartTime = value;
        //        }
        //        else
        //        {
        //            data.CycleStartTime = ServiceCycleTime.Zero;
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("啟動時間的起始時間不是有效的時間格式 (HH:mm)");
        //            this.ShowJsAlert(msg);
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 啟動時間的結束時間
        //    if (data.CycleUnit == ServiceCycleUnit.Day)
        //    {
        //        data.CycleEndTime = ServiceCycleTime.Zero;
        //    }
        //    else
        //    {
        //        string txt = this.TextEndTime.Text.Trim();
        //        if (String.IsNullOrEmpty(txt))
        //        {
        //            this.ShowMustInputAlert("啟動時間的結束時間");
        //            return false;
        //        }
        //        ServiceCycleTime value;
        //        if (ServiceCycleTime.TryParse(txt, out value))
        //        {
        //            data.CycleEndTime = value;
        //        }
        //        else
        //        {
        //            data.CycleEndTime = ServiceCycleTime.Zero;
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("啟動時間的結束時間不是有效的時間格式 (HH:mm)");
        //            this.ShowJsAlert(msg);
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 應用程式路徑檔名
        //    {
        //        data.AppFileName = this.tbxFileName.Text.Trim();
        //        if (String.IsNullOrEmpty(data.AppFileName))
        //        {
        //            this.ShowMustInputAlert("應用程式路徑檔名");
        //            return false;
        //        }
        //    }
        //    #endregion

        //    #region 應用程式命令參數
        //    data.AppArguments = this.tbxArguments.Text.Trim();
        //    #endregion

        //    return true;
        //}

        ///// <summary>
        ///// 取得 Config 中的 ServiceConfig 資料
        ///// </summary>
        ///// <param name="datas">傳回 ServiceConfig 資料陣列</param>
        ///// <param name="datas">傳回是否有 Config 設定資料</param>
        ///// <returns>傳回處理結果</returns>
        //private XmlResult GetServiceConfigs(out ServiceConfig[] datas, out bool hasConfig)
        //{
        //    datas = null;

        //    ConfigEntity config = null;
        //    Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SERVICE_CONFIG);
        //    XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this, where, null, out config);
        //    if (xmlResult.IsSuccess && config != null && !String.IsNullOrEmpty(config.ConfigValue))
        //    {
        //        ServiceConfigHelper helper = new ServiceConfigHelper();
        //        if (!helper.DeXmlString(config.ConfigValue, out datas))
        //        {
        //            xmlResult = new XmlResult(false, "服務項目設定解析失敗", CoreStatusCode.UNKNOWN_ERROR, null);
        //        }
        //    }
        //    hasConfig = config != null;
        //    return xmlResult;
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        this.InitialUI();

        //        #region 檢查維護權限
        //        if (!this.HasMaintainAuth())
        //        {
        //            this.ShowErrorMessage(ErrorCode.S_NO_AUTHORIZE_FOR_MAINTAIN, "無維護權限");
        //            return;
        //        }
        //        #endregion

        //        #region 處理參數
        //        KeyValueList<string> QueryString = Session["QueryString"] as KeyValueList<string>;
        //        if (QueryString == null || QueryString.Count == 0)
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("缺少網頁參數");
        //            this.ShowSystemMessage(msg);
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }

        //        this.Action = QueryString.TryGetValue("Action", String.Empty);
        //        this.EditJobCubeType = QueryString.TryGetValue("JobCubeType", String.Empty);

        //        if (!ActionMode.IsMaintinaMode(this.Action)
        //            || (!ActionMode.IsPKeyEditableMode(this.Action) && String.IsNullOrEmpty(this.EditJobCubeType)))
        //        {
        //            //[TODO] 固定顯示訊息的收集
        //            string msg = this.GetLocalized("網頁參數不正確");
        //            this.ShowSystemMessage(msg);
        //            this.ccbtnOK.Visible = false;
        //            return;
        //        }
        //        #endregion

        //        #region 取得維護資料
        //        ServiceConfig data = null;
        //        switch (this.Action)
        //        {
        //            case ActionMode.Insert:   //新增
        //                #region 新增
        //                {
        //                    //空的資料
        //                    data = new ServiceConfig();
        //                }
        //                #endregion
        //                break;
        //            case ActionMode.Modify:   //修改
        //            case ActionMode.Delete:   //刪除
        //                #region 修改 | 刪除
        //                {
        //                    string action = this.GetLocalized("查詢要維護的資料");

        //                    #region 查詢資料
        //                    {
        //                        ServiceConfig[] serviceConfigs = null;
        //                        bool hasConfig = false;
        //                        XmlResult xmlResult = this.GetServiceConfigs(out serviceConfigs, out hasConfig);
        //                        if (xmlResult.IsSuccess && serviceConfigs != null && serviceConfigs.Length > 0)
        //                        {
        //                            foreach (ServiceConfig serviceConfig in serviceConfigs)
        //                            {
        //                                if (serviceConfig.JobCubeType == this.EditJobCubeType)
        //                                {
        //                                    data = serviceConfig;
        //                                    break;
        //                                }
        //                            }
        //                        }

        //                        if (!xmlResult.IsSuccess)
        //                        {
        //                            this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                            this.ccbtnOK.Visible = false;
        //                            return;
        //                        }
        //                        if (data == null)
        //                        {
        //                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                            this.ccbtnOK.Visible = false;
        //                            return;
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                #endregion
        //                break;
        //        }
        //        #endregion

        //        this.BindEditData(data);
        //    }
        //}

        //protected void ccbtnOK_Click(object sender, EventArgs e)
        //{
        //    ServiceConfig serviceConfig = null;
        //    if (!this.GetAndCheckEditData(out serviceConfig))
        //    {
        //        return;
        //    }

        //    #region 取得 ConfigEntity
        //    List<ServiceConfig> serviceConfigs = null;
        //    bool hasConfig = false;
        //    {
        //        ServiceConfig[] datas = null;
        //        XmlResult xmlResult = this.GetServiceConfigs(out datas, out hasConfig);
        //        if (!xmlResult.IsSuccess)
        //        {
        //            string action2 = this.GetLocalized("讀取服務項目設定資料");
        //            this.ShowActionFailureMessage(action2, xmlResult.Code, xmlResult.Message);
        //            return;
        //        }
        //        if (datas == null || datas.Length == 0)
        //        {
        //            serviceConfigs = new List<ServiceConfig>(1);
        //        }
        //        else
        //        {
        //            serviceConfigs = new List<ServiceConfig>(datas);
        //        }
        //    }
        //    #endregion

        //    string action = ActionMode.GetActionLocalized(this.Action);
        //    string backUrl = "S5600009.aspx";
        //    switch (this.Action)
        //    {
        //        case ActionMode.Insert:     //新增
        //            #region 新增
        //            {
        //                foreach (ServiceConfig one in serviceConfigs)
        //                {
        //                    if (serviceConfig.JobCubeType == one.JobCubeType)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
        //                        return;
        //                    }
        //                }
        //                serviceConfigs.Add(serviceConfig);

        //                ServiceConfigHelper helper = new ServiceConfigHelper();
        //                ConfigEntity data = new ConfigEntity();
        //                data.ConfigKey = ConfigKeyCodeTexts.SERVICE_CONFIG;
        //                data.ConfigValue = helper.ToXmlString(serviceConfigs);

        //                int count = 0;
        //                XmlResult xmlResult = null;
        //                if (hasConfig)
        //                {
        //                    xmlResult = DataProxy.Current.Update<ConfigEntity>(this, data, out count);
        //                }
        //                else
        //                {
        //                    xmlResult = DataProxy.Current.Insert<ConfigEntity>(this, data, out count);
        //                }

        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        if (hasConfig)
        //                        {
        //                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "無法儲存服務項目設定資料");
        //                        }
        //                        else
        //                        {
        //                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "無法儲存服務項目設定資料");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //        case ActionMode.Modify:     //修改
        //            #region 修改
        //            {
        //                if (!hasConfig)
        //                {
        //                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    return;
        //                }
        //                bool isExist = false;
        //                for(int idx = 0; idx < serviceConfigs.Count; idx++)
        //                {
        //                    if (serviceConfigs[idx].JobCubeType == serviceConfig.JobCubeType)
        //                    {
        //                        serviceConfigs[idx] = serviceConfig;
        //                        isExist = true;
        //                        break;
        //                    }
        //                }
        //                if (!isExist)
        //                {
        //                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    return;
        //                }

        //                ServiceConfigHelper helper = new ServiceConfigHelper();
        //                ConfigEntity data = new ConfigEntity();
        //                data.ConfigKey = ConfigKeyCodeTexts.SERVICE_CONFIG;
        //                data.ConfigValue = helper.ToXmlString(serviceConfigs);

        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.Update<ConfigEntity>(this, data, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "無法儲存服務項目設定資料");
        //                    }
        //                    else
        //                    {
        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //        case ActionMode.Delete:     //刪除
        //            #region 刪除
        //            {
        //                if (!hasConfig)
        //                {
        //                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    return;
        //                }

        //                int removeIndex = -1;
        //                for (int idx = 0; idx < serviceConfigs.Count; idx++)
        //                {
        //                    if (serviceConfigs[idx].JobCubeType == serviceConfig.JobCubeType)
        //                    {
        //                        removeIndex = idx;
        //                        break;
        //                    }
        //                }
        //                if (removeIndex > -1)
        //                {
        //                    serviceConfigs.RemoveAt(removeIndex);
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
        //                    return;
        //                }

        //                ServiceConfigHelper helper = new ServiceConfigHelper();
        //                ConfigEntity data = new ConfigEntity();
        //                data.ConfigKey = ConfigKeyCodeTexts.SERVICE_CONFIG;
        //                data.ConfigValue = helper.ToXmlString(serviceConfigs);

        //                int count = 0;
        //                XmlResult xmlResult = DataProxy.Current.Update<ConfigEntity>(this, data, out count);
        //                if (xmlResult.IsSuccess)
        //                {
        //                    if (count < 1)
        //                    {
        //                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "無法儲存服務項目設定資料");
        //                    }
        //                    else
        //                    {
        //                        this.ShowActionSuccessAlert(action, backUrl);
        //                    }
        //                }
        //                else
        //                {
        //                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
        //                }
        //            }
        //            #endregion
        //            break;
        //    }
        //}
        #endregion

        /// <summary>
        /// 初始化使用介面
        /// </summary>
        private void InitialUI()
        {
            #region 服務名稱
            {
                #region [MDY:20160413] 排序
                CodeTextList items = JobCubeTypeCodeTexts.GetNonRealTimeJob();
                items.Sort(delegate(CodeText x, CodeText y)
                {
                    return x.Code.CompareTo(y.Code);
                });
                #endregion

                WebHelper.SetDropDownListItems(this.ddlJobCubeType, DefaultItem.Kind.Select, false, items, true, true, 0, null);
            }
            #endregion

            #region 啟動週期間隔
            this.tbxStartDate.Text = String.Empty;
            this.tbxEndDate.Text = String.Empty;
            this.tbxDaysInterval.Text = String.Empty;
            #endregion

            #region 啟動時間間隔
            this.tbxStartTime.Text = String.Empty;
            this.tbxEndTime.Text = String.Empty;
            this.tbxTimeInterval.Text = String.Empty;
            #endregion

            #region 應用程式路徑檔名 & 應用程式命令參數
            this.tbxFileName.Text = String.Empty;
            this.tbxArguments.Text = String.Empty;
            #endregion

            #region 狀態
            {
                CodeText[] items = new CodeText[] { new CodeText("Y", "啟用"), new CodeText("N", "停用") };
                WebHelper.SetDropDownListItems(this.ddlStatus, DefaultItem.Kind.Select, false, items, false, true, 0, null);
            }
            #endregion

            this.ccbtnOK.Visible = false;
        }

        /// <summary>
        /// 結繫編輯資料
        /// </summary>
        /// <param name="data">編輯資料</param>
        private void BindEditData(ServiceConfig2 data)
        {
            #region 處理控制項的 Enabled
            #region PKey 欄位
            bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
            this.ddlJobCubeType.Enabled = isPKeyEditable;
            #endregion

            #region Data 欄位
            bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);
            this.tbxStartDate.Enabled = isDataEditable;
            this.tbxEndDate.Enabled = isDataEditable;
            this.tbxDaysInterval.Enabled = isDataEditable;
            this.tbxStartTime.Enabled = isDataEditable;
            this.tbxEndTime.Enabled = isDataEditable;
            this.tbxTimeInterval.Enabled = isDataEditable;
            this.tbxFileName.Enabled = isDataEditable;
            this.tbxArguments.Enabled = isDataEditable;
            this.ddlStatus.Enabled = isDataEditable;
            #endregion
            #endregion

            if (data == null)
            {
                data = new ServiceConfig2();
            }

            #region 服務名稱
            WebHelper.SetDropDownListSelectedValue(this.ddlJobCubeType, data.JobCubeType);
            #endregion

            #region 啟動週期間隔
            DaysCycle cycle = data.Cycle;
            if (cycle == null)
            {
                this.tbxStartDate.Text = String.Empty;
                this.tbxEndDate.Text = String.Empty;
                this.tbxDaysInterval.Text = String.Empty;
            }
            else
            {
                this.tbxStartDate.Text = cycle.StartDate == null ? String.Empty : cycle.StartDate.Value.ToString("yyyy/MM/dd");
                this.tbxEndDate.Text = cycle.EndDate == null ? String.Empty : cycle.EndDate.Value.ToString("yyyy/MM/dd");
                this.tbxDaysInterval.Text = cycle.DaysInterval.ToString();
            }
            #endregion

            #region 啟動時間間隔
            CycleTime cycleTime = cycle == null ? null : cycle.CycleTime;
            if (cycleTime == null)
            {
                this.tbxStartTime.Text = String.Empty;
                this.tbxEndTime.Text = String.Empty;
                this.tbxTimeInterval.Text = String.Empty;
            }
            else
            {
                this.tbxStartTime.Text = cycleTime.StartTime.ToString();
                this.tbxEndTime.Text = cycleTime.EndTime.ToString();
                this.tbxTimeInterval.Text = cycleTime.Interval.ToString();
            }
            #endregion

            #region 應用程式路徑檔名 & 應用程式命令參數
            this.tbxFileName.Text = data.AppFileName;
            this.tbxArguments.Text = data.AppArguments;
            #endregion

            #region 狀態
            WebHelper.SetDropDownListSelectedValue(this.ddlStatus, (data.Enabled ? "Y" : "N"));
            #endregion

            this.ccbtnOK.Visible = true;
        }

        /// <summary>
        /// 取得並檢查輸入的維護資料
        /// </summary>
        /// <param name="data">傳回輸入的維護資料</param>
        /// <returns>輸入資料正確則傳回 true，否則傳回 false</returns>
        private bool GetAndCheckEditData(out ServiceConfig2 data)
        {
            data = new ServiceConfig2();

            #region PKey 欄位
            bool isPKeyEditable = ActionMode.IsPKeyEditableMode(this.Action);
            #region 服務名稱
            if (isPKeyEditable)
            {
                data.JobCubeType = this.ddlJobCubeType.SelectedValue;
            }
            else
            {
                data.JobCubeType = this.EditJobCubeType;
            }
            if (String.IsNullOrEmpty(data.JobCubeType))
            {
                this.ShowMustInputAlert("服務名稱");
                return false;
            }
            #endregion
            #endregion

            #region Data 欄位
            bool isDataEditable = ActionMode.IsDataEditableMode(this.Action);
            if (isDataEditable)
            {
                DaysCycle cycle = data.Cycle = new DaysCycle();

                #region 啟動週期間隔
                {
                    #region 起始日期
                    {
                        string txt = this.tbxStartDate.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date))
                            {
                                cycle.StartDate = date;
                            }
                            else
                            {
                                string msg = this.GetLocalized("啟動週期間隔的起始日期不正確，必須為 YYYY/MM/DD 格式的西元年月日");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.ShowMustInputAlert("啟動週期間隔的起始日期");
                            return false;
                        }
                    }
                    #endregion

                    #region 結束日期
                    {
                        string txt = this.tbxEndDate.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            DateTime date;
                            if (DateTime.TryParse(txt, out date))
                            {
                                cycle.EndDate = date;
                            }
                            else
                            {
                                string msg = this.GetLocalized("啟動週期間隔的結束日期不正確，必須為 YYYY/MM/DD 格式的西元年月日");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                    }
                    #endregion

                    #region 間隔日數
                    {
                        string txt = this.tbxDaysInterval.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            Int32 value;
                            if (Int32.TryParse(txt, out value) && value >= DaysCycle.MinInterval && value <= DaysCycle.MaxInterval)
                            {
                                cycle.DaysInterval = value;
                            }
                            else
                            {
                                string msg = this.GetLocalized(String.Format("啟動週期間隔的間隔日數不正確，必須為 {0} 至 {1} 的整數", DaysCycle.MinInterval, DaysCycle.MaxInterval));
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.ShowMustInputAlert("啟動週期間隔的間隔日數");
                            return false;
                        }
                    }
                    #endregion

                    #region 檢查邏輯
                    if (cycle.StartDate != null && cycle.EndDate != null)
                    {
                        if (cycle.EndDate.Value < cycle.StartDate.Value)
                        {
                            string msg = this.GetLocalized("啟動週期間隔的結束日期不可小於起始日期");
                            this.ShowJsAlert(msg);
                            return false;
                        }
                        if (cycle.EndDate.Value != cycle.StartDate.Value && cycle.DaysInterval == 0)
                        {
                            string msg = this.GetLocalized("啟動週期間隔的結束日期不等於起始日期時，間隔日數必須大於 0");
                            this.ShowJsAlert(msg);
                            return false;
                        }
                    }
                    if (cycle.EndDate == null && cycle.DaysInterval == 0)
                    {
                        string msg = this.GetLocalized("啟動週期間隔的結束日期不指定時，間隔日數必須大於 0");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                    #endregion
                }
                #endregion

                #region 啟動時間間隔
                {
                    CycleTime cycleTime = new CycleTime();
                    #region 起始時間
                    {
                        string txt = this.tbxStartTime.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            HourMinute hm;
                            if (HourMinute.TryParse(txt, out hm))
                            {
                                cycleTime.StartTime = hm;
                            }
                            else
                            {
                                string msg = this.GetLocalized("啟動時間間隔的起始時間不正確，必須為 HH:MM 格式的5碼24小時制時分");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.ShowMustInputAlert("啟動時間間隔的起始時間");
                            return false;
                        }
                    }
                    #endregion

                    #region 結束時間
                    {
                        string txt = this.tbxEndTime.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            HourMinute hm;
                            if (HourMinute.TryParse(txt, out hm))
                            {
                                cycleTime.EndTime = hm;
                            }
                            else
                            {
                                string msg = this.GetLocalized("啟動時間間隔的結束時間不正確，必須為 HH:MM 格式的5碼24小時制時分");
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.ShowMustInputAlert("啟動時間間隔的結束時間");
                            return false;
                        }
                    }
                    #endregion

                    #region 間隔分鐘數
                    {
                        string txt = this.tbxTimeInterval.Text.Trim();
                        if (!String.IsNullOrEmpty(txt))
                        {
                            Int32 value;
                            if (Int32.TryParse(txt, out value) && value >= CycleTime.MinInterval && value <= CycleTime.MaxInterval)
                            {
                                cycleTime.Interval = value;
                            }
                            else
                            {
                                string msg = this.GetLocalized(String.Format("啟動時間間隔的間隔分鐘數不正確，必須為 {0} 至 {1} 的整數", CycleTime.MinInterval, CycleTime.MaxInterval));
                                this.ShowJsAlert(msg);
                                return false;
                            }
                        }
                        else
                        {
                            this.ShowMustInputAlert("啟動時間間隔的間隔分鐘數");
                            return false;
                        }
                    }
                    #endregion

                    #region 檢查邏輯
                    if (cycleTime.EndTime < cycleTime.StartTime)
                    {
                        string msg = this.GetLocalized("啟動時間間隔的結束時間不可小於起始時間");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                    if (cycleTime.EndTime != cycleTime.StartTime && cycleTime.Interval == 0)
                    {
                        string msg = this.GetLocalized("啟動時間間隔的結束時間不等於起始時間時，間隔分鐘數必須大於 0");
                        this.ShowJsAlert(msg);
                        return false;
                    }
                    #endregion

                    cycle.CycleTime = cycleTime;
                }
                #endregion

                #region 應用程式路徑檔名 & 應用程式命令參數
                {
                    data.AppFileName = this.tbxFileName.Text.Trim();
                    if (String.IsNullOrEmpty(data.AppFileName))
                    {
                        this.ShowMustInputAlert("應用程式路徑檔名");
                        return false;
                    }

                    data.AppArguments = this.tbxArguments.Text.Trim();
                }
                #endregion

                #region 狀態
                {
                    string txt = this.ddlStatus.SelectedValue;
                    if (!String.IsNullOrEmpty(txt))
                    {
                        data.Enabled = txt.Equals("Y");
                    }
                    else
                    {
                        this.ShowMustInputAlert("狀態");
                        return false;
                    }
                }
                #endregion
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得 Config 中的 ServiceConfig 資料
        /// </summary>
        /// <param name="datas">傳回 ServiceConfig2 資料陣列</param>
        /// <param name="datas">傳回是否有 Config 設定資料</param>
        /// <returns>傳回處理結果</returns>
        private XmlResult GetServiceConfigs(out List<ServiceConfig2> datas, out bool hasConfig)
        {
            datas = null;

            ConfigEntity config = null;
            Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SERVICE_CONFIG);
            XmlResult xmlResult = DataProxy.Current.SelectFirst<ConfigEntity>(this, where, null, out config);
            if (xmlResult.IsSuccess && config != null && !String.IsNullOrEmpty(config.ConfigValue))
            {
                string errmsg = null;
                ServiceConfigHelper helper = new ServiceConfigHelper();
                if (!helper.TryDeXml(config.ConfigValue, out errmsg, out datas))
                {
                    xmlResult = new XmlResult(false, errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                }
            }
            hasConfig = config != null;
            return xmlResult;
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
                this.EditJobCubeType = QueryString.TryGetValue("JobCubeType", String.Empty);

                if (!ActionMode.IsMaintinaMode(this.Action)
                    || (!ActionMode.IsPKeyEditableMode(this.Action) && String.IsNullOrEmpty(this.EditJobCubeType)))
                {
                    //[TODO] 固定顯示訊息的收集
                    string msg = this.GetLocalized("網頁參數不正確");
                    this.ShowSystemMessage(msg);
                    this.ccbtnOK.Visible = false;
                    return;
                }
                #endregion

                #region 取得維護資料
                ServiceConfig2 data = null;
                switch (this.Action)
                {
                    case ActionMode.Insert:   //新增
                        #region 新增
                        {
                            //空的資料
                            data = new ServiceConfig2();
                            data.Cycle = new DaysCycle(DateTime.Today, null, 1, null);
                        }
                        #endregion
                        break;
                    case ActionMode.Modify:   //修改
                    case ActionMode.Delete:   //刪除
                        #region 修改 | 刪除
                        {
                            string action = this.GetLocalized("查詢要維護的資料");

                            #region 查詢資料
                            {
                                List<ServiceConfig2> serviceConfigs = null;
                                bool hasConfig = false;
                                XmlResult xmlResult = this.GetServiceConfigs(out serviceConfigs, out hasConfig);
                                if (xmlResult.IsSuccess)
                                {
                                    if (serviceConfigs != null && serviceConfigs.Count > 0)
                                    {
                                        data = serviceConfigs.Find(x => x.JobCubeType == this.EditJobCubeType);
                                    }
                                    if (data == null)
                                    {
                                        this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "資料不存在");
                                        this.ccbtnOK.Visible = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    this.ShowActionFailureMessage(action, xmlResult.Code, xmlResult.Message);
                                    this.ccbtnOK.Visible = false;
                                    return;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        break;
                }
                #endregion

                this.BindEditData(data);
            }
        }

        protected void ccbtnOK_Click(object sender, EventArgs e)
        {
            #region 取得並檢查輸入的維護資料
            ServiceConfig2 serviceConfig = null;
            if (!this.GetAndCheckEditData(out serviceConfig))
            {
                return;
            }
            #endregion

            #region 取得 ConfigEntity
            List<ServiceConfig2> serviceConfigs = null;
            bool hasConfig = false;
            {
                XmlResult xmlResult = this.GetServiceConfigs(out serviceConfigs, out hasConfig);
                if (xmlResult.IsSuccess)
                {
                    if (serviceConfigs == null)
                    {
                        serviceConfigs = new List<ServiceConfig2>(1);
                    }
                }
                else
                {
                    string action2 = this.GetLocalized("讀取服務項目設定資料");
                    this.ShowActionFailureMessage(action2, xmlResult.Code, xmlResult.Message);
                    return;
                }
            }
            #endregion

            string action = ActionMode.GetActionLocalized(this.Action);
            string backUrl = "S5600009.aspx";
            switch (this.Action)
            {
                case ActionMode.Insert:     //新增
                    #region 新增
                    {
                        if (serviceConfigs.Find(x => x.JobCubeType == serviceConfig.JobCubeType) != null)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "資料已存在");
                            return;
                        }
                        serviceConfigs.Add(serviceConfig);
                        serviceConfigs.Sort(ServiceConfig2.Comparison);

                        ServiceConfigHelper helper = new ServiceConfigHelper();
                        ConfigEntity data = new ConfigEntity();
                        data.ConfigKey = ConfigKeyCodeTexts.SERVICE_CONFIG;
                        data.ConfigValue = helper.TryToXml(serviceConfigs);
                        if (String.IsNullOrEmpty(data.ConfigValue))
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.S_SERIALIZED_FAILURE, "服務項目設定資料 XML 化失敗");
                            return;
                        }

                        int count = 0;
                        XmlResult xmlResult = null;
                        if (hasConfig)
                        {
                            xmlResult = DataProxy.Current.Update<ConfigEntity>(this, data, out count);
                        }
                        else
                        {
                            xmlResult = DataProxy.Current.Insert<ConfigEntity>(this, data, out count);
                        }

                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                if (hasConfig)
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_EXISTS, "無法儲存排程作業資料");
                                }
                                else
                                {
                                    this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "無法儲存排程作業資料");
                                }
                            }
                            else
                            {
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
                        if (!hasConfig)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "排程作業資料不存在");
                            return;
                        }
                        bool isExist = false;
                        for (int idx = 0; idx < serviceConfigs.Count; idx++)
                        {
                            if (serviceConfigs[idx].JobCubeType == serviceConfig.JobCubeType)
                            {
                                serviceConfigs[idx] = serviceConfig;
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "服務項目設定資料不存在");
                            return;
                        }
                        serviceConfigs.Sort(ServiceConfig2.Comparison);

                        ServiceConfigHelper helper = new ServiceConfigHelper();
                        ConfigEntity data = new ConfigEntity();
                        data.ConfigKey = ConfigKeyCodeTexts.SERVICE_CONFIG;
                        data.ConfigValue = helper.TryToXml(serviceConfigs);
                        if (String.IsNullOrEmpty(data.ConfigValue))
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.S_SERIALIZED_FAILURE, "服務項目設定資料 XML 化失敗");
                            return;
                        }

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<ConfigEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "無法儲存排程作業資料");
                            }
                            else
                            {
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
                        if (!hasConfig)
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "排程作業資料不存在");
                            return;
                        }

                        int removeIndex = serviceConfigs.FindIndex(x => x.JobCubeType == serviceConfig.JobCubeType);
                        if (removeIndex > -1)
                        {
                            serviceConfigs.RemoveAt(removeIndex);
                            serviceConfigs.Sort(ServiceConfig2.Comparison);
                        }
                        else
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "服務項目設定資料不存在");
                            return;
                        }

                        ServiceConfigHelper helper = new ServiceConfigHelper();
                        ConfigEntity data = new ConfigEntity();
                        data.ConfigKey = ConfigKeyCodeTexts.SERVICE_CONFIG;
                        data.ConfigValue = helper.TryToXml(serviceConfigs);
                        if (String.IsNullOrEmpty(data.ConfigValue))
                        {
                            this.ShowActionFailureMessage(action, ErrorCode.S_SERIALIZED_FAILURE, "服務項目設定資料 XML 化失敗");
                            return;
                        }

                        int count = 0;
                        XmlResult xmlResult = DataProxy.Current.Update<ConfigEntity>(this, data, out count);
                        if (xmlResult.IsSuccess)
                        {
                            if (count < 1)
                            {
                                this.ShowActionFailureMessage(action, ErrorCode.D_DATA_NOT_FOUND, "無法儲存排程作業資料");
                            }
                            else
                            {
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
        #endregion
    }
}