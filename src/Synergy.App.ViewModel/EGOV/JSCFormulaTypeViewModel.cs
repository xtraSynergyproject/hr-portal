using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class JSCFormulaTypeViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string PropertyColumn { get; set; }
        public PropertyColumnTypeEnum PropertyColumnType { get; set; }
    }
}
