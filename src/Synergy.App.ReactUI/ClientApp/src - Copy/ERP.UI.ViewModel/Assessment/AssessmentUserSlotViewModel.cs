using ERP.Utility;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AssessmentUserSlotViewModel 
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Url { get; set; }
        public Language? PreferredLanguage { get; set; }
        public string AssessmentInterviewUrl { get; set; }
        public string AssessmentInterviewDuratrion { get; set; }
        public string AssessmentInterviewScheduledStartDate { get; set; }

        public string EnableLanguageChange { get; set; }
        public string MinistryName { get; set; }
        public string JobTitle { get; set; }
        public string Proctor { get; set; }
        public long? ScheduleId { get; set; }
        public long? MinistryId { get; set; }
        public long? InterviewPanelId { get; set; } 
        public long? ProctorId { get; set; }
        public AssessmentScheduleTypeEnum? SlotType { get; set; }

        public long? AssessmentSetId { get; set; }

        public string AssessmentSet { get; set; }
        public bool? IsAssessmentCreated { get; set; }
        public BoolStatus? IsArchive { get; set; }

    }
}
