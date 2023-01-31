using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMS.UI.Utility
{
    public static class WebExtension
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, Dictionary<string, string> messages)
        {
            foreach (var item in messages)
            {
                modelState.AddModelError(item.Key, item.Value);
            }
        }
        public static string GetCultureName(HttpContext context)
        {

            if (context != null && context.User != null && context.User.HasClaim(x => x.Type == ClaimTypes.WindowsDeviceClaim))
            {
                return context.User.FindFirst(ClaimTypes.WindowsDeviceClaim).Value;
            }
            return "en-US";
        }


    }
}
