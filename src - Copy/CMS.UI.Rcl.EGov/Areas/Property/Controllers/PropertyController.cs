using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.Property.Controllers
{
    [Area("Property")]
    public class PropertyController : ApplicationController
    {
        private readonly IPropertyBusiness _propertyBusiness;
        private readonly IUserContext _userContext;
        public PropertyController(IPropertyBusiness propertyBusiness, IUserContext userContext)
        {

            _propertyBusiness = propertyBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SearchProperty()
        {
            return View("SearchProperty", new PayPropertyTaxViewModel
            {
                DataAction = DataActionEnum.Create,
            });
        }
        public ActionResult PropertyRegistrationIndex(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }
        public async Task<IActionResult> ReadSearchProperty(string city, string propertyId, string ownerName, string oldPropertyId, string houseNo, string street, string mobile, string wardNo, string postalCode, string email)
        {
            //var result = await _pmtBusiness.GetWBSItemData(null, null, null, null, _userContext.UserId, _userContext.UserRoleIds, null, null, null, null, _userContext.UserRoleCodes);
            //return Json(result.ToList());

            var result = await _propertyBusiness.GetPropertySearch( city,  propertyId,  ownerName,  oldPropertyId,  houseNo,  street,  mobile,  wardNo,  postalCode,  email);
            
            return Json(result);
        }       


        public async  Task<IActionResult> TaxCalulation(string PropertyId)
        {

            //PropertyId = "S-07.09.2021-4";
            var model = await _propertyBusiness.GetPropertyTaxbyId(PropertyId);
            model.PropertyTax = 1362;
            model.EducationCess = 648;
            model.UrbanDev = 216;
            model.SewaKar = 450;
            model.SewaKar = 0;
            model.AdditionalSamekitKar = 0;
            model.Rebate = 0;
            model.Penalty = 178;
            model.Total = 2854;
            //PayPropertyTaxViewModel model = new PayPropertyTaxViewModel();

            return View(model);
        }

        public async Task<IActionResult> ReadCurrentYearProperty(string propertyId)
        {
            //var result = await _pmtBusiness.GetWBSItemData(null, null, null, null, _userContext.UserId, _userContext.UserRoleIds, null, null, null, null, _userContext.UserRoleCodes);
            //return Json(result.ToList());

            var result = await _propertyBusiness.GetCurrentYearSummary( propertyId);
            //var dsResult = result.ToDataSourceResult(request);
            return Json(result);
        }

        public IActionResult ReadYearWiseBreakUp()
        {


            List<PayPropertyTaxViewModel> model = new List<PayPropertyTaxViewModel>();
            //var result = await _pmtBusiness.GetWBSItemData(null, null, null, null, _userContext.UserId, _userContext.UserRoleIds, null, null, null, null, _userContext.UserRoleCodes);
            //return Json(result.ToList());

            var result = _propertyBusiness.GetYearWiseBreakUp();
            
            return Json(result);
        }


    }
}
