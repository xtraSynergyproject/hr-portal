using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCLandRateViewModel
    {        
        public string Code { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public double Value { get; set; }
        public string Error { get; set; }
        public string FinancialYear { get; set; }
        public string FormulaType { get; set; }
        public double RangeFrom { get; set; }
        public double RangeTo { get; set; }
        public string PropertyColumn { get; set; }
        public PropertyColumnTypeEnum PropertyColumnType { get; set; }
        public double ValueOfCommercial_InLacs { get; set; }
        public double ValueOfResidential_InLacs { get; set; }
        public double ResidentialRate { get; set; }
        public double CommercialRate { get; set; }
        public double RebateRate { get; set; }
        public double VacantArea { get; set; }
        public double VacantAmount { get; set; }
    }
}
