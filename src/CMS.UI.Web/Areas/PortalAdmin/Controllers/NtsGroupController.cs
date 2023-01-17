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

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class NtsGroupController : ApplicationController
    {
        private INtsGroupBusiness _ntsGroupBusiness;
        private INtsGroupTemplateBusiness _ntsGroupTemplateBusiness;
        private INtsGroupUserGroupBusiness _ntsGroupUserGroupBusiness;
        private static IUserContext _userContext;
        private IPortalBusiness _portalBusiness;

        public NtsGroupController(INtsGroupBusiness ntsGroupBusiness, INtsGroupTemplateBusiness ntsGroupTemplateBusiness
            , INtsGroupUserGroupBusiness ntsGroupUserGroupBusiness
            , IUserContext userContext
            , IPortalBusiness portalBusiness)
        {
            _ntsGroupBusiness = ntsGroupBusiness;
            _ntsGroupTemplateBusiness = ntsGroupTemplateBusiness;
            _ntsGroupUserGroupBusiness = ntsGroupUserGroupBusiness;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetGroupListByType(NtsTypeEnum type)
        {
            var templateList = await _ntsGroupBusiness.GetList(x => x.NtsType == type);
            var t = Json(templateList);
            return t;
        }

        public async Task<ActionResult> ReadData()
        {
            var model = await _ntsGroupBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            //var data = model.Result.ToList();
            //var dsResult = model.ToDataSourceResult(request);
            return Json(model);
        }

        public IActionResult CreateNtsGroup()
        {
            var model = new NtsGroupViewModel
            {
                DataAction = DataActionEnum.Create,
                LegalEntityId = _userContext.LegalEntityId,
                PortalId = _userContext.PortalId,
                //Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),

            };
            //var portal = await _portalBusiness.GetSingleById(_userContext.PortalId);

            //ViewBag.Layout = $"~/Views/Shared/Themes/{_userContext.PortalTheme}/_Layout.cshtml";

            return View("Manage", model);
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

    }
}
