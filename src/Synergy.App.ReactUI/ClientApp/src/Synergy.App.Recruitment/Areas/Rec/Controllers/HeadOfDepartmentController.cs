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
    public class HeadOfDepartmentController : ApplicationController
    {
        private readonly IHeadOfDepartmentBusiness _headOfDepartmentBusiness;

        public HeadOfDepartmentController(IHeadOfDepartmentBusiness headOfDepartmentBusiness)
        {
            _headOfDepartmentBusiness = headOfDepartmentBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> ReadHodList([DataSourceRequest] DataSourceRequest request)
        {
            var hmmodelList = await _headOfDepartmentBusiness.GetHodList();
            var dsResult = hmmodelList.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public IActionResult CreateHOD()
        {
            return View("ManageHOD", new HeadOfDepartmentViewModel
            {
                DataAction = DataActionEnum.Create,
            });
        }

        public async Task<IActionResult> EditHOD(string Id)
        {
            var hodmodel = await _headOfDepartmentBusiness.GetSingleById(Id);
            var organizations = await _headOfDepartmentBusiness.GetList<HeadOfDepartmentOrganizationViewModel, HeadOfDepartmentOrganization>(x => x.HeadOfDepartmentId == Id);
            if (organizations != null && organizations.Count() > 0)
            {
                hodmodel.OrganizationId = organizations.Select(x => x.OrganizationId).ToArray();
            }
            if (hodmodel != null)
            {
                hodmodel.DataAction = DataActionEnum.Edit;
                return View("ManageHOD", hodmodel);
            }
            return View("ManageHOD", new HeadOfDepartmentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageHOD(HeadOfDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _headOfDepartmentBusiness.Create(model);
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
                    var result = await _headOfDepartmentBusiness.Edit(model);
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
            return View("ManageHOD", model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _headOfDepartmentBusiness.Delete(id);
            return Json(new { success = true });
        }

    }
}