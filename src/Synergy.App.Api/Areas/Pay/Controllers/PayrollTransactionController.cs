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
using Newtonsoft.Json;
using System.Linq;

namespace Synergy.App.Api.Areas.Pay.Controllers
{
    [Route("pay/payrolltransaction")]
    [ApiController]
    public class PayrollTransactionController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private INoteBusiness _noteBusiness;
        private IPayrollTransactionsBusiness _payrollTransaction;
        private ICmsBusiness _cmsBusiness;
        private IPayrollElementBusiness _payrollElementBusiness;
        public PayrollTransactionController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IPayrollTransactionsBusiness payrollTransaction, INoteBusiness noteBusiness, IUserContext userContext,
            ICmsBusiness cmsBusiness, IPayrollElementBusiness payrollElementBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _noteBusiness = noteBusiness;
            _payrollTransaction = payrollTransaction;
            _cmsBusiness = cmsBusiness;
            _payrollElementBusiness = payrollElementBusiness;

        }

        [HttpGet]
        [Route("GetPayrollElementsList")]
        public async Task<IActionResult> GetPayrollElementsList()
        {
            var data = await _cmsBusiness.GetPayrollElementList();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetPayrollTraansactions")]
        public async Task<IActionResult> GetPayrollTraansactions()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("PayrollTransaction", "");

            return Ok(data);
        }

        [HttpGet]
        [Route("CreatePayrollTransaction")]
        public async Task<IActionResult> CreatePayrollTransaction(string transactionId, string cbm = null)
        {
            var model = new PayrollTransactionViewModel();
            if (cbm.IsNotNullAndNotEmpty())
            {
                 model = await _payrollTransaction.GetPayrollTransactionDetails(transactionId);
                var element = await _payrollElementBusiness.GetPayrollElementById(model.ElementId);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.EffectiveDate =  DateTime.Now.Date;
            }
            return Ok(model);
            
        }
       

        [HttpPost]
        [Route("ManagePayrollTransaction")]
        public async Task<IActionResult> ManagePayrollTransaction(PayrollTransactionViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var elementList = await _payrollElementBusiness.GetElementListForPayrollRun(DateTime.Now);
            var element = elementList.Where(x => x.ElementId == model.ElementId).FirstOrDefault();
            if (element.IsNotNull())
            {
                if (element.ElementClassification == ElementClassificationEnum.Earning)
                {
                    model.Amount = model.EarningAmount;
                }
                else if (element.ElementClassification == ElementClassificationEnum.Deduction)
                {
                    model.Amount = model.DeductionAmount * -1;
                }
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var templateModel = new NoteTemplateViewModel
                {
                    ActiveUserId = _userContext.UserId,
                    DataAction = model.DataAction,
                    TemplateCode = "PayrollTransaction"
                };
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = "Payroll Transaction";
                newmodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
            }
            else
            {
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = model.DataAction;
                templateModel.NoteId = model.NtsNoteId;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.NoteSubject = model.NoteSubject;
                newmodel.DataAction = DataActionEnum.Edit;
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel.Json = JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(newmodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
            }


            return Ok(new { success = false, error = ModelState });

        }

        [HttpGet]
        [Route("DeletePayrollTransaction")]
        public async Task<ActionResult> DeletePayrollTransaction(string NoteId)
        {
            var result = await _payrollTransaction.DeletePayrollTransaction(NoteId);
            return Ok(new { success = result });
        }
    }
}
