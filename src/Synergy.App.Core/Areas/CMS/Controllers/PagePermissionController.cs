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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class PagePermissionController : ApplicationController
    {
        private readonly IPageMemberBusiness _memberBusiness;
        private readonly IPageMemberGroupBusiness _groupBusiness;
        public PagePermissionController(IPageMemberBusiness memberBusiness, IPageMemberGroupBusiness groupBusiness)
        {
            _memberBusiness = memberBusiness;
            _groupBusiness = groupBusiness;
        }
        public IActionResult PagePermission(string portalId,string pageId)
        {
            var model = new PageMemberViewModel { PortalId = portalId,PageId= pageId };
            return View(model);
        }
        public IActionResult Memberlist()
        {
            return View();
        }
        public IActionResult CreateMember()
        {
            return View("ManageMember", new MemberViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            });
        }
        public async Task<IActionResult> EditMember(string Id)
        {
            var member = await _memberBusiness.GetSingleById(Id);
           
            if (member != null)
            {
                //if (member.MemberStatus == StatusEnum.Active)
                //{
                //    member.Status = true;
                //}
                member.DataAction = DataActionEnum.Edit;
                return View("ManageMember", member);
            }
            return View("ManageMember", new MemberViewModel());
        }       
        public async Task<JsonResult> ReadPermissionData(string id,string portalId)
        {
            var result = await _memberBusiness.GetPagePermission(id, portalId);
            return Json(result.ToList());
        }
        public async Task<JsonResult> ReadGroupPermissionData(string id, string portalId)
        {
            var result = await _groupBusiness.GetPageMemberGroupPermission(id, portalId);
            return Json(result.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> ManagePagePermission(string data, string groupdata,string pageId,string portalId)
        {
            try
            {
                var model = new PageMemberViewModel{MemberIds=data,PageId=pageId,PortalId=portalId };
               
                var result = await _memberBusiness.Create(model);
                    //if (result.IsSuccess)
                    //{
                    //    return Json(new { success = true, result = result.Item });
                    //}
                    //else
                    //{
                    //    ModelState.AddModelErrors(result.Messages);
                    //    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                    //}


                var grpmodel = new PageMemberGroupViewModel { MemberGroupIds = groupdata, PageId = pageId, PortalId = portalId };

                 await _groupBusiness.Create(grpmodel);
               
                 return Json(new { success = true });
                

                //else if (model.DataAction == DataActionEnum.Edit)
                //{
                //    var result = await _memberBusiness.Edit(model);
                //    if (result.IsSuccess)
                //    {                        
                //        return Json(new { success = true, result = result.Item });
                //    }
                //    else
                //    {
                //        ModelState.AddModelErrors(result.Messages);
                //        return Json(new { success = false, errors = ModelState.SerializeErrors() });
                //    }
                //}
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
           
        }
    }
}
