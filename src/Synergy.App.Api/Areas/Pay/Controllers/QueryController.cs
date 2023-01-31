using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
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

namespace CMS.UI.Web.Api.Areas.Pay.Controllers
{
    [Route("pay/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        IPayrollBatchBusiness _payrollBatchBusiness;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
        IPayrollBatchBusiness payrollBatchBusiness
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _payrollBatchBusiness = payrollBatchBusiness;

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
                return Ok(model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("GetPayGroupList")]
        public async Task<JsonResult> GetPayGroupList()
        {
            var result = await _payrollBatchBusiness.GetPayGroupList();

            return Json(result);
        }

        [HttpGet]
        [Route("GetPayCalenderList")]
        public async Task<JsonResult> GetPayCalenderList()
        {
            var result = await _payrollBatchBusiness.GetPayCalenderList();

            return Json(result);
        }

        [HttpGet]
        [Route("GetPayBankBranchList")]
        public async Task<JsonResult> GetPayBankBranchList()
        {
            var result = await _payrollBatchBusiness.GetPayBankBranchList();

            return Json(result);
        }
    }
}
