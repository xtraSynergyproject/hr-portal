using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class PermissionController : ApplicationController
    {
        IPermissionBusiness _permissionBusiness;
        IUserContext _userContext;
        public PermissionController(IPermissionBusiness permissionBusiness, IUserContext userContext)
        {
            _permissionBusiness = permissionBusiness;
            _userContext = userContext;
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

        public async Task<IActionResult> ReadPermissionData()
        {
            var model =await _permissionBusiness.GetList(x=>x.PortalId== _userContext.PortalId && x.LegalEntityId==_userContext.LegalEntityId);
           return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePermission(PermissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;
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
