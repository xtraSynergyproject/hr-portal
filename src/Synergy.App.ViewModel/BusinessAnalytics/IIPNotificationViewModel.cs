using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class IIPNotificationViewModel
    {
        public string Incident { get; set; }
        public string IncidentSubType { get; set; }
        public string IncidentNumber { get; set; }
        public string IncidentTime { get; set; }
        public string Remark { get; set; }       
        public double Latitude { get; set; }
        public double Longitude { get; set; }       
        public string PoliceStation { get; set; }
        public string DistrictCode { get; set; }
       
    }
}
