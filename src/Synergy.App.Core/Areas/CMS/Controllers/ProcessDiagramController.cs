using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
////using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.UI.Web.Controllers
{
    [Area("Cms")]
    public class ProcessDiagramController : ApplicationController
    {
        private readonly IBreMasterMetadataBusiness _breMasterMetadataBusiness;
        IBusinessRuleModelBusiness _ruleBusiness;
        IBreMasterMetadataBusiness _masterMetaBusiness;
        private readonly IBreMetadataBusiness _breMetadataBusiness;
        private readonly IBusinessRuleBusiness _breRuleBusiness;
        private readonly IBusinessRuleModelBusiness _breRuleModelBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;
        IBreResultBusiness _breResultBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly IBreMasterColumnMetadataBusiness _masterColumnMetadataBusiness;
        private readonly IBreMasterTableMetadataBusiness _masterTableMetadataBusiness;
        IPageBusiness _pageBusiness;
        public ProcessDiagramController(IBreMasterMetadataBusiness breMasterMetadataBusiness,
            IBusinessRuleModelBusiness ruleBusiness,
            ITemplateBusiness templateBusiness,
            IBusinessRuleModelBusiness breRuleModelBusiness,
            IColumnMetadataBusiness columnMetadataBusiness,
            IBreMasterMetadataBusiness masterMetaBusiness,
            IBreMetadataBusiness breMetadataBusiness,
            IBusinessRuleBusiness breRuleBusiness
            , IBreResultBusiness breResultBusiness,
            ITableMetadataBusiness tableMetadataBusiness
            , IUserContext userContext
            , IBreMasterColumnMetadataBusiness masterColumnMetadataBusiness
            , IBreMasterTableMetadataBusiness masterTableMetadataBusiness
            , IPageBusiness pageBusiness)
        {
            _breMasterMetadataBusiness = breMasterMetadataBusiness;
            _ruleBusiness = ruleBusiness;
            _templateBusiness = templateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _masterMetaBusiness = masterMetaBusiness;
            _breMetadataBusiness = breMetadataBusiness;
            _breRuleModelBusiness = breRuleModelBusiness;
            _breRuleBusiness = breRuleBusiness;
            _breResultBusiness = breResultBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _userContext = userContext;
            _masterColumnMetadataBusiness = masterColumnMetadataBusiness;
            _masterTableMetadataBusiness = masterTableMetadataBusiness;
            _pageBusiness = pageBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetContentTreeList(string id)
        {
            var result = await _pageBusiness.GetProcessDiagramTreeList(id);
            var model = result.ToList();
            return Json(model);
        }
        public ActionResult ProcessDiagram(string menuGroupId)
        {
            ViewBag.MenuGroupId = menuGroupId;
            return View();
        }
        public async Task<ActionResult> GetPageByMenuId(string menuId)
        {
            return Json(await _pageBusiness.GetPageOnMenuGroupAndPortalId(menuId));
        }
        public async Task<ActionResult> GetTemplateByPageId(string pageId)
        {
            var page=await _pageBusiness.GetSingleById(pageId);
            if (page != null)
            {
                return Json(await _templateBusiness.GetList(x => x.Id == page.TemplateId));
            }
            else 
            {
                return Json(null);
            }           
        }
        public ActionResult BusinessDiagram()
        {
            return View();
        }

    }
}
