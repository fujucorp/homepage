using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 週期時間類別
    /// </summary>
    [Serializable]
    public class CycleTime
    {
        #region Static Readonly
        /// <summary>
        /// 最小週期間隔分鐘數 (1)
        /// </summary>
        public static readonly int MinInterval = 0;

        /// <summary>
        /// 最大週期間隔分鐘數 (1440)
        /// </summary>
        public static readonly int MaxInterval = 1440;
        #endregion

        #region Member
        /// <summary>
        /// 所有時間集合
        /// </summary>
        private List<HourMinute> _TimeList = null;

        /// <summary>
        /// 起始結束時間已 Ready 旗標
        /// </summary>
        private bool _IsReadyFlag = false;
        #endregion

        #region Property
        private HourMinute _StartTime;
        /// <summary>
        /// 週期起始時間
        /// </summary>
        public virtual HourMinute StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                _TimeList = null;
                _IsReadyFlag = false;
            }
        }

        private HourMinute _EndTime;
        /// <summary>
        /// 週期結束時間
        /// </summary>
        public virtual HourMinute EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
                _TimeList = null;
                _IsReadyFlag = false;
            }
        }

        private int _Interval = 0;
        /// <summary>
        /// 週期間隔分鐘數
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">指定值小於 CycleTime.MinInterval 或大於 CycleTime.MaxInterval</exception>
        public virtual int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                if (value < MinInterval || value > MaxInterval)
                {
                    throw new ArgumentOutOfRangeException("Interval", "指定值小於 CycleTime.MinInterval 或大於 CycleTime.MaxInterval");
                }
                _Interval = value;
                _TimeList = null;
            }
        }
        #endregion

        #region Readonly Property
        /// <summary>
        /// 取得週期時間訊息
        /// </summary>
        [XmlIgnore]
        public virtual string Info
        {
            get
            {
                return String.Format("{0} ~ {1}，間隔 {2} 分鐘", this.StartTime, this.EndTime, this.Interval);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 週期時間類別 物件
        /// </summary>
        public CycleTime()
        {
            _IsReadyFlag = false;
        }

        /// <summary>
        /// 建構 週期時間類別 物件，且指定的起始時間如果大於結束時間則自動互換
        /// </summary>
        /// <param name="startTime">指定週期起始時間</param>
        /// <param name="endTime">指定週期結束時間</param>
        /// <param name="interval">指定週期間隔分鐘數</param>
        /// <exception cref="System.ArgumentOutOfRangeException">指定值小於 CycleTime.MinInterval 或大於 CycleTime.MaxInterval</exception>
        public CycleTime(HourMinute startTime, HourMinute endTime, int interval)
        {
            if (startTime.Value <= endTime.Value)
            {
                this.StartTime = startTime;
                this.EndTime = endTime;
            }
            else
            {
                this.StartTime = endTime;
                this.EndTime = startTime;
            }
            this.Interval = interval;
            _IsReadyFlag = true;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此物件的 StartTime 是否小於等於 EndTime
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public virtual bool IsReady()
        {
            _IsReadyFlag = (this.StartTime.Value <= this.EndTime.Value);
            return _IsReadyFlag;
        }

        /// <summary>
        /// 如果 StartTime 大於 EndTime 則將兩值互換
        /// </summary>
        public virtual void ToReady()
        {
            if (this.StartTime.Value > this.EndTime.Value)
            {
                TimeSpan sTime = this.EndTime.Value;
                TimeSpan eTime = this.StartTime.Value;
                this.StartTime = new HourMinute(sTime);
                this.EndTime = new HourMinute(eTime);
            }
            _IsReadyFlag = true;
        }

        /// <summary>
        /// 取得由小到大排序過得所有時間集合
        /// </summary>
        /// <returns>傳回時間集合</returns>
        protected virtual List<HourMinute> GetTimeList()
        {
            if (_TimeList == null || _TimeList.Count == 0)
            {
                if (!_IsReadyFlag)
                {
                    this.ToReady();
                }

                if (this.Interval == 0)
                {
                    //沒有間隔分鐘數，表示只有開始時間的一次
                    _TimeList = new List<HourMinute>(1);
                    _TimeList.Add(this.StartTime);
                }
                else
                {
                    int count = Convert.ToInt32(Math.Ceiling((this.EndTime.Value - this.StartTime.Value).TotalMinutes / this.Interval)) + 1;
                    _TimeList = new List<HourMinute>(count);
                    DateTime chkTime = DateTime.MinValue.SetHourMinute(this.StartTime);
                    DateTime maxTime = DateTime.MinValue.SetHourMinute(this.EndTime);
                    while (chkTime <= maxTime)
                    {
                        HourMinute hm = chkTime.GetHourMinute();
                        _TimeList.Add(hm);
                        chkTime = chkTime.AddMinutes(this.Interval);
                    }
                }
            }
            return _TimeList;
        }

        /// <summary>
        /// 取得由小到大排序過得所有時間集合
        /// </summary>
        /// <returns>傳回時間集合</returns>
        public virtual HourMinute[] GetHourMinutes()
        {
            List<HourMinute> list = GetTimeList();
            return list.ToArray();
        }

        /// <summary>
        /// 取得最後的時間 (週期時間集合最後一筆)
        /// </summary>
        /// <returns>傳回最後的週期時間</returns>
        public virtual HourMinute GetLastHourMinute()
        {
            List<HourMinute> list = this.GetTimeList();
            if (list.Count == 0)
            {
                return this.StartTime;
            }
            else
            {
                return list[list.Count - 1];
            }
        }

        /// <summary>
        /// 取得指定日期的由小到大排序過得所有週期日期時間集合
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>傳回週期日期時間集合</returns>
        public virtual DateTime[] GetDateTimes(DateTime date)
        {
            List<HourMinute> hms = this.GetTimeList();
            List<DateTime> list = new List<DateTime>(hms.Count);
            foreach (HourMinute hm in hms)
            {
                list.Add(date.SetHourMinute(hm));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 取得指定日期的首筆週期日期時間
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>傳回首筆週期日期時間</returns>
        public virtual DateTime GetFirstDateTime(DateTime date)
        {
            return date.SetHourMinute(this.StartTime);
        }

        /// <summary>
        /// 取得指定日期的尾筆週期日期時間
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>傳回尾筆週期日期時間</returns>
        public virtual DateTime GetLastDateTime(DateTime date)
        {
            return date.SetHourMinute(this.GetLastHourMinute());
        }

        /// <summary>
        /// 取得大於或等於指定比較日期時間的最小週期日期時間
        /// </summary>
        /// <param name="chkTime">指定比較日期時間</param>
        /// <returns>傳回大於或等於指定比較日期時間的最小週期日期時間</returns>
        public virtual DateTime GetCeilingDateTime(DateTime chkTime)
        {
            DateTime checkTime = chkTime.TruncSecond();
            if (!_IsReadyFlag)
            {
                this.ToReady();
            }

            #region 先跟首筆週期日期時間比
            {
                DateTime firstTime = this.GetFirstDateTime(checkTime);
                if (checkTime <= firstTime)
                {
                    //checkTime 小於等於首筆始時間，所以 >= checkTime 的最小值為同一日的起始時間
                    return firstTime;
                }
            }
            #endregion

            #region 再跟尾筆週期日期時間比
            {
                DateTime lastTime = this.GetLastDateTime(checkTime);
                if (checkTime == lastTime)
                {
                    return lastTime;
                }
                if (checkTime > lastTime)
                {
                    //checkTime 大於尾筆時間，所以 >= checkTime 的最小值為隔一日的起始時間
                    return this.GetFirstDateTime(checkTime.AddDays(1));
                }
            }
            #endregion

            #region 介於起始與結束時間之間
            {
                List<HourMinute> list = this.GetTimeList();
                TimeSpan chkTS = checkTime.GetHourMinute().Value;
                HourMinute myHM = list.Find(x => x.Value >= chkTS);  //找出第1個大於等於的時間
                return checkTime.SetHourMinute(myHM);
            }
            #endregion
        }

        /// <summary>
        /// 取得小於或等於指定比較日期時間的最大週期日期時間
        /// </summary>
        /// <param name="chkTime">指定比較日期時間</param>
        /// <returns>傳回小於或等於指定比較日期時間的最大週期日期時間</returns>
        public virtual DateTime GetFloorDateTime(DateTime chkTime)
        {
            DateTime checkTime = chkTime.TruncSecond();
            if (!_IsReadyFlag)
            {
                this.ToReady();
            }

            #region 先跟首筆週期日期時間比
            {
                DateTime firstTime = this.GetFirstDateTime(checkTime);
                if (checkTime == firstTime)
                {
                    return firstTime;
                }
                if (checkTime < firstTime)
                {
                    //checkTime 小於首筆時間，所以 <= chkDate 的最大值為前一日的結束時間
                    return this.GetLastDateTime(checkTime.AddDays(-1));
                }
            }
            #endregion

            #region 再跟尾筆週期日期時間比
            {
                DateTime lastTime = this.GetLastDateTime(checkTime);
                if (checkTime >= lastTime)
                {
                    return lastTime;
                }
            }
            #endregion

            #region 介於起始與結束時間之間
            {
                List<HourMinute> list = this.GetTimeList();
                TimeSpan chkTS = checkTime.GetHourMinute().Value;
                HourMinute myHM = list.FindLast(x => x.Value <= chkTS);  //找出從後面算起第1個小於或等於的時間
                return checkTime.SetHourMinute(myHM);
            }
            #endregion
        }

        /// <summary>
        /// 取得指定時間是否符合此週期時間設定
        /// </summary>
        /// <param name="chkTime">指定時間</param>
        /// <returns>是則回 true，否則傳回 false</returns>
        public virtual bool IsMyTime(DateTime chkTime)
        {
            List<HourMinute> list = this.GetTimeList();
            if (list != null && list.Count > 0)
            {
                HourMinute chkHM = chkTime.GetHourMinute();
                if (list.FindIndex(x => x == chkHM) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        #region [MDY:20220108] 用來檢查指定時間區間是否包含週期時間
        /// <summary>
        /// 取得指定時間區間是否有符合在此週期時間設定
        /// </summary>
        /// <param name="minTime">指定時間區間的起始時間</param>
        /// <param name="maxTime">指定時間區間的結束時間</param>
        /// <returns>有則傳回 true，否則傳回 false</returns>
        public virtual bool HaveMyTime(HourMinute minTime, HourMinute maxTime)
        {
            List<HourMinute> list = this.GetTimeList();
            if (list != null && list.Count > 0)
            {
                return (list.FindIndex(x => x >= minTime && x <= maxTime) > -1);
            }
            return false;
        }
        #endregion
        #endregion

        #region Static Method
        /// <summary>
        /// 嘗試解析指定字串，轉成 CycleTime 物件
        /// </summary>
        /// <param name="text">指定字串</param>
        /// <param name="time">成功則傳回 CycleTime 物件，否則傳回 null</param>
        /// <returns>失敗傳回 false，否則傳回 true</returns>
        public static bool TryParse(string text, out CycleTime time)
        {
            time = null;
            string[] values = text.Split(new char[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 3)
            {
                HourMinute startTime, endTime;
                int interval;
                if (HourMinute.TryParse(values[0], out startTime)
                    && HourMinute.TryParse(values[1], out endTime)
                    && (Int32.TryParse(values[2], out interval) && interval >= MinInterval && interval <= MaxInterval))
                {
                    time = new CycleTime(startTime, endTime, interval);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 解析指定字串，轉成 CycleTime 物件
        /// </summary>
        /// <param name="text">指定字串</param>
        /// <returns>傳回 HourMinute 結構</returns>
        /// <exception cref="System.FormatException">指定值不是有效的 CycleTime 字串表示</exception>
        public static CycleTime Parse(string text)
        {
            string[] values = text.Split(new char[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 3)
            {
                HourMinute startTime, endTime;
                int interval;
                if (HourMinute.TryParse(values[0], out startTime)
                    && HourMinute.TryParse(values[1], out endTime)
                    && (Int32.TryParse(values[2], out interval) && interval >= MinInterval && interval <= MaxInterval))
                {
                    return new CycleTime(startTime, endTime, interval);
                }
            }
            throw new FormatException("指定值不是有效的 CycleTime 字串表示");
        }
        #endregion

        #region Override Object's Method
        /// <summary>
        /// 取得此物件的字串表示
        /// </summary>
        /// <returns>傳回表示字串</returns>
        public override string ToString()
        {
            return String.Format("{0}-{1},{2}", this.StartTime, this.EndTime, this.Interval);
        }
        #endregion
    }

    /// <summary>
    /// 週期資料承載抽象類別
    /// </summary>
    [Serializable]
    public abstract class Cycle
    {
        #region Static Readonly
        /// <summary>
        /// Xml 節點名稱 ： Cycle
        /// </summary>
        public static readonly string XmlNodeName = "Cycle";

        /// <summary>
        /// 週期種類 屬性名稱 ： kind
        /// </summary>
        public static readonly string KindAttributName = "kind";
        #endregion

        #region Abstract Property
        /// <summary>
        /// 週期種類
        /// </summary>
        [XmlIgnore]
        public abstract string Kind
        {
            get;
        }

        /// <summary>
        /// 週期訊息
        /// </summary>
        [XmlIgnore]
        public abstract string Info
        {
            get;
        }
        #endregion

        #region Abstract Method
        /// <summary>
        /// 取得指定日期是否符合此週期設定的日期
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public abstract bool IsMyDate(DateTime date);

        /// <summary>
        /// 取得第一個週期日期
        /// </summary>
        /// <returns>傳回週期日期 或 null</returns>
        public abstract DateTime? GetFirstCycleDate();

        /// <summary>
        /// 取得最後的週期日期
        /// </summary>
        /// <returns>傳回週期日期 或 null</returns>
        public abstract DateTime? GetLastCycleDate();

        /// <summary>
        /// 取得大於或等於指定比較日期的最小週期日期
        /// </summary>
        /// <param name="chkDate">指定比較日期</param>
        /// <returns>傳回週期日期</returns>
        public abstract DateTime? GetCycleDateCeiling(DateTime chkDate);

        /// <summary>
        /// 取得小於或等於指定比較日期的最大週期日期
        /// </summary>
        /// <param name="chkDate">指定比較日期</param>
        /// <returns>傳回週期日期</returns>
        public abstract DateTime? GetCycleDateFloor(DateTime chkDate);

        /// <summary>
        /// 繼承類別自己額外屬性的 ReadFromXml
        /// </summary>
        /// <param name="xmlNode">指定 XmlNode</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        protected abstract bool MyReadFromXml(XmlNode xmlNode);

        /// <summary>
        /// 繼承類別自己額外屬性的 WriteToXml
        /// </summary>
        /// <param name="writer">指定 XmlWriter</param>
        protected abstract void MyWriteToXml(XmlWriter writer);
        #endregion

        #region Property
        private DateTime? _StartDate = null;
        /// <summary>
        /// 週期的開始日期 (去除時間的部份)
        /// </summary>
        public virtual DateTime? StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                if (value == null)
                {
                    _StartDate = null;
                }
                else
                {
                    _StartDate = value.Value.Date;
                }
            }
        }

        private DateTime? _EndDate = null;
        /// <summary>
        /// 週期的結束日期 (去除時間的部份)
        /// </summary>
        public virtual DateTime? EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                if (value == null)
                {
                    _EndDate = null;
                }
                else
                {
                    _EndDate = value.Value.Date;
                }
            }
        }

        /// <summary>
        /// 週期日的週期時間
        /// </summary>
        public virtual CycleTime CycleTime
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 週期資料承載抽象類別 物件
        /// </summary>
        protected Cycle()
        {

        }

        /// <summary>
        /// 建構 週期資料基底類別 物件
        /// </summary>
        /// <param name="startDate">週期開始日期</param>
        /// <param name="endDate">週期結束日期</param>
        /// <param name="cycleTime">週期日的週期時間</param>
        protected Cycle(DateTime? startDate, DateTime? endDate, CycleTime cycleTime)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.CycleTime = cycleTime;
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得此物件的資料是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public virtual bool IsReady()
        {
            return (this.IsDateReady() && this.CycleTime != null && this.CycleTime.IsReady());
        }

        /// <summary>
        /// 取得此物件的日期資料是否準備好
        /// </summary>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public virtual bool IsDateReady()
        {
            return (this.StartDate != null && (this.EndDate == null || this.StartDate.Value <= this.EndDate.Value));
        }

        /// <summary>
        /// 從指定 XmlNode 讀入資料至此物件
        /// </summary>
        /// <param name="xmlNode">指定 XmlNode</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public virtual bool ReadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null || xmlNode.LocalName != XmlNodeName)
            {
                return false;
            }

            #region Kind
            {
                string kind = null;
                XmlAttribute xAttr = xmlNode.Attributes[KindAttributName];
                if (xAttr != null)
                {
                    kind = xAttr.Value.Trim();
                }
                if (kind != this.Kind)
                {
                    return false;
                }
            }
            #endregion

            #region StartDate
            {
                XmlNode xNode = xmlNode.SelectSingleNode("StartDate");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                {
                    string txt = xNode.InnerText.Trim();
                    if (!String.IsNullOrEmpty(txt))
                    {
                        DateTime date;
                        if (DateTime.TryParse(txt, out date))
                        {
                            this.StartDate = date;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region EndDate
            {
                XmlNode xNode = xmlNode.SelectSingleNode("EndDate");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                {
                    string txt = xNode.InnerText.Trim();
                    if (!String.IsNullOrEmpty(txt))
                    {
                        DateTime date;
                        if (DateTime.TryParse(txt, out date))
                        {
                            this.EndDate = date;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region CycleTime
            {
                XmlNode xNode = xmlNode.SelectSingleNode("CycleTime");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element)
                {
                    string txt = xNode.InnerText.Trim();
                    if (!String.IsNullOrEmpty(txt))
                    {
                        CycleTime time;
                        if (CycleTime.TryParse(txt, out time))
                        {
                            this.CycleTime = time;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            #endregion

            #region MyReadFromXml
            return this.MyReadFromXml(xmlNode);
            #endregion
        }

        /// <summary>
        /// 將此物件資料寫入至指定 XmlWriter
        /// </summary>
        /// <param name="writer">指定 XmlWriter</param>
        public virtual void WriteToXml(XmlWriter writer)
        {
            #region Cycle Start
            writer.WriteStartElement(XmlNodeName);
            #endregion

            #region Kind
            writer.WriteAttributeString(KindAttributName, this.Kind ?? String.Empty);
            #endregion

            #region StartDate
            writer.WriteElementString("StartDate", this.StartDate == null ? String.Empty : this.StartDate.Value.ToString("yyyy/MM/dd"));
            #endregion

            #region EndDate
            writer.WriteElementString("EndDate", this.EndDate == null ? String.Empty : this.EndDate.Value.ToString("yyyy/MM/dd"));
            #endregion

            #region CycleTime
            writer.WriteElementString("CycleTime", this.CycleTime == null ? String.Empty : this.CycleTime.ToString());
            #endregion

            #region MyWriteToXml
            this.MyWriteToXml(writer);
            #endregion

            #region Cycle Ending
            writer.WriteEndElement();
            #endregion
        }

        /// <summary>
        /// 取得指定日期時間是否符合此週期設定的日期與時間
        /// </summary>
        /// <param name="chkTime">指定日期時間</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public virtual bool IsTimeUp(DateTime chkTime)
        {
            if (this.CycleTime != null && this.IsMyDate(chkTime))
            {
                return this.CycleTime.IsMyTime(chkTime);
            }
            return false;
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 載入指定 XmlNode 取得指定 Cycle 泛型物件
        /// </summary>
        /// <typeparam name="T">指定 Cycle 泛型</typeparam>
        /// <param name="xmlNode">指定 XmlNode </param>
        /// <returns>成功則傳回指定 Cycle 泛型物件，否則傳回 null</returns>
        public static T LoadXmlNode<T>(XmlNode xmlNode) where T : Cycle, new()
        {
            T cycle = new T();
            if (cycle.ReadFromXml(xmlNode))
            {
                return cycle;
            }
            return null;
        }
        #endregion
    }

    /// <summary>
    /// 日間隔週期資料承載類別
    /// </summary>
    [Serializable]
    public class DaysCycle : Cycle
    {
        #region Const
        /// <summary>
        /// 此類別的週期種類常數： Days
        /// </summary>
        public const string KindName = "Days";
        #endregion

        #region Static Readonly
        /// <summary>
        /// 最小週期間隔日數 (0)
        /// </summary>
        public static readonly int MinInterval = 0;

        /// <summary>
        /// 最大週期間隔日數 (999)
        /// </summary>
        public static readonly int MaxInterval = 999;
        #endregion

        #region Implement Cycle's Abstract Property
        /// <summary>
        /// 週期種類
        /// </summary>
        [XmlIgnore]
        public override string Kind
        {
            get
            {
                return KindName;
            }
        }

        /// <summary>
        /// 週期訊息
        /// </summary>
        [XmlIgnore]
        public override string Info
        {
            get
            {
                return String.Format("{0} ~ {1}，間隔 {2} 日"
                    , this.StartDate == null ? "未指定" : this.StartDate.Value.ToString("yyyy/MM/dd")
                    , this.EndDate == null ? "未指定" : this.EndDate.Value.ToString("yyyy/MM/dd")
                    , this.DaysInterval);
            }
        }
        #endregion

        #region Implement Cycle's Abstract Method
        /// <summary>
        /// 取得指定日期是否符合此週期設定的日期 (日期資料必須準備好)
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public override bool IsMyDate(DateTime date)
        {
            DateTime? firstDate = this.GetFirstCycleDate();
            if (firstDate != null)
            {
                DateTime checkDate = date.Date;
                if (checkDate == firstDate.Value)
                {
                    return true;
                }
                if (checkDate > firstDate.Value && (this.EndDate == null || checkDate <= this.EndDate.Value) && this.DaysInterval > 0)
                {
                    return (((checkDate - firstDate.Value).TotalDays % this.DaysInterval) == 0);
                }
            }
            return false;
        }

        /// <summary>
        /// 取得第一個週期日期 (日資料必須準備好)
        /// </summary>
        /// <returns>傳回週期日期 或 null</returns>
        public override DateTime? GetFirstCycleDate()
        {
            //因為是日間隔，所以 StartDate 一定是第一個週期日期
            if (this.IsDateReady())
            {
                return this.StartDate;
            }
            return null;
        }

        /// <summary>
        /// 取得最後的週期日期
        /// </summary>
        /// <returns>傳回週期日期 或 null</returns>
        public override DateTime? GetLastCycleDate()
        {
            //有指定 EndDate，但不一定符合日間隔數，以無條件捨去計算小於等與 EndDate 的最大週期日期
            DateTime? firstDate = this.GetFirstCycleDate();
            if (firstDate != null)
            {
                if (this.DaysInterval == 0)
                {
                    //沒有間隔日，最後一天就是第一天
                    return firstDate;
                }
                else if (this.EndDate != null)
                {
                    int count = Convert.ToInt32((this.EndDate.Value - firstDate.Value).TotalDays / this.DaysInterval);
                    DateTime date = firstDate.Value.AddDays(this.DaysInterval * count);
                }
            }
            return null;
        }

        /// <summary>
        /// 取得大於或等於指定比較日期的最小週期日期
        /// </summary>
        /// <param name="chkDate">指定比較日期</param>
        /// <returns>傳回週期日期</returns>
        public override DateTime? GetCycleDateCeiling(DateTime chkDate)
        {
            DateTime? firstDate = this.GetFirstCycleDate();
            if (firstDate != null)
            {
                DateTime checkDate = chkDate.Date;

                #region 先跟第一個週期日期比
                if (checkDate <= firstDate.Value)
                {
                    return firstDate.Value;
                }
                #endregion

                #region 再跟最後的週期日期比
                DateTime? lastDate = this.GetLastCycleDate();
                if (lastDate != null)
                {
                    if (checkDate == lastDate.Value)
                    {
                        return lastDate.Value;
                    }
                    if (checkDate > lastDate.Value)
                    {
                        return null;
                    }
                }
                #endregion

                #region 介於第一個與最後的週期日期之間 (無條件進位)
                if (this.DaysInterval > 0)
                {
                    int days = Convert.ToInt32((checkDate - firstDate.Value).TotalDays);
                    if (days % this.DaysInterval == 0)
                    {
                        return checkDate;
                    }
                    else
                    {
                        int count = (days / this.DaysInterval) + 1;
                        DateTime date = firstDate.Value.AddDays(count * this.DaysInterval);
                        //因為是無條件進位，所以要檢查不能大於 lastDate
                        if (lastDate == null || date <= lastDate.Value)
                        {
                            return date;
                        }
                        else
                        {
                            return lastDate.Value;
                        }
                    }
                }
                #endregion
            }
            return null;
        }

        /// <summary>
        /// 取得小於或等於指定比較日期的最大週期日期
        /// </summary>
        /// <param name="chkDate">指定比較日期</param>
        /// <returns>傳回週期日期</returns>
        public override DateTime? GetCycleDateFloor(DateTime chkDate)
        {
            DateTime? firstDate = this.GetFirstCycleDate();
            if (firstDate != null)
            {
                DateTime checkDate = chkDate.Date;

                #region 先跟第一個週期日期比
                if (checkDate == firstDate.Value)
                {
                    return firstDate.Value;
                }
                if (checkDate < firstDate.Value)
                {
                    return null;
                }
                #endregion

                #region 再跟最後的週期日期比
                DateTime? lastDate = this.GetLastCycleDate();
                if (lastDate != null && checkDate >= lastDate.Value)
                {
                    return lastDate.Value;
                }
                #endregion

                #region 介於第一個與最後的週期日期之間 (無條件捨去)
                if (this.DaysInterval > 0)
                {
                    int days = Convert.ToInt32((checkDate - firstDate.Value).TotalDays);
                    int count = days / this.DaysInterval;
                    DateTime date = firstDate.Value.AddDays(count * this.DaysInterval);
                    //因為是無條件捨去，所以不會大於 lastDate
                    return date;
                }
                #endregion
            }
            return null;
        }

        /// <summary>
        /// 繼承類別自己額外屬性的 ReadFromXml
        /// </summary>
        /// <param name="xmlNode">指定 XmlNode</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        protected override bool MyReadFromXml(XmlNode xmlNode)
        {
            #region DaysInterval
            {
                int value;
                XmlNode xNode = xmlNode.SelectSingleNode("DaysInterval");
                if (xNode != null && xNode.NodeType == XmlNodeType.Element && Int32.TryParse(xNode.InnerText.Trim(), out value) && value >= MinInterval && value <= MaxInterval)
                {
                    this.DaysInterval = value;
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
        /// 繼承類別自己額外屬性的 WriteToXml
        /// </summary>
        /// <param name="writer">指定 XmlWriter</param>
        protected override void MyWriteToXml(XmlWriter writer)
        {
            #region DaysInterval
            writer.WriteElementString("DaysInterval", this.DaysInterval.ToString());
            #endregion
        }
        #endregion

        #region Property
        private int _DaysInterval = 0;
        /// <summary>
        /// 週期的日間隔數
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">指定值小於 DaysCycle.MinInterval 或大於 DaysCycle.MaxInterval</exception>
        public virtual int DaysInterval
        {
            get
            {
                return _DaysInterval;
            }
            set
            {
                if (value < MinInterval || value > MaxInterval)
                {
                    throw new System.ArgumentOutOfRangeException("DaysInterval", "指定值小於 DaysCycle.MinInterval 或大於 DaysCycle.MaxInterval");
                }
                _DaysInterval = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 日間隔週期資料承載類別 物件
        /// </summary>
        public DaysCycle()
        {

        }

        /// <summary>
        /// 建構 日間隔週期資料承載類別 物件
        /// </summary>
        /// <param name="startDate">週期的開始日期</param>
        /// <param name="endDate">週期的結束日期</param>
        /// <param name="daysInterval">週期的日間隔數</param>
        /// <param name="startTime">週期日的週期時間開始時間</param>
        /// <param name="endTime">週期日的週期時間結束時間</param>
        /// <param name="timeInterval">週期日的週期時間間隔數</param>
        /// <exception cref="System.ArgumentOutOfRangeException">daysInterval 指定值小於 DaysCycle.MinInterval 或大於 DaysCycle.MaxInterval</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">timeInterval 指定值小於 CycleTime.MinInterval 或大於 CycleTime.MaxInterval</exception>
        public DaysCycle(DateTime startDate, DateTime? endDate, int daysInterval, HourMinute startTime, HourMinute endTime, int timeInterval)
            : this(startDate, endDate, daysInterval, new CycleTime(startTime, endTime, timeInterval))
        {
        }

        /// <summary>
        /// 建構 日間隔週期資料承載類別 物件
        /// </summary>
        /// <param name="startDate">週期的開始日期</param>
        /// <param name="endDate">週期的結束日期</param>
        /// <param name="daysInterval">週期的日間隔數</param>
        /// <param name="cycleTime">週期日的週期時間</param>
        /// <exception cref="System.ArgumentOutOfRangeException">daysInterval 指定值小於 CycleTime.MinInterval 或大於 CycleTime.MaxInterval</exception>
        public DaysCycle(DateTime startDate, DateTime? endDate, int daysInterval, CycleTime cycleTime)
            : base(startDate, endDate, cycleTime)
        {
            this.DaysInterval = daysInterval;
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 載入指定 XmlNode 取得 DaysCycle 物件
        /// </summary>
        /// <param name="xmlNode">指定 XmlNode </param>
        /// <returns>成功則傳回 DaysCycle 物件，否則傳回 null</returns>
        public static DaysCycle LoadXmlNode(XmlNode xmlNode)
        {
            DaysCycle cycle = new DaysCycle();
            if (cycle.ReadFromXml(xmlNode))
            {
                return cycle;
            }
            return null;
        }
        #endregion
    }
}
