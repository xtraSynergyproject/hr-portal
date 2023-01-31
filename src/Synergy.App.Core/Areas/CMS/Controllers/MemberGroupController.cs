using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.ViewModel;
using Synergy.App.Common;
////using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
////using Kendo.Mvc.UI;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class MemberGroupController : ApplicationController
    {
        // GET: MemberGroup

        private IMemberGroupBusiness _memberGroupBusiness;
        private readonly IPortalBusiness _portalBusiness;


        public MemberGroupController(IMemberGroupBusiness memberGroupBusiness, IPortalBusiness portalBusiness)
        {
            _memberGroupBusiness = memberGroupBusiness;
            _portalBusiness = portalBusiness;
                }
           

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = _memberGroupBusiness.GetList();
        //    var data = model.Result.ToList();

        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
        public ActionResult ReadData()
        {
            var model = _memberGroupBusiness.GetList();
            var data = model.Result.ToList();           
            return Json(data);
        }

        public IActionResult MemberGroupList()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPortalList()
        {
            var data = await _portalBusiness.GetList();
            return Json(data);
        }
        public IActionResult CreateMemberGroup()
        {
            return View("ManageMemberGroup", new MemberGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
        });
        }
        public async Task<IActionResult> EditMemberGroup(string Id)
        {
            var member = await _memberGroupBusiness.GetSingleById(Id);

            if (member != null)
            {   
                if (member.Status == true)
                {
                    member.GroupStatus = StatusEnum.Active;
                }
                member.DataAction = DataActionEnum.Edit;
                return View("ManageMemberGroup", member);
            }
            return View("ManageMemberGroup", new MemberGroupViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageMemberGroup(MemberGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Status==true)
                {
                    model.GroupStatus = StatusEnum.Active;
                }
                else
                {
                    model.GroupStatus = StatusEnum.Inactive;
                }
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _memberGroupBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                       // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _memberGroupBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageMemberGroup", model);
        }

        // GET: MemberGroup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MemberGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberGroup/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MemberGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberGroup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemberGroup/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
