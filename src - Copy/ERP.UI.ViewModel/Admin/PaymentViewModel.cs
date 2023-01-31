using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PaymentViewModel :  ViewModelBase
    {
        public string Plan { get; set; }
        public string NoOfUser { get; set; }
        public double TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BillingEmail { get; set; }
    }
}


