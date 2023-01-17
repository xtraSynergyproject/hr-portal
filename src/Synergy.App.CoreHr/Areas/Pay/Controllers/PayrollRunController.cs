using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class PayrollRunController : Controller
    {
        private readonly ILegalEntityBusiness _legalEntityBusiness;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        IUserContext _UserContext;

        public PayrollRunController(ILegalEntityBusiness legalEntityBusiness, IPayrollRunBusiness payrollRunBusiness, ICmsBusiness cmsBusiness,
            IPayrollBatchBusiness payrollBatchBusiness, IUserContext userContext)
        {
            _legalEntityBusiness = legalEntityBusiness;
            _payrollRunBusiness = payrollRunBusiness;
            _cmsBusiness = cmsBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
            _UserContext = userContext;
        }

        public IActionResult Index()
        {
            return View();
        }
       

        public async Task<JsonResult> ReadPayrollBatchList()
        {
            //var paybatchlist = await _cmsBusiness.GetDataListByTemplate("PayrollRun", "");
            
            var paybatchlist = await _payrollRunBusiness.GetPayrollBatchList();
            var dsResult = paybatchlist;
            return Json(dsResult);
        }

        public async Task<ActionResult> Manage(string payrollId, string id)
        {
            ViewBag.Title = "Payroll Details";
           
           var batchmodel = await _payrollBatchBusiness.ViewModelList(payrollId);
           var view = batchmodel.FirstOrDefault();            
            
            var payrunmodel = await _payrollRunBusiness.ViewModelList(id);
            var model = payrunmodel.FirstOrDefault();

            model.DataAction = DataActionEnum.Edit;
            model.Name = view.Name;
            model.Description = view.Description;
            model.PayrollGroupId = view.PayrollGroupId;
            model.YearMonth = view.YearMonth;
            model.PayrollStartDate = view.PayrollStartDate;
            model.PayrollEndDate = view.PayrollEndDate;
            model.PayrollGroupName = view.PayrollGroupName;            
                       
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(PayrollRunViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ExecutedBy = _UserContext.UserId;
                //if (model.DataAction == DataActionEnum.Create)
                //{
                //    //return CreatePayrollRun(model);
                //}
                if (model.DataAction == DataActionEnum.Edit)
                {
                    return await CorrectPayrollRun(model);
                }

                //else if (model.DataAction == DataActionEnum.Delete)
                //{
                //    //return DeletePayrollRun(model);
                //}
                //else
                //{
                //    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                //    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                //}
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }

        private async Task<IActionResult> CorrectPayrollRun(PayrollRunViewModel model)
        {
            var result = await _payrollRunBusiness.Correct(model);
            if (result==null)
            {
                //result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), id = model.Id, state = model.PayrollStateEnd.ToString() });
            }
        }

        //public async Task<ActionResult> ReadPayrollRunPersonData(string payrollGroupId, string payrollId, string payrollRunId, PayrollStateEnum payrollState, int yearMonth)
        //{    
        //    var model = await _payrollRunBusiness.PayrollRunPersonData(payrollGroupId, payrollId, payrollRunId, DateTime.Today, yearMonth);

        //    var j = Json(model);
        //    return j;

        //}
        public async Task<ActionResult> ReadPayrollRunPersonData(string payrollGroupId, string payrollId, string payrollRunId, PayrollStateEnum payrollState, int yearMonth)
        {
            var model = await _payrollRunBusiness.PayrollRunPersonData(payrollGroupId, payrollId, payrollRunId, DateTime.Today, yearMonth);

            var j = Json(model);
            return j;

        }

        [HttpPost]
        public async Task<ActionResult> AddPersonToPayrollRun(string payrollRunId, string persons, string payRunNoteId)
        {
            await _payrollRunBusiness.AddPersonToPayrollRun(payrollRunId, persons, payRunNoteId);
            return Json(new { success = true });
        }

        public async Task<ActionResult> ReadPayrollRunListData( string payrollGroupId, string payrollId, string payrollRunId, PayrollStateEnum payrollState, int yearMonth)
        {
            var param = new Dictionary<string, object>();
            param.Add("Id", payrollId);

            var model = await _payrollRunBusiness.GetPayrollRunData(payrollGroupId, payrollId, payrollRunId, DateTime.Today, yearMonth);

            var j = Json(model);
            return j;

        }
        public async Task<ActionResult>  ReadPayrollData()
        {
            var list = await _payrollRunBusiness.GetPayrollRunList();
            var j = Json(list);
           
            return j;

        }
        public async Task<ActionResult> CorrectAccural(string payrollId, string payrollRunId)
        {
            //var param = new Dictionary<string, object>();
            //param.Add("payrollId", payrollId);
            //param.Add("payrollRunId", payrollRunId);
            var viewlist =await _payrollBatchBusiness.ViewModelList(payrollId);
            var view = viewlist.FirstOrDefault();
            var modellist =await _payrollRunBusiness.ViewModelList(payrollRunId);
            var model= modellist.FirstOrDefault();
            if (model.IsNotNull()) 
            {
                model.DataAction = DataActionEnum.Edit;
                model.Name = view.Name;
                model.Description = view.Description;
                model.PayrollGroupId = view.PayrollGroupId;
                model.PayrollBatchId = view.Id;
                model.YearMonth = view.YearMonth;
                model.PayrollStartDate = view.PayrollStartDate;
                model.PayrollEndDate = view.PayrollEndDate;
                model.PayrollGroupName = view.PayrollGroupName;
                model.OrganizationName = view.OrganizationName;
            }
           
            return View("ManageAccural", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageAccural(PayrollRunViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Edit)
                {
                    var result =await _payrollRunBusiness.EditAccrual(model);
                    if (!result.IsSuccess)
                    {
                        result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                        return Json(new { success = false, errors = ModelState.SerializeErrors() });
                    }
                    else
                    {

                        return Json(new { success = true, operation = model.DataAction.ToString(), id = model.Id });
                    }
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPayrollClosedCount()
        {
            var list = await _payrollRunBusiness.GetPayrollRunList();
            foreach (var l in list)
            {
                if (l.PayrollStateEnd != PayrollStateEnum.ClosePayroll)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = true });

        }
       
    }
}
