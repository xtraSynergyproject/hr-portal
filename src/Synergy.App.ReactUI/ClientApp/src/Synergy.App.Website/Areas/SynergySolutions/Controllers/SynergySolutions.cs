using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Repository;

namespace CMS.UI.Web.Areas.SynergySolutions.Controllers
{
    [Area("SynergySolutions")]
    public class SynergySolutions : ApplicationController
    {
        ICompanySettingBusiness _companySettings;
        IEmailBusiness _emailBusiness;
        IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<ApplicationDocumentViewModel> _queryRepo;
        private ICmsBusiness _cmsBusiness;
        private readonly IFormTemplateBusiness _formBusiness;
        public SynergySolutions(ICompanySettingBusiness companySettings, INoteBusiness noteBusiness, IUserBusiness userBusiness
            , ICmsBusiness cmsBusiness, IUserContext userContext
            , IRepositoryQueryBase<ApplicationDocumentViewModel> queryRepo
            , IEmailBusiness emailBusiness)
        {
            _companySettings = companySettings;
            _userBusiness = userBusiness;
            _userContext = userContext;
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _emailBusiness = emailBusiness;
            _cmsBusiness = cmsBusiness;
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
        //public static string GetMetatdata(string pageName)
        //{
        //    pageName = pageName.ToLower();
        //    string meta = "";
        //    switch (pageName)
        //    {
        //        case "egovernance":
        //        case "egovernanceind":
        //            meta = "We are introducing the flexible path for modern digital experiences for government agencies and citizens.Visit for details!";
        //            break;
        //        case "home":
        //            meta = "Synergy accelerates and reduces the cost of digital transformation by streamlining processes, identifying inefficiencies, and providing insights. Schedule demo today!";
        //            break;
        //        case "overview":
        //        case "synergyplatformoverview":
        //            meta = "The Synergy low code digital transformation platform is perfect for enterprises that want modules to be customised to match their individual specifications and changing demands.";
        //            break;
        //        case "technologiesused":
        //            meta = "Synergy used the technology which is designed to simplify complicated corporate processes, handle unstructured data, and increase consumer involvement in response to changing needs.";
        //            break;
        //        case "businessprocessmanagement":
        //            meta = "Business Process Management and Automation (BPM) Software by Synergy drives end-to-end process automation and enables continuous improvement. Visit now!";
        //            break;
        //        case "mobileapp":
        //            meta = "Synergy low code digital transformation platform has innovative mobile application platform. It provide collaborative and customer centric mobile platform. Visit now!";
        //            break;
        //        case "apiconnector":
        //            meta = "Synergy can connect to practically any external API using the API Connector. It is a robust, user-friendly tool for instantly importing data from any API into a Google Sheets. Visit now.";
        //            break;
        //        case "analyticsdashboard":
        //            meta = "Analytics and dashboards from Synergy are a strong tool set for businesses to absorb, organise, identify, and analyse data in order to provide actionable insights. Visit us for more information.";
        //            break;
        //        case "servicedesk":
        //            meta = "Synergy’s service desk empowers teams to deliver great service experiences and ensures your employees and customers can get help quickly. Visit us for details!";
        //            break;
        //        case "salesmarketing":
        //            meta = "Synergy’s sales and marketing solution helps in better engagement and coordination between teams and improve lead generation revenue. Visit us for details!";
        //            break;
        //        case "recruitmentassessment":
        //            meta = "Synergy’s Recruitment assessment solution makes it easier for recruiters to find, track, assess, and hire the best prospects quickly and make the informed decision. Visit us now.";
        //            break;
        //        case "projectmanagementnew":
        //            meta = "Synergy’s Project management solution helps every business to plan and manage the project successfully and complete its listed goals and deliverables. Visit us for details!";
        //            break;
        //        case "synergyeye":
        //            meta = "Synergy eye protect you by offering security, safety and sanctuary for home, business or office. Let the eye of vigilance never be closed. Contact us for details.";
        //            break;
        //        case "citizenmobileapp":
        //            meta = "Synergy provides citizen mobile applications services. It helps for e-governance, smart city services, public grievances and more. Visit us for details!";
        //            break;
        //        case "documentmanagement":
        //            meta = "Synergy’s eDMS handles documents by electronically storing, organizing, indexing and filing. It can be retrieved as and when required at the click of a button. Visit for details!";
        //            break;
        //        case "humancapitalmanagement":
        //            meta = "Synergy’s HCMS is a comprehensive solution to all HR needs, all integrated into one, to meet the management\'s requirements. It is a browser-based application, simple to use and does not require any special training.";
        //            break;
        //        case "financialmanagement":
        //            meta = "Synergy’s financial management manages income, expenses, and assets that company use. It also improves short terms and long terms business performance. Visit for details!";
        //            break;
        //        case "intelligencepolicing":
        //            meta = "Synergy\'s intelligent policing offers a variety of AI tools and approaches that aid in the policing process, such as decision assistance and optimization strategies. Visit now!";
        //            break;
        //        case "socialmediaanalytics":
        //            meta = "Synergy\'s social media analytics collects and analyses audience data from social networks and can be utilized to improve strategic business decisions.Schedule demo today.";
        //            break;
        //        case "successstory":
        //            meta = "Know more about Synergy’s success story, how we help our clients achieve the digital transformation goal.";
        //            break;
        //        case "ourteam":
        //            meta = "Know about the incredible team members of Synergy. Here we nurture and develop the unique business ideas.";
        //            break;
        //        case "synergycareer":
        //            meta = "Add your magic to ours. You\'ve come here because you want to shape the future.Find work as a software developer, business analyst, or in a variety of other fields.";
        //            break;
        //        case "aboutus":
        //            meta = "Synergy is a pioneer in low-code digital transformation. Discover our company\'s goal, vision, and culture. For details visit us.";
        //            break;
        //        case "company":
        //            meta = "Synergy low code digital transformation platform is used by successful businesses to design and deploy complex, content - driven, customer - engaging business apps.";
        //            break;
        //        case "contactus":
        //        case "contactusindia":
        //            meta = "Contact us for details about Synergy. We are employee friendly organization and believe that employees are the greatest assets. We encourage our employees to provide their input and ideas to create a better environment.";
        //            break;
        //        case "requestconsultation":
        //            meta = "Our team is happy to answer your queries. Please complete the form below. We\'ll get back to you as soon as possible!";
        //            break;
        //        case "termsofuse":
        //            meta = "Know more about the terms of use of Synergy low code digital transformation. Terms of use are the rules, specifications, and requirements for the use of a product or service.";
        //            break;
        //        case "privacypolicy":
        //            meta = "This Privacy Policy is meant to help you understand what information we collect, why we collect it and how you can update, manage, export, and delete your information.";
        //            break;
        //        case "casemanagement":
        //        case "facedetection":
        //        case "predictivemaintenance":
        //        case "numberplatedetection":
        //            meta = "Read about digital transformation use cases of Synergy’s customers in the various industry and sectors like financial services, public sector, manufacturing and healthcare.";
        //            break;
        //        case "contentmanagement":
        //            meta = "Synergy's content management solution manages content digitally for businesses. It enables its users to create, modify, store and publish content digitally. Visit us for details!";
        //            break;
        //        default:
        //            break;
        //    }
        //    if (meta.IsNotNullAndNotEmpty())
        //    {
        //        return $"<meta name=\"description\" content=\"{meta}\" />";
        //    }
        //    return meta;
        //}

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

            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_CONSULTATION_MAIL");
            if (notificationTemplateModel.IsNotNull())
            {
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{FirstName}}", data.FullName)
                .Replace("{{LastName}}", data.LastName).Replace("{{Email}}", data.Email)
                .Replace("{{ContactNumber}}", data.ContactNumber).Replace("{{Country}}", data.Country)
                .Replace("{{ProductIntrested}}", data.ProductIntrested).Replace("{{CompanyName}}", data.CompanyName)
                .Replace("{{JobTitle}}", data.JobTitle).Replace("{{NumberOfEmployees}}", data.NumberOfEmployees)
                .Replace("{{Message}}", data.Message);
                notificationTemplateModel.Subject = notificationTemplateModel.Subject.Replace("{{FirstName}}", data.FullName)
              .Replace("{{LastName}}", data.LastName).Replace("{{Email}}", data.Email)
              .Replace("{{ContactNumber}}", data.ContactNumber).Replace("{{Country}}", data.Country)
              .Replace("{{ProductIntrested}}", data.ProductIntrested).Replace("{{CompanyName}}", data.CompanyName)
              .Replace("{{JobTitle}}", data.JobTitle).Replace("{{NumberOfEmployees}}", data.NumberOfEmployees)
              .Replace("{{Message}}", data.Message);
                var recipients = await _companySettings.GetSingle(x => x.Code == "WEBSITE_SUPPORT_RECIPIENTS");
                if (recipients != null)
                {
                    var result = await _emailBusiness.SendMailAsync(
                        new EmailViewModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            To = recipients.Value,
                            Subject = notificationTemplateModel.Subject,
                            Body = notificationTemplateModel.Body,
                            DataAction = DataActionEnum.Create
                        });

                    if (result.IsSuccess)
                    {
                        var reply = await _companySettings.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_CONSULTATION_REPLY");
                        if (reply.IsNotNull())
                        {
                            reply.Subject = reply.Subject.Replace("{{FirstName}}", data.FullName).Replace("{{LastName}}", data.LastName)
                           .Replace("{{Email}}", data.Email).Replace("{{ProductIntrested}}", data.ProductIntrested);
                            reply.Body = reply.Body.Replace("{{FirstName}}", data.FullName).Replace("{{LastName}}", data.LastName)
                           .Replace("{{Email}}", data.Email).Replace("{{ProductIntrested}}", data.ProductIntrested);
                            await _emailBusiness.SendMailAsync(
                            new EmailViewModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                To = data.Email,
                                Subject = reply.Subject,
                                Body = reply.Body,
                                DataAction = DataActionEnum.Create
                            });
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
                else
                {
                    return Json(new { success = false, error = "No support user created" });
                }

            }
            return Json(new { success = false, error = "No email template created" });
        }

        [HttpPost]
        public async Task<IActionResult> SendContactUsEmail(RequestConsultationViewModel data)
        {

            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_CONTACTUS_MAIL");
            if (notificationTemplateModel.IsNotNull())
            {
                notificationTemplateModel.Subject = notificationTemplateModel.Subject.Replace("{{Name}}", data.FirstName)
             .Replace("{{Email}}", data.Email).Replace("{{Phone}}", data.ContactNumber)
             .Replace("{{Subject}}", data.Subject).Replace("{{Message}}", data.Message);
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{Name}}", data.FirstName)
                .Replace("{{Email}}", data.Email).Replace("{{Phone}}", data.ContactNumber)
                .Replace("{{Subject}}", data.Subject).Replace("{{Message}}", data.Message);
                var recipients = await _companySettings.GetSingle(x => x.Code == "WEBSITE_SUPPORT_RECIPIENTS");
                if (recipients != null)
                {

                    var result = await _emailBusiness.SendMailAsync(
                      new EmailViewModel
                      {
                          Id = Guid.NewGuid().ToString(),
                          To = recipients.Value,
                          Subject = notificationTemplateModel.Subject,
                          Body = notificationTemplateModel.Body,
                          DataAction = DataActionEnum.Create
                      });
                    if (result.IsSuccess)
                    {
                        var reply = await _companySettings.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_CONTACTUS_REPLY");
                        if (reply.IsNotNull())
                        {
                            reply.Subject = reply.Subject.Replace("{{Name}}", data.FirstName).Replace("{{LastName}}", data.LastName)
                            .Replace("{{Email}}", data.Email);
                            reply.Body = reply.Body.Replace("{{Name}}", data.FirstName).Replace("{{LastName}}", data.LastName)
                           .Replace("{{Email}}", data.Email);

                            await _emailBusiness.SendMailAsync(
                          new EmailViewModel
                          {
                              Id = Guid.NewGuid().ToString(),
                              To = data.Email,
                              Subject = reply.Subject,
                              Body = reply.Body,
                              DataAction = DataActionEnum.Create
                          });
                        }

                        return Json(new { success = true });
                    }
                }
                else
                {
                    return Json(new { success = false, error = "No support user created" });
                }

            }
            return Json(new { success = false, error = "No email template created" });
        }
        public IActionResult ProjectManagementFeatures()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> DocumentManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_SEDMS_BRO_VER_1.8");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_SEDMS_DATA_VER_1.9");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_SEDMS_BRO_VER_1.8");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_SEDMS_DATA_VER-1.9");

            }
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
        public async Task<IActionResult> BusinessProcessAutomation()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult BusinessRuleEngine()
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
        public async Task <IActionResult> EdmsArchitecture()
        {
            ViewBag.Portal = _userContext.PortalName;
           
            return View();
        }
        public IActionResult EdmsModuleBenefits()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> Egovernance()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_EGOV_VER_1.14");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_EGOV_DS_VER_1.7");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SNYERGY_IN_EGOV_BRO_VER_1.14");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_EGOV_DATA_VER_1.7");

            }
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
        public async Task<IActionResult> IntegratedIntelligencePortal()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_INTELLIGENT_POLICING_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_INTELLIGENT_POLICE_DS_VER_1.7");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_INTELLIGENTPOL_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_INTELLIGENTPOL_DATA_VER_1.7");

            }
            return View();
        }
        public IActionResult InternalSocialMedia()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public IActionResult OtherSolutions()
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

        public async Task<IActionResult> ProjectManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");

            }
            return View();
        }

        public IActionResult Worksnaps()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        #region 1
        public async Task<IActionResult> ContentMangement()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public async Task<string> GetDocumentId(string documentCode)
        {
            var query = $@" select * from public.""ApplicationDocument"" where ""Code"" = '{documentCode}' ";
            var data = await _queryRepo.ExecuteQuerySingle(query, null);
            string docId = null;
            if (data.IsNotNull())
            {
                docId = data.DocumentId;
            }
            return docId;
        }

        public async Task<IActionResult> GetDocumentId2(string documentCode)
        {
            var query = $@" select * from public.""ApplicationDocument"" where ""Code"" = '{documentCode}' ";
            var data = await _queryRepo.ExecuteQuerySingle(query, null);
            var docId = data.DocumentId;
            return Json(new { success = true, id = docId });
        }

        // mobile app

        public IActionResult UploadResume(string jobType)
        {
            UploadResumeViewModel model = new();
            model.JobName = jobType;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUploadResume(UploadResumeViewModel model)
        {
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "CANDIDATE_RESUME";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
            notemodel.DataAction = DataActionEnum.Create;
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Resume not uploaded." });

        }
        public async Task<IActionResult> MobileApp()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public IActionResult TermsnConditions()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public IActionResult OurTeam()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Partners()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult PartnerRegistrationForm()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        // project management 
        public async Task<IActionResult> ProjectManagementNew()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_PM_BRO_VER_1.10");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_PM_DS_VER_1.11");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_PROJECTMGMT_BRO_VER_1.10");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_PROJECTMGMT_DATA_VER_1.11");

            }
            return View();
        }

        public async Task<IActionResult> RecruitmentAssessment()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_RECRUIT_PORTAL_BRO_VER_1.6");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_RECRUIT_PORTAL_DS_VER_1.7");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_RECRUITMENT_BRO_VER_1.6");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_RECRUITMENT_DATA_VER_1.7");

            }
            return View();
        }

        public IActionResult Resources()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> SalesMarketing()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_SM_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_SALES_MKTG_DS_VER_1.7");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_SALESMARKETING_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_SALESMARKETING_DATA_VER_1.7");

            }
            return View();
        }

        public async Task<IActionResult> ServiceDesk()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_SERVICEDESK_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_SERVICEDESK_DS_VER_1.12");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_SERVICEDESK_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_SERVICEDESK_DATA_VER_1.12");

            }
            return View();
        }

        public IActionResult SuccessStory()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public IActionResult SynergyCareer()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public async Task<IActionResult> SynergyEye()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_EYE_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_EYE_DS_VER_1.8");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_EYE_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_EYE_DATA_VER_1.8");

            }
            return View();
        }

        public async Task<IActionResult> SynergyPlatformOverview()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_PLATFORM_BRO_VER_1.12");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_PFM_DS_VER_1.6");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_INDIA_PLATFORM_BRO_VER_1.12");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_PLATFORM_DATA_VER_1.6");

            }
            return View();
        }

        public async Task<IActionResult> TechnologyUsed()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> WhitePapers()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.pageId = await GetDocumentId("SYNERGY_AE_PFM_WHITEPAPER_VER_1.5");
                
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.pageId = await GetDocumentId("SYNERGY_IND_PFM_WHITEPAPER_VER_1.5");
               

            }
            return View();
        }

        #endregion

        #region 2

        public async Task<IActionResult> AnalyticsAndDashboard()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public async Task<IActionResult> APIConnector()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }

        public IActionResult Blogs()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> CitizenMobile()
        {
            ViewBag.brochureId = await GetDocumentId("CITIZEN_MOBILE_APP_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("CITIZEN_MOBILE_APP_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult Company()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> FinancialManagementSolution()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_XERP_FIN_AC_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_XERP_FIN_AC_DS_VER_1.11");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_XERPFINACC_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_XERPFINACC_DATA_VER_1.11");

            }
            return View();
        }
        public async Task<IActionResult> HumanCapitalManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_HCM_BRO_VER_1.11");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_HCMS_DS_VER_1.10");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_HCM_BRO_VER_1.11");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_HCMS_DATA_VER_1.10");

            }
            return View();
        }
        public async Task<IActionResult> IntelligencePolicing()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_INTELLIGENT_POLICING_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_INTELLIGENT_POLICE_DS_VER_1.7");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_INTELLIGENTPOL_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_INTELLIGENTPOL_DATA_VER_1.7");

            }
            return View();
        }
        public IActionResult CaseManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult FaceRecognition()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult NumberPlateDetection()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public IActionResult PredictiveMaintenanceUsingMachineLearning()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> SocialMediaAnalytics()
        {
            ViewBag.brochureId = await GetDocumentId("SOCIAL_MEDIA_ANALYTICS_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SOCIAL_MEDIA_ANALYTICS_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> SynergyConnector()
        {
            ViewBag.brochureId = await GetDocumentId("SYNERGY_PLATFORM_BROCHURE");
            ViewBag.dataSheetId = await GetDocumentId("SYNERGY_PLATFORM_DATASHEET");
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> InventoryManagement()
        {
            ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_INVENTORY_SOLUTION_VER_1.8");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_INVENTORY_SOL_DS_VER_1.6");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_INVENTORY_BRO_VER_1.8");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_INVENTORY_DATA_VER_1.6");

            }
            return View();
        }
        public async Task<IActionResult> DataScienceAIMILOT()
        {
            ViewBag.Portal = _userContext.PortalName;
            return View();
        }
        public async Task<IActionResult> GetCountryList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("SYNERGY_COUNTRY", "");
            return Json(data);
        }
        public async Task<IActionResult> GetStateList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("SYNERGY_STATE", "");
            return Json(data);
        }
        public async Task<IActionResult> Assessment()
        {
          ViewBag.Portal = _userContext.PortalName;
            if (ViewBag.Portal == "SynergySolutions")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_AE_ASSESSMENT_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_AE_ASSESSMENT_MOD_DS_VER_1.6");
            }
            else if (ViewBag.Portal == "SynergySolutionsIndia")
            {
                ViewBag.brochureId = await GetDocumentId("SYNERGY_IN_ASSESSMENT_BRO_VER_1.7");
                ViewBag.dataSheetId = await GetDocumentId("SYNERGY_IN_ASSESSMENT_DATA_VER_1.6");

            }
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> PartnerRegistrationForm(PartnerRegistrationViewModel model)
        {

            var formTempModel = new FormTemplateViewModel();
            formTempModel.DataAction = DataActionEnum.Create;
            formTempModel.TemplateCode = "SYNERGY_PARTNER_REGISTRATION";
            var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
            formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
             var result = await _cmsBusiness.ManageForm(formmodel);
            if (result.IsSuccess)
            {
              await SendPartnerRegistration(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> SendPartnerRegistration(PartnerRegistrationViewModel data)
        {

            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_PARTNER_REGISTRATION_EMAIL");
            if (notificationTemplateModel.IsNotNull())
            {
                notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{FullName}}", data.FullName)
                .Replace("{{MobileNo}}", data.MobileNo).Replace("{{LandlineNo}}", data.LandlineNo)
                .Replace("{{ContactPerson}}", data.ContactPerson).Replace("{{Designation}}", data.Designation)
                .Replace("{{Gstin}}", data.Gstin).Replace("{{Tan}}", data.Tan)
                .Replace("{{Pan}}", data.Pan).Replace("{{Captcha}}", data.Captcha)
                .Replace("{{City}}", data.City).Replace("{{Town}}", data.Town).Replace("{{Address}}", data.Address)
                .Replace("{{DateOfBirth}}", data.DateOfBirth).Replace("{{PinCode}}", data.PinCode).Replace("{{OrganisationName}}", data.OrganisationName)
                /*.Replace("{{Gender}}", data.Gender).*/.Replace("{{Profession}}", data.Profession);
                notificationTemplateModel.Subject = notificationTemplateModel.Subject.Replace("{{FullName}}", data.FullName)
              .Replace("{{MobileNo}}", data.MobileNo).Replace("{{LandlineNo}}", data.LandlineNo)
                .Replace("{{ContactPerson}}", data.ContactPerson).Replace("{{Designation}}", data.Designation)
                .Replace("{{Gstin}}", data.Gstin).Replace("{{Tan}}", data.Tan)
                .Replace("{{Pan}}", data.Pan).Replace("{{Captcha}}", data.Captcha)
                .Replace("{{City}}", data.City).Replace("{{Town}}", data.Town).Replace("{{Address}}", data.Address)
                .Replace("{{DateOfBirth}}", data.DateOfBirth).Replace("{{PinCode}}", data.PinCode).Replace("{{OrganisationName}}", data.OrganisationName)
                /*.Replace("{{Gender}}", data.Gender)*/.Replace("{{Profession}}", data.Profession);
                var recipients = await _companySettings.GetSingle(x => x.Code == "WEBSITE_SUPPORT_RECIPIENTS");
                if (recipients != null)
                {
                    var result = await _emailBusiness.SendMailAsync(
                        new EmailViewModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            To = recipients.Value,
                            Subject = notificationTemplateModel.Subject,
                            Body = notificationTemplateModel.Body,
                            DataAction = DataActionEnum.Create
                        });

                    if (result.IsSuccess)
                    {
                       return Json(new { success = true });
                    }
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
                else
                {
                    return Json(new { success = false, error = "No support user created" });
                }

            }
            return Json(new { success = false, error = "No email template created" });
        }
        [HttpPost]
        public async Task<IActionResult> SendWhitePaperEmail(PartnerRegistrationViewModel data)
        {

            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SYNERGY_WHITE_PAPER_EMAIL");
            if (notificationTemplateModel.IsNotNull())
            {

                //var recipients = await _companySettings.GetSingle(x => x.Code == "WEBSITE_SUPPORT_RECIPIENTS");
                var recipient = data.Email;
                string[] attachments = { data.PageId };


                if (recipient.IsNotNullAndNotEmpty())
                {
                    if (BusinessHelper.IsEmailPublicDomain(recipient))
                    {
                        return Json(new { success = false, error = "Please enter your official email id." });
                    }
                    var result = await _emailBusiness.SendMailAsync(
                        new EmailViewModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            To = recipient,
                            Subject = notificationTemplateModel.Subject,
                            Body = notificationTemplateModel.Body,
                            DataAction = DataActionEnum.Create,
                            AttachmentIds= attachments,
                        });

                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = result.Messages.ToHtmlError() });
                }
                else
                {
                    return Json(new { success = false, error = "No support user created" });
                }

            }
            return Json(new { success = false, error = "No email template created" });
        }

        #endregion

    }
    public class UploadResumeViewModel : NoteTemplateViewModel
    {
        public string JobName { get; set; }
        public string Resume { get; set; }

    }
    public class RequestConsultationViewModel
    {
        public string FullName { get; set; }
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
