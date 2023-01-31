using AutoMapper;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class NotificationTemplateController : ApplicationController
    {
        private readonly INotificationTemplateBusiness _notificationTemplateBusiness;
        IMapper _autoMapper;

        public NotificationTemplateController(INotificationTemplateBusiness notificationTemplateBusiness
            , IMapper autoMapper)
        {
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _autoMapper = autoMapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageNotification(string id, string templateId, NtsTypeEnum ntsType)
        {
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
            var list = await _notificationTemplateBusiness.GetList(x => x.IsTemplate == true);
            foreach (var i in list)
            {
                i.ActionStatusCode = String.Join(",", i.ActionStatusCodes);
            }
            var json = Json(list.ToDataSourceResult(request));
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
        [HttpGet]
        public async Task<IActionResult> GetNotificationIdNameList(string groupCode)
        {
            var dataList = await _notificationTemplateBusiness.GetList(x => x.GroupCode == groupCode);
            return Json(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationTemplateData(string templateId)
        {
            var data = await _notificationTemplateBusiness.GetSingleById(templateId);
            return Json(new { subject = data.Subject, body = data.Body });
        }
    }
}
