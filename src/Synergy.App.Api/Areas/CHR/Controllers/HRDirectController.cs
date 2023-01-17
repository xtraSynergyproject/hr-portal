using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.DMS.Models;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Api.Areas.CHR.Controllers
{
    [Route("chr/hrdirect")]
    [ApiController]
    public class HRDirectController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDMSDocumentBusiness _documentBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;




        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public HRDirectController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IDMSDocumentBusiness documentBusiness, INoteBusiness noteBusiness,
         IHRCoreBusiness hRCoreBusiness, IPayrollBatchBusiness payrollBatchBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _documentBusiness = documentBusiness;
            _noteBusiness = noteBusiness;
            _hRCoreBusiness = hRCoreBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
        }

        [HttpGet]
        [Route("GetMisconductGridData")]
        public async Task<ActionResult> GetMisconductGridData(string userId,string portalName)
        {
            await Authenticate(userId,portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var model = await _hRCoreBusiness.GetMisconductDetails(userId);
            return Ok(model);
        }

        [HttpGet]
        [Route("EmployeeProfile")]
        public async Task<ActionResult> EmployeeProfile(string userId,string portalName,string personId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            
            var person = await _hRCoreBusiness.GetEmployeeProfile(personId);
            
            return Ok(person);
        }

        [HttpGet]
        [Route("Assignment")]
        public async Task<ActionResult> Assignment(string personId, string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty() ? new string[] { } : _userContext.UserRoleCodes.Split(",");
            var model = await _hRCoreBusiness.GetAssignmentDetails(personId, "");
            var assignment = model.FirstOrDefault();
            assignment.UserRoleCodes = userrole;
            return Ok(assignment);
        }

        [HttpGet]
        [Route("Contract")]
        public async Task<IActionResult> Contract(string userId, string portalName, string personId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty() ? new string[] { } : _userContext.UserRoleCodes.Split(",");
            AssignmentViewModel model = new AssignmentViewModel();
            var data = await _hRCoreBusiness.GetContractDetail(personId);
            if (data.IsNotNull())
            {
                data.UserRoleCodes = userrole;
                return Ok(data);
            }
            model.UserRoleCodes = userrole;
            return Ok(model);
        }

        [HttpGet]
        [Route("Payroll")]
        public async Task<IActionResult> Payroll(string userId, string portalName, string salaryInfoId, string personId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty() ? new string[] { } : _userContext.UserRoleCodes.Split(",");
            var model = new SalaryInfoViewModel();
            if (salaryInfoId.IsNotNullAndNotEmpty())
            {
                var SalaryInfo = await _payrollBatchBusiness.GetSalaryInfoDetails(salaryInfoId);
                if (SalaryInfo != null)
                {
                    model = SalaryInfo.FirstOrDefault();
                }
            }

            if (personId.IsNotNullAndNotEmpty())
            {
                model.PersonId = personId;
            }
            model.UserRoleCodes = userrole;

            return Ok(model);
        }

        [HttpGet]
        [Route("GetPersonVirtualData")]
        public async Task<ActionResult> GetPersonVirtualData(int page, int pageSize, string filters, string hierarchyId, string parentId = null)
        {
            var list = await _hRCoreBusiness.GetFilteredPersonListByOrgId(filters, pageSize, page, null);
           
            return Ok(new { Data = list.Data, Total = list.Total });

        }

        [HttpGet]
        [Route("GetPersonList")]
        public async Task<IActionResult> GetPersonList(string userId, string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var result = await _hRCoreBusiness.GetPersonList();

            return Ok(result);

        }
    }
}
