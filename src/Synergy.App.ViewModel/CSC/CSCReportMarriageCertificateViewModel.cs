using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class CSCReportMarriageCertificateViewModel
    {
        public string ServiceId { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime? ElectronicApplicationDate { get; set; }
        public string ElectronicApplicationDateText { get; set; }
        public string ChoiceCenter { get; set; }
        public double? NetAmount { get; set; }
        public string RegistrationNumber { get; set; }
        public string GroomName { get; set; }
        public string GroomFatherName { get; set; }
        public int GroomAge { get; set; }
        public string GroomAddress { get; set; }
        public string GroomNameHi { get; set; }
        public string GroomFatherNameHi { get; set; }
        public string GroomAddressHi { get; set; }
        public string GroomImageId { get; set; }
        public string GroomImage { get; set; }
        public string BrideName { get; set; }
        public string BrideFatherName { get; set; }
        public int BrideAge { get; set; }
        public string BrideAddress { get; set; }
        public string BrideNameHi { get; set; }
        public string BrideFatherNameHi { get; set; }
        public string BrideAddressHi { get; set; }
        public string BrideImageId { get; set; }
        public string BrideImage { get; set; }
        public string GroomBrideImageId { get; set; }
        public string GroomBrideImage { get; set; }
        public DateTime? MarriageDate { get; set; }
        public string MarriageDateText { get; set; }
        public string Section1 { get; set; }
        public string Section1Hi { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameHi { get; set; }
        public string MunicipalOfficeName { get; set; }
        public string MunicipalOfficeNameHi { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedDateText { get; set; }
        public string AuthoritySignature { get; set; }
        public string AuthorityName { get; set; }
        public string RequestedByUserId { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintDateText { get; set; }
        public string GovtLogo { get; set; }
        public string SealLogo { get; set; }
        
    }
}
