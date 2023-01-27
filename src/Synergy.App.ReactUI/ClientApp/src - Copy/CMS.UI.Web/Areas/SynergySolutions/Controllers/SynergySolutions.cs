using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.SynergySolutions.Controllers
{
    [Area("SynergySolutions")]
    public class SynergySolutions : ApplicationController
    {
        INotificationBusiness _notificationBusiness;
        IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        public SynergySolutions(INotificationBusiness notificationBusiness, IUserBusiness userBusiness, IUserContext userContext) 
        {
            _notificationBusiness = notificationBusiness;
            _userBusiness = userBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Home()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        
        public IActionResult AboutUs()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult RequestConsultation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(RequestConsultationViewModel data)
        {
            //if (data.Captcha != "7")
            //{
            //    return Json(new { success = false, error = "Wrong answer! Please enter right answer." });
            //}


            var notificationTemplateModel = await _notificationBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_CONSULTATION_MAIL");
            if (notificationTemplateModel.IsNotNull())
            {
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{FirstName}}", data.FirstName);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{LastName}}", data.LastName);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Email}}", data.Email);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{ContactNumber}}", data.ContactNumber);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Country}}", data.Country);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{ProductIntrested}}", data.ProductIntrested);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{CompanyName}}", data.CompanyName);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{JobTitle}}", data.JobTitle);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{NumberOfEmployees}}", data.NumberOfEmployees);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Message}}", data.Message);
                var user = await _userBusiness.GetSingle(x => x.Email == "admin@Synergy.com");
                var viewModel = new NotificationViewModel()
                {
                    To = "info@extranet.ae",
                    //ToUserId = model.Id,
                    // FromUserId = model.CreatedBy,
                    Recipient= user,
                    Subject = notificationTemplateModel.Subject,
                    Body = notificationTemplateModel.Body,
                    SendAlways = true,
                    NotifyByEmail = true,
                     DynamicObject = data
                };
                var result=await _notificationBusiness.Create(viewModel);
                if (result.IsSuccess) 
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false,error= result.Messages.ToHtmlError() });
            }
            return Json(new { success = false});
        }

        [HttpPost]
        public async Task<IActionResult> SendContactUsEmail(RequestConsultationViewModel data)
        {

            var notificationTemplateModel = await _notificationBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_CONTACTUS_MAIL");
            if (notificationTemplateModel.IsNotNull())
            {
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Name}}", data.FirstName);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Email}}", data.Email);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Phone}}", data.ContactNumber);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Subject}}", data.Subject);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Message}}", data.Message);
                var user = await _userBusiness.GetSingle(x => x.Email == "admin@Synergy.com");
                var viewModel = new NotificationViewModel()
                {
                    To = "info@extranet.ae",
                    //ToUserId = model.Id,
                    // FromUserId = model.CreatedBy,
                    Recipient = user,
                    Subject = notificationTemplateModel.Subject,
                    Body = notificationTemplateModel.Body,
                    SendAlways = true,
                    NotifyByEmail = true,
                    DynamicObject = data
                };
                var result = await _notificationBusiness.Create(viewModel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = result.Messages.ToHtmlError() });
            }
            return Json(new { success = false });
        }
        public IActionResult ProjectManagementFeatures()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult DocumentManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult ContactUs()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult ContactUsIndia()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult AnalyticsandBI()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult AnalyticsData()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult ArtificialIntelligence()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult BusinessProcessAutomation()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Conferencing()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult DMSFeatures()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult EdmsArchitecture()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult EdmsModuleBenefits()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Egovernance()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Features()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult HRSolutions()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult IntegratedIntelligencePortal()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult InternalSocialMedia()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult MobileApp()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult OtherSolutions()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult ProjectManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Services()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult UIBuilder()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult UnifiedHelpdesk()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Worksnaps()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

    }

    public class RequestConsultationViewModel
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProductIntrested { get; set; }
        public string ContactNumber { get; set; }
        public string NumberOfEmployees { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string Captcha { get; set; }

        
    }
}
