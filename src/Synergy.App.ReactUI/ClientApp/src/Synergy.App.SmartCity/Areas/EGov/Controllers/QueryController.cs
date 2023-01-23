using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace Synergy.App.SmartCity.Areas.EGov.Controllers
{
    [Route("egov/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("GetJSCBillPaymentReport")]
        public async Task<IActionResult> GetJSCBillPaymentReport(string serviceNo)
        {
            var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
            var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
            var _portalBusiness = _serviceProvider.GetService<IPortalBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();

            try
            {
                var model = new JSCBillPaymentReportViewModel();
                var servicedata = await _serviceBusiness.GetSingle(x => x.ServiceNo == serviceNo);
                if (servicedata!=null)
                {
                    model = await _smartCityBusiness.GetJSCBillPaymentDetails(servicedata.Id);
                    if (model.IsNotNull())
                    {
                        model.DueDateText = string.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, model.DueDate);
                    }
                    else
                    {
                        model = new JSCBillPaymentReportViewModel();
                    }
                    var portaData = await _portalBusiness.GetSingle(x => x.Name == "JammuSmartCityCustomer");
                    if (portaData!=null)
                    {
                        if (portaData.LogoId.IsNotNullAndNotEmpty())
                        {
                            var bytes = await _fileBusiness.GetFileByte(portaData.LogoId);
                            if (bytes.Length > 0)
                            {
                                model.Logo = Convert.ToBase64String(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    else
                    {
                        var comp = await _companyBusiness.GetSingle(x => x.Code == "SYNERGY");
                        var bytes = await _fileBusiness.GetFileByte(comp.LogoFileId);
                        if (bytes.Length > 0)
                        {
                            model.Logo = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetPropertyTaxReport/{ddNo}")]
        public async Task<IActionResult> GetPropertyTaxReport(string ddNo,string year)
        {
            var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
            
            try
            {
                var model = await _smartCityBusiness.GetPropertyTaxReportData(ddNo,year);
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetPropertyTaxReportFloorData/{ddNo}")]
        public async Task<IActionResult> GetPropertyTaxReportFloorData(string ddNo, string year)
        {
            var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();

            try
            {
                var model = await _smartCityBusiness.GetPropertyTaxReportFloorData(ddNo, year);
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
    }
}
