using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class AssessmentResultViewModel: NoteTemplateViewModel
    {
        public string AssessmentId { get; set; }
        public string ScheduledAssessmentId { get; set; }
        public string PreferredLanguageId { get; set; }
        public string UserId { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentType { get; set; }
        public DateTime? ScheduledStartDate { get; set; }        
        public DateTime? ScheduledEndDate { get; set; }        
        public DateTime? ActualStartDate { get; set; }        
        public DateTime? ActualEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string NtsNoteId { get; set; }
        public string DisplayScheduledStartDate { get; set; }
        public string DisplayScheduledEndDate { get; set; }
        public string DisplayActualStartDate { get; set; }
        public string DisplayActualEndDate { get; set; }
        public string TimeElapsed { get; set; }
        public string TimeElapsedSec { get; set; }
        public string CurrentQuestionId { get; set; }
        public string AssessmentStatus { get; set; }        
        public string PreferredLanguageCode { get; set; }
        public double? Score { get; set; }
        public int? Duration { get; set; }
        public string AssessmentUrl { get; set; }
        public string ServiceId { get; set; }
        public bool? IsAssessmentStopped { get; set; }
        public int? AssessmentInterviewDuratrion { get; set; }
        public double? AssessmentInterviewWeightage { get; set; }   
        public string ServiceStatusCode { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationLogo { get; set; }
        public string UserName { get; set; }
        public string JobName { get; set; }
        public string CurrentDateText { get; set; }
        public string ReportSummary { get; set; }
        public string AssessorComment { get; set; }
        public string Type { get; set; }
        public string ReportName { get; set; }
        public string ReportGroupName { get; set; }
        public int? ReportWeightage { get; set; }
        public double? ReportScore { get; set; }
        public bool? IncludeInReport { get; set; }
        public string ReportColor { get; set; }
    }
}
