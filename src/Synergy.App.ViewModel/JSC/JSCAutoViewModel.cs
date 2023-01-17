using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCAutoViewModel
    {
        public string AutoId { get; set; }
        public string AutoNumber { get; set; }
        public string JMCNumber { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string Mobile { get; set; }
        public string TransferStation { get; set; }
        public string TransferStationName { get; set; }
        public string AutoTypeId { get; set; }
        public string AutoTypeName { get; set; }
        public double WetWasteCapacityKgs { get; set; }        
        public string DryWasteCapacityKgs { get; set; }
        public string CollectorId { get; set; }   
        public string CollectorName { get; set; }   
        public string UserId { get; set; }
        public string WardNo { get; set; }
        public string RouteCoordinatorName { get; set; }
    }
}
