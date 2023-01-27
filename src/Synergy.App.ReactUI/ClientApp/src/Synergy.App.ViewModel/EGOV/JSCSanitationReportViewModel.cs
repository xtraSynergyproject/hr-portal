using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCSanitationReportViewModel
    {
        public string AutoId { get; set; }
        public string AutoNumber { get; set; }
        public string WardNo { get; set; }
        public string WetWasteCapacityKgs { get; set; }
        public string DryWasteCapacityKgs { get; set; }
        public string TotalWasteCapacityKgs { get; set; }
        public string QuantityOfSolidWasteAsPerVehicleCapacity { get; set; }
        public string WorkStartTime { get; set; }
        public string WorkEndTime { get; set; }
        public string Date { get; set; }
        public string WetWaste { get; set; }
        public string DryWaste { get; set; }
        public string WetWaste1 { get; set; }
        public string DryWaste1 { get; set; }
        public string Remarks { get; set; }
        public string NoOfHousesFromWhere70OrAboveSegregatedWasteReceived { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureTime { get; set; }
        public long NoOfHouseHold { get; set; }
        public long NoOfCommercial { get; set; }
        public long NoOfHouseHoldCovered { get; set; }
        public long NoOfCommercialCovered { get; set; }
        public long NoOfTrips { get; set; }
        public string TransferStationId { get; set; }

        public string SlNo { get; set; }

        public string NameOfBulkWasteGenerator { get; set; }

        public string SignatureOfBWGRepresentative { get; set; }

        public string nameOfTheRouteCoordinator { get; set; }
    }
}
