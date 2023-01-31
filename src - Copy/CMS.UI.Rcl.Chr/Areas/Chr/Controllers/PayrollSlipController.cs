using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class PayrollSlipController : ApplicationController
    {

        private readonly ISalaryEntryBusiness _salaryEntryBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;
        public PayrollSlipController(IHRCoreBusiness hrCoreBusiness,IUserContext UserContext, ISalaryEntryBusiness salaryEntryBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = UserContext;
            _salaryEntryBusiness = salaryEntryBusiness;
        }
       
        public ActionResult Index(string EmpId, int? year = null, MonthEnum? month = null, LayoutModeEnum? lo=null)            
        {            
            year = year ?? DateTime.Today.Year;
            month = month ?? (MonthEnum)DateTime.Today.Month;
            var date = DateTime.Now.ApplicationNow().Date;
            var model = new SalaryEntryViewModel { Year = year, Month = month };
            var yearMonth = string.Concat(year, ((int)month).ToString().PadLeft(2, '0')).ToSafeInt();
            if (EmpId.IsNotNull())
            {
                model.PersonId = EmpId;
            }
            if (lo == LayoutModeEnum.Iframe)
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            //model.Elements = _businessSalaryElement.GetList(x => x.YearMonth == yearMonth).Where(x => x.Name.IsNotNullAndNotEmpty()).DistinctBy(x => x.Name).OrderBy(x => x.Name).Select(x => x.Name).ToArray();
            return View(model);
            
        }
        
        public async Task<IActionResult> ReadPaySlipData( SalaryEntryViewModel search = null)
        {
            var model = new List<SalaryEntryViewModel>();
            if (search.Month != null && search.Year != null)
            {
                search.YearMonth = string.Concat(search.Year, ((int)search.Month).ToString().PadLeft(2, '0')).ToSafeInt();
                model =  await _salaryEntryBusiness.GetPaySalaryDetails(search);
            }
            var j = Json(model);
            return j;

        }





    }
}
