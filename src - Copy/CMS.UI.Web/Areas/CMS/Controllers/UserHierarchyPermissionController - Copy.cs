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

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserHierarchyPermissionController : ApplicationController

    {

        private IUserHierarchyPermissionBusiness _UserHierarchyPermissionBusiness;
     
        private readonly IPortalBusiness _portalBusiness;
        private readonly IUserContext _userContext;

        public UserHierarchyPermissionController(IUserHierarchyPermissionBusiness UserHierarchyPermissionBusiness,IPortalBusiness portalBusiness,
            IUserContext userContext)
        {
            _UserHierarchyPermissionBusiness = UserHierarchyPermissionBusiness;
         
            _portalBusiness = portalBusiness;
            _userContext = userContext;
        }




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetUserHierarchyPermissionList()
        {
            var data = await _UserHierarchyPermissionBusiness.GetList();
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            return Json(res);
        }
        //public async Task<JsonResult> GetModuleList()
        //{
        //    var data = await _moduleBusiness.GetList();
        //    return Json(data);
        //}
        public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await _UserHierarchyPermissionBusiness.GetList();
            var data = model;

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public IActionResult Create()
        {
            return View("Manage", new UserHierarchyPermissionViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
       /* public async Task<IActionResult> Edit(string Id)
        {
            var module = await _UserHierarchyPermissionBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new UserHierarchyPermissionViewModel());
        }*/
        public async Task<IActionResult> Edit(string Id)
        {
            var UserHierarchyPermission = await _UserHierarchyPermissionBusiness.GetSingleById(Id);

            if (UserHierarchyPermission != null)
            {
                
              
                UserHierarchyPermission.DataAction = DataActionEnum.Edit;
                return View("Manage", UserHierarchyPermission);
            }
            return View("Manage", new UserHierarchyPermissionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(UserHierarchyPermissionViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _UserHierarchyPermissionBusiness.Create(model);
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
                    var result = await _UserHierarchyPermissionBusiness.Edit(model);
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
            await _UserHierarchyPermissionBusiness.Delete(id);
            return View("Index", new UserHierarchyPermissionViewModel());
        }
        //public async Task<IActionResult> Delete(string Id)
        //{
        //    await _UserHierarchyPermissionBusiness.Delete(Id);
        //    return Json(true);
        //}
        //public ActionResult Delete(string id, string ru = null)
        //{
        //    ViewBag.Title = "Delete Task Work Time";
        //    var model = _moduleBusiness.GetSingle(x => x.Id == id);
        //    model.Operation = DataOperation.Delete;
        //    model.ReturnUrl = ru;
        //    var result = _moduleBusiness.Delete(model);
        //    if (!result.IsSuccess)
        //    {
        //        result.Message.Each(x => ModelState.AddModelError(x.Key, x.Value));
        //        return Json(new { success = false, errors = ModelState.SerializeErrors() });
        //    }
        //    else
        //    {
        //        return Json(new { success = true, operation = model.Operation.ToString(), Id = model.Id, ru = model.ReturnUrl });
        //    }
        //}
        //public async Task<ActionResult> GetUserHierarchyPermissionByOwner()
        //{
        //    var data =await _UserHierarchyPermissionBusiness.GetUserHierarchyPermissionByOwner(_userContext.UserId);
        //    return Json(data);
        //}
        //public async Task<ActionResult> GetUserHierarchyPermissionMemberList(string UserHierarchyPermissionId)
        //{
        //    var data = await _UserHierarchyPermissionBusiness.GetUserHierarchyPermissionMemberList(UserHierarchyPermissionId);
        //    return Json(data);
        //}
        
    }
}