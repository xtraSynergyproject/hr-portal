using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class AssessmentCategoryTypeViewModel : ViewModelBase
    {
        public long? TechnicalassessmentId { get; set; }
        public long? AssessmentInterviewId { get; set; }
        public long? CaseStudyassessmentId { get; set; }
        public long? SurveyassessmentId { get; set; }
        public IList<AssessmentDetailViewModel> TechnicalAssessment { get; set; }
        public AssessmentDetailViewModel AssessmentInterview { get; set; }
        public AssessmentDetailViewModel CaseStudyAssessment { get; set; }
        public AssessmentDetailViewModel SurveyAssessment { get; set; }
        public DateTime? TechnicalAssessmentScheduledStartDate { get; set; }
        public DateTime? TechnicalAssessmentScheduledEndDate { get; set; }
        public string TechnicalAssessmentUrl { get; set; }
        public DateTime? CaseStudyAssessmentScheduledStartDate { get; set; }
        public DateTime? CaseStudyAssessmentScheduledEndDate { get; set; }
        public string CaseStudyAssessmentUrl { get; set; }
        public DateTime? SurveyAssessmentScheduledStartDate { get; set; }
        public DateTime? SurveyAssessmentScheduledEndDate { get; set; }
        public string SurveyAssessmentUrl { get; set; }
        public DateTime? AssessmentInterviewScheduledStartDate { get; set; }
        public DateTime? AssessmentInterviewScheduledEndDate { get; set; }
        public string AssessmentInterviewUrl { get; set; }
        public Language PreferredLanguage { get; set; }
    }
    public class AssessmentDetailViewModel : ViewModelBase
    {
        public string AssessmentName { get; set; }
        public AssessmentTypeEnum? Type { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? ScheduledEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int? TimeElapsed { get; set; }
        public int? CurrentQuestionId { get; set; }
        public string AssessmentStatus { get; set; }
        public Language? PreferredLanguage { get; set; }
        public string AssessmentUrl { get; set; }
        public long? ServiceId { get; set; }
        public bool? IsAssessmentStopped { get; set; }
        public int? AssessmentInterviewDuratrion { get; set; }
        public double? AssessmentInterviewWeightage { get; set; }

        public string UserName { get; set; }
        public string JobName { get; set; }

        public double? Score { get; set; }

        public int? Duration { get; set; }

        public int? SequenceNo { get; set; }

        public string Email { get; set; }
    }
}
