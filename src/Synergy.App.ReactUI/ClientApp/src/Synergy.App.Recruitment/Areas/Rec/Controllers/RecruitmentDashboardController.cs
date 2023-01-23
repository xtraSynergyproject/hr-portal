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
using CMS.Data.Repository;
using Synergy.App.DataModel;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class RecruitmentDashboardController : ApplicationController
    {
        private readonly IManpowerSummaryCommentBusiness _manpowerSummaryCommentBusiness;
        private readonly IManpowerRecruitmentSummaryBusiness _business;
        private readonly IUserContext _userContext;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IRecruitmentElementBusiness _recruitmentElementBusiness;
        private readonly IJobAdvertisementBusiness _jobAdvertisementBusiness;
        private readonly IApplicationBeneficaryBusiness _applicationBeneficaryBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly IRecTaskBusiness _recTaskBusiness;
        public RecruitmentDashboardController(IManpowerSummaryCommentBusiness manpowerSummaryCommentBusiness,
            IManpowerRecruitmentSummaryBusiness business, IUserContext userContext, IUserRoleBusiness userRoleBusiness,
            IApplicationBusiness applicationBusiness, IRecruitmentElementBusiness recruitmentElementBusiness,
            IJobAdvertisementBusiness jobAdvertisementBusiness, IApplicationBeneficaryBusiness applicationBeneficaryBusiness, IMasterBusiness masterBusiness,
            IRecTaskBusiness recTaskBusiness)
        {
            _manpowerSummaryCommentBusiness = manpowerSummaryCommentBusiness;
            _business = business;
            _userContext = userContext;
            _userRoleBusiness = userRoleBusiness;
            _applicationBusiness = applicationBusiness;
            _recruitmentElementBusiness = recruitmentElementBusiness;
            _jobAdvertisementBusiness = jobAdvertisementBusiness;
            _applicationBeneficaryBusiness = applicationBeneficaryBusiness;
            _masterBusiness = masterBusiness;
            _recTaskBusiness = recTaskBusiness;
        }
        public async Task<IActionResult> Index(string jobAdvId, string orgId,string permissions)
        {
            var model = new RecruitmentDashboardViewModel
            {
                JobAdvId = jobAdvId,
                OrganizationId = orgId
            };
            ViewBag.Permissions = permissions;
            ViewBag.Role = _userContext.UserRoleCodes;
            //var count = await _recTaskBusiness.GetActiveListByUserId(_userContext.UserId);
            var count = await _recTaskBusiness.GetPendingTaskListByUserId(_userContext.UserId);
            var PendingCount = count.Where(x => x.TaskStatusCode == "INPROGRESS" || x.TaskStatusCode == "OVERDUE").Count();
            model.GridTable = await _business.GetJobByOrgUnit(_userContext.UserId);
            //if(_userContext.UserRoleCodes.Contains("HR"))
            //{
            //    PendingCount = PendingCount;
            //}
            ViewBag.PendingTaskCount = PendingCount;
            var userrole = _userContext.UserRoleIds.Split(",");
            if (userrole.Count() > 0)
            {
                model.UserRoleId = userrole[0];
            }
            var DirectHiring = await _recTaskBusiness.GetList(x => x.TemplateCode == "DIRECT_HIRING" && x.TaskStatus != "Draft");
            model.DirectHiring = DirectHiring.Count;
            return View(model);
        }
        public IActionResult Report()
        {           
            ViewBag.Role = _userContext.UserRoleCodes;
          
            return View();
        }

        public async Task<IActionResult> IntentToOffer(string appid)
        {
            var model = new RecruitmentPayElementViewModel();
            model.Basic = 0;
            model.ProfessionalAllowance = 0;
            model.UtilityAllowance = 0;
            model.HRA = 0;
            model.FRA = 0;
            model.Transportation = 0;
            model.FurnishingAllowance = 0;
            var appmodel = await _applicationBusiness.GetSingleById(appid);
            model.ApplicantName = appmodel.FirstName + ' ' + appmodel.MiddleName + ' ' + appmodel.LastName;
            if (appmodel.OfferGrade.IsNotNullAndNotEmpty())
            {
                var gradename = await _applicationBusiness.GetGrade(appmodel.OfferGrade);
                model.Grade = gradename.Name;
                // var temp = model.Grade.Split("-");
                // model.GradeNumber =Int32.Parse(temp[1]);
            }
            model.SalaryRevision = appmodel.SalaryRevision;
            model.SalaryRevisionAmount = appmodel.SalaryRevisionAmount;
            model.SalaryRevisionComment = appmodel.SalaryRevisionComment;
            if (appmodel.OfferDesigination.IsNotNullAndNotEmpty())
            {
                var jobname = await _masterBusiness.GetJobNameById(appmodel.OfferDesigination);
                model.Desigination = jobname.Name;
            }
            // model.Desigination = appmodel.OfferDesigination;
            model.AnnualLeave = appmodel.AnnualLeave;
            if (appmodel.AccommodationId.IsNotNullAndNotEmpty())
            {
                var accom = await _recruitmentElementBusiness.GetAccomadationValue(appmodel.AccommodationId);
                model.AccommodationName = accom.Name;
            }
            if (appmodel.VisaCategory.IsNotNullAndNotEmpty())
            {
                var visacategory = await _recruitmentElementBusiness.GetAccomadationValue(appmodel.VisaCategory);
                model.VisaCategoryName = visacategory.Name;
            }
            var list = await _recruitmentElementBusiness.GetElementData(appid);
            foreach (var a in list)
            {
                if (a.ElementName == "Basic")
                {
                    if (a.Value != null)
                    {
                        model.Basic = a.Value;
                    }
                }
                else if (a.ElementName == "Bonus")
                {
                    model.Bonus = a.Value;
                }
                else if (a.ElementName == "Mobile Allowance")
                {
                    model.MobileAllowance = a.Value;
                }
                else if (a.ElementName == "Transportation")
                {
                    if (a.Value != null)
                    {
                        model.Transportation = a.Value;
                    }
                    if (a.Comment != null)
                    {
                        model.TransportationText = a.Comment;
                    }
                }

                else if (a.ElementName == "Furnishing Allowance")
                {
                    if (a.Value != null)
                    {
                        model.FurnishingAllowance = a.Value;
                    }

                }

                else if (a.ElementName == "Food")
                {
                    model.Food = a.Value;
                }
                else if (a.ElementName == "Professional Allowance")
                {
                    if (a.Value != null)
                    {
                        model.ProfessionalAllowance = a.Value;
                    }
                }
                else if (a.ElementName == "Utility Allowance")
                {
                    if (a.Value != null)
                    {
                        model.UtilityAllowance = a.Value;
                    }

                }
                else if (a.ElementName == "HRA")
                {
                    if (a.Value != null)
                    {
                        model.HRA = a.Value;
                    }

                }
                else if (a.ElementName == "FRA")
                {
                    if (a.Value != null)
                    {
                        model.FRA = a.Value;
                    }

                }
                else if (a.ElementName == "Laundry")
                {
                    model.Laundry = a.Value;
                }
            }
            if (model.Grade != null && Int32.Parse(model.Grade) <= 16 && model.AccommodationName != null && model.AccommodationName == "Own Accommodation")
            {
                model.Total = model.Basic + model.ProfessionalAllowance + model.FRA + model.Transportation;
            }
            else if (model.Grade != null && Int32.Parse(model.Grade) <= 16 && model.AccommodationName != null && model.AccommodationName == "Company Provided Accommodation")
            {
                model.Total = model.Basic + model.ProfessionalAllowance;
            }
            else
            {
                model.Total = model.Basic + model.ProfessionalAllowance + model.HRA + model.UtilityAllowance;
            }

            //model.ApplicationId = appid;
            return View(model);
        }



        public IActionResult RecruitmentPayElement(string applicatiodid)
        {
            var model = new RecruitmentCandidateElementInfoViewModel();
            model.DataAction = DataActionEnum.Create;
            model.ApplicationId = applicatiodid;
            return View("_RecruitmentPayElement", model);
        }

        public async Task<IActionResult> EditRecruitmentPayElement(string applicatiodid, string Id)
        {
            //var model = new RecruitmentCandidateElementInfoViewModel();
            var model = await _recruitmentElementBusiness.GetSingleById(Id);
            if (model != null)
            {
                model.DataAction = DataActionEnum.Edit;
                model.ApplicationId = applicatiodid;
            }
            return View("_RecruitmentPayElement", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePayElement(RecruitmentCandidateElementInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {

                    var res = await _recruitmentElementBusiness.Create(model);
                    if (res.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }


                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var res = await _recruitmentElementBusiness.Edit(model);
                    if (res.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }


                }
            }

            return View("_RecruitmentPayElement", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetElementIdNameList()
        {
            var list = await _recruitmentElementBusiness.GetPayElementIdNameList();
            return Json(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsertIdNameList()
        {
            var list = await _recruitmentElementBusiness.GetUserIdNameList();
            return Json(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationtList()
        {
            var list = await _recruitmentElementBusiness.GetLocationIdNameList();
            return Json(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetGradeIdNameList()
        {
            var list = await _recruitmentElementBusiness.GetGradeIdNameList();
            return Json(list);
        }

        public async Task<IActionResult> RecruitmentElement(string appId, string state, string taskStatus)
        {
            var model = new RecruitmentCandidateElementInfoViewModel();
            var temp = _userContext.UserRoleCodes.Split(",");

            if (temp.Contains("HR"))
            {
                model.IsHr = true;
            }
            if (taskStatus.IsNotNullAndNotEmpty())
            {
                model.TaskStatus = taskStatus;
            }
            model.ApplicationId = appId;
            model.ApplcationStateName = state;
            if (appId.IsNotNull())
            {
                var appmodel = await _applicationBusiness.GetSingleById(model.ApplicationId);
                //var gradename = await _applicationBusiness.GetNationality(appmodel.NationalityId);
                model.ApplicantName = appmodel.FirstName + ' ' + appmodel.MiddleName + ' ' + appmodel.LastName;
                // model.Nationality = gradename.Name;
                if (appmodel.JobId.IsNotNullAndNotEmpty())
                {
                   // var jobname = await _masterBusiness.GetJobNameById(appmodel.JobId);
                    model.OfferDesigination = appmodel.JobId;
                }
                var manpowertype = await _jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(appmodel.JobId);
                model.OfferGrade = appmodel.OfferGrade;
                model.OfferDesigination = appmodel.OfferDesigination?? appmodel.JobId;
                model.GaecNo = appmodel.GaecNo;
                model.JoiningDate = appmodel.JoiningDate;
                model.OfferSignedBy = appmodel.OfferSignedBy ?? "SATISH G. PILLAI";
                model.AnnualLeave = appmodel.AnnualLeave;
                model.AnnualLeave = appmodel.AnnualLeave;
                model.SalaryRevision = appmodel.SalaryRevision;
                model.SalaryRevisionAmount = appmodel.SalaryRevisionAmount;
                model.SalaryOnAppointment = appmodel.SalaryOnAppointment.IsNotNullAndNotEmpty()? Int32.Parse(appmodel.SalaryOnAppointment):0;
                model.SalaryRevisionComment = appmodel.SalaryRevisionComment;
                model.VisaCategory = appmodel.VisaCategory;
                model.AccommodationId = appmodel.AccommodationId;
                model.TravelOriginAndDestination = "Mumbai - Doha";

                if (model.ApplcationStateName == "FinalOfferSent")
                {
                    if (appmodel.FinalOfferReference.IsNullOrEmpty())
                    {
                        var str = await _recruitmentElementBusiness.GenerateFinalOfferRef(appmodel.ApplicationNo);
                        appmodel.FinalOfferReference = str;
                        await _applicationBusiness.Edit(appmodel);

                    }
                    if ((manpowertype.ManpowerTypeCode == "Worker" || manpowertype.ManpowerTypeCode == "UnskilledWorker" || manpowertype.ManpowerTypeCode == "Welder") && appmodel.OfferGrade == null)
                    {
                        var grade = await _recruitmentElementBusiness.GetGrade("10");
                        model.OfferGrade = grade.Id;

                    }
                    model.FinalOfferReference = appmodel.FinalOfferReference;
                    model.ContractStartDate = appmodel.ContractStartDate == null ? DateTime.Now : appmodel.ContractStartDate;
                    model.JoiningNotLaterThan = appmodel.JoiningNotLaterThan == null ? DateTime.Now.AddDays(30) : appmodel.JoiningNotLaterThan;
                    model.IsTrainee = appmodel.IsTrainee;

                    model.ServiceCompletion = appmodel.ServiceCompletion;
                    model.TravelOriginAndDestination = appmodel.TravelOriginAndDestination == null ? "Mumbai - Doha" : appmodel.TravelOriginAndDestination;
                    model.VehicleTransport = appmodel.VehicleTransport;
                    model.IsLocalCandidate = appmodel.IsLocalCandidate;
                    model.ServiceCompletion = appmodel.ServiceCompletion ?? 6;
                }
            }

            return View("RecruitmentElement", model);
        }

        public async Task<IActionResult> JoiningSource(string appId, string taskStatus, string rs = null)
        {
            var model = new ApplicationViewModel();
            // model.ApplicationId = appId;
            if (appId.IsNotNull())
            {
                model = await _applicationBusiness.GetSingleById(appId);
                model.FullName = model.FirstName + ' ' + model.MiddleName + ' ' + model.LastName;
                //var gradename = await _applicationBusiness.GetNationality(appmodel.NationalityId);
                //model.ApplicantName = appmodel.FirstName;
                //// model.Nationality = gradename.Name;
                //model.OfferDesigination = appmodel.OfferDesigination;
                //model.OfferGrade = appmodel.OfferGrade;
                //model.GaecNo = appmodel.GaecNo;
                //model.JoiningDate = appmodel.JoiningDate;
                //model.OfferSignedBy = appmodel.OfferSignedBy;
            }
            //if (rs.IsNullOrEmpty())
            //{
            //    ViewBag.Source = "Hr";
            //}
            if (taskStatus.IsNotNullAndNotEmpty())
            {
                model.TaskStatus = taskStatus;
            }

            return View("JoiningSource", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJoiningSource(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appmodel = await _applicationBusiness.GetSingleById(model.Id);
                appmodel.Sourcing = model.Sourcing;
                appmodel.FirstName = model.FirstName;
                appmodel.LastName = model.LastName;
                appmodel.MiddleName = model.MiddleName;
                appmodel.NationalityId = model.NationalityId;
                appmodel.GaecNo = model.GaecNo;
                appmodel.JobNo = model.JobNo;
                appmodel.ReportingToId = model.ReportingToId;
                appmodel.NextOfKin = model.NextOfKin;
                appmodel.NextOfKinRelationship = model.NextOfKinRelationship;
                appmodel.NextOfKinEmail = model.NextOfKinEmail;
                appmodel.NextOfKinPhoneNo = model.NextOfKinPhoneNo;
                //appmodel.OfferSignedBy = model.OfferSignedBy;
                appmodel.OtherNextOfKin = model.OtherNextOfKin;
                appmodel.OtherNextOfKinRelationship = model.OtherNextOfKinRelationship;
                appmodel.OtherNextOfKinEmail = model.OtherNextOfKinEmail;
                appmodel.OtherNextOfKinPhoneNo = model.OtherNextOfKinPhoneNo;

                appmodel.WitnessName1 = model.WitnessName1;
                appmodel.WitnessDesignation1 = model.WitnessDesignation1;
                appmodel.WitnessGAEC1 = model.WitnessGAEC1;
                appmodel.WitnessDate1 = model.WitnessDate1;

                appmodel.WitnessName2 = model.WitnessName2;
                appmodel.WitnessDesignation2 = model.WitnessDesignation2;
                appmodel.WitnessGAEC2 = model.WitnessGAEC2;
                appmodel.WitnessDate2 = model.WitnessDate2;
                appmodel.DateOfArrival = model.DateOfArrival;
                appmodel.JoiningDate = model.JoiningDate;

                await _applicationBusiness.Edit(appmodel);
            }

            return View("JoiningSource", model);
        }

        public async Task<JsonResult> ReadPayElementData([DataSourceRequest] DataSourceRequest request, string appid)
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _recruitmentElementBusiness.GetElementData(appid);
            var appmodel = await _applicationBusiness.GetSingleById(appid);
            foreach (var a in model)
            {
                if (a.ElementName == "Basic")
                {
                    if (a.Value == null && appmodel.SalaryOnAppointment != null)
                    {
                        a.Value = Double.Parse(appmodel.SalaryOnAppointment);
                    }
                }
            }
            //if (model.Count==0)
            //{
            //    var cmodel = new List<RecruitmentCandidateElementInfoViewModel>();
            //    var list = await _recruitmentElementBusiness.GetPayElementIdNameList();
            //    foreach(var a in list)
            //    {
            //        var cmodel1 = new RecruitmentCandidateElementInfoViewModel
            //        {
            //            ElementId=a.Id,
            //            ElementName = a.ElementName
            //        };
            //        cmodel.Add(cmodel1);
            //    }

            //    var data1 = cmodel.ToDataSourceResult(request);
            //    return Json(data1);
            //}
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        [HttpPost]
        public async Task<IActionResult> ManageApplicationElement(RecruitmentCandidateElementInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appmodel = await _applicationBusiness.GetSingleById(model.ApplicationId);
                appmodel.OfferDesigination = model.OfferDesigination;
                appmodel.OfferGrade = model.OfferGrade;
                appmodel.GaecNo = model.GaecNo;
                appmodel.JoiningDate = model.JoiningDate;
                appmodel.OfferSignedBy = model.OfferSignedBy;
                appmodel.AnnualLeave = model.AnnualLeave;
                appmodel.AccommodationId = model.AccommodationId;
                appmodel.VisaCategory = model.VisaCategory;
                appmodel.SalaryRevision = model.SalaryRevision;
                appmodel.SalaryRevisionAmount = model.SalaryRevisionAmount;
                appmodel.SalaryOnAppointment = model.SalaryOnAppointment.ToString();
                appmodel.SalaryRevisionComment = model.SalaryRevisionComment;
                if (model.ApplcationStateName == "FinalOfferSent")
                {
                    appmodel.ContractStartDate = model.ContractStartDate;
                    appmodel.JoiningNotLaterThan = model.JoiningNotLaterThan;
                    appmodel.IsTrainee = model.IsTrainee;

                    appmodel.ServiceCompletion = model.ServiceCompletion;
                    appmodel.TravelOriginAndDestination = model.TravelOriginAndDestination;
                    appmodel.VehicleTransport = model.VehicleTransport;
                    appmodel.IsLocalCandidate = model.IsLocalCandidate;

                }
                await _applicationBusiness.Edit(appmodel);
                if (model.JsonPayElement.IsNotNull())
                {
                    var JsonPayElement = JsonConvert.DeserializeObject<List<RecruitmentCandidatePayElementViewModel>>(model.JsonPayElement);

                    foreach (var a in JsonPayElement)
                    {
                        var jc = new RecruitmentCandidateElementInfoViewModel();
                        jc.ApplicationId = model.ApplicationId;
                        jc.Value = a.Value;
                        jc.ElementId = a.PayId;
                        jc.Comment = a.Comment;
                        if (a.ElementId.IsNotNullAndNotEmpty())
                        {
                            jc.Id = a.Id;

                            var res = await _recruitmentElementBusiness.Edit(jc);
                            if (res.IsSuccess)
                            {
                                ViewBag.Success = true;
                            }
                        }
                        else if (a.Value.IsNotNull() || a.Comment.IsNotNullAndNotEmpty())
                        {

                            // jc.Value = a.Value;
                            var res = await _recruitmentElementBusiness.Create(jc);
                            if (res.IsSuccess)
                            {
                                ViewBag.Success = true;
                            }
                        }
                    }
                }
            }

            if (model.ApplcationStateName != "FinalOfferSent")
            {
                //var appDetails = await _applicationBusiness.GetApplicationDetail(model.ApplicationId);
                //if (appDetails.IsNotNull() && (appDetails.GradeCode > 16) && _userContext.Email== "test_rejni@gmail.com")
                //{
                //    return View("RecruitmentElement", model);
                //}
                return RedirectToAction("IntentToOffer", new { appid = model.ApplicationId });
            }
            else
            {
                return RedirectToAction("FinalOffer", "RecruitmentProcess", new { applicationId = model.ApplicationId });
            }
            return View("RecruitmentElement", model);
        }

        public async Task<IActionResult> DeleteElement(string Id)
        {
            await _recruitmentElementBusiness.Delete(Id);
            return Json(true);
        }


        public async Task<IActionResult> JobAdvertisementStatistic(string jobAdvId, string state, string orgId, long count, string status,string permissions="")
        {
            ViewBag.Permissions = permissions;
            var model = new ApplicationViewModel();
            model.JobId = jobAdvId;
            model.ApplicationState = state;
            model.ApplicationStatus = status;
            if (state == null)
            {
                model.ApplicationStateName = "Number Of Applications";
            }
            else if (state == "InterviewsCompleted" && status == "WAITLISTED")
            {
                model.ApplicationStateName = "Waitlist By HM";
            }
            else
            {
                var state1 = await _applicationBusiness.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == state);
                model.ApplicationStateName = state1.Name;
            }
            model.OrganizationId = orgId;
            model.StateCount = count;
            //var manpowertype = await _jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(jobAdvId);
            // model.ManpowerTypeCode = manpowertype.ManpowerTypeCode;
            if (jobAdvId.IsNotNullAndNotEmpty())
            {
                var jobname = await _masterBusiness.GetJobNameById(jobAdvId);
                model.JobTitle = jobname.Name;
            }
            else
            {
                model.JobTitle = "All";
            }
          
           
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetRecruitmentDashboardCount(string orgId)
        {
            var list = await _business.GetRecruitmentDashobardCount(orgId);
            return Json(list);
        }

        [HttpGet]
        public async Task<ActionResult> GetRecruitmentDashboardByOrgJob(string orgId, string jobAdvId,string permission="")
        {
            var data = await _business.GetManpowerRecruitmentSummaryByOrgJob(orgId, jobAdvId,permission);
            return Json(data);
        }

        public async Task<JsonResult> ReadJobAdvertisementData([DataSourceRequest] DataSourceRequest request, string jobAdvId, string orgId, string state, string tempcode, string nexttempcode, string visatempcode, string status,string permission="")
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetJobAdvertismentState(jobAdvId, orgId, state, tempcode, nexttempcode, visatempcode, status, permission);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }
        public async Task<JsonResult> ReadTotalJobAdvertisementData([DataSourceRequest] DataSourceRequest request, string jobAdvId, string orgId, string state, string tempcode, string nexttempcode, string visatempcode)
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetTotalApplication(jobAdvId, orgId, state, tempcode, nexttempcode, visatempcode);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }
        public async Task<JsonResult> GetJobAdvertismentApplication([DataSourceRequest] DataSourceRequest request, string jobAdvId, string orgId, string state, string tempcode, string tempCodeOther,string permission="")
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetJobAdvertismentApplication(jobAdvId, orgId, state, tempcode, tempCodeOther,permission);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        public async Task<JsonResult> GetDirectHiringData([DataSourceRequest] DataSourceRequest request,string jobAdvId,string orgId,string permission="")
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetDirictHiringData(jobAdvId,orgId,permission);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }


        public async Task<JsonResult> GetWorkerPoolApplication([DataSourceRequest] DataSourceRequest request, string jobAdvId, string orgId, string state, string tempcode)
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetWorkerPoolApplication(jobAdvId, orgId, state, tempcode);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        public async Task<ActionResult> UpdateApplicationStatus(string type, string status, string applicationId, string CandidateProfileId, string state)
        {
            //var applicationstate = type;
            await _applicationBusiness.UpdateJobAdvtApplicationStatus(type, status, applicationId, CandidateProfileId, state);

            return Json(new { success = true });

        }

        public async Task<IActionResult> ApplicationStateTrack(string applicationId)
        {
            var model = new ApplicationStateTrackDetailViewModel();
            var appdetails = await _applicationBusiness.GetAppDetails(applicationId);
            model.ApplicationId = applicationId;
            model.CandidateName = String.Concat(appdetails.FirstName, ' ', appdetails.MiddleName, ' ', appdetails.LastName);
            model.AppliedDate = appdetails.AppliedDate;
            model.JobName = appdetails.JobName;
            model.OrgName = appdetails.OrganizationName;
            model.ApplicationStateName = appdetails.ApplicationStateName;
            model.ApplicationStatusName = appdetails.ApplicationStatusName;
            model.BatchName = appdetails.BatchName;
            ViewBag.LoginUserId = _userContext.UserId;
            var userrole = _userContext.UserRoleCodes.Split(",");
            ViewBag.IsUserHR = userrole.Contains("HR");
            return View(model);
        }
        public async Task<IActionResult> ReadApplicationStateTrackDetails([DataSourceRequest] DataSourceRequest request, string applicationId)
        {
            var list = await _applicationBusiness.GetAppStateTrackDetailsByCand(applicationId);
            var dsResult = list.ToDataSourceResult(request);
            return Json(dsResult);
            //foreach (var s in apstmodel)
            //{
            //    if (s.StateCode == "ShortListByHr")
            //    {
            //        model.ShortListByHrDate = s.ChangedDate;
            //        model.ShortListByHrUpdatedBy = s.ChangedByName;
            //        model.ShortListByHrStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "ShortListByHm")
            //    {
            //        model.ShortListByHmDate = s.ChangedDate;
            //        model.ShortListByHmUpdatedBy = s.ChangedByName;
            //        model.ShortListByHmStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "InterviewsCompleted")
            //    {
            //        model.InterviewCompletedDate = s.ChangedDate;
            //        model.InterviewCompletedUpdatedBy = s.ChangedByName;
            //        model.InterviewCompletedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "IntentToOfferSent")
            //    {
            //        model.IntentToOfferSentDate = s.ChangedDate;
            //        model.IntentToOfferSentUpdatedBy = s.ChangedByName;
            //        model.IntentToOfferSentStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "MedicalCompleted")
            //    {
            //        model.MedicalCompletedDate = s.ChangedDate;
            //        model.MedicalCompletedUpdatedBy = s.ChangedByName;
            //        model.MedicalCompletedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "VisaAppointmentTaken")
            //    {
            //        model.VisaAppointmentTakenDate = s.ChangedDate;
            //        model.VisaAppointmentTakenUpdatedBy = s.ChangedByName;
            //        model.VisaAppointmentTakenStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "BiometricCompleted")
            //    {
            //        model.BiometricCompletedDate = s.ChangedDate;
            //        model.BiometricCompletedUpdatedBy = s.ChangedByName;
            //        model.BiometricCompletedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "VisaApproved")
            //    {
            //        model.VisaApprovedDate = s.ChangedDate;
            //        model.VisaApprovedUpdatedBy = s.ChangedByName;
            //        model.VisaApprovedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "VisaSentToCandidates")
            //    {
            //        model.VisaSentToCandidateDate = s.ChangedDate;
            //        model.VisaSentToCandidateUpdatedBy = s.ChangedByName;
            //        model.VisaSentToCandidateStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "FlightTicketsBooked")
            //    {
            //        model.FlightTicketsBookedDate = s.ChangedDate;
            //        model.FlightTicketsBookedUpdatedBy = s.ChangedByName;
            //        model.FlightTicketsBookedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "CandidateArrived")
            //    {
            //        model.CandidateArrivedDate = s.ChangedDate;
            //        model.CandidateArrivedUpdatedBy = s.ChangedByName;
            //        model.CandidateArrivedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "Joined")
            //    {
            //        model.JoinedDate = s.ChangedDate;
            //        model.JoinedUpdatedBy = s.ChangedByName;
            //        model.JoinedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "FinalOfferAccepted")
            //    {
            //        model.FinalOfferAcceptedDate = s.ChangedDate;
            //        model.FinalOfferAcceptedUpdatedBy = s.ChangedByName;
            //        model.FinalOfferAcceptedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "UnReviewed")
            //    {
            //        model.UnReviewedDate = s.ChangedDate;
            //        model.UnReviewedUpdatedBy = s.ChangedByName;
            //        model.UnReviewedStateId = s.ApplicationStateId;
            //    }
            //    else if (s.StateCode == "FinalOfferSent")
            //    {
            //        model.FinalOfferSentDate = s.ChangedDate;
            //        model.FinalOfferSentUpdatedBy = s.ChangedByName;
            //        model.FinalOfferSentStateId = s.ApplicationStateId;
            //    }
            //}

            //return View("ApplicationStateTrack", new ApplicationStateTrackViewModel());
            // return View("ApplicationStateTrack", model);
        }


        public IActionResult CreateBeneFiciary(string applicatiodid)
        {
            var model = new ApplicationBeneficiaryViewModel();
            model.DataAction = DataActionEnum.Create;
            model.ApplicationId = applicatiodid;
            return View("_ApplicationBeneficiary", model);
        }

        public async Task<IActionResult> EditBeneFiciary(string applicatiodid, string Id)
        {
            //var model = new RecruitmentCandidateElementInfoViewModel();
            var model = await _recruitmentElementBusiness.GetBeneficiartDataByid(Id);
            if (model != null)
            {
                model.DataAction = DataActionEnum.Edit;
                model.ApplicationId = applicatiodid;
            }
            return View("_ApplicationBeneficiary", model);
        }

        public async Task<IActionResult> DeleteBeneFiciary(string Id)
        {
            await _applicationBeneficaryBusiness.Delete(Id);
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> ManageBeneficiary(ApplicationBeneficiaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {

                    var res = await _recruitmentElementBusiness.Beneficiarycreate(model, DataActionEnum.Create);
                    if (res)
                    {
                        ViewBag.Success = true;
                    }


                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var res = await _recruitmentElementBusiness.Beneficiarycreate(model, DataActionEnum.Edit);
                    if (res)
                    {
                        ViewBag.Success = true;
                    }


                }
            }

            return View("_ApplicationBeneficiary", model);
        }

        public async Task<JsonResult> ReadBeneficiaryData([DataSourceRequest] DataSourceRequest request, string appid)
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _recruitmentElementBusiness.GetBeneficiartData(appid);
            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        public async Task<IActionResult> CompetenceMatrix(string appId, string taskStatus, string rs = null)
        {
            var model = new ApplicationViewModel();
            // model.ApplicationId = appId;
            if (appId.IsNotNull())
            {
                model = await _applicationBusiness.GetSingleById(appId);
                //var gradename = await _applicationBusiness.GetNationality(appmodel.NationalityId);
                //model.ApplicantName = appmodel.FirstName;
                //// model.Nationality = gradename.Name;
                //model.OfferDesigination = appmodel.OfferDesigination;
                //model.OfferGrade = appmodel.OfferGrade;
                //model.GaecNo = appmodel.GaecNo;
                //model.JoiningDate = appmodel.JoiningDate;
                //model.OfferSignedBy = appmodel.OfferSignedBy;
            }
            if (rs.IsNullOrEmpty())
            {
                model.Source = "Hr";
            }
            if (taskStatus.IsNotNullAndNotEmpty())
            {
                model.TaskStatus = taskStatus;
            }
            return View("CompetenceMatrix", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCompetenceMatrix(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appmodel = await _applicationBusiness.GetSingleById(model.Id);
                if (model.Source == "Hr")
                {
                    appmodel.RequirementQualification = model.RequirementQualification;
                    appmodel.RequirementTechnical = model.RequirementTechnical;
                    appmodel.RequirementExperience = model.RequirementExperience;
                    appmodel.RequirementSpecialization = model.RequirementSpecialization;
                    appmodel.RequirementITSkills = model.RequirementITSkills;
                }
                else
                {
                    appmodel.ActualQualification = model.ActualQualification;
                    appmodel.ActualTechnical = model.ActualTechnical;
                    appmodel.ActualExperience = model.ActualExperience;
                    appmodel.ActualSpecialization = model.ActualSpecialization;
                    appmodel.ActualITSkills = model.ActualITSkills;
                    appmodel.NatureOfWork = model.NatureOfWork;
                    appmodel.TrainingsUndergone = model.TrainingsUndergone;
                    appmodel.CountriesWorked = model.CountriesWorked;
                    appmodel.OrganizationWorked = model.OrganizationWorked;
                    appmodel.FieldOfExposure = model.FieldOfExposure;
                    appmodel.PositionsWorked = model.PositionsWorked;
                    appmodel.DrivingLicense = model.DrivingLicense;
                    appmodel.ExtraCurricular = model.ExtraCurricular;
                    appmodel.AnyOtherLanguage = model.AnyOtherLanguage;
                }
                var result = await _applicationBusiness.Edit(appmodel);
                if (result.IsSuccess)
                {
                    ViewBag.Success = true;
                }
            }

            return View("CompetenceMatrix", model);
        }

        public async Task<ActionResult> WorkerPool1(string workerbatchid, long level)
        {
            var model = new ApplicationViewModel();
            var temp = _userContext.UserRoleCodes.Split(",");
            model.UserRoleCodes = temp;
            var service1 = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == workerbatchid);
            if (level == 1 && service1.IsNotNull())
            {
                var task = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == service1.Id && x.TemplateCode == "WORKER_POOL_HOD");
                if (task.IsNotNull())
                {
                    model.TaskStatus = task.TaskStatusCode;
                }
                
            }
            else if (level == 2 && service1.IsNotNull())
            {
                var task = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == service1.Id && x.TemplateCode == "WORKER_POOL_HR_HEAD");
                if (task.IsNotNull())
                {
                    model.TaskStatus = task.TaskStatusCode;
                }
            }
            else if (level == 3 && service1.IsNotNull())
            {
                var task = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == service1.Id && x.TemplateCode == "WORKER_POOL_PLANNING");
                if (task.IsNotNull())
                {
                    model.TaskStatus = task.TaskStatusCode;
                }
            }
            else if (level == 4 && service1.IsNotNull())
            {
                var task = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == service1.Id && x.TemplateCode == "WORKER_POOL_ED");
                if (task.IsNotNull())
                {
                    model.TaskStatus = task.TaskStatusCode;
                }
            }
            model.WorkerBatchId = workerbatchid;
            model.level = level;

            return View(model);
        }


        public async Task<JsonResult> ReadWorkPool1Data([DataSourceRequest] DataSourceRequest request, string workerbatchid)
        {

            //var model = await _applicationBusiness.GetCandiadteShortListData(search);
            var model = await _applicationBusiness.GetWorkPool1Data(workerbatchid);

            var data = model.ToDataSourceResult(request);
            return Json(data);

        }

        public async Task<IActionResult> UpdateWorkerApproval(string Id, bool approval, long level, bool level0)
        {

            if (level0)
            {
                var model = await _applicationBusiness.GetWorkPool1Data(Id);
                if (level == 1)
                {
                    foreach (var item in model)
                    {
                        var appmodel = await _applicationBusiness.GetSingleById(item.Id);
                        appmodel.HodApproval = approval;
                        await _applicationBusiness.Edit(appmodel);
                    }
                }
                else if (level == 2)
                {
                    foreach (var item in model)
                    {
                        var appmodel = await _applicationBusiness.GetSingleById(item.Id);
                        appmodel.HRHeadApproval = approval;
                        await _applicationBusiness.Edit(appmodel);
                    }
                }
                else if (level == 3)
                {
                    foreach (var item in model)
                    {
                        var appmodel = await _applicationBusiness.GetSingleById(item.Id);
                        appmodel.PlanningApproval = approval;
                        await _applicationBusiness.Edit(appmodel);
                    }
                }
                else if (level == 4)
                {
                    foreach (var item in model)
                    {
                        var appmodel = await _applicationBusiness.GetSingleById(item.Id);
                        appmodel.EDApproval = approval;
                        await _applicationBusiness.Edit(appmodel);
                    }
                }


                return Json(true);


            }
            else
            {
                var appmodel = await _applicationBusiness.GetSingleById(Id);
                if (level == 1)
                {
                    appmodel.HodApproval = approval;
                }
                else if (level == 2)
                {
                    appmodel.HRHeadApproval = approval;
                }
                else if (level == 3)
                {
                    appmodel.PlanningApproval = approval;
                }
                else if (level == 4)
                {
                    appmodel.EDApproval = approval;
                }
                var res = await _applicationBusiness.Edit(appmodel);
                if (res.IsSuccess)
                {
                    return Json(true);
                }
            }

            return Json(false);
        }


        public async Task<IActionResult> ManageWorkerPool1(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // var appmodel = await _applicationBusiness.GetList(e=>e.WorkerBatchId==model.WorkerBatchId);
                if (model.JsonWorkerPool1.IsNotNull())
                {
                    var jsonworkerpool = JsonConvert.DeserializeObject<List<WorkerPool1ViewModel>>(model.JsonWorkerPool1);

                    foreach (var item in jsonworkerpool)
                    {
                        var appmodel = await _applicationBusiness.GetSingleById(item.Id);
                        if (item != null)
                        {

                            if (model.level == 2)
                            {
                                appmodel.HRHeadApproval = item.HRApprovl;
                                appmodel.HRHeadComment = item.HRHeadComment;


                            }
                            else if (model.level == 1)
                            {

                                appmodel.HodApproval = item.HodApprovl;
                                appmodel.HodComment = item.HodComment;

                                //if (item.HodApprovl == null)
                                //{
                                //    return Json(new { success = false, error = "Please Select Yes or No for Approval", workerbatchid = model.WorkerBatchId, level = model.level });
                                //}
                                //else
                                //{
                                //    appmodel.HodApproval = item.HodApprovl;
                                //    appmodel.HodComment = item.HodComment;

                                //}


                            }
                            else if (model.level == 3)
                            {

                                appmodel.PlanningApproval = item.PlanningApprovl;
                                appmodel.PlanningComment = item.PlanningComment;


                            }

                            else if (model.level == 4)
                            {

                                appmodel.EDApproval = item.EDApprovl;
                                appmodel.EDComment = item.EDComment;


                            }





                        }
                        var result = await _applicationBusiness.Edit(appmodel);
                    }
                }

            }

            return Json(new { success = true, workerbatchid = model.WorkerBatchId, level = model.level });
        }

        public async Task<JsonResult> GetUserPendingTaskByRole([DataSourceRequest] DataSourceRequest request, string orgId)
        {
            var list = await _recTaskBusiness.GetPendingTaskDetailsForUser(_userContext.UserId, orgId, _userContext.UserRoleCodes);
            return Json(list);
        }

        public async Task<IActionResult> GetTaskByOrgUnit([DataSourceRequest] DataSourceRequest request, string userRoleId)
        {
            var Data = await _business.GetTaskByOrgUnit(_userContext.UserId, userRoleId);
            return Json(Data.ToDataSourceResult(request));
        }
        public ActionResult Inbox()
        {
            var model = new ApplicationSearchViewModel();
            return View(model);
        }
        public IActionResult ReportPendingForHM()
        {           

            return View();
        }
        public async Task<IActionResult> ReadPendingForHM([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _applicationBusiness.ReadPendingForHM();
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public IActionResult ReportRejectedForHM()
        {

            return View();
        }
        
        public async Task<IActionResult> ReadRejectedForHM([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _applicationBusiness.ReadRejectedForHM();
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public IActionResult ReportFutureCandidate()
        {

            return View();
        }
        public async Task<IActionResult> ReadFutureCandidate([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _applicationBusiness.ReadFutureCandidate();
            if(_userContext.UserRoleCodes.Contains("HM"))
            {
                result = result.Where(x => x.HiringManagerId == _userContext.UserId).ToList();
            }
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public IActionResult ReportPendingForUser()
        {

            return View();
        }
        public async Task<IActionResult> ReadPendingForUser([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _applicationBusiness.ReadPendingForUser();
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }
        public IActionResult ReportPendingTaskED()
        {
            return View();
        }
        public async Task<IActionResult> ReadReportPendingTaskEDData([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<ApplicationViewModel>();
            var result1 = await _applicationBusiness.GetApplicationPendingTaskByUserRole("ED");
            if (result1 != null)
            {
                result = result1.ToList();
            }
            var data = result.ToDataSourceResult(request);
            return Json(data);
        }
        public IActionResult ReportPendingTaskHR()
        {
            return View();
        }
        public async Task<IActionResult> ReadReportPendingTaskHRData([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<ApplicationViewModel>();
            var result1 = await _applicationBusiness.GetApplicationPendingTaskByUserRole("HR");
            if (result1 != null)
            {
                result = result1.ToList();
            }
            var data = result.ToDataSourceResult(request);
            return Json(data);
        }
        public async Task<IActionResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string stageName, string stageId, string batchId, string expandingList)
        {
            var result = await _recTaskBusiness.GetBulkApprovalMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId, batchId, expandingList, _userContext.UserRoleCodes);
            var model = result.ToList();
            return Json(model);
        }
        //public async Task<JsonResult> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId)
        //{
        //    var result = await _recTaskBusiness.GetBulkApprovalMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds);
        //    var model = result.ToList();
        //    return Json(model);
        //}

        public IActionResult PendingTaskHOD()
        {
            return View();
        }

        public async Task<IActionResult> ReadPendingTaskHOD([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _recTaskBusiness.GetPendingTaskListHod();
            var j = Json(result.ToDataSourceResult(request));
            return j;
        }

        public async Task<IActionResult> DeleteApplication(string appId)
        {
            await _applicationBusiness.Delete(appId);
            return Json(new { success = true });
        }
    }
}
