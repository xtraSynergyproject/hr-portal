using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class AssessmentViewModel : ViewModelBase
    {

        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string SubjectAr { get; set; }
        public string DescriptionAr { get; set; }
        public long? FileId { get; set; }
        public long? FileIdAr { get; set; }
        public long? SerialNo { get; set; }
        public string Option { get; set; }
        public string ScoreOption { get; set; }
        public string OptionAr { get; set; }
        public bool? IsAnswer { get; set; }
        public bool? IsAnswerAr { get; set; }
        public long? NoteId { get; set; }
        public long? SequenceNo { get; set; }
        public long? TotalCount { get; set; }
        public long? ServiceId { get; set; }
        public long? ParentServiceId { get; set; }
        public AssessmentTypeEnum? AssessmentType { get; set; }
        public long? NextId { get; set; }
        public long? PreviousId { get; set; }
        public long? FirstId { get; set; }
        public long? LastId { get; set; }
        public string JobTitle { get; set; }

        public long? AnswerMultipleFieldValueId { get; set; }

        public long? MultipleFieldValueId { get; set; }

        public List<AssessmentViewModel> OptionList { get; set; }

        public long? TimeElapsed { get; set; }
        public long? TimeElapsedSec { get; set; }
        public long? AssessmentDuration { get; set; }
        public long? CurrentQuestionId { get; set; }

        public long? OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ScheduledStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ScheduledEndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ActualStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ActualEndDate { get; set; }
        public NtsActionEnum? ServiceTemplateAction { get; set; }

        public List<AssessmentAnswerViewModel> AnswerList { get; set; }
        public string CandidateAnswerComment { get; set; }
        public Language? PreferredLanguage { get; set; }
        public bool? IsAssessmentStopped { get; set; }
        public string AssessmentStatus { get; set; }
        public long? InterviewServiceId { get; set; }
        public long? UserId { get; set; }

        public string ExcelTemplate { get; set; }

        public string Data { get; set; }

        public double? Weightage { get; set; }

        public double? Score { get; set; }
        public string InterviewerComment { get; set; }
        public long? AnswerId { get; set; }
        public string CandidateAnswer { get; set; }
        public string AssessmentNo { get; set; }
        public bool? IsAdmin { get; set; }
        public string PanelTeamName { get; set; }
        public string AssessmentInterviewUrl { get; set; }

        public bool? ShowStartButton
        {
            get
            {
                if (ScheduledStartDate.HasValue)
                {
                    if (DateTime.Now >= ScheduledStartDate.Value.AddHours(-1))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }
        public long? UserReportId { get; set; }
        public string Mobile { get; set; }
        public string MinistryName { get; set; }
        public string InterviewUrl { get; set; }
        public string MCQStatus { get; set; }
        public string CSStatus { get; set; }
        public string AssessmentZoomUrl { get; set; }
        public string CaseStudyZoomUrl { get; set; }
        public long? ProctorUserId { get; set; }
        public long? PhotoId { get; set; }
        public long? IdentificationId { get; set; }
        public long? RecordingId { get; set; }
        public long? JobRequestId { get; set; }

        public int? EmailCount { get; set; }
        public bool? IsRowArchived { get; set; }
        public bool? IsArchived { get; set; }
        public string ScoreText { get; set; }
        public int? MaximumOption { get; set; }
    }
    public class AssessmentAnswerViewModel : ViewModelBase
    {
        public string Question { get; set; }
        public string QuestionAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string RightAnswer { get; set; }
        public string CandidateAnswer { get; set; }
        public string CandidateAnswerAr { get; set; }
        public string CandidateAnswerComment { get; set; }
        public string CandidateAnswerCommentAr { get; set; }
        public long SeraillNo { get; set; }
        public BoolStatus Accurate { get; set; }

        public string Option1 { get; set; }
        public string Option1Ar { get; set; }
        public string Option2 { get; set; }
        public string Option2Ar { get; set; }
        public string Option3 { get; set; }
        public string Option3Ar { get; set; }
        public string Option4 { get; set; }
        public string Option4Ar { get; set; }

        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string Option9 { get; set; }
        public string Option10 { get; set; }

        public double? Score { get; set; }
        public string InterviewerComment { get; set; }
        public long? AnswerId { get; set; }
        public long? FileId { get; set; }

        public double? ScoreOption { get; set; }
        public string ScoreOption1 { get; set; }
        public string ScoreOption2 { get; set; }
        public string ScoreOption3 { get; set; }
        public string ScoreOption4 { get; set; }
        public string ScoreOption5 { get; set; }
        public string ScoreOption6 { get; set; }
        public string ScoreOption7 { get; set; }
        public string ScoreOption8 { get; set; }
        public string ScoreOption9 { get; set; }
        public string ScoreOption10 { get; set; }

    }

    public class AssessmentInterviewViewModel : ViewModelBase
    {
        public long NoteId { get; set; }
        public long? SeraillNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Section { get; set; }
        public string Remarks { get; set; }
        public string Question { get; set; }
        public string CandidateAnswer { get; set; }
        public string Score { get; set; }
        public string InterviewerComment { get; set; }
    }
    public class AssessmentQuestionOptionViewModel
    {
        public long NoteId { get; set; }
        public long? SeraillNo { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string Option9 { get; set; }
        public string Option10 { get; set; }
        public string RightAnswer { get; set; }
        public string AnswerComment { get; set; }

        public string ScoreOption1 { get; set; }
        public string ScoreOption2 { get; set; }
        public string ScoreOption3 { get; set; }
        public string ScoreOption4 { get; set; }
        public string ScoreOption5 { get; set; }
        public string ScoreOption6 { get; set; }
        public string ScoreOption7 { get; set; }
        public string ScoreOption8 { get; set; }
        public string ScoreOption9 { get; set; }
        public string ScoreOption10 { get; set; }

        public string QuestionAr { get; set; }
        public string OptionAr1 { get; set; }
        public string OptionAr2 { get; set; }
        public string OptionAr3 { get; set; }
        public string OptionAr4 { get; set; }
        public string OptionAr5 { get; set; }
        public string OptionAr6 { get; set; }
        public string OptionAr7 { get; set; }
        public string OptionAr8 { get; set; }
        public string OptionAr9 { get; set; }
        public string OptionAr10 { get; set; }
        public string RightAnswerAr { get; set; }
        public string AnswerCommentAr { get; set; }

    }
    public class AssessmentSetAssessmentViewModel : AssessmentViewModel
    {
        public long? AssessmentItemId { get; set; }
        public long? RowId { get; set; }
        public long? AssessmentSetId { get; set; }
        public string FieldName { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }
    public class AssessmentResultViewModel : AssessmentViewModel
    {
        public string ReportGroupName { get; set; }
        public string ReportName { get; set; }
        public double? ReportWeightage { get; set; }
        public double? ReportScore { get; set; }
        public bool? IncludeInReport { get; set; }
        public long? SlotId { get; set; }
        public string ReportSummary { get; set; }
        public string AssessorComment { get; set; }
        public string ReportColor { get; set; }
        public string Organization { get; set; }
        public string Type { get; set; }
    }

    public class AssessmentAllQuestionViewModel
    {
        public long NoteId { get; set; }
        public long? ArabicNoteId { get; set; }
        public string JobTitle { get; set; }
        public string Subject { get; set; }
        public long? SeraillNo { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        //public string Option5 { get; set; }
        //public string Option6 { get; set; }
        //public string Option7 { get; set; }
        //public string Option8 { get; set; }
        //public string Option9 { get; set; }
        //public string Option10 { get; set; }
        public string RightAnswer { get; set; }
        public string AnswerComment { get; set; }

        public string QuestionAr { get; set; }
        public string OptionAr1 { get; set; }
        public string OptionAr2 { get; set; }
        public string OptionAr3 { get; set; }
        public string OptionAr4 { get; set; }
        //public string OptionAr5 { get; set; }
        //public string OptionAr6 { get; set; }
        //public string OptionAr7 { get; set; }
        //public string OptionAr8 { get; set; }
        //public string OptionAr9 { get; set; }
        //public string OptionAr10 { get; set; }
        public string RightAnswerAr { get; set; }
        public string AnswerCommentAr { get; set; }

    }
}
