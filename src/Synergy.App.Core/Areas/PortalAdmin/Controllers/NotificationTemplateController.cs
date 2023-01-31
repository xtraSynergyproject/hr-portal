using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class NotificationTemplateController : ApplicationController
    {
        private readonly INotificationTemplateBusiness _notificationTemplateBusiness;
        IMapper _autoMapper;
        private static IUserContext _userContext;
        private static INotificationBusiness _notificationBusiness;
        public NotificationTemplateController(INotificationTemplateBusiness notificationTemplateBusiness
            , IMapper autoMapper, IUserContext userContext, INotificationBusiness notificationBusiness)
        {
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _autoMapper = autoMapper;
            _userContext = userContext;
            _notificationBusiness = notificationBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageNotification(string id, string templateId, NtsTypeEnum ntsType)
        {
            var portal = await _notificationTemplateBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            var model = new NotificationTemplateViewModel();

            var noteTemplate = await _notificationTemplateBusiness.GetSingleById(id);
            if (noteTemplate == null)
            {
                model.TemplateId = templateId;
                model.NtsType = ntsType;
                model.IsTemplate = true;
                model.DataAction = DataActionEnum.Create;
            }
            else
            {
                model = noteTemplate;
                //model.IsTemplate = true;
                model.DataAction = DataActionEnum.Edit;
            }
            return View("ManageNotificationTemplate", model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageNotification(NotificationTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _notificationTemplateBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _notificationTemplateBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageNotificationTemplate", model);
        }
        public async Task<ActionResult> ReadNotificationTemplateData([DataSourceRequest] DataSourceRequest request)
        {
            var list = await _notificationTemplateBusiness.GetList(x=>x.IsTemplate == true && x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            var json = Json(list);
            //var json = Json(list.ToDataSourceResult(request));
            return json;
        }
        public async Task<ActionResult> ReadNotificationTemplateDataList()
        {
            var list = await _notificationTemplateBusiness.GetList(x => x.IsTemplate == true);
            foreach (var i in list)
            {
                i.ActionStatusCode = String.Join(",", i.ActionStatusCodes);
            }
            var json = Json(list);
            return json;
        }


        public async Task<IActionResult> DeleteNoteNotification(string id)
        {
            await _notificationTemplateBusiness.Delete(id);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationParentIdNameList()
        {
            var dataList = await _notificationTemplateBusiness.GetList(x => x.IsTemplate == true);
            return Json(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanyIdNameList()
        {
            var dataList = await _notificationTemplateBusiness.GetListGlobal<CompanyViewModel,Company>();
            return Json(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList());
        }
        public async Task<IActionResult> NotificationIndex(DateTime? date, LayoutModeEnum? lo=null, ReferenceTypeEnum? refType=null, string refTypeId=null)
        {
            NotificationViewModel model = new NotificationViewModel();
            if (date.IsNotNull())
            {
                model.WeekDate = date.Value;
            }
            else 
            {
                model.WeekDate = DateTime.Now;               
            }
            model.FirstDayName = model.WeekDate.Value.AddDays(-3).Date.ToDefaultDateFormat();
            model.SecondDayName = model.WeekDate.Value.AddDays(-2).Date.ToDefaultDateFormat();
            model.ThirdDayName = model.WeekDate.Value.AddDays(-1).Date.ToDefaultDateFormat();
            model.CurrentDayName = model.WeekDate.Value.Date.ToDefaultDateFormat();
            model.FifthDayName = model.WeekDate.Value.AddDays(1).Date.ToDefaultDateFormat();
            model.SixthDayName = model.WeekDate.Value.AddDays(2).Date.ToDefaultDateFormat();
            model.SeventhDayName = model.WeekDate.Value.AddDays(3).Date.ToDefaultDateFormat();
            model.ReferenceTypeId = refTypeId.IsNotNullAndNotEmpty() ? refTypeId : null;            
            if (refType.IsNotNull())
            {
                ViewBag.Type = refType;
                //var nlist = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20);
               // model.Notifications = nlist.Where(x => x.CreatedDate.Date == date.Value.Date && x.ReferenceType == refType).ToList();                 
            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                ViewBag.lo = "Popup";
            }
            return View(model);
        }
        public async Task<ActionResult> ReadNotificationData([DataSourceRequest] DataSourceRequest request,DateTime date, ReferenceTypeEnum? refType=null, bool read = false, bool archive = false, string refTypeId=null, bool completedStatus = false)
        {
            var list = await _notificationBusiness.GetNotificationList(date, refType,read,archive, refTypeId);
            foreach (var lst in list)
            {
                string pattern = @$"<a id='ext_url'.+?</a>";
                var r = new Regex(pattern, RegexOptions.Singleline);
                lst.Body = lst.Body.IsNotNullAndNotEmpty() ? r.Replace(lst.Body, "") : "";
            }
            if (!completedStatus)
            {
                list = list.Where(x => x.ActionStatus != NotificationActionStatusEnum.Completed).ToList();
            }
            var json = Json(list);
            //var json = Json(list.ToDataSourceResult(request));
            return json;
        }

        public async Task<ActionResult> ReadNotificationlist(DateTime? date, ReferenceTypeEnum? refType = null, bool read = false, bool archive = false, bool starred = false, string refTypeId=null, bool completedStatus = false)
        {
            var list = new List<NotificationViewModel>();
            if (refType.IsNotNull())
            {
                ViewBag.Type = refType;
                var nlist = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20);
                list = nlist.Where(x => x.CreatedDate.Date == date.Value.Date && x.ReferenceType == refType).ToList();
            }
            else
            {
               var nlist = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20,refTypeId);
                list = nlist.ToList();
            }

            if (read)
            {
                list = list.Where(x => x.ReadStatus != ReadStatusEnum.Read).ToList();
            }            
            if (!archive)
            {
                list = list.Where(x => x.IsArchived ==false).ToList();
            }
            else
            {
                list = list.Where(x => x.IsArchived == true).ToList();
            }
            if (starred)
            {
                list = list.Where(x => x.IsStarred == true).ToList();
            }
            if (!completedStatus)
            {
                list = list.Where(x => x.ActionStatus != NotificationActionStatusEnum.Completed).ToList();
            }
            foreach (var lst in list)
            {
                string pattern = @$"<a id='ext_url'.+?</a>";
                var r = new Regex(pattern, RegexOptions.Singleline);
                lst.Body = lst.Body.IsNotNullAndNotEmpty() ? r.Replace(lst.Body, "") : "";
            }
            var json = Json(list);
            return json;
        }


        [HttpPost]
        public async Task<IActionResult> ArchiveNotification(string id)
        {
            await _notificationBusiness.ArchiveNotification(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UnArchiveNotification(string id)
        {
            await _notificationBusiness.UnArchiveNotification(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> StarNotification(string id)
        {
            await _notificationBusiness.StarNotification(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UnStarNotification(string id)
        {
            await _notificationBusiness.UnStarNotification(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> MarkNotificationAsNotRead(string id)
        {
            await _notificationBusiness.MarkNotificationAsNotRead(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllNotificationAsRead(string userId, string portalId)
        {
            await _notificationBusiness.MarkAllNotificationAsRead(userId, portalId);
            return Json(new { success = true });
        }

        public async Task<ActionResult> ReadNotifications([DataSourceRequest] DataSourceRequest request,string referenceTypeId,string id)
        {
            if (referenceTypeId.IsNotNullAndNotEmpty() && referenceTypeId!="null")
            {
                var list = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20, referenceTypeId);
                var json = Json(list);
                //var json = Json(list.ToDataSourceResult(request));
                return json;
            }
            else 
            {
                var list = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20, null,id);
                var json = Json(list);
               // var json = Json(list.ToDataSourceResult(request));
                return json;
            }          
        }

        public async Task<IActionResult> NewNotificationIndex(DateTime? date, LayoutModeEnum? lo = null, ReferenceTypeEnum? refType = null, string refTypeId = null)
        {
            NotificationViewModel model = new NotificationViewModel();
            if (date.IsNotNull())
            {
                model.WeekDate = date.Value;
            }
            else
            {
                model.WeekDate = DateTime.Now;
            }
            model.FirstDayName = model.WeekDate.Value.AddDays(-3).Date.ToDefaultDateFormat();
            model.SecondDayName = model.WeekDate.Value.AddDays(-2).Date.ToDefaultDateFormat();
            model.ThirdDayName = model.WeekDate.Value.AddDays(-1).Date.ToDefaultDateFormat();
            model.CurrentDayName = model.WeekDate.Value.Date.ToDefaultDateFormat();
            model.FifthDayName = model.WeekDate.Value.AddDays(1).Date.ToDefaultDateFormat();
            model.SixthDayName = model.WeekDate.Value.AddDays(2).Date.ToDefaultDateFormat();
            model.SeventhDayName = model.WeekDate.Value.AddDays(3).Date.ToDefaultDateFormat();
            model.ReferenceTypeId = refTypeId.IsNotNullAndNotEmpty() ? refTypeId : null;
            if (refType.IsNotNull())
            {
                ViewBag.Type = refType;
            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                ViewBag.lo = "Popup";
            }
            return View(model);
        }

    }
}
