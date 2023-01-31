using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ComparisonViewModel : ViewModelBase
    {
        public long? Target { get; set; }
        public long? Actual { get; set; }
        public string UserName { get; set; }
    }
}

