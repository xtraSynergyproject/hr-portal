using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class HomeController : ApplicationController
    {

        public HomeController()
        {

        }
        public async Task Test(string msg)
        {
            var k = 123;
            //Enqueue<HomeController>(x => x.ExecuteProcessDesignComponent("ad", null));
            //return "we";
        }
        public IActionResult Index(string msg)
        {
            Enqueue<HomeController>(x => x.Test("ad"));
            return View();
        }
        public void Enqueue<T>(Expression<Func<T, Task>> methodCall) where T : HomeController
        {
            var method = methodCall.Compile();
            method.Invoke((T)this);
            var g = methodCall.Compile();

            g.DynamicInvoke(null, null);
            //BackgroundJob.Enqueue<T>(methodCall);
        }
    }
}
