using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
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
    [Authorize(Policy = nameof(AuthorizeCMS))]
    public class AdminController : ApplicationController
    {
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IPageBusiness _pageBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IUserContext _userContext;

        public AdminController(ICmsBusiness cmsBusiness, IPageBusiness pageBusiness
            , IPortalBusiness portalBusiness, IUserContext userContext)
        {
            _cmsBusiness = cmsBusiness;
            _pageBusiness = pageBusiness;
            _portalBusiness = portalBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> GetAdminTreeList(string id)
        {
            //var result = await _seetingsBusiness.GetDocumentTypeTreeList(id);
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                list.Add(new TreeViewViewModel
                {
                    id = "module",
                    Name = "Module",
                    DisplayName = "Module",
                    ParentId = null,
                    hasChildren = false,
                    expanded = false,

                });
                list.Add(new TreeViewViewModel
                {
                    id = "subModule",
                    Name = "Sub Module",
                    DisplayName = "Sub Module",
                    ParentId = null,
                    hasChildren = false,
                    expanded = false,

                });
                list.Add(new TreeViewViewModel
                {
                    id = "menuGroup",
                    Name = "Menu Group",
                    DisplayName = "MenuGroup",
                    ParentId = null,
                    hasChildren = false,
                    expanded = false,

                });
                list.Add(new TreeViewViewModel
                {
                    id = "user",
                    Name = "User",
                    DisplayName = "User",
                    ParentId = null,
                    hasChildren = false,
                    expanded = false,

                });
                list.Add(new TreeViewViewModel
                {
                    id = "userrole",
                    Name = "User Role",
                    DisplayName = "User Role",
                    ParentId = null,
                    hasChildren = false,
                    expanded = false,

                });

            }

            return Json(list.ToList());
        }

        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string templateCode)
        {
            var result1 = await _cmsBusiness.GetInboxMenuItem(id, type, templateCode);
            var model1 = result1.ToList();
            return Json(model1);
        }

        public async Task<IActionResult> ReadInboxData(string id, string type, string templateCode)
        {
            var result1 = await _cmsBusiness.ReadInboxData(id, type, templateCode);
            var model1 = result1.ToList();
            return Json(model1);
        }

        public IActionResult TECProcessInbox(string templateCode)
        {
            ViewBag.TemplateCode = templateCode;
            return View();
        }

        public IActionResult InboxData(string id, string type, string templateCode)
        {
            ViewBag.Id = id;
            ViewBag.Type = type;
            ViewBag.TemplateCode = templateCode;
            return View();
        }

        public async Task<IActionResult> SideMenuIndex(string groupCode, string pageId, string pName = null)
        {
            var pagedetails = await _pageBusiness.GetSingleById(pageId);
            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            var menus = await _portalBusiness.GetMenuItems(portal, _userContext.UserId, _userContext.CompanyId, groupCode);
            menus = menus.Where(x => x.IsHidden == false).ToList();
            if (pName.IsNullOrEmpty() && menus.Count > 0)
            {
                var mg = menus.Where(x => x.PageType == TemplateTypeEnum.MenuGroup).FirstOrDefault();
                var page = menus.Where(x => x.PageType != TemplateTypeEnum.MenuGroup && x.ParentId == mg.Id).OrderBy(x => x.SequenceOrder).FirstOrDefault();
                ViewBag.PageName = page.Name;
                ViewBag.PageType = page.PageType;
            }
            else
            {
                var pagedetails1 = await _pageBusiness.GetSingle(x => x.Name == pName);
                ViewBag.PageType = pagedetails1.PageType;
                ViewBag.PageName = pName;
            }
            if (pagedetails.IsNotNull())
            {
                ViewBag.PageDisplayName = pagedetails.Title;
            }
            ViewBag.Menus = menus;
            ViewBag.PortalId = _userContext.PortalId;

            return View();
        }

    }
}
