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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    [Authorize(Policy = nameof(AuthorizeCMS))]
    public class ApplicationErrorController : ApplicationController
    {
        private readonly IApplicationErrorBusiness _applicationErrorBusiness;
        public ApplicationErrorController(IApplicationErrorBusiness applicationErrorBusiness)
        {
            _applicationErrorBusiness = applicationErrorBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ExceptionPopUpAsync(string Id)
        {
            var data = await _applicationErrorBusiness.GetSingleById(Id);
            
            return View(data);
        }

        public async Task<JsonResult> ReadApplicationDocumentData()
        {

            var list = await _applicationErrorBusiness.GetList();
            list=list.OrderByDescending(x => x.LastUpdatedDate).ToList();
            return Json(list);
        }

        
    }
}
