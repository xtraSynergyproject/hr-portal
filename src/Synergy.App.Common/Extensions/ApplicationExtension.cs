using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Synergy.App.Common
{
    public static class ApplicationExtension
    {
        public static async Task<string> ErrorResponseMessage(this HttpResponseMessage response)
        {
            string msg = await response.Content.ReadAsStringAsync();
            // check message only for 404 ??
            if (response.StatusCode == HttpStatusCode.BadRequest && !string.IsNullOrWhiteSpace(msg))
            {
                var json = JsonDocument.Parse(msg);
                if (json.RootElement.TryGetProperty("Message", out JsonElement prop))
                {
                    return prop.GetString();
                }
            }
            return response.StatusCode.ToString();
        }
        public static string ErrorFromErrorCode(this string code) => code switch
        {
            null => "",
            "NoFaceFound" => "We did not find a suitable face. Please position your face in the center.",
            "MultipleFacesFound" => "We found more than one face.",
            "LiveDetectionFailed" => "The submitted samples do not prove that they are recorded from a live person.",
            "NoTemplateAvailable" => "You have not enrolled yourself yet.",
            "NotEnoughSamples" => "Not enough valid samples have been provided.",
            "ChallengeResponseFailed" => "ChallengeResponseFailed", // we do not use challenge respose here
            "ExecutionOfJobTimedOut" => "ExecutionOfJobTimedOut", // ups
            "MissingData" => "Not all images have been supplied.",
            "InvalidSampleData" => "The submitted samples could not be decoded into images.",
            _ => code
        };

        public static string HintFromResult(this string code) => code switch
        {
            null => "",
            "NoFaceFound" => "We did not find a face. Please position your face in the center. ",
            "MultipleFacesFound" => "We found multiple faces or a strongly uneven background distracted us. Your face should fill the circle completely.",
            "UnsuitableImage" => "We did not find a suitable face. Please position your face in the center and make sure you are fully visible. ",
            "UnnaturalMotionDetected" => "We observed unnatural motion. Please make sure to look straight in the first picture, then nod your head slightly.",
            "DontMoveDevice" => "Don’t move the device you are using. ",
            _ => ""
        };
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
        public static string LimitSize(this string value, int length)
        {
            if (value.IsNullOrEmpty() || value.Length <= length)
            {
                return value;

            }
            return $"{value.Substring(0, length - 4)}...";
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
        public static bool IsNullOrEmptyOrValue(this string s, string val)
        {
            return String.IsNullOrEmpty(s) || s == val;
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
        public static string[] RemoveItemFromArray(this string[] array, string item)
        {
            if (item.IsNullOrEmpty())
            {
                return array;
            }
            return array.Where(x => x != item).ToArray();

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
        public static object SerializeErrors(this ModelStateDictionary modelState)
        {
            var errorKeys = (from item in modelState
                             where item.Value.Errors.Any()
                             select item);
            var dict = new Dictionary<string, Dictionary<string, string[]>>();
            foreach (var item in errorKeys)
            {
                var child = new Dictionary<string, string[]>();
                child.Add("errors", item.Value.Errors.Select(x => x.ErrorMessage).ToArray());
                dict.Add(item.Key, child);

            }
            return dict;
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
                var error = "<ul>";
                if (text.GetType() == typeof(Dictionary<string, string>))
                {
                    var dict = (Dictionary<string, string>)text;
                    foreach (var err in dict)
                    {

                        error = string.Concat(error, "<li>", err.Value, "</li>");
                    }
                    error = string.Concat(error, "</ul>");
                    return error;
                }
                else if (text.GetType() == typeof(Dictionary<string, Dictionary<string, string[]>>))
                {
                    var d = (Dictionary<string, Dictionary<string, string[]>>)text;
                    foreach (var err in d.Values.ToList())
                    {
                        foreach (var e in err)
                        {
                            error = string.Concat(error, "<li>", e.Value[0], "</li>");
                        }
                    }
                    error = string.Concat(error, "</ul>");
                    return error;
                }
                var obj = (Dictionary<string, Dictionary<string, object>>)text;
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
        public static string ToDefaultRuntimeWorkflowText(this string text)
        {
            if (text.IsNotNullAndNotEmpty())
            {
                return text;

            }
            return "Manage Workflow";
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
        public static string ToYyyyMmDdFormat(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.YYYYMMDD, value);
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
        public static string ToDD_YYYY_MM_DD(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultYY_MM_DD, value);
        }
        public static string ToDD_MM_YYYY(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultDD_MM_YYYY, value);
        }
        public static string ToDD_MM_YYYY_HH_MM_SS(this DateTime value)
        {
            return String.Format(ApplicationConstant.DateAndTime.DefaultDD_MM_YYYY_HH_MM_SS, value);
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

        /// <summary>
        /// Calculates percentage for a value based on min and max value
        /// Percentage varies from 0(0%) to 1(100%)
        /// </summary>
        /// <param name="minValue">minValue</param>
        /// <param name="maxValue">maxValue</param>
        /// <param name="value">value</param>
        /// <returns>value percentage</returns>
        public static double CalculatePercentage(double minValue, double maxValue, double value)
        {
            try
            {
                // Find percentage of the value using min & max values
                double range = maxValue - minValue;
                double correctedStartValue = value - minValue;
                double valueP = correctedStartValue / range;
                return valueP;
            }
            catch (DivideByZeroException)
            {
                // returning double default value if DivideByZeroException occurs.
                return default;
            }
        }

        /// <summary>
        /// Extracts R, G, B and A values from the color string
        /// </summary>
        /// <param name="colorString">colorString (should be in rgba format)</param>
        /// <returns>list of values as r,g,b,a</returns>
        private static List<double> ExtractRGBAValues(string colorString)
        {
            int rgbaOffset = 5;
            colorString = colorString.Substring(rgbaOffset, colorString.Length - rgbaOffset - 1);
            List<double> rgbaValues = colorString.Split(',').Select(sC => double.Parse(sC.Trim(), System.Globalization.CultureInfo.InvariantCulture)).ToList();
            return rgbaValues;
        }

        /// <summary>
        /// Generates gradient color value based on start, min color and value percentage
        /// </summary>
        /// <param name="startColor">startColor</param>
        /// <param name="endColor">endColor</param>
        /// <param name="valueP">valueP</param>
        /// <returns>color value</returns>
        public static string GenerateGradientColor(string startColor, string endColor, double valueP)
        {
            // Extract rgba values from start & end colors
            // extracting r, g, b values from the start color string
            List<double> startRGBAValues = ExtractRGBAValues(startColor);
            double rMin = startRGBAValues[0];
            double gMin = startRGBAValues[1];
            double bMin = startRGBAValues[2];
            double aMin = startRGBAValues[3];

            // extracting r, g, b values from the end color string
            List<double> endRGBAValues = ExtractRGBAValues(endColor);
            double rMax = endRGBAValues[0];
            double gMax = endRGBAValues[1];
            double bMax = endRGBAValues[2];
            double aMax = endRGBAValues[3];

            // Defining color domain for the calculation
            // Should have minimum 2 percentColors
            List<ConditionalFormat.ByScale.ColorPercentage> percentColors = new List<ConditionalFormat.ByScale.ColorPercentage>
    {
        new ConditionalFormat.ByScale.ColorPercentage { Percentage = 0.0, R = rMin, G = gMin, B = bMin, A = aMin },
        new ConditionalFormat.ByScale.ColorPercentage { Percentage = 1.0, R = rMax, G = gMax, B = bMax, A = aMax }
    };

            // Find rgba color string for the given value
            int i;
            for (i = 1; i < percentColors.Count - 1; i++)
            {
                if (valueP < percentColors[i].Percentage)
                {
                    break;
                }
            }

            // Finds the colorValue for the given percentage based on min and max colors
            ConditionalFormat.ByScale.ColorPercentage lower = percentColors[i - 1];
            ConditionalFormat.ByScale.ColorPercentage upper = percentColors[i];
            double rangeP = upper.Percentage - lower.Percentage;
            double rangePct = (valueP - lower.Percentage) / rangeP;
            double pctLower = 1 - rangePct;
            double pctUpper = rangePct;
            int rAverage = (int)Math.Floor((lower.R * pctLower) + (upper.R * pctUpper));
            int gAverage = (int)Math.Floor((lower.G * pctLower) + (upper.G * pctUpper));
            int bAverage = (int)Math.Floor((lower.B * pctLower) + (upper.B * pctUpper));
            int aAverage = (int)Math.Floor((lower.A * pctLower) + (upper.A * pctUpper));

            // Formatting RGBA color string from the calculated values
            string colorValue = string.Format("rgba({0}, {1}, {2}, {3})", rAverage, gAverage, bAverage, aAverage);
            return colorValue;
        }
    }
}


namespace ConditionalFormat.ByScale
{
    public class ColorPercentage
    {
        public double Percentage { get; set; }
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public double A { get; set; }
    }
}