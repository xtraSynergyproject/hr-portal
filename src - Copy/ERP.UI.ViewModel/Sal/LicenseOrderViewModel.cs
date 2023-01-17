using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class LicenseOrderViewModel : ViewModelBase
    {
        public string ServiceNo { get; set; }
        public string Customer { get; set; }       
        public string Product { get; set; }        
        public DateTime? ExpiryDate { get; set; }      
        public long? UserCount { get; set; }        
        public string MachineName { get; set; }
        public string DefaultView { get; set; }


    }
}

