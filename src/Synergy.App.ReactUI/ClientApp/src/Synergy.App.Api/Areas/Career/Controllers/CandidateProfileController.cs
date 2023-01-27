using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Career.Controllers
{
    [Route("career/candidateProfile")]
    [ApiController]
    public class CandidateProfileController : ApiController
    {
        private readonly ICareerPortalBusiness _careerPortalBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly INoteBusiness _noteBusiness;
        private readonly ILOVBusiness _lOVBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public CandidateProfileController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
           ICareerPortalBusiness careerPortalBusiness, ICmsBusiness cmsBusiness, ITableMetadataBusiness tableMetadataBusiness,
           INoteBusiness noteBusiness
            , ILOVBusiness lOVBusiness,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _careerPortalBusiness = careerPortalBusiness;
            _cmsBusiness = cmsBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _noteBusiness = noteBusiness;
            _lOVBusiness = lOVBusiness;
        }

        [HttpGet]
        [Route("PrintableView")]
        public async Task<IActionResult> PrintableView(string candidateProfileId, string applicationId)
        {
            //var exist = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == candidateProfileId);
            if (applicationId != null)
            {
                var appmodel = await _careerPortalBusiness.GetApplicationDetails(candidateProfileId, null);
                appmodel.Criterias = await _careerPortalBusiness.GetCriteriaData(applicationId, "Criteria");
                appmodel.Criterias = await _careerPortalBusiness.GetApplicationJobCriteriaList(applicationId, "Criteria");


                //appmodel.Criterias = await _applicationJobCriteriaBusiness.GetList(x => x.ApplicationId == applicationId && x.Type == "Criteria");
                appmodel.Skills = await _careerPortalBusiness.GetCriteriaData(applicationId, "Skills");
                appmodel.Skills = await _careerPortalBusiness.GetApplicationJobCriteriaList(applicationId, "Skills");

                //appmodel.Skills = await _applicationJobCriteriaBusiness.GetList(x => x.ApplicationId == applicationId && x.Type == "Skills");
                appmodel.OtherInformations = await _careerPortalBusiness.GetCriteriaData(applicationId, "OtherInformation");
                appmodel.OtherInformations = await _careerPortalBusiness.GetApplicationJobCriteriaList(applicationId, "OtherInformation");

                //appmodel.OtherInformations = await _applicationJobCriteriaBusiness.GetList(x => x.ApplicationId == applicationId && x.Type == "OtherInformation");

                return Ok( appmodel);
            }
            else
            {
                var candmodel = await _careerPortalBusiness.GetCandProfileDetails(candidateProfileId);
                if (candmodel == null)
                {
                    var model = new CandidateProfileViewModel();
                    return Ok( model);
                }
                else
                {
                    return Ok( candmodel);
                }
            }
        }

    }
}
