using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PlanViewModel :  ViewModelBase
    {
        public string Subject { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
    }
}


