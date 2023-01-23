using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;


namespace Synergy.App.ViewModel
{
    public class JobAdvertisementViewModel : JobAdvertisement
    {
        //public string ManpowerRecruitmentSummaryId { get; set; }
        //public string JobId { get; set; }
        //public string Description { get; set; }
        //public string Qualification { get; set; }
        //public string NationalityId { get; set; }
        //public string LocationId { get; set; }
        //public string NoOfPosition { get; set; }
        //public string Experience { get; set; }
        public string JobAdvId { get; set; }
        public string JobCategory { get; set; }
        public string JobDescription { get; set; }
        public string ManpowerType { get; set; }
        //public string Responsibilities { get; set; }
        //public DateTime? ExpiryDate { get; set; }
        //public DateTime? NeededDate { get; set; }
        public string JobAdvertisementStatus { get; set; }
        //public string RoleId { get; set; }
        public string ActionName { get; set; }
        public string JobName { get; set; }
        public string JobCriteria { get; set; }
        public string Skills { get; set; }
        public string OtherInformation { get; set; }
        public string SaveType { get; set; }
        public bool flag { get; set; }
        public string[] UserRoleCodes { get; set; }
        public string LocationName { get; set; }
        public string NationalityName { get; set; }
        public string QualificationName { get; set; }
        public string JobCategoryName { get; set; }
        public string ManpowerTypeName { get; set; }
        public string ManpowerTypeCode { get; set; }
        public string CandidateId { get; set; }
      //  public StatusEnum JobStatus { get; set; }
        public bool IsCandidateDetailsFilled { get; set; }
        public bool AlreadyApplied { get; set; }
        public bool IsBookmarked { get; set; }
        public string DraftId { get; set; }
        public string SubmitId { get; set; }
        public string ApprovalId { get; set; }
        public string Layout { get; set; }
        public string ShowJobDesc { get; set; }
        public long? Active { get; set; }
        public long? InActive { get; set; }

        public string JobadvtId { get; set; }
        // public Page Page { get; set; }
        public long? UnReviewed { get; set; }
      
        public long? ShortlistedByHrCalculated { get; set; }
     
        public long? ShortlistedForInterviewCalculated { get; set; }
        public long? InterviewCompletedCalculated { get; set; }
        public long? FinalOfferAcceptedCalculated { get; set; }
        public long? CandidateJoinedCalculated { get; set; }

        public long? DirectHiring { get; set; }
        public long? NoOfApplication { get; set; }
        public long? ShortlistedByHr { get; set; }
        public long? ShortlistedForInterview { get; set; }
        public long? InterviewCompleted { get; set; }
        public long? WaitlistByHM { get; set; }
        public long? IntentToOfferSent { get; set; }
        public long? FinalOfferSent { get; set; }
        public long? FinalOfferAccepted { get; set; }
        public long? VisaTransfer { get; set; }
        public long? MedicalCompleted { get; set; }
        public long? VisaAppointmentTaken { get; set; }
        public long? BiometricCompleted { get; set; }
        public long? VisaApproved { get; set; }
        public long? VisaSentToCandidates { get; set; }
        public long? FightTicketsBooked { get; set; }
        public long? CandidateArrived { get; set; }
        public long? CandidateJoined { get; set; }
        public long? WorkVisa { get; set; }
        public long? BusinessVisa { get; set; }
        public long? WorkerJoined { get; set; }
        public long? WorkPermit { get; set; }
        public long? WorkerPool { get; set; }
        public long? PostWorkerJoined { get; set; }
        public long? PostStaffJoined { get; set; }
        public long? Joined { get; set; }
        public DateTime? CreateDate { get; set; }
        public string HiringManagerId { get; set; }
        public string RecruiterId { get; set; }
        public long? Count { get; set; }
        public bool IsView { get; set; }
        public List<string> AgencyIds { get; set; } 
        public string[] Agencies { get; set; }
        public string JobAdvNoteId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceStatusCode { get; set; }

        public string userId { get; set; }
    }
}
