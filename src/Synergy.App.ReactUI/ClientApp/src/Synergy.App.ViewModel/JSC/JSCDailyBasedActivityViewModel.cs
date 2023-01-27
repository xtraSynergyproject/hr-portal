using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCDailyBasedActivityViewModel:ViewModelBase
    {        
        public string PointName { get; set; }
        public string InitialPicture { get; set; }
        public string AfterCleaningPicture { get; set; }
        public string VehicleId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string VehicleNumber { get; set; }
        public string CollectionStartDateTime { get; set; }
        public string CollectionEndDateTime { get; set; }
    }
}
