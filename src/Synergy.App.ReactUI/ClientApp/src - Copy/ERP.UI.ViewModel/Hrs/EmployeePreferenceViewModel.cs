using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmployeePreferenceViewModel : BaseViewModel
    {


        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public bool? ShowPhoto { get; set; }

        public bool IsEmployeeMale { get; set; }

        [Display(Name = "Show My Photo. (If you select this option, your photo will be displayed in “Enterprise Organization Chart” and other employee related pages.Everybody in the company will be able to see your photo.)")]
        public string ShowMyPhoto { get; set; }

    }
}
