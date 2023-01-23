using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class CandidateExperienceBySectorViewModel : DataModelBase
    {      
        public string CandidateProfileId { get; set; }       
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Category { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
        public string SectorName { get; set; }
        public string IndustryName { get; set; }
        public string CategoryName { get; set; }
    }
}
