using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class CandidateDrivingLicenseViewModel : DataModelBase
    {
        public string CandidateProfileId { get; set; }
        public string CountryId { get; set; }
        public string LicenseType { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ValidUpTo { get; set; }
        public bool IsLatest { get; set; }
        public string CountryName { get; set; }
        public string LicenseTypeName { get; set; }
    }
}
