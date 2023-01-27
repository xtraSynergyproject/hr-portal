using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class UserPromotionViewModel : ViewModelBase
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Promotion2017 { get; set; }
        public string Promotion2018 { get; set; }
        public string Promotion2019 { get; set; }

    }
}


