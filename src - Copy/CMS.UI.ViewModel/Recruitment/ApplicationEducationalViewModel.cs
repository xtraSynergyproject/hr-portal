using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ApplicationEducationalViewModel : ApplicationEducational
    {
        public string QualificationName { get; set; }
        public string SpecializationName { get; set; }
        public string EducationTypeName { get; set; }
        public string CountryName { get; set; }
        //public string AttachmentName { get; set; }

        //public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }

    }
}
