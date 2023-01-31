using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCRefuseCompactorViewModel:ViewModelBase
    {        
        public string DustbinId { get; set; }
        public string DustbinName { get; set; }
        public DateTime? CollectionDateTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
