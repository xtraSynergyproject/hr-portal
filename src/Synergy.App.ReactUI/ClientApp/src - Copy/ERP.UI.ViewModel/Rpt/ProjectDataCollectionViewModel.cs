using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ProjectDataCollectionViewModel 
    {
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string ServiceStatus { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public string ProjectLocation { get; set; }
        public string SizesRange { get; set; }
        //public string LocationOnGoogle { get; set; }
        public string PricesRange { get; set; }
        public string Website { get; set; }
        public string ProjectStatus { get; set; }
        public string Remarks { get; set; }
    }
}
