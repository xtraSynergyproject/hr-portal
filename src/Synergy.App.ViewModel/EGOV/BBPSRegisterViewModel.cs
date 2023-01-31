using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class BBPSRegisterViewModel
    {
        public string ConsumerNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhotoId { get; set; }
        public string ServiceType { get; set; }
    }
    
}
