using Synergy.App.Business;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.TMS.Controllers
{
    [Area("Tms")]
    public class TicketManagementController : ApplicationController
    {
        ITicketAssessmentBusiness _ticketManagementBusiness;
        public TicketManagementController(ITicketAssessmentBusiness ticketManagementBusiness)
        {
            _ticketManagementBusiness = ticketManagementBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> HelpdeskDashboard()
        {
            
                var data= await _ticketManagementBusiness.GetRequestForServiceChart("");
            var salViolatedData = await _ticketManagementBusiness.GetOverDueRequest();
            ViewBag.OpenRequestCount = data.Count();
            ViewBag.SLAViolated = salViolatedData.Count();
            ViewBag.ServiceApproachingViolationInaMin = data.Where(x=>(DateTime.Now-x.DueDate).Value.TotalMinutes <= 60).Count();
           // ViewBag.ServiceApproachingViolationIn120Min = data.Where(x => (DateTime.Now - x.DueDate).Value.TotalMinutes <= 120 && ).Count();
            ViewBag.ServiceApproachingViolation = data.Where(x => (DateTime.Now - x.DueDate).Value.TotalMinutes > 60).Count();
            return View();
        }

        public async Task<IActionResult> ReadHelpDeskBox1(string type)
        {
            var data = await _ticketManagementBusiness.GetRequestCounts(type);
            return Json(data);
        }
        
        public async Task<IActionResult> ReadHelpeskSLAViolation(string type)
        {
            var data = await _ticketManagementBusiness.GetRequestCountsSLAViolation(type);
            return Json(data);
        }
        public async Task<ActionResult> GetRequestChartStatus(string type)
        {
            HelpDeskViewModel search = new HelpDeskViewModel();            
            var viewModel = await _ticketManagementBusiness.GetRequestForServiceChart(type);
            if (type == "Category")
            {
                var list1 = viewModel.GroupBy(x => x.CategoryCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.Name).FirstOrDefault(), Id = group.Select(x => x.Name).FirstOrDefault() }).ToList();
                var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
                return Json(list);
            }
            else if (type == "Service")
            {
                var list1 = viewModel.GroupBy(x => x.TemplateCode).Select(group => new { Value = group.Count(), Type = group.Select(x => x.TemplateName).FirstOrDefault(), Id = group.Select(x => x.TemplateName).FirstOrDefault() }).ToList();
                var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
                return Json(list);
            }
            else
            {
                var list1 = viewModel.GroupBy(x => x.Priority).Select(group => new { Value = group.Count(), Type = group.Select(x => x.Priority).FirstOrDefault(), Id = group.Select(x => x.Priority).FirstOrDefault() }).ToList();
                var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
                return Json(list);
            }
           
        }
        
        public async Task<IActionResult> ReadHelpDeskBox2( string type)
        {
            var data = await _ticketManagementBusiness.GetChartData(type);
            return Json(data);
        }

        public async Task<IActionResult> ReadPendingTaskCounts([DataSourceRequest] DataSourceRequest request)
        {
            var data = await _ticketManagementBusiness.GetPendingTaskCountWithAssignee();
            return Json(data);
            //return Json(data.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadHelpDeskRequestRecieved()
        {
            var data = await _ticketManagementBusiness.GetServiceSLAVoilationInLast20Days();
            return Json(data);
        }
         public async Task<IActionResult> ReadHelpDeskRequestClosed()
        {
            var data = await _ticketManagementBusiness.GetServiceSLAVoilationCompletedIn20Days();
            return Json(data);
        }

    }
}
