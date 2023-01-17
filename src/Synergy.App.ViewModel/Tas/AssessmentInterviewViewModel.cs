using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.ViewModel
{
   public class AssessmentInterviewViewModel
    {
        public string Id { get; set; }
        public string NoteId { get; set; }
        public string OwnerUserId { get; set; }
        public string ServiceId { get; set; }

        public bool? IsRowArchived { get; set; }
        public string PanelTeamName { get; set; }

        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? ScheduledEndDate { get; set; }

        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string OwnerUserName { get; set; }
        public string Email { get; set; }
        public string AssessmentInterviewUrl { get; set; }
        public string Subject { get; set; }
        public string AssessmentStatus { get; set; }
        public string IsArchived { get; set; }

        public bool IsAssessmentStopped { get; set; }
        public string  TableId { get; set; }

        public string Data { get; set; }
        public string Score { get; set; }
        public string PreferredLanguage { get; set; }
        public string Mobile { get; set; }
        public string MinistryName { get; set; }
        public string AssessmentZoomUrl { get; set; }
        public string CaseStudyZoomUrl { get; set; }
        public string MCQStatus { get; set; }
        public string CSStatus { get; set; }

        public string PhotoId { get; set; }

        public string DisplayScheduledStartDate { get; set; }
        public string DisplayScheduledEndDate { get; set; }
        public string DisplayActualStartDate { get; set; }
        public string DisplayActualEndDate { get; set; }
        public string UserId { get; set; }
        public string InterviewScheduleId { get; set; }
        public string JobTitle { get; set; }

        public string CandidateCVId { get; set; }
        public string InterviewSheetId { get; set; }

        public string Source { get; set; }

    }
}
