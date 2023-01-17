using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.Property.Controllers
{
    [Area("Property")]
    public class PayPropertyTaxController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly IUserInfoBusiness _UserInfoBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPropertyBusiness _propertyBusiness;

        public PayPropertyTaxController(IUserInfoBusiness UserInfoBusiness, IUserContext userContext, IServiceBusiness serviceBusiness, IPropertyBusiness propertyBusiness)
        {

            _UserInfoBusiness = UserInfoBusiness;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _propertyBusiness = propertyBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult> GetPropertyTaxDetails(string PropertyId)
        {

            var model = await _propertyBusiness.GetPropertyTaxbyId(PropertyId);

            //PayPropertyTaxViewModel model =new PayPropertyTaxViewModel { PropertyId=PropertyId,OwnerName="Mirza Kaleem",Locality="Aur" };

            return View(model);
        
        }

        [HttpPost]
        public async Task<ActionResult> SavePropertyTax(PayPropertyTaxViewModel model)
        {

           ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
           serviceModel.ActiveUserId = _userContext.UserId;
           serviceModel.TemplateCode = "PAY_PROPERTY_TAX";
           var service = await _serviceBusiness.GetServiceDetails(serviceModel);
           
           
           service.OwnerUserId = _userContext.UserId;
           service.StartDate = DateTime.Now;
           service.ActiveUserId = _userContext.UserId;
           service.DataAction = DataActionEnum.Create;
           
           service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
           service.Json = JsonConvert.SerializeObject(model);
           
           var res = await _serviceBusiness.ManageService(service);
            return Json(new { success = true });
        }





    }
}
