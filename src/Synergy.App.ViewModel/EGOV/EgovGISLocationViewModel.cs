using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel.EGOV
{   public class EgovGISLocationViewModel : NoteViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }

}
