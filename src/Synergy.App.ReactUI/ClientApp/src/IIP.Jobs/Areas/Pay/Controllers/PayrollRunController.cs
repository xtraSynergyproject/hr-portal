using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Synergy.App.ViewModel.Pay;
using Newtonsoft.Json;
using System.Linq;

namespace Synergy.App.Api.Areas.Pay.Controllers
{
    [Route("pay/PayrollRun")]
    [ApiController]
    public class PayrollRunController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;

        public PayrollRunController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider,  IPayrollRunBusiness payrollRunBusiness, ICmsBusiness cmsBusiness,
            IPayrollBatchBusiness payrollBatchBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _payrollRunBusiness = payrollRunBusiness;
            _cmsBusiness = cmsBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
        }



        [HttpGet]
        [Route("ReadPayrollBatchList")]
        public async Task<IActionResult> ReadPayrollBatchList()
        {
            var paybatchlist = await _payrollRunBusiness.GetPayrollBatchList();
            var dsResult = paybatchlist;
            return Ok(dsResult);
        }

        [HttpGet]
        [Route("CreatePayrollRun")]
        public async Task<IActionResult> CreatePayrollRun(string payrollId, string id)
        {
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

            return Ok(model);
        }

        [HttpPost]
        [Route("ManagePayrollRun")]
        public async Task<IActionResult> ManagePayrollRun(PayrollRunViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            model.ExecutedBy = _userContext.UserId;
                if (model.DataAction == DataActionEnum.Edit)
                {
                    return await CorrectPayrollRun(model);
                }

                return Ok(new { success = false, errors = ModelState.SerializeErrors() });
            
        }

        [HttpGet]
        [Route("CorrectPayrollRun")]
        private async Task<IActionResult> CorrectPayrollRun(PayrollRunViewModel model)
        {
            var result = await _payrollRunBusiness.Correct(model);
            if (result == null)
            {
                return Ok(new { success = false, errors = ModelState.ToString() });
            }
            else
            {
                return Ok(new { success = true, operation = model.DataAction.ToString(), id = model.Id, state = model.PayrollStateEnd.ToString() });
            }
        }

        [HttpGet]
        [Route("ReadPayrollRunPersonData")]
        public async Task<IActionResult> ReadPayrollRunPersonData(string payrollGroupId, string payrollId, string payrollRunId, PayrollStateEnum payrollState, int yearMonth)
        {
            var model = await _payrollRunBusiness.PayrollRunPersonData(payrollGroupId, payrollId, payrollRunId, DateTime.Today, yearMonth);

            var j = Ok(model);
            return j;

        }

        [HttpGet]
        [Route("AddPersonToPayrollRun")]
        public async Task<IActionResult> AddPersonToPayrollRun(string payrollRunId, string persons, string payRunNoteId)
        {
            await _payrollRunBusiness.AddPersonToPayrollRun(payrollRunId, persons, payRunNoteId);
            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("ReadPayrollRunListData")]
        public async Task<IActionResult> ReadPayrollRunListData(string payrollGroupId, string payrollId, string payrollRunId, PayrollStateEnum payrollState, int yearMonth)
        {
            //var param = new Dictionary<string, object>();
            //param.Add("Id", payrollId);

            var model = await _payrollRunBusiness.GetPayrollRunData(payrollGroupId, payrollId, payrollRunId, DateTime.Today, yearMonth);

            var j = Ok(model);
            return j;

        }
        [HttpGet]
        [Route("ReadPayrollData")]
        public async Task<IActionResult> ReadPayrollData()
        {
            var list = await _payrollRunBusiness.GetPayrollRunList();
            var j = Ok(list);

            return j;

        }
        [HttpGet]
        [Route("CorrectAccural")]
        public async Task<IActionResult> CorrectAccural(string payrollId, string payrollRunId)
        {
            var viewlist = await _payrollBatchBusiness.ViewModelList(payrollId);
            var view = viewlist.FirstOrDefault();
            var modellist = await _payrollRunBusiness.ViewModelList(payrollRunId);
            var model = modellist.FirstOrDefault();
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

            return Ok( model);
        }
        [HttpPost]
        [Route("ManageAccural")]
        public async Task<IActionResult> ManageAccural(PayrollRunViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _payrollRunBusiness.EditAccrual(model);
                    if (!result.IsSuccess)
                    {
                        // result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                        foreach (var x in result.Messages)
                        {
                            ModelState.AddModelError(x.Key, x.Value);
                        }
                        return Ok(new { success = false, errors = ModelState.SerializeErrors() });
                    }
                    else
                    {

                        return Ok(new { success = true, operation = model.DataAction.ToString(), id = model.Id });
                    }
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                    return Ok(new { success = false, errors = ModelState.SerializeErrors() });
                }
            }
            else
            {
                return Ok(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }

        [HttpGet]
        [Route("GetPayrollClosedCount")]
        public async Task<IActionResult> GetPayrollClosedCount()
        {
            var list = await _payrollRunBusiness.GetPayrollRunList();
            foreach (var l in list)
            {
                if (l.PayrollStateEnd != PayrollStateEnum.ClosePayroll)
                {
                    return Ok(new { success = false });
                }
            }
            return Ok(new { success = true });

        }

    }
}
