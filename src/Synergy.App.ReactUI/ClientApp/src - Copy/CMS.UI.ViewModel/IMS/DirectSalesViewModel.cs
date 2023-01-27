using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class DirectSalesViewModel:ServiceTemplateViewModel
    {
        public string ProposalValue { get; set; }
        public string Customer { get; set; }
        public string CustomerName { get; set; }
        public string WorkflowStatus { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string CustomerGSTNo { get; set; }
        public string CustomerRefNo { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ShippingAddress { get; set; }
        public string ProposalType { get; set; }
        public string ProposalSource { get; set; }
        public string NextFollowUpDate { get; set; }
        public string CompetitionWith { get; set; }
        public string Summary { get; set; }
        public string ProposalDate { get; set; }
    }
}
