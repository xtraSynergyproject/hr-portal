using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace CMS.UI.Web.Areas.TAS.Controllers
{
    [Route("TAS/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        [HttpGet]
        [Route("GetAssessmentReport/{userId}")]
        public async Task<IActionResult> GetAssessmentReport(string userId)
        {
            try
            {
                var _talentAssessmentBusiness = _serviceProvider.GetService<ITalentAssessmentBusiness>();
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
                var _sponsorBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();

                var model = await _talentAssessmentBusiness.GetAssessmentReportDataPDFForUser(userId);
                var modelReport = new AssessmentPDFReportViewModel();
                modelReport.CurrentDateText = DateTime.Today.ToDefaultDateFormat();
                modelReport.UserName = model.UserName;
                modelReport.JobName = model.JobName;
                //modelReport.TotalScore = model.TotalScore.HasValue ? model.TotalScore : 0.0;
                modelReport.TechnicalQuestionnaireScore = model.TechnicalQuestionnaireScore ?? 0;
                modelReport.CaseAnalysisScore = model.CaseAnalysisScore ?? 0;
                modelReport.TechnicalInterviewScore = model.TechnicalInterviewScore ?? 0;
                modelReport.TechnicalQuestionCount = model.TechnicalQuestionCount;
                modelReport.CaseStudyQuestionCount = model.CaseStudyQuestionCount;

                var user = await _userBusiness.GetSingleById(userId);
                var company = await _companyBusiness.GetSingleById(user.CompanyId);
                modelReport.OrganizationName = company.Name;
                if (company.LogoFileId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(company.LogoFileId);
                    if (bytes.Length>0)
                    {
                        modelReport.OrganizationLogo = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                }
                var sponsor = await _sponsorBusiness.GetSingleById(user.SponsorId);

                if (sponsor!=null &&  sponsor.ImageId.IsNotNullAndNotEmpty())
                {
                    var spbytes = await _fileBusiness.GetFileByte(sponsor.ImageId);
                    if (spbytes.Length > 0)
                    {
                        modelReport.SponsorLogo = Convert.ToBase64String(spbytes, 0, spbytes.Length);
                    }

                }
                return Ok(modelReport);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetAssessmentResultReport/{userId}")]
        public async Task<IActionResult> GetAssessmentResultReport(string userId)
        {
            try
            {
                var _talentAssessmentBusiness = _serviceProvider.GetService<ITalentAssessmentBusiness>();
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
                var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();

                var modelReport = await _talentAssessmentBusiness.GetManageAssessmentResult(userId);
                modelReport.CurrentDateText = DateTime.Today.ToDefaultDateFormat();
                modelReport.Score = modelReport.Score.IsNotNull() ? modelReport.Score : 0;
                
                var user = await _userBusiness.GetSingleById(userId);
                var company = await _companyBusiness.GetSingleById(user.CompanyId);
                if (company.LogoFileId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(company.LogoFileId);
                    if (bytes.Length > 0)
                    {
                        modelReport.OrganizationLogo = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                }
                return Ok(modelReport);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetAssessmentResultItems/{userId}")]
        public async Task<IActionResult> GetAssessmentResultItems(string userId)
        {
            try
            {
                var _talentAssessmentBusiness = _serviceProvider.GetService<ITalentAssessmentBusiness>();
                var model = await _talentAssessmentBusiness.GetManageAssessmentResultList(userId);
                var data = model.Where(x => x.IncludeInReport == null || x.IncludeInReport == true).ToList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetAssessmentFinalReport/{userId}")]
        public async Task<IActionResult> GetAssessmentFinalReport(string userId)
        {
            try
            {
                var _userReportBusiness = _serviceProvider.GetService<IUserReportBusiness>();
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
                var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();

                var model = await _userReportBusiness.GetUserReportData(userId);


                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetAssessmentSubordinate/{userId}")]
        public async Task<IActionResult> GetAssessmentSubordinate(string userId)
        {
            try
            {
                var _userReportBusiness = _serviceProvider.GetService<IUserReportBusiness>();
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
                var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();

                var model = await _userReportBusiness.GetUserReportData(userId);
                
               
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
