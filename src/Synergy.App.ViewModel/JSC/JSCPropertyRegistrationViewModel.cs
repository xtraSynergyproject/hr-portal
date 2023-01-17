using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class JSCPropertyRegistrationViewModel : JSCComplaintViewModel
    {
        public int CustomerId { get; set; }
        public string PropertyName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }



    }
}
