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
    [Route("pay/PayrollRunResult")]
    [ApiController]
    public class PayrollRunResultController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly IPayrollRunResultBusiness _payrollRunResultBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        public PayrollRunResultController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IPayrollRunBusiness payrollRunBusiness, ICmsBusiness cmsBusiness,
            IPayrollRunResultBusiness payrollRunResultBusiness, IPayrollBatchBusiness payrollBatchBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _payrollRunBusiness = payrollRunBusiness;
            _cmsBusiness = cmsBusiness;
            _payrollRunResultBusiness = payrollRunResultBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
        }

        [HttpGet]
        [Route("ReadPayrollBatchList")]
        public async Task<IActionResult> ReadPayrollBatchList()
        {
            var paybatchlist = await _cmsBusiness.GetDataListByTemplate("PayrollRun", "");
            var result = paybatchlist;
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPayrollClosedCount")]
        public async Task<IActionResult> GetPayrollClosedCount()
        {

            var list = await _payrollRunBusiness.GetPayrollBatchList();
            foreach (var l in list)
            {
                if (l.PayrollStateEnd != PayrollStateEnum.ClosePayroll)
                {
                    return Ok(new { success = false });
                }
            }
            return Ok(new { success = true });

        }

        [HttpGet]
        [Route("PayrollResult")]
        public async Task<IActionResult> PayrollResult(string payrollRunId, string payrollId, ElementCategoryEnum? elementCategory = null, LayoutModeEnum? layoutMode = null)
        {
            var payrollmodel = await _payrollRunBusiness.ViewModelList(payrollRunId);
            var model = payrollmodel.FirstOrDefault();

            if (model != null && payrollId.IsNotNull())
            {
                payrollId = model.PayrollBatchId;
            }

            var batchModel = await _payrollBatchBusiness.ViewModelList(payrollId);
            var view = batchModel.FirstOrDefault();

            model.DataAction = DataActionEnum.Edit;
            model.Name = view.Name;
            model.Description = view.Description;
            model.PayrollBatchId = payrollId;
            model.PayrollGroupId = view.PayrollGroupId;
            model.YearMonth = view.YearMonth;
            model.PayrollStartDate = view.PayrollStartDate;
            model.PayrollEndDate = view.PayrollEndDate;
            model.PayrollGroupName = view.PayrollGroupName;
           
            var runElementList = await _payrollRunBusiness.GetStandardElementListForPayrollRunData(view, view.PayrollGroupId, payrollId, payrollRunId, view.YearMonth.Value);
            model.RunElements = runElementList.Select(x => x.Name).Distinct().ToArray();
            model.SalaryElements = await _payrollRunResultBusiness.GetDistinctElementDisplayName(payrollRunId, elementCategory);

            return Ok(model);
        }

        [HttpGet]
        [Route("ReadPaySalaryElementData")]
        public async Task<IActionResult> ReadPaySalaryElementData(string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            var model = await _payrollRunResultBusiness.GetPaySalaryElementDetails(payrollRunId, elementCategory);
            model = model.OrderBy(o => o.PersonNo).ToList();

            var j = Ok(model);
            return j;

        }

    }
}
