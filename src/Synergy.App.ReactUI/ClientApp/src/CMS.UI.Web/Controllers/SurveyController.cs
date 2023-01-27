using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.Web;
using Hangfire;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web
{
    public class SurveyController : ApplicationController
    {
        private readonly IPortalBusiness _portalBusiness;
        public SurveyController(IPortalBusiness portalBusiness)
        {
            _portalBusiness = portalBusiness;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string surveyCode = null)
        {
            var survey = new SurveyScheduleViewModel();
            survey.SurveyCode = surveyCode;
            var portal = await _portalBusiness.GetSingleGlobal(x => x.Name == "TalentAssessment");
            if (portal != null)
            {
                survey.PortalLogoId = portal.LogoId;
            }
            return View("~/Areas/TAS/Views/TalentAssessment/SurveyRegister.cshtml", survey);
            //return RedirectToAction("SurveyRegister", "TalentAssessment", new { @area = "TAS" });
        }

    }
}
