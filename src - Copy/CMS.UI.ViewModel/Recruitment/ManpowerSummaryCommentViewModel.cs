using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ManpowerSummaryCommentViewModel : DataModelBase
    {
        public string Comment { get; set; }
        public string ManpowerRecruitmentSummaryId { get; set; }
        public string UserRoleId { get; set; }
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public long? Requirement { get; set; }
        public long? Available { get; set; }
        [Display(Name = "Sub Contract")]
        public long? Planning { get; set; }
        public long? Transfer { get; set; }
        public long? Balance
        {
            get
            {
                return Requirement - Available;
            }
        }
        public string CreatedByName { get; set; }
        public string UserRoleCode { get; set; }
    }
}
