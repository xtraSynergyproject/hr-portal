using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AssessmentDetailViewModel : NoteTemplateViewModel
    {
        public string AssessmentId { get; set; }
        public string ScheduledAssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentType { get; set; }
        public DateTime? ScheduledStartDate { get; set; }        
        public DateTime? ScheduledEndDate { get; set; }        
        public DateTime? ActualStartDate { get; set; }        
        public DateTime? ActualEndDate { get; set; }
        public string DisplayScheduledStartDate { get; set; }
        public string DisplayScheduledEndDate { get; set; }
        public string DisplayActualStartDate { get; set; }
        public string DisplayActualEndDate { get; set; }
        public string TimeElapsed { get; set; }
        public string TimeElapsedSec { get; set; }
        public string CurrentQuestionId { get; set; }
        public string CurrentTopicId { get; set; }
        public string AssessmentStatus { get; set; }
        public string PreferredLanguageId { get; set; }
        public string PreferredLanguageCode { get; set; }
        public string AssessmentUrl { get; set; }
        public string ServiceId { get; set; }
        public bool? IsAssessmentStopped { get; set; }
        public int? AssessmentInterviewDuratrion { get; set; }
        public double? AssessmentInterviewWeightage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string JobName { get; set; }

        public double? Score { get; set; }

        public int? Duration { get; set; }

        public int? SequenceNo { get; set; }

        public string Email { get; set; }

        public string Title { get; set; } 
        public string Question { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionDescriptionAr { get; set; }
        public string FileId { get; set; }
        public string FileIdAr { get; set; }
        public string SerialNo { get; set; }
        public string Option { get; set; }
        public string ScoreOption { get; set; }
        public string OptionAr { get; set; }
        public bool? IsAnswer { get; set; }
        public bool? IsAnswerAr { get; set; }       
        
        public long? TotalCount { get; set; }
       
        public string ParentServiceId { get; set; }        
        public string NextId { get; set; }
        public string PreviousId { get; set; }
        public string FirstId { get; set; }
        public string LastId { get; set; }
        public string JobTitle { get; set; }

        public string AnswerMultipleFieldValueId { get; set; }

        public string MultipleFieldValueId { get; set; }

        public List<AssessmentQuestionsOptionViewModel> OptionList { get; set; }                
        
        public int? AssessmentDuration { get; set; }
                              
        public NtsActionEnum? ServiceTemplateAction { get; set; }
        
        public string CandidateAnswerComment { get; set; }
               
        public string InterviewServiceId { get; set; }
        
        public string ExcelTemplate { get; set; }

        public string Data { get; set; }

        public double? Weightage { get; set; }

       
        public string InterviewerComment { get; set; }
        public string AnswerId { get; set; }
        public string CandidateAnswer { get; set; }
        public string AssessmentNo { get; set; }
        public bool? IsAdmin { get; set; }
        public string PanelTeamName { get; set; }
        public string AssessmentInterviewUrl { get; set; }
        public List<AssessmentAnswerViewModel> AnswerList { get; set; }

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
        public string UserReportId { get; set; }
        public string Mobile { get; set; }
        public string MinistryName { get; set; }
        public string InterviewUrl { get; set; }
        public string MCQStatus { get; set; }
        public string CSStatus { get; set; }
        public string AssessmentZoomUrl { get; set; }
        public string CaseStudyZoomUrl { get; set; }
        public string ProctorUserId { get; set; }
        public string PhotoId { get; set; }
        public string IdentificationId { get; set; }
        public string RecordingId { get; set; }
        public string JobRequestId { get; set; }

        public int? EmailCount { get; set; }
        public bool? IsRowArchived { get; set; }
        public bool? IsArchived { get; set; }
        public string ScoreText { get; set; }
        public int? MaximumOption { get; set; }
        public List<AssessmentDetailViewModel> AssessmentList { get; set; }

        public string TopicId { get; set; }
        public string TopicName { get; set; }
        public string SurveyScheduleUserId { get; set; }

        public int? TopicSequenceNo { get; set; }
    }
}
