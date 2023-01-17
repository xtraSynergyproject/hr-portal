using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class LicenseViewModel : ViewModelBase
    {
        [Required]
        public string LicenseKey { get; set; }
        public LicenseTypeEnum? LicenseType { get; set; }
        public long? UserCount { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public long? RemainingSeats { get; set; }
        public bool CanAddUser { get; set; }
        public bool IsLicenseValid { get; set; }
        public bool IsExpired { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ExpiryDate { get; set; }
        public string MachineName { get; set; }
        public string ApplicationUrl { get; set; }
        public long? SalesOrderId { get; set; }
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }


    }
}


