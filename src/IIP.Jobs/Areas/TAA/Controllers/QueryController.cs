using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Api.Controllers;

namespace Synergy.App.Api.Areas.TAA.Controllers
{
    [Route("taa/query")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        [HttpGet]
        [Route("AccessLogList")]
        public async Task<IActionResult> AccessLogList(string personId = null, string userId = null, string userIds = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            try
            {
                if (personId.IsNullOrEmpty())
                {
                    var model = await _hRCoreBusiness.GetAllAccessLogList(startDate, dueDate, userId);
                    return Ok(model);
                }
                else
                {
                    var model = await _hRCoreBusiness.GetAccessLogList(personId, userId, userIds, startDate, dueDate);
                    return Ok(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("AttendanceList")]
        public async Task<IActionResult> AttendanceList(string userId, DateTime? searchFromDate, DateTime? searchToDate, string organisationId, string empStatus, string payrollRunId = null)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _attendanceBusiness = _serviceProvider.GetService<IAttendanceBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            try
            {
                var person = await _hRCoreBusiness.GetPersonDetailByUserId(userId);
                var personIds = new List<string>();
                var organisationIds = new List<string>();
                var empStatuss = new List<string>();
                if (person!=null)
                {
                    personIds.Add(person.Id);
                }
                if (organisationId.IsNotNullAndNotEmpty())
                {
                    organisationIds = organisationId.Split(',').ToList();
                }
                if (empStatus.IsNotNullAndNotEmpty())
                {
                    empStatuss = empStatus.Split(',').ToList();
                    //var empStatusCode = empStatus.Split(',').ToList();
                    //foreach (var s in empStatusCode)
                    //{
                    //    if (s.IsNotNullAndNotEmpty())
                    //    {
                    //        var lov = await _lovBusiness.GetSingle(x=>x.LOVType== "LOV_PERSON_STATUS" && x.Code==s);
                    //        if(lov!=null)
                    //        empStatuss.Add(lov.Id);
                    //    }
                    //}
                }
                var model = await _attendanceBusiness.GetAttendanceListByDate(organisationIds, personIds, searchFromDate, searchToDate, empStatuss, payrollRunId);
                return Ok(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("LeaveDetail")]
        public async Task<IActionResult> LeaveDetail(string userId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            try
            {
                var model = await _hRCoreBusiness.GetLeaveDetail(userId);
                if (model != null)
                {
                    model = model.OrderBy(x=>x.AppliedDate).ToList();
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
