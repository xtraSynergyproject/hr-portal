using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PerformanceSummaryViewModel
    {
        public string DisplayName { get; set; }
        public int? EmployeeId { get; set; }
        public int? PositionId { get; set; }
        public int? HierarchyNameId { get; set; }
        public DateTime AsOnDate { get; set; }

    }
}
