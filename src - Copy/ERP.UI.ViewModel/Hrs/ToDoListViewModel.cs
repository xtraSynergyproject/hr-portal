using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ToDoListViewModel
    {
        [Display(Name = "My Activity")]
        public string Activity { get; set; }
        public int Notes { get; set; }
        public int Tasks { get; set; }
        public int Incidents { get; set; }

    }
}


