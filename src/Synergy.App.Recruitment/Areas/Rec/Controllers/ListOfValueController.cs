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
    public class ListOfValueController : ApplicationController
    {

        private IListOfValueBusiness _ListOfValueBusiness;

        public ListOfValueController(IListOfValueBusiness ListOfValueBusiness)
        {
            _ListOfValueBusiness = ListOfValueBusiness;
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
            var model = await _ListOfValueBusiness.GetList();

            var dsResult = model.OrderBy(x=>x.SequenceOrder).ToDataSourceResult(request);
            return Json(dsResult);
        }
        public IActionResult Create(string jobAdvertisementId)
        {
            return View("Manage", new ListOfValueViewModel
            {
                DataAction = DataActionEnum.Create,

            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
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