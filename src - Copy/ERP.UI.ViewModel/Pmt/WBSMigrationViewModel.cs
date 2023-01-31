using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class WBSMigrationViewModel : ViewModelBase
    {
        public string WbsItemName { get; set; }
        public long WbsItemId { get; set; }
        public long DocumentId { get; set; }
        public long? ServiceId { get; set; }
        public string ServiceStartDate { get; set; }
        public string IncomingIssueCode { get; set; }
        public string OutgoingIssueCode { get; set; }
        public string IncomingIssueCodeValue { get; set; }
        public string OutgoingIssueCodeValue { get; set; }
        public string SubmittedDate { get; set; }        
        public string ReceivedDate { get; set; }
       
    }
}
