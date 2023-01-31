using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class MapMarkerViewModel : NoteViewModel
    {
       public string Id { get; set; }
       public string Name { get; set; }
       public string Latitude { get; set; }
       public string Longitude { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Count { get; set; }
        
        public string AreaName { get; set; }
        public string CircleAndMarkCoo { get; set; }
        public string PolyCoord { get; set; }
        public string MarkType { get; set; }
        public string CircleRadius { get; set; }
        public string MarkTypeName { get; set; }
    }
}
