using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ManpowerRecruitmentSummaryViewModel :ManpowerRecruitmentSummary
    {  
      //  [Required]
      //  [Display(Name ="Job Title")]
       // public string JobId { get; set; }
        public string JobTitle { get; set; }
     //   [Required]
      //  [Display(Name = "Organization Unit")]
       // public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string JobAdvertisementId { get; set; }
        public string JobDescriptionId { get; set; }
        public string JobAdvertisementName { get; set; }
        public long? SubContract { get; set; }

        //  public long? Requirement { get; set; }
        //  public long? Separation { get; set; }
        //  public long? Available { get; set; }
        //  [Display(Name = "Sub Contract")]
        //  public long? Planning { get; set; }
        //  public long? Transfer { get; set; }
        //  public long? Balance { get; set; }       
        public long? OrgUnit { get; set; }
        public long? PlanningUnit { get; set; }
        public long? Hr { get; set; }
        public long? Active { get; set; }
        public long? InActive { get; set; }
        public long? ShortlistedByHr { get; set; }
        public long? DirectHiring { get; set; }
        //public long? ShortlistedByHrCalculated { get; set; }
        public long? ShortlistedForInterview { get; set; }
        //public long? ShortlistedForInterviewCalculated { get; set; }
        public long? InterviewCompleted { get; set; }
        //public long? InterviewCompletedCalculated { get; set; }
        //public long? NoOfApplication { get; set; }
        public long? IntentToOffer { get; set; }
        //public long? IntentToOfferAccepted { get; set; }
        public long? FinalOfferAccepted { get; set; }
        //public long? FinalOfferAcceptedCalculated { get; set; }
        //public long? MedicalCompleted { get; set; }
        //public long? VisaAppointmentTaken { get; set; }
        //public long? BiometricCompleted { get; set; }
        //public long? VisaApproved { get; set; }
        //public long? VisaSentToCandidates { get; set; }
        //public long? FightTicketsBooked { get; set; }
        //public long? CandidateArrived { get; set; }
        public long? CandidateJoined { get; set; }
        //public long? CandidateJoinedCalculated { get; set; }
        public string CreatedByName { get; set; }
        public string JobadvtId { get; set; }

        public string ActionName { get; set; }
        
        //public long? UnReviewed { get; set; }
        public long? VisaTransfer { get; set; }
        public long? BusinessVisa { get; set; }
        public long? WorkerVisa { get; set; }
        public long? WorkPermit { get; set; }
        public long? FinalOffer { get; set; }       
        public long? WorkerJoined { get; set; }
        public long? WorkerPool { get; set; }
        public long? Joined { get; set; }
        public long? PostStaffJoined { get; set; }
        public long? PostWorkerJoined { get; set; }

        public long? Count { get; set; }

        public string ManpowerType { get; set; }

        public string ServiceId { get; set; }
    }
}
