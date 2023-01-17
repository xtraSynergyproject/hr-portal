using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TicketViewModel : ViewModelBase
    {
       
        public string Contact { get; set; }
        public string RelatedTo { get; set; }
        public string Summary { get; set; }
        public string Tkt_Description { get; set; }
        public string AssingnTo { get; set; }
        public string CcUsers { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DueTime { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Branches { get; set; }
        public string Attach { get; set; }
       
    }
}

