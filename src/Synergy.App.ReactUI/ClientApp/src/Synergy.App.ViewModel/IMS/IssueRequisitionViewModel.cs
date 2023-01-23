using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class IssueRequisitionViewModel:ServiceTemplateViewModel
    {
        public string IssueType { get; set; }
        public string Remarks { get; set; }
        public string IssueTo { get; set; }
        public string Department { get; set; }
        public string Employee { get; set; }
        public DateTime? IssuedOn { get; set; }      
        //public string RequisitionId { get; set; }
        public string Items { get; set; }
        public string ItemId { get; set; }
        public string WarehouseId { get; set; }
        public string RequisitionItemId { get; set; }
        public ImsIssueTypeEnum IssueReferenceType { get; set; }
        public string IssueReferenceId { get; set; }
        public string SerialNoIds { get; set; }
    }
}
