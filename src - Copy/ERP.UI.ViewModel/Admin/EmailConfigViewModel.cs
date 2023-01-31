using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmailConfigViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Smtp Sender Name")]
        public string SmtpSenderName { get; set; }
        [Required]
        [Display(Name = "Smtp Host")]
        public string SmtpHost { get; set; }
        [Required]
        [Display(Name = "Smtp Port")]
        public int SmtpPort { get; set; }
        [Required]
        [Display(Name = "Smtp From Id")]
        public string SmtpFromId { get; set; }
        [Required]
        [Display(Name = "Smtp User Id")]
        public string SmtpUserId { get; set; }
        [Required]
        [Display(Name = "Smtp Password")]
        public string SmtpPassword { get; set; }
    }
}


