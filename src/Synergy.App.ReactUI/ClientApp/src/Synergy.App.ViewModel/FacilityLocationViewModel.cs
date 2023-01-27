using System;

namespace Synergy.App.ViewModel
{
    public class FacilityLocationViewModel:ViewModelBase
    {        
        public string LocationName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string YatraLocationId { get; set; }
        public string YatraLocationName { get; set; }

    }
}
