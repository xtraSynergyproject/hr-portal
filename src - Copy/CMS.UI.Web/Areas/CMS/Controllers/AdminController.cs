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

        public AdminController(ICmsBusiness cmsBusiness)
        {
            _cmsBusiness = cmsBusiness;
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

    }
}
