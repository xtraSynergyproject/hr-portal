using AutoMapper;
using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
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
    public class SalaryController : ApplicationController
    {
        private readonly ISalaryEntryBusiness _salaryEntryBusiness;
        public SalaryController(ISalaryEntryBusiness salaryEntryBusiness)
        {
            _salaryEntryBusiness = salaryEntryBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Payslip(int? year = null, MonthEnum? month = null, string personId = null)
        {
            year = year ?? DateTime.Today.Year;
            month = month ?? (MonthEnum)DateTime.Today.Month;
            var date = DateTime.Now.ApplicationNow().Date;
            var model = new SalaryEntryViewModel { Year = year, Month = month };
            var yearMonth = string.Concat(year, ((int)month).ToString().PadLeft(2, '0')).ToSafeInt();
            if (personId.IsNotNull())
            {
                model.PersonId = personId;
            }
            //model.Elements = _businessSalaryElement.GetList(x => x.YearMonth == yearMonth).Where(x => x.Name.IsNotNullAndNotEmpty()).DistinctBy(x => x.Name).OrderBy(x => x.Name).Select(x => x.Name).ToArray();
            return View(model);
        }
        public async  Task<ActionResult> ReadSalaryData(SalaryEntryViewModel search = null)
        {
            var model = new List<SalaryEntryViewModel>();
            if (search.Month != null && search.Year != null)
            {
                search.YearMonth = string.Concat(search.Year, ((int)search.Month).ToString().PadLeft(2, '0')).ToSafeInt();
                model = await _salaryEntryBusiness.GetPaySalaryDetails(search);
            }
            var j = Json(model);
            return j;

        }
    }
}
