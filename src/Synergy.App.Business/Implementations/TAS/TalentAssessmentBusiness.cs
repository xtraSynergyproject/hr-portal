using AutoMapper;
using AutoMapper.Configuration;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hangfire;

namespace Synergy.App.Business
{
    public class TalentAssessmentBusiness : BusinessBase<NoteViewModel, NtsNote>, ITalentAssessmentBusiness
    {
        private readonly IRepositoryQueryBase<AssessmentViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<AssessmentDetailViewModel> _queryAssDetail;
        private readonly IRepositoryQueryBase<AssessmentQuestionsViewModel> _queryAssQues;
        private readonly IRepositoryQueryBase<AssessmentResultViewModel> _queryAssRes;
        private readonly IRepositoryQueryBase<AssessmentInterviewViewModel> _queryAssInterView;
        private readonly IRepositoryQueryBase<AssessmentAnswerViewModel> _queryAssAns;
        private IUserContext _userContext;
        private IServiceBusiness _serviceBusiness;
        private INoteBusiness _noteBusiness;
        private readonly IServiceProvider _sp;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<AssignmentViewModel> _queryAssignment;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _idNameRepo;
        private readonly ITalentAssessmentQueryBusiness _talentAssessmentQueryBusiness;

        private readonly IRepositoryQueryBase<InterViewQuestionViewModel> _QueryInterviewquestion;
        private IUserBusiness _userBusiness;
        private readonly IFileBusiness _fileBusiness;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public TalentAssessmentBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
            , IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> queryRepo1, IServiceProvider sp,
            IUserContext userContext, IServiceBusiness serviceBusiness, INoteBusiness noteBusiness,
            IRepositoryQueryBase<AssessmentViewModel> queryRepo, IRepositoryQueryBase<AssignmentViewModel> queryAssignment
           , IRepositoryQueryBase<AssessmentQuestionsViewModel> queryAssQues,
                IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo, IRepositoryQueryBase<AssessmentDetailViewModel> queryAssDetail
            , IRepositoryQueryBase<IdNameViewModel> idNameRepo, IRepositoryQueryBase<AssessmentResultViewModel> queryAssRes,
                ILOVBusiness lOVBusiness, IRepositoryQueryBase<AssessmentInterviewViewModel> queryAssInterView,
                IRepositoryQueryBase<AssessmentAnswerViewModel> queryAssAns,
                ITalentAssessmentQueryBusiness talentAssessmentQueryBusiness,
                IRepositoryQueryBase<InterViewQuestionViewModel> QueryInterviewquestion
            , IUserBusiness userBusiness
            , IFileBusiness fileBusiness
            // , IHangfireScheduler hangfireScheduler
            ) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _sp = sp;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _queryRepo = queryRepo;
            _queryAssignment = queryAssignment;
            _queryAssQues = queryAssQues;
            _queryTWRepo = queryTWRepo;
            _queryAssDetail = queryAssDetail;
            _idNameRepo = idNameRepo;
            _queryAssRes = queryAssRes;
            _lOVBusiness = lOVBusiness;
            _queryAssInterView = queryAssInterView;
            _queryAssAns = queryAssAns;
            _QueryInterviewquestion = QueryInterviewquestion;
            _userBusiness = userBusiness;
            _fileBusiness = fileBusiness;
            _talentAssessmentQueryBusiness = talentAssessmentQueryBusiness;
            // _hangfireScheduler = hangfireScheduler;
        }
        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsForQuestion(string questionNoteId)
        {
            return await _talentAssessmentQueryBusiness.GetOptionsForQuestion(questionNoteId);
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsByQuestionId(string questionId)
        {
            return await _talentAssessmentQueryBusiness.GetOptionsByQuestionId(questionId);
        }


        public async Task<List<AssessmentQuestionsViewModel>> GetQuestion()
        {
            return await _talentAssessmentQueryBusiness.GetQuestion();
        }

        public async Task<List<AssessmentQuestionsViewModel>> GetTreeListQuestion()
        {
            return await _talentAssessmentQueryBusiness.GetTreeListQuestion();
        }

        public async Task<IList<AssessmentQuestionsViewModel>> GetAssessmentQuestions(string topic, string level)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentQuestions(topic, level);

            return result;
        }

        public async Task<AssessmentQuestionsOptionViewModel> GetOptionsById(string Id)
        {
            return await _talentAssessmentQueryBusiness.GetOptionsById(Id);
        }
        public async Task DeleteOption(string id)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var sal = await _tableMetadataBusiness.GetTableDataByColumn("TAS_QUESTION_OPTION", "", "Id", id);
            if (sal != null)
            {
                await _talentAssessmentQueryBusiness.DeleteOption(id);
                await _noteBusiness.Delete(sal["NtsNoteId"].ToString());
            }
        }

        public async Task<List<AssessmentViewModel>> GetAssessmentsList(string type, string searchtext)
        {

            var result = await _talentAssessmentQueryBusiness.GetAssessmentsList(type, searchtext);
            return result;

        }
        public async Task<bool> DeleteAssessment(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                await _talentAssessmentQueryBusiness.DeleteAssessment(NoteId);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<AssessmentViewModel> GetAssessmentDetailsById(string assmntId)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentDetailsById(assmntId);
            return result;
        }

        public async Task CopyAssessment(AssessmentViewModel model)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_ASSESSMENT";
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            //var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);

            service.ServiceSubject = model.AssessmentName;
            service.OwnerUserId = _userContext.UserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Create;

            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            service.Json = JsonConvert.SerializeObject(model);

            var res = await _serviceBusiness.ManageService(service);

            if (res.IsSuccess)
            {
                var questions = await GetQuestionsByAssessmentId(model.AssessmentId);

                foreach (var qId in questions)
                {
                    var noteTemplate = new NoteTemplateViewModel();
                    noteTemplate.ActiveUserId = _userContext.UserId;
                    noteTemplate.TemplateCode = "TAS_ASSESSMENT_QUESTION";
                    noteTemplate.DataAction = DataActionEnum.Create;
                    var assQues = await _noteBusiness.GetNoteDetails(noteTemplate);
                    assQues.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                    var assessmentQuesModel = new AssessmentViewModel();
                    assessmentQuesModel.AssessmentId = res.Item.UdfNoteTableId;
                    assessmentQuesModel.QuestionId = qId.QuestionId;

                    assQues.Json = JsonConvert.SerializeObject(assessmentQuesModel);

                    var assQuesResult = await _noteBusiness.ManageNote(assQues);
                }

            }

        }

        public async Task<List<AssessmentViewModel>> GetQuestionsByAssessmentId(string assessmentId)
        {
            var result = await _talentAssessmentQueryBusiness.GetQuestionsByAssessmentId(assessmentId);
            return result;
        }

        public async Task<List<CapacityRiskViewModel>> GetJobCapacityData(string departmentId)
        {
            var dept = await _talentAssessmentQueryBusiness.GetJobTitles(departmentId);
            var deptids = "'" + string.Join("','", dept.Select(x => x.Id)) + "'";

            return await _talentAssessmentQueryBusiness.GetJobCapacityData(deptids);
        }

        public async Task<List<CapacityRiskViewModel>> GetJobCapacityChartData(string jobId)
        {
            return await _talentAssessmentQueryBusiness.GetJobCapacityChartData(jobId);
        }
        public async Task<AssignmentViewModel> GetAssignmentDetails(string userId)
        {
            var queryData = await _talentAssessmentQueryBusiness.GetAssignmentDetails(userId);
            return queryData;
        }
        public async Task<List<AssignmentViewModel>> ReadBadgeData(string userId)
        {
            return await _talentAssessmentQueryBusiness.ReadBadgeData(userId);
        }
        public async Task<List<AssignmentViewModel>> ReadAssessmentData(string userId)
        {
            return await _talentAssessmentQueryBusiness.ReadAssessmentData(userId);
        }


        public async Task<List<CapacityRiskViewModel>> GetJobCapacityChartData(string jobId, string departmentId)
        {
            var jobids = "";
            if (departmentId.IsNotNullAndNotEmpty())
            {
                var dept = await _talentAssessmentQueryBusiness.GetJobTitles(departmentId);
                jobids = "'" + string.Join("','", dept.Select(x => x.Id)) + "'";

            }


            return await _talentAssessmentQueryBusiness.GetJobCapacityChartData(jobids, jobId);
        }

        public async Task<List<AssessmentQuestionsViewModel>> GetMappedQuestionsList(string assessmentId)
        {
            var result = await _talentAssessmentQueryBusiness.GetMappedQuestionsList(assessmentId);
            return result;

        }

        public async Task<bool> UpdateSequenceNo(string Id, long? sequenceNo)
        {
            await _talentAssessmentQueryBusiness.UpdateSequenceNo(Id, sequenceNo);
            return true;
        }

        public async Task<List<AssessmentQuestionsViewModel>> GetAllQuestions(string assessmentTypeId)
        {
            return await _talentAssessmentQueryBusiness.GetAllQuestions(assessmentTypeId);
        }

        public async Task<bool> DeleteAssessmentQuestion(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                await _talentAssessmentQueryBusiness.DeleteAssessmentQuestion(NoteId);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<bool> MapQuestionsToAssessment(string assessmentId, string qIds)
        {

            string[] QId = qIds.Trim(',').Split(",").ToArray();

            var mappedquelist = await GetMappedQuestionsList(assessmentId);
            var quecount = mappedquelist.Count();
            var seq = quecount > 0 ? quecount : 0;

            foreach (var id in QId)
            {
                var noteTemplate = new NoteTemplateViewModel();
                noteTemplate.ActiveUserId = _userContext.UserId;
                noteTemplate.TemplateCode = "TAS_ASSESSMENT_QUESTION";
                noteTemplate.DataAction = DataActionEnum.Create;
                var assQues = await _noteBusiness.GetNoteDetails(noteTemplate);
                assQues.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                var assessmentQuesModel = new AssessmentViewModel();
                assessmentQuesModel.AssessmentId = assessmentId;
                assessmentQuesModel.QuestionId = id;
                assessmentQuesModel.SequenceOrder = seq + 1;

                assQues.Json = JsonConvert.SerializeObject(assessmentQuesModel);

                var assQuesResult = await _noteBusiness.ManageNote(assQues);

                seq++;
            }
            return true;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceTaskViewData(string projectId)
        {
            var queryData = await _talentAssessmentQueryBusiness.ReadPerformanceTaskViewData(projectId);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceDevelopmentViewData()
        {
            var queryData = await _talentAssessmentQueryBusiness.ReadPerformanceDevelopmentViewData();
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyList()
        {
            var result = await _talentAssessmentQueryBusiness.GetCompetencyList();
            return result;
        }

        public async Task<List<AssignmentViewModel>> ReadCertificationData(string userId)
        {
            return await _talentAssessmentQueryBusiness.ReadCertificationData(userId);
        }
        public async Task<List<AssignmentViewModel>> ReadSkillData(string userId)
        {
            return await _talentAssessmentQueryBusiness.ReadSkillData(userId);
        }

        public async Task<List<AssessmentDetailViewModel>> GetAssessmentListByUserId(string userId, string statusCode)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentListByUserId(userId, statusCode);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<List<AssessmentDetailViewModel>> GetCompletedAssessmentListByUserId(string userId)
        {
            var result = await _talentAssessmentQueryBusiness.GetCompletedAssessmentListByUserId(userId);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }
        public async Task<List<IdNameViewModel>> GetAssessmentSetListIdName()
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentSetListIdName();
            return result.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAssessmentListIdName(string assSetId)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentListIdName(assSetId);
            return result.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAssessmentProctorList()
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentProctorList();
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetIndicatorList(string levelId = null)
        {
            var result = await _talentAssessmentQueryBusiness.GetIndicatorList(levelId);
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyLevelList(string noteId = null)
        {
            var result = await _talentAssessmentQueryBusiness.GetCompetencyLevelList(noteId);
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyLevelListByTopic(string topicId)
        {
            var result = await _talentAssessmentQueryBusiness.GetCompetencyLevelListByTopic(topicId);
            return result.ToList();
        }

        public async Task<IdNameViewModel> GetQuestionCountByTopic(string topicId)
        {
            var result = await _talentAssessmentQueryBusiness.GetQuestionCountByTopic(topicId);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetJobTitle(string loggedInUserId)
        {
            var result = await _talentAssessmentQueryBusiness.GetJobTitle(loggedInUserId);
            result = result.Where(x => x.Name.IsNotNullAndNotEmpty()).ToList();
            return result;
        }
        public async Task<List<AssessmentDetailViewModel>> GetAllAssessmentsList()
        {
            var result = await _talentAssessmentQueryBusiness.GetAllAssessmentsList();

            return result;
        }

        public async Task<AssessmentResultViewModel> GetQuestion(string serviceId)
        {
            var result = await _talentAssessmentQueryBusiness.GetQuestion(serviceId);
            return result;
        }
        public async Task<SurveyTopicViewModel> GetSurveyQuestion(string serviceId)
        {
            var result = await _talentAssessmentQueryBusiness.GetSurveyQuestion(serviceId);
            return result;
        }

        public async Task<SurveyScheduleViewModel> GetSurveyClosingInstruction(string surveyId)
        {
            var result = await _talentAssessmentQueryBusiness.GetSurveyClosingInstruction(surveyId);
            return result;
        }

        public async Task<AssessmentDetailViewModel> GetTryoutQuestion(string assessmentId, string currQueId, string lang = null)
        {
            return await GetAssessmentTryoutQuestion(assessmentId, currQueId, lang);

        }
        public async Task<AssessmentDetailViewModel> GetAssessmentTryoutQuestion(string assessmentId, string curQueId, string lang)
        {
            var result = new AssessmentDetailViewModel();
            var list = await _talentAssessmentQueryBusiness.GetQuestions(assessmentId, lang);

            if (list != null && list.Count != 0)
            {
                if (curQueId.IsNullOrEmpty())
                {
                    result.NoteId = list.Where(x => x.SequenceNo == 1).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == 2).Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    result.NoteId = curQueId;
                    var seq = list.Where(x => x.Id == curQueId).Select(x => x.SequenceNo).FirstOrDefault();
                    var next = seq + 1;
                    var prev = seq - 1;
                    result.PreviousId = list.Where(x => x.SequenceNo == prev).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == next).Select(x => x.Id).FirstOrDefault();
                }
                result.FirstId = list.First().Id;
                result.LastId = list.Last().Id;

                result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

                if (result.NoteId.IsNotNull())
                {
                    var question = list.Where(x => x.Id == result.NoteId).FirstOrDefault();
                    if (question != null)
                    {
                        result.Subject = HttpUtility.HtmlDecode(question.Question);
                        result.Description = HttpUtility.HtmlDecode(question.QuestionDescription);
                        result.TotalCount = list.Count;
                        result.SequenceNo = list.Where(x => x.Id == result.NoteId).Select(x => x.SequenceNo).FirstOrDefault();
                        result.AssessmentDuration = list.Where(x => x.Id == result.NoteId).Select(x => x.AssessmentDuration).FirstOrDefault();

                        result.FileId = list.Where(x => x.Id == result.NoteId).Select(x => x.FileId).FirstOrDefault();

                    }
                    result.OptionList = await GetTryoutQuestionOptions(result.NoteId, lang);
                    //if (result.OptionList != null && result.OptionList.Count > 0)
                    //{
                    //    var answer = result.OptionList.FirstOrDefault();
                    //    if (answer != null)
                    //    {
                    //        //result.CandidateAnswer = answer.CandidateAnswerComment;
                    //        result.CandidateAnswerComment = answer.AnswerComment;
                    //        result.Score = answer.Score;
                    //        //result.InterviewerComment = answer.InterviewerComment;
                    //        result.AnswerId = answer.AnswerId;
                    //    }
                    //}
                }
            }
            return result;
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetTryoutQuestionOptions(string noteId, string lang)
        {
            var result = await _talentAssessmentQueryBusiness.GetTryoutQuestionOptions(noteId, lang);
            return result;
        }

        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionEnglish(string serviceId, string currQueId)
        {
            if (currQueId.IsNullOrEmpty())
            {
                var res = await _talentAssessmentQueryBusiness.GetAssessmentQuestionEnglish(serviceId);

                return await GetAssessmentQuestionParent(serviceId, res.CurrentQuestionId, "ENGLISH");
            }
            else
            {
                return await GetAssessmentQuestionParent(serviceId, currQueId, "ENGLISH");
            }

        }
        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionArabic(string serviceId, string currQueId)
        {
            if (currQueId.IsNullOrEmpty())
            {
                var res = await _talentAssessmentQueryBusiness.GetAssessmentQuestionArabic(serviceId);

                return await GetAssessmentQuestionParent(serviceId, res.CurrentQuestionId, "ARABIC");
            }
            else
            {
                return await GetAssessmentQuestionParent(serviceId, currQueId, "ARABIC");
            }

        }

        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionParent(string serviceId, string curQueId, string lang)
        {
            var result = new AssessmentDetailViewModel();

            var list = await _talentAssessmentQueryBusiness.GetAssessmentQuestionsDetail(serviceId, lang);

            if (list != null && list.Count != 0)
            {
                if (curQueId.IsNullOrEmpty())
                {
                    result.NoteId = list.Where(x => x.SequenceNo == 1).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == 2).Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    result.NoteId = curQueId;
                    var seq = list.Where(x => x.Id == curQueId).Select(x => x.SequenceNo).FirstOrDefault();
                    var next = seq + 1;
                    var prev = seq - 1;
                    result.PreviousId = list.Where(x => x.SequenceNo == prev).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == next).Select(x => x.Id).FirstOrDefault();
                }
                result.FirstId = list.First().Id;
                result.LastId = list.Last().Id;
                result.AssessmentId = list.First().AssessmentId;
                result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

                if (result.NoteId.IsNotNull())
                {
                    var question = list.Where(x => x.Id == result.NoteId).FirstOrDefault();
                    if (question != null)
                    {
                        result.Subject = HttpUtility.HtmlDecode(question.Question);
                        result.Description = HttpUtility.HtmlDecode(question.QuestionDescription);
                        result.TotalCount = list.Count;
                        result.SequenceNo = list.Where(x => x.Id == result.NoteId).Select(x => x.SequenceNo).FirstOrDefault();
                        result.AssessmentDuration = list.Where(x => x.Id == result.NoteId).Select(x => x.AssessmentDuration).FirstOrDefault();
                        result.TimeElapsed = list.Where(x => x.Id == result.NoteId).Select(x => x.TimeElapsed).FirstOrDefault();
                        result.TimeElapsedSec = list.Where(x => x.Id == result.NoteId).Select(x => x.TimeElapsedSec).FirstOrDefault();
                        result.FileId = list.Where(x => x.Id == result.NoteId).Select(x => x.FileId).FirstOrDefault();
                        result.IsAssessmentStopped = list.Where(x => x.Id == result.NoteId).Select(x => x.IsAssessmentStopped).FirstOrDefault();
                    }
                    result.OptionList = await GetOptions(result.NoteId, serviceId, lang, result.AssessmentId);
                    if (result.OptionList != null && result.OptionList.Count > 0)
                    {
                        var answer = result.OptionList.FirstOrDefault();
                        if (answer != null)
                        {
                            //result.CandidateAnswer = answer.CandidateAnswerComment;
                            result.CandidateAnswerComment = answer.AnswerComment;
                            result.Score = answer.Score;
                            //result.InterviewerComment = answer.InterviewerComment;
                            result.AnswerId = answer.AnswerId;
                        }
                    }

                }
            }

            return result;
        }

        public async Task<AssessmentAnswerViewModel> GetQuestionByAnswerId(string noteId)
        {
            var result = await _talentAssessmentQueryBusiness.GetQuestionByAnswerId(noteId);
            return result;
        }

        public async Task<bool> UpdateScoreComment(List<AssessmentAnswerViewModel> modelList)
        {
            await _talentAssessmentQueryBusiness.UpdateScoreComment(modelList);

            return true;

        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptions(string noteId, string serviceId, string lang, string assessmentId)
        {
            var result = await _talentAssessmentQueryBusiness.GetOptions(noteId, serviceId, lang, assessmentId);
            return result;
        }

        public async Task SubmitAssessmentAnswer(string serviceId, bool isSubmit, string currentQuestionId, string multipleFieldValueId, string comment, int? timeElapsed, int? timeElapsedSec, string nextQuestionId, string currentAnswerId, string assessmentType, string userId, string assessmentId)
        {

            if (currentAnswerId.IsNotNullAndNotEmpty())
            {
                //if(currentAnswerId != multipleFieldValueId)
                //{

                var result = await _talentAssessmentQueryBusiness.GetAssessmentAnswer(serviceId, currentQuestionId);
                if (result != null)
                {
                    var serdetail = await _serviceBusiness.GetSingle(x => x.UdfNoteId == result.NtsNoteId);

                    ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    serviceModel.ActiveUserId = _userContext.UserId;
                    serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_ANSWER";
                    serviceModel.ServiceId = serdetail.Id;
                    var service = await _serviceBusiness.GetServiceDetails(serviceModel);
                    service.DataAction = DataActionEnum.Edit;
                    service.AllowPastStartDate = true;

                    result.OptionId = multipleFieldValueId;
                    result.Comment = comment;

                    service.Json = JsonConvert.SerializeObject(result);

                    var res = await _serviceBusiness.ManageService(service);
                }
                //}
            }
            else
            {
                var model = new AssessmentAnswerViewModel()
                {
                    QuestionId = currentQuestionId,
                    OptionId = multipleFieldValueId,
                    Comment = comment,
                    UserId = userId,
                    AssessmentId = assessmentId
                };

                ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                serviceModel.ActiveUserId = _userContext.UserId;
                serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_ANSWER";
                var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                //service.ServiceSubject = ;
                service.OwnerUserId = _userContext.UserId;
                service.StartDate = DateTime.Now;
                service.DueDate = DateTime.Now.AddDays(10);
                service.ActiveUserId = _userContext.UserId;
                service.DataAction = DataActionEnum.Create;
                service.ParentServiceId = serviceId;
                service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                service.Json = JsonConvert.SerializeObject(model);

                var res = await _serviceBusiness.ManageService(service);
            }

            ServiceTemplateViewModel serviceModel1 = new ServiceTemplateViewModel();
            serviceModel1.ActiveUserId = _userContext.UserId;
            serviceModel1.TemplateCode = "TAS_USER_ASSESSMENT_RESULT";
            serviceModel1.ServiceId = serviceId;
            var service1 = await _serviceBusiness.GetServiceDetails(serviceModel1);

            var assResult = await _talentAssessmentQueryBusiness.GetAssessmentResult(service1);

            if (isSubmit)
            {
                service1.DataAction = DataActionEnum.Edit;
                service1.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                service1.AllowPastStartDate = true;
                await _serviceBusiness.ManageService(service1);

                await _talentAssessmentQueryBusiness.UpdateAssessmentResult(service1);

                await UpdateAssessment(serviceId, timeElapsed, timeElapsedSec, nextQuestionId);
            }
            else
            {
                await UpdateAssessment(serviceId, timeElapsed, timeElapsedSec, nextQuestionId);
            }

        }

        public async Task SubmitSurveyAnswer(SurveyTopicViewModel model, bool isSubmit, string nextTopicId)
        {

            model.QuestionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SurveyQuestionViewModel>>(model.QuestionAnswerList);

            await _talentAssessmentQueryBusiness.UpdateSurveyResultAnswer(model);

            if (isSubmit)
            {
                ServiceTemplateViewModel serviceModel1 = new ServiceTemplateViewModel();
                serviceModel1.ActiveUserId = _userContext.UserId;
                serviceModel1.TemplateCode = "S_SURVEY_RESULT";
                serviceModel1.ServiceId = model.ServiceId;
                var service1 = await _serviceBusiness.GetServiceDetails(serviceModel1);

                service1.DataAction = DataActionEnum.Edit;
                service1.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                service1.AllowPastStartDate = true;
                await _serviceBusiness.ManageService(service1);

                var query1 = $@"Update cms.""N_SURVEY_S_SurveyResult"" set ""SurveyEndDate""='{DateTime.Now}'
                                where ""Id""='{service1.UdfNoteTableId}'";
                await _queryAssRes.ExecuteCommand(query1, null);


            }

            await UpdateSurvey(model.ServiceId, nextTopicId);


        }

        public async Task<bool> UpdateAssessment(string serviceId, int? timeElapsed, int? timeElapsedSec, string nextQuestionId = null, DateTime? startDate = null)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_RESULT";
            serviceModel.ServiceId = serviceId;
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            await _talentAssessmentQueryBusiness.UpdateAssessment(service, timeElapsed, timeElapsedSec, nextQuestionId, startDate);

            var result = await _talentAssessmentQueryBusiness.GetAssessmentStatus(serviceId);
            return result;

        }
        public async Task<bool> UpdateSurvey(string serviceId, string nextQTopicId = null, DateTime? startDate = null)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "S_SURVEY_RESULT";
            serviceModel.ServiceId = serviceId;
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            await _talentAssessmentQueryBusiness.UpdateSurvey(service, nextQTopicId);
            return true;

        }
        public async Task ManageAssessmentStatus(string serviceId, bool isAssessmentStopped)
        {
            var sermodel = await _serviceBusiness.GetSingle(x => x.ParentServiceId == serviceId);
            await _talentAssessmentQueryBusiness.ManageAssessmentStatus(isAssessmentStopped, sermodel);
        }

        public async Task<List<AssessmentInterviewViewModel>> GetAssessmentInterview(string UserID, bool isarchived = false, string source = null)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentInterview(UserID, isarchived, source);
            return result;


        }

        public async Task UpdateCandidateId(string userId, string fileId, string type)
        {
            await _talentAssessmentQueryBusiness.UpdateCandidateId(userId, fileId, type);
        }

        public async Task<AssessmentInterviewViewModel> GetInterviewService(string UserID)
        {
            var result = await _talentAssessmentQueryBusiness.GetInterviewService(UserID);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetInterviewAssessorList(string UserId)
        {
            var result = await _talentAssessmentQueryBusiness.GetInterviewAssessorList(UserId);
            return result;
        }


        public async Task<MemoryStream> GetInterViewQuestionExcel(string UserId, string TableId)



        {
            var model = await GetQuestionListofUser(UserId, TableId);
            int row = 1;
            var ms = new MemoryStream();
            var reportList = new List<string>() { "Sheet1" };
            var modelData = new AssessmentViewModel();
            using (var sl = new SLDocument())
            {
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);


                    sl.SetCellValue("A1", "NoteId");
                    sl.SetCellValue("B1", "SeraillNo");
                    sl.SetCellValue("C1", "Subject");
                    sl.SetCellValue("D1", "Description");
                    sl.SetCellValue("E1", "Level");
                    sl.SetCellValue("F1", "Indicatior");
                    sl.SetCellValue("G1", "Remarks");
                    sl.SetCellValue("H1", "Question");
                    sl.SetCellValue("I1", "CandidateAnswer");
                    sl.SetCellValue("J1", "Score");
                    sl.SetCellValue("K1", "InterviewerComment");

                    if (rep == "Sheet1")







                        row++;


                    if (model.IsNotNull())
                    {

                        foreach (var item in model)

                        {



                            //sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), item.NoteId);
                            sl.SetCellValue(string.Concat("B", row), item.SlNo);
                            sl.SetCellValue(string.Concat("C", row), item.NoteSubject);
                            sl.SetCellValue(string.Concat("D", row), item.NoteDescription);
                            sl.SetCellValue(string.Concat("E", row), item.ProficiencyLevel);
                            sl.SetCellValue(string.Concat("F", row), item.Indicator);
                            sl.SetCellValue(string.Concat("G", row), item.Remark);
                            sl.SetCellValue(string.Concat("H", row), item.Question);
                            sl.SetCellValue(string.Concat("I", row), item.CandidateAnswer);
                            sl.SetCellValue(string.Concat("J", row), item.Score);
                            sl.SetCellValue(string.Concat("K", row), item.InterviewerComment);






                            row++;
                        }
                    }










                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }



        public async Task<List<InterViewQuestionViewModel>> GetRandamQuestions()
        {
            var result = await _talentAssessmentQueryBusiness.GetRandamQuestions();
            return result;


        }


        public async Task<List<InterViewQuestionViewModel>> GetQuestionDetails(string UserId, string TableId)
        {
            var result = await _talentAssessmentQueryBusiness.GetQuestionDetails(UserId, TableId);
            return result;


        }


        public async Task<List<InterViewQuestionViewModel>> GetQuestionListofUser(string UserId, string TableId)
        {
            var result = await _talentAssessmentQueryBusiness.GetQuestionListofUser(UserId, TableId);
            return result;


        }

        public async Task<AssessmentReportViewModel> GetCandidateCaseStudyReport(string userId)
        {
            var model = new AssessmentReportViewModel();
            //var userModel =
            //if (userModel != null)
            //{
            //    model.OwnerUserName = userModel.UserName;
            //    model.Email = userModel.Email;
            //}

            //model.TechnicalAssessment = GetAssessmentReport(userId, "S_ASSESSMENT_QUISTIONNAIRE_RESULT");
            model.CaseStudyAssessment = await GetAssessmentReport(userId);

            if (model.CaseStudyAssessment != null && model.CaseStudyAssessment.AnswerList != null && model.CaseStudyAssessment.AnswerList.Count > 0)
            {
                model.CaseStudyText = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.Description;

            }
            return model;
        }
        public async Task<AssessmentDetailViewModel> GetAssessmentReport(string UserID)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentReport(UserID);

            if (result.IsNotNull())
            {
                result.AnswerList = await GetQuestionOptionList(result.Id, UserID);
            }
            return result;



        }
        public async Task<List<AssessmentDetailViewModel>> GetAssessmentReporTechnical(string UserID)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentReporTechnical(UserID);

            if (result.IsNotNull())
            {
                foreach (var item in result)
                {
                    item.AnswerList = await GetQuestionOptionList(item.Id, UserID);

                }


            }
            return result;



        }




        public async Task<List<AssessmentAnswerViewModel>> GetQuestionOptionList(string AssesmentId, string UserId)
        {



            var res = await _talentAssessmentQueryBusiness.GetQuestionOptionList(AssesmentId, UserId);


            Int32 row = 0, MaxNo = 0;
            foreach (var item in res)
            {

                row++;
                item.SeraillNo = row;
                var options = await GetOptionList(item.QuestionId);

                if (MaxNo < options.Count)
                {
                    MaxNo = Convert.ToInt32(options.Count);
                    item.MaxoptionNo = MaxNo;
                }
                if (options.IsNotNull())
                {
                    foreach (var optionitem in options)
                    {
                        if (optionitem.IsRightAnswer == true)
                        {
                            item.RightAnswer = optionitem.Option;
                            item.RightAnswerAr = optionitem.OptionArabic;

                        }


                        // switch (optionitem.SequenceOrder)
                        // {
                        //     case 1:
                        //         item.Option1 = optionitem.Option;
                        //         item.Option1Ar = optionitem.OptionArabic;
                        //         item.ScoreOption1 = Convert.ToString(optionitem.Score);
                        //         if (MaxNo < 1)
                        //         {
                        //             MaxNo = 1;
                        //         }
                        //         
                        //         break;
                        //     case 2:
                        //         item.Option2 = optionitem.Option;
                        //         item.Option2Ar = optionitem.OptionArabic;
                        //         item.ScoreOption2 = Convert.ToString(optionitem.Score);
                        //         if (MaxNo < 1)
                        //         {
                        //             MaxNo = 1;
                        //         }
                        //         break;
                        //     case 3:
                        //         item.Option3 = optionitem.Option;
                        //         item.Option3Ar = optionitem.OptionArabic;
                        //         item.ScoreOption3 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 4:
                        //         item.Option4 = optionitem.Option;
                        //         item.Option4Ar = optionitem.OptionArabic;
                        //         item.ScoreOption4 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 5:
                        //         item.Option5 = optionitem.Option;
                        //         item.ScoreOption5 = Convert.ToString(optionitem.Score);
                        //         break;
                        //
                        //     case 6:
                        //         item.Option6 = optionitem.Option;
                        //         item.ScoreOption6 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 7:
                        //         item.Option7= optionitem.Option;
                        //         item.ScoreOption7 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 8:
                        //         item.Option8 = optionitem.Option;
                        //         item.ScoreOption8 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 9:
                        //         item.Option9 = optionitem.Option;
                        //         item.ScoreOption9 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 10:
                        //         item.Option10 = optionitem.Option;
                        //         item.ScoreOption10 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     default:
                        //         break;
                        // }




                    }
                    if (item.RightAnswer.IsNotNull() && item.CandidateAnswer.IsNotNull())
                    {
                        if (item.RightAnswer == item.CandidateAnswer)
                        {
                            item.Accurate = BoolStatus.Yes;
                        }
                        else { item.Accurate = BoolStatus.No; }
                    }
                }

            }
            return res.OrderBy(x => x.SeraillNo).ToList();
        }



        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionList(string QuestionId)
        {

            var res = await _talentAssessmentQueryBusiness.GetOptionList(QuestionId);
            return res;


        }



        public async Task<MemoryStream> GetInterviewerCaseStudyDataExcel(AssessmentReportViewModel model)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>() { "Case Study Report" };
            var modelData = new AssessmentDetailViewModel();
            using (var sl = new SLDocument())
            {
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);
                    sl.SetColumnWidth("O", 20);
                    sl.SetColumnWidth("P", 20);
                    sl.SetColumnWidth("Q", 20);
                    sl.SetColumnWidth("R", 20);
                    sl.SetColumnWidth("S", 20);

                    // sl.HideColumn("S");

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", rep + " - Interviewer");
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 
                    if (rep != "Case Study Data")
                    {
                        if (rep == "Case Study Report")
                            modelData = model.CaseStudyAssessment;

                        if (modelData.IsNotNull())
                        {
                            sl.SetCellStyle("A5", "R5", ExcelHelper.GetTopBorderStyle(sl));
                            sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                            sl.SetCellStyle("R5", "R7", ExcelHelper.GetRightBorderStyle(sl));
                            sl.SetCellStyle("A7", "R7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                            sl.SetCellStyle("A5", "R7", ExcelHelper.FillColorStyle(sl));

                            sl.MergeWorksheetCells("A5", "B5");
                            sl.SetCellValue("A5", "Name");
                            sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("A6", "B7");
                            sl.SetCellValue("A6", model.OwnerUserName);
                            sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C5", "C5");
                            sl.SetCellValue("C5", "Job Title");
                            sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C6", "C7");
                            sl.SetCellValue("C6", model.JobTitle);
                            sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D5", "E5");
                            sl.SetCellValue("D5", "Email ID");
                            sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D6", "E7");
                            sl.SetCellValue("D6", model.Email);
                            sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F5", "F5");
                            sl.SetCellValue("F5", "Assessment Name");
                            sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F6", "F7");
                            sl.SetCellValue("F6", modelData.Title);
                            sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G5", "G5");
                            sl.SetCellValue("G5", "Scheduled Start Date");
                            sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G6", "G7");
                            sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H5", "H5");
                            sl.SetCellValue("H5", "Scheduled End Date");
                            sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H6", "H7");
                            sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I5", "I5");
                            sl.SetCellValue("I5", "Actual Start Date");
                            sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I6", "I7");
                            sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J5", "J5");
                            sl.SetCellValue("J5", "Actual End Date");
                            sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J6", "J7");
                            sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            int row = 9;


                            sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), "NtsNoteId");
                            sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                            sl.SetCellValue(string.Concat("B", row), "Sl No");
                            sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                            sl.SetCellValue(string.Concat("C", row), "Question");
                            sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                            char first = 'C';

                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Right Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Accurate");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Score");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Interviewer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Score Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }


                            row++;
                            if (modelData.AnswerList != null)
                            {
                                foreach (var ans in modelData.AnswerList)
                                {
                                    sl.SetRowHeight(row, 30);
                                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                    sl.SetCellValue(string.Concat("A", row), ans.NtsNoteId);
                                    sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                    sl.SetCellValue(string.Concat("B", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                    sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                    sl.SetCellValue(string.Concat("C", row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                    sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    var options = await GetOptionList(ans.QuestionId);
                                    options = options.OrderBy(x => x.SequenceOrder).ToList();
                                    first = 'C';
                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }
                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }



                                    first = (char)((int)first + 1);

                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.RightAnswerAr : ans.RightAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    first = (char)((int)first + 1);

                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.Accurate.ToString());
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.Score.IsNotNull() ? ans.Score.ToString() : "");
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.InterviewerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), "");
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));






                                    if (ans.AnswerId.HasValue)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), ans.AnswerId.Value);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }


                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), item.Score);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }


                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }


                                    //    sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                                    //    sl.SetCellValue(string.Concat("T", row), ans.ScoreOption1);
                                    //    sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                                    //    sl.SetCellValue(string.Concat("U", row), ans.ScoreOption2);
                                    //    sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                                    //    sl.SetCellValue(string.Concat("V", row), ans.ScoreOption3);
                                    //    sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                                    //    sl.SetCellValue(string.Concat("W", row), ans.ScoreOption4);
                                    //    sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                                    //    sl.SetCellValue(string.Concat("X", row), ans.ScoreOption5);
                                    //    sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                                    //    sl.SetCellValue(string.Concat("Y", row), ans.ScoreOption6);
                                    //    sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                                    //    sl.SetCellValue(string.Concat("Z", row), ans.ScoreOption7);
                                    //    sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("AA", row), string.Concat("AA", row));
                                    //    sl.SetCellValue(string.Concat("AA", row), ans.ScoreOption8);
                                    //    sl.SetCellStyle(string.Concat("AA", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("AB", row), string.Concat("AB", row));
                                    //    sl.SetCellValue(string.Concat("AB", row), ans.ScoreOption9);
                                    //    sl.SetCellStyle(string.Concat("AB", row), string.Concat("AB", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("AC", row), string.Concat("AC", row));
                                    //    sl.SetCellValue(string.Concat("AC", row), ans.ScoreOption10);
                                    //    sl.SetCellStyle(string.Concat("AC", row), string.Concat("AC", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    row++;
                                }
                            }

                        }

                    }
                    else
                    {
                        sl.MergeWorksheetCells(string.Concat("A", 5), string.Concat("J", 30));
                        sl.SetCellValue(string.Concat("A", 5), model.CaseStudyText);
                        sl.SetCellStyle(string.Concat("A", 5), string.Concat("J", 30), ExcelHelper.GetTextCaseStudyStyle(sl));

                        sl.SetCellStyle("A5", "J5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A30", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("J5", "J30", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A30", "J30", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }



        public async Task<MemoryStream> GetCandidateCaseStudyDataExcel(AssessmentReportViewModel model)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>() { "Case Study Data", "Case Study Report" };
            var modelData = new AssessmentDetailViewModel();
            using (var sl = new SLDocument())
            {
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", rep + " - Candidate");
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 
                    if (rep != "Case Study Data")
                    {
                        if (rep == "Case Study Report")
                            modelData = model.CaseStudyAssessment;

                        if (modelData.IsNotNull())
                        {
                            sl.SetCellStyle("A5", "N5", ExcelHelper.GetTopBorderStyle(sl));
                            sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                            sl.SetCellStyle("N5", "N7", ExcelHelper.GetRightBorderStyle(sl));
                            sl.SetCellStyle("A7", "N7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                            sl.SetCellStyle("A5", "N7", ExcelHelper.FillColorStyle(sl));

                            sl.MergeWorksheetCells("A5", "B5");
                            sl.SetCellValue("A5", "Name");
                            sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("A6", "B7");
                            sl.SetCellValue("A6", model.OwnerUserName);
                            sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C5", "C5");
                            sl.SetCellValue("C5", "Job Title");
                            sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C6", "C7");
                            sl.SetCellValue("C6", model.JobTitle);
                            sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D5", "E5");
                            sl.SetCellValue("D5", "Email ID");
                            sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D6", "E7");
                            sl.SetCellValue("D6", model.Email);
                            sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F5", "F5");
                            sl.SetCellValue("F5", "Assessment Name");
                            sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F6", "F7");
                            sl.SetCellValue("F6", modelData.Title);
                            sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G5", "G5");
                            sl.SetCellValue("G5", "Scheduled Start Date");
                            sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G6", "G7");
                            sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H5", "H5");
                            sl.SetCellValue("H5", "Scheduled End Date");
                            sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H6", "H7");
                            sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I5", "I5");
                            sl.SetCellValue("I5", "Actual Start Date");
                            sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I6", "I7");
                            sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J5", "J5");
                            sl.SetCellValue("J5", "Actual End Date");
                            sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J6", "J7");
                            sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));


                            int row = 9;

                            sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), "Sl No");
                            sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                            sl.SetCellValue(string.Concat("B", row), "Question");
                            sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                            char first = 'B';

                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            }

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("M", row), string.Concat("M", row));
                            //sl.SetCellValue(string.Concat("M", row), "Candidate Answer");
                            //sl.SetCellStyle(string.Concat("M", row), string.Concat("M", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("N", row), string.Concat("N", row));
                            //sl.SetCellValue(string.Concat("N", row), "Answer Comment");
                            //sl.SetCellStyle(string.Concat("N", row), string.Concat("N", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                            //sl.SetCellValue(string.Concat("C", row), "Option 1");
                            //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                            //sl.SetCellValue(string.Concat("D", row), "Option 2");
                            //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                            //sl.SetCellValue(string.Concat("E", row), "Option 3");
                            //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                            //sl.SetCellValue(string.Concat("F", row), "Option 4");
                            //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                            //sl.SetCellValue(string.Concat("G", row), "Option 5");
                            //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                            //sl.SetCellValue(string.Concat("H", row), "Option 6");
                            //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                            //sl.SetCellValue(string.Concat("I", row), "Option 7");
                            //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                            //sl.SetCellValue(string.Concat("J", row), "Option 8");
                            //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                            //sl.SetCellValue(string.Concat("K", row), "Option 9");
                            //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                            //sl.SetCellValue(string.Concat("L", row), "Option 10");
                            //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                            //sl.SetCellValue(string.Concat("I", row), "");
                            //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                            //sl.SetCellValue(string.Concat("J", row), "");
                            //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            row++;
                            if (modelData.AnswerList != null)
                            {
                                foreach (var ans in modelData.AnswerList)
                                {
                                    sl.SetRowHeight(row, 30);
                                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                    sl.SetCellValue(string.Concat("A", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                    sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                    sl.SetCellValue(string.Concat("B", row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                    sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    var options = await GetOptionList(ans.QuestionId);
                                    options = options.OrderBy(x => x.SequenceOrder).ToList();
                                    first = 'B';
                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }
                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                    //sl.SetCellValue(string.Concat("C", row), ans.Option1);
                                    //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                    //sl.SetCellValue(string.Concat("D", row), ans.Option2);
                                    //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                    //sl.SetCellValue(string.Concat("E", row), ans.Option3);
                                    //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                                    //sl.SetCellValue(string.Concat("F", row), ans.Option4);
                                    //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                                    //sl.SetCellValue(string.Concat("G", row), ans.Option5);
                                    //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                                    //sl.SetCellValue(string.Concat("H", row), ans.Option6);
                                    //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                                    //sl.SetCellValue(string.Concat("I", row), ans.Option7);
                                    //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                                    //sl.SetCellValue(string.Concat("J", row), ans.Option8);
                                    //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                                    //sl.SetCellValue(string.Concat("K", row), ans.Option9);
                                    //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                                    //sl.SetCellValue(string.Concat("L", row), ans.Option10);
                                    //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    row++;
                                }
                            }

                        }

                    }
                    else
                    {
                        sl.MergeWorksheetCells(string.Concat("A", 5), string.Concat("J", 30));
                        sl.SetCellValue(string.Concat("A", 5), model.CaseStudyText);
                        sl.SetCellStyle(string.Concat("A", 5), string.Concat("J", 30), ExcelHelper.GetTextCaseStudyStyle(sl));

                        sl.SetCellStyle("A5", "J5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A30", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("J5", "J30", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A30", "J30", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetAssessmentReportDataExcel(AssessmentReportViewModel model)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();
            //{ "Assessment Report", "Case Study Data", "Case Study Report" };

            reportList.Add("Case Study Data");
            reportList.Add("Case Study Report");

            using (var sl = new SLDocument())
            {
                foreach (var modelData in model.TechnicalAssessment)
                {
                    var Title = modelData.Title;
                    if (Title.IsNotNull())
                    {
                        if (modelData.Title.Length > 30)
                            Title = modelData.Title.Substring(0, 29);
                    }

                    sl.AddWorksheet(Title);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);
                    sl.SetColumnWidth("O", 20);
                    sl.SetColumnWidth("P", 20);
                    sl.SetColumnWidth("R", 20);

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", modelData.Title);
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 


                    if (modelData.IsNotNull())
                    {
                        sl.SetCellStyle("A5", "N5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("N5", "N7", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A7", "N7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                        sl.SetCellStyle("A5", "N7", ExcelHelper.FillColorStyle(sl));

                        sl.MergeWorksheetCells("A5", "B5");
                        sl.SetCellValue("A5", "Name");
                        sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("A6", "B7");
                        sl.SetCellValue("A6", model.OwnerUserName);
                        sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("C5", "C5");
                        sl.SetCellValue("C5", "Job Title");
                        sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("C6", "C7");
                        sl.SetCellValue("C6", model.JobTitle);
                        sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("D5", "E5");
                        sl.SetCellValue("D5", "Email ID");
                        sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("D6", "E7");
                        sl.SetCellValue("D6", model.Email);
                        sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("F5", "F5");
                        sl.SetCellValue("F5", "Assessment Name");
                        sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("F6", "F7");
                        sl.SetCellValue("F6", modelData.Subject);
                        sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("G5", "G5");
                        sl.SetCellValue("G5", "Scheduled Start Date");
                        sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("G6", "G7");
                        sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("H5", "H5");
                        sl.SetCellValue("H5", "Scheduled End Date");
                        sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("H6", "H7");
                        sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("I5", "I5");
                        sl.SetCellValue("I5", "Actual Start Date");
                        sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("I6", "I7");
                        sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("J5", "J5");
                        sl.SetCellValue("J5", "Actual End Date");
                        sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("J6", "J7");
                        sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("K5", "N7");
                        sl.SetCellStyle("K5", "N7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        int row = 9;

                        sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                        sl.SetCellValue(string.Concat("A", row), "Sl No");
                        sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                        sl.SetCellValue(string.Concat("B", row), "Question");
                        sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));



                        Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                        char first = 'B';

                        for (int i = 1; i <= maxNo; i++)
                        {
                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Option" + i);
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        }



                        //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                        //sl.SetCellValue(string.Concat("C", row), "Option 1");
                        //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                        //sl.SetCellValue(string.Concat("D", row), "Option 2");
                        //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                        //sl.SetCellValue(string.Concat("E", row), "Option 3");
                        //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                        //sl.SetCellValue(string.Concat("F", row), "Option 4");
                        //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                        //sl.SetCellValue(string.Concat("G", row), "Option 5");
                        //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                        //sl.SetCellValue(string.Concat("H", row), "Option 6");
                        //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                        //sl.SetCellValue(string.Concat("I", row), "Option 7");
                        //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                        //sl.SetCellValue(string.Concat("J", row), "Option 8");
                        //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                        //sl.SetCellValue(string.Concat("K", row), "Option 9");
                        //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                        //sl.SetCellValue(string.Concat("L", row), "Option 10");
                        //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Right Answer");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Accurate");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        //first = (char)((int)first + 1);
                        //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        //sl.SetCellValue(string.Concat(first, row), "");
                        //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));




                        for (int i = 1; i <= maxNo; i++)
                        {
                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Score Option" + i);
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        }




                        //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                        //sl.SetCellValue(string.Concat("Q", row), "Score Option 1");
                        //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                        //sl.SetCellValue(string.Concat("R", row), "Score Option 2");
                        //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                        //sl.SetCellValue(string.Concat("S", row), "Score Option 3");
                        //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                        //sl.SetCellValue(string.Concat("T", row), "Score Option 4");
                        //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                        //sl.SetCellValue(string.Concat("U", row), "Score Option 5");
                        //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                        //sl.SetCellValue(string.Concat("V", row), "Score Option 6");
                        //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                        //sl.SetCellValue(string.Concat("W", row), "Score Option 7");
                        //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //
                        //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                        //sl.SetCellValue(string.Concat("X", row), "Score Option 8");
                        //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                        //sl.SetCellValue(string.Concat("Y", row), "Score Option 9");
                        //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                        //sl.SetCellValue(string.Concat("Z", row), "Score Option 10");
                        //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        row++;
                        if (modelData.AnswerList != null)
                        {
                            foreach (var ans in modelData.AnswerList)
                            {
                                sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                sl.SetCellValue(string.Concat("A", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                sl.SetCellValue(string.Concat("B", row), modelData.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                var options = await GetOptionList(ans.QuestionId);
                                options = options.OrderBy(x => x.SequenceOrder).ToList();
                                first = 'B';
                                foreach (var item in options)
                                {
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                }

                                if (maxNo > options.Count)
                                {
                                    Int32 k = maxNo - options.Count;

                                    for (int i = 0; i < k; i++)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), "");
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }
                                }

                                // sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                // sl.SetCellValue(string.Concat("C", row), ans.Option1);
                                // sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                // sl.SetCellValue(string.Concat("D", row), ans.Option2);
                                // sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                // sl.SetCellValue(string.Concat("E", row), ans.Option3);
                                // sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                                // sl.SetCellValue(string.Concat("F", row), ans.Option4);
                                // sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                                // sl.SetCellValue(string.Concat("G", row), ans.Option5);
                                // sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                                // sl.SetCellValue(string.Concat("H", row), ans.Option6);
                                // sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                                // sl.SetCellValue(string.Concat("I", row), ans.Option7);
                                // sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                                // sl.SetCellValue(string.Concat("J", row), ans.Option8);
                                // sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                                // sl.SetCellValue(string.Concat("K", row), ans.Option9);
                                // sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                                // sl.SetCellValue(string.Concat("L", row), ans.Option10);
                                // sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                first = (char)((int)first + 1);

                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.RightAnswerAr : ans.RightAnswer);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), ans.Accurate.ToString());
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                //first = (char)((int)first + 1);
                                //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                //sl.SetCellValue(string.Concat(first, row), "");
                                //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                foreach (var item in options)
                                {
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), item.Score);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                }


                                //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                                //sl.SetCellValue(string.Concat("Q", row), ans.ScoreOption1);
                                //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                                //sl.SetCellValue(string.Concat("R", row), ans.ScoreOption2);
                                //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                                //sl.SetCellValue(string.Concat("S", row), ans.ScoreOption3);
                                //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                                //sl.SetCellValue(string.Concat("T", row), ans.ScoreOption4);
                                //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                                //sl.SetCellValue(string.Concat("U", row), ans.ScoreOption5);
                                //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                                //sl.SetCellValue(string.Concat("V", row), ans.ScoreOption6);
                                //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                                //sl.SetCellValue(string.Concat("W", row), ans.ScoreOption7);
                                //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                                //sl.SetCellValue(string.Concat("X", row), ans.ScoreOption8);
                                //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                                //sl.SetCellValue(string.Concat("Y", row), ans.ScoreOption9);
                                //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                                //sl.SetCellValue(string.Concat("Z", row), ans.ScoreOption10);
                                //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));



                                row++;
                            }
                        }

                    }



                }
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);
                    var modelData = new AssessmentDetailViewModel();
                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);
                    sl.SetColumnWidth("O", 20);
                    sl.SetColumnWidth("P", 20);

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", rep);
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 
                    if (rep != "Case Study Data")
                    {
                        //if (rep == "Assessment Report")
                        //    modelData = model.TechnicalAssessment;
                        //else 
                        if (rep == "Case Study Report")
                            modelData = model.CaseStudyAssessment;

                        if (modelData.IsNotNull())
                        {
                            sl.SetCellStyle("A5", "N5", ExcelHelper.GetTopBorderStyle(sl));
                            sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                            sl.SetCellStyle("N5", "N7", ExcelHelper.GetRightBorderStyle(sl));
                            sl.SetCellStyle("A7", "N7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                            sl.SetCellStyle("A5", "N7", ExcelHelper.FillColorStyle(sl));

                            sl.MergeWorksheetCells("A5", "B5");
                            sl.SetCellValue("A5", "Name");
                            sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("A6", "B7");
                            sl.SetCellValue("A6", model.OwnerUserName);
                            sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C5", "C5");
                            sl.SetCellValue("C5", "Job Title");
                            sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C6", "C7");
                            sl.SetCellValue("C6", model.JobTitle);
                            sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D5", "E5");
                            sl.SetCellValue("D5", "Email ID");
                            sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D6", "E7");
                            sl.SetCellValue("D6", model.Email);
                            sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F5", "F5");
                            sl.SetCellValue("F5", "Assessment Name");
                            sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F6", "F7");
                            sl.SetCellValue("F6", modelData.Subject);
                            sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G5", "G5");
                            sl.SetCellValue("G5", "Scheduled Start Date");
                            sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G6", "G7");
                            sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H5", "H5");
                            sl.SetCellValue("H5", "Scheduled End Date");
                            sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H6", "H7");
                            sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I5", "I5");
                            sl.SetCellValue("I5", "Actual Start Date");
                            sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I6", "I7");
                            sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J5", "J5");
                            sl.SetCellValue("J5", "Actual End Date");
                            sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J6", "J7");
                            sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("K6", "N7");
                            sl.SetCellStyle("K6", "N7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            int row = 9;

                            sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), "Sl No");
                            sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                            sl.SetCellValue(string.Concat("B", row), "Question");
                            sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                            char first = 'B';

                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }

                            //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                            //sl.SetCellValue(string.Concat("C", row), "Option 1");
                            //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                            //sl.SetCellValue(string.Concat("D", row), "Option 2");
                            //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                            //sl.SetCellValue(string.Concat("E", row), "Option 3");
                            //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                            //sl.SetCellValue(string.Concat("F", row), "Option 4");
                            //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                            //sl.SetCellValue(string.Concat("G", row), "Option 5");
                            //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                            //sl.SetCellValue(string.Concat("H", row), "Option 6");
                            //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                            //sl.SetCellValue(string.Concat("I", row), "Option 7");
                            //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                            //sl.SetCellValue(string.Concat("J", row), "Option 8");
                            //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                            //sl.SetCellValue(string.Concat("K", row), "Option 9");
                            //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                            //sl.SetCellValue(string.Concat("L", row), "Option 10");
                            //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Right Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            //first = (char)((int)first + 1);
                            //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            //sl.SetCellValue(string.Concat(first, row), "");
                            //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Accurate");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Score Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }

                            //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                            //sl.SetCellValue(string.Concat("Q", row), "Score Option 1");
                            //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                            //sl.SetCellValue(string.Concat("R", row), "Score Option 2");
                            //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                            //sl.SetCellValue(string.Concat("S", row), "Score Option 3");
                            //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                            //sl.SetCellValue(string.Concat("T", row), "Score Option 4");
                            //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                            //sl.SetCellValue(string.Concat("U", row), "Score Option 5");
                            //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                            //sl.SetCellValue(string.Concat("V", row), "Score Option 6");
                            //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                            //sl.SetCellValue(string.Concat("W", row), "Score Option 7");
                            //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //
                            //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                            //sl.SetCellValue(string.Concat("X", row), "Score Option 8");
                            //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                            //sl.SetCellValue(string.Concat("Y", row), "Score Option 9");
                            //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                            //sl.SetCellValue(string.Concat("Z", row), "Score Option 10");
                            //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            row++;
                            if (modelData.AnswerList != null)
                            {
                                foreach (var ans in modelData.AnswerList)
                                {
                                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                    sl.SetCellValue(string.Concat("A", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                    sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                    sl.SetCellValue(string.Concat("B", row), modelData.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                    sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));



                                    var options = await GetOptionList(ans.QuestionId);
                                    options = options.OrderBy(x => x.SequenceOrder).ToList();
                                    first = 'B';
                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }

                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }
                                    //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                    //sl.SetCellValue(string.Concat("C", row), ans.Option1);
                                    //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                    //sl.SetCellValue(string.Concat("D", row), ans.Option2);
                                    //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                    //sl.SetCellValue(string.Concat("E", row), ans.Option3);
                                    //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                                    //sl.SetCellValue(string.Concat("F", row), ans.Option4);
                                    //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                                    //sl.SetCellValue(string.Concat("G", row), ans.Option5);
                                    //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                                    //sl.SetCellValue(string.Concat("H", row), ans.Option6);
                                    //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                                    //sl.SetCellValue(string.Concat("I", row), ans.Option7);
                                    //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                                    //sl.SetCellValue(string.Concat("J", row), ans.Option8);
                                    //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                                    //sl.SetCellValue(string.Concat("K", row), ans.Option9);
                                    //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                                    //sl.SetCellValue(string.Concat("L", row), ans.Option10);
                                    //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.RightAnswerAr : ans.RightAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    //first = (char)((int)first + 1);
                                    //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    //sl.SetCellValue(string.Concat(first, row),"");
                                    //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.Accurate.ToString());
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));



                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), item.Score);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }

                                    //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                                    //sl.SetCellValue(string.Concat("Q", row), ans.ScoreOption1);
                                    //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                                    //sl.SetCellValue(string.Concat("R", row), ans.ScoreOption2);
                                    //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                                    //sl.SetCellValue(string.Concat("S", row), ans.ScoreOption3);
                                    //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                                    //sl.SetCellValue(string.Concat("T", row), ans.ScoreOption4);
                                    //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                                    //sl.SetCellValue(string.Concat("U", row), ans.ScoreOption5);
                                    //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                                    //sl.SetCellValue(string.Concat("V", row), ans.ScoreOption6);
                                    //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                                    //sl.SetCellValue(string.Concat("W", row), ans.ScoreOption7);
                                    //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                                    //sl.SetCellValue(string.Concat("X", row), ans.ScoreOption8);
                                    //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                                    //sl.SetCellValue(string.Concat("Y", row), ans.ScoreOption9);
                                    //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                                    //sl.SetCellValue(string.Concat("Z", row), ans.ScoreOption10);
                                    //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    row++;
                                }
                            }

                        }

                    }
                    else
                    {
                        if (model.FileId.IsNotNullAndNotEmpty())
                        {
                            //var img = _fileBusiness.GetSingleById(model.FileId.Value);
                            //if (img != null)
                            //{
                            var bytes = await _fileBusiness.GetFileByte(model.FileId);
                            if (bytes.Count() > 0)
                            {
                                SLPicture pic = new SLPicture(bytes, DocumentFormat.OpenXml.Packaging.ImagePartType.Jpeg);
                                pic.SetPosition(3, 0);
                                pic.LockWithSheet = true;
                                pic.ResizeInPixels(800, 400);
                                sl.InsertPicture(pic);
                                sl.SetRowHeight(4, 250);
                                sl.MergeWorksheetCells(string.Concat("A", 4), string.Concat("J", 4));
                            }
                            //}
                        }

                        sl.MergeWorksheetCells(string.Concat("A", 5), string.Concat("J", 30));
                        sl.SetCellValue(string.Concat("A", 5), model.CaseStudyText);
                        sl.SetCellStyle(string.Concat("A", 5), string.Concat("J", 30), ExcelHelper.GetTextCaseStudyStyle(sl));

                        sl.SetCellStyle("A5", "J5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A30", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("J5", "J30", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A30", "J30", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }

        public async Task<AssessmentReportViewModel> GetAssessmentReportexcel(string userId)
        {
            var model = new AssessmentReportViewModel();


            model.TechnicalAssessment = await GetAssessmentReporTechnical(userId);
            model.CaseStudyAssessment = await GetAssessmentReport(userId);

            if (model.CaseStudyAssessment != null && model.CaseStudyAssessment.AnswerList != null && model.CaseStudyAssessment.AnswerList.Count > 0)
            {
                model.CaseStudyText = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.Description;
                if (model.CaseStudyAssessment.PreferredLanguageCode == "ENGLISH")
                {
                    model.FileId = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.FileId;
                }
                else
                {
                    model.FileId = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.FileIdAr;
                    if (model.FileId.IsNullOrEmpty())
                    {
                        model.FileId = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.FileId;
                    }
                }


            }
            return model;
        }


        public async Task ChangeUserProfilePhoto(string photoId, string userId)
        {
            await _talentAssessmentQueryBusiness.ChangeUserProfilePhoto(photoId, userId);
        }



        public async Task<AssessmentInterviewViewModel> GetserviceScore(string ServiceID)
        {
            var result = await _talentAssessmentQueryBusiness.GetserviceScore(ServiceID);
            return result;
        }

        public async Task<List<InterViewQuestionViewModel>> GetInterviewAssessmentQuestions(string userId, string Tableid)
        {
            var result = await _talentAssessmentQueryBusiness.GetInterviewAssessmentQuestions(userId, Tableid);
            return result;
        }



        public async Task<bool> CalculateQuestionaireResultScore(string UserId)
        {
            var questionaireResultServices = await GetAssessmentQuistionNaireResultByUser(UserId);
            var questionLists = await GetAssessmentReporTechnical(UserId);
            foreach (var questionaireResultService in questionaireResultServices)
            {

                //var technicalScore = questionaireResultService.Score;
                //if (technicalScore != null)
                //{
                double technicalScore = 0.0;
                double totalQuestions = 0.0;
                double correctAnswer = 0.0;
                var score = "0.0";
                var questionList = questionLists.Where(x => x.Id == questionaireResultService.Id).FirstOrDefault();
                if (questionList != null)
                {

                    totalQuestions = questionList.AnswerList.Count();
                    var maximumoption = questionList.MaximumOption ?? 3;
                    var rightAnsCount = questionList.AnswerList.Where(e => e.RightAnswer.IsNotNullAndNotEmpty()).Count();
                    if (rightAnsCount <= 0)
                    {
                        if (totalQuestions > 0)
                        {
                            var optionscoretotal = questionList.AnswerList.Sum(e => e.ScoreOption ?? 0);

                            score = ((optionscoretotal / (totalQuestions * maximumoption)) * 100).RoundPayrollSummaryAmount().ToString();
                            technicalScore = Convert.ToDouble(score);
                        }
                        else
                        {
                            technicalScore = 0.0;
                        }
                    }
                    else
                    {
                        if (totalQuestions > 0)
                        {
                            correctAnswer = questionList.AnswerList.Where(e => e.Accurate == BoolStatus.Yes).Count();

                            score = ((correctAnswer / totalQuestions) * 100).RoundPayrollSummaryAmount().ToString();
                            technicalScore = Convert.ToDouble(score);
                        }
                        else
                        {
                            technicalScore = 0.0;
                        }
                    }


                    await UpdateUserResult(questionaireResultService.ServiceId, technicalScore.ToString());
                }

            }

            // }

            return true;
        }


        public async Task<bool> CalculateCaseStudyResultScore(string userId)
        {
            var caseStudyResultService = await GetAssessmentCaseStudyResultByUser(userId);
            if (caseStudyResultService != null)
            {

                double caseStudyScore = 0.0;

                double totalCaseStudyQuestions = 0.0;
                double correctCaseStudyAnswer = 0.0;
                var score = "0.0";
                var caseStudyQuestionList = await GetAssessmentReport(userId);
                if (caseStudyQuestionList != null)
                {
                    totalCaseStudyQuestions = caseStudyQuestionList.AnswerList.Count();
                    if (totalCaseStudyQuestions > 0)
                    {
                        foreach (var item in caseStudyQuestionList.AnswerList)
                        {
                            if (item.Accurate == BoolStatus.Yes && item.Score.IsNotNull())
                            {
                                correctCaseStudyAnswer = correctCaseStudyAnswer + item.Score.Value;
                            }
                        }
                        score = ((correctCaseStudyAnswer / (totalCaseStudyQuestions * 4)) * 100).RoundPayrollSummaryAmount().ToString();
                        caseStudyScore = Convert.ToDouble(score);
                    }
                    else
                    {
                        caseStudyScore = 0.0;
                    }

                }
                else
                {
                    caseStudyScore = 0.0;
                }

                await UpdateUserResult(caseStudyResultService.ServiceId, caseStudyScore.ToString());
                //var cypher = @"match (s:NTS_Service{Id:{serviceId},IsDeleted:0}) 
                // optional match(s)<-[:R_ServiceFieldValue_Service]-(nfv1:NTS_ServiceFieldValue{IsDeleted:0})-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'score'})

                //set nfv1.Code={code},nfv1.Value={value} return 1";
                //var prms = new Dictionary<string, object>();
                //prms.Add("Status", StatusEnum.Active.ToString());
                //prms.Add("CompanyId", CompanyId);
                //prms.Add("serviceId", caseStudyResultService.Id);
                //prms.Add("code", score);
                //prms.Add("value", score);

                //ExecuteCypherList<long>(cypher, prms);

            }

            return true;
        }

        public async Task<List<AssessmentDetailViewModel>> GetAssessmentQuistionNaireResultByUser(string UserID)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentQuistionNaireResultByUser(UserID);
            return result;

        }


        public async Task<AssessmentDetailViewModel> GetAssessmentCaseStudyResultByUser(string UserID)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentCaseStudyResultByUser(UserID);
            return result;

        }


        public async Task<bool> UpdateUserResult(string Id, string Scores)
        {
            await _talentAssessmentQueryBusiness.UpdateUserResult(Id, Scores);

            return true;

        }

        public async Task<bool> UpdateInterViewScore(string Id, string Scores, string ActualStartDate, string ActualEndDate)
        {
            await _talentAssessmentQueryBusiness.UpdateInterViewScore(Id, Scores, ActualStartDate, ActualEndDate);


            return true;

        }

        public async Task<IdNameViewModel> GetCanidateId(string UserId)
        {


            var result = await _talentAssessmentQueryBusiness.GetCanidateId(UserId);
            return result;
        }

        public async Task<bool> UpdateAssessmentArchived(string serviceIds, bool archive = false)
        {
            await _talentAssessmentQueryBusiness.UpdateAssessmentArchived(serviceIds, archive);
            return true;
        }


        public async Task<IdNameViewModel> Getinterviewid(string ServiceId)
        {

            var res = await _talentAssessmentQueryBusiness.Getinterviewid(ServiceId);
            return res;

        }

        public async Task<List<AssessmentCalendarViewModel>> GetCalendarScheduleList(string loggedInUserId = null, string role = null)
        {
            var result = await _talentAssessmentQueryBusiness.GetCalendarScheduleList(loggedInUserId, role);
            return result;
        }
        public async Task<AssessmentCalendarViewModel> GetCalendarScheduleById(string id)
        {

            var result = await _talentAssessmentQueryBusiness.GetCalendarScheduleById(id);
            return result;
        }
        public async Task<List<EmailViewModel>> GetSlotUserEmail(string slotId)
        {

            var result = await _talentAssessmentQueryBusiness.GetSlotUserEmail(slotId);
            return result.ToList();
        }
        public async Task<EmailViewModel> GetEmailById(string emailId)
        {

            var result = await _talentAssessmentQueryBusiness.GetEmailById(emailId);
            return result;
        }
        public async Task DeleteAssessmentSchedule(string id)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var _serviceBusiness = _sp.GetService<IServiceBusiness>();

            var res = await _tableMetadataBusiness.GetTableDataByColumn("TAS_USER_ASSESSMENT_SCHEDULE", "", "Id", id);
            if (res != null)
            {
                await _talentAssessmentQueryBusiness.DeleteAssessmentSchedule(id);

                await _noteBusiness.Delete(res["NtsNoteId"].ToString());
                var ser = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == id);
                await _serviceBusiness.Delete(ser.Id);
            }
        }
        public async Task<AssessmentPDFReportViewModel> GetAssessmentReportDataPDFForUser(string userId)
        {
            //var _userbusiness = BusinessHelper.GetInstance<IUserBusiness>();
            AssessmentPDFReportViewModel model = new AssessmentPDFReportViewModel();
            //var TechnicalAssessment = await GetAssessmentReportByUserId(userId, "S_ASSESSMENT_QUISTIONNAIRE_RESULT");
            //var CaseStudyAssessment = await GetAssessmentReportByUserId(userId, "S_ASSESSMENT_CASESTUDY_RESULT");
            var TechnicalAssessment = await GetAssessmentReportByUserId(userId, "TECHNICAL");
            var CaseStudyAssessment = await GetAssessmentReportByUserId(userId, "CASE_STUDY");
            var InterviewAssessment = await GetInterviewAssessmentReportByUserId(userId);
            model.CaseAnalysisScore = 0.0;
            if (userId.IsNotNullAndNotEmpty())
            {
                var user = await _userBusiness.GetSingleById(userId);
                if (user != null)
                {
                    model.UserName = user.Name;
                    model.JobName = user.JobTitle;
                }
            }
            if (TechnicalAssessment != null)
            {
                model.JobName = TechnicalAssessment.JobTitle;
                //model.UserName = TechnicalAssessment.OwnerUserName;
                model.TechnicalQuestionCount = TechnicalAssessment.TotalCount.ToString();
                model.TechnicalQuestionnaireScore = TechnicalAssessment.Score ?? 0.0;
                model.AssessmentTechnicalDuratrion = TechnicalAssessment.AssessmentDuration;
                model.TechnicalWeightage = TechnicalAssessment.Weightage;
            }
            if (CaseStudyAssessment != null)
            {
                model.CaseStudyQuestionCount = CaseStudyAssessment.TotalCount.ToString();
                model.CaseStudyWeightage = CaseStudyAssessment.Weightage;
                model.CaseAnalysisScore = CaseStudyAssessment.Score ?? 0.0;
                model.AssessmentCaseStudyDuratrion = CaseStudyAssessment.AssessmentDuration;
            }
            if (InterviewAssessment != null)
            {
                model.TechnicalInterviewScore = InterviewAssessment.Score ?? 0.0;
                model.InterviewWeightage = InterviewAssessment.AssessmentInterviewWeightage;
                model.AssessmentInterviewDuratrion = InterviewAssessment.AssessmentInterviewDuratrion;
            }
            return model;
        }
        public async Task<AssessmentViewModel> GetAssessmentReportByUserId(string userId, string masterCode)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentReportByUserId(userId, masterCode);
            return result;
        }
        public async Task<AssessmentDetailViewModel> GetInterviewAssessmentReportByUserId(string userId)
        {
            var result = await _talentAssessmentQueryBusiness.GetInterviewAssessmentReportByUserId(userId);
            return result;

        }
        public async Task<AssessmentResultViewModel> GetManageAssessmentResult(string userId)
        {
            var data = await _talentAssessmentQueryBusiness.GetManageAssessmentResult(userId);

            if (data != null)
            {
                data.Type = "";
                var assessmentlist = await GetManageAssessmentResultList(userId);
                var assessments = assessmentlist.Where(x => (x.IncludeInReport == null || x.IncludeInReport == true)).ToList();
                if (assessments.Count > 0)
                {
                    var a = assessments.FirstOrDefault();
                    if (a.ReportSummary.IsNotNullAndNotEmpty())
                    {
                        data.ReportSummary = a.ReportSummary;
                    }
                    data.Type = a.Type;
                }
                //if (data.ReportSummary.IsNullOrEmpty())
                //{
                //    data.ReportSummary = assessmentReportSummary;
                //}
                //data.ReportColor = reportColor;
                data.Score = (assessments.Sum(x => x.ReportScore * (x.ReportWeightage / 100.0)) ?? 0).RoundToTwoDecimal();
            }
            //var data = new AssessmentResultViewModel();
            return data;
        }
        public async Task<List<AssessmentResultViewModel>> GetManageAssessmentResultList(string userId)
        {
            var assessment = await _talentAssessmentQueryBusiness.GetManageAssessmentResultList(userId);
            return assessment.ToList();
        }


        public async Task<List<AssessmentViewModel>> GetAssessmentSetList()
        {


            var Result = await _talentAssessmentQueryBusiness.GetAssessmentSetList();
            return Result;

        }

        public async Task<List<AssessmentViewModel>> GetAssessmentSetAssessmentList(string AssessmentSetId)
        {
            var Result = await _talentAssessmentQueryBusiness.GetAssessmentSetAssessmentList(AssessmentSetId);
            return Result;

        }

        public async Task<bool> DeleteAssessmentSet(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                await _talentAssessmentQueryBusiness.DeleteAssessmentSet(NoteId);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAssessmentSetAsessment(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                await _talentAssessmentQueryBusiness.DeleteAssessmentSetAsessment(NoteId);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<List<IdNameViewModel>> GetAssessmentListToSet(string TypeId)
        {
            var Result = await _talentAssessmentQueryBusiness.GetAssessmentListToSet(TypeId);
            return Result;

        }


        public async Task<AssessmentViewModel> GetAssessmentSetAssessmentByid(string NoteId)
        {
            var Result = await _talentAssessmentQueryBusiness.GetAssessmentSetAssessmentByid(NoteId);
            return Result;

        }

        public async Task<IdNameViewModel> GetSquenceNoExistAssessmentSet(string AssessmentSetId, string SequenceNo)
        {
            var Result = await _talentAssessmentQueryBusiness.GetSquenceNoExistAssessmentSet(AssessmentSetId, SequenceNo);
            return Result;
        }

        public async Task<IdNameViewModel> GetAssessmentExistAssessmentSet(string AssessmentSetId, string AssessmentId, string Id)
        {
            var Result = await _talentAssessmentQueryBusiness.GetAssessmentExistAssessmentSet(AssessmentSetId, AssessmentId, Id);
            return Result;
        }


        public async Task<List<AssessmentInterviewViewModel>> GetAssessmentReportMinistryList(string SponsorId = null, string userId = null)
        {
            var Result = await _talentAssessmentQueryBusiness.GetAssessmentReportMinistryList(SponsorId, userId);
            return Result;

        }

        public async Task<IdNameViewModel> GetSponsorDetailsByUserId(string UserId)
        {
            var result = await _talentAssessmentQueryBusiness.GetSponsorDetailsByUserId(UserId);
            return result;
        }
        public async Task<List<AssessmentDetailViewModel>> GetAssessmentListByUserId(string userId, string statusCode, string typeId)
        {

            var result = await _talentAssessmentQueryBusiness.GetAssessmentListByUserId(userId, statusCode, typeId);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<string> GetUserIdentificationByUserId(string userId)
        {
            var model = await _talentAssessmentQueryBusiness.GetUserIdentificationByUserId(userId);
            return model;
        }
        public async Task<List<AssessmentInterviewViewModel>> GetInterviewList(string UserID, string status)
        {
            var result = await _talentAssessmentQueryBusiness.GetInterviewList(UserID, status);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<IdNameViewModel> GetIndicatorbyName(string Name)
        {
            var result = await _talentAssessmentQueryBusiness.GetIndicatorbyName(Name);
            return result;
        }

        public async Task<IdNameViewModel> GetCompetencybyName(string Name)
        {
            var result = await _talentAssessmentQueryBusiness.GetCompetencybyName(Name);
            return result;
        }


        public async Task<IdNameViewModel> GetAssessmentbyName(string Name)
        {
            var result = await _talentAssessmentQueryBusiness.GetAssessmentbyName(Name);
            return result;
        }
        public async Task<SurveyScheduleViewModel> GetSurveyDetails(string surveyScheduleUserId, string surveyCode = null)
        {
            var result = await _talentAssessmentQueryBusiness.GetSurveyDetails(surveyScheduleUserId, surveyCode);
            return result;
        }
        //public async Task<SurveyScheduleViewModel> GetSurveyDetailsByCode(string surveyCode)
        //{
        //    var query = @$"select s.""Id"" as SurveyScheduleUserId,s.""SurveyUserId"",ss.""Id"" as SurveyScheduleId,ss.""SurveyScheduleName"",
        //    ss.""PreferredLanguage"",ss.""SurveyId"",(s.""SurveyExpiryDate"")::TIMESTAMP::DATE,ss.""StartingInstruction"",sr.""Id"" as SurveyResultId,
        //    sr.""CurrentTopicId"",lov.""Code"" as LanguageCode,ser.""Id"" as ServiceId,s.""SurveyLink"" as SurveyLink,sun.""PortalId"" as PortalId
        //    from cms.""N_SURVEY_SurveyScheduleUser"" as s
        //    join public.""NtsNote"" as sun on s.""NtsNoteId""=sun.""Id"" and sun.""IsDeleted""=false and sun.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join cms.""N_SURVEY_SurveySchedule"" as ss on s.""SurveyScheduleId"" = ss.""Id"" and ss.""IsDeleted"" = false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join public.""LOV"" as lov on ss.""PreferredLanguage""=lov.""Id"" and lov.""IsDeleted""=false
        //    left join cms.""N_SURVEY_S_SurveyResult"" as sr on s.""Id"" = sr.""SurveyScheduleUserId"" and sr.""IsDeleted"" = false and sr.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join public.""NtsNote"" as n on sr.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join public.""NtsService"" as ser on n.""Id""=ser.""UdfNoteId"" and ser.""IsDeleted""=false and ser.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    where s.""SurveyCode"" = '{surveyCode}' and s.""IsDeleted"" = false and s.""CompanyId""='{_repo.UserContext.CompanyId}' ";

        //    var result = await _idNameRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
        //    return result;
        //}

        public async Task UpdatePreferredLanguage(string surveyScheduleId, string lang, string surveyResultId)
        {
            await _talentAssessmentQueryBusiness.UpdatePreferredLanguage(surveyScheduleId, lang, surveyResultId);
        }

        //Survey

        public async Task<SurveyTopicViewModel> GetSurveyAssessmentQuestion(string serviceId, string currTopicId, string lang)
        {
            if (currTopicId.IsNullOrEmpty())
            {

                var res = await _talentAssessmentQueryBusiness.GetSurveyAssessmentQuestion(serviceId);

                return await GetSurveyAssessmentQuestionParent(serviceId, res.CurrentTopicId, lang);
            }
            else
            {
                return await GetSurveyAssessmentQuestionParent(serviceId, currTopicId, lang);
            }

        }

        public async Task<SurveyTopicViewModel> GetSurveyAssessmentQuestionParent(string serviceId, string currTopicId, string lang)
        {
            var result = new SurveyTopicViewModel();

            var list = await _talentAssessmentQueryBusiness.GetAssessmentData(serviceId, lang);

            if (list != null && list.Count != 0)
            {
                if (currTopicId.IsNullOrEmpty())
                {
                    result.TopicId = list.Where(x => x.TopicSequenceNo == 1).Select(x => x.TopicId).FirstOrDefault();
                    result.NextId = list.Where(x => x.TopicSequenceNo == 2).Select(x => x.TopicId).FirstOrDefault();
                    result.TopicName = list.Where(x => x.TopicSequenceNo == 1).Select(x => x.TopicName).FirstOrDefault();
                }
                else
                {
                    result.TopicId = currTopicId;
                    var seq = list.Where(x => x.TopicId == currTopicId).Select(x => x.TopicSequenceNo).FirstOrDefault();
                    var next = seq + 1;
                    var prev = seq - 1;
                    result.PreviousId = list.Where(x => x.TopicSequenceNo == prev).Select(x => x.TopicId).FirstOrDefault();
                    result.NextId = list.Where(x => x.TopicSequenceNo == next).Select(x => x.TopicId).FirstOrDefault();
                    result.TopicName = list.Where(x => x.TopicId == currTopicId).Select(x => x.TopicName).FirstOrDefault();

                }
                result.FirstId = list.First().TopicId;
                result.LastId = list.Last().TopicId;
                // result.AssessmentId = list.First().AssessmentId;
                //result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

                if (result.TopicId.IsNotNull())
                {
                    result.QuestionList = new List<SurveyQuestionViewModel>();
                    var listQuestion = list.Where(x => x.TopicId == result.TopicId).ToList();
                    foreach (var question in listQuestion)
                    {
                        var survey = new SurveyQuestionViewModel();
                        survey.QuestionId = question.Id;
                        survey.QuestionName = HttpUtility.HtmlDecode(question.Question);
                        survey.EnableComment = question.EnableComment;

                        survey.OptionList = await GetSurveyOptions(question.Id, serviceId, lang, list.First().AssessmentId, question.SurveyScheduleUserId);
                        if (survey.OptionList != null && survey.OptionList.Count > 0)
                        {
                            var answer = survey.OptionList.Where(x => x.AnswerId != null).FirstOrDefault();
                            if (answer != null)
                            {
                                survey.AnswerComment = answer.AnswerComment;
                                //result.Score = answer.Score;
                                survey.AnswerId = answer.AnswerId;
                            }
                            survey.AnswerItemId = survey.OptionList.FirstOrDefault().AnswerItemId;
                        }
                        result.QuestionList.Add(survey);
                    }
                }
            }

            return result;
        }
        public async Task<SurveyScheduleViewModel> GetSurveyScheduleById(string surveyId)
        {
            var data = await _talentAssessmentQueryBusiness.GetSurveyScheduleById(surveyId);
            return data;
        }
        public async Task<List<SurveyScheduleViewModel>> GetSurveyScheduleList(string surveyId, string status)
        {
            var data = await _talentAssessmentQueryBusiness.GetSurveyScheduleList(surveyId, status);
            return data;
        }
        public async Task<SurveyScheduleViewModel> GetSurveyScheduleDetails(string surveyScheduleId)
        {
            var data = await _talentAssessmentQueryBusiness.GetSurveyScheduleDetails(surveyScheduleId);
            return data;
        }
        public async Task<List<AssessmentQuestionsOptionViewModel>> GetSurveyOptions(string noteId, string serviceId, string lang, string assessmentId, string surveyScheduleUserId)
        {
            var result = await _talentAssessmentQueryBusiness.GetSurveyOptions(noteId, serviceId, lang, assessmentId, surveyScheduleUserId);
            return result;
        }

        public async Task ExecuteSurveyForUsers(string NoteId, string PortalId)
        {
            var _userPortalBusiness = _sp.GetService<IUserPortalBusiness>();
            var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
            var _notificationBusiness = _sp.GetService<INotificationBusiness>();

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.NoteId = NoteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var LastUpdatedBy = notemodel.LastUpdatedBy; //rowData1.ContainsKey("LastUpdatedBy") ? Convert.ToString(rowData1["LastUpdatedBy"]) : "";
            var UserDetails = rowData1.ContainsKey("UserDetails") ? Convert.ToString(rowData1["UserDetails"]) : "";
            var SurveyExpiryDate = rowData1.ContainsKey("SurveyExpiryDate") ? Convert.ToString(rowData1["SurveyExpiryDate"]) : "";
            var SurveyScheduleId = notemodel.UdfNoteTableId; //rowData1.ContainsKey("Id") ? Convert.ToString(rowData1["Id"]) : "";
            // update existing survey schedule user Expiry date to New Expiry Date 
            await UpdateScheduleSurveyUserExpiryDate(SurveyScheduleId, SurveyExpiryDate);
            if (UserDetails.IsNotNull())
            {
                var users = UserDetails.Split(";");
                if (users.Count() > 0)
                {
                    foreach (var user in users)
                    {
                        if (user.IsNotNullAndNotEmpty())
                        {
                            string UserId = "";
                            var details = user.Split("|");
                            var email1 = details[0];
                            var Name = email1;
                            if (details.Length > 1)
                            {
                                Name = details[1];
                            }

                            // var email1 = email[1];
                            // check if user email exist
                            var exisitingUser = await _userBusiness.GetSingle(x => x.Email == email1);
                            if (exisitingUser != null)
                            {
                                UserId = exisitingUser.Id;
                                // check if user has access to survey portal
                                var allowedPortals = await _userPortalBusiness.GetList(x => x.UserId == exisitingUser.Id && x.PortalId == PortalId);
                                if (!allowedPortals.Any(x => x.PortalId == PortalId))
                                {
                                    var res1 = await _userPortalBusiness.Create(new UserPortalViewModel
                                    {
                                        UserId = exisitingUser.Id,
                                        PortalId = PortalId,
                                    });
                                }
                                // check survey is already scheduled for the user
                                var isExist = await GetSurveyScheduleUser(SurveyScheduleId, UserId);
                                if (!isExist)
                                {
                                    // Schedule the Survey for the User
                                    if (UserId.IsNotNullAndNotEmpty())
                                    {
                                        var TempModel = new NoteTemplateViewModel();
                                        TempModel.TemplateCode = "SURVEY_SCHEDULE_USER";
                                        TempModel.ActiveUserId = LastUpdatedBy;
                                        var model = await _noteBusiness.GetNoteDetails(TempModel);
                                        model.OwnerUserId = LastUpdatedBy;
                                        model.DataAction = DataActionEnum.Create;
                                        dynamic exo = new System.Dynamic.ExpandoObject();
                                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", SurveyScheduleId);
                                        ((IDictionary<String, Object>)exo).Add("SurveyExpiryDate", SurveyExpiryDate);
                                        ((IDictionary<String, Object>)exo).Add("SurveyUserId", UserId);
                                        var surveyCode = await GetSurveyCode();
                                        ((IDictionary<String, Object>)exo).Add("SurveyCode", Convert.ToString(surveyCode));
                                        var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                                        var param = Helper.EncryptJavascriptAesCypher($"surveyCode={surveyCode}");
                                        var link = $"{baseurl}survey?enc={param}";
                                        ((IDictionary<String, Object>)exo).Add("SurveyLink", link);

                                        model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var res = await _noteBusiness.ManageNote(model);


                                        //rowData1["SurveyLink"] = link;
                                        //var notificationTemplateModel = await _notificationBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.CompanyId == model.CompanyId && x.Code == "SURVEY_WELCOME_EMAIL");
                                        //if (notificationTemplateModel.IsNotNull())
                                        //{
                                        //    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{link}}", link);
                                        //    var notificationModel = new NotificationViewModel()
                                        //    {
                                        //        To = email1,
                                        //        ToUserId = UserId,
                                        //        // FromUserId = uc.UserId,
                                        //        Subject = notificationTemplateModel.Subject,
                                        //        Body = notificationTemplateModel.Body,
                                        //        SendAlways = true,
                                        //        NotifyByEmail = true,
                                        //        DynamicObject = model
                                        //    };
                                        //    await _notificationBusiness.Create(notificationModel);
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                // Create new User
                                var UserModel = new UserViewModel();
                                UserModel.Name = Name;
                                UserModel.Email = email1;
                                UserModel.Portal = new List<string>();
                                UserModel.Portal.Add(PortalId);
                                var result = await _userBusiness.Create(UserModel);
                                if (result.IsSuccess)
                                {
                                    UserId = result.Item.Id;
                                    var res1 = await _userPortalBusiness.Create(new UserPortalViewModel
                                    {
                                        UserId = result.Item.Id,
                                        PortalId = PortalId,
                                    });
                                    // Schedule the Survey for the User
                                    if (UserId.IsNotNullAndNotEmpty())
                                    {
                                        var TempModel = new NoteTemplateViewModel();
                                        TempModel.TemplateCode = "SURVEY_SCHEDULE_USER";
                                        TempModel.ActiveUserId = LastUpdatedBy;
                                        var model = await _noteBusiness.GetNoteDetails(TempModel);
                                        model.OwnerUserId = LastUpdatedBy;
                                        model.DataAction = DataActionEnum.Create;
                                        dynamic exo = new System.Dynamic.ExpandoObject();
                                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", SurveyScheduleId);
                                        ((IDictionary<String, Object>)exo).Add("SurveyExpiryDate", SurveyExpiryDate);
                                        ((IDictionary<String, Object>)exo).Add("SurveyUserId", UserId);
                                        var surveyCode = await GetSurveyCode();
                                        ((IDictionary<String, Object>)exo).Add("SurveyCode", Convert.ToString(surveyCode));
                                        var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                                        var param = Helper.EncryptJavascriptAesCypher($"surveyCode={surveyCode}");
                                        var link = $"{baseurl}survey?enc={param}";
                                        ((IDictionary<String, Object>)exo).Add("SurveyLink", link);

                                        model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var res = await _noteBusiness.ManageNote(model);

                                        //var customUrl = HttpUtility.UrlEncode($"surveyScheduleUserId={res.Item.UdfNoteTableId}");
                                        //var param = Helper.EncryptJavascriptAesCypher($"page=surveyhome&customurl={customUrl}");
                                        //var portal = await _userPortalBusiness.GetSingleById<PortalViewModel, Portal>(PortalId);
                                        //var link = $"{baseurl}portal/{portal.Name}?enc={param}";
                                        //rowData1["SurveyLink"] = link;

                                        //var notificationTemplateModel = await _notificationBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.CompanyId == model.CompanyId && x.Code == "SURVEY_WELCOME_EMAIL");
                                        //if (notificationTemplateModel.IsNotNull())
                                        //{
                                        //    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{link}}", link);
                                        //    var notificationModel = new NotificationViewModel()
                                        //    {
                                        //        To = email1,
                                        //        ToUserId = UserId,
                                        //        // FromUserId = uc.UserId,
                                        //        Subject = notificationTemplateModel.Subject,
                                        //        Body = notificationTemplateModel.Body,
                                        //        SendAlways = true,
                                        //        NotifyByEmail = true,
                                        //        DynamicObject = model
                                        //    };
                                        //    await _notificationBusiness.Create(notificationModel);
                                        //}
                                    }
                                }

                            }

                        }
                    }
                }

            }
            //Updated To Completed           
            var SurveyStatus = rowData1.ContainsKey("SurveyStatus") ? Convert.ToString(rowData1["SurveyStatus"]) : "";
            if (SurveyStatus.IsNotNull())
            {
                rowData1["SurveyStatus"] = (int)((SurveyStatusEnum)Enum.Parse(typeof(SurveyStatusEnum), SurveyStatusEnum.ScheduleCompleted.ToString()));
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
        }
        public async Task<List<SurveyScheduleViewModel>> GetSurveyDetail(string id)
        {
            var data = await _talentAssessmentQueryBusiness.GetSurveyDetail(id);
            return data;
        }

        public async Task<CommandResult<List<SurveyScheduleViewModel>>> GetValidateSurveyScheduleUserDetails(string surveyId, string userDetails, string surveyScheduleId)
        {
            var result = await _talentAssessmentQueryBusiness.GetSurveyScheduleDetailsById(surveyId, surveyScheduleId);
            var errorList = new Dictionary<string, string>();
            if (userDetails.IsNotNullAndNotEmpty())
            {
                var userData = userDetails.Split(";");
                if (userData.Count() > 0)
                {
                    var i = 1;
                    foreach (var user in userData)
                    {
                        var u = user.Split("|");
                        var uemail = u[0];
                        if (uemail.IsNotNullAndNotEmpty())
                        {
                            foreach (var rdata in result)
                            {
                                if (rdata.UserDetails.IsNotNullAndNotEmpty())
                                {
                                    var rusers = rdata.UserDetails.Split(";");
                                    if (rusers.Count() > 0)
                                    {
                                        foreach (var ruser in rusers)
                                        {
                                            var ru = ruser.Split("|");
                                            var ruemail = ru[0];
                                            if (ruemail.IsNotNullAndNotEmpty() && ruemail == uemail)
                                            {
                                                errorList.Add("UserEmail" + i, "Survey Schedule : " + rdata.SurveyScheduleName + ", " + uemail + " email already exist.");
                                            }
                                            i++;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    if (errorList.Count > 0)
                    {
                        return CommandResult<List<SurveyScheduleViewModel>>.Instance(result, false, errorList);
                    }
                }
            }

            return CommandResult<List<SurveyScheduleViewModel>>.Instance(result, true, errorList);
        }
        private async Task<long> GetSurveyCode()
        {
            var random = new Random();
            var isUniq = false;
            var surveyCode = random.Next(100000, 999999);
            while (isUniq == false)
            {
                var result = await _talentAssessmentQueryBusiness.GetSurveyCode(surveyCode);
                if (result == null)
                {
                    isUniq = true;
                    return surveyCode;
                }
            }
            return surveyCode;

        }

        public async Task<ServiceTemplateViewModel> GetSurveyResultDetails(string surveyId, string surveyScheduleUserId, string surveyUserId)
        {
            var result = await _talentAssessmentQueryBusiness.GetSurveyResultDetails(surveyId, surveyScheduleUserId, surveyUserId);
            return result;
        }
        public async Task<ServiceTemplateViewModel> CheckSurveyResultExist(string surveyScheduleUserId)
        {
            var result = await _talentAssessmentQueryBusiness.CheckSurveyResultExist(surveyScheduleUserId);
            return result;
        }

        public async Task<List<SurveyQuestionViewModel>> GetSurveyResult(string surveyResultId, string lang)
        {

            var result = await _talentAssessmentQueryBusiness.GetSurveyResult(surveyResultId, lang);
            return result;
        }

        public async Task<bool> GetSurveyScheduleUser(string SurveyScheduleId, string UserId)
        {

            var data = await _talentAssessmentQueryBusiness.GetSurveyScheduleUser(SurveyScheduleId, UserId);
            if (data.IsNotNull() && data.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task UpdateScheduleSurveyUserExpiryDate(string surveyScheduleId, string expiryDate)
        {
            await _talentAssessmentQueryBusiness.UpdateScheduleSurveyUserExpiryDate(surveyScheduleId, expiryDate);
        }

        public async Task GenerateSurveyDetails(string surveyScheduleId)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var SurveyScheduleUserList = await GetSurveyDetail(surveyScheduleId);
            if (SurveyScheduleUserList.IsNotNull() && SurveyScheduleUserList.Count() > 0)
            {
                foreach (var user in SurveyScheduleUserList)
                {
                    // check if survey result exist or not     
                    var surveyresult = await CheckSurveyResultExist(user.SurveyScheduleUserId);
                    if (surveyresult.IsNotNull())
                    {
                        var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_S_SURVEY_RESULT", null, "SurveyScheduleUserId", user.SurveyScheduleUserId);
                        //check if end date is present
                        if (existing.IsNotNull())
                        {

                            var EndDate = Convert.ToString(existing["SurveyEndDate"]);
                            if (EndDate.IsNullOrEmpty())
                            {
                                // get all Question and check if already have service for the answer result
                                var questionslist = await GetQuestionsForSurveyResult(user.SurveyId);
                                if (questionslist.IsNotNull())
                                {
                                    foreach (var question in questionslist)
                                    {
                                        var isAnswerResultExist = await CheckIfSurveyAnswerResultExist(question.Id, existing["Id"].ToString());
                                        if (!isAnswerResultExist)
                                        {
                                            // create Survey Answer result service                                       
                                            var serviceTempModel1 = new ServiceTemplateViewModel();
                                            serviceTempModel1.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                                            serviceTempModel1.ActiveUserId = user.SurveyUserId;
                                            var servicemodel1 = await _serviceBusiness.GetServiceDetails(serviceTempModel1);
                                            servicemodel1.DataAction = DataActionEnum.Create;
                                            dynamic exo1 = new System.Dynamic.ExpandoObject();
                                            ((IDictionary<String, Object>)exo1).Add("SurveyId", user.SurveyId);
                                            ((IDictionary<String, Object>)exo1).Add("SurveyResultId", surveyresult.UdfNoteTableId);
                                            ((IDictionary<String, Object>)exo1).Add("QuestionId", question.Id);
                                            ((IDictionary<String, Object>)exo1).Add("UserId", user.SurveyUserId);
                                            servicemodel1.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                                            servicemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                            var res1 = await _serviceBusiness.ManageService(servicemodel1);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    // does not exist
                    else
                    {
                        // Create Survey Result service
                        var serviceTempModel = new ServiceTemplateViewModel();
                        serviceTempModel.TemplateCode = "S_SURVEY_RESULT";
                        serviceTempModel.ActiveUserId = user.SurveyUserId;
                        var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                        servicemodel.DataAction = DataActionEnum.Create;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", user.Id);
                        ((IDictionary<String, Object>)exo).Add("PreferredLanguage", user.PreferredLanguage);
                        ((IDictionary<String, Object>)exo).Add("SurveyUserId", user.SurveyUserId);
                        ((IDictionary<String, Object>)exo).Add("SurveyId", user.SurveyId);
                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleUserId", user.SurveyScheduleUserId);
                        servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        var res = await _serviceBusiness.ManageService(servicemodel);
                        // fetch all the Question and create answer service
                        var questionslist = await GetQuestionsForSurveyResult(user.SurveyId);
                        if (questionslist.IsNotNull())
                        {
                            foreach (var question in questionslist)
                            {
                                // Create Survey Result Answer service
                                var serviceTempModel1 = new ServiceTemplateViewModel();
                                serviceTempModel1.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                                serviceTempModel1.ActiveUserId = _userContext.UserId;
                                var servicemodel1 = await _serviceBusiness.GetServiceDetails(serviceTempModel1);
                                servicemodel1.DataAction = DataActionEnum.Create;
                                dynamic exo1 = new System.Dynamic.ExpandoObject();
                                ((IDictionary<String, Object>)exo1).Add("SurveyId", user.SurveyId);
                                ((IDictionary<String, Object>)exo1).Add("SurveyResultId", res.Item.UdfNoteTableId);
                                ((IDictionary<String, Object>)exo1).Add("QuestionId", question.Id);
                                ((IDictionary<String, Object>)exo1).Add("UserId", user.SurveyUserId);
                                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                                servicemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                var res1 = await _serviceBusiness.ManageService(servicemodel1);
                            }
                        }
                    }
                }
            }
        }

        public async Task<SurveyScheduleViewModel> GenerateDummySurveyDetails(string surveyScheduleId, int count, string userId = null)
        {
            var surveySchedule = await _talentAssessmentQueryBusiness.GetSurveyScheduleDetails(surveyScheduleId);
            SurveyScheduleViewModel surveyScheduleUser = null;
            if (surveySchedule != null)
            {
                for (int i = 0; i < count; i++)
                {

                    //create a survey scheduleuser here
                    surveyScheduleUser = new SurveyScheduleViewModel();
                    var TempModel = new NoteTemplateViewModel();
                    TempModel.TemplateCode = "SURVEY_SCHEDULE_USER";
                    TempModel.ActiveUserId = _userContext.UserId;
                    var model = await _noteBusiness.GetNoteDetails(TempModel);
                    model.OwnerUserId = _userContext.UserId;
                    model.DataAction = DataActionEnum.Create;
                    dynamic exo11 = new System.Dynamic.ExpandoObject();
                    ((IDictionary<String, Object>)exo11).Add("SurveyScheduleId", surveyScheduleId);
                    ((IDictionary<String, Object>)exo11).Add("SurveyExpiryDate", surveySchedule.SurveyExpiryDate);
                    ((IDictionary<String, Object>)exo11).Add("SurveyUserId", userId);
                    var surveyCode = await GetSurveyCode();
                    ((IDictionary<String, Object>)exo11).Add("SurveyCode", Convert.ToString(surveyCode));
                    var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
                    var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                    var param = Helper.EncryptJavascriptAesCypher($"surveyCode={surveyCode}");
                    var link = $"{baseurl}survey?enc={param}";
                    ((IDictionary<String, Object>)exo11).Add("SurveyLink", link);

                    model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo11);
                    var result = await _noteBusiness.ManageNote(model);
                    surveyScheduleUser.UdfNoteTableId = result.Item.UdfNoteTableId;
                    // Create Survey Result service
                    var serviceTempModel = new ServiceTemplateViewModel();
                    serviceTempModel.TemplateCode = "S_SURVEY_RESULT";
                    serviceTempModel.ActiveUserId = userId ?? _userContext.UserId;
                    var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                    servicemodel.DataAction = DataActionEnum.Create;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", surveySchedule.Id);
                    ((IDictionary<String, Object>)exo).Add("PreferredLanguage", surveySchedule.PreferredLanguage);
                    ((IDictionary<String, Object>)exo).Add("SurveyUserId", userId);
                    ((IDictionary<String, Object>)exo).Add("SurveyId", surveySchedule.SurveyId);
                    ((IDictionary<String, Object>)exo).Add("SurveyScheduleUserId", surveyScheduleUser.UdfNoteTableId);
                    servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    var res = await _serviceBusiness.ManageService(servicemodel);
                    // fetch all the Question and create answer service
                    var questionslist = await GetQuestionsForSurveyResult(surveySchedule.SurveyId);
                    if (questionslist.IsNotNull())
                    {
                        foreach (var question in questionslist)
                        {
                            // Create Survey Result Answer service
                            var serviceTempModel1 = new ServiceTemplateViewModel();
                            serviceTempModel1.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                            serviceTempModel1.ActiveUserId = _userContext.UserId;
                            var servicemodel1 = await _serviceBusiness.GetServiceDetails(serviceTempModel1);
                            servicemodel1.DataAction = DataActionEnum.Create;
                            dynamic exo1 = new System.Dynamic.ExpandoObject();
                            ((IDictionary<String, Object>)exo1).Add("SurveyId", surveySchedule.SurveyId);
                            ((IDictionary<String, Object>)exo1).Add("SurveyResultId", res.Item.UdfNoteTableId);
                            ((IDictionary<String, Object>)exo1).Add("QuestionId", question.Id);
                            ((IDictionary<String, Object>)exo1).Add("UserId", userId);
                            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                            servicemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                            var res1 = await _serviceBusiness.ManageService(servicemodel1);
                        }
                    }

                }
            }
            return surveyScheduleUser;
        }

        public async Task<SurveyScheduleViewModel> AssignDummySurveyToUser(string surveyScheduleId, string userId)
        {
            var surveyScheduleUser = await _talentAssessmentQueryBusiness.AssignDummySurveyToUser(surveyScheduleId, userId);
            if (surveyScheduleUser == null)
            {
                surveyScheduleUser = await GenerateDummySurveyDetails(surveyScheduleId, 1, userId);
            }
            var hangfireScheduler = _sp.GetService<IHangfireScheduler>();
            BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.GenerateDummySurveyDetails(surveyScheduleId, 1, _repo.UserContext.ToIdentityUser(), null));
            return surveyScheduleUser;
        }
        public async Task<List<SurveyTopicViewModel>> GetQuestionsForSurveyResult(string surveyId)
        {
            var list = await _talentAssessmentQueryBusiness.GetQuestionsForSurveyResult(surveyId);
            return list;
        }
        public async Task<bool> CheckIfSurveyAnswerResultExist(string questionId, string surveyResultId)
        {

            var data = await _talentAssessmentQueryBusiness.CheckIfSurveyAnswerResultExist(questionId, surveyResultId);
            if (data.IsNotNull())
            {
                return true;
            }
            return false;
        }


        public async Task<MemoryStream> GetBusinessUserSurveyReportDataExcel()
        {



            var model = await _talentAssessmentQueryBusiness.GetHRUserSurveyData();

            var ms = new MemoryStream();
            var reportList = new List<string>();
            //{ "Assessment Report", "Case Study Data", "Case Study Report" };
            //reportList.Add("Survey Data");

            using (var sl = new SLDocument())
            {
                //sl.AddWorksheet("Survey Data");

                // SLPageSettings pageSettings = new SLPageSettings();
                // pageSettings.ShowGridLines = true;
                // sl.SetPageSettings(pageSettings);

                // sl.SetColumnWidth("A", 20);
                // sl.SetColumnWidth("B", 20);
                // sl.SetColumnWidth("C", 20);                    

                // sl.SetCellValue("A1", "Question");
                // sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                // sl.SetCellValue("B1", "Answer");
                // sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                // sl.SetCellValue("C1", "Comment");
                // sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowCayanStyle(sl));


                sl.AddWorksheet("Survey Data - Business User");
                var modelData = new SurveyQuestionViewModel();
                new SLPageSettings().ShowGridLines = true;
                sl.SetPageSettings(new SLPageSettings());

                sl.SetColumnWidth("A", 30);
                sl.SetColumnWidth("B", 20);
                sl.SetColumnWidth("C", 20);
                sl.SetColumnWidth("D", 30);
                sl.SetColumnWidth("E", 60);
                sl.SetColumnWidth("F", 20);
                sl.SetColumnWidth("G", 60);

                sl.SetCellValue("A1", "Division");
                sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("B1", "Career Level");
                sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("C1", "Market");
                sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("D1", "HR Function");
                sl.SetCellStyle("D1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("E1", "Question");
                sl.SetCellStyle("E1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("F1", "Answer");
                sl.SetCellStyle("F1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("G1", "Comment");
                sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowHeadingStyle(sl));


                if (model.Count > 0)
                {
                    int row = 2;

                    Int32 maxNo = model.Count;

                    foreach (var item in model)
                    {
                        char first = 'A';

                        first = (char)((int)first);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.Division);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.CareerLevel);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.Market);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.HRFunction);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.QuestionName);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.AnswerName);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.AnswerComment);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        row++;
                    }
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }
            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetHRUserSurveyReportDataExcel()
        {



            var model = await _talentAssessmentQueryBusiness.GetHRUserSurveyData();

            var ms = new MemoryStream();
            var reportList = new List<string>();
            //{ "Assessment Report", "Case Study Data", "Case Study Report" };

            //reportList.Add("Survey Data");

            using (var sl = new SLDocument())
            {
                //sl.AddWorksheet("Survey Data");

                // SLPageSettings pageSettings = new SLPageSettings();
                // pageSettings.ShowGridLines = true;
                // sl.SetPageSettings(pageSettings);

                // sl.SetColumnWidth("A", 20);
                // sl.SetColumnWidth("B", 20);
                // sl.SetColumnWidth("C", 20);                    

                // sl.SetCellValue("A1", "Question");
                // sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                // sl.SetCellValue("B1", "Answer");
                // sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                // sl.SetCellValue("C1", "Comment");
                // sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowCayanStyle(sl));


                sl.AddWorksheet("Survey Data - HR User");
                var modelData = new SurveyQuestionViewModel();
                new SLPageSettings().ShowGridLines = true;
                sl.SetPageSettings(new SLPageSettings());

                sl.SetColumnWidth("A", 30);
                sl.SetColumnWidth("B", 20);
                sl.SetColumnWidth("C", 20);
                sl.SetColumnWidth("D", 30);
                sl.SetColumnWidth("E", 60);
                sl.SetColumnWidth("F", 20);
                sl.SetColumnWidth("G", 60);

                sl.SetCellValue("A1", "Division");
                sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("B1", "Career Level");
                sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("C1", "Market");
                sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("D1", "HR Function");
                sl.SetCellStyle("D1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("E1", "Question");
                sl.SetCellStyle("E1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("F1", "Answer");
                sl.SetCellStyle("F1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                sl.SetCellValue("G1", "Comment");
                sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowHeadingStyle(sl));

                if (model.Count > 0)
                {
                    int row = 2;

                    Int32 maxNo = model.Count;

                    foreach (var item in model)
                    {
                        char first = 'A';

                        first = (char)((int)first);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.Division);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.CareerLevel);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.Market);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.HRFunction);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.QuestionName);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.AnswerName);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), item.AnswerComment);
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        row++;
                    }
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }
            ms.Position = 0;
            return ms;
        }
        public async Task<long> GetSurveyUserCount(string prefix)
        {

            return await _talentAssessmentQueryBusiness.GetSurveyUserCount(prefix);
        }

    }
}

