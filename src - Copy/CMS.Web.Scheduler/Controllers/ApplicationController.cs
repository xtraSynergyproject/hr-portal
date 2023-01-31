using CMS.Business;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Scheduler
{
    public class ApplicationController : Controller
    {
        
        public IActionResult PopupRedirect(string message, bool reloadParentOnClose = false, bool enableBackButton = false, string backButtonUrl = "", string backButtonText = "", bool enableCreateNewButton = false, string createNewButtonUrl = "", string createNewButtonText = "")
        {
            return RedirectToAction("popupsuccess", "home", new { Area = "", msg = message, erp = reloadParentOnClose, ebb = enableBackButton, bburl = backButtonUrl, bbText = backButtonText, ecb = enableCreateNewButton, cburl = createNewButtonUrl, cbText = createNewButtonText });
        }
        //public string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        //{
        //    // first find the ViewEngine for this view
        //    ViewEngineResult viewEngineResult = null;
        //    if (partial)
        //        viewEngineResult = Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.Found()
        //    else
        //        viewEngineResult = Microsoft.AspNetCore.Mvc.ViewEngines.Engines.FindView(context, viewPath, null);

        //    if (viewEngineResult == null)
        //        throw new FileNotFoundException("View cannot be found.");

        //    // get the view and attach the model to view data
        //    var view = viewEngineResult.View;
        //    context.Controller.ViewData.Model = model;

        //    string result = null;

        //    using (var sw = new StringWriter())
        //    {
        //        var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
        //        view.Render(ctx, sw);
        //        result = sw.ToString();
        //    }

        //    return result;
        //}
        //public async Task<string> RenderViewToStringAsync(IActionResult actionResult, IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor)
        //{
        //    if (actionResult == null) throw new ArgumentNullException(nameof(actionResult));

        //    var httpContext = new DefaultHttpContext
        //    {
        //        RequestServices = serviceProvider
        //    };

        //    var actionContext = new ActionContext(httpContext, contextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());

        //    using (var stream = new MemoryStream())
        //    {
        //        httpContext.Response.Body = stream; // inject a convenient memory stream
        //        try
        //        {
        //            await actionResult.ExecuteResultAsync(actionContext); // execute view result on that stream
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }

        //        httpContext.Response.Body.Position = 0;
        //        return new StreamReader(httpContext.Response.Body).ReadToEnd(); // collect the content of the stream
        //    }
        //}
        public async Task<string> RenderViewToStringAsync(string viewName, dynamic model, IHttpContextAccessor contextAccessor, IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider)
        {
            try
            {
                var actionContext = new ActionContext(contextAccessor.HttpContext, contextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());

                await using var sw = new StringWriter();
                var viewResult = FindView(actionContext, viewName, razorViewEngine);

                if (viewResult == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.RenderAsync(viewContext);
                return sw.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        private IView FindView(ActionContext actionContext, string viewName, IRazorViewEngine razorViewEngine)
        {
            var getViewResult = razorViewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = razorViewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }
    }

}
