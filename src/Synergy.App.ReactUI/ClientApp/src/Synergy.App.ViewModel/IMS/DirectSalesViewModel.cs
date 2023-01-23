using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class DirectSalesViewModel:ServiceTemplateViewModel
    {
        public string ProposalValue { get; set; }
        public string Customer { get; set; }
        public string CustomerName { get; set; }
        public string WorkflowStatus { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonName { get; set; }
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


        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTNo { get; set; }
        public string PINNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string BaseValue { get; set; }
        public string TAXValue { get; set; }
        public string OrderValue { get; set; }
        public string OrderDocumentId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Address { get; set; }

    }
}
