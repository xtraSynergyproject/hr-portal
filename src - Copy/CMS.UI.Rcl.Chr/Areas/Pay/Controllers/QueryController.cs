using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Route("pay/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        [HttpGet]
        [Route("GetPaySlipDetails/{id}")]
        public async Task<IActionResult> GetPaySlipDetails(string id)
        {
            var _salaryEntryBusiness = _serviceProvider.GetService<ISalaryEntryBusiness>();
            try
            {
                var model = await _salaryEntryBusiness.GetPaySlipHeaderDetails(id);
                if (model != null)
                {
                    string amtInWords = string.Concat(Humanizer.NumberToWordsExtension.ToWords(Convert.ToInt64(model.NetAmount)), " ", model.CurrencyCode, " Only");
                    model.NetAmountInWords = amtInWords.ToUpper();
                    model.PaySlipEarning = await _salaryEntryBusiness.GetEarningElementForPayslipPdf(id);
                    model.PaySlipDeduction = await _salaryEntryBusiness.GetDeductionElementForPayslipPdf(id);
                }
                if (id == "072198e6-59c6-464d-a518-509cab789cfdemo")
                {
                    model = new SalaryEntryViewModel();
                    model.CompanyNameBasedOnLegalEntity = "Synergy";
                    model.SalaryName = "Pay Slip Oct 2021";
                    model.SponsorshipNo = "P9991";
                    model.PersonNo = "P123";
                    model.PersonFullName = "Mohd Aasim";
                    model.JobName = "Sr. Officer";
                    model.OrganizationName = "Synergy Finance";
                    model.DateOfJoin = System.DateTime.Today.AddMonths(-1);
                    model.BankName = "HDFC Bank";
                    model.BankAccountNo = "11009871122334455";
                    model.GrossSalary = 45000.00;
                    model.VacationBalance = 20;
                    model.ActualWorkingDays = 26;
                    model.EmployeeWorkingDays = 21;
                    model.SickLeaveDays = 1;
                    model.AnnualLeaveDays = 1;
                    model.UnpaidLeaveDays = 1;
                    model.OtherLeaveDays = 1;
                    model.TotalEarning = 45000.00;
                    model.TotalDeduction = 5000.00;
                    model.NetAmount = 40000.00;
                    string amtInWords = string.Concat(Humanizer.NumberToWordsExtension.ToWords(Convert.ToInt64(model.NetAmount)), " ", model.CurrencyCode, " Only");
                    model.NetAmountInWords = amtInWords.ToUpper();
                    var earningList = new List<SalaryElementEntryViewModel>();
                    earningList.Add(new SalaryElementEntryViewModel {Name= "Basic Salary",EarningAmount=22000  });
                    earningList.Add(new SalaryElementEntryViewModel {Name= "HRA",EarningAmount=18000  });
                    earningList.Add(new SalaryElementEntryViewModel {Name= "Other Allowance", EarningAmount=5000  });
                    model.PaySlipEarning = earningList;
                    var deductionList = new List<SalaryElementEntryViewModel>();
                    deductionList.Add(new SalaryElementEntryViewModel { Name = "PF", DeductionAmount = 3000 });
                    deductionList.Add(new SalaryElementEntryViewModel { Name = "Tax", DeductionAmount = 2000 });
                    deductionList.Add(new SalaryElementEntryViewModel { Name=" "});
                    model.PaySlipDeduction = deductionList;
                }
                return Ok(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
