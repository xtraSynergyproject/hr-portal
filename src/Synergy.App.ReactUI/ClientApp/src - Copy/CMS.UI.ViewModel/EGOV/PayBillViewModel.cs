using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
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
