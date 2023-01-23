using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ApplicationExperienceByJobViewModel : DataModelBase
    {
        public string JobName { get; set; }
        public string ApplicationId { get; set; }              
        public string JobId { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
