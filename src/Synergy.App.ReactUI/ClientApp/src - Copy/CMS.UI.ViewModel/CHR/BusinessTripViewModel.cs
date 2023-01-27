using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class BusinessTripViewModel
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string EmployeeName { get; set; }

        public string Purpose { get; set; }
        public DateTime BusinessTripStartDate { get; set; }
        public DateTime BusinessTripEndDate { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public Int64 VersionNo { get; set; }
        public Int64 SequenceOrder  { get; set; }

        public DateTime StartDate { get; set; }
        public string ServiceNo { get; set; }

        public string NtsNoteId { get; set; }
        public string ClaimServiceId { get; set; }
        public string ClaimServiceNo { get; set; }

    }
}
