using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class EGovDashboardViewModel
    {
        public IList<EgovDashboardViewModel> PaymentDueList { get; set; }
        public IList<EgovDashboardViewModel> ServiceList { get; set; }
        public string TemplateCodes { get; set; }
    }

    public class EgovDashboardViewModel
    {
        public string ServiceName { get; set; }
        public long DueAmount { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string BillDate { get; set; }
        public int? Count { get; set; }
        public int? InProgressCount { get; set; }
        public int? CompletedCount { get; set; }
        public int? RejectedCount { get; set; }
        public string TemplateCode { get; set; }
    }
}
