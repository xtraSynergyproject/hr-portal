using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Synergy.App.ViewModel
{
    public class RecDashboardViewModel : DataModelBase
    {          
        [Display(Name = "Organization Unit")]
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string JobAdvId { get; set; }
        public string JobName { get; set; }

        public long? Requirement { get; set; }
        public long? Seperation { get; set; }
        public long? Available { get; set; }
        [Display(Name = "Sub Contract")]
        public long? Planning { get; set; }
        public long? Transfer { get; set; }
        public long? Balance { get; set; }        
        public long? OrgUnit { get; set; }
        public long? PlanningUnit { get; set; }
        public long? Hr { get; set; }
        public long? Active { get; set; }
        public long? InActive { get; set; }
        public long? ShortlistedByHr { get; set; }
        public long? ShortlistedForInterview { get; set; }
        public long? WaitlistByHM { get; set; }
        public long? InterviewCompleted { get; set; }
        public long? NoOfApplication { get; set; }
        public long? IntentToOfferSent { get; set; }
        public long? IntentToOfferAccepted { get; set; }
        public long? FinalOfferAccepted { get; set; }
        public long? MedicalCompleted { get; set; }
        public long? VisaAppointmentTaken { get; set; }
        public long? BiometricCompleted { get; set; }
        public long? VisaApproved { get; set; }
        public long? VisaSentToCandidates { get; set; }
        public long? FightTicketsBooked { get; set; }
        public long? CandidateArrived { get; set; }
        public long? CandidateJoined { get; set; }
        public string CreatedByName { get; set; }
        public long? UnReviewed { get; set; }
        public long? VisaTransfer { get; set; }
        public long? WorkVisa { get; set; }
        public long? BusinessVisa { get; set; }
        public long? WorkerJoined { get; set; }
        public long? WorkPermit { get; set; }
        public long? WorkerPool { get; set; }
        public long? PostWorkerJoined { get; set; }
        public long? PostStaffJoined { get; set; }
        public long? Joined { get; set; }
        public long? DirectHiring { get; set; }
        public DataTable GridTable { get; set; }
        public string UserRoleId { get; set; }
    }
}
