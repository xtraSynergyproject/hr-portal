using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.TMS.Controller
{
    [Route("tms/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("ReadHelpDeskBox1")]
        public async Task<ActionResult> ReadHelpDeskBox1( string type)
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            var data = await _ticketManagementBusiness.GetRequestCounts(type);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadHelpDeskBox2")]
        public async Task<ActionResult> ReadHelpDeskBox2(string type)
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            var data = await _ticketManagementBusiness.GetChartData(type);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadPendingTaskCounts")]
        public async Task<ActionResult> ReadPendingTaskCounts()
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            var data = await _ticketManagementBusiness.GetPendingTaskCountWithAssignee();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetRequestChartStatus")]
        public async Task<ActionResult> GetRequestChartStatus(string type)
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            HelpDeskViewModel search = new HelpDeskViewModel();
            var viewModel = await _ticketManagementBusiness.GetRequestForServiceChart(type);
            if (type == "Category")
            {
                var list1 = viewModel.GroupBy(x => x.CategoryCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.Name).FirstOrDefault(), Id = group.Select(x => x.Name).FirstOrDefault() }).ToList();
                var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
                return Ok(list);
            }
            else if (type == "Service")
            {
                var list1 = viewModel.GroupBy(x => x.TemplateCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateName).FirstOrDefault(), Id = group.Select(x => x.TemplateName).FirstOrDefault() }).ToList();
                var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
                return Ok(list);
            }
            else
            {
                var list1 = viewModel.GroupBy(x => x.Priority).Select(group => new { Value = group.Count(), Type = group.Select(x => x.Priority).FirstOrDefault(), Id = group.Select(x => x.Priority).FirstOrDefault() }).ToList();
                var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
                return Ok(list);
            }

        }

        [HttpGet]
        [Route("ReadHelpeskSLAViolation")]
        public async Task<ActionResult> ReadHelpeskSLAViolation( string type)
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            var data = await _ticketManagementBusiness.GetRequestCountsSLAViolation(type);
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadHelpDeskRequestRecieved")]
        public async Task<ActionResult> ReadHelpDeskRequestRecieved()
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            var data = await _ticketManagementBusiness.GetServiceSLAVoilationInLast20Days();
            return Ok(data);
        }

        [HttpGet]
        [Route("ReadHelpDeskRequestClosed")]
        public async Task<IActionResult> ReadHelpDeskRequestClosed()
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            var data = await _ticketManagementBusiness.GetServiceSLAVoilationCompletedIn20Days();
            return Ok(data);
        }

        [HttpGet]
        [Route("HelpdeskDashboard")]
        public async Task<IActionResult> HelpdeskDashboard()
        {
            var _ticketManagementBusiness = _serviceProvider.GetService<ITicketAssessmentBusiness>();
            HelpdeskDashboard helpdesk = new HelpdeskDashboard();
            var data = await _ticketManagementBusiness.GetRequestForServiceChart("");
            var salViolatedData = await _ticketManagementBusiness.GetOverDueRequest();
            helpdesk.OpenRequestCount = data.Count();
            helpdesk.SLAViolated = salViolatedData.Count();
            helpdesk.ServiceApproachingViolationInaMin = data.Where(x => (DateTime.Now - x.DueDate).Value.TotalMinutes <= 60).Count();
            // ViewBag.ServiceApproachingViolationIn120Min = data.Where(x => (DateTime.Now - x.DueDate).Value.TotalMinutes <= 120 && ).Count();
            helpdesk.ServiceApproachingViolation = data.Where(x => (DateTime.Now - x.DueDate).Value.TotalMinutes > 60).Count();
            return Ok(helpdesk);
        }
    }

    public class HelpdeskDashboard {

        //public int SalViolatedData { get; set; }
        public int OpenRequestCount { get; set; }
        public int SLAViolated { get; set; }
        public int ServiceApproachingViolationInaMin { get; set; }

        public int ServiceApproachingViolation { get; set; }
    }
}
