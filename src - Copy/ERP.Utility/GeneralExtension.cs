using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
//using System.Web.Http.ModelBinding;
using System.Web.ModelBinding;

namespace ERP.Utility
{
    public static class GeneralExtension
    {

        public static object HandleDBNull(this object value)
        {
            return value == DBNull.Value ? null : value;
        }
        public static bool ValidateIP(this string ip)
        {
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
                return true;
            else
                return false;
        }

        public static long NextItem(this List<long> list, long? currentItem = null)
        {
            if (currentItem == null)
            {
                return list.FirstOrDefault();
            }
            else
            {
                var currentIndex = list.FindIndex(x => x == currentItem);
                return list[currentIndex + 1];
            }
        }
        public static Expression<Func<T, bool>> ToExpression<T>(this Func<T, bool> f)
        {
            return x => f(x);
        }
        public static string ToCaseInsensitive(this string value)
        {
            return string.Concat(" =~ '(?i)", value, "'");
        }
        public static string ToTimeSpanString(this TimeSpan? value)
        {
            if (value != null)
            {
                if (value.Value.Days > 0)
                {
                    return string.Concat(value.Value.Days, ".", value.Value.Hours, ":", value.Value.Minutes);
                }
                return string.Concat(value.Value.Hours, ":", value.Value.Minutes);
            }
            return "0:0";
        }
        public static string ToTimeSpanString(this TimeSpan value)
        {
            if (value.Days > 0)
            {
                return string.Concat(value.Days, ".", value.Hours, ":", value.Minutes);
            }
            return string.Concat(value.Hours, ":", value.Minutes);
        }
        public static string ToCaseInsensitiveContains(this string value)
        {
            return string.Concat(" =~ \"(?i).*", value, ".*\" ");
        }
        public static string ToCaseInsensitiveStartsWith(this string value)
        {
            return string.Concat(" =~ \"(?i)", value, ".*\" ");
        }
        public static string ToCaseInsensitiveEndsWith(this string value)
        {
            return string.Concat(" =~ \"(?i).*", value, "\" ");
        }
        public static bool IsNotNull(this object value)
        {
            return value != null;
        }
        public static bool IsNullOrZero(this int? value)
        {
            return value == null || value == 0;
        }
        public static void Add(this List<KeyValuePair<string, string>> list, string key, string value)
        {
            list.Add(new KeyValuePair<string, string>(key, value));
        }
        public static Dictionary<string, object> AddItem(this Dictionary<string, object> list, string key, object value)
        {
            list.Add(key, value);
            return list;
        }
        public static Dictionary<string, object> AddIfNotExists(this Dictionary<string, object> list, string key, object value)
        {
            if (!list.ContainsKey(key))
            {
                list.Add(key, value);
            }
            return list;
        }
        public static bool IsTrue(this bool? value)
        {
            return value.HasValue && value.Value;
        }
        public static bool IsNotNullAndNotEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }
        public static TimeSpan? IgnoreSeconds(this TimeSpan? value)
        {
            if (value != null)
            {
                return TimeSpan.FromMinutes(Math.Round(value.Value.TotalMinutes, 0));
            }
            return value;
        }
        public static string DomainSafeName(this System.Security.Principal.IIdentity value)
        {
            return value.Name.ToLower().Replace(String.Concat(Constant.DomainName, "\\"), String.Empty).Replace("@dca.com", string.Empty).Replace("CHANHOME", string.Empty);
        }
        public static string SerializeErrors(this ModelStateDictionary modelState)
        {
            return string.Join("<br/>", modelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
        }
        public static string ModuleSafeName(this string value)
        {
            return value
                .Replace("ADM_", String.Empty)
                .Replace("HRS_", string.Empty)
                .Replace("NTS_", string.Empty)
                .Replace("GEN_", string.Empty)
                .Replace("PMS_", string.Empty)
                .Replace("SPS_", string.Empty)
                .Replace("TAA_", string.Empty)
                .Replace("PAY_", string.Empty)
                .Replace("PMT_", string.Empty)
                .Replace("MKT_", string.Empty)
                .Replace("CLK_", string.Empty);
        }
        public static string ToSafeString(this int value)
        {
            return Convert.ToString(value);
        }
        public static string ReplaceRelationshipIdAndName<T>(this string value, long id) where T : class
        {
            return value.Replace("{R_Id}", Convert.ToString(id)).Replace("{R_RName}", typeof(T).Name);
        }
        public static string ReplaceRelationshipId(this string value, long id)
        {
            return value.Replace("{R_Id}", Convert.ToString(id));
        }
        public static string ToSafeString(this double value)
        {
            return Convert.ToString(value);
        }
        public static string ToSafeString(this long value)
        {
            return Convert.ToString(value);
        }
        public static bool HasValueGreaterthanZero(this long? value)
        {
            return value.HasValue && value.Value > 0;
        }
        public static bool IsNullOrZero(this long? value)
        {
            return value == null || value.Value == 0;
        }
        public static string ToSafeString(this long? value)
        {
            return Convert.ToString(value);
        }
        public static string ToInitCap(this string value)
        {
            return value.IsNullOrEmpty() ? string.Empty : String.Concat(char.ToUpper(value[0]), value.Substring(1).ToLower());
        }
        public static bool EqualsIgnoreCase(this string value, string text)
        {
            return value == null ? false : value.Equals(text, StringComparison.OrdinalIgnoreCase);
        }
        public static string ToCurrency(this int value)
        {
            return String.Format("{0:N}", value).Replace(".00", "").Replace(".0", "");
        }
        public static string ToCurrency(this decimal value)
        {
            return String.Format("{0:N}", value);
        }
        public static string ToCurrency(this decimal? value)
        {
            return String.Format("{0:N}", Convert.ToDecimal(value));
        }
        public static string ToSafeString(this decimal value)
        {
            return Convert.ToString(value);
        }
        public static string ToSafeString(this int? value)
        {
            return Convert.ToString(value);
        }

        public static int ToSafeInt(this int? value)
        {
            return Convert.ToInt32(value);
        }
        public static IList<System.Web.Mvc.SelectListItem> GetYearList(string value)
        {
            var currentYear = DateTime.Today.Year;
            var list = new List<System.Web.Mvc.SelectListItem>();
            while (currentYear >= Constant.ApplicationStartYear)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = currentYear.ToString(), Value = currentYear.ToString(), Selected = currentYear.ToString() == value });
                currentYear--;
            }
            return list;
        }

        public static Dictionary<string, object> SetReadOnlyWithClass<T>(this string txtToCheckIn, Expression<Func<T>> propertyLambda, string className = "")
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }
            var ret = new Dictionary<string, object>();
            if (className.IsNotNullAndNotEmpty())
            {
                ret.Add("class", className);
            }

            if (txtToCheckIn == null || !txtToCheckIn.Contains(string.Concat(",", me.Member.Name, ",")))
            {
                ret.Add("readonly", "readonly");
            }

            return ret;
        }
        public static bool HasPropertyName<T>(this string txtToCheckIn, Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }


            return txtToCheckIn == null ? false : txtToCheckIn.Contains(string.Concat(",", me.Member.Name, ","));
        }


        public static void AppendClass(this Dictionary<string, object> dict, string className)
        {
            if (!dict.ContainsKey("class"))
            {
                dict.Add("class", className);
            }
            else
            {
                var cls = dict["class"];
                dict["class"] = string.Concat(cls, " ", className);
            }
        }

        public static string ToMobileFormat(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return value.ToSafeString();
            }
            var number = value.Length < 9 ? value : value.Substring(value.Length - 9);
            return String.Concat("00971", number);
        }
        public static int SafeValue(this int? value)
        {
            return Convert.ToInt32(value);
        }
        public static long SafeValue(this long? value)
        {
            return Convert.ToInt64(value);
        }

        public static decimal SafeValue(this decimal? value)
        {
            return Convert.ToDecimal(value);
        }
        public static byte SafeValue(this byte? value)
        {
            return Convert.ToByte(value);
        }
        public static bool SafeValue(this bool? value)
        {
            return Convert.ToBoolean(value);
        }
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static string ToDefaultDateFormat(this DateTime value)
        {
            return String.Format(Constant.Annotation.DefaultDateFormat, value);
        }
        public static string ToDefaultDateTimeFormat(this DateTime value)
        {
            return String.Format(Constant.Annotation.DateTimeFormat, value);
        }
        public static string ChangeToFormat(this DateTime value, string format)
        {
            return String.Format(format, value);
        }
        public static string ToDefaultDateFormat(this DateTime? value)
        {
            return String.Format(Constant.Annotation.DefaultDateFormat, value);
        }
        public static string ToDefaultTimeFormat(this DateTime? value)
        {
            return String.Format(Constant.Annotation.LongTimeFormat, value);
        }
        public static string ToDefaultDateTimeFormat(this DateTime? value)
        {
            return String.Format(Constant.Annotation.DefaultDateTimeFormat, value);
        }
        public static string ToDefaultDateTimeFormatOnly(this DateTime? value)
        {
            return String.Format(Constant.Annotation.DefaultDateTimeFormatOnly, value);
        }
        public static string FirstLetterUpperCase(this string value)
        {
            if (value.Trim().Length == 0)
            {
                return string.Empty;
            }
            return value.Trim().Substring(0, 1).ToUpper();
        }
        public static string ToDayAndDateFormat(this DateTime? value)
        {
            return String.Format(Constant.Annotation.ToDayAndDateFormat, value);
        }
        public static string ToDD_MMM_YYYY(this DateTime? value)
        {
            return value == null ? String.Empty : String.Format(Constant.Annotation.DefaultDateFormat, value);
        }
        public static string ToDD_MMM_YYYY_HHMMSS(this DateTime? value)
        {
            return value == null ? String.Empty : String.Format(Constant.Annotation.LongDateTimeFormat, value);
        }
        public static string ToYYYY_MM_DD(this DateTime? value)
        {
            return value == null ? String.Empty : String.Format(Constant.Annotation.ToYYYY_MM_DD_Dash, value);
        }
        public static string ToDD_MMM_YYYY_HHMMSS(this DateTime value)
        {
            return String.Format(Constant.Annotation.LongDateTimeFormat, value);
        }

        public static bool IsLastDayOfMonth(this DateTime value)
        {
            var days = DateTime.DaysInMonth(value.Year, value.Month);
            return days == value.Day;
        }
        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }
        public static DateTime LastDateOfMonth(this DateTime value)
        {
            var days = DateTime.DaysInMonth(value.Year, value.Month);
            return new DateTime(value.Year, value.Month, days, value.Hour, value.Minute, value.Second);
        }
        public static DateTime FirstDateOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
        public static string ToMMM_YYYY(this DateTime value)
        {
            return String.Format("{0:MMM yyyy}", value);
        }
        public static string ToUTCFormat(this DateTime value)
        {
            return String.Format(Constant.Annotation.UTCFormat, value);
        }

        public static string AddDoubleQuotes(this string value)
        {
            return "\"" + value + "\"";
        }


        public static string ToYYY_MM_DD_HHMMSS(this DateTime value)
        {
            return String.Format(Constant.Annotation.Long_YYYY_MM_DD, value);
        }
        public static string ToYYY_MM_DD(this DateTime value)
        {
            return String.Format(Constant.Annotation.YYYY_MM_DD, value);
        }
        public static string ToYYYY_MM_DD_Dash(this DateTime value)
        {
            return String.Format(Constant.Annotation.ToYYYY_MM_DD_Dash, value);
        }
        public static DateTime ApplicationNow(this DateTime value)
        {
            return DateTime.Now;
        }

        public static int GetWeekNumberOfMonth(this DateTime value)
        {
            var date = value.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
        public static void SetPropertyValue(this object obj, string propertyName, object propertyValue)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            prop.SetValue(obj, propertyValue);

        }
        public static DateTime FirstDayOfYear(this DateTime value)
        {
            return Convert.ToDateTime(string.Concat(value.Year, ".1/1"));
        }
        public static DateTime LastDayOfYear(this DateTime value)
        {
            return Convert.ToDateTime(string.Concat(value.Year, "/12/31"));
        }
        public static int YearsTillNow(this DateTime value)
        {
            int years = DateTime.Now.Year - value.Year;
            DateTime today = DateTime.Now;
            if (value > today.AddYears(-years))
            {
                years--;
            }
            return years;
        }
        public static string ExactDuration(this DateTime value, DateTime? endDate = null)
        {
            DateTime today = endDate ?? DateTime.Today;

            //value = new DateTime(2015, 2, 28);
            //today = new DateTime(2015, 3, 29);
            int years = today.Year - value.Year;
            if (value.Month > today.Month && years != 0)
            {
                years--;
            }
            int months = (12 + today.Month - value.Month) % 12;
            if (value.Day > today.Day && months != 0)
            {
                months--;
            }
            int days = 0;
            if (value.Day > today.Day)
            {
                days = today.Day + (DateTime.DaysInMonth(value.Year, value.Month) - value.Day);
            }
            else if (value.Day < today.Day)
            {
                days = today.Day - value.Day;
            }
            days = days + DateTime.Compare(today, value.AddYears(years).AddMonths(months).AddDays(days));
            return String.Concat(years, " Year(s) ", months, " Month(s) ", days, " Day(s)");
        }

        //public static string ExactDurationOld(this DateTime value)
        //{
        //    //var k = Convert.ToDateTime("29/01/2014").AddMonths(1);
        //    //value = value.AddDays(-1);
        //    value = new DateTime(2000, 10, 1);

        //    int years = DateTime.Now.Year - value.Year;
        //    DateTime today = DateTime.Now.Date;

        //    if (value > today.AddYears(-years))
        //    {
        //        years--;
        //    }
        //    int month = today.Subtract(new TimeSpan(value.AddYears(years).Ticks)).Month;
        //    if (value > today.AddYears(-years).AddMonths(-month))
        //    {
        //        month--;
        //    }
        //    var j = today.AddTicks(value.AddYears(years).AddMonths(month).Date.Ticks * -1);
        //    int days = today.Subtract(new TimeSpan(value.AddYears(years).AddMonths(month).Date.Ticks)).Day;

        //    if (value > today.AddYears(-years).AddMonths(-month).AddDays(-days))
        //    {
        //        days--;
        //    }
        //    return String.Concat(years, " Year(s) ", month, " Month(s) ", days, " Day(s)");
        //    //DateTime today = DateTime.Now;
        //    //var dateDif = DateTime.Now.Subtract(new TimeSpan(value.Ticks));
        //    //int years = dateDif.Year;
        //    //if (value > today.AddYears(-years))
        //    //{
        //    //    years--;
        //    //}
        //    //int months = dateDif.Month;
        //    //if (value > today.AddYears(-years).AddMonths(-months))
        //    //{
        //    //    months--;
        //    //}
        //    //int days = dateDif.Day;
        //    //if (value > today.AddYears(-years).AddMonths(-months).AddDays(-days))
        //    //{
        //    //    days--;
        //    //}
        //    //return String.Concat(years, years > 1 ? " Years " : " Year ", months, months > 1 ? " Months " : " Month ", days, days > 1 ? " Days" : "Day");
        //}

        public static bool IsRangeInServerDate(this DateTime? value, DateTime? toDate)
        {
            var result = (value == null || value.Value.Date <= DateTime.Now.ApplicationNow()) &&
                (toDate == null || toDate >= DateTime.Now.ApplicationNow() || toDate == Constant.SystemMinDate);
            return result;
        }


        //public static bool IsInHierarchy(this int value, string reportingLine)
        //{
        //    return reportingLine != null && reportingLine.Contains(String.Concat(",", value, ","));
        //}

        public static bool IsInHierarchy(this long value, string reportingLine)
        {
            return reportingLine != null && reportingLine.Contains(String.Concat(",", value, ","));
        }

        public static bool IsListInHierarchy(this string listValue, string reportingLine)
        {
            if (reportingLine.IsNullOrEmpty() || listValue.IsNullOrEmpty())
            {
                return false;
            }
            var list = listValue.Split(',').Where(x => x != "").ToList();
            foreach (var item in list)
            {
                if (reportingLine.Contains(String.Concat(",", item, ",")))
                {
                    return true;
                }
            }
            return false;
        }
        public static void AddValueAndHideIfNullOrEmpty(this System.Web.UI.HtmlControls.HtmlGenericControl value, string text)
        {
            if (text.IsNullOrEmpty())
            {
                value.Visible = false;
            }
            else
            {
                value.InnerHtml += text;
            }
        }



        public static bool IsNullOrEmptyOrZero(this string s)
        {
            return String.IsNullOrEmpty(s) || s == "0";
        }
        public static bool IsNullOrEmptyOrValue(this string s, string value)
        {
            return String.IsNullOrEmpty(s) || s == value;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
        public static bool IsNullOrEmptyOrWhiteSpace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
        public static bool CaseContains(this string baseString, string textToSearch, StringComparison comparisonMode)
        {
            return !baseString.IsNullOrEmpty() && (baseString.IndexOf(textToSearch, comparisonMode) != -1);
        }
        public static string ToSafeString(this string s)
        {
            return s == null ? string.Empty : Convert.ToString(s);
        }

        public static string ToSafeStringWithoutNull(this string s)
        {
            return s == null || s.ToLower() == "null" ? string.Empty : Convert.ToString(s);
        }

        public static string ToSafeString(this DateTime s)
        {
            return Convert.ToString(s);
        }
        //public static string ToSafeString(this long s)
        //{
        //    return Convert.ToString(s);
        //}

        //public static string ToSafeString(this long? s)
        //{
        //    return Convert.ToString(s);
        //}

        public static string ToSafeString(this DateTime? s)
        {
            return s == null ? string.Empty : Convert.ToString(s);
        }


        public static string ToNonBreakingSpace(this string s)
        {
            return s == null ? String.Empty : s.Replace("-", "&#8209;");
        }
        public static string ToSafeString(this byte? s)
        {
            return Convert.ToString(s);
        }
        public static int ToSafeInt(this string s)
        {
            return s.IsNullOrEmpty() ? 0 : Convert.ToInt32(s);
        }
        public static string ToHHMMFormat(this TimeSpan? s)
        {
            return s == null ? "00:00" : s.Value.ToHHMMFormat();
        }
        public static string ToHHMMFormat(this TimeSpan s)
        {
            if (s.Days > 0)
            {
                return String.Concat(s.Days, ":", s.Hours.ToString().PadLeft(2, '0'), ":", s.Minutes.ToString().PadLeft(2, '0'));
            }
            return String.Concat(s.Hours.ToString().PadLeft(2, '0'), ":", s.Minutes.ToString().PadLeft(2, '0'));
        }
        public static bool ToSafeBool(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return false;
            }
            else if (s == "1" || s.ToLower() == "true")
            {
                return true;
            }
            return false;
        }
        public static bool ToSafeBool(this bool? s)
        {
            return Convert.ToBoolean(s);
        }
        public static bool? ToeSafeNullableBool(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            else if (s == "1" || s.ToLower() == "true")
            {
                return true;
            }
            return false;
        }
        //public static long? ToeSafeNullablelong(this string s)
        //{
        //    if (s.IsNullOrEmpty())
        //    {
        //        return null;
        //    }
        //    return Convert.ToUInt64(s);
        //}
        public static int? ToSafeNullableInt(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            return Convert.ToInt32(s);
        }

        public static double? ToSafeNullableDouble(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            return Convert.ToDouble(s);
        }
        public static long? ToSafeNullableLong(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            return Convert.ToInt64(s);
        }
        public static long ToSafeLong(this string s)
        {
            return s.IsNullOrEmpty() ? 0 : Convert.ToInt64(s);
        }
        //public static long ToSafelong(this string s)
        //{
        //    return s.IsNullOrEmpty() ? 0 : Convert.ToUInt64(s);
        //}
        public static decimal ToSafeDecimal(this string s)
        {
            return s.IsNullOrEmpty() ? 0 : Convert.ToDecimal(s);
        }
        public static double ToSafeDouble(this string s)
        {
            return s.IsNullOrEmpty() ? 0 : Convert.ToDouble(s);
        }
        public static decimal? ToSafeNullableDecimal(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            return Convert.ToDecimal(s);
        }
        public static string ToSafeNullableString(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            return Convert.ToString(s);
        }
        public static DateTime ToSafeDateTime(this string s)
        {
            return s.IsNullOrEmpty() ? Constant.ApplicationMinDate : Convert.ToDateTime(s);
        }
        public static DateTime ToServerTime(this DateTime s, string legalEntityCode)
        {
            if (legalEntityCode == "CAYAN_UAE")
            {
                return s.AddHours(-1);
            }
            return s;
        }
        public static TimeSpan ToServerTimeSpan(this TimeSpan s, string legalEntityCode)
        {
            if (legalEntityCode == "CAYAN_UAE")
            {
                return s.Add(new TimeSpan(-1, 0, 0));
            }
            return s;
        }
        public static int ServerToLocalTimeDiff(this string legalEntityCode)
        {
            if (legalEntityCode == "CAYAN_UAE")
            {
                return 1;
            }
            return 0;
        }

        public static DateTime ServerToLocalTime(this DateTime s, string legalEntityCode)
        {
            if (legalEntityCode == "CAYAN_UAE")
            {
                return s.AddHours(1);
            }
            return s;
        }
        public static TimeSpan ServerToLocalTimeSpan(this TimeSpan s, string legalEntityCode)
        {
            if (legalEntityCode == "CAYAN_UAE")
            {
                return s.Add(new TimeSpan(1, 0, 0));
            }
            return s;
        }
        public static DateTime ToSafeDateTimeMax(this string s)
        {
            return s.IsNullOrEmpty() ? Constant.ApplicationMaxDate : Convert.ToDateTime(s);
        }
        public static DateTime? ToSafeNullableDateTime(this string s)
        {
            if (s.IsNullOrEmptyOrWhiteSpace())
            {
                return null;
            }
            return Convert.ToDateTime(s);
        }
        public static string PadLeftIfNotNullElseEmpty(this string s, string concatValue)
        {
            return s.IsNullOrEmpty() ? string.Empty : String.Concat(concatValue, s);
        }
        public static string PadRightIfNotNullElseEmpty(this string s, string concatValue)
        {
            return s.IsNullOrEmpty() ? string.Empty : String.Concat(s, concatValue);
        }
        public static string ToSpaceDelimeted(this string s)
        {
            return System.Text.RegularExpressions.Regex.Replace(s, "([a-z](?=[A-Z0-9])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
        public static string HtmlEncode(this string s)
        {
            return System.Web.HttpUtility.HtmlEncode(s);
        }
        public static string XmlEncode(this string s)
        {
            return s.ToSafeString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
        public static string XmlTaleoEncode(this string s)
        {
            return s.ToSafeString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace(" ", "_20").Replace("(", "_28").Replace(")", "_29").Replace("?", "_3f").Replace(".", "_2e").Replace("/", "_2f").Replace("-", "_2d");
        }
        public static string XmlTaleoDecode(this string s)
        {
            return s.ToSafeString().Replace("_20", " ").Replace("_28", "(").Replace("_29", ")").Replace("_3f", "?").Replace("_2e", ".").Replace("_2f", "/").Replace("_2d", "-");
        }
        public static string HtmlDecode(this string s)
        {
            return System.Web.HttpUtility.HtmlDecode(s);
        }

        public static string UrlEncode(this string s)
        {
            return System.Web.HttpUtility.UrlEncode(s);
        }
        public static string UrlDecode(this string s)
        {
            return System.Web.HttpUtility.UrlDecode(s);
        }
        public static string AllErrorMessage(this Exception exception)
        {
            var msg = exception.Message;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                msg = string.Concat(msg, ". Inner Exception Type: ", exception.GetType(), " . Inner Exception: ", exception.Message.ToSafeString(), ". Stack Trace: ", exception.StackTrace.ToSafeString(), Environment.NewLine);
            }
            return msg;
        }
        public static string HtmlBold(this string s)
        {
            if (s == null)
            {
                return string.Empty;
            }
            return String.Concat("<b>", s, "</b>"); ;
        }
        public static string AppendHtmlSpace(this string s, int count = 1)
        {
            if (s == null)
            {
                return string.Empty;
            }
            return string.Concat(s, String.Concat(Enumerable.Repeat("&nbsp;", count).ToArray()));

        }
        public static string PadRight(this string s, int totalWidth, string paddingString)
        {
            if (s == null)
            {
                return string.Empty;
            }
            if (s.Length < totalWidth)
            {
                return string.Concat(s, String.Concat(Enumerable.Repeat(paddingString, totalWidth - s.Length).ToArray()));
            }
            return s;

        }
        public static string LimitTo(this string s, int count)
        {
            if (s == null)
            {
                return string.Empty;
            }
            if (s.Length > count)
            {
                return string.Concat(s.Substring(0, count - 3), "...");
            }
            else
            {
                return s;
            }

        }
        public static string LimitWithToolTip(this string s, int count)
        {
            if (s == null)
            {
                return string.Empty;
            }
            if (s.Length > count)
            {
                return string.Concat("<label", " title=\"", s, "\" >", s.Substring(0, count - 3), "...", "</label>");
            }
            else
            {
                return s;
            }

        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(x => x.FirstOrDefault());
        }

    }
}
