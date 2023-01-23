using System;

namespace Synergy.App.ViewModel
{
    public class FacilityViewModel : ViewModelBase
    {
        public string FacilityTypeId { get; set; }
        public string FacilityTypeName { get; set; }
        public string FacilityCode { get; set; }
        public string Address { get; set; }
        public string FacilityLocationId { get; set; }
        public string FacilityLocationName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string YatraLocationId { get; set; }
        public string YatraLocationName { get; set; }
        public string HealthStatusId { get; set; }
        public string HealthStatusName { get; set; }

    }
}
