using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Synergy.App.Business;

namespace Synergy.App.Emirates.Areas.Controllers
{
    [Area("CaseManagement")]
    public class CaseManagementController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ITemplateBusiness _templateBusiness;

        public CaseManagementController(IUserContext userContext, IServiceBusiness serviceBusiness, IPortalBusiness portalBusiness,
            IComponentResultBusiness componentResultBusiness, ITemplateBusiness templateBusiness)
        {
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _portalBusiness = portalBusiness;
            _componentResultBusiness = componentResultBusiness;
            _templateBusiness = templateBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Diagram()
        {
            return View();
        }

        public IActionResult CustomerCaseSummary(string pageId)
        {
            ViewBag.UserId = _userContext.UserId;
            ViewBag.CategoryCode = "CSM_CASES";
            ViewBag.PortalId = _userContext.PortalId;
            return View();
        }

        public async Task<ActionResult> GetStatusWiseChartByTemplateCode(string tempCode, string requestby = null)
        {
            ServiceSearchViewModel search = new ServiceSearchViewModel();
            search.UserId = _userContext.UserId;
            var viewModel = await _serviceBusiness.GetStatusWiseChartByTemplateCode(tempCode, requestby);
            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = viewModel.Select(x => x.Type).ToList(),
                ItemValueSeries = viewModel.Select(x => x.Value).ToList(),
                ItemStatusColor = viewModel.Select(x => x.StatusColor).ToList(),
                Code = viewModel.Select(x => x.Code).FirstOrDefault(),
            };
            return Json(newlist);
        }

        public async Task<IActionResult> CSMFulfillerDashboard(string portalNames = null)
        {
            if (portalNames.IsNotNull())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                ViewBag.PortalIds = string.Join(",", portals.Select(x => x.Id).ToArray());
            }
            ViewBag.UserId = _userContext.UserId;
            ViewBag.CategoryCode = "CSM_CASES,CSM_INTERNAL";
            ViewBag.PortalNames = portalNames;
            ViewBag.PortalId = _userContext.UserId;

            return View();
        }

        public async Task<ActionResult> GetStatusChartByTemplateCode(string tempCode, string requestby = null, string deptId = null, string tempId = null, string userId = null, DateTime? st = null, DateTime? dt = null)
        {
            var UserId = userId.IsNotNullAndNotEmpty() ? userId : _userContext.UserId;
            var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, UserId, null, tempCode, null, null, null, tempId, null, null, deptId, st, dt);

            long dcount = viewModel.Where(x => x.InboxStatus == EmailInboxTypeEnum.Drafted).Count();
            long pcount = viewModel.Where(x => x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            long ccount = viewModel.Where(x => x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = new List<string> { "Draft", "Pending", "Completed" },
                ItemValueSeries = new List<long> { dcount, pcount, ccount },
                ItemStatusColor = new List<string> { "#17a2b8", "#007bff", "#13b713" },
                Code = tempCode,
            };
            return Json(newlist);
            //return Json(viewModel);
        }

        public async Task<ActionResult> GetHeatMapChart(string tempCode, string requestby = null, string deptId = null, string tempId = null, string userId = null, DateTime? st = null, DateTime? dt = null)
        {
            var UserId = userId.IsNotNullAndNotEmpty() ? userId : _userContext.UserId;
            var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, UserId, null, tempCode, null, null, null, tempId, null, null, deptId, st, dt);

            var tempName = viewModel.Select(x => x.TemplateName).FirstOrDefault();

            //var dates = new List<DateTime?>();

            //foreach (var item in viewModel.OrderBy(x=>x.StartDate.Value.Date).GroupBy(x => x.StartDate.Value.Date))
            //{
            //    dates.Add(item.Key);
            //}

            var list = new List<HeatMapDataViewModel>();

            var lcount = viewModel.Where(x => x.PendingDays < 7).Count();
            var mcount = viewModel.Where(x => x.PendingDays >= 7 && x.PendingDays < 40).Count();
            var hcount = viewModel.Where(x => x.PendingDays >= 40).Count();

            list.Add(new HeatMapDataViewModel()
            {
                x = "<1 week",
                y = lcount,
            });
            list.Add(new HeatMapDataViewModel()
            {
                x = "2-3 weeks",
                y = mcount
            });
            list.Add(new HeatMapDataViewModel()
            {
                x = ">3 weeks",
                y = hcount
            });

            var heatmapdata = new List<HeatMapViewModel>()
            {
                new HeatMapViewModel()
                {
                    name = tempName,
                    data = list
                }
            };

            return Json(new { seriesList = heatmapdata });
        }

        public async Task<IActionResult> MonitoringDashboard(string templateCodes = null, string catCodes = null, string groupCodes = null, string portalNames = null, string pageId = null, string pageName = null, bool forallusers = false, bool allowCreation = false)
        {
            var userId = forallusers ? null : _userContext.UserId;
            ViewBag.UserId = userId;
            ViewBag.PortalId = _userContext.PortalId;

            var templates = await _templateBusiness.GetTemplateServiceList(templateCodes, catCodes, null, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, null, groupCodes);
            templates = templates.OrderBy(x => x.DisplayName).ToList();

            string[] codes = templates.Select(x => x.Code).ToArray();
            ViewBag.TemplateCodes = string.Join(",", codes);
            var templateId = templates.Select(x => x.Id).FirstOrDefault();
            ViewBag.TemplateId = templateId;

            var emailList = await _componentResultBusiness.GetNtsEmailList(null, null, userId, portalNames, null, catCodes, null, null, null, null, null, null, null, null);
            emailList = emailList.DistinctBy(x => x.TargetId).ToList();
            ViewBag.SerPenCount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            ViewBag.SerComCount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

            ViewBag.CommPenCount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            ViewBag.CommComCount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

            ViewBag.PageName = pageName;
            ViewBag.PortalNames = portalNames;
            ViewBag.AllowCreation = allowCreation;
            ViewBag.CategoryCodes = catCodes;
            ViewBag.TemplatesCodes = templateCodes;

            return View();
        }

        public async Task<IActionResult> ReadMonitoringDashboarGridData(string templateCodes = null, string portalNames = null, string userId = null, DateTime? st = null, DateTime? dt = null, string pageType = null)
        {
            var emailList = await _componentResultBusiness.GetNtsEmailList(null, null, userId, portalNames, templateCodes, null, null, null, null, null, null, null, st, dt);
            emailList = emailList.DistinctBy(x => x.TargetId).ToList();
            var templates = emailList.GroupBy(x => x.TemplateId).Select(x => x.Key).ToArray();

            var emailgridlist = new List<NtsEmailViewModel>();

            if (pageType == "Monitoring")
            {
                foreach (var temp in templates)
                {
                    var pcount = emailList.Where(x => x.TemplateId == temp && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                    var ccount = emailList.Where(x => x.TemplateId == temp && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                    var data = new NtsEmailViewModel()
                    {
                        TemplateId = temp,
                        TemplateName = emailList.Where(x => x.TemplateId == temp).Select(x => x.TemplateName).FirstOrDefault(),
                        PendingCount = pcount,
                        CompletedCount = ccount
                    };
                    emailgridlist.Add(data);
                }
            }
            else if (pageType == "Stage")
            {
                foreach (var temp in templates)
                {
                    var pcount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                    var ccount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                    var data = new NtsEmailViewModel()
                    {
                        TemplateId = temp,
                        TemplateName = emailList.Where(x => x.TemplateId == temp).Select(x => x.TemplateName).FirstOrDefault(),
                        PendingCount = pcount,
                        CompletedCount = ccount
                    };
                    emailgridlist.Add(data);
                }
            }
            else if (pageType == "Communication")
            {
                foreach (var temp in templates)
                {
                    var pcount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                    var ccount = emailList.Where(x => x.TemplateId == temp && (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                    var data = new NtsEmailViewModel()
                    {
                        TemplateId = temp,
                        TemplateName = emailList.Where(x => x.TemplateId == temp).Select(x => x.TemplateName).FirstOrDefault(),
                        PendingCount = pcount,
                        CompletedCount = ccount
                    };
                    emailgridlist.Add(data);
                }
            }

            emailgridlist = emailgridlist.OrderBy(x => x.TemplateName).ToList();

            return Json(emailgridlist);

        }

        [HttpGet]
        public async Task<IActionResult> GetNtsEmailStatusCountByTemplate(string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, EmailTypeEnum? emailType = null, EmailInboxTypeEnum? inboxStatus = null, string catId = null, string tempId = null, string deptId = null, DateTime? st = null, DateTime? dt = null, string pageType = null)
        {
            var emailList = await _componentResultBusiness.GetNtsEmailList(null, null, userId, portalNames, templateCodes, catCodes, groupCodes, catId, tempId, emailType, inboxStatus, deptId, st, dt);
            emailList = emailList.DistinctBy(x => x.TargetId).ToList();
            if (pageType == "Monitoring")
            {
                var spcount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                var sccount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                var cpcount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                var cccount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                return Json(new { success = true, serpending = spcount, sercompleted = sccount, compending = cpcount, comcompleted = cccount });
            }
            else if (pageType == "Stage")
            {
                var spcount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                var sccount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                return Json(new { success = true, serpending = spcount, sercompleted = sccount });
            }
            else
            {
                var spcount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
                var sccount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

                return Json(new { success = true, serpending = spcount, sercompleted = sccount });
            }

        }

        public async Task<IActionResult> StageDashboard(string templateCodes = null, string catCodes = null, string groupCodes = null, string portalNames = null, string pageId = null, string pageName = null, bool forallusers = false, bool allowCreation = false)
        {
            var userId = forallusers ? null : _userContext.UserId;
            ViewBag.UserId = userId;
            ViewBag.PortalId = _userContext.PortalId;

            var templates = await _templateBusiness.GetTemplateServiceList(templateCodes, catCodes, null, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, null, groupCodes);
            templates = templates.OrderBy(x => x.DisplayName).ToList();

            string[] codes = templates.Select(x => x.Code).ToArray();
            ViewBag.TemplateCodes = string.Join(",", codes);
            var templateId = templates.Select(x => x.Id).FirstOrDefault();
            ViewBag.TemplateId = templateId;

            var emailList = await _componentResultBusiness.GetNtsEmailList(null, null, userId, portalNames, null, catCodes, null, null, null, null, null, null, null, null);
            emailList = emailList.DistinctBy(x => x.TargetId).ToList();
            ////ViewBag.SerPenCount = emailList.Where(x => x.TargetType == NtsEmailTargetTypeEnum.Service && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            ////ViewBag.SerComCount = emailList.Where(x => x.TargetType == NtsEmailTargetTypeEnum.Service && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

            var pcount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            var ccount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();
            ViewBag.PenCount = pcount;
            ViewBag.ComCount = ccount;
            ViewBag.TotalCount = pcount + ccount;

            ViewBag.IconFileId = templates.Select(x => x.IconFileId).FirstOrDefault();
            ViewBag.PageName = pageName;
            ViewBag.PortalNames = portalNames;
            ViewBag.AllowCreation = allowCreation;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReadStageEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, EmailTypeEnum? emailType = null, EmailInboxTypeEnum? inboxStatus = null, string catId = null, string tempId = null, string deptId = null, DateTime? st = null, DateTime? dt = null)
        {
            var dlist = await _componentResultBusiness.GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, catId, tempId, emailType, inboxStatus, deptId, st, dt);
            dlist = dlist.DistinctBy(x => x.TargetId).ToList();
            dlist = dlist.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask)).ToList();
            return Json(dlist);
        }
        [HttpGet]
        public async Task<ActionResult> GetStageChartByWorkflowStatus(string tempCode, string deptId = null, string tempId = null, string userId = null, DateTime? st = null, DateTime? dt = null)
        {
            var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, userId, null, tempCode, null, null, null, tempId, null, null, deptId, st, dt);
            viewModel = viewModel.DistinctBy(x => x.TargetId).ToList();
            var dlist = viewModel.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask)).ToList();
            var wfStatus = dlist.GroupBy(x => x.WorkflowStatus).Select(x => x.Key).ToArray();

            var list = new List<NtsEmailViewModel>();

            foreach (var item in wfStatus)
            {
                if (item == null)
                {
                    continue;
                }
                var color = "";
                var count = dlist.Where(x => x.WorkflowStatus == item).Count();
                if (item.Contains("Pending"))
                {
                    color = "#007bff";
                }
                else if (item.Contains("Completed"))
                {
                    color = "#13b713";
                }
                else if (item.Contains("Started"))
                {
                    color = "#17a2b8";
                }
                else if (item.Contains("Rejected"))
                {
                    color = "#f10b0b";
                }
                list.Add(new NtsEmailViewModel() { WorkflowStatus = item, Count = count, StatusColor = color });
            }
            list = list.OrderBy(x => x.WorkflowStatus).ToList();

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = list.Select(x => x.WorkflowStatus).ToList(),
                ItemValueSeries = list.Select(x => x.Count).ToList(),
                ItemStatusColor = list.Select(x => x.StatusColor).ToList(),
            };
            return Json(newlist);
        }

        public async Task<ActionResult> GetStageHeatMapChart(string tempCode, string deptId = null, string tempId = null, string userId = null, DateTime? st = null, DateTime? dt = null)
        {
            var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, userId, null, tempCode, null, null, null, tempId, null, null, deptId, st, dt);
            viewModel = viewModel.DistinctBy(x => x.TargetId).ToList();
            var dlist = viewModel.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.Service || x.TargetType == NtsEmailTargetTypeEnum.StepTask)).ToList();

            string[] tempNames = dlist.GroupBy(x => x.TemplateName).Select(x => x.Key).ToArray();

            //var dates = new List<DateTime?>();

            //foreach (var item in viewModel.OrderBy(x=>x.StartDate.Value.Date).GroupBy(x => x.StartDate.Value.Date))
            //{
            //    dates.Add(item.Key);
            //}

            var heatmapdata = new List<HeatMapViewModel>();

            foreach (var temp in tempNames)
            {
                var lcount = dlist.Where(x => x.TemplateName == temp && x.PendingDays < 7).Count();
                var mcount = dlist.Where(x => x.TemplateName == temp && (x.PendingDays >= 7 && x.PendingDays < 14)).Count();
                var hcount = dlist.Where(x => x.TemplateName == temp && x.PendingDays >= 14).Count();

                var list = new List<HeatMapDataViewModel>();

                list.Add(new HeatMapDataViewModel()
                {
                    x = "<1 week",
                    y = lcount,
                });
                list.Add(new HeatMapDataViewModel()
                {
                    x = "2-3 weeks",
                    y = mcount
                });
                list.Add(new HeatMapDataViewModel()
                {
                    x = ">3 weeks",
                    y = hcount
                });

                var mapdata = new HeatMapViewModel()
                {
                    name = temp,
                    data = list
                };

                heatmapdata.Add(mapdata);
            }

            return Json(new { seriesList = heatmapdata });
        }

        public async Task<IActionResult> CommunicationDashboard(string templateCodes = null, string catCodes = null, string groupCodes = null, string portalNames = null, string pageId = null, string pageName = null, bool forallusers = false, bool allowCreation = false)
        {
            var userId = forallusers ? null : _userContext.UserId;
            ViewBag.UserId = userId;
            ViewBag.PortalId = _userContext.PortalId;

            var templates = await _templateBusiness.GetTemplateServiceList(templateCodes, catCodes, null, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, null, groupCodes);
            templates = templates.OrderBy(x => x.DisplayName).ToList();

            string[] codes = templates.Select(x => x.Code).ToArray();
            ViewBag.TemplateCodes = string.Join(",", codes);
            var templateId = templates.Select(x => x.Id).FirstOrDefault();
            ViewBag.TemplateId = templateId;

            var emailList = await _componentResultBusiness.GetNtsEmailList(null, null, userId, portalNames, null, catCodes, null, null, null, null, null, null, null, null);
            emailList = emailList.DistinctBy(x => x.TargetId).ToList();
            ////ViewBag.SerPenCount = emailList.Where(x => x.TargetType == NtsEmailTargetTypeEnum.Service && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            ////ViewBag.SerComCount = emailList.Where(x => x.TargetType == NtsEmailTargetTypeEnum.Service && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();

            var pcount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            var ccount = emailList.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask) && x.InboxStatus == EmailInboxTypeEnum.Completed).Count();
            ViewBag.PenCount = pcount;
            ViewBag.ComCount = ccount;
            ViewBag.TotalCount = pcount + ccount;

            ViewBag.IconFileId = templates.Select(x => x.IconFileId).FirstOrDefault();
            ViewBag.PageName = pageName;
            ViewBag.PortalNames = portalNames;
            ViewBag.AllowCreation = allowCreation;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReadCommEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, EmailTypeEnum? emailType = null, EmailInboxTypeEnum? inboxStatus = null, string catId = null, string tempId = null, string deptId = null, DateTime? st = null, DateTime? dt = null)
        {
            var dlist = await _componentResultBusiness.GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, catId, tempId, emailType, inboxStatus, deptId, st, dt);
            dlist = dlist.DistinctBy(x => x.TargetId).ToList();
            dlist = dlist.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask)).ToList();
            return Json(dlist);
        }
        [HttpGet]
        public async Task<ActionResult> GetCommChartByWorkflowStatus(string tempCode, string deptId = null, string tempId = null, string userId = null, DateTime? st = null, DateTime? dt = null)
        {
            var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, userId, null, tempCode, null, null, null, tempId, null, null, deptId, st, dt);
            viewModel = viewModel.DistinctBy(x => x.TargetId).ToList();
            var dlist = viewModel.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask)).ToList();
            var wfStatus = dlist.GroupBy(x => x.WorkflowStatus).Select(x => x.Key).ToArray();

            var list = new List<NtsEmailViewModel>();

            foreach (var item in wfStatus)
            {
                var color = "";
                var count = dlist.Where(x => x.WorkflowStatus == item).Count();
                if (item.Contains("Pending"))
                {
                    color = "#007bff";
                }
                else if (item.Contains("Completed"))
                {
                    color = "#13b713";
                }
                else if (item.Contains("Started"))
                {
                    color = "#17a2b8";
                }
                else if (item.Contains("Rejected"))
                {
                    color = "#f10b0b";
                }
                list.Add(new NtsEmailViewModel() { WorkflowStatus = item, Count = count, StatusColor = color });
            }
            list = list.OrderBy(x => x.WorkflowStatus).ToList();

            var newlist = new ProjectDashboardChartViewModel
            {
                ItemValueLabel = list.Select(x => x.WorkflowStatus).ToList(),
                ItemValueSeries = list.Select(x => x.Count).ToList(),
                ItemStatusColor = list.Select(x => x.StatusColor).ToList(),
            };
            return Json(newlist);
        }

        public async Task<ActionResult> GetCommHeatMapChart(string tempCode, string deptId = null, string tempId = null, string userId = null, DateTime? st = null, DateTime? dt = null)
        {
            var viewModel = await _componentResultBusiness.GetNtsEmailList(null, null, userId, null, tempCode, null, null, null, tempId, null, null, deptId, st, dt);
            viewModel = viewModel.DistinctBy(x => x.TargetId).ToList();
            var dlist = viewModel.Where(x => (x.TargetType == NtsEmailTargetTypeEnum.SubTask || x.TargetType == NtsEmailTargetTypeEnum.AcceptanceTask)).ToList();

            string[] tempNames = dlist.GroupBy(x => x.TemplateName).Select(x => x.Key).ToArray();

            //var dates = new List<DateTime?>();

            //foreach (var item in viewModel.OrderBy(x=>x.StartDate.Value.Date).GroupBy(x => x.StartDate.Value.Date))
            //{
            //    dates.Add(item.Key);
            //}

            var heatmapdata = new List<HeatMapViewModel>();

            foreach (var temp in tempNames)
            {
                var lcount = dlist.Where(x => x.TemplateName == temp && x.PendingDays < 7).Count();
                var mcount = dlist.Where(x => x.TemplateName == temp && (x.PendingDays >= 7 && x.PendingDays < 14)).Count();
                var hcount = dlist.Where(x => x.TemplateName == temp && x.PendingDays >= 14).Count();

                var list = new List<HeatMapDataViewModel>();

                list.Add(new HeatMapDataViewModel()
                {
                    x = "<1 week",
                    y = lcount,
                });
                list.Add(new HeatMapDataViewModel()
                {
                    x = "2-3 weeks",
                    y = mcount
                });
                list.Add(new HeatMapDataViewModel()
                {
                    x = ">3 weeks",
                    y = hcount
                });

                var mapdata = new HeatMapViewModel()
                {
                    name = temp,
                    data = list
                };

                heatmapdata.Add(mapdata);
            }

            return Json(new { seriesList = heatmapdata });
        }

    }
}
