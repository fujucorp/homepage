using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Entities
{
    /// <summary>
    /// 時分結構型別
    /// </summary>
    [Serializable]
    public struct HourMinute : IComparable, IXmlSerializable, IComparable<HourMinute>, IEquatable<HourMinute>
    {
        #region Static Readonly
        /// <summary>
        /// 每日小時數 (24)
        /// </summary>
        private static readonly int _HoursPerDay = 24;

        /// <summary>
        /// 每日分鐘數 (1440)
        /// </summary>
        private static readonly int _MinutesPerDay = 1440;

        /// <summary>
        /// 時分字串的規則運算式
        /// </summary>
        private static readonly Regex _HHMMRegex = new Regex("^([0-1][0-9]|2[0-3]):([0-5][0-9])$", RegexOptions.Compiled);

        /// <summary>
        /// 最小值
        /// </summary>
        public static readonly HourMinute MinValue = new HourMinute(0, 0);

        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly HourMinute MaxValue = new HourMinute(23, 59);
        #endregion

        #region Member
        /// <summary>
        /// 儲存時分實際值的 TimeSpan 型別變數
        /// </summary>
        private TimeSpan _Value;
        #endregion

        #region Property
        /// <summary>
        /// 取得小時的值
        /// </summary>
        [XmlIgnore]
        public int Hour
        {
            get
            {
                return _Value.Hours;
            }
        }

        /// <summary>
        /// 取得分鐘的值
        /// </summary>
        [XmlIgnore]
        public int Minute
        {
            get
            {
                return _Value.Minutes;
            }
        }

        /// <summary>
        /// 取得 TimeSpan 表示值
        /// </summary>
        [XmlIgnore]
        public TimeSpan Value
        {
            get
            {
                return _Value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 建構 時分結構型別 實體
        /// </summary>
        /// <param name="value">指定 TimeSpan</param>
        public HourMinute(TimeSpan value)
            : this(value.Hours, value.Minutes)
        {

        }

        /// <summary>
        /// 建構 時分結構型別 實體
        /// </summary>
        /// <param name="hours">指定小時的值</param>
        /// <param name="minutes">指定分鐘的值</param>
        public HourMinute(int hours, int minutes)
        {
            int value = ((hours % _HoursPerDay * 60) + (minutes % _MinutesPerDay) + _MinutesPerDay) % _MinutesPerDay;
            _Value = new TimeSpan(0, value, 0);
        }

        /// <summary>
        /// 建構 時分結構型別 實體
        /// </summary>
        /// <param name="dateTime">指定日期</param>
        public HourMinute(DateTime dateTime)
        {
            _Value = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
        }
        #endregion

        #region Method
        /// <summary>
        /// 取得加上指定 TimeSpan 後新的 HourMinute 實體
        /// </summary>
        /// <param name="value">指定 TimeSpan</param>
        /// <returns>如果 value 參數等於 0 小時 0 分鐘 則傳回自己，否則傳回新值的新的 HourMinute 實體</returns>
        private HourMinute Add(TimeSpan value)
        {
            int hours = value.Hours;
            int minutes = value.Minutes;
            if (hours == 0 && minutes == 0)
            {
                return this;
            }
            else
            {
                TimeSpan ts = this.Value.Add(new TimeSpan(hours, minutes, 0));
                return new HourMinute(ts);
            }
        }

        /// <summary>
        /// 取得加上指定小時數後新的 HourMinute 實體
        /// </summary>
        /// <param name="hours">指定小時數</param>
        /// <returns>如果 hours 參數為 0 則傳回自己，否則傳回新值的新的 HourMinute 實體</returns>
        public HourMinute AddHours(int hours = 0)
        {
            if (hours == 0)
            {
                return this;
            }
            else
            {
                TimeSpan ts = this.Value.Add(new TimeSpan(hours % _HoursPerDay, 0, 0));
                return new HourMinute(ts);
            }
        }

        /// <summary>
        /// 取得加上指定分鐘數後新的 HourMinute 實體
        /// </summary>
        /// <param name="minutes">指定分鐘數</param>
        /// <returns>如果 minutes 參數為 0 則傳回自己，否則傳回新值的新的 HourMinute 實體</returns>
        public HourMinute AddMinutes(int minutes = 0)
        {
            if (minutes == 0)
            {
                return this;
            }
            else
            {
                TimeSpan ts = this.Value.Add(new TimeSpan(0, minutes % _MinutesPerDay, 0));
                return new HourMinute(ts);
            }
        }

        /// <summary>
        /// 取得減去指定 TimeSpan 後新的 HourMinute 實體
        /// </summary>
        /// <param name="value">指定 TimeSpan</param>
        /// <returns>如果 value 參數等於 0 小時 0 分鐘 則傳回自己，否則傳回新值的新的 HourMinute 實體</returns>
        public HourMinute Subtract(TimeSpan value)
        {
            int hours = value.Hours;
            int minutes = value.Minutes;
            if (hours == 0 && minutes == 0)
            {
                return this;
            }
            else
            {
                TimeSpan ts = this.Value.Subtract(new TimeSpan(hours, minutes, 0));
                return new HourMinute(ts);
            }
        }

        /// <summary>
        /// 取得減去指定 HourMinute 後的 TimeSpan 實體 (兩者相差的小時與分鐘數，以 TimeSpan 表示)
        /// </summary>
        /// <param name="value">指定 HourMinute</param>
        /// <returns>傳回兩者相差的小時與分鐘數，以 TimeSpan 表示</returns>
        public TimeSpan Subtract(HourMinute value)
        {
            TimeSpan ts = this.Value.Subtract(value.Value);
            return ts;
        }
        #endregion

        #region Static Method
        /// <summary>
        /// 嘗試解析指定字串，轉成 HourMinute 實體
        /// </summary>
        /// <param name="text">指定字串</param>
        /// <param name="value">成功則傳回 HourMinute 實體，否則傳回 HourMinute.MinValue</param>
        /// <returns>成功則傳回 true，否則傳回 false</returns>
        public static bool TryParse(string text, out HourMinute value)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                Match m = _HHMMRegex.Match(text.Trim());
                if (m.Success)
                {
                    value = new HourMinute(Int32.Parse(m.Groups[1].Value), Int32.Parse(m.Groups[2].Value));
                    return true;
                }
            }
            value = HourMinute.MinValue;
            return false;
        }

        /// <summary>
        /// 解析指定字串，轉成 HourMinute 實體
        /// </summary>
        /// <param name="text">指定字串</param>
        /// <returns>成功則傳回 HourMinute 實體，否則丟出例外</returns>
        /// <exception cref="System.FormatException">指定值不是有效的 HourMinute 字串表示</exception>
        public static HourMinute Parse(string text)
        {
            Match m = _HHMMRegex.Match(text);
            if (m.Success)
            {
                HourMinute hm = new HourMinute(Int32.Parse(m.Groups[1].Value), Int32.Parse(m.Groups[2].Value));
                return hm;
            }
            throw new FormatException("指定值不是有效的 HourMinute 字串表示");
        }
        #endregion

        #region Overload Operator
        /// <summary>
        /// Overload Operator - (HourMinute 減去 HourMinute)
        /// </summary>
        /// <param name="a">被減數 HourMinute</param>
        /// <param name="b">減數 HourMinute</param>
        /// <returns>傳回兩者相差的小時與分鐘數，以 TimeSpan 表示</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static TimeSpan operator -(HourMinute a, HourMinute b)
        {
            return a.Subtract(b);
        }

        /// <summary>
        /// Overload Operator - (HourMinute 減去 TimeSpan)
        /// </summary>
        /// <param name="a">被減數 HourMinute</param>
        /// <param name="b">減數 TimeSpan</param>
        /// <returns>傳回兩者相差的小時與分鐘數，以 TimeSpan 表示</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static HourMinute operator -(HourMinute a, TimeSpan b)
        {
            return a.Subtract(b);
        }

        /// <summary>
        /// Overload Operator + (HourMinute 加上 TimeSpan)
        /// </summary>
        /// <param name="a">被加數 HourMinute</param>
        /// <param name="b">加數 TimeSpan</param>
        /// <returns>傳回兩者相差的小時與分鐘數，以 TimeSpan 表示</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static HourMinute operator +(HourMinute a, TimeSpan b)
        {
            return a.Add(b);
        }

        /// <summary>
        /// Overload Operator == (比較兩個 HourMinute 是否等於)
        /// </summary>
        /// <param name="a">被比較的第一個 HourMinute</param>
        /// <param name="b">被比較的第二個 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(HourMinute a, HourMinute b)
        {
            return a.Value.Equals(b.Value);
        }

        /// <summary>
        /// Overload Operator != (比較兩個 HourMinute 是否不等於)
        /// </summary>
        /// <param name="a">被比較的第一個 HourMinute</param>
        /// <param name="b">被比較的第二個 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(HourMinute a, HourMinute b)
        {
            return !a.Value.Equals(b.Value);
        }

        /// <summary>
        /// Overload Operator < (比較第一個 HourMinute 是否小於第二個 HourMinute)
        /// </summary>
        /// <param name="a">被比較的第一個 HourMinute</param>
        /// <param name="b">被比較的第二個 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator <(HourMinute a, HourMinute b)
        {
            return a.Value < b.Value;
        }

        /// <summary>
        /// Overload Operator <= (比較第一個 HourMinute 是否小於等於第二個 HourMinute)
        /// </summary>
        /// <param name="a">被比較的第一個 HourMinute</param>
        /// <param name="b">被比較的第二個 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator <=(HourMinute a, HourMinute b)
        {
            return a.Value <= b.Value;
        }

        /// <summary>
        /// Overload Operator > (比較第一個 HourMinute 是否大於第二個 HourMinute)
        /// </summary>
        /// <param name="a">被比較的第一個 HourMinute</param>
        /// <param name="b">被比較的第二個 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator >(HourMinute a, HourMinute b)
        {
            return a.Value > b.Value;
        }

        /// <summary>
        /// Overload Operator >= (比較第一個 HourMinute 是否大於等於第二個 HourMinute)
        /// </summary>
        /// <param name="a">被比較的第一個 HourMinute</param>
        /// <param name="b">被比較的第二個 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        [System.Runtime.TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator >=(HourMinute a, HourMinute b)
        {
            return a.Value >= b.Value;
        }
        #endregion

        #region Override Object's Method
        /// <summary>
        /// 取得此實體的字串表示
        /// </summary>
        /// <returns>傳回表示字串</returns>
        public override string ToString()
        {
            return String.Format("{0:00}:{1:00}", this.Hour, this.Minute);
        }

        /// <summary>
        /// 取得此實體是否等於指定物件
        /// </summary>
        /// <param name="value">指定物件</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public override bool Equals(object value)
        {
            if (value is HourMinute)
            {
                return this.Value.Equals(((HourMinute)value).Value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 傳回此實體的雜湊碼
        /// </summary>
        /// <returns>傳回雜湊碼</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion

        #region Implement IComparable & IComparable<HourMinute>, IEquatable<HourMinute>
        /// <summary>
        /// 取得此實體與指定 HourMinute 的比較結果
        /// </summary>
        /// <param name="value">指定 HourMinute</param>
        /// <returns>比 value 小則傳回 -1; 比 value 大則傳回 1; 相等則傳回 0</returns>
        public int CompareTo(HourMinute value)
        {
            return this.Value.CompareTo(value.Value);
        }

        /// <summary>
        /// 取得此實體與指定物件的比較結果
        /// </summary>
        /// <param name="value">指定物件</param>
        /// <returns>比 value 小則傳回 -1; 比 value 大或 value 為 null 則傳回 1; 相等則傳回 0</returns>
        /// <exception cref="System.ArgumentException">指定值不是 HourMinute 型別</exception>
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (value is HourMinute)
            {
                return this.Value.CompareTo(((HourMinute)value).Value);
            }
            else
            {
                throw new ArgumentException("指定值不是 HourMinute 型別", "value");
            }
        }

        /// <summary>
        /// 取得此實體是否等於指定 HourMinute
        /// </summary>
        /// <param name="value">指定 HourMinute</param>
        /// <returns>是則傳回 true，否則傳回 false</returns>
        public bool Equals(HourMinute value)
        {
            return this.Value.Equals(value.Value);
        }
        #endregion

        #region Implement IXmlSerializable
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            _Value = HourMinute.Parse(reader.ReadString()).Value;
            if (!reader.EOF)
            {
                reader.Read();
                reader.MoveToContent();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(this.ToString());
        }
        #endregion
    }

    /// <summary>
    /// DateTime 類別的擴充方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 取得此 DateTime 實體去除秒數後新的 DateTime 實體
        /// </summary>
        /// <param name="value">指定此 DateTime 實體</param>
        /// <returns>傳回去除秒數後新的 DateTime 實體</returns>
        public static DateTime TruncSecond(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
        }

        /// <summary>
        /// 取得此 DateTime 實體的 HourMinute 實體
        /// </summary>
        /// <param name="value">指定此 DateTime 實體</param>
        /// <returns>傳回 HourMinute 實體</returns>
        public static HourMinute GetHourMinute(this DateTime value)
        {
            return new HourMinute(value);
        }

        /// <summary>
        /// 取得指定 HourMinute 值後新的 DateTime 實體
        /// </summary>
        /// <param name="value">指定此 DateTime 實體</param>
        /// <param name="hm">指定 HourMinute 值</param>
        /// <returns>傳回指定 HourMinute 值後新的 DateTime 實體</returns>
        public static DateTime SetHourMinute(this DateTime value, HourMinute hm)
        {
            return new DateTime(value.Year, value.Month, value.Day, hm.Hour, hm.Minute, 0);
        }

        /// <summary>
        /// 取得與此 DateTime 實體同年月最後一天新的 DateTime 實體
        /// </summary>
        /// <param name="value">指定此 DateTime 實體</param>
        /// <returns>傳回新的 DateTime 實體</returns>
        public static DateTime GetLastDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        /// <summary>
        /// 取得與此 DateTime 實體同年月第一天新的 DateTime 實體
        /// </summary>
        /// <param name="value">指定此 DateTime 實體</param>
        /// <returns>傳回新的 DateTime 實體</returns>
        public static DateTime GetFirstDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
    }
}
