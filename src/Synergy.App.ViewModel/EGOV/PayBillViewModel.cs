using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class PayBillViewModel
    {
        public string ConsumerNo { get; set; }
        public string ConsumerName { get; set; }
        public string AccountId { get; set; }
        public string BillMonth { get; set; }
        public DateTime BillIssueDate { get; set; }
        public long BillAmount { get; set; }
        public long MinimumPayableAmount { get; set; }
        public long TotalAmount { get; set; }
        public DateTime PayByDate { get; set; }
    }
}
