using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCSearchComplaintViewModel
    {
        public string WardId { get; set; }    
        public string DepartmentId { get; set; }    
        public string ComplaintTypeId { get; set; }    
        public string ComplaintNo { get; set; }    
        public string ComplaintName { get; set; }    
        public string StatusCode { get; set; }    
        public DateTime? ComplaintDate { get; set; }
        public long NoOfDaysPending { get; set; }

    }
}
