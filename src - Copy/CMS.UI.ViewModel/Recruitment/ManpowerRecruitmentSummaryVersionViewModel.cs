using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ManpowerRecruitmentSummaryVersionViewModel : DataModelBase
    {
        public string ManpowerRecruitmentSummaryId { get; set; }        
        [Display(Name = "Job Title")]
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        [Display(Name = "Organization Unit")]
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public long? Requirement { get; set; }
        public long? Seperation { get; set; }
        public long? Available { get; set; }
        [Display(Name = "Sub Contract")]
        public long? Planning { get; set; }
        public long? Transfer { get; set; }
        public long? Balance { get; set; }
        //{
        //    get
        //    {
        //        return Requirement-Available;
        //    }
        //}
        public string CreatedByName { get; set; }

    }
}
