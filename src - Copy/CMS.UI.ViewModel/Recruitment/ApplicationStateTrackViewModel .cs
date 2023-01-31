using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ApplicationStateTrackViewModel : ApplicationStateTrack
    {
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ShortListByHrDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ShortListByHmDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? InterviewCompletedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? IntentToOfferSentDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? MedicalCompletedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? VisaAppointmentTakenDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? BiometricCompletedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? VisaApprovedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? VisaSentToCandidateDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? FlightTicketsBookedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? CandidateArrivedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? JoinedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? FinalOfferAcceptedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? UnReviewedDate { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? FinalOfferSentDate { get; set; }
        public string ShortListByHrUpdatedBy { get; set; }
        public string ShortListByHmUpdatedBy { get; set; }
        public string InterviewCompletedUpdatedBy { get; set; }
        public string IntentToOfferSentUpdatedBy { get; set; }
        public string MedicalCompletedUpdatedBy { get; set; }
        public string VisaAppointmentTakenUpdatedBy { get; set; }
        public string BiometricCompletedUpdatedBy { get; set; }
        public string VisaApprovedUpdatedBy { get; set; }
        public string VisaSentToCandidateUpdatedBy { get; set; }
        public string FlightTicketsBookedUpdatedBy { get; set; }
        public string CandidateArrivedUpdatedBy { get; set; }
        public string JoinedUpdatedBy { get; set; }
        public string FinalOfferAcceptedUpdatedBy { get; set; }
        public string UnReviewedUpdatedBy { get; set; }
        public string FinalOfferSentUpdatedBy { get; set; }
        public string FullName { get; set; }
        [DisplayFormat(DataFormatString =ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? AppliedDate { get; set; }
        public string ChangedByName { get; set; }
        public string StateCode { get; set; }
        public string ShortListByHrStateId { get; set; }
        public string ShortListByHmStateId { get; set; }
        public string InterviewCompletedStateId { get; set; }
        public string IntentToOfferSentStateId { get; set; }
        public string MedicalCompletedStateId { get; set; }
        public string VisaAppointmentTakenStateId { get; set; }
        public string BiometricCompletedStateId { get; set; }
        public string VisaApprovedStateId { get; set; }
        public string VisaSentToCandidateStateId { get; set; }
        public string FlightTicketsBookedStateId { get; set; }
        public string CandidateArrivedStateId { get; set; }
        public string JoinedStateId { get; set; }
        public string FinalOfferAcceptedStateId { get; set; }
        public string UnReviewedStateId { get; set; }
        public string FinalOfferSentStateId { get; set; }

    }
}
