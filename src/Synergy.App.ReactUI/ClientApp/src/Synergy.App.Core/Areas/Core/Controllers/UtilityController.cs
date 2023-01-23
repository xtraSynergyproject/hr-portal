using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.Web;
using Hangfire;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web
{
    public class UtilityController : ApplicationController
    {
        [HttpGet]
        public double DateDiff(string startDate, string endDate)
        {
            var ed = endDate.ToSafeDateTime().Date;
            var sd = startDate.ToSafeDateTime().Date;
            return Synergy.App.Common.Helper.LeaveDuration(sd, ed);
        }
        [HttpGet]
        public TimeSpan GetSLA(string startDate, string endDate)
        {
            var ed = endDate.ToSafeDateTime();
            var sd = startDate.ToSafeDateTime();
            var diff = ed.Subtract(sd);
            return diff;
        }
        [HttpGet]
        public double GetSLAInSeconds(string startDate, string endDate)
        {
            var ed = endDate.ToSafeDateTime();
            var sd = startDate.ToSafeDateTime();
            var diff = ed.Subtract(sd);
            return diff.TotalSeconds;
        }
        [HttpGet]
        public double LeaveDurationForHours(double hours, double workingHours = 8)
        {
            return Helper.LeaveDurationForHours(hours, workingHours);
        }

        [HttpGet]
        public string AddDays(string date, long days, string dateFormat)
        {
            var dt = date.ToSafeDateTime();
            return dt.AddDays(days).ToYYY_MM_DD();
        }
        [HttpGet]
        public string AddSeconds(string date, long seconds)
        {
            var dt = date.ToSafeDateTime();
            var result = dt.AddSeconds(seconds).YYYY_MM_DD_HH_MM();
            return result;
        }
    }
}
