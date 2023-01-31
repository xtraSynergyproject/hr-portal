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
    public class UserRoleStageController : ApplicationController
    {
        private IUserRoleBusiness _userRoleBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private IUserRoleStageChildBusiness _userRoleStageChildBusiness;
        private IUserRoleStageParentBusiness _userRoleStageParentBusiness;
        

        public UserRoleStageController(IUserRoleBusiness userRoleBusiness, IUserRoleUserBusiness userRoleUserBusiness,
             IUserRoleStageChildBusiness userRoleStageChildBusiness,
        IUserRoleStageParentBusiness userRoleStageParentBusiness)
        {
            _userRoleBusiness = userRoleBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _userRoleStageChildBusiness = userRoleStageChildBusiness;
            _userRoleStageParentBusiness = userRoleStageParentBusiness;
          
        }
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = _userRoleBusiness.GetList();
            var data = model.Result.ToList();

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public IActionResult CreateUserRoleStageParent(string id)
        {
            var model = new UserRoleStageParentViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model.DataAction = DataActionEnum.Edit;
                model.Id = id;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            return View("ManageUserRoleStageParent", model);
        }

        public IActionResult CreateUserRoleStageChild(string inboxStageId)
        {
            return View("ManageUserRoleStageChild", new UserRoleStageChildViewModel
            {
                DataAction = DataActionEnum.Create,
                InboxStageId= inboxStageId
            });
        }

        public async Task<IActionResult> EditUserRoleStageParent(string Id)
        {
            var member = await _userRoleStageParentBusiness.GetSingleById(Id);

            if (member != null)
            {
                member.DataAction = DataActionEnum.Edit;
                return View("ManageUserRoleStageParent", member);
            }
            return View("ManageUserRoleStageParent", new UserRoleStageParentViewModel());
        }

        public async Task<IActionResult> DeleteUserRoleStageParent(string Id)
        {


            await _userRoleStageParentBusiness.Delete(Id);
            return Json(true);
        }

        public async Task<IActionResult> EditUserRoleStageChild(string Id)
        {
            var model = await _userRoleStageChildBusiness.GetSingleById(Id);

            if (model != null)
            {
                model.DataAction = DataActionEnum.Edit;
                model.TextStatusCode = String.Join(",",model.StatusCode);

                return View("ManageUserRoleStageChild", model);
            }
            return View("ManageUserRoleStageChild", new UserRoleStageChildViewModel());
        }

        public async Task<IActionResult> DeleteUserRoleStageChild(string Id)
        {


            await _userRoleStageChildBusiness.Delete(Id);
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoleStageParent(UserRoleStageParentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _userRoleStageParentBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, id = result.Item.Id });

                    }
                    else
                    {
                        return Json(new { success = false, id = result.Item.Id });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _userRoleStageParentBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        return Json(new { success = true, id = result.Item.Id });
                    }
                    else
                    {
                        return Json(new { success = false, id = result.Item.Id });
                    }
                }
            }

            return Json(new { success = false, id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoleChildParent(UserRoleStageChildViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    model.StatusCode = model.TextStatusCode.Split(',').ToArray();
                    //model.StatusCode = String.Join(",", statuscode);
                    var result = await _userRoleStageChildBusiness.Create(model);
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
                    model.StatusCode = model.TextStatusCode.Split(',').ToArray();
                    var result = await _userRoleStageChildBusiness.Edit(model);
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

            return View("ManageUserRoleStageChild", model);
        }


        [HttpGet]
        public async Task<ActionResult> GetUserRoleIdNameList()
        {
            var data = await _userRoleBusiness.GetList();
            return Json(data);
        }


        public async Task<ActionResult> GetUserRoleStageParentList([DataSourceRequest] DataSourceRequest request)
        {
            var data = await _userRoleStageParentBusiness.GetUserRoleStageParentList();
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUserRoleStageParentListData()
        {
            var data = await _userRoleStageParentBusiness.GetUserRoleStageParentList();
            return Json(data);
        }

        public async Task<ActionResult> GetUserRoleStageChildList([DataSourceRequest] DataSourceRequest request,string inboxStageId)
        {
            var data = await _userRoleStageChildBusiness.GetList(x=>x.InboxStageId== inboxStageId);
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetUserRoleStageChildListData(string inboxStageId)
        {
            var data = await _userRoleStageChildBusiness.GetList(x => x.InboxStageId == inboxStageId);
            return Json(data);
        }
    }
}
