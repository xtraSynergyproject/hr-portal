using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class SynergySalesController : ApplicationController

    {
        private ISalesBusiness _salesBusiness;
        private readonly IUserContext _userContext;

        public SynergySalesController(ISalesBusiness salesBusiness, IPortalBusiness portalBusiness,
            IUserContext userContext)
        {
            _salesBusiness = salesBusiness;


            _userContext = userContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> License()
        {
            var url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(Request);
            var model = await _salesBusiness.GetSelfLicense(Helper.GetDomain(new System.Uri(url)));
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ValidateLicense(LicenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(Request);
                var isValid = await _salesBusiness.EvaluateLicense(model.LicenseKey, Helper.GetDomain(new System.Uri(url)));
                if (isValid)
                {
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid License Key");
                    return Json(new { success = false, errors = ModelState.ToHtmlError() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.ToHtmlError() });
            }
        }
    }
}