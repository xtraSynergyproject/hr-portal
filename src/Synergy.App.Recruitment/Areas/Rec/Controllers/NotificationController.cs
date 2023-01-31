using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class NotificationController : ApplicationController
    {

        private IPushNotificationBusiness _pushNotificationBusiness;
        private INotificationBusiness _notificationBusiness;
        private IRecTaskBusiness _taskBusiness;
        private IUserContext _userContext;

        public NotificationController(IPushNotificationBusiness pushNotificationBusiness, IUserContext userContext, IRecTaskBusiness taskBusiness
            , INotificationBusiness notificationBusiness)
        {
            _pushNotificationBusiness = pushNotificationBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _notificationBusiness = notificationBusiness;
        }
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<ActionResult> TopNotificationsViewFromBell()
        {
            NotificationSearchViewModel listModel = new NotificationSearchViewModel
            {
                Notifications = await _pushNotificationBusiness.GetNotificationList(_userContext.UserId, 0)
            };
            return PartialView("_Notification", listModel);
        }
        public async Task<ActionResult> UnReadNotificationCount(ModuleEnum? moduleName = null)
        {
            IList<NotificationViewModel> model = null;
            model = await _pushNotificationBusiness.GetList(x => x.ToUserId == _userContext.UserId && x.ReadStatus == ReadStatusEnum.NotRead);
            if (moduleName.IsNotNull())
            {
                model = model.Where(x => x.ModuleName == moduleName).ToList();
            }
            var count = model != null ? model.Count() : -1;
            return Json(new { UnReadCount = count });
        }
        
        //public ActionResult UpdateReadNotification(long Id, string layout)
        //{
        //    if (layout != "")
        //    {
        //        ViewBag.Layout = layout;
        //    }           
        //    var single = _notificationBusiness.ViewModelList<NotificationViewModel>("n.Id={Id}", userParam, "").FirstOrDefault();

        //    if (single.ReadStatus == ReadStatusEnum.NotRead)
        //    {
        //        var result = _notificationBusiness.UpdateAsRead(single);
        //        if (!result.IsSuccess)
        //        {
        //            result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
        //            return Json(new { success = false, errors = ModelState.SerializeErrors() });
        //        }
        //    }
        //    if (single.Url != null)
        //    {
        //        var url = Regex.Replace(single.Url, @"\t|\n|\r", "");
        //        return Redirect(url);
        //    }
        //    else
        //    {
        //        return Manage(single, layout);
        //    }
        //}
        public async Task<bool> UpdateAllReadNotification()
        {
            await _pushNotificationBusiness.SetAllNotificationRead(_userContext.UserId);
            return true;
        }
        public async Task<ActionResult> ReadNotificationData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _pushNotificationBusiness.GetList(x => x.ToUserId == _userContext.UserId);
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadNotificationDataByTask([DataSourceRequest] DataSourceRequest request, NtsTypeEnum referenceType, string referenceId)
        {
            var model = new List<NotificationViewModel>();
            var id = new List<string>();
            if (referenceType == NtsTypeEnum.Task)
            {
                var refId = new List<string>();

                var service = await _taskBusiness.GetSingleById(referenceId);
                if (service.IsNotNull() && service.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                {
                    var ids = await _taskBusiness.GetList(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task);
                    refId = ids.Select(x => x.Id).ToList();
                    refId.Add(service.ReferenceTypeId);
                }
                else
                {
                    refId.Add(referenceId);
                }

                model = await _pushNotificationBusiness.GetList(x => refId.Contains(x.ReferenceTypeId));
            }
            if (referenceType == NtsTypeEnum.Service)
            {
                //var service = await _taskBusiness.GetSingleById(referenceId);
                var ids = await _taskBusiness.GetList(x => x.ReferenceTypeId == referenceId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task);
                var refId = ids.Select(x => x.Id).ToList();
                refId.Add(referenceId);
                model = await _pushNotificationBusiness.GetList(x => refId.Contains(x.ReferenceTypeId));
            }
            model = model.OrderByDescending(x => x.CreatedDate).ToList();
            //await _notificationBusiness.GetList(x=>x.ReferenceTypeId==referenceId);
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> NotificationView(string Id, string layout)
        {
            if (layout != "" && layout != null)
            {
                ViewBag.Layout = layout;
            }
            ViewBag.Title = "View Notification";

            var single = await _pushNotificationBusiness.GetSingleById(Id);

            if (single.ReadStatus == ReadStatusEnum.NotRead)
            {
                var result = await _pushNotificationBusiness.UpdateAsRead(single);
            }
            if (layout != "" && layout != null)
            {
                ViewBag.Layout = layout;
            }
            return View("ViewNotification", single);
        }
    }
}