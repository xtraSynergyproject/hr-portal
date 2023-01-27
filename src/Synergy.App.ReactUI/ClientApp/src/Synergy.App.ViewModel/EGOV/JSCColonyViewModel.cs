using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCColonyViewModel
    {
        public string ParentId { get; set; }
        public string type { get; set; }
        public dynamic geometry { get; set; }
        public string col_id { get; set; }
        public string col_name { get; set; }
        public string col_area { get; set; }
    }
    
}
