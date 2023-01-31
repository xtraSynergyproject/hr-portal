using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.CHR.Controllers
{
    [Route("chr/payrollslip")]
    [ApiController]
    public class PayrollSlipController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISalaryEntryBusiness _salaryEntryBusiness;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public PayrollSlipController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, ISalaryEntryBusiness salaryEntryBusiness, IUserContext userContext) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _salaryEntryBusiness = salaryEntryBusiness;
        }

        [HttpGet]
        [Route("ReadPaySlipData")]
        public async Task<IActionResult> ReadPaySlipData(MonthEnum? month, int? year, string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var model = new List<SalaryEntryViewModel>();
            var search = new SalaryEntryViewModel();
            if (month != null && year != null)
            {
                search.YearMonth = string.Concat(year, ((int)month).ToString().PadLeft(2, '0')).ToSafeInt();
                search.Month = month;
                search.Year = year;
                model = await _salaryEntryBusiness.GetPaySalaryDetails(search);
            }
            return Ok(model);

        }



    }
}
