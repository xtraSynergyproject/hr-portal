using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class CSCReportOBCCertificateViewModel
    {
        public string ApplicationReferenceNumber { get; set; }
        public DateTime? ElectronicApplicationDate { get; set; }
        public string ElectronicApplicationDateText { get; set; }
        public string ChoiceCenter { get; set; }
        public double? NetAmount { get; set; }
        public string RegistrationNumber { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantFatherName { get; set; }
        public string ApplicantResident { get; set; }
        public string ApplicantTehasil { get; set; }
        public string ApplicantDistrict { get; set; }
        public string ApplicantCaste { get; set; }
        public string ApplicantNameHi { get; set; }
        public string ApplicantFatherNameHi { get; set; }
        public string ApplicantResidentHi { get; set; }
        public string ApplicantTehasilHi { get; set; }
        public string ApplicantDistrictHi { get; set; }
        public string ApplicantCasteHi { get; set; }
        public string OBCSection1 { get; set; }
        public string OBCSection2 { get; set; }
        public string OBCSection3 { get; set; }
        public string OBCSection1Hi { get; set; }
        public string OBCSection2Hi { get; set; }
        public string OBCSection3Hi { get; set; }

        public DateTime? IssueDate { get; set; }
        public string IssueDateText { get; set; }
        public string AuthoritySignature { get; set; }
        public string AuthorityName { get; set; }
        public string ProvisionalAuthorityName { get; set; }
        public string FinalAuthorityName { get; set; }
        public string GovtLogo { get; set; }
        public string SealLogo { get; set; }
    }
}
