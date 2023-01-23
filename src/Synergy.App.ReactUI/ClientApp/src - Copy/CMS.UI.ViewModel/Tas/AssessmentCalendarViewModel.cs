using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kendo.Mvc.UI;
using System;
namespace CMS.UI.ViewModel
{
    public class AssessmentCalendarViewModel : ISchedulerEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormat)]
        public DateTime Start { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateTimeFormat)]
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public long? Duration { get; set; }
        public string Url { get; set; }
        public string JobName { get; set; }
        public string JobId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string PreferredLanguage { get; set; }
        public string PreferredLanguageId { get; set; }
        public string PreferredLanguageText { get; set; }
        public string TechnicalAssessmentName { get; set; }
        public string MonitoringTypeId { get; set; }
        public string AssessmentScheduledStartDate { get; set; }

        public string TechnicalAssessmentUrl { get; set; }

        public string CaseStudyName { get; set; }

        public string CaseStudyScheduledStartDate { get; set; }

        public string CaseStudyUrl { get; set; }

        public string MinistryName { get; set; }
        public string JobTitle { get; set; }
        public string MobileNo { get; set; }
        public string Proctor { get; set; }
        public string Interviewer { get; set; }
        public string LineManager { get; set; }

        public string SurveyName { get; set; }

        public string SurveyScheduledStartDate { get; set; }

        public string SurveyUrl { get; set; }

        public string AssessmentInterviewDuration { get; set; }
        public string AssessmentInterviewUrl { get; set; }

        public string AssessmentInterviewPanel { get; set; }
        public string InterviewScheduledStartDate { get; set; }
        public string Color { get; set; }
        public string CandidateName { get; set; }
        public string CandidateId { get; set; }
        public string AssessmentTypeShort { get; set; }
        public string UrlShort { get; set; }
        public string ScheduleId { get; set; }
        public string MinistryId { get; set; }
        public string InterviewPanelId { get; set; }
        public string ProctorId { get; set; }
        public string AttachmentId { get; set; }
        public bool? IsAssessmentCreated { get; set; }
        public string AssessmentSetId { get; set; }
        public AssessmentScheduleTypeEnum? SlotType { get; set; }

        public string Email { get; set; }
        public StatusEnum? Status { get; set; }
        public string Remarks { get; set; }
        public string AssessmentSet { get; set; }
        public string AssessmentNames { get; set; }
        public string SlotTypeText { get; set; }
        public string SlotStatusText { get; set; }
        public bool? IsCandidateAvailabilityTaskCreated { get; set; }
        public bool? IsProctorAvailabilityTaskCreated { get; set; }
        public bool? IsInterviewerAvailabilityTaskCreated { get; set; }
        public string CandidateAvailableTime { get; set; }
        public string ProctorAvailableTime { get; set; }
        public string InterviewerAvaiableTime { get; set; }
        public string AssessmentId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string InterviewWeightage { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentTypeName { get; set; }
        public string Location { get; set; }
    }
}
