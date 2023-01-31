using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PositionChartExcelViewModel
    {
        public virtual int HierarchyNameId { get; set; }
        public virtual DateTime AsOnDate { get; set; }

    }
}
