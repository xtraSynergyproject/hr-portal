using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectDashboardController : ApplicationController
    {
        private readonly IProjectManagementBusiness _projectManagementBusiness;

        public ProjectDashboardController(IProjectManagementBusiness projectManagementBusiness)
        {
            _projectManagementBusiness = projectManagementBusiness;
        }
        public async Task<IActionResult> Index()
        {
            var billable = 0.0;
            var nonBillable = 0.0;
            var model = new ProgramDashboardViewModel();
            model= await _projectManagementBusiness.GetPerformanceDashboard();
            ViewBag.Billable = billable;
            ViewBag.NonBillable = nonBillable;
           int v= await GetTimelogload();
            model.AllTime = v;
            return View(model);
        }


        public async Task<ActionResult> GetprojectbyStatus()
        {            
            var model = await _projectManagementBusiness.GetProjectStatus();
            if (model.IsNotNull())
            {
                model = model.OrderBy(x => x.Value).ToList();
                var list = new ProjectDashboardChartViewModel
                {
                    ItemValueLabel = model.Select(x => x.Type).ToList(),
                    ItemValueSeries = model.Select(x => x.Value).ToList()
                };
                return Json(list);
            }
            return null;
            //return Json(model);
        }

        public async Task<ActionResult> GettopfiveProject()
        {
            var model = await _projectManagementBusiness.GetTopfiveProject();
            var j = Json(model);
            return j;
        }

        public async Task<ActionResult> GetTimeLog()
        {
            
            var model = await _projectManagementBusiness.GetTimeLog();

            //foreach (var item in model)
            //{
            //    if (item.Type == "Billable")
            //    {
            //        ViewBag.Billable = item.Days;
            //    }
            //    else { ViewBag.NonBillable = item.Days; }
            //}

            //return Json(model);            

            var list = new ProjectDashboardChartViewModel
            {
                LineChartValueSeries = model.Select(x => x.Days).ToList(),
                ItemValueLabel = model.Select(x=>x.Type).ToList()
            };
            return Json(list);


        }

        public async Task<int> GetTimelogload()
        {
            int sum = 0;
            var model = await _projectManagementBusiness.GetTimeLog();
            foreach (var item in model)
            {
                sum = sum + (int)item.Days;
                if (item.Type == "Billable")
                {
                    ViewBag.Billable = item.Days;
                    
                }
                else { ViewBag.NonBillable = item.Days; }

            }

            return sum;
        }


        public async Task<ActionResult> GetProjectwiseUsers()
        {
            var model = await _projectManagementBusiness.GetProjectwiseUsers();
            //return Json(model);

            var list = new ProjectDashboardChartViewModel
            {
                ItemValueSeries = model.Select(x => x.UserCount).ToList(),
                ItemValueLabel = model.Select(x => x.ProjectName).ToList()
            };
            return Json(list);
        }


        public async Task<ActionResult> GetTaskDetails()
        {
            var model = await _projectManagementBusiness.GetTaskDetails();
            var j = Json(model);
            return j;
        }

        public async Task<ActionResult> GetProjecTaskStatus()
        {
            var model = await _projectManagementBusiness.GetProjecTaskStatus();
            //return Json(model);

            model = model.OrderBy(x => x.Type).ToList();
            var list = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = model.Select(x => x.Type).ToList(),
                ItemValueSeries = model.Select(x => x.Value).ToList()
            };
            return Json(list);
        }

    }
}
