using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CMS.Common
{
    public static class ApplicationExtension
    {
        public static double PayrollDailyAmountAsPerWorkingDays(this double value)
        {
            var amount = value / 22;
            return amount.RoundToTwoDecimal();
        }

        public static double ProrataRoundingAdjustment(this double value, double fullAmount, double divisor)
        {
            return (fullAmount - (value * divisor)).RoundToTwoDecimal();
        }
        public static double RoundPayrollAmount(this double? value)
        {
            return Math.Round(Convert.ToDouble(value), 2);
        }
        public static T GetService<T>(this IServiceProvider sp)
        {
            return (T)sp.GetService(typeof(T));
        }
        public static string ToMMM_YYYY(this DateTime value)
        {
            return String.Format("{0:MMM yyyy}", value);
        }
        public static double RoundToTwoDecimal(this double value)
        {
            return Math.Round(value, 2);
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
        public static string ToJson(this DataRow dr)
        {
            var dict = dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
            return JsonConvert.SerializeObject(dict);
        }
        public static string ToJson(this Dictionary<string, object> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        public static dynamic ToDynamicObject(this DataRow dr)
        {
            var json = dr.ToJson();
            var expConverter = new ExpandoObjectConverter();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
            return result;
        }
        public static dynamic ToDynamicObject(this Dictionary<string, object> dr)
        {
            var json = dr.ToJson();
            var expConverter = new ExpandoObjectConverter();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
            return result;
        }
        public static dynamic JsonToDynamicObject(this string json)
        {
            var expConverter = new ExpandoObjectConverter();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
            return result;
        }
        public static DataTable ToDataTable(this List<dynamic> list)
        {
            //var settings = new JsonSerializerSettings { DateFormatString = "dd-MMM-yyyy hh:mm:ss" };
            var json = JsonConvert.SerializeObject(list);
            var dataTable = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
            return dataTable;
        }
        public static Dictionary<string, object> ToDictionary(this DataRow dr)
        {
            return dr.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dr[c]);
        }
        public static string ResourceValue(this Dictionary<string, string> dict, string resourceKey)
        {
            var data = "";
            if (!dict.TryGetValue(resourceKey, out data))
            {
                data = resourceKey;
            }
            return data;
        }
        public static double RoundPayrollSummaryAmount(this double value)
        {
            return Math.Round(value);
        }
        public static string IHtmlToString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }
        }
        public static void SetPropertyValue(this object obj, string propertyName, double propertyValue)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            prop.SetValue(obj, propertyValue);

        }
        public static bool ToSafeBool(this bool? s)
        {
            return Convert.ToBoolean(s);
        }
        public static bool ToSafeBool(this string s)
        {
            return Convert.ToBoolean(s);
        }
        public static bool ToSafeBool(ref Guid s)
        {
            return Convert.ToBoolean(s);
        }
        public static string ToDefaultCreateButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Create";
        }
        public static string ToHHMMFormat(this TimeSpan? s)
        {
            return s == null ? "00:00" : s.ToHHMMFormat();
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
            return "00:00";
        }
        public static string ToTimeSpanString(this TimeSpan value)
        {

            if (value.Days > 0)
            {
                return string.Concat(value.Days, ".", value.Hours, ":", value.Minutes);
            }
            return string.Concat(value.Hours, ":", value.Minutes);
        }
        public static string ToDefaultRequestButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Request";
        }
        public static string HtmlEncode(this string text)
        {

            return System.Web.HttpUtility.HtmlEncode(text);
        }
        public static string HtmlDecode(this string text)
        {

            return System.Web.HttpUtility.HtmlDecode(text);
        }
        public static string ToHtmlError(this object text)
        {
            if (text != null)
            {
                var obj = (Dictionary<string, Dictionary<string, object>>)text;
                var error = "<ul>";
                foreach (var err in obj.Values.ToList())
                {
                    foreach (var e in err)
                    {
                        error = string.Concat(error, "<li>", ((string[])e.Value)[0], "</li>");
                    }
                }
                error = string.Concat(error, "</ul>");
                return error;
            }
            return string.Empty;



        }
        //public static string DictionaryToHtmlError(this Dictionary<string, string> text)
        //{
        //    if (text != null)
        //    {
        //        var error = "<ul>";
        //        foreach (var err in text)
        //        {

        //            error = string.Concat(error, "<li>", err.Value, "</li>");
        //        }
        //        error = string.Concat(error, "</ul>");
        //        return error;
        //    }
        //    return string.Empty;



        //}

        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }
        public static int ToSafeInt(this int? value)
        {
            return Convert.ToInt32(value);
        }
        public static int ToSafeInt(this string s)
        {
            return s.IsNullOrEmpty() ? 0 : Convert.ToInt32(s);
        }
        public static int? ToSafeNullableInt(this string s)
        {
            if (s.IsNullOrEmpty())
            {
                return null;
            }
            return Convert.ToInt32(s);
        }
        public static string ToDefaultCompleteButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Complete";
        }
        public static string Humanize(this DateTime? value)
        {
            if (value == null)
            {
                return null;

            }
            return Humanize(value.Value);
        }
        public static string Humanize(this DateTime value)
        {
            return Humanizer.DateHumanizeExtensions.Humanize(value);
        }

        public static string ToDefaultRejectButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Reject";
        }
        public static string ToDefaultReturnButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Return";
        }
        public static string ToDefaultSaveButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Save";
        }
        public static string ToDefaultSubmitButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Submit";
        }
        public static string ToDefaultSaveAsDraftButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Save As Draft";
        }
        public static string ToDefaultEditButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Edit";
        }
        public static string ToDefaultBackButtonText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Back";
        }
        public static string ToDefaultNoteNumberText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Note Number";
        }
        public static string ToDefaultTaskNumberText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Task Number";
        }
        public static string ToDefaultServiceNumberText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Service Number";
        }
        public static string ToSentenceCase(this string str)
        {
            // return System.Text.RegularExpressions.Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
            return System.Text.RegularExpressions.Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToUpper(m.Value[1])}");
        }
        public static string ToDefaultSubjectText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Subject";
        }
        public static string ToDefaultTaskNoText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Task No";
        }
        public static string ToDefaultServiceNoText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Service No";
        }
        public static string ToDefaultDescriptionText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Description";
        }
        public static string ToDefaultAssignedToUserText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Assigned To";
        }
        //public static object ToDbValue(this object s, DataColumnTypeEnum type)
        //{
        //    if (s == null)
        //    {
        //        return null;
        //    }
        //    return type switch
        //    {
        //        DataColumnTypeEnum.Text => @$"'{s}'",
        //        DataColumnTypeEnum.DateTime => @$"'{((DateTime)s).ToDatabaseDateFormat()}'",
        //        DataColumnTypeEnum.Bool => @$"{(((bool)s) ? "true" : "false")}",
        //        _ => s,
        //    };
        //}
        public static string ToDatabaseDateFormat(this DateTime value)
        {
            return String.Format(ApplicationConstant.Database.DateFormat, value);
        }
        public static string GetValue(this Dictionary<string, string> dict, string key)
        {
            if (dict.IsNotNull())
            {
                string val = null;
                dict.TryGetValue(key, out val);
                return val;
            }
            else
            {
                return null;
            }
        }
        public static Dictionary<string, string> ToDictionary(this string value)
        {
            var dict = new Dictionary<string, string>();
            if (value.IsNullOrEmpty())
            {
                return dict;
            }
            var items = value.Split('&');
            foreach (var item in items)
            {
                var splt = item.Split('=');
                if (splt.Length == 2)
                {
                    dict.Add(splt[0], splt[1]);
                }
            }
            return dict;

        }

        public static string ToDefaultDateFormat(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, value);
        }
        public static string ToYYYY_MM_DD_DateFormat(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.YYYY_MM_DD, value);
        }
        public static string ToDefaultDateTimeFormat(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultDateTimeFormat, value);
        }
        public static string ToDefaultDateFormat(this DateTime? value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, value);
        }
        public static string ToYYYY_MM_DD_DateFormat(this DateTime? value)
        {
            return String.Format(ApplicationConstant.DateAndTime.YYYY_MM_DD, value);
        }
        public static string ToDefaultDateTimeFormat(this DateTime? value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultDateTimeFormat, value);
        }
        public static string ToDD_MMM_YYYY_HHMMSS(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.LongDateTimeFormat, value);
        }
        public static string ToSequenceNumberFormat(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.SequenceNumberFormat, value);
        }
        public static DateTime ApplicationNow(this DateTime value)
        {
            return DateTime.Now;
        }
        public static DataColumnTypeEnum ToDataColumnType(this string s)
        {
            return s switch
            {
                "text" => DataColumnTypeEnum.Text,
                "boolean" => DataColumnTypeEnum.Bool,
                "timestamp without time zone" => DataColumnTypeEnum.DateTime,
                "integer" => DataColumnTypeEnum.Integer,
                "double precision" => DataColumnTypeEnum.Double,
                "bigint" => DataColumnTypeEnum.Long,
                _ => DataColumnTypeEnum.Text,
            };
        }
        public static DataColumnTypeEnum ToDatabaseColumn(this Type s)
        {
            var t = s.Name;
            if (s == typeof(System.String))
            {
                return DataColumnTypeEnum.Text;
            }
            if (s == typeof(System.Boolean))
            {
                return DataColumnTypeEnum.Bool;
            }
            if (s == typeof(System.DateTime))
            {
                return DataColumnTypeEnum.DateTime;
            }
            if (s == typeof(System.Int32))
            {
                return DataColumnTypeEnum.Integer;
            }
            if (s == typeof(System.Double))
            {
                return DataColumnTypeEnum.Double;
            }
            if (s == typeof(System.Int64))
            {
                return DataColumnTypeEnum.Long;
            }
            return DataColumnTypeEnum.Text;
        }
        public static bool IsTrue(this bool? s)
        {
            return Convert.ToBoolean(s);
        }
        public static bool IsNullOrTrue(this bool? s)
        {
            return s == null || Convert.ToBoolean(s);
        }

        public static bool IsNotNullAndNotEmpty(this string s)
        {
            return !String.IsNullOrEmpty(s);
        }
        public static bool IsNotNullAndNotEmptyAndNotValue(this string s, string value)
        {
            return !String.IsNullOrEmpty(s) && s != value;
        }

        public static bool IsNotNull(this object value)
        {
            return value != null;
        }
        public static bool IsNullOrZero(this int? value)
        {
            return value == null || value == 0;
        }
        public static string ToStatusIcon(this PortalStatusEnum s)
        {
            switch (s)
            {
                case PortalStatusEnum.Live:
                    return "fa fa-sm fa-check-circle c-green";
                case PortalStatusEnum.Inactive:
                    return "fa fa-sm fa-times-circle c-red";
                case PortalStatusEnum.Maintenance:
                    return "fa fa-sm fa-wrench c-blue";
                default:
                    return "fa fa-sm fa-wrench c-blue";
            }
        }

        public static string ToIconTitle(this PortalStatusEnum s)
        {
            switch (s)
            {
                case PortalStatusEnum.Live:
                    return "Live";
                case PortalStatusEnum.Inactive:
                    return "Inactive";
                case PortalStatusEnum.Maintenance:
                    return "Under maintenance";
                default:
                    return "Under maintenance";
            }
        }
        public static string Coalesce(this string s, string newText)
        {
            return String.IsNullOrEmpty(s) ? newText : s;
        }
        public static DateTime? Coalesce(this DateTime? s, DateTime? newText)
        {
            return s == null ? newText : s;
        }




        public static T? ToNullableEnum<T>(this string name) where T : struct
        {
            if (string.IsNullOrEmpty(name))
            {
                return default(T?);
            }
            return (T)Enum.Parse(typeof(T), name);
        }


        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static void SetPropertyValue(this object obj, string propertyName, object propertyValue)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            prop.SetValue(obj, propertyValue);

        }
        public static DateTime ToSafeDateTime(this string s)
        {
            return s.IsNullOrEmpty() ? ApplicationConstant.DateAndTime.MinDate : Convert.ToDateTime(s);
        }
        public static string ToYYY_MM_DD(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.YYYY_MM_DD, value);
        }
        public static DateTime LastDayOfYear(this DateTime value)
        {
            return Convert.ToDateTime(string.Concat(value.Year, "/12/31"));
        }
        public static string YYYY_MM_DD_HH_MM(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.YYYY_MM_DD_HH_MM, value);
        }
        public static bool IsNullOrEmptyOrWhiteSpace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
        public static DateTime FirstDateOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
        public static DateTime LastDateOfMonth(this DateTime value)
        {
            var days = DateTime.DaysInMonth(value.Year, value.Month);
            return new DateTime(value.Year, value.Month, days, value.Hour, value.Minute, value.Second);
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
        public static IList<SelectListItem> GetYearList(string value)
        {
            var currentYear = DateTime.Today.Year;
            var list = new List<SelectListItem>();
            while (currentYear >= ApplicationConstant.ApplicationStartYear)
            {
                list.Add(new SelectListItem { Text = currentYear.ToString(), Value = currentYear.ToString(), Selected = currentYear.ToString() == value });
                currentYear--;
            }
            return list;
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(x => x.FirstOrDefault());
        }
        public static bool IsLastDayOfMonth(this DateTime value)
        {
            var days = DateTime.DaysInMonth(value.Year, value.Month);
            return days == value.Day;
        }
        public static double PayrollDailyAmount(this double value, DateTime date)
        {
            var days = DateTime.DaysInMonth(date.Year, date.Month);
            var amount = value / (double)days;
            return amount.RoundToTwoDecimal();
        }
    }
}
