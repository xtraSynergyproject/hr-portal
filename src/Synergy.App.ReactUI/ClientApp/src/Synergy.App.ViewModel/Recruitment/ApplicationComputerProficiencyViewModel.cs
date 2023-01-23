using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ApplicationComputerProficiencyViewModel : DataModelBase
    {
        public string ProficiencyLevelName { get; set; }
        public string ApplicationId { get; set; }       
        public string Program { get; set; }        
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}
