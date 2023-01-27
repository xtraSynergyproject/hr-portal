using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class BLSVisaAppointmentViewModel : ServiceTemplateViewModel
    {
        public string LegalLocationId { get; set; }
        public string LocationName { get; set; }
        public string ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string AppointmentDateText { get; set; }
        public string AppointmentSlot { get; set; }
        public string AppointmentSlotId { get; set; }
        public string AppointmentDateSlotText { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string CurrentContactNumber { get; set; }
        public string CurrentContactNumberText { get; set; }
        public string IndiaContactNumber { get; set; }
        public string PassportAlphabet { get; set; }
        public string PassportDigits { get; set; }
        public string PassportNumber { get; set; }
        public string PassportCountryId { get; set; }
        public string ApplicantEmail { get; set; }
        public string Description { get; set; }
        public string FileNumber { get; set; }
        // public string CaptchaId { get; set; }
        //public string CaptchaText { get; set; }
        public string AppointmentStatusId { get; set; }
        public string AppointmentStatusName { get; set; }
        public string AppointmentStatusCode { get; set; }
        public string VisaTypeId { get; set; }
        public string VisaTypeName { get; set; }
        public string ImageId { get; set; }
        public string ApplicantPhotoId { get; set; }
        public string PhotoId { get; set; }
        public string Photo { get; set; }
        public string AppointmentFor { get; set; }
        public string CenterId { get; set; }
        public DateTime? TravelDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IsVisaIssuedEarlier { get; set; }
        public string AppointmentCategoryId { get; set; }
        public string AppointmentCategoryName { get; set; }
        public string VerificationCode { get; set; }
        public int ApplicantsNo { get; set; }
        public string nextSectionId { get; set; }
        public string IssuePlace { get; set; }
        public string PassportType { get; set; }
        public bool IsSMS { get; set; }
        public bool IsPhotograph { get; set; }
        public string AppointmentAddress { get; set; }
        public string QRCodeId { get; set; }
        public string QRCode { get; set; }
        public string BarCodeId { get; set; }
        public string BarCode { get; set; }
        public string LocationLatitude { get; set; }
        public string LocationLongitude { get; set; }
        public string LocationImageId { get; set; }
        public string LocationImage { get; set; }
        public string FirstName { get; set; }
        public string ApplicantsDetailsList { get; set; }
        public string ApplicantsDetailsList1 { get; set; }
        public string TotalAmount { get; set; }
        public string BLSAPPLICANTDETAILS { get; set; }

        public string SurnameAtBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string NationalityName { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationServiceId { get; set; }
        public string CurrentTaskId { get; set; }
        public string CurrentTaskTemplate { get; set; }
        public string AppointmentId { get; set; }
        public string ApplicationStatusCode { get; set; }
        public string ApplicationStatusId { get; set; }
        public string VisaApplicationServiceId { get; set; }
        public string VisaApplicationServiceTemplateCode { get; set; }
        public string NtsNoteId { get; set; }
        public List<BLSApplicantViewModel> blsApplicantList { get; set; }
        public List<BLSVisaAppointmentViewModel> BlsAppointmentList { get; set; }
        public int MaximumAllowedDays { get; set; }
        public string WeekDays { get; set; }
        public string Holidays { get; set; }
        public string FullSlot { get; set; }
        public string PaymentReferenceNo { get; set; }
        public string VisaAppointmentServiceId { get; set; }
        public long? ApplicantsCount { get; set; }
        public string ValueAddedServices { get; set; }
        public string ServiceNo { get; set; }
    }

    public class BLSCustomerViewModel
    {
        public string UserId { get; set; }
        public string LocationId { get; set; }
    }

    public class VisaTypeViewModel
    {
        public string VisaType { get; set; }
        public string Id { get; set; }
        public string VisaTypeCode { get; set; }
        public string AppointmentFee { get; set; }
        public string VisaFee { get; set; }
    }

    public class BLSApplicantViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AppointmentSlot { get; set; }
        public string PassportNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string PassportTypeId { get; set; }
        public string PassportCountryId { get; set; }
        public string IssuePlace { get; set; }
        public string ParentId { get; set; }
        public string Id { get; set; }
        public string AppointmentId { get; set; }
        public string ApplicantEmail { get; set; }
        public string CurrentContactNumber { get; set; }
        public string IssueDateT { get; set; }
        public string ExpiryDateT { get; set; }
        public string DateOfBirthT { get; set; }
        public string ApplicationServiceId { get; set; }
        public string ApplicationStatusCode { get; set; }
    }

    public class ValueAddedServicesViewModel
    {
        public string ServiceType { get; set; }
        public int ServiceCharges { get; set; }
        public string ServiceCode { get; set; }
        public string Id { get; set; }
        public bool IsSelected { get; set; }
        public string VASId { get; set; }
        public double Rate { get; set; }
        public int? Quantity { get; set; }
        public double Total { get; set; }
        public string ParentId { get; set; }
        public string VASDescription { get; set; }
    }

    public class BLSAPiViewModel
    {
        public string Api { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class BLSApplicationStatusViewModel
    {
        public string Status { get; set; }
        public string Comment { get; set; }
        public string UpdatedById { get; set; }
        public string ClientIp { get; set; }
        public string ClientMacId { get; set; }
        public string Id { get; set; }
    }

}
