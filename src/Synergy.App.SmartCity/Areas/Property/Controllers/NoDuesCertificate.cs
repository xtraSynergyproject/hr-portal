using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Synergy.App.Business;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using System.Net.Http.Headers;
//using Kendo.Mvc.UI;
//using Kendo.Mvc.Extensions;
using System.IO;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.Property.Controllers
{
    [Area("Property")]
    public class NoDuesCertificateController : ApplicationController
    {
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPropertyBusiness _propertyBusiness;
        private static IUserContext _userContext;
        private readonly IEmailBusiness _emailBusiness;
        private readonly INotificationTemplateBusiness _notificationTemplateBusiness;
        public NoDuesCertificateController(IUserContext userContext, IServiceBusiness serviceBusiness,
            IPropertyBusiness propertyBusiness, IEmailBusiness emailBusiness, INotificationTemplateBusiness notificationTemplateBusiness)
        {
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _propertyBusiness = propertyBusiness;
            _emailBusiness = emailBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
        }


        public IActionResult Index(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);
        }

        public async Task<IActionResult> ReadNDCServiceDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, bool isDisableCreate = false, string mode = null, string templatecode = null)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);

            if (isDisableCreate)
            {
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate));
                return j;
            }
            else
            {
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                return j;
            }
        }

        public async Task<IActionResult> ReadNDCServiceDataOverdue(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
            var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderBy(x => x.CreatedDate));
            return j;
        }

        public async Task<IActionResult> ReadNDCServiceDataCompleted(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
            var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_REJECT").OrderByDescending(x => x.LastUpdatedDate));
            return j;
        }

        public async Task<IActionResult> ReadNDCServiceDataClosed(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
            var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").OrderByDescending(x => x.LastUpdatedDate));
            return j;
        }

        public async Task<IActionResult> DownloadNoDueCertificate(string propertyServiceId)
        {
            var model = await _propertyBusiness.GetNDCDetails(propertyServiceId);

            return View(model);
        }

        public async Task<IActionResult> SendOTP(string serviceId)
        {
            Random rand = new Random();
            string otp = Convert.ToString(rand.Next(100000, 999999));

            DateTime currentTime = DateTime.Now;
            DateTime expiryDateTime = currentTime.AddMinutes(5);

            await _propertyBusiness.UpdateOTP(serviceId, otp, expiryDateTime);

            var notificationTemplateModel = await _notificationTemplateBusiness.GetSingle(x => x.Code == "DOWNLOAD_NDC_OTP");

            EmailViewModel emailModel = new EmailViewModel();
            if (notificationTemplateModel.IsNotNull())
            {
                var body = notificationTemplateModel.Body;
                if (body.Contains("{{user-name}}"))
                {
                    body = body.Replace("{{user-name}}", _userContext.Name);
                }
                if (body.Contains("{{otp}}"))
                {
                    body = body.Replace("{{otp}}", otp);
                }

                emailModel.Subject = notificationTemplateModel.Subject;
                emailModel.Body = body;
            }
            else
            {
                emailModel.Subject = "OTP for Download No Dues Certificate";
                emailModel.Body = "Dear " + _userContext.Name + "<br/> Greetings! <br/><br/> You have requested for a new OTP to download No Dues Certificate.<br/> " +
                    "Enter the following OTP to proceed.<br/>OTP: " + otp + "<br/>(This OTP is valid for next 5 minutes)";
            }
            emailModel.CompanyId = _userContext.CompanyId;
            emailModel.To = _userContext.Email;
            emailModel.DataAction = DataActionEnum.Create;
            emailModel.Id = Guid.NewGuid().ToString();
            var resultemail = await _emailBusiness.SendMailAsync(emailModel);
            if (resultemail.IsSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Try after some time" });
            }

        }

        public async Task<IActionResult> ValidateOTP(string serviceId, string curOtp)
        {
            var result = await _propertyBusiness.ValidateOTP(serviceId, curOtp);

            if (result.Otp == curOtp && result.OtpExpiryDate >= DateTime.Now)
            {
                return Json(new { success = true, data = result });
            }
            else if (result.Otp == curOtp && result.OtpExpiryDate < DateTime.Now)
            {
                return Json(new { success = false, errormsg = "OTP has been Expired, Resend and try again" });
            }
            else
            {
                return Json(new { success = false, errormsg = "Incorrect OTP, Enter valid OTP" });
            }


        }
    }

}
