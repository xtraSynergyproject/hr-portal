using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class ManpowerRequirementSummaryController : ApplicationController
    {
        private readonly IManpowerSummaryCommentBusiness _manpowerSummaryCommentBusiness;
        private readonly IManpowerRecruitmentSummaryBusiness _business;
        private readonly IUserContext _userContext;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IOrganizationDocumentBusiness _OrganizationDocumentBusiness;
        public ManpowerRequirementSummaryController(IManpowerSummaryCommentBusiness manpowerSummaryCommentBusiness,
            IManpowerRecruitmentSummaryBusiness business, IUserContext userContext, IUserRoleBusiness userRoleBusiness, IOrganizationDocumentBusiness OrganizationDocumentBusiness)
        {
            _manpowerSummaryCommentBusiness = manpowerSummaryCommentBusiness;
            _business = business;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _OrganizationDocumentBusiness = OrganizationDocumentBusiness;
        }
        public IActionResult Index(string pageId,string permissions)
        {
            ViewBag.Permissions = permissions;
            return View();
        }
        public IActionResult ActiveManpowerRecruitmentSummary(string pageId, string permissions)
        {
            ViewBag.Permissions = permissions;
            return View("_ActiveManpowerRecruitmentSummary");
        }
        public IActionResult ManpowerRequirementSummaryVersion(string summaryId)
        {
            return View("_ManpowerRequirementSummaryVersion",new ManpowerRecruitmentSummaryVersionViewModel
            {
                ManpowerRecruitmentSummaryId = summaryId

            });
        }
        public ActionResult ReadManpowerRequirementSummaryData([DataSourceRequest] DataSourceRequest request, string permission)
        {            
            var res = _business.GetManpowerRecruitmentSummaryData(permission).Result.OrderByDescending(x => x.CreatedDate);
            var dsResult = res.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public ActionResult ReadActiveManpowerRequirementSummaryData([DataSourceRequest] DataSourceRequest request)
        {
            var res = _business.GetActiveManpowerRecruitmentSummaryData().Result.OrderByDescending(x => x.CreatedDate);
            var dsResult = res.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<IActionResult> ReadManpowerRequirementSummaryVersionData([DataSourceRequest] DataSourceRequest request,string id)
        {            
            var model =await _business.GetManpowerRecruitmentSummaryVersionData(id);
           // var res = model.Result;
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public IActionResult Comment(string summaryId,string userRoleCode)
        {
            return View(new ManpowerSummaryCommentViewModel
            {                
                ManpowerRecruitmentSummaryId = summaryId,
                UserRoleCode=userRoleCode
            });
        }
        public async Task<IActionResult> ReadData([DataSourceRequest] DataSourceRequest request,string id,string userRoleCode)
        {
            //var model = _manpowerSummaryCommentBusiness.GetList(x=>x.ManpowerRecruitmentSummaryId==id);
            var model =await _business.GetManpowerSummaryCommentData(id, userRoleCode);
           // var data = model.Result;
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public IActionResult Create(string summaryId,long versionNo)
        { 
            var model = new ManpowerSummaryCommentViewModel
            {
                DataAction = DataActionEnum.Create,
                ManpowerRecruitmentSummaryId = summaryId,
                VersionNo = versionNo,                

            };
            if (_userContext.UserRoleCodes.IsNotNullAndNotEmpty() &&(_userContext.UserRoleCodes.Contains("ORG_UNIT") || _userContext.UserRoleCodes.Contains("HR") || _userContext.UserRoleCodes.Contains("PLANNING")))
            {
                var userRoleCode = _userContext.UserRoleCodes.Split(',').ToList();

                foreach (var item in userRoleCode)
                {
                    if(item== "ORG_UNIT" || item == "HR" || item == "PLANNING")
                    {
                        var userrole = _userRoleBusiness.GetSingle(x => x.Code == item).Result;
                        if (userrole.IsNotNull())
                        {
                            model.UserRoleId = userrole.Id;
                        }
                        break;
                    }
                }
                
            }
            return View("Manage", model);
        }
       
        public async Task<IActionResult> Edit(string Id)
        {
            var module = await _manpowerSummaryCommentBusiness.GetSingleById(Id);

            if (module != null)
            {

                module.DataAction = DataActionEnum.Edit;
                return View("Manage", module);
            }
            return View("Manage", new ManpowerSummaryCommentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ManpowerSummaryCommentViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _manpowerSummaryCommentBusiness.Create(model);
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
                    var result = await _manpowerSummaryCommentBusiness.Edit(model);
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
        public async Task<IActionResult> Delete(string id)
        {
            await _manpowerSummaryCommentBusiness.Delete(id);
            return View("Index", new ManpowerSummaryCommentViewModel());
        }
        public IActionResult CreateManpowerRequirement()
        {
            return View("CreateManpowerRequirement", new ManpowerRecruitmentSummaryViewModel
            {
                DataAction = DataActionEnum.Create,                
            });
        }
        public async Task<IActionResult> EditManpowerRequirement(string id)
        {
            var module = await _business.GetSingleById(id);
            //var module = await _business.GetManpowerRecruitmentSummaryCalculatedData(id);
            if (module != null)
            {
                module.DataAction = DataActionEnum.Edit;
                return View("CreateManpowerRequirement", module);
            }
            return View("CreateManpowerRequirement", new ManpowerRecruitmentSummaryViewModel());
        }
        
        public async Task<IActionResult> DeleteManpowerRequirement(string id)
        {
            var state = await _business.GetState(id);
            if(state != null)
            {
                if (state.ActionName == "Draft" || state.ActionName == null)
                {
                    await _business.Delete(id);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            
            }else
            {
                await _business.Delete(id);
                return Json(new { success = true });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> ManageManpowerRequirement(ManpowerRecruitmentSummaryViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _business.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false , errors = result.Messages });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _business.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false, errors = result.Messages });
                    }
                }
            }

            // return View("CreateManpowerRequirement", model);
            return Json(new { success = true});
        }

        [HttpGet]
        public ActionResult GetJobIdNameList()
        {
            var list = _business.GetJobIdNameList().Result;
            return Json(list);
        }
        [HttpGet]
        public ActionResult GetOrgIdNameList()
        {
            var list = _business.GetOrganizationIdNameList().Result;
            return Json(list);
        }

    

        public async Task<IActionResult> ReadManpowerUniqueJobData([DataSourceRequest] DataSourceRequest request)
        {
            var res = await _business.GetManpowerUniqueJobData();
            res = res.OrderBy(x => x.JobName).ToList();
            var dsResult = res.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<IActionResult> GetJobDescriptionTaskList([DataSourceRequest] DataSourceRequest request, string manpowerId)
        {
            var result = await _business.GetJobDescriptionTaskList(manpowerId);
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public IActionResult Task(string manpowerId)
        {
            var model = new RecTaskViewModel { ReferenceTypeId = manpowerId };
            return View("_Tasks",model);
        }


        public IActionResult organizationDocument(string id)
        {
            var module = new OrganizationDocumentsViewModel();
            module.OrganizationId = id;
            module.Id = null;
            module.Ids = null;
            ////var module = await _business.GetManpowerRecruitmentSummaryCalculatedData(id);
            //if (module != null)
            //{
            //    module.DataAction = DataActionEnum.Edit;
            //    return View("ManageOrganizationDocuments", module);
            //}
            return View("ManageOrganizationDocuments", module);
        }

        [HttpPost]


        public async Task<IActionResult> ManageOrganizationDocuments(OrganizationDocumentsViewModel Model)
        {

            if (Model.Ids.IsNullOrEmpty())
            {

                var maxvl = await _OrganizationDocumentBusiness.GetList(x=>x.OrganizationId==Model.OrganizationId);
                var maxv = maxvl.Max(x => x.Version);
                if (maxv != null)
                {
                    Model.Version = Convert.ToInt32(maxv) + 1;
                }
                else { Model.Version = 0; }

                Model.DataAction = DataActionEnum.Create;
                var result = await _OrganizationDocumentBusiness.Create(Model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, message = "Save" });

                }
                else
                {
                    return Json(new { success = false, errors = result.Messages });
                }

            }
            else 
            {

               

                Model.DataAction = DataActionEnum.Edit;
                Model.Id = Model.Ids;
                var result = await _OrganizationDocumentBusiness.Edit(Model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, message="Update" });

                }
                else
                {
                    return Json(new { success = false, errors = result.Messages });
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckManpowerRequirement(string jobId)
        {
            var rec = await _business.GetList(x => x.JobId == jobId);

            if (rec.Count>0)
            {               
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        public async Task<IActionResult> GetOrganizationDocuments([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var result = await _OrganizationDocumentBusiness.GetOrgListById(Id);
            //var data = result.OrderByDescending(x => x.Version);
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        [HttpPost]

        public async Task<IActionResult> DeleteOrganizationDocument(string Id)
        {

            await _OrganizationDocumentBusiness.Delete(Id);
            return Json("True");
        }


    }
}
