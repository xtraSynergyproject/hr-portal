using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
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

namespace CMS.Web.Areas.CHR.Controllers
{
    [Route("CHR/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        [HttpGet]
        [Route("GetLeaveReport/{serviceId}")]
        public async Task<IActionResult> GetLeaveReport(string serviceId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _performanceManagementBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
            var _leaveBalanceSheetBusiness = _serviceProvider.GetService<ILeaveBalanceSheetBusiness>();
            try
            {
                var user = await _hRCoreBusiness.GetUserFullInfo(serviceId);
                var report = new LeaveReportingViewModel();
                if (user != null)
                {
                    report.Email = user.Email;
                    report.OwnerEmployeeNo = user.PersonNo;
                    report.HireDate = user.DateOfJoin;
                    report.Grade = user.GradeName;
                    report.JobName = user.JobName;
                    report.OwnerDepartmentName = user.DepartmentName;
                    report.OwnerLocationtName = user.LocationName;
                    report.OwnerDisplayName = user.PersonFullName;
                    report.Mobile = user.WorkPhone;
                    report.Vacation = user.AnnualLeaveEntitlement;
                    var leavedetail = await _hRCoreBusiness.GetLeaveDetail(user.UserId);
                    if (leavedetail!=null)
                    {
                        var leave = leavedetail.Where(x => x.ServiceId == serviceId).FirstOrDefault();
                        if (leave!=null)
                        {
                            report.LeaveType = leave.LeaveType;
                            report.LeaveDuration = leave.DurationText;
                            report.LeaveStartDate = leave.StartDate.ToDefaultDateFormat();
                            report.LeaveEndDate = leave.EndDate.ToDefaultDateFormat();
                            report.Status = leave.LeaveStatus;
                            report.RequestTime = leave.AppliedDate.ToDefaultDateTimeFormat();
                            report.TelephoneNumber = leave.TelephoneNumber;
                            report.AddressDetail = leave.AddressDetail;
                            report.OtherInformation = leave.OtherInformation;
                            var lvbal = await _leaveBalanceSheetBusiness.GetLeaveBalance(leave.AppliedDate.Value, "ANNUAL_LEAVE", user.UserId);
                            report.LeaveBalance = lvbal.ToString();
                        }
                    }

                }
                return Ok(report);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetLeaveServiceStepTask/{serviceId}")]
        public async Task<IActionResult> GetLeaveServiceStepTask(string serviceId)
        {
            try
            {
                var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
                var stepTaskList = await _componentResultBusiness.GetStepTaskList(serviceId);
                return Ok(stepTaskList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("GetCRPFEmployeeDetails/{employeeId}")]
        public async Task<IActionResult> GetCRPFEmployeeDetails(string employeeId)
        {
            try
            {
                var _candidateProfileBusiness = _serviceProvider.GetService<ICandidateProfileBusiness>();
                var result = await _candidateProfileBusiness.GetCRPFEmployeeDetails(employeeId);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


}
