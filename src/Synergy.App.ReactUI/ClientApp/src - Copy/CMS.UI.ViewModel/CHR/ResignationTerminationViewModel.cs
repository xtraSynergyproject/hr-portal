using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{ 
  public  class ResignationTerminationViewModel
    {


        public string Id { get; set; }
        public string ServiceNo { get; set; }
        public string Subject { get; set; }
        public DateTime  ResignationTerminationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ServiceStatus  { get; set; }
        public string ClearanceForm { get; set; }
        public DateTime EndOfService { get; set; }

        public string DisplayName { get; set; }

        public string Reason { get; set; }
        public string Comment { get; set; }

        public string UserId { get; set; }
    }
}
