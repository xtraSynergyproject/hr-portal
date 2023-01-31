using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PersonEducationDocumentViewModel : PersonDocumentViewModel
    {
        [Display(Name = "Qualification Name")]
        public string QualificationName { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Completion Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
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
