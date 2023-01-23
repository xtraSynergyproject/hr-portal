using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.REC.Controllers
{
    [Route("rec/RecruitmentMaster")]
    [ApiController]
    public class RecruitmentMasterController : ApiController
    {
        private readonly IRecruitmentTransactionBusiness _recruitmentTransactionBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IServiceProvider _serviceProvider;
        public RecruitmentMasterController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            IRecruitmentTransactionBusiness recruitmentTransactionBusiness) : base(serviceProvider)
        {
            _customUserManager = customUserManager;
            _serviceProvider = serviceProvider;
            _recruitmentTransactionBusiness = recruitmentTransactionBusiness;
        }

        [HttpGet]
        [Route("GetIdNameList")]
        public async Task<IActionResult> GetIdNameList(string type)
        {
            var data = await _recruitmentTransactionBusiness.GetIdNameList(type);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCountryIdNameList")]
        public async Task<IActionResult> GetCountryIdNameList()
        {
            var list = await _recruitmentTransactionBusiness.GetCountryIdNameList();
            return Ok(list);
        }
    }
}
