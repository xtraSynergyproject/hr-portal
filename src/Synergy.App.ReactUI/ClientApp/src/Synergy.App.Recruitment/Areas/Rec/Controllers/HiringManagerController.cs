using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class HiringManagerController : ApplicationController
    {
        private readonly IHiringManagerBusiness _hiringManagerBusiness;

        public HiringManagerController(IHiringManagerBusiness hiringManagerBusiness)
        {
            _hiringManagerBusiness = hiringManagerBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult CreateHiringManager()
        {
            return View("ManageHiringManager", new HiringManagerViewModel
            {
                DataAction = DataActionEnum.Create,                
            });
        }
        public async Task<IActionResult> EditHiringManager(string Id)
        {
            var hmmodel = await _hiringManagerBusiness.GetSingleById(Id);
            var organizations = await _hiringManagerBusiness.GetList<HiringManagerOrganizationViewModel, HiringManagerOrganization>(x => x.HiringManagerId == Id);
            if (organizations!=null && organizations.Count()>0) 
            {
                hmmodel.OrganizationId = organizations.Select(x => x.OrganizationId).ToArray();
            }
            if (hmmodel != null)
            {
                hmmodel.DataAction = DataActionEnum.Edit;
                return View("ManageHiringManager", hmmodel);
            }
            return View("ManageHiringManager", new HiringManagerViewModel());
        }

        public async Task<JsonResult> ReadHiringManagerList([DataSourceRequest] DataSourceRequest request)
        {
            var hmmodelList = await _hiringManagerBusiness.GetHiringManagersList(); 
            var dsResult = hmmodelList.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<JsonResult> GetHiringManagerList()
        {
            var hmmodelList = await _hiringManagerBusiness.GetHiringManagersList();
            return Json(hmmodelList);
        }

        [HttpPost]
        public async Task<IActionResult> ManageHiringManager(HiringManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _hiringManagerBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return PopupRedirect("Menu Group created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _hiringManagerBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return PopupRedirect("Menu Group edited successfully", true);                        
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("ManageHiringManager", model);
        }       

        public async Task<IActionResult> Delete(string id)
        {
            await _hiringManagerBusiness.Delete(id);            
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<ActionResult> GetUserIdNameList()
        {
            var data = await _hiringManagerBusiness.GetUserIdList();
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetHMListByOrgId(string orgId)
        {
            var data = await _hiringManagerBusiness.GetHMListByOrgId(orgId);
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetHODListByOrgId(string orgId)
        {
            var data = await _hiringManagerBusiness.GetHODListByOrgId(orgId);
            return Json(data);
        }
    }
}