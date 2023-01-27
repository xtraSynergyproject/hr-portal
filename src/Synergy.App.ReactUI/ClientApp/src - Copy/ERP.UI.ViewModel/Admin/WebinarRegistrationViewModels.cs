using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class WebinarRegistrationViewModel: ViewModelBase
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        [Display(Name = "Will you attend?")]
        public bool? CanAttend { get; set; }
        public string SuccessMessage { get; set; }

        [Display(Name = "Country of Work")]
        public string CountryOfWork { get; set; }
        public string[] Countries { get; set; }
        public string Country
        {
            get; set;
        }

        public DateTime? CreatedOnDate
        {
            get
            {
                return CreatedDate.ServerToLocalTime("CAYAN_UAE");
            }
        }

        [Display(Name = "Email Count")]
        public int? EmailCount { get; set; }

        public long? CountryId { get; set; }

        public long WebinarId { get; set; }
        public string WebinarName { get; set; }

        public bool? BulkUpload { get; set; }

        public long? TopBannerId { get; set; }
        public long? LeftBannerId { get; set; }
        public long? BottomBannerId { get; set; }
        public long? BottomRightBannerId { get; set; }
        public string RegisteredSuccessMessage { get; set; }
        public long? NotificationTemplateId { get; set; }

        public long? RemoveWebinarId { get; set; }
    }

}
