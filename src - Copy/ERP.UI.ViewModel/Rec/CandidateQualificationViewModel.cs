using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class CandidateQualificationViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Qualification Name")]
        public string QualificationName { get; set; }
        [Display(Name = "Set this as My Highest Qualification")]
        public bool? IsHighestQualification { get; set; }
        [Required]
        [Display(Name = "Graduation Year")]
        public int? GraduationYear { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        
        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
       
        [Display(Name = "University")]
        public string University { get; set; }
       
        [Display(Name = "Location")]
        public string Location { get; set; }

        public long? CandidateId { get; set; }
        public long? CountryId { get; set; }
        [Display(Name = "Qualification Id")]
        public long? QualificationId { get; set; }
        [Display(Name = "Snapshot Date")]
        public DateTime? SnapshotDate { get; set; }
        public long? ApplicationId { get; set; }
        public long? UserId { get; set; }

    }

}


