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
using NumberToEnglishWordConverter;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class RecruitmentProcessController : ApplicationController
    {
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IJobAdvertisementBusiness _jdBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly IApprovarSignatureBusiness _approvarSignatureBusiness;
        private readonly IRecTaskBusiness _recTaskBusiness;

        public RecruitmentProcessController(IApplicationBusiness applicationBusiness, IJobAdvertisementBusiness jdBusiness, IMasterBusiness masterBusiness
            ,IApprovarSignatureBusiness approvarSignatureBusiness
            ,IRecTaskBusiness recTaskBusiness)
        {
            _applicationBusiness = applicationBusiness;
            _jdBusiness = jdBusiness;
            _masterBusiness = masterBusiness;
            _approvarSignatureBusiness = approvarSignatureBusiness;
            _recTaskBusiness = recTaskBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> JoiningReport(string appid)
        {
           // var model = new ApplicationViewModel();
            var appmodel = await _applicationBusiness.GetSingleById(appid);
            var sign = await _applicationBusiness.GetUserSign();
            if (sign != null)
            {
                appmodel.SignatureId = sign.Id;
            }
           
            var jd = await _jdBusiness.GetJobManpowerType(appmodel.JobId);
            if (jd.Code != "Staff")
            {
                //if (appmodel.ReportingToId.IsNotNullAndNotEmpty())
                //{
                //    var gradename = await _applicationBusiness.GetUserName(appmodel.ReportingToId);
                //    if (gradename.IsNotNull())
                //    {
                //        appmodel.ReportToName = gradename.Name;
                //    }
                //}

                if (appmodel.NationalityId.IsNotNullAndNotEmpty())
                {
                    var gradename = await _applicationBusiness.GetNationality(appmodel.NationalityId);
                    if (gradename.IsNotNull())
                    {
                        appmodel.NationalityName = gradename.Name;
                    }
                }
                if (appmodel.TitleId.IsNotNullAndNotEmpty())
                {
                    var gradename = await _applicationBusiness.GetTitle(appmodel.TitleId);
                    if (gradename.IsNotNull())
                    {
                        appmodel.TitleName = gradename.Name;
                    }
                }
                if (appmodel.JobId.IsNotNullAndNotEmpty())
                {
                    var jobname = await _masterBusiness.GetJobNameById(appmodel.JobId);
                    appmodel.OfferDesigination = jobname.Name;
                }
                if (jd.Code == "Worker" || jd.Code == "UnskilledWorker" || jd.Code == "Welder")
                {
                    ViewBag.type = "Workers";
                }
                
                return View("JoiningReport", appmodel);
            }
            else
            {
                var staffDetails = await _applicationBusiness.GetStaffJoiningDetails(appid);
                
                if (staffDetails.JobId.IsNotNullAndNotEmpty())
                {
                    var jobname = await _masterBusiness.GetJobNameById(appmodel.JobId);
                    staffDetails.OfferDesigination = jobname.Name;
                }
                staffDetails.JoiningTime = String.Format("{0:HH:mm}", staffDetails.CandJoiningDate);
                return View("StaffJoiningReport", staffDetails);
            }
        }

        public async Task<IActionResult> PersonalData(string appid)
        {
            // var model = new ApplicationViewModel();
            var appmodel = await _applicationBusiness.GetSingleById(appid);
            //if (appmodel.ReportingToId.IsNotNullAndNotEmpty())
            //{
            //    var gradename = await _applicationBusiness.GetUserName(appmodel.ReportingToId);
            //    appmodel.ReportToName = gradename.Name;
            //}

            if (appmodel.NationalityId.IsNotNullAndNotEmpty())
            {
                var gradename = await _applicationBusiness.GetNationality(appmodel.NationalityId);
                appmodel.NationalityName = gradename.Name;
            }

            if (appmodel.TitleId.IsNotNullAndNotEmpty())
            {
                var gradename = await _applicationBusiness.GetTitle(appmodel.TitleId);
                appmodel.TitleName = gradename.Name;
            }
            return View(appmodel);
        }

        public async Task<IActionResult> Declaration(string appid)
        {
            // var model = new ApplicationViewModel();
            var appmodel = await _applicationBusiness.GetApplicationDeclarationData(appid);
           
            return View(appmodel);
        }

        public IActionResult AnnexureForStaff()
        {
            return View();
        }
        public async Task<IActionResult> FinalOffer(string applicationId)
        {
            var app = await _applicationBusiness.GetSingleById(applicationId);
            var jd = await _jdBusiness.GetJobManpowerType(app.JobId);
            var sign = await _applicationBusiness.GetUserSign();
            var approversigns = await _approvarSignatureBusiness.GetList();

            if (sign != null)
            {
                app.SignatureId = sign.Id;
            }
            if (jd.Code == "Worker" || jd.Code == "UnskilledWorker")
            {
                var wsadetails = await _applicationBusiness.GetWSADetails(applicationId);
                //wsadetails.CurrentDate = DateTime.UtcNow;
                wsadetails.BasicInWords = new NumberToEnglishWordConverter.NumberToEnglishWordConverter().changeCurrencyToWords(wsadetails.Basic);
                if (sign != null)
                {
                    wsadetails.SignatureId = sign.Id;
                }
                //wsadetails.SignatureId = app.SignatureId;
                if (approversigns.Count > 0)
                {
                    var approversign = approversigns.FirstOrDefault();
                    var previousService = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == applicationId && x.TemplateCode == "PREPARE_FINAL_OFFER" && x.NtsType == NtsTypeEnum.Service);
                    var service = await _recTaskBusiness.GetStepTaskListByService(previousService.Id);
                    var hr = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_RECRUITER").FirstOrDefault();
                    var hod = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_HR_HEAD").FirstOrDefault();
                    var ed = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_ED").FirstOrDefault();
                    if (approversign.ApprovarSignature1.IsNotNullAndNotEmpty())
                    {
                        if (hr.IsNotNull() && hr.TaskStatusCode == "COMPLETED")
                        {
                            wsadetails.SignatureId1 = approversign.ApprovarSignature1;
                        }
                    }
                    if (approversign.ApprovarSignature2.IsNotNullAndNotEmpty())
                    {
                        if (hod.IsNotNull() && hod.TaskStatusCode == "COMPLETED")
                        {
                            wsadetails.SignatureId2 = approversign.ApprovarSignature2;
                        }
                    }
                    if (approversign.ApprovarSignature3.IsNotNullAndNotEmpty())
                    {
                        if (ed.IsNotNull() && ed.TaskStatusCode == "COMPLETED")
                        {
                            wsadetails.SignatureId3 = approversign.ApprovarSignature3;
                        }
                    }
                }
                return View("WorkerOffer", wsadetails);
            }
            else if (jd.Code == "Welder")
            {
                var wsadetails = await _applicationBusiness.GetWSADetails(applicationId);
                //wsadetails.CurrentDate = DateTime.UtcNow;
                wsadetails.BasicInWords = new NumberToEnglishWordConverter.NumberToEnglishWordConverter().changeCurrencyToWords(wsadetails.Basic);
                if (sign != null)
                {
                    wsadetails.SignatureId = sign.Id;
                }
                //wsadetails.SignatureId = app.SignatureId;
                if (approversigns.Count > 0)
                {
                    var approversign = approversigns.FirstOrDefault();
                    var previousService = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == applicationId && x.TemplateCode == "PREPARE_FINAL_OFFER" && x.NtsType == NtsTypeEnum.Service);
                    var service = await _recTaskBusiness.GetStepTaskListByService(previousService.Id);
                    var hr = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_RECRUITER").FirstOrDefault();
                    var hod = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_HR_HEAD").FirstOrDefault();
                    var ed = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_ED").FirstOrDefault();
                    if (approversign.ApprovarSignature1.IsNotNullAndNotEmpty())
                    {
                        if (hr.IsNotNull() && hr.TaskStatusCode == "COMPLETED")
                        {
                            wsadetails.SignatureId1 = approversign.ApprovarSignature1;
                        }
                    }
                    if (approversign.ApprovarSignature2.IsNotNullAndNotEmpty())
                    {
                        if (hod.IsNotNull() && hod.TaskStatusCode == "COMPLETED")
                        {
                            wsadetails.SignatureId2 = approversign.ApprovarSignature2;
                        }
                    }
                    if (approversign.ApprovarSignature3.IsNotNullAndNotEmpty())
                    {
                        if (ed.IsNotNull() && ed.TaskStatusCode == "COMPLETED")
                        {
                            wsadetails.SignatureId3 = approversign.ApprovarSignature3;
                        }
                    }
                }
                return View("WelderOffer", wsadetails);
            }
            else if (jd.Code == "DriverAndOperator")
            {
                var appdetails = await _applicationBusiness.GetNameById(applicationId);
                if (sign != null)
                {
                    appdetails.SignatureId = sign.Id;
                }
                // appdetails.SignatureId = app.SignatureId;
                if (approversigns.Count > 0)
                {
                    var approversign = approversigns.FirstOrDefault();
                    var previousService = await _recTaskBusiness.GetSingle(x => x.ReferenceTypeId == applicationId && x.TemplateCode == "PREPARE_FINAL_OFFER" && x.NtsType == NtsTypeEnum.Service);
                    var service = await _recTaskBusiness.GetStepTaskListByService(previousService.Id);
                    var hr = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_RECRUITER").FirstOrDefault();
                    var hod = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_HR_HEAD").FirstOrDefault();
                    var ed = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_ED").FirstOrDefault();
                    if (approversign.ApprovarSignature1.IsNotNullAndNotEmpty())
                    {
                        if (hr.IsNotNull() && hr.TaskStatusCode == "COMPLETED")
                        {
                            appdetails.SignatureId1 = approversign.ApprovarSignature1;
                        }
                    }
                    if (approversign.ApprovarSignature2.IsNotNullAndNotEmpty())
                    {
                        if (hod.IsNotNull() && hod.TaskStatusCode == "COMPLETED")
                        {
                            appdetails.SignatureId2 = approversign.ApprovarSignature2;
                        }
                    }
                    if (approversign.ApprovarSignature3.IsNotNullAndNotEmpty())
                    {
                        if (ed.IsNotNull() && ed.TaskStatusCode == "COMPLETED")
                        {
                            appdetails.SignatureId3 = approversign.ApprovarSignature3;
                        }
                    }
                }
                return View("DriverOffer", appdetails);
            }
            var offerdetails = await _applicationBusiness.GetOfferDetails(applicationId);
            offerdetails.ContractYear = Convert.ToDateTime(offerdetails.CreatedDate).Year;
            offerdetails.FullName = String.Concat(offerdetails.FirstName, ' ', offerdetails.MiddleName, ' ', offerdetails.LastName);
            offerdetails.CandJoiningDate = offerdetails.JoiningNotLaterThan;
           // offerdetails.OfferCreatedDate = offerdetails.CreatedDate.ToString("dd MMMM yyyy");            
            offerdetails.OfferCreatedDate = offerdetails.OfferCreatedDate;
            if (sign != null)
            {
                offerdetails.SignatureId = sign.Id;
            }
            //offerdetails.SignatureId = app.SignatureId;
           // return View("StaffOffer", offerdetails);
            return Redirect("~/Cms/OfferReport?appId="+ applicationId);
        }
        //public async Task<IActionResult> ServiceAgreement(string applicationId= "c6e7c2c6-1984-4c34-be31-4bd4cb5d7949")
        //{
        //    var appdetails = await _applicationBusiness.GetNameById(applicationId);
        //    return View("ServiceAgreement", appdetails);
        //}
        //public async Task<IActionResult> WorkerServiceAgreement(string applicationId)
        //{
        //    var wsadetails = await _applicationBusiness.GetWSADetails(applicationId);
        //    wsadetails.CurrentDate = DateTime.UtcNow;            
        //    wsadetails.BasicInWords = new NumberToEnglishWordConverter.NumberToEnglishWordConverter().changeCurrencyToWords(wsadetails.Basic);
        //    return View("WorkerServiceAgreement", wsadetails);
        //}

        public async Task<IActionResult> ConfidentialityAgreement(string appId)
        {
            var cadetails = await _applicationBusiness.GetConfidentialAgreementDetails(appId);
           // cadetails.FullName = String.Concat(cadetails.FirstName, ' ', cadetails.MiddleName, ' ', cadetails.LastName);            
            return View("ConfidentialityAgreement", cadetails);
        }

        public async Task<IActionResult> CompetenceMatrix(string appId)
        {
            if (appId != null)
            {
                var cmdetails = await _applicationBusiness.GetCompetenceMatrixDetails(appId);
                cmdetails.FullName = String.Concat(cmdetails.FirstName, ' ', cmdetails.MiddleName, ' ', cmdetails.LastName);
                cmdetails.CandJoiningDate = cmdetails.JoiningNotLaterThan;
                return View("CompetenceMatrix", cmdetails);
            }
            else
            {
                return View("CompetenceMatrix", new ApplicationViewModel());
            }
        }

        //public async Task<IActionResult> StaffJoiningReport(string appid)
        //{
        //    var staffDetails = await _applicationBusiness.GetStaffJoiningDetails(appid);            
        //    staffDetails.JoiningTime = String.Format("{0:HH:mm}", staffDetails.CandJoiningDate);           
        //    return View(staffDetails);
        //}

    }
}

