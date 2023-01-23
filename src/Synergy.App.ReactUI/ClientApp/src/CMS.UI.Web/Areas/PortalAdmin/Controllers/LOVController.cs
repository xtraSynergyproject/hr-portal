using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class LOVController : ApplicationController
    {
        private ILOVBusiness _LOVBusiness;
        private static IUserContext _userContext;
        public LOVController(ILOVBusiness LOVBusiness, IUserContext userContext)
        {
            _LOVBusiness = LOVBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetListOfValueList(string type)
        {
            var data = await _LOVBusiness.GetList(x => x.LOVType == type && x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }


        [HttpGet]
        public async Task<JsonResult> GetListOfValueByid(string ID)
        {
            var data = await _LOVBusiness.GetList(x => x.Id == ID && x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }

        [HttpGet]
        public async Task<JsonResult> GetAllListOfValue(string type)
        {
            var data = await _LOVBusiness.GetList(x => x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }
       
        public async Task<ActionResult> ReadData()
        {
            var model = await _LOVBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);

            var dsResult = model.OrderBy(x => x.SequenceOrder);
            return Json(dsResult);
        }
        public async Task<ActionResult> Create(string jobAdvertisementId)
        {
            var portal = await _LOVBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View("Manage", new LOVViewModel
            {
                DataAction = DataActionEnum.Create,

            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var portal = await _LOVBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var ListOfValue = await _LOVBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {

                ListOfValue.DataAction = DataActionEnum.Edit;
                return View("Manage", ListOfValue);
            }
            return View("Manage", new LOVViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(LOVViewModel model)
        {
            if (ModelState.IsValid)
            {
                 model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _LOVBusiness.Create(model);
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
                    var result = await _LOVBusiness.Edit(model);
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

            return View("Manage", model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _LOVBusiness.Delete(id);
            return View("Index", new LOVViewModel());
        }

        public async Task<ActionResult> GetLOVIdNameList(string lovType)
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var model = await _LOVBusiness.GetList(x => x.LOVType == lovType);

            list = model.Select(x => new IdNameViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            }).ToList();
            return Json(list);
        }


    }
}
