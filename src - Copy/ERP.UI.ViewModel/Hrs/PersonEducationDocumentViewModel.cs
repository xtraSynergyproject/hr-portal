
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonEducationDocumentViewModel :PersonDocumentViewModel
    {
        [Display(Name = "Qualification Name")]
        public string QualificationName { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Completion Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        [Display(Name = "Institute / University Name")]
        public string University { get; set; }
        [Display(Name = "Location (City, Country)")]
        public string Location { get; set; }
        [Display(Name = "Attested")]
        public string IsMOFA { get; set; }
        public string Attachment { get; set; }

    }
}
