using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility
{
    public static class EnumerableExtension
    {
        public static string ToHtmlTable<T>(this IEnumerable<T> list, List<string> headerList, List<CustomTableStyle> customTableStyles, params Func<T, object>[] columns)
        {
            if (customTableStyles == null)
                customTableStyles = new List<CustomTableStyle>();

            var tableCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Table).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var trCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tr).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var tcCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var tc1Css = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc1).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var tc2Css = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc2).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var tc3Css = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc3).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var thCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Th).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var tdCss = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Td).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";
            var td1Css = string.Join(" ", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Td1).Where(w => w.ClassNameList != null).SelectMany(s => s.ClassNameList)) ?? "";

            var tableInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Table).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var trInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tr).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var thInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Th).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var tdInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Td).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var tcInlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var tc1InlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc1).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var tc2InlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc2).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var tc3InlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Tc3).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";
            var td1InlineCss = string.Join(";", customTableStyles?.Where(w => w.CustomTableStylePosition == CustomTableStylePosition.Td1).Where(w => w.InlineStyleValueList != null).SelectMany(s => s.InlineStyleValueList?.Select(x => String.Format("{0}:{1}", x.Key, x.Value)))) ?? "";

            var sb = new StringBuilder();

            sb.Append($"<table{(string.IsNullOrEmpty(tableCss) ? "" : $" class=\"{tableCss}\"")}{(string.IsNullOrEmpty(tableInlineCss) ? "" : $" style=\"{tableInlineCss}\"")}>");
            if (headerList != null)
            {
                sb.Append($"<tr{(string.IsNullOrEmpty(trCss) ? "" : $" class=\"{trCss}\"")}{(string.IsNullOrEmpty(trInlineCss) ? "" : $" style=\"{trInlineCss}\"")}>");
                foreach (var header in headerList)
                {
                    sb.Append($"<th{(string.IsNullOrEmpty(thCss) ? "" : $" class=\"{thCss}\"")}{(string.IsNullOrEmpty(thInlineCss) ? "" : $" style=\"{thInlineCss}\"")}>{header}</th>");
                }
                sb.Append("</tr>");
            }
            foreach (var item in list)
            {
                var ite = item.GetType().ToString();
                if (item.GetType().ToString() == "ERP.UI.ViewModel.AttendanceViewModel")
                {
                    //dynamic dy = (IDictionary<Object, Object>)item;
                    var otHours = item.GetType().GetProperty("SystemOTHoursText").GetValue(item);
                    var deductionHours = item.GetType().GetProperty("SystemDeductionHoursText").GetValue(item);
                    var rosterText = item.GetType().GetProperty("RosterText").GetValue(item);
                    var accessLogText = item.GetType().GetProperty("AccessLogText").GetValue(item);
                    var systemAttendance = item.GetType().GetProperty("SystemAttendance").GetValue(item);
                    if (rosterText.ToString() != "" && otHours.ToString() != "0" && otHours.ToString() != "00:00")
                    {
                        sb.Append($"<tr{(string.IsNullOrEmpty(tcCss) ? "" : $" class=\"{tcCss}\"")}{(string.IsNullOrEmpty(tcInlineCss) ? "" : $" style=\"{tcInlineCss}\"")}>");
                        foreach (var column in columns)
                            sb.Append($"<td{(string.IsNullOrEmpty(td1Css) ? "" : $" class=\"{td1Css}\"")}{(string.IsNullOrEmpty(td1InlineCss) ? "" : $" style=\"{td1InlineCss}\"")}>{column(item)}</td>");
                        sb.Append("</tr>");
                    }
                    else if (rosterText.ToString() != "" && deductionHours.ToString() != "0" && deductionHours.ToString() != "00:00")
                    {
                        sb.Append($"<tr{(string.IsNullOrEmpty(tc1Css) ? "" : $" class=\"{tc1Css}\"")}{(string.IsNullOrEmpty(tc1InlineCss) ? "" : $" style=\"{tc1InlineCss}\"")}>");
                        foreach (var column in columns)
                            sb.Append($"<td{(string.IsNullOrEmpty(td1Css) ? "" : $" class=\"{td1Css}\"")}{(string.IsNullOrEmpty(td1InlineCss) ? "" : $" style=\"{td1InlineCss}\"")}>{column(item)}</td>");
                        sb.Append("</tr>");
                    }
                    else if (rosterText.ToString() != "" && rosterText.ToString() != "DayOff" && systemAttendance.IsNotNull() && systemAttendance.ToString() == "Absent")
                    {
                        sb.Append($"<tr{(string.IsNullOrEmpty(tc2Css) ? "" : $" class=\"{tc2Css}\"")}{(string.IsNullOrEmpty(tc2InlineCss) ? "" : $" style=\"{tc2InlineCss}\"")}>");
                        foreach (var column in columns)
                            sb.Append($"<td{(string.IsNullOrEmpty(td1Css) ? "" : $" class=\"{td1Css}\"")}{(string.IsNullOrEmpty(td1InlineCss) ? "" : $" style=\"{td1InlineCss}\"")}>{column(item)}</td>");
                        sb.Append("</tr>");
                    }
                    else if (rosterText.ToString() == "DayOff" && accessLogText.ToString() != "")
                    {
                        sb.Append($"<tr{(string.IsNullOrEmpty(tc3Css) ? "" : $" class=\"{tc3Css}\"")}{(string.IsNullOrEmpty(tc3InlineCss) ? "" : $" style=\"{tc3InlineCss}\"")}>");
                        foreach (var column in columns)
                            sb.Append($"<td{(string.IsNullOrEmpty(td1Css) ? "" : $" class=\"{td1Css}\"")}{(string.IsNullOrEmpty(td1InlineCss) ? "" : $" style=\"{td1InlineCss}\"")}>{column(item)}</td>");
                        sb.Append("</tr>");
                    }
                    else
                    {
                        sb.Append($"<tr{(string.IsNullOrEmpty(trCss) ? "" : $" class=\"{trCss}\"")}{(string.IsNullOrEmpty(trInlineCss) ? "" : $" style=\"{trInlineCss}\"")}>");
                        foreach (var column in columns)
                            sb.Append($"<td{(string.IsNullOrEmpty(tdCss) ? "" : $" class=\"{tdCss}\"")}{(string.IsNullOrEmpty(tdInlineCss) ? "" : $" style=\"{tdInlineCss}\"")}>{column(item)}</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append($"<tr{(string.IsNullOrEmpty(trCss) ? "" : $" class=\"{trCss}\"")}{(string.IsNullOrEmpty(trInlineCss) ? "" : $" style=\"{trInlineCss}\"")}>");
                    foreach (var column in columns)
                        sb.Append($"<td{(string.IsNullOrEmpty(tdCss) ? "" : $" class=\"{tdCss}\"")}{(string.IsNullOrEmpty(tdInlineCss) ? "" : $" style=\"{tdInlineCss}\"")}>{column(item)}</td>");
                    sb.Append("</tr>");
                }

            }

            sb.Append("</table>");

            return sb.ToString();
        }

        public class CustomTableStyle
        {
            public CustomTableStylePosition CustomTableStylePosition { get; set; }

            public List<string> ClassNameList { get; set; }
            public Dictionary<string, string> InlineStyleValueList { get; set; }
        }

        public enum CustomTableStylePosition
        {
            Table,
            Tr,
            Th,
            Td,
            Tc,
            Tc1,
            Tc2,
            Tc3,
            Td1
        }
    }
}
