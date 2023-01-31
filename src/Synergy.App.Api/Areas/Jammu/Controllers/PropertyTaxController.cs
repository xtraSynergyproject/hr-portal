using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.Api.Areas.EGov.Models;
using Synergy.App.Api.Controllers;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Jammu.Controllers
{
    [Route("Jammu/PropertyTax")]
    [ApiController]
    public class PropertyTaxController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly ISmartCityBusiness _smartCityBusiness;
        private readonly IPortalBusiness _portalBusiness;

        public PropertyTaxController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            ISmartCityBusiness smartCityBusiness, IPortalBusiness portalBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _smartCityBusiness = smartCityBusiness;
            _portalBusiness = portalBusiness;
        }

        [HttpGet]
        [Route("GetJSCPendingPaymentsList")]
        public async Task<IActionResult> GetJSCPendingPaymentsList(string portalNames = null, string propertyId = null)
        {
            string ids = null; //_userContext.PortalId;
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            var data = await _smartCityBusiness.GetJSCPaymentsList(ids);
            if (propertyId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.SourceReferenceId == propertyId).ToList();
            }
            var j = Ok(data.Where(x => x.PaymentStatusCode == "JSC_NOT_PAID").OrderBy(x => x.DueDate));

            return j;
        }


        [HttpGet]
        [Route("GetJSCCompletedPaymentsList")]
        public async Task<IActionResult> GetJSCCompletedPaymentsList(string portalNames = null, string propertyId = null)
        {
            string ids = null; //_userContext.PortalId;
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }

            var data = await _smartCityBusiness.GetJSCPaymentsList(ids);
            if (propertyId.IsNotNullAndNotEmpty())
            {
                data = data.Where(x => x.SourceReferenceId == propertyId).ToList();
            }
            var j = Ok(data.Where(x => x.PaymentStatusCode == "JSC_SUCCESS").OrderByDescending(x => x.DueDate));
            return j;
        }


    }
}
