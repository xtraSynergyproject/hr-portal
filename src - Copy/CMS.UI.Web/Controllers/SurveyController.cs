using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CMS.Business;
using CMS.Common;
using CMS.Common.Utilities;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
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
