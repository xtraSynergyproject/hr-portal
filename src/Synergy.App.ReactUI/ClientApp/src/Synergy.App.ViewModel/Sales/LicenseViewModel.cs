using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class LicenseViewModel : NoteTemplateViewModel
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
        public DateTime? ExpiryDate { get; set; }
        public string MachineName { get; set; }
        public string ApplicationUrl { get; set; }
        public string SalesOrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string PrivateKey { get; set; }


    }
}


