using CMS.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMS.UI.Web
{

    public class HttpModuleMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpModuleMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;
            // context.Response.Headers.Add("x-frame-options", "SAMEORIGIN");
            if (context.Request.QueryString.HasValue)
            {
                var query = context.Request.QueryString.Value;
                if (context.Request.QueryString.Value.StartsWith("?enc="))
                {
                    var dq = Helper.DecryptJavascriptAesCypher(query.Replace("?enc=", ""));
                    var q = $"?{dq}";
                    context.Request.QueryString = QueryString.FromUriComponent(q);
                }
                //var query = context.Request.QueryString.Value;
                //var path = context.Request.Path.Value;
                //if (context.Request.Path.Value.Contains(".js") || context.Request.Path.Value.Contains(".js"))
                //{

                //}
                //else if (context.Request.QueryString.Value.StartsWith("?enc="))
                //{
                //    var dq = Helper.DecryptJavascriptAesCypher(query.Replace("?enc=", ""));
                //    var q = $"?{dq}";
                //    context.Request.QueryString = QueryString.FromUriComponent(q);
                //}
                //else if (context.Request.Method== "GET")
                //{
                //    var eq = Helper.EncryptQS(query.Replace("?", ""));
                //    var q = $"{path}?enc={eq}";
                //    context.Response.Redirect(q);
                //}
            }
            //#if !DEBUG
            //if (context.Request..RawUrl.Contains("?"))
            //{
            //    var query = Helper.GetQueryString(context.Request.RawUrl);
            //    var path = Helper.GetVirtualPath();
            //    var contentType = context.Request.ContentType;
            //    //skip for css and js files

            //    if (query.StartsWith(Constant.QueryStringVersionParam, StringComparison.OrdinalIgnoreCase) ||
            //       context.Request.RawUrl.Contains(".axd") ||
            //       query.StartsWith("ReturnUrl", StringComparison.OrdinalIgnoreCase)
            //        )
            //    {
            //        //Do nothing

            //    }
            //    else if (query.StartsWith(Constant.QueryStringEncryptionParam, StringComparison.OrdinalIgnoreCase))
            //    {
            //        var decryptedQuery = Helper.DecryptedQueryString(query);
            //        context.RewritePath(path, string.Empty, decryptedQuery);
            //    }
            //    else if (context.Request.HttpMethod == "GET")
            //    {
            //        // LogManager.Write("plain query: ", query);
            //        var encQuery = Helper.EncryptedQueryString(query);
            //        var redirectUrl = String.Concat(path, encQuery);
            //        // LogManager.Write("encoded query: ", encQuery);
            //        context.Response.Redirect(redirectUrl);
            //    }
            //}
            //#endif
            await _next(context);

            // Do tasks after middleware here, aka 'EndRequest'
            // ...
        }
    }
}
