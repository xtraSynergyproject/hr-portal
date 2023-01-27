using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class GrantAccessController : ApplicationController
    {
        private IGrantAccessBusiness _business;
        private IUserBusiness _userBusiness;
        private IUserContext _userContext;
        public GrantAccessController(IGrantAccessBusiness business, IUserBusiness userBusiness, IUserContext userContext)
        {
            _business = business;
            _userBusiness = userBusiness;
            _userContext = userContext;
        }
        private async Task<IList<UserViewModel>> GetSwitchToUserList(DataSourceRequest request = null)
        {
            var hasLoggedinAsPermission = _userContext.IsSystemAdmin
                 || _userContext.LoggedInAsType == LoggedInAsTypeEnum.LoginAsDifferentUser;
            dynamic searchParam = null;
            if (request != null && request.Filters.Count > 0)
            {
                searchParam = request.Filters.FirstOrDefault();
                searchParam = searchParam.ConvertedValue;

            }
            return await _userBusiness.GetSwitchToUserList(_userContext.UserId, _userContext.LoggedInAsByUserId, hasLoggedinAsPermission, searchParam);

        }
        public async Task<ActionResult> GetSwitchToUserVirtualData([DataSourceRequest] DataSourceRequest request)
        {
            var data = await GetSwitchToUserList(request);
            request.Filters.Clear();
            return Json(data);
        }
        public async Task<ActionResult> GetSwitchUserValueMapper(string value)
        {
            var dataItemIndex = -1;
            var data = await GetSwitchToUserList();
            if (value.IsNotNullAndNotEmpty())
            {
                var item = data.FirstOrDefault(x => x.Id == value);
                dataItemIndex = data.IndexOf(item);
            }
            return Json(dataItemIndex);
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Manage GrantAccess";
            return View("_Index");
        }


        public async Task<ActionResult> Create()
        {
            ViewBag.Title = "Create GrantAccess";
            var model = new GrantAccessViewModel
            {
                DataAction = DataActionEnum.Create,
                GrantStatus = GrantStatusEnum.Granted,
                StartDate = DateTime.Now.Date,
                //EndDate = DateTime.MaxValue.Date
                EndDate = DateTime.Now.Date.AddYears(50)
            };
            return View("_Add", model);
        }


        public async Task<ActionResult> Correct(string id)
        {
            ViewBag.Title = "Correct GrantAccess";
            var model = await _business.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;


            return View("_Add", model);
        }

        //public async Task<ActionResult> ReadGrantAccessData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model =await _business.GetGrantAccessList(_userContext.UserId);
        //    var j = Json(model.ToDataSourceResult(request));
        //    return j;
        //}
        public async Task<ActionResult> ReadGrantAccessData()
        {
            var model = await _business.GetGrantAccessList(_userContext.UserId);
            var j = Json(model);
            return j;
        }
        [HttpPost]
        public async Task<ActionResult> Manage(GrantAccessViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    return await CreateGrantAccess(model);
                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    return await CorrectGrantAccess(model);
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }



        private async Task<ActionResult> CreateGrantAccess(GrantAccessViewModel model)
        {
            var result = await _business.Create(model);
            if (!result.IsSuccess)
            {
                foreach (var x in result.Messages)
                {
                    ModelState.AddModelError(x.Key, x.Value);
                }
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), GrantAccessId = model.Id });
            }
        }



        private async Task<ActionResult> CorrectGrantAccess(GrantAccessViewModel model)
        {
            var result = await _business.Edit(model);
            if (!result.IsSuccess)
            {
                foreach (var x in result.Messages)
                {
                    ModelState.AddModelError(x.Key, x.Value);
                }
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), GrantAccessId = model.Id });
            }
        }


        //[HttpGet]
        //public async Task<ActionResult> GetIdNameList()
        //{
        //    var data =await _business.GetActiveIdNameList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}
