using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telerik.Web.Spreadsheet;

namespace CMS.UI.Web.Areas.TAS.Controllers
{
    [Area("Tas")]
    public class TalentAssessmentController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ITalentAssessmentBusiness _talentAssessmentBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITeamBusiness _teamBusiness;
        private readonly IEmailBusiness _emailBusiness;

        private readonly IUserBusiness _userBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IUserReportBusiness _userReportBusiness;
        private readonly IUserPromotionBusiness _userPromotionBusiness;
        private readonly IMapper _autoMapper;
        private readonly AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationAccessBusiness _applicationAccessBusiness;
        private readonly IUserPortalBusiness _userportalBusiness;

        public TalentAssessmentController(IHRCoreBusiness hRCoreBusiness, IPortalBusiness portalBusiness, IUserContext userContext,
            INoteBusiness noteBusiness, ILOVBusiness lovBusiness, ICmsBusiness cmsBusiness, ITableMetadataBusiness tableMetadataBusiness,
            ITalentAssessmentBusiness talentAssessmentBusiness, IServiceBusiness serviceBusiness, IWebHostEnvironment webHostEnvironment
            , IUserBusiness userBusiness
            , ITeamBusiness teamBusiness, IEmailBusiness emailBusiness, ITemplateBusiness templateBusiness, IFileBusiness fileBusiness
            , IUserReportBusiness userReportBusiness, IUserPromotionBusiness userPromotionBusiness, IMapper autoMapper
            , AuthSignInManager<ApplicationIdentityUser> customUserManager, IApplicationAccessBusiness applicationAccessBusiness,
            IConfiguration configuration, IUserPortalBusiness userportalBusiness)
        {
            _portalBusiness = portalBusiness;
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _talentAssessmentBusiness = talentAssessmentBusiness;
            _hRCoreBusiness = hRCoreBusiness;
            _lovBusiness = lovBusiness;
            _cmsBusiness = cmsBusiness;
            _serviceBusiness = serviceBusiness;
            _webHostEnvironment = webHostEnvironment;
            _userBusiness = userBusiness;
            _teamBusiness = teamBusiness;
            _emailBusiness = emailBusiness;
            _templateBusiness = templateBusiness;
            _fileBusiness = fileBusiness;
            _userReportBusiness = userReportBusiness;
            _userPromotionBusiness = userPromotionBusiness;
            _autoMapper = autoMapper;
            _customUserManager = customUserManager;
            _configuration = configuration;
            _applicationAccessBusiness = applicationAccessBusiness;
            _userportalBusiness = userportalBusiness;
        }

        public IActionResult Index()
        {
            return View();

        }

        [AllowAnonymous]
        public async Task<IActionResult> SurveyRegister()
        {
            var survey = new SurveyScheduleViewModel();
            var portal = await _portalBusiness.GetSingleById(_userContext.PortalId);
            if (portal != null)
            {
                survey.PortalLogoId = portal.LogoId;
                survey.PortalId = portal.Id;
            }
            return View(survey);
        }
        [HttpPost]
        public async Task<IActionResult> SurveyRegister(SurveyScheduleViewModel model)
        {
            var validate = await ValidateSurveyCode(model.SurveyCode);
            if (validate.Item1)
            {
                return Json(new { success = true, link = validate.Item2 });
            }
            else
            {
                return Json(new { success = false, error = validate.Item2 });
            }
        }
        private async Task<Tuple<bool, string, string>> ValidateSurveyCode(string surveyCode)
        {
            if (surveyCode.IsNullOrEmpty())
            {
                return new Tuple<bool, string, string>(false, "Please Enter Your Survey Code.", null);
            }
            var survey = await _talentAssessmentBusiness.GetSurveyDetails(null, surveyCode);

            if (survey != null)
            {
                if (survey.SurveyEndDate.HasValue)
                {
                    if (survey.LanguageCode == "ARABIC")
                    {
                        return new Tuple<bool, string, string>(false, "لقد أكملت هذا الاستطلاع", null);
                    }
                    return new Tuple<bool, string, string>(false, "You have completed this survey.", null);
                }
                else if (survey.SurveyExpiryDate.HasValue && survey.SurveyExpiryDate < DateTime.Today)
                {
                    return new Tuple<bool, string, string>(false, "This survey is expired.", null);
                }
                if (survey.SurveyUserId != _userContext.UserId)
                {
                    await SetUserContext(survey.SurveyUserId, survey.PortalId);
                }
                var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                //var customUrl = HttpUtility.UrlEncode($"surveyScheduleUserId={survey.SurveyScheduleUserId}");
                var param = Helper.EncryptJavascriptAesCypher($"surveyCode={surveyCode}");
                var portal = await _portalBusiness.GetSingleById(survey.PortalId);
                var link = $"{baseurl}tas/talentassessment/surveyhome?enc={param}";


                return new Tuple<bool, string, string>(true, link, survey.SurveyScheduleUserId);

                //var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                //var customUrl = HttpUtility.UrlEncode($"surveyScheduleUserId={survey.SurveyScheduleUserId}");
                //var param = Helper.EncryptJavascriptAesCypher($"page=surveyhome&customurl={customUrl}");
                //var portal = await _portalBusiness.GetSingleById(survey.PortalId);
                //var link = $"{baseurl}portal/TalentAssessment?enc={param}";
                //return Json(new { success = true, link = link });
            }

            return new Tuple<bool, string, string>(false, "Invalid survey code.", null);
        }
        public async Task<IActionResult> SurveyHome(string surveyCode, bool validated)
        {

            if (surveyCode.IsNullOrEmpty())
            {
                return View(new SurveyScheduleViewModel { Message = "This survey link is invalid" });
            }
            var survey = await _talentAssessmentBusiness.GetSurveyDetails(null, surveyCode);
            ViewBag.Portal = await _portalBusiness.GetSingleById(survey.PortalId);
            //if (survey == null || survey.SurveyUserId != _userContext.UserId)
            //{
            //    return View(new SurveyScheduleViewModel { Message = "This survey link is invalid" });
            //}
            if (survey != null)
            {
                //if (survey.SurveyUserId != _userContext.UserId)
                //{
                //    return View(new SurveyScheduleViewModel { Message = "This survey link is invalid or you are not authorized to access this survey" });
                //}
                if (validated)
                {
                    if (survey.SurveyUserId != _userContext.UserId)
                    {
                        return View(new SurveyScheduleViewModel { Message = "This survey link is invalid or you are not authorized to access this survey" });
                    }
                }
                else
                {
                    if (survey.SurveyUserId != _userContext.UserId)
                    {
                        await SetUserContext(survey.SurveyUserId, survey.PortalId);
                        RedirectToAction("SurveyHome", new { surveyCode = surveyCode, validated = true });
                    }
                }
                survey.StartingInstruction = HttpUtility.HtmlDecode(survey.StartingInstruction);
                if (survey.SurveyEndDate.HasValue)
                {
                    if (survey.LanguageCode == "ARABIC")
                    {
                        survey.Message = "لقد أكملت هذا الاستطلاع";
                    }
                    else
                    {
                        survey.Message = "You have completed this survey.";
                    }
                }
                else if (survey.SurveyExpiryDate.HasValue && survey.SurveyExpiryDate < DateTime.Today)
                {
                    if (survey.LanguageCode == "ARABIC")
                    {
                        survey.Message = "انتهت صلاحية هذا الاستطلاع";
                    }
                    else
                    {
                        survey.Message = "This survey is expired.";
                    }
                }
                return View(survey);
            }

            return View(new SurveyScheduleViewModel { Message = "This survey link is invalid" });
        }

        public async Task SetUserContext(string userId, string portalId)
        {
            var user = await _userBusiness.GetSingleById(userId);
            if (user != null)
            {
                user = await _userBusiness.ValidateUser(user.Email);

                if (user != null)
                {
                    var portal = await _portalBusiness.GetSingleById(portalId);
                    var id = new ApplicationIdentityUser
                    {
                        Id = user.Id,
                        UserName = user.Name,
                        IsSystemAdmin = user.IsSystemAdmin,
                        Email = user.Email,
                        UserUniqueId = user.Email,
                        CompanyId = user.CompanyId,
                        CompanyCode = user.CompanyCode,
                        CompanyName = user.CompanyName,
                        JobTitle = user.JobTitle,
                        PhotoId = user.PhotoId,
                        UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                        UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                        PortalId = portal.Id,
                        IsGuestUser = _userContext.IsGuestUser,
                        UserPortals = user.UserPortals,
                        LoggedInAsByUserId = user.Id,
                        LoggedInAsByUserName = user.Name,
                        LegalEntityId = user.LegalEntityId,
                        LegalEntityCode = user.LegalEntityCode,
                        PersonId = user.PersonId,
                        PositionId = user.PositionId,
                        DepartmentId = user.DepartmentId,
                        PortalName = portal.Name,
                        PortalTheme = portal.Theme.ToString()
                    };
                    id.MapClaims();
                    await _customUserManager.SignInAsync(id, true);
                }

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetSurveyDetails(string surveyId, string surveyScheduleUserId, string surveyUserId)
        {
            var surveyResultExist = await _talentAssessmentBusiness.GetSurveyResultDetails(surveyId, surveyScheduleUserId, surveyUserId);
            if (surveyResultExist != null && surveyResultExist.SurveyStartDate.HasValue)
            {
                var langCode = "ENGLISH";
                var lang = await _lovBusiness.GetSingleById(surveyResultExist.PreferredLanguageId);
                if (lang != null)
                {
                    langCode = lang.Code;

                }
                return Json(new { success = true, serviceId = surveyResultExist.ServiceId, langCode = langCode, topicId = surveyResultExist.CurrentTopicId });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> StartSurvey(string surveyId, string surveyScheduleId, string surveyScheduleUserId, string surveyUserId, string preferredLanguageId)
        {
            var surveyResultExist = await _talentAssessmentBusiness.GetSurveyResultDetails(surveyId, surveyScheduleUserId, surveyUserId);
            if (surveyResultExist != null)
            {
                var langCode = "ENGLISH";
                if (preferredLanguageId.IsNotNullAndNotEmpty())
                {
                    var lang = await _lovBusiness.GetSingleById(preferredLanguageId);
                    if (lang != null)
                    {
                        langCode = lang.Code;
                        await _talentAssessmentBusiness.UpdatePreferredLanguage(surveyScheduleId, preferredLanguageId, surveyResultExist.UdfNoteTableId);
                    }
                }
                var survey = await _talentAssessmentBusiness.GetAssessmentDetailsById(surveyId);
                return Json(new { success = true, serviceId = surveyResultExist.ServiceId, langCode = langCode, surveyName = survey.AssessmentName });
            }
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "S_SURVEY_RESULT";
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            //var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);

            //service.ServiceSubject = type;
            service.OwnerUserId = _userContext.UserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Create;
            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            var surveyResultModel = new SurveyResultViewModel()
            {
                SurveyId = surveyId,
                SurveyScheduleId = surveyScheduleId,
                SurveyScheduleUserId = surveyScheduleUserId,
                SurveyUserId = surveyUserId,
                PreferredLanguage = preferredLanguageId,
                SurveyStartDate = DateTime.Now
            };

            service.Json = JsonConvert.SerializeObject(surveyResultModel);

            var res = await _serviceBusiness.ManageService(service);


            if (res.IsSuccess)
            {
                //await _talentAssessmentBusiness.UpdatePreferredLanguage(surveyScheduleId, preferredLanguageId);
                var lang = await _lovBusiness.GetSingleById(preferredLanguageId);
                var survey = await _talentAssessmentBusiness.GetAssessmentDetailsById(surveyId);
                return Json(new { success = true, serviceId = res.Item.ServiceId, langCode = lang.Code, surveyName = survey.AssessmentName });
            }
            else
            {
                return Json(new { success = false, data = res.Message });
            }

        }

        public IActionResult Topics()
        {
            if (_userContext.PortalId.IsNotNull())
            {
                ViewBag.PortalId = _userContext.PortalId;
            }
            return View();
        }

        public async Task<IActionResult> GetCompetencyLevelList()
        {
            var result = await _talentAssessmentBusiness.GetCompetencyLevelList();
            return Json(result);
        }

        public async Task<JsonResult> GetTopicTreeList(string id, string parentId, string title)
        {
            var list = new List<TASTreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                var noteList = await _noteBusiness.GetList(x => x.TemplateCode == "TAS_TOPIC" && x.ParentNoteId == null);
                long total = 0;
                string[] idlist = noteList.Select(x => x.Id).ToArray();
                var ids = string.Join("','", idlist);
                var quesList = await _talentAssessmentBusiness.GetQuestionCountByTopic(ids);
                total = quesList.Count;
                //foreach (var item in noteList)
                //{
                //    var quesList = await _talentAssessmentBusiness.GetQuestionCountByTopic(item.Id);
                //    total += quesList.Count;

                //}
                list.Add(new TASTreeViewViewModel
                {
                    id = "topics",
                    Name = "Topics",
                    DisplayName = "Topics",
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    Count = total,
                    children = true,
                    text = "Topics" + " (" + total + ")",
                    parent = "#",
                    a_attr = new { data_id = "topics", data_type = "Root", data_name = "Topics", data_parentId = "#" },
                    icon = "fas fa-folder"
                });
            }
            else if (id == "topics")
            {
                var noteList = await _noteBusiness.GetList(x => x.TemplateCode == "TAS_TOPIC" && x.ParentNoteId == null);

                foreach (var item in noteList)
                {
                    // var levelList = await _noteBusiness.GetList(x => x.TemplateCode == "TAS_COMPETENCY_LEVEL" && x.ParentNoteId == item.Id);
                    long total = 0;
                    //var quesList = await _noteBusiness.GetList(x => x.TemplateCode == "TAS_QUESTION" && x.ParentNoteId == item.Id);
                    var quesList = await _talentAssessmentBusiness.GetQuestionCountByTopic(item.Id);
                    total = quesList.Count;

                    //  }

                    var obj = new TASTreeViewViewModel()
                    {
                        id = item.Id,
                        Name = item.NoteSubject,
                        DisplayName = item.NoteSubject,
                        ParentId = item.ParentNoteId.IsNotNull() ? item.ParentNoteId : "topics",
                        hasChildren = true,
                        expanded = false,
                        Type = "topic",
                        Count = total,
                        children = true,
                        text = item.NoteSubject + " (" + total + ")",
                        parent = id,
                        a_attr = new { data_id = item.Id, data_type = "topic", data_name = item.NoteSubject, data_parentId = id },
                        icon = "fal fa-file-contract"
                    };

                    list.Add(obj);
                }
            }
            else
            {
                var noteList = await _noteBusiness.GetList(x => x.ParentNoteId == id && x.TemplateCode == "TAS_TOPIC");
                // var noteList = await _noteBusiness.GetList(x => x.ParentNoteId == id);
                foreach (var item in noteList)
                {
                    long total = 0;
                    // var quesList = await _noteBusiness.GetList(x => x.TemplateCode == "TAS_QUESTION" && x.ParentNoteId == item.Id);
                    var quesList = await _talentAssessmentBusiness.GetQuestionCountByTopic(item.Id);
                    total = quesList.Count;
                    var obj = new TASTreeViewViewModel()
                    {
                        id = item.Id,
                        Name = item.NoteSubject,
                        DisplayName = item.NoteSubject,
                        ParentId = item.ParentNoteId.IsNotNull() ? item.ParentNoteId : "topics",
                        hasChildren = true,
                        expanded = false,
                        Type = "topic",
                        Count = total,
                        children = true,
                        text = item.NoteSubject + " (" + total + ")",
                        parent = id,
                        a_attr = new { data_id = item.Id, data_type = "topic", data_name = item.NoteSubject, data_parentId = id },
                        icon = "fal fa-file-contract"
                    };

                    list.Add(obj);
                }
                //var treelist = noteList.Select(x => new TreeViewViewModel()
                //{
                //    id = x.Id,
                //    Name = x.NoteSubject,
                //    DisplayName = x.NoteSubject,
                //    ParentId = x.ParentNoteId.IsNotNull() ? x.ParentNoteId : "topics",
                //    hasChildren = true,
                //    expanded = false,
                //    Type = "topic"
                //});
                //list.AddRange(treelist);
                // if (treelist.IsNotNull())
                // {
                var levelList = await _talentAssessmentBusiness.GetCompetencyLevelListByTopic(id);
                list.AddRange(levelList.Select(x => new TASTreeViewViewModel()
                {
                    id = x.Id,
                    Name = x.Name,
                    DisplayName = x.Name,
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "level",
                    Count = x.Count,
                    children = false,
                    text = x.Name + " (" + x.Count + ")",
                    parent = id,
                    a_attr = new { data_id = x.Id, data_type = "level", data_name = x.Name, data_parentId = id },
                    icon = "fal fa-layer-group"
                }));
                //var where = $@"  and ""N_TAS_Question"".""TopicId"" = '{id}' and ""N_TAS_Question"".""CompentencyLevelId"" = '{x.Id}'";
                //var data = await _cmsBusiness.GetDataListByTemplate("TAS_QUESTION", "", where);
                //var total = data.Rows.Count;
                //if (total != 0)
                //{
                //    var obj = new TreeViewViewModel()
                //    {


            }
            list = list.OrderBy(x => x.DisplayName).ToList();
            return Json(list.ToList());
        }

        public IActionResult ManageTopic(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageTopic(string subject, string parentId)
        {
            var model = new NoteTemplateViewModel
            {
                TemplateCode = "TAS_TOPIC"
            };
            var note = await _noteBusiness.GetNoteDetails(model);
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            note.NoteSubject = subject;
            note.StartDate = DateTime.Now;
            var lovMeduim = await _lovBusiness.GetSingle(x => x.Code == "NOTE_PRIORITY_MEDIUM");
            note.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
            note.DataAction = Synergy.App.Common.DataActionEnum.Create;
            note.ParentNoteId = parentId.IsNullOrEmpty() ? null : parentId;

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("Topic", subject);

            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            var result = await _noteBusiness.ManageNote(note);
            return Json(result);
        }

        public IActionResult Levels()
        {
            if (_userContext.PortalId.IsNotNull())
            {
                ViewBag.PortalId = _userContext.PortalId;
            }
            return View();
        }

        public async Task<JsonResult> GetLevelsTreeList(string id, string parentId)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                var compList = await _talentAssessmentBusiness.GetCompetencyList();
                var total = 0;
                foreach (var item in compList)
                {
                    var ind = await _talentAssessmentBusiness.GetIndicatorList(item.Id);
                    total += ind.Count;
                }
                list.Add(new TreeViewViewModel
                {
                    id = "levels",
                    Name = "Levels",
                    DisplayName = "Levels",
                    ParentId = null,
                    hasChildren = true,
                    expanded = false,
                    Type = "Root",
                    Count = total,
                    children = true,
                    text = "Levels" + " (" + total + ")",
                    parent = "#",
                    a_attr = new { data_id = "levels", data_type = "Root", data_name = "Levels", data_parentId = "#" },
                    icon = "fas fa-folder"
                });
            }
            else if (id == "levels")
            {
                var compList = await _talentAssessmentBusiness.GetCompetencyList();

                foreach (var item in compList)
                {
                    var ind = await _talentAssessmentBusiness.GetIndicatorList(item.Id);
                    var obj = new TreeViewViewModel()
                    {
                        id = item.Id,
                        Name = item.Name,
                        DisplayName = item.Name,
                        ParentId = "levels",
                        hasChildren = false,
                        expanded = false,
                        Type = "level",
                        Count = ind.Count,
                        children = false,
                        text = item.Name + " (" + ind.Count + ")",
                        parent = id,
                        a_attr = new { data_id = item.Id, data_type = "level", data_name = item.Name, data_parentId = "levels" },
                        icon = "fal fa-layer-group"
                    };

                    list.Add(obj);
                }
            }

            return Json(list.ToList());
        }

        public IActionResult AssessmentQuestionsIndex()
        {
            return View();
        }
        public async Task<IActionResult> AssessmentQuestions(QuestionTypeEnum? questionType, string noteId, LayoutModeEnum? lo, string topicId, string levelId)
        {
            AssessmentQuestionsViewModel model = new AssessmentQuestionsViewModel();
            if (questionType.IsNotNull())
            {
                model.QuestionType = questionType.Value;
            }
            model.DataAction = DataActionEnum.Create;
            model.TopicId = topicId;
            model.CompentencyLevelId = levelId;
            if (noteId.IsNotNullAndNotEmpty())
            {
                var data = await _tableMetadataBusiness.GetTableDataByColumn("TAS_QUESTION", "", "NtsNoteId", noteId);
                if (data != null)
                {
                    model.Id = data["Id"].ToString();
                    model.Question = data["Question"].ToString();
                    model.QuestionArabic = data["QuestionArabic"].ToString();
                    model.QuestionDescription = data["QuestionDescription"].ToString();
                    model.QuestionDescriptionArabic = data["QuestionDescriptionArabic"].ToString();
                    model.NoteId = data["NtsNoteId"].ToString();
                    model.QuestionAttachmentId = data["QuestionAttachmentId"].ToString();
                    model.QuestionArabicAttachmentId = data["QuestionArabicAttachmentId"].ToString();
                    // model.TopicId = data["TopicId"].ToString();
                    model.CompentencyLevelId = data["CompentencyLevelId"].ToString();
                    model.IndicatorId = data["IndicatorId"].ToString();
                    model.AssessmentTypeId = data["AssessmentTypeId"].ToString();
                    model.EnableComment = (data["EnableComment"].ToString() != null && data["EnableComment"].ToString() != "null" && data["EnableComment"].ToString() != "") ? Convert.ToBoolean(data["EnableComment"]) : false;
                    model.DataAction = DataActionEnum.Edit;
                    if (model.TopicId.IsNullOrEmpty())
                    {
                        model.TopicId = data["TopicId"].ToString();
                    }
                }
            }
            if (lo == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }
        public IActionResult AssessmentQuestionOption(string noteId)
        {
            AssessmentQuestionsViewModel model = new AssessmentQuestionsViewModel();
            model.NoteId = noteId;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageQuestions(AssessmentQuestionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Question = model.Question != null ? HttpUtility.HtmlDecode(model.Question) : null;
                model.QuestionArabic = model.QuestionArabic != null ? HttpUtility.HtmlDecode(model.QuestionArabic) : null;
                model.QuestionDescription = model.QuestionDescription != null ? HttpUtility.HtmlDecode(model.QuestionDescription) : null;
                model.QuestionDescriptionArabic = model.QuestionDescriptionArabic != null ? HttpUtility.HtmlDecode(model.QuestionDescriptionArabic) : null;
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "TAS_QUESTION";
                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.ParentNoteId = model.TopicId;
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        // Create 5 options by default for Question
                        for (var i = 0; i < 5; i++)
                        {
                            AssessmentQuestionsOptionViewModel optionModel = new AssessmentQuestionsOptionViewModel();
                            optionModel.Option = "Option " + i;
                            optionModel.OptionArabic = "Option " + i;
                            optionModel.IsRightAnswer = false;
                            optionModel.Score = 0;
                            optionModel.AnswerKey = "";
                            optionModel.DataAction = DataActionEnum.Create;
                            optionModel.ParentNoteId = result.Item.NoteId;
                            optionModel.SequenceOrder = i + 1;
                            await ManageOptions(optionModel);
                        }
                        return Json(new { success = true, result = result.Item });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    // noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.Options.IsNotNull())
                        {
                            var options = JsonConvert.DeserializeObject<List<AssessmentQuestionsOptionViewModel>>(model.Options);
                            var existingList = await _talentAssessmentBusiness.GetOptionsForQuestion(model.NoteId);
                            if (existingList != null)
                            {
                                List<string> newlistids = new List<string>();
                                foreach (var component in options)
                                {
                                    newlistids.Add(component.OptionId);
                                }
                                var diff = existingList.Where(item => !newlistids.Contains(item.Id));
                                if (diff.Any())
                                {
                                    foreach (var item in diff)
                                    {
                                        // Delete the option
                                        await _talentAssessmentBusiness.DeleteOption(item.OptionId);
                                    }
                                }
                                // Check for existing Components
                                //long sequenceOrder = 1;
                                foreach (var component in options)
                                {
                                    var data = await _talentAssessmentBusiness.GetOptionsById(component.OptionId);//_tableMetadataBusiness.GetTableDataByColumn("TAS_QUESTION_OPTION", "", "Id", component.OptionId);
                                    if (data != null)
                                    {
                                        // edit existing
                                        data.DataAction = DataActionEnum.Edit;
                                        data.ParentNoteId = result.Item.NoteId;
                                        data.Option = component.Option;
                                        data.OptionArabic = component.OptionArabic;
                                        data.IsRightAnswer = component.IsRightAnswer;
                                        data.Score = component.Score;
                                        data.SequenceOrder = component.SequenceOrder;
                                        data.AnswerKey = component.AnswerKey;
                                        await ManageOptions(data);
                                    }
                                    else
                                    {
                                        // Create New Option
                                        component.ParentNoteId = result.Item.NoteId;
                                        component.DataAction = DataActionEnum.Create;
                                        component.SequenceOrder = component.SequenceOrder;
                                        await ManageOptions(component);
                                    }
                                    //sequenceOrder = sequenceOrder + 1;
                                }
                            }



                        }
                        return Json(new { success = true, result = result.Item });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            else
            {
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }

        }
        public async Task<IActionResult> ReadQuestion([DataSourceRequest] DataSourceRequest request)
        {
            var data = new List<AssessmentQuestionsViewModel>();
            // ViewBag.JobCriterias = await _listOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");

            //var model = await _talentAssessmentBusiness.GetQuestion();
            var model = await _talentAssessmentBusiness.GetTreeListQuestion();
            data = model.ToList();

            var dsResult = data;
            //var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public async Task<object> ReadQuestionTree(string id)
        {
            var data = new List<AssessmentQuestionsViewModel>();
            // ViewBag.JobCriterias = await _listOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");

            //var model = await _talentAssessmentBusiness.GetQuestion();
            var model = await _talentAssessmentBusiness.GetTreeListQuestion();
            if (id.IsNotNullAndNotEmpty())
            {
                model = model.Where(x => x.ParentId == id).ToList();
            }
            else
            {
                model = model.Where(x => x.ParentId == null).ToList();
            }
            data = model.ToList();
            // var dt = JsonConvert.SerializeObject(data);
            //var newList = new List<FileExplorerViewModel>();
            //newList.AddRange(data.ToList().Select(x => new FileExplorerViewModel
            //{
            //    key = x.Id,
            //    title = x.NoteSubject,
            //    lazy = true,
            //    // FieldDataType = x.FieldDataType.ToString(),
            //    ParentId = id
            //}));
            var json = JsonConvert.SerializeObject(data);
            // var dsResult = data.ToDataSourceResult(request);
            return json;
        }

        public async Task<IActionResult> ReadQuestionOptions(/*[DataSourceRequest] DataSourceRequest request,*/string questionNoteId)
        {
            var data = new List<AssessmentQuestionsOptionViewModel>();
            // ViewBag.JobCriterias = await _listOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            if (questionNoteId.IsNotNullAndNotEmpty())
            {
                var model = await _talentAssessmentBusiness.GetOptionsForQuestion(questionNoteId);
                data = model.OrderBy(x => x.SequenceOrder).ToList();
            }
            //  var dsResult = data.ToDataSourceResult(request);
            // return Json(dsResult);
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> ManageOptions(AssessmentQuestionsOptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "TAS_QUESTION_OPTION";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                    notemodel.ParentNoteId = model.ParentNoteId;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    noteTempModel.ParentNoteId = model.ParentNoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            else
            {
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }

        }

        public IActionResult Assessments()
        {
            AssessmentViewModel model = new AssessmentViewModel()
            {
                ActiveUserId = _userContext.UserId,
            };
            return View(model);
        }

        public async Task<IActionResult> ReadAssessmentData(/*[DataSourceRequest] DataSourceRequest request,*/ string type, string searchtext)
        {
            var result = await _talentAssessmentBusiness.GetAssessmentsList(type, searchtext);

            return Json(result/*.ToDataSourceResult(request)*/);

        }
        public async Task<IActionResult> EmployeeDashboard()
        {
            // var model = new PersonProfileViewModel();
            var userId = _userContext.UserId;

            var person = await _talentAssessmentBusiness.GetAssignmentDetails(userId);
            // model.PersonId = person.PersonId;

            return View("EmployeeDashboard", person);
        }
        public async Task<IActionResult> ReadBadgeData([DataSourceRequest] DataSourceRequest request)
        {
            // var model = new PersonProfileViewModel();
            var userId = _userContext.UserId;

            var person = await _talentAssessmentBusiness.ReadBadgeData(userId);
            // model.PersonId = person.PersonId;

            return Json(person);
            //return Json(person.ToDataSourceResult(request));
        }
        public async Task<IActionResult> ReadAssessmentsData([DataSourceRequest] DataSourceRequest request)
        {
            // var model = new PersonProfileViewModel();
            var userId = _userContext.UserId;

            var person = await _talentAssessmentBusiness.ReadAssessmentData(userId);
            // model.PersonId = person.PersonId;

            return Json(person);
            // return Json(person.ToDataSourceResult(request));
        }
        public async Task<IActionResult> ReadcertificationData([DataSourceRequest] DataSourceRequest request)
        {
            // var model = new PersonProfileViewModel();
            var userId = _userContext.UserId;

            var person = await _talentAssessmentBusiness.ReadCertificationData(userId);
            // model.PersonId = person.PersonId;

            return Json(person);
        }
        public async Task<IActionResult> ReadSkillData([DataSourceRequest] DataSourceRequest request)
        {
            // var model = new PersonProfileViewModel();
            var userId = _userContext.UserId;

            var person = await _talentAssessmentBusiness.ReadSkillData(userId);
            // model.PersonId = person.PersonId;

            return Json(person);
        }
        public async Task<ActionResult> ReadDevelopmentServiceData([DataSourceRequest] DataSourceRequest request)
        {
            var list = await _talentAssessmentBusiness.ReadPerformanceDevelopmentViewData();
            return Json(list);
        }
        public async Task<ActionResult> ReadPerformanceTaskView([DataSourceRequest] DataSourceRequest request, string Id)
        {


            var list = await _talentAssessmentBusiness.ReadPerformanceTaskViewData(Id);

            return Json(list);
            //}
        }

        public async Task<ActionResult> DeleteAssessment(string Id)
        {
            var result = await _talentAssessmentBusiness.DeleteAssessment(Id);
            return Json(new { success = result });
        }

        public IActionResult CopyAssessment(string assessmentId)
        {
            AssessmentViewModel model = new AssessmentViewModel()
            {
                AssessmentId = assessmentId,
                DataAction = DataActionEnum.Create
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CopyAssessment(AssessmentViewModel model)
        {
            var assessment = await _talentAssessmentBusiness.GetAssessmentDetailsById(model.AssessmentId);
            model.AssessmentDescription = assessment.AssessmentDescription;
            model.AssessmentTypeId = assessment.AssessmentTypeId;
            model.JobId = assessment.JobId;
            model.Weightage = assessment.Weightage;
            model.AssessmentDuration = assessment.AssessmentDuration;

            await _talentAssessmentBusiness.CopyAssessment(model);

            return Json(new { success = true });
        }
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, AssessmentQuestionsOptionViewModel lov)
        {

            return Json(new[] { lov });
        }

        public IActionResult ManageQuestions(string assessmentId, string assessmentTypeId)
        {
            var model = new AssessmentViewModel()
            {
                AssessmentId = assessmentId,
                AssessmentTypeId = assessmentTypeId
            };
            return View(model);
        }

        public async Task<IActionResult> ReadMappedQuestions([DataSourceRequest] DataSourceRequest request, string AssessmentId)
        {
            var result = await _talentAssessmentBusiness.GetMappedQuestionsList(AssessmentId);

            return Json(result);
        }
        public async Task<IActionResult> ReadMappedQuestions1(string AssessmentId)
        {
            var result = await _talentAssessmentBusiness.GetMappedQuestionsList(AssessmentId);

            return Json(result);
        }

        public async Task<ActionResult> UpdateSequenceNo(string Id, long? sequenceNo, string assessmentId)
        {
            var qlist = await _talentAssessmentBusiness.GetMappedQuestionsList(assessmentId);
            var exist = qlist.Where(x => x.SequenceOrder == sequenceNo && x.Id != Id).ToList();
            if (exist.Any())
            {
                return Json(new { success = false, error = "Sequence No already exists." });
            }
            else
            {
                var result = await _talentAssessmentBusiness.UpdateSequenceNo(Id, sequenceNo);
                return Json(new { success = result });
            }
        }

        public async Task<IActionResult> ReadQuestions([DataSourceRequest] DataSourceRequest request, string topic, string level)
        {
            //var data = new System.Data.DataTable();
            //if (level.IsNotNull())
            //{
            //    //var where = $@"  and ""N_TAS_Question"".""TopicId"" = '{topic}' and ""N_TAS_Question"".""CompentencyLevelId"" = '{level}'";
            //    var where = $@"  and ""NtsNote"".""ParentNoteId"" = '{topic}' and ""N_TAS_Question"".""CompentencyLevelId"" = '{level}'";
            //    data = await _cmsBusiness.GetDataListByTemplate("TAS_QUESTION", "", where);
            //}
            //else if (level == null && topic.IsNotNull())
            //{
            //    //var where = $@" and ""N_TAS_Question"".""TopicId"" = '{topic}'";
            //    var where = $@" and ""NtsNote"".""ParentNoteId"" = '{topic}'";
            //    data = await _cmsBusiness.GetDataListByTemplate("TAS_QUESTION", "", where);
            //}
            //else
            //{
            //    data = await _cmsBusiness.GetDataListByTemplate("TAS_QUESTION", "");
            //}

            var data = await _talentAssessmentBusiness.GetAssessmentQuestions(topic, level);

            return Json(data);
            //return Json(data);
        }

        public async Task<IActionResult> ReadIndicators(/*[DataSourceRequest] DataSourceRequest request, */string level)
        {
            var data = new System.Data.DataTable();
            if (level.IsNotNullAndNotEmpty())
            {
                var where = $@" and ""N_TAS_Indicator"".""CompetencyLevelId"" = '{level}'";
                data = await _cmsBusiness.GetDataListByTemplate("INDICATOR", "", where);
            }
            else
            {
                data = await _cmsBusiness.GetDataListByTemplate("INDICATOR", "");
            }
            return Json(data/*.ToDataSourceResult(request)*/);
        }
        public async Task<ActionResult> DeleteAssessmentQuestion(string Id)
        {
            var result = await _talentAssessmentBusiness.DeleteAssessmentQuestion(Id);
            return Json(new { success = result });
        }

        public IActionResult AssessmentDashboard()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ImportQuestionnaire(IList<IFormFile> file)
        {
            if (file != null)
            {
                var fileName = "";
                var physicalPath = "";
                foreach (var fil in file)
                {
                    fileName = Path.GetFileName(fil.FileName);
                    string contentRootPath = _webHostEnvironment.ContentRootPath;

                    string path = "";

                    physicalPath = Path.Combine(contentRootPath, "wwwroot", fileName);
                    //physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                    //file.SaveAs(physicalPath);
                    using (FileStream stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        fil.CopyTo(stream);
                    }
                }
                // var result = UploadTenantDetails(physicalPath);
                //var result = await ManageQuestionnaire(physicalPath);
                var result = await ManageQuestionnaireExcel(physicalPath);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

                if (result.Count == 0)
                {
                    //return Json(new { success = true, operation = DataOperation.Create.ToString() });
                    return Json(new { success = true, errors = result });
                }
                else
                {
                    return Json(new { success = false, errors = result });
                }

            }

            return Content("");
        }
        public async Task<List<string>> ManageQuestionnaire(string physicalPath)
        {
            var errorList = new List<string>();
            //CommandResult<NoteViewModel> result;            
            try
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    using (TextFieldParser parser = new TextFieldParser(physicalPath))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        parser.ReadFields();
                        var i = 1;
                        var topicId = "";
                        var levelId = "";
                        var AssessmentTypeId = "";
                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields().ToList();
                            try
                            {

                                if (i == 1)
                                {
                                    var topic = fields[0];
                                    var assessment = fields[1];
                                    var lovMeduim = await _lovBusiness.GetSingle(x => x.Code == "NOTE_PRIORITY_MEDIUM");
                                    var texist = await _noteBusiness.GetSingle(x => x.NoteSubject == topic && x.TemplateCode == "TAS_TOPIC");
                                    if (texist == null)
                                    {
                                        var model = new NoteTemplateViewModel
                                        {
                                            TemplateCode = "TAS_TOPIC"
                                        };
                                        var note = await _noteBusiness.GetNoteDetails(model);
                                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        note.NoteSubject = topic;
                                        note.StartDate = DateTime.Now;

                                        note.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
                                        note.DataAction = Synergy.App.Common.DataActionEnum.Create;

                                        dynamic exo = new System.Dynamic.ExpandoObject();

                                        ((IDictionary<String, Object>)exo).Add("Topic", topic);

                                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var result = await _noteBusiness.ManageNote(note);
                                        topicId = result.Item.NoteId;
                                    }
                                    else
                                    {
                                        topicId = texist.Id;
                                    }

                                    //compentency 


                                    var assesstype = await _talentAssessmentBusiness.GetAssessmentbyName(assessment);
                                    if (assesstype.IsNotNull())
                                    {

                                        AssessmentTypeId = assesstype.Id;
                                    }



                                    //var cexist = await _noteBusiness.GetSingle(x => x.NoteSubject == topic && x.TemplateCode == "TAS_COMPETENCY_LEVEL" && x.ParentNoteId == topicId);
                                    //if (cexist == null)
                                    //{
                                    //    var cmodel = new NoteTemplateViewModel
                                    //    {
                                    //        TemplateCode = "TAS_COMPETENCY_LEVEL"
                                    //    };
                                    //    var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                    //    cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    //    cnote.NoteSubject = level;
                                    //    cnote.StartDate = DateTime.Now;
                                    //    cnote.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
                                    //    cnote.DataAction = Common.DataActionEnum.Create;
                                    //    cnote.ParentNoteId = topicId;
                                    //    dynamic exo1 = new System.Dynamic.ExpandoObject();
                                    //
                                    //    ((IDictionary<String, Object>)exo1).Add("CompetencyLevel1", level);
                                    //    ((IDictionary<String, Object>)exo1).Add("sequence", 1);
                                    //
                                    //    cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    //    cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                    //    var result1 = await _noteBusiness.ManageNote(cnote);
                                    //    levelId = result1.Item.NoteId;
                                    //}
                                    //else
                                    //{
                                    //    levelId = cexist.Id;
                                    //}
                                }
                                else if (i == 2)
                                {

                                }

                                else
                                {
                                    var Competency = fields[0];
                                    var indcator = fields[1];
                                    var question = fields[2];
                                    var questionAr = fields[3];
                                    var desc = fields[4];
                                    var descAr = fields[5];
                                    var ImageName = fields[6];
                                    var ImageNameAr = fields[7];
                                    var rightAns = fields[8];
                                    var score = fields[9];
                                    var AttachmentId = "";
                                    var AttachmentIdAr = "";


                                    if (ImageName.IsNotNull())
                                    {
                                        var imgfile = await _fileBusiness.GetFileByName(ImageName);
                                        if (imgfile.IsNotNull())
                                        {
                                            AttachmentId = imgfile.Id;
                                        }
                                    }

                                    if (ImageNameAr.IsNotNull())
                                    {
                                        var imgfile = await _fileBusiness.GetFileByName(ImageNameAr);
                                        if (imgfile.IsNotNull())
                                        {
                                            AttachmentIdAr = imgfile.Id;
                                        }
                                    }

                                    if (Competency.IsNotNull())
                                    {
                                        var CompetencyResult = await _talentAssessmentBusiness.GetCompetencybyName(Competency);
                                        if (CompetencyResult == null)
                                        {

                                            var cmodel = new NoteTemplateViewModel
                                            {
                                                TemplateCode = "TAS_COMPETENCY_LEVEL"
                                            };
                                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.NoteSubject = Competency;
                                            cnote.StartDate = DateTime.Now;

                                            cnote.DataAction = Synergy.App.Common.DataActionEnum.Create;

                                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                                            ((IDictionary<String, Object>)exo1).Add("CompetencyLevel", Competency);

                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                            var result1 = await _noteBusiness.ManageNote(cnote);
                                            Competency = result1.Item.UdfNoteTableId;


                                        }
                                        else { Competency = CompetencyResult.Id; }
                                    }



                                    if (indcator.IsNotNull())
                                    {
                                        var indicatorResult = await _talentAssessmentBusiness.GetIndicatorbyName(indcator);
                                        if (indicatorResult == null)
                                        {

                                            var cmodel = new NoteTemplateViewModel
                                            {
                                                TemplateCode = "INDICATOR"
                                            };
                                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.NoteSubject = indcator;
                                            cnote.StartDate = DateTime.Now;

                                            cnote.DataAction = Synergy.App.Common.DataActionEnum.Create;

                                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                                            ((IDictionary<String, Object>)exo1).Add("CompetencyLevelId", Competency);
                                            ((IDictionary<String, Object>)exo1).Add("IndicatorName", indcator);

                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                            var result1 = await _noteBusiness.ManageNote(cnote);
                                            indcator = result1.Item.UdfNoteTableId;

                                        }
                                        else { indcator = indicatorResult.Id; }
                                    }


                                    var noteTempModel = new NoteTemplateViewModel();
                                    var qModel = new AssessmentQuestionsViewModel();
                                    noteTempModel.DataAction = Synergy.App.Common.DataActionEnum.Create;
                                    noteTempModel.ActiveUserId = _userContext.UserId;
                                    noteTempModel.TemplateCode = "TAS_QUESTION";
                                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                    qModel.CompentencyLevelId = Competency;
                                    qModel.IndicatorId = indcator;
                                    qModel.AssessmentTypeId = AssessmentTypeId;
                                    qModel.Question = question.Replace("�", "\"");
                                    qModel.Question = qModel.Question.Replace("'", "");
                                    qModel.QuestionArabic = questionAr.Replace("�", "\"");
                                    qModel.QuestionArabic = qModel.QuestionArabic.Replace("'", "");
                                    qModel.QuestionDescription = desc;
                                    qModel.QuestionDescriptionArabic = descAr;
                                    qModel.QuestionAttachmentId = AttachmentId;
                                    qModel.QuestionArabicAttachmentId = AttachmentIdAr;




                                    // qModel.QuestionAttachmentId = data["QuestionAttachmentId"].ToString();
                                    // qModel.TopicId = topicId;
                                    // qModel.CompentencyLevelId = levelId;
                                    qModel.DataAction = DataActionEnum.Create;
                                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(qModel);
                                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    notemodel.ParentNoteId = topicId;
                                    var qresult = await _noteBusiness.ManageNote(notemodel);
                                    if (qresult.IsSuccess)
                                    {
                                        // Create 5 options by default for Question
                                        var k = 9;
                                        var l = 10;
                                        for (var j = 1; j < 5; j++)
                                        {

                                            AssessmentQuestionsOptionViewModel optionModel = new AssessmentQuestionsOptionViewModel();
                                            optionModel.Option = fields[k + j];
                                            optionModel.OptionArabic = fields[l + j];
                                            optionModel.IsRightAnswer = false;
                                            if (optionModel.Option == rightAns)
                                            {
                                                optionModel.IsRightAnswer = true;
                                            }
                                            if (score.IsNotNullAndNotEmpty())
                                            {
                                                optionModel.Score = Convert.ToDouble(score);
                                            }
                                            optionModel.AnswerKey = "";
                                            optionModel.DataAction = DataActionEnum.Create;
                                            optionModel.ParentNoteId = qresult.Item.NoteId;
                                            optionModel.SequenceOrder = j;
                                            await ManageOptions(optionModel);
                                            k++;
                                            l++;
                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                errorList.Add(string.Concat("Error Questionnaire :", fields[0]));
                                errorList.Add(string.Concat("Error :", ex.Message));
                                return errorList;
                            }
                            // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                            i++;
                        }
                    }
                }
                return errorList;
            }
            catch (Exception ex)
            {
                errorList.Add(string.Concat("Error :", ex.Message));
                return errorList; ;
            }
        }
        public async Task<List<string>> ManageQuestionnaireExcel(string physicalPath)
        {
            var errorList = new List<string>();
            //CommandResult<NoteViewModel> result;            
            try
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    using (var sl = new SLDocument(physicalPath))
                    {
                        var stats = sl.GetWorksheetStatistics();
                        var i = 2;
                        var topicId = "";
                        var levelId = "";
                        var AssessmentTypeId = "";
                        while (i <= stats.EndRowIndex)
                        {
                            //var fields = parser.ReadFields().ToList();
                            try
                            {

                                if (i == 2)
                                {
                                    var Sequence = sl.GetCellValueAsInt64(i, 1);//fields[0];
                                    var topic = sl.GetCellValueAsString(i, 2).Trim();//fields[0];
                                    var assessment = sl.GetCellValueAsString(i, 3).Trim(); //fields[1];
                                    var lovMeduim = await _lovBusiness.GetSingle(x => x.Code == "NOTE_PRIORITY_MEDIUM");
                                    var texist = await _noteBusiness.GetSingle(x => x.NoteSubject == topic && x.TemplateCode == "TAS_TOPIC");
                                    if (texist == null)
                                    {
                                        var model = new NoteTemplateViewModel
                                        {
                                            TemplateCode = "TAS_TOPIC"
                                        };
                                        var note = await _noteBusiness.GetNoteDetails(model);
                                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        note.NoteSubject = topic;
                                        note.StartDate = DateTime.Now;
                                        note.SequenceOrder = Sequence;
                                        note.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
                                        note.DataAction = Synergy.App.Common.DataActionEnum.Create;

                                        dynamic exo = new System.Dynamic.ExpandoObject();

                                        ((IDictionary<String, Object>)exo).Add("Topic", topic);

                                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var result = await _noteBusiness.ManageNote(note);
                                        topicId = result.Item.NoteId;
                                    }
                                    else
                                    {
                                        topicId = texist.Id;
                                    }

                                    //compentency 


                                    var assesstype = await _talentAssessmentBusiness.GetAssessmentbyName(assessment);
                                    if (assesstype.IsNotNull())
                                    {

                                        AssessmentTypeId = assesstype.Id;
                                    }



                                    //var cexist = await _noteBusiness.GetSingle(x => x.NoteSubject == topic && x.TemplateCode == "TAS_COMPETENCY_LEVEL" && x.ParentNoteId == topicId);
                                    //if (cexist == null)
                                    //{
                                    //    var cmodel = new NoteTemplateViewModel
                                    //    {
                                    //        TemplateCode = "TAS_COMPETENCY_LEVEL"
                                    //    };
                                    //    var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                    //    cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    //    cnote.NoteSubject = level;
                                    //    cnote.StartDate = DateTime.Now;
                                    //    cnote.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
                                    //    cnote.DataAction = Common.DataActionEnum.Create;
                                    //    cnote.ParentNoteId = topicId;
                                    //    dynamic exo1 = new System.Dynamic.ExpandoObject();
                                    //
                                    //    ((IDictionary<String, Object>)exo1).Add("CompetencyLevel1", level);
                                    //    ((IDictionary<String, Object>)exo1).Add("sequence", 1);
                                    //
                                    //    cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    //    cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                    //    var result1 = await _noteBusiness.ManageNote(cnote);
                                    //    levelId = result1.Item.NoteId;
                                    //}
                                    //else
                                    //{
                                    //    levelId = cexist.Id;
                                    //}
                                }
                                else if (i == 3)
                                {

                                }

                                else
                                {
                                    var Competency = sl.GetCellValueAsString(i, 1).Trim(); //fields[0];
                                    var indcator = sl.GetCellValueAsString(i, 2).Trim();  //fields[1];
                                    var question = sl.GetCellValueAsString(i, 3).Trim(); //fields[2];
                                    var questionAr = sl.GetCellValueAsString(i, 4).Trim(); // fields[3];
                                    var desc = sl.GetCellValueAsString(i, 5).Trim(); //fields[4];
                                    var descAr = sl.GetCellValueAsString(i, 6).Trim();  //fields[5];
                                    var ImageName = sl.GetCellValueAsString(i, 7).Trim(); //fields[6];
                                    var ImageNameAr = sl.GetCellValueAsString(i, 8).Trim(); //fields[7];
                                    var rightAns = sl.GetCellValueAsString(i, 9).Trim(); //fields[8];
                                    var score = sl.GetCellValueAsString(i, 10).Trim(); //fields[9];
                                    var AttachmentId = "";
                                    var AttachmentIdAr = "";


                                    if (ImageName.IsNotNull())
                                    {
                                        var imgfile = await _fileBusiness.GetFileByName(ImageName);
                                        if (imgfile.IsNotNull())
                                        {
                                            AttachmentId = imgfile.Id;
                                        }
                                    }

                                    if (ImageNameAr.IsNotNull())
                                    {
                                        var imgfile = await _fileBusiness.GetFileByName(ImageNameAr);
                                        if (imgfile.IsNotNull())
                                        {
                                            AttachmentIdAr = imgfile.Id;
                                        }
                                    }

                                    if (Competency.IsNotNull())
                                    {
                                        var CompetencyResult = await _talentAssessmentBusiness.GetCompetencybyName(Competency);
                                        if (CompetencyResult == null)
                                        {

                                            var cmodel = new NoteTemplateViewModel
                                            {
                                                TemplateCode = "TAS_COMPETENCY_LEVEL"
                                            };
                                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.NoteSubject = Competency;
                                            cnote.StartDate = DateTime.Now;

                                            cnote.DataAction = Synergy.App.Common.DataActionEnum.Create;

                                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                                            ((IDictionary<String, Object>)exo1).Add("CompetencyLevel", Competency);

                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                            var result1 = await _noteBusiness.ManageNote(cnote);
                                            Competency = result1.Item.UdfNoteTableId;


                                        }
                                        else { Competency = CompetencyResult.Id; }
                                    }



                                    if (indcator.IsNotNull())
                                    {
                                        var indicatorResult = await _talentAssessmentBusiness.GetIndicatorbyName(indcator);
                                        if (indicatorResult == null)
                                        {

                                            var cmodel = new NoteTemplateViewModel
                                            {
                                                TemplateCode = "INDICATOR"
                                            };
                                            var cnote = await _noteBusiness.GetNoteDetails(cmodel);
                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.NoteSubject = indcator;
                                            cnote.StartDate = DateTime.Now;

                                            cnote.DataAction = Synergy.App.Common.DataActionEnum.Create;

                                            dynamic exo1 = new System.Dynamic.ExpandoObject();

                                            ((IDictionary<String, Object>)exo1).Add("CompetencyLevelId", Competency);
                                            ((IDictionary<String, Object>)exo1).Add("IndicatorName", indcator);

                                            cnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                            cnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                            var result1 = await _noteBusiness.ManageNote(cnote);
                                            indcator = result1.Item.UdfNoteTableId;

                                        }
                                        else { indcator = indicatorResult.Id; }
                                    }


                                    var noteTempModel = new NoteTemplateViewModel();
                                    var qModel = new AssessmentQuestionsViewModel();
                                    noteTempModel.DataAction = Synergy.App.Common.DataActionEnum.Create;
                                    noteTempModel.ActiveUserId = _userContext.UserId;
                                    noteTempModel.TemplateCode = "TAS_QUESTION";
                                    //noteTempModel.ParentNoteId = model.ParentNoteId;
                                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                                    qModel.CompentencyLevelId = Competency;
                                    qModel.IndicatorId = indcator;
                                    qModel.AssessmentTypeId = AssessmentTypeId;
                                    qModel.Question = question.Replace("�", "\"");
                                    qModel.Question = qModel.Question.Replace("'", "");
                                    qModel.QuestionArabic = questionAr.Replace("�", "\"");
                                    qModel.QuestionArabic = qModel.QuestionArabic.Replace("'", "");
                                    qModel.QuestionDescription = desc;
                                    qModel.QuestionDescriptionArabic = descAr;
                                    qModel.QuestionAttachmentId = AttachmentId;
                                    qModel.QuestionArabicAttachmentId = AttachmentIdAr;




                                    // qModel.QuestionAttachmentId = data["QuestionAttachmentId"].ToString();
                                    // qModel.TopicId = topicId;
                                    // qModel.CompentencyLevelId = levelId;
                                    qModel.DataAction = DataActionEnum.Create;
                                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(qModel);
                                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                    notemodel.ParentNoteId = topicId;
                                    var qresult = await _noteBusiness.ManageNote(notemodel);
                                    if (qresult.IsSuccess)
                                    {
                                        // Create 5 options by default for Question
                                        var k = 10;
                                        var l = 11;
                                        for (var j = 1; j < 6; j++)
                                        {

                                            AssessmentQuestionsOptionViewModel optionModel = new AssessmentQuestionsOptionViewModel();
                                            optionModel.Option = sl.GetCellValueAsString(i, k + j).Trim(); //fields[k + j];
                                            optionModel.OptionArabic = sl.GetCellValueAsString(i, l + j).Trim(); //fields[l + j];
                                            optionModel.IsRightAnswer = false;
                                            if (optionModel.Option == rightAns)
                                            {
                                                optionModel.IsRightAnswer = true;
                                            }
                                            if (score.IsNotNullAndNotEmpty())
                                            {
                                                optionModel.Score = Convert.ToDouble(score);
                                            }
                                            optionModel.AnswerKey = "";
                                            optionModel.DataAction = DataActionEnum.Create;
                                            optionModel.ParentNoteId = qresult.Item.NoteId;
                                            optionModel.SequenceOrder = j;
                                            await ManageOptions(optionModel);
                                            k++;
                                            l++;
                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                //errorList.Add(string.Concat("Error Questionnaire :", fields[0]));
                                errorList.Add(string.Concat("Error Questionnaire :", sl.GetCellValueAsString(i, 1).Trim()));
                                errorList.Add(string.Concat("Error :", ex.Message));
                                return errorList;
                            }
                            // errorList.Add(string.Concat("Success Questionnaire :", fields[0]));
                            i++;
                        }
                    }
                }
                return errorList;
            }
            catch (Exception ex)
            {
                errorList.Add(string.Concat("Error :", ex.Message));
                return errorList; ;
            }
        }
        public async Task<IActionResult> ReadAssessmentList([DataSourceRequest] DataSourceRequest request, string Status)
        {
            var result = await _talentAssessmentBusiness.GetAssessmentListByUserId(_userContext.UserId, Status);
            return Json(result);
            // return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadCompletedAssessmentList([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _talentAssessmentBusiness.GetCompletedAssessmentListByUserId(_userContext.UserId);
            return Json(result);
            //  return Json(result.ToDataSourceResult(request));
        }

        public IActionResult ManageAssessments(string userId = null)
        {
            ViewBag.UserId = userId;
            return View();
        }
        public async Task<IActionResult> ReadAssessmentsList(/*[DataSourceRequest] DataSourceRequest request,*/ string userId = null)
        {
            var result = await _talentAssessmentBusiness.GetAllAssessmentsList();
            if (userId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.UserId == userId).ToList();
            }
            return Json(result/*.ToDataSourceResult(request)*/);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessment(string assessmentId, string userId, string type, string lang, string serviceId, string scheduledAssessmentId)
        {
            var assResultModel = new AssessmentResultViewModel()
            {
                AssessmentId = assessmentId,
                UserId = userId,
                PreferredLanguageId = lang,
                IsAssessmentStopped = false,
                ScheduledAssessmentId = scheduledAssessmentId
            };

            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_RESULT";
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            //var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);

            service.ServiceSubject = type;
            service.OwnerUserId = _userContext.UserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Create;
            service.ParentServiceId = serviceId;
            service.ServiceStatusCode = "SERVICE_STATUS_DRAFT";

            service.Json = JsonConvert.SerializeObject(assResultModel);

            var res = await _serviceBusiness.ManageService(service);

            //return Json(new { success = res.IsSuccess, error=res.HtmlError.ToHtmlError() });
            if (res.IsSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = res.HtmlError.ToHtmlError() });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAssessmentInProgess(string serviceId)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            var service = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_SCHEDULE";
            serviceModel.ServiceId = serviceId;
            serviceModel.DataAction = DataActionEnum.Edit;
            serviceModel.SetUdfValue = true;
            service = await _serviceBusiness.GetServiceDetails(serviceModel);
            //service.StartDate = DateTime.Now;
            var exo = service.ColumnList.ToDictionary(x => x.Name, x => x.Value);

            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Edit;
            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            service.Json = JsonConvert.SerializeObject(exo);
            service.AllowPastStartDate = true;
            var res = await _serviceBusiness.ManageService(service);

            if (res.IsSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                //return Json(new { success = false});
                ModelState.AddModelErrors(res.Messages);
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }

        public IActionResult DosDontPage(string serviceId, string assessmentType, string language, string status)
        {
            var model = new AssessmentDetailViewModel()
            {
                ServiceId = serviceId,
                AssessmentType = assessmentType,
                PreferredLanguageCode = language,
                AssessmentStatus = status
            };

            return View(model);
        }

        public async Task<ActionResult> AssessmentInstruction()
        {
            var list = await _talentAssessmentBusiness.GetCalendarScheduleList();
            var url = list.Where(x => x.CandidateId == _userContext.UserId).FirstOrDefault();
            var model = new AssessmentDetailViewModel();
            if (url != null)
            {
                model.AssessmentZoomUrl = url.Url;
            }
            return View(model);
        }

        public IActionResult Tryout(string Id, string type, string currentQueId = null, string LanguageCode = null, string Status = null, LayoutModeEnum layout = LayoutModeEnum.Main)
        {
            var model = new AssessmentResultViewModel()
            {
                AssessmentId = Id,
                PreferredLanguageCode = LanguageCode,
                LayoutMode = layout,
                AssessmentType = type,
                CurrentQuestionId = currentQueId
            };
            return View(model);
        }

        public async Task<ActionResult> GetTryoutQuestion(string assessmentId, string currentQueId, string lang = null)
        {
            var result = await _talentAssessmentBusiness.GetTryoutQuestion(assessmentId, currentQueId, lang);
            return Json(result);
        }

        public async Task<IActionResult> Questions(string Id, string type, string currentQueId = null, string LanguageCode = null, string Status = null, LayoutModeEnum layout = LayoutModeEnum.Main)
        {
            var result = await _talentAssessmentBusiness.GetQuestion(Id);
            result.AssessmentType = type;
            result.PreferredLanguageCode = LanguageCode;
            if (LanguageCode != null)
            {
                result.ServiceId = Id;
                result.CurrentQuestionId = currentQueId;
                result.PreferredLanguageCode = LanguageCode;
            }
            if (Status == "Draft")
            {
                await _talentAssessmentBusiness.UpdateAssessment(Id, null, null, null, DateTime.Now);
            }
            result.LayoutMode = layout;
            return View(result);
        }

        public async Task<IActionResult> GetIndicatorList()
        {
            var result = await _talentAssessmentBusiness.GetIndicatorList(null);
            return Json(result);
        }
        public async Task<IActionResult> GetTopicList()
        {

            var noteList = await _noteBusiness.GetList(x => x.TemplateCode == "TAS_TOPIC");
            return Json(noteList);
        }



        public async Task<ActionResult> GetAssessmentQuestion(string serviceId, string currentQueId, string lang = null)
        {
            AssessmentDetailViewModel result = null;
            if (lang != null)
            {
                if (lang == "ARABIC")
                {
                    result = await _talentAssessmentBusiness.GetAssessmentQuestionArabic(serviceId, currentQueId);
                }
                else
                {
                    result = await _talentAssessmentBusiness.GetAssessmentQuestionEnglish(serviceId, currentQueId);
                }
            }
            else
            {
                result = await _talentAssessmentBusiness.GetAssessmentQuestionEnglish(serviceId, currentQueId);
            }
            return Json(result);

        }
        [HttpPost]
        public async Task<ActionResult> SubmitAssessmentAnswer(string serviceId, bool isSubmit, string currentQuestionId, string multipleFieldValueId, string comment, int? timeElapsed, int? timeElapsedSec, string nextQuestionId, string currentAnswerId, string assessmentType, string assessmentId)
        {
            await _talentAssessmentBusiness.SubmitAssessmentAnswer(serviceId, isSubmit, currentQuestionId, multipleFieldValueId, comment, timeElapsed, timeElapsedSec, nextQuestionId, currentAnswerId, assessmentType, _userContext.UserId, assessmentId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAssessment(string serviceId, int? timeElapsed, int? timeElapsedSec, string currentQuestionId)
        {
            var result = await _talentAssessmentBusiness.UpdateAssessment(serviceId, timeElapsed, timeElapsedSec, currentQuestionId);
            return Json(new { success = true, IsAssessmentStopped = result });
        }

        [HttpPost]
        public async Task<ActionResult> ManageAssessmentStatus(string serviceId, bool isAssessmentStopped)
        {
            await _talentAssessmentBusiness.ManageAssessmentStatus(serviceId, isAssessmentStopped);
            return Json(new { success = true });
        }


        public IActionResult MapAssessmentQuestions(string assessmentId, string assessmentTypeId)
        {
            var model = new AssessmentViewModel()
            {
                AssessmentId = assessmentId,
                AssessmentTypeId = assessmentTypeId
            };
            return View("MapQuestions", model);
        }

        public async Task<IActionResult> ReadUnMappedQuestions([DataSourceRequest] DataSourceRequest request, string AssessmentId, string AssessmentTypeId)
        {
            var qList = await _talentAssessmentBusiness.GetAllQuestions(AssessmentTypeId);

            var existingList = await _talentAssessmentBusiness.GetQuestionsByAssessmentId(AssessmentId);

            foreach (var ques in existingList)
            {
                //qList = qList.Where(x => x.Id != ques.QuestionId).ToList();
                qList = qList.Where(x => x.Question != ques.Question).ToList();
            }

            return Json(qList);
        }

        [HttpPost]
        public async Task<IActionResult> MapQuestions(string assessmentId, string qIds)
        {
            var result = await _talentAssessmentBusiness.MapQuestionsToAssessment(assessmentId, qIds);

            return Json(new { success = result });
        }
        public async Task<ActionResult> InterviewSchedule()
        {
            var role = "INTERVIEWER";
            var userid = _userContext.UserId;
            //var user = _userBusiness.GetUserList(LoggedInUserId, "");
            var user = await _userBusiness.ValidateUserById(userid);
            var isAdmin = _userContext.IsSystemAdmin;
            if (isAdmin /*LoggedInUserIsAdmin*/)
            {
                role = "ADMIN";
            }
            if (user.IsNotNull() && user.UserRoles.IsNotNull())
            {
                foreach (var r in user.UserRoles)
                {
                    //var roleData = _userroleBusiness.GetSingle(x => x.Id == r);
                    if (r.Name == "Proctor")
                    {
                        role = "PROCTOR";
                    }
                    else if (isAdmin /*LoggedInUserIsAdmin*/)
                    {
                        role = "ADMIN";
                    }
                    else if (r.Name == "Central")
                    {
                        role = "CENTRAL";
                    }
                    else
                    {
                        role = "CANDIDATE";
                    }
                }
            }

            //var data = _business.GetCalendarScheduleList(LoggedInUserId, role);
            //var list = new List<IdNameViewModel>();
            //foreach (var p in data)
            //{
            //    list.Add(new IdNameViewModel
            //    {
            //        Id = p.Id,
            //        Name = p.Title,
            //        Code = p.Color,
            //    });
            //}


            ViewBag.UserRole = role;
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetAssessmentUserList()
        {
            List<IdNameViewModel> model = new List<IdNameViewModel>();
            var data = await _userBusiness.GetList();
            model = data.Select(e => new IdNameViewModel()
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return Json(model);
        }
        [HttpGet]
        public async Task<JsonResult> GetJobTitle()
        {
            var loggedInUserId = _userContext.UserId;
            var data = await _talentAssessmentBusiness.GetJobTitle(loggedInUserId);
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetAssessmentSponserList()
        {
            List<IdNameViewModel> model = new List<IdNameViewModel>();
            //var data = _sponsorBusiness.GetActiveIdNameList();
            //model = data.Select(e => new IdNameViewModel()
            //{
            //    Id = e.Id,
            //    Name = e.Name
            //}).ToList();
            return Json(model);
        }
        [HttpGet]
        public async Task<ActionResult> GetAssessmentProctorList()
        {
            var data = await _talentAssessmentBusiness.GetAssessmentProctorList();
            return Json(data);
        }
        public async Task<JsonResult> ReadCalendarInterviewScheduleData([DataSourceRequest] DataSourceRequest request, string type, string[] jobTitle, string[] proctor, string[] interviewer,
        string title, string[] candidate/*, long[] ministry*/)
        {
            var schedueList = await SchedulerData(type, jobTitle, proctor, interviewer, title, candidate/*, ministry*/);
            return Json(schedueList/*.ToDataSourceResult(request)*/);
        }

        public async Task<JsonResult> ReadCalendarInterviewScheduleDataTui(string type, string[] jobTitle, string[] proctor, string[] interviewer,
        string title, string[] candidate/*, long[] ministry*/)
        {
            var schedueList = await SchedulerData(type, jobTitle, proctor, interviewer, title, candidate/*, ministry*/);
            return Json(schedueList);
        }
        private async Task<JsonResult> SchedulerData(string type, string[] jobTitle, string[] proctor, string[] interviewer, string title, string[] candidate/*, long[] ministry*/)
        {
            var result = new List<AssessmentCalendarViewModel>();
            var filterList = new List<AssessmentCalendarViewModel>();
            var role = "INTERVIEWER";
            //var user = _userBusiness.GetUserList(LoggedInUserId, "");
            var userid = _userContext.UserId;
            var isAdmin = _userContext.IsSystemAdmin;
            var user = await _userBusiness.ValidateUserById(userid);
            if (user.IsNotNull() && user.UserRoles.IsNotNull())
            {
                foreach (var r in user.UserRoles)
                {
                    //var roleData = _userroleBusiness.GetSingle(x => x.Id == r);
                    if (r.Name == "Proctor")
                    {
                        role = "PROCTOR";
                    }
                    else if (isAdmin /*LoggedInUserIsAdmin*/)
                    {
                        role = "ADMIN";
                    }
                    else if (r.Name == "Central")
                    {
                        role = "CENTRAL";
                    }
                    else if (r.Name == "Assessment User")
                    {
                        role = "CANDIDATE";
                    }
                }
            }

            var proctors = await _talentAssessmentBusiness.GetCalendarScheduleList(_userContext.UserId, role);
            foreach (var p in proctors)
            {
                p.IsAllDay = false;
                //if (p.MinistryId.IsNotNull())
                //{
                //    var ministryDetails = _sponsorBusiness.GetSingleById(p.MinistryId.Value);
                //    p.MinistryName = ministryDetails.Name;
                //}
                //else
                //{
                //    p.MinistryName = "-";
                //}

                p.JobTitle = p.JobTitle.IsNullOrEmpty() ? "-" : p.JobTitle;
                if (p.Description == "Technical")
                {
                    p.AssessmentTypeShort = "TA";
                }
                else if (p.Description == "Case Study")
                {
                    p.AssessmentTypeShort = "CS";
                }
                else
                {
                    p.AssessmentTypeShort = p.Description;
                }
                if (p.Url.IsNotNullAndNotEmpty() && p.Url.Length > 4)
                {
                    //p.UrlShort = p.Url.Substring(p.Url.Length - 4);
                    p.UrlShort = p.Url;
                }
                else
                {
                    p.UrlShort = "-";
                }
                if (p.Color == null)
                {
                    p.Color = "#DCDCDC";
                }
                if (p.CandidateName.IsNullOrEmpty())
                {
                    p.CandidateName = "-";
                }

                if (p.SlotType == AssessmentScheduleTypeEnum.Assessment)
                {
                    p.Description = "Proctor";
                    p.SlotTypeText = AssessmentScheduleTypeEnum.Assessment.ToString();
                }
                else if (p.SlotType == AssessmentScheduleTypeEnum.Interview)
                {
                    p.Description = "Interviewer";
                    p.SlotTypeText = AssessmentScheduleTypeEnum.Interview.ToString();
                }
                else
                {
                    p.Description = "";
                }

                if (p.Description == "Interviewer")
                {
                    if (p.InterviewPanelId.IsNotNull())
                    {
                        //var datalist = await _teamBusiness.GetActiveList();
                        var datalist = await _teamBusiness.GetList();
                        var data = datalist.Where(x => x.Id == p.InterviewPanelId).FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            p.Title = data.Name;
                        }
                        p.Color = "#4aa5f0";
                    }
                    else
                    {
                        p.Title = "-";
                    }

                }
                else if (p.Description == "Proctor")
                {
                    if (p.ProctorId.IsNotNull())
                    {
                        var datalist = await _userBusiness.GetUserList();
                        var data = datalist.Where(x => x.Id == p.ProctorId).FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            p.Title = data.Name;
                        }
                        p.Color = "#8cc265";
                    }
                    else
                    {
                        p.Title = "-";
                    }
                }
                else
                {
                    p.Title = "-";
                }
                if (role == "CANDIDATE" || role == "INTERVIEWER")
                {
                    p.Email = "-";
                }
                else
                {
                    if (p.Email.IsNullOrEmpty())
                    {
                        p.Email = "-";
                    }
                }

                if (p.MobileNo.IsNullOrEmpty())
                {
                    p.MobileNo = "Not Available";
                }
                if (p.AssessmentTypeName.IsNullOrEmpty())
                {
                    p.AssessmentTypeName = "";
                }
                if (p.AssessmentName.IsNullOrEmpty())
                {
                    p.AssessmentName = "";
                }

            }
            var schedueList = new List<AssessmentCalendarViewModel>();

            if (type == ((int)AssessmentScheduleTypeEnum.Assessment).ToString())
            {
                //schedueList = proctors.Where(x => x.Description.Contains("Proctor")).ToList();
                proctors = proctors.Where(x => x.Description.Contains("Proctor")).ToList();
            }
            else if (type == ((int)AssessmentScheduleTypeEnum.Interview).ToString())
            {
                //schedueList = proctors.Where(x => x.Description.Contains("Interview")).ToList();
                proctors = proctors.Where(x => x.Description.Contains("Interview")).ToList();
            }
            //if (title.IsNotNullAndNotEmpty())
            //{
            //    schedueList = proctors.Where(x => x.Title == title).ToList();
            //}                      

            if (proctor.IsNotNull())
            {
                foreach (var j in proctor)
                {
                    //var proctorlist = proctors.Where(x => x.ProctorId == j).ToList();
                    schedueList.AddRange(proctors.Where(x => x.ProctorId == j).ToList());
                }
            }

            if (interviewer.IsNotNull())
            {
                foreach (var j in interviewer)
                {
                    //var interviewerlist = proctors.Where(x => x.InterviewPanelId == j).ToList();
                    schedueList.AddRange(proctors.Where(x => x.InterviewPanelId == j).ToList());
                }
            }

            if (jobTitle.IsNotNull())
            {
                foreach (var j in jobTitle)
                {
                    //var joblist = proctors.Where(x => x.JobTitle == j).ToList();
                    schedueList.AddRange(proctors.Where(x => x.JobTitle == j).ToList());
                }
            }

            if (candidate.IsNotNull())
            {
                foreach (var j in candidate)
                {
                    //var canlist = proctors.Where(x => x.CandidateId == j).ToList();
                    schedueList.AddRange(proctors.Where(x => x.CandidateId == j).ToList());
                }
            }

            //if (ministry.IsNotNull())
            //{
            //    foreach (var j in ministry)
            //    {
            //        schedueList.AddRange(proctors.Where(x => x.MinistryId == j).ToList());
            //    }
            //}

            if ((type == null || type == "") && jobTitle.Count() == 0 && proctor.Count() == 0 && interviewer.Count() == 0 && title.IsNullOrEmpty() && candidate.Count() == 0  /*&& ministry == null*/)
            {
                schedueList = proctors;
            }

            //return schedueList.DistinctBy(x => x.Id).ToList();
            var list = new List<ScheduleInfo>();
            var schedulers = schedueList.DistinctBy(x => x.Id).ToList();
            schedueList.ForEach(x => list.Add(new ScheduleInfo
            {
                id = x.Id,
                calendarId = x.Id,
                title = x.Title,
                start = x.Start.ToString(),
                end = x.End.ToString(),
                color = x.Color,

            }));
            return Json(list);
        }
        [HttpGet]
        public async Task<ActionResult> GetAssessmentSetList()
        {
            var data = await _talentAssessmentBusiness.GetAssessmentSetListIdName();
            return Json(data);
        }
        [HttpGet]
        public async Task<ActionResult> GetAssessmentList(string assSetId)
        {
            var data = await _talentAssessmentBusiness.GetAssessmentListIdName(assSetId);
            return Json(data);

        }
        public async Task<ActionResult> ViewCalendarEvent(string id, DateTime? start)
        {
            var role = "INTERVIEWER";
            var userid = _userContext.UserId;
            var isAdmin = _userContext.IsSystemAdmin;
            var user = await _userBusiness.ValidateUserById(userid);
            //var user = _userBusiness.GetUserList(LoggedInUserId, "");
            if (user.IsNotNull() && user.UserRoles.IsNotNull())
            {
                foreach (var r in user.UserRoles)
                {
                    //var roleData = _userroleBusiness.GetSingle(x => x.Id == r);
                    if (r.Name == "Proctor")
                    {
                        role = "PROCTOR";
                    }
                    else if (isAdmin /*LoggedInUserIsAdmin*/)
                    {
                        role = "ADMIN";
                    }
                    else if (r.Name == "Admin")
                    {
                        role = "ADMIN";
                    }
                    else if (r.Name == "Central")
                    {
                        role = "CENTRAL";
                    }
                    else
                    {
                        role = "CANDIDATE";
                    }
                }
            }
            ViewBag.Role = role;
            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                var model = new AssessmentCalendarViewModel
                {
                    Start = start.IsNotNull() ? start.Value : DateTime.Now
                };
                model.Status = StatusEnum.Active;
                model.End = model.Start;
                ViewBag.AssessmentType = "";
                return View(model);
            }
            else
            {
                var model = await _talentAssessmentBusiness.GetCalendarScheduleList(_userContext.UserId, role);
                var data = model.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.AssessmentType = data.SlotType;
                ViewBag.VersionNo = "";
                //var noteDetails = _noteBusiness.GetDetails(new NoteViewModel { Id = data.Id });
                //if (noteDetails.IsNotNull())
                //{
                //    ViewBag.VersionNo = noteDetails.VersionNo;
                //}
                //var data = new AssessmentCalendarViewModel();
                //data.Start = DateTime.Today;
                //data.End = DateTime.Today;
                return View(data);
            }
        }
        public async Task<ActionResult> PreviewEmail(string slotId)
        {
            var List = await _talentAssessmentBusiness.GetSlotUserEmail(slotId);
            return View("_PreviewEmail", List);
        }
        public async Task<ActionResult> ViewEmail(string emailId, string slotId)
        {
            var email = await _emailBusiness.GetSingleById<EmailViewModel, Email>(emailId);
            //var email = await _talentAssessmentBusiness.GetEmailById(emailId);
            //var calendar = BusinessHelper.MapModel<EmailViewModel, EmailCalendarViewModel>(email);
            var calendar = email;
            calendar.ReferenceId = slotId;
            //calendar.EmailId = email.Id;
            calendar.DataAction = DataActionEnum.Edit;
            calendar.Operation = DataOperation.Correct;
            return View("Email", calendar);
        }
        public async Task<IActionResult> Email(string slotId, string userId, DateTime start, DateTime end)
        {
            var calendar = new EmailViewModel();
            var user = await _userBusiness.GetSingleById(userId);
            calendar.To = user.Email;
            calendar.RecipientName = user.Name;
            calendar.StartDate = start;
            calendar.EndDate = end;
            calendar.SlotId = slotId;
            calendar.ReferenceId = slotId;
            //calendar.ReferenceNode = NodeEnum.NTS_Note;
            calendar.DataAction = DataActionEnum.Create;
            calendar.Operation = DataOperation.Create;
            return View(calendar);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailViewModel calendar)
        {

            await _emailBusiness.SendCalendarMail(calendar);

            //if (calendar.ReferenceId.IsNotNull())
            //{
            //    var note = new NoteViewModel
            //    {
            //        Id = calendar.ReferenceId,
            //        DataAction = DataActionEnum.Edit,
            //    };

            //    var newmodel = await _noteBusiness.GetSingle(note);

            //    foreach (var n in newmodel.Controls)
            //    {
            //        if (n.FieldName == "planStartDate")
            //        {
            //            //n.Value = calendar.StartDate.ToString();
            //            n.Value = calendar.StartDate.ToDefaultDateTimeFormat();
            //            n.Code = calendar.StartDate.Value.ToUTCFormat();
            //        }

            //        if (n.FieldName == "planEndDate")
            //        {
            //            n.Value = calendar.EndDate.ToDefaultDateTimeFormat();
            //            n.Code = calendar.EndDate.Value.ToUTCFormat();
            //        }
            //    }
            //    newmodel.TemplateAction = NtsActionEnum.Submit;
            //    newmodel.DataAction = DataActionEnum.Edit;
            //    var result = _noteBusiness.Manage(newmodel);
            //}
            //if (calendar.ReferenceId.IsNotNull())
            //{
            //    var calschedule = await _talentAssessmentBusiness.GetCalendarScheduleById(calendar.ReferenceId);
            //    if (calschedule.ServiceId.IsNotNullAndNotEmpty())
            //    {
            //        dynamic exo = new System.Dynamic.ExpandoObject();
            //        ((IDictionary<String, Object>)exo).Add("JobTitle", calschedule.JobTitle);
            //        ((IDictionary<String, Object>)exo).Add("UserId", calschedule.CandidateId);
            //        ((IDictionary<String, Object>)exo).Add("InterviewerId", calschedule.InterviewPanelId);
            //        ((IDictionary<String, Object>)exo).Add("ProctorId", calschedule.ProctorId);
            //        ((IDictionary<String, Object>)exo).Add("Url", calschedule.Url);
            //        ((IDictionary<String, Object>)exo).Add("AttachmentId", calschedule.AttachmentId);
            //        ((IDictionary<String, Object>)exo).Add("AssessmentSetId", calschedule.AssessmentSetId);
            //        ((IDictionary<String, Object>)exo).Add("SlotType", (int)calschedule.SlotType);
            //        ((IDictionary<String, Object>)exo).Add("Status", (int)calschedule.Status);
            //        ((IDictionary<String, Object>)exo).Add("AssessmentId", calschedule.AssessmentId);
            //        ((IDictionary<String, Object>)exo).Add("PreferedLanguageId", calschedule.PreferredLanguageId);
            //        ((IDictionary<String, Object>)exo).Add("MonitoringTypeId", calschedule.MonitoringTypeId);
            //        ((IDictionary<String, Object>)exo).Add("InterviewWeightage", calschedule.InterviewWeightage);

            //        ((IDictionary<String, Object>)exo).Add("StartDate", calendar.StartDate);
            //        ((IDictionary<String, Object>)exo).Add("EndDate", calendar.EndDate);

            //        ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            //        var service = new ServiceTemplateViewModel();
            //        serviceModel.ActiveUserId = _userContext.UserId;
            //        serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_SCHEDULE";
            //        serviceModel.ServiceId = calschedule.ServiceId;
            //        serviceModel.DataAction = DataActionEnum.Edit;
            //        service = await _serviceBusiness.GetServiceDetails(serviceModel);

            //        service.ActiveUserId = _userContext.UserId;
            //        service.DataAction = DataActionEnum.Edit;
            //        service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            //        service.Json = JsonConvert.SerializeObject(exo);
            //        var res = await _serviceBusiness.ManageService(service);
            //    }
            //}
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> AddUpdateCalendarEvents(AssessmentCalendarViewModel model, bool isDrag = false)
        {

            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("JobTitle", model.JobTitle);
            ((IDictionary<String, Object>)exo).Add("AssessmentStartDate", model.Start);
            ((IDictionary<String, Object>)exo).Add("EndDate", model.End);
            ((IDictionary<String, Object>)exo).Add("UserId", model.CandidateId);
            ((IDictionary<String, Object>)exo).Add("InterviewerId", model.InterviewPanelId);
            ((IDictionary<String, Object>)exo).Add("ProctorId", model.ProctorId);
            ((IDictionary<String, Object>)exo).Add("Url", model.Url);
            ((IDictionary<String, Object>)exo).Add("AttachmentId", model.AttachmentId);
            ((IDictionary<String, Object>)exo).Add("AssessmentSetId", model.AssessmentSetId);
            ((IDictionary<String, Object>)exo).Add("SlotType", (int)model.SlotType);
            ((IDictionary<String, Object>)exo).Add("Status", (int)model.Status);
            ((IDictionary<String, Object>)exo).Add("AssessmentId", model.AssessmentId);
            ((IDictionary<String, Object>)exo).Add("PreferedLanguageId", model.PreferredLanguageId);
            ((IDictionary<String, Object>)exo).Add("MonitoringTypeId", model.MonitoringTypeId);
            ((IDictionary<String, Object>)exo).Add("InterviewWeightage", model.InterviewWeightage);
            ((IDictionary<String, Object>)exo).Add("Location", model.Location);

            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            var service = new ServiceTemplateViewModel();
            if (model.Id.IsNullOrEmpty())
            {
                serviceModel.ActiveUserId = _userContext.UserId;
                serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_SCHEDULE";
                service = await _serviceBusiness.GetServiceDetails(serviceModel);

                service.ServiceSubject = model.JobTitle;
                service.OwnerUserId = _userContext.UserId;
                service.StartDate = DateTime.Now;
                service.DueDate = DateTime.Now.AddDays(2);
                service.ActiveUserId = _userContext.UserId;
                service.DataAction = DataActionEnum.Create;
                service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                service.Json = JsonConvert.SerializeObject(exo);
            }
            else
            {
                if (model.ServiceId.IsNotNullAndNotEmpty())
                {
                    serviceModel.ActiveUserId = _userContext.UserId;
                    serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_SCHEDULE";
                    serviceModel.ServiceId = model.ServiceId;
                    serviceModel.DataAction = DataActionEnum.Edit;
                    service = await _serviceBusiness.GetServiceDetails(serviceModel);

                    service.ActiveUserId = _userContext.UserId;
                    service.DataAction = DataActionEnum.Edit;
                    service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    service.Json = JsonConvert.SerializeObject(exo);
                    service.AllowPastStartDate = true;
                }
            }

            var res = await _serviceBusiness.ManageService(service);
            if (res.IsSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = res.HtmlError.ToHtmlError() });
            }

            //var note = new NoteViewModel();
            //if (model.Id == 0)
            //{
            //    note = new NoteViewModel
            //    {
            //        TemplateMasterCode = "ASSESSMENT_SLOT",
            //        Operation = DataOperation.Create,
            //        OwnerUserId = LoggedInUserId,
            //        ActiveUserId = LoggedInUserId,
            //        RequestedByUserId = LoggedInUserId,
            //        TemplateAction = NtsActionEnum.Submit,
            //        Status = StatusEnum.Active,
            //        ReferenceType = NoteReferenceTypeEnum.Self,
            //    };
            //}
            //else
            //{
            //    note = new NoteViewModel
            //    {
            //        Id = model.Id,
            //        Operation = DataOperation.Correct,
            //    };
            //}
            //var newmodel = _noteBusiness.GetDetails(note);

            //if (!isDrag)
            //{
            //    foreach (var n in newmodel.Controls)
            //    {
            //        if (n.FieldName == "planStartDate")
            //        {
            //            n.Value = model.Start.ToDefaultDateTimeFormat();
            //            n.Code = model.Start.ToUTCFormat();
            //        }

            //        if (n.FieldName == "planEndDate")
            //        {
            //            n.Value = model.End.ToDefaultDateTimeFormat();
            //            n.Code = model.End.ToUTCFormat();
            //        }

            //        if (n.FieldName == "jobTitle")
            //        {
            //            n.Value = model.JobTitle.IsNotNull() ? model.JobTitle.ToString() : "";
            //            n.Code = model.JobTitle.IsNotNull() ? model.JobTitle.ToString() : "";
            //        }

            //        if (n.FieldName == "attachment")
            //        {
            //            n.Value = model.AttachmentId.IsNotNull() ? model.AttachmentId.ToString() : "";
            //            n.Code = model.AttachmentId.IsNotNull() ? model.AttachmentId.ToString() : "";
            //        }

            //        if (n.FieldName == "ministry")
            //        {
            //            n.Value = model.MinistryId.IsNotNull() ? model.MinistryId.ToString() : "";
            //            n.Code = model.MinistryId.IsNotNull() ? model.MinistryId.ToString() : "";
            //        }

            //        if (n.FieldName == "interviewer")
            //        {
            //            n.Value = model.InterviewPanelId.IsNotNull() ? model.InterviewPanelId.ToString() : "";
            //            n.Code = model.InterviewPanelId.IsNotNull() ? model.InterviewPanelId.ToString() : "";
            //        }

            //        if (n.FieldName == "proctor")
            //        {
            //            n.Value = model.ProctorId.IsNotNull() ? model.ProctorId.ToString() : "";
            //            n.Code = model.ProctorId.IsNotNull() ? model.ProctorId.ToString() : "";
            //        }

            //        if (n.FieldName == "url")
            //        {
            //            n.Value = model.Url.IsNotNull() ? model.Url.ToString() : "";
            //            n.Code = model.Url.IsNotNull() ? model.Url.ToString() : "";
            //        }
            //        if (n.FieldName == "attachment")
            //        {
            //            n.Value = model.AttachmentId.IsNotNull() ? model.AttachmentId.ToString() : "";
            //            n.Code = model.AttachmentId.IsNotNull() ? model.AttachmentId.ToString() : "";
            //        }
            //        if (n.FieldName == "slotType")
            //        {
            //            n.Value = model.SlotType.IsNotNull() ? model.SlotType.ToString() : "";
            //            n.Code = model.SlotType.IsNotNull() ? model.SlotType.ToString() : "";
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var n in newmodel.Controls)
            //    {
            //        if (n.FieldName == "planStartDate")
            //        {
            //            n.Value = model.Start.ToDefaultDateTimeFormat();
            //            n.Code = model.Start.ToUTCFormat();
            //        }

            //        if (n.FieldName == "planEndDate")
            //        {
            //            n.Value = model.End.ToDefaultDateTimeFormat();
            //            n.Code = model.End.ToUTCFormat();
            //        }
            //    }
            //}
            //if (model.CandidateId.IsNotNull())
            //{
            //    newmodel.ReferenceType = NoteReferenceTypeEnum.User;
            //    newmodel.ReferenceTo = model.CandidateId;
            //}
            //if (model.AssessmentSetId.IsNotNull())
            //{
            //    newmodel.ReferenceTypeCode = ReferenceTypeEnum.NTS_Note;
            //    newmodel.ReferenceNode = NodeEnum.NTS_Note;
            //    newmodel.ReferenceId = model.AssessmentSetId;
            //}

            //if (model.Status.IsNotNull())
            //{
            //    newmodel.Status = model.Status.Value;
            //}


            //var result = _noteBusiness.Manage(newmodel);
            //if (!result.IsSuccess)
            //{
            //    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
            //    return Json(new { success = false, action = newmodel.TemplateAction.ToString(), errors = ModelState.SerializeErrors() });
            //}
            //else
            //{
            //    return Json(new { success = true, action = newmodel.TemplateAction.ToString(), operation = model.Id.ToString(), NoteId = model.Id });
            //}
        }

        public async Task<IActionResult> DeleteSchedule(string id)
        {
            await _talentAssessmentBusiness.DeleteAssessmentSchedule(id);
            return Json(new { success = true });
        }
        public IActionResult ManageInterview(string userId = null, string source = null)
        {
            var model = new AssessmentInterviewViewModel();
            ViewBag.UserId = userId;
            model.Source = source;
            return View(model);
        }

        public async Task<IActionResult> GetAssessmentInterviewList([DataSourceRequest] DataSourceRequest request, bool isarchived = false, string userId = null, string source = null)
        {
            string role = _userContext.UserRoleCodes;
            var result = await _talentAssessmentBusiness.GetAssessmentInterview(_userContext.UserId, isarchived, source);
            //result.ForEach(e => e.IsAdmin = LoggedInUserIsAdmin);
            if (!isarchived)
            {
                result = result.Where(u => u.IsRowArchived != true).ToList();
            }
            if (userId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.Id == userId).ToList();
            }

            var j = Json(result);
            return j;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessmentInterview(string userId, string interviewScheduleId)
        {

            //TimeSpan? duration = null;
            //duration = new TimeSpan(0, 30, 0);
            int row = 1;


            var assessment = await _talentAssessmentBusiness.GetInterviewService(userId);

            if (assessment != null && assessment.ServiceId != null)
            {
                return Json(new { success = true, id = assessment.ServiceId });
            }

            var assResultModel = new AssessmentInterviewViewModel()
            {
                ScheduledStartDate = DateTime.Now,
                ScheduledEndDate = DateTime.Now.AddMinutes(30),
                ActualStartDate = DateTime.Now,
                UserId = userId,
                InterviewScheduleId = interviewScheduleId
            };
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_INTERVIEW";
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            var user = await _userBusiness.GetSingle(x => x.Id == userId);
            //var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);
            if (user.IsNotNull())
            {
                service.ServiceSubject = "Interview - " + user.Name;
            }
            else { service.ServiceSubject = "Interview - "; }
            service.OwnerUserId = userId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Create;

            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            service.Json = JsonConvert.SerializeObject(assResultModel);

            var res = await _serviceBusiness.ManageService(service);

            if (res.IsSuccess)
            {
                //var servicedet = await _talentAssessmentBusiness.Getinterviewid(res.Item.ServiceId);
                //var Ques = await _talentAssessmentBusiness.GetRandamQuestions();



                //if (Ques.IsNotNull())
                //{
                //    foreach (var item in Ques)
                //    {

                //        var model = new InterViewQuestionViewModel()
                //        {

                //            Question = item.Question,
                //            UserId = userId,
                //            ServiceId = servicedet.Id,
                //            SINO = row.ToString(),
                //        };
                //        row++;
                //        var noteTempModel = new NoteTemplateViewModel();
                //        noteTempModel.DataAction = DataActionEnum.Create;
                //        noteTempModel.ActiveUserId = _userContext.UserId;
                //        noteTempModel.TemplateCode = "TAS_INTERVIEW_QUESTION";
                //        noteTempModel.NoteSubject = item.NoteSubject;
                //        noteTempModel.NoteDescription = item.NoteDescription;
                //        //noteTempModel.pa = model.ParentNoteId;
                //        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                //        notemodel.Json = JsonConvert.SerializeObject(model);
                //        notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                //        notemodel.NoteSubject = item.NoteSubject;
                //        notemodel.NoteDescription = item.NoteDescription;



                //        var result = await _noteBusiness.ManageNote(notemodel);
                //    }
                //}
            }

            return Json(new { success = true });

        }



        public ActionResult AssessmentInterview(string serviceId, string UserId)
        {
            var assessment = new AssessmentInterviewViewModel { ServiceId = serviceId, OwnerUserId = UserId };

            return View(assessment);
        }


        [HttpPost]
        public async Task<ActionResult> Load(string serviceId, string UserID)
        {

            //InterViewQuestionViewModel model = new InterViewQuestionViewModel();

            //Stream stream = await _talentAssessmentBusiness.GetInterViewQuestionExcel(UserID, serviceId);
            //var workbook = Workbook.Load(stream, ".xlsx");
            //JObject jObject = JObject.Parse(workbook.ToJson());
            //return Content(jObject.ToString(), Telerik.Web.Spreadsheet.MimeTypes.JSON);

            var service = await _talentAssessmentBusiness.GetInterviewService(UserID);
            var assess1 = "";
            var assess2 = "";
            if (service != null)
            {
                var assessor = await _talentAssessmentBusiness.GetInterviewAssessorList(UserID);

                var m = 1;
                foreach (var item in assessor)
                {
                    if (m == 1)
                    {
                        assess1 = item.Name;
                    }
                    else
                    {
                        assess2 = item.Name;
                    }
                    if (m == 2) { break; }
                    m++;

                }
            }


            var tempcode = "TAS_INTERVIEW";
            var temp = await _templateBusiness.GetSingle(x => x.Code == tempcode);
            if (temp != null && temp.OtherAttachmentId.IsNotNullAndNotEmpty())
            {
                var doc = await _fileBusiness.GetSingleById(temp.OtherAttachmentId);
                var byteArray = await _fileBusiness.DownloadMongoFileByte(doc.MongoFileId);
                Stream stream = new MemoryStream(byteArray);
                var workbook = Workbook.Load(stream, doc.FileExtension);

                JObject jObject = JObject.Parse(workbook.ToJson());
                string sheet1 = (string)jObject.SelectToken("activeSheet");

                JArray sheets = (JArray)jObject.SelectToken("sheets");

                foreach (JObject sheet in sheets)
                {

                    JArray rows = (JArray)sheet.SelectToken("rows");
                    if (service != null)
                    {
                        foreach (JObject row in rows)
                        {
                            var index = (int)row.SelectToken("index");

                            if (index == 3)
                            {
                                JObject cell0 = (JObject)row.SelectToken("cells[1]");
                                cell0.Add(new JProperty("value", service.OwnerUserName));


                                JObject cell2 = (JObject)row.SelectToken("cells[2]");
                                cell2.Add(new JProperty("value", assess1));

                                JObject cell3 = (JObject)row.SelectToken("cells[3]");
                                cell3.Add(new JProperty("value", service.JobTitle));

                                JObject cell4 = (JObject)row.SelectToken("cells[5]");
                                cell4.Add(new JProperty("value", service.ScheduledStartDate.Value.Date.ToString("dd-MM-yyyy")));

                                JObject cell5 = (JObject)row.SelectToken("cells[6]");
                                cell5.Add(new JProperty("value", service.ScheduledStartDate.Value.Date.ToString("dd-MM-yyyy")));

                                JObject cell8 = (JObject)row.SelectToken("cells[8]");
                                cell8.Add(new JProperty("value", service.ActualStartDate.Value.Date.ToString("dd-MM-yyyy")));

                                JObject cell9 = (JObject)row.SelectToken("cells[9]");
                                cell9.Add(new JProperty("value", service.ActualStartDate.Value.Date.ToString("dd-MM-yyyy")));

                            }
                            if (index == 5)
                            {
                                JObject cell0 = (JObject)row.SelectToken("cells[1]");
                                cell0.Add(new JProperty("value", ""));

                                JObject cell2 = (JObject)row.SelectToken("cells[2]");
                                cell2.Add(new JProperty("value", assess2));

                                JObject cell3 = (JObject)row.SelectToken("cells[3]");
                                cell3.Add(new JProperty("value", service.AssessmentInterviewUrl));

                                if (service.ScheduledStartDate.IsNotNull())
                                {
                                    JObject cell4 = (JObject)row.SelectToken("cells[5]");
                                    cell4.Add(new JProperty("value", service.ScheduledStartDate.Value.ToString("HH:mm")));
                                }
                                if (service.ScheduledStartDate.IsNotNull())
                                {
                                    JObject cell5 = (JObject)row.SelectToken("cells[6]");
                                    //cell5.Add(new JProperty("value", service.ScheduledStartDate.Value.AddMinutes(60).ToString("HH:mm")));
                                    cell5.Add(new JProperty("value", service.ScheduledEndDate.Value.ToString("HH:mm")));
                                }
                                if (service.ActualStartDate.IsNotNull())
                                {
                                    JObject cell8 = (JObject)row.SelectToken("cells[8]");
                                    cell8.Add(new JProperty("value", service.ActualStartDate.Value.ToString("HH:mm")));
                                }

                                if (service.ActualEndDate.IsNotNull())
                                {
                                    JObject cell9 = (JObject)row.SelectToken("cells[9]");
                                    //cell9.Add(new JProperty("value", service.ActualStartDate.Value.AddMinutes(60).ToString("HH:mm")));
                                    cell9.Add(new JProperty("value", service.ActualEndDate.Value.ToString("HH:mm")));
                                }
                            }

                        }
                    }

                    var note = await _talentAssessmentBusiness.GetQuestionListofUser(UserID, serviceId);
                    var i = 8;
                    foreach (var item in note)
                    {

                        if (i == 11)
                        {
                            i++;
                        }
                        JObject row = new JObject();
                        row.Add(new JProperty("index", i));


                        JObject cell0 = new JObject();

                        cell0.Add(new JProperty("index", 0));
                        cell0.Add(new JProperty("value", item.NoteId));

                        JObject cell1 = new JObject();

                        cell1.Add(new JProperty("index", 1));
                        cell1.Add(new JProperty("value", item.SequenceOrder));

                        JObject cell2 = new JObject();
                        cell2.Add(new JProperty("index", 2));
                        cell2.Add(new JProperty("value", item.NoteSubject));

                        JObject cell3 = new JObject();
                        cell3.Add(new JProperty("index", 3));
                        cell3.Add(new JProperty("value", item.NoteDescription));

                        JObject cell4 = new JObject();
                        cell4.Add(new JProperty("index", 4));
                        cell4.Add(new JProperty("value", item.ProficiencyLevel));
                        JObject cell5 = new JObject();
                        cell5.Add(new JProperty("index", 5));
                        cell5.Add(new JProperty("value", item.Indicator));
                        JObject cell6 = new JObject();
                        cell6.Add(new JProperty("index", 6));
                        cell6.Add(new JProperty("value", item.Remark));
                        JObject cell7 = new JObject();
                        cell7.Add(new JProperty("index", 7));
                        cell7.Add(new JProperty("value", item.Question));
                        JObject cell8 = new JObject();
                        cell8.Add(new JProperty("index", 8));
                        cell8.Add(new JProperty("value", item.CandidateAnswer));
                        JObject cell9 = new JObject();
                        cell9.Add(new JProperty("index", 9));
                        cell9.Add(new JProperty("value", item.Score));
                        JObject cell10 = new JObject();
                        cell10.Add(new JProperty("index", 10));
                        cell10.Add(new JProperty("value", item.InterviewerComment));

                        JArray cells = new JArray();

                        cells.Add(cell0);
                        cells.Add(cell1);
                        cells.Add(cell2);
                        cells.Add(cell3);
                        cells.Add(cell4);
                        cells.Add(cell5);
                        cells.Add(cell6);
                        cells.Add(cell7);
                        cells.Add(cell8);
                        cells.Add(cell9);
                        cells.Add(cell10);

                        row.Add(new JProperty("cells", cells));

                        rows.Add(row);

                        i++;
                    }

                }


                return Content(jObject.ToString(), Telerik.Web.Spreadsheet.MimeTypes.JSON);
            }

            return Json(new { data = "" });


        }


        public async Task<ActionResult> SaveInterviewAssessment(AssessmentInterviewViewModel model)
        {
            var errorList = new List<string>();
            string jsonString = model.Data;
            JObject jObject = JObject.Parse(jsonString);
            string sheet1 = (string)jObject.SelectToken("activeSheet");
            //long serviceId = 0;
            JArray sheets = (JArray)jObject.SelectToken("sheets");
            //var note = _business.GetServiceReferenceNoteInterviewData(model.ServiceId.Value);
            foreach (JToken sheet in sheets)
            {
                string sheetname = (string)sheet.SelectToken("name");

                var list = new List<long>();

                JArray rows = (JArray)sheet.SelectToken("rows");
                var serial = 1;
                foreach (JToken row in rows)
                {
                    var index = (int)row.SelectToken("index");

                    if (index > 7 && index != 11 && index < 30)
                    {


                        var noteid = "";
                        long seqNo = 0;
                        var title = "";
                        var description = "";
                        var level = "";
                        var section = "";
                        var remark = "";
                        var question = "";
                        var answer = "";
                        var score = "";
                        var comment = "";

                        JArray cells = (JArray)row.SelectToken("cells");
                        var k = 0;
                        foreach (JToken cell in cells)
                        {
                            var z = (int)cell.SelectToken("index");
                            if (z == 0)
                                noteid = (string)cell.SelectToken("value");
                            else if (z == 1)
                            {
                                var seq = (long?)cell.SelectToken("value");
                                if (seq.IsNotNull())
                                    seqNo = seq.Value;
                            }
                            else if (z == 2)
                                title = (string)cell.SelectToken("value");
                            else if (z == 3)
                                description = (string)cell.SelectToken("value");
                            else if (z == 4)
                                level = (string)cell.SelectToken("value");
                            else if (z == 5)
                                section = (string)cell.SelectToken("value");
                            else if (z == 6)
                                remark = (string)cell.SelectToken("value");
                            else if (z == 7)
                                question = (string)cell.SelectToken("value");
                            else if (z == 8)
                                answer = (string)cell.SelectToken("value");
                            else if (z == 9)
                                score = (string)cell.SelectToken("value");
                            else if (z == 10)
                                comment = (string)cell.SelectToken("value");


                            k++;
                        }

                        if (question.IsNotNullAndNotEmpty())
                        {
                            var model1 = new InterViewQuestionViewModel()
                            {

                                Question = question,
                                UserId = model.OwnerUserId,
                                // ServiceId = model.ServiceId,
                                SequenceOrder = seqNo,
                                ProficiencyLevel = level,
                                Indicator = section,
                                Remark = remark,

                                CandidateAnswer = answer,
                                Score = score,
                                InterviewerComment = comment,


                                //ProficiencyLevel = "Leadership",
                                //Indicatior = "Innovative & Disruptive",
                                //Remark = "Catalyst for change at the individual and institutional level; entrepreneurial Risk Taker and adventurous for whom nothing is impossible",

                            };
                            var noteTempModel = new NoteTemplateViewModel();
                            if (noteid.IsNotNull())
                            {

                                noteTempModel.DataAction = DataActionEnum.Edit;
                                noteTempModel.NoteId = noteid;

                            }
                            else
                            {
                                noteTempModel.DataAction = DataActionEnum.Create;
                                noteTempModel.ActiveUserId = _userContext.UserId;
                                noteTempModel.TemplateCode = "TAS_INTERVIEW_QUESTION";
                            }


                            //noteTempModel.pa = model.ParentNoteId;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                            notemodel.Json = JsonConvert.SerializeObject(model1);
                            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            notemodel.NoteSubject = title;
                            notemodel.NoteDescription = description;
                            notemodel.ParentServiceId = model.ServiceId;
                            notemodel.SequenceOrder = seqNo;

                            var result = await _noteBusiness.ManageNote(notemodel);
                        }


                        serial++;
                    }


                }


            }

            return Json(new { success = true });

        }
        //public async Task<ActionResult> SaveInreviewerCaseStudyScore(AssessmentInterviewViewModel model)
        //{
        //    var errorList = new List<string>();
        //    List<AssessmentAnswerViewModel> ansList = new List<AssessmentAnswerViewModel>();
        //    string jsonString = model.Data;
        //    JObject jObject = JObject.Parse(jsonString);
        //    string sheet1 = (string)jObject.SelectToken("activeSheet");
        //    //long serviceId = 0;
        //    JArray sheets = (JArray)jObject.SelectToken("sheets");

        //    string answerId = null;
        //    var optcount = 0;
        //    //var note = _business.GetServiceReferenceNoteInterviewData(model.ServiceId.Value);
        //    foreach (JToken sheet in sheets)
        //    {
        //        string sheetname = (string)sheet.SelectToken("name");

        //        var list = new List<long>();

        //        JArray rows = (JArray)sheet.SelectToken("rows");

        //        foreach (JToken row in rows)
        //        {
        //            var index = (int)row.SelectToken("index");
        //            if (index >= 9)
        //            {
        //                JArray cells = (JArray)row.SelectToken("cells");
        //                foreach (JToken cell in cells)
        //                {
        //                    var z = (int)cell.SelectToken("index");
        //                    if (z == 0)
        //                    {
        //                        answerId = (string)cell.SelectToken("value");

        //                        var res = await _talentAssessmentBusiness.GetQuestionByAnswerId(answerId);
        //                        var opts = await _talentAssessmentBusiness.GetOptionsByQuestionId(res.QuestionId);
        //                        var count = opts.Count;
        //                        optcount = count > optcount ? count : optcount;
        //                    }
        //                }
        //            }

        //        }

        //        foreach (JToken row in rows)
        //        {
        //            var index = (int)row.SelectToken("index");

        //            if (index >= 9)
        //            {
        //                var slNo = "0";
        //                var ques = "";
        //                var opt1 = "";
        //                var opt2 = "";
        //                var opt3 = "";
        //                var opt4 = "";
        //                var opt5 = "";
        //                var opt6 = "";
        //                var opt7 = "";
        //                var opt8 = "";
        //                var opt9 = "";
        //                var opt10 = "";
        //                var rightans = "";
        //                var candans = "";
        //                var accurate = "";
        //                var anscomment = "";
        //                double? score = null;
        //                var interviewerComment = "";

        //                JArray cells = (JArray)row.SelectToken("cells");

        //                var k = 0;
        //                foreach (JToken cell in cells)
        //                {
        //                    var z = (int)cell.SelectToken("index");
        //                    if (z == 0)
        //                        answerId = (string)cell.SelectToken("value");
        //                    //else if (z == 1)
        //                    //    slNo = (string)cell.SelectToken("value");
        //                    //else if (z == 2)
        //                    //    ques = (string)cell.SelectToken("value");
        //                    //else if (z == 3)
        //                    //    opt1 = (string)cell.SelectToken("value");
        //                    //else if (z == 4)
        //                    //    opt2 = (string)cell.SelectToken("value");
        //                    //else if (z == 5)
        //                    //    opt3 = (string)cell.SelectToken("value");
        //                    //else if (z == 6)
        //                    //    opt4 = (string)cell.SelectToken("value");
        //                    //else if (z == 7)
        //                    //    opt5 = (string)cell.SelectToken("value");
        //                    //else if (z == 8)
        //                    //    opt6 = (string)cell.SelectToken("value");
        //                    //else if (z == 9)
        //                    //    opt7 = (string)cell.SelectToken("value");
        //                    //else if (z == 10)
        //                    //    opt8 = (string)cell.SelectToken("value");
        //                    //else if (z == 11)
        //                    //    opt9 = (string)cell.SelectToken("value");
        //                    //else if (z == 12)
        //                    //    opt10 = (string)cell.SelectToken("value");
        //                    //else if (z == 13)
        //                    //    rightans = (string)cell.SelectToken("value");
        //                    //else if (z == 14)
        //                    //    candans = (string)cell.SelectToken("value");
        //                    //else if (z == 15)
        //                    //    accurate = (string)cell.SelectToken("value");
        //                    //else if (z == 16)
        //                    //    anscomment = (string)cell.SelectToken("value");
        //                    //else if (z == 17)
        //                    //{
        //                    //    if (cell.SelectToken("value").IsNotNull())
        //                    //        score = Convert.ToDouble(cell.SelectToken("value"));
        //                    //}
        //                    //else if (z == 18)
        //                    //    interviewerComment = (string)cell.SelectToken("value");
        //                    //else if (z == 19)
        //                    //    answerId = (string)(cell.SelectToken("value"));

        //                    if (z == optcount + 7)
        //                    {
        //                        if (cell.SelectToken("value").IsNotNull())
        //                            score = Convert.ToDouble(cell.SelectToken("value"));
        //                    }
        //                    else if (z == optcount + 8)
        //                    {
        //                        interviewerComment = (string)cell.SelectToken("value");
        //                    }

        //                    k++;
        //                }
        //                if (score.IsNotNull())
        //                {
        //                    var ans = new AssessmentAnswerViewModel()
        //                    {
        //                        Id = answerId,
        //                        Score = score,
        //                        CaseStudyComment = interviewerComment
        //                    };
        //                    ansList.Add(ans);
        //                }
        //            }
        //        }

        //        var result = await _talentAssessmentBusiness.UpdateScoreComment(ansList);
        //        if (result)
        //        {
        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            return Json(new { success = false });
        //        }

        //    }

        //    return Json(new { success = true });
        //}
        public async Task<ActionResult> SaveInreviewerCaseStudyScore(AssessmentInterviewViewModel model)
        {
            var errorList = new List<string>();
            List<AssessmentAnswerViewModel> ansList = new List<AssessmentAnswerViewModel>();
            string jsonString = model.Data;
            JToken token = JObject.Parse(jsonString);

            string answerId = null;
            var optcount = 0;
            // JToken records = token.SelectToken("Sheets");

            foreach (JToken childitem in token.Children())
            {

                foreach (JToken grandChild in childitem.Children())
                {
                    foreach (JToken grandChild1 in grandChild.Children())
                    {
                        var property2 = grandChild1 as JProperty;
                        if (property2 != null)
                        {
                            Console.WriteLine(property2.Name + ":" + property2.Value);
                        }
                        if (property2 != null && property2.Name == "rows")
                        {
                            foreach (JToken grandGrandChild in grandChild1.Children())
                            {

                                foreach (JToken leastChild in grandGrandChild.Children())
                                {
                                    var rowproperty = leastChild as JProperty;
                                    if (rowproperty != null && rowproperty.Name != "len")
                                    {
                                        var str = rowproperty.Name;
                                        var index = str.ToSafeInt();
                                        if (index >= 9)
                                        {
                                            var ans = new AssessmentAnswerViewModel();
                                            //                {
                                            //                    Id = answerId,
                                            //                    Score = score,
                                            //                    CaseStudyComment = interviewerComment
                                            //                };
                                            foreach (JToken Child in leastChild.Children())
                                            {
                                                foreach (JToken Child1 in Child.Children())
                                                {
                                                    foreach (JToken Child2 in Child1.Children())
                                                    {
                                                        foreach (JToken Child3 in Child2.Children())
                                                        {
                                                            var cellroperty = Child3 as JProperty;
                                                            if (cellroperty != null)
                                                            {
                                                                var cellstr = cellroperty.Name;
                                                                var cellindex = cellstr.ToSafeInt();
                                                                if (cellindex == 0)
                                                                {

                                                                    foreach (JToken Child4 in Child3.Children())
                                                                    {
                                                                        foreach (JToken Child5 in Child4.Children())
                                                                        {
                                                                            var property = Child5 as JProperty;

                                                                            if (property != null)
                                                                            {
                                                                                ans.Id = (string)property.Value;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (cellindex == 11)
                                                                {

                                                                    foreach (JToken Child4 in Child3.Children())
                                                                    {
                                                                        foreach (JToken Child5 in Child4.Children())
                                                                        {
                                                                            var property = Child5 as JProperty;

                                                                            if (property != null && property.Value != null)
                                                                            {
                                                                                ans.Score = (double)property.Value;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (cellindex == 12)
                                                                {

                                                                    foreach (JToken Child4 in Child3.Children())
                                                                    {
                                                                        foreach (JToken Child5 in Child4.Children())
                                                                        {
                                                                            var property = Child5 as JProperty;

                                                                            if (property != null && property.Value != null)
                                                                            {
                                                                                ans.CaseStudyComment = (string)property.Value;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (ans.Score.IsNotNull())
                                            {
                                                ansList.Add(ans);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }


            }
            //var note = _business.GetServiceReferenceNoteInterviewData(model.ServiceId.Value);
            //    foreach (JToken sheet in sheets)
            //{
            //    string sheetname = (string)sheet.SelectToken("name");

            //    var list = new List<long>();

            //    JArray rows = (JArray)sheet.SelectToken("rows");

            //    foreach (JToken row in rows)
            //    {
            //        var index = (int)row.SelectToken("index");
            //        if (index >= 9)
            //        {
            //            JArray cells = (JArray)row.SelectToken("cells");
            //            foreach (JToken cell in cells)
            //            {
            //                var z = (int)cell.SelectToken("index");
            //                if (z == 0)
            //                {
            //                    answerId = (string)cell.SelectToken("value");

            //                    var res = await _talentAssessmentBusiness.GetQuestionByAnswerId(answerId);
            //                    var opts = await _talentAssessmentBusiness.GetOptionsByQuestionId(res.QuestionId);
            //                    var count = opts.Count;
            //                    optcount = count > optcount ? count : optcount;
            //                }
            //            }
            //        }

            //    }

            //    foreach (JToken row in rows)
            //    {
            //        var index = (int)row.SelectToken("index");

            //        if (index >= 9)
            //        {                        
            //            var slNo = "0";
            //            var ques = "";
            //            var opt1 = "";
            //            var opt2 = "";
            //            var opt3 = "";
            //            var opt4 = "";
            //            var opt5 = "";
            //            var opt6 = "";
            //            var opt7 = "";
            //            var opt8 = "";
            //            var opt9 = "";
            //            var opt10 = "";
            //            var rightans = "";
            //            var candans = "";
            //            var accurate = "";
            //            var anscomment = "";
            //            double? score = null;
            //            var interviewerComment = "";

            //            JArray cells = (JArray)row.SelectToken("cells");

            //            var k = 0;
            //            foreach (JToken cell in cells)
            //            {
            //                var z = (int)cell.SelectToken("index");
            //                if (z == 0)
            //                    answerId = (string)cell.SelectToken("value");
            //                //else if (z == 1)
            //                //    slNo = (string)cell.SelectToken("value");
            //                //else if (z == 2)
            //                //    ques = (string)cell.SelectToken("value");
            //                //else if (z == 3)
            //                //    opt1 = (string)cell.SelectToken("value");
            //                //else if (z == 4)
            //                //    opt2 = (string)cell.SelectToken("value");
            //                //else if (z == 5)
            //                //    opt3 = (string)cell.SelectToken("value");
            //                //else if (z == 6)
            //                //    opt4 = (string)cell.SelectToken("value");
            //                //else if (z == 7)
            //                //    opt5 = (string)cell.SelectToken("value");
            //                //else if (z == 8)
            //                //    opt6 = (string)cell.SelectToken("value");
            //                //else if (z == 9)
            //                //    opt7 = (string)cell.SelectToken("value");
            //                //else if (z == 10)
            //                //    opt8 = (string)cell.SelectToken("value");
            //                //else if (z == 11)
            //                //    opt9 = (string)cell.SelectToken("value");
            //                //else if (z == 12)
            //                //    opt10 = (string)cell.SelectToken("value");
            //                //else if (z == 13)
            //                //    rightans = (string)cell.SelectToken("value");
            //                //else if (z == 14)
            //                //    candans = (string)cell.SelectToken("value");
            //                //else if (z == 15)
            //                //    accurate = (string)cell.SelectToken("value");
            //                //else if (z == 16)
            //                //    anscomment = (string)cell.SelectToken("value");
            //                //else if (z == 17)
            //                //{
            //                //    if (cell.SelectToken("value").IsNotNull())
            //                //        score = Convert.ToDouble(cell.SelectToken("value"));
            //                //}
            //                //else if (z == 18)
            //                //    interviewerComment = (string)cell.SelectToken("value");
            //                //else if (z == 19)
            //                //    answerId = (string)(cell.SelectToken("value"));

            //                if (z == optcount + 7)
            //                {
            //                    if (cell.SelectToken("value").IsNotNull())
            //                        score = Convert.ToDouble(cell.SelectToken("value"));
            //                }else if(z== optcount + 8)
            //                {
            //                    interviewerComment = (string)cell.SelectToken("value");
            //                }                          

            //                k++;
            //            }
            //            if (score.IsNotNull())
            //            {
            //                var ans = new AssessmentAnswerViewModel()
            //                {
            //                    Id = answerId,
            //                    Score = score,
            //                    CaseStudyComment = interviewerComment
            //                };
            //                ansList.Add(ans);
            //            }
            //        }                    
            //    }

            var result = await _talentAssessmentBusiness.UpdateScoreComment(ansList);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

            //return Json(new { success = true });
        }


        public ActionResult CandidateCaseStudy(string userId)
        {
            var assessment = new InterViewQuestionViewModel { UserId = userId };

            return View(assessment);
        }



        public async Task<string> LoadCandidateCaseStudy(string userId)
        {
            var model = await _talentAssessmentBusiness.GetCandidateCaseStudyReport(userId);


            var userdata = await _userBusiness.GetSingle(x => x.Id == userId);
            if (userdata.IsNotNull())
            {
                model.OwnerUserName = userdata.Name;
                model.JobTitle = userdata.JobTitle;
                model.Email = userdata.Email;
            }

            MemoryStream stream = await _talentAssessmentBusiness.GetCandidateCaseStudyDataExcel(model);
            var bytearr = stream.ToArray();
            var ContentBase64 = Convert.ToBase64String(bytearr);
            return ContentBase64;
            //var workbook = Workbook.Load(stream, ".xlsx");
            //JObject jObject = JObject.Parse(workbook.ToJson());
            //return Content(jObject.ToString(), Telerik.Web.Spreadsheet.MimeTypes.JSON);
        }


        public ActionResult InterviewerCaseStudy(string userId)
        {
            var assessment = new InterViewQuestionViewModel { UserId = userId };

            return View(assessment);
        }

        //[HttpPost]
        public async Task<string> LoadInterviewerCaseStudy(string userId)
        {

            var model = await _talentAssessmentBusiness.GetCandidateCaseStudyReport(userId);
            if (model != null && model.CaseStudyAssessment != null)
            {
                foreach (var item in model.CaseStudyAssessment.AnswerList)
                {
                    item.Question = item.Question.Replace("<br/>", "").Replace("<br>", "").Replace("</br>", "");
                    item.QuestionAr = item.QuestionAr.Replace("<br/>", "").Replace("<br>", "").Replace("</br>", "");
                }

            }

            var userdata = await _userBusiness.GetSingle(x => x.Id == userId);
            if (userdata.IsNotNull())
            {
                model.OwnerUserName = userdata.Name;
                model.JobTitle = userdata.JobTitle;
                model.Email = userdata.Email;
            }
            MemoryStream stream = await _talentAssessmentBusiness.GetInterviewerCaseStudyDataExcel(model);
            var bytearr = stream.ToArray();
            var ContentBase64 = Convert.ToBase64String(bytearr);
            return ContentBase64;
            //var workbook = Workbook.Load(stream, ".xlsx");
            //JObject jObject = JObject.Parse(workbook.ToJson());
            //return Content(jObject.ToString(), Telerik.Web.Spreadsheet.MimeTypes.JSON);
        }

        public async Task<ActionResult> AssessmentReportDataExcel(string userId)
        {
            var model = await _talentAssessmentBusiness.GetAssessmentReportexcel(userId);

            var userdata = await _userBusiness.GetSingle(x => x.Id == userId);
            if (userdata.IsNotNull())
            {
                model.OwnerUserName = userdata.Name;
                model.JobTitle = userdata.JobTitle;
                model.Email = userdata.Email;
            }
            var report = string.Concat("AssessmentReport_", model.Email, ".xlsx");
            var ms = await _talentAssessmentBusiness.GetAssessmentReportDataExcel(model);
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report);
        }

        public async Task<ActionResult> BusinessUserSurveyReportDataExcel()
        {
            var model = await _talentAssessmentBusiness.GetBusinessUserSurveyReportDataExcel();

            var report = string.Concat("SurveyData_BusinessUser.xlsx");
            return File(model, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report);
        }

        public async Task<ActionResult> HRUserSurveyReportDataExcel()
        {
            var model = await _talentAssessmentBusiness.GetHRUserSurveyReportDataExcel();

            var report = string.Concat("SurveyData_HRUser.xlsx");
            return File(model, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report);
        }


        public ActionResult UploadCandidateDocument(string serviceId, string userId, string type)
        {
            ViewBag.serviceId = serviceId;
            ViewBag.userId = userId;
            ViewBag.type = type;
            ViewBag.Title = type;
            var model = new AssessmentInterviewViewModel { Id = userId };
            return View(model);
        }

        public async Task<IActionResult> ChangeUserProfilePhoto(string photoId, string userId)
        {

            await _talentAssessmentBusiness.ChangeUserProfilePhoto(photoId, userId);
            return Json(new { success = true });

        }

        public async Task<IActionResult> SubmitCandidateId(string UserId, string fileId)
        {
            var exist = await _cmsBusiness.GetDataListByTemplate("TAS_CANDIDATE_ID", "", $@" and ""N_TAS_CandidateId"".""UserId"" = '{UserId}'");
            if (exist.Rows.Count == 0)
            {
                var model1 = new CandidateIdViewModel
                {
                    UserId = UserId,
                    CandidateId = fileId,
                };

                var data = await _talentAssessmentBusiness.GetCanidateId(UserId);

                //string noteid =data.Id;
                var noteTempModel = new NoteTemplateViewModel();
                if (data.IsNotNull() && data.Id.IsNotNull())
                {
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.NoteId = data.Id;
                }
                else { noteTempModel.DataAction = DataActionEnum.Create; }

                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "TAS_CANDIDATE_ID";
                //noteTempModel.pa = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

                var result = await _noteBusiness.ManageNote(notemodel);
                return Json(new { success = true });
            }
            else
            {
                await _talentAssessmentBusiness.UpdateCandidateId(UserId, fileId, "Id");
                return Json(new { success = true });
            }

        }

        public async Task<IActionResult> UploadInterviewSheet(string UserId, string fileId)
        {
            var exist = await _cmsBusiness.GetDataListByTemplate("TAS_CANDIDATE_ID", "", $@" and ""N_TAS_CandidateId"".""UserId"" = '{UserId}'");
            if (exist.Rows.Count == 0)
            {
                var model1 = new CandidateIdViewModel
                {
                    UserId = UserId,
                    InterviewSheetId = fileId,
                };

                var data = await _talentAssessmentBusiness.GetCanidateId(UserId);

                //string noteid =data.Id;
                var noteTempModel = new NoteTemplateViewModel();
                if (data.IsNotNull() && data.Id.IsNotNull())
                {
                    noteTempModel.DataAction = DataActionEnum.Edit;
                    noteTempModel.NoteId = data.Id;
                }
                else { noteTempModel.DataAction = DataActionEnum.Create; }

                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "TAS_CANDIDATE_ID";
                //noteTempModel.pa = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

                var result = await _noteBusiness.ManageNote(notemodel);
                return Json(new { success = true });
            }
            else
            {
                await _talentAssessmentBusiness.UpdateCandidateId(UserId, fileId, "InterviewSheet");
                return Json(new { success = true });
            }

        }

        [HttpPost]
        public async Task<IActionResult> CompleteInterview(string serviceId, string userId, string Tableid)
        {
            var assResultModel = new AssessmentInterviewViewModel()
            {
                ActualStartDate = DateTime.Now,
                ActualEndDate = DateTime.Now,
            };

            //var GetScore = await _talentAssessmentBusiness.GetserviceScore(serviceId);

            Int32 srno = 0;
            var questionList = await _talentAssessmentBusiness.GetInterviewAssessmentQuestions(userId, serviceId);
            double firstThreeQuestionScore = 0.0;
            double remainingQuestionScore = 0.0;
            if (questionList != null && questionList.Count > 0)
            {
                foreach (var item in questionList)
                {
                    srno++;

                    if (srno == 1 || srno == 2 || srno == 3)
                    {
                        if (item.Score != null)
                        {
                            firstThreeQuestionScore = firstThreeQuestionScore + Convert.ToDouble(item.Score);
                        }
                    }
                    else
                    {
                        if (item.Score != null)
                        {
                            remainingQuestionScore = remainingQuestionScore + Convert.ToDouble(item.Score);
                        }
                    }
                }
                firstThreeQuestionScore = (firstThreeQuestionScore / 12) * 0.2;
                if (questionList.Count() > 3)
                {
                    remainingQuestionScore = (remainingQuestionScore / ((questionList.Count() - 3) * 4)) * 0.8;
                }

                var s = ((firstThreeQuestionScore + remainingQuestionScore) * 100).RoundPayrollSummaryAmount().ToString();
                assResultModel.Score = s;
            }
            else
            {
                assResultModel.Score = "0.0";
            }

            await _talentAssessmentBusiness.UpdateInterViewScore(Tableid, assResultModel.Score, assResultModel.ActualStartDate.ToString(), assResultModel.ActualEndDate.ToString());

            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_INTERVIEW";
            serviceModel.ServiceId = serviceId;
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);
            service.LastUpdatedBy = _userContext.UserId;
            service.DataAction = DataActionEnum.Edit;
            service.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";

            service.Json = JsonConvert.SerializeObject(assResultModel);

            var res = await _serviceBusiness.ManageService(service);

            await _talentAssessmentBusiness.CalculateQuestionaireResultScore(userId);
            await _talentAssessmentBusiness.CalculateCaseStudyResultScore(userId);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateArchived(string assessments, bool archive = false)
        {
            if (assessments.IsNotNullAndNotEmpty())
            {
                var serviceIds = assessments.TrimEnd(',');

                var result = await _talentAssessmentBusiness.UpdateAssessmentArchived(serviceIds, archive);
                if (result)
                {
                    return Json(new { success = true, });
                }
                else
                {
                    return Json(new { success = false, errors = "Invalid operation" });
                }
            }

            return Json(new { success = false, errors = "Invalid operation" });
        }

        public ActionResult AssessmentSet()
        {
            var model = new AssessmentViewModel { ActiveUserId = _userContext.UserId };
            return View(model);
        }


        public async Task<IActionResult> ReadAssessmentSetData(/*[DataSourceRequest] DataSourceRequest request*/)
        {
            var result = await _talentAssessmentBusiness.GetAssessmentSetList();

            return Json(result/*.ToDataSourceResult(request)*/);

        }

        public async Task<ActionResult> DeleteAssessmentSet(string Id)
        {
            var result = await _talentAssessmentBusiness.DeleteAssessmentSet(Id);
            return Json(new { success = result });
        }

        public ActionResult AssessmentSetAssessment(string assessmentSetId)
        {
            var model = new AssessmentSetAssessmentViewModel { AssessmentSetId = assessmentSetId };
            return View(model);
        }


        public async Task<IActionResult> ReadAssessmentSetAssessmentData(/*[DataSourceRequest] DataSourceRequest request,*/string AssessmentSetId)
        {
            var result = await _talentAssessmentBusiness.GetAssessmentSetAssessmentList(AssessmentSetId);

            return Json(result/*.ToDataSourceResult(request)*/);

        }

        public async Task<IActionResult> MapAssessmentSetAssessment(string AssessmentSetId, string Id)
        {
            AssessmentSetAssessmentViewModel Model = new AssessmentSetAssessmentViewModel();
            if (Id.IsNotNull())
            {
                var data = await _talentAssessmentBusiness.GetAssessmentSetAssessmentByid(Id);
                Model.AssessmentSetId = AssessmentSetId;
                Model.DataAction = DataActionEnum.Edit;
                Model.SequenceOrder = data.SequenceOrder;
                Model.AssessmentType = data.AssessmentType;
                Model.AssessmentId = data.AssessmentName;
                Model.NoteId = Id;
                Model.Id = data.Id;
                Model.Ids = data.Id;
            }
            else
            {
                Model.AssessmentSetId = AssessmentSetId;
                Model.DataAction = DataActionEnum.Create;
            }

            return View(Model);
        }

        public async Task<IActionResult> GetAssessmentListToSet(string AssessmentType)
        {
            var data = await _talentAssessmentBusiness.GetAssessmentListToSet(AssessmentType);
            return Json(data);
        }

        public async Task<ActionResult> SaveAssessmentSetAssessment(AssessmentSetAssessmentViewModel Model)
        {
            var noteTempModel = new NoteTemplateViewModel();
            if (Model.DataAction == DataActionEnum.Create)
            {
                var data = await _talentAssessmentBusiness.GetSquenceNoExistAssessmentSet(Model.AssessmentSetId, Convert.ToString(Model.SequenceOrder));
                if (data.IsNotNull())
                {
                    return Json(new { success = false, error = "Sequence No. Already Exist" });
                }
                var dataname = await _talentAssessmentBusiness.GetAssessmentExistAssessmentSet(Model.AssessmentSetId, Model.AssessmentId, Model.Id);
                if (dataname.IsNotNull())
                {
                    return Json(new { success = false, error = "Assessment Already Exist" });
                }
                noteTempModel.DataAction = DataActionEnum.Create;
            }
            else if (Model.DataAction == DataActionEnum.Edit)
            {
                var dataname = await _talentAssessmentBusiness.GetAssessmentExistAssessmentSet(Model.AssessmentSetId, Model.AssessmentId, Model.Ids);
                if (dataname.IsNotNull())
                {
                    return Json(new { success = false, error = "Assessment Already Exist" });
                }
                noteTempModel.DataAction = DataActionEnum.Create;
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.NoteId = Model.NoteId;
            }

            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "TAS_ASSESSMENT_SET_ASSESSMENT";
            //noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
            notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            var result = await _noteBusiness.ManageNote(notemodel);

            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = result.HtmlError.ToHtmlError() });
            }
        }

        public async Task<ActionResult> DeleteAssessmentSetAssessment(string Id)
        {
            var result = await _talentAssessmentBusiness.DeleteAssessmentSetAsessment(Id);
            return Json(new { success = result });
        }

        public ActionResult AssessmentReportMinistry(string userId = null)
        {
            ViewBag.UserId = userId;
            return View();
        }

        public async Task<ActionResult> GetAssessmentReportMinistryList(/*[DataSourceRequest] DataSourceRequest request,*/ string userId = null)
        {
            var sponsor = await _talentAssessmentBusiness.GetSponsorDetailsByUserId(_userContext.UserId);
            var result = new List<AssessmentInterviewViewModel>();

            if (sponsor.Id.IsNotNull())
            {
                result = await _talentAssessmentBusiness.GetAssessmentReportMinistryList(sponsor.Id);
            }
            if (_userContext.IsSystemAdmin)
            {
                result = await _talentAssessmentBusiness.GetAssessmentReportMinistryList(null, userId);
            }

            var j = Json(result/*.ToDataSourceResult(request)*/);
            return j;
        }

        public async Task<ActionResult> CandidateProfile(string userId)
        {
            var user = await _userBusiness.GetSingleById(userId);
            ViewBag.fileId = user.PhotoId;
            ViewBag.identificationfileId = await _talentAssessmentBusiness.GetUserIdentificationByUserId(userId);
            ViewBag.UserDetails = user;
            ViewBag.AssessmentTypes = await _lovBusiness.GetList(x => x.LOVType == "ASSESSMENT_TYPE");
            ViewBag.UserData = user;
            //ViewBag.AssessmentSummary = _business.GetAssessmentReportDataPDFForUser(userId);
            ViewBag.UserId = userId;
            return View();
        }
        public async Task<IActionResult> ReadTalentAssessmentsList([DataSourceRequest] DataSourceRequest request, string Status, string typeId, string userId)
        {
            var result = await _talentAssessmentBusiness.GetAssessmentListByUserId(userId, Status, typeId);
            return Json(result);
        }
        public async Task<IActionResult> ReadInterviewList([DataSourceRequest] DataSourceRequest request, string Status, string userId)
        {
            var result = await _talentAssessmentBusiness.GetInterviewList(userId, Status);
            return Json(result);
        }

        [HttpGet]
        public virtual ActionResult Download()
        {
            //Download csv
            //    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "ExcelFormate/Question.csv");
            //   string fullPath = "~/ExcelFormate/Question.csv";
            //   byte[] bytes = System.IO.File.ReadAllBytes(path);

            //   System.IO.FileInfo exportFile = new System.IO.FileInfo(path);
            //return PhysicalFile(exportFile.FullName, "text/csv", string.Format("Export-{0}.csv", "Question"));

            //Download xlsx
            string path = Path.Combine(this._webHostEnvironment.WebRootPath, "ExcelFormate/QuestionSheet.xlsx");
            string fullPath = "~/ExcelFormate/QuestionSheet.xlsx";
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            System.IO.FileInfo exportFile = new System.IO.FileInfo(path);
            return PhysicalFile(exportFile.FullName, "text/xlsx", string.Format("Export-{0}.xlsx", "Question"));

            //Send the File to Download.
            // return File(bytes, "application/octet-stream", "test");
            //return File(path, "application/vnd.ms-excel","Question");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IList<IFormFile> file)
        {
            try
            {
                var fileexit = "";

                foreach (var file1 in file)
                {
                    var fileName = Path.GetFileName(file1.FileName);
                    var isFileExist = await _fileBusiness.GetFileByName(fileName);
                    if (isFileExist.IsNotNull())
                    {
                        if (fileexit.IsNullOrEmpty())
                        {
                            fileexit += "Image Name" + fileName;
                        }
                        else { fileexit += "," + fileName; }
                    }
                }

                if (fileexit.IsNullOrEmpty())
                {
                    CommandResult<FileViewModel> result1 = null;
                    foreach (var file2 in file)
                    {
                        var fileName = Path.GetFileName(file2.FileName);
                        var ms = new MemoryStream();
                        file2.OpenReadStream().CopyTo(ms);
                        result1 = await _fileBusiness.Create(new FileViewModel
                        {
                            ContentByte = ms.ToArray(),
                            ContentType = file2.ContentType,
                            ContentLength = file2.Length,
                            FileName = fileName,
                            FileExtension = Path.GetExtension(file2.FileName)
                        }
                        );
                    }

                    if (result1.IsSuccess)
                    {
                        return Json(new { success = true, fileId = result1.Item.Id, filename = result1.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }
                }
                else
                {
                    return Json(new { success = false, errors = "Already exist", Type = "Image" });
                    //return Json(new { success = false, errors = fileexit+" Already Exist" });
                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }


        public async Task<IActionResult> Survey(string Id, string type, string currentTopicId = null, string LanguageCode = null, string surveyScheduleUserId = null, string Status = null, LayoutModeEnum layout = LayoutModeEnum.Main)
        {

            var result = new SurveyTopicViewModel();

            result = await _talentAssessmentBusiness.GetSurveyQuestion(Id);
            //  result.PreferredLanguageCode = LanguageCode;
            // result.PreferredLanguageCode = "ENGLISH";
            if (LanguageCode != null)
            {
                result.ServiceId = Id;
                result.TopicId = currentTopicId;
                result.PreferredLanguageCode = LanguageCode;
            }
            if (Status == "Draft")
            {
                await _talentAssessmentBusiness.UpdateSurvey(Id, currentTopicId);
            }
            result.LayoutMode = layout;
            result.SurveyScheduleUserId = surveyScheduleUserId;
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSurvey(string serviceId, string currentTopicId)
        {
            var result = await _talentAssessmentBusiness.UpdateSurvey(serviceId, currentTopicId);
            return Json(new { success = true, IsAssessmentStopped = result });
        }

        public async Task<ActionResult> GetSurveyQuestions(string serviceId, string currentTopicId, string lang = null)
        {

            var result = await _talentAssessmentBusiness.GetSurveyAssessmentQuestion(serviceId, currentTopicId, lang);

            return Json(result);

        }

        [HttpPost]
        public async Task<ActionResult> SubmitSurveyAnswer(SurveyTopicViewModel model, bool isSubmit, string currentTopicId)
        {
            await _talentAssessmentBusiness.SubmitSurveyAnswer(model, isSubmit, currentTopicId);
            return Json(new { success = true });
        }

        public async Task<IActionResult> SurveyClosingInstruction(string Id, string surveyScheduleUserId)
        {
            var result = await _talentAssessmentBusiness.GetSurveyClosingInstruction(Id);
            result.SurveyScheduleUserId = surveyScheduleUserId;
            result.ClosingInstruction = HttpUtility.HtmlDecode(result.ClosingInstruction);
            return View(result);
        }

        public ActionResult UploadEmployeeList()
        {
            return View();
        }
        public async Task<ActionResult> ReadUserEmployeeList([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<UserReportEmployeeViewModel>();

            var data = await _userReportBusiness.GetList();

            if (data.Count == 0 || data == null)
            {
                //  result.Add(new UserReportEmployeeViewModel { Email = "" });
            }
            else
            {
                result = _autoMapper.Map<IList<TASUserReportViewModel>, IList<UserReportEmployeeViewModel>>(data).ToList();
                result = result.OrderBy(x => x.Email).ToList();
            }

            return Json(result);
            //return Json(result.ToDataSourceResult(request));
        }
        public async Task<ActionResult> ReadUserPromoList([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<UserPromotionViewModel>();
            var data = await _userPromotionBusiness.GetActiveList();

            if (data.Count == 0 || data == null)
            {
                result.Add(new UserPromotionViewModel { EmployeeNumber = "" });
            }
            else
            {
                result = data.ToList();
            }
            return Json(result);
        }
        [HttpPost]
        public async Task<ActionResult> ManageUserEmployeeList(UserReportEmployeeSubmit model)
        {
            var result = new UserReportEmployeeSubmit()
            {
                Created = new List<UserReportEmployeeViewModel>(),
                Updated = new List<UserReportEmployeeViewModel>(),
                Destroyed = new List<UserReportEmployeeViewModel>()
            };

            if (model.IsNotNull())
            {
                if (model.Created != null)
                {
                    foreach (var created in model.Created)
                    {
                        var data = _autoMapper.Map<UserReportEmployeeViewModel, TASUserReportViewModel>(created);
                        var user = await _userBusiness.GetSingle(x => x.Email == created.Email);
                        if (user != null)
                        {
                            data.UserId = user.Id;
                            var result2 = await _userReportBusiness.Create(data);
                            created.Id = result2.Item.Id;
                            result.Created.Add(created);
                        }
                    }
                }

                if (model.Updated != null)
                {
                    foreach (var updated in model.Updated)
                    {
                        var user = await _userBusiness.GetSingle(x => x.Email == updated.Email);

                        var data = await _userReportBusiness.GetSingleById(updated.Id);
                        if (user != null)
                        {
                            data.UserId = user.Id;
                            data.Email = user.Id;
                            data.Entity = updated.Email;
                            data.NewSide = updated.NewSide;
                            data.NationalityCategory = updated.NationalityCategory;
                            data.Sex = updated.Sex;
                            data.Age = updated.Age;
                            data.UserJobTitle = updated.UserJobTitle;
                            data.TypeOfOrganizationalUnit = updated.TypeOfOrganizationalUnit;
                            data.EmployeeNumber = updated.EmployeeNumber;
                            data.UserFullName = updated.UserFullName;
                            data.ManagerNumber = updated.ManagerNumber;
                            data.ManagerName = updated.ManagerName;
                            data.Grievances2018 = updated.Grievances2018;
                            data.Grievances2019 = updated.Grievances2019;
                            data.SubordinateGrievances2018 = updated.SubordinateGrievances2018;
                            data.SubordinateGrievances2019 = updated.SubordinateGrievances2019;
                            data.RewardsAndIncentives2018 = updated.RewardsAndIncentives2018;
                            data.RewardsAndIncentives2019 = updated.RewardsAndIncentives2019;
                            data.SubordinateRewardsAndIncentives2018 = updated.SubordinateRewardsAndIncentives2018;
                            data.SubordinateRewardsAndIncentives2019 = updated.SubordinateRewardsAndIncentives2019;
                            // data.Internshipfor2019 = updated.Internshipfor2019;
                            // data.PerformanceRewardsAndIncentiveIn2019And2018 = updated.PerformanceRewardsAndIncentiveIn2019And2018;
                            data.Promotions2017 = updated.Promotions2017;
                            data.Promotions2018 = updated.Promotions2018;
                            data.Promotions2019 = updated.Promotions2019;

                            data.SubordinatePromotions2017 = updated.SubordinatePromotions2017;
                            data.SubordinatePromotions2018 = updated.SubordinatePromotions2018;
                            data.SubordinatePromotions2019 = updated.SubordinatePromotions2019;
                            // data.Promotions = updated.Promotions;
                            data.JobPerformance2017 = updated.JobPerformance2017;
                            data.JobPerformance2018 = updated.JobPerformance2018;
                            data.JobPerformance2019 = updated.JobPerformance2019;
                            data.AverageJobPerformanceForThreeYears = updated.AverageJobPerformanceForThreeYears;
                            data.FunctionalPerformanceResult = updated.FunctionalPerformanceResult;
                            data.Violations2018 = updated.Violations2018;
                            data.Violations2019 = updated.Violations2019;
                            data.SubordinateViolations2018 = updated.SubordinateViolations2018;
                            data.SubordinateViolations2019 = updated.SubordinateViolations2019;
                            data.Qualification = updated.Qualification;
                            data.ResultOfEducationalQualification = updated.ResultOfEducationalQualification;
                            data.NumberOfTrainingHours2019 = updated.NumberOfTrainingHours2019;
                            data.NumberOfSubordinateTrainingHours2019 = updated.NumberOfSubordinateTrainingHours2019;
                            //data.PerformanceRewardsAndIncentiveIn2019And2018 = updated.PerformanceRewardsAndIncentiveIn2019And2018;
                            data.Nationality = updated.Nationality;
                            data.DateOfHiring = updated.DateOfHiring;
                            data.NumberOfYearsOfService = updated.NumberOfYearsOfService;
                            data.ResultOfTheNumberOfYearsOfService = updated.ResultOfTheNumberOfYearsOfService;
                            data.TotalNoOfExperienceInSameJob = updated.TotalNoOfExperienceInSameJob;
                            data.Class = updated.Class;
                            data.ClassClass = updated.ClassClass;
                            data.OrganizationalUnit = updated.OrganizationalUnit;
                            data.TypeOfContract = updated.TypeOfContract;
                            data.LastChangeInPosition = updated.LastChangeInPosition;
                            data.NoOfSubordinateEmployeePromotion2017 = updated.NoOfSubordinateEmployeePromotion2017;
                            data.NoOfSubordinateEmployeePromotion2018 = updated.NoOfSubordinateEmployeePromotion2018;
                            data.NoOfSubordinateEmployeePromotion2019 = updated.NoOfSubordinateEmployeePromotion2019;
                            data.DepartmentEmployeeCount = updated.DepartmentEmployeeCount;
                            data.MonthYear = updated.MonthYear;
                            data.ResultComparedEmp1 = updated.ResultComparedEmp1;
                            data.ResultComparedEmp2 = updated.ResultComparedEmp2;
                            data.ResultComparedEmp3 = updated.ResultComparedEmp3;
                            data.ResultComparedEmp4 = updated.ResultComparedEmp4;
                            data.ResultComparedEmpValue1 = updated.ResultComparedEmpValue1;
                            data.ResultComparedEmpValue2 = updated.ResultComparedEmpValue2;
                            data.ResultComparedEmpValue3 = updated.ResultComparedEmpValue3;
                            data.ResultComparedEmpValue4 = updated.ResultComparedEmpValue4;
                            data.IsSubordinateReport = updated.IsSubordinateReport;

                            var result2 = await _userReportBusiness.Edit(data);
                            result.Updated.Add(updated);
                        }

                    }
                }

                if (model.Destroyed != null)
                {

                }

                return Json(result);
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ManageUserPromoList(UserPromotionSubmit model)
        {
            var result = new UserPromotionSubmit()
            {
                Created = new List<UserPromotionViewModel>(),
                Updated = new List<UserPromotionViewModel>(),
                Destroyed = new List<UserPromotionViewModel>()
            };


            if (model.IsNotNull())
            {
                if (model.Created != null)
                {
                    foreach (var created in model.Created)
                    {
                        if (created.EmployeeNumber != null)
                        {
                            var result2 = await _userPromotionBusiness.Create(created);
                            created.Id = result2.Item.Id;
                            result.Created.Add(created);
                        }
                    }
                }

                if (model.Updated != null)
                {
                    foreach (var updated in model.Updated)
                    {

                        var result2 = await _userPromotionBusiness.Edit(updated);
                        result.Updated.Add(updated);

                    }
                }

                if (model.Destroyed != null)
                {

                }

                return Json(result);
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public ActionResult UploadAssessmentResult()
        {
            return View();
        }
        public async Task<ActionResult> ReadAssessmentResult([DataSourceRequest] DataSourceRequest request)
        {

            var model = await _userReportBusiness.GetAssessmentResultList();
            return Json(model);
            // return Json(model.ToDataSourceResult(request));
        }
        [HttpPost]
        public async Task<ActionResult> ManageUserAssessmentResult(AssessmentResultSubmit model)
        {
            var result = new AssessmentResultSubmit()
            {
                Created = new List<UserAssessmentResultViewModel>(),
                Updated = new List<UserAssessmentResultViewModel>(),
                Destroyed = new List<UserAssessmentResultViewModel>()
            };

            if (model.IsNotNull())
            {
                if (model.Created != null)
                {

                }

                if (model.Updated != null)
                {
                    foreach (var m in model.Updated)
                    {
                        var data = await _userReportBusiness.GetSingleById(m.Id);
                        if (data.IsNotNull())
                        {
                            data.TotalScore = m.TotalScore.IsNotNull() ? long.Parse(m.TotalScore) : 0;
                            data.ResultOfTechnicalAssessment = m.TechnicalAssessment10Percent;
                            data.TechnicalScore = m.TechnicalAssessmentScore;
                            data.ResultOfCaseStudy = m.CaseStudy10Percent;
                            data.CaseStudyScore = m.CaseStudyScore;
                            data.ResultOfTechnicalInterview = m.TechnicalInterview15Percent;
                            data.InterviewScore = m.TechnicalInterviewScore;
                            data.ILM1 = m.ILM1;
                            data.ILM2 = m.ILM2;
                            data.ILM3 = m.ILM3;
                            data.ILM4 = m.ILM4;
                            data.ILM5 = m.ILM5;
                            data.ILM6 = m.ILM6;
                            data.ILM7 = m.ILM7;
                            data.ILM8 = m.ILM8;
                            data.ILM9 = m.ILM9;
                            data.LeadershipScore = m.ILMScore;
                            data.Control = m.Control;
                            data.Commitment = m.Commitment;
                            data.TheChallenge = m.TheChallenge;
                            data.Altafah = m.Altafah;
                            data.MentalAbilityScore = m.MTQScore;
                            var res = await _userReportBusiness.Edit(data);
                        }

                    }

                }

                if (model.Destroyed != null)
                {

                }

                return Json(result);
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public async Task<ActionResult> AssessmentReportForm(string userId)
        {
            var model = await _userReportBusiness.GetUserReportData(userId);
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateUserReport(TASUserReportViewModel model)
        {

            var data = await _userReportBusiness.GetUserReportData(model.UserId);
            if (data != null)
            {

                data.Entity = model.Entity;
                data.NewSide = model.NewSide;
                data.NationalityCategory = model.NationalityCategory;
                data.Sex = model.Sex;
                data.Age = model.Age;
                data.UserJobTitle = model.UserJobTitle;
                data.TypeOfOrganizationalUnit = model.TypeOfOrganizationalUnit;
                data.EmployeeNumber = model.EmployeeNumber;
                data.UserFullName = model.UserFullName;
                data.Grievances2018 = model.Grievances2018;
                data.Grievances2019 = model.Grievances2019;
                data.SubordinateGrievances2018 = model.SubordinateGrievances2018;
                data.SubordinateGrievances2019 = model.SubordinateGrievances2019;
                data.RewardsAndIncentives2018 = model.RewardsAndIncentives2018;
                data.RewardsAndIncentives2019 = model.RewardsAndIncentives2019;
                data.SubordinateRewardsAndIncentives2018 = model.SubordinateRewardsAndIncentives2018;
                data.SubordinateRewardsAndIncentives2019 = model.SubordinateRewardsAndIncentives2019;
                data.JobPerformance2017 = model.JobPerformance2017;
                data.JobPerformance2018 = model.JobPerformance2018;
                data.JobPerformance2019 = model.JobPerformance2019;

                data.Promotions2017 = model.Promotions2017;
                data.Promotions2018 = model.Promotions2018;
                data.Promotions2019 = model.Promotions2019;
                //  data.Promotions = model.Promotions;

                data.AverageJobPerformanceForThreeYears = model.AverageJobPerformanceForThreeYears;
                data.FunctionalPerformanceResult = model.FunctionalPerformanceResult;
                data.Violations2018 = model.Violations2018;
                data.Violations2019 = model.Violations2019;
                data.SubordinateViolations2018 = model.SubordinateViolations2018;
                data.SubordinateViolations2019 = model.SubordinateViolations2019;
                data.Qualification = model.Qualification;
                data.ResultOfEducationalQualification = model.ResultOfEducationalQualification;
                data.NumberOfTrainingHours2019 = model.NumberOfTrainingHours2019;
                //data.PerformanceRewardsAndIncentiveIn2019And2018 = model.PerformanceRewardsAndIncentiveIn2019And2018;
                data.Nationality = model.Nationality;
                data.DateOfHiring = model.DateOfHiring;
                data.NumberOfYearsOfService = model.NumberOfYearsOfService;
                data.ResultOfTheNumberOfYearsOfService = model.ResultOfTheNumberOfYearsOfService;
                data.Class = model.Class;

                data.TechnicalScore = model.TechnicalScore;
                data.CaseStudyScore = model.CaseStudyScore;
                data.InterviewScore = model.InterviewScore;
                data.MentalAbilityScore = model.MentalAbilityScore;
                data.LeadershipScore = model.LeadershipScore;

                data.NoOfSubordinateEmployeePromotion2017 = model.NoOfSubordinateEmployeePromotion2017;
                data.NoOfSubordinateEmployeePromotion2018 = model.NoOfSubordinateEmployeePromotion2018;
                data.NoOfSubordinateEmployeePromotion2019 = model.NoOfSubordinateEmployeePromotion2019;
                data.DepartmentEmployeeCount = model.DepartmentEmployeeCount;
                data.MonthYear = model.MonthYear;
                data.ResultComparedEmp1 = model.ResultComparedEmp1;
                data.ResultComparedEmp2 = model.ResultComparedEmp2;
                data.ResultComparedEmp3 = model.ResultComparedEmp3;
                data.ResultComparedEmp4 = model.ResultComparedEmp4;
                data.ResultComparedEmpValue1 = model.ResultComparedEmpValue1;
                data.ResultComparedEmpValue2 = model.ResultComparedEmpValue2;
                data.ResultComparedEmpValue3 = model.ResultComparedEmpValue3;
                data.ResultComparedEmpValue4 = model.ResultComparedEmpValue4;
                data.IsSubordinateReport = model.IsSubordinateReport;

                var result2 = await _userReportBusiness.Edit(data);
            }


            return Json(new { success = true });
        }

        public async Task<string> GetFileBase64(string mongoId)
        {
            var ContentBase64 = Convert.ToBase64String(await _fileBusiness.DownloadMongoFileByte(mongoId));
            return ContentBase64;
        }

        public async Task<string> GetTopicDataByCode(string code)
        {
            var where = $@"  and ""N_TAS_Topic"".""Code"" = '{code}'";
            var data = await _cmsBusiness.GetDataListByTemplate("TAS_TOPIC", "", where);
            foreach (DataRow dr in data.Rows)
            {
                var fileId = dr["TopicDataFileId"].ToString();
                if (fileId.IsNotNullAndNotEmpty())
                {
                    return fileId;
                }
            }

            return null;
        }

        public IActionResult OpenSurveyRegister(string surveyScheduleId)
        {
            var model = new SurveyScheduleViewModel()
            {
                SurveyScheduleId = surveyScheduleId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OpenSurveyRegister(string surveyScheduleId, string orgId, string orgName /*string fullName, string email, string jobTitle*/)
        {
            var userId = "";
            var portal = await _portalBusiness.GetSingle(x => x.Name == "HRClimate");

            var prefix = "SurveyUser";
            var suffix = "@survey.test";

            var notemodel = new NoteTemplateViewModel() { NumberGenerationType = NumberGenerationTypeEnum.SystemGenerated };

            var noteno = await _noteBusiness.GenerateNextNoteNo(notemodel);

            var useremail = $"{prefix}-{noteno}{suffix}";
            var count = await _talentAssessmentBusiness.GetSurveyUserCount("SurveyUser_");
            var usermodel = new UserViewModel()
            {
                Name = String.Concat("SurveyUser_", noteno),
                Email = useremail,
                SignatureId = orgId,
                Status = StatusEnum.Active,
                SequenceOrder = count,
                JobTitle = orgName
            };

            var result = await _userBusiness.Create(usermodel);
            if (result.IsSuccess)
            {
                userId = result.Item.Id;

                var userportalmodel = new UserPortalViewModel()
                {
                    UserId = userId,
                    PortalId = portal.Id
                };

                var userportal = await _userportalBusiness.Create(userportalmodel);
                if (!userportal.IsSuccess)
                {
                    return Json(new { success = false, error = userportal.Messages });
                }
            }
            else
            {
                return Json(new { success = false, error = "Not able to continue to survey. Please contact administrator" });
            }

            //var user = await _userBusiness.GetSingle(x => x.Email == email);
            //if (user.IsNotNull())
            //{
            //    userId = user.Id;
            //    if (portal.IsNotNull())
            //    {
            //        var res = await _userportalBusiness.GetSingle(x => x.UserId == userId && x.PortalId == portal.Id);

            //        if (!res.IsNotNull())
            //        {
            //            var userportalmodel = new UserPortalViewModel()
            //            {
            //                UserId = userId,
            //                PortalId = portal.Id
            //            };

            //            var userportal = await _userportalBusiness.Create(userportalmodel);
            //            if (!userportal.IsSuccess)
            //            {
            //                return Json(new { success = false, error = userportal.Messages });
            //            }
            //        }                    
            //    }
            //}
            //else
            //{
            //    var usermodel = new UserViewModel()
            //    {
            //        Name = fullName,
            //        Email = email,
            //        JobTitle = jobTitle, 
            //        Status = StatusEnum.Active
            //    };

            //    var result = await _userBusiness.Create(usermodel);
            //    if (result.IsSuccess)
            //    {
            //        userId = result.Item.Id;

            //        var userportalmodel = new UserPortalViewModel()
            //        {
            //            UserId = userId,
            //            PortalId = portal.Id
            //        };

            //        var userportal = await _userportalBusiness.Create(userportalmodel);
            //        if (!userportal.IsSuccess)
            //        {
            //            return Json(new { success = false, error = userportal.Messages });
            //        }
            //    }
            //    else
            //    {
            //        return Json(new { success = false, error="Not able to create the user. Please contact administrator" });
            //    }
            //}

            var user = await _userBusiness.ValidateUserById(userId);
            if (user != null)
            {
                if (user.Status == StatusEnum.Inactive)
                {
                    return Json(new { success = false, error = "User is inactive. Please activate the user or contact administrator" });
                }

                var identity = new ApplicationIdentityUser
                {
                    Id = user.Id,
                    UserName = user.Name,
                    IsSystemAdmin = user.IsSystemAdmin,
                    Email = user.Email,
                    UserUniqueId = user.Email,
                    CompanyId = user.CompanyId,
                    CompanyCode = user.CompanyCode,
                    CompanyName = user.CompanyName,
                    JobTitle = user.JobTitle,
                    PhotoId = user.PhotoId,
                    UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                    UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                    PortalId = portal.Id,
                    UserPortals = user.UserPortals,
                    IsGuestUser = user.IsGuestUser,
                    LegalEntityId = user.LegalEntityId,
                    LegalEntityCode = user.LegalEntityCode,
                    PersonId = user.PersonId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                };
                if (portal.Id.IsNotNullAndNotEmpty())
                {
                    //var portal = await _portalBusiness.GetSingleById(portal.Id);
                    if (portal != null)
                    {
                        identity.PortalTheme = portal.Theme.ToString();
                        identity.PortalName = portal.Name;
                    }
                }
                identity.MapClaims();
                var loginResult = await _customUserManager.PasswordSignInAsync(identity, user.Password, true, lockoutOnFailure: false);
                if (loginResult.Succeeded)
                {
                    await _applicationAccessBusiness.CreateAccessLog(Request, AccessLogTypeEnum.Login);
                    var surveydata = await _talentAssessmentBusiness.AssignDummySurveyToUser(surveyScheduleId, userId);

                    return Json(new { success = true, surveylink = surveydata.SurveyLink });
                }
                else
                {
                    return Json(new { success = false, error = "Invalid User Name/Password" });
                }
            }
            else
            {
                return Json(new { success = false, error = "Invalid User Name/Password" });
            }

            //return Json(new { success = false,error= "Not able to create the user. Please contact administrator" });

        }

    }
}
