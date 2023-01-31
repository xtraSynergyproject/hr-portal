using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class JobDescriptionViewModel : JobDescription
    {
        public string TaskStatus { get; set; }
        public string JobCriteria { get; set; }
        public string Skills { get; set; }
        public string OtherInformation { get; set; }

    }
}
