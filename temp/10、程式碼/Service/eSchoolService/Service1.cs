using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Fuju;
using Fuju.DB;
using Fuju.DB.Data;
using Entities;

namespace eSchoolService
{
    public partial class Service1 : ServiceBase
    {
        #region Member
        private string _HostName = System.Environment.MachineName.Trim();

        #region [MDY:2018xxxx] 改用 ServiceItem2 物件
        #region [OLD]
        //private List<ServiceItem> _ServiceItems = new List<ServiceItem>();
        #endregion

        private List<ServiceItem2> _ServiceItems = new List<ServiceItem2>();
        #endregion
        #endregion

        #region EventLog 相關
        private string _EventSource = "eSchoolService";
        private string _EventLog = "eSchool";
        private int _EventID = 1;

        private void InitialEventLog()
        {
            _EventSource = this.ServiceName;
            if (!EventLog.SourceExists(_EventSource))
            {
                EventLog.CreateEventSource(_EventSource, _EventLog);
            }
        }

        private void WriteInfoEventLog(string log)
        {
            this.EventLog.WriteEntry(log, EventLogEntryType.Information, _EventID);
        }

        private void WriteErrorEventLog(string log)
        {
            this.EventLog.WriteEntry(log, EventLogEntryType.Error, _EventID);
        }
        #endregion

        #region FileLog 相關
        private class FileLoger
        {
            private string _LogName = null;
            public string LogName
            {
                get
                {
                    return _LogName;
                }
                private set
                {
                    _LogName = value == null ? null : value.Trim();
                }
            }

            private string _LogPath = null;
            public string LogPath
            {
                get
                {
                    return _LogPath;
                }
                private set
                {
                    _LogPath = value == null ? String.Empty : value.Trim();
                }
            }

            public bool IsDebug
            {
                get;
                private set;
            }

            private string _LogFileName = null;

            public FileLoger(string logName)
            {
                this.LogName = logName;
                this.LogPath = ConfigurationManager.AppSettings.Get("LOG_PATH");
                string logMode = ConfigurationManager.AppSettings.Get("LOG_MDOE");
                if (String.IsNullOrEmpty(logMode))
                {
                    this.IsDebug = false;
                }
                else
                {
                    this.IsDebug = (logMode.Trim().Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase));
                }
                this.Initial();
            }

            public FileLoger(string logName, string path, bool isDebug)
            {
                this.LogName = logName;
                this.LogPath = path;
                this.IsDebug = isDebug;
                this.Initial();
            }

            public string Initial()
            {
                if (!String.IsNullOrEmpty(this.LogPath))
                {
                    try
                    {
                        DirectoryInfo info = new DirectoryInfo(this.LogPath);
                        if (!info.Exists)
                        {
                            info.Create();
                        }
                        if (String.IsNullOrEmpty(this.LogName))
                        {
                            string fileName = String.Format("{0:yyyyMMdd}.log", DateTime.Today);
                            _LogFileName = Path.Combine(info.FullName, fileName);
                        }
                        else
                        {
                            string fileName = String.Format("{0}_{1:yyyyMMdd}.log", this.LogName, DateTime.Today);
                            _LogFileName = Path.Combine(info.FullName, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                return null;
            }

            public void WriteLog(string msg)
            {
                if (!String.IsNullOrEmpty(_LogFileName) && msg != null)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(_LogFileName, true, Encoding.Default))
                        {
                            if (String.IsNullOrEmpty(msg))
                            {
                                sw.WriteLine(String.Empty);
                            }
                            else
                            {
                                sw.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, msg);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteLog(string format, params object[] args)
            {
                if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
                {
                    try
                    {
                        this.WriteLog(String.Format(format, args));
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            public void WriteDebugLog(string msg)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(msg);
                }
            }

            public void WriteDebugLog(string format, params object[] args)
            {
                if (this.IsDebug)
                {
                    this.WriteLog(format, args);
                }
            }
        }

        private FileLoger _FileLog = null;
        #endregion

        #region Timer 相關
        #region [MDY:20220108] 新的下次觸發間隔時間計算，所以不再使用
        //private int _MinTimerInterval = 1000;   //最小 Timer 等待時間 1 秒
        #endregion

        private Timer _Timer = new Timer() { Enabled = false };

        #region FileLog Initial 失敗，先不寫 Event Log
        //private bool _LogFailEventSaved = false;
        #endregion

        #region [MDY:20220108] 新的下次觸發間隔時間計算方式且取消下次觸發時間
        #region [OLD]
        //#region [MDY:20160828] 修正觸發時間跨分鐘的問題
        ///// <summary>
        ///// 取得下次觸發時間 (00秒) (本次觸發時間加 1 分鐘，並去除秒數)
        ///// </summary>
        ///// <param name="time">指定本次觸發時間</param>
        ///// <returns>傳回下次觸發時間</returns>
        //private DateTime GetNextTrigerTime(DateTime time)
        //{
        //    return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0).AddMinutes(1);
        //}

        ///// <summary>
        ///// 取得目前時間到下次觸發時間的間隔毫秒數
        ///// </summary>
        ///// <param name="time">指定下次觸發時間</param>
        ///// <returns>傳回間隔毫秒數，如果下次觸發時間小於或等於目前時間則傳回10*1000毫秒</returns>
        //private int GetIntervalMilliseconds(DateTime nextTime)
        //{
        //    DateTime chkTime = DateTime.Now;

        //    #region [MDY:2018xxxx] 無條件進位到秒，避免觸發間隔不足的問題
        //    #region [OLD]
        //    //int milliseconds = Convert.ToInt32((nextTime - chkTime).TotalMilliseconds);
        //    #endregion

        //    int milliseconds = Convert.ToInt32(Math.Ceiling((nextTime - chkTime).TotalSeconds)) * 1000;
        //    #endregion

        //    if (milliseconds < _MinTimerInterval)
        //    {
        //        return _MinTimerInterval;
        //    }
        //    else
        //    {
        //        return milliseconds;
        //    }
        //}
        //#endregion
        #endregion

        DateTime _ServiceStartTime = DateTime.Now;

        /// <summary>
        /// 取得到下次觸發的間隔毫秒數
        /// </summary>
        /// <param name="startTime">指定本次起始時間</param>
        /// <returns>傳回間隔毫秒數</returns>
        private int GetTimerInterval()
        {
            return (60 - DateTime.Now.Second) * 1000;
        }
        #endregion

        private void TimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            #region [MDY:20220108] 取消下次觸發時間
            #region [OLD]
            //#region [MDY:20160828] 修正觸發時間跨分鐘的問題
            //DateTime now = DateTime.Now;
            //DateTime nextTime = GetNextTrigerTime(now);  //下次觸發時間 (00秒)
            //#endregion
            #endregion

            DateTime startTime = DateTime.Now;
            #endregion

            #region Initial FileLog
            {
                string errmsg = _FileLog.Initial();

                #region FileLog Initial 失敗，先不寫 Event Log
                //if (!String.IsNullOrEmpty(errmsg))
                //{
                //    if (!_LogFailEventSaved)
                //    {
                //        this.WriteErrorEventLog("日誌檔無法初始化，錯誤訊息：" + errmsg);
                //        _LogFailEventSaved = true;
                //    }
                //}
                //else
                //{
                //    _LogFailEventSaved = false;
                //}
                #endregion
            }
            #endregion

            #region WriteDebugLog
            {
                _FileLog.WriteDebugLog("開始執行 {0} 服務處理程序", this.ServiceName);
            }
            #endregion

            #region 處理服務項目
            _Timer.Enabled = false;
            try
            {
                #region [MDY:2018xxxx] 改用 ServiceItem2 物件
                #region [OLD]
                //List<ServiceItem> items = null;
                #endregion

                List<ServiceItem2> items = null;
                #endregion

                #region 讀取服務項目設定資料
                {
                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();
                    Result result = this.RefreshServiceItems(out items);
                    if (!result.IsSuccess)
                    {
                        _FileLog.WriteLog("讀取服務項目設定，錯誤訊息：{0}", result.Message);
                        return;
                    }
                    if (items == null || items.Count == 0)
                    {
                        _FileLog.WriteLog("目前未設定任何服務項目");
                        return;
                    }

                    //#region [模擬測試]
                    //{
                    //    System.Threading.SpinWait.SpinUntil(() => false, 30000);
                    //    _FileLog.WriteDebugLog("模擬不明原因 delay 30 秒");
                    //}
                    //#endregion

                    stopWatch.Stop();
                    if (stopWatch.ElapsedMilliseconds > 15000L)
                    {
                        _FileLog.WriteLog("讀取服務項目設定資料超過 15 秒，伺服器可能處理異常忙碌狀態");
                    }
                }
                #endregion

                #region [MDY:2018xxxx] 改用 ServiceItem2 物件
                #region [OLD]
                //foreach (ServiceItem item in items)
                //{
                //    if (!item.IsReady())
                //    {
                //        _FileLog.WriteLog("{0} 服務項目設定未準備好", item.JobCubeType);
                //        continue;
                //    }
                //    if (!item.Config.Enabled)
                //    {
                //        _FileLog.WriteDebugLog("{0} 服務項目設定停用", item.JobCubeType);
                //        continue;
                //    }

                //    if (item.IsTimeUp(now))
                //    {
                //        string arguments = item.Config.AppArguments;
                //        //[TODO]:如果需要對參數加工寫在這裡
                //        string errmsg = item.RunApp(arguments);
                //        if (!String.IsNullOrEmpty(errmsg))
                //        {
                //            _FileLog.WriteLog(errmsg);
                //        }
                //        else
                //        {
                //            _FileLog.WriteDebugLog("{0} 服務項目已執行", item.JobCubeType);
                //        }
                //    }
                //    else
                //    {
                //        DateTime? nextRunTime = item.GetNextRunTime();
                //        if (nextRunTime != null)
                //        {
                //            _FileLog.WriteDebugLog("{0} 服務項目未到執行時間 {1:yyyy/MM/dd HH:mm:ss}", item.JobCubeType, nextRunTime.Value);
                //        }
                //        else
                //        {
                //            _FileLog.WriteDebugLog("{0} 服務項目未到執行時間}", item.JobCubeType);

                //        }
                //    }
                //}
                #endregion

                #region [MDY:20220108] 增加補執行處理
                foreach (ServiceItem2 item in items)
                {
                    if (!item.IsReady())
                    {
                        _FileLog.WriteLog("{0} 服務項目設定未準備好", item.JobCubeType);
                        continue;
                    }
                    if (!item.Config.Enabled)
                    {
                        _FileLog.WriteDebugLog("{0} 服務項目設定停用", item.JobCubeType);
                        continue;
                    }

                    string lastRunTimeInfo = item.LastRunTime == null ? String.Empty : String.Format("(上次執行時間：{0:yyyy/MM/dd HH:mm:ss})", item.LastRunTime.Value);

                    if (item.IsRunTime(startTime))
                    {
                        #region 已到執行時間，執行 App
                        //[TODO]:如果需要對參數加工寫在這裡
                        string otherArg = null;
                        string errmsg = item.RunApp(otherArg);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            _FileLog.WriteDebugLog("{0} 服務項目已執行失敗。{1}", item.JobCubeType, errmsg);
                        }
                        else
                        {
                            _FileLog.WriteDebugLog("{0} 服務項目已執行成功。 {1}", item.JobCubeType, lastRunTimeInfo);
                        }
                        #endregion
                    }
                    else if (item.IsRemedyItem(startTime, _ServiceStartTime))
                    {
                        #region 未到執行時間但必須補執行
                        //[TODO]:如果需要對參數加工寫在這裡
                        string otherArg = null;
                        string errmsg = item.RunApp(otherArg);
                        if (!String.IsNullOrEmpty(errmsg))
                        {
                            _FileLog.WriteDebugLog("{0} 服務項目補執行失敗。{1}", item.JobCubeType, errmsg);
                        }
                        else
                        {
                            _FileLog.WriteDebugLog("{0} 服務項目補執行成功。{1}", item.JobCubeType, lastRunTimeInfo);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 未到執行時間且不用補執行
                        DateTime? nextRunTime = item.GetNextRunTime(startTime);
                        if (nextRunTime != null)
                        {
                            _FileLog.WriteDebugLog("{0} 服務項目未到執行時間 {1:yyyy/MM/dd HH:mm:ss} {2}", item.JobCubeType, nextRunTime.Value, lastRunTimeInfo);
                        }
                        else
                        {
                            _FileLog.WriteDebugLog("{0} 服務項目無法計算執行時間 {1}", item.JobCubeType, lastRunTimeInfo);
                        }
                        #endregion
                    }

                    //#region [模擬測試]
                    //{
                    //    System.Threading.SpinWait.SpinUntil(() => false, 1000);
                    //    _FileLog.WriteDebugLog("模擬啟動項目耗時 1 秒 ({0:yyyy/MM/dd HH:mm:ss})", DateTime.Now);
                    //}
                    //#endregion
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                _FileLog.WriteDebugLog("處理服務項目發生例外，錯誤訊息： {0}", ex.Message);
            }
            finally
            {
                #region [MDY:20220108] 改用 GetTimerInterval()
                #region [OLD]
                //#region [MDY:20160828] 修正觸發時間跨分鐘的問題
                //_Timer.Interval = this.GetIntervalMilliseconds(nextTime);
                //#endregion
                #endregion

                _Timer.Interval = this.GetTimerInterval();
                #endregion

                _Timer.Enabled = true;
            }
            #endregion

            #region WriteDebugLog
            {
                _FileLog.WriteDebugLog("結束執行 {0} 服務處理程序 (等候 {1} 毫秒後再次觸發) \r\n", this.ServiceName, _Timer.Interval);
            }
            #endregion
        }
        #endregion

        #region ServiceItem 相關
        #region [MDY:2018xxxx] 改用 ServiceItem2 物件
        #region [OLD]
        //private Result RefreshServiceItems(out List<ServiceItem> serviceItems)
        //{
        //    Result result = null;

        //    #region 取得 ServiceConfig
        //    ServiceConfig[] serviceConfigs = null;
        //    using (EntityFactory factory = new EntityFactory())
        //    {
        //        ConfigEntity config = null;
        //        Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SERVICE_CONFIG);
        //        result = factory.SelectFirst<ConfigEntity>(where, null, out config);
        //        if (result.IsSuccess)
        //        {
        //            if (config != null && !String.IsNullOrEmpty(config.ConfigValue))
        //            {
        //                ServiceConfigHelper helper = new ServiceConfigHelper();
        //                if (!helper.DeXmlString(config.ConfigValue, out serviceConfigs))
        //                {
        //                    result = new Result(false, "服務項目設定解析失敗", CoreStatusCode.UNKNOWN_ERROR, null);
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region 更新 ServiceItem 的 ServiceConfig
        //    if (result.IsSuccess)
        //    {
        //        if (serviceConfigs == null || serviceConfigs.Length == 0)
        //        {
        //            _ServiceItems.Clear();
        //        }
        //        else
        //        {
        //            #region 先全部 Disable
        //            foreach (ServiceItem serviceItem in _ServiceItems)
        //            {
        //                serviceItem.Config.Enabled = false;
        //            }
        //            #endregion

        //            #region 替換 Config 或新增 ServiceItem
        //            foreach (ServiceConfig serviceConfig in serviceConfigs)
        //            {
        //                if (serviceConfig.Enabled)
        //                {
        //                    bool isExits = false;
        //                    foreach (ServiceItem serviceItem in _ServiceItems)
        //                    {
        //                        if (serviceItem.Config.JobCubeType == serviceConfig.JobCubeType)
        //                        {
        //                            //找到就替換 Config
        //                            isExits = true;
        //                            serviceItem.Config = serviceConfig;
        //                            break;
        //                        }
        //                    }
        //                    if (!isExits)
        //                    {
        //                        _ServiceItems.Add(new ServiceItem(serviceConfig));
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region 移除所有 Disable
        //            for (int idx = _ServiceItems.Count - 1; idx >= 0; idx--)
        //            {
        //                if (!_ServiceItems[idx].Config.Enabled)
        //                {
        //                    _ServiceItems.RemoveAt(idx);
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    #endregion

        //    serviceItems = _ServiceItems;
        //    return result;
        //}
        #endregion

        /// <summary>
        /// Refresh ServiceItems 資料
        /// </summary>
        /// <param name="serviceItems"></param>
        /// <returns></returns>
        private Result RefreshServiceItems(out List<ServiceItem2> serviceItems)
        {
            Result result = null;

            #region 取得 ServiceConfig
            List<ServiceConfig2> serviceConfigs = null;
            using (EntityFactory factory = new EntityFactory())
            {
                ConfigEntity config = null;
                Expression where = new Expression(ConfigEntity.Field.ConfigKey, ConfigKeyCodeTexts.SERVICE_CONFIG);
                result = factory.SelectFirst<ConfigEntity>(where, null, out config);
                if (result.IsSuccess)
                {
                    if (config != null && !String.IsNullOrEmpty(config.ConfigValue))
                    {
                        string errmsg = null;
                        ServiceConfigHelper helper = new ServiceConfigHelper();
                        if (!helper.TryDeXml(config.ConfigValue, out errmsg, out serviceConfigs))
                        {
                            result = new Result(false, "服務項目設定解析失敗，" + errmsg, CoreStatusCode.UNKNOWN_ERROR, null);
                        }
                    }
                }
            }
            #endregion

            #region 更新 ServiceItem 的 ServiceConfig
            if (result.IsSuccess)
            {
                if (serviceConfigs == null || serviceConfigs.Count == 0)
                {
                    serviceItems = new List<ServiceItem2>(0);
                }
                else
                {
                    serviceConfigs.Sort(ServiceConfig2.Comparison);
                    serviceItems = new List<ServiceItem2>(serviceConfigs.Count);
                    foreach (ServiceConfig2 serviceConfig in serviceConfigs)
                    {
                        ServiceItem2 newItem = new ServiceItem2(serviceConfig);
                        ServiceItem2 oldItem = _ServiceItems.Find(x => x.JobCubeType == serviceConfig.JobCubeType);
                        if (oldItem != null)
                        {
                            newItem.LastRunTime = oldItem.LastRunTime;
                        }
                        serviceItems.Add(newItem);
                    }
                }
                _ServiceItems.Clear();
                _ServiceItems = serviceItems;
            }
            else
            {
                serviceItems = new List<ServiceItem2>(0);
            }
            #endregion

            return result;
        }
        #endregion
        #endregion

        public Service1()
        {
            InitializeComponent();
        }

        private void InitialService()
        {
            //_EventSource = this.ServiceName;
            this.InitialEventLog();

            _FileLog = new FileLoger(this.ServiceName);

            #region Timer
            #region [MDY:20220108] 改用 GetTimerInterval()
            #region [OLD]
            //#region [MDY:20160828] 修正觸發時間跨分鐘的問題
            //_Timer.Interval = this.GetIntervalMilliseconds(this.GetNextTrigerTime(DateTime.Now));
            //#endregion
            #endregion

            _ServiceStartTime = DateTime.Now;

            _Timer.Interval = this.GetTimerInterval();
            #endregion

            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            _Timer.Enabled = true;
            _Timer.AutoReset = true;
            #endregion
        }

        protected override void OnStart(string[] args)
        {
            this.InitialService();

            string log = String.Format("{0} Start (等候 {1} 毫秒後開始觸發處理程序)\r\n", this.ServiceName, _Timer.Interval);
            _FileLog.WriteLog(log);

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _Timer.Enabled = false;

            string log = String.Concat(this.ServiceName, " Stop\r\n");
            _FileLog.WriteLog(log);

            base.OnStop();
        }

        protected override void OnPause()
        {
            _Timer.Enabled = false;

            string log = String.Concat(this.ServiceName, " Pause");
            _FileLog.WriteLog(log);

            base.OnPause();
        }

        protected override void OnContinue()
        {
            _Timer.Enabled = true;

            string log = String.Concat(this.ServiceName, " Continue");
            _FileLog.WriteLog(log);

            base.OnContinue();
        }

        protected override void OnShutdown()
        {
            _Timer.Enabled = false;

            string log = String.Concat(this.ServiceName, " Shutdown");
            _FileLog.WriteLog(log);

            base.OnShutdown();
        }
    }

    #region 服務項目資料承載類別
    #region [MDY:2018xxxx] 改用 ServiceItem2 物件
    #region [OLD}
    ///// <summary>
    ///// 服務項目資料承載類別
    ///// </summary>
    //sealed class ServiceItem
    //{
    //    #region Property
    //    private ServiceConfig _Config = null;
    //    /// <summary>
    //    /// 服務項目設定
    //    /// </summary>
    //    public ServiceConfig Config
    //    {
    //        get
    //        {
    //            return _Config;
    //        }
    //        set
    //        {
    //            _Config = value;
    //        }
    //    }

    //    /// <summary>
    //    /// 取得批次處理佇列作業類別代碼 (參考 JobCubeTypeCodeTexts)
    //    /// </summary>
    //    public string JobCubeType
    //    {
    //        get
    //        {
    //            if (Config == null)
    //            {
    //                return String.Empty;
    //            }
    //            else
    //            {
    //                return Config.JobCubeType ?? String.Empty;
    //            }
    //        }
    //    }

    //    private DateTime _LastRunTime;
    //    /// <summary>
    //    /// 取得最後一次執行的時間 (或此物件初始化時間)
    //    /// </summary>
    //    public DateTime LastRunTime
    //    {
    //        get
    //        {
    //            return _LastRunTime;
    //        }
    //        private set
    //        {
    //            _LastRunTime = value;
    //        }
    //    }
    //    #endregion

    //    #region Constructor
    //    /// <summary>
    //    /// 建構服務項目資料承載類別
    //    /// </summary>
    //    /// <param name="config"></param>
    //    public ServiceItem(ServiceConfig config)
    //    {
    //        this.Config = config;
    //        if (config.CycleUnit == ServiceCycleUnit.Day)
    //        {
    //            this.LastRunTime = this.TruncateSecond(DateTime.Now.AddDays(-1).AddMinutes(-1));
    //        }
    //        else
    //        {
    //            this.LastRunTime = this.TruncateSecond(DateTime.Now.AddMinutes(-1));
    //        }
    //    }
    //    #endregion

    //    #region Method
    //    /// <summary>
    //    /// 取得此服務項目的設定是否準備好
    //    /// </summary>
    //    /// <returns>是則傳回 true，否則傳回 false</returns>
    //    public bool IsReady()
    //    {
    //        return _Config == null ? false : _Config.IsReady();
    //    }

    //    /// <summary>
    //    /// 取得此服務項目是否到時 (以時間間隔計算)
    //    /// </summary>
    //    /// <param name="now">指定比對的時間</param>
    //    /// <returns>是則傳回 true，否則傳回 false</returns>
    //    public bool IsTimeUp(DateTime now)
    //    {
    //        if (_Config != null && _Config.IsCycleReady())
    //        {
    //            ServiceCycleTime chkTime = new ServiceCycleTime(now.Hour, now.Minute);
    //            TimeSpan interval = (this.TruncateSecond(now) - this.TruncateSecond(LastRunTime));
    //            switch (_Config.CycleUnit)
    //            {
    //                case ServiceCycleUnit.Minute:
    //                    #region Minute
    //                    {
    //                        if (interval.TotalMinutes >= _Config.CycleValue)
    //                        {
    //                            if (_Config.CycleStartTime == _Config.CycleEndTime)
    //                            {
    //                                if (_Config.CycleStartTime == ServiceCycleTime.Zero)
    //                                {
    //                                    //00:00 ~ 00:00 (全天)
    //                                    return true;
    //                                }
    //                                else
    //                                {
    //                                    return (_Config.CycleStartTime == chkTime);
    //                                }
    //                            }
    //                            else if (_Config.CycleEndTime < _Config.CycleStartTime)
    //                            {
    //                                //換天
    //                                return (chkTime >= _Config.CycleStartTime);
    //                            }
    //                            else
    //                            {
    //                                //同天
    //                                return (_Config.CycleStartTime <= chkTime && chkTime <= _Config.CycleEndTime);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            return false;
    //                        }
    //                    }
    //                    #endregion
    //                case ServiceCycleUnit.Day:
    //                    #region Day
    //                    {
    //                        #region [Old] 改為如果是每天則只以時間點計算
    //                        //return (interval.TotalDays >= _Config.CycleValue && _Config.CycleStartTime == chkTime);
    //                        #endregion

    //                        #region 只以時間點計算
    //                        if (_Config.CycleValue == 1)
    //                        {
    //                            return (_Config.CycleStartTime == chkTime);
    //                        }
    //                        else
    //                        {
    //                            return (interval.TotalDays >= _Config.CycleValue && _Config.CycleStartTime == chkTime);
    //                        }
    //                        #endregion
    //                    }
    //                    #endregion
    //            }
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// 取得下此執行的時間
    //    /// </summary>
    //    /// <returns></returns>
    //    public DateTime? GetNextRunTime()
    //    {
    //        if (_Config != null && _Config.CycleValue > 0 && _Config.CycleUnit != ServiceCycleUnit.Empty)
    //        {
    //            DateTime lastTime = this.TruncateSecond(_LastRunTime);
    //            switch (_Config.CycleUnit)
    //            {
    //                case ServiceCycleUnit.Minute:
    //                    #region Minute
    //                    {
    //                        DateTime nextDateTime = lastTime.AddMinutes(_Config.CycleValue);
    //                        if (_Config.CycleStartTime == _Config.CycleEndTime)
    //                        {
    //                            return nextDateTime;
    //                        }
    //                        else
    //                        {
    //                            ServiceCycleTime nextTime = new ServiceCycleTime(nextDateTime.Hour, nextDateTime.Minute);
    //                            if (_Config.CycleEndTime < _Config.CycleStartTime)
    //                            {
    //                                //換天
    //                                if (nextTime < _Config.CycleStartTime)
    //                                {
    //                                    nextDateTime = new DateTime(nextDateTime.Year, nextDateTime.Month, nextDateTime.Day, Config.CycleStartTime.Hour, _Config.CycleStartTime.Minute, 0);
    //                                }
    //                                return nextDateTime;
    //                            }
    //                            else
    //                            {
    //                                //同天
    //                                if (nextTime < _Config.CycleStartTime)
    //                                {
    //                                    nextDateTime = new DateTime(nextDateTime.Year, nextDateTime.Month, nextDateTime.Day, Config.CycleStartTime.Hour, _Config.CycleStartTime.Minute, 0);
    //                                }
    //                                else if (nextTime > _Config.CycleEndTime)
    //                                {
    //                                    nextDateTime = (new DateTime(nextDateTime.Year, nextDateTime.Month, nextDateTime.Day, Config.CycleStartTime.Hour, _Config.CycleStartTime.Minute, 0)).AddDays(1);
    //                                }
    //                                return nextDateTime;
    //                            }
    //                        }
    //                    }
    //                    #endregion
    //                case ServiceCycleUnit.Day:
    //                    #region Day
    //                    {
    //                        DateTime nextDateTime = lastTime.AddDays(_Config.CycleValue);
    //                        return new DateTime(nextDateTime.Year, nextDateTime.Month, nextDateTime.Day, Config.CycleStartTime.Hour, _Config.CycleStartTime.Minute, 0);
    //                    }
    //                    #endregion
    //            }
    //        }
    //        return null;
    //    }

    //    public string RunApp(string arguments)
    //    {
    //        this.LastRunTime = DateTime.Now;

    //        try
    //        {
    //            Process myProcess = new Process();

    //            myProcess.StartInfo.FileName = _Config.AppFileName;
    //            if (!String.IsNullOrWhiteSpace(arguments))
    //            {
    //                myProcess.StartInfo.Arguments = arguments.Trim();
    //            }

    //            if (!myProcess.Start())
    //            {
    //                return string.Format("執行 {0} ({1}) 程式失敗，回傳值 {2}", this.JobCubeType, _Config.AppFileName, myProcess.ExitCode);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return string.Format("執行 {0} ({1}) 程式發生例外，錯誤訊息： {2}", this.JobCubeType, _Config.AppFileName, ex.Message);
    //        }

    //        return null;
    //    }

    //    private DateTime TruncateSecond(DateTime time)
    //    {
    //        return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
    //    }
    //    #endregion
    //}
    #endregion

    /// <summary>
    /// 服務項目資料承載類別
    /// </summary>
    sealed class ServiceItem2
    {
        #region Property
        private ServiceConfig2 _Config = null;
        /// <summary>
        /// 服務項目設定
        /// </summary>
        public ServiceConfig2 Config
        {
            get
            {
                return _Config;
            }
            set
            {
                _Config = value;
            }
        }

        /// <summary>
        /// 取得批次處理佇列作業類別代碼 (參考 JobCubeTypeCodeTexts)
        /// </summary>
        public string JobCubeType
        {
            get
            {
                if (this.Config == null)
                {
                    return String.Empty;
                }
                else
                {
                    return this.Config.JobCubeType ?? String.Empty;
                }
            }
        }

        private DateTime? _LastRunTime = null;
        /// <summary>
        /// 取得最後一次執行的時間 (或此物件初始化時間)
        /// </summary>
        public DateTime? LastRunTime
        {
            get
            {
                return _LastRunTime;
            }
            set
            {
                _LastRunTime = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 服務項目資料承載類別 物件
        /// </summary>
        /// <param name="config">服務項目設定</param>
        public ServiceItem2(ServiceConfig2 config)
        {
            this.Config = config;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此服務項目的設定是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsReady()
        {
            return _Config == null ? false : _Config.IsReady();
        }

        /// <summary>
        /// 取得指定日期時間是否為此服務項目的執行時間
        /// </summary>
        /// <param name="chkDateTime">指定日期時間</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsRunTime(DateTime chkDateTime)
        {
            if (_Config != null && _Config.IsCycleReady())
            {
                return _Config.IsTimeUp(chkDateTime);
            }
            return false;
        }

        #region [MDY:20220203] 修正 00:00 會判斷錯誤的問題
        /// <summary>
        /// 取得此服務項目是否為需要補救項目
        /// </summary>
        /// <param name="triggerTime">指定本次觸發時間</param>
        /// <param name="serviceStartTime">指定服務起始時間</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsRemedyItem(DateTime triggerTime, DateTime serviceStartTime)
        {
            //本次觸發時間的前一分鐘為檢查日期時間的最大值
            DateTime maxDateTime = triggerTime.AddMinutes(-1);
            if (_Config != null && _Config.IsCycleReady() && _Config.Cycle.IsMyDate(maxDateTime) && maxDateTime >= serviceStartTime)
            {
                HourMinute maxTime = maxDateTime.GetHourMinute();
                if (this.LastRunTime.HasValue && maxDateTime.Date == this.LastRunTime.Value.Date)
                {
                    HourMinute minTime = this.LastRunTime.Value.AddMinutes(1).GetHourMinute();
                    return minTime <= maxTime ? _Config.HaveMyTime(minTime, maxTime) : false;
                }
                else
                {
                    HourMinute minTime = serviceStartTime.Date != maxDateTime.Date ? maxDateTime.Date.GetHourMinute() : serviceStartTime.AddMinutes(1).GetHourMinute();
                    return _Config.HaveMyTime(minTime, maxTime);
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 取得此服務項目比指定日期時間大的下次執行時間
        /// </summary>
        /// <param name="chkDateTime">指定日期時間</param>
        /// <returns></returns>
        public DateTime? GetNextRunTime(DateTime chkDateTime)
        {
            DateTime? nextTime = null;
            if (_Config != null && _Config.IsCycleReady())
            {
                chkDateTime = chkDateTime.AddMinutes(1);
                DateTime? nextDate = _Config.Cycle.GetCycleDateCeiling(chkDateTime);
                if (nextDate != null)
                {
                    if (nextDate == chkDateTime.Date)
                    {
                        nextTime = _Config.Cycle.CycleTime.GetCeilingDateTime(chkDateTime);
                    }
                    else
                    {
                        nextTime = _Config.Cycle.CycleTime.GetCeilingDateTime(nextDate.Value);
                    }
                }
            }
            return nextTime;
        }

        /// <summary>
        /// 執行此服務項目指定的 App
        /// </summary>
        /// <param name="otherArg"></param>
        /// <returns></returns>
        public string RunApp(string otherArg)
        {
            this.LastRunTime = DateTime.Now;

            try
            {
                Process myProcess = new Process();

                myProcess.StartInfo.FileName = _Config.AppFileName;
                string arguments = otherArg == null ? _Config.AppArguments : otherArg + " " + _Config.AppArguments;
                if (!String.IsNullOrWhiteSpace(arguments))
                {
                    myProcess.StartInfo.Arguments = arguments.Trim();
                }

                if (!myProcess.Start())
                {
                    return string.Format("執行 {0} ({1}) 程式失敗，回傳值 {2}", this.JobCubeType, _Config.AppFileName, myProcess.ExitCode);
                }
            }
            catch (Exception ex)
            {
                return string.Format("執行 {0} ({1}) 程式發生例外，錯誤訊息： {2}", this.JobCubeType, _Config.AppFileName, ex.Message);
            }

            return null;
        }
        #endregion
    }
    #endregion
    #endregion
}
