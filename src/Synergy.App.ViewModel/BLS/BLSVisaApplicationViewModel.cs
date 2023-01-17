using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class BLSVisaApplicationViewModel
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string SurnameatBirth { get; set; }
        public string FirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string CountryOfBirthId { get; set; }
        public string CurrentNationalityId { get; set; }
        public string OriginalNationalityId { get; set; }
        public string GenderId { get; set; }
        public string MaritalStatusId { get; set; }
        public string MinorDetails { get; set; }
        public string NationalIdNo { get; set; }
        public string ApplicantsEmailId { get; set; }
        public string TelephoneNo { get; set; }
        public string HomeAddress { get; set; }
        public string ResidenceInOtherCountry { get; set; }
        public string CurrentOccupation { get; set; }
        public string ResidencePermitNo { get; set; }
        public string EmployerDetails { get; set; }
        public DateTime? ResidenceValidUntil { get; set; }
        public string PurposeOfJourneyId { get; set; }
        public string PurposeOfStay { get; set; }
        public string MSOfMainDestination { get; set; }
        public string MSOfFirstEntry { get; set; }
        public string NumberOfEntriesId { get; set; }
        public string FingerPrintsCollectedPreviously { get; set; }
        public DateTime? CollectedDate { get; set; }
        public string VisaStickerNo { get; set; }
        public string FinalIssuedBy { get; set; }
        public DateTime? FinalValidFrom { get; set; }
        public DateTime? FinalUntil { get; set; }
        public string SurnameandFirstNameofIP { get; set; }
        public string DetailOfAccomodation { get; set; }
        public string AccomodationTelephoneNo { get; set; }
        public string NameOfInvitingCompany { get; set; }
        public string ContactPersonDetail { get; set; }
        public string CPTelephoneNo { get; set; }
        public string CostOfStay { get; set; }
        public string MeanOfTransportId { get; set; }
        public string SponserId { get; set; }
        public string ApplicationServiceId { get; set; }
        public string AppointmentId { get; set; }
        public string NtsNoteId { get; set; }
        public string ApplicationStatusId { get; set; }
        public string PassportFrontId { get; set; }
        public string PassportBackId { get; set; }
        public string FileNumber { get; set; }
        public string PhotographId { get; set; }
        public string FarwardedToMofa { get; set; }
        public string VisaStampingCompleted { get; set; }
    }
}
