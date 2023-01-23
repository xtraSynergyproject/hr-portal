using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITalentAssessmentBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task DeleteOption(string id);
        Task<List<AssessmentQuestionsViewModel>> GetQuestion();
        Task<IList<IdNameViewModel>> GetCompetencyLevelList(string noteId = null);
        Task<IList<IdNameViewModel>> GetCompetencyLevelListByTopic(string topicId);
        Task<IdNameViewModel> GetQuestionCountByTopic(string topicId);
        Task<List<AssessmentQuestionsViewModel>> GetTreeListQuestion();
        Task<AssessmentQuestionsOptionViewModel> GetOptionsById(string Id);
        public Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsForQuestion(string questionNoteId);
        Task<List<AssessmentViewModel>> GetAssessmentsList(string type, string searchtext);
        Task<bool> DeleteAssessment(string NoteId);
        Task ManageAssessmentStatus(string serviceId, bool isAssessmentStopped);
        Task<AssessmentViewModel> GetAssessmentDetailsById(string assmntId);
        Task CopyAssessment(AssessmentViewModel model);
        Task<List<AssessmentViewModel>> GetQuestionsByAssessmentId(string assessmentId);
        Task<List<CapacityRiskViewModel>> GetJobCapacityData(string departmentId);
        Task<List<CapacityRiskViewModel>> GetJobCapacityChartData(string jobId, string departmentId);
        Task<bool> UpdateSequenceNo(string Id, long? sequenceNo);
        Task<AssignmentViewModel> GetAssignmentDetails(string userId);
        public Task<List<AssignmentViewModel>> ReadBadgeData(string userId);
        Task<List<AssessmentQuestionsViewModel>> GetMappedQuestionsList(string assessmentId);
        Task<bool> DeleteAssessmentQuestion(string NoteId);
        Task<bool> MapQuestionsToAssessment(string assessmentId, string qIds);
        Task<List<AssessmentQuestionsViewModel>> GetAllQuestions(string assessmentTypeId);
        Task<List<AssignmentViewModel>> ReadAssessmentData(string userId);
        Task<AssessmentDetailViewModel> GetAssessmentQuestionParent(string serviceId, string curQueId, string lang);
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceTaskViewData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceDevelopmentViewData();
        Task<List<AssignmentViewModel>> ReadCertificationData(string userId);
        Task<List<AssignmentViewModel>> ReadSkillData(string userId);
        Task<List<AssessmentDetailViewModel>> GetAssessmentListByUserId(string userId, string statusCode);
        Task<List<AssessmentDetailViewModel>> GetCompletedAssessmentListByUserId(string userId);
        Task<List<AssessmentDetailViewModel>> GetAllAssessmentsList();
        Task<AssessmentResultViewModel> GetQuestion(string serviceId);
        Task<AssessmentDetailViewModel> GetAssessmentQuestionEnglish(string serviceId, string currQueId);
        Task<AssessmentDetailViewModel> GetAssessmentQuestionArabic(string serviceId, string currQueId);
        Task<List<AssessmentQuestionsOptionViewModel>> GetOptions(string noteId, string serviceId, string lang, string assessmentId);
        Task<List<IdNameViewModel>> GetAssessmentProctorList();
        Task SubmitAssessmentAnswer(string serviceId, bool isSubmit, string currentQuestionId, string multipleFieldValueId, string comment, int? timeElapsed, int? timeElapsedSec, string nextQuestionId, string currentAnswerId, string assessmentType, string userId, string assessmentId);
        Task<List<IdNameViewModel>> GetJobTitle(string loggedInUserId);
        Task<bool> UpdateAssessment(string serviceId, int? timeElapsed, int? timeElapsedSec, string nextQuestionId = null, DateTime? startDate = null);
        Task<AssessmentDetailViewModel> GetTryoutQuestion(string assessmentId, string currQueId, string lang);
        Task<AssessmentDetailViewModel> GetAssessmentTryoutQuestion(string assessmentId, string curQueId, string lang);
        Task<List<AssessmentQuestionsOptionViewModel>> GetTryoutQuestionOptions(string noteId, string lang);
        Task<List<IdNameViewModel>> GetAssessmentListIdName(string assSetId);
        Task<List<AssessmentInterviewViewModel>> GetAssessmentInterview(string UserID, bool isarchived = false, string source = null);
        Task DeleteAssessmentSchedule(string id);
        Task<AssessmentInterviewViewModel> GetInterviewService(string UserID);
        Task<AssessmentAnswerViewModel> GetQuestionByAnswerId(string noteId);
        Task<MemoryStream> GetInterViewQuestionExcel(string UserId, string TableId);
        Task<List<InterViewQuestionViewModel>> GetRandamQuestions();
        Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsByQuestionId(string questionId);
        Task<AssessmentReportViewModel> GetCandidateCaseStudyReport(string userId);
        Task<bool> UpdateScoreComment(List<AssessmentAnswerViewModel> modelList);
        Task<MemoryStream> GetCandidateCaseStudyDataExcel(AssessmentReportViewModel model);
        Task<List<IdNameViewModel>> GetAssessmentSetListIdName();
        Task<List<IdNameViewModel>> GetInterviewAssessorList(string UserId);
        Task<MemoryStream> GetInterviewerCaseStudyDataExcel(AssessmentReportViewModel model);
        Task<IList<AssessmentQuestionsViewModel>> GetAssessmentQuestions(string topic, string level);
        Task<AssessmentReportViewModel> GetAssessmentReportexcel(string userId);
        Task UpdateCandidateId(string userId, string fileId, string type);
        Task<MemoryStream> GetAssessmentReportDataExcel(AssessmentReportViewModel model);
        Task ChangeUserProfilePhoto(string photoId, string userId);
        Task UpdatePreferredLanguage(string surveyScheduleId, string lang, string surveyResultId);
        Task<IList<IdNameViewModel>> GetIndicatorList(string levelId = null);
        Task<AssessmentInterviewViewModel> GetserviceScore(string ServiceID);

        Task<List<InterViewQuestionViewModel>> GetInterviewAssessmentQuestions(string userId, string Tableid);
        Task<bool> CalculateCaseStudyResultScore(string userId);
        Task<bool> CalculateQuestionaireResultScore(string UserId);

        Task<bool> UpdateInterViewScore(string Id, string Scores, string ActualStartDate, string ActualEndDate);

        Task<IdNameViewModel> GetCanidateId(string UserId);
        Task<IList<IdNameViewModel>> GetCompetencyList();
        Task<bool> UpdateAssessmentArchived(string serviceIds, bool archive = false);
        Task<IdNameViewModel> Getinterviewid(string ServiceId);
        Task<List<AssessmentCalendarViewModel>> GetCalendarScheduleList(string loggedInUserId = null, string role = null);
        Task<AssessmentPDFReportViewModel> GetAssessmentReportDataPDFForUser(string userId);
        Task<AssessmentViewModel> GetAssessmentReportByUserId(string userId, string masterCode);
        Task<AssessmentDetailViewModel> GetInterviewAssessmentReportByUserId(string userId);
        Task<AssessmentResultViewModel> GetManageAssessmentResult(string userId);
        Task<List<AssessmentResultViewModel>> GetManageAssessmentResultList(string userId);

        Task<List<AssessmentViewModel>> GetAssessmentSetList();
        Task<bool> DeleteAssessmentSet(string NoteId);
        Task<List<IdNameViewModel>> GetAssessmentListToSet(string TypeId);
        Task<List<AssessmentViewModel>> GetAssessmentSetAssessmentList(string AssessmentSetId);
        Task<bool> DeleteAssessmentSetAsessment(string NoteId);
        Task<AssessmentViewModel> GetAssessmentSetAssessmentByid(string NoteId);

        Task<IdNameViewModel> GetSquenceNoExistAssessmentSet(string AssessmentSetId, string SequenceNo);

        Task<IdNameViewModel> GetAssessmentExistAssessmentSet(string AssessmentSetId, string AssessmentId, string Id);
        Task<List<AssessmentInterviewViewModel>> GetAssessmentReportMinistryList(string SponsorId = null, string userId = null);
        Task<IdNameViewModel> GetSponsorDetailsByUserId(string UserId);
        Task<string> GetUserIdentificationByUserId(string userId);
        Task<List<AssessmentInterviewViewModel>> GetInterviewList(string UserID, string status);
        Task<List<AssessmentDetailViewModel>> GetAssessmentListByUserId(string userId, string statusCode, string typeId);
        Task<AssessmentCalendarViewModel> GetCalendarScheduleById(string id);
        Task<List<EmailViewModel>> GetSlotUserEmail(string slotId);
        Task<EmailViewModel> GetEmailById(string emailId);

        Task<IdNameViewModel> GetIndicatorbyName(string Name);
        Task<IdNameViewModel> GetCompetencybyName(string Name);
        Task<IdNameViewModel> GetAssessmentbyName(string Name);
        Task<List<InterViewQuestionViewModel>> GetQuestionListofUser(string UserId, string ServiceId);
        Task<SurveyScheduleViewModel> GetSurveyScheduleById(string surveyId);
        Task<List<SurveyScheduleViewModel>> GetSurveyScheduleList(string surveyId, string status);
        Task<SurveyScheduleViewModel> GetSurveyScheduleDetails(string surveyScheduleId);
        Task<SurveyTopicViewModel> GetSurveyAssessmentQuestion(string serviceId, string currTopicId, string lang);
        Task<bool> UpdateSurvey(string serviceId, string nextQTopicId = null, DateTime? startDate = null);
        Task<SurveyTopicViewModel> GetSurveyQuestion(string serviceId);
        Task SubmitSurveyAnswer(SurveyTopicViewModel model, bool isSubmit, string nextTopicId);
        Task ExecuteSurveyForUsers(string NoteId, string PortalId);
        Task GenerateSurveyDetails(string data);
        Task<SurveyScheduleViewModel> GetSurveyDetails(string surveyScheduleUserId, string surveyCode = null);
        Task<SurveyScheduleViewModel> GetSurveyClosingInstruction(string surveyId);
        Task<List<SurveyScheduleViewModel>> GetSurveyDetail(string id);
        Task<List<SurveyQuestionViewModel>> GetSurveyResult(string surveyResultId, string lang);
        //Task<SurveyScheduleViewModel> GetSurveyDetailsByCode(string surveyCode);
        Task<CommandResult<List<SurveyScheduleViewModel>>> GetValidateSurveyScheduleUserDetails(string surveyId, string userDetails, string surveyScheduleId);
        Task<ServiceTemplateViewModel> GetSurveyResultDetails(string surveyId, string surveyScheduleUserId, string surveyUserId);

        Task<MemoryStream> GetBusinessUserSurveyReportDataExcel();
        Task<MemoryStream> GetHRUserSurveyReportDataExcel();
        Task<SurveyScheduleViewModel> GenerateDummySurveyDetails(string surveyScheduleId, int count, string userId = null);
        Task<SurveyScheduleViewModel> AssignDummySurveyToUser(string surveyScheduleId, string userId);
        Task<long> GetSurveyUserCount(string prefix);
    }
}
