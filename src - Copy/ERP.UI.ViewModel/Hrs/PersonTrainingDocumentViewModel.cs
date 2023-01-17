
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonTrainingDocumentViewModel : PersonDocumentViewModel
    {
        [Display(Name = "Training Name")]
        public string TrainingSubject { get; set; }
        [Display(Name = "Institute / University Name")]
        public string InstituteUniversityName { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Training Completion Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Location (City, Country)")]
        public string Location { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string Attachment { get; set; }

    }
}
