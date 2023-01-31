using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Data.Model;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class ListOfValueController : ApplicationController
    {
        private static IUserContext _userContext;
        private IListOfValueBusiness _ListOfValueBusiness;

        public ListOfValueController(IListOfValueBusiness ListOfValueBusiness, IUserContext userContext)
        {
               
            _ListOfValueBusiness = ListOfValueBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetListOfValueList(string type, string viewData = null)
        {
            var data = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == type && x.Status != StatusEnum.Inactive);
            if(viewData != null)
            {
                ViewData[viewData] = data.OrderBy(x => x.SequenceOrder);
            }
            return Json(data.OrderBy(x => x.SequenceOrder));
        }
        [HttpGet]
        public async Task<JsonResult> GetListOfValueListById(string parentId)
        {
            var data = new List<ListOfValueViewModel>();
            if (parentId.IsNotNullAndNotEmpty())
            {
                data = await _ListOfValueBusiness.GetList(x => x.ParentId == parentId && x.Status != StatusEnum.Inactive);
            }
           
           
            return Json(data.OrderBy(x => x.SequenceOrder));
        }

        [HttpGet]
        public async Task<JsonResult> GetListOfValueById(string id)
        {
            var data = await _ListOfValueBusiness.GetSingle(x => x.Id == id && x.Status != StatusEnum.Inactive);

            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllListOfValue(string type)
        {
            var data = await _ListOfValueBusiness.GetList(x=>x.Status != StatusEnum.Inactive);
            return Json(data.OrderBy(x => x.SequenceOrder));
        }
        public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _ListOfValueBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);

            var dsResult = model.OrderBy(x=>x.SequenceOrder).ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<IActionResult> Create(string jobAdvertisementId)
        {
            var portal = await _ListOfValueBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View("Manage", new ListOfValueViewModel
            {
                DataAction = DataActionEnum.Create,

            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var portal = await _ListOfValueBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var ListOfValue = await _ListOfValueBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {

                ListOfValue.DataAction = DataActionEnum.Edit;
                return View("Manage", ListOfValue);
            }
            return View("Manage", new ListOfValueViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ListOfValueViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _ListOfValueBusiness.Create(model);
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
                    var result = await _ListOfValueBusiness.Edit(model);
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
            await _ListOfValueBusiness.Delete(id);
            return View("Index", new ListOfValueViewModel());
        }

        [HttpGet]
        public async Task<JsonResult> GetCriteriaListOfValue(string type, string viewData = null,bool parentid=false)
        {
            var data = new List<ListOfValueViewModel>();
            if(parentid==false)
            {
                data = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == type && x.Status != StatusEnum.Inactive && x.ParentId == null);
            }
            else
            {
                data = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == type && x.Status != StatusEnum.Inactive && x.ParentId!=null);
            }
            
            if (viewData != null)
            {
                ViewData[viewData] = data.OrderBy(x => x.SequenceOrder);
            }
            return Json(data.OrderBy(x => x.SequenceOrder));
        }

        [HttpGet]
        public async Task<JsonResult> GetOtherCriteriaListOfValue(ReferenceTypeEnum type,string jobid ,string ddlvalue, string viewData = null)
        {

            var data = new List<ListOfValueViewModel>();
            //if(ddlvalue=="List of Value")
            //{
                data = await _ListOfValueBusiness.GetList(x => x.ReferenceTypeCode == type && x.ReferenceTypeId==jobid && x.Status != StatusEnum.Inactive&&x.ListOfValueType=="LOV_TYPE");


                if (viewData != null)
                {
                    ViewData[viewData] = data.OrderBy(x => x.SequenceOrder);
                }
               // return Json(data.OrderBy(x => x.SequenceOrder));
            //}
            return Json(data.OrderBy(x => x.SequenceOrder));
        }

    }
}