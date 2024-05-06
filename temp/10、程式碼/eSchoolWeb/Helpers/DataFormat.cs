using System;
using System.Text.RegularExpressions;

using Fuju;

namespace eSchoolWeb
{
    /// <summary>
    /// 資料格式化抽象類別
    /// </summary>
    public abstract class DataFormat : Entities.DataFormat
    {
        #region 項目文字格式化相關
        /// <summary>
        /// 取得項目的格式化文字
        /// </summary>
        /// <param name="item">指定項目資料的代碼文字對照物件。</param>
        /// <param name="showValue">指定項目的文字是否需要顯示項目的值。</param>
        /// <param name="needLocalized">指定項目的文字是否需要做 Localized 處理。</param>
        /// <param name="spaceString">指定項目的文字後面附加的空白字串。</param>
        /// <returns>成功則傳回項目的文字，否則傳回空字串。</returns>
        public static string GetItemText(CodeText item, bool showValue, bool needLocalized, string spaceString)
        {
            if (item != null)
            {
                string txt = needLocalized ? WebHelper.GetLocalized(item.Text) : item.Text;
                if (String.IsNullOrEmpty(spaceString))
                {
                    return showValue ? String.Concat(item.Code, " - ", txt) : txt;
                }
                else
                {
                    return showValue ? String.Concat(item.Code, " - ", txt, spaceString) : String.Concat(txt, spaceString);
                }
            }
            return String.Empty;
        }
        #endregion

        #region 日期文字格式化相關
        /// <summary>
        /// 取得日期的格式化文字
        /// </summary>
        /// <param name="date">指定日期。</param>
        /// <returns>傳回日期的格式化文字。</returns>
        public static string GetDateText(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// 取得日期字串的格式化文字
        /// </summary>
        /// <param name="dateText">指定日期字串</param>
        /// <param name="returnEmpty">指定如果 dateText 為非日期字串是否傳回空字串，是則傳回空字串，否則傳回 dateText</param>
        /// <returns>傳回日期的格式化文字</returns>
        public static string FormatDateText(string dateText, bool returnEmpty = true)
        {
            DateTime? date = ConvertDateText(dateText);
            if (date == null)
            {
                return returnEmpty ? String.Empty : dateText;
            }
            else
            {
                return GetDateText(date.Value);
            }
        }

        /// <summary>
        /// 取得 Date8 日期的格式文字
        /// </summary>
        /// <param name="dateTxt">指定 Date8 日期。</param>
        /// <returns>傳回日期的格式字串。</returns>
        public static string GetDate8Text(string dateTxt)
        {
            DateTime date;
            dateTxt = dateTxt == null ? String.Empty : dateTxt.Trim();
            if (Common.TryConvertDate8(dateTxt, out date))
            {
                return GetDateText(date);
            }
            return dateTxt;
        }

        /// <summary>
        /// 取得 TWDate7 日期的格式文字
        /// </summary>
        /// <param name="twDate7">指定 TWDate7 日期。</param>
        /// <returns>傳回日期的格式字串。</returns>
        public static string ConvertTWDate7ToDate(string twDate7)
        {
            DateTime date;
            twDate7 = twDate7 == null ? String.Empty : twDate7.Trim();
            if (Common.TryConvertTWDate7(twDate7, out date))
            {
                return GetDateText(date);
            }
            return twDate7;
        }

        /// <summary>
        /// 取得日期時間的格式文字
        /// </summary>
        /// <param name="date8">指定日期時間。</param>
        /// <returns>傳回日期時間的格式字串。</returns>
        public static string GetDateTimeText(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 轉換日期格式文字成 Date8 日期格式文字
        /// </summary>
        /// <param name="dateText">日期格式文字</param>
        /// <param name="failToEmpty">指定如果無法處理是否則傳回空字串</param>
        /// <returns>傳回Date8 日期格式文字</returns>
        public static string ConvertDateToDate8(string dateText, bool failToEmpty)
        {
            DateTime date;
            dateText = dateText == null ? String.Empty : dateText.Trim();
            if (DateTime.TryParse(dateText, out date))
            {
                return Common.GetDate8(date);
            }
            return (failToEmpty) ? String.Empty : dateText;
        }

        /// <summary>
        /// 轉換日期格式文字成 TWDate7 日期 (民國年3碼+2碼月+2碼日) 格式文字
        /// </summary>
        /// <param name="dateText">日期格式文字</param>
        /// <param name="failToEmpty">指定如果無法處理是否則傳回空字串</param>
        /// <returns>傳回 TWDate7 日期格式文字</returns>
        public static string ConvertDateToTWDate7(string dateText, bool failToEmpty)
        {
            DateTime date;
            dateText = dateText == null ? String.Empty : dateText.Trim();
            if (DateTime.TryParse(dateText, out date))
            {
                return Common.GetTWDate7(date);
            }
            return (failToEmpty) ? String.Empty : dateText;
        }
        #endregion

        #region 金額文字格式化相關
        /// <summary>
        /// 取得金額的格式化文字
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string GetAmountText(decimal amount)
        {
            //return (amount / 1.000000000000000000000000000000000m).ToString();
            return amount.ToString("0.############################");
        }

        /// <summary>
        /// 取得金額的格式化文字
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string GetAmountText(decimal? amount)
        {
            return amount == null ? String.Empty : GetAmountText(amount.Value);
        }

        /// <summary>
        /// 取得金額的格式化文字
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string GetAmountCommaText(decimal? amount)
        {
            return amount == null ? String.Empty : amount.Value.ToString("#,0.############################");
        }
        #endregion

        #region 整數文字格式化相關
        public static string GetIntegerText(decimal value)
        {
            return value.ToString("0");
        }

        public static string GetIntegerText(decimal? value)
        {
            return value == null ? String.Empty : value.Value.ToString("0");
        }
        #endregion
    }
}