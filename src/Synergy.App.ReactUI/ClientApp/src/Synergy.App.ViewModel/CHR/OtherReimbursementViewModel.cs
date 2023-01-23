using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class OtherReimbursementViewModel
    {
        public DateTime EventDate { get; set; }
        public string ReimbursementDetails { get; set; }
        public string ReimbursementAmount { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string ServiceNo { get; set; }
        public string NtsNoteId { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }

    }
}
