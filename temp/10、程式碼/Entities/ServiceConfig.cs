using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 服務週期單位列舉
    /// </summary>
    public enum ServiceCycleUnit
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Empty = 0,
        /// <summary>
        /// 分鐘
        /// </summary>
        Minute = 1,
        /// <summary>
        /// 天
        /// </summary>
        Day = 2
    }

    /// <summary>
    /// 服務週期的時間結構
    /// </summary>
    public struct ServiceCycleTime
    {
        #region Static Readonly
        /// <summary>
        /// 0 時 0分
        /// </summary>
        public static readonly ServiceCycleTime Zero = new ServiceCycleTime();
        #endregion

        #region Property
        private int _Hour;
        /// <summary>
        /// 時間的時 (0~23)
        /// </summary>
        public int Hour
        {
            get
            {
                return _Hour;
            }
            private set
            {
                if (value < 0)
                {
                    _Hour = 0;
                }
                else
                {
                    _Hour = value % 24;
                }
            }
        }

        private int _Minute;
        /// <summary>
        /// 時間的分 (0~59)
        /// </summary>
        public int Minute
        {
            get
            {
                return _Minute;
            }
            private set
            {
                if (value < 0)
                {
                    _Minute = 0;
                }
                else
                {
                    _Minute = value % 60;
                }
            }
        }

        /// <summary>
        /// 取得總分鐘數
        /// </summary>
        public int TotalMinutes
        {
            get
            {
                return this.Hour * 60 + this.Minute;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構服務週期的時間結構
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public ServiceCycleTime(int hour, int minute)
        {
            _Hour = hour;
            _Minute = minute;
        }
        #endregion

        #region Override Object Method
        public override bool Equals(Object obj)
        {
            return obj is ServiceCycleTime && this == (ServiceCycleTime)obj;
        }
        public override int GetHashCode()
        {
            return _Hour.GetHashCode() ^ _Minute.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0:00}:{1:00}", this.Hour, this.Minute);
        }
        #endregion

        #region Object Operator Method
        public static bool operator ==(ServiceCycleTime x, ServiceCycleTime y)
        {
            return x.Hour == y.Hour && x.Minute == y.Minute;
        }
        public static bool operator !=(ServiceCycleTime x, ServiceCycleTime y)
        {
            return !(x == y);
        }
        public static bool operator >(ServiceCycleTime x, ServiceCycleTime y)
        {
            return (x.TotalMinutes > y.TotalMinutes);
        }
        public static bool operator >=(ServiceCycleTime x, ServiceCycleTime y)
        {
            return (x.TotalMinutes >= y.TotalMinutes);
        }
        public static bool operator <(ServiceCycleTime x, ServiceCycleTime y)
        {
            return (x.TotalMinutes < y.TotalMinutes);
        }
        public static bool operator <=(ServiceCycleTime x, ServiceCycleTime y)
        {
            return (x.TotalMinutes <= y.TotalMinutes);
        }
        #endregion

        #region Static Method
        private static Regex _CycleTimeReg = null;
        /// <summary>
        /// 嘗試轉換字串成 ServiceCycleTime 型別
        /// </summary>
        /// <param name="txt">指定要轉換字串</param>
        /// <param name="time">成功則傳回傳換後的 ServiceCycleTime，否則傳回 ServiceCycleTime.Zero</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool TryParse(string txt, out ServiceCycleTime time)
        {
            time = ServiceCycleTime.Zero;

            if (IsMatchFormat(txt))
            {
                time = Parse(txt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得指定文字是否符合服務週期時間的文字格式
        /// </summary>
        /// <param name="txt">指定文字</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public static bool IsMatchFormat(string txt)
        {
            if (String.IsNullOrEmpty(txt))
            {
                return false;
            }
            if (_CycleTimeReg == null)
            {
                _CycleTimeReg = new Regex("^([01]?[0-9]|2[0-3]):([0-5]?[0-9])$", RegexOptions.Compiled);
            }
            return _CycleTimeReg.IsMatch(txt);
        }

        /// <summary>
        /// 轉換字串成 ServiceCycleTime 型別
        /// </summary>
        /// <param name="txt">指定要轉換字串</param>
        /// <returns>傳回傳換後的 ServiceCycleTime</returns>
        public static ServiceCycleTime Parse(string txt)
        {
            string[] parts = txt.Split(new char[] { ':' });
            int hour = Int32.Parse(parts[0]);
            int minute = Int32.Parse(parts[1]);
            return new ServiceCycleTime(hour, minute);
        }
        #endregion
    }

    /// <summary>
    /// 服務項目設定承載類別
    /// </summary>
    [Serializable]
    public sealed class ServiceConfig
    {
        #region Static Readonly
        /// <summary>
        /// Xml 根節點名稱 ： ServiceConfig
        /// </summary>
        public static readonly string RootLocalName = "ServiceConfig";
        #endregion

        #region Property
        private string _JobCubeType = null;
        /// <summary>
        /// 批次處理佇列作業類別代碼 (參考 JobCubeTypeCodeTexts)
        /// </summary>
        public string JobCubeType
        {
            get
            {
                return _JobCubeType;
            }
            set
            {
                _JobCubeType = value == null ? null : value.Trim();
            }
        }

        private ServiceCycleUnit _CycleUnit = ServiceCycleUnit.Empty;
        /// <summary>
        /// 服務週期單位
        /// </summary>
        public ServiceCycleUnit CycleUnit
        {
            get
            {
                return _CycleUnit;
            }
            set
            {
                _CycleUnit = value;
            }
        }

        private int _CycleValue = 0;
        /// <summary>
        /// 服務週期值
        /// </summary>
        public int CycleValue
        {
            get
            {
                return _CycleValue;
            }
            set
            {
                _CycleValue = value < 0 ? 0 : value;
            }
        }

        private ServiceCycleTime _CycleStartTime = ServiceCycleTime.Zero;
        /// <summary>
        /// 服務週期的起始時間
        /// </summary>
        public ServiceCycleTime CycleStartTime
        {
            get
            {
                return _CycleStartTime;
            }
            set
            {
                _CycleStartTime = value;
            }
        }

        private ServiceCycleTime _CycleEndTime = ServiceCycleTime.Zero;
        /// <summary>
        /// 服務週期的結束時間
        /// </summary>
        public ServiceCycleTime CycleEndTime
        {
            get
            {
                return _CycleEndTime;
            }
            set
            {
                _CycleEndTime = value;
            }
        }

        private string _AppFileName = null;
        /// <summary>
        /// 應用程式檔案路徑名稱
        /// </summary>
        public string AppFileName
        {
            get
            {
                return _AppFileName;
            }
            set
            {
                _AppFileName = value == null ? null : value.Trim();
            }
        }

        private string _AppArguments = null;
        /// <summary>
        /// 應用程式命令參數
        /// </summary>
        public string AppArguments
        {
            get
            {
                return _AppArguments;
            }
            set
            {
                _AppArguments = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// 取得載入 XmlNode 的資料是否成功
        /// </summary>
        [XmlIgnore]
        internal bool LoadXmlOK
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構服務項目設定承載類別
        /// </summary>
        public ServiceConfig()
        {
            this.LoadXmlOK = false;
        }

        public ServiceConfig(XmlNode xNode)
        {
            this.LoadXmlOK = this.LoadXml(xNode);
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此設定的資料是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsReady()
        {
            return (this.IsCycleReady() && JobCubeTypeCodeTexts.IsDefine(_JobCubeType) && !String.IsNullOrEmpty(_AppFileName));
        }

        /// <summary>
        /// 取得此設定的週期資料是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsCycleReady()
        {
            return (_CycleUnit != ServiceCycleUnit.Empty && _CycleValue > 0);
        }

        /// <summary>
        /// 將此物件的設定資料寫入 XmlWriter
        /// </summary>
        /// <param name="writer">指定 XmlWriter</param>
        public void ToXmlWriter(XmlWriter writer)
        {
            if (writer != null)
            {
                #region Root Start
                writer.WriteStartElement(RootLocalName);
                #endregion

                #region JobCubeType
                writer.WriteElementString("JobCubeType", this.JobCubeType ?? String.Empty);
                #endregion

                #region CycleUnit
                writer.WriteElementString("CycleUnit", this.CycleUnit.ToString());
                #endregion

                #region CycleValue
                writer.WriteElementString("CycleValue", this.CycleValue.ToString());
                #endregion

                #region CycleStartTime
                writer.WriteElementString("CycleStartTime", this.CycleStartTime.ToString());
                #endregion

                #region CycleStartTime
                writer.WriteElementString("CycleEndTime", this.CycleEndTime.ToString());
                #endregion

                #region AppFileName
                writer.WriteElementString("AppFileName", this.AppFileName ?? String.Empty);
                #endregion

                #region AppParameter
                writer.WriteElementString("AppParameter", this.AppArguments ?? String.Empty);
                #endregion

                #region Enabled
                writer.WriteElementString("Enabled", this.Enabled ? "Y" : "N");
                #endregion

                #region Root End
                writer.WriteEndElement();
                #endregion

                writer.Flush();
            }
        }

        /// <summary>
        /// 載入 XmlNode 資料
        /// </summary>
        /// <param name="xNode">指定 XmlNode</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool LoadXml(XmlNode xRoot)
        {
            if (xRoot == null || xRoot.LocalName != RootLocalName)
            {
                return false;
            }

            #region JobCubeType
            {
                XmlNode xNode = xRoot.SelectSingleNode("JobCubeType");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.JobCubeType = xNode.InnerText.Trim();
            }
            #endregion

            #region CycleUnit
            {
                ServiceCycleUnit value = ServiceCycleUnit.Empty;
                XmlNode xNode = xRoot.SelectSingleNode("CycleUnit");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element && Enum.TryParse<ServiceCycleUnit>(xNode.InnerText.Trim(), true, out value))
                {
                    this.CycleUnit = value;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region CycleValue
            {
                int value;
                XmlNode xNode = xRoot.SelectSingleNode("CycleValue");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element && Int32.TryParse(xNode.InnerText.Trim(), out value))
                {
                    this.CycleValue = value;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region CycleStartTime
            {
                ServiceCycleTime value;
                XmlNode xNode = xRoot.SelectSingleNode("CycleStartTime");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element && ServiceCycleTime.TryParse(xNode.InnerText.Trim(), out value))
                {
                    this.CycleStartTime = value;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region CycleEndTime
            {
                ServiceCycleTime value;
                XmlNode xNode = xRoot.SelectSingleNode("CycleEndTime");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element && ServiceCycleTime.TryParse(xNode.InnerText.Trim(), out value))
                {
                    this.CycleEndTime = value;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region AppFileName
            {
                XmlNode xNode = xRoot.SelectSingleNode("AppFileName");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.AppFileName = xNode.InnerText.Trim();
            }
            #endregion

            #region AppParameter
            {
                XmlNode xNode = xRoot.SelectSingleNode("AppParameter");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.AppArguments = xNode.InnerText.Trim();
            }
            #endregion

            #region Enabled
            {
                XmlNode xNode = xRoot.SelectSingleNode("Enabled");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    return false;
                }
                this.Enabled = xNode.InnerText.Trim().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得作業類別名稱
        /// </summary>
        /// <returns></returns>
        public string GetJobCubeTypeName()
        {
            return JobCubeTypeCodeTexts.GetText(_JobCubeType);
        }

        /// <summary>
        /// 取得週期單位名稱
        /// </summary>
        /// <returns></returns>
        public string GetCycleUnitName()
        {
            switch (_CycleUnit)
            {
                case ServiceCycleUnit.Minute:
                    return "分鐘";
                case ServiceCycleUnit.Day:
                    return "天";
                case ServiceCycleUnit.Empty:
                    return "未指定";
            }
            return "未定義的週期單位";
        }
        #endregion
    }

    /// <summary>
    /// 服務項目設定工具類別
    /// </summary>
    public sealed class ServiceConfigHelper
    {
        #region Static Readonly
        /// <summary>
        /// Xml 根節點名稱 ： ServiceConfigs
        /// </summary>
        public static readonly string RootLocalName = "ServiceConfigs";
        #endregion

        #region [MDY:2018xxxx] 改用 ServiceConfig2 物件
        #region [OLD]
        //#region Method
        ///// <summary>
        ///// 取得指定服務項目設定陣列的 Xml 字串表示
        ///// </summary>
        ///// <param name="configs">指定服務項目設定陣列</param>
        ///// <returns>成功則傳回 Xml 字串，否則傳回 null</returns>
        //public string ToXmlString(ICollection<ServiceConfig> configs)
        //{
        //    XmlWriterSettings settings = new XmlWriterSettings();
        //    settings.CheckCharacters = false;
        //    settings.Encoding = Encoding.UTF8;
        //    settings.Indent = true;
        //    settings.NewLineOnAttributes = false;
        //    settings.OmitXmlDeclaration = true;

        //    string xml = null;
        //    try
        //    {
        //        using (MemoryStream memory = new MemoryStream())
        //        {
        //            using (XmlWriter writer = XmlWriter.Create(memory, settings))
        //            {
        //                #region Root Start
        //                writer.WriteStartElement(RootLocalName);
        //                #endregion

        //                foreach (ServiceConfig config in configs)
        //                {
        //                    config.ToXmlWriter(writer);
        //                }

        //                #region Root End
        //                writer.WriteEndElement();
        //                #endregion

        //                writer.Flush();
        //            }

        //            memory.Position = 0;
        //            using (StreamReader sr = new StreamReader(memory))
        //            {
        //                xml = sr.ReadToEnd();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return xml;
        //}

        ///// <summary>
        ///// 取得指定 Xml 字串表示的服務項目設定陣列
        ///// </summary>
        ///// <param name="xml">指定 Xml 字串</param>
        ///// <param name="configs">成功則傳回服務項目設定陣列，否傳回 null;</param>
        ///// <returns>成功則傳回 true，否則傳回 false</returns>
        //public bool DeXmlString(string xml, out ServiceConfig[] configs)
        //{
        //    configs = null;
        //    if (!String.IsNullOrWhiteSpace(xml))
        //    {
        //        try
        //        {
        //            XmlDocument xdoc = new XmlDocument();
        //            xdoc.LoadXml(xml);
        //            XmlElement xRoot = xdoc.DocumentElement;
        //            if (xRoot.LocalName == RootLocalName)
        //            {
        //                XmlNodeList xNodes = xRoot.SelectNodes(ServiceConfig.RootLocalName);
        //                if (xNodes != null && xNodes.Count > 0)
        //                {
        //                    configs = new ServiceConfig[xNodes.Count];
        //                    int idx = 0;
        //                    foreach (XmlNode xNode in xNodes)
        //                    {
        //                        ServiceConfig config = new ServiceConfig(xNode);
        //                        if (!config.LoadXmlOK)
        //                        {
        //                            configs = null;
        //                            return false;
        //                        }
        //                        configs[idx] = config;
        //                        idx++;
        //                    }
        //                    return true;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    return false;
        //}
        //#endregion
        #endregion

        #region Method
        /// <summary>
        /// 將指定 ServiceConfig 轉 ServiceConfig2 物件
        /// </summary>
        /// <param name="oldConfig">指定 ServiceConfig</param>
        /// <returns>成功則傳回 ServiceConfig2 物件，否則傳回 null</returns>
        public ServiceConfig2 Convert(ServiceConfig oldConfig)
        {
            ServiceConfig2 config = null;
            if (oldConfig != null)
            {
                DateTime sDate = new DateTime(2017, 01, 01);
                DaysCycle cycle = null;
                switch (oldConfig.CycleUnit)
                {
                    case ServiceCycleUnit.Empty:
                        #region 週期單位未指定
                        {

                        }
                        #endregion
                        break;
                    case ServiceCycleUnit.Minute:
                        #region 週期單位分鐘：表示每 1 天，每 n 分鐘一次
                        {
                            HourMinute startTime = new HourMinute(oldConfig.CycleStartTime.Hour, oldConfig.CycleStartTime.Minute);
                            HourMinute endTime = new HourMinute(oldConfig.CycleEndTime.Hour, oldConfig.CycleEndTime.Minute);
                            CycleTime cycleTime = new CycleTime(startTime, endTime, oldConfig.CycleValue);
                            cycle = new DaysCycle(sDate, null, 1, cycleTime);
                        }
                        #endregion
                        break;
                    case ServiceCycleUnit.Day:
                        #region 週期單位天：表示每 n 天一次
                        {
                            HourMinute startTime = new HourMinute(oldConfig.CycleStartTime.Hour, oldConfig.CycleStartTime.Minute);
                            HourMinute endTime = new HourMinute(oldConfig.CycleStartTime.Hour, oldConfig.CycleStartTime.Minute);
                            CycleTime cycleTime = new CycleTime(startTime, endTime, 0);
                            cycle = new DaysCycle(sDate, null, oldConfig.CycleValue, cycleTime);
                        }
                        #endregion
                        break;
                }

                config = new ServiceConfig2();
                config.JobCubeType = oldConfig.JobCubeType;
                config.Cycle = cycle;
                config.AppFileName = oldConfig.AppFileName;
                config.AppArguments = oldConfig.AppArguments;
                config.Enabled = oldConfig.Enabled;
            }
            return config;
        }

        /// <summary>
        /// 將指定服務項目設定陣列轉成 Xml 字串表示
        /// </summary>
        /// <param name="configs"></param>
        /// <returns></returns>
        public string TryToXml(ICollection<ServiceConfig2> configs)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            string xml = null;
            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(memory, settings))
                    {
                        #region Root Start
                        writer.WriteStartElement(RootLocalName);
                        #endregion

                        foreach (ServiceConfig2 config in configs)
                        {
                            config.WriteToXml(writer);
                        }

                        #region Root End
                        writer.WriteEndElement();
                        #endregion

                        writer.Flush();
                    }

                    memory.Position = 0;
                    using (StreamReader sr = new StreamReader(memory))
                    {
                        xml = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {

            }

            return xml;
        }

        /// <summary>
        /// 將指定 Xml 字串表示轉成服務項目設定陣列
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="errmsg"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public bool TryDeXml(string xml, out string errmsg, out List<ServiceConfig2> configs)
        {
            errmsg = null;
            configs = null;
            int fail1Count = 0;
            int fail2Count = 0;
            int fail3Count = 0;
            if (!String.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xml);
                    XmlElement xRoot = xdoc.DocumentElement;
                    if (xRoot.LocalName == RootLocalName)
                    {
                        #region 處理 ServiceConfig2
                        {
                            XmlNodeList xNodes = xRoot.SelectNodes(ServiceConfig2.RootLocalName);
                            if (xNodes != null && xNodes.Count > 0)
                            {
                                configs = new List<ServiceConfig2>(xNodes.Count);
                                foreach (XmlNode xNode in xNodes)
                                {
                                    ServiceConfig2 config = ServiceConfig2.LoadXmlNode(xNode);
                                    if (config != null)
                                    {
                                        configs.Add(config);
                                    }
                                    else
                                    {
                                        fail1Count++;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 處理 ServiceConfig 轉成 ServiceConfig2
                        {
                            XmlNodeList xNodes = xRoot.SelectNodes(ServiceConfig.RootLocalName);
                            if (xNodes != null && xNodes.Count > 0)
                            {
                                if (configs == null)
                                {
                                    configs = new List<ServiceConfig2>(xNodes.Count);
                                }
                                else if (configs.Capacity < configs.Count + xNodes.Count)
                                {
                                    configs.Capacity = configs.Count + xNodes.Count;
                                }
                                foreach (XmlNode xNode in xNodes)
                                {
                                    ServiceConfig oldConfig = new ServiceConfig(xNode);
                                    if (oldConfig.LoadXmlOK)
                                    {
                                        ServiceConfig2 config = this.Convert(oldConfig);
                                        if (config != null)
                                        {
                                            configs.Add(config);
                                        }
                                        else
                                        {
                                            fail3Count++;
                                        }
                                    }
                                    else
                                    {
                                        fail2Count++;
                                    }
                                }
                                return true;
                            }
                        }
                        #endregion

                        if (fail1Count > 0)
                        {
                            errmsg += String.Format("解析設定資料失敗 {0} 筆。", fail1Count);
                        }
                        if (fail2Count > 0)
                        {
                            errmsg += String.Format("解析舊格式的設定資料失敗 {0} 筆。", fail2Count);
                        }
                        if (fail3Count > 0)
                        {
                            errmsg += String.Format("舊格式資料轉新格式資料失敗 {0} 筆。", fail3Count);
                        }

                        return String.IsNullOrEmpty(errmsg);
                    }
                }
                catch (Exception ex)
                {
                    errmsg = "解析設定資料發生例外，" + ex.Message;
                }
            }
            return false;
        }
        #endregion
        #endregion
    }

    #region [MDY:2018xxxx] 改用 ServiceConfig2 物件
    /// <summary>
    /// 服務項目設定承載類別
    /// </summary>
    [Serializable]
    public sealed class ServiceConfig2
    {
        #region Static Readonly
        /// <summary>
        /// Xml 根節點名稱 ： ServiceConfig
        /// </summary>
        public static readonly string RootLocalName = "ServiceConfig2";
        #endregion

        #region Property
        private string _JobCubeType = null;
        /// <summary>
        /// 批次處理佇列作業類別代碼 (參考 JobCubeTypeCodeTexts)
        /// </summary>
        public string JobCubeType
        {
            get
            {
                return _JobCubeType;
            }
            set
            {
                _JobCubeType = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 服務週期單位
        /// </summary>
        public DaysCycle Cycle
        {
            get;
            set;
        }

        private string _AppFileName = null;
        /// <summary>
        /// 應用程式檔案路徑名稱
        /// </summary>
        public string AppFileName
        {
            get
            {
                return _AppFileName;
            }
            set
            {
                _AppFileName = value == null ? null : value.Trim();
            }
        }

        private string _AppArguments = null;
        /// <summary>
        /// 應用程式命令參數
        /// </summary>
        public string AppArguments
        {
            get
            {
                return _AppArguments;
            }
            set
            {
                _AppArguments = value == null ? null : value.Trim();
            }
        }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        #region [Old]
        ///// <summary>
        ///// 取得載入 XmlNode 的資料是否成功
        ///// </summary>
        //[XmlIgnore]
        //internal bool LoadXmlOK
        //{
        //    get;
        //    private set;
        //}
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 服務項目設定承載類別 物件
        /// </summary>
        public ServiceConfig2()
        {
            #region [Old]
            //this.LoadXmlOK = false;
            #endregion
        }

        #region [Old]
        ///// <summary>
        ///// 建構 服務項目設定承載類別 物件
        ///// </summary>
        ///// <param name="xmlNode"></param>
        //public ServiceConfig2(XmlNode xmlNode)
        //{
        //    this.LoadXmlOK = this.ReadFromXml(xmlNode);
        //}
        #endregion
        #endregion

        #region Method
        /// <summary>
        /// 取得此物件的資料是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsReady()
        {
            return (this.IsCycleReady() && JobCubeTypeCodeTexts.IsDefine(this.JobCubeType) && !String.IsNullOrEmpty(this.AppFileName));
        }

        /// <summary>
        /// 取得此物件的週期資料是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool IsCycleReady()
        {
            return (this.Cycle != null && this.Cycle.IsReady());
        }

        /// <summary>
        /// 取得指定日期時間是否為此服務項目的執行日期時間
        /// </summary>
        /// <param name="chkDateTime">指定日期時間</param>
        /// <returns>是則回 true，否則傳回 false</returns>
        public bool IsTimeUp(DateTime chkDateTime)
        {
            if (this.Cycle != null && this.Cycle.IsReady())
            {
                if (this.Cycle.IsMyDate(chkDateTime) && this.Cycle.CycleTime.IsMyTime(chkDateTime))
                {
                    return true;
                }
            }
            return false;
        }

        #region [MDY:20220108] 用來檢查指定時間區間是否包含執行時間
        /// <summary>
        /// 取得指定時間區間是否有符合此服務項目的執行日期時間
        /// </summary>
        /// <param name="minTime">指定時間區間的起始時間</param>
        /// <param name="maxTime">指定時間區間的結束時間</param>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public bool HaveMyTime(HourMinute minTime, HourMinute maxTime)
        {
            if (this.Cycle != null && this.Cycle.IsReady())
            {
                return this.Cycle.CycleTime.HaveMyTime(minTime, maxTime);
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 將此物件資料寫入至指定 XmlWriter
        /// </summary>
        /// <param name="writer">指定 XmlWriter</param>
        public void WriteToXml(XmlWriter writer)
        {
            if (writer != null)
            {
                #region Root Start
                writer.WriteStartElement(RootLocalName);
                #endregion

                #region JobCubeType
                writer.WriteElementString("JobCubeType", this.JobCubeType ?? String.Empty);
                #endregion

                #region Cycle
                this.Cycle.WriteToXml(writer);
                #endregion

                #region AppFileName
                writer.WriteElementString("AppFileName", this.AppFileName ?? String.Empty);
                #endregion

                #region AppParameter
                writer.WriteElementString("AppParameter", this.AppArguments ?? String.Empty);
                #endregion

                #region Enabled
                writer.WriteElementString("Enabled", this.Enabled ? "Y" : "N");
                #endregion

                #region Root End
                writer.WriteEndElement();
                #endregion

                writer.Flush();
            }
        }

        /// <summary>
        /// 從指定 XmlNode 讀入資料至此物件
        /// </summary>
        /// <param name="xNode">指定 XmlNode</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        private bool ReadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null || xmlNode.LocalName != RootLocalName)
            {
                return false;
            }

            #region JobCubeType
            {
                XmlNode xNode = xmlNode.SelectSingleNode("JobCubeType");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                {
                    this.JobCubeType = xNode.InnerText.Trim();
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region CycleUnit
            {
                if (!this.Cycle.ReadFromXml(xmlNode))
                {
                    return false;
                }
            }
            #endregion

            #region AppFileName
            {
                XmlNode xNode = xmlNode.SelectSingleNode("AppFileName");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    this.AppFileName = xNode.InnerText.Trim();
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region AppParameter
            {
                XmlNode xNode = xmlNode.SelectSingleNode("AppParameter");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    this.AppArguments = xNode.InnerText.Trim();
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region Enabled
            {
                XmlNode xNode = xmlNode.SelectSingleNode("Enabled");
                if (xNode == null || xNode.NodeType != XmlNodeType.Element)
                {
                    this.Enabled = xNode.InnerText.Trim().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得作業類別名稱
        /// </summary>
        /// <returns></returns>
        public string GetJobCubeTypeName()
        {
            return JobCubeTypeCodeTexts.GetText(_JobCubeType);
        }

        /// <summary>
        /// 排序用 Comparison 方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int Comparison(ServiceConfig2 x, ServiceConfig2 y)
        {
            return x.JobCubeType.CompareTo(y.JobCubeType);
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 載入指定 XmlNode 取得 ServiceConfig2 物件
        /// </summary>
        /// <param name="xmlNode">指定 XmlNode</param>
        /// <returns>成功則傳回 ServiceConfig2 物件，否則傳回 null</returns>
        public static ServiceConfig2 LoadXmlNode(XmlNode xmlNode)
        {
            ServiceConfig2 config = null;

            if (xmlNode != null && xmlNode.LocalName == RootLocalName)
            {
                config = new ServiceConfig2();

                #region JobCubeType
                {
                    XmlNode xNode = xmlNode.SelectSingleNode("JobCubeType");
                    if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                    {
                        config.JobCubeType = xNode.InnerText.Trim();
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

                #region Cycle
                {
                    XmlNode xNode = xmlNode.SelectSingleNode(DaysCycle.XmlNodeName);
                    if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                    {
                        config.Cycle = DaysCycle.LoadXmlNode<DaysCycle>(xNode);
                    }
                    if (config.Cycle == null)
                    {
                        return null;
                    }
                }
                #endregion

                #region AppFileName
                {
                    XmlNode xNode = xmlNode.SelectSingleNode("AppFileName");
                    if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                    {
                        config.AppFileName = xNode.InnerText.Trim();
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

                #region AppParameter
                {
                    XmlNode xNode = xmlNode.SelectSingleNode("AppParameter");
                    if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                    {
                        config.AppArguments = xNode.InnerText.Trim();
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion

                #region Enabled
                {
                    XmlNode xNode = xmlNode.SelectSingleNode("Enabled");
                    if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                    {
                        config.Enabled = xNode.InnerText.Trim().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion
            }

            return config;
        }
        #endregion
    }
    #endregion
}
