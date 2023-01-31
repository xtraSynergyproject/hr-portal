using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using CMS.UI.Utility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class PayrollTransactionController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private INoteBusiness _noteBusiness;
        private IPayrollTransactionsBusiness _payrollTransaction;
        private ICmsBusiness _cmsBusiness;
        private IPayrollElementBusiness _payrollElementBusiness;
        public PayrollTransactionController(IPayrollTransactionsBusiness payrollTransaction, INoteBusiness noteBusiness, IUserContext userContext,
            ICmsBusiness cmsBusiness, IPayrollElementBusiness payrollElementBusiness)
        {
            _noteBusiness = noteBusiness;
            _payrollTransaction = payrollTransaction;
            _userContext = userContext;
            _cmsBusiness = cmsBusiness;
            _payrollElementBusiness = payrollElementBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetPayrollElementsList()
        {
            var data = await _cmsBusiness.GetPayrollElementList();

            return Json(data);
        }
        public async Task<IActionResult> GetPayrollTraansactions()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("PayrollTransaction", "");
            
            return Json(data);
        }

        public IActionResult CreatePayrollTransaction(string cbm = null)
        {
            var model = new PayrollTransactionViewModel
            {
                DataAction = DataActionEnum.Create,
                EffectiveDate=DateTime.Now.Date
            };
            return View("ManagePayrollTransaction", model);
        }
        public async Task<IActionResult> EditPayrollTransaction(string transactionId, string cbm = null)
        {
            //var model = new NoteTemplateViewModel
            //{
            //    NoteId = transactionId
            //};
            //var newmodel = await _noteBusiness.GetNoteDetails(model);
            var newModel = await _payrollTransaction.GetPayrollTransactionDetails(transactionId);
            var element=await _payrollElementBusiness.GetPayrollElementById(newModel.ElementId);
            newModel.DataAction = DataActionEnum.Edit;
            ViewBag.CallbackMethod = cbm;
            ViewBag.ElementType = element.ElementClassification.ToString();
            return View("ManagePayrollTransaction", newModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> ManagePayrollTransaction(PayrollTransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
              var elementList=await _payrollElementBusiness.GetElementListForPayrollRun(DateTime.Now);
                var element = elementList.Where(x => x.ElementId == model.ElementId).FirstOrDefault();
                if (element.IsNotNull())
                {
                    if (element.ElementClassification == ElementClassificationEnum.Earning)
                    {
                        model.Amount = model.EarningAmount;
                    }
                    else if (element.ElementClassification == ElementClassificationEnum.Deduction) 
                    {
                        model.Amount = model.DeductionAmount*-1;
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
                        return Json(new { success = true });
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
                    newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(newmodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors() });

        }

        public async Task<ActionResult> DeletePayrollTransaction(string NoteId)
        {
            var result = await _payrollTransaction.DeletePayrollTransaction(NoteId);
            return Json(new { success = result });
        }
    }
}
