using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
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
