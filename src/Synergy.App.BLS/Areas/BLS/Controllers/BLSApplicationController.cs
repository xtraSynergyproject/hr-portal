using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
//using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.WebUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using IronBarCode;
//usin ronBarCode;
using ZXing.Common;

namespace Synergy.App.BLS.Areas.Controllers
{
    [Area("BLS")]
    public class BLSApplicationController : ApplicationController
    {
        private IServiceBusiness _serviceBusiness;
        private INoteBusiness _noteBusiness;
        private IUserContext _userContext;
        private ILOVBusiness _lovBusiness;
        private IUserBusiness _userBusiness;
        private IBLSBusiness _blsBusiness;
        private IFileBusiness _fileBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly INotificationTemplateBusiness _notificationTemplateBusiness;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IUserPortalBusiness _userPortalBusiness;
        private readonly IQRCodeBusiness _qrCodeBusiness;
        private readonly IServiceProvider _sp;
        private readonly ITemplateBusiness _templateBusiness;

        public BLSApplicationController(IServiceBusiness serviceBusiness
            , IUserContext userContext
            , ILOVBusiness lovBusiness
            , IUserBusiness userBusiness
            , IBLSBusiness blsBusiness
            , ICmsBusiness cmsBusiness
            , IEmailBusiness emailBusiness
            , IUserPortalBusiness userPortalBusiness
            , INotificationTemplateBusiness notificationTemplateBusiness
            , IDNTCaptchaValidatorService validatorService
            , IFileBusiness fileBusiness
            , INoteBusiness noteBusiness
            , IQRCodeBusiness qrCodeBusiness
            , IServiceProvider sp
            , ITemplateBusiness templateBusiness)
        {
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _lovBusiness = lovBusiness;
            _userBusiness = userBusiness;
            _blsBusiness = blsBusiness;
            _emailBusiness = emailBusiness;
            _cmsBusiness = cmsBusiness;
            _notificationTemplateBusiness = notificationTemplateBusiness;
            _validatorService = validatorService;
            _fileBusiness = fileBusiness;
            _noteBusiness = noteBusiness;
            _userPortalBusiness = userPortalBusiness;
            _qrCodeBusiness = qrCodeBusiness;
            _sp = sp;
            _templateBusiness = templateBusiness;
        }

        public async Task<IActionResult> BLSVisaAppointment(string msg = null)
        {
            //if (_userContext.UserId.IsNullOrEmpty() || _userContext.IsGuestUser)
            //{
            //    var portal = await _userBusiness.GetSingle<PortalViewModel, Portal>(x => x.Name == "BLSCustomer");
            //    return Redirect("Account/Login", new { area = "", portalId = portal?.Id });
            //}
            var model = new BLSVisaAppointmentViewModel();
            var refNo = "";
            var serId = "";
            if (msg.IsNotNullAndNotEmpty())
            {
                var values = msg.Split('|');
                serId = values[0];
                refNo = values[1];
            }

            if (refNo.IsNotNullAndNotEmpty())
            {
                var appdata = await _blsBusiness.GetAppointmentDetailsByServiceId(serId);
                model = appdata.FirstOrDefault();
                var data = await _blsBusiness.GetSettingsData();
                ViewBag.EmailSettings = data.AppointmentEmailInstruction;
                var settingsList = data.AppointmentInstruction.Split(',');
                ViewBag.SettingsList = settingsList;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.ServiceId = Guid.NewGuid().ToString();

                var captcha = await _userBusiness.Create<CaptchaViewModel, Captcha>(new CaptchaViewModel
                {
                    RetryCount = -1,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    ReferenceId = model.ServiceId,
                    ReferenceType = ReferenceTypeEnum.NTS_Service.ToString()
                });
                var appointment = await _userBusiness.Create<BLSAppointmentViewModel, BLSAppointment>(new BLSAppointmentViewModel
                {
                    CaptchaId = captcha.Item.Id,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    ServiceId = model.ServiceId
                });
                model.AppointmentId = appointment.Item.Id;
                var data = await _blsBusiness.GetSettingsData();
                ViewBag.EmailSettings = data.AppointmentEmailInstruction;
                var settingsList = data.AppointmentInstruction.Split(',');
                ViewBag.SettingsList = settingsList;
                ViewBag.CaptchaParam = string.Concat("data=", System.Web.HttpUtility.UrlEncode(Helper.Encrypt(appointment.Item.CaptchaId)));

            }

            return View(model);
        }
        public IActionResult EmptyResult()
        {
            return PartialView("_EmptyDetectionResult");
        }
        [HttpPost]
        public async Task<IActionResult> SubmitLivenessDetection()
        {
            try
            {
                var appointmentId = Convert.ToString(Request.Form["appointmentId"]);
                var appointment = await _userBusiness.GetSingleById<BLSAppointmentViewModel, BLSAppointment>(appointmentId);
                if (appointment == null)
                {
                    return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel { ErrorString = "Invalid Appointment Id" });
                }
                var appId = "6816efca-1ae9-46f6-adbb-9167f2107db2";
                var appSecret = "09qWhRMlrjWoAcz/xW4P2k7i";
                var endPoint = "https://bws.bioid.com/extension/";
                byte[] image1 = null, image2 = null, image3 = null;

                var liveimage1 = Request.Form.Files["image1"];
                if (liveimage1 != null)
                {
                    using MemoryStream ms = new();
                    await liveimage1.CopyToAsync(ms);
                    image1 = ms.ToArray();
                }
                var liveimage2 = Request.Form.Files["image2"];
                if (liveimage2 != null)
                {
                    using MemoryStream ms = new();
                    await liveimage2.CopyToAsync(ms);
                    image2 = ms.ToArray();
                }

                if (image1 == null || image2 == null)
                {
                    return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel { ErrorString = "At least one image was not uploaded completely!" });
                }

                // for additional hints we need to know is it a mobile device or not
                bool isMobile = bool.Parse(Request.Form["isMobile"]);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", EncodeCredentials(appId, appSecret));
                var requestBody = new
                {
                    liveimage1 = "data:image/png;base64," + Convert.ToBase64String(image1),
                    liveimage2 = "data:image/png;base64," + Convert.ToBase64String(image2)
                };
                using var content = JsonContent.Create(requestBody);
                using var response = await httpClient.PostAsync($"{endPoint}livedetection?state=details", content);

                string msg = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest && !string.IsNullOrWhiteSpace(msg))
                    {
                        var json = JsonDocument.Parse(msg);
                        if (json.RootElement.TryGetProperty("Message", out JsonElement prop))
                        {
                            msg = prop.GetString();
                        }

                        return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel { ErrorString = msg.ErrorFromErrorCode() });
                    }
                    return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel { ErrorString = response.StatusCode.ToString() });
                }

                var result = JsonSerializer.Deserialize<LiveDetectionResult>(msg);

                string resultHint = String.Empty;
                if (result.Samples != null && result.Samples.Count > 0)
                {
                    foreach (var error in result.Samples.SelectMany(sampleResult => sampleResult.Errors).Select(error => error))
                    {
                        // Display error only as hint without title 'Liveness Detection says: This was fake!'
                        if (error.Code == "NoFaceFound" || error.Code == "MultipleFacesFound")
                            return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel { ErrorString = error.Code.HintFromResult() });

                        string hint = error.Code.HintFromResult();
                        resultHint = string.Concat(resultHint, resultHint.Contains(hint) ? String.Empty : hint);
                        if (error.Code == "UnnaturalMotionDetected" & isMobile)
                        {
                            // add additional hint for mobile devices
                            resultHint = resultHint.Insert(0, new string("DontMoveDevice").HintFromResult());
                        }
                    }
                }

                var src = await _fileBusiness.GetFileByte(appointment.ApplicantPhotoId);

                var client = new HttpClient();
                var request = new MultipartFormDataContent();
                var image_data1 = new MemoryStream(src);//File.OpenRead("obama1.jpg");
                var image_data2 = new MemoryStream(image1); //File.OpenRead("obama2.jpg");
                request.Add(new StreamContent(image_data1), "image1", Path.GetFileName("test-image6.jpg"));
                request.Add(new StreamContent(image_data2), "image2", Path.GetFileName("test-image7.jpg"));
                var output = await client.PostAsync("https://demo.aitalkx.com/deepstackai/v1/vision/face/match", request);
                var jsonString = await output.Content.ReadAsStringAsync();
                var res = JObject.Parse(jsonString);

                var similiar = res.SelectToken("similarity");
                if (similiar != null && double.Parse(similiar.ToString()) < 0.65)
                {
                    return PartialView("_LivenessDetectionResult",
                        new LivenessDetectionResultModel { ErrorString = "The uploaded photo and live photo are not matching" });

                }

                bool live = result.Success;


                var file = await _fileBusiness.Create(new FileViewModel
                {
                    ContentByte = image1,
                    ContentType = "image/png",
                    ContentLength = image1.Length,
                    FileName = "Appointment.png",
                    FileExtension = ".png"
                }
                );
                appointment.PhotoId = file.Item.Id;
                await _userBusiness.Edit<BLSAppointmentViewModel, BLSAppointment>(appointment);

                return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel() { PhotoId = appointment.PhotoId, Live = live, ResultHint = resultHint });

            }
            catch (Exception ex)
            {
                return PartialView("_LivenessDetectionResult", new LivenessDetectionResultModel { ErrorString = ex.Message });
            }
        }

        public static string EncodeCredentials(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes($"{userName}:{password}"));
        }
        public async Task<IActionResult> GetLocationList(string userId = null)
        {
            var list = await _blsBusiness.getBLSLocationList(userId);
            return Json(list);
        }
        public async Task<IActionResult> GetAppointmentCategoryList(string userId = null)
        {
            var list = await _blsBusiness.GetAppointmentCategoryList(userId);
            return Json(list);
        }
        public async Task<IActionResult> GetVisaTypeList()
        {
            var list = await _blsBusiness.GetVisaTypes();
            return Json(list);
        }
        public async Task<IActionResult> AppointmentDetails(string serviceId, bool lo = false)
        {
            if (lo)
            {
                ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{_userContext.PortalTheme}/_Layout.cshtml";
            }
            var data = await _blsBusiness.GetAppointmentDetailsByServiceId(serviceId);
            var model = data.FirstOrDefault();
            if (model.IsNotNull())
            {
                model.AppointmentDateText = string.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, model.AppointmentDate);
                //model.PhotoId = _userContext.PhotoId;
                if (model.ApplicationServiceId.IsNotNullAndNotEmpty())
                {
                    var stepList = await _blsBusiness.GetList<TaskViewModel, NtsTask>(x => x.ParentServiceId == model.ApplicationServiceId, y => y.TaskStatus);
                    var step = stepList.FirstOrDefault(x => x.TaskStatus != null && (x.TaskStatus.Code == "TASK_STATUS_INPROGRESS" || x.TaskStatus.Code == "TASK_STATUS_OVERDUE"));
                    if (step != null)
                    {
                        model.CurrentTaskId = step.Id;
                        model.CurrentTaskTemplate = step.TemplateCode;
                    }
                    else
                    {
                        model.CurrentTaskId = "";
                    }
                }
            }
            else
            {
                model = new BLSVisaAppointmentViewModel();
            }
            return View(model);
        }
        public async Task<IActionResult> GetSlotsList(DateTime date, string loc, string serviceType, string visaType, string category, string slotids)
        {
            var list = await _blsBusiness.GetSlotValues(date, loc, serviceType, visaType, category);
            if (slotids.IsNotNullAndNotEmpty())
            {

                foreach (var item in list)
                {

                    if (slotids.Contains(item.Name))
                    {
                        item.Code = "Exist";
                    }
                }
            }
            list = list.OrderBy(x => x.Name).ToList();
            return Json(list);
        }

        public async Task<PartialViewResult> VisaAppointmentForm(string id, string serviceId, string appointmentId = null)
        {
            BLSVisaAppointmentViewModel model = new();
            model = await _blsBusiness.GetDataById(id);
            model.DataAction = DataActionEnum.Edit;
            model.ServiceId = serviceId;
            var appointDetails = await _blsBusiness.GetAppointmentSlotById(id);
            model.blsApplicantList = appointDetails;
            model.AppointmentId = appointmentId;

            var vas = await _blsBusiness.GetValueAddedServices();
            vas = vas.Where(x => x.ServiceCode != "BLS_VAS_PREMIUM").ToList();
            ViewBag.ValueAddedServices = vas;
            //var visaDetails = await _blsBusiness.GetVisaTypeDetails(model.VisaTypeId);
            //var totalamt = int.Parse(visaDetails.AppointmentFee) + (int.Parse(visaDetails.VisaFee) * model.ApplicantsNo);
            //model.TotalAmount = totalamt.ToString();
            //var setting = await _blsBusiness.GetSettingsData();
            //if (setting != null)
            //{
            //    model.MaximumAllowedDays = setting.MaximumAllowedDays;
            //}
            //var slotlist = await _blsBusiness.GetTimeSlotList(model.LegalLocationId);
            //if (slotlist.Count > 0)
            //{
            //    var ids = slotlist.Select(x => (int)x.Day);
            //    var day = string.Join(',', ids);
            //    model.WeekDays = day;
            //}
            //var holiday = await _blsBusiness.GetHolidays(model.LegalLocationId);
            //var holi = new List<string>();
            //foreach (var item in holiday)
            //{
            //    var dur = (item.EndDate - item.StartDate).Days;
            //    for (var i = 0; i <= dur; i++)
            //    {
            //        var start = item.StartDate.AddDays(i);
            //        holi.Add(start.ToDD_YYYY_MM_DD());
            //    }
            //}
            //model.Holidays = string.Join(",", holi);
            return PartialView(model);
        }
        public async Task<IActionResult> AppointmentDateByLocation(string loc)
        {
            BLSVisaAppointmentViewModel model = new();
            var setting = await _blsBusiness.GetSettingsData();
            if (setting != null)
            {
                model.MaximumAllowedDays = setting.MaximumAllowedDays;
            }
            var slotlist = await _blsBusiness.GetTimeSlotList(loc);
            if (slotlist.Count > 0)
            {
                var ids = slotlist.Select(x => (int)x.Day);
                var day = string.Join(',', ids);
                model.WeekDays = day;
            }
            var holiday = await _blsBusiness.GetHolidays(loc);
            var holi = new List<string>();
            foreach (var item in holiday)
            {
                var dur = (item.EndDate - item.StartDate).Days;
                for (var i = 0; i <= dur; i++)
                {
                    var start = item.StartDate.AddDays(i);
                    holi.Add(start.ToDD_YYYY_MM_DD());
                }
            }
            model.Holidays = string.Join(",", holi);
            return Json(new { MaximumAllowedDays = model.MaximumAllowedDays, WeekDays = model.WeekDays, Holidays = model.Holidays });
        }

        public async Task<IActionResult> GetAppointmentDate(string loc, string category)
        {
            var list = await _blsBusiness.GetAppointmentDate();
            var appoint = new List<string>();
            var str = "";
            foreach (var item in list)
            {
                var checkslot = await _blsBusiness.GetSlotValues(item.StartDate, loc, "", "", category);
                var check = checkslot.Where(x => x.Code != "Exist").ToList();
                if (check.Count > 0)
                {

                }
                else
                {
                    appoint.Add(item.StartDate.ToDD_YYYY_MM_DD());
                }
            }
            str = string.Join(",", appoint);
            return Json(str);
        }
        public IActionResult LivenessDetection(string appointmentId)
        {
            return View(new BLSAppointmentViewModel { Id = appointmentId });
        }
        public async Task<PartialViewResult> VisaAppointmentPaymentForm(string id, string serviceId)
        {
            BLSVisaAppointmentViewModel model = new();
            model = await _blsBusiness.GetDataById(id);
            model.DataAction = DataActionEnum.Edit;
            model.ServiceId = serviceId;
            model.OwnerUserId = _userContext.UserId;
            var visaDetails = await _blsBusiness.GetVisaTypeDetails(model.VisaTypeId);
            ViewBag.ApplicationFee = int.Parse(visaDetails.AppointmentFee);
            ViewBag.VisaFee = int.Parse(visaDetails.VisaFee);
            var vas = await _blsBusiness.GetValueAddedServices();

            var appCat = await _blsBusiness.GetAppointmentCategoryList("", model.AppointmentCategoryId);
            if (appCat.FirstOrDefault().Code != "CATEGORY_PREMIUM")
            {
                vas = vas.Where(x => x.ServiceCode != "BLS_VAS_PREMIUM").ToList();
            }
            //foreach (var item in vas)
            //{
            //    if (model.IsSMS && item.ServiceCode == "BLS_VAS_SMS")
            //    {
            //        model.TotalAmount = Convert.ToString(Convert.ToDouble(model.TotalAmount) + item.ServiceCharges);
            //        item.IsSelected = true;
            //    }
            //    else if (model.IsPhotograph && item.ServiceCode == "BLS_VAS_PRINTOUT")
            //    {
            //        model.TotalAmount = Convert.ToString(Convert.ToDouble(model.TotalAmount) + item.ServiceCharges);
            //        item.IsSelected = true;
            //    }
            //    else if (appCat.FirstOrDefault().Code == "CATEGORY_PREMIUM" && item.ServiceCode == "BLS_VAS_PREMIUM")
            //    {
            //        model.TotalAmount = Convert.ToString(Convert.ToDouble(model.TotalAmount) + item.ServiceCharges);
            //        item.IsSelected = true;
            //    }
            //}

            var selVAS = await _blsBusiness.GetSelectedVAS(id);
            foreach(var item in vas)
            {
                foreach(var item1 in selVAS)
                {
                    if(item.Id == item1.VASId)
                    {
                        item.IsSelected = true;
                        model.TotalAmount = Convert.ToString(Convert.ToDouble(model.TotalAmount) + item.ServiceCharges);
                    }
                }
            }

            ViewBag.ValueAddedServices = vas;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageBLSVisaAppointment(BLSVisaAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _userBusiness.GetSingleById<BLSAppointmentViewModel, BLSAppointment>(model.AppointmentId);
                if (appointment == null)
                {
                    return Json(new { success = false, error = "Invalid appointment Id." });
                }
                if (model.DataAction == DataActionEnum.Create)
                {

                    if (appointment.ServiceId != model.ServiceId)
                    {
                        return Json(new { success = false, error = "Invalid appointment service Id." });
                    }
                    if (appointment.EmailVerificationCode != model.VerificationCode)
                    {
                        return Json(new { success = false, error = "Email verification code is not valid. Please enter correct code." });
                    }
                    var captcha = await _userBusiness.GetSingleById<CaptchaViewModel, Captcha>(appointment.CaptchaId);
                    if (captcha == null || captcha.IsVerified == false)
                    {
                        return Json(new { success = false, error = "Appoitnment is not verified. Please verify the appointment." });
                    }
                    //var service = await _serviceBusiness.GetSingleById(model.ServiceId);
                    //if (service != null)
                    //{
                    //    return Json(new { success = false, error = "Appointment already created for given id." });
                    //}
                    //if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    //{
                    //    return Json(new { success = false, error = "Invalid Captcha." });
                    //}
                    model.BLSAPPLICANTDETAILS = model.ApplicantsDetailsList;
                    var serviceTempModel = new ServiceTemplateViewModel();
                    serviceTempModel.DataAction = model.DataAction;
                    serviceTempModel.ActiveUserId = _userContext.UserId;

                    serviceTempModel.TemplateCode = "BLS_VISA_APPOINTMENT";
                    var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                    var lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_DRAFTED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                    model.AppointmentStatusId = lov?.Id;

                    serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    serviceModel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    serviceModel.DataAction = DataActionEnum.Create;
                    serviceModel.ServiceId = model.ServiceId;
                    var result = await _serviceBusiness.ManageService(serviceModel);
                    if (result.IsSuccess)
                    {

                        List<BLSVisaAppointmentViewModel> slots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BLSVisaAppointmentViewModel>>(model.ApplicantsDetailsList);
                        foreach (var slot in slots)
                        {
                            var slotmodel = new BLSAppointmentSlotViewModel()
                            {
                                AppointmentId = model.AppointmentId,
                                ApplicantNo = model.ApplicantsNo.ToString(),
                                AppointmentDate = model.AppointmentDate,
                                AppointmentTime = slot.AppointmentSlot,
                                AppointmentStatus = BLSAppointmentStatusEnum.Pending
                            };

                            var slotresult = await _userBusiness.Create<BLSAppointmentSlotViewModel, BLSAppointmentSlot>(slotmodel);
                        }
                        model.Id = result.Item.UdfNoteTableId;
                        model.ServiceId = result.Item.ServiceId;
                        model.DataAction = DataActionEnum.Edit;
                        return Json(new { success = true, model = model });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

                }
                else
                {
                    if (appointment.ApplicantPhotoId.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Please upload applicant photo" });
                    }
                    if (appointment.PhotoId.IsNullOrEmpty())
                    {
                        return Json(new { success = false, error = "Applicant is not verified" });
                    }
                    model.BLSAPPLICANTDETAILS = model.ApplicantsDetailsList1;
                    if (model.VisaTypeId.IsNotNullAndNotEmpty())
                    {
                        var visaDetails = await _blsBusiness.GetVisaTypeDetails(model.VisaTypeId);
                        var totalamt = int.Parse(visaDetails.AppointmentFee) + (int.Parse(visaDetails.VisaFee) * model.ApplicantsNo);
                        model.TotalAmount = totalamt.ToString();
                    }

                    var serviceTempModel = new ServiceTemplateViewModel();
                    serviceTempModel.DataAction = DataActionEnum.Edit;
                    serviceTempModel.SetUdfValue = true;
                    serviceTempModel.ActiveUserId = _userContext.UserId;
                    serviceTempModel.ServiceId = model.ServiceId;
                    var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                    var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
                    var baseUrl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                    //var qrCodeData = $"{baseUrl}Portal/BLSEmployee?pageName=appointmentDetails&customUrl={ HttpUtility.UrlEncode("serviceId=" + model.ServiceId) }";
                    var qrCodeData = serviceModel.ServiceNo;
                    var qrCode = await _fileBusiness.GenerateBarCodeFile(qrCodeData, QRCodeDataTypeEnum.Text, QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum.NTS_Service, serviceModel.Id);
                    model.QRCodeId = qrCode.Item1;
                    var barCode = await _fileBusiness.GenerateBarCodeFile(qrCodeData, QRCodeDataTypeEnum.Text, QRCodeTypeEnum.CODE_128, ReferenceTypeEnum.NTS_Service, serviceModel.Id);
                    model.BarCodeId = barCode.Item1;
                    var lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_DRAFTED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                    model.AppointmentStatusId = lov?.Id;
                    serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    serviceModel.DataAction = DataActionEnum.Edit;
                    serviceModel.ActiveUserId = _userContext.UserId;
                    var result = await _serviceBusiness.ManageService(serviceModel);
                    if (result.IsSuccess)
                    {
                        List<BLSVisaAppointmentViewModel> slots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BLSVisaAppointmentViewModel>>(model.ApplicantsDetailsList1);
                        foreach (var slot in slots)
                        {
                            var slotmodel = new BLSAppointmentSlotViewModel()
                            {
                                AppointmentId = model.AppointmentId,
                                ApplicantNo = model.ApplicantsNo.ToString(),
                                AppointmentDate = model.AppointmentDate,
                                AppointmentTime = slot.AppointmentSlot,
                                AppointmentStatus = BLSAppointmentStatusEnum.Pending
                            };

                            var slotresult = await _userBusiness.Create<BLSAppointmentSlotViewModel, BLSAppointmentSlot>(slotmodel);
                        }

                        await CreateApplicationVAS(model);

                        return Json(new { success = true, model = model });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

                }

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        private async Task<bool> CreateApplicationVAS(BLSVisaAppointmentViewModel model)        
        {
            var vas = await _blsBusiness.GetValueAddedServices();
            var applications = await _blsBusiness.GetVisaApplicationDetailsByAppId(model.Id);

            var appCat = await _blsBusiness.GetAppointmentCategoryList("", model.AppointmentCategoryId);
            if (appCat.FirstOrDefault().Code != "CATEGORY_PREMIUM")
            {
                vas = vas.Where(x => x.ServiceCode != "BLS_VAS_PREMIUM").ToList();
            }
            var selectedVAS = model.ValueAddedServices.Split(",").ToArray();
            foreach (var app in applications)
            {
                foreach (var item in selectedVAS)
                {
                    var data = vas.Where(x => x.Id == item).FirstOrDefault();
                    var vasmodel = new ValueAddedServicesViewModel()
                    {
                        VASId = item,
                        Rate = data.ServiceCharges,
                        Quantity = 1,
                        Total = data.ServiceCharges,
                        ParentId = app.Id
                    };

                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Create;
                    formTempModel.TemplateCode = "BLSApplicationVAS";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(vasmodel);
                    var res = await _cmsBusiness.ManageForm(formmodel);
                }
            }
            return true;
        }


        [HttpPost]
        public async Task<IActionResult> TotalFeeUpdate(string id, string serviceId, string totalAmount,string selVAS = null)
        {
            var model = await _blsBusiness.GetDataById(id);
            model.ServiceId = serviceId;
            model.BLSAPPLICANTDETAILS = null;

            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Edit;
            serviceTempModel.SetUdfValue = true;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.ServiceId = model.ServiceId;
            var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            serviceModel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
            serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            serviceModel.DataAction = DataActionEnum.Edit;
            serviceModel.ActiveUserId = _userContext.UserId;
            var result = await _serviceBusiness.ManageService(serviceModel);
            if (result.IsSuccess)
            {
                if (selVAS.IsNotNullAndNotEmpty())
                {
                    model.ValueAddedServices = selVAS;
                    await CreateApplicationVAS(model);                    
                }
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = serviceModel.UdfNoteId;
                noteTempModel.SetUdfValue = true;
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                rowData["TotalAmount"] = totalAmount;
                if (rowData.ContainsKey("BLSAPPLICANTDETAILS"))
                {
                    rowData["BLSAPPLICANTDETAILS"] = null;
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
                if (update.IsSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                //return Json(new { success = true, model = model });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public IActionResult ReadAppointment()
        {
            return View(new BLSVisaAppointmentViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ReadAppointment(BLSVisaAppointmentViewModel model)
        {
            if (model.ServiceNo.IsNullOrEmpty())
            {
                return Json(new { success = false, error = "Appointment number required" });
            }
            var serviceModel = await _serviceBusiness.GetSingle(x => x.ServiceNo == model.ServiceNo);
            if (serviceModel == null)
            {
                return Json(new { success = false, error = "Appointment number not valid" });
            }
            return Json(new { success = true, serviceId = serviceModel.Id });
        }
        public IActionResult CancelAppointment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageCancelAppointment(BLSVisaAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newModel = await _blsBusiness.CheckEmailandServiceNo(model.ApplicantEmail, model.ServiceNo);
                if (!newModel.IsNotNull())
                {
                    return Json(new { success = false, error = "Please enter correct Appointment No and Email." });
                }
                else
                {
                    if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    {
                        return Json(new { success = false, error = "Invalid Captcha." });
                    }
                    else
                    {
                        var aps = await _lovBusiness.GetSingle(x => x.Id == newModel.AppointmentStatusId);
                        if (aps.Code == "BLS_APPOINTMENT_CANCELED")
                        {
                            return Json(new { success = false, error = "Appointment Cancelled Already" });
                        }
                        else
                        {
                            var serviceTempModel = new ServiceTemplateViewModel();
                            serviceTempModel.DataAction = DataActionEnum.Edit;
                            serviceTempModel.ActiveUserId = _userContext.UserId;
                            serviceTempModel.ServiceId = newModel.ServiceId;
                            var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

                            serviceModel.ServiceStatusCode = "SERVICE_STATUS_CANCEL";
                            serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(newModel);
                            var result = await _serviceBusiness.ManageService(serviceModel);
                            if (result.IsSuccess)
                            {
                                var noteTempModel = new NoteTemplateViewModel();
                                noteTempModel.NoteId = serviceModel.UdfNoteId;
                                noteTempModel.SetUdfValue = true;
                                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                                var rowData = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                                var lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_CANCELED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                                rowData["AppointmentStatusId"] = lov?.Id;
                                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                                var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
                                if (update.IsSuccess)
                                {
                                    return Json(new { success = true });
                                }
                                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                            }
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }
                    }
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public IActionResult FillVisaApplication()
        {
            return View();
        }

        public async Task<IActionResult> ManageVisaApplication(BLSVisaAppointmentViewModel model)
        {
            var existingModel = await _blsBusiness.GetVisaAppointmentByParams(model.ApplicantEmail, model.ServiceNo);

            if (existingModel.IsNotNull())
            {
                var aps = await _lovBusiness.GetSingle(x => x.Id == existingModel.AppointmentStatusId);
                if (aps.Code == "BLS_APPOINTMENT_CANCELED")
                {
                    return Json(new { success = false, error = "Appointment Cancelled Already" });
                }
                else
                {
                    var data = await _blsBusiness.GetAppointmentDetailsByServiceId(existingModel.ServiceId);
                    //var data = await _blsBusiness.GetMyAppointmentsList(existingModel.ServiceId);
                    model = data.FirstOrDefault();
                    var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED");
                    model.ApplicationStatusId = status.Id;
                    return Json(new { success = true, model = model });
                }
            }
            return Json(new { success = false, error = "Enter Correct Details" });

        }

        public async Task<IActionResult> BLSUpdateAppointmentImage(string serviceId, string fileId)
        {
            var serviceTempModel = new ServiceTemplateViewModel();
            serviceTempModel.DataAction = DataActionEnum.Edit;
            serviceTempModel.ActiveUserId = _userContext.UserId;
            serviceTempModel.ServiceId = serviceId;
            var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.NoteId = serviceModel.UdfNoteId;
            noteTempModel.SetUdfValue = true;
            var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            rowData["ImageId"] = fileId;
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);

            var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
            if (update.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }
        public async Task<IActionResult> ViewAppointmentReceipt(string serviceId)
        {
            var model = new BLSVisaAppointmentViewModel();
            var data = await _blsBusiness.GetAppointmentDetailsByServiceId(serviceId);
            if (data.IsNotNull())
            {
                foreach (var item in data)
                {
                    item.AppointmentDateText = string.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, item.AppointmentDate);
                    item.AppointmentDateSlotText = string.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, item.AppointmentDate) + " " + item.AppointmentSlot;
                    if (item.CurrentContactNumber.IsNotNullAndNotEmpty())
                    {
                        if (item.CurrentContactNumber.Length == 10)
                        {
                            item.CurrentContactNumberText = item.CurrentContactNumber.Substring(0, 1) + "******" + item.CurrentContactNumber.Substring(item.CurrentContactNumber.Length - 3, 3);
                        }
                        else
                        {
                            item.CurrentContactNumberText = item.CurrentContactNumber.Substring(0, 1) + "******" + item.CurrentContactNumber.Substring(item.CurrentContactNumber.Length - 3, 3);
                            //model.CurrentContactNumberText = model.CurrentContactNumber;
                        }
                        //.Substring(0, 1) + "******" + model.CurrentContactNumber.Substring(model.CurrentContactNumber.Length - 3, 3);
                    }
                    //model.AppointmentAddress = "Shop No# 13, Ground Floor, Zeenah Building of Budget Rent a Car, Opposite to Deira City Center P3 Parking, Deira, Dubai";
                    //model.PhotoId = _userContext.PhotoId;
                }
                model = data.FirstOrDefault();
                model.BlsAppointmentList = data;
            }

            model.ApplicantsCount = data.Count();

            ViewBag.UserId = _userContext.UserId;
            return View(model);
        }
        public IActionResult ReprintAppointmentLetter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageReprintDetailsRequest(BLSReprintDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    return Json(new { success = false, error = "Invalid Captcha." });
                }

                var existingModel = await _blsBusiness.GetVisaAppointmentByParams(model.ApplicantEmail, model.AppointmentNo);
                if (existingModel.IsNotNull())
                {
                    return Json(new { success = true, model = existingModel });
                }
                return Json(new { success = false, error = "Enter Correct Details" });

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> BLSRequestVerificationCode(string email, string appointmentId)
        {
            if (!Helper.IsValidEmail(email))
            {
                return Json(new { success = false, error = "Please enter valid email id" });
            }
            var appointment = await _userBusiness.GetSingleById<BLSAppointmentViewModel, BLSAppointment>(appointmentId);
            if (appointment == null)
            {
                return Json(new { success = false, error = "Invalid appointment" });
            }
            var random = new Random();
            var verificationCode = Convert.ToString(random.Next(10000000, 99999999));
            appointment.EmailVerificationCode = verificationCode;
            await _userBusiness.Edit<BLSAppointmentViewModel, BLSAppointment>(appointment);
            var notificationTemplateModel = await _notificationTemplateBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_EMAIL_VERIFICATION");
            EmailViewModel emailModel = new EmailViewModel();
            if (notificationTemplateModel.IsNotNull())
            {
                var body = notificationTemplateModel.Body;
                if (body.Contains("{{user-name}}"))
                {
                    body = body.Replace("{{user-name}}", email);
                }
                if (body.Contains("{{verification-code}}"))
                {
                    body = body.Replace("{{verification-code}}", verificationCode);
                }
                emailModel.To = email;
                emailModel.Subject = notificationTemplateModel.Subject;
                emailModel.Body = body;
            }
            else
            {
                emailModel.CompanyId = _userContext.CompanyId;
                emailModel.To = email;
                emailModel.Subject = "BLS Visa Appointment - Email Verification";
                emailModel.Body = "Dear " + email + "<br/> Greetings! <br/><br/> Your verification code as below <br> " + verificationCode + "<br> ";
            }
            var resultemail = await _emailBusiness.SendMail(emailModel);
            if (resultemail.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }



        public IActionResult BLSMoroccoVisaApplication()
        {
            return View();
        }

        public IActionResult BLSRegistration(string portalId)
        {
            UserViewModel model = new();
            return View();
        }
        //public async Task<string> GenerateBarcode(string text)
        //{
        //    //var QCwriter = new BarcodeWriter<string>();
        //    //QCwriter.Format = BarcodeFormat.CODE_128;
        //    ////QCwriter.Renderer = new ZXing.Rendering.BitmapRenderer();
        //    //var result = QCwriter.Write(text);
        //    ////var QCwriter = new BarcodeWriter();
        //    ////QCwriter.Format = BarcodeFormat.QR_CODE;
        //    ////r result = QCwriter.Write(QCText);

        //    ////var QCwriter = new BarcodeWriter
        //    ////{
        //    ////    Format = BarcodeFormat.CODE_128,
        //    ////    Options = { Width = 200, Height = 50, Margin = 4 },
        //    ////    Renderer = new ZXing.Rendering.BitmapRenderer()
        //    ////};

        //    ////var result = QCwriter.Write(text);
        //    //var barcodeBitmap = new Bitmap(result);

        //    //using (MemoryStream memory = new MemoryStream())
        //    //{
        //    //    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
        //    //    byte[] bytes = memory.ToArray();
        //    //    var res = await _fileBusiness.Create(new FileViewModel
        //    //    {
        //    //        ContentByte = bytes,
        //    //        ContentType = "image/jpeg",
        //    //        ContentLength = bytes.Length,
        //    //        FileName = "barcode.jpg",
        //    //        FileExtension = ".jpg"
        //    //    });
        //    //    return res.Item.Id;
        //    //}

        //    GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode("Hello barcode", BarcodeWriterEncoding.Code128);
        //    var x = barcode.ToJpegBinaryData();
        //    var res = await _fileBusiness.Create(new FileViewModel
        //    {
        //        ContentByte = x,
        //        ContentType = "image/jpeg",
        //        ContentLength = x.Length,
        //        FileName = "barcode.jpg",
        //        FileExtension = ".jpg"
        //    });
        //    return res.Item.Id;
        //}
        public async Task<IActionResult> GenerateBarcode2(string text)
        {
            //byte[] byteArray;
            //var width = 250; // width of the Qr Code   
            //var height = 250; // height of the Qr Code   
            //var margin = 0;
            //var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            //{
            //    Format = ZXing.BarcodeFormat.CODE_128,
            //    Options = new QrCodeEncodingOptions
            //    {
            //        Height = height,
            //        Width = width,
            //        Margin = margin
            //    }
            //};
            //var pixelData = qrCodeWriter.Write(text);


            //MemoryStream ms = new MemoryStream(pixelData.Pixels);
            //Image i = Image.FromStream(ms,true,false);
            //MemoryStream ms2 = new MemoryStream();
            //i.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
            //return File(ms2.ToArray(), "image/jpeg");
            var width = 250;
            var height = 250;
            var margin = 0;
            //var barcodeWriter = new ZXing.BarcodeWriterPixelData
            //{
            //    Format = ZXing.BarcodeFormat.CODE_128,
            //    Options = new EncodingOptions
            //    {
            //        Height = height,
            //        Width = width,
            //        Margin = margin
            //    },
            //    Renderer = new PixelDataRenderer
            //    {
            //        Foreground = new PixelDataRenderer.Color(unchecked((int)0xFF000000)),
            //        Background = new PixelDataRenderer.Color(unchecked((int)0xFFFFFFFF)),
            //    }
            //};
            text = "https://localhost:44346/home/loadqrcode?id=11c7f70d-fc2d-4f89-ba65-8aaefb9829cc";
            var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };

            using (var image = barcodeWriter.Write(text))
            {
                var format = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance;
                MemoryStream ms2 = new MemoryStream();
                image.Save(ms2, format);
                return File(ms2.ToArray(), "image/jpeg");

            }
        }
        //private async Task<Tuple<string, string, MemoryStream>> GenerateBarCode(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Url, QRCodeTypeEnum codeType = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false)
        //{
        //    var width = 250;
        //    var height = 250;
        //    var margin = 0;
        //    var barcodeFormat = (BarcodeFormat)codeType;
        //    if (barcodeFormat != BarcodeFormat.QR_CODE)
        //    {
        //        height = 100;
        //    }
        //    var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
        //    {
        //        Format = barcodeFormat,
        //        Options = new EncodingOptions
        //        {
        //            Height = height,
        //            Width = width,
        //            Margin = margin
        //        }
        //    };
        //    var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
        //    var qrCodeId = Guid.NewGuid().ToString();
        //    var qrCodeUrl = @$"{ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration)}home/loadqrcode?id={qrCodeId}";
        //    using (var image = barcodeWriter.Write(qrCodeUrl))
        //    {
        //        var format = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance;
        //        var ms = new MemoryStream();
        //        image.Save(ms, format);
        //        var bytes = ms.ToArray();
        //        var file = await _fileBusiness.Create(new FileViewModel
        //        {
        //            ContentByte = bytes,
        //            ContentType = "image/jpeg",
        //            ContentLength = bytes.Length,
        //            FileName = "barcode.jpg",
        //            FileExtension = ".jpg"
        //        });


        //        var qrCode = await _fileBusiness.Create<QRCodeDataViewModel, QRCodeData>(new QRCodeDataViewModel
        //        {
        //            QRCodeDataType = dataType,
        //            Data = data,
        //            QRCodeType = codeType,
        //            QrCodeUrl = qrCodeUrl,
        //            ReferenceType = referenceType,
        //            ReferenceTypeId = referenceId,
        //            IsPopup = isPopup,
        //            Id = qrCodeId
        //        });
        //        return new Tuple<string, string, MemoryStream>(file.Item.Id, qrCode.Item.Id, ms);
        //    }
        //}
        public async Task<IActionResult> RegisterBLSUser(UserViewModel model)
        {
            BLSCustomerViewModel customerModel = new();
            customerModel.LocationId = model.LocationId;
            model.SendWelcomeEmail = true;
            if (ModelState.IsValid)
            {
                model.SendWelcomeEmail = true;
                var result = await _userBusiness.Create(model);
                if (result.IsSuccess)
                {
                    if (result.Item.PortalId.IsNotNull())
                    {
                        var val = await _userPortalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = result.Item.Id,
                            PortalId = result.Item.PortalId,
                        });
                    }
                    customerModel.UserId = result.Item.Id;
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Create;
                    formTempModel.TemplateCode = "BLS_CUSTOMER";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(customerModel);
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }


        public async Task<IActionResult> ForwardToMOFA(string serviceId, string applicationId)
        {
            //var service = await _serviceBusiness.GetSingleById(serviceId);
            var data = await _blsBusiness.GetSchengenVisaApplicationDetailsById(applicationId);
            data.ServiceId = serviceId;

            return View(data);
        }

        public async Task<IActionResult> GenerateQRCode(string serviceId, string applicationId = null)
        {
            //var service = await _serviceBusiness.GetSingleById(serviceId);
            //var data = await _blsBusiness.GetSchengenVisaApplicationDetailsById(applicationId);
            ViewBag.ServiceId = serviceId;

            return View();
        }

        public async Task<IActionResult> UpdateVisaApprovalDetails(string serviceId)
        {
            //var service = await _serviceBusiness.GetSingleById(serviceId);

            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = serviceId });
            var noteTempModel1 = new NoteTemplateViewModel();
            noteTempModel1.SetUdfValue = true;
            noteTempModel1.NoteId = service.UdfNoteId;
            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
            var rowData = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["ForwardedToMofa"] = true;

            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);

            return Json(new { success = true });
        }

        public IActionResult BLSCustomerHome()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> OnlinePayment(string ntsId, string noteTableId, long amount, NtsTypeEnum ntsType, string assigneeUserId, string returnUrl)
        {
            var model = new OnlinePaymentViewModel()
            {
                NtsId = ntsId,
                UdfTableId = noteTableId,
                Amount = amount,
                NtsType = ntsType,
                UserId = assigneeUserId,
                ReturnUrl = returnUrl
            };

            var result = await _blsBusiness.UpdateOnlinePaymentDetails(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true, requestURL = result.Item.RequestUrl/*, returnurl*/ });
            }
            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        }

        public async Task<IActionResult> PaymentResponse(string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                return View(new OnlinePaymentViewModel { PaymentStatusCode = "ERROR", UserMessage = "Invalid Message from Payment Gateway" });
            }
            var responseViewModel = await ValidatePaymentResponse(msg);
            ///Update Online payment
            await _blsBusiness.UpdateOnlinePayment(responseViewModel);
            if (responseViewModel.PaymentStatusCode == "SUCCESS")
            {
                responseViewModel.UserMessage = $"Your payment has been completed successfully. Please note the reference number: {responseViewModel.PaymentReferenceNo} for further communication.";
                // update udf paymentstatusid,paymentreferenceno

                if (responseViewModel.NtsType == NtsTypeEnum.Service)
                {
                    //ServiceTemplateViewModel model = new ServiceTemplateViewModel();
                    //model.ServiceId = responseViewModel.NtsId;
                    //model.DataAction = DataActionEnum.Edit;
                    //model.ActiveUserId = _userContext.UserId;
                    //model.SetUdfValue = true;
                    //var serviceModel = await _serviceBusiness.GetServiceDetails(model);

                    //serviceModel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    //serviceModel.DataAction = DataActionEnum.Edit;
                    //serviceModel.ActiveUserId = _userContext.UserId;
                    //serviceModel.AllowPastStartDate = true;
                    //var result = await _serviceBusiness.ManageService(serviceModel);

                    var serviceTempModel = new ServiceTemplateViewModel();
                    serviceTempModel.DataAction = DataActionEnum.Edit;
                    serviceTempModel.SetUdfValue = true;
                    serviceTempModel.ActiveUserId = _userContext.UserId;
                    serviceTempModel.ServiceId = responseViewModel.NtsId;
                    var serviceModel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

                    serviceModel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    serviceModel.DataAction = DataActionEnum.Edit;
                    serviceModel.ActiveUserId = _userContext.UserId;

                    var appdata = await _blsBusiness.GetDataById(serviceModel.UdfNoteTableId);
                    appdata.BLSAPPLICANTDETAILS = null;
                    serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(appdata);

                    var result = await _serviceBusiness.ManageService(serviceModel);

                    if (result.IsSuccess)
                    {
                        var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = responseViewModel.NtsId });
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.SetUdfValue = true;
                        noteTempModel.NoteId = service.UdfNoteId;
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                        if (rowData.ContainsKey("PaymentStatusId"))
                        {
                            rowData["PaymentStatusId"] = responseViewModel.PaymentStatusId;
                        }
                        if (rowData.ContainsKey("PaymentReferenceNo"))
                        {
                            rowData["PaymentReferenceNo"] = responseViewModel.PaymentReferenceNo;
                        }
                        if (rowData.ContainsKey("BLSAPPLICANTDETAILS"))
                        {
                            rowData["BLSAPPLICANTDETAILS"] = null;
                        }
                        if (rowData.ContainsKey("AppointmentStatusId"))
                        {
                            var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_CONFIRMED");
                            rowData["AppointmentStatusId"] = status.Id;
                        }

                        var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        var update = await _noteBusiness.EditNoteUdfTable(notemodel, data, notemodel.UdfNoteTableId);
                    }
                    //var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel() { ServiceId = responseViewModel.NtsId });
                    //var noteTempModel = new NoteTemplateViewModel();
                    //noteTempModel.SetUdfValue = true;
                    //noteTempModel.NoteId = service.UdfNoteId;
                    //var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    //var rowData = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                    //if (rowData.ContainsKey("PaymentStatusId"))
                    //{
                    //    rowData["PaymentStatusId"] = responseViewModel.PaymentStatusId;
                    //}
                    //if (rowData.ContainsKey("PaymentReferenceNo"))
                    //{
                    //    rowData["PaymentReferenceNo"] = responseViewModel.PaymentReferenceNo;
                    //}
                    //if (rowData.ContainsKey("BLSAPPLICANTDETAILS"))
                    //{
                    //    rowData["BLSAPPLICANTDETAILS"] = null;
                    //}

                    //var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                    //var update = await _noteBusiness.EditNoteUdfTable(notemodel, data, notemodel.UdfNoteTableId);

                }
            }
            else
            {
                responseViewModel.UserMessage = responseViewModel.ResponseError;
            }

            return View(responseViewModel);
        }

        private async Task<OnlinePaymentViewModel> ValidatePaymentResponse(string msg)
        {
            var values = msg.Split('|');
            var id = values[1];
            var model = await _blsBusiness.GetOnlinePayment(id);
            if (model == null)
            {
                return new OnlinePaymentViewModel
                {
                    PaymentStatusCode = "ERROR",
                    ResponseError = "Invalid Transaction"
                };
            }
            model.AuthStatus = values[14];
            model.ResponseErrorCode = values[23];
            model.ResponseError = values[24];
            model.PaymentReferenceNo = values[2];
            if (model.PaymentReferenceNo == "NA")
            {
                model.PaymentReferenceNo = "";
            }
            model.ResponseChecksumValue = values[25];
            model.ResponseMessage = msg.Replace($"|{model.ResponseChecksumValue}", "");

            model.ResponseUrl = Request.Path;

            var paymentStatus = await _blsBusiness.GetList<LOVViewModel, LOV>(x => x.LOVType == "PAYMENT_STATUS");
            switch (model.AuthStatus)
            {
                case "0300":
                    var success = paymentStatus.FirstOrDefault(x => x.Code == "SUCCESS");
                    if (success != null)
                    {
                        model.PaymentStatusId = success.Id;
                        model.PaymentStatusCode = success.Code;
                        model.PaymentStatusName = success.Name;
                    }
                    break;
                default:
                    var fail = paymentStatus.FirstOrDefault(x => x.Code == "ERROR");
                    if (fail != null)
                    {
                        model.PaymentStatusId = fail.Id;
                        model.PaymentStatusCode = fail.Code;
                        model.PaymentStatusName = fail.Name;
                    }
                    if (model.ResponseError.IsNullOrEmpty() || model.ResponseError == "NA")
                    {
                        model.ResponseError = "An error occured while processing your request.";
                    }
                    if (model.PaymentReferenceNo.IsNotNullAndNotEmpty())
                    {
                        model.ResponseError = $"{model.ResponseError} Please note the reference number: {model.PaymentReferenceNo} for further communication.";
                    }
                    break;
            }
            return model;
        }


        public IActionResult RescheduleAppointment()
        {
            BLSVisaAppointmentViewModel model = new();
            return View(model);
        }

        public async Task<PartialViewResult> RescheduleDetails(string id, string serviceId)
        {
            BLSVisaAppointmentViewModel model = new();
            model = await _blsBusiness.GetDataById(id);
            model.DataAction = DataActionEnum.Edit;
            model.ServiceId = serviceId;
            var applicantsList = await _blsBusiness.GetApplicantsList(id);
            model.blsApplicantList = applicantsList;

            //List<BLSApplicantViewModel> foos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BLSApplicantViewModel>>(model.BLSAPPLICANTDETAILS);
            //model.blsApplicantList = foos;

            var setting = await _blsBusiness.GetSettingsData();
            if (setting != null)
            {
                model.MaximumAllowedDays = setting.MaximumAllowedDays;
            }
            var slotlist = await _blsBusiness.GetTimeSlotList(model.LegalLocationId);
            if (slotlist.Count > 0)
            {
                var ids = slotlist.Select(x => (int)x.Day);
                var day = string.Join(',', ids);
                model.WeekDays = day;
            }
            var holiday = await _blsBusiness.GetHolidays(model.LegalLocationId);
            var holi = new List<string>();
            foreach (var item in holiday)
            {
                var dur = (item.EndDate - item.StartDate).Days;
                for (var i = 0; i <= dur; i++)
                {
                    var start = item.StartDate.AddDays(i);
                    holi.Add(start.ToDD_YYYY_MM_DD());
                }
            }
            model.Holidays = string.Join(",", holi);
            model.AppointmentDate = null;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRescheduleAppointment(BLSVisaAppointmentViewModel model)
        {
            var newModel = await _blsBusiness.CheckEmailandServiceNo(model.ApplicantEmail, model.ServiceNo);
            if (!newModel.IsNotNull())
            {
                return Json(new { success = false, error = "Please enter correct Appointment No and Email." });
            }
            else
            {
                var aps = await _lovBusiness.GetSingle(x => x.Id == newModel.AppointmentStatusId);
                if (aps.Code == "BLS_APPOINTMENT_CANCELED")
                {
                    return Json(new { success = false, error = "Appointment Cancelled Already" });
                }
                else
                {
                    //if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    //{
                    //    return Json(new { success = false, error = "Invalid Captcha." });
                    //}
                    return Json(new { success = true, model = newModel });
                }
                //return Json(new { success = true, model = newModel });
            }
        }

        public async Task<IActionResult> ManageRescheduleDetails(BLSVisaAppointmentViewModel model)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
            {
                return Json(new { success = false, error = "Invalid Captcha." });
            }
            //var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(model.blsApplicantList);

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.NoteId = model.NtsNoteId;
            noteTempModel.SetUdfValue = true;
            var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            rowData["BLSAPPLICANTDETAILS"] = model.ApplicantsDetailsList;
            rowData["AppointmentDate"] = model.AppointmentDate;

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            var update = await _noteBusiness.EditNoteUdfTable(noteModel, data, noteModel.UdfNoteTableId);

            if (update.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });

        }

        public IActionResult AppointmentTimeSlotIndex()
        {

            return View();
        }

        public async Task<IActionResult> GetAppointmentTimeSlotList()
        {
            var data = await _blsBusiness.GetAllTimeSlotList();
            return Json(data);

        }
        public async Task<IActionResult> GetTimeSlotList(int duration, string Id)
        {
            var data = new List<TimeSlot>();
            var start = new TimeSpan(0, 0, 0);
            var end = new TimeSpan(24, 0, 0);
            var exist = await _blsBusiness.GetTimeSlotByParentId(Id);
            var check = false;
            for (var i = 0; i <= 100; i++)
            {
                if (start != end)
                {
                    var ed = start.Add(new TimeSpan(0, duration, 0));
                    var ex = exist.FirstOrDefault(x => x.StartTime == start);
                    if (ex != null)
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                    }
                    data.Add(new TimeSlot { Slot = start.ToString(@"hh\:mm") + "-" + ed.ToString(@"hh\:mm"), Checked = check });
                    start = ed;
                }
                else
                {
                    break;
                }
            }
            return Json(data);

        }
        public async Task<IActionResult> AppointmentTimeSlot(string id = null)
        {
            BLSTimeSlotViewModel model = new();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _blsBusiness.GetTimeSlotById(id);
                model.DataAction = DataActionEnum.Edit;

            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.SlotDuration = 60;
            }

            return View(model);
        }

        public async Task<IActionResult> ManageTimeSlot(BLSTimeSlotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var str = model.TimeSlots.Split(',');
                var list = new List<TimeSlot>();
                foreach (var item in str)
                {
                    var timestr = item.Split('-');
                    var data = new TimeSlot
                    {
                        StartTime = TimeSpan.Parse(timestr[0]),
                        EndTime = TimeSpan.Parse(timestr[1]),
                    };
                    list.Add(data);
                }
                if (model.DataAction == DataActionEnum.Create)
                {
                    var temp = await _templateBusiness.GetSingle<TemplateViewModel, Template>(x => x.Code == "VISA_TIMESLOT_SETTINGS");

                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Create;
                    formTempModel.TemplateCode = "VISA_TIMESLOT_SETTINGS";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    model.BLS_TIME_SLOT = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    formmodel.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Edit;
                    formTempModel.TemplateCode = "VISA_TIMESLOT_SETTINGS";
                    //formTempModel.Id = 
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.TemplateCode = "VISA_TIMESLOT_SETTINGS";
                    model.BLS_TIME_SLOT = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    formmodel.Page = new PageViewModel { Template = new Template { TableMetadataId = formmodel.TableMetadataId } };
                    formmodel.RecordId = model.Id;
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        ViewBag.Success = true;
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> GetCityList()
        {
            var list = await _cmsBusiness.GetDataListByTemplate("BLS_CITY", "", "");
            return Json(list);
        }

        public async Task<IActionResult> GetStateList()
        {
            var list = await _cmsBusiness.GetDataListByTemplate("BLS_STATE", "", "");
            return Json(list);
        }

        public async Task<IActionResult> MyAppointments()
        {
            var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED");
            ViewBag.ApplicationStatus = status.Id;
            return View();
        }
        public async Task<IActionResult> ReadMyAppointmentsList()
        {
            var data = await _blsBusiness.GetMyAppointmentsList();
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetCountryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("BLS_COUNTRY", "", "");
            return Json(data);

        }

        public async Task<IActionResult> PaymentConfirmation(string serviceId)
        {
            var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED");
            ViewBag.ApplicationStatus = status.Id;
            ViewBag.ServiceId = serviceId;

            var apps = await _blsBusiness.GetMyAppointmentsList(serviceId);
            var parentId = apps.Select(x => x.Id).FirstOrDefault();
            var appsList = await _blsBusiness.GetAppointmentDetailsWithApplicants(parentId);
            var model = appsList.FirstOrDefault();
            var appservice = await _blsBusiness.GetVisaApplicationDetailsByAppId(parentId);
            var count = appsList.Count();
            var serIds = appservice.Select(x => x.ApplicationServiceId).ToArray();
            var i = 0;
            foreach(var app in appsList)
            {
                app.ApplicationServiceId = serIds[i];
                app.DateOfBirthT = app.DateOfBirth?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                i++;
            }

            ViewBag.VisaApplicationList = appsList;
            return View(model);
        }

        public async Task<IActionResult> FillVisaApplicationList(string serviceId)
        {
            var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED");
            ViewBag.ApplicationStatus = status.Id;
            ViewBag.ServiceId = serviceId;
            ViewBag.PortalId = _userContext.PortalId;
            var apps = await _blsBusiness.GetMyAppointmentsList(serviceId);
            var parentId = apps.Select(x => x.Id).FirstOrDefault();
            var appsList = await _blsBusiness.GetAppointmentDetailsWithApplicants(parentId);
            var model = appsList.FirstOrDefault();
            var appservice = await _blsBusiness.GetVisaApplicationDetailsByAppId(parentId);
            var count = appsList.Count();
            var serIds = appservice.Select(x => x.ApplicationServiceId).ToArray();
            var appStatus = appservice.Select(x => x.ApplicationStatusCode).ToArray();
            var i = 0;
            foreach (var app in appsList)
            {
                app.ApplicationServiceId = serIds[i];
                app.ApplicationStatusCode = appStatus[i];
                app.DateOfBirthT = app.DateOfBirth?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
                app.AppointmentSlot = string.Concat(app.AppointmentDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat)," ",app.AppointmentSlot);
                i++;
            }

            ViewBag.VisaApplicationList = appsList;
            return View();
        }
    }
}
