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

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class PortalDetailsController : ApplicationController

    {

        private IPageBusiness _pageBusiness;
        private readonly IPortalBusiness _portalBusiness;


        public PortalDetailsController(IPageBusiness pageBusiness, IPortalBusiness portalBusiness)
        {
            _pageBusiness = pageBusiness;
            _portalBusiness = portalBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
 
       
        public async Task<ActionResult> ReadData()
        {
            var model = await _pageBusiness.PortalDetails();
            return Json(model);
        }
    }
}