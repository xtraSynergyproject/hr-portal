using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PersonTrainingDocumentViewModel : PersonDocumentViewModel
    {
        [Display(Name = "Training Name")]
        public string TrainingSubject { get; set; }
        [Display(Name = "Institute / University Name")]
        public string InstituteUniversityName { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Training Completion Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Location (City, Country)")]
        public string Location { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string Attachment { get; set; }


    }
}
