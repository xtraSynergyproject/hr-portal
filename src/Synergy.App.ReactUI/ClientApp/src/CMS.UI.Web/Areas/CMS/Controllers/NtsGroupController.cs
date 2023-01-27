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
    public class NtsGroupController : ApplicationController
    {
        private INtsGroupBusiness _ntsGroupBusiness;
        private INtsGroupTemplateBusiness _ntsGroupTemplateBusiness;
        private INtsGroupUserGroupBusiness _ntsGroupUserGroupBusiness;

        public NtsGroupController(INtsGroupBusiness ntsGroupBusiness, INtsGroupTemplateBusiness ntsGroupTemplateBusiness
            , INtsGroupUserGroupBusiness ntsGroupUserGroupBusiness)
        {
            _ntsGroupBusiness = ntsGroupBusiness;
           _ntsGroupTemplateBusiness = ntsGroupTemplateBusiness;
            _ntsGroupUserGroupBusiness = ntsGroupUserGroupBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> GetGroupListByType(NtsTypeEnum type)
        {
            var templateList = await _ntsGroupBusiness.GetList(x=>x.NtsType==type);
            var t = Json(templateList);
            return t;
        }

        //public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = _ntsGroupBusiness.GetList();
        //    var data = model.Result.ToList();

        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}

        public async Task<ActionResult> ReadData()
        {
            var model = await _ntsGroupBusiness.GetList();
            return Json(model);
        }

        public IActionResult CreateNtsGroup()
        {
            return View("Manage", new NtsGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            });
        }

        public async Task<IActionResult> EditNtsGroup(string Id)
        {
            var member = await _ntsGroupBusiness.GetSingleById(Id);

            if (member != null)
            {
                var TempList = await _ntsGroupTemplateBusiness.GetList(x => x.NtsGroupId == Id);
                if (TempList != null && TempList.Count() > 0)
                {
                    member.TemplateIds = TempList.Select(x => x.TemplateId).ToList();

                }
                var TempList1 = await _ntsGroupUserGroupBusiness.GetList(x => x.NtsGroupId == Id);
                if (TempList1 != null && TempList1.Count() > 0)
                {
                    member.UserGroupIds = TempList1.Select(x => x.UserGroupId).ToList();

                }

                member.DataAction = DataActionEnum.Edit;
                return View("Manage", member);
            }
            return View("Manage", new NtsGroupViewModel());

        }

        public async Task<IActionResult> DeleteNtsGroup(string Id)
        {


            await _ntsGroupBusiness.Delete(Id);
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(NtsGroupViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _ntsGroupBusiness.Create(model);
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
                    var result = await _ntsGroupBusiness.Edit(model);
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
        //public async Task<ActionResult> GetNtsGroupIdNameList()
        //{
        //    var data = await _NtsGroupBusiness.GetList();
        //    return Json(data);
        //}
    }
}
