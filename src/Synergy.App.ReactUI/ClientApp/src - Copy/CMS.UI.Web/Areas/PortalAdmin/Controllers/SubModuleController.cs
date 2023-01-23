using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class SubModuleController : Controller
    {
        

        private readonly ISubModuleBusiness _submoduleBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private static IUserContext _UserContext;
        public SubModuleController(ISubModuleBusiness subModuleBusiness, IModuleBusiness moduleBusiness, IUserContext UserContext)
        {
            _submoduleBusiness = subModuleBusiness;
            _moduleBusiness = moduleBusiness;
            _UserContext = UserContext;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetSubModuleList()
        {
            var data = await _submoduleBusiness.GetList(x=>x.LegalEntityId==_UserContext.LegalEntityId && x.PortalId == _UserContext.PortalId);
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            return Json(res);
        }
        /* public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
         {
             var model = _submoduleBusiness.GetList();
             var data = model.Result;

             var dsResult = data.ToDataSourceResult(request);
             return Json(dsResult);
         }*/
        //public async Task<JsonResult> ReadData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var module = await _moduleBusiness.GetList(x => x.LegalEntityId == _UserContext.LegalEntityId  && x.PortalId == _UserContext.PortalId);

        //    var data = await _submoduleBusiness.GetList(x => x.LegalEntityId == _UserContext.LegalEntityId && x.PortalId == _UserContext.PortalId);
        //    //var data = model.Result.ToList();

        //    var res = from d in data
        //              join s in module
        //              on d.ModuleId equals s.Id

        //              select new SubModuleViewModel
        //              {
        //                  Name = d.Name,
        //                  ShortName = d.ShortName,
        //                  ModuleName = s.Name,
        //                  Status = d.Status,
        //                  Id = d.Id
        //              };

        //    var dsResult = res.ToDataSourceResult(request);
        //    return Json(dsResult);

        //}
        public async Task<ActionResult> ReadData()
        {
            var model = await _submoduleBusiness.GetPortalSubModuleList();
            return Json(model);
        }

        public IActionResult Create()
        {
            return View("Manage", new SubModuleViewModel
            {
                DataAction = DataActionEnum.Create,
                PortalId = _UserContext.PortalId,
                LegalEntityId=_UserContext.LegalEntityId,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            }) ;
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var module = await _submoduleBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new SubModuleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(SubModuleViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _submoduleBusiness.Create(model);
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
                    var result = await _submoduleBusiness.Edit(model);
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
            await _submoduleBusiness.Delete(id);
            return View("Index", new SubModuleViewModel());
        }


    }
}
