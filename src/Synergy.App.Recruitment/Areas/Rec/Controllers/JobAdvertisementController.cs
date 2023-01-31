using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class JobAdvertisementController : Controller
    {
        private readonly IJobAdvertisementBusiness _jobAdvertisementBusiness;
        private readonly IJobAdvertisementTrackBusiness _jobAdvertisementTrackBusiness;
        private readonly IJobCriteriaBusiness _jobCriteriaBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IListOfValueBusiness _ListOfValueBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly ICandidateProfileBusiness _candidateProfileBusiness;
        private readonly IRecTaskBusiness _taskBusiness;
        public IPageBusiness _pageBusiness;
        private readonly IJobDescriptionBusiness _jobDescriptionBusiness;
        private readonly IWebHelper _webApi;

        public JobAdvertisementController(IJobAdvertisementBusiness jobAdvertisementBusiness, IJobCriteriaBusiness jobCriteriaBusiness,
            IApplicationBusiness applicationBusiness, IListOfValueBusiness listOfValueBusiness, IJobAdvertisementTrackBusiness jobAdvertisementTrackBusiness,
            IUserContext userContext, IUserRoleBusiness userRoleBusiness, IMasterBusiness masterBusiness
            , IPageBusiness pageBusiness, ICandidateProfileBusiness candidateProfileBusiness, IRecTaskBusiness taskBusiness
            , IJobDescriptionBusiness jobDescriptionBusiness,
            IWebHelper webApi)
        {
            _jobAdvertisementBusiness = jobAdvertisementBusiness;
            _jobAdvertisementTrackBusiness = jobAdvertisementTrackBusiness;
            _jobCriteriaBusiness = jobCriteriaBusiness;
            _applicationBusiness = applicationBusiness;
            _ListOfValueBusiness = listOfValueBusiness;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _masterBusiness = masterBusiness;
            _pageBusiness = pageBusiness;
            _candidateProfileBusiness = candidateProfileBusiness;
            _taskBusiness = taskBusiness;
            _jobDescriptionBusiness = jobDescriptionBusiness;
            _webApi = webApi;
        }
        public async Task<IActionResult> Index(string summaryId, string id, string jobId,string orgId,long? requirement, string permission,bool isView=false, string layout=null)
        {
            var res = new JobAdvertisementViewModel();
            //var master = new IdNameViewModel();
            //master = await _masterBusiness.GetJobNameById(jobId);
            //res.JobName = master.Name;
            ViewBag.Permission = permission;
            ViewBag.JobCriterias = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            var actionresult = await _applicationBusiness.GetListOfValueByType("LOV_JOBADVTACTION");
            var manpowertype = await _jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(jobId);
            if (manpowertype != null)
            {
                res.ManpowerType = manpowertype.ManpowerTypeName;
            }

            var approvalid = "";
            var submitid = "";
            var draftid = "";
            foreach (var a in actionresult)
            {
                if (a.Name == "Approve")
                {
                    approvalid = a.Id;
                }
                if (a.Name == "Submit")
                {
                    submitid = a.Id;
                }
                if (a.Name == "Draft")
                {
                    draftid = a.Id;
                }
            }
            res.ApprovalId = approvalid;
            res.SubmitId = submitid;
            res.DraftId = draftid;
            //res.ManpowerRecruitmentSummaryId = summaryId;
            res.JobId = jobId;
            //res.OrganizationId = orgId;
           
            var temp = _userContext.UserRoleCodes.Split(",");
            res.UserRoleCodes = temp;
            if (requirement.IsNotNull())
            {
                res.NoOfPosition =  requirement;
            }
            res.DataAction = DataActionEnum.Create;
            var jd = await _jobDescriptionBusiness.GetSingle(x => x.JobId == jobId);
            if (jd != null)
            {
                res.Description = jd.Description;
                res.Responsibilities = jd.Responsibilities;
                res.Experience = jd.Experience;
                res.Qualification = jd.Qualification;
                res.JobCategoryId = jd.JobCategoryId;
                res.ShowJobDesc = jd.Id;
            }
            if (layout.IsNotNull())
            {

                ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";

            }
            if (id.IsNotNull())
            {
                var model = await _jobAdvertisementBusiness.GetSingleById(id);

                if (manpowertype != null)
                {
                    model.ManpowerType = manpowertype.ManpowerTypeName;
                }
                model.DataAction = DataActionEnum.Edit;
                model.ApprovalId = approvalid;
                model.SubmitId = submitid;
                model.DraftId = draftid;
                model.UserRoleCodes = temp;
                model.Layout = layout;
                model.ShowJobDesc = null;
                model.IsView = isView;
                model.AgencyIds = model.AgencyId.ToList();
                return View(model);
            }
            res.Status = StatusEnum.Active;
            res.IsView = isView;
            return View(res);
        }

        public ActionResult JobAdvertisement(string pageId,string permissions)
        {
            ViewBag.Permissions = permissions;
            return View();
        }

        public async Task<IActionResult> ViewJobAdvertisement(string id,string layout)
        {
            
            var model = await _jobAdvertisementBusiness.GetSingle(x=>x.JobId==id);
            if (model != null)
            {
                var manpowertype = await _jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                if (manpowertype != null)
                {
                    model.ManpowerType = manpowertype.ManpowerTypeName;
                }
            }
            if (layout.IsNotNullAndNotEmpty())
            {

                ViewBag.Layout = "~/Views/Shared/_PopupLayout.cshtml";

            }
            return View("ViewJobAdvertisement", model);
        }
        public async Task<IActionResult> ViewJobAdvCandidate(string applicationId)
        {
            var model = new JobAdvertisementViewModel();
            var appmodel = await _applicationBusiness.GetApplicationForJobAdv(applicationId);
            if (appmodel.IsNotNull())
            {

                //model = await _jobAdvertisementBusiness.GetSingle(x=>x.JobId==appmodel.JobId && x.Status == StatusEnum.Active);
                var result = await _jobAdvertisementBusiness.GetList(x => x.JobId == appmodel.JobId);
                model = result.OrderByDescending(x => x.CreatedDate).FirstOrDefault();                

                //model.OrganizationId = appmodel.OrganizationId;
                model.HiringManagerId = appmodel.HiringManagerId;
                model.RecruiterId = appmodel.RecruiterId;

                var manpowertype = await _jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
                if (manpowertype != null)
                {
                    model.ManpowerType = manpowertype.ManpowerTypeName;
                }
            }

            return View("ViewJobAdvCandidate", model);
        }

        public async Task<IActionResult> ViewEditJobAdvertisement(string id)
        {
            var model = await _jobAdvertisementBusiness.GetSingleById(id);
            var temp = _userContext.UserRoleCodes.Split(",");
            var actionresult = await _applicationBusiness.GetListOfValueByType("LOV_JOBADVTACTION");
            //var approvalid = "";
            var submitid = "";
            // var draftid = "";
            model.UserRoleCodes = temp;
            foreach (var a in actionresult)
            {
                if (a.Name == "Submit")
                {
                    submitid = a.Id;
                }

            }
            //if (temp.Contains("HR")&&model.ActionId==submitid)
            //{
            //    return RedirectToAction("JobEditAdvertisement", model);
            //}
            return RedirectToAction("index",new { id=model.Id, jobId = model.JobId});
        }

        public async Task<IActionResult> JobDetails(string jobAdvId, string pageId, bool isDirectLogin= false)
        {
            var model = await _webApi.GetApiAsync<JobAdvertisementViewModel>("Recruitment/Query/GetJobNameById?jobId="+ jobAdvId);//_jobAdvertisementBusiness.GetNameById(jobAdvId);
            var manpowertype = await _webApi.GetApiAsync<JobAdvertisementViewModel>("Recruitment/Query/GetJobIdNameListByJobAdvertisement?jobId=" + model.JobId);//_jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(model.JobId);
            if (manpowertype != null)
            {
                model.ManpowerType = manpowertype.ManpowerTypeName;
            }
            if (Request.HttpContext.User.Identity.IsAuthenticated == true && !_userContext.IsGuestUser)
            {
                var candidate = await _candidateProfileBusiness.IsCandidateProfileFilled();
                if (isDirectLogin)
                {
                    if (candidate.Item2 == false)
                    {
                        return RedirectToAction("Index", "CandidateProfile", new { area = "Recruitment", jobAdvId = jobAdvId });
                    }
                    else if (candidate.Item2 == true)
                    {
                        return RedirectToAction("ApplyJob", "CandidateProfile", new { area = "Recruitment", jobAdvId = jobAdvId, candidateProfileId = candidate.Item1.Id });
                    }
                }
                if (candidate != null && candidate.Item1 != null)
                {
                    var alreadyApplied = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == candidate.Item1.Id && x.JobAdvertisementId == jobAdvId);
                    model.AlreadyApplied = alreadyApplied != null;
                    model.CandidateId = candidate.Item1.Id;
                    model.IsCandidateDetailsFilled = candidate.Item2;
                    if (candidate.Item1.BookMarks != null && candidate.Item1.BookMarks.Contains(jobAdvId))
                    {
                        model.IsBookmarked = true;
                    }
                    else
                    {
                        model.IsBookmarked = false;
                    }
                }
            } 
            ViewBag.Page = await _webApi.GetApiAsync<PageViewModel>("Recruitment/Query/GetPageForExecution?PageId=" + pageId); //await _pageBusiness.GetPageForExecution(pageId);
            return View(model);
        }
        public IActionResult CreateJobCriteria(string JobadvtId)
        {
            var res = new JobCriteriaViewModel();
            // res.JobAdvertisementId = JobadvtId;
            res.DataAction = DataActionEnum.Create;
            res.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            //return View("_ManageJobAdvertisement", new JobCriteriaViewModel
            //{
            //    DataAction = DataActionEnum.Create,
            //    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            //});
            return View("_ManageJobAdvertisement", res);
        }

        public async Task<IActionResult> EditJobCriteria(string Id)
        {
            var member = await _jobCriteriaBusiness.GetSingleById(Id);

            if (member != null)
            {

                member.DataAction = DataActionEnum.Edit;
                return View("_ManageJobAdvertisement", member);
            }
            return View("_ManageJobAdvertisement", new JobCriteriaViewModel());
        }

        public async Task<IActionResult> ReadJobCriteriaData([DataSourceRequest] DataSourceRequest request, string jobadvtid,string jobdesc)
        {
            var data = new List<JobCriteriaViewModel>();
            ViewBag.JobCriterias = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            if (jobadvtid.IsNotNullAndNotEmpty()&& jobdesc.IsNullOrEmpty())
            {              
                    var model = await _jobAdvertisementBusiness.GetJobCriteriaList("Criteria", jobadvtid);
                    data = model.ToList();
            }
           else
           {
                    var model = await _jobDescriptionBusiness.GetJobDescCriteriaList("Criteria", jobdesc);
                    var result = model.ToList().ToDataSourceResult(request);
                    return Json(result);
           }
            //  var data = model.Result.ToList();
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<IActionResult> DeleteCriteria([DataSourceRequest] DataSourceRequest request, string Id)
        {
            if (Id != null)
            {
                await _jobCriteriaBusiness.Delete(Id);
            }
            return Json(new { success=true});
        }

        public async Task<IActionResult> ManageBookmark(string jobAdvId)
        {
            List<string> list = new List<string>();
            var model = await _candidateProfileBusiness.GetSingle(x=>x.UserId == _userContext.UserId);

            bool exist = model.BookMarks.Contains(jobAdvId);
            if (exist)
            {
                list = model.BookMarks.ToList();
                list.Remove(jobAdvId);
                model.BookMarks = list.ToArray();
                await _candidateProfileBusiness.Edit(model);
                return Json(new { success = false });
            }
            else
            {
                list = model.BookMarks.ToList();
                list.Add(jobAdvId);
                model.BookMarks = list.ToArray();
                await _candidateProfileBusiness.Edit(model);
                return Json(new { success = true });
            }           
        }


        public IActionResult CreateJobSkills(string JobadvtId)
        {
            var res = new JobCriteriaViewModel();
            //  res.JobAdvertisementId = JobadvtId;
            res.DataAction = DataActionEnum.Create;
            res.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            //return View("_ManageJobAdvertisement", new JobCriteriaViewModel
            //{
            //    DataAction = DataActionEnum.Create,
            //    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            //});
            return View("JobAdvertisementInActive", res);
        }

        public async Task<IActionResult> EditJobSkills(string Id)
        {
            var member = await _jobCriteriaBusiness.GetSingleById(Id);

            if (member != null)
            {

                member.DataAction = DataActionEnum.Edit;
                return View("_ManageJobAdvertisement", member);
            }
            return View("_ManageJobAdvertisement", new JobCriteriaViewModel());
        }

        public async Task<IActionResult> ReadSkillsData([DataSourceRequest] DataSourceRequest request, string jobadvtid, string jobdesc)
        {
            //var model = _jobCriteriaBusiness.GetList(x => x.Type == "Skills" && x.JobAdvertisementId == jobadvtid);
            //var data = model.Result.ToList();

            //var dsResult = data.ToDataSourceResult(request);
            //return Json(dsResult);
           ViewBag.JobCriterias = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            var data = new List<JobCriteriaViewModel>();
            if (jobadvtid.IsNotNullAndNotEmpty()&& jobdesc.IsNullOrEmpty())
            {
 
                    var model = await _jobAdvertisementBusiness.GetJobCriteriaList("Skills", jobadvtid);
                data = model.ToList();
              }
            else
               {
                var model = await _jobDescriptionBusiness.GetJobDescCriteriaList("Skills", jobdesc);
                var result = model.ToList().ToDataSourceResult(request);
                return Json(result);

               }


            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);


        }

        public IActionResult CreateInfo(string JobadvtId)
        {
            var res = new JobCriteriaViewModel();
            //  res.JobAdvertisementId = JobadvtId;
            res.DataAction = DataActionEnum.Create;
            res.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            //return View("_ManageJobAdvertisement", new JobCriteriaViewModel
            //{
            //    DataAction = DataActionEnum.Create,
            //    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            //});
            return View("JobAdvertisementActive", res);
        }

        public async Task<IActionResult> EditInfo(string Id)
        {
            var member = await _jobCriteriaBusiness.GetSingleById(Id);

            if (member != null)
            {

                member.DataAction = DataActionEnum.Edit;
                return View("JobAdvertisementActive", member);
            }
            return View("JobAdvertisementActive", new JobCriteriaViewModel());
        }

        public IActionResult ViewJobAdvertisementI(string jobId,string permission)
        {
            var res = new JobAdvertisementViewModel();
            res.JobId = jobId;
            ViewBag.Permissions = permission;
            return View("JobAdvertisementInActive", res);
        }

        public IActionResult ViewJobAdvertisementA(string jobId,string orgid)
        {
            var res = new JobAdvertisementViewModel();
            res.JobId = jobId;
            return View("JobAdvertisementActive", res);
        }


        public async Task<IActionResult> ReadInfoData([DataSourceRequest] DataSourceRequest request, string jobadvtid, string jobdesc)
        {
            //var model = _jobCriteriaBusiness.GetList(x => x.Type == "OtherInformation");
            //var data = model.Result.ToList();

            //var dsResult = data.ToDataSourceResult(request);
            //return Json(dsResult);
            ViewBag.JobCriterias = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");

            var data = new List<JobCriteriaViewModel>();
            if (jobadvtid.IsNotNullAndNotEmpty()&& jobdesc.IsNullOrEmpty())
            {

                    var model = await _jobAdvertisementBusiness.GetJobCriteriaList("OtherInformation", jobadvtid);
                    data = model.ToList();
                }
                else
                {
                    var model = await _jobDescriptionBusiness.GetJobDescCriteriaList("OtherInformation", jobdesc);
                    var result = model.ToList().ToDataSourceResult(request);
                    return Json(result);
                }
              


            //  var data = model.Result.ToList();

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<ActionResult> ReadJobadvtDataA([DataSourceRequest] DataSourceRequest request, string jobid)
        {
            var temp = _userContext.UserRoleCodes.Split(",");
            var roleid = "";

            if (temp.Contains("HR"))
            {
                var hrrole = _userRoleBusiness.GetSingle(x => x.Code == "HR");
                roleid = hrrole.Result.Id;
            }
            else if (temp.Contains("CORPCOMP"))
            {
                var corprole = _userRoleBusiness.GetSingle(x => x.Code == "CORPCOMP");
                roleid = corprole.Result.Id;
            }

            var data = new List<JobAdvertisementViewModel>();
            if (jobid.IsNotNullAndNotEmpty() && roleid.IsNotNullAndNotEmpty())
            {
               // var model = await _jobAdvertisementBusiness.GetList(x => x.JobId == jobid && x.RoleId == roleid && x.Status==StatusEnum.Active);
                var model = await _jobAdvertisementBusiness.GetJobAdvertisement(jobid,roleid,StatusEnum.Active);
                data = model.ToList();
            }


            //  var data = model.Result.ToList();

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<ActionResult> ReadJobadvtDataI([DataSourceRequest] DataSourceRequest request, string jobid)
        {
            var temp = _userContext.UserRoleCodes.Split(",");
            var roleid = "";

            //if (temp.Contains("HR"))
            //{
            //    var hrrole = _userRoleBusiness.GetSingle(x => x.Code == "HR");
            //    roleid = hrrole.Result.Id;
            //}
            //else if (temp.Contains("CORPCOMP"))
            //{
            //    var corprole = _userRoleBusiness.GetSingle(x => x.Code == "CORPCOMP");
            //    roleid = corprole.Result.Id;
            //}

            var data = new List<JobAdvertisementViewModel>();
            if (jobid.IsNotNullAndNotEmpty())
            {
                // var model = await _jobAdvertisementBusiness.GetList(x => x.JobId == jobid && x.RoleId == roleid && x.Status == StatusEnum.Inactive);
                var model = await _jobAdvertisementBusiness.GetJobAdvertisement(jobid, roleid, StatusEnum.Inactive);
                data = model.ToList();
            }


            //  var data = model.Result.ToList();

            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }


        //public async Task<IActionResult> DeleteCriteria(string Id)
        //{
        //    await _jobCriteriaBusiness.Delete(Id);
        //    return Json(true);
        //}

        public async Task<ActionResult> GetListOfValue(string type)
        {
            var result = await _applicationBusiness.GetListOfValueByType(type);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJobCriteria(JobAdvertisementViewModel model)
        {
            var actionresult = await _applicationBusiness.GetListOfValueByType("LOV_JOBADVTACTION");
            var draftactionid = "";
            var submitactionid = "";
            var approveactionid = "";
            foreach (var a in actionresult)
            {
                if (a.Name == "Approve")
                {
                    approveactionid = a.Id;
                }
                if (a.Name == "Submit")
                {
                    submitactionid = a.Id;
                }
                if (a.Name == "Draft")
                {
                    draftactionid = a.Id;
                }
            }
            var temp = _userContext.UserRoleCodes.Split(",");
            var hrroliid = "";
            var corproliid = "";
            if (temp.Contains("HR"))
            {
                var hrrole = await _userRoleBusiness.GetSingle(x => x.Code == "HR");
                hrroliid = hrrole.Id;
            }
            model.RoleId = hrroliid;


            if (model.SaveType == "APPROVE")
            {
                if (temp.Contains("CORPCOMP"))
                {
                    var corprole = await _userRoleBusiness.GetSingle(x => x.Code == "CORPCOMP");
                    corproliid = corprole.Id;
                    model.ApprovedDate = DateTime.Now;
                }
                model.RoleId = corproliid;
            }

            if (model.Status==0)
            {
                model.Status = StatusEnum.Active;
            }

            if (ModelState.IsValid)
            {
                // model.DataAction = DataActionEnum.Create;
                if (model.AgencyIds.IsNotNull())
                {
                    model.AgencyId = model.AgencyIds.ToArray();
                }
                if (model.DataAction == DataActionEnum.Create)
                {
                   
                    // model.Type = "Criteria";
                    if (model.SaveType == "DRAFT")
                    {
                        model.RoleId = hrroliid;
                        if (model.SaveType != "")
                        {
                            var m = await _applicationBusiness.GetListOfValueAction("LOV_JOBADVTACTION", model.SaveType);
                            model.ActionId = m.FirstOrDefault().Id;
                            model.ActionName = m.FirstOrDefault().Name;
                        }
                        
                        var result = await _jobAdvertisementBusiness.Create(model);
                        if (result.IsSuccess)
                        {
                            if (model.JobCriteria.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                               
                                var jc = new JobCriteriaViewModel();
                                foreach (var a in jobcriteria)
                                {
                                    a.JobAdvertisementId = result.Item.Id;
                                    a.Type = "Criteria";
                                    jc = a;
                                    await _jobCriteriaBusiness.Create(jc);
                                }
                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                var jc = new JobCriteriaViewModel();
                                foreach (var a in jobcriteria)
                                {
                                    a.JobAdvertisementId = result.Item.Id;
                                    a.Type = "Skills";
                                    jc = a;
                                    await _jobCriteriaBusiness.Create(jc);
                                }
                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                var jc = new JobCriteriaViewModel();
                                foreach (var a in jobcriteria)
                                {
                                    a.JobAdvertisementId = result.Item.Id;
                                    a.Type = "OtherInformation";
                                    jc = a;
                                    await _jobCriteriaBusiness.Create(jc);
                                }
                            }


                            // jc=await _jobCriteriaBusiness.Create(jobcriteria);
                            ViewBag.Success = true;
                            // return RedirectToAction("membergrouplist");
                            //return PopupRedirect("Portal created successfully");
                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }
                    }

                    else if (model.SaveType == "APPROVE")
                    {
                        if (model.Id.IsNullOrEmpty())
                        {
                            model.RoleId = hrroliid;
                            if (model.SaveType != "")
                            {
                                var m = await _applicationBusiness.GetListOfValueAction("LOV_JOBADVTACTION", model.SaveType);
                                model.ActionId = m.FirstOrDefault().Id;
                                model.ActionName = m.FirstOrDefault().Name;
                            }
                            var result = await _jobAdvertisementBusiness.Create(model);

                            if (result.IsSuccess)
                            {

                                var m = new JobAdvertisementTrackViewModel();
                                m.Description = model.Description;
                                m.Qualification = model.Qualification;
                                m.NoOfPosition = model.NoOfPosition;
                                m.NationalityId = model.NationalityId;
                                m.Experience = model.Experience;
                                m.JobCategory = model.JobCategory;
                                m.NeededDate = model.NeededDate;
                                m.ExpiryDate = model.ExpiryDate;
                                m.JobName = model.JobName;
                                m.LocationId = model.LocationId;
                                m.JobAdvertisementId = model.Id;
                                m.RoleId = model.RoleId;
                                await _jobAdvertisementTrackBusiness.Create(m);


                                if (model.JobCriteria.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                    var jc = new JobCriteriaViewModel();
                                    foreach (var a in jobcriteria)
                                    {
                                        a.JobAdvertisementId = result.Item.Id;
                                        a.Type = "Criteria";
                                        jc = a;
                                        await _jobCriteriaBusiness.Create(jc);
                                    }
                                }

                                if (model.Skills.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                    var jc = new JobCriteriaViewModel();
                                    foreach (var a in jobcriteria)
                                    {
                                        a.JobAdvertisementId = result.Item.Id;
                                        a.Type = "Skills";
                                        jc = a;
                                        await _jobCriteriaBusiness.Create(jc);
                                    }
                                }
                                if (model.OtherInformation.IsNotNull())
                                {
                                    var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                    var jc = new JobCriteriaViewModel();
                                    foreach (var a in jobcriteria)
                                    {
                                        a.JobAdvertisementId = result.Item.Id;
                                        a.Type = "OtherInformation";
                                        jc = a;
                                        await _jobCriteriaBusiness.Create(jc);
                                    }
                                }

                                // jc=await _jobCriteriaBusiness.Create(jobcriteria);
                                await _taskBusiness.AssignTaskForJobAdvertisement(model.Id,model.JobName,model.CreatedDate,null);
                                ViewBag.Success = true;
                                // return RedirectToAction("membergrouplist");
                                //return PopupRedirect("Portal created successfully");
                            }
                            else
                            {
                                ModelState.AddModelErrors(result.Messages);
                            }
                        }
                    }


                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    if (model.SaveType == "DRAFT" && model.ActionId != submitactionid)
                    {
                        if (model.SaveType != "")
                        {
                            var m = await _applicationBusiness.GetListOfValueAction("LOV_JOBADVTACTION", model.SaveType);
                            model.ActionId = m.FirstOrDefault().Id;
                            model.ActionName = m.FirstOrDefault().Name;
                        }
                        var result = await _jobAdvertisementBusiness.Edit(model);
                        if (result.IsSuccess)
                        {

                            if (model.JobCriteria.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                var existingjobcriteria = await _jobAdvertisementBusiness.GetJobCriteriaList("Criteria", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {
                                        
                                            await _jobCriteriaBusiness.Delete(p.Id);
                                        
                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    foreach (var p in jobcriteria)
                                    {
                                       
                                            var res = await _jobCriteriaBusiness.Create(new JobCriteriaViewModel
                                            {
                                                JobAdvertisementId = result.Item.Id,
                                                Type = "Criteria",
                                                Weightage=p.Weightage,
                                                CriteriaType=p.CriteriaType,
                                                Criteria=p.Criteria
                                        });
                                        
                                    }
                                }


                                //var jc = new JobCriteriaViewModel();
                                //foreach (var a in jobcriteria)
                                //{
                                //    foreach(var ex in existingjobcriteria)
                                //    {
                                //        if (ex.Id == a.Id)
                                //        {
                                //            a.JobAdvertisementId = result.Item.Id;
                                //            a.Type = "Criteria";
                                //            jc = a;
                                //            if (jc.Id != "")
                                //            {
                                //                await _jobCriteriaBusiness.Edit(jc);
                                //            }
                                //            else
                                //            {
                                //                await _jobCriteriaBusiness.Create(jc);
                                //            }

                                //        }
                                //        else
                                //        {
                                //            await _jobCriteriaBusiness.Delete(ex.Id);
                                //        }
                                //    }
                                   
                                //}
                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                var existingjobcriteria = await _jobAdvertisementBusiness.GetJobCriteriaList("Skills", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {

                                        await _jobCriteriaBusiness.Delete(p.Id);

                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    foreach (var p in jobcriteria)
                                    {

                                        var res = await _jobCriteriaBusiness.Create(new JobCriteriaViewModel
                                        {
                                            JobAdvertisementId = result.Item.Id,
                                            Type = "Skills",
                                            Weightage = p.Weightage,
                                            CriteriaType = p.CriteriaType,
                                            Criteria = p.Criteria
                                        });

                                    }
                                }

                                //var jc = new JobCriteriaViewModel();
                                //foreach (var a in jobcriteria)
                                //{
                                //    a.JobAdvertisementId = result.Item.Id;
                                //    a.Type = "Skills";
                                //    jc = a;
                                //    if (jc.Id != "")
                                //    {
                                //        await _jobCriteriaBusiness.Edit(jc);
                                //    }
                                //    else
                                //    {
                                //        await _jobCriteriaBusiness.Create(jc);
                                //    }
                                //}
                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                var existingjobcriteria = await _jobAdvertisementBusiness.GetJobCriteriaList("OtherInformation", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {

                                        await _jobCriteriaBusiness.Delete(p.Id);

                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    foreach (var p in jobcriteria)
                                    {

                                        var res = await _jobCriteriaBusiness.Create(new JobCriteriaViewModel
                                        {
                                            JobAdvertisementId = result.Item.Id,
                                            Type = "OtherInformation",
                                            Weightage = p.Weightage,
                                            CriteriaType = p.CriteriaType,
                                            Criteria = p.Criteria,
                                            ListOfValueTypeId=p.ListOfValueTypeId
                                        });

                                    }
                                }

                                //var jc = new JobCriteriaViewModel();
                                //foreach (var a in jobcriteria)
                                //{
                                //    a.JobAdvertisementId = result.Item.Id;
                                //    a.Type = "OtherInformation";
                                //    jc = a;
                                //    if (jc.Id != "")
                                //    {
                                //        await _jobCriteriaBusiness.Edit(jc);
                                //    }
                                //    else
                                //    {
                                //        await _jobCriteriaBusiness.Create(jc);
                                //    }
                                //}
                            }

                            // jc=await _jobCriteriaBusiness.Create(jobcriteria);
                            ViewBag.Success = true;
                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }



                    }

                    else if (model.SaveType == "APPROVE")
                    {

                        if (model.SaveType != "")
                        {
                            var m = await _applicationBusiness.GetListOfValueAction("LOV_JOBADVTACTION", model.SaveType);
                            model.ActionId = m.FirstOrDefault().Id;
                            model.ActionName = m.FirstOrDefault().Name;
                        }
                        var result = await _jobAdvertisementBusiness.Edit(model);
                        if (result.IsSuccess)
                        {

                            var m = new JobAdvertisementTrackViewModel();
                            m.Description = model.Description;
                            m.Qualification = model.Qualification;
                            m.NoOfPosition = model.NoOfPosition;
                            m.NationalityId = model.NationalityId;
                            m.Experience = model.Experience;
                            m.JobCategory = model.JobCategory;
                            m.NeededDate = model.NeededDate;
                            m.ExpiryDate = model.ExpiryDate;
                            m.JobName = model.JobName;
                            m.LocationId = model.LocationId;
                            m.JobAdvertisementId = model.Id;
                            m.RoleId = model.RoleId;
                            await _jobAdvertisementTrackBusiness.Create(m);

                            if (model.JobCriteria.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                                var existingjobcriteria = await _jobAdvertisementBusiness.GetJobCriteriaList("Criteria", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {

                                        await _jobCriteriaBusiness.Delete(p.Id);

                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    foreach (var p in jobcriteria)
                                    {

                                        var res = await _jobCriteriaBusiness.Create(new JobCriteriaViewModel
                                        {
                                            JobAdvertisementId = result.Item.Id,
                                            Type = "Criteria",
                                            Weightage = p.Weightage,
                                            CriteriaType = p.CriteriaType,
                                            Criteria = p.Criteria
                                        });

                                    }
                                }


                                //var jc = new JobCriteriaViewModel();
                                //foreach (var a in jobcriteria)
                                //{
                                //    foreach(var ex in existingjobcriteria)
                                //    {
                                //        if (ex.Id == a.Id)
                                //        {
                                //            a.JobAdvertisementId = result.Item.Id;
                                //            a.Type = "Criteria";
                                //            jc = a;
                                //            if (jc.Id != "")
                                //            {
                                //                await _jobCriteriaBusiness.Edit(jc);
                                //            }
                                //            else
                                //            {
                                //                await _jobCriteriaBusiness.Create(jc);
                                //            }

                                //        }
                                //        else
                                //        {
                                //            await _jobCriteriaBusiness.Delete(ex.Id);
                                //        }
                                //    }

                                //}
                            }

                            if (model.Skills.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                                var existingjobcriteria = await _jobAdvertisementBusiness.GetJobCriteriaList("Skills", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {

                                        await _jobCriteriaBusiness.Delete(p.Id);

                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    foreach (var p in jobcriteria)
                                    {

                                        var res = await _jobCriteriaBusiness.Create(new JobCriteriaViewModel
                                        {
                                            JobAdvertisementId = result.Item.Id,
                                            Type = "Skills",
                                            Weightage = p.Weightage,
                                            CriteriaType = p.CriteriaType,
                                            Criteria = p.Criteria
                                        });

                                    }
                                }

                                //var jc = new JobCriteriaViewModel();
                                //foreach (var a in jobcriteria)
                                //{
                                //    a.JobAdvertisementId = result.Item.Id;
                                //    a.Type = "Skills";
                                //    jc = a;
                                //    if (jc.Id != "")
                                //    {
                                //        await _jobCriteriaBusiness.Edit(jc);
                                //    }
                                //    else
                                //    {
                                //        await _jobCriteriaBusiness.Create(jc);
                                //    }
                                //}
                            }
                            if (model.OtherInformation.IsNotNull())
                            {
                                var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                                var existingjobcriteria = await _jobAdvertisementBusiness.GetJobCriteriaList("OtherInformation", model.Id);
                                if (existingjobcriteria.IsNotNull())
                                {
                                    foreach (var p in existingjobcriteria)
                                    {

                                        await _jobCriteriaBusiness.Delete(p.Id);

                                    }
                                }
                                if (jobcriteria.IsNotNull())
                                {
                                    foreach (var p in jobcriteria)
                                    {

                                        var res = await _jobCriteriaBusiness.Create(new JobCriteriaViewModel
                                        {
                                            JobAdvertisementId = result.Item.Id,
                                            Type = "OtherInformation",
                                            Weightage = p.Weightage,
                                            CriteriaType = p.CriteriaType,
                                            Criteria = p.Criteria,
                                            ListOfValueTypeId = p.ListOfValueTypeId
                                        });

                                    }
                                }

   
                            }


                            await _taskBusiness.AssignTaskForJobAdvertisement(model.Id, model.JobName, model.CreatedDate, null);
                            // jc=await _jobCriteriaBusiness.Create(jobcriteria);
                            ViewBag.Success = true;
                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }

                    }

                    //else if (model.SaveType == "SUBMIT")
                    //{
                    //    if (model.SaveType != "")
                    //    {
                    //        var m = await _applicationBusiness.GetListOfValueAction("LOV_JOBADVTACTION", model.SaveType);
                    //        model.ActionId = m.FirstOrDefault().Id;
                    //        model.ActionName = m.FirstOrDefault().Name;
                    //    }
                    //    var result = await _jobAdvertisementBusiness.Edit(model);


                    //    if (result.IsSuccess)
                    //    {

                    //        var m = new JobAdvertisementTrackViewModel();
                    //        m.Description = model.Description;
                    //        m.Qualification = model.Qualification;
                    //        m.NoOfPosition = model.NoOfPosition;
                    //        m.NationalityId = model.NationalityId;
                    //        m.Experience = model.Experience;
                    //        m.JobCategory = model.JobCategory;
                    //        m.NeededDate = model.NeededDate;
                    //        m.ExpiryDate = model.ExpiryDate;
                    //        m.JobName = model.JobName;
                    //        m.LocationId = model.LocationId;
                    //        m.JobAdvertisementId = model.Id;
                    //        m.RoleId = model.RoleId;
                    //        await _jobAdvertisementTrackBusiness.Create(m);
                    //        if (model.JobCriteria.IsNotNull())
                    //        {
                    //            var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.JobCriteria);
                    //            var jc = new JobCriteriaViewModel();
                    //            foreach (var a in jobcriteria)
                    //            {
                    //                a.JobAdvertisementId = result.Item.Id;
                    //                a.Type = "Criteria";
                    //                jc = a;
                    //                if (jc.Id != "")
                    //                {
                    //                    await _jobCriteriaBusiness.Edit(jc);
                    //                }
                    //                else
                    //                {
                    //                    await _jobCriteriaBusiness.Create(jc);
                    //                }
                    //            }
                    //        }

                    //        if (model.Skills.IsNotNull())
                    //        {
                    //            var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.Skills);
                    //            var jc = new JobCriteriaViewModel();
                    //            foreach (var a in jobcriteria)
                    //            {
                    //                a.JobAdvertisementId = result.Item.Id;
                    //                a.Type = "Skills";
                    //                jc = a;
                    //                if (jc.Id != "")
                    //                {
                    //                    await _jobCriteriaBusiness.Edit(jc);
                    //                }
                    //                else
                    //                {
                    //                    await _jobCriteriaBusiness.Create(jc);
                    //                }
                    //            }
                    //        }
                    //        if (model.OtherInformation.IsNotNull())
                    //        {
                    //            var jobcriteria = JsonConvert.DeserializeObject<List<JobCriteriaViewModel>>(model.OtherInformation);
                    //            var jc = new JobCriteriaViewModel();
                    //            foreach (var a in jobcriteria)
                    //            {
                    //                a.JobAdvertisementId = result.Item.Id;
                    //                a.Type = "OtherInformation";
                    //                jc = a;
                    //                if (jc.Id != "")
                    //                {
                    //                    await _jobCriteriaBusiness.Edit(jc);
                    //                }
                    //                else
                    //                {
                    //                    await _jobCriteriaBusiness.Create(jc);
                    //                }
                    //            }
                    //        }

                    //        // jc=await _jobCriteriaBusiness.Create(jobcriteria);
                    //        ViewBag.Success = true;
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelErrors(result.Messages);
                    //    }
                    //}
                }
            }

            return Json(new { success = true, id = model.Id, jobId = model.JobId });
            //  return Json(model);
            // return Index(model.ManpowerRecruitmentSummaryId);
            //  return View("index",model);
            //return RedirectToAction("index", new { summaryId = model.ManpowerRecruitmentSummaryId, id = model.Id, jobId = model.JobId });
            //   return PartialView("index", new { summaryId = model.ManpowerRecruitmentSummaryId, id = model.Id, jobId = model.JobId });

        }


        public async Task<ActionResult> GetCriteriaNameById(string id, string type)
        {
            //var model = new List<IdNameViewModel>();
            //model.Add(new IdNameViewModel() { Id = "T0001", Name = "Tabel1" });
            //model.Add(new IdNameViewModel() { Id = "T0002", Name = "Tabel2" });
            //model.Add(new IdNameViewModel() { Id = "T0003", Name = "Tabel3" });

            var data = await _ListOfValueBusiness.GetList(x => x.ListOfValueType == type);
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            var name = res.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
            return Json(name);
        }

        public async Task<ActionResult> GetOtherLOVNameById(string id)
        {
            var data = await _ListOfValueBusiness.GetList(x => x.Id == id);
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            var name = res.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
            return Json(name);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJobSkills(JobCriteriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    model.Type = "Skills";
                    var result = await _jobCriteriaBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {


                    var result = await _jobCriteriaBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("JobAdvertisementInActive", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageInfo(JobCriteriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    model.Type = "Other Information";
                    var result = await _jobCriteriaBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        // return RedirectToAction("membergrouplist");
                        //return PopupRedirect("Portal created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {


                    var result = await _jobCriteriaBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                        //return RedirectToAction("/Member/Index");
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("JobAdvertisementInActive", model);
        }
        public async Task<IActionResult> Edit(string id)
        {

            if (id != null)
            {
                var model = await _jobAdvertisementBusiness.GetCalculatedData(id);
                model.DataAction = DataActionEnum.Edit;
                return View("_Manage", model);

            }
            else
            {
                return View("_Manage", new JobAdvertisementViewModel());
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Manage(JobAdvertisementViewModel model)
        {
            if (ModelState.IsValid)
            {
                
              


                    //if (model.DataAction == DataActionEnum.Create)
                    //{
                    //    var result = await _jobAdvertisementBusiness.Create(model);
                    //    if (result.IsSuccess)
                    //    {
                    //        ViewBag.Success = true;
                    //        // return RedirectToAction("membergrouplist");
                    //        //return PopupRedirect("Portal created successfully");
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelErrors(result.Messages);
                    //    }
                    //}
                    if (model.DataAction == DataActionEnum.Edit)
                    {

                        var mod = await _jobAdvertisementBusiness.GetCalculatedData(model.Id);
                        mod.ShortlistedByHr = model.ShortlistedByHr;
                        mod.ShortlistedByHrCalculated = model.ShortlistedByHrCalculated;
                        mod.ShortlistedForInterview = model.ShortlistedForInterview;
                        mod.ShortlistedForInterviewCalculated = model.ShortlistedForInterviewCalculated;
                        mod.InterviewCompleted = model.InterviewCompleted;
                        mod.InterviewCompletedCalculated = model.InterviewCompletedCalculated;
                        mod.FinalOfferAccepted = model.FinalOfferAccepted;
                        mod.FinalOfferAcceptedCalculated = model.FinalOfferAcceptedCalculated;
                        mod.CandidateJoined = model.CandidateJoined;
                        mod.CandidateJoinedCalculated = model.CandidateJoinedCalculated;
                   

                    var result = await _jobAdvertisementBusiness.Edit(mod);
                        if (result.IsSuccess)
                        {
                            ViewBag.Success = true;
                            //return RedirectToAction("/Member/Index");
                            //return PopupRedirect("Portal edited successfully");
                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }
                   
                }
                }
          

                // return View("_Manage", model);
                return Json(new { success = true});
        }

        public async Task<ActionResult> GetJobDescription(string JobId)
        {
            var result = await _applicationBusiness.GetJobdescription(JobId);
            return Json(result);
        }

        public async Task<ActionResult> GetJobDescriptionbyId(string Id)
        {
            var result = await _applicationBusiness.GetJobdescriptionById(Id);
            return Json(result);
        }

        public IActionResult CreateList(string jobid,string lovtype,string id)
        {
            return View("ManageListOfValue", new ListOfValueViewModel
            {
                ReferenceTypeCode = ReferenceTypeEnum.REC_Job,
                ReferenceTypeId = jobid,
                Code=lovtype,
                Id=id,
                DataAction=DataActionEnum.Create
            }); ;
        }
        public async Task<IActionResult> EditList(string Id)
        {
            var ListOfValue = await _ListOfValueBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {

                ListOfValue.DataAction = DataActionEnum.Edit;
                return View("ManageListOfValue", ListOfValue);
            }
            return View("ManageListOfValue", new ListOfValueViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageListOfValue(ListOfValueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lovcode = String.Concat(model.Name.Where(c => !Char.IsWhiteSpace(c)));

                var exist = await _ListOfValueBusiness.GetSingle(x=>x.Name == model.Name && x.Id != model.Id);
                if (exist != null)
                {
                    return Json(new { success = false, error = "LOV Type Name already exist" });
                }
                else
                {
                    if (model.Json.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Enter LOV values" });
                    }
                    else
                    {
                        var lovs = JsonConvert.DeserializeObject<List<ListOfValueViewModel>>(model.Json);
                        foreach (var item in lovs)
                        {
                            if (item.Name.IsNullOrEmptyOrWhiteSpace())
                            {
                                return Json(new { success = false, error = "Enter LOV name" });
                            }
                        }
                    }                    

                    model.Code = String.Concat("JOB_" + lovcode);
                    model.ListOfValueType = "LOV_TYPE";
                    model.Status = StatusEnum.Active;
                    if (model.DataAction == DataActionEnum.Create)
                    {

                        var result = await _ListOfValueBusiness.Create(model);

                        if (result.IsSuccess)
                        {
                            if (model.Json.IsNotNullAndNotEmpty())
                            {
                                try
                                {
                                    var json = JsonConvert.DeserializeObject<List<ListOfValueViewModel>>(model.Json);
                                    foreach (var item in json)
                                    {
                                        var childlovcode = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                        item.Code = String.Concat("JOB_" + childlovcode);
                                        if (item.ParentId == null)
                                        {
                                            item.ListOfValueType = result.Item.Code;
                                            item.ParentId = result.Item.Id;
                                        }
                                        item.ReferenceTypeId = result.Item.ReferenceTypeId;
                                        item.ReferenceTypeCode = result.Item.ReferenceTypeCode;

                                        item.Status = StatusEnum.Active;
                                        var r = await _ListOfValueBusiness.Create(item);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            return Json(new { success = true, lovtype = result.Item.Code, id = result.Item.Id });

                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }
                    }
                    else if (model.DataAction == DataActionEnum.Edit)
                    {
                        var result = await _ListOfValueBusiness.Edit(model);

                        if (result.IsSuccess)
                        {
                            if (model.Json.IsNotNullAndNotEmpty())
                            {
                                var json = JsonConvert.DeserializeObject<List<ListOfValueViewModel>>(model.Json);
                                var existing = await _ListOfValueBusiness.GetTreeList(result.Item.Id);
                                foreach (var item1 in existing)
                                {
                                    var exitem = json.Where(x => x.Id == item1.Id).ToList();
                                    if (exitem.Count == 0)
                                    {
                                        await _ListOfValueBusiness.Delete(item1.Id);
                                    }
                                }
                                var newexisting = await _ListOfValueBusiness.GetTreeList(result.Item.Id);
                                foreach (var item in json)
                                {
                                    var exitem = newexisting.Where(x => x.Id == item.Id).ToList();
                                    if (item.Id.IsNotNullAndNotEmpty() && exitem.Count != 0)
                                    {
                                        var childlovcode = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                        item.Code = String.Concat("JOB_" + childlovcode);
                                        if (item.ParentId == null)
                                        {
                                            item.ListOfValueType = result.Item.Code;
                                            item.ParentId = result.Item.Id;
                                        }
                                        item.ReferenceTypeId = result.Item.ReferenceTypeId;
                                        item.ReferenceTypeCode = result.Item.ReferenceTypeCode;

                                        item.Status = StatusEnum.Active;
                                        var r = await _ListOfValueBusiness.Edit(item);
                                    }
                                    else
                                    {
                                        var childlovcode = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                        item.Code = String.Concat("JOB_" + childlovcode);
                                        if (item.ParentId == null)
                                        {
                                            item.ListOfValueType = result.Item.Code;
                                            item.ParentId = result.Item.Id;
                                        }
                                        item.ReferenceTypeId = result.Item.ReferenceTypeId;
                                        item.ReferenceTypeCode = result.Item.ReferenceTypeCode;

                                        item.Status = StatusEnum.Active;
                                        var r = await _ListOfValueBusiness.Create(item);
                                    }
                                }
                            }

                            return Json(new { success = true, lovtype = result.Item.Code, id = result.Item.Id, action = "Edit" });

                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }
                    }
                }          
   
            }         

            return View("ManageListOfValue", model);
        }
        public IActionResult ViewListOfValue(string jobid)
        {
            return View("ViewListOfValue", new ListOfValueViewModel
            {
                ReferenceTypeCode = ReferenceTypeEnum.REC_Job,
                ReferenceTypeId = jobid,
                //Code = lovtype,
               // Id = id

            });
        }
       
        
        
        public async Task<ActionResult> GetList([DataSourceRequest] DataSourceRequest request,string jobid,string lovtype,string parentid)
        {
            var data = await _ListOfValueBusiness.GetTreeList(parentid);
           // var data1 = data.Where(x => x.ParentId == parentid).ToList();
            foreach(var item in data)
            {
                if (item.ParentId == parentid)
                {
                    item.ParentId = null;
                }  
            }
            return Json(data.ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetViewList([DataSourceRequest] DataSourceRequest request, string jobid)
        {
            var data = await _ListOfValueBusiness.GetList(x => x.ReferenceTypeCode == ReferenceTypeEnum.REC_Job && x.ListOfValueType == "LOV_TYPE" && x.ReferenceTypeId == jobid && x.Status != StatusEnum.Inactive);
            return Json(data.ToDataSourceResult(request));
        }

        public JsonResult Destroy([DataSourceRequest] DataSourceRequest request, ListOfValueViewModel lov)
        {
            if (ModelState.IsValid)
            {
                _ListOfValueBusiness.Delete(lov.Id);
            }

            return Json(new[] { lov }.ToTreeDataSourceResult(request));
        }

        public JsonResult Create([DataSourceRequest] DataSourceRequest request, ListOfValueViewModel lov)
        {
            lov.DataAction = DataActionEnum.Create;
            lov.Id = Guid.NewGuid().ToString();
            lov.CreatedDate = DateTime.Now;
            lov.LastUpdatedDate = DateTime.Now;
            lov.Status = StatusEnum.Active;
            //lov.Code = String.Concat("JOB_"+lov.Name.Where(c => !Char.IsWhiteSpace(c)));
            //if (lov.ParentId == null)
            //{
            //    lov.ListOfValueType = parentCode;
            //}
            //lov.Status = StatusEnum.Active;
            //_ListOfValueBusiness.Create(lov);


            return Json(new[] { lov }.ToTreeDataSourceResult(request));
        }

        public JsonResult Update([DataSourceRequest] DataSourceRequest request, ListOfValueViewModel lov)
        {
      
            return Json(new[] { lov }.ToTreeDataSourceResult(request));
        }
    }
}
