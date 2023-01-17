using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Threading.Tasks;
using Synergy.App.ViewModel.Pay;

namespace Synergy.App.Api.Areas.Pay.Controllers
{
    [Route("pay/MandetoryDeduction")]
    [ApiController]
    public class MandetoryDeductionController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        public MandetoryDeductionController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IPayrollBusiness payrollBusiness,
            IPayrollRunBusiness payrollRunBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _payrollRunBusiness = payrollRunBusiness;
        }

        [HttpGet]
        [Route("GetListData")]
        public async Task<IActionResult> GetListData(string Id)
        {
            var list = await _payrollRunBusiness.GetSingleElementById(Id);
            return Ok(list);

        }

        [HttpGet]
        [Route("CreateElement")]
        public async Task<IActionResult> CreateElement(string Id)
        {
            var model = new MandatoryDeductionElementViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                var result = await _payrollRunBusiness.GetSingleElementEntryById(Id);
                if (result != null)
                {
                    model = result;
                    model.DataAction = DataActionEnum.Edit;
                }
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.MandatoryDeductionId = Id;    
            }
            return Ok(model);
        }

        [HttpGet]
        [Route("GetPayrollElementData")]
        public async Task<IActionResult> GetPayrollElementData()
        {
            var list = await _payrollRunBusiness.GetPayRollElementName();
            return Ok(list);
        }


        [HttpPost]
        [Route("ManageMandatoryDeductionElement")]
        public async Task<IActionResult> ManageMandatoryDeductionElement(MandatoryDeductionElementViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                await _payrollRunBusiness.CreateMandatoryDeductionElement(model);
                return Ok(new { success = true });
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                await _payrollRunBusiness.EditMandatoryDeductionElement(model);
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }


        [HttpGet]
        [Route("DeleteElementData")]
        public async Task<IActionResult> DeleteElementData(string Id)
        {
            await _payrollRunBusiness.DeleteMandatoryDeductionElement(Id);
            return Ok(new { success = true });
        }

        

        [HttpGet]
        [Route("GetSlabListData")]
        public async Task<IActionResult> GetSlabListData(string Id)
        {
            var list = await _payrollRunBusiness.GetSingleSlabById(Id);

            return Ok(list);

        }
        [HttpGet]
        [Route("CreateSlab")]
        public async Task<IActionResult> CreateSlab(string Id)
        {

            var model = new MandatoryDeductionSlabViewModel();
            if (Id.IsNotNullAndNotEmpty())
            {
                var result = await _payrollRunBusiness.GetSingleSlabEntryById(Id);
                if (result != null)
                {
                    model = result;
                    model.DataAction = DataActionEnum.Edit;
                }
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.MandatoryDeductionId = Id;
            }
            return Ok(model);
        }


        [HttpPost]
        [Route("ManageMandatoryDeductionSlab")]
        public async Task<IActionResult> ManageMandatoryDeductionSlab(MandatoryDeductionSlabViewModel model)
        {
            if (!(model.SlabFrom <= model.SlabTo))
            {
                return Ok(new { success = false, error = "SlabFrom should not be greater than SlabTo." });
            }
            if (!(model.ESD <= model.EED))
            {
                return Ok(new { success = false, error = "Effective Start Date should not be greater than Effective End Date." });
            }
            var IsExist = await _payrollRunBusiness.ValidateMandatoryDeductionSlab(model);
            if (IsExist)
            {
                return Ok(new { success = false, error = "Slab with Effective Date range already exist." });
            }

            if (model.DataAction == DataActionEnum.Create)
            {
                await _payrollRunBusiness.CreateMandatoryDeductionSlab(model);
                return Ok(new { success = true });
            }
            else if (model.DataAction == DataActionEnum.Edit)
            {
                await _payrollRunBusiness.EditMandatoryDeductionSlab(model);
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpGet]
        [Route("DeleteSlabData")]
        public async Task<IActionResult> DeleteSlabData(string id)
        {
            await _payrollRunBusiness.DeleteMandatoryDeductionSlab(id);
            return Ok(new { success = true });
        }
    }
}
