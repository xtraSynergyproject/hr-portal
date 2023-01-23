using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCInwardOutwardReportViewModel : ViewModelBase
    {
        public string SlNo { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string VehicleCapacity { get; set; }
        public string VehicleReachingTime { get; set; }
        public string VehicleDepartureTime { get; set; }
        public string WetWaste { get; set; }
        public string DryWaste { get; set; }
        public string Remarks { get; set; }
    }
}
