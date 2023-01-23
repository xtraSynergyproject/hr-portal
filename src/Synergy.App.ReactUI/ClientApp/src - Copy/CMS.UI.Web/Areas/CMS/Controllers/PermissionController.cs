using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class PermissionController : ApplicationController
    {
        IPermissionBusiness _permissionBusiness;
        public PermissionController(IPermissionBusiness permissionBusiness)
        {
            _permissionBusiness = permissionBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePermission()
        {
            return View("ManagePermission", new PermissionViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            });
        }
        public async Task<IActionResult> EditPermission(string Id)
        {
            var member = await _permissionBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                return View("ManagePermission", member);
            }
            return View("ManagePermission", new PermissionViewModel());
        }

        //public ActionResult ReadPermissionData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = _permissionBusiness.GetList();
        //    var data = model.Result;

        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
      
        public async Task<ActionResult> ReadPermissionData()
        {
            var model = await _permissionBusiness.GetList();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePermission(PermissionViewModel model)
        {
            if (ModelState.IsValid)
            {                
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _permissionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return PopupRedirect("Permission created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _permissionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return PopupRedirect("Permission edited successfully", true);                        
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("ManagePermission", model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _permissionBusiness.Delete(id);
            //return View("Index", new PermissionViewModel());
            return Json(new { success = true });
        }

    }
}
