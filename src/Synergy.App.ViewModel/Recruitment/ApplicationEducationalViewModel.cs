using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
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
