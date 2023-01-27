using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ApplicationExperienceBySectorViewModel : DataModelBase
    {
        public string SectorName { get; set; }
        public string IndustryName { get; set; }
        public string CategoryName { get; set; }
        public string ApplicationId { get; set; }              
        public string Sector { get; set; }       
        public string Industry { get; set; }        
        public string Category { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
