using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCFormulaFactorViewModel
    {        
        public string Code { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string FinancialYear { get; set; }
        public string FormulaType { get; set; }
        public double RangeFrom { get; set; }
        public double RangeTo { get; set; }
        public string PropertyColumn { get; set; }
        public PropertyColumnTypeEnum PropertyColumnType { get; set; }
    }
}
