using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCGarbageCollectionViewModel
    {
        public string ParcelId { get; set; }
        public string CollectedByUserId { get; set; }
        public string CollectedByUserName { get; set; }
        public DateTime? CollectionDateTime { get; set; }
        public string PropertyName { get; set; }
        public bool IsGarbageCollected { get; set; }
        public string WardNo { get; set; }
        public string WardName { get; set; }
        public long TotalProperty { get; set; }
        public long TotalCollectedProperty { get; set; }
        public string AutoNumber { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ComplaintNo { get; set; }
        public string ComplaintId { get; set; }
        public string DDN { get; set; }
        public string GarbageTypeName { get; set; }
        public long DryWasteInKgs { get; set; }
        public long WetWasteInKgs { get; set; }
        public long WasteResidential { get; set; }
        public long WasteCommercial { get; set; }
        public long WetWasteResidentialInKgs { get; set; }
        public long WetWasteCommercialInKgs { get; set; }
        public long DryWasteResidentialInKgs { get; set; }
        public long DryWasteCommercialInKgs { get; set; }
        public int ComplaintCount { get; set; }
        public string CollectionId { get; set; }
        public string VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleTypeId { get; set; }
        public string VehicleCapacity { get; set; }
        public string VehicleReachingTime { get; set; }
        public string VehicleDepartureTime { get; set; }
        public string WetWaste { get; set; }
        public string DryWaste { get; set; }
        public string Remarks { get; set; }

        public string SlNo { get; set; }
        public string Name { get; set; }
        public string Count { get; set; }
    }
}
