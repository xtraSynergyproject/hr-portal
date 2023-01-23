using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ProjectViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Project Name")]
        public string Name { get; set; }
         //[Required]
        [Display(Name = "Project Code")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Project Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Required]
        [Display(Name = "Measurement Unit")]
        public SalMeasurementUnitEnum MeasurementUnit { get; set; }
        [Display(Name = "Floor Plan")]
        public string Attachments { get; set; }
        public long? AttachmentId { get; set; }

        [Display(Name = "Proposal Plan")]
        public string Proposal { get; set; }
        public long? ProposalId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        [Required]
        [Display(Name = "Project Type")]
        public SalProjectTypeEnum? ProjectType { get; set; }
    }
}

