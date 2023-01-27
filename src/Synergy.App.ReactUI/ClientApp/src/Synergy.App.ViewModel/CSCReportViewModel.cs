using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class CSCReportViewModel 
    {
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string LocalAreaName { get; set; }
        public string TehasilBlockName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string Name { get; set; }
        public string GenderName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateText { get; set; }
        public string BirthPlace { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentAddress2 { get; set; }
        public string CurrentAddress3 { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentAddress2 { get; set; }
        public string PermanentAddress3 { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegistrationDateText { get; set; }
        public string Remarks { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssueDateText { get; set; }
        public string AuthoritySignature { get; set; }
        public string AuthorityAddress { get; set; }
        public string GovtLogo { get; set; }
        public string BirthDeathLogo { get; set; }
        public string SealLogo { get; set; }
    }
}
