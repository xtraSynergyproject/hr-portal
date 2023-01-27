using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RecruitmentCandidateElementInfoViewModel : RecruitmentCandidateElementInfo
    {
        public string AgencyId { get; set; }
        public string OfferDesigination { get; set; }
        public string OfferGrade { get; set; }
        public string GaecNo { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string OfferSignedBy { get; set; }
        public string ElementName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplcationStateName { get; set; }
        public string FinalOfferReference { get; set; }
        public string Nationality { get; set; }
        public string PayId { get; set; }
        public string JsonPayElement { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ContractStartDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? JoiningNotLaterThan { get; set; }
        public bool? IsTrainee { get; set; }
        public string TravelOriginAndDestination { get; set; }

        public string VehicleTransport { get; set; }
        public long? AnnualLeave { get; set; }
        public long? ServiceCompletion { get; set; }
       
        public bool? IsLocalCandidate { get; set; }
        public bool? SalaryRevision { get; set; }
        public double? SalaryRevisionAmount { get; set; }
        public double? SalaryOnAppointment { get; set; }
        public string VisaCategory { get; set; }
        public string AccommodationId { get; set; }
        public string AccommodationValue { get; set; }

        public bool? IsHr { get; set; }
        public string TaskStatus { get; set; }


    }
}
