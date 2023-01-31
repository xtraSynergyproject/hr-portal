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

namespace CMS.UI.Web.Areas.Pms.Controllers
{
    [Area("Pms")]
    public class PerformanceBoardController : ApplicationController
    {
        private readonly IPerformanceManagementBusiness _PerformanceManagementBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IUserContext _userContext;
        public PerformanceBoardController(IPerformanceManagementBusiness performancetManagementBusiness
            , IUserContext userContext, IServiceBusiness serviceBusiness, ILOVBusiness lOVBusiness)
        {
            _PerformanceManagementBusiness = performancetManagementBusiness;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _lOVBusiness = lOVBusiness;
        }

        public ActionResult Index(string mode, string permissions)
        {
            var model = new ProgramDashboardViewModel();
            model.Mode = mode;
            ViewBag.Permissions = permissions;
            ViewBag.LoggedInUserId = _userContext.UserId;
            return View(model);
        }

        public ActionResult ListView(string mode, string permissions)
        {
            var model = new ProgramDashboardViewModel();
            model.Mode = mode;
            ViewBag.Permissions = permissions;
            return View(model);
        }

        public async Task<ActionResult> ReadProjectDataOld(string mode)
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


            return Json(list);
        }

        public async Task<ActionResult> ReadProjectData(string mode)
        {
            if (mode == "Shared")
            {
                var list = await _PerformanceManagementBusiness.GetPerformanceSharedData();
                foreach (var a in list)
                {
                    var res = await _serviceBusiness.GetList(x => x.ParentServiceId == a.Id && x.TemplateCode == "PMS_PERFORMANCE_DOCUMENT_STAGE"
                    && x.IsDeleted == false);
                    foreach (var r in res)
                    {
                        a.StageList.Add(new StageViewModel
                        {
                            StageId = r.Id,
                            DocumentStageId = r.UdfNoteTableId,
                            StageName = r.ServiceSubject,
                            StartDate = r.StartDate,
                            EndDate = r.DueDate,
                            Goals = 1,
                            Competency = 2,
                            Status = r.ServiceStatusName
                        });
                    }

                    //a.StageList = new List<StageModel>
                    //{

                    //    new StageModel
                    //    {
                    //        StageId = "1",
                    //        StageName = "Stage 1",
                    //        StartDate = DateTime.Now,
                    //        EndDate = DateTime.Now,
                    //        Status = "In Progress",
                    //        Goals = 5,
                    //        Competency = 7
                    //    },
                    //     new StageModel
                    //    {
                    //        StageId = "2",
                    //        StageName = "Stage 2",
                    //        StartDate = DateTime.Now,
                    //        EndDate = DateTime.Now,
                    //        Status = "In Progress",
                    //        Goals = 5,
                    //        Competency = 7
                    //    },
                    //      new StageModel
                    //    {
                    //        StageId = "3",
                    //        StageName = "Stage 3",
                    //        StartDate = DateTime.Now,
                    //        EndDate = DateTime.Now,
                    //        Status = "In Progress",
                    //        Goals = 5,
                    //        Competency = 7
                    //    },
                    //       new StageModel
                    //    {
                    //        StageId = "4",
                    //        StageName = "Stage 4",
                    //        StartDate = DateTime.Now,
                    //        EndDate = DateTime.Now,
                    //        Status = "In Progress",
                    //        Goals = 5,
                    //        Competency = 7
                    //    },  new StageModel
                    //    {
                    //        StageId = "5",
                    //        StageName = "Stage 5",
                    //        StartDate = DateTime.Now,
                    //        EndDate = DateTime.Now,
                    //        Status = "In Progress",
                    //        Goals = 5,
                    //        Competency = 7
                    //    }
                    //};
                }
                return Json(list);
            }
            else
            {
                var list = await _PerformanceManagementBusiness.GetPerformanceData(_userContext.UserId);
                //foreach (var a in list)
                //{
                //    var res = await _serviceBusiness.GetList(x => x.ParentServiceId == a.Id && x.TemplateCode == "PMS_PERFORMANCE_DOCUMENT_STAGE"
                //    && x.IsDeleted == false);
                //    a.StageList = new List<StageModel>();
                //    foreach (var r in res)
                //    {
                //        var statusResult = await _lOVBusiness.GetSingleById(r.ServiceStatusId);


                //            a.StageList.Add(new StageModel
                //            {
                //                StageId = r.Id,
                //                StageName = r.ServiceSubject.IsNotNullAndNotEmpty() ? r.ServiceSubject : "NA",
                //                StartDate = r.StartDate,
                //                EndDate = r.DueDate,
                //                Goals = 1,
                //                Competency = 2,
                //                Status = statusResult.IsNotNull() ? statusResult.Name : ""
                //            });
                //    }
                //}
                return Json(list);
            }

        }

        public ActionResult GetGoals(string PerformanceId, string PerformanceStage, string PerformanceUser)
        {
            var model = new GoalViewModel();
            model.Id = PerformanceId;
            model.PerformanceStage = PerformanceStage;
            model.PerformanceUser = PerformanceUser;
            return View(model);
        }


        public ActionResult GetCompentency(string PerformanceId, string PerformanceStage, string PerformanceUser)
        {
            var model = new GoalViewModel();
            model.Id = PerformanceId;
            model.PerformanceStage = PerformanceStage;
            model.PerformanceUser = PerformanceUser;
            return View(model);
        }


        public async Task<ActionResult> ReadGoals([DataSourceRequest] DataSourceRequest request, string PerformanceId, string stageId, string userId)
        {

            var list = await _PerformanceManagementBusiness.GetGoalWeightageByPerformanceId(PerformanceId, stageId, userId);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }


        public async Task<ActionResult> ReadCompentancy([DataSourceRequest] DataSourceRequest request, string PerformanceId, string stageId, string userId)
        {

            var list = await _PerformanceManagementBusiness.GetCompentencyWeightageByPerformanceId(PerformanceId, stageId, userId);
            return Json(list);
            //return Json(list.ToDataSourceResult(request));
        }


        [HttpPost]

        public JsonResult updategoals(string GoalList)
        {
            string[] main = GoalList.Split(",");
            for (int i = 0; i < main.Length; i++)
            {
                string[] getgoals = main[i].Split(":");
                if (getgoals.Length > 1)
                {
                    _PerformanceManagementBusiness.updateGoalWeightaged(getgoals[0], getgoals[1]);
                }

            }

            return Json("");
        }


        [HttpPost]

        public JsonResult updateCompentancy(string CompentencyList)
        {
            string[] main = CompentencyList.Split(",");
            for (int i = 0; i < main.Length; i++)
            {
                string[] getgoals = main[i].Split(":");
                if (getgoals.Length > 1)
                {
                    _PerformanceManagementBusiness.updateCompentancyWeightaged(getgoals[0], getgoals[1]);
                }

            }

            return Json("");
        }




        public ActionResult GetGoalsUserwise(string userId)
        {
            if (userId.IsNotNullAndNotEmpty())
            {
                ViewBag.UserId = userId;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGoalCompetency(string serviceId,string code)
        {
            var service = await _serviceBusiness.GetSingle(x=>x.UdfNoteTableId==serviceId);
            if (code == "PMS_COMPENTENCY")
            {
                await _PerformanceManagementBusiness.DeleteCompetency(serviceId);               
            }
            else if(code == "PMS_GOAL")
            {
                await _PerformanceManagementBusiness.DeleteGoal(serviceId);
            }
            else 
            {
                await _PerformanceManagementBusiness.DeleteDevelopment(serviceId);
            }
            await _PerformanceManagementBusiness.DeletService(service.Id);
            return Json(new { success=true});
        }
        

        public async Task<ActionResult> ReadGoalsUsrwise([DataSourceRequest] DataSourceRequest request, string PerformanceId, string Type, string UserID)
        {
            /// if (PerformanceId.IsNotNullAndNotEmpty() && UserID.IsNotNullAndNotEmpty() && Type.IsNotNullAndNotEmpty())
            {
                if (Type == "PMS_COMPENTENCY")
                {
                    var list = await _PerformanceManagementBusiness.GetCompentencyWeightageByPerformanceId(PerformanceId, null, null);
                    return Json(list);
                   // return Json(list.ToDataSourceResult(request));
                }
                else
                {
                    var list = await _PerformanceManagementBusiness.GetGoalWeightageByPerformanceId(PerformanceId, null, null);
                    return Json(list);
                   // return Json(list.ToDataSourceResult(request));
                }
            }
            // else 
            // {
            //     var model = new GoalViewModel();
            //     return Json(model);
            // }


        }



        public JsonResult updategoalsUserwise(string GoalList, string PerformanceId, string Type, string UserID)
        {
            string[] main = GoalList.Split(",");
            for (int i = 0; i < main.Length; i++)
            {
                string[] getgoals = main[i].Split(":");
                if (getgoals.Length > 1)
                {
                    if (Type == "PMS_COMPENTENCY")
                    {
                        _PerformanceManagementBusiness.updateCompentancyWeightaged(getgoals[0], getgoals[1]);
                    }
                    else
                    {
                        _PerformanceManagementBusiness.updateGoalWeightaged(getgoals[0], getgoals[1]);
                    }
                }

            }

            return Json("");
        }


    }
}
