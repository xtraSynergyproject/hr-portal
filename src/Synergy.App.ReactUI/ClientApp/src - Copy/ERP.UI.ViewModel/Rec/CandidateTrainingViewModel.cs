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
    public class CandidateTrainingViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public long? CandidateId { get; set; }
        public long? CountryId { get; set; }
        [Display(Name = "Training Id")]
        public long? TrainingId { get; set; }
        [Display(Name = "Snapshot Date")]
        public DateTime? SnapshotDate { get; set; }

        public long? ApplicationId { get; set; }
        public long? UserId { get; set; }
    }

}


