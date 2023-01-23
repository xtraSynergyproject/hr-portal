using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class OrderSummaryViewModel
    {
        public string MonthYear { get; set; }
        public decimal PreviousYearBalance { get; set; }
        public decimal NewOrdersValue { get; set; }
        public decimal BilledOrdersValue { get; set; }
        public decimal ClosingBalance { get; set; }
        public long SequenceOrder { get; set; }
        public string Quarter { get; set; }
    }
}
