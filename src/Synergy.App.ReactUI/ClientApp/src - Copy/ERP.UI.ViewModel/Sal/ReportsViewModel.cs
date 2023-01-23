using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ReportsViewModel : ViewModelBase
    {
        public ActivityReportsViewModel ActRpt { get; set; }
        public SalesReportsViewModel SalRpt { get; set; }
    }
}

