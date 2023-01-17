using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Business.Interface;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class UserSetController : ApplicationController
    {
        private IUserSetBusiness _userSetBusiness;
       

        public UserSetController(IUserSetBusiness userSetBusiness)
        {
            _userSetBusiness = userSetBusiness;
        
        }
        public IActionResult Index()
        {
            return View();
        }


      
        public async Task<ActionResult> ReadData()
        {
            var model = await _userSetBusiness.GetList();
            return Json(model);
        }
        public IActionResult Create()
        {
            return View("Manage", new UserSetViewModel
            {
                DataAction = DataActionEnum.Create,
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            });
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var member = await _userSetBusiness.GetSingleById(Id);

            if (member != null)
            {
                //    var TempList = await _UserSetTemplateBusiness.GetList(x => x.UserSetId == Id);
                //    if (TempList != null && TempList.Count() > 0)
                //    {
                //        member.TemplateIds = TempList.Select(x => x.TemplateId).ToList();

                //    }
                //    var TempList1 = await _UserSetUserGroupBusiness.GetList(x => x.UserSetId == Id);
                //    if (TempList1 != null && TempList1.Count() > 0)
                //    {
                //        member.UserGroupIds = TempList1.Select(x => x.UserGroupId).ToList();

                //    }
                //member.UserId = string.Join(",", member.Users.to);
                member.UserId = member.Users.ToList();
                member.DataAction = DataActionEnum.Edit;
                return View("Manage", member);
            }
            return View("Manage", new UserSetViewModel());

        }

        public async Task<IActionResult> Delete(string Id)
        {


            await _userSetBusiness.Delete(Id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(UserSetViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    model.Users = model.UserId.ToArray();
                    var result = await _userSetBusiness.Create(model);
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
                    model.Users = model.UserId.ToArray();
                    var result = await _userSetBusiness.Edit(model);
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

        //[HttpGet]
        //public async Task<ActionResult> GetUserSetIdNameList()
        //{
        //    var data = await _UserSetBusiness.GetList();
        //    return Json(data);
        //}
    }
}
