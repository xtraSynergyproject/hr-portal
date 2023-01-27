using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel.Pay;
//using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class MandatoryDeductionController : Controller
    {
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly IUserContext _userContext;

        public MandatoryDeductionController(IPayrollRunBusiness payrollRunBusiness, IUserContext userContext)
        {
            _payrollRunBusiness = payrollRunBusiness;
            _userContext = userContext;
        }

        public IActionResult Index(string MandatoryDeductionId)
        {
            ViewBag.MandatoryDeductionId = MandatoryDeductionId;
            return View();
        }

        public async Task<JsonResult> GetListData(string Id)
        {
            var list = await _payrollRunBusiness.GetSingleElementById(Id);
            return Json(list);

        }

        public IActionResult CreateElement(string Id)
        {
            var model = new MandatoryDeductionElementViewModel()
            {
                DataAction = DataActionEnum.Create,
                //LegalEntityId = _userContext.LegalEntityId
            };
            model.MandatoryDeductionId = Id;
            return View(model);
        }

        public async Task<JsonResult> GetPayrollElementData()
        {
            var list = await _payrollRunBusiness.GetPayRollElementName();
            return Json(list);
        }
       

        [HttpPost]
        public async Task<ActionResult> ManageMandatoryDeductionElement(MandatoryDeductionElementViewModel model)
        {
            

            if(model.DataAction == DataActionEnum.Create)
            {
                await _payrollRunBusiness.CreateMandatoryDeductionElement(model);
                return Json(new { success = true });
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                await _payrollRunBusiness.EditMandatoryDeductionElement(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        public async Task<IActionResult> EditElement(string Id)
        {
            var result = await _payrollRunBusiness.GetSingleElementEntryById(Id);
            if(result != null)
            {
                result.DataAction = DataActionEnum.Edit;
                return View("CreateElement", result);
            }
            return View("CreateElement", new MandatoryDeductionElementViewModel());
        }

        public async Task<IActionResult> DeleteElementData(string Id)
        {
            await _payrollRunBusiness.DeleteMandatoryDeductionElement(Id);
            return Json(new { success = true });
        }

        public IActionResult SlabIndex(string MandatoryDeductionId)
        {
            ViewBag.MandatoryDeductionId = MandatoryDeductionId;
            return View();
        }

        public async Task<JsonResult> GetSlabListData(string Id)
        {
            var list = await _payrollRunBusiness.GetSingleSlabById(Id);
            //return Json(list);

            return Json(list);

        }
        public IActionResult CreateSlab(string Id)
        {
            //create method
            var model = new MandatoryDeductionSlabViewModel()
            {
                DataAction = DataActionEnum.Create,
                MandatoryDeductionId = Id,
            };
            return View(model);

           
        }

        public async Task<IActionResult> EditSlab(string Id)
        {
            var result = await _payrollRunBusiness.GetSingleSlabEntryById(Id);
            if (result != null)
            {
                result.DataAction = DataActionEnum.Edit;
                return View("CreateSlab", result);
            }
            return View("CreateSlab", new MandatoryDeductionSlabViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> ManageMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model)
        {
            if (!(model.SlabFrom<=model.SlabTo))
            {
                return Json(new { success = false, error = "SlabFrom should not be greater than SlabTo." });
            }
            if (!(model.ESD <= model.EED))
            {
                return Json(new { success = false, error = "Effective Start Date should not be greater than Effective End Date." });
            }
            var IsExist = await _payrollRunBusiness.ValidateMandatoryDeductionSlab(model);
            if (IsExist)
            {
                return Json(new { success = false, error = "Slab with Effective Date range already exist." });
            }

            if (model.DataAction == DataActionEnum.Create)
            {
                await _payrollRunBusiness.CreateMandatoryDeductionSlab(model);
                return Json(new { success = true });
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                await _payrollRunBusiness.EditMandatoryDeductionSlab(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> DeleteSlabData(string id)
        {
            await _payrollRunBusiness.DeleteMandatoryDeductionSlab(id);
            return Json(new { success = true });
        }
    }
}
