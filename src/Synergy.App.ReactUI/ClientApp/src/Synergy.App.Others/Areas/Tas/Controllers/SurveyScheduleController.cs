using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Hangfire;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CMS.UI.Web.Areas.TAS.Controllers
{
    [Area("Tas")]
    public class SurveyScheduleController : ApplicationController
    {
        private IUserContext _userContext;
        private INoteBusiness _noteBusiness;
        private ICmsBusiness _cmsBusiness;
        private ITalentAssessmentBusiness _talentAssessmentBusiness;
        private IConfiguration _configuration;
        public SurveyScheduleController(IUserContext userContext, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness
            , ITalentAssessmentBusiness talentAssessmentBusiness, IConfiguration configuration)
        {
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
            _talentAssessmentBusiness = talentAssessmentBusiness;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult SurveyScheduleIndex()
        {
            return View();
        }
        public async Task<IActionResult> UpdateSurveySchedule(string id, bool isOpenSurveySchedule = false,bool isVersion=false)
        {
            SurveyScheduleViewModel model = new SurveyScheduleViewModel();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _talentAssessmentBusiness.GetSurveyScheduleDetails(id);
                model.DataAction = DataActionEnum.Edit;
                model.SurveyStatus = SurveyStatusEnum.NoteScheduled;
                model.IsOpenSurveySchedule = isOpenSurveySchedule;               
                if (isVersion)
                {
                    model.VersionNo = model.VersionNo + 1;
                }
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.SurveyStatus = SurveyStatusEnum.NoteScheduled;
                model.SurveyExpiryDate = System.DateTime.Today.AddDays(7);
            }

            return View("ManageSurveySchedule", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageSurveySchedule(SurveyScheduleViewModel model)
        {
            var assessment=await _talentAssessmentBusiness.GetAssessmentDetailsById(model.SurveyId);
           
            var validateResult = await _talentAssessmentBusiness.GetValidateSurveyScheduleUserDetails(model.SurveyId, model.UserDetails, model.Id);
            if (!validateResult.IsSuccess)
            {
                return Json(new { success = validateResult.IsSuccess, error = validateResult.HtmlError });
            }
            

            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    if (assessment.AssessmentType == "OPEN_SURVEY")
                    {
                        var surveySchedule = await _talentAssessmentBusiness.GetSurveyScheduleById(model.SurveyId);
                        if (surveySchedule != null && surveySchedule.Id!=model.Id)
                        {
                            return Json(new { success = false, error = "Survey already scheduled." });
                        }
                    }
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "SURVEY_SCHEDULE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Create;
                    notemodel.NoteSubject = model.SurveyScheduleName;
                    notemodel.NoteDescription = model.SurveyScheduleDescription;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        //if (assessment.AssessmentType == "OPEN_SURVEY")
                        //{                           
                        //    var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                        //    var param = Helper.EncryptJavascriptAesCypher($"surveyScheduleId={result.Item.UdfNoteTableId}");
                        //    var link = $"{baseurl}Survey/open?enc={param}";   
                        //    var OpenSurveyUrl = link;
                        //    var noteTempModel1 = new NoteTemplateViewModel();

                        //    noteTempModel1.NoteId = result.Item.NoteId;
                        //    noteTempModel1.SetUdfValue = true;
                        //    var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                        //    var rowData1 = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        //    var SurveyStatus = rowData1.ContainsKey("OpenSurveyUrl") ? Convert.ToString(rowData1["OpenSurveyUrl"]) : "";
                        //    if (SurveyStatus.IsNotNull())
                        //    {
                        //        rowData1["OpenSurveyUrl"] = OpenSurveyUrl;
                        //        var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                        //        var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);

                        //    }
                        //}
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.NoteId = model.NoteId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    notemodel.DataAction = DataActionEnum.Edit;
                    notemodel.NoteSubject = model.SurveyScheduleName;
                    notemodel.NoteDescription = model.SurveyScheduleDescription;
                    notemodel.VersionNo = model.VersionNo;
                    if (model.IsOpenSurveySchedule)
                    {
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }

                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        if (model.IsOpenSurveySchedule && assessment.AssessmentType == "OPEN_SURVEY")
                        {
                            var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);
                            var param = Helper.EncryptJavascriptAesCypher($"surveyScheduleId={result.Item.UdfNoteTableId}");
                            var link = $"{baseurl}Survey/open?enc={param}";
                            var OpenSurveyUrl = link;
                            var noteTempModel1 = new NoteTemplateViewModel();

                            noteTempModel1.NoteId = result.Item.NoteId;
                            noteTempModel1.SetUdfValue = true;
                            var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                            var rowData1 = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            var SurveyStatus = rowData1.ContainsKey("OpenSurveyUrl") ? Convert.ToString(rowData1["OpenSurveyUrl"]) : "";
                            if (SurveyStatus.IsNotNull())
                            {
                                rowData1["OpenSurveyUrl"] = OpenSurveyUrl;
                                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                                var update = await _noteBusiness.EditNoteUdfTable(notemodel1, data1, notemodel1.UdfNoteTableId);

                            }
                        }
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        public async Task<IActionResult> ReadSurveyScheduleData([DataSourceRequest] DataSourceRequest request, string surveyId, string status)
        {
            var result = await _talentAssessmentBusiness.GetSurveyScheduleList(surveyId,status);
            return Json(result);
            //return Json(result.ToDataSourceResult(request));
        }
        [HttpGet]
        public async Task<ActionResult> GetAssessmentIdNameList()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("SN_TAS_ASSESSMENT", "");
            return Json(data);
        }
        public IActionResult ViewDetails(string id)
        {
            var model = new SurveyScheduleViewModel()
            {
                Id = id,
            
            };
            return View("ViewDetails", model);
        }
        public async Task<IActionResult> ReadSurveyViewDetails([DataSourceRequest] DataSourceRequest request, string id)
        {
            var result = await _talentAssessmentBusiness.GetSurveyDetail(id);
            return Json(result);
           // return Json(result.ToDataSourceResult(request));
        }
        [HttpGet]
        public async Task<ActionResult> GetValidateSurveyScheduleUserDetails(string surveyId,string userDetails, string surveyScheduleId)
        {
            var result = await _talentAssessmentBusiness.GetValidateSurveyScheduleUserDetails(surveyId,userDetails, surveyScheduleId);
            return Json(new { success = result.IsSuccess, error = result.HtmlError });
        }
        public IActionResult ViewSurveyResult(string id)
        {
            var model = new SurveyScheduleViewModel()
            {
                Id = id,

            };
            return View("ViewSurveyResult", model);
        }
        public async Task<IActionResult> ReadSurveyResultDetails([DataSourceRequest] DataSourceRequest request, string id)
        {
           var model = await _talentAssessmentBusiness.GetSurveyResult(id, null);
            var j = Json(model);
            //var j = Json(model.ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> GenerateSurveyDetails(string SurveyScheduleId)
        {
            if (SurveyScheduleId.IsNotNullAndNotEmpty())
            {
                await _talentAssessmentBusiness.GenerateSurveyDetails(SurveyScheduleId);
                //BackgroundJob.Enqueue<HangfireScheduler>(x => x.GenerateSurveyDetails(SurveyScheduleId, _userContext.ToIdentityUser()));
            }
            return Json(new { success = true });
        }
    }
}
