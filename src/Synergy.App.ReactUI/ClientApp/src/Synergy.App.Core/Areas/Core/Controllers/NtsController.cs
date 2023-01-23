using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.Web;
using Hangfire;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web
{

    public class NtsController : ApplicationController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IPushNotificationBusiness _notifyBusiness;
        private readonly INtsBusiness _ntsBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IComponentResultBusiness _componentBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly ILOVBusiness _lOVBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsController(IUserBusiness userBusiness,
            AuthSignInManager<ApplicationIdentityUser> customUserManager, IUserContext userContext
            , IPortalBusiness portalBusiness, IPushNotificationBusiness notifyBusiness,
            INtsBusiness ntsBusiness, ITaskBusiness taskBusiness, ITemplateBusiness templateBusiness
            , IComponentResultBusiness componentBusiness, ITemplateCategoryBusiness templateCategoryBusiness, ILOVBusiness lOVBusiness)
        {
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
            _notifyBusiness = notifyBusiness;
            _ntsBusiness = ntsBusiness;
            _taskBusiness = taskBusiness;
            _templateBusiness = templateBusiness;
            _componentBusiness = componentBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _lOVBusiness = lOVBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MessageBox()
        {
            return View();
        }
        public IActionResult NtsList()
        {
            var model = new NotificationViewModel();
            return View(model);
        }
        [HttpGet]
        public IActionResult ReadNtsListData()
        {
            var a = new List<NotificationViewModel>();
            a.Add(new NotificationViewModel
            {
                PhotoId = "1",
                From = "Sano",
                Subject = "Mouse Not Working",
                Body = "Describe anout yout mobile future",
                JobTitle = "Manager",
                CreatedDate = DateTime.Now

            });
            a.Add(new NotificationViewModel
            {
                PhotoId = "1",
                From = "tina",
                Subject = "Mouse Not Working",
                Body = "Describe anout yout mobile future",
                JobTitle = "IT",
                CreatedDate = DateTime.Now

            });
            a.Add(new NotificationViewModel
            {
                PhotoId = "1",
                From = "Sano1",
                JobTitle = "Tester",
                Subject = "laptop Not Working",
                Body = "Describe1 anout yout mobile future",
                CreatedDate = DateTime.Now

            });
            a.Add(new NotificationViewModel
            {
                PhotoId = "1",
                From = "mena",
                Subject = "keypad Not Working",
                Body = "Describe2 anout yout mobile future",
                CreatedDate = DateTime.Now

            });
            a.Add(new NotificationViewModel
            {
                PhotoId = "1",
                From = "miho",
                Subject = "phone Not Working",
                Body = "Describe3 anout yout mobile future",
                CreatedDate = DateTime.Now

            });


            return Json(a);
        }

        public async Task<IActionResult> GetNtsMenuList(string id, string userId)
        {
            var result = await _ntsBusiness.GetNtsMenuList(id, _userContext.UserId, _userContext.Email);
            var model = result.ToList();
            return Json(model);
        }
        public async Task<IActionResult> UpdateRating(NtsTypeEnum ntsType, string ntsId, string userId, int rating, string ratingComment)
        {
            await _ntsBusiness.UpdateRating(ntsType, ntsId, userId, rating, ratingComment);
            return Json(new { success = true });
        }
        public async Task<IActionResult> RemoveRating(NtsTypeEnum ntsType, string ntsId, string userId)
        {
            await _ntsBusiness.RemoveRating(ntsType, ntsId, userId);
            return Json(new { success = true });
        }
        public async Task<IActionResult> ReadNtsList([DataSourceRequest] DataSourceRequest request)
        {
            //var result = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            //var j = Json(result.Where(x => x.TaskStatusCode == "COMPLETED").OrderByDescending(x => x.StartDate).ToDataSourceResult(request));
            var a = new List<TaskViewModel>();
            a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 1",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 2",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 3",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 4",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 5",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 6",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            });
            var j = Json(a);
            // var j = Json(a.ToDataSourceResult(request));

            return Json(j);
        }

        [Route("Core/Nts/NtsEmailPage")]
        public async Task<IActionResult> NtsEmailPage(string returnPageName, string serTempCodes, string statusCodes, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string userId = null,string tempId=null, NtsEmailTargetTypeEnum? targetType1=null, NtsEmailTargetTypeEnum? targetType2=null,string wfStatus=null, SLATypeEnum? slaType=null, DateTime? st = null, DateTime? dt = null)
        {
            ViewBag.PortalId = _userContext.PortalId;
            ViewBag.PageName = returnPageName;
            ViewBag.ServiceTemplateCodes = serTempCodes;
            ViewBag.StatusCodes = statusCodes;
            ViewBag.UserId = userId;
            if (ViewBag.UserId == null)
            {
                ViewBag.UserId = _userContext.UserId;
            }
            ViewBag.TemplateCodes = templateCodes;
            ViewBag.CategoryCodes = catCodes;
            ViewBag.PortalNames = portalNames;
            ViewBag.GroupCodes = groupCodes;
            ViewBag.TargetType1 = targetType1;
            ViewBag.TargetType2 = targetType2;
            ViewBag.TemplateId = tempId;
            ViewBag.WorkflowStatus = wfStatus;
            ViewBag.SLAType = slaType;
            ViewBag.FromDate = st;
            ViewBag.ToDate = dt;


            if (serTempCodes.IsNotNullAndNotEmpty())
            {
                var templates = await _templateBusiness.GetSingle(x => x.Code == serTempCodes && x.TemplateType == TemplateTypeEnum.Service);
                ViewBag.FiltersByTemplates = "Templates in " + templates.DisplayName;
            }
            else if (templateCodes.IsNotNullAndNotEmpty())
            {
                var templates = await _templateBusiness.GetTemplateServiceList(templateCodes, catCodes, null, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, ServiceTypeEnum.StandardService, groupCodes);
                string[] tnames = templates.Select(x => x.DisplayName).ToArray();                                              

                //var tcodes = templateCodes.Split(",").ToArray();
                //var templates = await _templateBusiness.GetList(x => tcodes.Contains(x.Code) && x.TemplateType == TemplateTypeEnum.Service);
                //string[] tnames = templates.Select(x => x.DisplayName).ToArray();

                ViewBag.FiltersByCategories = "Templates in " + String.Join(",", tnames);
            }
            if (catCodes.IsNotNullAndNotEmpty())
            {
                var codes = catCodes.Split(",").ToArray();
                var categories = await _templateCategoryBusiness.GetList(x => codes.Contains(x.Code) && x.TemplateType == TemplateTypeEnum.Service);
                string[] catnames = categories.Select(x => x.Name).ToArray();
                ViewBag.FiltersByCategories = "Categories in " + String.Join(",", catnames);
            }
            if (statusCodes.IsNotNullAndNotEmpty())
            {
                var statusname = "";
                if (statusCodes.ToLower().Contains("draft"))
                {
                    statusname = "Draft";
                }
                else if (statusCodes.ToLower().Contains("inprogress"))
                {
                    statusname = "Pending";
                }
                else if (statusCodes.ToLower().Contains("complete"))
                {
                    statusname = "Completed";
                }
                ViewBag.FiltersByStatus = "Status in " + statusname;
            }

            return View();
        }
        public async Task<IActionResult> ReadNtsEmailTree(EmailTypeEnum emailType, string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null)
        {
            var dlist = await _componentBusiness.GetNtsEmailTree(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes);
            return Json(dlist.Where(x => x.EmailType == emailType).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> ReadNtsEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null,EmailTypeEnum? emailType =null,EmailInboxTypeEnum? inboxStatus=null, string catId =null,string tempId=null,string deptId=null,DateTime? st=null,DateTime? dt=null, NtsEmailTargetTypeEnum? targetType1 = null, NtsEmailTargetTypeEnum? targetType2 = null,string wfStatus=null,SLATypeEnum? slaType=null)
        {
            var dlist = await _componentBusiness.GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, catId, tempId, emailType, inboxStatus,deptId,st,dt, targetType1, targetType2, wfStatus, slaType);
            //dlist = dlist.Where(x => !(x.TargetType == NtsEmailTargetTypeEnum.Service && x.ServicePlusId != null)).ToList();
            return Json(dlist.DistinctBy(x => x.TargetId));
        }
        public async Task<IActionResult> ReadAttachedReplies()
        {
            var result = await _ntsBusiness.GetAttachedReplies(_userContext.UserId, null);
            return Json(result);
        }

        [Route("Core/Nts/NtsEmailSummary")]
        public async Task<IActionResult> NtsEmailSummary(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllTasksForAdmin = false, bool showAllTask = false, string portalNames = null, string pageId = null, string pageName = null)
        {
            var portalId = _userContext.PortalId;
            if (portalNames.IsNotNull())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                portalId = string.Join(",", portals.Select(x => x.Id).ToArray());
            }
            var model = new TemplateViewModel()
            {
                TemplateCodes = templateCodes,
                CategoryCodes = catCodes,
                GroupCodes = groupCodes,
                ShowAllServicesForAdmin = showAllTasksForAdmin,
                UserId = _userContext.UserId,
                PortalId = portalId
            };
            if (showAllTask)
            {
                model.UserId = null;
            }
            else if (showAllTasksForAdmin && _userContext.IsSystemAdmin)
            {
                model.UserId = null;
            }
            ViewBag.PageName = pageName;
            ViewBag.PortalNames = portalNames;
            return View(model);
        }
        public async Task<IActionResult> ReadEmailSummaryList(string templateCodes = null, string catCodes = null, string groupCodes = null, bool showAllTasksForAdmin = false)
        {
            var dt = await _taskBusiness.GetTemplatesListWithTaskCount(templateCodes, catCodes, groupCodes, showAllTasksForAdmin);
            return Json(dt);
        }
        [Route("Core/Nts/NtsServiceEmailSummary")]
        public async Task<IActionResult> NtsServiceEmailSummary(string templateCodes = null, string catCodes = null, string groupCodes = null, string portalNames = null, string pageId = null, string pageName = null, bool forallusers= false)
        {
            var userId = forallusers ? null : _userContext.UserId;
            ViewBag.UserId = userId;
            //ViewBag.CategoryCode = "CSM_CASES";
            ViewBag.PortalId = _userContext.PortalId;

            var portalId = _userContext.PortalId;
            if (portalNames.IsNotNull())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                portalId = string.Join(",", portals.Select(x => x.Id).ToArray());
            }
            ViewBag.PortalIds = portalId;

            var templates = await _templateBusiness.GetTemplateServiceList(templateCodes, catCodes, null, null, null, TemplateCategoryTypeEnum.Standard, false, portalNames, null, groupCodes);
            templates = templates.OrderBy(x => x.DisplayName).ToList();

            string[] codes = templates.Select(x => x.Code).ToArray();
            ViewBag.TemplateCodes = string.Join(",", codes);

            ViewBag.Templates = codes;

            var temlist = new List<TemplateViewModel>();

            foreach (var temp in templates)
            {
                var tempmodel = new TemplateViewModel()
                {
                    Name = temp.DisplayName,
                    Code = temp.Code
                };
                temlist.Add(tempmodel);
            }

            var model = new TemplateViewModel()
            {
                TemplateCodes = ViewBag.TemplateCodes,
                Templates = ViewBag.Templates,
                TemplatesList = temlist,
            };

            ViewBag.PageName = pageName;
            ViewBag.PortalNames = portalNames;

            var emailList = await _componentBusiness.GetNtsEmailList(null, null, userId, portalNames, templateCodes, catCodes, groupCodes);

            ViewBag.TotalCount = emailList.Count();
            ViewBag.IconFileId = templates.Select(x => x.IconFileId).FirstOrDefault();

            return View(model);
        }

        [Route("Core/Nts/GetNtsEmailListCount")]
        [HttpGet]
        public async Task<IActionResult> GetNtsEmailListCount(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, EmailTypeEnum? emailType = null, EmailInboxTypeEnum? inboxStatus = null, string catId = null, string tempId = null, string deptId = null, DateTime? st = null, DateTime? dt = null)
        {
            var dlist = await _componentBusiness.GetNtsEmailList(serTempCodes, statusCodes, userId, portalNames, templateCodes, catCodes, groupCodes, catId, tempId, emailType, inboxStatus,deptId,st,dt);
            var pcount = dlist.Where(x => x.InboxStatus == EmailInboxTypeEnum.Pending).Count();
            var ccount = dlist.Where(x => x.InboxStatus == EmailInboxTypeEnum.Completed).Count();
            
            return Json(new { success=true, pending = pcount,completed=ccount});
        }
    }
}
