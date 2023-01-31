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
    public class PayrollRunResultController : Controller
    {
        private readonly ILegalEntityBusiness _legalEntityBusiness;
        private readonly IPayrollRunBusiness _payrollRunBusiness;
        private readonly IPayrollRunResultBusiness _payrollRunResultBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;

        public PayrollRunResultController(ILegalEntityBusiness legalEntityBusiness, IPayrollRunBusiness payrollRunBusiness, ICmsBusiness cmsBusiness,
            IPayrollRunResultBusiness payrollRunResultBusiness, IPayrollBatchBusiness payrollBatchBusiness)
        {
            _legalEntityBusiness = legalEntityBusiness;
            _payrollRunBusiness = payrollRunBusiness;
            _cmsBusiness = cmsBusiness;
            _payrollRunResultBusiness = payrollRunResultBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
        }

        public async Task<IActionResult> Index(string payrollRunId, string payrollBatchId, ElementCategoryEnum? elementCategory = null, LayoutModeEnum? layoutMode = null, bool? resultOnly = null)
        {
            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            var payrollRun =await _payrollRunBusiness.GetSingleById(payrollRunId);
            if (payrollRun != null && payrollRun.YearMonth.HasValue)
            {
                yearmonth = payrollRun.YearMonth.Value;
                year = yearmonth.ToString().Substring(0, 4).ToSafeInt();
                month = (MonthEnum)yearmonth.ToString().Substring(4, 2).ToSafeInt();
            }

            var model = new PayrollRunResultViewModel
            {
                PayrollRunId = payrollRunId,
                ElementCategory = elementCategory,
                PayrollId = payrollBatchId,

                YearMonth = yearmonth,
                Year = year,
                Month = month
            };

            //model.Elements = await _payrollRunResultBusiness.GetDistinctElement(payrollRunId, elementCategory);            

            ViewBag.ResultOnly = resultOnly.IsTrue();

            return View(model);
            //return View();
        }
        
        public async Task<JsonResult> ReadPayrollBatchList([DataSourceRequest] DataSourceRequest request)
        {
            var paybatchlist = await _cmsBusiness.GetDataListByTemplate("PayrollRun", "");

            //var paybatchlist = await _payrollRunBusiness.GetPayrollBatchList();
            var dsResult = paybatchlist.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [HttpGet]
        public async Task<ActionResult> GetPayrollClosedCount()
        {           
           
            var list =  await _payrollRunBusiness.GetPayrollBatchList();
            foreach (var l in list)
            {
                if (l.PayrollStateEnd != PayrollStateEnum.ClosePayroll)
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = true });

        }
        public async Task<ActionResult> PayrollResult(string payrollRunId, string payrollId, ElementCategoryEnum? elementCategory = null, LayoutModeEnum? layoutMode = null)
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
            //model.OrganizationName = view.OrganizationName;

            //var service = _businessPayrollRun.GetPayrollRunService(model.Id);
            //if (service != null)
            //{
            //    model.PayrollRunServiceId = service.Id;
            //}            

            var runElementList = await _payrollRunBusiness.GetStandardElementListForPayrollRunData(view, view.PayrollGroupId, payrollId, payrollRunId, view.YearMonth.Value);
            model.RunElements = runElementList.Select(x => x.Name).Distinct().ToArray();
            model.SalaryElements = await _payrollRunResultBusiness.GetDistinctElementDisplayName(payrollRunId, elementCategory);

            return View(model);
        }

        public async Task<ActionResult> ReadPaySalaryElementData( string payrollRunId, ElementCategoryEnum? elementCategory)
        {
            var model = await _payrollRunResultBusiness.GetPaySalaryElementDetails(payrollRunId, elementCategory);
            model = model.OrderBy(o => o.PersonNo).ToList();

            var j = Json(model);
            //j.MaxJsonLength = int.MaxValue;
            return j;

        }
    }
}
