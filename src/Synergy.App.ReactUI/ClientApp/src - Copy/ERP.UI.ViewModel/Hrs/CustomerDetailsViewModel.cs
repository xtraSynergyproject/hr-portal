using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class CustomerDetailsViewModel : ViewModelBase
    {
        public string CustomerName { get; set; }
        [Display(Name = "Department Name")]
        public long OrganizationId { get; set; }
        public string MobileNo { get; set; }
        public string UserName { get; set; }

    }
}
