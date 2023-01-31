using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("JobAdvertisement", Schema = "rec")]
    public class JobAdvertisement : DataModelBase
    {       
        [ForeignKey("Job")]
        public string JobId { get; set; }
        //[ForeignKey("Organization")]
        //public string OrganizationId { get; set; }

        public string Description { get; set; }
        public string QualificationId { get; set; }
        [ForeignKey("Nationality")]
        public string NationalityId { get; set; }
        [ForeignKey("Location")]
        public string JobLocationId { get; set; }
        public long? NoOfPosition { get; set; }
        public long? Experience { get; set; }
        [ForeignKey("ListOfValue")]
        public string JobCategoryId { get; set; }
        //[ForeignKey("ListOfValue")]
        //public string ManpowerTypeId { get; set; }
        public string Responsibilities { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        [ForeignKey("UserRole")]
        public string RoleId { get; set; }
        public string ActionId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        //public string[] AgencyId { get; set; }
        public string AgencyId { get; set; }
        //public long? NoOfApplication { get; set; }
        //public long? ShortlistedByHr { get; set; }
        //public long? ShortlistedForInterview { get; set; }
        //public long? InterviewCompleted { get; set; }
        //public long? IntentToOfferSent { get; set; }
        //public long? FinalOfferSent { get; set; }
        //public long? FinalOfferAccepted { get; set; }
        //public long? VisaTransfer { get; set; }
        //public long? MedicalCompleted { get; set; }
        //public long? VisaAppointmentTaken { get; set; }
        //public long? BiometricCompleted { get; set; }
        //public long? VisaApproved { get; set; }
        //public long? VisaSentToCandidates { get; set; }
        //public long? FightTicketsBooked { get; set; }
        //public long? CandidateArrived { get; set; }
        //public long? CandidateJoined { get; set; }
    }
}
