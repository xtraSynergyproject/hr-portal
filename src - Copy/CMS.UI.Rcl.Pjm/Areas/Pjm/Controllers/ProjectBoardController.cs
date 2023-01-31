using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectBoardController : ApplicationController
    {
        private readonly IProjectManagementBusiness _projectManagementBusiness;

        public ProjectBoardController(IProjectManagementBusiness projectManagementBusiness)
        {
            _projectManagementBusiness = projectManagementBusiness;
        }

        public ActionResult Index(string mode,string permissions)
        {
            var model = new ProgramDashboardViewModel();
            model.Mode = mode;
            ViewBag.Permissions = permissions;
            return View(model);
        }

        public ActionResult ListView(string mode, string permissions)
        {
            var model = new ProgramDashboardViewModel();
            model.Mode = mode;
            ViewBag.Permissions = permissions;
            return View(model);
        }
        public ActionResult TimeLineView(string mode, string permissions)
        {
            var model = new ProgramDashboardViewModel();
            model.Mode = mode;
            ViewBag.Permissions = permissions;
            return View(model);
        }


        public async Task<ActionResult> ReadProjectDataOld([DataSourceRequest] DataSourceRequest request, string mode)
        {
            var list = new List<ProgramDashboardViewModel>();
            if (mode == "Shared")
            {
                list = new List<ProgramDashboardViewModel>()
            {
                new ProgramDashboardViewModel()
                {
                    Id="1",
                    ProjectStatus="COMPLETED",
                    ProjectName="Recruitment - Issues and fix",
                    TemplateName="General Project",
                    CreatedBy="Saman Qureshi",
                    CreatedOn=DateTime.Now,
                    UpdatedOn=DateTime.Now.Date,
                    UserCount=20,
                    AllTaskCount=50,
                    FileCount=12,
                    StartDate=DateTime.Now.Date,
                    DueDate=DateTime.Now.Date,
                },
                new ProgramDashboardViewModel()
                {
                    Id="2",
                    ProjectStatus="INPROGRESS",
                    ProjectName="Synergy-Work-Management-Project",
                    TemplateName="General Project",
                    CreatedBy="System Admin",
                    CreatedOn=DateTime.Now,
                    UpdatedOn=DateTime.Now.Date,
                    UserCount=20,
                    AllTaskCount=50,
                    FileCount=12,
                    StartDate=DateTime.Now.Date,
                    DueDate=DateTime.Now.Date,
                }
            };
            }
            else
            {
                list = new List<ProgramDashboardViewModel>()
            {
                new ProgramDashboardViewModel()
                {
                    Id="1",
                    ProjectStatus="COMPLETED",
                    ProjectName="Recruitment",
                    TemplateName="General Project",
                    CreatedBy="Saman Qureshi",
                    CreatedOn=DateTime.Now,
                    UpdatedOn=DateTime.Now.Date,
                    UserCount=20,
                    AllTaskCount=50,
                    FileCount=12,
                    StartDate=DateTime.Now.Date,
                    DueDate=DateTime.Now.Date,
                },
                new ProgramDashboardViewModel()
                {
                    Id="2",
                    ProjectStatus="INPROGRESS",
                    ProjectName="CMS",
                    TemplateName="General Project",
                    CreatedBy="System Admin",
                    CreatedOn=DateTime.Now,
                    UpdatedOn=DateTime.Now.Date,
                    UserCount=20,
                    AllTaskCount=50,
                    FileCount=12,
                    StartDate=DateTime.Now.Date,
                    DueDate=DateTime.Now.Date,
                }
            };
            }
           
           
            return Json(list.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadProjectData(string mode)
        {
            if (mode == "Shared")
            {
                var list = await _projectManagementBusiness.GetProjectSharedData();
                return Json(list);
            }
            else
            {
                var list = await _projectManagementBusiness.GetProjectData();
                return Json(list);
            }
         
        }

        public async Task<ActionResult> ReadProjectGridData(string mode)
        {
            if (mode == "Shared")
            {
                var list = await _projectManagementBusiness.GetProjectSharedData();
                return Json(list);
            }
            else
            {
                var list = await _projectManagementBusiness.GetProjectData();
                return Json(list);
            }

        }

    }
}