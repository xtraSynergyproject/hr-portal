using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace CMS.Web.Scheduler
{
    public class LocalizationCustomMiddleware
    {
        RequestDelegate _next;

        public LocalizationCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cc = CultureInfo.CurrentCulture.Name;
            var nc = WebExtension.GetCultureName(context);
            if (cc != nc)
            {
                var ci = (CultureInfo)CultureInfo.GetCultureInfo(nc).Clone();
                ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
                ci.DateTimeFormat.LongDatePattern = "yyyy-MM-ddTHH:mm:ss.fff";
                ci.DateTimeFormat.LongTimePattern = "HH:mm:ss.fff";
                ci.DateTimeFormat.DateSeparator = "-";
                CultureInfo.CurrentCulture = ci;
                CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
            }
            await _next(context);
        }
    }
}
