using CMS.Business;
using CMS.Business.Interface.PortalAdmin;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PortalAdmin.Controllers
{
    [Area("PortalAdmin")]
    public class UdfPermissionSettingsController : ApplicationController
    {
        private IPageBusiness _pageBusiness;
        private IUserContext _userContext;
        private IUdfPermissionSettingsBusiness _udfPermissionSettingsBusiness;
        public UdfPermissionSettingsController(IPageBusiness pageBusiness, IUdfPermissionSettingsBusiness udfPermissionSettingsBusiness,IUserContext userContext)
        {
            _pageBusiness = pageBusiness;
            _udfPermissionSettingsBusiness = udfPermissionSettingsBusiness;
            _userContext = userContext;
        }
        public async Task<IActionResult> Index(string pageId)
        {
            if (pageId.IsNotNullAndNotEmpty())
            {
                var page = await _pageBusiness.GetSingleById(pageId);
                ViewBag.PageName = page.Title;
            }
            else
            {
                ViewBag.PageName = "";
            }
            return View();
        }
        public async Task<ActionResult> ReadData()
        {
            var model = await _udfPermissionSettingsBusiness.GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            var data = model;

            var dsResult = data;
            return Json(dsResult);
        }
        public async Task<ActionResult> Create()
        {
            var portal = await _udfPermissionSettingsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            return View("Manage", new UdfPermissionSettingsViewModel
            {
                DataAction = DataActionEnum.Create,
               
            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var portal = await _udfPermissionSettingsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal != null)
            {
                ViewBag.Layout = $"~/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";
            }
            var team = await _udfPermissionSettingsBusiness.GetSingleById(Id);

            if (team != null)
            {

                //var List = await _teamuserBusiness.GetList(x => x.TeamId == Id);
                //if (List != null && List.Count() > 0)
                //{
                //    team.UserIds = List.Select(x => x.UserId).ToList();
                //    team.TeamOwnerId = List.Where(x => x.IsTeamOwner == true).Select(x => x.UserId).FirstOrDefault();
                //}
                team.DataAction = DataActionEnum.Edit;
                return View("Manage", team);
            }
            return View("Manage", new UdfPermissionSettingsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(UdfPermissionSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PortalId = _userContext.PortalId;
                model.LegalEntityId = _userContext.LegalEntityId;

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _udfPermissionSettingsBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false, error = result.Messages.ToHtmlError() });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _udfPermissionSettingsBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                        return Json(new { success = false,error= result.Messages.ToHtmlError() });
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
             
        public async Task<IActionResult> ManageUdfPermission(string udfPermissionHeaderId=null)        
        {
            var result = await _udfPermissionSettingsBusiness.GetSingleById(udfPermissionHeaderId);

            var model = new UdfPermissionSettingsViewModel()
            {
                Id = udfPermissionHeaderId,
                NtsType = result.NtsType,
                TemplateCodes = result.TemplateCodes,
                CategoryCodes = result.CategoryCodes,
            };
            return View(model);
        }

        public IActionResult ReadUdfPermissionColumns(NtsTypeEnum? ntsType, string templatecodes=null, string categoryCodes=null, string udfPermissionHeaderId=null)
        {
            

            //return Json(ToDataSourceResult(request));
            return null;
        }
    }
}
