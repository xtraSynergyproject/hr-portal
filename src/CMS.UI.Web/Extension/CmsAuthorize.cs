using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CmsAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var portalName = context.HttpContext.GetRouteValue("portalName");
            var pageName = context.HttpContext.GetRouteValue("pageName");
            var domain = context.HttpContext.Request.Host;

        }
    }
}
