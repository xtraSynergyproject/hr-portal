using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace CMS.Web.Areas.Rec.Controllers
{
    [Route("rec/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private IMapper _autoMapper;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, IMapper autoMapper)
        {
            _serviceProvider = serviceProvider;
            _autoMapper = autoMapper;

        }
        // Old GEtStaff function
        #region
        //[HttpGet]
        //[Route("GetStaffOfferLetter/{applicationId}")]
        //public async Task<IActionResult> GetStaffOfferLetter(string applicationId)
        //{
        //    var _applicationBusiness = _serviceProvider.GetService<IApplicationBusiness>();
        //    var _jdBusiness = _serviceProvider.GetService<IJobAdvertisementBusiness>();
        //    var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
        //    var _signatureBusiness = _serviceProvider.GetService<IApprovarSignatureBusiness>();
        //    var _taskBusiness = _serviceProvider.GetService<IRecTaskBusiness>();
        //    var offerdetails = new ApplicationViewModel();
        //    try
        //    {
        //        var app = await _applicationBusiness.GetSingleById(applicationId);
        //        var jd = await _jdBusiness.GetJobManpowerType(app.JobId);
        //        //var sign = await _applicationBusiness.GetUserSign();
        //        if (jd.Code == "Staff")
        //        {
                   
        //            offerdetails = await _applicationBusiness.GetOfferDetails(applicationId);
        //            offerdetails.ContractYear = Convert.ToDateTime(offerdetails.CreatedDate).Year;
        //            offerdetails.FullName = String.Concat(offerdetails.FirstName, ' ', offerdetails.MiddleName, ' ', offerdetails.LastName);
        //            offerdetails.CandJoiningDate = offerdetails.JoiningNotLaterThan;
        //            // offerdetails.OfferCreatedDate = offerdetails.CreatedDate.ToString("dd MMMM yyyy");            
        //            offerdetails.OfferCreatedDate = offerdetails.OfferCreatedDate;

        //            var signs = await _signatureBusiness.GetList();
                    
        //            if (signs.Count>0)
        //            {
        //                var sign = signs.FirstOrDefault();
        //                var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == applicationId && x.TemplateCode == "PREPARE_FINAL_OFFER" && x.NtsType == NtsTypeEnum.Service);
        //                var service = await _taskBusiness.GetStepTaskListByService(previousService.Id);
        //                var hr = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_RECRUITER").FirstOrDefault();
        //                var hod = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_HR_HEAD").FirstOrDefault();
        //                var ed = service.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_ED").FirstOrDefault();
        //                if (sign.ApprovarSignature1.IsNotNullAndNotEmpty())
        //                {
        //                    if (hr.IsNotNull() && hr.TaskStatusCode == "COMPLETED")
        //                    {
        //                        var bytes = await _fileBusiness.GetFileByte(sign.ApprovarSignature1);
        //                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        //                        offerdetails.SignatureId = base64String;
        //                    }
        //                }
        //                if (sign.ApprovarSignature2.IsNotNullAndNotEmpty())
        //                {
        //                    if (hod.IsNotNull() && hod.TaskStatusCode == "COMPLETED")
        //                    {
        //                        var bytes = await _fileBusiness.GetFileByte(sign.ApprovarSignature2);
        //                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        //                        offerdetails.SignatureId2 = base64String;
        //                    }
        //                }
        //                if (sign.ApprovarSignature3.IsNotNullAndNotEmpty())
        //                {
        //                    if (ed.IsNotNull() && ed.TaskStatusCode == "COMPLETED")
        //                    {
        //                        var bytes = await _fileBusiness.GetFileByte(sign.ApprovarSignature3);
        //                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        //                        offerdetails.SignatureId3 = base64String;
        //                    }
        //                }
        //            }
        //            if (offerdetails.OfferCreatedDate.IsNotNull())
        //            {
        //                offerdetails.OfferCreatedDateText = offerdetails.OfferCreatedDate.ToDefaultDateFormat();
        //            }
        //            if (offerdetails.CandJoiningDate.IsNotNull())
        //            {
        //                offerdetails.CandJoiningDateText = offerdetails.CandJoiningDate.ToDefaultDateFormat();
        //            }
        //            if (!offerdetails.IsTrainee.IsTrue())
        //            {
        //                offerdetails.IsTrainee = false;
        //            }
        //            if (!offerdetails.IsLocalCandidate.IsTrue())
        //            {
        //                offerdetails.IsLocalCandidate = false;
        //            }

        //            if (offerdetails.FullName.IsNotNullAndNotEmpty())
        //            {
        //                //offerdetails.FullName = char.ToUpper(offerdetails.FullName[0]) + offerdetails.FullName.Substring(1);
        //                offerdetails.FullName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.FullName.ToLower());
        //                var name = offerdetails.FullName.ToLower();
        //                if (offerdetails.TitleName.IsNotNullAndNotEmpty())
        //                {
        //                    name = offerdetails.TitleName.ToLower() + " " + offerdetails.FullName.ToLower();
        //                }
        //                offerdetails.FullNameCaps = CultureInfo.CurrentCulture.TextInfo.ToUpper(name);
        //            }
        //            if (offerdetails.FirstName.IsNotNullAndNotEmpty())
        //            {
        //                //offerdetails.FullName = char.ToUpper(offerdetails.FullName[0]) + offerdetails.FullName.Substring(1);
        //                offerdetails.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.FirstName.ToLower());
        //            }
        //            if (offerdetails.PermanentAddressHouse.IsNotNullAndNotEmpty())
        //            {
        //                offerdetails.PermanentAddressHouse = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.PermanentAddressHouse.ToLower());
        //            }
        //            if (offerdetails.PermanentAddressStreet.IsNotNullAndNotEmpty())
        //            {
        //                offerdetails.PermanentAddressStreet = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.PermanentAddressStreet.ToLower());
        //            }
        //            if (offerdetails.PermanentAddressCity.IsNotNullAndNotEmpty())
        //            {
        //                offerdetails.PermanentAddressCity = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.PermanentAddressCity.ToLower());
        //            }
        //            if (offerdetails.PermanentAddressState.IsNotNullAndNotEmpty())
        //            {
        //                offerdetails.PermanentAddressState = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.PermanentAddressState.ToLower());
        //            }
        //            if (offerdetails.PermanentAddressCountryName.IsNotNullAndNotEmpty())
        //            {
        //                offerdetails.PermanentAddressCountryName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(offerdetails.PermanentAddressCountryName.ToLower());
        //            }

        //            if (offerdetails.BasicPay.IsNotNull())
        //            {
        //                offerdetails.BasicPayText = String.Format("{0:#,##0.##}", offerdetails.BasicPay);
        //            }
        //            if (offerdetails.ProfessionalAllowance.IsNotNull())
        //            {
        //                offerdetails.ProfessionalAllowanceText = String.Format("{0:#,##0.##}", offerdetails.ProfessionalAllowance);
        //            }
        //            if (offerdetails.FamilyHRA.IsNotNull())
        //            {
        //                offerdetails.FamilyHRAText = String.Format("{0:#,##0.##}", offerdetails.FamilyHRA);
        //            }
        //            if (offerdetails.UtilityAllowance.IsNotNull())
        //            {
        //                offerdetails.UtilityAllowanceText = String.Format("{0:#,##0.##}", offerdetails.UtilityAllowance);
        //            }
        //            if (offerdetails.FRA.IsNotNull())
        //            {
        //                offerdetails.FRAText = String.Format("{0:#,##0.##}", offerdetails.FRA);
        //            }

        //        }
        //        return Ok(offerdetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        #endregion
        [HttpGet]
        [Route("GetOfferLetter/{appId}")]
        public async Task<IActionResult> GetOfferLetter(string appId)
        {
            var _applicationBusiness = _serviceProvider.GetService<IRecruitmentTransactionBusiness>();          
           // var offerdetails = new RecOfferLetterReportViewModel();
            try
            {
                var detail = await _applicationBusiness.GetApplicationforOfferById(appId);
                var elementData = await _applicationBusiness.GetElementPayData(appId);
               // offerdetails= _autoMapper.Map<CandidateProfileViewModel, RecOfferLetterReportViewModel>(detail, offerdetails);
                if (elementData.Count > 0)
                {
                    detail.ElementList = elementData.ToList();
                }
                return Ok(detail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetOfferLetterElement/{appId}")]
        public async Task<IActionResult> GetOfferLetterElement(string appId)
        {
            var _applicationBusiness = _serviceProvider.GetService<IRecruitmentTransactionBusiness>();
           // var offerdetails = new RecOfferLetterReportViewModel();
            try
            {
                var elementData = await _applicationBusiness.GetElementPayData(appId);
                var gross = elementData.Where(x => x.ElementName != "PF" && x.ElementName != "ESI" && x.ElementName != "Professional Tax" && x.ElementName != "Gratuity").Sum(x => x.Value);
                gross = gross.HasValue ? gross.Value : 0;
                elementData.Add(new RecCandidatePayElementViewModel { ElementName = "Gross", Value =  gross});
                var deduction = elementData.Where(x => x.ElementName == "PF" && x.ElementName == "ESI" && x.ElementName == "Professional Tax").Sum(x => x.Value);
                deduction = deduction.HasValue ? deduction.Value : 0;
                var gratuity = elementData.FirstOrDefault(x => x.ElementName == "Gratuity");
                double? graValue = 0;
                double? bonusValue = 0;
                if (gratuity.IsNotNull())
                {
                     graValue = gratuity.Value;
                }
                var bonus = elementData.FirstOrDefault(x => x.ElementName == "Bonus");
                if (bonus.IsNotNull())
                {
                     bonusValue = bonus.Value;
                }
                elementData.Add(new RecCandidatePayElementViewModel { ElementName = "Deduction -B", Value = deduction });
                elementData.Add(new RecCandidatePayElementViewModel { ElementName = "Net Take Home-(A-B)=(C)", Value = gross - deduction });

                elementData.Add(new RecCandidatePayElementViewModel { ElementName = "Cost to Company (CTC) Per Month", Value = gross+ graValue+bonusValue});
                return Ok(elementData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
